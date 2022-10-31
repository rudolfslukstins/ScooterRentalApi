using ScooterRental.Core.Models;
using ScooterRental.Core.Services;
using ScooterRental.Db;
using ScooterRental.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using static ScooterRental.Services.RentalService;

namespace ScooterRental.Services
{
    public class RentalCompany : EntityService<Company>, IRentalCompany
    {
        public RentalCompany(ScooterRentalDbContext context) : base(context)
        {

        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            decimal income = 0;
            var yearRelevantScooter = new List<Rent>();

            yearRelevantScooter = RentalHistory(year, includeNotCompletedRentals);

            foreach (var scooter in yearRelevantScooter)
            {
                income += RentalFee(scooter);
            }

            return income;
        }

        public string EndRent(int id)
        {
            var scooter = _context.Scooters.FirstOrDefault(sco => sco.Id == id);
            scooter.IsRented = false;
            _context.Scooters.Update(scooter);

            var rentedScooter = _context.Rent.FirstOrDefault(s => s.ScooterId == scooter.Id && !s.EndTime.HasValue);
            rentedScooter.EndTime = DateTime.Now;
            var priceToPay = $"{RentalFee(rentedScooter)} $";
            _context.SaveChanges();

            return priceToPay;
        }


        public Scooter StartRent(int id)
        {
            var scooter = _context.Scooters.FirstOrDefault(sco => sco.Id == id);

            if (scooter != null)
            {
                if (scooter.IsRented)
                {
                    throw new ScooterRentedException();
                }
                
                scooter.IsRented = true;
                _context.Scooters.Update(scooter);

                var newRent = new Rent
                {
                    ScooterId = id,
                    StartTime = DateTime.Now,
                    PricePerMinute = scooter.PricePerMinute,
                };
                _context.Rent.Add(newRent);
                _context.SaveChanges();
                return scooter;
            }

            return null;
        }
    }
}