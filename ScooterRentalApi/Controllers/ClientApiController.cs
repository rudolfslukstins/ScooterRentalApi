using Microsoft.AspNetCore.Mvc;
using ScooterRental;
using ScooterRental.Core.Services;
using ScooterRental.Interfaces;

namespace ScooterRentalApi.Controllers
{
    [Route("client")]
    [ApiController]
    public class ClientApiController : Controller
    {
        private readonly IScooterService _scooterService;
        private readonly IRentalCompany _rentalCompany;

        public ClientApiController(IScooterService scooterService,
            IRentalCompany rentalCompany)
        {
            _scooterService = scooterService;
            _rentalCompany = rentalCompany;
        }

        [Route("Get-scooter")]
        [HttpGet]
        public IActionResult GetScooter(int id)
        {
            var scooter = _scooterService.GetScooterById(id);

            return Ok(scooter);
        }

        [Route("Get-available-scooters")]
        [HttpGet]
        public IActionResult GetAvailableScooters()
        {
            var availableScooters = _scooterService.GetScooters();

            return Ok(availableScooters);
        }

        [Route("Rent-scooter")]
        [HttpPost]
        public IActionResult RentScooter(int id)
        {
            var rentScooter = _rentalCompany.StartRent(id);
            return Ok(rentScooter);

        }

        [Route("Return-Scooter")]
        [HttpPost]
        public IActionResult ReturnScooter(int id)
        {
            var returnScooter = _rentalCompany.EndRent(id); 
            return Ok(returnScooter);
        }
    }
}
