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
    public partial class Initializer_MainAccountEditor : UserControl
    {
        public Initializer_MainAccountEditor()
        {
            InitializeComponent();
        }

        private void Initializer_MainAccountEditor_Load(object sender, EventArgs e)
        {
            InitAccounts();
        }

        private void InitAccounts()
        {
            foreach(AccountItem accitem in Initializer.activeInitializer.createdAccounts)
            {
                control_AccountEditor1.AddNewTab(accitem);
            }
        }
    }
}
