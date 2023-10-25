namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Container class for web request parameters of specified type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequestOf<T> : Request
    {
        /// <summary>
        /// Value
        /// </summary>
        public T? Value { get; set; }
    }
}
