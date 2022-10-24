using System;
using System.Collections.Generic;
using System.Linq;

namespace ScooterRental
{
    public static class RentalDetails 
    {
        public static decimal RentalFee(Rent rentedScooter)
        {
            var realEndTime = rentedScooter.EndTime ?? DateTime.UtcNow;
            var rentDay = (realEndTime.Date - rentedScooter.StartTime.Date).TotalDays;

            if (rentDay == 0)
            {
                var dayPrice = Math.Round(((decimal)(realEndTime - rentedScooter.StartTime)
                    .TotalMinutes * rentedScooter.PricePerMinute),2);

                return dayPrice > 20 ? 20 : dayPrice;
            }

            var firstDayIncome = (decimal)(1440 - rentedScooter.StartTime.TimeOfDay.TotalMinutes) * rentedScooter.PricePerMinute;
            var lastdayIncome =  (decimal)realEndTime.TimeOfDay.TotalMinutes * rentedScooter.PricePerMinute;
            var maxPricePerDay = 20;
            var daysBetween = Math.Min(1440 * rentedScooter.PricePerMinute, maxPricePerDay) * (decimal)(rentDay - 1) ;
            var result = Math.Min(firstDayIncome, maxPricePerDay) + daysBetween + Math.Min(lastdayIncome, maxPricePerDay);

            return result;
        }

        public static List<Rent> RentalHistory(List<Rent> rentedScooters,int? year, bool includeNotCompletedRentals)
        {
            var yearRelevantScooter = new List<Rent>();


            if (!year.HasValue && !includeNotCompletedRentals)
            {
                yearRelevantScooter = rentedScooters.Where(scooter => scooter.EndTime.HasValue).ToList();
            }
            else if (year.HasValue && includeNotCompletedRentals)
            {
                yearRelevantScooter = rentedScooters.Where(scooter => scooter.EndTime.HasValue && scooter.EndTime.Value.Year == year).ToList();
            }
            else if (includeNotCompletedRentals)
            {
                yearRelevantScooter = rentedScooters.Where(scooter => !scooter.EndTime.HasValue).ToList();
            }
            else
            {
                yearRelevantScooter = rentedScooters.Where(scooter => scooter.EndTime.HasValue && scooter.EndTime.Value.Year == year).ToList();
            }

            return yearRelevantScooter;
        }
    }
}