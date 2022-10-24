using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental.Tests
{
    [TestClass]
    public class ScooterServiceTests
    {
        private IScooterService _scooterService;
        private List<Scooter> _inventory;

        [TestInitialize]
        public void Setup()
        {
            _inventory = new List<Scooter>();
            _scooterService = new ScooterService(_inventory);
        }

        [TestMethod]
        public void AddScooter_AddValidScooter_ScooterAdded()
        {
            _scooterService.AddScooter("1",0.2m);
            _inventory.Count.Should().Be(1);
        }

        [TestMethod]
        public void AddScooter_AddScooterTwice_ThrowsDuplicateScooterException()
        {
            _scooterService.AddScooter("1",0.2m);

            Action act = () =>
                _scooterService.AddScooter("1", 0.2m);
            act.Should().Throw<DuplicateScooterException>()
                .WithMessage("Scooter with id 1 already exists");
        }

        [TestMethod]
        public void AddScooter_AddScooterWithNegativePriceZeroOrLess_ThrowsInvalidPriceException()
        {
            Action act = () =>
                _scooterService.AddScooter("1", -0.2m);
            act.Should().Throw<InvalidPriceException>()
                .WithMessage("Given price -0.2 not valid.");
        }

        [TestMethod]
        public void AddScooter_AddScooterNullOrEmptyID_ThrowsInvalidIDException()
        {
            Action act = () =>
                _scooterService.AddScooter("", 0.2m);
            act.Should().Throw<InvalidIDException>()
                .WithMessage("Id cannot be null or empty.");
        }

        [TestMethod]
        public void RemoveScooter_ScooterExists_ScooterIsRemoved()
        {
            _inventory.Add(new Scooter("1",0.2m));

            _scooterService.RemoveScooter("1");

            _inventory.Any(scooter => scooter.Id == "1").Should().BeFalse();
            _inventory.Count.Should().Be(0);
        }

        [TestMethod]
        public void RemoveScooter_ScooterDoesNotExist_ThrowsScooterDoesNotExistException()
        {
            Action act = () => 
                _scooterService.RemoveScooter("1");

            act.Should().Throw<ScooterDoesNotExistException>()
                .WithMessage("Scooter with Id 1 does not exist.");
        }

        [TestMethod]
        public void RemoveScooter_RemoveScooterNullOrEmptyIDGiven_ThrowsInvalidIDException()
        {
            Action act = () =>
                _scooterService.RemoveScooter("");

            act.Should().Throw<InvalidIDException>()
                .WithMessage("Id cannot be null or empty.");
        }

        [TestMethod]
        public void GetScooter_GetAllAvailableScootersInInventory_ReturnedAllExistingScooters()
        {
            _scooterService.GetScooters().Count.Should().Be(0);
        }

        [TestMethod]
        public void GetScooter_AddingScooter_CheckIfReturnsScooter()
        {
            _inventory.Add(new Scooter("1", 0.2m));
            _scooterService.GetScooters().Count.Should().Be(1);
        }

        [TestMethod]
        public void GetScooter_AddingMultipleScooters_CheckIfReturnsMultipleScooters()
        {
            _inventory.Add(new Scooter("1", 0.2m));
            _inventory.Add(new Scooter("2", 0.2m));

            _scooterService.GetScooters().Count.Should().Be(2);
        }

        [TestMethod]
        public void GetScooter_AddingTwoScooters_OnlyOneIsAvailable()
        {
            _inventory.Add(new Scooter("1", 0.2m));
            var rentedScooter = new Scooter("2", 0.2m);
            rentedScooter.IsRented = true;
            _inventory.Add(rentedScooter);

            _scooterService.GetScooters().Count.Should().Be(1);
        }

        [TestMethod]
        public void GetScooter_ReturnListOfScooters()
        {
            _inventory.Add(new Scooter("1", 0.2m));
            var scooter = _scooterService.GetScooters();
            scooter.Add(new Scooter("2",0.2m));
            var scooterList_2 = _scooterService.GetScooters();

            scooterList_2.Count.Should().Be(1);
        }

        [TestMethod]
        public void GetScooterByID_AddScooter_CheckIfReturnsScooterWithNeededID()
        {
            var scooter = new Scooter("1", 0.2m);
            _inventory.Add(scooter);

            _scooterService.GetScooterById("1").Should().Be(scooter);
        }

        [TestMethod]
        public void GetScooterByID_GetScooterWithNonExistentID_ThrowsScooterWithIDDoesNotExistException()
        {
            _inventory.Add(new Scooter("1", 0.2m));
            Action act = () => 
                _scooterService.GetScooterById("2");

            act.Should().Throw<ScooterWithIDDoesNotExistException>()
                .WithMessage("Scooter with ID 2 does not exist.");
        }

        [TestMethod]
        public void GetScooterByID_ScooterNullOrEmptyIDGiven_ThrowsInvalidIDException()
        {
            Action act = () =>
                _scooterService.GetScooterById("");

            act.Should().Throw<SearchScooterInvalidIDException>()
                .WithMessage("Id cannot be null or empty.");
        }

        [TestMethod]
        public void GetScooterByID_ScooterIsRented_CheckIfReturnsID()
        {
            var rentedScooter = new Scooter("1", 0.2m);
            rentedScooter.IsRented = true;
            _inventory.Add(rentedScooter);

            _scooterService.GetScooterById("1").Should().Be(rentedScooter);
        }
    }
}