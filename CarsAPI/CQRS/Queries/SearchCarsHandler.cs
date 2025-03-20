using CarsAPI.DTOs;
using CarsAPI.Interfaces;
using CarsAPI.Repositories;
using MediatR;

namespace CarsAPI.CQRS.Queries
{
    public class SearchCarsHandler : IRequestHandler<SearchCarsQuery, CarSearchResponse>
    {
        private readonly ICarSearchService _carSearchService;
        private readonly ICarRepository _carRepository;

        public SearchCarsHandler(ICarSearchService carSearchService, ICarRepository carRepository)
        {
            _carSearchService = carSearchService;
            _carRepository = carRepository;
        }

        public async Task<CarSearchResponse> Handle(SearchCarsQuery query, CancellationToken cancellationToken)
        {
            var allCars = await _carRepository.GetAllCarsAsync();

            return await _carSearchService.SearchCarsAsync(query.Request, allCars);
        }
    }
}
