namespace APIBaseTemplate.Datamodel.DTO
{
    public class Currency
    {
        /// <summary>
        /// Currency id
        /// </summary>
        public int? CurrencyId { get; set; }
        /// <summary>
        /// Currency code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Currency name
        /// </summary>
        public string Name { get; set; }
    }
}
