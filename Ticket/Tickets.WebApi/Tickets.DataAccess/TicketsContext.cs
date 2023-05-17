using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Core.Tickets;

namespace Tickets.DataAccess
{
    public class TicketsContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<Ticket> Tickets { get; set; }

        public TicketsContext (DbContextOptions<TicketsContext> options ) : base(options)
        {

        }
    }
}
