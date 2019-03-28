using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ownskit.Utils;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Borman.ZuSharp
{
    public interface ILogicContainer
    {
        void Log(bool isError, string message);
        void Log(bool isError, string message, params object[] args);
    }

    public partial class MainForm : Form, ILogicContainer
    {
        ZuLogic Logic { get; set; }
        Icon m_appIcon, m_appIconDisabled;
        StartWithWindowsUtil m_startWidthWindowsUtil;

        public MainForm(ZuLogic logic)
        {
            InitializeComponent();
            Logic = logic;

            Logic.Container = this;
            Logic.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                HideMe();
                SetVersionTitle();
                uxNotifyIcon.ContextMenuStrip = uxMenuContext;
                FillSettings();
                LoadIcons();
                uxActiveUpdateTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing form: " + ex.Message, "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseMe();
            }
        }

        private void SetVersionTitle()
        {
            Version v =  this.GetType().Assembly.GetName().Version;
            string versionString = String.Format("{0}.{1}", v.Major, v.Minor);
            this.Text += " - " + versionString;
            this.uxNotifyIcon.Text = String.Format("Буква Зю {0} - щёлкните два раза чтобы показать окно", versionString);
        }

        private void ShowMe()
        {
            this.ShowInTaskbar = true;
            this.TopMost = true;
            this.Show();
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }

        private void HideMe()
        {
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private bool iMeanIt = false;
        private void CloseMe()
        {
            iMeanIt = true;
            Close();
        }


        private void LoadIcons()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AppResources));
            
            this.m_appIcon = (System.Drawing.Icon)resources.GetObject("AppIcon");
            this.m_appIconDisabled = (System.Drawing.Icon)resources.GetObject("AppIconDisabled");

            if (this.m_appIcon == null || this.m_appIconDisabled == null)
            {
                throw new Exception("Cannot load icons");
            }
        }

        private void FillSettings()
        {
            ignoreUISettingsChanges = true;

            uxGermanKeyboard.Checked = Logic.Settings.GermanKeyboard;
            uxCheckUseEscape.Checked = Logic.Settings.UseEscape;
            uxCheckUseScrollLock.Checked = Logic.Settings.UseScrollLock;
            uxGermanKeyboard.Checked = Logic.Settings.GermanKeyboard;

            uxComboLayout.Items.Clear();

            foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
            {
                var selectionLanguage = new SelectionLanguage { Language = lang };
                uxComboLayout.Items.Add(selectionLanguage);
                uint low = User32.LoWord(lang.Handle);
                if (low == Logic.Settings.BindToWindowsLayout)
                {
                    uxComboLayout.SelectedItem = selectionLanguage;
                }
            }

            m_startWidthWindowsUtil = new StartWithWindowsUtil("LetterZu", Assembly.GetExecutingAssembly().Location);
            uxStartWithWindows.Checked = m_startWidthWindowsUtil.IsAutoStartEnabled();
            
            ignoreUISettingsChanges = false;
        }

        private void SaveSettings()
        {
            uint layoutId = Logic.Settings.BindToWindowsLayout;

            if (uxComboLayout.SelectedItem != null)
            {
                layoutId = User32.LoWord((uxComboLayout.SelectedItem as SelectionLanguage).Language.Handle);
            }

            string xml = String.Format(
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
  <appSettings>
      <add key=""LayoutFile"" value=""{0}""/>
      <add key=""BindToWindowsLayout"" value=""{1}""/>
      <add key=""UseScrollLock"" value=""{2}""/>
      <add key=""UseEscape"" value=""{3}""/>
      <add key=""GermanKeyboard"" value=""{4}""/>
  </appSettings>
</configuration>
",
                Logic.Settings.LayoutFile,
                layoutId,
                uxCheckUseScrollLock.Checked,
                uxCheckUseEscape.Checked,
                uxGermanKeyboard.Checked
                );

            string standartConfigFile = Application.ExecutablePath + ".config";
            string debugConfigFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "LetterZu.vshost.exe.config");

            File.WriteAllText(standartConfigFile, xml);

            if (File.Exists(debugConfigFile))
            {
                File.WriteAllText(debugConfigFile, xml);
            }
        }

        private string GetSettingsFilePath()
        {
            return Application.ExecutablePath + ".config";
        }

        public void Log(bool isError, string message)
        {
            //uxLog.Text = String.Format("{0}\r\n{1}", message, uxLog.Text);
            if (isError)
            {
                MessageBox.Show(message, "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Log(bool isError, string message, params object[] args)
        {
            string str = String.Format(message, args);
            Log(isError, str);
        }

        private void uxActiveUpdateTimer_Tick(object sender, EventArgs e)
        {
            RefreshActiveState();
        }

        private void RefreshActiveState()
        {
            bool active = Logic.IsActive;

            turnOnToolStripMenuItem.Checked = active;

            Icon icon = active ? m_appIcon : m_appIconDisabled;
            if (this.Icon != icon)
            {
                this.Icon = icon;
            }

            uxNotifyIcon.Visible = true;

            if (uxNotifyIcon.Icon != icon)
            {
                uxNotifyIcon.Icon = icon;                
            }

            uxToggleEnable.Checked = active;
        }

        private void uxBtnEnable_Click(object sender, EventArgs e)
        {
            Logic.ToggleActivate();
        }

        private class SelectionLanguage
        {
            public InputLanguage Language { get; set; }
            public override string ToString()
            {
                return Language.Culture.DisplayName;
            }
        }

        private bool ignoreUISettingsChanges = false;

        private void uxCheckUseScrollLock_CheckedChanged(object sender, EventArgs e)
        {
            if (ignoreUISettingsChanges)
            {
                return;
            }

            ignoreUISettingsChanges = true;
            
            SaveSettings();
            Logic.Restart();

            ignoreUISettingsChanges = false;
        }

        private void uxCheckUseEscape_CheckedChanged(object sender, EventArgs e)
        {
            if (ignoreUISettingsChanges)
            {
                return;
            }

            ignoreUISettingsChanges = true;

            SaveSettings();
            Logic.Restart();

            ignoreUISettingsChanges = false;
        }

        private void uxComboLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreUISettingsChanges)
            {
                return;
            }

            ignoreUISettingsChanges = true;
            
            SaveSettings();
            Logic.Restart();

            ignoreUISettingsChanges = false;
        }

        private void uxNotifyIconActive_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowMe();
            WindowState = FormWindowState.Normal;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (iMeanIt)
            {
                return;
            }

            e.Cancel = true;
            HideMe();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMe();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseMe();
        }

        private void uxToggleEnable_Click(object sender, EventArgs e)
        {
            Logic.ToggleActivate();
        }

        private void uxRebindCheckTimer_Tick(object sender, EventArgs e)
        {
            int RESTART_PERIOD_SECS = 60;

            if (!Logic.IsActive)
            {
                return;
            }

            var secondsFromLastKeyDown = (DateTime.Now - Logic.LastKeyDownOrRestart).TotalSeconds;
            if (secondsFromLastKeyDown > RESTART_PERIOD_SECS)
            {
                Logic.LastKeyDownOrRestart = DateTime.Now;
                Logic.Restart();
            }
        }

        private void uxStartWithWindows_Changed(object sender, EventArgs e)
        {
            if (ignoreUISettingsChanges)
            {
                return;
            }

            if (uxStartWithWindows.Checked)
            {
                m_startWidthWindowsUtil.SetAutoStart();
            }
            else
            {
                m_startWidthWindowsUtil.UnSetAutoStart();
            }
        }

        private void turnOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logic.ToggleActivate();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CloseMe();
        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string readMeFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\readme.rtf";
            System.Diagnostics.Process.Start(readMeFile);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=W665NTWPRV6K2");  
            Process.Start(sInfo);            
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutDlg = new About();
            aboutDlg.ShowDialog(this);
        }

        private void uxGermanKeyboard_CheckedChanged(object sender, EventArgs e)
        {
            if (ignoreUISettingsChanges)
            {
                return;
            }

            ignoreUISettingsChanges = true;

            SaveSettings();
            Logic.Restart();

            ignoreUISettingsChanges = false;
        }

    }
}
