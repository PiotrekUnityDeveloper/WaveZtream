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
            buttonTab.BackgroundImage = WaveZtream.Properties.Resources.buttonBackground_Default;
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

            accountNameBox.TextChanged += (e, sender) =>
            {
                buttonTab.Text = accountNameBox.Text;
                GetTabByPage(panelTab).tabName = buttonTab.Text;
            };

            //account type combobox
            ComboBox accountTypeComboBox = new ComboBox();
            accountTypeComboBox.Items.Clear();
            accountTypeComboBox.Items.Add("Admin");
            accountTypeComboBox.Items.Add("User");
            accountTypeComboBox.Items.Add("Guest");
            accountTypeComboBox.Items.Add("Temp");
            accountTypeComboBox.Items.Add("Limited");
            accountTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            accountTypeComboBox.SelectedItem = accountType.ToString();
            accountTypeComboBox.FlatStyle = FlatStyle.Popup;
            accountTypeComboBox.ForeColor = Color.White;
            accountTypeComboBox.BackColor = Color.FromArgb(15, 15, 15);
            accountTypeComboBox.Location = new Point(86, 44);
            accountTypeComboBox.Size = new Size(60, accountTypeComboBox.Size.Height);
            Label accountTypeLabel = new Label();
            accountTypeLabel.Text = accountType.ToString();
            accountTypeLabel.Font = new Font("Arial", 10);
            accountTypeLabel.ForeColor = Color.White;
            accountTypeLabel.Location = new Point(90, 42);
            accountTypeLabel.Size = new Size(70, accountTypeLabel.Size.Height);

            accountTypeComboBox.SelectedIndexChanged += (e, sender) =>
            {
                accountTypeLabel.Text = accountTypeComboBox.SelectedItem.ToString();
                accountTypeLabel.Show();
                accountTypeComboBox.Hide();
            };
            accountTypeComboBox.MouseLeave += (e, sender) =>
            {
                accountTypeLabel.Show();
                accountTypeComboBox.Hide();
            };
            accountTypeLabel.MouseEnter += (e, sender) =>
            {
                accountTypeLabel.Hide();
                accountTypeComboBox.Show();
            };

            //add the created controls to the content panel, and show them
            panelTab.Controls.Add(accountImageBox); // 1
            panelTab.Controls.Add(accountNameBox); // 2
            panelTab.Controls.Add(accountTypeComboBox); // 3
            panelTab.Controls.Add(accountTypeLabel); // 4
            accountImageBox.Show();
            accountNameBox.Show();
            accountTypeLabel.Show();
            //accountTypeComboBox.Show(); not shown by default
            accountTypeComboBox.Hide();

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

        private ButtonTab GetTabByPage(Panel tabPage)
        {
            ButtonTab btntab = null;
            foreach(ButtonTab tab in tabs)
            {
                if(tab.tabPage == tabPage)
                {
                    btntab = tab;
                    break;
                }
            }

            return btntab;
        }
    }

    public class ButtonTab
    {
        public string tabName;
        public Button tabButton;
        public Panel tabPage;
    }
}
