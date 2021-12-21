using System;
using FlashParcsLite.Data;
using FlashParcsLite.Data.Models;
using FlashParcsLite.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;

namespace FlashParcsLite.Tests.Integration.Repositories
{
    public class ParkingLocationRepositoryTests
    {
        private DbContextOptions<ParkingContext> Options => new DbContextOptionsBuilder<ParkingContext>()
              .UseInMemoryDatabase(databaseName: "ParkingDataBase")
              .Options;

        [TearDown]
        public void TearDown()
        {
            ClearDatabase();
        }

        [Test]
        public void TestAddVehicle_WhenLotIsNotFull_UpdatesVehicleCountForLocation()
        {
            CreateDataInDataBase(LotStatus.PartiallyFull);

            using (var context = new ParkingContext(Options))
            {
                var repo = new ParkingLocationRepository(context);
                var location = repo.AddVehicle(1);

                location.VehicleCount.ShouldBe(6);
            }
        }

        [Test]
        public void TestAddVehicle_WhenLotIsFull_ThrowsException()
        {
            CreateDataInDataBase(LotStatus.Full);
            using (var context = new ParkingContext(Options))
            {
                var repo = new ParkingLocationRepository(context);

                Should.Throw<InvalidOperationException>(() => repo.AddVehicle(1));
            }
        }

        [Test]
        public void TestRemoveVehicle_WhenLotIsEmpty_ThrowsException()
        {
            CreateDataInDataBase(LotStatus.Empty);
            using (var context = new ParkingContext(Options))
            {
                var repo = new ParkingLocationRepository(context);

                Should.Throw<InvalidOperationException>(() => repo.RemoveVehicle(1));
            }
        }

        [Test]
        public void TestRemoveVehicle_WhenLotIsNotEmpty_UpdatesVehicleCountForLocation()
        {
            CreateDataInDataBase(LotStatus.PartiallyFull);

            using (var context = new ParkingContext(Options))
            {
                var repo = new ParkingLocationRepository(context);
                var location = repo.RemoveVehicle(1);

                location.VehicleCount.ShouldBe(4);
            }
        }

        private void CreateDataInDataBase(LotStatus lotStatus)
        {
            var parkingLocation = new ParkingLocation
            {
                Id = 1,
                Name = "Mock Location 1",
                VehicleCount = lotStatus == LotStatus.Full ? 10 : (lotStatus == LotStatus.Empty ? 0 : 5),
                Capacity = 10
            };

            using (var context = new ParkingContext(Options))
            {
                context.ParkingLocations.Add(parkingLocation);
                context.SaveChanges();
            };
        }

        private void ClearDatabase()
        {
            using (var context = new ParkingContext(Options))
            {
                context.ParkingLocations.RemoveRange(context.ParkingLocations);
                context.SaveChanges();
            };
        }
    }

    public enum LotStatus
    {
        Full,
        Empty,
        PartiallyFull
    }
}
