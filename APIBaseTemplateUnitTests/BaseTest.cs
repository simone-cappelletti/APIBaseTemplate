namespace APIBaseTemplateUnitTests
{
    public class BaseTest : IDisposable
    {
        public MockData MockData { get; private set; }

        public BaseTest()
        {
            MockData = new MockData();
        }

        public virtual void Dispose()
        {
            MockData = null;
        }

        /// <summary>
        /// Clone an object by serializing and then deserializing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        protected static T Clone<T>(T source)
        {
            var serialized = System.Text.Json.JsonSerializer.Serialize<T>(source);
            return System.Text.Json.JsonSerializer.Deserialize<T>(serialized);
        }
    }
}
