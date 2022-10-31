using ScooterRental.Core.Models;

namespace ScooterRental.Core.Services
{
    public interface IRentalCompany
    {
        Scooter StartRent(int id);
        string EndRent(int id);
        decimal CalculateIncome(int? year, bool includeNotCompletedRentals);
    }
}