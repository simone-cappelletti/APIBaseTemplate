namespace APIBaseTemplate.Datamodel.Db
{
    public class City
    {
        /// <summary>
        /// City id
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// City name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// City region
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// Region references by <see cref="RegionId"/>
        /// </summary>
        public Region Region { get; set; }
    }
}
