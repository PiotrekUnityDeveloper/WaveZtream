using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WaveZtream
{
    public partial class StringItem : UserControl
    {
        public StringItem()
        {
            InitializeComponent();
        }

        public string[] randomAccountNames =
        {
            "ZapperX",
            "Anonym00n2000",
            "JuiceTheKidd",
            "BlissXz",
            "LeapLeafX",
            "wITEqq",
            "wITEqqX",
            "RegularUserX",
            "WAIDWMFL5555",
            "DefaultUserName6749",
            "XXElectronicX",
            "ElectronicX",
            "TripleFX",
            "BoobieLoverX",
            "AwakenedXfux",
            "DrillzX",
            "EpicBuzzcutX",
        };

        private Random rnd;

        private void StringItem_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "User";
            rnd = new Random();
            textBox1.Text = randomAccountNames[rnd.Next(0, randomAccountNames.Length - 1)].Replace("X", rnd.Next(000000, 999999).ToString());
        }
    }
}
