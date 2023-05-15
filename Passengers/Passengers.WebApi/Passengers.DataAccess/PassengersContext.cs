using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Passengers.Core.Passengers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passengers.DataAccess
{
    public class PassengersContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {


        public DbSet<Passenger> Passengers { get; set; }


        public PassengersContext(DbContextOptions<PassengersContext> options) : base(options)
        {

        }
    }
}
