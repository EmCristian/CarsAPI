
# Car Search API

This project is an API for searching and filtering car prices from a CSV file. Using ASP.NET Core, CsvHelper for parsing the CSV file, and MediatR for request handling, the application allows advanced searches, sorting, and pagination of car prices.

## Technologies Used

- **ASP.NET Core** - Main framework for building the API.
- **CsvHelper** - Used for reading and loading data from the CSV file.
- **MediatR** - Used for handling requests and communication between components in the application.
- **Postman** - For testing the API with predefined HTTP requests.

## Setup

### 1. Installation

To run the application locally, follow these steps:

1. Clone the repository:
   ```bash
   git clone https://github.com/EmCristian/CarsAPI.git
   ```

2. Install dependencies using `dotnet`:
   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. The API will be available at `http://localhost:xxxx`.

### 2. Configurations

In the `launchSettings.json` file, you can set the environment to development or production:

```json
"ASPNETCORE_ENVIRONMENT": "Development", // for development
// "ASPNETCORE_ENVIRONMENT": "Production"  // for production
```

In the development environment, error details will be exposed for easier debugging, while in production, errors will not reveal sensitive information.

### 3. Dependencies

In the `DependenciesConfig.cs` file, the dependencies for MediatR and API services are registered:

```csharp
public static void RegisterDependencies(this IServiceCollection services)
{
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
    services.AddScoped<ICarRepository, CarRepository>();
    services.AddScoped<ICarSearchService, CarSearchService>();
}
```

### 4. CSV File

The CSV file with the car data is stored in the `Data` folder. The path to this file is also configured in `appsettings.json`, so you can modify its location based on your needs.

### 5. Sorting Enums

The application allows sorting cars based on several fields, defined in enums:

```csharp
public enum SortField
{
    Price,
    Year,
    Mileage
}

public enum SortDirection
{
    Ascending,
    Descending
}
```

## API Endpoints

### 1. Search Cars

The main endpoint for searching cars is `GET /api/cars/search`. You can filter the search by various fields, such as `brand`, `minYear`, `maxPrice`, and others.

#### Example requests:

- General search:
  ```http
  GET {{baseUrl}}/api/cars/search
  ```

- Search by brand:
  ```http
  GET {{baseUrl}}/api/cars/search?brand=ford
  ```

- Search with filters for year, price, and sorting:
  ```http
  GET {{baseUrl}}/api/cars/search?brand=ford&minYear=2018&maxPrice=25000&sortBy=year_desc
  ```

- Search by fuel type:
  ```http
  GET {{baseUrl}}/api/cars/search?fuelType=Electric
  ```

- Pagination:
  ```http
  GET {{baseUrl}}/api/cars/search?page=2&pageSize=5
  ```

- Sorting by price:
  ```http
  GET {{baseUrl}}/api/cars/search?sortBy=price_desc
  ```

- Complex search with multiple filters:
  ```http
  GET {{baseUrl}}/api/cars/search?brand=ford&minYear=2018&maxMileage=100000&fuelType=Petrol&minPrice=10000&maxPrice=25000&sortBy=year_desc&page=1&pageSize=5
  ```

### 2. Postman Collection

[Download Postman Collection](https://github.com/EmCristian/CarsAPI/blob/master/postman-collection.json)

## Exception Handling Middleware

A custom middleware has been added to handle errors. It will return a generic error message for users without exposing sensitive details when an exception occurs in the API.
