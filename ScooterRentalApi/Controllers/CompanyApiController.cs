using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ScooterRental.Core.Services;
using ScooterRental.Interfaces;

namespace ScooterRentalApi.Controllers
{
    [Route("company")]
    [ApiController]
    public class CompanyApiController : Controller
    {
        private readonly IScooterService _scooterService;
        private readonly IRentalCompany _rentalCompany;
       

        public CompanyApiController(IScooterService scooterService,
            IRentalCompany rentalCompany)
        {
            _scooterService = scooterService;
            _rentalCompany = rentalCompany;

        }
        [Route("Add-Scooter")]
        [HttpPut]
        public IActionResult AddScooter(int id, decimal pricePerMinute)
        {
           var scooter = _scooterService.AddScooter(id, pricePerMinute);
           return Created("", scooter);
        }

        [Route("Get-income")]
        [HttpGet]
        public IActionResult GetIncome(int year, bool includeNotCompletedRentals)
        {
            var income = _rentalCompany.CalculateIncome(year, includeNotCompletedRentals);
            return Ok(income);
        }

        [Route("Delete-scooter")]
        [HttpDelete]
        public IActionResult DeleteScooter(int id)
        {
          _scooterService.RemoveScooter(id);

          return Ok(id);
        }
    }
}
