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

        public void AddNewTab(AccountItem accountItem)
        {
            string accountName = accountItem.accountName;
            Image accountImage = accountItem.accountImage;
            AccountType accountType = accountItem.accountType;

            // Initialize tab selection button
            Button buttonTab = new Button();

            buttonTab.Click += (e, sender) =>
            {
                OpenTab(buttonTab.Text);
            };

            buttonTab.Click += (e, sender) => { OpenTab(buttonTab.Text); };
            buttonTab.BackgroundImage = WaveZtream.Properties.Resources.waveztream_legacylogo;
            buttonTab.BackgroundImageLayout = ImageLayout.Stretch;
            buttonTab.ForeColor = Color.White;
            buttonTab.Font = new Font("Arial", 8);
            buttonTab.Enabled = true;
            buttonTab.Size = new Size(tabList.Size.Width, buttonTab.Height);
            buttonTab.Text = accountName;
            buttonTab.FlatStyle = FlatStyle.Popup;
            buttonTab.Location = new Point(0, tabs.Count * buttonTab.Height);
            tabList.Controls.Add(buttonTab);

            // Initialize new page
            Panel panelTab = new Panel();

            //add configuration settings to the panel
            //account profile image
            PictureBox accountImageBox = new PictureBox();
            accountImageBox.Image = accountImage;
            accountImageBox.Location = new Point(10, 10);
            accountImageBox.Size = new Size(70, 70);
            accountImageBox.BackColor = Color.FromArgb(15, 15, 15);
            accountImageBox.SizeMode = PictureBoxSizeMode.StretchImage;
            accountImageBox.Cursor = Cursors.Hand;
            //account name box
            TextBox accountNameBox = new TextBox();
            accountNameBox.Text = accountName;
            accountNameBox.Location = new Point(90, 10);
            accountNameBox.Size = new Size(180, 50);
            accountNameBox.BorderStyle = BorderStyle.None;
            accountNameBox.BackColor = Color.FromArgb(15, 15, 15);
            accountNameBox.ForeColor = Color.Lime;
            accountNameBox.Font = new Font("Arial", 18);

            //add the created controls to the content panel, and show them
            panelTab.Controls.Add(accountImageBox);
            panelTab.Controls.Add(accountNameBox);
            accountImageBox.Show();
            accountNameBox.Show();

            //added configuration settings to the panel, continue



            panelTab.Dock = DockStyle.Fill;
            panelTab.BackColor = Color.Transparent;
            panelTab.Visible = false;
            //panelTab.BackColor = GetRandomColor();
            panelTab.BackColor = Color.FromArgb(20, 20, 20);
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

        static Color GetRandomColor()
        {
            Random random = new Random();

            // Generate random RGB values
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);

            // Create and return a Color object
            return Color.FromArgb(red, green, blue);
        }
    }

    public class ButtonTab
    {
        public string tabName;
        public Button tabButton;
        public Panel tabPage;
    }
}
