﻿using System;
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
    public partial class borrowBookCardFail : Form
    {
        public borrowBookCardFail()
        {
            InitializeComponent();
            label1.Left = (this.ClientRectangle.Width - label1.Width) / 2;
            button1.Left = (this.ClientRectangle.Width - button1.Width) / 2;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BorrowBookCardFail_Load(object sender, EventArgs e)
        {

        }
    }
}
