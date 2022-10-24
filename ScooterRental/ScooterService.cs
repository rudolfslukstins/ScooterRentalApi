using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        private readonly List<Scooter> _scooters;

        public ScooterService(List<Scooter> inventory)
        {
            _scooters = inventory;
        }
        public void AddScooter(string id, decimal pricePerMinute)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidIDException();
            }

            if (pricePerMinute <= 0)
            {
                throw new InvalidPriceException(pricePerMinute);
            }

            if (_scooters.Any(scooter => scooter.Id == id))
            {
                throw new DuplicateScooterException(id);
            }

            _scooters.Add(new Scooter(id, pricePerMinute));
        }

        public Scooter GetScooterById(string scooterId)
        {
            if (string.IsNullOrWhiteSpace(scooterId))
            {
                throw new SearchScooterInvalidIDException();
            }

            if (_scooters.Any(scooter => scooter.Id != scooterId))
            {
                throw new ScooterWithIDDoesNotExistException(scooterId);
            }

            return _scooters.Find(scooter => scooter.Id == scooterId);
        }

        public IList<Scooter> GetScooters()
        {
            return _scooters.FindAll(scooter => !scooter.IsRented).ToList();
        }

        public void RemoveScooter(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidIDException();
            }

            var scooter = _scooters.FirstOrDefault(scooter => scooter.Id == id);

            if (scooter == null)
            {
                throw new ScooterDoesNotExistException(id);
            }
            _scooters.Remove(scooter);
        }
    }
}