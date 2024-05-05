using System;
using System.Windows.Forms;

namespace QModReloadedGUI
{
    public partial class FrmIntro : Form
    {
        public FrmIntro()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
