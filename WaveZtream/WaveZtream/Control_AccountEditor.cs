using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveZtream
{
    public partial class Control_AccountEditor : UserControl
    {
        public Control_AccountEditor()
        {
            InitializeComponent();
        }

        private List<ButtonTab> tabs = new List<ButtonTab>();
        private int selectedTab = 0;

        public void AddNewTab(string accountName)
        {
            // Initialize tab selection button
            Button buttonTab = new Button();
            buttonTab.Click += (e, sender) => { OpenTab(buttonTab.Text); };
            buttonTab.BackgroundImage = WaveZtream.Properties.Resources.waveztream_legacylogo;
            buttonTab.BackgroundImageLayout = ImageLayout.Stretch;
            buttonTab.ForeColor = Color.White;
            buttonTab.Font = new Font("Arial", 10);
            buttonTab.Enabled = true;
            buttonTab.Size = new Size(tabList.Size.Width, buttonTab.Height);
            buttonTab.Text = accountName;
            buttonTab.Location = new Point(0, tabs.Count * buttonTab.Height);
            tabList.Controls.Add(buttonTab);

            // Initialize new page
            Panel panelTab = new Panel();
            panelTab.Dock = DockStyle.Fill;
            panelTab.BackColor = Color.Transparent;
            panelTab.Visible = false;
            contentList.Controls.Add(panelTab);

            tabs.Add(new ButtonTab { tabButton = buttonTab, tabPage = panelTab, tabName = accountName });
            buttonTab.Show();

            if (selectedTab == 0) // no tab selected
            {
                OpenTab(accountName);
            }
        }

        private void OpenTab(string tabName)
        {
            foreach (ButtonTab btntab in tabs)
            {
                if (btntab.tabName == tabName)
                {
                    btntab.tabPage.Show();
                    btntab.tabButton.ForeColor = Color.Lime;
                }
                else
                {
                    btntab.tabPage.Hide();
                    btntab.tabButton.ForeColor = Color.White;
                }
            }
        }
    }

    public class ButtonTab
    {
        public string tabName;
        public Button tabButton;
        public Panel tabPage;
    }
}
