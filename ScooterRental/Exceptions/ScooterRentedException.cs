using System;

namespace ScooterRental.Exceptions
{
    public class ScooterRentedException: Exception
    {
        public ScooterRentedException() : base($"This scooter is already rented.")
        {
            
        }
    }
}