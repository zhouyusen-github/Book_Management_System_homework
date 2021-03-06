﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//作者：周宇森

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //第一个窗体居中
            this.StartPosition = FormStartPosition.CenterScreen;
            //label锁定居中
            label1.Left = (this.ClientRectangle.Width - label1.Width) / 2;
            //label1.BringToFront();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            borrowBook borrowBook1 = new borrowBook();
            //是子窗体在父窗体中间显示
            borrowBook1.StartPosition = FormStartPosition.CenterParent;
            borrowBook1.ShowDialog();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            returnBook returnBook1 = new returnBook();
            returnBook1.StartPosition = FormStartPosition.CenterParent;
            returnBook1.ShowDialog();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            inquiry inquiry1 = new inquiry();
            inquiry1.StartPosition = FormStartPosition.CenterParent;
            inquiry1.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //设置背景图片
            this.BackgroundImage = Image.FromFile("Resources/library.jpeg");//资源在Resources文件夹中
        }


        private void Button2_Click(object sender, EventArgs e)
        {
            introduction introduction1 = new introduction();
            introduction1.StartPosition = FormStartPosition.CenterParent;
            introduction1.ShowDialog();
        }
    }
}
