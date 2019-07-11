using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          AccessKeysForm
///   Description:    This class allows the user to enter access
///                   crededntials necessary to access weather data
///                   from specific weather data providers.
///   Author:         Paul O. Patterson     Date: May 13, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// Displays a Form which allows the users to enter access credentials.
    /// </summary>
    public partial class AccessKeysForm : Form
    {
        private const string TAG = "AccessKeysForm";
        public static AccessKeysForm frmKeys = null;
        public string m_key_provider = null;

        public AccessKeysForm()
        {
            InitializeComponent();
            frmKeys = this;
        }

        private void chkShowHidePwd_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowHidePwd.Checked)
            {
                pwdKeyValue.PasswordChar = '\0';
            }// end of if block
            else
            {
                pwdKeyValue.PasswordChar = '*';
            }// end of else block
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtKeyName == null || txtKeyName.Text.Length == 0
                    || txtKeyName.Text.Equals(""))
            {
                UtilityMethod.ShowMessage("Please enter a valid key name as given by the provider!", this,
                    WeatherLionMain.PROGRAM_NAME + " - No Key Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtKeyName.Focus();
            }// end of if block
            else if (pwdKeyValue == null || pwdKeyValue.Text.Length == 0
                    || pwdKeyValue.Text.Equals(""))
            {
                UtilityMethod.ShowMessage("Please enter a valid key value as given by the provider!", this,
                   WeatherLionMain.PROGRAM_NAME + " - No Key Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pwdKeyValue.Focus();
            }// end of else if block
            else
            {
                string[] encryptedKey = LionSecurityManager.Encrypt(pwdKeyValue.Text, true).Split(':');
                string selectedProvider = cboAccessProvider.SelectedItem.ToString();

                switch (selectedProvider)
                {
                    case "Here Maps Weather":
                        if (!LionSecurityManager.hereMapsRequiredKeys.Contains(txtKeyName.Text.ToLower()))
                        {
                            UtilityMethod.ShowMessage($"The {selectedProvider} does not require a key \"{ txtKeyName.Text}\"!",
                                this, $"{WeatherLionMain.PROGRAM_NAME} - Invalid Key Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtKeyName.Focus();

                            return;
                        }// end of if block 

                        break;
                    case "Yahoo! Weather":
                        if (!LionSecurityManager.yahooRequiredKeys.Contains(txtKeyName.Text.ToLower()))
                        {
                            UtilityMethod.ShowMessage($"The {selectedProvider} does not require a key \"{ txtKeyName.Text}\"!",
                                this, $"{WeatherLionMain.PROGRAM_NAME} - Invalid Key Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtKeyName.Focus();

                            return;
                        }// end of if block

                        break;
                    default:
                        break;
                }// end of switch block

                if (UtilityMethod.AddSiteKeyToDatabase(cboAccessProvider.SelectedItem.ToString(),
                        txtKeyName.Text, encryptedKey[0], encryptedKey[1]) == 1)
                {
                    if (txtKeyName.Enabled) txtKeyName.Text = ("");
                    pwdKeyValue.Text = "";

                    UtilityMethod.ShowMessage("The key was successfully added to the database.", this,
                            WeatherLionMain.PROGRAM_NAME + " - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtKeyName.Focus();
                }// end of if block
                else
                {
                    UtilityMethod.ShowMessage("The key could not be added to the database!"
                        + "\nPlease recheck the key and try again.", this,
                            WeatherLionMain.PROGRAM_NAME + " - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtKeyName.Focus();
                }// end of else block
            }// end of else block
        }// end of method btnAdd_Click

        private void btnDeleteKey_Click(object sender, EventArgs e)
        {
            if (txtKeyName == null || txtKeyName.Text.Length == 0
                    || txtKeyName.Text.Equals(""))
            {
                UtilityMethod.ShowMessage("Please enter a valid key name as given by the provider!", this,
                    WeatherLionMain.PROGRAM_NAME + " - No Key Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtKeyName.Focus();
            }// end of if block
            else
            {
                // confirm that user really wishes to delete the key
                string prompt = $"Are you sure that you wish to delete the " +
                                $"{txtKeyName.Text}\nkey assinged by {cboAccessProvider.SelectedItem}?\n" +
                                "This cannot be undone!";

                DialogResult result = MessageBox.Show(prompt, $"{WeatherLionMain.PROGRAM_NAME} + Delete Key", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);
                
                if (result == DialogResult.Yes)
                {
                    if (UtilityMethod.DeleteSiteKeyFromDatabase(cboAccessProvider.SelectedItem.ToString(),
                            txtKeyName.Text) == 1)
                    {
                        UtilityMethod.ShowMessage($"The {cboAccessProvider.SelectedItem} {txtKeyName.Text} has " +
                            $"been removed from the database.", this, WeatherLionMain.PROGRAM_NAME + " - Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtKeyName.Focus();
                    }// end of if block
                    else
                    {
                        UtilityMethod.ShowMessage($"An error occured while removing the {cboAccessProvider.SelectedItem} {txtKeyName.Text}" +
                           $"from the database!.\nPlease check the Key Provider and Key Name specified and try again.", 
                           this, WeatherLionMain.PROGRAM_NAME + " - Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        txtKeyName.Focus();
                    }// end of else block

                }// end of if block
                else
                {
                    UtilityMethod.ShowMessage("Key deletion aborted!",
                          this, WeatherLionMain.PROGRAM_NAME + " - Deletion Aborted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtKeyName.Focus();                    
                }// end of else block
            }// end of else block
        }// end of method btnDeleteKey_Click

        private void btnFinish_Click(object sender, EventArgs e)
        {
            LionSecurityManager.LoadAccessProviders();

            if (LionSecurityManager.keysMissing != null)
            {
                if (LionSecurityManager.keysMissing.Count > 0)
                {
                    DialogResult answer = LionSecurityManager.CheckForMissingKeys();
                    LionSecurityManager.accessGranted = false;

                    if (answer ==DialogResult.Yes)
                    {
                        if (!Visible)
                        {
                            Visible = true;
                        }// end of if block
                        else
                        {
                            txtKeyName.Focus();
                        }// end of else block
                    }// end of if block
                    else
                    {
                        string wp = null;
                        //string[] keys = null;

                        if (LionSecurityManager.keysMissing.Count > 1)
                        {
                            List<string> missingHereKeys = new List<string>();
                            List<string> missingYahooKeys = new List<string>();

                            foreach (string keyName in LionSecurityManager.keysMissing)
                            {
                                if (LionSecurityManager.hereMapsRequiredKeys.Contains(keyName))
                                {
                                    missingHereKeys.Add(keyName);
                                }// end of if block
                                else if (LionSecurityManager.hereMapsRequiredKeys.Contains(keyName))
                                {
                                    missingYahooKeys.Add(keyName);
                                }// end of else if block
                            }// end of for each loop

                            if (missingHereKeys.Count > 0)
                            {
                                string hKeys = string.Join(", ", missingHereKeys);
                                string fs = null;

                                if (UtilityMethod.NumberOfCharacterOccurences(',', hKeys) > 1)
                                {
                                    fs = UtilityMethod.ReplaceLast(",", ", and", hKeys);
                                }// end of if block
                                else if (UtilityMethod.NumberOfCharacterOccurences(',', hKeys) == 1)
                                {
                                    fs = hKeys.Replace(",", " and");
                                }// end of else block
                                else
                                {
                                    fs = hKeys;
                                }// end of else block

                                wp = WeatherLionMain.HERE_MAPS;
                                string kc = missingHereKeys.Count > 1 ? "keys" : "key";

                                UtilityMethod.ShowMessage($"{wp} cannot be used as a weather source without\n"
                                        + $"first adding the missing {kc} {fs}.", this, $"{WeatherLionMain.PROGRAM_NAME} - Missing Key", MessageBoxButtons.OK ,MessageBoxIcon.Error);

                            }// end of if block

                            if (missingYahooKeys.Count > 0)
                            {
                                string hKeys = string.Join(", ", missingHereKeys);
                                string fs = null;

                                if (UtilityMethod.NumberOfCharacterOccurences(',', hKeys) > 1)
                                {
                                    fs = UtilityMethod.ReplaceLast(",", ", and", hKeys);
                                }// end of if block
                                else if (UtilityMethod.NumberOfCharacterOccurences(',', hKeys) == 1)
                                {
                                    fs = hKeys.Replace(",", " and");
                                }// end of else block
                                else
                                {
                                    fs = hKeys;
                                }// end of else block

                                wp = WeatherLionMain.YAHOO_WEATHER;
                                string kc = missingHereKeys.Count > 1 ? "keys" : "key";

                                UtilityMethod.ShowMessage($"{wp} cannot be used as a weather source without\n"
                                       + $"first adding the missing {kc} {fs}.", this,
                                       title: $"{WeatherLionMain.PROGRAM_NAME} - Missing Key", buttons: MessageBoxButtons.OK,
                                       mbIcon: MessageBoxIcon.Error);                               

                            }// end of if block
                        }// end of if block
                    }// end of else block
                }// end of if block
            }// end of if block

            if (LionSecurityManager.webAccessGranted.Count >= 1 &&
                !LionSecurityManager.webAccessGranted.Contains("GeoNames"))
            {
                UtilityMethod.ShowMessage("This program requires a geonames username"
                        + " which was not stored in the database.\nIT IS FREE!", this,
                            title: $"{WeatherLionMain.PROGRAM_NAME} - Missing Key", buttons: MessageBoxButtons.OK,
                                mbIcon: MessageBoxIcon.Error);               
            }// end of else if block
            else if (LionSecurityManager.webAccessGranted.Count == 2 &&
                LionSecurityManager.webAccessGranted.Contains("GeoNames") &&
                    LionSecurityManager.webAccessGranted.Contains("Yr.no (Norwegian Metrological Institute)"))
            {
               UtilityMethod.ShowMessage("The program will only display weather data from"
                   + " Yr.no (Norwegian Metrological Institute).\nObtain access keys for"
                   + " the others if you wish to use them.", this,
                    title: $"{WeatherLionMain.PROGRAM_NAME} - Single Weather Providery", buttons: MessageBoxButtons.OK,
                            mbIcon: MessageBoxIcon.Information);
                LionSecurityManager.accessGranted = true;
                Close();
            }// end of else if block
            else
            {
                LionSecurityManager.accessGranted = true;
                Close();
            }// end of else block
        }// end of method btnFinish_Click

        private void SelectedProvider()
        {
            string selectedProvider = cboAccessProvider.SelectedItem.ToString();

            if (selectedProvider.Equals("GeoNames"))
            {
                txtKeyName.Enabled = false;
                txtKeyName.Text = "username";
                pwdKeyValue.Focus();
            }// end of if block
            else if (selectedProvider.Equals("Dark Sky Weather") ||
                     selectedProvider.Equals("Open Weather Map") ||
                     selectedProvider.Equals("Weather Bit") ||
                     selectedProvider.Equals("Weather Underground"))
            {
                txtKeyName.Enabled = false;
                txtKeyName.Text = "api_key";
                pwdKeyValue.Focus();
            }// end of if block	
            else
            {
                txtKeyName.Enabled = true;
                txtKeyName.Text = "";
                txtKeyName.Focus();
            }// end of else block
        }// end of method SelectedProvider

        private void cboAccessProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedProvider();
        }// end of method cboAccessProvider_SelectedIndexChanged

        private void AccessKeysForm_Load(object sender, EventArgs e)
        {
            // Load only the providers who require access keys
            List<string> wxOnly =
                    new List<string>(WeatherLionMain.providerNames.ToArray<string>());

            wxOnly.Sort();   // sort the list

            // GeoNames is not a weather provider so it cannot be select here
            if (wxOnly.Contains("Yr.no (Norwegian Metrological Institute)"))
                wxOnly.Remove("Yr.no (Norwegian Metrological Institute)");

            foreach (string provider in wxOnly)
            {
                cboAccessProvider.Items.Add(provider);
            }// end of for each loop

            if (m_key_provider != null)
            {
                cboAccessProvider.SelectedItem = m_key_provider;
                pwdKeyValue.Focus();
            }// end of if block
        }
    }
}// end of namespace WeatherLion
