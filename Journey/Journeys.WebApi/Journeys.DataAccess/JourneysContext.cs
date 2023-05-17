using Journeys.Core.Journeys;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journeys.DataAccess
{
    public class JourneysContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<Journey> Journeys { get; set; }
        public JourneysContext(DbContextOptions<JourneysContext> options) : base(options)
        {

        }
    }

}
