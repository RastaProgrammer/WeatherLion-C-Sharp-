using System;
using System.Threading;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          CityStorageService
///   Description:    This class is used as a background service for 
///                   saving information received from a webservice
///                   to local storage.
///   Author:         Paul O. Patterson     Date: June 29, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// A model for writing data regarding a specific world location
    /// locally.
    /// </summary>
    public class CityStorageService
    {
        private Thread thread;
        private const string TAG = "CityStorageService";

        public int index { get; set; }
        public string city { get; set; }
        public string log { get; set; }

        public CityStorageService()
        {
        }// end of default constructor

        public CityStorageService(int listIndex, string cityName)
        {
            index = listIndex;
            city = cityName;
        }// end of one-argument constructor

        protected void DoInBackground()
        {
            if (city.Trim().Length > 0 &&
                !UtilityMethod.IsKnownCity(city))
            {
                UtilityMethod.FindGeoNamesCity(city, null);
                Thread.Sleep(2000); // Wait 2 seconds for the CityDataService thread to be complete.
                StoreNewLocationLocally();
            }// end of if block

            log = $"{city} was successfully saved to local storage.";
            Done();
        }// end of method DoInBackground

        protected void Done()
        {
            UtilityMethod.LogMessage("info", log, "CityStorageService::Done");
        }// end of method Done

        /// <summary>
        /// Executes the <see cref="Thread"/>.
        /// </summary>
        public void Run()
        {
            thread = new Thread(DoInBackground);
            thread.Start();
        }// end of method Run

        /// <summary>
        /// Add new location locally if it does not already exists
        /// </summary>
        private void StoreNewLocationLocally()
        {
            string[] searchCity = city.Split(',');
            
            foreach (GeoNamesGeoLocation.GeoNames foundCity in GeoNamesGeoLocation.cityGeographicalData
                .geonames)
            {
                if (foundCity.name.Equals(searchCity[0].Trim().ToLower(), StringComparison.OrdinalIgnoreCase) &&
                        foundCity.adminCodes1.iso.Equals(searchCity[1].Trim(), StringComparison.OrdinalIgnoreCase) &&
                            !UtilityMethod.IsNumeric(foundCity.adminCodes1.iso) ||
                                foundCity.countryName.Equals(searchCity[1].Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    string cityName = UtilityMethod.ToProperCase(foundCity.name);
                    string countryName = UtilityMethod.ToProperCase(foundCity.countryName);
                    string countryCode = foundCity.countryCode.ToUpper();
                    string regionName = UtilityMethod
                            .ToProperCase(foundCity.adminName1);

                    string regionCode = null;
                    regionCode = foundCity.adminCodes1.iso?.ToUpper();

                    float Latitude = float.Parse(foundCity.lat);
                    float Longitude = float.Parse(foundCity.lng);

                    CityData cityData = new CityData(cityName, countryName, countryCode,
                            regionName, regionCode, Latitude, Longitude);

                    string currentCity = regionCode != null ? $"{cityName}, {regionCode}" : $"{cityName}, {countryName}";

                    if (!UtilityMethod.IsFoundInDatabase(currentCity))
                    {
                        UtilityMethod.AddCityToDatabase(cityName, countryName, countryCode,
                                regionName, regionCode, Latitude, Longitude);
                    }// end of if block

                    if (!UtilityMethod.IsFoundInJSONStorage(currentCity))
                    {
                        JSONHelper.ExportToJSON(cityData);
                    }// end of if block

                    if (!UtilityMethod.IsFoundInXMLStorage(currentCity))
                    {
                        XMLHelper.ExportToXML(cityData);
                    }// end of if block
                }// end of if block
            }// end of for each loop            
        }// end of method StoreNewLocationLocally
    }// end of class CityStorageService
}// end of namespace WeatherLion
