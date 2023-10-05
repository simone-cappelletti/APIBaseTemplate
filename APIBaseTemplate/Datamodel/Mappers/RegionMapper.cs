using APIBaseTemplate.Utils;

namespace APIBaseTemplate.Datamodel.Mappers
{
    public class RegionMapper
    {
        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to a new db item
        /// </summary>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public DbEntities.Region ToDb(DTO.Region dtoEntity)
        {
            var dbEntity = new DbEntities.Region();

            ToDb(dtoEntity, dbEntity);

            return dbEntity;
        }

        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to db item <paramref name="dbEntity"/>
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public void ToDb(DTO.Region dtoEntity, DbEntities.Region dbEntity)
        {
            Verify.IsNot.Null(dtoEntity);
            Verify.IsNot.Null(dbEntity);

            dbEntity.Name = dtoEntity.Name;
        }
        /// <summary>
        /// Map DB entity to DTO entity
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public DTO.Region ToDto(DbEntities.Region dbEntity)
        {
            if (dbEntity == null)
            {
                return null;
            }

            var dtoEntity = new DTO.Region()
            {
                RegionId = dbEntity.RegionId,
                Name = dbEntity.Name
            };

            return dtoEntity;
        }

        public IEnumerable<DTO.Region> ToDto(ICollection<DbEntities.Region> dbEntities)
        {
            foreach (var item in dbEntities)
            {
                yield return ToDto(item);
            }
        }
    }
}
