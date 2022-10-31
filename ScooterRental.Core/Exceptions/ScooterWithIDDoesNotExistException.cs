using System;

namespace ScooterRental.Exceptions
{
    public class ScooterWithIDDoesNotExistException : Exception
    {
        public ScooterWithIDDoesNotExistException(int id) : base($"Scooter with ID {id} does not exist.")
        {
            
        }
    }
}