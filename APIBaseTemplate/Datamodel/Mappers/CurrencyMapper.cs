using APIBaseTemplate.Utils;

namespace APIBaseTemplate.Datamodel.Mappers
{
    public class CurrencyMapper
    {
        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to a new db item
        /// </summary>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public DbEntities.Currency ToDb(DTO.Currency dtoEntity)
        {
            var dbEntity = new DbEntities.Currency();

            ToDb(dtoEntity, dbEntity);

            return dbEntity;
        }

        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to db item <paramref name="dbEntity"/>
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public void ToDb(DTO.Currency dtoEntity, DbEntities.Currency dbEntity)
        {
            Verify.IsNot.Null(dtoEntity);
            Verify.IsNot.Null(dbEntity);

            dbEntity.Code = dtoEntity.Code;
            dbEntity.Name = dtoEntity.Name;
        }

        /// <summary>
        /// Map DB entity to DTO entity
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public DTO.Currency ToDto(DbEntities.Currency dbEntity)
        {
            if (dbEntity == null)
            {
                return null;
            }

            var dtoEntity = new DTO.Currency()
            {
                CurrencyId = dbEntity.CurrencyId,
                Code = dbEntity.Code,
                Name = dbEntity.Name
            };

            return dtoEntity;
        }

        /// <summary>
        /// Map DB entity list to DTO entity
        /// </summary>
        /// <param name="dbEntities"></param>
        /// <returns></returns>
        public IEnumerable<DTO.Currency> ToDto(IEnumerable<DbEntities.Currency> dbEntities)
        {
            foreach (var item in dbEntities)
            {
                yield return ToDto(item);
            }
        }
    }
}
