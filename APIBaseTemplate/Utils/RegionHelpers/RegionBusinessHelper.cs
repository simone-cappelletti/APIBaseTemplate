using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Repositories.Repositories;

namespace APIBaseTemplate.Utils
{
    public class RegionBusinessHelper
    {
        /// <summary>
        /// Sanitize and normalize <paramref name="region"/>
        /// </summary>
        /// <param name="region"></param>
        internal static void SanitizeNormalize(Region region)
        {
            if (region == null)
            {
                return;
            }

            var sanitizationOpt = TextSanitizerHelper.GetSanitizationOptionsRemovePercentAndTrim();

            region.Name = TextSanitizerHelper.SanitizeTextSimply(region.Name, sanitizationOpt);
        }

        /// <summary>
        /// Common validation for <see cref="Region"/>
        /// </summary>
        /// <remarks>
        /// This validation is intended to be executed before storing or updating region in db
        /// </remarks>
        /// <param name="region"></param>
        /// <param name="insertMode"></param>
        internal static void RegionCommonValidation(
            Region region,
            bool insertMode,
            IRegionRepository _regionRepository)
        {
            // Region must be not null
            Verify.IsNot.Null(region, nameof(region));

            // Name must be specified
            Verify.IsNot.Null(region.Name, nameof(region.Name));

            if (insertMode)
            {
                // Creating

                // Name must be unique
                _ = _regionRepository.Single(
                    x => x.Name == region.Name,
                    ioEx => throw new RegionDuplicateException(nameof(region.Name), region.Name));
            }
            else
            {
                // Updating

                // Region must exists
                Verify.IsNot.Null(region.RegionId, nameof(region.RegionId));
                Verify.Is.Positive(region.RegionId.Value, nameof(region.RegionId));

                _ = _regionRepository.Single(
                    x => x.RegionId == region.RegionId.Value,
                    ioEx => throw new RegionSingleException(region.RegionId.Value));
            }
        }
    }
}
