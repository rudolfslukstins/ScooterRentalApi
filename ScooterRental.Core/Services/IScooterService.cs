using System.Collections.Generic;
using ScooterRental.Core.Models;

namespace ScooterRental.Interfaces
{
    public interface IScooterService
    {

        Scooter AddScooter(int id, decimal pricePerMinute);
        void RemoveScooter(int id);
        IList<Scooter> GetScooters();
        Scooter GetScooterById(int scooterId);
    }
}