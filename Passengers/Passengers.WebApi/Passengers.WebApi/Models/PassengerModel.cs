using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Passengers.WebApi.Models
{
    public class PassengerModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(32)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(32)]
        public string LastName { get; set; }
        [Required]
        public int Age { get; set; }
    }
}
