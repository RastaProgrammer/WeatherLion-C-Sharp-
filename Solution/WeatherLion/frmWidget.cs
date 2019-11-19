using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices; //Needed
using System.Drawing;
using System.Drawing.Text;
using TheArtOfDev.HtmlRenderer.WinForms;
using System.Drawing.Imaging;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          WidgetForm
///   Description:    This class is responsible for Widget UI 
///   Author:         Paul O. Patterson     Date: October 03, 2017
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// Main UI which dispalys the weather information.
    /// </summary>
    public partial class WidgetForm : AlphaForm
    {
        #region Custom Imports

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr custFont,
          uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

        FontFamily weatherFontFamily;
        Font weatherFont;

        #endregion

        #region Rounded Corners

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        #endregion

        #region Form Move

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        #endregion

        #region Global Declarations

        private static WidgetForm m_instance = null;
        public static string wxLocation = null;        

        private static Dictionary<string, int> monthNumber = new Dictionary<string, int>();
       
        private static bool connectedToInternet = UtilityMethod.HasInternetConnection();

        public static bool previousDataExists;
        public static string previousProvider;
        public static string previousLocation;
        public static DateTime lastUpdate;

        // Placeholder Constant
        public readonly string NO_READING_FORECAST = "0° 0°";
        public readonly string NO_READING = "0°F";
        public readonly string NO_READING_H = "0°";
        public readonly string FEELS_LIKE = "Feels Like";
        public readonly string NO_CONDITION = "Current Condition";
        public readonly string NO_LOCATION = "Current Location";
        public readonly string NO_WIND = "N 0 mph";
        public readonly string NO_HUMIDITY = "0%";
        public readonly string MONDAY = "Mon";
        public readonly string TUESDAY = "Tue";
        public readonly string WEDNESDAY = "Wed";
        public readonly string THURSDAY = "Thu";
        public readonly string FRIDAY = "Fri";
        public readonly string SUNRISE = "6:00 AM";
        public readonly string SUNSET = "6:00 PM";
        public readonly string PROVIDER = "Weather Provider";

        // Default Icon Images
        private static readonly string IMAGE_PATH = $@"{AppDomain.CurrentDomain.BaseDirectory}res\assets\img\";
        private static readonly string WIDGET_BACKGROUND_IMAGE = $@"{IMAGE_PATH}\backgrounds\default_bg.png";
        private static readonly string DEFAULT_WEATHER_IMAGE = $@"{IMAGE_PATH}\weather_images\miui\weather_15.png";
        private static readonly string OFFLINE_IMAGE = $@"{IMAGE_PATH}\icons\offline.png";
        private static readonly string REFRESH_IMAGE = $@"{IMAGE_PATH}\icons\refresh.png";
        private static readonly string WIND_IMAGE = $@"{IMAGE_PATH}\icons\wind.png";
        private static readonly string HUMIDITY_IMAGE = $@"{IMAGE_PATH}\icons\humidity.png";
        private static readonly string GEOLOCATION_IMAGE = $@"{IMAGE_PATH}\icons\geolocation.png";
        private static readonly string SUNRISE_IMAGE = $@"{IMAGE_PATH}\icons\sunrise.png";
        private static readonly string SUNSET_IMAGE = $@"{IMAGE_PATH}\icons\sunset.png";

        public bool sunriseIconsInUse;
        public bool sunriseUpdatedPerformed;
        public bool sunsetIconsInUse;
        public bool sunsetUpdatedPerformed;

        public bool iconSetSwtich;

        public volatile bool reAttempted;
        public volatile bool running;
        public volatile bool usingPreviousData = false; // flag for old weather data
        public volatile bool dataLoadedSuccessfully;
        public volatile bool applyPreferenceUpdates;

        public readonly string WEATHER_IMAGE_PATH_PREFIX = $@"{WeatherLionMain.RES_PATH}assets/img/weather_images/";

        public StringBuilder previousWeatherProvider = new StringBuilder();
        public Dictionary<string, string> preferenceUpdated = new Dictionary<string, string>();
        public static WidgetUpdateService ws;

        private const string TAG = "WidgetForm";
        
        #endregion

        #region Class Constructor

        public WidgetForm()
        {            
            WeatherLionMain.Launch();
            InitializeComponent();
            
            SetStyle(ControlStyles.Selectable, false);
            FormBorderStyle = FormBorderStyle.None;                       

            // round the corners until another way can be found
            //Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 28, 28));

            // load the custom Font
            LoadFont();

            // allocate Font to a control
            foreach (Control c in pnlWidget.Controls)
            {
                if (c is Label || c is Button || c is HtmlLabel)
                {
                    AllocateFont(weatherFontFamily, c, c.Font.Size);
                    c.ForeColor = Color.White;
                    c.MouseDown += new MouseEventHandler(ShowMenu);
                }// end of if block
                else if (c is FlowLayoutPanel || c is Panel)
                {
                    foreach (Control sc in c.Controls)
                    {
                        AllocateFont(weatherFontFamily, sc, sc.Font.Size);
                        sc.ForeColor = Color.White;
                        FixBorders(sc);
                        sc.MouseDown += new MouseEventHandler(ShowMenu);
                    }// end of inner foreach block
                }// end of if block
                else if (c is HtmlLabel)
                {
                    AllocateFont(weatherFontFamily, c, c.Font.Size);
                    c.MouseDown += new MouseEventHandler(ShowMenu);
                }// end of if block

                FixBorders(c);
                c.MouseDown += new MouseEventHandler(ShowMenu);
            }// end of foreach block           

            // indicate that the program is running
            running = true;

            m_instance = this;
            WeatherLionMain.runningWidget = GetInstance();
        }// end of default contructor       

        #endregion

        #region Form Controls

        #region Button Controls

        private void btnWindReading_TextChanged(object sender, EventArgs e)
        {
            btnWindReading.Size = GetTextSize(btnWindReading);
        }// end of method btnWindReading_TextChanged

        #endregion

        #region Form Events       

        private void frmWidget_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save configuration
            WeatherLionMain.storedPreferences.ConfigData.StartPosition = Location;
            WeatherLionMain.storedPreferences.StoredPreferences.Interval = tmrUpdate.Interval;
            Preference.SaveProgramConfiguration("config", "StartPosition", Location);
        }// end of method frmWidget_FormClosing            

        private void frmWidget_Load(object sender, EventArgs e)
        {
            bool exists = File.Exists(Preference.CONFIG_FILE);

            // load the appropriate background
            Image bgImage = Image.FromFile(
                $@"{IMAGE_PATH}\backgrounds\{WeatherLionMain.storedPreferences.StoredPreferences.WidgetBackground}_bg.png");

            BlendedBackground = (Bitmap) bgImage; // update the form's blended background image
            BackgroundUpdate();
            UpdateLayeredBackground();

            if (exists)
            {
                if (WeatherLionMain.storedPreferences.ConfigData.StartPosition.X != 0 || WeatherLionMain.storedPreferences.ConfigData.StartPosition.Y != 0)
                {
                    Location = WeatherLionMain.storedPreferences.ConfigData.StartPosition;
                }// end of if block
                else
                {
                    StartPosition = FormStartPosition.CenterScreen;
                }// end of else block

            }// end of if block           

            // check the update interval stored in the user preferences
            if (WeatherLionMain.storedPreferences.StoredPreferences.Interval != 0)
            {
                tmrUpdate.Interval = WeatherLionMain.storedPreferences.StoredPreferences.Interval;
            }// end of if block
            else
            {
                tmrUpdate.Interval = WeatherLionMain.DEFAULT_WIDGET_UPDATE_INTERVAL;
            }// end of else block

            // setup the refresh timer
            WidgetUpdateService.currentRefreshInterval = tmrUpdate.Interval;

            ws = new WidgetUpdateService(false, this);

            string wxDataFile = $"{WeatherLionMain.DATA_DIRECTORY_PATH}{WeatherLionMain.WEATHER_DATA_XML}";

            if (File.Exists(wxDataFile))
            {
                // Load old data into the widget first before contacting the provider
                ws.LoadPreviousWeatherData();
            }// end of if block

            // start the weather 'service'
            ws.Run();

            // start the weather refresh timer
            tmrUpdate.Start();

            // update the widget clock
            tmrCurrentTime.Start();

            // start the time that monitors user preference changes
            tmrPreferenceUpdate.Start();

            // position form on screen
            Left = Location.X;
            Top = Location.Y;            
        }// end of method frmWidget_Load   

        private void frmWidget_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    //places the menu at the pointer position
                    cmOptions.Show((Control)sender, new Point(e.X, e.Y));

                    break;
                case MouseButtons.Left:
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);

                    break;
                default:
                    break;
            }// end of switch block
        }// end of method frmWidget_MouseDown

        #endregion

        #region Label Controls

        private void lblBorder_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblBorder.DisplayRectangle,
                Color.White, ButtonBorderStyle.Solid);
        }

        #endregion

        #region Panel Controls

        private void pnlWidget_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    //places the menu at the pointer position
                    cmOptions.Show((Control)sender, new Point(e.X, e.Y));

                    break;
                case MouseButtons.Left:
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);

                    break;
                default:
                    break;
            }// end of switch block
        }// end of method pnlWidget_MouseDown

        #endregion

        #region PictureBox Controls

        private void picRefresh_Click(object sender, EventArgs e)
        {
            long updatePeriod = UtilityMethod.SubtractTime(new DateTime(), UtilityMethod.lastUpdated);

            if (updatePeriod < 10)
            {

            }// end of if block
            else
            {
                btnLocation.Text = "Refreshing...";

                ws = new WidgetUpdateService(false, this);
                ws.Run();
            }// end of else block
        }// end of method picRefresh_Click

        #endregion

        #region Right Click Menu Controls

        private void tlsAddKey_Click(object sender, EventArgs e)
        {
            WeatherLionMain.keys.ShowDialog(this);
        }// end of method tlsAddKey_Click

        private void tlsPreferences_Click(object sender, EventArgs e)
        {
            WeatherLionMain.preferences.ShowDialog(this);
        }// end of method tlsPreferences_Click

        private void tlsRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }// end of method tlsRefresh_Click

        private void tlsExit_Click(object sender, EventArgs e)
        {
            Application.Exit();     //Exit the application.
        }// end of method tlsExit_Click      

        #endregion

        #region Timing Events

        private void tmrCurrentTime_Tick(object sender, EventArgs e)
        {
            UpdateClock(DateTime.Now);
            CheckAstronomy();
        }// end of method tmrCurrentTime_Tick

        private void tmrIconUpdater_Tick(object sender, EventArgs e)
        {
            new IconUpdateService(this);
        }// end of method tmrIconUpdater_Tick        

        private void tmrPreferenceUpdate_Tick(object sender, EventArgs e)
        {
            if (applyPreferenceUpdates)
            {
                if (preferenceUpdated.Count > 0)
                {
                    foreach (string preference in preferenceUpdated.Keys)
                    {
                        switch (preference)
                        {
                            case WeatherLionMain.CURRENT_LOCATION_PREFERENCE:
                                // update the local preference file
                                Preference.SaveProgramConfiguration("prefs",
                                    preference, preferenceUpdated[preference]);

                                // update running data
                                WeatherLionMain.storedPreferences.StoredPreferences.Location =
                                    preferenceUpdated[preference];

                                // reset the re-attempted flag before running the service
                                reAttempted = false;

                                // run the weather service
                                ws = new WidgetUpdateService(false, this);
                                ws.Run();

                                break;
                            case WeatherLionMain.WEATHER_SOURCE_PREFERENCE:
                                // update the local preference file
                                Preference.SaveProgramConfiguration("prefs",
                                    preference, preferenceUpdated[preference]);

                                // update running data
                                WeatherLionMain.storedPreferences.StoredPreferences.Provider =
                                    preferenceUpdated[preference];

                                // reset the re-attempted flag before running the service
                                reAttempted = false;

                                // run the weather service
                                ws = new WidgetUpdateService(false, this);
                                ws.Run();

                                break;
                            case WeatherLionMain.UPDATE_INTERVAL:
                                // update the local preference file
                                Preference.SaveProgramConfiguration("prefs",
                                    preference, preferenceUpdated[preference]);

                                // update running data
                                WeatherLionMain.storedPreferences.StoredPreferences.Interval =
                                    int.Parse(preferenceUpdated[preference]);

                                break;
                            case WeatherLionMain.USE_SYSTEM_LOCATION_PREFERENCE:
                                // update the local preference file
                                Preference.SaveProgramConfiguration("prefs",
                                    preference, preferenceUpdated[preference].ToLower());

                                // update running data
                                WeatherLionMain.storedPreferences.StoredPreferences.UseSystemLocation =
                                    bool.Parse(preferenceUpdated[preference].ToLower());

                                // reset the re-attempted flag before running the service
                                reAttempted = false;

                                // run the weather service
                                ws = new WidgetUpdateService(false, this);
                                ws.Run();

                                break;
                            case WeatherLionMain.USE_METRIC_PREFERENCE:
                                // update the local preference file
                                Preference.SaveProgramConfiguration("prefs",
                                    preference, preferenceUpdated[preference].ToLower());

                                // update running data
                                WeatherLionMain.storedPreferences.StoredPreferences.UseMetric =
                                    bool.Parse(preferenceUpdated[preference].ToLower());

                                // update the units displayed on the widget
                                ws = new WidgetUpdateService(true, this);
                                ws.Run();

                                break;
                            case WeatherLionMain.ICON_SET_PREFERENCE:
                                // update the local preference file
                                Preference.SaveProgramConfiguration("prefs",
                                   preference, preferenceUpdated[preference]);

                                // update running data
                                WeatherLionMain.storedPreferences.StoredPreferences.IconSet =
                                    preferenceUpdated[preference];

                                WeatherLionMain.iconSet =
                                    WeatherLionMain.storedPreferences.StoredPreferences.IconSet;
                                UpdateIconSet();
                                break;
                            case WeatherLionMain.WIDGET_BACKGROUND_PREFERENCE:
                                // update the local preference file
                                Preference.SaveProgramConfiguration("prefs",
                                    preference, preferenceUpdated[preference]);

                                // update running data
                                WeatherLionMain.storedPreferences.StoredPreferences.WidgetBackground =
                                    preferenceUpdated[preference];

                                switch (WeatherLionMain.storedPreferences.StoredPreferences.WidgetBackground)
                                {
                                    case "default":
                                    default:                                    
                                        BlendedBackground =
                                            (Bitmap)Image.FromFile(WeatherLionMain.DEFAULT_BACKGROUND_IMAGE);
                                        Refresh();
                                        break;
                                    case "android":
                                        BlendedBackground =
                                            (Bitmap)Image.FromFile(WeatherLionMain.ANDROID_BACKGROUND_IMAGE);
                                        Refresh();
                                        break;
                                    case "rabalac":
                                        BlendedBackground =
                                           (Bitmap)Image.FromFile(WeatherLionMain.RABALAC_BACKGROUND_IMAGE);
                                        Refresh();
                                        break;
                                }// end of switch block

                                BackgroundUpdate();

                                break;
                            default:
                                break;
                        }// end of switch block
                    }// end of for each loop

                    // remove all items from the object
                    preferenceUpdated.Clear();

                    // refresh the UI immediately
                    Refresh();

                    // reset flag to perfom changes
                    applyPreferenceUpdates = false;
                }// end of if block         
            }// end of if block              
        }// end of method tmrPreferenceUpdate_Tick

        /// <summary>
        /// Update the widget at the time interval specified by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            ws = new WidgetUpdateService(false, this);
            ws.Run();
        }// end of method tmrUpdate_Tick

        #endregion

        #endregion

        #region Custom Font Handling

        private void AllocateFont(FontFamily ff, Control c, float size)
        {
            FontStyle? fontStyle = null;

            if (c.Name.Equals("lblWeatherCondition"))
            {
                fontStyle = FontStyle.Bold;
            }// end of if block
            else
            {
                fontStyle = FontStyle.Regular;
            }// end of else block

            c.Font = new Font(ff, size, (FontStyle)fontStyle);
        } // end of method AllocateFont

        private void LoadFont()
        {
            // sidebar font
            string s = $@"{AppDomain.CurrentDomain.BaseDirectory}res\assets\fonts\Samsungsans-Regular.ttf";
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(s);
            
            weatherFontFamily = pfc.Families[0];            
            weatherFont = new Font(weatherFontFamily, 10f, FontStyle.Regular);
        }// end of method LoadFont

        #endregion

        #region Action Methods                      

        /// <summary>
        /// Determines the time of day to ensure correct icons are displayed.
        /// </summary>
        public void CheckAstronomy()
        {
            btnSunrise.Text = WidgetUpdateService.sunriseTime.ToString();
            btnSunset.Text = WidgetUpdateService.sunsetTime.ToString();
            Image wxImage = null;

            // update icons based on the time of day in relation to sunrise and sunset times
            if (WidgetUpdateService.sunriseTime != null && WidgetUpdateService.sunsetTime != null)
            {
                DateTime rightNow = DateTime.Now;
                DateTime rn = DateTime.Now; // date time right now (rn)
                DateTime? nf = null; // date time night fall (nf)
                DateTime? su = null; // date time sun up (su)

                try
                {
                    string sunsetTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                        $"{UtilityMethod.Get24HourTime(WidgetUpdateService.sunsetTime.ToString())}";
                    string sunriseTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                        $"{UtilityMethod.Get24HourTime(WidgetUpdateService.sunriseTime.ToString())}";
                    nf = Convert.ToDateTime(sunsetTwenty4HourTime);
                    su = Convert.ToDateTime(sunriseTwenty4HourTime);
                } // end of try block
                catch (FormatException e)
                {
                    UtilityMethod.LogMessage(UtilityMethod.LogLevel.SEVERE, e.Message,
                        $"{TAG}::CheckAstronomy [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block

                StringBuilder currentConditionIcon = new StringBuilder();
                string currentCondition = UtilityMethod.ValidateCondition(
                    WidgetUpdateService.currentCondition.ToString());

                if (rn == nf || rn > nf || rn < su)
                {
                    if (currentCondition.ToLower().Contains("(night)"))
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append(
                            (string)UtilityMethod.weatherImages[currentCondition.ToLower()]
                        );
                    }// end of if block
                    else
                    {
                        // Yahoo has a habit of having sunny nights
                        if (WidgetUpdateService.currentCondition.ToString().ToLower().Equals("sunny"))
                        {
                            WidgetUpdateService.currentCondition.Clear();
                            WidgetUpdateService.currentCondition.Append("Clear");
                            lblWeatherCondition.Text = UtilityMethod.ToProperCase(
                                WidgetUpdateService.currentCondition.ToString());
                        }// end of if block

                        if (UtilityMethod.weatherImages.ContainsKey($"{currentCondition.ToLower()} (night)"))
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[$"{currentCondition.ToLower()} (night)"]);
                        }// end of if block
                        else
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToLower()]);
                        }// end of else block
                    }// end of else block

                    if (!sunsetUpdatedPerformed && !sunsetIconsInUse)
                    {
                        // Set image tooltip to current condition string
                        UtilityMethod.AddControlToolTip(picCurrentConditions, 
                            WidgetUpdateService.currentCondition.ToString().ToProperCase());                        

                        sunsetIconsInUse = true;
                        sunriseIconsInUse = false;
                        sunsetUpdatedPerformed = true;
                        sunriseUpdatedPerformed = false;
                    }// end of if block 
                    else if (iconSetSwtich)
                    {
                        // reset the flag after switch is made
                        iconSetSwtich = false;
                    }// end of else if block

                }// end of if block
                else if (iconSetSwtich)
                {
                    currentConditionIcon.Clear();
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToLower()]);

                    // reset the flag after switch is made
                    iconSetSwtich = false;
                }// end of else if block
                else
                {
                    currentConditionIcon.Clear();
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToLower()]);

                    if (!sunriseUpdatedPerformed && !sunriseIconsInUse)
                    {
                        // Set image tooltip to current condition string
                        UtilityMethod.AddControlToolTip(picCurrentConditions,
                            WidgetUpdateService.currentCondition.ToString().ToProperCase());
                        sunriseUpdatedPerformed = true;
                        sunsetUpdatedPerformed = false;
                    }// end of if block
                    else if (iconSetSwtich)
                    {
                        // Set image tooltip to current condition string
                        UtilityMethod.AddControlToolTip(picCurrentConditions,
                            WidgetUpdateService.currentCondition.ToString().ToProperCase());

                        // reset the flag after switch is made
                        iconSetSwtich = false;
                    }// end of else if block
                    else
                    {
                        sunriseIconsInUse = true;
                        sunsetIconsInUse = false;
                    }// end of else block

                }// end of else block 

                if (currentConditionIcon.Length == 0)
                {
                    currentConditionIcon.Append("na.png");                   
                }// end of if block

                string imagePath = $"{WEATHER_IMAGE_PATH_PREFIX}" +
                           $"{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}" +
                           $"/weather_{currentConditionIcon}";

                wxImage = Image.FromFile(imagePath);

                if (imagePath.Contains("google now") ||
                    imagePath.Contains("miui") ||
                    imagePath.Contains("weezle"))
                {
                    picCurrentConditions.Image = UtilityMethod.ResizeImage(wxImage, 120, 120);
                }// end of if block
                else
                {
                    picCurrentConditions.Image = UtilityMethod.ResizeImage(wxImage, 140, 140);
                }// end of else block       
            }// end of if block
        }// end of method CheckAstronomy

        /// <summary>
        /// Remove unwanted border line around button controls.
        /// </summary>
        /// <param name="c">The <see cref="Control"/> for which the border must be made invisible.</param>
        private void FixBorders(Control c)
        {
            if (c is Button)
            {
                ((Button)c).TabStop = false;
                ((Button)c).FlatStyle = FlatStyle.Flat;
                ((Button)c).FlatAppearance.BorderSize = 0;
                ((Button)c).FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); //transparent
            }// end of if block
        }// end of method FixBorders

        /// <summary>
        /// Returns a singleton instance of the current widget
        /// </summary>
        /// <returns></returns>
        public static WidgetForm GetInstance()
        {
            if (m_instance == null)
            {
                return new WidgetForm();
            }// end of if block
            else
            {
                return m_instance;
            }// end of else block
        }// end of method GetInstance

        /// <summary>
        /// Resize a control based on the text content.
        /// </summary>
        /// <param name="c">The <see cref="Control"/> to be resized.</param>
        /// <returns></returns>
        private Size GetTextSize(Control c)
        {
            Size padSize = TextRenderer.MeasureText(".", c.Font);
            Size textSize = TextRenderer.MeasureText(c.Text + ".", c.Font);
            return new Size(textSize.Width - padSize.Width, textSize.Height);
        }// end of method GetTextSize

        /// <summary>
        /// Displays the right-click popup menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowMenu(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    //places the menu at the pointer position
                    cmOptions.Show((Control)sender, new Point(e.X, e.Y));

                    break;
                case MouseButtons.Left:
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);

                    break;
                default:
                    break;
            }// end of switch block
        }// end of method ShowMenu

        /// <summary>
        /// Updates the current time displayed on the widget
        /// </summary>
        /// <param name="time"></param>
        public void UpdateClock(DateTime time)
        {
            string currentTime = time.ToString("h:mm");
            string timeOfDay = time.ToString("tt");
            lblClock.Text = $"<html>" +
                                $"<div style='text-align:center;font-style:inherit;><span style='color: #F49630;font-size: 44px'>{currentTime}</span>" +
                                $"<b><sub style='color: #F49630;font-size: 1.25em'>{timeOfDay}</sub></b></div>" +
                             "</html>";
        }// end of method UpdateClock

        /// <summary>
        /// Update the widget to use the selected icon set.
        /// </summary>
        private void UpdateIconSet()
        {
            string ico = WeatherLionMain.storedPreferences.StoredPreferences.IconSet;

            WeatherLionMain.iconSet = ico != null && ico.Equals("default") ?
                WeatherLionMain.DEFAULT_ICON_SET : ico;

            // indicate that the icons are being changed
            iconSetSwtich = true;

            // update based on time of day
            CheckAstronomy();

            StringBuilder wxImage = null;

            // Update five day weather icons
            for (int i = 1; i < 6; i++)
            {
               wxImage = new StringBuilder(
                    $"{WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.iconSet}/weather_" +
                        UtilityMethod.weatherImages[
                            UtilityMethod.controlTip.GetToolTip((PictureBox)Controls.Find($"picDay{i}Image",
                            true)[0]).ToLower()]);

                Image fxImage = UtilityMethod.ResizeImage(Image.FromFile(wxImage.ToString()), 40, 40);

                ((PictureBox)Controls.Find($"picDay{i}Image", true)[0]).Image = fxImage;
            }// end of for loop

            // Refresh the widget UI
            Refresh();
        }// end of method UpdateIconSet

        #endregion

        /// <summary>
        /// Sets the opacity of an image
        /// </summary>
        /// <param name="image">The image to be made transparent.</param>
        /// <param name="opacity">The percentage of opacity.</param>
        /// <returns></returns>
        public Image SetImageOpacity(Image image, float opacity)
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
            }// end of using block

            return bmp;
        }// end of method SetImageOpacity

        /// <summary>
        /// Determines the transparency key color to be used with the specific background.
        /// </summary>
        private void BackgroundUpdate()
        {
            switch (WeatherLionMain.storedPreferences.StoredPreferences.WidgetBackground)
            {
                case "default":
                    BackColor = Color.FromArgb(68, 71, 77);
                    TransparencyKey = Color.FromArgb(68, 71, 77);

                    break;
                case "android":
                    BackColor = Color.FromArgb(56, 132, 150);
                    TransparencyKey = Color.FromArgb(56, 132, 150);

                    break;
                case "rabalac":
                    BackColor = Color.FromArgb(101, 131, 72);
                    TransparencyKey = Color.FromArgb(101, 131, 72);

                    break;
                default:
                    break;
            }// end of switch block'
        }// end of method BackgroundUpdate
    }// end of partial class formWidget    
}// end of namespace WeatherLion
