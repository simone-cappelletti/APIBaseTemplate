using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Repositories;

namespace APIBaseTemplate.Utils
{
    /// <summary>
    /// <see cref="Airport"/> business helper class
    /// </summary>
    public class AirportBusinessHelper
    {
        /// <summary>
        /// Sanitize and normalize <paramref name="airport"/>
        /// </summary>
        /// <param name="airport"></param>
        internal static void SanitizeNormalize(Airport airport)
        {
            if (airport == null)
            {
                return;
            }

            var sanitizationOpt = TextSanitizerHelper.GetSanitizationOptionsRemovePercentAndTrim();

            airport.Code = TextSanitizerHelper.SanitizeTextSimply(airport.Code, sanitizationOpt);
            airport.Name = TextSanitizerHelper.SanitizeTextSimply(airport.Name, sanitizationOpt);
        }

        /// <summary>
        /// Common validation for <see cref="Airport"/>
        /// </summary>
        /// <remarks>
        /// This validation is intended to be executed before storing or updating airport in db
        /// </remarks>
        /// <param name="airport"></param>
        /// <param name="insertMode"></param>
        internal static void AirportCommonValidation(
            Airport airport,
            bool insertMode,
            IAirportRepository _airportRepository)
        {
            // Airport must be not null
            Verify.IsNot.Null(airport, nameof(airport));

            // Name and Code must be specified
            Verify.IsNot.NullOrEmpty(airport.Code, nameof(airport.Code));
            Verify.IsNot.NullOrEmpty(airport.Name, nameof(airport.Name));

            if (insertMode)
            {
                // Creating

                // Code must be unique
                var duplicate = _airportRepository.SingleOrDefault(
                    x => x.Code == airport.Code);

                if (duplicate != null)
                {
                    throw new AirportDuplicateException(nameof(airport.Code), airport.Code);
                }
            }
            else
            {
                // Updating

                // Airport must exists
                Verify.IsNot.Null(airport.AirportId, nameof(airport.AirportId));
                Verify.Is.Positive(airport.AirportId.Value, nameof(airport.AirportId));

                _ = _airportRepository.Single(
                    x => x.AirportId == airport.AirportId.Value,
                    ioEx => throw new AirportSingleException(airport.AirportId.Value));
            }
        }
    }
}
