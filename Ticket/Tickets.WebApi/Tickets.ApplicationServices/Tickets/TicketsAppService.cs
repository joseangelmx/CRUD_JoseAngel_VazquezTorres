using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Core.Tickets;
using Tickets.DataAccess.Repositories;

namespace Tickets.ApplicationServices.Tickets
{
    public class TicketsAppService : ITicketsAppService
    {
        private readonly IRepository<int, Ticket> _repository;
        public TicketsAppService(IRepository<int, Ticket> repository)
        {
            _repository = repository;
        }
        public async Task<int> AddTicketAsync(Ticket ticket)
        {
            await _repository.AddAsync(ticket);
            return ticket.Id;
        }

        public async Task DeleteTicketAsync(int ticketId)
        {
            await _repository.DeleteAsync(ticketId);
        }

        public async Task EditTicketAsync(Ticket ticket)
        {
            await _repository.UpdateAsync(ticket);
        }

        public async Task<Ticket> GetTicketAsync(int ticketId)
        {
            return await _repository.GetAsync(ticketId);
        }

        public async Task<List<Ticket>> GetTicketsAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }
    }
}
