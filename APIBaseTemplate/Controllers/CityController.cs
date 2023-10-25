using APIBaseTemplate.Common;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIBaseTemplate.Controllers
{
    /// <summary>
    /// WebAPI related to <see cref="City"/> items
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CityController
    {
        private readonly ILogger<CityController> _logger;
        private readonly ICityBusiness _business;

        /// <summary>
        /// Initialize <see cref="CityController"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="CityBusiness"></param>
        public CityController(
            ILogger<CityController> logger,
            ICityBusiness CityBusiness)
        {
            _logger = logger;
            _business = CityBusiness;
        }

        /// <summary>
        /// Search and returns a page of <see cref="City"/> items.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// Page of <see cref="City"/> items (in <see cref="PagedResult{T}.Items"/>) that meet the criteria as in <paramref name="request"/>.
        /// Returned object contains total number of records <see cref="PagedResult{T}.TotalRecordsWithoutPagination"/>,
        /// total pages <see cref="PagedResult{T}.TotalPages"/>,
        /// current sorting options <see cref="PagedResult{T}.Sortings"/>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [Route("search")]
        public ResponseOf<PagedResult<City>> Search(SearchCityRequest request)
        {
            _logger.LogTrace($"{nameof(Search)}");

            var response = new ResponseOf<PagedResult<City>>
            {
                Value = _business.Get(request)
            };

            return response;
        }

        /// <summary>
        /// Return specific <see cref="City"/> by id
        /// </summary>
        /// <param name="request">CityId</param>
        /// <returns>City with details</returns>
        [Route("get")]
        [HttpPost]
        public ResponseOf<City> GetById([FromBody] RequestOf<EntityNumberIdParam> request)
        {
            _logger.LogTrace($"{nameof(GetById)}");

            var response = new ResponseOf<City>
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
        /// Save a <see cref="City"/>
        /// </summary>
        /// <param name="request">item to update or insert</param>
        /// <remarks>
        /// If <see cref="City.CityId"/> has a value an update will be performed.
        /// If <see cref="City.CityId"/> doesn't have value an insert will be performed.
        /// </remarks>
        /// <returns>The updated/inserted <see cref="City"/> </returns>
        [Route("save")]
        [HttpPost]
        public ResponseOf<City> Save([FromBody] RequestOf<City> request)
        {
            _logger.LogTrace($"{nameof(Save)}");

            var response = new ResponseOf<City>
            {
                Value = _business.Save(request.Value)
            };

            return response;
        }

        /// <summary>
        /// Delete an <see cref="City"/>
        /// </summary>
        /// <param name="req">The City that must be deleted</param>
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
