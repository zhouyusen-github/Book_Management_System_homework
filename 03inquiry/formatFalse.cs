using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class formatFalse : Form
    {
        public formatFalse()
        {
            InitializeComponent();
            label1.Left = (this.ClientRectangle.Width - label1.Width) / 2;
        }

        private void FormatFalse_Load(object sender, EventArgs e)
        {

        }
    }
}
