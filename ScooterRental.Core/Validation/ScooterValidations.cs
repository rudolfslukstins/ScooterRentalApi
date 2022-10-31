using System.Data;
using FluentValidation;
using ScooterRental.Core.Models;

namespace ScooterRental.Core.Validation
{
    public class ScooterValidations : AbstractValidator<Scooter>
    {
        public ScooterValidations()
        {
            RuleFor(scooter=> scooter.PricePerMinute).NotEmpty().NotNull();
            RuleFor(scooter => scooter.Id).NotEmpty().NotNull();
        }
    }
}