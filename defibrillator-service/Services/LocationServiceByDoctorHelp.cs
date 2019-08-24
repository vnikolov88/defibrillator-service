using Flurl.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DefibrillatorService.Services
{
    public class LocationServiceByDoctorHelp : ILocationService
    {
        private readonly Random _random = new Random();
        private readonly TimeSpan MaxTTL = TimeSpan.FromDays(30);
        private TimeSpan RetryTTL => TimeSpan.FromHours(_random.Next(1, 24));
        private readonly IMemoryCache _cache;
        private readonly StartupOptions _options;

        public LocationServiceByDoctorHelp(
            IMemoryCache cache,
            IOptions<StartupOptions> options)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<(double Latitude, double Longitude)> GetLocationAsync(string address, CancellationToken cancellationToken)
        {
            var gpsMatch = Regex.Match(address, "^(?'lat'[-+]?[\\d]{1,2}\\.\\d+),\\s*(?'lon'[-+]?[\\d]{1,3}\\.\\d+?)$");
            if (gpsMatch.Success)
                return (double.Parse(gpsMatch.Groups["lat"].Value), double.Parse(gpsMatch.Groups["lon"].Value));
            else if (_cache.TryGetValue(address, out (double Latitude, double Longitude) result))
                return result;
            else
            {
                var encodedAddress = Convert.ToBase64String(Encoding.UTF8.GetBytes(address));
                var location = await $"{_options.DoctorHelpRestUrl}/api/v1/geo/locationgeocoordinates?address={encodedAddress}"
                    .GetJsonAsync(cancellationToken);

                var ttl = MaxTTL;
                if (location == null)
                {
                    result = (0, 0);
                    ttl = RetryTTL;
                }
                else
                {
                    result = ((double)location.Latitude, (double)location.Longitude);
                }

                _cache.Set(address, result, ttl);

                return result;
            }
        }
    }

}
