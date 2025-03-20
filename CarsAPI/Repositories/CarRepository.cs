using CarsAPI.Interfaces;
using CarsAPI.Models;
using CarsAPI.Services;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace CarsAPI.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly string _filePath;
        private readonly ILogger<CarRepository> _logger;

        public CarRepository(IConfiguration configuration, ILogger<CarRepository> logger)
        {
            _filePath = configuration.GetValue<string>("CarDataFilePath");

            if (string.IsNullOrEmpty(_filePath))
            {
                _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "car_price_dataset.csv");
            }
            _logger = logger;
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            var cars = new List<Car>();

            try
            {
                using (var reader = new StreamReader(_filePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true, // Tells CSVHelper that the first row contains column names
                    Delimiter = "," // Specify delimiter if it's not a comma (default)
                }))
                {
                    // Use the async version and directly convert it to a list
                    await foreach (var car in csv.GetRecordsAsync<Car>())
                    {
                        cars.Add(car);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading data from the CSV file: {FilePath}", _filePath);
            }

            return cars;
        }
    }
}
