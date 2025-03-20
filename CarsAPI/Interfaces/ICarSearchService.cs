using CarsAPI.DTOs;
using CarsAPI.Models;

namespace CarsAPI.Interfaces
{
    public interface ICarSearchService
    {
        Task<CarSearchResponse> SearchCarsAsync(CarSearchRequest request, IEnumerable<Car> allCars);
    }
}
