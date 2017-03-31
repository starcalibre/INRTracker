using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INRTracker
{
    /// <summary>
    /// Various utility methods for the application.
    /// </summary>
    public static class Common
    {
        private static readonly Random _rand = new Random();

        /// <summary>
        /// Generates a random DateTime object. Used for generating mock
        /// entries at design time.
        /// </summary>
        /// <returns></returns>
        public static DateTime RandomDateTime()
        {
            int year = _rand.Next(2012, 2018);
            int month = _rand.Next(1, 13);

            // just generate up to 28th, it's not worth the trouble
            // worrying about sampling days based on the month
            int day = _rand.Next(1, 29); 

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Generate a random float in the range [min, max].
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float RandomFloat(float min, float max)
        {
            return min + (max - min) * (float) _rand.NextDouble();
        }
    }
}
