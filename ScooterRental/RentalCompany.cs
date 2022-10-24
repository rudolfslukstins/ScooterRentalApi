using System;
using System.Collections.Generic;
using System.Linq;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using static ScooterRental.RentalDetails;

namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        private ScooterService _service;
        private List<Rent> _rentedScooters;
        
        public string Name { get; }

        public RentalCompany(string name,ScooterService service , List<Rent> rentedScooters)
        {
            Name = name;
            _service = service;
            _rentedScooters = rentedScooters;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            decimal income = 0;
            var yearRelevantScooter = new List<Rent>();

            yearRelevantScooter = RentalHistory(_rentedScooters, year,includeNotCompletedRentals);

            foreach (var scooter in yearRelevantScooter)
            {
                income += RentalFee(scooter);
            }

            return income;
        }

        public decimal EndRent(string id)
        {
            var scooter = _service.GetScooterById(id);
            var rentedScooter = _rentedScooters.FirstOrDefault(s => s.Id == id && !s.EndTime.HasValue);


            rentedScooter.EndTime = DateTime.Now;
            scooter.IsRented = false;

            return RentalFee(rentedScooter);
        }

        public void StartRent(string id)
        {
            var scooter = _service.GetScooterById(id);
            _rentedScooters.Add(new Rent(scooter.Id, scooter.PricePerMinute, DateTime.Now));

            if (_service.GetScooterById(id).IsRented)
            {
                throw new ScooterRentedException();
            }

            scooter.IsRented = true;
        }
    }
}