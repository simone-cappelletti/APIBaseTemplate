using APIBaseTemplate.Utils;

namespace APIBaseTemplate.Datamodel.Mappers
{
    public class AirportMapper
    {
        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to a new db item
        /// </summary>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public DbEntities.Airport ToDb(DTO.Airport dtoEntity)
        {
            var dbEntity = new DbEntities.Airport();

            ToDb(dtoEntity, dbEntity);

            return dbEntity;
        }

        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to db item <paramref name="dbEntity"/>
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public void ToDb(DTO.Airport dtoEntity, DbEntities.Airport dbEntity)
        {
            Verify.IsNot.Null(dtoEntity);
            Verify.IsNot.Null(dbEntity);

            dbEntity.Code = dtoEntity.Code;
            dbEntity.Name = dtoEntity.Name;
            dbEntity.CityId = dtoEntity.CityId;
        }

        /// <summary>
        /// Map DB entity to DTO entity
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public DTO.Airport ToDto(DbEntities.Airport dbEntity)
        {
            if (dbEntity == null)
            {
                return null;
            }

            var dtoEntity = new DTO.Airport()
            {
                Code = dbEntity.Code,
                Name = dbEntity.Name,
                CityId = dbEntity.CityId,
                City = Mappers.City.ToDto(dbEntity.City)
            };

            return dtoEntity;
        }

        /// <summary>
        /// Map DB entity list to DTO entity
        /// </summary>
        /// <param name="dbEntities"></param>
        /// <returns></returns>
        public IEnumerable<DTO.Airport> ToDto(IEnumerable<DbEntities.Airport> dbEntities)
        {
            foreach (var item in dbEntities)
            {
                yield return ToDto(item);
            }
        }
    }
}
