using APIBaseTemplate.Utils;

namespace APIBaseTemplate.Datamodel.Mappers
{
    public class AirlineMapper
    {
        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to a new db item
        /// </summary>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public DbEntities.Airline ToDb(DTO.Airline dtoEntity)
        {
            var dbEntity = new DbEntities.Airline();

            ToDb(dtoEntity, dbEntity);

            return dbEntity;
        }

        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to db item <paramref name="dbEntity"/>
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public void ToDb(DTO.Airline dtoEntity, DbEntities.Airline dbEntity)
        {
            Verify.IsNot.Null(dtoEntity);
            Verify.IsNot.Null(dbEntity);

            dbEntity.Code = dtoEntity.Code;
            dbEntity.Name = dtoEntity.Name;
            dbEntity.RegionId = dtoEntity.RegionId;
        }

        /// <summary>
        /// Map DB entity to DTO entity
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public DTO.Airline ToDto(DbEntities.Airline dbEntity)
        {
            if (dbEntity == null)
            {
                return null;
            }

            var dtoEntity = new DTO.Airline()
            {
                Code = dbEntity.Code,
                Name = dbEntity.Name,
                RegionId = dbEntity.RegionId,
                Region = Mappers.Region.ToDto(dbEntity.Region)
            };

            return dtoEntity;
        }

        /// <summary>
        /// Map DB entity list to DTO entity
        /// </summary>
        /// <param name="dbEntities"></param>
        /// <returns></returns>
        public IEnumerable<DTO.Airline> ToDto(IEnumerable<DbEntities.Airline> dbEntities)
        {
            foreach (var item in dbEntities)
            {
                yield return ToDto(item);
            }
        }
    }
}
