namespace WeatherLion
{
    partial class PreferencesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesForm));
            this.tabBackground = new System.Windows.Forms.TabPage();
            this.flpRabalac = new System.Windows.Forms.FlowLayoutPanel();
            this.radRabalac = new System.Windows.Forms.RadioButton();
            this.picRabalac = new System.Windows.Forms.PictureBox();
            this.lblRabalac = new System.Windows.Forms.Label();
            this.flpAndroid = new System.Windows.Forms.FlowLayoutPanel();
            this.radAndroid = new System.Windows.Forms.RadioButton();
            this.picAndroid = new System.Windows.Forms.PictureBox();
            this.lblAndroid = new System.Windows.Forms.Label();
            this.flpDefault = new System.Windows.Forms.FlowLayoutPanel();
            this.radDefault = new System.Windows.Forms.RadioButton();
            this.picDefault = new System.Windows.Forms.PictureBox();
            this.lblDefaultTitle = new System.Windows.Forms.Label();
            this.tabPreferences = new System.Windows.Forms.TabControl();
            this.tabWeather = new System.Windows.Forms.TabPage();
            this.chkUseMetric = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblUnits = new System.Windows.Forms.Label();
            this.chkUseSystemLocation = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblGeolocation = new System.Windows.Forms.Label();
            this.cboLocation = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblInterval = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboRefreshInterval = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDataDescription = new System.Windows.Forms.Label();
            this.cboWeatherProviders = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLocationDescription = new System.Windows.Forms.Label();
            this.lblLine = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.lblProvider = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.tabIconSet = new System.Windows.Forms.TabPage();
            this.flpIconSet = new System.Windows.Forms.FlowLayoutPanel();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.rtbAbout = new System.Windows.Forms.RichTextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.tabBackground.SuspendLayout();
            this.flpRabalac.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRabalac)).BeginInit();
            this.flpAndroid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAndroid)).BeginInit();
            this.flpDefault.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDefault)).BeginInit();
            this.tabPreferences.SuspendLayout();
            this.tabWeather.SuspendLayout();
            this.tabIconSet.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabBackground
            // 
            this.tabBackground.Controls.Add(this.flpRabalac);
            this.tabBackground.Controls.Add(this.flpAndroid);
            this.tabBackground.Controls.Add(this.flpDefault);
            this.tabBackground.Location = new System.Drawing.Point(4, 22);
            this.tabBackground.Name = "tabBackground";
            this.tabBackground.Padding = new System.Windows.Forms.Padding(3);
            this.tabBackground.Size = new System.Drawing.Size(652, 612);
            this.tabBackground.TabIndex = 3;
            this.tabBackground.Text = "Background";
            this.tabBackground.UseVisualStyleBackColor = true;
            // 
            // flpRabalac
            // 
            this.flpRabalac.Controls.Add(this.radRabalac);
            this.flpRabalac.Controls.Add(this.picRabalac);
            this.flpRabalac.Controls.Add(this.lblRabalac);
            this.flpRabalac.Location = new System.Drawing.Point(16, 399);
            this.flpRabalac.Name = "flpRabalac";
            this.flpRabalac.Size = new System.Drawing.Size(263, 135);
            this.flpRabalac.TabIndex = 8;
            // 
            // radRabalac
            // 
            this.radRabalac.Dock = System.Windows.Forms.DockStyle.Left;
            this.radRabalac.Location = new System.Drawing.Point(3, 3);
            this.radRabalac.Name = "radRabalac";
            this.radRabalac.Size = new System.Drawing.Size(17, 132);
            this.radRabalac.TabIndex = 0;
            this.radRabalac.TabStop = true;
            this.radRabalac.Tag = "rabalac";
            this.radRabalac.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radRabalac.UseVisualStyleBackColor = true;
            this.radRabalac.CheckedChanged += new System.EventHandler(this.radRabalac_CheckedChanged);
            // 
            // picRabalac
            // 
            this.picRabalac.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.picRabalac.Location = new System.Drawing.Point(26, 3);
            this.picRabalac.Name = "picRabalac";
            this.picRabalac.Size = new System.Drawing.Size(159, 132);
            this.picRabalac.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picRabalac.TabIndex = 1;
            this.picRabalac.TabStop = false;
            this.picRabalac.Click += new System.EventHandler(this.picRabalac_Click);
            // 
            // lblRabalac
            // 
            this.lblRabalac.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblRabalac.Font = new System.Drawing.Font("Arial", 11F);
            this.lblRabalac.Location = new System.Drawing.Point(191, 0);
            this.lblRabalac.Name = "lblRabalac";
            this.lblRabalac.Size = new System.Drawing.Size(68, 138);
            this.lblRabalac.TabIndex = 2;
            this.lblRabalac.Text = "Rabalac";
            this.lblRabalac.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flpAndroid
            // 
            this.flpAndroid.Controls.Add(this.radAndroid);
            this.flpAndroid.Controls.Add(this.picAndroid);
            this.flpAndroid.Controls.Add(this.lblAndroid);
            this.flpAndroid.Location = new System.Drawing.Point(16, 205);
            this.flpAndroid.Name = "flpAndroid";
            this.flpAndroid.Size = new System.Drawing.Size(263, 135);
            this.flpAndroid.TabIndex = 7;
            // 
            // radAndroid
            // 
            this.radAndroid.Dock = System.Windows.Forms.DockStyle.Left;
            this.radAndroid.Location = new System.Drawing.Point(3, 3);
            this.radAndroid.Name = "radAndroid";
            this.radAndroid.Size = new System.Drawing.Size(17, 132);
            this.radAndroid.TabIndex = 0;
            this.radAndroid.TabStop = true;
            this.radAndroid.Tag = "android";
            this.radAndroid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radAndroid.UseVisualStyleBackColor = true;
            this.radAndroid.CheckedChanged += new System.EventHandler(this.radAndroid_CheckedChanged);
            // 
            // picAndroid
            // 
            this.picAndroid.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.picAndroid.Location = new System.Drawing.Point(26, 3);
            this.picAndroid.Name = "picAndroid";
            this.picAndroid.Size = new System.Drawing.Size(159, 132);
            this.picAndroid.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAndroid.TabIndex = 1;
            this.picAndroid.TabStop = false;
            this.picAndroid.Click += new System.EventHandler(this.picAndroid_Click);
            // 
            // lblAndroid
            // 
            this.lblAndroid.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblAndroid.Font = new System.Drawing.Font("Arial", 11F);
            this.lblAndroid.Location = new System.Drawing.Point(191, 0);
            this.lblAndroid.Name = "lblAndroid";
            this.lblAndroid.Size = new System.Drawing.Size(68, 138);
            this.lblAndroid.TabIndex = 2;
            this.lblAndroid.Text = "Android";
            this.lblAndroid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flpDefault
            // 
            this.flpDefault.Controls.Add(this.radDefault);
            this.flpDefault.Controls.Add(this.picDefault);
            this.flpDefault.Controls.Add(this.lblDefaultTitle);
            this.flpDefault.Location = new System.Drawing.Point(16, 13);
            this.flpDefault.Name = "flpDefault";
            this.flpDefault.Size = new System.Drawing.Size(263, 135);
            this.flpDefault.TabIndex = 6;
            // 
            // radDefault
            // 
            this.radDefault.Dock = System.Windows.Forms.DockStyle.Left;
            this.radDefault.Location = new System.Drawing.Point(3, 3);
            this.radDefault.Name = "radDefault";
            this.radDefault.Size = new System.Drawing.Size(17, 132);
            this.radDefault.TabIndex = 0;
            this.radDefault.TabStop = true;
            this.radDefault.Tag = "default";
            this.radDefault.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radDefault.UseVisualStyleBackColor = true;
            this.radDefault.CheckedChanged += new System.EventHandler(this.radDefault_CheckedChanged);
            // 
            // picDefault
            // 
            this.picDefault.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.picDefault.Location = new System.Drawing.Point(26, 3);
            this.picDefault.Name = "picDefault";
            this.picDefault.Size = new System.Drawing.Size(159, 132);
            this.picDefault.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDefault.TabIndex = 1;
            this.picDefault.TabStop = false;
            this.picDefault.Click += new System.EventHandler(this.picDefault_Click);
            // 
            // lblDefaultTitle
            // 
            this.lblDefaultTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDefaultTitle.Font = new System.Drawing.Font("Arial", 11F);
            this.lblDefaultTitle.Location = new System.Drawing.Point(191, 0);
            this.lblDefaultTitle.Name = "lblDefaultTitle";
            this.lblDefaultTitle.Size = new System.Drawing.Size(68, 138);
            this.lblDefaultTitle.TabIndex = 2;
            this.lblDefaultTitle.Text = "Default";
            this.lblDefaultTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPreferences
            // 
            this.tabPreferences.Controls.Add(this.tabWeather);
            this.tabPreferences.Controls.Add(this.tabIconSet);
            this.tabPreferences.Controls.Add(this.tabBackground);
            this.tabPreferences.Controls.Add(this.tabAbout);
            this.tabPreferences.Location = new System.Drawing.Point(3, 12);
            this.tabPreferences.Name = "tabPreferences";
            this.tabPreferences.SelectedIndex = 0;
            this.tabPreferences.Size = new System.Drawing.Size(660, 638);
            this.tabPreferences.TabIndex = 8;
            // 
            // tabWeather
            // 
            this.tabWeather.AutoScroll = true;
            this.tabWeather.Controls.Add(this.chkUseMetric);
            this.tabWeather.Controls.Add(this.label7);
            this.tabWeather.Controls.Add(this.lblUnits);
            this.tabWeather.Controls.Add(this.chkUseSystemLocation);
            this.tabWeather.Controls.Add(this.label5);
            this.tabWeather.Controls.Add(this.lblGeolocation);
            this.tabWeather.Controls.Add(this.cboLocation);
            this.tabWeather.Controls.Add(this.btnSearch);
            this.tabWeather.Controls.Add(this.lblInterval);
            this.tabWeather.Controls.Add(this.label4);
            this.tabWeather.Controls.Add(this.label3);
            this.tabWeather.Controls.Add(this.cboRefreshInterval);
            this.tabWeather.Controls.Add(this.label2);
            this.tabWeather.Controls.Add(this.lblDataDescription);
            this.tabWeather.Controls.Add(this.cboWeatherProviders);
            this.tabWeather.Controls.Add(this.label1);
            this.tabWeather.Controls.Add(this.lblLocationDescription);
            this.tabWeather.Controls.Add(this.lblLine);
            this.tabWeather.Controls.Add(this.lblData);
            this.tabWeather.Controls.Add(this.lblProvider);
            this.tabWeather.Controls.Add(this.lblLocation);
            this.tabWeather.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabWeather.Location = new System.Drawing.Point(4, 22);
            this.tabWeather.Name = "tabWeather";
            this.tabWeather.Padding = new System.Windows.Forms.Padding(3);
            this.tabWeather.Size = new System.Drawing.Size(652, 612);
            this.tabWeather.TabIndex = 0;
            this.tabWeather.Text = "Weather";
            this.tabWeather.UseVisualStyleBackColor = true;
            // 
            // chkUseMetric
            // 
            this.chkUseMetric.AutoSize = true;
            this.chkUseMetric.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseMetric.Location = new System.Drawing.Point(16, 573);
            this.chkUseMetric.Name = "chkUseMetric";
            this.chkUseMetric.Size = new System.Drawing.Size(138, 22);
            this.chkUseMetric.TabIndex = 29;
            this.chkUseMetric.Text = "Use Metric ( °C )";
            this.chkUseMetric.UseVisualStyleBackColor = true;
            this.chkUseMetric.CheckedChanged += new System.EventHandler(this.chkUseMetric_CheckedChanged);
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(14, 560);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(616, 2);
            this.label7.TabIndex = 28;
            // 
            // lblUnits
            // 
            this.lblUnits.AutoSize = true;
            this.lblUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnits.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblUnits.Location = new System.Drawing.Point(5, 529);
            this.lblUnits.Name = "lblUnits";
            this.lblUnits.Size = new System.Drawing.Size(67, 29);
            this.lblUnits.TabIndex = 27;
            this.lblUnits.Text = "Units";
            // 
            // chkUseSystemLocation
            // 
            this.chkUseSystemLocation.AutoSize = true;
            this.chkUseSystemLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseSystemLocation.Location = new System.Drawing.Point(16, 492);
            this.chkUseSystemLocation.Name = "chkUseSystemLocation";
            this.chkUseSystemLocation.Size = new System.Drawing.Size(214, 22);
            this.chkUseSystemLocation.TabIndex = 26;
            this.chkUseSystemLocation.Text = "Use the system\'s IP location";
            this.chkUseSystemLocation.UseVisualStyleBackColor = true;
            this.chkUseSystemLocation.CheckedChanged += new System.EventHandler(this.chkUseSystemLocation_CheckedChanged);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 480);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(616, 2);
            this.label5.TabIndex = 25;
            // 
            // lblGeolocation
            // 
            this.lblGeolocation.AutoSize = true;
            this.lblGeolocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGeolocation.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblGeolocation.Location = new System.Drawing.Point(6, 448);
            this.lblGeolocation.Name = "lblGeolocation";
            this.lblGeolocation.Size = new System.Drawing.Size(192, 29);
            this.lblGeolocation.TabIndex = 24;
            this.lblGeolocation.Text = "Use Geolocation";
            // 
            // cboLocation
            // 
            this.cboLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLocation.FormattingEnabled = true;
            this.cboLocation.Location = new System.Drawing.Point(12, 57);
            this.cboLocation.MaxDropDownItems = 25;
            this.cboLocation.Name = "cboLocation";
            this.cboLocation.Size = new System.Drawing.Size(491, 26);
            this.cboLocation.TabIndex = 23;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(509, 56);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(120, 28);
            this.btnSearch.TabIndex = 22;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.lblInterval.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblInterval.Location = new System.Drawing.Point(179, 385);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(57, 18);
            this.lblInterval.TabIndex = 21;
            this.lblInterval.Text = "15 min.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Location = new System.Drawing.Point(10, 383);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(168, 18);
            this.label4.TabIndex = 20;
            this.label4.Text = "Weather refresh interval:";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 369);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(616, 2);
            this.label3.TabIndex = 19;
            // 
            // cboRefreshInterval
            // 
            this.cboRefreshInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRefreshInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRefreshInterval.FormattingEnabled = true;
            this.cboRefreshInterval.Items.AddRange(new object[] {
            "15",
            "30",
            "60"});
            this.cboRefreshInterval.Location = new System.Drawing.Point(13, 407);
            this.cboRefreshInterval.Name = "cboRefreshInterval";
            this.cboRefreshInterval.Size = new System.Drawing.Size(617, 26);
            this.cboRefreshInterval.TabIndex = 18;
            this.cboRefreshInterval.SelectedValueChanged += new System.EventHandler(this.cboRefreshInterval_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(6, 337);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 29);
            this.label2.TabIndex = 17;
            this.label2.Text = "Refresh Interval";
            // 
            // lblDataDescription
            // 
            this.lblDataDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataDescription.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblDataDescription.Location = new System.Drawing.Point(10, 288);
            this.lblDataDescription.Name = "lblDataDescription";
            this.lblDataDescription.Size = new System.Drawing.Size(606, 39);
            this.lblDataDescription.TabIndex = 16;
            this.lblDataDescription.Text = "Weather providers get weather data from different internet sources. They can diff" +
    "er by number of locations and provide different data.";
            // 
            // cboWeatherProviders
            // 
            this.cboWeatherProviders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWeatherProviders.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboWeatherProviders.FormattingEnabled = true;
            this.cboWeatherProviders.Location = new System.Drawing.Point(13, 246);
            this.cboWeatherProviders.Name = "cboWeatherProviders";
            this.cboWeatherProviders.Size = new System.Drawing.Size(617, 26);
            this.cboWeatherProviders.TabIndex = 15;
            this.cboWeatherProviders.SelectedValueChanged += new System.EventHandler(this.cboWeatherProviders_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 199);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(616, 2);
            this.label1.TabIndex = 14;
            // 
            // lblLocationDescription
            // 
            this.lblLocationDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocationDescription.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblLocationDescription.Location = new System.Drawing.Point(10, 95);
            this.lblLocationDescription.Name = "lblLocationDescription";
            this.lblLocationDescription.Size = new System.Drawing.Size(619, 62);
            this.lblLocationDescription.TabIndex = 13;
            this.lblLocationDescription.Text = resources.GetString("lblLocationDescription.Text");
            // 
            // lblLine
            // 
            this.lblLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLine.Location = new System.Drawing.Point(14, 41);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(616, 2);
            this.lblLine.TabIndex = 10;
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblData.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblData.Location = new System.Drawing.Point(6, 168);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(62, 29);
            this.lblData.TabIndex = 9;
            this.lblData.Text = "Data";
            // 
            // lblProvider
            // 
            this.lblProvider.AutoSize = true;
            this.lblProvider.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProvider.Location = new System.Drawing.Point(13, 216);
            this.lblProvider.Name = "lblProvider";
            this.lblProvider.Size = new System.Drawing.Size(125, 18);
            this.lblProvider.TabIndex = 8;
            this.lblProvider.Text = "Weather provider:";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblLocation.Location = new System.Drawing.Point(6, 7);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(104, 29);
            this.lblLocation.TabIndex = 11;
            this.lblLocation.Text = "Location";
            // 
            // tabIconSet
            // 
            this.tabIconSet.Controls.Add(this.flpIconSet);
            this.tabIconSet.Location = new System.Drawing.Point(4, 22);
            this.tabIconSet.Name = "tabIconSet";
            this.tabIconSet.Padding = new System.Windows.Forms.Padding(3);
            this.tabIconSet.Size = new System.Drawing.Size(652, 612);
            this.tabIconSet.TabIndex = 2;
            this.tabIconSet.Text = "Icon Set";
            this.tabIconSet.UseVisualStyleBackColor = true;
            // 
            // flpIconSet
            // 
            this.flpIconSet.Location = new System.Drawing.Point(27, 22);
            this.flpIconSet.Name = "flpIconSet";
            this.flpIconSet.Size = new System.Drawing.Size(594, 604);
            this.flpIconSet.TabIndex = 0;
            // 
            // tabAbout
            // 
            this.tabAbout.Controls.Add(this.rtbAbout);
            this.tabAbout.Location = new System.Drawing.Point(4, 22);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabAbout.Size = new System.Drawing.Size(652, 612);
            this.tabAbout.TabIndex = 1;
            this.tabAbout.Text = "About";
            this.tabAbout.UseVisualStyleBackColor = true;
            // 
            // rtbAbout
            // 
            this.rtbAbout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbAbout.Font = new System.Drawing.Font("Arial", 11F);
            this.rtbAbout.Location = new System.Drawing.Point(6, 43);
            this.rtbAbout.Name = "rtbAbout";
            this.rtbAbout.Size = new System.Drawing.Size(640, 449);
            this.rtbAbout.TabIndex = 15;
            this.rtbAbout.Text = "";
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(422, 656);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 28);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(298, 656);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(116, 28);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(544, 656);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(116, 28);
            this.btnApply.TabIndex = 11;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 687);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabPreferences);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(682, 730);
            this.MinimizeBox = false;
            this.Name = "PreferencesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            this.Activated += new System.EventHandler(this.frmPreferences_Activated);
            this.Load += new System.EventHandler(this.frmPreferences_Load);
            this.tabBackground.ResumeLayout(false);
            this.flpRabalac.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picRabalac)).EndInit();
            this.flpAndroid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAndroid)).EndInit();
            this.flpDefault.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picDefault)).EndInit();
            this.tabPreferences.ResumeLayout(false);
            this.tabWeather.ResumeLayout(false);
            this.tabWeather.PerformLayout();
            this.tabIconSet.ResumeLayout(false);
            this.tabAbout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabBackground;
        private System.Windows.Forms.TabControl tabPreferences;
        private System.Windows.Forms.TabPage tabWeather;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblUnits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblGeolocation;
        public System.Windows.Forms.ComboBox cboLocation;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox cboRefreshInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDataDescription;
        public System.Windows.Forms.ComboBox cboWeatherProviders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLocationDescription;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Label lblProvider;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.TabPage tabIconSet;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.RichTextBox rtbAbout;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.FlowLayoutPanel flpRabalac;
        private System.Windows.Forms.RadioButton radRabalac;
        private System.Windows.Forms.PictureBox picRabalac;
        private System.Windows.Forms.Label lblRabalac;
        private System.Windows.Forms.FlowLayoutPanel flpAndroid;
        private System.Windows.Forms.RadioButton radAndroid;
        private System.Windows.Forms.PictureBox picAndroid;
        private System.Windows.Forms.Label lblAndroid;
        private System.Windows.Forms.FlowLayoutPanel flpDefault;
        private System.Windows.Forms.RadioButton radDefault;
        private System.Windows.Forms.PictureBox picDefault;
        private System.Windows.Forms.Label lblDefaultTitle;
        private System.Windows.Forms.FlowLayoutPanel flpIconSet;
        public System.Windows.Forms.CheckBox chkUseMetric;
        public System.Windows.Forms.CheckBox chkUseSystemLocation;
        public System.Windows.Forms.Button btnSearch;
    }
}