using System.Linq;
using System.Threading.Tasks;
using FlashParcsLite.API.Controllers;
using FlashParcsLite.API.Hubs;
using FlashParcsLite.API.Models;
using FlashParcsLite.Data.Models;
using FlashParcsLite.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace FlashParcsLite.Tests.Unit.API
{
    public class ParkingLocationControllerTests
    {
        private Mock<ILogger<ParkingLocationController>> _mockLogger;
        private Mock<IParkingLocationRepository> _mockParkingLocationRepo;
        private Mock<IHubContext<ParkingHub>> _mockParkingHub;
        private Mock<IHubClients> _mockClients;
        private Mock<IClientProxy> _mockClientProxy;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<ParkingLocationController>>();
            _mockParkingLocationRepo = new Mock<IParkingLocationRepository>();


            _mockClientProxy = new Mock<IClientProxy>();
            _mockClients = new Mock<IHubClients>();
            _mockClients.Setup(r => r.All).Returns(_mockClientProxy.Object);
            _mockParkingHub = new Mock<IHubContext<ParkingHub>>();
            _mockParkingHub.Setup(r => r.Clients).Returns(_mockClients.Object);
        }

        [Test]
        public void GetLocations_WithNoLocations_ReturnsNotFound()
        {
            _mockParkingLocationRepo.Setup(s => s.GetParkingLocations()).Returns(Enumerable.Empty<ParkingLocation>());

            var controller = new ParkingLocationController(
                logger: _mockLogger.Object,
                parkingLocationRepository: _mockParkingLocationRepo.Object,
                hubContext: _mockParkingHub.Object);

            var actualResult = controller.Get();
            actualResult.ShouldBeOfType<NotFoundResult>();
        }

        [Test]
        public async Task UpdateVehicleCount_IncreaseVehicleCount_PublishesTheUpdatedLocationOnHubAsync()
        {
            var updatedParkingLocation = new ParkingLocation
            {
                Id = 1,
                Name = "Mock Location",
                VehicleCount = 1,
                Capacity = 10
            };
            var vehicleUpdateDto = new VehicleUpdateDto
            {
                ParkingLocationId = 1,
                UpdateVehicleCount = UpdateVehicleCount.Increase
            };

            _mockParkingLocationRepo.Setup(s => s.AddVehicle(vehicleUpdateDto.ParkingLocationId)).Returns(updatedParkingLocation);

            var controller = new ParkingLocationController(
                logger: _mockLogger.Object,
                parkingLocationRepository: _mockParkingLocationRepo.Object,
                hubContext: _mockParkingHub.Object);

            

            var actualResult = await controller.UpdateVehicleCountAsync(vehicleUpdateDto);

            _mockParkingLocationRepo.Verify(v => v.AddVehicle(vehicleUpdateDto.ParkingLocationId), Times.Once);

            _mockClients.Verify(c => c.All);
        }
    }
}
