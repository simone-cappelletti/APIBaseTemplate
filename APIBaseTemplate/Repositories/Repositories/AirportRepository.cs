﻿using APIBaseTemplate.Common;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Utils;
using Microsoft.EntityFrameworkCore;
using Airport = APIBaseTemplate.Datamodel.DbEntities.Airport;

namespace APIBaseTemplate.Repositories
{
    public interface IAirportRepository : IRepository<Airport>
    {
        /// <summary>
        /// Perform a search of <see cref="Airport"/> items using <paramref name="filter"/>
        /// </summary>
        /// <param name="filter"></param>
        IQueryable<Airport> Get(SearchAirportRequest filter);
    }

    public class AirportRepository : BaseRepository<Airport>, IAirportRepository
    {
        private readonly ILogger<AirportRepository> _logger;

        public AirportRepository(ILogger<AirportRepository> logger)
        {
            _logger = logger;
        }

        public IQueryable<Airport> Get(SearchAirportRequest request)
        {
            Verify.IsNot.Null(request, nameof(request));

            IQueryable<Airport> query = this.DbContext.Set<Airport>();

            query = ApplyFilters(query, request.Filters);
            query = AddIncludes(query, request.Options);

            return query;
        }

        private IQueryable<Airport> ApplyFilters(IQueryable<Airport> query, SearchAirportFilters filters)
        {
            if (filters == null)
                return query;

            var sanitizeOptions = EnmSimpleTextFilterSanitize.RemovePercent | EnmSimpleTextFilterSanitize.Trim | EnmSimpleTextFilterSanitize.ToUpper;

            // AirportId
            if (true == filters.AirportId.HasValue)
            {
                query = query.Where(x => x.AirportId == filters.AirportId);
            }

            // Code
            if (filters.Code != null)
            {
                filters.Code.Validate();
                string text = filters.Code.GetSimpleSanitizedValue(sanitizeOptions);
                query = query.WhereTextFilter(filters.Code.Operator,
                    isNull: null,
                    equalTo: x => x.Code.ToUpper() == text,
                    like: x => EF.Functions.Like(x.Code.ToUpper(), $"%{text}%"),
                    startsWith: x => EF.Functions.Like(x.Code.ToUpper(), $"{text}%"),
                    endsWith: x => EF.Functions.Like(x.Code.ToUpper(), $"%{text}"),
                    lessThan: x => x.Code.ToUpper().CompareTo(text) < 0,
                    greaterThan: x => x.Code.ToUpper().CompareTo(text) > 0,
                    inValues: null // this field doesn't support this operator
                    );
            }

            // Name
            if (filters.Name != null)
            {
                filters.Name.Validate();
                string text = filters.Name.GetSimpleSanitizedValue(sanitizeOptions);
                query = query.WhereTextFilter(filters.Name.Operator,
                    isNull: null,
                    equalTo: x => x.Name.ToUpper() == text,
                    like: x => EF.Functions.Like(x.Name.ToUpper(), $"%{text}%"),
                    startsWith: x => EF.Functions.Like(x.Name.ToUpper(), $"{text}%"),
                    endsWith: x => EF.Functions.Like(x.Name.ToUpper(), $"%{text}"),
                    lessThan: x => x.Name.ToUpper().CompareTo(text) < 0,
                    greaterThan: x => x.Name.ToUpper().CompareTo(text) > 0,
                    inValues: null // this field doesn't support this operator
                    );
            }

            // CityId
            if (false == filters.CityId.HasValue)
            {
                query = query.Where(x => x.CityId == filters.CityId);
            }

            return query;
        }

        private IQueryable<Airport> AddIncludes(IQueryable<Airport> query, SearchAirportOptions options)
        {
            if (options == null)
                return query;

            query = query.Include(x => x.City);

            return query;
        }
    }
}
