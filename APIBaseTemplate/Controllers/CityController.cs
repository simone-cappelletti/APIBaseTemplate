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
    public class CityController : IController<City>
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
        [Route("search")]
        [HttpGet]
        public ResponseOf<PagedResult<City>> Search(SearchCityRequest request)
        {
            _logger.LogTrace($"{nameof(Search)}");

            var response = new ResponseOf<PagedResult<City>>
            {
                Value = _business.Get(request)
            };

            return response;
        }

        /// <inheritdoc/>
        [Route("get")]
        [HttpGet]
        public ResponseOf<City> GetById([FromBody] RequestOf<EntityNumberIdParam> request)
        {
            _logger.LogTrace($"{nameof(GetById)}");

            var response = new ResponseOf<City>
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
        public ResponseOf<City> Save([FromBody] RequestOf<City> request)
        {
            _logger.LogTrace($"{nameof(Save)}");

            var response = new ResponseOf<City>
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
