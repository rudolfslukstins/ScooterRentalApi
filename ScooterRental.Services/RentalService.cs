using ScooterRental.Core.Models;
using ScooterRental.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using ScooterRental.Core.Services;
using SQLitePCL;

namespace ScooterRental.Services
{
    public class RentalService : EntityService<Rent>
    {
        public RentalService(ScooterRentalDbContext context) : base(context)
        {
        }
        public static decimal RentalFee(Rent rentedScooter)
        {
            var realEndTime = rentedScooter.EndTime ?? DateTime.UtcNow;
            var rentDay = (realEndTime.Date - rentedScooter.StartTime.Date).TotalDays;

            if (rentDay == 0)
            {
                var dayPrice = Math.Round(((decimal)(realEndTime - rentedScooter.StartTime)
                    .TotalMinutes * rentedScooter.PricePerMinute), 2);

                return dayPrice > 20 ? 20 : dayPrice;
            }

            var firstDayIncome = (decimal)(1440 - rentedScooter.StartTime.TimeOfDay.TotalMinutes) * rentedScooter.PricePerMinute;
            var lastdayIncome = (decimal)realEndTime.TimeOfDay.TotalMinutes * rentedScooter.PricePerMinute;
            var maxPricePerDay = 20;
            var daysBetween = Math.Min(1440 * rentedScooter.PricePerMinute, maxPricePerDay) * (decimal)(rentDay - 1);
            var result = Math.Min(firstDayIncome, maxPricePerDay) + daysBetween + Math.Min(lastdayIncome, maxPricePerDay);

            return result;
        }

        public static List<Rent> RentalHistory(int? year, bool includeNotCompletedRentals)
        {
            var yearRelevantScooters = new List<Rent>();
            var rentedScooters = _context.Rent.ToList();


            if (!year.HasValue && !includeNotCompletedRentals)
            {
                yearRelevantScooters = rentedScooters.Where(scooter => scooter.EndTime.HasValue).ToList();
            }
            else if (year.HasValue && includeNotCompletedRentals)
            {
                yearRelevantScooters = rentedScooters.Where(scooter => scooter.EndTime.HasValue && scooter.EndTime.Value.Year == year).ToList();
            }
            else if (includeNotCompletedRentals)
            {
                yearRelevantScooters = rentedScooters.Where(scooter => !scooter.EndTime.HasValue).ToList();
            }
            else
            {
                yearRelevantScooters = rentedScooters.Where(scooter => scooter.EndTime.HasValue && scooter.EndTime.Value.Year == year).ToList();
            }

            return yearRelevantScooters;
        }


    }
}