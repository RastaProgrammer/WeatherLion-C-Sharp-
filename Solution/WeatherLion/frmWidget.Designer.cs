namespace WeatherLion
{
    partial class WidgetForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WidgetForm));
            this.pnlWidget = new System.Windows.Forms.Panel();
            this.picCurrentConditions = new System.Windows.Forms.PictureBox();
            this.btnLocation = new System.Windows.Forms.Button();
            this.lblClock = new TheArtOfDev.HtmlRenderer.WinForms.HtmlLabel();
            this.btnSunset = new System.Windows.Forms.Button();
            this.btnWeatherProvider = new System.Windows.Forms.Button();
            this.lblWeatherCondition = new System.Windows.Forms.Label();
            this.flpAtmosphere = new System.Windows.Forms.FlowLayoutPanel();
            this.btnWindReading = new System.Windows.Forms.Button();
            this.btnHumidity = new System.Windows.Forms.Button();
            this.pnlDay3 = new System.Windows.Forms.Panel();
            this.lblDay3Day = new System.Windows.Forms.Label();
            this.lblDay3Temps = new System.Windows.Forms.Label();
            this.picDay3Image = new System.Windows.Forms.PictureBox();
            this.pnlDay2 = new System.Windows.Forms.Panel();
            this.lblDay2Day = new System.Windows.Forms.Label();
            this.lblDay2Temps = new System.Windows.Forms.Label();
            this.picDay2Image = new System.Windows.Forms.PictureBox();
            this.pnlDay4 = new System.Windows.Forms.Panel();
            this.lblDay4Day = new System.Windows.Forms.Label();
            this.lblDay4Temps = new System.Windows.Forms.Label();
            this.picDay4Image = new System.Windows.Forms.PictureBox();
            this.lblBorder = new System.Windows.Forms.Label();
            this.pnlDay5 = new System.Windows.Forms.Panel();
            this.lblDay5Day = new System.Windows.Forms.Label();
            this.lblDay5Temps = new System.Windows.Forms.Label();
            this.picDay5Image = new System.Windows.Forms.PictureBox();
            this.pnlDay1 = new System.Windows.Forms.Panel();
            this.lblDay1Day = new System.Windows.Forms.Label();
            this.lblDay1Temps = new System.Windows.Forms.Label();
            this.picDay1Image = new System.Windows.Forms.PictureBox();
            this.lblFeelsLike = new System.Windows.Forms.Label();
            this.picOffline = new System.Windows.Forms.PictureBox();
            this.lblDayLow = new System.Windows.Forms.Label();
            this.picRefresh = new System.Windows.Forms.PictureBox();
            this.lblDayHigh = new System.Windows.Forms.Label();
            this.lblCurrentTemperature = new System.Windows.Forms.Label();
            this.btnSunrise = new System.Windows.Forms.Button();
            this.tlsPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.tlsExit = new System.Windows.Forms.ToolStripMenuItem();
            this.cmOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tlsAddKey = new System.Windows.Forms.ToolStripMenuItem();
            this.tlsRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.tlsSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.tmrCurrentTime = new System.Windows.Forms.Timer(this.components);
            this.tmrIconUpdater = new System.Windows.Forms.Timer(this.components);
            this.tmrPreferenceUpdate = new System.Windows.Forms.Timer(this.components);
            this.pnlWidget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCurrentConditions)).BeginInit();
            this.flpAtmosphere.SuspendLayout();
            this.pnlDay3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDay3Image)).BeginInit();
            this.pnlDay2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDay2Image)).BeginInit();
            this.pnlDay4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDay4Image)).BeginInit();
            this.pnlDay5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDay5Image)).BeginInit();
            this.pnlDay1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDay1Image)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOffline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).BeginInit();
            this.cmOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlWidget
            // 
            this.pnlWidget.BackColor = System.Drawing.Color.Transparent;
            this.pnlWidget.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlWidget.Controls.Add(this.picCurrentConditions);
            this.pnlWidget.Controls.Add(this.btnLocation);
            this.pnlWidget.Controls.Add(this.lblClock);
            this.pnlWidget.Controls.Add(this.btnSunset);
            this.pnlWidget.Controls.Add(this.btnWeatherProvider);
            this.pnlWidget.Controls.Add(this.lblWeatherCondition);
            this.pnlWidget.Controls.Add(this.flpAtmosphere);
            this.pnlWidget.Controls.Add(this.pnlDay3);
            this.pnlWidget.Controls.Add(this.pnlDay2);
            this.pnlWidget.Controls.Add(this.pnlDay4);
            this.pnlWidget.Controls.Add(this.lblBorder);
            this.pnlWidget.Controls.Add(this.pnlDay5);
            this.pnlWidget.Controls.Add(this.pnlDay1);
            this.pnlWidget.Controls.Add(this.lblFeelsLike);
            this.pnlWidget.Controls.Add(this.picOffline);
            this.pnlWidget.Controls.Add(this.lblDayLow);
            this.pnlWidget.Controls.Add(this.picRefresh);
            this.pnlWidget.Controls.Add(this.lblDayHigh);
            this.pnlWidget.Controls.Add(this.lblCurrentTemperature);
            this.pnlWidget.Controls.Add(this.btnSunrise);
            this.pnlWidget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWidget.Location = new System.Drawing.Point(0, 0);
            this.pnlWidget.Name = "pnlWidget";
            this.pnlWidget.Size = new System.Drawing.Size(340, 288);
            this.pnlWidget.TabIndex = 0;
            this.pnlWidget.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlWidget_MouseDown);
            // 
            // picCurrentConditions
            // 
            this.picCurrentConditions.BackColor = System.Drawing.Color.Transparent;
            this.picCurrentConditions.Location = new System.Drawing.Point(6, 6);
            this.picCurrentConditions.Name = "picCurrentConditions";
            this.picCurrentConditions.Size = new System.Drawing.Size(132, 134);
            this.picCurrentConditions.TabIndex = 102;
            this.picCurrentConditions.TabStop = false;
            // 
            // btnLocation
            // 
            this.btnLocation.BackColor = System.Drawing.Color.Transparent;
            this.btnLocation.FlatAppearance.BorderSize = 0;
            this.btnLocation.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLocation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLocation.Font = new System.Drawing.Font("Samsung Sans", 11.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLocation.ForeColor = System.Drawing.Color.White;
            this.btnLocation.Image = ((System.Drawing.Image)(resources.GetObject("btnLocation.Image")));
            this.btnLocation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLocation.Location = new System.Drawing.Point(135, 112);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(217, 26);
            this.btnLocation.TabIndex = 118;
            this.btnLocation.Text = "Pine Hills, Wed 10:05 PM";
            this.btnLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLocation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLocation.UseVisualStyleBackColor = false;
            // 
            // lblClock
            // 
            this.lblClock.AutoSize = false;
            this.lblClock.BackColor = System.Drawing.Color.Transparent;
            this.lblClock.BaseStylesheet = null;
            this.lblClock.Font = new System.Drawing.Font("Samsung Sans", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClock.Location = new System.Drawing.Point(94, 124);
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size(150, 48);
            this.lblClock.TabIndex = 114;
            this.lblClock.TabStop = false;
            this.lblClock.Tag = "Clock";
            this.lblClock.Text = null;
            // 
            // btnSunset
            // 
            this.btnSunset.BackColor = System.Drawing.Color.Transparent;
            this.btnSunset.FlatAppearance.BorderSize = 0;
            this.btnSunset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSunset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSunset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSunset.Font = new System.Drawing.Font("Samsung Sans", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSunset.ForeColor = System.Drawing.Color.White;
            this.btnSunset.Image = ((System.Drawing.Image)(resources.GetObject("btnSunset.Image")));
            this.btnSunset.Location = new System.Drawing.Point(229, 134);
            this.btnSunset.Name = "btnSunset";
            this.btnSunset.Size = new System.Drawing.Size(94, 45);
            this.btnSunset.TabIndex = 124;
            this.btnSunset.Text = " 8:17 PM";
            this.btnSunset.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSunset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSunset.UseVisualStyleBackColor = false;
            // 
            // btnWeatherProvider
            // 
            this.btnWeatherProvider.BackColor = System.Drawing.Color.Transparent;
            this.btnWeatherProvider.FlatAppearance.BorderSize = 0;
            this.btnWeatherProvider.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnWeatherProvider.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnWeatherProvider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWeatherProvider.Font = new System.Drawing.Font("Samsung Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWeatherProvider.ForeColor = System.Drawing.Color.White;
            this.btnWeatherProvider.Image = ((System.Drawing.Image)(resources.GetObject("btnWeatherProvider.Image")));
            this.btnWeatherProvider.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnWeatherProvider.Location = new System.Drawing.Point(10, 261);
            this.btnWeatherProvider.Name = "btnWeatherProvider";
            this.btnWeatherProvider.Size = new System.Drawing.Size(323, 28);
            this.btnWeatherProvider.TabIndex = 121;
            this.btnWeatherProvider.TabStop = false;
            this.btnWeatherProvider.Text = "Yahoo! Weather";
            this.btnWeatherProvider.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnWeatherProvider.UseVisualStyleBackColor = false;
            // 
            // lblWeatherCondition
            // 
            this.lblWeatherCondition.BackColor = System.Drawing.Color.Transparent;
            this.lblWeatherCondition.Font = new System.Drawing.Font("Samsung Sans", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeatherCondition.ForeColor = System.Drawing.Color.White;
            this.lblWeatherCondition.Location = new System.Drawing.Point(138, 69);
            this.lblWeatherCondition.Name = "lblWeatherCondition";
            this.lblWeatherCondition.Size = new System.Drawing.Size(199, 22);
            this.lblWeatherCondition.TabIndex = 110;
            this.lblWeatherCondition.Text = "Mostly Cloudy";
            this.lblWeatherCondition.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // flpAtmosphere
            // 
            this.flpAtmosphere.BackColor = System.Drawing.Color.Transparent;
            this.flpAtmosphere.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.flpAtmosphere.Controls.Add(this.btnWindReading);
            this.flpAtmosphere.Controls.Add(this.btnHumidity);
            this.flpAtmosphere.Location = new System.Drawing.Point(138, 88);
            this.flpAtmosphere.Margin = new System.Windows.Forms.Padding(0);
            this.flpAtmosphere.Name = "flpAtmosphere";
            this.flpAtmosphere.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.flpAtmosphere.Size = new System.Drawing.Size(206, 26);
            this.flpAtmosphere.TabIndex = 123;
            this.flpAtmosphere.WrapContents = false;
            // 
            // btnWindReading
            // 
            this.btnWindReading.AutoSize = true;
            this.btnWindReading.BackColor = System.Drawing.Color.Transparent;
            this.btnWindReading.FlatAppearance.BorderSize = 0;
            this.btnWindReading.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnWindReading.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnWindReading.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWindReading.Font = new System.Drawing.Font("Samsung Sans", 11.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWindReading.ForeColor = System.Drawing.Color.White;
            this.btnWindReading.Image = ((System.Drawing.Image)(resources.GetObject("btnWindReading.Image")));
            this.btnWindReading.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWindReading.Location = new System.Drawing.Point(0, 0);
            this.btnWindReading.Margin = new System.Windows.Forms.Padding(0);
            this.btnWindReading.Name = "btnWindReading";
            this.btnWindReading.Size = new System.Drawing.Size(116, 29);
            this.btnWindReading.TabIndex = 97;
            this.btnWindReading.Text = "ESE 2 mph";
            this.btnWindReading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWindReading.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnWindReading.UseVisualStyleBackColor = false;
            this.btnWindReading.TextChanged += new System.EventHandler(this.btnWindReading_TextChanged);
            // 
            // btnHumidity
            // 
            this.btnHumidity.BackColor = System.Drawing.Color.Transparent;
            this.btnHumidity.FlatAppearance.BorderSize = 0;
            this.btnHumidity.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHumidity.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHumidity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHumidity.Font = new System.Drawing.Font("Samsung Sans", 11.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHumidity.ForeColor = System.Drawing.Color.White;
            this.btnHumidity.Image = ((System.Drawing.Image)(resources.GetObject("btnHumidity.Image")));
            this.btnHumidity.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHumidity.Location = new System.Drawing.Point(116, 0);
            this.btnHumidity.Margin = new System.Windows.Forms.Padding(0);
            this.btnHumidity.Name = "btnHumidity";
            this.btnHumidity.Size = new System.Drawing.Size(76, 26);
            this.btnHumidity.TabIndex = 98;
            this.btnHumidity.Text = "32%";
            this.btnHumidity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHumidity.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHumidity.UseVisualStyleBackColor = false;
            // 
            // pnlDay3
            // 
            this.pnlDay3.BackColor = System.Drawing.Color.Transparent;
            this.pnlDay3.Controls.Add(this.lblDay3Day);
            this.pnlDay3.Controls.Add(this.lblDay3Temps);
            this.pnlDay3.Controls.Add(this.picDay3Image);
            this.pnlDay3.Location = new System.Drawing.Point(141, 176);
            this.pnlDay3.Name = "pnlDay3";
            this.pnlDay3.Size = new System.Drawing.Size(64, 94);
            this.pnlDay3.TabIndex = 114;
            // 
            // lblDay3Day
            // 
            this.lblDay3Day.BackColor = System.Drawing.Color.Transparent;
            this.lblDay3Day.Font = new System.Drawing.Font("Segoe", 11.3F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDay3Day.ForeColor = System.Drawing.Color.White;
            this.lblDay3Day.Location = new System.Drawing.Point(0, 3);
            this.lblDay3Day.Name = "lblDay3Day";
            this.lblDay3Day.Size = new System.Drawing.Size(64, 22);
            this.lblDay3Day.TabIndex = 77;
            this.lblDay3Day.Text = "Wed 03";
            this.lblDay3Day.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDay3Temps
            // 
            this.lblDay3Temps.BackColor = System.Drawing.Color.Transparent;
            this.lblDay3Temps.Font = new System.Drawing.Font("Samsung Sans", 11.25F);
            this.lblDay3Temps.ForeColor = System.Drawing.Color.White;
            this.lblDay3Temps.Location = new System.Drawing.Point(0, 67);
            this.lblDay3Temps.Name = "lblDay3Temps";
            this.lblDay3Temps.Size = new System.Drawing.Size(64, 23);
            this.lblDay3Temps.TabIndex = 75;
            this.lblDay3Temps.Text = "0° 0°";
            this.lblDay3Temps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picDay3Image
            // 
            this.picDay3Image.BackColor = System.Drawing.Color.Transparent;
            this.picDay3Image.ErrorImage = null;
            this.picDay3Image.Location = new System.Drawing.Point(11, 28);
            this.picDay3Image.Name = "picDay3Image";
            this.picDay3Image.Size = new System.Drawing.Size(40, 40);
            this.picDay3Image.TabIndex = 57;
            this.picDay3Image.TabStop = false;
            // 
            // pnlDay2
            // 
            this.pnlDay2.BackColor = System.Drawing.Color.Transparent;
            this.pnlDay2.Controls.Add(this.lblDay2Day);
            this.pnlDay2.Controls.Add(this.lblDay2Temps);
            this.pnlDay2.Controls.Add(this.picDay2Image);
            this.pnlDay2.Location = new System.Drawing.Point(74, 176);
            this.pnlDay2.Name = "pnlDay2";
            this.pnlDay2.Size = new System.Drawing.Size(64, 94);
            this.pnlDay2.TabIndex = 113;
            // 
            // lblDay2Day
            // 
            this.lblDay2Day.BackColor = System.Drawing.Color.Transparent;
            this.lblDay2Day.Font = new System.Drawing.Font("Samsung Sans", 11.25F);
            this.lblDay2Day.ForeColor = System.Drawing.Color.White;
            this.lblDay2Day.Location = new System.Drawing.Point(0, 3);
            this.lblDay2Day.Name = "lblDay2Day";
            this.lblDay2Day.Size = new System.Drawing.Size(64, 22);
            this.lblDay2Day.TabIndex = 77;
            this.lblDay2Day.Text = "Tue 02";
            this.lblDay2Day.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDay2Temps
            // 
            this.lblDay2Temps.BackColor = System.Drawing.Color.Transparent;
            this.lblDay2Temps.Font = new System.Drawing.Font("Samsung Sans", 11.25F);
            this.lblDay2Temps.ForeColor = System.Drawing.Color.White;
            this.lblDay2Temps.Location = new System.Drawing.Point(0, 67);
            this.lblDay2Temps.Name = "lblDay2Temps";
            this.lblDay2Temps.Size = new System.Drawing.Size(64, 23);
            this.lblDay2Temps.TabIndex = 75;
            this.lblDay2Temps.Text = "0° 0°";
            this.lblDay2Temps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picDay2Image
            // 
            this.picDay2Image.BackColor = System.Drawing.Color.Transparent;
            this.picDay2Image.ErrorImage = null;
            this.picDay2Image.Location = new System.Drawing.Point(11, 28);
            this.picDay2Image.Name = "picDay2Image";
            this.picDay2Image.Size = new System.Drawing.Size(40, 40);
            this.picDay2Image.TabIndex = 57;
            this.picDay2Image.TabStop = false;
            // 
            // pnlDay4
            // 
            this.pnlDay4.BackColor = System.Drawing.Color.Transparent;
            this.pnlDay4.Controls.Add(this.lblDay4Day);
            this.pnlDay4.Controls.Add(this.lblDay4Temps);
            this.pnlDay4.Controls.Add(this.picDay4Image);
            this.pnlDay4.Location = new System.Drawing.Point(208, 176);
            this.pnlDay4.Name = "pnlDay4";
            this.pnlDay4.Size = new System.Drawing.Size(64, 94);
            this.pnlDay4.TabIndex = 115;
            // 
            // lblDay4Day
            // 
            this.lblDay4Day.BackColor = System.Drawing.Color.Transparent;
            this.lblDay4Day.Font = new System.Drawing.Font("Segoe", 11.3F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDay4Day.ForeColor = System.Drawing.Color.White;
            this.lblDay4Day.Location = new System.Drawing.Point(0, 3);
            this.lblDay4Day.Name = "lblDay4Day";
            this.lblDay4Day.Size = new System.Drawing.Size(64, 22);
            this.lblDay4Day.TabIndex = 77;
            this.lblDay4Day.Text = "Thu 04";
            this.lblDay4Day.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDay4Temps
            // 
            this.lblDay4Temps.BackColor = System.Drawing.Color.Transparent;
            this.lblDay4Temps.Font = new System.Drawing.Font("Samsung Sans", 11.25F);
            this.lblDay4Temps.ForeColor = System.Drawing.Color.White;
            this.lblDay4Temps.Location = new System.Drawing.Point(0, 67);
            this.lblDay4Temps.Name = "lblDay4Temps";
            this.lblDay4Temps.Size = new System.Drawing.Size(64, 23);
            this.lblDay4Temps.TabIndex = 75;
            this.lblDay4Temps.Text = "0° 0°";
            this.lblDay4Temps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picDay4Image
            // 
            this.picDay4Image.BackColor = System.Drawing.Color.Transparent;
            this.picDay4Image.ErrorImage = null;
            this.picDay4Image.Location = new System.Drawing.Point(11, 28);
            this.picDay4Image.Name = "picDay4Image";
            this.picDay4Image.Size = new System.Drawing.Size(40, 40);
            this.picDay4Image.TabIndex = 57;
            this.picDay4Image.TabStop = false;
            // 
            // lblBorder
            // 
            this.lblBorder.BackColor = System.Drawing.Color.Transparent;
            this.lblBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBorder.ForeColor = System.Drawing.Color.White;
            this.lblBorder.Location = new System.Drawing.Point(248, 29);
            this.lblBorder.Name = "lblBorder";
            this.lblBorder.Size = new System.Drawing.Size(41, 1);
            this.lblBorder.TabIndex = 122;
            // 
            // pnlDay5
            // 
            this.pnlDay5.BackColor = System.Drawing.Color.Transparent;
            this.pnlDay5.Controls.Add(this.lblDay5Day);
            this.pnlDay5.Controls.Add(this.lblDay5Temps);
            this.pnlDay5.Controls.Add(this.picDay5Image);
            this.pnlDay5.Location = new System.Drawing.Point(275, 176);
            this.pnlDay5.Name = "pnlDay5";
            this.pnlDay5.Size = new System.Drawing.Size(64, 94);
            this.pnlDay5.TabIndex = 116;
            // 
            // lblDay5Day
            // 
            this.lblDay5Day.BackColor = System.Drawing.Color.Transparent;
            this.lblDay5Day.Font = new System.Drawing.Font("Samsung Sans", 11.25F);
            this.lblDay5Day.ForeColor = System.Drawing.Color.White;
            this.lblDay5Day.Location = new System.Drawing.Point(0, 3);
            this.lblDay5Day.Name = "lblDay5Day";
            this.lblDay5Day.Size = new System.Drawing.Size(64, 22);
            this.lblDay5Day.TabIndex = 77;
            this.lblDay5Day.Text = "Fri 05";
            this.lblDay5Day.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDay5Temps
            // 
            this.lblDay5Temps.BackColor = System.Drawing.Color.Transparent;
            this.lblDay5Temps.Font = new System.Drawing.Font("Samsung Sans", 11.25F);
            this.lblDay5Temps.ForeColor = System.Drawing.Color.White;
            this.lblDay5Temps.Location = new System.Drawing.Point(0, 67);
            this.lblDay5Temps.Name = "lblDay5Temps";
            this.lblDay5Temps.Size = new System.Drawing.Size(64, 23);
            this.lblDay5Temps.TabIndex = 75;
            this.lblDay5Temps.Text = "0° 0°";
            this.lblDay5Temps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picDay5Image
            // 
            this.picDay5Image.BackColor = System.Drawing.Color.Transparent;
            this.picDay5Image.ErrorImage = null;
            this.picDay5Image.Location = new System.Drawing.Point(11, 28);
            this.picDay5Image.Name = "picDay5Image";
            this.picDay5Image.Size = new System.Drawing.Size(40, 40);
            this.picDay5Image.TabIndex = 57;
            this.picDay5Image.TabStop = false;
            // 
            // pnlDay1
            // 
            this.pnlDay1.BackColor = System.Drawing.Color.Transparent;
            this.pnlDay1.Controls.Add(this.lblDay1Day);
            this.pnlDay1.Controls.Add(this.lblDay1Temps);
            this.pnlDay1.Controls.Add(this.picDay1Image);
            this.pnlDay1.Location = new System.Drawing.Point(6, 176);
            this.pnlDay1.Name = "pnlDay1";
            this.pnlDay1.Size = new System.Drawing.Size(64, 94);
            this.pnlDay1.TabIndex = 103;
            // 
            // lblDay1Day
            // 
            this.lblDay1Day.BackColor = System.Drawing.Color.Transparent;
            this.lblDay1Day.Font = new System.Drawing.Font("Samsung Sans", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDay1Day.ForeColor = System.Drawing.Color.White;
            this.lblDay1Day.Location = new System.Drawing.Point(0, 3);
            this.lblDay1Day.Name = "lblDay1Day";
            this.lblDay1Day.Size = new System.Drawing.Size(64, 22);
            this.lblDay1Day.TabIndex = 77;
            this.lblDay1Day.Text = "Mon 01";
            this.lblDay1Day.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDay1Temps
            // 
            this.lblDay1Temps.BackColor = System.Drawing.Color.Transparent;
            this.lblDay1Temps.Font = new System.Drawing.Font("Samsung Sans", 11.25F);
            this.lblDay1Temps.ForeColor = System.Drawing.Color.White;
            this.lblDay1Temps.Location = new System.Drawing.Point(0, 67);
            this.lblDay1Temps.Name = "lblDay1Temps";
            this.lblDay1Temps.Size = new System.Drawing.Size(64, 23);
            this.lblDay1Temps.TabIndex = 75;
            this.lblDay1Temps.Text = "0° 0°";
            this.lblDay1Temps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picDay1Image
            // 
            this.picDay1Image.BackColor = System.Drawing.Color.Transparent;
            this.picDay1Image.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picDay1Image.ErrorImage = null;
            this.picDay1Image.InitialImage = null;
            this.picDay1Image.Location = new System.Drawing.Point(11, 28);
            this.picDay1Image.Name = "picDay1Image";
            this.picDay1Image.Size = new System.Drawing.Size(40, 40);
            this.picDay1Image.TabIndex = 57;
            this.picDay1Image.TabStop = false;
            // 
            // lblFeelsLike
            // 
            this.lblFeelsLike.BackColor = System.Drawing.Color.Transparent;
            this.lblFeelsLike.Font = new System.Drawing.Font("Samsung Sans", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFeelsLike.ForeColor = System.Drawing.Color.White;
            this.lblFeelsLike.Location = new System.Drawing.Point(139, 47);
            this.lblFeelsLike.Name = "lblFeelsLike";
            this.lblFeelsLike.Size = new System.Drawing.Size(198, 22);
            this.lblFeelsLike.TabIndex = 109;
            this.lblFeelsLike.Text = "Feels Like 97°";
            this.lblFeelsLike.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // picOffline
            // 
            this.picOffline.BackColor = System.Drawing.Color.Transparent;
            this.picOffline.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picOffline.BackgroundImage")));
            this.picOffline.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picOffline.ErrorImage = null;
            this.picOffline.Location = new System.Drawing.Point(287, 6);
            this.picOffline.Name = "picOffline";
            this.picOffline.Size = new System.Drawing.Size(21, 21);
            this.picOffline.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOffline.TabIndex = 104;
            this.picOffline.TabStop = false;
            // 
            // lblDayLow
            // 
            this.lblDayLow.BackColor = System.Drawing.Color.Transparent;
            this.lblDayLow.Font = new System.Drawing.Font("Samsung Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDayLow.ForeColor = System.Drawing.Color.White;
            this.lblDayLow.Location = new System.Drawing.Point(248, 27);
            this.lblDayLow.Name = "lblDayLow";
            this.lblDayLow.Size = new System.Drawing.Size(41, 22);
            this.lblDayLow.TabIndex = 108;
            this.lblDayLow.Text = "75°";
            this.lblDayLow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picRefresh
            // 
            this.picRefresh.BackColor = System.Drawing.Color.Transparent;
            this.picRefresh.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picRefresh.BackgroundImage")));
            this.picRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picRefresh.Location = new System.Drawing.Point(311, 6);
            this.picRefresh.Name = "picRefresh";
            this.picRefresh.Size = new System.Drawing.Size(21, 21);
            this.picRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picRefresh.TabIndex = 105;
            this.picRefresh.TabStop = false;
            this.picRefresh.Click += new System.EventHandler(this.picRefresh_Click);
            // 
            // lblDayHigh
            // 
            this.lblDayHigh.BackColor = System.Drawing.Color.Transparent;
            this.lblDayHigh.Font = new System.Drawing.Font("Samsung Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDayHigh.ForeColor = System.Drawing.Color.White;
            this.lblDayHigh.Location = new System.Drawing.Point(248, 9);
            this.lblDayHigh.Name = "lblDayHigh";
            this.lblDayHigh.Size = new System.Drawing.Size(41, 22);
            this.lblDayHigh.TabIndex = 107;
            this.lblDayHigh.Text = "96°";
            this.lblDayHigh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrentTemperature
            // 
            this.lblCurrentTemperature.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentTemperature.Font = new System.Drawing.Font("Samsung Sans", 32.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentTemperature.ForeColor = System.Drawing.Color.White;
            this.lblCurrentTemperature.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblCurrentTemperature.Location = new System.Drawing.Point(132, 5);
            this.lblCurrentTemperature.Name = "lblCurrentTemperature";
            this.lblCurrentTemperature.Size = new System.Drawing.Size(118, 47);
            this.lblCurrentTemperature.TabIndex = 106;
            this.lblCurrentTemperature.Text = "87°F";
            // 
            // btnSunrise
            // 
            this.btnSunrise.BackColor = System.Drawing.Color.Transparent;
            this.btnSunrise.FlatAppearance.BorderSize = 0;
            this.btnSunrise.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSunrise.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSunrise.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSunrise.Font = new System.Drawing.Font("Samsung Sans", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSunrise.ForeColor = System.Drawing.Color.White;
            this.btnSunrise.Image = ((System.Drawing.Image)(resources.GetObject("btnSunrise.Image")));
            this.btnSunrise.Location = new System.Drawing.Point(14, 134);
            this.btnSunrise.Name = "btnSunrise";
            this.btnSunrise.Size = new System.Drawing.Size(94, 45);
            this.btnSunrise.TabIndex = 119;
            this.btnSunrise.Text = "6:29 AM";
            this.btnSunrise.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSunrise.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSunrise.UseVisualStyleBackColor = false;
            // 
            // tlsPreferences
            // 
            this.tlsPreferences.Image = ((System.Drawing.Image)(resources.GetObject("tlsPreferences.Image")));
            this.tlsPreferences.Name = "tlsPreferences";
            this.tlsPreferences.Size = new System.Drawing.Size(161, 22);
            this.tlsPreferences.Text = "Preferences";
            this.tlsPreferences.Click += new System.EventHandler(this.tlsPreferences_Click);
            // 
            // tlsExit
            // 
            this.tlsExit.Image = ((System.Drawing.Image)(resources.GetObject("tlsExit.Image")));
            this.tlsExit.Name = "tlsExit";
            this.tlsExit.Size = new System.Drawing.Size(161, 22);
            this.tlsExit.Text = "Exit";
            this.tlsExit.Click += new System.EventHandler(this.tlsExit_Click);
            // 
            // cmOptions
            // 
            this.cmOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlsAddKey,
            this.tlsPreferences,
            this.tlsRefresh,
            this.tlsSeparator1,
            this.tlsExit});
            this.cmOptions.Name = "cmOptions";
            this.cmOptions.Size = new System.Drawing.Size(162, 98);
            // 
            // tlsAddKey
            // 
            this.tlsAddKey.Image = ((System.Drawing.Image)(resources.GetObject("tlsAddKey.Image")));
            this.tlsAddKey.Name = "tlsAddKey";
            this.tlsAddKey.Size = new System.Drawing.Size(161, 22);
            this.tlsAddKey.Text = "Add/Delete Keys";
            this.tlsAddKey.Click += new System.EventHandler(this.tlsAddKey_Click);
            // 
            // tlsRefresh
            // 
            this.tlsRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tlsRefresh.Image")));
            this.tlsRefresh.Name = "tlsRefresh";
            this.tlsRefresh.Size = new System.Drawing.Size(161, 22);
            this.tlsRefresh.Text = "Refresh";
            this.tlsRefresh.Click += new System.EventHandler(this.tlsRefresh_Click);
            // 
            // tlsSeparator1
            // 
            this.tlsSeparator1.Name = "tlsSeparator1";
            this.tlsSeparator1.Size = new System.Drawing.Size(158, 6);
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 900000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // tmrCurrentTime
            // 
            this.tmrCurrentTime.Tick += new System.EventHandler(this.tmrCurrentTime_Tick);
            // 
            // tmrIconUpdater
            // 
            this.tmrIconUpdater.Interval = 500;
            this.tmrIconUpdater.Tick += new System.EventHandler(this.tmrIconUpdater_Tick);
            // 
            // tmrPreferenceUpdate
            // 
            this.tmrPreferenceUpdate.Tick += new System.EventHandler(this.tmrPreferenceUpdate_Tick);
            // 
            // WidgetForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BlendedBackground = ((System.Drawing.Bitmap)(resources.GetObject("$this.BlendedBackground")));
            this.ClientSize = new System.Drawing.Size(340, 288);
            this.Controls.Add(this.pnlWidget);
            this.DoubleBuffered = true;
            this.DrawControlBackgrounds = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WidgetForm";
            this.Opacity = 0.95D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Weather Lion Widget";
            this.TransparencyKey = System.Drawing.Color.DarkGray;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWidget_FormClosing);
            this.Load += new System.EventHandler(this.frmWidget_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmWidget_MouseDown);
            this.pnlWidget.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCurrentConditions)).EndInit();
            this.flpAtmosphere.ResumeLayout(false);
            this.flpAtmosphere.PerformLayout();
            this.pnlDay3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picDay3Image)).EndInit();
            this.pnlDay2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picDay2Image)).EndInit();
            this.pnlDay4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picDay4Image)).EndInit();
            this.pnlDay5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picDay5Image)).EndInit();
            this.pnlDay1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picDay1Image)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOffline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).EndInit();
            this.cmOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlWidget;
        private System.Windows.Forms.Panel pnlDay3;
        public System.Windows.Forms.PictureBox picDay3Image;
        private System.Windows.Forms.Panel pnlDay2;
        public System.Windows.Forms.PictureBox picDay2Image;
        private System.Windows.Forms.Panel pnlDay4;
        public System.Windows.Forms.PictureBox picDay4Image;
        private System.Windows.Forms.Panel pnlDay5;
        public System.Windows.Forms.PictureBox picDay5Image;
        public System.Windows.Forms.PictureBox picCurrentConditions;
        private System.Windows.Forms.Panel pnlDay1;
        public System.Windows.Forms.PictureBox picDay1Image;
        public System.Windows.Forms.PictureBox picOffline;
        public System.Windows.Forms.PictureBox picRefresh;
        private System.Windows.Forms.ToolStripMenuItem tlsPreferences;
        private System.Windows.Forms.ToolStripMenuItem tlsExit;
        private System.Windows.Forms.ContextMenuStrip cmOptions;
        private System.Windows.Forms.ToolStripMenuItem tlsRefresh;
        private System.Windows.Forms.ToolStripSeparator tlsSeparator1;
        public System.Windows.Forms.Timer tmrUpdate;
        public System.Windows.Forms.Button btnSunrise;
        public System.Windows.Forms.Button btnLocation;
        public System.Windows.Forms.Label lblWeatherCondition;
        public System.Windows.Forms.Label lblFeelsLike;
        public System.Windows.Forms.Label lblCurrentTemperature;
        public System.Windows.Forms.Button btnWeatherProvider;
        public System.Windows.Forms.Label lblDay3Day;
        public System.Windows.Forms.Label lblDay3Temps;
        public System.Windows.Forms.Label lblDay2Day;
        public System.Windows.Forms.Label lblDay2Temps;
        public System.Windows.Forms.Label lblDay4Day;
        public System.Windows.Forms.Label lblDay4Temps;
        public System.Windows.Forms.Label lblDay5Day;
        public System.Windows.Forms.Label lblDay5Temps;
        public System.Windows.Forms.Label lblDay1Day;
        public System.Windows.Forms.Label lblDay1Temps;
        private System.Windows.Forms.ToolStripMenuItem tlsAddKey;
        public System.Windows.Forms.Button btnWindReading;
        public System.Windows.Forms.Button btnHumidity;
        public System.Windows.Forms.FlowLayoutPanel flpAtmosphere;
        public System.Windows.Forms.Label lblBorder;
        public System.Windows.Forms.Label lblDayLow;
        public System.Windows.Forms.Label lblDayHigh;
        public TheArtOfDev.HtmlRenderer.WinForms.HtmlLabel lblClock;
        private System.Windows.Forms.Timer tmrCurrentTime;
        public System.Windows.Forms.Timer tmrIconUpdater;
        private System.Windows.Forms.Timer tmrPreferenceUpdate;
        public System.Windows.Forms.Button btnSunset;
    }
}