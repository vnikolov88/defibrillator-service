using DefibrillatorService.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DefibrillatorService.Services
{
    public interface IDefibrillatorService
    {
        Task<IEnumerable<DefibrillatorLocation>> GetDefibrillatorLocationsAsync(double latitude, double longitude, double distanceKm, CancellationToken cancellationToken);
    }
}
