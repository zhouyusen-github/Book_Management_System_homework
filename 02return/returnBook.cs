using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;

namespace WindowsFormsApp1
{
    public partial class returnBook : Form
    {
        public returnBook()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //读取借书证号，和书名
            string cardNumber = textBox1.Text;
            string bookName = textBox2.Text;

            //建立连接
            string connect = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= Database1.accdb";
            OleDbConnection connection = new OleDbConnection(connect);
            connection.Open();

            //查询该借书证号是否存在
            string select1 = "SELECT *FROM cardData WHERE cardNumber = '" + cardNumber + "'";
            OleDbCommand command1 = new OleDbCommand(select1, connection);
            OleDbDataReader reader1 = command1.ExecuteReader();
            if (!reader1.Read())
            {
                textBox1.Text = "";
                Console.Beep(200, 300);//发出低沉警告音
                borrowBookCardFail borrowBookCardFail1 = new borrowBookCardFail();
                borrowBookCardFail1.StartPosition = FormStartPosition.CenterParent;//居中
                borrowBookCardFail1.ShowDialog();
                connection.Close();//关闭连接
                return;//如果不存在，程序不再执行
            }

            //查询是否存在该书
            string select2 = "SELECT *FROM bookData WHERE bookName = '" + bookName + "'";
            OleDbCommand command2 = new OleDbCommand(select2, connection);
            OleDbDataReader reader2 = command2.ExecuteReader();
            if (!reader2.Read()){
                borrowBookNameFail borrowBookNameFail1 = new borrowBookNameFail();
                textBox1.Text = "";
                Console.Beep(200, 300);//发出低沉警告音
                borrowBookNameFail1.StartPosition = FormStartPosition.CenterParent;//居中
                borrowBookNameFail1.ShowDialog();
                connection.Close();
                return;
            }

            //查询该书是否被该借书证号借出
            string select11 = "SELECT *FROM borrowData WHERE bookName = '" + bookName + "'AND cardNumber = '" + cardNumber + "'";
            OleDbDataAdapter accessData11 = new OleDbDataAdapter(select11, connection);
            DataSet dataSet11 = new DataSet();
            accessData11.Fill(dataSet11);
            int tableLength11 = dataSet11.Tables[0].Rows.Count;

            if (tableLength11 == 0) { //未被借出报错
                notBorrow notBorrow1 = new notBorrow();
                Console.Beep(200, 300);//发出低沉警告音
                notBorrow1.StartPosition = FormStartPosition.CenterParent;//居中
                notBorrow1.ShowDialog();
                textBox2.Text = "";
                connection.Close();
                return;
            }

            //该书若被该借书证号借出，则删除borrowData中对该书被该借书证号借出的记录
            string delete4 = "DELETE FROM borrowData WHERE bookName = '" + bookName + "' AND cardNumber = '" + cardNumber + "'";
            OleDbCommand command4 = new OleDbCommand(delete4, connection);
            command4.ExecuteNonQuery();

            //显示借书成功
            returnBookSuccess returnBookSuccess1 = new returnBookSuccess();
            Console.Beep(1200, 300);//发出高昂提示音
            returnBookSuccess1.StartPosition = FormStartPosition.CenterParent;//居中
            returnBookSuccess1.ShowDialog();

            connection.Close();//关闭连接

        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
