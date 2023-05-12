using Passengers.Core.Passengers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passengers.ApplicationServices.Passengers
{
    public interface IPassengersAppService
    {
        Task<List<Passenger>> GetPassengersAsync();
        Task<int> AddPassengerAsync(Passenger passenger);
        Task DeletePassengerAsync(int passengerId);
        Task<Passenger> GetPassengerAsync(int passengerId);
        Task EditPassengerAsync(Passenger passenger);
    }
}
