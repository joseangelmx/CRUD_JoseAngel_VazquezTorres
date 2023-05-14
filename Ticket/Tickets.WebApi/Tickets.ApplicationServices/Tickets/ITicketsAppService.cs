using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Core.Tickets;

namespace Tickets.ApplicationServices.Tickets
{
    public interface ITicketsAppService
    {
        Task<List<Ticket>> GetTicketsAsync();
        Task<int> AddTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(int ticketId);
        Task<Ticket> GetTicketAsync(int ticketId);
        Task EditTicketAsync(Ticket ticket);
    }
}
