using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Shared.Utils
{
    public static class ValidationUtils
    {
        // Converts a string to a DayOfWeek enum and validates it
        public static DayOfWeek ParseDayOfWeek(string dayOfWeek)
        {
            try
            {
                return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeek, true);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Invalid day of the week: {dayOfWeek}");
            }
        }
        public static bool ValidateNonEmptyString(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }
    }
}
