using Journeys.ApplicationServices.Journeys;
using Journeys.Core.Journeys;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journeys.UnitTest
{
    public class JourneysTest
    {
        protected TestServer server;
        protected IJourneysAppService service;

        [OneTimeSetUp]
        public void Setup()
        {
            this.server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            this.service = server.Host.Services.GetService<IJourneysAppService>();
        }
        [Test]
        public async Task AddJourneyAsync_Test()
        {
            // Arrange
            var journey = new Journey
            {
                DestinationId = 3,
                OriginId = 5,
                Departure = new DateTime(2023, 5, 3),
                Arrival = new DateTime(2023, 5, 4)
            };
            var journeyId = await service.AddJourneyAsync(journey);

            // Act
            var addedJourney = await service.GetJourneyAsync(journeyId);

            // Assert
            Assert.AreEqual(journeyId, addedJourney.Id);
            Assert.AreEqual(journey.DestinationId, addedJourney.DestinationId);
            Assert.AreEqual(journey.OriginId, addedJourney.OriginId);
            Assert.AreEqual(journey.Departure, addedJourney.Departure);
            Assert.AreEqual(journey.Arrival, addedJourney.Arrival);
        }
        [Test]
        public async Task GetJourneyAsync_Test()
        {
            // Arrange
            var journey = new Journey
            {
                DestinationId = 3,
                OriginId = 5,
                Departure = new DateTime(2023, 5, 3),
                Arrival = new DateTime(2023, 5, 4)
            };
            var journeyId = await service.AddJourneyAsync(journey);

            // Act
            var result = await service.GetJourneyAsync(journeyId);

            // Assert
            Assert.AreEqual(journeyId, result.Id);
            Assert.AreEqual(journey.DestinationId, result.DestinationId);
            Assert.AreEqual(journey.OriginId, result.OriginId);
            Assert.AreEqual(journey.Departure, result.Departure);
            Assert.AreEqual(journey.Arrival, result.Arrival);
        }
        [Test]
        public async Task GetJourneysAsync_Test()
        {
            // Arrange
            var journey = new Journey
            {
                DestinationId = 3,
                OriginId = 5,
                Departure = new DateTime(2023, 5, 3),
                Arrival = new DateTime(2023, 5, 4)
            };
            var journey2 = new Journey
            {
                DestinationId = 4,
                OriginId = 7,
                Departure = new DateTime(2023, 5, 6),
                Arrival = new DateTime(2023, 5, 7)
            };
            await service.AddJourneyAsync(journey);
            await service.AddJourneyAsync(journey2);

            // Act
            var result = await service.GetJourneysAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(journey.DestinationId, result[0].DestinationId);
            Assert.AreEqual(journey2.DestinationId, result[1].DestinationId);
        }

        [Test]
        public async Task EditJourneyAsync_Test()
        {
            // Arrange
            var journey = new Journey
            {
                DestinationId = 3,
                OriginId = 5,
                Departure = new DateTime(2023, 5, 3),
                Arrival = new DateTime(2023, 5, 4)
            };
            var journeyId = await service.AddJourneyAsync(journey);

            // Act
            var editedJourney = await service.GetJourneyAsync(journeyId);
            editedJourney.DestinationId = 1;
            editedJourney.OriginId = 3;
            await service.EditJourneyAsync(editedJourney);

            // Assert
            var result = await service.GetJourneyAsync(journeyId);
            Assert.NotNull(result);
            Assert.AreEqual(editedJourney.Id, result.Id);
            Assert.AreEqual(editedJourney.DestinationId, result.DestinationId);
            Assert.AreEqual(editedJourney.OriginId, result.OriginId);
            Assert.AreEqual(editedJourney.Departure, result.Departure);
            Assert.AreEqual(editedJourney.Arrival, result.Arrival);
        }

        [Test]
        public async Task DeleteJourneyAsync_Test()
        {
            // Arrange
            var journey = new Journey
            {
                DestinationId = 3,
                OriginId = 5,
                Departure = new DateTime(2023,5,3),
                Arrival = new DateTime(2023,5,4)
            };
            var journeyId = await service.AddJourneyAsync(journey);

            // Act
            await service.DeleteJourneyAsync(journeyId);
            var result = await service.GetJourneyAsync(journeyId);

            // Assert
            Assert.IsNull(result);
        }
    }
}
