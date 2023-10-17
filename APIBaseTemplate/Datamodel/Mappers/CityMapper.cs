using APIBaseTemplate.Utils;

namespace APIBaseTemplate.Datamodel.Mappers
{
    public class CityMapper
    {
        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to a new db item
        /// </summary>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public DbEntities.City ToDb(DTO.City dtoEntity)
        {
            var dbEntity = new DbEntities.City();

            ToDb(dtoEntity, dbEntity);

            return dbEntity;
        }

        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to db item <paramref name="dbEntity"/>
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public void ToDb(DTO.City dtoEntity, DbEntities.City dbEntity)
        {
            Verify.IsNot.Null(dtoEntity);
            Verify.IsNot.Null(dbEntity);

            dbEntity.Name = dtoEntity.Name;
            dbEntity.RegionId = dtoEntity.RegionId;
        }

        /// <summary>
        /// Map DB entity to DTO entity
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public DTO.City ToDto(DbEntities.City dbEntity)
        {
            if (dbEntity == null)
            {
                return null;
            }

            var dtoEntity = new DTO.City()
            {
                CityId = dbEntity.CityId,
                Name = dbEntity.Name,
                Region = Mappers.Region.ToDto(dbEntity.Region),
                RegionId = dbEntity.RegionId
            };

            return dtoEntity;
        }

        /// <summary>
        /// Map DB entity list to DTO entity
        /// </summary>
        /// <param name="dbEntities"></param>
        /// <returns></returns>
        public IEnumerable<DTO.City> ToDto(IEnumerable<DbEntities.City> dbEntities)
        {
            foreach (var item in dbEntities)
            {
                yield return ToDto(item);
            }
        }
    }
}
