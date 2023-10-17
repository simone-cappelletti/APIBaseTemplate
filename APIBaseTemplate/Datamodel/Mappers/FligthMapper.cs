using APIBaseTemplate.Utils;

namespace APIBaseTemplate.Datamodel.Mappers
{
    public class FligthMapper
    {
        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to a new db item
        /// </summary>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public DbEntities.Fligth ToDb(DTO.Fligth dtoEntity)
        {
            var dbEntity = new DbEntities.Fligth();

            ToDb(dtoEntity, dbEntity);

            return dbEntity;
        }

        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to db item <paramref name="dbEntity"/>
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public void ToDb(DTO.Fligth dtoEntity, DbEntities.Fligth dbEntity)
        {
            Verify.IsNot.Null(dtoEntity);
            Verify.IsNot.Null(dbEntity);

            dbEntity.Code = dtoEntity.Code;
            dbEntity.AirlineId = dtoEntity.AirlineId;
            dbEntity.DepartureAirportId = dtoEntity.DepartureAirportId;
            dbEntity.ArrivalAirportId = dtoEntity.ArrivalAirportId;
            dbEntity.DepartureTime = dtoEntity.DepartureTime;
            dbEntity.ArrivalTime = dtoEntity.ArrivalTime;
            dbEntity.Terminal = dtoEntity.Terminal;
            dbEntity.Gate = dtoEntity.Gate;
            dbEntity.IsDeleted = dtoEntity.IsDeleted;
            dbEntity.FligthServices = Mappers.FligthService.ToDb(dtoEntity.FligthServices);
        }

        /// <summary>
        /// Map DB entity to DTO entity
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public DTO.Fligth ToDto(DbEntities.Fligth dbEntity)
        {
            if (dbEntity == null)
            {
                return null;
            }

            var dtoEntity = new DTO.Fligth()
            {
                Code = dbEntity.Code,
                AirlineId = dbEntity.AirlineId,
                Airline = Mappers.Airline.ToDto(dbEntity.Airline),
                DepartureAirportId = dbEntity.DepartureAirportId,
                DepartureAirport = Mappers.Airport.ToDto(dbEntity.DepartureAirport),
                ArrivalAirportId = dbEntity.ArrivalAirportId,
                ArrivalAirport = Mappers.Airport.ToDto(dbEntity.ArrivalAirport),
                DepartureTime = dbEntity.DepartureTime,
                ArrivalTime = dbEntity.ArrivalTime,
                Terminal = dbEntity.Terminal,
                Gate = dbEntity.Gate,
                IsDeleted = dbEntity.IsDeleted,
                FligthServices = Mappers.FligthService.ToDto(dbEntity.FligthServices)
            };

            return dtoEntity;
        }

        /// <summary>
        /// Map DB entity list to DTO entity
        /// </summary>
        /// <param name="dbEntities"></param>
        /// <returns></returns>
        public IEnumerable<DTO.Fligth> ToDto(IEnumerable<DbEntities.Fligth> dbEntities)
        {
            foreach (var item in dbEntities)
            {
                yield return ToDto(item);
            }
        }
    }
}
