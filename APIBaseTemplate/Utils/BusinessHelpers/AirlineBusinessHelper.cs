using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Repositories;

namespace APIBaseTemplate.Utils
{
    /// <summary>
    /// <see cref="Airline"/> business helper class
    /// </summary>
    public class AirlineBusinessHelper
    {
        /// <summary>
        /// Sanitize and normalize <paramref name="airline"/>
        /// </summary>
        /// <param name="airline"></param>
        internal static void SanitizeNormalize(Airline airline)
        {
            if (airline == null)
            {
                return;
            }

            var sanitizationOpt = TextSanitizerHelper.GetSanitizationOptionsRemovePercentAndTrim();

            airline.Code = TextSanitizerHelper.SanitizeTextSimply(airline.Code, sanitizationOpt);
            airline.Name = TextSanitizerHelper.SanitizeTextSimply(airline.Name, sanitizationOpt);
        }

        /// <summary>
        /// Common validation for <see cref="Airline"/>
        /// </summary>
        /// <remarks>
        /// This validation is intended to be executed before storing or updating airline in db
        /// </remarks>
        /// <param name="airline"></param>
        /// <param name="insertMode"></param>
        internal static void AirlineCommonValidation(
            Airline airline,
            bool insertMode,
            IAirlineRepository _airlineRepository)
        {
            // Airline must be not null
            Verify.IsNot.Null(airline, nameof(airline));

            // Name and Code must be specified
            Verify.IsNot.NullOrEmpty(airline.Code, nameof(airline.Code));
            Verify.IsNot.NullOrEmpty(airline.Name, nameof(airline.Name));

            if (insertMode)
            {
                // Creating

                // Code must be unique
                var duplicate = _airlineRepository.SingleOrDefault(
                    x => x.Code == airline.Code);

                if (duplicate != null)
                {
                    throw new AirlineDuplicateException(nameof(airline.Code), airline.Code);
                }
            }
            else
            {
                // Updating

                // Airline must exists
                Verify.IsNot.Null(airline.AirlineId, nameof(airline.AirlineId));
                Verify.Is.Positive(airline.AirlineId.Value, nameof(airline.AirlineId));

                _ = _airlineRepository.Single(
                    x => x.AirlineId == airline.AirlineId.Value,
                    ioEx => throw new AirlineSingleException(airline.AirlineId.Value));
            }
        }
    }
}
