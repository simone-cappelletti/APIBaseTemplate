namespace APIBaseTemplate.Utils
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Miminize hour part on date time
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime MinimizeHourPart(this DateTime date) =>
            new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Kind);

        /// <summary>
        /// Maximize hour part on datetime
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime MaximizeHourPart(this DateTime date) =>
            new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999, date.Kind);
    }
}
