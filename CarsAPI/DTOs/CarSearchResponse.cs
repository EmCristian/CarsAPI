using CarsAPI.Models;

namespace CarsAPI.DTOs
{
    public class CarSearchResponse
    {
        public IEnumerable<Car> Results { get; set; }
        public Aggregations Aggregations { get; set; }
        public Pagination Pagination { get; set; }
    }
}
