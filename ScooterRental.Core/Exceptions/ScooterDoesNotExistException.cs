using System;

namespace ScooterRental.Exceptions
{
    public class ScooterDoesNotExistException : Exception
    {
        public ScooterDoesNotExistException(int id) : base($"Scooter with Id {id} does not exist.")
        {
            
        }
    }
}