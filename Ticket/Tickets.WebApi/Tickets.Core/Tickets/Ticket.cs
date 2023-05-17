using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Core.Tickets
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int JourneyId { get; set; }
        [Required]
        public int PassengerId { get; set; }
        [Required]
        public int Seat { get; set; }
    }
}
