namespace APIBaseTemplate.Datamodel.Mappers
{
    public static class Mappers
    {
        /// <summary>
        /// Mapper for <see cref="DTO.Region"/> model from <see cref="DbEntities.Region"/>)
        /// </summary>
        public readonly static RegionMapper Region = new();

        /// <summary>
        /// Mapper for <see cref="DTO.City"/> model from <see cref="DbEntities.City"/>)
        /// </summary>
        public readonly static CityMapper City = new();

        /// <summary>
        /// Mapper for <see cref="DTO.Airline"/> model from <see cref="DbEntities.Airline"/>)
        /// </summary>
        public readonly static AirlineMapper Airline = new();

        /// <summary>
        /// Mapper for <see cref="DTO.Airport"/> model from <see cref="DbEntities.Airport"/>)
        /// </summary>
        public readonly static AirportMapper Airport = new();

        /// <summary>
        /// Mapper for <see cref="DTO.Fligth"/> model from <see cref="DbEntities.Fligth"/>)
        /// </summary>
        public readonly static FligthMapper Fligth = new();

        /// <summary>
        /// Mapper for <see cref="DTO.FligthService"/> model from <see cref="DbEntities.FligthService"/>)
        /// </summary>
        public readonly static FligthServiceMapper FligthService = new();

        /// <summary>
        /// Mapper for <see cref="DTO.Currency"/> model from <see cref="DbEntities.Currency"/>)
        /// </summary>
        public readonly static CurrencyMapper Currency = new();
    }
}
