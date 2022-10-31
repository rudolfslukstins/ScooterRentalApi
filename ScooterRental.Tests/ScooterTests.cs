using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScooterRental.Core.Models;
using ScooterRental.Interfaces;

namespace ScooterRental.Tests
{
    [TestClass]
    public class ScooterTests
    {
        private IScooterService _service;

        [TestMethod]
        public void ScooterCreation_IDAndPricePerMinuteSetCorrectly()
        {
            var scooter = _service.AddScooter(1, 0.2m);
            
            scooter.Id.Should().Be(1);
            scooter.PricePerMinute.Should().Be(0.2m);
            scooter.IsRented.Should().BeFalse();
        }
    }
}