using System;

namespace ScooterRental.Exceptions
{
    public class SearchScooterInvalidIDException : Exception
    {
        public SearchScooterInvalidIDException() : base("Id cannot be null or empty.")
        {

        }
    }
}