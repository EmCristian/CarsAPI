using System.ComponentModel.DataAnnotations;

namespace CarsAPI.DTOs
{
    public class CarSearchRequest
    {
        public string? Brand { get; set; }
        public int? MinYear { get; set; }
        public int? MaxMileage { get; set; }
        public string? FuelType { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        [RegularExpression("^(price|year|mileage)_(asc|desc)$",
            ErrorMessage = "SortBy must be one of: price_asc, price_desc, year_asc, year_desc, mileage_asc, mileage_desc")]
        /// <summary>
        /// price_asc, price_desc, year_asc, year_desc, mileage_asc, mileage_desc.
        /// </summary>
        public string? SortBy { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
