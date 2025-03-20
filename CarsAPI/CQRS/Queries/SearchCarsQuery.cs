using CarsAPI.DTOs;
using MediatR;

namespace CarsAPI.CQRS.Queries
{
    public class SearchCarsQuery : IRequest<CarSearchResponse>
    {
        public CarSearchRequest Request { get; }
        public SearchCarsQuery(CarSearchRequest request) => Request = request;
    }
}
