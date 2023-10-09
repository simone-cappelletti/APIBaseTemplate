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
    }
}
