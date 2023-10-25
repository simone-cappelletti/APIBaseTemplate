namespace APIBaseTemplate.Common.Exceptions
{
    public class CurrencySingleException : CurrencyException
    {
        public CurrencySingleException(int currencyId) :
            base($"Currency single fault", CurrencyErrorCodes.SINGLE_ERROR, (nameof(currencyId), currencyId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }

        public CurrencySingleException(int currencyId, Exception inner) :
            base($"Currency single fault", inner, CurrencyErrorCodes.SINGLE_ERROR, (nameof(currencyId), currencyId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }
    }
}
