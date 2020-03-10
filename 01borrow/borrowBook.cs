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
    public partial class borrowBook : Form
    {
        public borrowBook()
        {
            InitializeComponent();
        }

        private void BorrowBook_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //读取借书证号，和书名
            string cardNumber = textBox1.Text;
            string bookName = textBox2.Text;

            //获取当前时间
            DateTime nowTime = DateTime.Now;
            string newTimeString = nowTime.ToString();

            //建立连接
            string connect = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= Database1.accdb";//第二个参数为文件的路径，记得在字符串前加@
            OleDbConnection connection = new OleDbConnection(connect);//oledbconnection主要是应用于access的数据库连接，sqlconnection主要是针对sql server数据库连接的方法。
            connection.Open();

            //查询该借书证号是否存在
            string select1 = "SELECT *FROM cardData WHERE cardNumber = '" + cardNumber + "'";
            OleDbCommand command1 = new OleDbCommand(select1, connection);
            OleDbDataReader reader1 = command1.ExecuteReader();
            if (!reader1.Read()) {         
                textBox1.Text = "";
                Console.Beep(200,300);//发出低沉警告音
                borrowBookCardFail borrowBookCardFail1 = new borrowBookCardFail();
                borrowBookCardFail1.StartPosition = FormStartPosition.CenterParent;//居中
                borrowBookCardFail1.ShowDialog();
                connection.Close();//关闭连接
                return;//如果不存在，程序不再执行
            }

            //每人允许借书数量(根据题意设置为1，如有需要修改此处即可)
            int canBorrow = 1;

            //检查该借书证号是否借满了借书允许数量
            string select2 = "SELECT *FROM borrowData WHERE cardNumber = '" + cardNumber + "'";
            OleDbDataAdapter accessData2 = new OleDbDataAdapter(select2, connection);
            DataSet dataSet = new DataSet();
            accessData2.Fill(dataSet);
            int tableLength = dataSet.Tables[0].Rows.Count;
            if (tableLength == canBorrow)
            {
                haveBorrowOneBook haveBorrowOneBook1 = new haveBorrowOneBook();
                textBox1.Text = "";
                Console.Beep(200, 300);//发出低沉警告音
                haveBorrowOneBook1.StartPosition = FormStartPosition.CenterParent;
                haveBorrowOneBook1.ShowDialog();
                connection.Close();
                return;
            }

            //查询该书是否存在
            string select3 = "SELECT *FROM bookData WHERE bookName = '" + bookName + "'";
            OleDbCommand command3 = new OleDbCommand(select3, connection);
            OleDbDataReader reader3 = command3.ExecuteReader();
            if (!reader3.Read())
            {
                borrowBookNameFail borrowBookNameFail1 = new borrowBookNameFail();
                textBox2.Text = "";
                Console.Beep(200, 300);//发出低沉警告音
                borrowBookNameFail1.StartPosition = FormStartPosition.CenterParent;
                borrowBookNameFail1.ShowDialog();
                connection.Close();
                return;
            }

            //查询该书是否还有库存
                //先确定书本被借出数量
            string select11 = "SELECT *FROM borrowData WHERE bookName = '" + bookName + "'";
            OleDbDataAdapter accessData11 = new OleDbDataAdapter(select11, connection);
            DataSet dataSet11 = new DataSet();
            accessData11.Fill(dataSet11);
            int tableLength11 = dataSet11.Tables[0].Rows.Count;
                //再查找库存数量
            string select12 = "SELECT *FROM bookData WHERE bookName = '" + bookName + "'";
            OleDbDataAdapter accessData12 = new OleDbDataAdapter(select12, connection);
            DataSet dataSet12 = new DataSet();
            accessData12.Fill(dataSet12);
            int bookNumbers = int.Parse(dataSet12.Tables[0].Rows[0]["numbers"].ToString());

            if (tableLength11 == bookNumbers)//如果被借出数量等于库存数量，说明输入错误
            {
                borrowed borrowed1 = new borrowed();
                Console.Beep(200, 300);//发出低沉警告音
                borrowed1.StartPosition = FormStartPosition.CenterParent;
                borrowed1.ShowDialog();
                connection.Close();
                return;
            }

            //获取author名字
            string select5 = "SELECT *FROM bookData WHERE bookName = '" + bookName + "'";
            OleDbDataAdapter accessData5 = new OleDbDataAdapter(select5, connection);
            DataSet dataSet5 = new DataSet();
            accessData5.Fill(dataSet5);
            string author = dataSet5.Tables[0].Rows[0]["author"].ToString();

            //获取借阅者名字,以供存入借阅信息时使用
            string select6 = "SELECT *FROM cardData WHERE cardNumber = '" + cardNumber + "'";
            OleDbDataAdapter accessData6 = new OleDbDataAdapter(select6, connection);
            DataSet dataSet6 = new DataSet();
            accessData6.Fill(dataSet6);
            string borrowerName = dataSet6.Tables[0].Rows[0]["borrowerName"].ToString();

            //在borrowData中存入借书记录
            string insert9 = "INSERT INTO borrowData(cardNumber, borrowerName, bookName, author, borrowDate)VALUES('" + cardNumber + "', '" + borrowerName + "', '" + bookName + "', '" + author + "', '" + newTimeString + "')";//注意逗号，最好是从sql教学网站直接复制语句，这样就不会出错，因为照着打都可能错                                                                                                                                    //INSERT INTO Persons (LastName, Address) VALUES ('Wilson', 'Champs-Elysees')
            OleDbCommand command9 = new OleDbCommand(insert9, connection);
            command9.ExecuteNonQuery();

            //弹出借书成功网页
            borrowSuccess borrowSuccess1 = new borrowSuccess();
            Console.Beep(1200, 300);//发出高昂提示音
            borrowSuccess1.StartPosition = FormStartPosition.CenterParent;
            borrowSuccess1.ShowDialog();

            connection.Close();//关闭连接
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
