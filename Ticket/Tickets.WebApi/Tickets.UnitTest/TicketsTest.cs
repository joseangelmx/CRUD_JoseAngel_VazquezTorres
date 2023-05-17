using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using Tickets.ApplicationServices.Tickets;
using Microsoft.Extensions.DependencyInjection;
using Tickets.Core.Tickets;
using System.Threading.Tasks;

namespace Tickets.UnitTest
{
    [TestFixture]
    public class TicketsTest
    {
        protected TestServer server;
        protected ITicketsAppService service;
        [OneTimeSetUp]
        public void Setup()
        {
            this.server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            this.service = server.Host.Services.GetService<ITicketsAppService>();
        }

        [Test]
        public async Task AddTicketAsync_Test()
        {
            // Arrange
            var ticket = new Ticket
            {
                JourneyId = 3,
                PassengerId = 2,
                Seat = 3
            };

            // Act
            var ticketId = await service.AddTicketAsync(ticket);
            var addedTicket = await service.GetTicketAsync(ticketId);

            // Assert
            Assert.AreEqual(ticketId, addedTicket.Id);
            Assert.AreEqual(ticket.JourneyId, addedTicket.JourneyId);
            Assert.AreEqual(ticket.PassengerId, addedTicket.PassengerId);
            Assert.AreEqual(ticket.Seat, addedTicket.Seat);
        }
        [Test]
        public async Task GetTicketAsync_Test()
        {
            // Arrange
            var ticket = new Ticket
            {
                JourneyId = 3,
                PassengerId = 2,
                Seat = 3
            };

            // Act
            var ticketId = await service.AddTicketAsync(ticket);
            var result = await service.GetTicketAsync(ticketId);

            // Assert
            Assert.AreEqual(ticketId, result.Id);
            Assert.AreEqual(ticket.JourneyId, result.JourneyId);
            Assert.AreEqual(ticket.PassengerId, result.PassengerId);
            Assert.AreEqual(ticket.Seat, result.Seat);
        }


        [Test]
        public async Task GetTicketsAsync_Test()
        {
            // Arrange
            var ticket = new Ticket
            {
                JourneyId = 3,
                PassengerId = 2,
                Seat = 3
            };
            var ticket2 = new Ticket
            {
                JourneyId = 4,
                PassengerId = 2,
                Seat = 3
            };
            await service.AddTicketAsync(ticket);
            await service.AddTicketAsync(ticket2);

            // Act
            var result = await service.GetTicketsAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(ticket.JourneyId, result[0].JourneyId);
            Assert.AreEqual(ticket2.JourneyId, result[1].JourneyId);
        }

        [Test]
        public async Task EditTicketAsync_Test()
        {
            // Arrange
            var ticket = new Ticket
            {
                JourneyId = 3,
                PassengerId = 2,
                Seat = 3
            };
            var ticketId = await service.AddTicketAsync(ticket);

            // Act
            var editedTicket = await service.GetTicketAsync(ticketId);
            editedTicket.JourneyId = 7;
            editedTicket.Seat = 35;
            await service.EditTicketAsync(editedTicket);

            // Assert
            var result = await service.GetTicketAsync(ticketId);
            Assert.NotNull(result);
            Assert.AreEqual(ticketId, result.Id);
            Assert.AreEqual(editedTicket.JourneyId, result.JourneyId);
            Assert.AreEqual(editedTicket.PassengerId, result.PassengerId);
            Assert.AreEqual(editedTicket.Seat, result.Seat);
        }
        [Test]
        public async Task DeleteTicketAsync_Test()
        {
            // Arrange
            var ticket = new Ticket
            {
                JourneyId = 3,
                PassengerId = 2,
                Seat = 3
            };
            var ticketId = await service.AddTicketAsync(ticket);
            // Act
            await service.DeleteTicketAsync(ticketId);
            var result = await service.GetTicketAsync(ticketId);

            // Assert
            Assert.IsNull(result);
        }
    }
}