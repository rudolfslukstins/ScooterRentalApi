using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace ScooterRental
{
    public class Rent
    {
        public string Id { get; set; }
        public decimal PricePerMinute { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        
        public Rent(string id, decimal pricePerMinute, DateTime startTime)
        {
            Id = id;
            PricePerMinute = pricePerMinute;
            StartTime = startTime;
        }
    }
}