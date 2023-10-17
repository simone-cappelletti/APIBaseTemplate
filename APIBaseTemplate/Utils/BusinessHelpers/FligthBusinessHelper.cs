using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Common;
using APIBaseTemplate.Repositories;
using APIBaseTemplate.Datamodel.DTO;

namespace APIBaseTemplate.Utils
{
    /// <summary>
    /// <see cref="Fligth"/> business helper class
    /// </summary>
    public class FligthBusinessHelper
    {
        /// <summary>
        /// Sanitize and normalize <paramref name="fligth"/>
        /// </summary>
        /// <param name="fligth"></param>
        internal static void SanitizeNormalize(Fligth fligth)
        {
            if (fligth == null)
            {
                return;
            }

            var sanitizationOpt = TextSanitizerHelper.GetSanitizationOptionsRemovePercentAndTrim();

            fligth.Code = TextSanitizerHelper.SanitizeTextSimply(fligth.Code, sanitizationOpt);
            fligth.Terminal = TextSanitizerHelper.SanitizeTextSimply(fligth.Terminal, sanitizationOpt);
            fligth.Gate = TextSanitizerHelper.SanitizeTextSimply(fligth.Gate, sanitizationOpt);
        }

        /// <summary>
        /// Common validation for <see cref="Fligth"/>
        /// </summary>
        /// <remarks>
        /// This validation is intended to be executed before storing or updating fligth in db
        /// </remarks>
        /// <param name="fligth"></param>
        /// <param name="insertMode"></param>
        internal static void FligthCommonValidation(
            Fligth fligth,
            bool insertMode,
            IFligthRepository _fligthRepository)
        {
            // Fligth must be not null
            Verify.IsNot.Null(fligth, nameof(fligth));

            // Name, Terminal and Gate must be specified
            Verify.IsNot.NullOrEmpty(fligth.Code, nameof(fligth.Code));
            Verify.IsNot.NullOrEmpty(fligth.Terminal, nameof(fligth.Terminal));
            Verify.IsNot.NullOrEmpty(fligth.Gate, nameof(fligth.Gate));

            if (insertMode)
            {
                // Creating

                // Code must be unique
                var duplicate = _fligthRepository.SingleOrDefault(
                    x => x.Code == fligth.Code);

                if (duplicate != null)
                {
                    throw new FligthDuplicateException(nameof(fligth.Code), fligth.Code);
                }
            }
            else
            {
                // Updating

                // Fligth must exists
                Verify.IsNot.Null(fligth.FligthId, nameof(fligth.FligthId));
                Verify.Is.Positive(fligth.FligthId.Value, nameof(fligth.FligthId));

                _ = _fligthRepository.Single(
                    x => x.FligthId == fligth.FligthId.Value,
                    ioEx => throw new FligthSingleException(fligth.FligthId.Value));
            }
        }
    }
}
