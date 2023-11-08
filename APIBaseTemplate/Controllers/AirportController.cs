using APIBaseTemplate.Common;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIBaseTemplate.Controllers
{
    /// <summary>
    /// WebAPI related to <see cref="Airport"/> items
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : IController<Airport>
    {
        private readonly ILogger<AirportController> _logger;
        private readonly IAirportBusiness _business;

        /// <summary>
        /// Initialize <see cref="AirportController"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="airportBusiness"></param>
        public AirportController(
            ILogger<AirportController> logger,
            IAirportBusiness airportBusiness)
        {
            _logger = logger;
            _business = airportBusiness;
        }

        /// <summary>
        /// Search and returns a page of <see cref="Airport"/> items.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// Page of <see cref="Airport"/> items (in <see cref="PagedResult{T}.Items"/>) that meet the criteria as in <paramref name="request"/>.
        /// Returned object contains total number of records <see cref="PagedResult{T}.TotalRecordsWithoutPagination"/>,
        /// total pages <see cref="PagedResult{T}.TotalPages"/>,
        /// current sorting options <see cref="PagedResult{T}.Sortings"/>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        [Route("search")]
        [HttpGet]
        public ResponseOf<PagedResult<Airport>> Search(SearchAirportRequest request)
        {
            _logger.LogTrace($"{nameof(Search)}");

            var response = new ResponseOf<PagedResult<Airport>>
            {
                Value = _business.Get(request)
            };

            return response;
        }

        /// <inheritdoc/>
        [Route("get")]
        [HttpGet]
        public ResponseOf<Airport> GetById([FromBody] RequestOf<EntityNumberIdParam> request)
        {
            _logger.LogTrace($"{nameof(GetById)}");

            var response = new ResponseOf<Airport>
            {
                Value = _business.GetById(request.Value.Id)
            };

            return response;
        }

        /// <inheritdoc/>
        [Route("sortingParameters")]
        [HttpGet]
        public ResponseOf<List<string>> GetSortingParameters()
        {
            _logger.LogTrace($"{nameof(GetSortingParameters)}");

            var response = new ResponseOf<List<string>>
            {
                Value = _business.GetSortingParameters().ToList()
            };

            return response;
        }

        /// <inheritdoc/>
        [Route("save")]
        [HttpPost]
        public ResponseOf<Airport> Save([FromBody] RequestOf<Airport> request)
        {
            _logger.LogTrace($"{nameof(Save)}");

            var response = new ResponseOf<Airport>
            {
                Value = _business.Save(request.Value)
            };

            return response;
        }

        /// <inheritdoc/>
        [Route("delete")]
        [HttpDelete]
        public Response Delete([FromBody] RequestOf<EntityNumberIdParam> req)
        {
            _logger.LogTrace($"{nameof(Delete)}");

            _business.Delete(req.Value.Id);

            return new Response();
        }
    }
}
