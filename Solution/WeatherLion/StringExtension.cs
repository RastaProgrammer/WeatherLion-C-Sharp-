using System;
using System.Text;
using System.Text.RegularExpressions;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          StringExtension
///   Description:    This allows the user to perform string 
///                   manipulations not natively built into
///                   C#.
///   Author:         Paul O. Patterson     Date: October 03, 2017
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    ///  This allows the user to perform string manipulations not natively built into C#.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// A list of comapss sectors.
        /// </summary>
        private static string[] compassSectors = { "N", "NNE", "NE", "ENE", "E", "ESE",
                                "SE", "SSE", "S", "SSW", "SW", "WSW",
                                "W", "WNW", "NW", "NNW" };

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a angle of degree representing a compass direction.
        /// </summary>
        /// <param name="degrees">The angle of the direction</param>
        /// <remarks>The value must be numeric and less than 360\u00B0.</remarks>
        /// <returns>The converted value.</returns>
        public static string CompassDirection(this float degrees)
        {
            int index = (int)((degrees / 22.5) + 0.5);

            return compassSectors[index % 16];
        }// end of method CompassDirection

        /// <summary>
        /// Checks if a string contains a whole word on its own
        /// </summary>
        /// <param name="input">A search <see cref="string"/></param>
        /// <param name="word">A <see cref="string"/> being searched for</param>
        /// <returns>True if the whole word is found, otherwise False.</returns>
        public static bool ContainsWholeWord(this string input, string word)
        {
            return Regex.Match(input, $@"\b{word}\b", RegexOptions.IgnoreCase).Success;
        }// end of method ContainsWholeWord

        /// <summary>
        /// dayDateFormat a Uri so that is is compatible with a valid standard <see cref="Uri"/> <see cref="string"/>
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> <see cref="string"/> to be formatted</param>
        /// <returns>A valid formatted <see cref="Uri"/> <see cref="string"/>.</returns>
        public static string EscapeUriString(this string uri)
        {
            return Uri.EscapeDataString(uri);
        }// end of method EscapeUriString

        /// <summary>
        /// Converts a string representation of a time in 12hr format
        /// to a time in 24hr format.
        /// </summary>
        /// <param name="time">A string representation of a time in 12hr format</param>
        /// <remarks>Value must be a time value represented as a string.</remarks>
        /// <returns>A string representation of a time in 24hr format.</returns>
        public static string Get24HourTime(this string time)
        {
            StringBuilder realTime = new StringBuilder(time);

            if (!realTime.ToString().Contains(" "))
            {
                int insertionPoint = time.IndexOf(":") + 2;

                realTime = new StringBuilder(time).Insert(time.Length - 2, " ");
            }// end of if block

            int hour = int.Parse(realTime.ToString().Split(':')[0].Trim());
            int minute = int.Parse(realTime.ToString().Split(':')[1].Trim().Split(' ')[0].Trim());
            string meridiem = realTime.ToString().Split(' ')[1].Trim();
            string t = null;

            if (meridiem.ToLower().Equals("am"))
            {
                t = string.Format("{0:00}:{1:00}", hour < 10 ? 0 + hour : hour == 12 ? 0 : hour, minute);
            }// end of if block
            else if (meridiem.ToLower().Equals("pm"))
            {
                t = string.Format("{0:00}:{1:00}", hour < 12 ? 12 + hour : hour, minute);
            }// end of else if block

            return t;
        }// end of method Get24HourTime

        /// <summary>
        /// Test if a <see cref="string"/> value is a number.
        /// </summary>
        /// <param name="value">A <see cref="string"/> value to be tested.</param>
        /// <returns>A <see cref="bool"/> true/false depending on the result of the test.</returns>
        public static bool IsNumeric(this string value)
        {
            bool result = value != null && double.TryParse(value, out double number);

            return result;
        }// end of method IsNumeric

        /// <summary>
        /// Searches for the number of times that a <see cref="char"/> is found in a <see cref="string"/>
        /// </summary>
        /// <param name="checkString">The <see cref="string"/> that contains the specified <see cref="char"/></param>
        /// <param name="c">The <see cref="char"/> to look for in a <see cref="string"/></param>
        /// <returns>An <see cref="int"/> representing the number of occurrences of the search character</returns>
        public static int NumberOfCharacterOccurences(this string checkString, char c)
        {
            int cn = 0;

            for (int i = 0; i < checkString.Length; i++)
            {
                if (c == checkString[i])
                {
                    cn++;
                }// end of if block
            }// end of for each loop

            return cn;
        }// end of method NumberOfCharacterOccurences

        /// <summary>
        /// Replace all instances of a character withing a <see cref="string"/>
        /// </summary>
        /// <param name="input">A <see cref="string"/> value containg a character</param>
        /// <param name="c">The <see cref="char"/> to be replaced</param>
        /// <param name="replacement">The replacement <see cref="char"/> or <see cref="string"/>.</param>
        /// <returns></returns>
        public static string ReplaceAll(this string input, string c, string replacement)
        {
            return Regex.Replace(input, $@"{c}+", replacement);
        }// end of method ReplaceAll

        /// <summary>
        /// Replace the last occurrence of a <see cref="string"/> contained in another <see cref="string"/>
        /// </summary>
        /// <param name="source">The <see cref="string"/> that contains another <see cref="string"/>.</param>
        /// <param name="find">The <see cref="string"/> to look for in another <see cref="string"/>.</param>
        /// <param name="replace">The replacement <see cref="string"/></param>
        /// <returns>A modified <see cref="string"/> reflecting the requested change</returns>
        public static string ReplaceLast(this string source, string find, string replace)
        {
            int place = source.LastIndexOf(find);

            if (place == -1)
            {
                return source;
            }// end of if block

            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }// end of method ReplaceLast

        /// <summary>
        /// Converts a string formatted with the proper case structure
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be formatted.</param>
        /// <returns>The <see cref="string"/> formatted with the proper case.</returns>
        public static string ToProperCase(this string str)
        {
            string[] sep = { " ", ",", "-", "/", "'" };
            int cycle = str.Length;
            StringBuilder sequence = new StringBuilder(str.ToLower());

            for (int i = 0; i <= cycle; i++)
            {
                if (i == 0 && char.IsLetter(sequence[i]))
                {
                    sequence[sequence.ToString().IndexOf(char.ToString(sequence[i]))] =
                        char.ToUpper(sequence[sequence.ToString().IndexOf(char.ToString(sequence[i]))]);
                }// end of if block
                else if ((i < cycle) && char.IsLetter(sequence[i]) &&
                        (sequence[i - 1] == 'c' && sequence[i - 2] == 'M'))
                {
                    sequence[sequence.ToString().ToString().IndexOf(char.ToString(sequence[i]))] =
                        char.ToUpper(sequence[sequence.ToString().IndexOf(char.ToString(sequence[i]))]);
                }// end of else if block
                else if ((i < cycle) && char.IsLetter(sequence[i]) &&
                        (char.ToString(sequence[i - 1]).Equals(sep[0]) ||
                        char.ToString(sequence[i - 1]).Equals(sep[1])))
                {
                    sequence[i] = char.ToUpper(sequence[i]);
                }// end of else if block
            }// end of for loop

            return sequence.ToString();
        }// end of method ProperCase       
    }// end of class StringExtension
}// end of namespace WeatherLion
