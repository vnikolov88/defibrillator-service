using DefibrillatorService.Extensions;
using DefibrillatorService.Models;
using DefibrillatorService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DefibrillatorService.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class DefibrillatorController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly IDefibrillatorService _defibrillatorService;

        public DefibrillatorController(
            ILocationService locationService,
            IDefibrillatorService defibrillatorService)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _defibrillatorService = defibrillatorService ?? throw new ArgumentNullException(nameof(defibrillatorService));
        }

        [HttpGet("cannary")]
        public IActionResult Cannary() => Ok();

        [HttpGet("list")]
        public async Task<ActionResult<PagedSearch<DefibrillatorLocation>>> Get(
            CancellationToken cancellationToken,
            int distance,
            string address,
            int page = 1,
            int pageSize = 20)
        {
            // Get location
            var decodedAddress = Encoding.UTF8.GetString(Convert.FromBase64String(address));
            (double Latitude, double Longitude) = await _locationService.GetLocationAsync(decodedAddress, cancellationToken);
            
            var defibrillators = await _defibrillatorService.GetDefibrillatorLocationsAsync(Latitude, Longitude, distance, cancellationToken);
            return defibrillators?.Page(page, pageSize);
        }
    }
}
