using System.Threading;
using System.Threading.Tasks;

namespace DefibrillatorService.Services
{
    public interface ILocationService
    {
        Task<(double Latitude, double Longitude)> GetLocationAsync(string address, CancellationToken cancellationToken);
    }
}
