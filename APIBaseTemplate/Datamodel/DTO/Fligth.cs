namespace APIBaseTemplate.Datamodel.DTO
{
    public class Fligth
    {
        /// <summary>
        /// Fligth id
        /// </summary>
        public int? FligthId { get; set; }
        /// <summary>
        /// Fligth code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Id of the fligth's airline
        /// </summary>
        public int AirlineId { get; set; }
        /// <summary>
        /// Airline referenced by <see cref="AirlineId"/>
        /// </summary>
        public Airline Airline { get; set; }
        /// <summary>
        /// Id of the departure airport
        /// </summary>
        public int DepartureAirportId { get; set; }
        /// <summary>
        /// Departure airport referenced by <see cref="DepartureAirportId"/>
        /// </summary>
        public Airport DepartureAirport { get; set; }
        /// <summary>
        /// Id of the arrival airport
        /// </summary>
        public int ArrivalAirportId { get; set; }
        /// <summary>
        /// Arrival airport referenced by <see cref="ArrivalAirportId"/>
        /// </summary>
        public Airport ArrivalAirport { get; set; }
        /// <summary>
        /// Fligth departure time
        /// </summary>
        public DateTime DepartureTime { get; set; }
        /// <summary>
        /// Fligth arrival time
        /// </summary>
        public DateTime ArrivalTime { get; set; }
        /// <summary>
        /// Fligth terminal
        /// </summary>
        public string Terminal { get; set; }
        /// <summary>
        /// Fligth gate
        /// </summary>
        public string Gate { get; set; }
        /// <summary>
        /// Support to logical deletion
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Fligth Services
        /// </summary>
        public IEnumerable<FligthService> FligthServices { get; set; }
    }
}
