using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using static System.Collections.Specialized.BitVector32;
using System.Diagnostics;

namespace ScooterRental.Tests
{
    [TestClass]
    public class RentalCompanyTests
    {
        private IRentalCompany _rentalCompany;
        private List<Rent> _rentalList;
        private ScooterService _scooter;
        private List<Scooter> _inventory;

        [TestInitialize]
        public void Setup()
        {
            _inventory = new List<Scooter>();
            _scooter = new ScooterService(_inventory);
            _rentalList = new List<Rent>();
            _rentalCompany = new RentalCompany("RudoScoo", _scooter, _rentalList);
        }

        [TestMethod]
        public void StartRent_StartRentWithScooterAlreadyRenter_ThrowScooterRentedException()
        {
            var newScooter = new Scooter("1", 0.2m);
            newScooter.IsRented = true;
            _inventory.Add(newScooter);

            Action act = () =>
                _rentalCompany.StartRent(newScooter.Id);

            act.Should().Throw<ScooterRentedException>()
                .WithMessage("This scooter is already rented.");
        }

        [TestMethod]
        public void StartRent_StartRentWithScooter_ShowsThatScooterIsRented()
        {
            var newScooter = new Scooter("1", 0.2m);
            _scooter.AddScooter(newScooter.Id,newScooter.PricePerMinute);

            var rentedScooter = new Rent(newScooter.Id, newScooter.PricePerMinute, DateTime.Now);
            _rentalCompany.StartRent(rentedScooter.Id);

            _rentalList.Count.Should().Be(1);
        }

        [TestMethod]
        public void EndRent_EndRentingOfScooter_ScooterIsNotRented()
        {
            
            var scooter = new Scooter("1", 0.2m);
            _scooter.AddScooter(scooter.Id, scooter.PricePerMinute);
            _scooter.GetScooterById(scooter.Id).IsRented = true;
            
            var rentedScooter = new Rent(scooter.Id, scooter.PricePerMinute, DateTime.Now);
            _rentalList.Add(rentedScooter);
            _rentalCompany.EndRent(rentedScooter.Id);
            
            _scooter.GetScooterById(scooter.Id).IsRented.Should().BeFalse();
            rentedScooter.EndTime.Should().HaveValue();
        }

        [TestMethod]
        public void EndRent_ScooterDoesNotExist_ThrowsScooterDoesNotExistException()
        {
            var newScooter = new Scooter("2", 0.2m);
            _scooter.AddScooter(newScooter.Id, newScooter.PricePerMinute);
            _scooter.GetScooterById(newScooter.Id).IsRented = true;
            var rentedscooter = new Rent(newScooter.Id, newScooter.PricePerMinute, DateTime.Now);
            _rentalList.Add(rentedscooter);

            Action act = () =>
                _rentalCompany.EndRent("1");

            act.Should().Throw<ScooterWithIDDoesNotExistException>()
                .WithMessage($"Scooter with ID 1 does not exist.");
        }

        [TestMethod]
        public void EndRent_EndRentWithScooterWithInValidID_ThrowsInvalidIDException()
        {
            var newScooter = new Scooter("2", 0.2m);
            _scooter.AddScooter(newScooter.Id, newScooter.PricePerMinute);
            _scooter.GetScooterById(newScooter.Id).IsRented = true;
            var rentedScooter = new Rent(newScooter.Id, newScooter.PricePerMinute, DateTime.Now);
            _rentalList.Add(rentedScooter);

            Action act = () =>
                _rentalCompany.EndRent("");

            act.Should().Throw<SearchScooterInvalidIDException>()
                .WithMessage("Id cannot be null or empty.");
        }

        [TestMethod]
        public void EndRent_EndRentOneDay()
        {
            var scooter = new Scooter("1", 0.2m);
            _scooter.AddScooter(scooter.Id, scooter.PricePerMinute);
            _scooter.GetScooterById(scooter.Id).IsRented = true;
            var rentedScooter = new Rent(scooter.Id, scooter.PricePerMinute, DateTime.Now.AddMinutes(-10));
            _rentalList.Add(rentedScooter);

            var result = _rentalCompany.EndRent(rentedScooter.Id);
            scooter.IsRented.Should().BeFalse();

            rentedScooter.EndTime.HasValue.Should().BeTrue();
            result.Should().Be(2);
        }

        [TestMethod]
        [DataRow(2022, true, 1892)]
        [DataRow(2020,true,24)]
        [DataRow(2021,true,656)]
        [DataRow(2022,false,1892)]
        [DataRow(null,false,2572)]
        [DataRow(null,true,-34)]

        public void CalculateIncome_CalculateIncomeWithTwoScootersInSameYear(int? year , bool includeNotCompletedRentals, double expectedResult)
        {
            decimal expect = Convert.ToDecimal(expectedResult);
            MockHistoryScooters();
            var result = _rentalCompany.CalculateIncome(year, includeNotCompletedRentals);
            
            result.Should().Be(expect);
        }

        [TestMethod]

        public void Name_GetName_Name()
        {
            _rentalCompany.Name.Should().Be("RudoScoo");
        }

        public void MockHistoryScooters()
        {
            _rentalList.Add(new Rent("1",0.2m,new DateTime(2019, 12,31,23,00,0)));
            _rentalList[0].EndTime = new DateTime(2020, 1, 1, 1, 0, 0);//24

            _rentalList.Add(new Rent("2", 0.2m, new DateTime(2020, 12, 1, 23, 00, 0)));
            _rentalList[1].EndTime = new DateTime(2021, 1,1,1,0,0);//624

            _rentalList.Add(new Rent("3", 0.2m, new DateTime(2021, 11, 1, 23, 0, 0)));
            _rentalList[2].EndTime = new DateTime(2021,11 , 2, 20, 0, 0);//32

            _rentalList.Add(new Rent("4", 0.2m, new DateTime(2022, 9, 1, 1, 0, 0)));
            _rentalList[3].EndTime = new DateTime(2022, 9,2 , 3, 0, 0);//40

            _rentalList.Add(new Rent("5", 0.2m, new DateTime(2022, 6, 1, 1, 0, 0)));
            _rentalList[4].EndTime = new DateTime(2022, 9, 1, 1, 0, 0);//1852

            _rentalList.Add(new Rent("6", 0.2m, DateTime.Now.AddMinutes(-10)));//2
        }
    }
}