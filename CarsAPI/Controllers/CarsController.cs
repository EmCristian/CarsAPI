using CarsAPI.CQRS.Queries;
using CarsAPI.DTOs;
using CarsAPI.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICarSearchService _carSearchService;
        private readonly ILogger<CarsController> _logger;

        public CarsController(IMediator mediator, ICarSearchService carSearchService, ILogger<CarsController> logger)
        {
            _mediator = mediator;
            _carSearchService = carSearchService;
            _logger = logger;
        }

        /// <summary>
        /// Search for cars with various filters and get aggregated statistics
        /// </summary>
        /// <param name="request">Search parameters</param>
        /// <returns>Filtered car list with statistics</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/cars/search?brand=ford&minYear=2018&maxPrice=25000&sortBy=year_desc&page=1&pageSize=10
        ///
        /// Sort options:
        /// - price_asc: Sort by price, ascending
        /// - price_desc: Sort by price, descending
        /// - year_asc: Sort by year, ascending
        /// - year_desc: Sort by year, descending
        /// - mileage_asc: Sort by mileage, ascending
        /// - mileage_desc: Sort by mileage, descending
        [HttpGet("search")]
        public async Task<ActionResult<CarSearchResponse>> Search([FromQuery] CarSearchRequest request)
        {
            // Validate request
            if (request.Page <= 0)
            {
                return BadRequest("Page must be greater than 0");
            }

            if (request.PageSize <= 0 || request.PageSize > 100)
            {
                return BadRequest("PageSize must be between 1 and 100");
            }

            if (request.MinPrice.HasValue && request.MaxPrice.HasValue && request.MinPrice > request.MaxPrice)
            {
                return BadRequest("MinPrice cannot be greater than MaxPrice");
            }

            var response = await _mediator.Send(new SearchCarsQuery(request));
            return Ok(response);
        }
    }
}
