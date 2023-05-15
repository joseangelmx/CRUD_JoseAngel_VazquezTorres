using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using Passengers.ApplicationServices.Passengers;
using Microsoft.Extensions.DependencyInjection;
using Passengers.Core.Passengers;
using System.Threading.Tasks;

namespace Passengers.UnitTest
{
    [TestFixture]
    public class PassengersTest
    {
        protected TestServer server;
        protected IPassengersAppService service;
        [OneTimeSetUp]
        public void Setup()
        {
            this.server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            this.service = server.Host.Services.GetService<IPassengersAppService>();
        }

        [Test]
        public async Task AddPassengerAsync_Test()
        {
            // Arrange
            var passenger = new Passenger
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30
            };

            // Act
            var passengerId = await service.AddPassengerAsync(passenger);
            var addedPassenger = await service.GetPassengerAsync(passengerId);

            // Assert
            Assert.AreEqual(passengerId, addedPassenger.Id);
            Assert.AreEqual(passenger.FirstName, addedPassenger.FirstName);
            Assert.AreEqual(passenger.LastName, addedPassenger.LastName);
            Assert.AreEqual(passenger.Age, addedPassenger.Age);
        }
        [Test]
        public async Task GetPassengerAsync_Test()
        {
            // Arrange
            var passenger = new Passenger
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30
            };
            var passengerId = await service.AddPassengerAsync(passenger);

            // Act
            var result = await service.GetPassengerAsync(passengerId);

            // Assert
            Assert.AreEqual(passengerId, result.Id);
            Assert.AreEqual(passenger.FirstName, result.FirstName);
            Assert.AreEqual(passenger.LastName, result.LastName);
            Assert.AreEqual(passenger.Age, result.Age);
        }


        [Test]
        public async Task GetPassengersAsync_Test()
        {
            // Arrange
            var passenger1 = new Passenger
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30
            };
            var passenger2 = new Passenger
            {
                FirstName = "Jane",
                LastName = "Doe",
                Age = 25
            };
            await service.AddPassengerAsync(passenger1);
            await service.AddPassengerAsync(passenger2);

            // Act
            var result = await service.GetPassengersAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(passenger1.FirstName, result[0].FirstName);
            Assert.AreEqual(passenger2.FirstName, result[1].FirstName);
        }
     
        [Test]
        public async Task EditPassengerAsync_Test()
        {
            // Arrange
            var passenger = new Passenger
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30
            };
            var passengerId = await service.AddPassengerAsync(passenger);

            // Act
            var editedPassenger = await service.GetPassengerAsync(passengerId);
            editedPassenger.FirstName = "Jack";
            editedPassenger.Age = 35;
            await service.EditPassengerAsync(editedPassenger);

            // Assert
            var result = await service.GetPassengerAsync(passengerId);
            Assert.NotNull(result);
            Assert.AreEqual(editedPassenger.Id, result.Id);
            Assert.AreEqual(editedPassenger.FirstName, result.FirstName);
            Assert.AreEqual(editedPassenger.LastName, result.LastName);
            Assert.AreEqual(editedPassenger.Age, result.Age);
        }




        [Test]
        public async Task DeletePassengerAsync_Test()
        {
            // Arrange
            var passenger = new Passenger
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30
            };
            var passengerId = await service.AddPassengerAsync(passenger);

            // Act
            await service.DeletePassengerAsync(passengerId);
            var result = await service.GetPassengerAsync(passengerId);

            // Assert
            Assert.IsNull(result);
        }
    }
}