using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Repositories;

namespace APIBaseTemplate.Utils
{
    /// <summary>
    /// <see cref="FligthService"/> business helper class
    /// </summary>
    public class FligthServiceBusinessHelper
    {
        /// <summary>
        /// Sanitize and normalize <paramref name="fligthService"/>
        /// </summary>
        /// <param name="fligthService"></param>
        internal static void SanitizeNormalize(FligthService fligthService)
        {
            if (fligthService == null)
            {
                return;
            }
        }

        /// <summary>
        /// Common validation for <see cref="FligthService"/>
        /// </summary>
        /// <remarks>
        /// This validation is intended to be executed before storing or updating fligth service in db
        /// </remarks>
        /// <param name="fligthService"></param>
        /// <param name="insertMode"></param>
        internal static void FligthServiceCommonValidation(
            FligthService fligthService,
            bool insertMode,
            IFligthServiceRepository _fligthServiceRepository)
        {
            // FligthService must be not null
            Verify.IsNot.Null(fligthService, nameof(fligthService));

            if (insertMode)
            {
                // Creating
            }
            else
            {
                // Updating

                // Currency must exists
                Verify.IsNot.Null(fligthService.FligthServiceId, nameof(fligthService.FligthServiceId));
                Verify.Is.Positive(fligthService.FligthServiceId.Value, nameof(fligthService.FligthServiceId));

                _ = _fligthServiceRepository.Single(
                    x => x.FligthServiceId == fligthService.FligthServiceId.Value,
                    ioEx => throw new FligthServiceSingleException(fligthService.FligthServiceId.Value));
            }
        }
    }
}
