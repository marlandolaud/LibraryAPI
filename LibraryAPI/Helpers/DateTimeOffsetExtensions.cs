using System;

namespace LibraryAPI.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// gets the current age based on the date of birth
        /// </summary>
        /// <param name="dateOfBirth"></param>
        /// <returns>the number of years alive</returns>
        public static int GetCurrentAge(this DateTimeOffset dateOfBirth)
        {
            var currentDate = DateTime.UtcNow;
            int age = currentDate.Year - dateOfBirth.Year;
            if (currentDate < dateOfBirth.AddYears(age))
            {
                age--;
            }
            return age;
        }
    }
}