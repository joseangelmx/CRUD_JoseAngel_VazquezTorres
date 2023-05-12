using Microsoft.EntityFrameworkCore;
using Passengers.Core.Passengers;
using Passengers.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passengers.ApplicationServices.Passengers
{
    public class PassengersAppService : IPassengersAppService
    {
        private readonly IRepository<int, Passenger> _repository;
        public PassengersAppService(IRepository<int, Passenger> repository)
        {
            _repository = repository;
        }
        public async Task<int> AddPassengerAsync(Passenger passenger)
        {
            await _repository.AddAsync(passenger);
            return passenger.Id;
        }

        public async Task DeletePassengerAsync(int passengerId)
        {
            await _repository.DeleteAsync(passengerId);
        }

        public async Task EditPassengerAsync(Passenger passenger)
        {
            await _repository.UpdateAsync(passenger);
        }

        public async Task<Passenger> GetPassengerAsync(int passengerId)
        {
            return await _repository.GetAsync(passengerId);
        }

        public async Task<List<Passenger>> GetPassengersAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }
    }
}
