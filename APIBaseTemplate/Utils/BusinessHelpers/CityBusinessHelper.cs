using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Repositories.Repositories;

namespace APIBaseTemplate.Utils
{
    /// <summary>
    /// <see cref="City"/> business helper class
    /// </summary>
    public class CityBusinessHelper
    {
        /// <summary>
        /// Sanitize and normalize <paramref name="city"/>
        /// </summary>
        /// <param name="city"></param>
        internal static void SanitizeNormalize(City city)
        {
            if (city == null)
            {
                return;
            }

            var sanitizationOpt = TextSanitizerHelper.GetSanitizationOptionsRemovePercentAndTrim();

            city.Name = TextSanitizerHelper.SanitizeTextSimply(city.Name, sanitizationOpt);
        }

        /// <summary>
        /// Common validation for <see cref="City"/>
        /// </summary>
        /// <remarks>
        /// This validation is intended to be executed before storing or updating city in db
        /// </remarks>
        /// <param name="city"></param>
        /// <param name="insertMode"></param>
        internal static void CityCommonValidation(
            City city,
            bool insertMode,
            ICityRepository _cityRepository)
        {
            // City must be not null
            Verify.IsNot.Null(city, nameof(city));

            // Name must be specified
            Verify.IsNot.NullOrEmpty(city.Name, nameof(city.Name));

            if (insertMode)
            {
                // Creating

                // Name must be unique
                var duplicate = _cityRepository.SingleOrDefault(
                    x => x.Name == city.Name);

                if (duplicate != null)
                {
                    throw new CityDuplicateException(nameof(city.Name), city.Name);
                }
            }
            else
            {
                // Updating

                // City must exists
                Verify.IsNot.Null(city.CityId, nameof(city.CityId));
                Verify.Is.Positive(city.CityId.Value, nameof(city.CityId));

                _ = _cityRepository.Single(
                    x => x.CityId == city.CityId.Value,
                    ioEx => throw new CitySingleException(city.CityId.Value));
            }
        }
    }
}
