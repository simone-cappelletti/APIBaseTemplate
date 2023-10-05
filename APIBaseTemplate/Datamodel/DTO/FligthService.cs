using APIBaseTemplate.Datamodel.DbEntities;

namespace APIBaseTemplate.Datamodel.DTO
{
    public class FligthService
    {
        /// <summary>
        /// Flight service id
        /// </summary>
        public int? FligthServiceId { get; set; }
        /// <summary>
        /// Flight service type
        /// </summary>
        /// <example>
        /// Hand luggage
        /// Hold luggage
        /// Fligt insurance
        /// </example>
        public FlightServiceType PriceType { get; set; }
        /// <summary>
        /// Fligth service cost amount
        /// </summary>
        public double Amout { get; set; }
        /// <summary>
        /// Id of the currency of the fligth service
        /// </summary>
        public int CurrencyId { get; set; }
        /// <summary>
        /// Currency referenced by <see cref="CurrencyId"/>
        /// </summary>
        public Currency Currency { get; set; }
        /// <summary>
        /// Id of the fligth
        /// </summary>
        public int FligthId { get; set; }
        /// <summary>
        /// Fligth referenced by <see cref="FligthId"/>
        /// </summary>
        public Fligth Fligth { get; set; }
    }

    /// <summary>
    /// Types of flight services
    /// </summary>
    public enum FlightServiceType
    {
        Fligth = 0,
        HandLuggage = 1,
        HoldLuggage = 2,
        FligthInsurance = 3
    }
}
