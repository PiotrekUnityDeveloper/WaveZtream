using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveZtream
{
    public partial class Control_StringList : UserControl
    {
        public Control_StringList()
        {
            InitializeComponent();
        }

        public List<StringItem> userAccounts = new List<StringItem>();

        private void button1_Click(object sender, EventArgs e)
        {
            StringItem item = new StringItem();
            item.Show();
            panel1.Controls.Add(item);
            item.Location = new Point(0, userAccounts.Count * 56);
            userAccounts.Add(item);

            CheckForOverflow();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(userAccounts.Count == 0)
                return;

            StringItem itemToDelete = userAccounts[userAccounts.Count - 1];
            userAccounts.RemoveAt(userAccounts.Count - 1);
            panel1.Controls.Remove(itemToDelete);
            itemToDelete.Dispose();

            CheckForOverflow();
        }

        private void CheckForOverflow()
        {
            if (userAccounts.Count * 56 > panel1.Height)
            {
                panel1.Height = userAccounts.Count * 56;
            }
            else if(userAccounts.Count * 56 < panel1.Height && panel1.Height > 262)
            {
                panel1.Height = userAccounts.Count * 56;
            }
            else if(panel1.Height <= 262)
            {
                vScrollBar1.Enabled = false;
                vScrollBar1.Visible = false;
            }

            if(panel1.Height > 262)
            {
                vScrollBar1.Visible = true;
                vScrollBar1.Enabled = true;

                vScrollBar1.Maximum = ((panel1.Height * userAccounts.Count) - this.Height) / 15;
                vScrollBar1.Minimum = 0;
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            panel1.Location = new Point(panel1.Location.X, 3 - vScrollBar1.Value);
        }

        private void Control_StringList_Load(object sender, EventArgs e)
        {

        }

        public List<AccountItem> GetCreatedAccounts()
        {
            List<AccountItem> accounts = new List<AccountItem>();

            foreach(StringItem item in userAccounts)
            {
                accounts.Add(new AccountItem { accountName = item.GetAccountItem().accountName, accountImage = item.GetAccountItem().accountImage, accountType = item.GetAccountItem().accountType });
            }

            return accounts;
        }
    }
}
