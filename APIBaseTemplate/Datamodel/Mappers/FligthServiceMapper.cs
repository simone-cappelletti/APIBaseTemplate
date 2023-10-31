using APIBaseTemplate.Utils;

namespace APIBaseTemplate.Datamodel.Mappers
{
    public class FligthServiceMapper
    {
        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to a new db item
        /// </summary>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public DbEntities.FligthService ToDb(DTO.FligthService dtoEntity)
        {
            var dbEntity = new DbEntities.FligthService();

            ToDb(dtoEntity, dbEntity);

            return dbEntity;
        }

        /// <summary>
        /// Copy data from dto item <paramref name="dtoEntity"/> to db item <paramref name="dbEntity"/>
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="dtoEntity"></param>
        /// <returns></returns>
        public void ToDb(DTO.FligthService dtoEntity, DbEntities.FligthService dbEntity)
        {
            Verify.IsNot.Null(dtoEntity);
            Verify.IsNot.Null(dbEntity);

            dbEntity.FlightServiceType = ToDb(dtoEntity.FlightServiceType);
            dbEntity.Amount = dtoEntity.Amount;
            dbEntity.CurrencyId = dtoEntity.CurrencyId;
            dbEntity.FligthId = dtoEntity.FligthId;
        }

        /// <summary>
        /// Map DB entity to DTO entity
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public DTO.FligthService ToDto(DbEntities.FligthService dbEntity)
        {
            if (dbEntity == null)
            {
                return null;
            }

            var dtoEntity = new DTO.FligthService()
            {
                FligthServiceId = dbEntity.FligthServiceId,
                FlightServiceType = ToDto(dbEntity.FlightServiceType),
                Amount = dbEntity.Amount,
                CurrencyId = dbEntity.CurrencyId,
                Currency = Mappers.Currency.ToDto(dbEntity.Currency),
                FligthId = dbEntity.FligthId,
                Fligth = Mappers.Fligth.ToDto(dbEntity.Fligth)
            };

            return dtoEntity;
        }

        /// <summary>
        /// Map DB entity list to DTO entity
        /// </summary>
        /// <param name="dbEntities"></param>
        /// <returns></returns>
        public IEnumerable<DTO.FligthService> ToDto(IEnumerable<DbEntities.FligthService> dbEntities)
        {
            foreach (var item in dbEntities)
            {
                yield return ToDto(item);
            }
        }

        /// <summary>
        /// Map DTO entity list to Db entity
        /// </summary>
        /// <param name="dtoEntities"></param>
        /// <returns></returns>
        public IEnumerable<DbEntities.FligthService> ToDb(IEnumerable<DTO.FligthService> dtoEntities)
        {
            foreach (var item in dtoEntities)
            {
                yield return ToDb(item);
            }
        }

        /// <summary>
        /// Maps DB enum <see cref="DbEntities.FlightServiceType"/> into DTO enum <see cref="DTO.FlightServiceType"/>
        /// </summary>
        /// <param name="dbEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DTO.FlightServiceType ToDto(DbEntities.FlightServiceType dbEnum)
        {
            return dbEnum switch
            {
                DbEntities.FlightServiceType.Fligth => DTO.FlightServiceType.Fligth,
                DbEntities.FlightServiceType.HandLuggage => DTO.FlightServiceType.HandLuggage,
                DbEntities.FlightServiceType.HoldLuggage => DTO.FlightServiceType.HoldLuggage,
                DbEntities.FlightServiceType.FligthInsurance => DTO.FlightServiceType.FligthInsurance,
                _ => throw new ArgumentOutOfRangeException(nameof(dbEnum), $"Unsupported value >{dbEnum}<"),
            };
        }

        /// <summary>
        /// Maps DTO enum <see cref="DTO.FlightServiceType"/> into DB enum <see cref="DbEntities.FlightServiceType"/>
        /// </summary>
        /// <param name="dtoEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DbEntities.FlightServiceType ToDb(DTO.FlightServiceType dtoEnum)
        {
            return dtoEnum switch
            {
                DTO.FlightServiceType.Fligth => DbEntities.FlightServiceType.Fligth,
                DTO.FlightServiceType.HandLuggage => DbEntities.FlightServiceType.HandLuggage,
                DTO.FlightServiceType.HoldLuggage => DbEntities.FlightServiceType.HoldLuggage,
                DTO.FlightServiceType.FligthInsurance => DbEntities.FlightServiceType.FligthInsurance,
                _ => throw new ArgumentOutOfRangeException(nameof(dtoEnum), $"Unsupported value >{dtoEnum}<"),
            };
        }

        /// <summary>
        /// Map DB entity list to DTO entity
        /// </summary>
        /// <param name="dbEntities"></param>
        /// <returns></returns>
        public IEnumerable<DTO.FlightServiceType> ToDto(IEnumerable<DbEntities.FlightServiceType> dbEntities)
        {
            foreach (var item in dbEntities)
            {
                yield return ToDto(item);
            }
        }

        /// <summary>
        /// Map DTO entity list to Db entity
        /// </summary>
        /// <param name="dtoEntities"></param>
        /// <returns></returns>
        public IEnumerable<DbEntities.FlightServiceType> ToDb(IEnumerable<DTO.FlightServiceType> dtoEntities)
        {
            foreach (var item in dtoEntities)
            {
                yield return ToDb(item);
            }
        }
    }
}
