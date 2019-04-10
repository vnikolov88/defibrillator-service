using DefibrillatorService.Extensions;
using DefibrillatorService.Models;
using Flurl.Http;
using GeoCoordinatePortable;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DefibrillatorService.Services
{
    public class DefibrillatorServiceByDefiWeb : IDefibrillatorService
    {
        private readonly StartupOptions _options;
        public DefibrillatorServiceByDefiWeb(
            IOptions<StartupOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<IEnumerable<DefibrillatorLocation>> GetDefibrillatorLocationsAsync(double latitude, double longitude, double distanceKm, CancellationToken cancellationToken)
        {
            var center = new GeoCoordinate(latitude, longitude);

            var xml = CreateSearchCriteria(center, distanceKm);

            var url = $"{_options.DefiWebRestUrl}/api/rest/v3/locations/";

            var authHeaders = GetAuthHeaders(url);

            var xmlContent = xml.SerializeObjectToXml();

            var returned = await url
                .WithHeader("X-DW-Authenticate", authHeaders.Authenticate)
                .WithHeader("X-DW-AuthTimestamp", authHeaders.AuthTimestamp)
                .WithHeader("Accept", "application/json")
                .PostAsync(new StringContent(xmlContent, Encoding.UTF8, "application/xml"))
                .ReceiveJson<DefiWeb[]>();
            
            return returned.SelectMany(x => x.Locations).Select(x => 
                new DefibrillatorLocation {
                    Latitude = x.Detail.Position.Latitude,
                    Longitude = x.Detail.Position.Longitude,
                    Name = x.Address?.Name,
                    Street = x.Address?.Street,
                    City = x.Address?.City,
                    Zip = x.Address?.Zip,
                    Description = x.Detail?.Description?.Text,
                    DistanceKm = new GeoCoordinate(x.Detail.Position.Latitude, x.Detail.Position.Longitude).GetDistanceTo(center) / 1000
                }).OrderBy(x => x.DistanceKm);
        }

        private MapSearchCriteria CreateSearchCriteria(GeoCoordinate center, double distanceKm)
        {
            var distanceMeters = distanceKm * 1000;
            return new MapSearchCriteria
            {
                Polygons = new Polygon[] {
                        new Polygon {
                            Name = "0",
                            SRID = 4326,
                            ClusterDistance = 10,
                            Points = new Point[]
                            {
                                PointFromGeoCoordinate(center.OffsetBy(-distanceMeters, distanceMeters)),
                                PointFromGeoCoordinate(center.OffsetBy(-distanceMeters, -distanceMeters)),
                                PointFromGeoCoordinate(center.OffsetBy(distanceMeters, -distanceMeters)),
                                PointFromGeoCoordinate(center.OffsetBy(distanceMeters, distanceMeters)),
                                PointFromGeoCoordinate(center.OffsetBy(-distanceMeters, distanceMeters))
                            },
                            ItemTypes = new ItemType[]
                            {
                                ItemType.EAD
                            }
                        }
                    },
                LimitingCriteria = new LimitingCriteria
                {
                    IncludeItemTypes = new string[0],
                    LocationTypes = new LocationTypes
                    {
                        Mode = LocationTypesMode.EXCLUDE,
                        IsMobile = false,
                        LocationType = new LocationType { Item = string.Empty }
                    }
                }
            };
        }

        private (string Authenticate, string AuthTimestamp) GetAuthHeaders(string url)
        {
            var now = DateTime.UtcNow;
            var dateStr = now.ToString("yyyy-MM-dd'T'HH:mm:ss");

            String stringToEncode = $"{_options.DefiWebAppId}:{dateStr}:POST:{url}";
            byte[] encoded = stringToEncode.EncodeHMACSHA256(_options.DefiWebPassword);
            String b64Enc = Convert.ToBase64String(encoded);
            var authHeader = _options.DefiWebAppId + ":" + b64Enc;

            var timestamp = now.ToString("dd.MM.yyyy HH:mm:ss");
            return (authHeader, timestamp);
        }

        private Point PointFromGeoCoordinate(GeoCoordinate geoCoordinate) => new Point { Latitude = geoCoordinate.Latitude, Longitude = geoCoordinate.Longitude };
    }
}
