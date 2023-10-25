using APIBaseTemplate.Common;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIBaseTemplate.Controllers
{
    /// <summary>
    /// WebAPI related to <see cref="Region"/> items
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : IController<Region>
    {
        private readonly ILogger<RegionController> _logger;
        private readonly IRegionBusiness _business;

        /// <summary>
        /// Initialize <see cref="RegionController"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="RegionBusiness"></param>
        public RegionController(
            ILogger<RegionController> logger,
            IRegionBusiness RegionBusiness)
        {
            _logger = logger;
            _business = RegionBusiness;
        }

        /// <inheritdoc/>
        [HttpPost]
        [Route("search")]
        public ResponseOf<PagedResult<Region>> Search(SearchRegionRequest request)
        {
            _logger.LogTrace($"{nameof(Search)}");

            var response = new ResponseOf<PagedResult<Region>>
            {
                Value = _business.Get(request)
            };

            return response;
        }

        /// <inheritdoc/>
        [Route("get")]
        [HttpPost]
        public ResponseOf<Region> GetById([FromBody] RequestOf<EntityNumberIdParam> request)
        {
            _logger.LogTrace($"{nameof(GetById)}");

            var response = new ResponseOf<Region>
            {
                Value = _business.GetById(request.Value.Id)
            };

            return response;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        [Route("save")]
        [HttpPost]
        public ResponseOf<Region> Save([FromBody] RequestOf<Region> request)
        {
            _logger.LogTrace($"{nameof(Save)}");

            var response = new ResponseOf<Region>
            {
                Value = _business.Save(request.Value)
            };

            return response;
        }

        /// <inheritdoc/>
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
