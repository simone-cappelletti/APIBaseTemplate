namespace APIBaseTemplate.Datamodel.DTO
{
    public class Airline
    {
        /// <summary>
        /// Airline id
        /// </summary>
        public int? AirlineId { get; set; }
        /// <summary>
        /// Airline code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Airline name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Id of the airline's region
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// Region referenced by <see cref="RegionId"/>
        /// </summary>
        public Region Region { get; set; }
    }
}
