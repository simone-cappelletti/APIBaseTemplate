namespace APIBaseTemplate.Datamodel.Db
{
    public class Airport
    {
        /// <summary>
        /// Airport id
        /// </summary>
        public int AirportId { get; set; }
        /// <summary>
        /// Airport code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Airport name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Id of the airport's city
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// City referenced by <see cref="CityId"/>
        /// </summary>
        public City City { get; set; }
    }
}
