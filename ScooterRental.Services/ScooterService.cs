using System.Collections.Generic;
using System.Linq;
using ScooterRental.Core.Models;
using ScooterRental.Db;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental.Services
{
    public class ScooterService : EntityService<Scooter>, IScooterService
    {
        public ScooterService(ScooterRentalDbContext context) : base(context)
        {
        }

        public Scooter AddScooter(int id, decimal pricePerMinute)
        {

            if (pricePerMinute <= 0)
            {
                throw new InvalidPriceException(pricePerMinute);
            }

            if (_context.Scooters.Any(scooter => scooter.Id == id))
            {
                throw new DuplicateScooterException(id);
            }

            var scooter = new Scooter
            {
                Id = id,
                PricePerMinute = pricePerMinute,
            };
            Create(scooter);
            return scooter;
        }

        public Scooter GetScooterById(int scooterId)
        {

            if (_context.Scooters.Any(scooter => scooter.Id != scooterId))
            {
                throw new ScooterWithIDDoesNotExistException(scooterId);
            }

            return _context.Scooters.FirstOrDefault(scooter => scooter.Id == scooterId);
        }

        public IList<Scooter> GetScooters()
        {
            var scooterList = Query().Where(scooter => scooter.IsRented == false);
            return scooterList.ToList();
        }

        public void RemoveScooter(int id)
        {
            var scooter = _context.Scooters.FirstOrDefault(scooter => scooter.Id == id);

            if (scooter == null)
            {
                throw new ScooterDoesNotExistException(id);
            }
            _context.Scooters.Remove(scooter);
            _context.SaveChanges();
        }
    }
}