using CarsAPI.Models;

namespace CarsAPI.Interfaces
{
    public interface ICarRepository
    {
        Task<List<Car>> GetAllCarsAsync();
    }
}
