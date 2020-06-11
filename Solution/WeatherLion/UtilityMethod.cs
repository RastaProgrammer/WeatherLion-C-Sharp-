using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Drawing.Drawing2D;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          UtilityMethod
///   Description:    This class contains multi-purpose methods for
///                   completing differnt action required for the
///                   program operation.
///   Author:         Paul O. Patterson     Date: October 03, 2017
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// This class provides all the utility functions that the program may
    /// need to perform simple tasks and basic calculations.
    /// </summary>
    public static class UtilityMethod
    {
        #region Field Variables

        private const string TAG = "UtilityMethod";

        public static string[] yahooWeatherCodes =
        {
           "tornado", "tropical storm", "hurricane", "severe thunderstorms",
           "thunderstorms", "mixed rain and snow", "mixed rain and sleet",
           "mixed snow and sleet", "freezing drizzle", "drizzle", "freezing rain",
           "showers", "showers", "snow flurries", "light snow showers", "blowing snow",
           "snow", "hail", "sleet", "dust", "foggy", "haze", "smoky", "blustery", "windy",
           "cold", "cloudy", "mostly cloudy (night)", "mostly cloudy (day)",
           "partly cloudy (night)", "partly cloudy (day)", "clear (night)", "sunny",
           "fair (night)", "fair (day)", "mixed rain and hail", "hot",
           "isolated thunderstorms", "scattered thunderstorms", "scattered thunderstorms",
           "scattered showers", "heavy snow", "scattered snow showers", "heavy snow",
           "partly cloudy", "thundershowers", "snow showers", "isolated thundershowers"
        };

        /// <summary>
        /// A list of comapss sectors.
        /// </summary>
        public static string[] compassSectors = { "N", "NNE", "NE", "ENE", "E", "ESE",
                                "SE", "SSE", "S", "SSW", "SW", "WSW",
                                "W", "WNW", "NW", "NNW" };

        /// <summary>
        /// A collection of compass cardinal points.
        /// </summary>
        public static Hashtable cardinalPoints = new Hashtable()
        {
            {"E", "East"},
            {"N", "North"},
            {"S", "South"},
            {"W", "West"}
        };

        #region US States
        /// <summary>
        /// Maps a US state's two-letter code to its full name
        /// </summary>
        public static Hashtable usStatesByCode = new Hashtable()
        {
            {"AL", "Alabama"},
            {"AK", "Alaska"},
            {"AZ", "Arizona"},
            {"AR", "Arkansas"},
            {"CA", "California"},
            {"CO", "Colorado"},
            {"CT", "Connecticut"},
            {"DE", "Delaware"},
            {"FL", "Florida"},
            {"GA", "Georgia"},
            {"HI", "Hawaii"},
            {"ID", "Idaho"},
            {"IL", "Illinois"},
            {"IN", "Indiana"},
            {"IA", "Iowa"},
            {"KS", "Kansas"},
            {"KY", "Kentucky"},
            {"LA", "Louisiana"},
            {"ME", "Maine"},
            {"MD", "Maryland"},
            {"MA", "Massachusetts"},
            {"MI", "Michigan"},
            {"MN", "Minnesota"},
            {"MS", "Mississippi"},
            {"MO", "Missouri"},
            {"MT", "Montana"},
            {"NE", "Nebraska"},
            {"NV", "Nevada"},
            {"NH", "New Hampshire"},
            {"NJ", "New Jersey"},
            {"NM", "New Mexico"},
            {"NY", "New York"},
            {"NC", "North Carolina"},
            {"ND", "North Dakota"},
            {"OH", "Ohio"},
            {"OK", "Oklahoma"},
            {"OR", "Oregon"},
            {"PA", "Pennsylvania"},
            {"RI", "Rhode Island"},
            {"SC", "South Carolina"},
            {"SD", "South Dakota"},
            {"TN", "Tennessee"},
            {"TX", "Texas"},
            {"UT", "Utah"},
            {"VT", "Vermont"},
            {"VA", "Virginia"},
            {"WA", "Washington"},
            {"WV", "West Virginia"},
            {"WI", "Wisconsin"},
            {"WY", "Wyoming"}
        };

        /// <summary>
        /// Maps a US state's full name to its two-letter code
        /// </summary>
        public static Hashtable usStatesByName = new Hashtable()
        {
            {"Alabama", "AL"},
            {"Alaska", "AK"},
            {"Arizona", "AZ"},
            {"Arkansas", "AR"},
            {"California", "CA"},
            {"Colorado", "CO"},
            {"Connecticut", "CT"},
            {"Delaware", "DE"},
            {"Florida", "FL"},
            {"Georgia", "GA"},
            {"Hawaii", "HI"},
            {"Idaho", "ID"},
            {"Illinois", "IL"},
            {"Indiana", "IN"},
            {"Iowa", "IA"},
            {"Kansas", "KS"},
            {"Kentucky", "KY"},
            {"Louisiana", "LA"},
            {"Maine", "ME"},
            {"Maryland", "MD"},
            {"Massachusetts", "MA"},
            {"Michigan", "MI"},
            {"Minnesota", "MN"},
            {"Mississippi", "MS"},
            {"Missouri", "MO"},
            {"Montana", "MT"},
            {"Nebraska", "NE"},
            {"Nevada", "NV"},
            {"New Hampshire", "NH"},
            {"New Jersey", "NJ"},
            {"New Mexico", "NM"},
            {"New York", "NY"},
            {"North Carolina", "NC"},
            {"North Dakota", "ND"},
            {"Ohio", "OH"},
            {"Oklahoma", "OK"},
            {"Oregon", "OR"},
            {"Pennsylvania", "PA"},
            {"Rhode Island", "RI"},
            {"South Carolina", "SC"},
            {"South Dakota", "SD"},
            {"Tennessee", "TN"},
            {"Texas", "TX"},
            {"Utah", "UT"},
            {"Vermont", "VT"},
            {"Virginia", "VA"},
            {"Washington", "WA"},
            {"West Virginia", "WV"},
            {"Wisconsin", "WI"},
            {"Wyoming", "WY"}
        };

        #endregion

        #region World Countries Collection
        /// <summary>
        /// Maps a country's two-letter code to its full name
        /// </summary>
        public static Hashtable worldCountries = new Hashtable()
        {
            {"AF", "Afghanistan"},
            {"AX", "Aland Islands"},
            {"AL", "Albania"},
            {"DZ", "Algeria"},
            {"AS", "American Samoa"},
            {"AD", "Andorra"},
            {"AO", "Angola"},
            {"AI", "Anguilla"},
            {"AQ", "Antarctica"},
            {"AG", "Antigua and Barbuda"},
            {"AR", "Argentina"},
            {"AM", "Armenia"},
            {"AW", "Aruba"},
            {"AU", "Australia"},
            {"AT", "Austria"},
            {"AZ", "Azerbaijan"},
            {"BS", "Bahamas"},
            {"BH", "Bahrain"},
            {"BD", "Bangladesh"},
            {"BB", "Barbados"},
            {"BY", "Belarus"},
            {"BE", "Belgium"},
            {"BZ", "Belize"},
            {"BJ", "Benin"},
            {"BM", "Bermuda"},
            {"BT", "Bhutan"},
            {"BO", "Bolivia"},
            {"BA", "Bosnia and Herzegovina"},
            {"BW", "Botswana"},
            {"BV", "Bouvet Island"},
            {"BR", "Brazil"},
            {"VG", "British Virgin Islands"},
            {"IO", "British Indian Ocean Territory"},
            {"BN", "Brunei Darussalam"},
            {"BG", "Bulgaria"},
            {"BF", "Burkina Faso"},
            {"BI", "Burundi"},
            {"KH", "Cambodia"},
            {"CM", "Cameroon"},
            {"CA", "Canada"},
            {"CV", "Cape Verde"},
            {"KY", "Cayman Islands"},
            {"CF", "Central African Republic"},
            {"TD", "Chad"},
            {"CL", "Chile"},
            {"CN", "China"},
            {"HK", "Hong Kong, SAR China"},
            {"MO", "Macao, SAR China"},
            {"CX", "Christmas Island"},
            {"CC", "Cocos (Keeling) Islands"},
            {"CO", "Colombia"},
            {"KM", "Comoros"},
            {"CG", "Congo (Brazzaville)"},
            {"CD", "Congo, (Kinshasa)"},
            {"CK", "Cook Islands"},
            {"CR", "Costa Rica"},
            {"CI", "Côte d'Ivoire"},
            {"HR", "Croatia"},
            {"CU", "Cuba"},
            {"CY", "Cyprus"},
            {"CZ", "Czech Republic"},
            {"DK", "Denmark"},
            {"DJ", "Djibouti"},
            {"DM", "Dominica"},
            {"DO", "Dominican Republic"},
            {"EC", "Ecuador"},
            {"EG", "Egypt"},
            {"SV", "El Salvador"},
            {"GQ", "Equatorial Guinea"},
            {"ER", "Eritrea"},
            {"EE", "Estonia"},
            {"ET", "Ethiopia"},
            {"FK", "Falkland Islands (Malvinas)"},
            {"FO", "Faroe Islands"},
            {"FJ", "Fiji"},
            {"FI", "Finland"},
            {"FR", "France"},
            {"GF", "French Guiana"},
            {"PF", "French Polynesia"},
            {"TF", "French Southern Territories"},
            {"GA", "Gabon"},
            {"GM", "Gambia"},
            {"GE", "Georgia"},
            {"DE", "Germany"},
            {"GH", "Ghana"},
            {"GI", "Gibraltar"},
            {"GR", "Greece"},
            {"GL", "Greenland"},
            {"GD", "Grenada"},
            {"GP", "Guadeloupe"},
            {"GU", "Guam"},
            {"GT", "Guatemala"},
            {"GG", "Guernsey"},
            {"GN", "Guinea"},
            {"GW", "Guinea-Bissau"},
            {"GY", "Guyana"},
            {"HT", "Haiti"},
            {"HM", "Heard and Mcdonald Islands"},
            {"VA", "Holy See (Vatican City State)"},
            {"HN", "Honduras"},
            {"HU", "Hungary"},
            {"IS", "Iceland"},
            {"IN", "India"},
            {"ID", "Indonesia"},
            {"IR", "Iran, Islamic Republic of"},
            {"IQ", "Iraq"},
            {"IE", "Ireland"},
            {"IM", "Isle of Man"},
            {"IL", "Israel"},
            {"IT", "Italy"},
            {"JM", "Jamaica"},
            {"JP", "Japan"},
            {"JE", "Jersey"},
            {"JO", "Jordan"},
            {"KZ", "Kazakhstan"},
            {"KE", "Kenya"},
            {"KI", "Kiribati"},
            {"KP", "Korea (North)"},
            {"KR", "Korea (South)"},
            {"KW", "Kuwait"},
            {"KG", "Kyrgyzstan"},
            {"LA", "Lao PDR"},
            {"LV", "Latvia"},
            {"LB", "Lebanon"},
            {"LS", "Lesotho"},
            {"LR", "Liberia"},
            {"LY", "Libya"},
            {"LI", "Liechtenstein"},
            {"LT", "Lithuania"},
            {"LU", "Luxembourg"},
            {"MK", "Macedonia, Republic of"},
            {"MG", "Madagascar"},
            {"MW", "Malawi"},
            {"MY", "Malaysia"},
            {"MV", "Maldives"},
            {"ML", "Mali"},
            {"MT", "Malta"},
            {"MH", "Marshall Islands"},
            {"MQ", "Martinique"},
            {"MR", "Mauritania"},
            {"MU", "Mauritius"},
            {"YT", "Mayotte"},
            {"MX", "Mexico"},
            {"FM", "Micronesia, Federated States of"},
            {"MD", "Moldova"},
            {"MC", "Monaco"},
            {"MN", "Mongolia"},
            {"ME", "Montenegro"},
            {"MS", "Montserrat"},
            {"MA", "Morocco"},
            {"MZ", "Mozambique"},
            {"MM", "Myanmar"},
            {"NA", "Namibia"},
            {"NR", "Nauru"},
            {"NP", "Nepal"},
            {"NL", "Netherlands"},
            {"AN", "Netherlands Antilles"},
            {"NC", "New Caledonia"},
            {"NZ", "New Zealand"},
            {"NI", "Nicaragua"},
            {"NE", "Niger"},
            {"NG", "Nigeria"},
            {"NU", "Niue"},
            {"NF", "Norfolk Island"},
            {"MP", "Northern Mariana Islands"},
            {"NO", "Norway"},
            {"OM", "Oman"},
            {"PK", "Pakistan"},
            {"PW", "Palau"},
            {"PS", "Palestinian Territory"},
            {"PA", "Panama"},
            {"PG", "Papua New Guinea"},
            {"PY", "Paraguay"},
            {"PE", "Peru"},
            {"PH", "Philippines"},
            {"PN", "Pitcairn"},
            {"PL", "Poland"},
            {"PT", "Portugal"},
            {"PR", "Puerto Rico"},
            {"QA", "Qatar"},
            {"RE", "Réunion"},
            {"RO", "Romania"},
            {"RU", "Russian Federation"},
            {"RW", "Rwanda"},
            {"BL", "Saint-Barthélemy"},
            {"SH", "Saint Helena"},
            {"KN", "Saint Kitts and Nevis"},
            {"LC", "Saint Lucia"},
            {"MF", "Saint-Martin (French part)"},
            {"PM", "Saint Pierre and Miquelon"},
            {"VC", "Saint Vincent and Grenadines"},
            {"WS", "Samoa"},
            {"SM", "San Marino"},
            {"ST", "Sao Tome and Principe"},
            {"SA", "Saudi Arabia"},
            {"SN", "Senegal"},
            {"RS", "Serbia"},
            {"SC", "Seychelles"},
            {"SL", "Sierra Leone"},
            {"SG", "Singapore"},
            {"SK", "Slovakia"},
            {"SI", "Slovenia"},
            {"SB", "Solomon Islands"},
            {"SO", "Somalia"},
            {"ZA", "South Africa"},
            {"GS", "South Georgia and the South Sandwich Islands"},
            {"SS", "South Sudan"},
            {"ES", "Spain"},
            {"LK", "Sri Lanka"},
            {"SD", "Sudan"},
            {"SR", "Suriname"},
            {"SJ", "Svalbard and Jan Mayen Islands"},
            {"SZ", "Swaziland"},
            {"SE", "Sweden"},
            {"CH", "Switzerland"},
            {"SY", "Syrian Arab Republic (Syria)"},
            {"TW", "Taiwan, Republic of China"},
            {"TJ", "Tajikistan"},
            {"TZ", "Tanzania, United Republic of"},
            {"TH", "Thailand"},
            {"TL", "Timor-Leste"},
            {"TG", "Togo"},
            {"TK", "Tokelau"},
            {"TO", "Tonga"},
            {"TT", "Trinidad and Tobago"},
            {"TN", "Tunisia"},
            {"TR", "Turkey"},
            {"TM", "Turkmenistan"},
            {"TC", "Turks and Caicos Islands"},
            {"TV", "Tuvalu"},
            {"UG", "Uganda"},
            {"UA", "Ukraine"},
            {"AE", "United Arab Emirates"},
            {"GB", "United Kingdom"},
            {"US", "United States of America"},
            {"UM", "US Minor Outlying Islands"},
            {"UY", "Uruguay"},
            {"UZ", "Uzbekistan"},
            {"VU", "Vanuatu"},
            {"VE", "Venezuela (Bolivarian Republic)"},
            {"VN", "Viet Nam"},
            {"VI", "Virgin Islands, US"},
            {"WF", "Wallis and Futuna Islands"},
            {"EH", "Western Sahara"},
            {"YE", "Yemen"},
            {"ZM", "Zambia"},
            {"ZW", "Zimbabwe"}
        };

        /// <summary>
        /// // Maps a country's full name to its two-letter code
        /// </summary>
        public static Hashtable worldCountryCodes = new Hashtable()
        {
            {"Afghanistan", "AF"},
            {"Aland Islands", "AX"},
            {"Albania", "AL"},
            {"Algeria", "DZ"},
            {"American Samoa", "AS"},
            {"Andorra", "AD"},
            {"Angola", "AO"},
            {"Anguilla", "AI"},
            {"Antarctica", "AQ"},
            {"Antigua and Barbuda", "AG"},
            {"Argentina", "AR"},
            {"Armenia", "AM"},
            {"Aruba", "AW"},
            {"Australia", "AU"},
            {"Austria", "AT"},
            {"Azerbaijan", "AZ"},
            {"Bahamas", "BS"},
            {"Bahrain", "BH"},
            {"Bangladesh", "BD"},
            {"Barbados", "BB"},
            {"Belarus", "BY"},
            {"Belgium", "BE"},
            {"Belize", "BZ"},
            {"Benin", "BJ"},
            {"Bermuda", "BM"},
            {"Bhutan", "BT"},
            {"Bolivia", "BO"},
            {"Bosnia and Herzegovina", "BA"},
            {"Botswana", "BW"},
            {"Bouvet Island", "BV"},
            {"Brazil", "BR"},
            {"British Virgin Islands", "VG"},
            {"British Indian Ocean Territory", "IO"},
            {"Brunei Darussalam", "BN"},
            {"Bulgaria", "BG"},
            {"Burkina Faso", "BF"},
            {"Burundi", "BI"},
            {"Cambodia", "KH"},
            {"Cameroon", "CM"},
            {"Canada", "CA"},
            {"Cape Verde", "CV"},
            {"Cayman Islands", "KY"},
            {"Central African Republic", "CF"},
            {"Chad", "TD"},
            {"Chile", "CL"},
            {"China", "CN"},
            {"Hong Kong", "HK"},
            {"Macao", "MO"},
            {"Christmas Island", "CX"},
            {"Cocos (Keeling) Islands", "CC"},
            {"Colombia", "CO"},
            {"Comoros", "KM"},
            {"Congo (Brazzaville)", "CG"},
            {"Congo", "CD"},
            {"Cook Islands", "CK"},
            {"Costa Rica", "CR"},
            {"Côte d'Ivoire", "CI"},
            {"Croatia", "HR"},
            {"Cuba", "CU"},
            {"Cyprus", "CY"},
            {"Czech Republic", "CZ"},
            {"Denmark", "DK"},
            {"Djibouti", "DJ"},
            {"Dominica", "DM"},
            {"Dominican Republic", "DO"},
            {"Ecuador", "EC"},
            {"Egypt", "EG"},
            {"El Salvador", "SV"},
            {"Equatorial Guinea", "GQ"},
            {"Eritrea", "ER"},
            {"Estonia", "EE"},
            {"Ethiopia", "ET"},
            {"Falkland Islands (Malvinas)", "FK"},
            {"Faroe Islands", "FO"},
            {"Fiji", "FJ"},
            {"Finland", "FI"},
            {"France", "FR"},
            {"French Guiana", "GF"},
            {"French Polynesia", "PF"},
            {"French Southern Territories", "TF"},
            {"Gabon", "GA"},
            {"Gambia", "GM"},
            {"Georgia", "GE"},
            {"Germany", "DE"},
            {"Ghana", "GH"},
            {"Gibraltar", "GI"},
            {"Greece", "GR"},
            {"Greenland", "GL"},
            {"Grenada", "GD"},
            {"Guadeloupe", "GP"},
            {"Guam", "GU"},
            {"Guatemala", "GT"},
            {"Guernsey", "GG"},
            {"Guinea", "GN"},
            {"Guinea-Bissau", "GW"},
            {"Guyana", "GY"},
            {"Haiti", "HT"},
            {"Heard and Mcdonald Islands", "HM"},
            {"Holy See (Vatican City State)", "VA"},
            {"Honduras", "HN"},
            {"Hungary", "HU"},
            {"Iceland", "IS"},
            {"India", "IN"},
            {"Indonesia", "ID"},
            {"Iran", "IR"},
            {"Iraq", "IQ"},
            {"Ireland", "IE"},
            {"Isle of Man", "IM"},
            {"Israel", "IL"},
            {"Italy", "IT"},
            {"Jamaica", "JM"},
            {"Japan", "JP"},
            {"Jersey", "JE"},
            {"Jordan", "JO"},
            {"Kazakhstan", "KZ"},
            {"Kenya", "KE"},
            {"Kiribati", "KI"},
            {"Korea (North)", "KP"},
            {"Korea (South)", "KR"},
            {"Kuwait", "KW"},
            {"Kyrgyzstan", "KG"},
            {"Lao PDR", "LA"},
            {"Latvia", "LV"},
            {"Lebanon", "LB"},
            {"Lesotho", "LS"},
            {"Liberia", "LR"},
            {"Libya", "LY"},
            {"Liechtenstein", "LI"},
            {"Lithuania", "LT"},
            {"Luxembourg", "LU"},
            {"Macedonia", "MK"},
            {"Madagascar", "MG"},
            {"Malawi", "MW"},
            {"Malaysia", "MY"},
            {"Maldives", "MV"},
            {"Mali", "ML"},
            {"Malta", "MT"},
            {"Marshall Islands", "MH"},
            {"Martinique", "MQ"},
            {"Mauritania", "MR"},
            {"Mauritius", "MU"},
            {"Mayotte", "YT"},
            {"Mexico", "MX"},
            {"Micronesia", "FM"},
            {"Moldova", "MD"},
            {"Monaco", "MC"},
            {"Mongolia", "MN"},
            {"Montenegro", "ME"},
            {"Montserrat", "MS"},
            {"Morocco", "MA"},
            {"Mozambique", "MZ"},
            {"Myanmar", "MM"},
            {"Namibia", "NA"},
            {"Nauru", "NR"},
            {"Nepal", "NP"},
            {"Netherlands", "NL"},
            {"Netherlands Antilles", "AN"},
            {"New Caledonia", "NC"},
            {"New Zealand", "NZ"},
            {"Nicaragua", "NI"},
            {"Niger", "NE"},
            {"Nigeria", "NG"},
            {"Niue", "NU"},
            {"Norfolk Island", "NF"},
            {"Northern Mariana Islands", "MP"},
            {"Norway", "NO"},
            {"Oman", "OM"},
            {"Pakistan", "PK"},
            {"Palau", "PW"},
            {"Palestinian Territory", "PS"},
            {"Panama", "PA"},
            {"Papua New Guinea", "PG"},
            {"Paraguay", "PY"},
            {"Peru", "PE"},
            {"Philippines", "PH"},
            {"Pitcairn", "PN"},
            {"Poland", "PL"},
            {"Portugal", "PT"},
            {"Puerto Rico", "PR"},
            {"Qatar", "QA"},
            {"Réunion", "RE"},
            {"Romania", "RO"},
            {"Russian Federation", "RU"},
            {"Rwanda", "RW"},
            {"Saint-Barthélemy", "BL"},
            {"Saint Helena", "SH"},
            {"Saint Kitts and Nevis", "KN"},
            {"Saint Lucia", "LC"},
            {"Saint-Martin (French part)", "MF"},
            {"Saint Pierre and Miquelon", "PM"},
            {"Saint Vincent and Grenadines", "VC"},
            {"Samoa", "WS"},
            {"San Marino", "SM"},
            {"Sao Tome and Principe", "ST"},
            {"Saudi Arabia", "SA"},
            {"Senegal", "SN"},
            {"Serbia", "RS"},
            {"Seychelles", "SC"},
            {"Sierra Leone", "SL"},
            {"Singapore", "SG"},
            {"Slovakia", "SK"},
            {"Slovenia", "SI"},
            {"Solomon Islands", "SB"},
            {"Somalia", "SO"},
            {"South Africa", "ZA"},
            {"South Georgia and the South Sandwich Islands", "GS"},
            {"South Sudan", "SS"},
            {"Spain", "ES"},
            {"Sri Lanka", "LK"},
            {"Sudan", "SD"},
            {"Suriname", "SR"},
            {"Svalbard and Jan Mayen Islands", "SJ"},
            {"Swaziland", "SZ"},
            {"Sweden", "SE"},
            {"Switzerland", "CH"},
            {"Syrian Arab Republic (Syria)", "SY"},
            {"Taiwan", "TW"},
            {"Tajikistan", "TJ"},
            {"Tanzania", "TZ"},
            {"Thailand", "TH"},
            {"Timor-Leste", "TL"},
            {"Togo", "TG"},
            {"Tokelau", "TK"},
            {"Tonga", "TO"},
            {"Trinidad and Tobago", "TT"},
            {"Tunisia", "TN"},
            {"Turkey", "TR"},
            {"Turkmenistan", "TM"},
            {"Turks and Caicos Islands", "TC"},
            {"Tuvalu", "TV"},
            {"Uganda", "UG"},
            {"Ukraine", "UA"},
            {"United Arab Emirates", "AE"},
            {"United Kingdom", "GB"},
            {"USA", "US"},
            {"United States", "US"},
            {"United States of America", "US"},
            {"US Minor Outlying Islands", "UM"},
            {"Uruguay", "UY"},
            {"Uzbekistan", "UZ"},
            {"Vanuatu", "VU"},
            {"Venezuela (Bolivarian Republic)", "VE"},
            {"Viet Nam", "VN"},
            {"Virgin Islands", "VI"},
            {"Wallis and Futuna Islands", "WF"},
            {"Western Sahara", "EH"},
            {"Yemen", "YE"},
            {"Zambia", "ZM"},
            {"Zimbabwe", "ZW"}
        };

        #endregion

        #region Weather Icons Collection

        /// <summary>
        /// A collection of known weather codes using their description as a key.
        /// </summary>
        public static Hashtable weatherImages = new Hashtable()
        {
            {"tornado", "0.png"},
            {"tropical storm", "1.png"},
            {"hurricane", "1.png"},
            {"severe thunderstorm", "1.png"},
            {"severe thunderstorms", "1.png"},
            {"thunderstorm", "1.png"},
            {"thunderstorms", "1.png"},
            {"tstorms", "1.png"},
            {"t-storms", "1.png"},
            {"freezing rain", "2.png"},
            {"mixed rain and hail", "2.png"},
            {"mixed rain and snow", "2.png"},
            {"mixed rain and sleet", "2.png"},
            {"sleet", "2.png"},
            {"light snow showers", "2.png"},
            {"freezing drizzle", "2.png"},
            {"blowing snow", "3.png"},
            {"heavy snow", "3.png"},
            {"mixed snow and sleet", "3.png"},
            {"scattered snow showers", "3.png"},
            {"snow", "3.png"},
            {"snow showers", "3.png"},            
            {"light rain", "4.png"},
            {"moderate rain", "4.png"},
            {"sprinkles", "4.png"},
            {"heavy intensity rain", "5.png"},
            {"heavy rain", "5.png"},
            {"heavy rain showers", "5.png"},
            {"rain", "5.png"},
            {"rain showers", "5.png"},
            {"shower rain", "5.png"},
            {"showers", "5.png"},
            {"snow flurries", "5.png"},
            {"hail", "5.png"},
            {"dust", "6.png"},
            {"smoky", "6.png"},
            {"fog", "7.png"},
            {"foggy", "7.png"},
            {"haze", "7.png"},
            {"mist", "7.png"},
            {"misty", "7.png"},
            {"clouds", "8.png"},
            {"cloudy", "8.png"},
            {"overcast", "8.png"},
            {"broken clouds", "8.png"},
            {"overcast clouds", "8.png"},
            {"scattered clouds", "8.png"},
            {"cold", "9.png"},
            {"mostly cloudy", "10.png"},
            {"mostly cloudy (night)", "11.png"},
            {"few clouds", "12.png"},
            {"humid", "12.png"},
            {"humid and partly cloudy", "12.png"},
            {"mostly clear", "12.png"},
            {"partly cloudy", "12.png"},
            {"mostly sunny", "12.png"},
            {"few clouds (night)", "13.png"},
            {"humid  (night)", "13.png"},
            {"mostly sunny (night)", "13.png"},
            {"mostly clear (night)", "13.png"},
            {"partly cloudy (night)", "13.png"},
            {"clear (night)", "14.png"},
            {"clear sky (night)", "14.png"},
            {"fair (night)", "14.png"},
            {"sunny (night)", "14.png"},
            {"clear", "15.png"},
            {"clear sky", "15.png"},
            {"fair", "15.png"},
            {"sunny", "15.png"},
            {"hot", "15.png"},
            {"sky is clear", "15.png"},
            {"chance of a thunderstorm", "16.png"},
            {"isolated t-storms", "16.png"},
            {"isolated thunderstorms", "16.png"},
            {"isolated thundershowers", "16.png"},
            {"isolated t-showers", "16.png"},
            {"scattered thunderstorms", "16.png"},
            {"scattered t-storms", "16.png"},
            {"scattered tstorms", "16.png"},
            {"thundershowers", "16.png"},            
            {"thunderstorm with rain", "16.png"},
            {"drizzle", "17.png"},
            {"scattered showers", "17.png"},
            {"isolated showers", "17.png"},
            {"chance of rain", "17.png"},
            {"light rain showers", "17.png"},
            {"light shower rain", "17.png"},
            {"chance of a thunderstorm (night)", "18.png"},
            {"isolated thunderstorms (night)", "18.png"},
            {"isolated t-storms (night)", "18.png"},
            {"isolated thundershowers (night)", "18.png"},
            {"isolated t-showers (night)", "18.png"},
            {"scattered thunderstorms (night)", "18.png"},
            {"scattered t-storms (night)", "18.png"},
            {"thundershowers (night)", "18.png"},
            {"t-showers (night)", "18.png"},
            {"drizzle (night)", "19.png"},
            {"scattered showers (night)", "19.png"},
            {"isolated showers (night)", "19.png"},
            {"chance of rain (night)", "19.png"},
            {"light rain showers (night)", "19.png"},
            {"light shower rain (night)", "19.png"},
            {"dust (night)", "20.png"},
            {"smoky (night)", "20.png"},
            {"blustery (night)", "20.png"},
            {"breezy (night)", "20.png"},
            {"blustery", "20.png"},
            {"windy (night)", "21.png"},
            {"breez", "21.png"},
            {"breeze", "21.png"},
            {"breezy", "21.png"},
            {"wind", "21.png"},
            {"windy", "21.png"},
            {"not available", "na.png"}
        };

        #endregion

        #region World Timezone Collection
        /// <summary>
        /// World timezone offsets
        /// </summary>
        /// <remarks>Some timezones a commented out because they are duplicate keys but they are different</remarks>
        public static Hashtable worldTimezonesOffsets = new Hashtable()
        {
            {"A", "+01:00"},
            {"ACDT", "+10:30"},
            {"ACST", "+09:30"},
            {"ADT", "-03:00"},
            {"AEDT", "+11:00"},
            {"AEST", "+10:00"},
            {"AFT", "+04:30"},
            {"AKDT", "-08:00"},
            {"AKST", "-09:00"},
            {"ALMT", "+06:00"},
            //{"AMST", "+05:00"}, Armenia Summer Time
            {"AMST", "-03:00"},
            //{"AMT", "+04:00"}, Armenia Time
            {"AMT", "-04:00"},
            {"ANAST", "+12:00"},
            {"ANAT", "+12:00"},
            {"AQTT", "+05:00"},
            {"ART", "-03:00"},
            //{"AST", "+03:00"}, Arabia Standard Time
            {"AST", "-04:00"},
            {"AWDT", "+09:00"},
            {"AWST", "+08:00"},
            {"AZOST", "00:00"},
            {"AZOT", "-01:00"},
            {"AZST", "+05:00"},
            {"AZT", "+04:00"},
            {"B", "+02:00"},
            {"BNT", "+08:00"},
            {"BOT", "-04:00"},
            {"BRST", "-02:00"},
            {"BRT", "-03:00"},
            //{"BST", "+06:00"}, Bangladesh Standard Time
            {"BST", "+01:00"},
            {"BTT", "+06:00"},
            {"C", "+03:00"},
            {"CAST", "+08:00"},
            {"CAT", "+02:00"},
            {"CCT", "+06:30"},
            //{"CDT", "-04:00"}, Cuba daylight Time
            {"CDT", "-05:00"},
            {"CEST", "+02:00"},
            {"CET", "+01:00"},
            {"CHADT", "+13:45"},
            {"CHAST", "+12:45"},
            {"CKT", "-10:00"},
            {"CLST", "-03:00"},
            {"CLT", "-04:00"},
            {"COT", "-05:00"},
            //{"CST", "+08:00"}, China Standard Time
            //{"CST", "-05:00"}, Cuba Standard Time
            {"CST", "-06:00"},
            {"CVT", "-01:00"},
            {"CXT", "+07:00"},
            {"ChST", "+10:00"},
            {"D", "+04:00"},
            {"DAVT", "+07:00"},
            {"E", "+05:00"},
            {"EASST", "-05:00"},
            {"EAST", "-06:00"},
            {"EAT", "+03:00"},
            {"ECT", "-05:00"},
            {"EDT", "-04:00"},
            //{"EDT", "+11:00"}, Pacific Eastern Daylight Time
            {"EEST", "+03:00"},
            {"EET", "+02:00"},
            {"EGST", "00:00"},
            {"EGT", "-01:00"},
            {"EST", "-05:00"},
            {"ET", "-05:00"},
            {"F", "+06:00"},
            {"FJST", "+13:00"},
            {"FJT", "+12:00"},
            {"FKST", "-03:00"},
            {"FKT", "-04:00"},
            {"FNT", "-02:00"},
            {"G", "+07:00"},
            {"GALT", "-06:00"},
            {"GAMT", "-09:00"},
            {"GET", "+04:00"},
            {"GFT", "-03:00"},
            {"GILT", "+12:00"},
            {"GMT", "00:00"},
            {"GST", "+04:00"},
            {"GYT", "-04:00"},
            {"H", "+08:00"},
            {"HAA", "-03:00"},
            {"HAC", "-05:00"},
            {"HADT", "-09:00"},
            {"HAE", "-04:00"},
            {"HAP", "-07:00"},
            {"HAR", "-06:00"},
            {"HAST", "-10:00"},
            {"HAT", "-02:30"},
            {"HAY", "-08:00"},
            {"HKT", "+08:00"},
            {"HLV", "-04:30"},
            {"HNA", "-04:00"},
            {"HNC", "-06:00"},
            {"HNE", "-05:00"},
            {"HNP", "-08:00"},
            {"HNR", "-07:00"},
            {"HNT", "-03:30"},
            {"HNY", "-09:00"},
            {"HOVT", "+07:00"},
            {"I", "+09:00"},
            {"ICT", "+07:00"},
            {"IDT", "+03:00"},
            {"IOT", "+06:00"},
            {"IRDT", "+04:30"},
            {"IRKST", "+09:00"},
            {"IRKT", "+09:00"},
            {"IRST", "+03:30"},
            {"IST", "+02:00"},
            //{"IST", "+05:30"}, India Standard Time
            //{"IST", "+01:00"}, Irish Standard Time
            {"JST", "+09:00"},
            {"K", "+10:00"},
            {"KGT", "+06:00"},
            {"KRAST", "+08:00"},
            {"KRAT", "+08:00"},
            {"KST", "+09:00"},
            {"KUYT", "+04:00"},
            {"L", "+11:00"},
            {"LHDT", "+11:00"},
            {"LHST", "+10:30"},
            {"LINT", "+14:00"},
            {"M", "+12:00"},
            {"MAGST", "+12:00"},
            {"MAGT", "+12:00"},
            {"MART", "-09:30"},
            {"MAWT", "+05:00"},
            {"MDT", "-06:00"},
            {"MESZ", "+02:00"},
            {"MEZ", "+01:00"},
            {"MHT", "+12:00"},
            {"MMT", "+06:30"},
            {"MSD", "+04:00"},
            {"MSK", "+04:00"},
            {"MST", "-07:00"},
            {"MUT", "+04:00"},
            {"MVT", "+05:00"},
            {"MYT", "+08:00"},
            {"N", "-01:00"},
            {"NCT", "+11:00"},
            {"NDT", "-02:30"},
            {"NFT", "+11:30"},
            {"NOVST", "+07:00"},
            {"NOVT", "+06:00"},
            {"NPT", "+05:45"},
            {"NST", "-03:30"},
            {"NUT", "-11:00"},
            {"NZDT", "+13:00"},
            {"NZST", "+12:00"},
            {"O", "-02:00"},
            {"OMSST", "+07:00"},
            {"OMST", "+07:00"},
            {"P", "-03:00"},
            {"PDT", "-07:00"},
            {"PET", "-05:00"},
            {"PETST", "+12:00"},
            {"PETT", "+12:00"},
            {"PGT", "+10:00"},
            {"PHOT", "+13:00"},
            {"PHT", "+08:00"},
            {"PKT", "+05:00"},
            {"PMDT", "-02:00"},
            {"PMST", "-03:00"},
            {"PONT", "+11:00"},
            {"PST", "-08:00"},
            {"PT", "-08:00"},
            {"PWT", "+9:00"},
            {"PYST", "-03:00"},
            {"PYT", "-04:00"},
            {"Q", "-04:00"},
            {"R", "-05:00"},
            {"RET", "+04:00"},
            {"S", "-06:00"},
            {"SAMT", "+04:00"},
            {"SAST", "+02:00"},
            {"SBT", "+11:00"},
            {"SCT", "+04:00"},
            {"SGT", "+08:00"},
            {"SRT", "-03:00"},
            {"SST", "-11:00"},
            {"T", "-07:00"},
            {"TAHT", "-10:00"},
            {"TFT", "+05:00"},
            {"TJT", "+05:00"},
            {"TKT", "+13:00"},
            {"TLT", "+09:00"},
            {"TMT", "+05:00"},
            {"TVT", "+12:00"},
            {"U", "-08:00"},
            {"ULAT", "+08:00"},
            {"UTC", "00:00"},
            {"UYST", "-02:00"},
            {"UYT", "-03:00"},
            {"UZT", "+05:00"},
            {"V", "-09:00"},
            {"VET", "-04:30"},
            {"VLAST", "+11:00"},
            {"VLAT", "+11:00"},
            {"VUT", "+11:00"},
            {"W", "-10:00"},
            {"WAST", "+02:00"},
            {"WAT", "+01:00"},
            {"WEST", "+01:00"},
            {"WESZ", "+01:00"},
            {"WET", "00:00"},
            {"WFT", "+12:00"},
            {"WGST", "-02:00"},
            {"WGT", "-03:00"},
            {"WIB", "+07:00"},
            {"WIT", "+09:00"},
            {"WITA", "+08:00"},
            {"WST", "+1:00"},
            //{"WST", "+13:00"}, West Samoa Time
            {"WT", "00:00"},
            {"X", "-11:00"},
            {"Y", "-12:00"},
            {"YAKST", "+10:00"},
            {"YAKT", "+10:00"},
            {"YAPT", "+10:00"},
            {"YEKST", "+06:00"},
            {"YEKT", "+06:00"},
            {"Z", "00:00"}
        };

        #endregion

        public enum LogLevel
        {
            SEVERE,
            INFO,
            WARNING
        }

        public static DateTime lastUpdated;
        public static bool refreshRequested;
        public static bool weatherWidgetEnabled = true;
        
        public static List<string> subDirectoriesFound = new List<string>();

        #endregion

        #region Database Methods

        /// <summary>
        /// Saves a city to a local SQLite 3 database.  
        /// </summary>
        /// <param name="cityName">The name of the city</param>
        /// <param name="countryName">The name of the country</param>
        /// <param name="countryCode">The country's corresponding two-letter country code</param>
        /// <param name="regionName">The name of the region in which the city is located</param>
        /// <param name="regionCode">The region's corresponding two-letter region code</param>
        /// <param name="latitude">The line of latitude value</param>
        /// <param name="longitude">The line of longitude value</param>
        /// <returns>An <see cref="int"/> value indicating success or failure.<br /> 1 for success and 0 for failure</returns>
        public static int AddCityToDatabase(string cityName, string countryName, string countryCode,
         string regionName, string regionCode, float latitude, float longitude)
        {
            int affected_rows = 0;
            string insertSQL = "INSERT INTO WorldCities.world_cities ( CityName, CountryName, CountryCode,"
                    + " RegionName, RegionCode, Latitude, Longitude, DateAdded ) VALUES" +
                    " ( @city, @country, @code, @region, @rcode, @lat, @lon, @date )";

            try
            {
                using (SQLiteCommand comm = WeatherLionMain.conn.CreateCommand())
                {
                    comm.CommandText = insertSQL;
                    comm.Parameters.Add("@city", DbType.String);
                    comm.Parameters["@city"].Value = cityName;

                    comm.Parameters.Add("@country", DbType.String);
                    comm.Parameters["@country"].Value = countryName;

                    comm.Parameters.Add("@code", DbType.String);
                    comm.Parameters["@code"].Value = countryCode;

                    comm.Parameters.Add("@region", DbType.String);
                    comm.Parameters["@region"].Value = regionName;

                    comm.Parameters.Add("@rcode", DbType.String);
                    comm.Parameters["@rcode"].Value = regionCode;

                    comm.Parameters.Add("@lat", DbType.Decimal);
                    comm.Parameters["@lat"].Value = latitude;

                    comm.Parameters.Add("@lon", DbType.Decimal);
                    comm.Parameters["@lon"].Value = longitude;

                    comm.Parameters.Add("@date", DbType.String);
                    comm.Parameters["@date"].Value = string.Format("{0:ddd, MMM, dd, yyyy h:mm tt}", DateTime.UtcNow);

                    IDataReader iDr = comm.ExecuteReader();

                    affected_rows = iDr.RecordsAffected;

                }// end of using block             

                return affected_rows;
            }// end of try block
            catch (Exception)
            {
                return 0;
            }// end of catch block
        }// end of method AddCityToDatabase

        /// <summary>
        /// Adds a web service account key to a local SQLite 3 database.  
        /// </summary>
        /// <param name="keyProvider">The provider of the access key</param>
        /// <param name="keyName">The name of the access key</param>
        /// <param name="keyValue">The access key string (encrypted)</param>
        /// <param name="hexValue">A hex key</param>        
        /// <returns>An <see cref="int"/> value indicating success or failure.<br /> 1 for success and 0 for failure</returns>
        public static int AddSiteKeyToDatabase(string keyProvider, string keyName, string keyValue, string hexValue)
        {
            // Redundancy check
            if (!CheckIfTableExists("wak", "access_keys"))
            {
                string tableSQL =
                        "CREATE table IF NOT EXISTS wak.access_keys "
                        + "(KeyProvider TEXT, KeyName TEXT, KeyValue TEXT(64))";

                try
                {
                    SQLiteConnection db = new SQLiteConnection($"Data Source={WeatherLionMain.conn};FailIfMissing=True;");

                    using (SQLiteCommand comm = db.CreateCommand())
                    {
                        comm.CommandText = tableSQL;
                        IDataReader dr = comm.ExecuteReader();
                    }// end of using block

                }// end of try block
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }// end of catch block
                finally
                {
                    WeatherLionMain.conn.Close();
                }// end of finally block

            }// end of if block

            int affected_rows = 0;
            string insertSQL =
                $"INSERT INTO wak.access_keys ( KeyProvider, KeyName, KeyValue, Hex) VALUES ( @provider, @kn, @kv, @hex )";

            try
            {
                using (SQLiteCommand comm = WeatherLionMain.conn.CreateCommand())
                {
                    comm.CommandText = insertSQL;
                    comm.Parameters.Add("@provider", DbType.String);
                    comm.Parameters["@provider"].Value = keyProvider;

                    comm.Parameters.Add("@kn", DbType.String);
                    comm.Parameters["@kn"].Value = keyName;

                    comm.Parameters.Add("@kv", DbType.String);
                    comm.Parameters["@kv"].Value = keyValue;

                    comm.Parameters.Add("@hex", DbType.String);
                    comm.Parameters["@hex"].Value = hexValue;

                    IDataReader iDr = comm.ExecuteReader();

                    affected_rows = iDr.RecordsAffected;
                }// end of using block

                return affected_rows;
            }// end of try block
            catch (SQLiteException e)
            {
                Console.WriteLine("SQLite Error: " + e.Message);
                return 0;
            }// end of catch block
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }// end of second catch block}
        }// end of method AddSiteKeyToDatabase

        /// <summary>
        /// Attach a database to another SQLite 3 database.  
        /// </summary>
        /// <param name="dbName">The name of the database</param>
        /// <param name="alias">An alias for the table</param>
        /// <returns>An <see cref="int"/> value indicating success or failure.<br /> 1 for success and 0 for failure</returns>
        public static int AttachDatabase(string dbName, string alias)
        {
            //string attachSQL =
            //         "ATTACH DATABASE '" + dbName + "' as " + alias + "";

            string attachSQL ="ATTACH DATABASE '@storage' as @nickname";
          
            try
            {
                WeatherLionMain.conn.Open();

                using (SQLiteCommand comm = WeatherLionMain.conn.CreateCommand())
                {
                    comm.CommandText = attachSQL;

                    comm.Parameters.Add("@storage", DbType.String);
                    comm.Parameters["@storage"].Value = dbName;

                    comm.Parameters.Add("@nickname", DbType.String);
                    comm.Parameters["@nickname"].Value = alias;

                    IDataReader dr = comm.ExecuteReader();
                    return 1;
                }// end of using block

            }// end of try block
            catch (Exception)
            {
                return 0;
            }// end of catch block
            finally
            {
                WeatherLionMain.conn.Close();
            }// end of finally block
        }// end of method AttachDatabase

        /// <summary>
        /// Attach a database to another SQLite 3 database.  
        /// </summary>
        /// <param name="dbName">The name of the database</param>
        /// <param name="tableName">The name of the table</param>
        /// <param name="columnNames">An array representing the columns</param>
        /// <param name="columnValues">An array representing the column values</param>
        /// <returns>A <see cref="bool"/> value indicating success or failure.</returns>
        public static bool CheckIfRecordExist(string dbName, string tableName,
            string[] columnNames, string[] columnValues)
        {
            bool exists = false;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < columnNames.Length; i++)
            {
                sb.Append($"{columnNames[i]}='{columnValues[i]}'");

                if (i != columnNames.Length - 1)
                {
                    sb.Append(" AND ");
                }// end of if block
            }// end of for loop

            string SQL = $"SELECT * FROM {dbName}.{tableName} WHERE {sb.ToString()}";

            try
            {
                using (SQLiteCommand comm = WeatherLionMain.conn.CreateCommand())
                {
                    comm.CommandText = SQL;
                    IDataReader dr = comm.ExecuteReader();

                    while (dr.Read())
                    {
                        exists = true;
                    }// end of while loop
                }// end of using block

            }// end of try block
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }// end of catch block

            return exists;
        }// end of method CheckIfRecordExist

        /// <summary>
        /// Attach a database to another SQLite 3 database.  
        /// </summary>
        /// <param name="dbName">The name of the database</param>
        /// <param name="tableName">The name of the table</param>
        /// <returns>A <see cref="bool"/> value indicating success or failure.</returns>
        public static bool CheckIfTableExists(string dbName, string tableName)
        {
            int affected_rows = 0;
            string fullyQualifiedDBName = $"{dbName}.sqlite_master";
            string SQL = $"SELECT name FROM {fullyQualifiedDBName} WHERE type='table' AND name=@table";

            try
            {
                using (SQLiteCommand comm = WeatherLionMain.conn.CreateCommand())
                {
                    comm.CommandText = SQL;
                    comm.Parameters.Add("@table", DbType.String);
                    comm.Parameters["@table"].Value = tableName;
                    IDataReader dr = comm.ExecuteReader();

                    while (dr.Read())
                    {
                        affected_rows++;
                    }// end of while loop
                }// end of using block

            }// end of try block
            catch (Exception e)
            {
                LogMessage(LogLevel.SEVERE, e.Message,
                    $"{nameof(WeatherLion)}::init [line: {GetExceptionLineNumber(e)}]");
            }// end of catch block

            return affected_rows > 0;
        }// end of method CheckIfTableExists

        /// <summary>
        /// Creates the world cities SQLite 3 database.  
        /// </summary>
        public static int CreateWorldCitiesDatabase()
        {
            if (WeatherLionMain.conn == null)
            {
                WeatherLionMain.conn = ConnectionManager.GetInstance().GetConnection();                
            }// end of if block

            int success = 0;
            string worldCitiesTable =
                      "CREATE TABLE WorldCities.world_cities (CityName TEXT, CountryName TEXT, CountryCode TEXT (2), "
                              + "RegionName TEXT, RegionCode TEXT (2), Latitude REAL, Longitude REAL, DateAdded TEXT)";

            try
            {
                using (SQLiteCommand comm = WeatherLionMain.conn.CreateCommand())
                {
                    comm.CommandText = worldCitiesTable;
                    IDataReader dr = comm.ExecuteReader();

                    success = 1;
                }// end of using block
            }// end of try block
            catch (Exception e)
            {
                LogMessage(LogLevel.SEVERE, e.Message, 
                    $"{TAG}::CreateWorldCitiesDatabase [line: {GetExceptionLineNumber(e)}]");
            }// end of catch block

            return success;
        }// end of method CreateWorldCitiesDatabase

        /// <summary>
        /// Creates the access keys SQLite 3 database.  
        /// </summary>
        public static int CreateWSADatabase()
        {
            int success = 0;

            string siteAccessTable =
                    "CREATE TABLE wak.access_keys (KeyProvider TEXT, KeyName TEXT, KeyValue TEXT(64), Hex TEXT)";

            try
            {
                using (SQLiteCommand comm = WeatherLionMain.conn.CreateCommand())
                {
                    comm.CommandText = siteAccessTable;
                    IDataReader dr = comm.ExecuteReader();

                    success = 1;
                }// end of using block

            }// end of try block
            catch (Exception e)
            {
                LogMessage(LogLevel.SEVERE, e.Message,
                    $"{TAG}::CreateWSADatabase [line: {GetExceptionLineNumber(e)}]");
            }// end of catch block

            return success;
        }// end of method CreateWSADatabase

        /// <summary>
        /// Removes a key that is stored in the local database
        /// </summary>
        /// <param name="keyProvider">The name of the web service that supplies the key</param>
        /// <param name="keyName">The name of the key</param>
        /// <returns><para>An <see cref="int"/> value indicating success or failure.</para>1 for success and 0 for failure.</returns>
        public static int DeleteSiteKeyFromDatabase(string keyProvider, string keyName)
        {
            int affected_rows = 0;
            string SQL = "DELETE FROM access_keys WHERE KeyProvider = @provider AND keyName = @key";

            try
            {
                using (SQLiteCommand comm = WeatherLionMain.conn.CreateCommand())
                {
                    comm.CommandText = SQL;
                    comm.Parameters.Add("@provider", DbType.String);
                    comm.Parameters["@provider"].Value = keyProvider;

                    comm.Parameters.Add("@key", DbType.String);
                    comm.Parameters["@key"].Value = keyName;                          

                    IDataReader iDr = comm.ExecuteReader();
                    affected_rows = iDr.RecordsAffected;
                }// end of using block              

                return affected_rows;
            }// end of try block
            catch (Exception e)
            {
                LogMessage(LogLevel.SEVERE, e.Message,
                    $"{TAG}::DeleteSiteKeyFromDatabase [line: {GetExceptionLineNumber(e)}]");

                return 0;
            }// end of catch block
        }// end of method DeleteSiteKeyFromDatabase

        /// <summary>
        /// Retrieves a city from a local SQLite 3 database
        /// </summary>
        /// <param name="cityName">The name of the city</param>
        /// <param name="regionCode">The region's corresponding two-letter region code</param>
        /// <param name="countryName">The name of the country</param>
        /// <returns>An <see cref="object"/> of the <see cref="CityData"/> custom class</returns>
        public static CityData GetCityDataFromDatabase(string cityName, string regionCode, string countryName)
        {
            string SQL;
            int found = 0;

            if (regionCode != null && countryName != null)
            {
                SQL = "SELECT CityName, CountryName, CountryCode, RegionCode, Latitude,"
                        + " Longitude FROM WorldCities.world_cities WHERE CityName=@city AND RegionCode=@region AND CountryName=@country";
            }// end of if block
            else if (regionCode != null && countryName == null)
            {
                SQL = "SELECT CityName, CountryName, CountryCode, RegionCode, Latitude,"
                        + " Longitude FROM WorldCities.world_cities WHERE CityName=@city AND RegionCode=@region";
            }// end of else if block
            else if (regionCode == null && countryName != null)
            {
                SQL = "SELECT CityName, CountryName, CountryCode, RegionCode, Latitude"
                        + ", Longitude FROM WorldCities.world_cities WHERE CityName=@city AND CountryName=@country";
            }// end of else if block
            else
            {
                SQL = "SELECT CityName, CountryName, CountryCode, RegionCode, Latitude,"
                        + " Longitude FROM WorldCities.world_cities WHERE CityName=@city";
            }// end of else block
             
            try
		    {
                using (SQLiteCommand comm = WeatherLionMain.conn.CreateCommand())
                {
                    if (regionCode != null && countryName != null)
                    {        
                        comm.Parameters.Add("@city", DbType.String);
                        comm.Parameters["@city"].Value = cityName;

                        comm.Parameters.Add("@region", DbType.String);
                        comm.Parameters["@region"].Value = regionCode;

                        comm.Parameters.Add("@country", DbType.String);
                        comm.Parameters["@country"].Value = countryName;
                    }// end of if block
                    else if (regionCode != null && countryName == null)
                    {
                        comm.Parameters.Add("@city", DbType.String);
                        comm.Parameters["@city"].Value = cityName;

                        comm.Parameters.Add("@region", DbType.String);
                        comm.Parameters["@region"].Value = regionCode;
                    }// end of else if block
                    else if (regionCode == null && countryName != null)
                    {
                        comm.Parameters.Add("@city", DbType.String);
                        comm.Parameters["@city"].Value = cityName;

                        comm.Parameters.Add("@country", DbType.String);
                        comm.Parameters["@country"].Value = countryName;
                    }// end of else if block
                    else
                    {
                        comm.Parameters.Add("@city", DbType.String);
                        comm.Parameters["@city"].Value = cityName;
                    }// end of else block
                    
                    comm.CommandText = SQL;
                    IDataReader dr = comm.ExecuteReader();

                    while (dr.Read())
                    {
                        found++;
                    }// end of while loop

                    if (found > 0)
                    {
                        return new CityData(dr.GetString(0), dr.GetString(1), dr.GetString(2),
                                dr.GetString(3), dr.GetFloat(4), dr.GetFloat(5));
                    }// end of if block
                    else
                    {
                        return null;
                    }// end of else block
                }// end of using block             
            }// end of try block
            catch (Exception e)
            {
                LogMessage(LogLevel.SEVERE, e.Message,
                    $"{TAG}::GetCityDataFromDatabase [line: {GetExceptionLineNumber(e)}]");

                return null;
            }// end of catch block
        }// end of method GetCityDataFromDatabase

        #endregion

        #region Weather Calculations

        /// <summary>
        /// Calculate the wind chill for temperatures at or below 50° F and wind speeds above 3 mph  
        /// </summary>
        /// <param name="fTemp"> A temperature measured in Fahrenheit</param>
        /// <param name="mphWind"> A wind speed measure in miles per hour (mph)</param>
        /// <remarks>The fTemp value must be an integer</remarks>
        /// <remarks>The mphWind value must be an integer</remarks>
        /// <returns>A integer value representing the calculated wind chill.</returns>
        public static int CalculateWindChill(int fTemp, int mphWind)
        {
            // The wind chill calculator only works for temperatures at or below 50 ° F
            // and wind speeds above 3 mph.
            if (fTemp > 50 || mphWind < 3)
            {
                return fTemp;
            }// end of if block
            else
            {
                return (int)(35.74 + (0.6215 * fTemp) - (35.75 * Math.Pow(mphWind, 0.16))
                        + (0.4275 * fTemp * Math.Pow(mphWind, 0.16)));
            }// end of else block    	
        }// end of method CalculateWindChill

        /// <summary>
        /// Heat index computed using air temperature F and relative humidity  
        /// </summary>
        /// <param name="airTemp"> The current air temperature reading</param>
        /// <param name="relativeHumidity"> The current relative humidity reading</param>
        /// <returns>A double representing the heat index value.</returns>
        /// <author>Kevin Sharp and Mark Klein</author>
        /// <see cref="https://www.wpc.ncep.noaa.gov/html/heatindex.shtml"/>
        public static double HeatIndex(double airTemp, double relativeHumidity)
        {
            double hi = 0;
            double hiTemp = 0;
            double hiFinal = 0;
            double adj1 = 0;
            double adj2 = 0;
            double adj = 0;

            if (relativeHumidity > 100)
            {
                return 0;
            }// end of if block
            else if (relativeHumidity < 0)
            {
                return 0;
            }//end of else if block
            else if (airTemp <= 40.0)
            {
                hi = airTemp;
            }//end of else if block
            else
            {
                hiTemp = 61.0 + ((airTemp - 68.0) * 1.2) + (relativeHumidity * 0.094);
                hiFinal = 0.5 * (airTemp + hiTemp);

                if (hiFinal > 79.0)
                {
                    hi = -42.379 + 2.04901523
                        * airTemp + 10.14333127
                        * relativeHumidity - 0.22475541
                        * airTemp * relativeHumidity
                        - 6.83783 * (Math.Pow(10, -3))
                        * (Math.Pow(airTemp, 2)) - 5.481717
                        * (Math.Pow(10, -2)) * (Math.Pow(relativeHumidity, 2))
                        + 1.22874 * (Math.Pow(10, -3)) * (Math.Pow(airTemp, 2))
                        * relativeHumidity + 8.5282 * (Math.Pow(10, -4))
                        * airTemp * (Math.Pow(relativeHumidity, 2))
                        - 1.99 * (Math.Pow(10, -6)) * (Math.Pow(airTemp, 2))
                        * (Math.Pow(relativeHumidity, 2));

                    if ((relativeHumidity <= 13) && (airTemp >= 80.0)
                            && (airTemp <= 112.0))
                    {
                        adj1 = (13.0 - relativeHumidity) / 4.0;
                        adj2 = Math.Sqrt((17.0 - Math.Abs(airTemp - 95.0)) / 17.0);
                        adj = adj1 * adj2;
                        hi = hi - adj;
                    }// end of if block
                    else if ((relativeHumidity > 85.0) && (airTemp >= 80.0)
                        && (airTemp <= 87.0))
                    {
                        adj1 = (relativeHumidity - 85.0) / 10.0;
                        adj2 = (87.0 - airTemp) / 5.0;
                        adj = adj1 * adj2;
                        hi = hi + adj;
                    }// end of else if block
                }// end of if block
                else
                {
                    hi = hiFinal;
                }// end of else block
            }// end of else block

            double tempc2 = (airTemp - 32) * .556;
            double rh2 = 1 - relativeHumidity / 100;
            double tdpc2 = tempc2 - (((14.55 + .114 * tempc2) * rh2)
                    + (Math.Pow(((2.5 + .007 * tempc2) * rh2), 3))
                    + ((15.9 + .117 * tempc2)) * (Math.Pow(rh2, 14)));

            return Math.Round(hi);
        }// end of method HeatIndex

        /// <summary>
        /// Heat index computed using air temperature and dew point temperature. Degrees F  
        /// </summary>
        /// <param name="airTemp"> The current air temperature reading</param>
        /// <param name="dewPoint"> The current dew point reading</param>
        /// <returns>A double representing the heat index value.</returns>
        /// <author>Kevin Sharp and Mark Klein</author>
        /// <see cref="https://www.wpc.ncep.noaa.gov/html/heatindex.shtml"/>
        public static double HeatIndexDew(double airTemp, double dewPoint)
        {
            double hi = 0;
            double vaporPressure = 0;
            double satVaporPressure = 0;
            double relativeHumidity = 0;
            double hiTemp = 0;
            double hiFinal = 0;
            double adj1 = 0;
            double adj2 = 0;
            double adj = 0;

            double tc2 = (airTemp - 32) * .556;
            double tdc2 = (dewPoint - 32) * .556;

            if (tc2 < tdc2)
            {
                return 0;
            }// end of if block
            else if (airTemp <= 40.0)
            {
                hi = airTemp;
            }// end of else if block
            else
            {
                vaporPressure = 6.11 * (Math.Pow(10, 7.5 * (tdc2 / (237.7 + tdc2))));
                satVaporPressure = 6.11 * (Math.Pow(10, 7.5 * (tc2 / (237.7 + tc2))));
                relativeHumidity = Math.Round(100.0 * (vaporPressure / satVaporPressure));
                hiTemp = 61.0 + ((airTemp - 68.0) * 1.2) + (relativeHumidity * 0.094);
                hiFinal = 0.5 * (airTemp + hiTemp);

                if (hiFinal > 79.0)
                {
                    hi = -42.379 + 2.04901523 * airTemp
                        + 10.14333127 * relativeHumidity
                        - 0.22475541 * airTemp
                        * relativeHumidity - 6.83783
                        * (Math.Pow(10, -3)) * (Math.Pow(airTemp, 2))
                        - 5.481717 * (Math.Pow(10, -2))
                        * (Math.Pow(relativeHumidity, 2))
                        + 1.22874 * (Math.Pow(10, -3))
                        * (Math.Pow(airTemp, 2)) * relativeHumidity
                        + 8.5282 * (Math.Pow(10, -4))
                        * airTemp * (Math.Pow(relativeHumidity, 2))
                        - 1.99 * (Math.Pow(10, -6))
                        * (Math.Pow(airTemp, 2)) * (Math.Pow(relativeHumidity, 2));

                    if ((relativeHumidity <= 13.0) && (airTemp >= 80.0)
                        && (airTemp <= 112.0))
                    {
                        adj1 = (13.0 - relativeHumidity) / 4.0;
                        adj2 = Math.Sqrt((17.0 - Math.Abs(airTemp - 95.0)) / 17.0);
                        adj = adj1 * adj2;
                        hi = hi - adj;
                    }// end of if block
                    else if ((relativeHumidity > 85.0) && (airTemp >= 80.0)
                         && (airTemp <= 87.0))
                    {
                        adj1 = (relativeHumidity - 85.0) / 10.0;
                        adj2 = (87.0 - airTemp) / 5.0;
                        adj = adj1 * adj2;
                        hi = hi + adj;
                    }// end of else if block 
                }// end of if block
                else
                {
                    hi = hiFinal;
                }// end of else block
            }// end of else block

            string answer = Math.Round(hi) + " F" + " / "
                    + Math.Round((hi - 32) * .556) + " C";
            string relativeHumidityS = relativeHumidity + "%";

            return Math.Round(hi);
        }// end of method HeatIndexDew

        /// <summary>
        /// Heat index computed using air temperature C and relative humidity
        /// </summary>
        /// <param name="airTempCelsius"> The current air temperature reading</param>
        /// <param name="relativeHumidity">The current relative humidity reading</param>
        /// <returns>A double representing the heat index value.</returns>
        /// <author>Kevin Sharp and Mark Klein</author>
        /// <see cref="https://www.wpc.ncep.noaa.gov/html/heatindex.shtml"/>
        public static double HeatIndexCelsius(double airTempCelsius, double relativeHumidity)
        {
            double hi = 0;
            double tempAirInFahrenheit = 0;
            double hiTemp = 0;
            double fpTemp = 0;
            double hiFinal = 0;
            double adj1 = 0;
            double adj2 = 0;
            double adj = adj1 * adj2;

            if (relativeHumidity > 100)
            {
                return 0;
            }// end of if block
            else if (relativeHumidity < 0)
            {
                return 0;
            }// end of else if block
            else if (airTempCelsius <= 4.44)
            {
                hi = airTempCelsius;
            }// end of else if block
            else
            {
                tempAirInFahrenheit = 1.80 * airTempCelsius + 32.0;
                hiTemp = 61.0 + ((tempAirInFahrenheit - 68.0) * 1.2) + (relativeHumidity * 0.094);
                fpTemp = airTempCelsius;
                hiFinal = 0.5 * (fpTemp + hiTemp);

                if (hiFinal > 79.0)
                {
                    hi = -42.379 + 2.04901523
                        * tempAirInFahrenheit + 10.14333127
                        * relativeHumidity - 0.22475541
                        * tempAirInFahrenheit * relativeHumidity
                        - 6.83783 * (Math.Pow(10, -3))
                        * (Math.Pow(tempAirInFahrenheit, 2))
                        - 5.481717 * (Math.Pow(10, -2))
                        * (Math.Pow(relativeHumidity, 2))
                        + 1.22874 * (Math.Pow(10, -3))
                        * (Math.Pow(tempAirInFahrenheit, 2))
                        * relativeHumidity + 8.5282
                        * (Math.Pow(10, -4)) * tempAirInFahrenheit
                        * (Math.Pow(relativeHumidity, 2))
                        - 1.99 * (Math.Pow(10, -6))
                        * (Math.Pow(tempAirInFahrenheit, 2))
                        * (Math.Pow(relativeHumidity, 2));

                    if ((relativeHumidity <= 13) && (tempAirInFahrenheit >= 80.0)
                            && (tempAirInFahrenheit <= 112.0))
                    {
                        adj1 = (13.0 - relativeHumidity) / 4.0;
                        adj2 = Math.Sqrt((17.0 - Math.Abs(tempAirInFahrenheit - 95.0)) / 17.0);
                        adj = adj1 * adj2;
                        hi = hi - adj;
                    }// end of if block
                    else if ((relativeHumidity > 85.0) && (tempAirInFahrenheit >= 80.0)
                            && (tempAirInFahrenheit <= 87.0))
                    {
                        adj1 = (relativeHumidity - 85.0) / 10.0;
                        adj2 = (87.0 - tempAirInFahrenheit) / 5.0;
                        adj = adj1 * adj2;
                        hi = hi + adj;
                    }// end of else if block
                }// end of if block
                else
                {
                    hi = hiFinal;
                }// end of else block
            }

            string heatIndexS = Math.Round(hi) + " F"
            + " / " + Math.Round((hi - 32) * .556) + " C";
            double rh3 = 1 - relativeHumidity / 100;
            double tdpc3 = airTempCelsius - (((14.55 + .114 * airTempCelsius) * rh3)
                    + (Math.Pow(((2.5 + .007 * airTempCelsius) * rh3), 3))
                    + ((15.9 + .117 * airTempCelsius)) * (Math.Pow(rh3, 14)));
            string dewpt = Math.Round(1.80 * tdpc3 + 32.0)
                    + " F" + " / " + Math.Round(tdpc3) + " C";

            return Math.Round((hi - 32) * .556);
        }// end of method HeatIndexCelsius

        /// <summary>
        /// Heat index computed using air temperature and dew point temperature. Degrees C
        /// </summary>
        /// <param name="airTempCelsius"> The current air temperature reading</param>
        /// <param name="dewPointCelsius">The current dew point reading</param>
        /// <returns>A double representing the heat index value.</returns>
        /// <author>Kevin Sharp and Mark Klein</author>
        /// <see cref="https://www.wpc.ncep.noaa.gov/html/heatindex.shtml"/>
        public static double HeatIndexDewCelsius(double airTempCelsius, double dewPointCelsius)
        {
            double hi = 0;
            double vaporPressure = 0;
            double satVaporPressure = 0;
            double relativeHumidity = 0;
            double airTempInFahrenheit = 0;
            double hiTemp = 0;
            double fpTemp = 0;
            double hiFinal = 0;
            double adj1 = 0;
            double adj2 = 0;
            double adj = 0;

            if (airTempCelsius < dewPointCelsius)
            {
                return 0;
            }// end of if block
            else if (airTempCelsius <= 4.44)
            {
                hi = airTempCelsius;
            }// end of else if block
            else
            {
                vaporPressure = 6.11 * (Math.Pow(10, 7.5 *
                               (dewPointCelsius / (237.7 + dewPointCelsius))));
                satVaporPressure = 6.11 * (Math.Pow(10, 7.5 *
                              (airTempCelsius / (237.7 + airTempCelsius))));
                relativeHumidity = Math.Round(100.0 * (vaporPressure / satVaporPressure));
                airTempInFahrenheit = 1.80 * airTempCelsius + 32.0;
                hiTemp = 61.0 + ((airTempInFahrenheit - 68.0) * 1.2)
                        + (relativeHumidity * 0.094);
                fpTemp = airTempInFahrenheit;
                hiFinal = 0.5 * (fpTemp + hiTemp);

                if (hiFinal > 79.0)
                {
                    hi = -42.379 + 2.04901523 * airTempInFahrenheit
                         + 10.14333127 * relativeHumidity - 0.22475541
                         * airTempInFahrenheit * relativeHumidity
                         - 6.83783 * (Math.Pow(10, -3))
                         * (Math.Pow(airTempInFahrenheit, 2))
                         - 5.481717 * (Math.Pow(10, -2))
                         * (Math.Pow(relativeHumidity, 2))
                         + 1.22874 * (Math.Pow(10, -3))
                         * (Math.Pow(airTempInFahrenheit, 2))
                         * relativeHumidity + 8.5282
                         * (Math.Pow(10, -4))
                         * airTempInFahrenheit * (Math.Pow(relativeHumidity, 2))
                         - 1.99 * (Math.Pow(10, -6)) * (Math.Pow(airTempInFahrenheit, 2))
                         * (Math.Pow(relativeHumidity, 2));

                    if ((relativeHumidity <= 13.0) && (airTempInFahrenheit >= 80.0)
                        && (airTempInFahrenheit <= 112.0))
                    {
                        adj1 = (13.0 - relativeHumidity) / 4.0;
                        adj2 = Math.Sqrt((17.0 - Math.Abs(airTempInFahrenheit - 95.0)) / 17.0);
                        adj = adj1 * adj2;
                        hi = hi - adj;
                    }// end of if block
                    else if ((relativeHumidity > 85.0) && (airTempInFahrenheit >= 80.0)
                        && (airTempInFahrenheit <= 87.0))
                    {
                        adj1 = (relativeHumidity - 85.0) / 10.0;
                        adj2 = (87.0 - airTempInFahrenheit) / 5.0;
                        adj = adj1 * adj2;
                        hi = hi + adj;
                    }// end of else if block
                }// end of if block
                else
                {
                    hi = hiFinal;
                }// end of else block
            }// end of else block

            string heatDewCelsius =
                   Math.Round(hi) + " F" + " / " + Math.Round((hi - 32) * .556) + " C";
            string heatDewCelsiusRelativeHumidity = relativeHumidity + "%";

            return Math.Round((hi - 32) * .556);
        }// end of method HeatIndexDewPointCelsius

        #endregion
                
        #region Unit Conversions

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a temperature in Celsius and converts it to Fahrenheit.
        /// </summary>
        /// <param name="celsius">The temperature in Celsius</param>
        /// <remarks>The value must be a floating point number.</remarks>
        /// <returns>The converted value in Fahrenheit.</returns>
        public static float CelsiusToFahrenheit(float celsius)
        {
            return (float)(celsius * 1.8 + 32);
        }// end of method CelsiusToFahrenheit

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a temperature in Celsius and converts it to Kelvin.
        /// </summary>
        /// <param name="celsius">The temperature in Celsius</param>
        /// <remarks>The value must be a floating point number.</remarks>
        /// <returns>The converted value in Kelvin.</returns>
        public static double CelsiusToKelvin(float celsius)
        {
            return celsius + 273.15;
        }// end of method CelsiusToKelvin

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a temperature in Fahrenheit and converts it to Celsius.
        /// </summary>
        /// <param name="fahrenheit">The temperature in Fahrenheit</param>
        /// <remarks>The value must be a floating point number.</remarks>
        /// <returns>The converted value in Celsius.</returns>
        public static float FahrenheitToCelsius(float fahrenheit)
        {
            float celsius = (float) Math.Round((fahrenheit - 32) / 1.8);

            return float.Parse(celsius.ToString("0.##"), CultureInfo.InvariantCulture.NumberFormat);
        }// end of method FahrenheitToCelsius

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a temperature in Fahrenheit and converts it to Kelvin.
        /// </summary>
        /// <param name="fahrenheit">The temperature in Fahrenheit</param>
        /// <remarks>The value must be a floating point number.</remarks>
        /// <returns>The converted value in Kelvin.</returns>
        public static float FahrenheitToKelvin(float fahrenheit)
        {
            float kelvin = (float) Math.Round((fahrenheit + 459.67) * 0.5555555555555556);

            return float.Parse(kelvin.ToString("0.##"), CultureInfo.InvariantCulture.NumberFormat);
        }// end of method FahrenheitToKelvin

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a temperature in Kelvin and converts it to Celsius.
        /// </summary>
        /// <param name="kelvin">The temperature in Fahrenheit</param>
        /// <remarks>The value must be a floating point number.</remarks>
        /// <returns>The converted value in Celsius.</returns>
        public static float KelvinToCelsius(float kelvin)
        {
            float celsius = (float) Math.Round(kelvin - 273.15);

            return float.Parse(celsius.ToString("0.##"), CultureInfo.InvariantCulture.NumberFormat);
        }// end of method KelvinToCelsius

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a temperature in Kelvin and converts it to Celsius.
        /// </summary>
        /// <param name="kelvin">The temperature in Fahrenheit</param>
        /// <remarks>The value must be a floating point number.</remarks>
        /// <returns>The converted value in Fahrenheit.</returns>
        public static float KelvinToFahrenheit(float kelvin)
        {
            float fahrenheit = (float) Math.Round(kelvin * 1.8 - 459.67);

            return float.Parse(fahrenheit.ToString("0.##"), CultureInfo.InvariantCulture.NumberFormat);
        }// end of method KelvinToFahrenheit

        /// <summary>
        /// Converts milliseconds to minutes  
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds to be converted</param>
        /// <remarks>The value must be an integer number.</remarks>
        /// <returns>The converted time value.</returns>
        public static int MillisecondsToMinutes(int milliseconds)
        {
            return milliseconds / 60000;
        }// end of method MillisecondsToMinutes

        /// <summary>
        /// Converts a minute time value to milliseconds.  
        /// </summary>
        /// <param name="minutes">The number of minutes to be converted</param>
        /// <remarks>The value must be an integer number.</remarks>
        /// <returns>The converted time value.</returns>
        public static int MinutesToMilliseconds(int minutes)
        {
            return minutes * 60000;
        }// end of method MinutesToMillisesonds

        /// <summary>
        /// Converts a double value into Km/h unit measurement.  
        /// </summary>
        /// <param name="mps">The value to be converted in mps</param>
        /// <remarks>The value must be a double.</remarks>
        /// <returns>The value after the conversion.</returns>
        public static double MpsToKmh(double mps)
        {
            return mps * 3.6;
        }// end of method MpsToKmh

        /// <summary>
        /// Accepts a numeric value of type double that represents  
        /// a rate of speed in mph (Miles per hour) and converts it to km/h (Kilometers per hour).  
        /// </summary>
        /// <param name="mph">The rate of speed in mph (Miles per hour).</param>
        /// <remarks>The value must be a double.</remarks>
        /// <returns>The converted rate of speed value in km/h (Kilometers per hour).</returns>
        public static double MphToKmh(double mph)
        {
            return mph * 1.60934;
        }// end of method MphToKmh

        /// <summary>
        /// Accepts a numeric value of type double that represents  
        /// a rate of speed in kmh (Kilometers per hour) and converts it to mph (Miles per hour).  
        /// </summary>
        /// <param name="kmh">The rate of speed in kmh (Kilometers per hour).</param>
        /// <remarks>The value must be a double.</remarks>
        /// <returns>The converted rate of speed value in mph (Miles per hour).</returns>
        public static double KmhToMph(double kmh)
        {
            return kmh * 0.621371;
        }// end of method KmhToMph

        /// <summary>
        /// Converts a value in kilometers per hour to meters per second  
        /// </summary>
        /// <param name="kmh">The rate of speed in kmh (Kilometers per hour).</param>
        /// <remarks>The value must be a double.</remarks>
        /// <returns>The converted value in meters per second.</returns>
        public static double KmhToMps(double kmh)
        {
            return kmh * 0.277778;
        }// end of method KmhToMps

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a rate of speed in Mps (Meters per second) and converts it to Mph (Miles per hour). 
        /// </summary>
        /// <param name="mps">The rate of speed in Mps (Miles per second).</param>
        /// <remarks>The value must be a floating point number.</remarks>
        /// <returns>The converted rate of speed value in Mph (Miles per hour).</returns>
        public static float MpsToMph(float mps)
        {
            float mph = (float) Math.Round(mps * 2.23694);

            return float.Parse(mph.ToString("0.##"), CultureInfo.InvariantCulture.NumberFormat);
        }// end of method MpsToMph

        /// <summary>
        /// Accepts a numeric value of type float that represents
        /// a rate of speed in Mph (Miles per hour) and converts it to Mph (Meters per seconds).
        /// </summary>
        /// <param name="mph">The rate of speed in Mph (Miles per hour).</param>
        /// <returns>The converted rate of speed value in Mph (Meters per seconds).</returns>
        public static float MphToMps(float mph)
        {
            float mps = (float) Math.Round(mph * 0.44704);

            return float.Parse(mps.ToString("0.##"), CultureInfo.InvariantCulture.NumberFormat);
        }// end of method MphToMps

        #endregion

        public static ToolTip controlTip = new ToolTip();

        #region Miscellaneous Methods

        /// <summary>
        /// Adds a tooltip to a control.
        /// </summary>
        /// <param name="c">The control in which the tooltip should be added to.</param>
        /// <param name="tip">The tooltip message.</param>
        public static void AddControlToolTip(Control c, string tip)
        {
            // Set image tooltip to current condition string
            c.Invoke((MethodInvoker)delegate { controlTip.SetToolTip(c, tip.ToProperCase()); });
            //controlTip.SetToolTip(c, tip.ToProperCase());
        }// end of method AddControlToolTip

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a angle of degree representing a compass direction.
        /// </summary>
        /// <param name="degrees">The angle of the direction</param>
        /// <remarks>The value must be numeric and less than 360\u00B0.</remarks>
        /// <returns>The converted value.</returns>
        public static string CompassDirection(float degrees)
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
        public static bool ContainsWholeWord(string input, string word)
        {
            return Regex.Match(input, $@"\b{word}\b", RegexOptions.IgnoreCase).Success;
        }// end of method ContainsWholeWord

        /// <summary>
        /// Uses data received from the GeoNames web service in a <see cref="string"/> JSON format  
        /// and converts it to a list containig <see cref="CityData"/> <see cref="object"/>s.
        /// </summary>
        /// <param name="cityJSON">The angle of the direction</param>
        /// <remarks>The list return may only contain one <see cref="object"/>.</remarks>
        /// <returns>A list containig <see cref="CityData"/> <see cref="object"/>s.</returns>
        public static List<CityData> CreateGeoNamesCityData(string cityJSON)
        {
            CityData currentCityData = null;
            List<CityData> citiesFound = null;
            string cityName = null;
            string countryName = null;
            string countryCode = null;
            string localCityName = null;
            string regionCode = null;
            string regionName = null;
            float latitude = 0;
            float longitude = 0;

            if (cityJSON != null)
            {
                var city = JsonConvert.DeserializeObject<dynamic>(cityJSON);

                if (city.Count < 1)
                {
                    cityName = city["name"].ToString();
                    countryName = city["countryName"].ToString();
                    countryCode = city["countryCode"].ToString();
                    localCityName = city["toponymName"].ToString();
                    regionCode = city["adminCode1"].ToString();
                    regionName = city["countryCode"].ToString().Equals("US") ?
                                 usStatesByCode[regionCode].ToString() :
                                 null;
                    latitude = float.Parse(city["lat"].ToString());
                    longitude = float.Parse(city["lng"].ToString());

                    currentCityData = new CityData
                    {
                        cityName = cityName,
                        countryName = countryName,
                        countryCode = countryCode,
                        regionCode = regionCode,
                        regionName = regionName,
                        latitude = latitude,
                        longitude = longitude
                    };

                    citiesFound.Add(currentCityData);
                }// end of if block
                else
                {
                    // convert the array of geonames to a string for further parsing 
                    string geoNames = city["geonames"].ToString();

                    // deserialize the string into objects
                    List<dynamic> cityDataList = JsonConvert.DeserializeObject<List<dynamic>>(geoNames);
                    citiesFound = new List<CityData>();

                    for (var i = 0; i < cityDataList.Count; i++)
                    {
                        // if there is no country then it is not needed
                        if (cityDataList[i]["countryName"] == null) continue;

                        cityName = cityDataList[i]["name"].ToString();
                        countryName = cityDataList[i]["countryName"].ToString();
                        countryCode = cityDataList[i]["countryCode"].ToString();
                        localCityName = cityDataList[i]["toponymName"].ToString();
                        regionCode = cityDataList[i]["adminCode1"].ToString();
                        regionName = cityDataList[i]["countryCode"].ToString().Equals("US") ?
                                     usStatesByCode[regionCode].ToString() :
                                     null;
                        latitude = float.Parse(cityDataList[i]["lat"].ToString());
                        longitude = float.Parse(cityDataList[i]["lng"].ToString());

                        currentCityData = new CityData
                        {
                            cityName = cityName,
                            countryName = countryName,
                            countryCode = countryCode,
                            regionCode = regionCode,
                            regionName = regionName,
                            latitude = latitude,
                            longitude = longitude
                        };

                        citiesFound.Add(currentCityData);
                    }// end of for loop
                }// end of else block
            }// end of if block 
            
            return citiesFound;
        }// end of method CreateGeoNamesCityData

        /// <summary>
        /// Uses data received from the Here Maps web service in a <see cref="string"/> JSON format  
        /// and converts it to a <see cref="CityData"/> <see cref="object"/>s.
        /// </summary>
        /// <param name="cityJSON">The angle of the direction</param>
        /// <remarks>Here Maps only returns one city data so it has to be specific ex: Pine Hills, FL.</remarks>
        /// <returns>A <see cref="CityData"/> <see cref="object"/>s.</returns>
        public static CityData CreateHereCityData(string cityJSON)
        {
            CityData currentCityData = null;
            
            if (cityJSON != null)
            {
                string countryName = null;
                string countryCode = null;
                string localCityName = null;
                string regionCode = null;
                string regionName = null;
                float latitude = 0;
                float longitude = 0;

                var city = JsonConvert.DeserializeObject<dynamic>(cityJSON);
                var response = city["Response"];
                var view = response["View"];    // will return an array with only one index which is 0
                var result = view[0]["Result"];

                var place = result[0];
                var location = place["Location"];
                var displayPosition = location["DisplayPosition"];
                var navigationPosition = location["NavigationPosition"][0]; // an array
                var address = location["Address"];
                var additionalData = address["AdditionalData"]; // an array

                countryName = UtilityMethod.ToProperCase(additionalData[0]["value"].ToString());
                countryCode = worldCountryCodes[countryName].ToString().ToUpper();
                regionName = additionalData[1]["value"];
                regionCode = address["State"]?.ToString();
                localCityName = address["Country"].ToString().ToUpper().Equals("USA") ?
                                    address["District"].ToString() :
                                    address["City"].ToString();                
                latitude = float.Parse(navigationPosition["Latitude"].ToString());
                longitude = float.Parse(navigationPosition["Longitude"].ToString());

                currentCityData = new CityData
                {
                    cityName = localCityName,
                    countryName = countryName,
                    countryCode = countryCode,
                    regionName = regionName,
                    regionCode = regionCode,
                    latitude = latitude,
                    longitude = longitude
                };
            }// end of if block

            return currentCityData;
        }// end of method CreateHereCityData

        /// <summary>
        /// Decode a Uri so that is is compatible with a valid standard <see cref="Uri"/> <see cref="string"/>
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> <see cref="string"/> to be formatted</param>
        /// <returns>A valid formatted <see cref="Uri"/> <see cref="string"/>.</returns>
        public static string EscapeUriString(string uri)
        {
            return Uri.EscapeDataString(uri);
        }// end of method EscapeUriString 

        /// <summary>
        /// Finds the closest word match to word.
        /// </summary>
        /// <param name="phraseList">A array containing a list of strings for check against.</param>
        /// <param name="searchPhrase">A string to search the list for.</param>
        /// <returns>The closest match to the query string.</returns>
        public static string FindClosestWordMatch(string[] phraseList, string searchPhrase)
        {
            StringBuilder closestMatch = new StringBuilder();
            StringBuilder mostLikelyMatch = new StringBuilder();
            int closest = searchPhrase.Length;
            int highestProbability = 0; // this is the highest percentage of a possible match

            foreach (string phrase in phraseList)
            {
                int changesNeeded = GetLevenshteinDistance(searchPhrase, phrase);
                int wordProbability = PercentageMatch(phrase, searchPhrase);

                // if 50% or greater match then its is a possible substitution
                if (wordProbability > highestProbability)
                {
                    highestProbability = wordProbability;
                    mostLikelyMatch.Clear();
                    mostLikelyMatch.Append(phrase);
                }// end of if block

                if (changesNeeded < closest)
                {
                    closest = changesNeeded;
                    closestMatch.Clear();
                    closestMatch.Append(phrase);
                }// end of if block
            }// end of for loop

            // if both algorithms came up with the same string
            // then just return any one
            if (closestMatch.ToString().Equals(mostLikelyMatch.ToString()))
            {
                return closestMatch.ToString();
            }// end of if block
            else
            {
                // if both algorithms came up with different strings,
                // use the one with the highest percentage match
                int a = PercentageMatch(closestMatch.ToString(), searchPhrase);
                int b = PercentageMatch(mostLikelyMatch.ToString(), searchPhrase);

                if (a > b)
                {
                    return closestMatch.ToString();
                }// end of if block                
                else
                {
                    return mostLikelyMatch.ToString();
                }// end of else block
            }// end of else block 
        }// end of method FindClosestWordMatch

        public static void FindGeoNamesCity(string cityName, PreferencesForm caller)
        {
            int maxRows = 100;

            if (cityName.Contains(" "))
            {
                cityName = cityName.Replace(" ", "+");
            }// end of if block

            // All commas must be replaced with the + symbols for the HERE Maps web service
            if (cityName.Contains(","))
            {
                cityName = cityName.Replace(",", "+");
            }// end of if block

            string cityUrl =
                    "http://api.geonames.org/searchJSON?q=" +
                        EscapeUriString(cityName.ToLower()) +
                            "&maxRows=" + maxRows +
                            "&username=" + WidgetUpdateService.geoNameAccount;

            if (HasInternetConnection())
            {
                //run an background service
                CityDataService cd = new CityDataService(cityUrl, "geo", caller);
                cd.Run();
            }// end of if block
            else
            {
                MessageBox.Show("No Internet Connection.", WeatherLionMain.PROGRAM_NAME,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }// end of else block
        }// end of method FindHereCity

        public static void FindHereCity(string cityName)
        {
            // All spaces must be replaced with the + symbols for the HERE Maps web service
            if (cityName.Contains(" "))
            {
                cityName = cityName.Replace(" ", "+");
            }// end of if block

            // All commas must be replaced with the + symbols for the HERE Maps web service
            if (cityName.Contains(","))
            {
                cityName = cityName.Replace(",", "+");
            }// end of if block

            string cityUrl =
                    "https://geocoder.api.here.com/6.2/geocode.json?"
                    + "app_id=" + WidgetUpdateService.hereAppId
                    + "&app_code=" + WidgetUpdateService.hereAppCode
                    + "&searchtext=" + EscapeUriString(cityName.ToLower());

            if (HasInternetConnection())
            {
                //run an background service
                CityDataService cd = new CityDataService(cityUrl, "here");
                //cd.execute();
            }// end of if block
            else
            {
                MessageBox.Show("No Internet Connection.", WeatherLionMain.PROGRAM_NAME, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }// end of else block
        }// end of method findHereCity

        /// <summary>
        /// Returns the line number were the exception occurred in the code
        /// </summary>
        /// <param name="e">The exception that was thrown by the compiler.</param>
        /// <returns>The line number at which the exception was thrown.</returns>
        public static int GetExceptionLineNumber(Exception e)
        {
            // The stack trace might be very long so we need only the last line number
            // were the error occurred.
            string es = Regex.Replace(e.ToString(), "[^a-zA-Z0-9_.: ]+", "", RegexOptions.Compiled);
            int lineIndex = es.IndexOf("line");
            string lineTruncated = es.ToString().Substring(lineIndex).Trim().Replace("line ", "");
            StringBuilder lineString = new StringBuilder();
            int x = 0;
            int lineNumber = 0;

            // get all characters before encountering a space.
            // these character which appear immediately after the word line should be numbers
            while (lineTruncated[x] != ' ')
            {
                lineString.Append(lineTruncated[x]);
                x++;
            }// end of while loop

            if (lineString.Length > 0)
            {
                lineNumber = int.Parse(lineString.ToString());
            }// end of if block
            
            return lineNumber;
        }// end of method GetExceptionLineNumber

        /// <summary>
        /// Returns a Nullable<DateTime> object from a string representation of a date  
        /// </summary>
        /// <param name="date">A string representation of a date</param>
        /// <remarks>The value must be a string which represents a valid date formatted MM/dd/yyyy.</remarks>
        /// <returns>The converted DateTime object or null if not parsed.</returns>
        public static Nullable<DateTime> GetDate(string date)
        {
            Nullable<DateTime> startDate;
            string[] ds = date.Split('/');
            StringBuilder properlyformattedDate = new StringBuilder();

            properlyformattedDate.Append(int.Parse(ds[0]) < 10 ? "0" + ds[0] + "/" : ds[0] + "/");
            properlyformattedDate.Append(int.Parse(ds[1]) < 10 ? "0" + ds[0] + "/" : ds[1] + "/");
            properlyformattedDate.Append(ds[2]);
           
            try
            {
                startDate = DateTime.ParseExact(properlyformattedDate.ToString(), "MM/dd/yyyy",
                    CultureInfo.InvariantCulture);
                return startDate;
            }// end of try block
            catch (Exception e)
            {
                MessageBox.Show(e.Message, WeatherLionMain.PROGRAM_NAME,
                               MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                
            }// end of catch block

            return null;
        }// end of method GetDate

        /// <summary>
        /// Accepts a numeric value of type long that represents  
        /// a Unix time value.
        /// </summary>
        /// <param name="unixTimeValue">The numerical time value</param>
        /// <remarks>The value must be of the long integer data type.</remarks>
        /// <returns>The converted DateTime object.</returns>
        public static DateTime GetDateTime(long unixTimeValue)
        {
            DateTime standardDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

            return standardDateTime = standardDateTime.AddSeconds(unixTimeValue).ToLocalTime();
        }// end of method GetDateTime

        /// <summary>
        /// Accepts a 24hr time and converts it to a 12hr time.
        /// </summary>
        /// <param name="hour">The 24hr clock hour time value</param>
        /// <param name="minute">The minute time value</param>
        /// <remarks>Example: 00:00 file return 12:00 AM.</remarks>
        /// <returns>Formatted 12hr time.</returns>
        public static string Get12HourTime(int hour, int minute)
        {
            // 24 hour times might return a negative if the time-zone
            // offset is subtracted from 00 or 24hrs
            if (hour < 0) hour = 24 + hour;            

            //return $"{(hour > 12 ? hour - 12 : (hour == 0 ? 12 : hour))}:" +
            //       $"{(minute < 10 ? ("0" + minute.ToString()) : minute.ToString())} {(hour > 12 ? "PM" : "AM")}";

            return string.Format("{0}:{1} {2}",
                    (hour > 12 ? hour - 12 : (hour == 0 ? 12 : hour)),
                    (minute < 10 ? minute == 0 ? "00" : ("0" + minute.ToString())
                    : minute.ToString()),
                    (hour > 12 ? "PM" : "AM"));
        }// end of method Get12HourTime
        
        /// <summary>
        /// Converts a string representation of a time in 12hr format
        /// to a time in 24hr format.
        /// </summary>
        /// <param name="time">A string representation of a time in 12hr format</param>
        /// <remarks>Value must be a time value represented as a string.</remarks>
        /// <returns>A string representation of a time in 24hr format.</returns>
        public static string Get24HourTime(string time)
        {
            StringBuilder realTime = new StringBuilder(time);
           
            if (!realTime.ToString().Contains(" "))
            {
                int insertionPoint = time.IndexOf(":") + 2;           
                realTime = new StringBuilder(time).Insert(time.Length - 2, " ");
            }// end of if block

            int hour = int.Parse(realTime.ToString().Split(':')[0].Trim());
            int minute = int.Parse(realTime.ToString().Split(':')[1].Trim().Split(' ')[0].Trim());
            string meridian = realTime.ToString().Split(' ')[1].Trim();
            string t = null;

            if (meridian.ToLower().Equals("am"))
            {
                t = string.Format("{0:00}:{1:00}", hour < 10 ? 0 + hour : hour == 12 ? 0 : hour, minute);
            }// end of if block
            else if (meridian.ToLower().Equals("pm"))
            {
                t = string.Format("{0:00}:{1:00}", hour < 12 ? 12 + hour : hour, minute);
            }// end of else if block

            return t;
        }// end of method Get24HourTime

        /// <summary>
        /// Returns the number of files found in a specific path
        /// </summary>
        /// <param name="path">The location to search for files.</param>
        /// <remarks>Value must be a string representing a system file path.</remarks>
        /// <returns>The number of files found.</returns>
        public static int GetFileCount(string path)
        {
            // searches the current directory and sub directory
            int fCount = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length;

            return fCount;
        }// end of method GetFileCount

        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        /// <param name="firstString">The <see cref="string"/> the be checked</param>
        /// <param name="secondString">The <see cref="string"/> the string that the fistString will be compared to</param>
        /// <returns></returns>
        public static int GetLevenshteinDistance(string firstString, string secondString)
        {
            int n = firstString.Length;
            int m = secondString.Length;
            int[,] distance = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }// end of if block

            if (m == 0)
            {
                return n;
            }// end of if block

            // Step 2
            for (int i = 0; i <= n; distance[i, 0] = i++) ;

            for (int j = 0; j <= m; distance[0, j] = j++) ;

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (secondString[j - 1] == firstString[i - 1]) ? 0 : 1;

                    // Step 6
                    distance[i, j] = Math.Min(
                        Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                    distance[i - 1, j - 1] + cost);
                }// end of inner for loop
            }// end of outer for loop

            // Step 7
            return distance[n , m];
        }// end of method GetLevenshteinDistance

        /// <summary>
        /// Locate any subdirectories found in a specific directory
        /// </summary>
        /// <param name="path">A directory path which may contain subdirectories.</param>
        /// <remarks>Value must be a string representing a system file path.</remarks>
        /// <returns>A List containing the names of all subdirectories found in the given path.</returns>
        public static List<string> GetSubdirectories(string path)
        {
            List<string> subDirectoryNames = new List<string>();
            List<string> subDirectoriesPath = Directory.GetDirectories(path, "*", SearchOption.AllDirectories).ToList();

            foreach (string fullPath in subDirectoriesPath)
            {
                subDirectoryNames.Add(new DirectoryInfo(fullPath).Name);
            }// end of for each block

            // return a list containing only the directory names
            return subDirectoryNames;
        }// end of method GetSubdirectories

        /// <summary>
        /// Retrieves the IP Address from the system.
        /// </summary>
        /// <returns>The system's ip address and returns it as a <see cref="string"/>.</returns>
        public static string GetSystemIpAddress()
        {
            string ip = null;
            StringBuilder ipAddress = new StringBuilder();
            string output = null;

            if( HasInternetConnection())
            {
                try
                {
                    ip = HttpHelper.DownloadUrl( "http://ifconfig.me/ip" );
                }// end of try block 
                catch (Exception e) 
                {
                    LogMessage( LogLevel.WARNING, e.Message,
                        $"{TAG}::GetSystemIpAddress [line: {UtilityMethod.GetExceptionLineNumber(e)}]");                              			
                }// end of catch block

                if(ip == null)
                {
                    // configure the windows process and redirect the output data into a string
                    using (Process proc = new Process())
                    {
                        proc.StartInfo.FileName = "cmd.exe";
                        proc.StartInfo.Arguments = "/c nslookup myip.opendns.com resolver1.opendns.com";
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.RedirectStandardOutput = true;
                        proc.Start();
                        output = proc.StandardOutput.ReadToEnd();
                    }// end of using block

                    int i = 0; 
                
                    // the line containing the public ip address
                    string pIpAddLine = "Address:  ";

                    if (output != null)
                    {
                        i++;

                        foreach (var line in output.Split(new string[] { Environment.NewLine },
                        StringSplitOptions.RemoveEmptyEntries))
                        {
                            // The public ip should appear after the 2nd line
                            if(i > 2)
                            {
                                if (line != null)
                                {
                                    if (line.Contains(pIpAddLine))
                                    {
                                        ip = line.Substring(pIpAddLine.Length);
                                        break;
                                    }// end of if block
                                }// end of if block   
                            }// end of if block                      
                        }// end of for each loop
                    }// end of if block                   
                }// end of if block          

                if(ip == null)
                {
                    // fallback in case the command encountered an error
                    string[][] jsonUrls = new string[][]{
                        new string[] { "https://www.trackip.net/ip?json", "IP" },
                        new string[] { "https://api.ipify.org?format=json", "ip" }
                    };

                    string strJSON = null;
                    string[] urlUsed = null;
                        
                    while (strJSON == null)
                    {
                        foreach (string[] url in jsonUrls)
                        {
                            try
                            {
                                strJSON = HttpHelper.DownloadUrl(url[0]);
                                urlUsed = url;
                            }// end of try block
                            catch (Exception)
                            {
                                strJSON = null;
                            }// end of catch block
                        }// end of for each loop
                    }// end of while loop

                    try
                    {
                        var json = JsonConvert.DeserializeObject<dynamic>(strJSON);

                        // Check if a JSON was returned from the web service
                        if (json != null)
                        {
                            // Get the String returned from the object
                            ip = (urlUsed[1]);
                        }// end of if block			
                    }// end of try block
                    catch (Exception je)
                    {
                        LogMessage(LogLevel.SEVERE, je.Message,
                            $"UtilityMethod::getSystemIpAddress [line: {UtilityMethod.GetExceptionLineNumber(je)}]");
                    }// end of catch block                      
                }// end of if block                       
            }// end of if block
            else
            {
                 ShowMessage("No Internet Connection.", null,
                    title: $"{WeatherLionMain.PROGRAM_NAME}", buttons: MessageBoxButtons.OK,
                        mbIcon: MessageBoxIcon.Error);
            }// end of else block

            // Return the data from specified url
            return ip;

        }// end of method GetSystemIpAddress

        /// <summary>
        /// Determines weather a path represents a directory or a file.
        /// </summary>
        /// <param name="path">The path to the obejct in question</param>
        /// <returns>A <see cref="string"/> representing the type of object.</returns>
        public static string GetPathType(string path)
        {
            string type = null;

            if (Directory.Exists(path))
            {
                type = "directory";
            }// end of if block
            else if (File.Exists(path))
            {
                type = "file";
            }
            // end of else if block
            return type;
        }// end of method GetPathType

        /**
     * Get the duration of time that has elapsed since a certain date.
     *
     * @param pastDate The date in the past to be compared to.
     * @return  A {@code String} representing the time frame that has passed.
     */
        public static string GetTimeSince(DateTime pastDate)
        {
            //milliseconds
            long difference = (long) (new DateTime() - pastDate).TotalMilliseconds;

            long secondsInMilli = 1000;
            long minutesInMilli = secondsInMilli * 60;

            long elapsedMinutes = difference / minutesInMilli;
            string timeElapsed;

            if (elapsedMinutes >= 60)
            {
                // an hour or more
                int hours = (int)elapsedMinutes / 60;
                timeElapsed = hours > 1 ? hours + " hours ago" : hours + " hour ago";
            }// end of if block
            else if (elapsedMinutes >= 1440)
            {
                // a day or more
                int days = (int)elapsedMinutes / 1440;
                timeElapsed = days > 1 ? days + " days ago" : days + " day ago";
            }// end of if block
            else if (elapsedMinutes >= 10080)
            {
                // a week or more
                int weeks = (int)elapsedMinutes / 10080;
                timeElapsed = weeks > 1 ? weeks + " weeks ago" : weeks + " week ago";
            }// end of if block
            else if (elapsedMinutes >= 43830)
            {
                // a month or more
                int months = (int)elapsedMinutes / 1440;
                timeElapsed = months > 1 ? months + " months ago" : months + " month ago";
            }// end of if block
            else if (elapsedMinutes >= 525960)
            {
                // a year or more
                int years = (int)elapsedMinutes / 525960;
                timeElapsed = years > 1 ? years + " years ago" : years + " year ago";
            }// end of if block
            else
            {
                int seconds = (int)elapsedMinutes / 60;

                if (elapsedMinutes < 1)
                {
                    // time in seconds
                    timeElapsed = seconds > 1 ? seconds + " seconds ago" : seconds + " second ago";
                }// end of if block
                else
                {
                    // time in minutes
                    timeElapsed = elapsedMinutes > 1 ? elapsedMinutes + " minutes ago" : elapsedMinutes + " minute ago";
                }// end of else block
            }// end of else block

            return timeElapsed;
        }// end of method GetTimeSince

        /// <summary>
        /// Uses the computers Internet connection to determine the current city location of the connection.
        /// </summary>
        /// <returns>An <see cref="object"/> of the<see cref="CityData"/>.</returns>
        public static CityData GetSystemLocation()
        {
            CityData cd = null;
            string publicIp = GetSystemIpAddress();
            string ak = "1bb227db8f2dca1b9a3917fb403e2e99"; // Get your own free key from the website
            string ipStackUrl = $"http://api.ipstack.com/{publicIp}?access_key={ak}";

            try
            {
               // get ip details from web service
               string strJSON = HttpHelper.DownloadUrl(ipStackUrl);                

               if (strJSON != null)
               {
                    dynamic publicIpData = JsonConvert.DeserializeObject<dynamic>(strJSON);
                                        
                    //ipstack.com implementation
                    string ip = publicIpData["ip"].ToString();
                    string city = publicIpData["city"].ToString();
                    string regionName = publicIpData["region_name"].ToString();
                    string regionCode = publicIpData["region_code"].ToString();
                    string countryName = publicIpData["country_name"].ToString();
                    string countryCode = publicIpData["country_code"].ToString();
                    string zipCode = publicIpData["zip"].ToString();                   
                    string latitude = publicIpData["latitude"].ToString();
                    string longitude = publicIpData["longitude"].ToString();                    

                    // create a new CityData object
                    cd = new CityData(city, countryName, countryCode, regionName,
                            regionCode, float.Parse(latitude), float.Parse(longitude));
                }// end of if block             
            }// end of try block
            catch (Exception e)
            {
                LogMessage(LogLevel.WARNING, "Error get ip address from ipstack.com!",
                    $"{TAG} ::GetSystemLocation [line: {GetExceptionLineNumber(e)}]");

                LogMessage(LogLevel.INFO, "Falling back to ipapi.co for data.",
                    $"{TAG} ::GetSystemLocation");

                string ipApiUrl = $"https://ipapi.co/{publicIp}/json";
                string strJSON = HttpHelper.DownloadUrl(ipApiUrl);
                dynamic publicIpData = JsonConvert.DeserializeObject<dynamic>(strJSON);

                //ipstack.com implementation
                string city = publicIpData["city"].ToString();
                string regionName = publicIpData["region"].ToString();
                string regionCode = publicIpData["region_code"].ToString();
                string countryName = publicIpData["country_name"].ToString();
                string countryCode = publicIpData["country"].ToString();
                string zipCode = publicIpData["postal"].ToString();
                string latitude = publicIpData["latitude"].ToString();
                string longitude = publicIpData["longitude"].ToString();                

                // create a new CityData object
                cd = new CityData(city, countryName, countryCode, regionName,
                        regionCode, float.Parse(latitude), float.Parse(longitude));
            }// end of try block         

            return cd;
        }// end of method GetSystemLocation

        /// <summary>
        /// Determines if a city has been previously stored a a local JSON file.
        /// </summary>
        /// <param name="cityName">The name of the city</param>
        /// <returns>True/False dependent on the outcome of the check.</returns>
        public static bool IsFoundInJSONStorage(string cityName)
        {
            string[] city = cityName.Split(',');
            bool found = false;

            //JSON File Search
            if (File.Exists(JSONHelper.PREVIOUSLY_FOUND_CITIES_JSON))
            {
                string jsonString = null;

                try
                {
                    jsonString = File.ReadAllText(JSONHelper.PREVIOUSLY_FOUND_CITIES_JSON);
                }// end of try block
                catch (Exception e)
                {
                    LogMessage(LogLevel.SEVERE, e.Message, "UtilityMethod::IsFoundInJSONStorage");
                }// end of catch block

                if (jsonString != null)
                {
                    // convert the file JSON into a list of objects
                    List<CityData> cityDataList = JsonConvert.DeserializeObject<List<CityData>>(jsonString);

                    foreach (CityData c in cityDataList)
                    {
                        string cCityName = c.cityName;
                        string cRegionName = c.regionName;
                        string cRegionCode = c.regionCode;
                        string cCountryName = c.countryName;
                        bool containsNumber = IsNumeric(cRegionCode);

                        if (cityName.Equals(cCityName + ", " + cCountryName, StringComparison.OrdinalIgnoreCase) ||
                            !containsNumber && cityName.Equals(cCityName + ", " + cRegionCode, StringComparison.OrdinalIgnoreCase))
                        {
                            found = true;
                            LogMessage(LogLevel.INFO, cityName + " was found in the JSON storage.",
                                    TAG + "::IsFoundInJSONStorage");
                        }// end of if block                        
                    }// end of for each loop  
                }// end of if block
            }// end of if block            

            return found;
        }// end of method isFoundInJSONStorage

        /// <summary>
        /// Determines if a city has been previously stored a a local XML file.
        /// </summary>
        /// <param name="cityName">The name of the city</param>
        /// <returns>True/False dependent on the outcome of the check.</returns>
        public static bool IsFoundInXMLStorage(string cityName)
        {
            bool found = false;

            //XML file search   
            if(File.Exists(XMLHelper.PREVIOUSLY_FOUND_CITIES_XML))
            {
                XmlDocument cityData = new XmlDocument();
                cityData.Load(XMLHelper.PREVIOUSLY_FOUND_CITIES_XML);
                List<CityData> cd = new List<CityData>();

                try
                {
                    XmlNode rootNode = cityData.DocumentElement;
                    XmlNodeList list = cityData.SelectNodes("//City");

                    for (int i = 0; i < list.Count; i++)
                    {
                        XmlElement node = (XmlElement)list[i];
                        string cCityName = node.GetElementsByTagName("CityName")[0].InnerText;
                        string cRegionName = node.GetElementsByTagName("RegionName")[0].InnerText;
                        string cRegionCode = node.GetElementsByTagName("RegionCode")[0].InnerText;
                        string cCountryName = node.GetElementsByTagName("CountryName")[0].InnerText;
                        bool containsNumber = IsNumeric(cRegionCode);

                        if (cityName.Equals(cCityName + ", " + cCountryName, StringComparison.OrdinalIgnoreCase) ||
                            !containsNumber && cityName.Equals(cCityName + ", " + cRegionCode, StringComparison.OrdinalIgnoreCase))
                        {
                            found = true;
                            LogMessage(LogLevel.INFO, $"{cityName} was found in the XML storage.",
                                    "UtilityMethod::IsFoundInXMLStorage");
                        }// end of if block
                    }// end of for loop    		 		

                }// end of try block 
                catch (IOException io)
                {
                    LogMessage(LogLevel.SEVERE, io.Message,
                            "UtilityMethod::IsFoundInXMLStorage");
                }// end of catch block 
            }// end of if block          

            return found;
        }// end of method IsFoundInXMLStorage

        /// <summary>
        /// Ensures that the city entered by the user is correctly formatted.
        /// </summary>
        /// <param name="cityName">A string representing the of a city.</param>
        /// <remarks>Value must be a string representing the name of a city.</remarks>
        /// <returns>A boolean value of True/False dependent on the result of the test.</returns>
        public static bool IsValidCityName(string cityName)
        {
            return cityName.Contains(",");
        }// end of method IsValidCityName

        /// <summary>
        /// Determines whether a string is a valid JSON object.
        /// </summary>
        /// <seealso cref="https://stackoverflow.com/a/36807675"/>
        /// <param name="strInput">The <see cref="string"/> to be validated.</param>
        /// <returns>True or False dependent on the success or failure of the operation.</returns>
        public static bool IsValidJson<T>(string strInput)
        {
            strInput = strInput.Trim();

            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JsonConvert.DeserializeObject<T>(strInput);
                    return true;
                }// end of try block
                catch // not valid
                {
                    return false;
                }// end of catch block
            }// end of if block
            else
            {
                return false;
            }// end of else block
        }// end of method IsValidJson

        /// <summary>
        /// Determines wheather the host machine is connected to the internet.
        /// </summary>
        /// <returns>A boolean value of True/False dependent on the result of the test.</returns>
        public static bool HasInternetConnection()
        {
            return NetworkHelper.HasNetworkAccess();
        }// end of method HasInternetConnection 

        /// <summary>
        /// Determines wheather a city is already been used.
        /// </summary>
        /// <param name="cityName">A string representation of a city name.</param>
        /// <returns>A boolean value of True/False dependent on the result of the test.</returns>
        public static bool IsKnownCity(string cityName)
        {
            if (cityName.Contains(","))
            {
                return IsFoundInDatabase(cityName);
            }// end of if block   		

            return false;
        }// end of method IsKnownCity

        /// <summary>
        /// Determines wheather a city is already stored in the local database.
        /// </summary>
        /// <param name="cityName">A string representation of a city name.</param>
        /// <returns>A boolean value of True/False dependent on the result of the test.</returns>
        public static bool IsFoundInDatabase(string cityName)
        {
            // Check SQLite Database
            string SQL;
            string[] city = cityName.Split(',');

            if (city[1].Trim().Length == 2)
            {
                SQL = "SELECT CityName, CountryName, CountryCode, RegionCode, "
                        + "Latitude, Longitude FROM WorldCities.world_cities WHERE CityName=@city AND RegionCode=@other " +
                        " AND typeof(RegionCode) = 'integer'";
            }// end of if block
            else
            {
                SQL = "SELECT CityName, CountryName, CountryCode, RegionCode, "
                        + "Latitude, Longitude FROM WorldCities.world_cities WHERE CityName=@city AND CountryName=@other";
            }// end of else block

            try
		    {
                using (SQLiteCommand comm = WeatherLionMain.conn.CreateCommand())
                {
                    comm.CommandText = SQL;
                    comm.Parameters.Add("@city", DbType.String);
                    comm.Parameters["@city"].Value = city[0].Trim();

                    comm.Parameters.Add("@other", DbType.String);
                    comm.Parameters["@other"].Value = city[1].Trim();

                    IDataReader dr = comm.ExecuteReader();
                    int found = 0;

                    while (dr.Read())
                    {
                        found++;
                    }// end of while loop

                    if (found > 0)
                    {
                        return true;
                    }// end of if block	
                }// end of using block

                		
            }// end of try block
            catch (Exception)
            {              
            }// end of catch block

            return false;
        }// end of method IsFoundInDatabase

        /// <summary>
        /// Test if a <see cref="string"/> value is a number
        /// </summary>
        /// <param name="value">value A <see cref="string"/> value to be tested.</param>
        /// <remarks>Value must be a <see cref="string"/> to be converted without numbers.</remarks>
        /// <returns>A <see cref="bool"/> true/false depending on the result of the test.</returns>
        public static bool IsNumeric(string value)
        {
            return value != null && double.TryParse(value, out double number);
        }// end of method IsNumeric

        /// <summary>
        /// Check if a string value is valid JSON data
        /// </summary>
        /// <param name="strJSON">A <see cref="string"/> representing JSON data</param>
        /// <returns>True/False dependent on the outcome of the test.</returns>
        public static bool IsValidJson(string strJSON)
        {
            if (string.IsNullOrWhiteSpace(strJSON))
            {
                return false;
            }// end of if block

            var value = strJSON.Trim();

            if ((value.StartsWith("{") && value.EndsWith("}")) || //For object
                (value.StartsWith("[") && value.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(value);
                    return true;
                }// end of try block
                catch (JsonReaderException)
                {
                    return false;
                }// end of catch block
            }// end of if block

            return false;
        }// end of method IsValidJson

        /// <summary>
        /// Use the logger to log messages locally
        /// </summary>
        /// <param name="level">Level of the log</param>
        /// <param name="message">Message to be logged</param>
        /// <param name="inMethod">The method in which the data required logging</param>
        public static void LogMessage(LogLevel level, string message, string inMethod)
        {
            WidgetLogging logger = new WidgetLogging();
            logger.Log(message, level, inMethod);                          
        }// end of method LogMessage

        /// <summary>
        /// Prompts the user of a error that occurred due to missing program requirements.
        /// </summary>
        /// <param name="asset">A <see cref="string"/> representing the program asset that is missing</param>
        public static void MissingRequirementsPrompt(string asset)
        {
            // prompt the user
            ShowMessage("There are missing files or information that are neccessary for"
                    + " the program to run and\ntherefore renders the program currupt and unable to lauch!", null,
                    $"{WeatherLionMain.PROGRAM_NAME} ({asset})", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // log message
            UtilityMethod.LogMessage(LogLevel.SEVERE, "Missing: " + asset,
                    "WeatherLionMain::missingAssetPrompt");

            Application.Exit(); // terminate the program
        }// end of method MissingAssetPrompt

        /// <summary>
        /// Searches for the number of times that a <see cref="char"/> is found in a <see cref="string"/>
        /// </summary>
        /// <param name="c">The <see cref="char"/> to look for in a <see cref="string"/></param>
        /// <param name="checkString">The <see cref="string"/> that contains the specified <see cref="char"/></param>
        /// <returns>An <see cref="int"/> representing the number of occurrences of the search character</returns>
        public static int NumberOfCharacterOccurences(char c, string checkString)
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
        /// Determines what percentage match occurs when both strings are compared
        /// </summary>
        /// <param name="mainString"><The <see cref="string"/> that should contain a similar 
        /// <see cref="string"/></param>
        /// <param name="searchString">The <see cref="string"/> that needs to be matched</param>
        /// <returns>The numeric percentage of the comparison</returns>
        public static int PercentageMatch(string mainString, string searchString)
        {
            string[] ms_words = mainString.Split(' ');
            string[] ss_words = searchString.Split(' ');
            int num_words_found = 0;
            float match_percentage = 0;

            foreach (string w in ss_words)
            {
                if (mainString.Contains(w))
                {
                    // the string contains this word
                    num_words_found++; // increment the number of words found
                }// end of if block
            }// end of for each loop

            // what percentage of the main string is the search string
            match_percentage = ((float) num_words_found / (float) ms_words.Length) * 100.0f;

            // the main string cannot be shorter than the search string for a 100% match
            if (ms_words.Length < ss_words.Length)
            {
                match_percentage = ((float) num_words_found / (float) ss_words.Length) * 100.0f;
            }// end of if block	

            return (int) match_percentage;
        }// end of function percentageMatch

        /// <summary>
        /// Replace all instances of a character withing a <see cref="string"/>
        /// </summary>
        /// <param name="input">A <see cref="string"/> value containg a character</param>
        /// <param name="c">The <see cref="char"/> to be replaced</param>
        /// <param name="replacement">The replacement <see cref="char"/> or <see cref="string"/>.</param>
        /// <returns></returns>
        public static string ReplaceAll(string input, string c, string replacement)
        {
            return Regex.Replace(input, $@"{c}+", replacement);
        }// end of method ReplaceAll

        /// <summary>
        /// Replace the last occurrence of a <see cref="string"/> contained in another <see cref="string"/>
        /// </summary>
        /// <param name="find">The <see cref="string"/> to look for in another <see cref="string"/></param>
        /// <param name="replace">The replacement <see cref="string"/></param>
        /// <param name="source">The <see cref="string"/> that contains another {@code String}</param>
        /// <returns>A modified <see cref="string"/> reflecting the requested change</returns>
        public static string ReplaceLast(string find, string replace, string source)
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
        /// Displays a dialog for a response from the user  
        /// </summary>
        /// <param name="msg">The message to be displayed to the user</param>
        /// <param name="title">The dialog box title</param>
        /// <returns>The <see cref="DialogResult"/> returned by the user's response</returns>
        public static DialogResult ResponseBox(string msg, string title)
        {
            return MessageBox.Show(msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);
        }// end of method ResponseBox

        /// <summary>
        /// Uses the GeoNames web service to return the geographical location of a city using it's name.
        /// </summary>
        /// <param name="wxLocation">The location of the city to be found.</param>
        /// <returns>A <see cref="string"/> representation of a JSON <see cref="object"/> returned from the web service.</returns>
        public static string RetrieveGeoNamesGeoLocationUsingAddress(string wxLocation)
        {
            int maxRows = 10;

            wxLocation = wxLocation.Contains(",") ?
                   wxLocation.Substring(0, wxLocation.IndexOf(",")).ToLower() :
                   wxLocation;

            string strJSON = null;
            string geoUrl =
                    "http://api.geonames.org/searchJSON?" +
                    "name_equals=" + wxLocation.ToLower() +
                    "&maxRows=" + maxRows +
                    "&username=" + WidgetUpdateService.geoNameAccount;

            if (HasInternetConnection())
            {
                try
                {
                    strJSON = HttpHelper.DownloadUrl(geoUrl);
                }// end of try block
                catch (IOException e)
                {
                    LogMessage(LogLevel.SEVERE, e.Message,
                        "UtilityMethod::RetrieveGeoNamesGeoLocationUsingAddress [line: " +
                        $"{UtilityMethod.GetExceptionLineNumber(e)}]");                    
                }// end of catch block

            }// end of if block
            else
            {
                ShowMessage("No Internet Connection.");
            }// end of else block

            // Return the data from specified url
            return strJSON;

        }// end of method RetrieveGeoNamesGeoLocationUsingAddress

        /// <summary>
        /// Uses the Geonames web service to return the geographical location of a city using it's coordinates.
        /// </summary>
        /// <param name="lat">The line of latitude that the city is located</param>
        /// <param name="lng">The line of longitude that the city is located</param>
        /// <returns>A <seealso cref="string"/> representation of a JSON <seealso cref="Object"/> returned from the web service.</returns>
        public static string RetrieveGeoNamesGeoLocationUsingCoordinates(float lat, float lng)
        {
            string strJSON = null;
            string geoUrl =
                    "http://api.geonames.org/findNearbyPlaceNameJSON?" +
                    "lat=" + lat +
                    "&lng=" + lng +
                    "&username=" + WidgetUpdateService.geoNameAccount;

            if (HasInternetConnection())
            {
                try
                {
                    strJSON = HttpHelper.DownloadUrl(geoUrl);
                }// end of try block
                catch (IOException e)
                {
                    LogMessage(LogLevel.SEVERE, e.Message,
                        "UtilityMethod::RetrieveGeoNamesGeoLocationUsingAddress [line: " +
                        $"{UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block

            }// end of if block
            else
            {
                ShowMessage("No Internet Connection.");
            }// end of else block

            // Return the data from specified url
            return strJSON;
        }// end of method RetrieveGeoNamesGeoLocationUsingCoordinates

        /// <summary>
        ///  Uses the Here Maps web service to return the geographical location of a city using it's name.
        /// </summary>
        /// <param name="wxLocation">The location of the city to be found.</param>
        /// <returns>A <see cref="string"/> representation of a JSON <see cref="object"/> returned from the web service.</returns>
        public static string RetrieveHereGeoLocationUsingAddress(string wxLocation)
        {
            // All spaces must be replaced with the + symbols for the HERE Maps web service
            if (wxLocation.Contains(" "))
            {
                wxLocation = wxLocation.Replace(" ", "+");
            }// end of if block

            // All commas must be replaced with the + symbols for the HERE Maps web service
            if (wxLocation.Contains(","))
            {
                wxLocation = wxLocation.Replace(",", "+");
            }// end of if block

            string strJSON = null;
            string geoUrl =
                    "https://geocoder.api.here.com/6.2/geocode.json?"
                    + "app_id=" + WidgetUpdateService.hereAppId
                    + "&app_code=" + WidgetUpdateService.hereAppCode
                    + "&searchtext=" + EscapeUriString(wxLocation.ToLower());

            if (HasInternetConnection())
            {
                try
                {
                    strJSON = HttpHelper.DownloadUrl(geoUrl);
                }// end of try block
                catch (IOException e)
                {
                    LogMessage(LogLevel.SEVERE, e.Message,
                        "UtilityMethod::retrieveHereGeoLocationUsingAddress [line: " +
                        $"{UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block

            }// end of if block
            else
            {
                ShowMessage("No Internet Connection.");
            }// end of else block

            // Return the data from specified url
            return strJSON;
        }// end of method RetrieveHereGeoLocationUsingAddress

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        /// <seealso href="https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp/24199315#24199315">Stack Overflow</seealso>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }// end of method ResizeImage

        /// <summary>
        /// Retrieves weather information from a specific weather provider's web service URL.
        /// </summary>
        /// <param name="wxUrl">The providers web service URL</param>
        /// <returns>A <see cref="string"/> representation of a JSON <see cref="object"/> returned from the web service.</returns>
        public static string RetrieveWeatherData(string wxUrl)
        {
            string strJSON = null;

            try
            {
                strJSON = HttpHelper.DownloadUrl(wxUrl);
            }// end of try block
            catch (Exception e)
            {
                LogMessage(LogLevel.SEVERE, "No data returned from " + wxUrl,
                        $"UtilityMethod::retrieveWeatherData  [line: {UtilityMethod.GetExceptionLineNumber(e)}]");

            }// end of catch block

            return strJSON;
        }// end of method RetrieveWeatherData

        /// <summary>
        /// Round a value to 2 decimal places  
        /// </summary>
        /// <param name="value">The value to be rounded</param>
        /// <remarks>The value must be of the double data type.</remarks>
        /// <returns>The rounded value to 2 decimal places</returns>
        public static float RoundValue(double value)
        {
            float rounded = (float)Math.Round(value);

            return float.Parse(rounded.ToString("0.##"), CultureInfo.InvariantCulture.NumberFormat);
        }// end of method RoundValue

        /// <summary>
        /// Saves a file to a specified location on disc.  
        /// </summary>
        /// <param name="fileName">The name of the file to be stored.</param>
        /// <param name="path">The path to the specified file.</param>
        /// <param name="content">The content to be stored in the specified file</param>
        public static void SaveToFile(string fileName, string path, string content)
        {
            string pathString = path + fileName;
            bool append = File.Exists(pathString);                   

            using (var sWriter = new StreamWriter(pathString, append))
            {
                sWriter.WriteLine(content);
            }// end of using block
        }// end of method SaveToFile

        /// <summary>
        /// Sets the transparancy of an image
        /// </summary>
        /// <param name="image">The image to be made transparent</param>
        /// <param name="opacity">The level of opacity to be applied to the image.</param>
        /// <returns>An image with transparency added.</returns>
        public static Image SetImageOpacity(Image image, float opacity)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                ColorMatrix matrix = new ColorMatrix
                {
                    Matrix33 = opacity
                };
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default,
                                                  ColorAdjustType.Bitmap);
                g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height),
                                   0, 0, image.Width, image.Height,
                                   GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }// end of method SetImageOpacity

        /// <summary>
        /// Attempts to calculate the size of a file in <see cref="byte"/>s
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>A <see cref="long"/> representing the size of the file in <see cref="byte"/>s</returns>
        public static long Size(string filePath)
        {
            return new FileInfo(filePath).Length;
        }// end of method 

        public static void ShowMessage(string msg, IWin32Window owner = null, string title = WeatherLionMain.PROGRAM_NAME,
            MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon mbIcon = MessageBoxIcon.Information)
        {
            MessageBox.Show(owner, msg, title, buttons, mbIcon);                     
        }// end of method ShowMessage

        /// <summary>
        /// Subtracts one time value from another.
        /// </summary>
        /// <param name="firstPeriod">The first time value</param>
        /// <param name="secondPeriod">The second time value</param>
        /// <returns>The result of the subtraction expressed as a <see cref="long"/></returns>
        public static long SubtractTime(DateTime firstPeriod, DateTime secondPeriod)
        {
            return (long) firstPeriod.Subtract(secondPeriod).TotalMilliseconds;
        }// end of method SubtractTime

        /// <summary>
        /// Returns an RGB Color which corresponds with a temperature
        /// </summary>
        /// <param name="t">The temperature to be tested</param>
        /// <returns>A new <see cref="Color"/></returns>
        public static Color TemperatureColor(int t)
        {
            Color c;
            t = WeatherLionMain.storedPreferences.StoredPreferences.UseMetric
                ? (int) CelsiusToFahrenheit(t)
                : t;

            if (t <= 40)
            {
                c = Color.FromArgb(45, 99, 252); // Cold
            }// end of if block
            else if (Math.Abs(t - 60) + Math.Abs(41 - t) == Math.Abs(41 - 60))
            {
                c = Color.FromArgb(151, 205, 251); // Chilly
            }// end of else if block	
            else if (Math.Abs(t - 70) + Math.Abs(84 - t) == Math.Abs(84 - 70))
            {
                c = Color.FromArgb(152, 211, 0); // Warm
            }// end of else if block		
            else if (t > 85)
            {
                c = Color.FromArgb(253, 0, 3); // Hot
            }// end of else if block
            else
            {
                c = Color.FromArgb(0, 172, 74); // Normal
            }// end of else block

            return c;
        }// end of method TemperatureColor

        /// <summary>
        /// Determines whether or not a connectivity check needs to be performed.
        /// </summary>
        /// <returns>A <see cref="bool"/> value of true/false dependent on the outcome of the test.</returns>
        public static bool TimeForConnectivityCheck()
        {
            int interval = WeatherLionMain.storedPreferences.StoredPreferences.Interval;
            long minutesToGo = MillisecondsToMinutes(interval);
            bool ready = false;

            if (lastUpdated != null && !UpdateRequired())
            {
               DateTime nextUpdateDue = lastUpdated.AddMinutes(MillisecondsToMinutes(interval));

                //milliseconds
                long difference = nextUpdateDue.Ticks - DateTime.Now.Ticks;

                long secondsInMilli = 1000;
                long minutesInMilli = secondsInMilli * 60;
                minutesToGo = difference / minutesInMilli;

                ready = minutesToGo <= 1;
            }// end of if block
            else if (UpdateRequired() || lastUpdated != null)
            {
                ready = true;
            }// end of else if block

            return ready;
        }// end of method TimeForConnectivityCheck

        /// <summary>
        /// Converts a sequence to alphabetic characters to sentence case.
        /// </summary>
        /// <param name="str">The string value to be converted.</param>
        /// <remarks>Value must be a <see cref="string"/> to be converted without numbers.</remarks>
        /// <returns>A <see cref="string"/> representation of text in a sentence case format.</returns>
        public static string ToProperCase(string str)
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
                        char.ToString(sequence[i - 1]).Equals(sep[1]) ||
                        char.ToString(sequence[i - 1]).Equals(sep[3])))
                {
                    sequence[i] = char.ToUpper(sequence[i]);
                }// end of else if block
            }// end of for loop

            return sequence.ToString();
        }// end of method ToProperCase

        /// <summary>
        /// Determine if the widget needs to be refreshed based on the specified refresh period.
        /// </summary>
        /// <returns>True/False depending on the result of the check.</returns>
        public static bool UpdateRequired()
        {
            if (lastUpdated == null)
            {
                return true;
            }// end of if block

            if (refreshRequested)
            {
                return true;
            }// end of if block

            int interval = WeatherLionMain.storedPreferences.StoredPreferences.Interval;

            //milliseconds
            long difference = Math.Abs( DateTime.Now.Ticks - lastUpdated.Ticks );

            long secondsInMilli = 1000;
            long minutesInMilli = secondsInMilli * 60;

            long elapsedMinutes = difference / minutesInMilli;
            difference = difference % minutesInMilli;

            if (elapsedMinutes >= interval)
            {
                return true;
            }// end of if block
            else
            {
                return false;
            }// end of else block
        }// end of method UpdateRequired

        /// <summary>
        /// Ensures that the city entered by the user is correctly formatted.
        /// </summary>
        /// <param name="cityName"> The 24hr clock hour time value</param>
        /// <example>Pine Hills, FL</example>
        /// <returns>A boolean value of True/False dependent on the result of the test.</returns>
        public static bool ValidCityName(ref string cityName)
        {
            if (!cityName.Contains(","))
            {
                return false;
            }// end of if block

            cityName.ToProperCase();

            return true;
        }// end of method ValidCityName

        /// <summary>
        /// Returns a valid weather condition that is relevant to the application
        /// </summary>
        /// <param name="condition">The weather condition to be validated</param>
        /// <returns>A <see cref="string"/> value representing a valid weather condition</returns>
        public static string ValidateCondition(string condition)
        {
            condition = condition.ToLower();

            if (condition.Contains("until"))
            {
                condition = condition.Substring(0, condition.IndexOf("until") - 1).Trim();
            }// end of if block

            if (condition.Contains("starting"))
            {
                condition = condition.Substring(0, condition.IndexOf("starting") - 1).Trim();
            }// end of if block

            if (condition.Contains("overnight"))
            {
                condition = condition.Substring(0, condition.IndexOf("overnight") - 1).Trim();
            }// end of if block

            if (condition.Contains("night"))
            {
                condition = condition.ReplaceAll("night", "").Trim();
            }// end of if block

            if (condition.Contains("possible"))
            {
                condition = condition.ReplaceAll("possible", "").Trim();
            }// end of if block

            if (condition.Contains("throughout"))
            {
                condition = condition.Substring(0, condition.IndexOf("throughout") - 1).Trim();
            }// end of if block

            if (condition.Contains(" in "))
            {
                condition = condition.Substring(0, condition.IndexOf(" in ") - 1).Trim();
            }// end of if block

            if (condition.ToLower().Contains("and"))
            {
                string[] conditions = condition.ToLower().Split(new string[] { "and" },
                    StringSplitOptions.None);

                condition = conditions[0].Trim();
            }// end of if block

            if (condition.ToLower().Contains("(day)"))
            {
                condition = condition.Replace("(day)", "").Trim();
            }// end of if block
            else if (condition.ToLower().Contains("(night)"))
            {
                condition = condition.Replace("(night)", "").Trim();
            }// end of if block

            // create a new method for locating the nearest match
            condition = FindClosestWordMatch( weatherImages.Keys.Cast<string>().ToArray(), condition);

            return condition.ToProperCase();
        }// end of method ValidateCondition

        #endregion

        #region Web Service Handlers

        public static void FindYahooCity(string cityName, ref CityData cityData)
        {
            string cityUrl =
             $"https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20geo.places%20where%20text%3D%22" +
             $"{Uri.EscapeUriString(cityName.ToLower())}" +
             $"%22&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";

            if (HasInternetConnection())
            {
                try
                {
                    WebRequest webRequest = (HttpWebRequest)WebRequest.Create(cityUrl);
                    string serviceData = null;

                    using (var response = webRequest.GetResponse())
                    {
                        using (StreamReader reader =
                                           new StreamReader(response.GetResponseStream()))
                        {
                            serviceData = reader.ReadToEnd();
                        }// end of inner using
                    }// end of outer using             

                    if (serviceData != null)
                    {
                        CityData.DeserializeCityJSON(serviceData, ref cityData);
                    }// end of if block
                }// end of try block
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotFound)
                        {

                        }// end of if block
                        else
                        {

                        }// end of else block
                    }// end of if block               
                }// end of catch block
                catch (Exception)
                {
                }// end of catch block 
            }// end of if block
            else
            {
                ShowMessage("No Internet Connection.", PreferencesForm.ActiveForm,
                    WeatherLionMain.PROGRAM_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }// end of else block
        }// end of method FindYahooCity

        #endregion
    }// end of class UtilityMethod
}// end of namespace WeatherLion
