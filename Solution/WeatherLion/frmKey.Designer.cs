namespace WeatherLion
{
    partial class AccessKeysForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccessKeysForm));
            this.lblAccessProvider = new System.Windows.Forms.Label();
            this.lblKeyName = new System.Windows.Forms.Label();
            this.lblKeyValue = new System.Windows.Forms.Label();
            this.chkShowHidePwd = new System.Windows.Forms.CheckBox();
            this.cboAccessProvider = new System.Windows.Forms.ComboBox();
            this.txtKeyName = new System.Windows.Forms.TextBox();
            this.pwdKeyValue = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDeleteKey = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblAccessProvider
            // 
            this.lblAccessProvider.AutoSize = true;
            this.lblAccessProvider.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccessProvider.Location = new System.Drawing.Point(8, 26);
            this.lblAccessProvider.Name = "lblAccessProvider";
            this.lblAccessProvider.Size = new System.Drawing.Size(103, 16);
            this.lblAccessProvider.TabIndex = 0;
            this.lblAccessProvider.Text = "Access Provider:";
            // 
            // lblKeyName
            // 
            this.lblKeyName.AutoSize = true;
            this.lblKeyName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeyName.Location = new System.Drawing.Point(7, 56);
            this.lblKeyName.Name = "lblKeyName";
            this.lblKeyName.Size = new System.Drawing.Size(70, 16);
            this.lblKeyName.TabIndex = 1;
            this.lblKeyName.Text = "Key Name:";
            // 
            // lblKeyValue
            // 
            this.lblKeyValue.AutoSize = true;
            this.lblKeyValue.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeyValue.Location = new System.Drawing.Point(9, 85);
            this.lblKeyValue.Name = "lblKeyValue";
            this.lblKeyValue.Size = new System.Drawing.Size(69, 16);
            this.lblKeyValue.TabIndex = 2;
            this.lblKeyValue.Text = "Key Value:";
            // 
            // chkShowHidePwd
            // 
            this.chkShowHidePwd.AutoSize = true;
            this.chkShowHidePwd.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowHidePwd.Location = new System.Drawing.Point(288, 107);
            this.chkShowHidePwd.Name = "chkShowHidePwd";
            this.chkShowHidePwd.Size = new System.Drawing.Size(83, 20);
            this.chkShowHidePwd.TabIndex = 3;
            this.chkShowHidePwd.Text = "Show Key";
            this.chkShowHidePwd.UseVisualStyleBackColor = true;
            this.chkShowHidePwd.CheckedChanged += new System.EventHandler(this.chkShowHidePwd_CheckedChanged);
            // 
            // cboAccessProvider
            // 
            this.cboAccessProvider.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAccessProvider.FormattingEnabled = true;
            this.cboAccessProvider.Location = new System.Drawing.Point(113, 23);
            this.cboAccessProvider.Name = "cboAccessProvider";
            this.cboAccessProvider.Size = new System.Drawing.Size(260, 24);
            this.cboAccessProvider.TabIndex = 4;
            this.cboAccessProvider.SelectedIndexChanged += new System.EventHandler(this.cboAccessProvider_SelectedIndexChanged);
            // 
            // txtKeyName
            // 
            this.txtKeyName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKeyName.Location = new System.Drawing.Point(113, 52);
            this.txtKeyName.Name = "txtKeyName";
            this.txtKeyName.Size = new System.Drawing.Size(260, 23);
            this.txtKeyName.TabIndex = 5;
            // 
            // pwdKeyValue
            // 
            this.pwdKeyValue.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pwdKeyValue.Location = new System.Drawing.Point(114, 81);
            this.pwdKeyValue.Name = "pwdKeyValue";
            this.pwdKeyValue.PasswordChar = '*';
            this.pwdKeyValue.Size = new System.Drawing.Size(260, 23);
            this.pwdKeyValue.TabIndex = 6;
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(66, 130);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add Key";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDeleteKey
            // 
            this.btnDeleteKey.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteKey.Location = new System.Drawing.Point(171, 130);
            this.btnDeleteKey.Name = "btnDeleteKey";
            this.btnDeleteKey.Size = new System.Drawing.Size(100, 30);
            this.btnDeleteKey.TabIndex = 8;
            this.btnDeleteKey.Text = "Delete Key";
            this.btnDeleteKey.UseVisualStyleBackColor = true;
            this.btnDeleteKey.Click += new System.EventHandler(this.btnDeleteKey_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinish.Location = new System.Drawing.Point(277, 130);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(100, 30);
            this.btnFinish.TabIndex = 9;
            this.btnFinish.Text = "Finish";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // AccessKeysForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 169);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.btnDeleteKey);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.pwdKeyValue);
            this.Controls.Add(this.txtKeyName);
            this.Controls.Add(this.cboAccessProvider);
            this.Controls.Add(this.chkShowHidePwd);
            this.Controls.Add(this.lblKeyValue);
            this.Controls.Add(this.lblKeyName);
            this.Controls.Add(this.lblAccessProvider);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AccessKeysForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Access Key Entry";
            this.Load += new System.EventHandler(this.AccessKeysForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAccessProvider;
        private System.Windows.Forms.Label lblKeyName;
        private System.Windows.Forms.Label lblKeyValue;
        private System.Windows.Forms.CheckBox chkShowHidePwd;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDeleteKey;
        private System.Windows.Forms.Button btnFinish;
        public System.Windows.Forms.ComboBox cboAccessProvider;
        public System.Windows.Forms.TextBox txtKeyName;
        public System.Windows.Forms.TextBox pwdKeyValue;
    }
}