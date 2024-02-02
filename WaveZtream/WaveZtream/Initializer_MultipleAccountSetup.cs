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
    public partial class Initializer_MultipleAccountSetup : UserControl
    {
        public Initializer_MultipleAccountSetup()
        {
            InitializeComponent();
        }

        private void Initializer_MultipleAccountSetup_Load(object sender, EventArgs e)
        {

        }
    }

    public class AccountItem
    {
        public string accountName { get; set; }
        public Image accountImage { get; set; }
        public AccountType accountType { get; set; }
    }
}

public enum AccountType
{
    Admin,
    User,
    Guest,
    Temp,
    Limited,
}
