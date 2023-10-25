using APIBaseTemplate.Common;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIBaseTemplate.Controllers
{
    /// <summary>
    /// WebAPI related to <see cref="Airline"/> items
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AirlineController
    {
        private readonly ILogger<AirlineController> _logger;
        private readonly IAirlineBusiness _business;

        /// <summary>
        /// Initialize <see cref="AirlineController"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="airlineBusiness"></param>
        public AirlineController(
            ILogger<AirlineController> logger,
            IAirlineBusiness airlineBusiness)
        {
            _logger = logger;
            _business = airlineBusiness;
        }

        /// <summary>
        /// Search and returns a page of <see cref="Airline"/> items.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// Page of <see cref="Airline"/> items (in <see cref="PagedResult{T}.Items"/>) that meet the criteria as in <paramref name="request"/>.
        /// Returned object contains total number of records <see cref="PagedResult{T}.TotalRecordsWithoutPagination"/>,
        /// total pages <see cref="PagedResult{T}.TotalPages"/>,
        /// current sorting options <see cref="PagedResult{T}.Sortings"/>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [Route("search")]
        public ResponseOf<PagedResult<Airline>> Search(SearchAirlineRequest request)
        {
            _logger.LogTrace($"{nameof(Search)}");

            var response = new ResponseOf<PagedResult<Airline>>
            {
                Value = _business.Get(request)
            };

            return response;
        }

        /// <summary>
        /// Return specific <see cref="Airline"/> by id
        /// </summary>
        /// <param name="request">AirlineId</param>
        /// <returns>Airline with details</returns>
        [Route("get")]
        [HttpPost]
        public ResponseOf<Airline> GetById([FromBody] RequestOf<EntityNumberIdParam> request)
        {
            _logger.LogTrace($"{nameof(GetById)}");

            var response = new ResponseOf<Airline>
            {
                Value = _business.GetById(request.Value.Id)
            };

            return response;
        }

        /// <summary>
        /// Returns available sorting parameters
        /// </summary>
        /// <returns></returns>
        [Route("sortingParameters")]
        [HttpPost]
        public ResponseOf<List<string>> GetSortingParameters()
        {
            _logger.LogTrace($"{nameof(GetSortingParameters)}");

            var response = new ResponseOf<List<string>>
            {
                Value = _business.GetSortingParameters().ToList()
            };

            return response;
        }

        /// <summary>
        /// Save a <see cref="Airline"/>
        /// </summary>
        /// <param name="request">item to update or insert</param>
        /// <remarks>
        /// If <see cref="Airline.AirlineId"/> has a value an update will be performed.
        /// If <see cref="Airline.AirlineId"/> doesn't have value an insert will be performed.
        /// </remarks>
        /// <returns>The updated/inserted <see cref="Airline"/> </returns>
        [Route("save")]
        [HttpPost]
        public ResponseOf<Airline> Save([FromBody] RequestOf<Airline> request)
        {
            _logger.LogTrace($"{nameof(Save)}");

            var response = new ResponseOf<Airline>
            {
                Value = _business.Save(request.Value)
            };

            return response;
        }

        /// <summary>
        /// Delete an <see cref="Airline"/>
        /// </summary>
        /// <param name="req">The Airline that must be deleted</param>
        /// <returns>Empty response if ok</returns>
        [Route("delete")]
        [HttpPost]
        public Response Delete([FromBody] RequestOf<EntityNumberIdParam> req)
        {
            _logger.LogTrace($"{nameof(Delete)}");

            _business.Delete(req.Value.Id);

            return new Response();
        }
    }
}
