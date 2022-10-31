//using System;
//using System.Collections.Generic;
//using FluentAssertions;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ScooterRental.Core.Models;
//using ScooterRental.Core.Services;
//using ScooterRental.Interfaces;
//using ScooterRental.Services;


//namespace ScooterRental.Tests
//{
//    [TestClass]
//    public class RentalDetailsTests
//    {

//        private List<Rent> _rentalList;
//        private ScooterService _scooter;
//        private List<Scooter> _inventory;
//        private IRentalCompany _rentalCompany;

//        [TestInitialize]
//        public void Setup()
//        {
//            _inventory = new List<Scooter>();
//            _scooter = new ScooterService(_inventory);
//            _rentalList = new List<Rent>();
//            _rentalCompany = new RentalCompany("RudoScoo", _scooter, _rentalList); ;
//        }

//        [TestMethod]
//        public void RentalFee_RentFeeForMoreThanDay()
//        {
//            var scooter = new Scooter("1", 0.2m);
//            _scooter.AddScooter(scooter.Id, scooter.PricePerMinute);
//            _scooter.GetScooterById(scooter.Id).IsRented = true;
//            var rentedScooter = new Rent(scooter.Id, scooter.PricePerMinute, DateTime.Now.AddMinutes(-10));
//            _rentalList.Add(rentedScooter);

//            var result = _rentalCompany.EndRent(rentedScooter.Id);
//            scooter.IsRented.Should().BeFalse();

//            rentedScooter.EndTime.HasValue.Should().BeTrue();
//            result.Should().Be(2);
//        }

//        [TestMethod]
//        public void RentalFee_RentStartAtMidnightAndEndNexTDay()
//        {
//            var scooter = new Scooter("1", 0.2m);
//            _scooter.AddScooter(scooter.Id, scooter.PricePerMinute);
//            _scooter.GetScooterById(scooter.Id).IsRented = true;
//            var rentedScooter = new Rent(scooter.Id, scooter.PricePerMinute, DateTime.Now.AddMinutes(-1500));
//            _rentalList.Add(rentedScooter);

//            var result = _rentalCompany.EndRent(rentedScooter.Id);
//            scooter.IsRented.Should().BeFalse();

//            rentedScooter.EndTime.HasValue.Should().BeTrue();
//            result.Should().Be(40);
//        }
//    }
//}