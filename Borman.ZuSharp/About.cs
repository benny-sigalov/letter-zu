using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Borman.ZuSharp
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://www.fileformat.info/info/unicode/char/30ba/index.htm");  
            Process.Start(sInfo);                        
        }

        private void About_Load(object sender, EventArgs e)
        {
            SetVersionTitle();
        }

        private void SetVersionTitle()
        {
            Version v = this.GetType().Assembly.GetName().Version;
            string versionString = String.Format("{0}.{1}", v.Major, v.Minor);
            uxTitleLabel.Text += " - " + versionString;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://www.letterzu.com");
            Process.Start(sInfo);     
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("mailto:support@letterzu.com");
            Process.Start(sInfo);     
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
