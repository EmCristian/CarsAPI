using CarsAPI.DTOs;
using CarsAPI.Interfaces;
using CarsAPI.Models;
using CarsAPI.Sorting;

namespace CarsAPI.Services
{
    public class CarSearchService : ICarSearchService
    {
        private readonly ILogger<CarSearchService> _logger;

        public CarSearchService(ILogger<CarSearchService> logger)
        {
            _logger = logger;
        }

        public async Task<CarSearchResponse> SearchCarsAsync(CarSearchRequest request, IEnumerable<Car> allCars)
        {
            _logger.LogInformation("Processing car search request with filters: {Request}", request);

            //filtering
            var filteredCars = FilterCars(allCars, request);


            //Sorting
            var (field, direction) = SortOptionExtensions.FromQueryString(request.SortBy);
            var sortedCars = SortCars(filteredCars, field, direction);

            // Calculate total count before pagination
            int totalCount = sortedCars.Count();

            // Apply pagination
            var pagedCars = sortedCars
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Compute aggregations
            var aggregations = ComputeAggregations(filteredCars);

            return new CarSearchResponse
            {
                Results = pagedCars,
                Aggregations = aggregations,
                Pagination = new Pagination
                {
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize
                }
            };
        }

        private IEnumerable<Car> FilterCars(IEnumerable<Car> cars, CarSearchRequest request)
        {
            var query = cars.AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(request.Brand))
            {
                query = query.Where(c => c.Brand.Contains(request.Brand, StringComparison.OrdinalIgnoreCase));
            }

            if (request.MinYear.HasValue)
            {
                query = query.Where(c => c.Year >= request.MinYear.Value);
            }

            if (request.MaxMileage.HasValue)
            {
                query = query.Where(c => c.Mileage <= request.MaxMileage.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.FuelType))
            {
                query = query.Where(c => c.Fuel_Type.Equals(request.FuelType, StringComparison.OrdinalIgnoreCase));
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(c => c.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(c => c.Price <= request.MaxPrice.Value);
            }

            return query.ToList();
        }

        private IEnumerable<Car> SortCars(IEnumerable<Car> cars, SortField? field, SortDirection direction)
        {
            if (!field.HasValue)
            {
                return cars.OrderBy(c => c.Brand); 
            }

            return field switch
            {
                SortField.Price => direction == SortDirection.Ascending
                    ? cars.OrderBy(c => c.Price)
                    : cars.OrderByDescending(c => c.Price),
                SortField.Year => direction == SortDirection.Ascending
                    ? cars.OrderBy(c => c.Year)
                    : cars.OrderByDescending(c => c.Year),
                SortField.Mileage => direction == SortDirection.Ascending
                    ? cars.OrderBy(c => c.Mileage)
                    : cars.OrderByDescending(c => c.Mileage),
                _ => cars.OrderBy(c => c.Brand)
            };
        }

        private Aggregations ComputeAggregations(IEnumerable<Car> cars)
        {
            if (!cars.Any())
            {
                return new Aggregations
                {
                    AveragePrice = 0,
                    MostCommonFuelType = "N/A",
                    NewestCarYear = 0
                };
            }

            var averagePrice = cars.Average(c => c.Price);

            var mostCommonFuelType = cars
                .GroupBy(c => c.Fuel_Type)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key ?? "N/A";

            var newestCarYear = cars.Max(c => c.Year);

            return new Aggregations
            {
                AveragePrice = Math.Round(averagePrice, 2),
                MostCommonFuelType = mostCommonFuelType,
                NewestCarYear = newestCarYear
            };
        }
    }
}
