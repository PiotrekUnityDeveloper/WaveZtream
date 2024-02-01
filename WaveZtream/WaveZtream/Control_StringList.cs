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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StringItem itemToDelete = userAccounts[userAccounts.Count - 1];
            userAccounts.RemoveAt(userAccounts.Count - 1);
            panel1.Controls.Remove(itemToDelete);
            itemToDelete.Dispose();
        }
    }
}
