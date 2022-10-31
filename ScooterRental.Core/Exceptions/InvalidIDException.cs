using System;

namespace ScooterRental.Exceptions
{
    public class InvalidIDException : Exception
    {
        public InvalidIDException() : base("Id cannot be null or empty.")
        {

        }
    }
}