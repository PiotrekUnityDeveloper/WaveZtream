using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveZtream
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TransparencyKey = Color.Black;
            CheckForFirstTimeInstallation();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void CheckForFirstTimeInstallation()
        {
            string configPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            MessageBox.Show(configPath);

            if (!Directory.Exists(configPath))
            {
                MessageBox.Show("Your system appears to have no appdata folder. Store configurations in source folder? This means everything you configure is lost after uninstallation unless you backup it!");
            }
            else
            {

            }
        }
    }
}
