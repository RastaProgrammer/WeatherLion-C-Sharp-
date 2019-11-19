using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          PreferencesForm
///   Description:    This class is responsible allowing the user to
///                   update certain aspect of the widget UI as well
///                   as the weather data provider.
///   Author:         Paul O. Patterson     Date: October 03, 2017
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// Allows the user's to modifiy the operation of the widget and it's UI elements.
    /// </summary>
    public partial class PreferencesForm : Form
    {
        private static readonly string IMAGE_PATH = $@"{AppDomain.CurrentDomain.BaseDirectory}res\assets\img\";
        private static readonly string DEFAULT_BACKGROUND_IMAGE = $@"{IMAGE_PATH}\backgrounds\default_bg.png";
        private static readonly string ANDROID_BACKGROUND_IMAGE = $@"{IMAGE_PATH}\backgrounds\android_bg.png";
        private static readonly string RABALAC_BACKGROUND_IMAGE = $@"{IMAGE_PATH}\backgrounds\rabalac_bg.png";

        private static PreferencesForm frmPreference = null;
        public static bool locationSelected;

        public static string currentCity;
        public static StringBuilder searchCity = new StringBuilder();
        public static string[] cityNames;

        private const string TAG = "PreferencesForm";

        public PreferencesForm()
        {
            InitializeComponent();
        }

        #region Form Events

        private void frmPreferences_Activated(object sender, EventArgs e)
        {
            cboLocation.Text = WeatherLionMain.storedPreferences.StoredPreferences.Location;
        }// end of method frmPreferences_Activated

        private void frmPreferences_Load(object sender, EventArgs e)
        {
            frmPreference = this;

            flpIconSet.Controls.Clear();    // remove any previous icon set used and reload at runtime

            LoadInstalledIconPacks();

            // Weather tab
            #region Test Data (un-comment lines for testing)
            //WeatherLionMain.authorizedProviders = new string[] {
            //        WeatherLionMain.DARK_SKY, WeatherLionMain.OPEN_WEATHER,
            //        WeatherLionMain.WEATHER_BIT, WeatherLionMain.YAHOO_WEATHER };

            //WeatherLionMain.storedPreferences.StoredPreferences.Provider = WeatherLionMain.DARK_SKY;
            //WeatherLionMain.storedPreferences.StoredPreferences.Location = "Pine Hills, FL";
            //WeatherLionMain.storedPreferences.StoredPreferences.Interval = 1800000;
            //WeatherLionMain.storedPreferences.StoredPreferences.UseMetric = false;
            //WeatherLionMain.storedPreferences.StoredPreferences.UseSystemLocation = false;
            //WeatherLionMain.storedPreferences.StoredPreferences.WidgetBackground = "default";
            //WeatherLionMain.storedPreferences.StoredPreferences.IconSet = "hero";
            //List<string> preferenceUpdated = new List<string>();

            #endregion

            // load the combo box with the default weather providers
            cboWeatherProviders.Items.Clear(); // refresh the list
            cboWeatherProviders.Items.AddRange(WeatherLionMain.authorizedProviders);

            // select current weather provider
            cboWeatherProviders.SelectedItem = WeatherLionMain.storedPreferences.StoredPreferences.Provider;

            // select the current weather provider
            cboLocation.Text = WeatherLionMain.storedPreferences.StoredPreferences.Location;

            // select the current refresh period
            string updateInterval = UtilityMethod.MillisecondsToMinutes(
                WeatherLionMain.storedPreferences.StoredPreferences.Interval).ToString();

            lblInterval.Text = $"{updateInterval} min.";
            cboRefreshInterval.SelectedItem = updateInterval;

            chkUseMetric.Checked = WeatherLionMain.storedPreferences.StoredPreferences.UseMetric;

            chkUseSystemLocation.Checked = WeatherLionMain.storedPreferences.StoredPreferences.UseSystemLocation;

            // load the three background options
            picDefault.ImageLocation = DEFAULT_BACKGROUND_IMAGE;
            picAndroid.ImageLocation = ANDROID_BACKGROUND_IMAGE;
            picRabalac.ImageLocation = RABALAC_BACKGROUND_IMAGE;

            // Icon Set tab
            foreach (Control c in tabIconSet.Controls)
            {
                if (c is FlowLayoutPanel)
                {
                    foreach (Control sc in c.Controls)
                    {
                        if (sc is FlowLayoutPanel)
                        {
                            foreach (Control tc in sc.Controls)
                            {
                                if (tc is RadioButton)
                                {
                                    RadioButton rb = tc as RadioButton;                                   

                                    rb.CheckedChanged += new EventHandler(IconSetChanged);
                                }// end of if block
                                else if (tc is PictureBox)
                                {
                                    PictureBox pb = tc as PictureBox;
                                    
                                    pb.Click += new EventHandler(SelectIconSetRadioButton);
                                }// end of if block
                            }// end of for each loop
                        }// end of if block
                    }// end of inner foreach block
                }// end of if block                     
            }// end of foreach block

            // Background tab
            // Make the radio buttons on each page mutually exclusive
            foreach (Control c in tabBackground.Controls)
            {
                if (c is FlowLayoutPanel)
                {
                    foreach (Control sc in c.Controls)
                    {
                        if (sc is RadioButton)
                        {
                            RadioButton rb = sc as RadioButton;
                            
                            rb.CheckedChanged += new EventHandler(BackgroundChanged);
                        }// end of if block
                        else if(sc is PictureBox)
                        {
                            PictureBox pb = sc as PictureBox;
                            
                            pb.Click += new EventHandler(SelectBackgroundRadioButton);
                        }// end of if block
                    }// end of inner foreach block
                }// end of if block                     
            }// end of foreach block   

            switch (WeatherLionMain.storedPreferences.StoredPreferences.WidgetBackground)
            {
                case "default":
                    radDefault.Checked = true;
                    break;
                case "android":
                    radAndroid.Checked = true;
                    break;
                case "rabalac":
                    radRabalac.Checked = true;
                    break;
                default:
                    break;
            }// end of switch block                

            // About tab
            rtbAbout.Rtf = @"{\rtf1\pc \qc \b Weather Lion\b0" +
                @"\line Author: Paul O. Patterson" +
                @"\line BushBungalo Productions™  2005 - " + DateTime.Now.Year +
                @"\line Version: 1.0" +
                @"\line © All rights reserved" +
                @"\line\line\ql\par Weather Lion is an ongoing effort to create a desktop weather widget " +
                @"using the C# programming language as well as other languages as I " +
                @"continue to grow as a computer programmer.\par0" +
                @"\line\line\line Praise Ye YAH!!!}";

            // ensure that the changes list is empty (comment out during testing)
            WeatherLionMain.runningWidget.preferenceUpdated.Clear();

            LoadKnownPlaces();
        }// end of method frmPreferences_Load          

        /// <summary>
        /// Ensures that only one radio button is check on a page. This approach is
        /// taken due to the fact that each radio button is in a separate parent container.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> that triggered the event</param>
        /// <param name="e">The event that occured</param>
        private void BackgroundChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rb)
            {
                if (rb.Checked)
                {
                    foreach (Control c in tabBackground.Controls)
                    {
                        if (c is FlowLayoutPanel)
                        {
                            foreach (Control sc in c.Controls)
                            {
                                if (sc is RadioButton)
                                {
                                    RadioButton sb = sc as RadioButton;

                                    if (!sb.Name.Equals(rb.Name))
                                    {
                                        sb.Checked = false;
                                    }// end of if block
                                }// end of if block
                            }// end of inner foreach block
                        }// end of if block                     
                    }// end of foreach block 
                }// end of if block
            }// end of if block
        }// end of method BackgroundChanged

        /// <summary>
        /// Ensures that only one radio button is check on a page. This approach is
        /// taken due to the fact that each radio button is in a separate parent container.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> that triggered the event</param>
        /// <param name="e">The event that occured</param>
        private void IconSetChanged(object sender, EventArgs e)
        {
            string packName = null;

            if (sender is RadioButton rb)
            {
                if (rb.Checked)
                {
                    packName = (string)rb.Tag;

                    foreach (Control c in tabIconSet.Controls)
                    {
                        if (c is FlowLayoutPanel)
                        {
                            foreach (Control mc in c.Controls)
                            {
                                if (mc is FlowLayoutPanel)
                                {
                                    foreach (Control ic in mc.Controls)
                                    {
                                        if (ic is RadioButton)
                                        {
                                            RadioButton sb = ic as RadioButton;

                                            if (!sb.Name.Equals(rb.Name))
                                            {
                                                sb.Checked = false;
                                            }// end of if block
                                        }// end of if block
                                    }// end of second inner for each loop
                                }// end of if block                                
                            }// end of first inner foreach block
                        }// end of if block                     
                    }// end of outter foreach block 
                }

                // comment out during testing
                if (!WeatherLionMain.storedPreferences.StoredPreferences.Equals(packName))
                {
                    if (WeatherLionMain.runningWidget.preferenceUpdated.ContainsKey(
                        WeatherLionMain.ICON_SET_PREFERENCE))
                    {
                        WeatherLionMain.runningWidget.preferenceUpdated[WeatherLionMain.ICON_SET_PREFERENCE] =
                            packName;
                    }// end of if block
                    else
                    {
                        WeatherLionMain.runningWidget.preferenceUpdated.Add(
                            WeatherLionMain.ICON_SET_PREFERENCE, packName);
                    }// end of else block                                      
                }// end of if block
            }// end of if block
        }// end of method IconSetChanged

        // Load all available icon packs
        private void LoadInstalledIconPacks()
        {
            if (WeatherLionMain.iconPackList.Count > 0)
            {
                WeatherLionMain.iconPackList.Clear();
                WeatherLionMain.iconSetControls.Clear();
                WeatherLionMain.iconPacksLoaded = false;
            }// end of if block

            WeatherLionMain.iconPackList =
                UtilityMethod.GetSubdirectories("res/assets/img/weather_images");           

            WeatherLionMain.iconSetControls = new Hashtable();

            // Add icon packs dynamically
            foreach (string packName in WeatherLionMain.iconPackList)
            {
                FlowLayoutPanel iconSelectionContainer = new FlowLayoutPanel
                {
                    Size = new Size(140, 148)
                };

                //iconSelectionContainer.BorderStyle = BorderStyle.FixedSingle;
                var margin = iconSelectionContainer.Margin;
                margin.Bottom = 10;
                iconSelectionContainer.Margin = margin;

                Label packTitle = new Label
                {
                    AutoSize = false,
                    Text = UtilityMethod.ToProperCase(packName),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(iconSelectionContainer.Width, 20),
                    Font = new Font("Arial", 11, FontStyle.Bold)
                };

                string wiPath = $@"{AppDomain.CurrentDomain.BaseDirectory}res\assets\img\weather_images\";
                bool previewImageExists = File.Exists($@"{wiPath}{packName}\preview_image.png");
                string displayIcon = previewImageExists ? "preview_image.png" : "weather_10.png";
                string wxIcon = $@"{wiPath}{packName}\{displayIcon}";

                PictureBox packDefaultImage = new PictureBox
                {
                    ImageLocation = wxIcon,
                    Name = $"pic{UtilityMethod.ToProperCase(packName)}",
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(iconSelectionContainer.Width, 100),
                    Tag = packName.ToLower()
                };

                RadioButton iconSelector = new RadioButton
                {
                    AutoSize = false,
                    BackColor = Color.Transparent,
                    CheckAlign = ContentAlignment.MiddleCenter,
                    Name = $"rad{UtilityMethod.ToProperCase(packName)}",
                    Size = new Size(iconSelectionContainer.Width, 14),
                    Tag = packName.ToLower()
                };

                iconSelector.CheckedChanged += new EventHandler(IconSetChanged);

                iconSelectionContainer.Controls.Add(packTitle);
                iconSelectionContainer.Controls.Add(packDefaultImage);
                iconSelectionContainer.Controls.Add(iconSelector);

                // Add icon selections to FlowLayoutPanel
                flpIconSet.Controls.Add(iconSelectionContainer);

                List<Control> components = new List<Control>
                {
                    packTitle,  // Add the component that displays the icon pack title
                    packDefaultImage, // Add the component that displays the icon pack default image
                    iconSelector // Add the component that displays the radio button to select the pack
                };

                WeatherLionMain.iconSetControls.Add(packName, components);
            }// end of foreach loop

            //string set = "hero"; //(un-comment during testing)
            string set = WeatherLionMain.storedPreferences.StoredPreferences.IconSet;

            ((RadioButton)
                ((List<Control>)
                    WeatherLionMain.iconSetControls[set])[2]).Checked = true;

            WeatherLionMain.iconPacksLoaded = true;
            StringBuilder packs = new StringBuilder(string.Join(", ", WeatherLionMain.iconPackList.ToArray()));
            packs.Insert(packs.ToString().LastIndexOf(",") + 1, " and");
            UtilityMethod.LogMessage(UtilityMethod.LogLevel.INFO, $"Icon Packs Installed: {packs.ToString()}",
                $"{TAG}::LoadInstalledIconPacks");
        }// end of method LoadInstalledIconPacks
                
        #endregion

        #region Button Events
        private void btnOk_Click(object sender, EventArgs e)
        {
            // apply the location setting
            SaveLocationPreference();

            // get rid of the form     
            Close();
        }// end of method btnOk_Click

        private void btnCancel_Click(object sender, EventArgs e)
        {
            WeatherLionMain.runningWidget.applyPreferenceUpdates = true;
            Close();
        }// end of method btnCancel_Click

        private void btnApply_Click(object sender, EventArgs e)
        {
            // this flag will be updated after loading success (comment this line during form testing)
            WeatherLionMain.runningWidget.dataLoadedSuccessfully = false;

            // this flag will be updated after loading success (comment this line during form testing)
            WeatherLionMain.runningWidget.applyPreferenceUpdates = true;

            SaveLocationPreference();
        }// end of method btnApply_Click  

        #endregion

        #region Combo Box Events
        private void cboRefreshInterval_SelectedValueChanged(object sender, EventArgs e)
        {
            int selectedInterval = int.Parse(cboRefreshInterval.SelectedItem.ToString()).MinutesToMilliseconds();

            if (selectedInterval != WeatherLionMain.storedPreferences.StoredPreferences.Interval)
            {
                // notify the widget of this update
                if (!WeatherLionMain.runningWidget.preferenceUpdated.ContainsKey(
                        WeatherLionMain.UPDATE_INTERVAL))
                {
                    WeatherLionMain.runningWidget.preferenceUpdated.Add(WeatherLionMain.UPDATE_INTERVAL,
                        UtilityMethod.MinutesToMilliseconds(int.Parse(cboRefreshInterval.Text)).ToString());
                }// end of if block

                lblInterval.Text = $"{cboRefreshInterval.Text} min.";              

            }// end of if block            
        }// end of method cboRefreshInterval_SelectedValueChanged

        private void cboWeatherProviders_SelectedValueChanged(object sender, EventArgs e)
        {
            string selectedProvider = cboWeatherProviders.Text;

            if (!selectedProvider.Equals(WeatherLionMain.storedPreferences.StoredPreferences.Provider))
            {
                // notify the widget of this update
                if (!WeatherLionMain.runningWidget.preferenceUpdated.ContainsKey(
                        WeatherLionMain.WEATHER_SOURCE_PREFERENCE))
                {
                    WeatherLionMain.runningWidget.preferenceUpdated.Add(
                            WeatherLionMain.WEATHER_SOURCE_PREFERENCE, cboWeatherProviders.Text);
                }// end of if block

                WeatherLionMain.runningWidget.previousWeatherProvider.Clear(); // clear the string
                WeatherLionMain.runningWidget.previousWeatherProvider.Append(
                    WeatherLionMain.storedPreferences.StoredPreferences.Provider);               
            }// end of if block
        }// end of method cboWeatherProviders_SelectedValueChanged

        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Image == null)
            {
                string loadingImage = $@"{AppDomain.CurrentDomain.BaseDirectory}res\assets\img\icons\ajax-loader.gif";
                btnSearch.Image = Image.FromFile(loadingImage);
                btnSearch.Text = "Searching...";

                searchCity.Clear(); // clear any previous searches
                searchCity.Append(cboLocation.Text);

                if (searchCity.ToString().Trim().Length > 0 &&
                    !UtilityMethod.IsKnownCity(searchCity.ToString()))
                {
                    // ignore anything that comes after a comma
                    if (searchCity.ToString().Contains(","))
                    {                        
                        searchCity.Remove(searchCity.ToString().IndexOf(","), 
                            searchCity.Length - searchCity.ToString().IndexOf(","));
                    }// end of if block

                   UtilityMethod.FindGeoNamesCity(searchCity.ToString(), frmPreference);
                }// end of if block
            }// end of if block                    
        }// end of method btnSearch_Click

        /// <summary>
        /// Load a list of previous place that were searched for
        /// </summary>
        private void LoadKnownPlaces()
        {
            List<CityData> previousSearches = JSONHelper.ImportFromJSON();
            List<string> searchList = new List<string>();

            // when the program is first runs there will be no previous searches so
            // this function does nothing on the first run
            if (previousSearches != null)
            {
                foreach (CityData city in previousSearches)
                {
                    if (city.regionCode != null && !UtilityMethod.IsNumeric(city.regionCode))
                    {
                        searchList.Add($"{city.cityName}, {city.regionCode}");
                    }// end of if block
                    else
                    {
                        searchList.Add($"{city.cityName}, {city.countryName}");
                    }// end of else block
                }// end of for each loop

                searchList.Sort();
                string[] s = searchList.ToArray();
                cboLocation.Items.AddRange(s);
                var source = new AutoCompleteStringCollection();
                source.AddRange(s);

                // Set the locations combo box to auto complete
                cboLocation.Items.Clear();                
                cboLocation.AutoCompleteCustomSource = source;           
                cboLocation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboLocation.AutoCompleteSource = AutoCompleteSource.CustomSource;                
                
                // select the first of the previous weather locations if one
                // was'nt stored in the preferences		
                if (WeatherLionMain.storedPreferences.StoredPreferences.Location == null)
                {
                    cboLocation.SelectedIndex = 0;
                }// end of if block
                else
                {
                    cboLocation.Text = WeatherLionMain.storedPreferences.StoredPreferences.Location;
                }// end of else block
            }// end of if block        
        }// end of method LoadKnownPlaces         

        /// <summary>
        /// Save the new location to the user's preference file.
        /// </summary>
        private void SaveLocationPreference()
        {
            string currentLocation = null;

            // the location setting is the only one that does not get written
            // to the preferences files automatically so the user has to do this
            // explicitly but clicking OK or Apply.		
            if (!cboLocation.Text.Equals(WeatherLionMain.storedPreferences.StoredPreferences.Location)
                && cboLocation.Text.Trim().Length > 0)
            {
                // combine the city and the region as the current location
                string[] location = cboLocation.Text.ToString().Split(',');

                if (location.Length > 0)
                {
                    // countries who have a region such as a state or municipality
                    if (location.Length > 2)
                    {
                        currentLocation = location[0].Trim() + ", " + location[1].Trim();
                    }// end of if block
                    else
                    {
                        currentLocation = cboLocation.Text.ToString().Trim();
                    }// end of else block

                    // notify the widget of this update (comment out code during form testing)
                    if (!WeatherLionMain.runningWidget.preferenceUpdated.ContainsKey(
                            WeatherLionMain.CURRENT_LOCATION_PREFERENCE))
                    {
                        WeatherLionMain.runningWidget.preferenceUpdated.Add(
                                WeatherLionMain.CURRENT_LOCATION_PREFERENCE, currentLocation);
                        locationSelected = true;
                    }// end of if block

                    if (WeatherLionMain.runningWidget != null)
                    {
                        if (!WeatherLionMain.runningWidget.Visible)
                        {
                            WeatherLionMain.runningWidget.Visible = true;
                        }// end of if block
                    }// end of if block
                }// end of if block
            }// end of if block
            else
            {
                currentLocation = cboLocation.Text;
            }// end of else block

            if (!UtilityMethod.IsFoundInJSONStorage(currentLocation))
            {
                //run an background service
                CityStorageService cs = new CityStorageService(cboLocation.SelectedIndex,
                    cboLocation.Text);
                cs.Run();
            }// end of if block
        }// end of method SaveLocationPreference        

        private void chkUseSystemLocation_CheckedChanged(object sender, EventArgs e)
        {
            cboLocation.Enabled = !chkUseSystemLocation.Checked;

            // notify the widget of this update
            if (!WeatherLionMain.runningWidget.preferenceUpdated.ContainsKey(
                    WeatherLionMain.USE_SYSTEM_LOCATION_PREFERENCE))
            {
                WeatherLionMain.runningWidget.preferenceUpdated.Add(
                    WeatherLionMain.USE_SYSTEM_LOCATION_PREFERENCE, 
                    chkUseSystemLocation.Checked.ToString());
            }// end of if block            
        }// end of method chkUseSystemLocation_CheckedChanged

        private void chkUseMetric_CheckedChanged(object sender, EventArgs e)
        {
            // notify the widget of this update
            if (!WeatherLionMain.runningWidget.preferenceUpdated.ContainsKey(
                    WeatherLionMain.USE_METRIC_PREFERENCE))
            {               
                WeatherLionMain.runningWidget.preferenceUpdated.Add(
                    WeatherLionMain.USE_METRIC_PREFERENCE, chkUseMetric.Checked.ToString());
            }// end of if block           
        }// end of method chkUseMetric_CheckedChanged

        private void radDefault_CheckedChanged(object sender, EventArgs e)
        {
            UpdateWidgetBackgroundPrefs((RadioButton)sender);
        }// end of method 

        private void radAndroid_CheckedChanged(object sender, EventArgs e)
        {
            UpdateWidgetBackgroundPrefs((RadioButton)sender);
        }// end of method radAndroid_CheckedChanged

        private void radRabalac_CheckedChanged(object sender, EventArgs e)
        {
            UpdateWidgetBackgroundPrefs((RadioButton)sender);
        }// end of method radAndroid_CheckedChanged

        /// <summary>
        /// Update the widget's UI with the selected background image
        /// </summary>
        /// <param name="c">The control (<see cref="RadioButton"/> that the action was performed on.</param>
        private void UpdateWidgetBackgroundPrefs(Control c)
        {
            RadioButton selectedBackground = (RadioButton) c;
            string bgName = (string)selectedBackground.Tag;

            if (selectedBackground.Checked)
            {
                if (!WeatherLionMain.storedPreferences.StoredPreferences.WidgetBackground.Equals(bgName) &&
                        selectedBackground.Checked)
                {               
                    WeatherLionMain.runningWidget.preferenceUpdated.Add(
                        WeatherLionMain.WIDGET_BACKGROUND_PREFERENCE, bgName);
                }// end of if block
            }// end of if block
        }// end of method UpdateWidgetBackgroundPrefs

        /// <summary>
        /// Perform a check event on the<see cref="RadioButton"/> that corresponds with the <see cref="PictureBox"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectBackgroundRadioButton(object sender, EventArgs e)
        {
            PictureBox c = (PictureBox)sender;

            string rbName = c.Name.Substring(3).ToLower();
            RadioButton rbControl = (RadioButton)Controls.Find($"rad{UtilityMethod.ToProperCase(rbName)}", true)[0];

            rbControl.Checked = true;
        }// end of method SelectBackgroundRadioButton

        /// <summary>
        /// Perform a check event on the<see cref="RadioButton"/> that corresponds with the <see cref="PictureBox"/>
        /// </summary>
        /// <param name="c">The <see cref="Control"/> that requires the action.</param>
        private void SelectIconSetRadioButton(object sender, EventArgs e)
        {
            PictureBox c = (PictureBox)sender;

            string rbName = c.Name.Substring(3).ToLower();
            RadioButton rbControl = (RadioButton)Controls.Find($"rad{UtilityMethod.ToProperCase(rbName)}", true)[0];

            rbControl.Checked = true;
        }// end of method SelectIconSetRadioButton

        private void picDefault_Click(object sender, EventArgs e)
        {
            SelectBackgroundRadioButton(sender, e);
        }// end of method picDefault_Click

        private void picAndroid_Click(object sender, EventArgs e)
        {
            SelectBackgroundRadioButton(sender, e);
        }// end of method picAndroid_Click

        private void picRabalac_Click(object sender, EventArgs e)
        {
            SelectBackgroundRadioButton(sender, e);
        }// end of method picRabalac_Click
    }// end of partial class PreferencesForm
}// end of namespace WeatherLion
