using System;

namespace ScooterRental.Exceptions
{
    public class ScooterDoesNotExistException : Exception
    {
        public ScooterDoesNotExistException(string id) : base($"Scooter with Id {id} does not exist.")
        {
            
        }
    }
}