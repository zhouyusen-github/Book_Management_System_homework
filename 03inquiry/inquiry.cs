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
using System.Collections;

namespace WindowsFormsApp1
{
    public partial class inquiry : Form
    {
        public inquiry()
        {
            InitializeComponent();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //清空左侧文本框
            string[] empty = { "" };
            richTextBox1.Lines = empty;

            //通过作者姓名查询借书人
            if (comboBox1.Text.Equals("借书人"))
            {
                //读取文本框信息
                string author = textBox1.Text;

                //建立连接
                string connect = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database1.accdb";//第二个参数为文件的路径，记得在字符串前加@
                OleDbConnection connection = new OleDbConnection(connect);
                connection.Open();

                //获取该作者的所有书
                string select1 = "SELECT *FROM bookData WHERE author = '" + author + "'";
                OleDbDataAdapter accessData1 = new OleDbDataAdapter(select1, connection);
                DataSet dataSet1 = new DataSet();
                accessData1.Fill(dataSet1);
                int tableLength = dataSet1.Tables[0].Rows.Count;

                //如果没有该作者的书
                if (tableLength == 0)
                {
                    wrongAuthor w1 = new wrongAuthor();
                    Console.Beep(200, 300);
                    connection.Close();
                    w1.StartPosition = FormStartPosition.CenterParent;//居中
                    w1.ShowDialog();
                    textBox1.Text = "";
                    
                    return;//结束程序
                }

                //展示该作者的书的具体情况
                string[] authorBooks = new string[tableLength];//书名
                int[] authorBooksNumbers = new int[tableLength];//该书馆藏量
                ArrayList showList = new ArrayList();//预备显示内容
                for (int i = 0; i < tableLength; i++)
                {                  
                    authorBooks[i] =  dataSet1.Tables[0].Rows[i]["bookName"].ToString();                   
                    authorBooksNumbers[i] = int.Parse(dataSet1.Tables[0].Rows[i]["numbers"].ToString());

                    //在馆藏图书表里找到书本名字，再去借书记录里找
                    string select11 = "SELECT *FROM borrowData WHERE bookName = '" + authorBooks[i] + "'";
                    OleDbDataAdapter accessData11 = new OleDbDataAdapter(select11, connection);
                    DataSet dataSet11 = new DataSet();
                    accessData11.Fill(dataSet11);
                    int tableLength11 = dataSet11.Tables[0].Rows.Count;

                    showList.Add("书名:" + authorBooks[i] + " " + "馆藏" + authorBooksNumbers[i] + "本");
                    if (tableLength11 > 0) {
                        showList.Add("借出该书" + tableLength11 + "本，借阅者如下：");
                        for (int k = 0; k < tableLength11; k++)
                        {
                            showList.Add("借书证号:" + dataSet11.Tables[0].Rows[k]["cardNumber"].ToString() + " " + "借阅人:" + dataSet11.Tables[0].Rows[k]["borrowerName"].ToString());
                        }
                        showList.Add("馆中仍有该书" + (authorBooksNumbers[i] - tableLength11) + "本");
                        
                    } else {
                        showList.Add("该书全部未借出");
                    }
                    
                    showList.Add("");
                }

                //将内容展示至左侧文本框
                int showListLength = showList.Count;
                string[] show = new string[showListLength];
                for (int i = 0; i < showListLength; i++) {
                    show[i] = showList[i].ToString();
                }
                richTextBox1.Lines = show;
                Console.Beep(1200, 300);
                connection.Close();
            }

            //通过借书证号查询借书情况
            if (comboBox1.Text.Equals("借书情况"))
            {
                //读取信息
                string cardNumber = textBox1.Text;

                //建立连接
                string connect = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database1.accdb";//第二个参数为文件的路径，记得在字符串前加@
                OleDbConnection connection = new OleDbConnection(connect);
                connection.Open();

                //查询该借书证号是否存在
                string select1 = "SELECT *FROM cardData WHERE cardNumber = '" + cardNumber + "'";
                OleDbCommand command1 = new OleDbCommand(select1, connection);
                OleDbDataReader reader1 = command1.ExecuteReader();
                if (!reader1.Read())
                {
                    textBox1.Text = "";
                    Console.Beep(200, 300);
                    connection.Close();
                    borrowBookCardFail b1 = new borrowBookCardFail();
                    b1.StartPosition = FormStartPosition.CenterParent;//居中
                    b1.ShowDialog();
                    return;
                }

                //读取该借书证借书信息
                string select2 = "SELECT *FROM borrowData WHERE cardNumber = '" + cardNumber + "'";
                OleDbDataAdapter accessData2 = new OleDbDataAdapter(select2, connection);
                DataSet dataSet = new DataSet();
                accessData2.Fill(dataSet);

                //展示该借书证借书情况至左侧文本框
                int tableLength = dataSet.Tables[0].Rows.Count;
                if (tableLength == 0)
                {
                    string[] noBorrow = { "该卡未借书" };
                    richTextBox1.Lines = noBorrow;
                }
                else
                {
                    string[] borrowBook = new string[tableLength];
                    for (int i = 0; i < tableLength; i++)
                    {
                        borrowBook[i] = "书名:" + dataSet.Tables[0].Rows[i]["bookName"].ToString() + " 借书日期：" + Convert.ToDateTime(dataSet.Tables[0].Rows[i]["borrowDate"].ToString()).ToLongDateString().ToString();
                    }
                    richTextBox1.Lines = borrowBook;
                }
                Console.Beep(1200, 300);
                connection.Close();
            }

            //通过日期查询过期者
            if (comboBox1.Text.Equals("过期者"))
            {
                //读取文本框信息
                string input = textBox1.Text;

                //获取现在时间
                DateTime dateNow;
                if (input == "")
                {
                    dateNow = DateTime.Now;
                }
                else
                {
                    try
                    {
                        dateNow = Convert.ToDateTime(input + " " + "23:59:59");
                    //为了更符合实际情况，最后还书日当天书就过期了
                    }

                    //输入格式错误，则报错
                    catch (System.FormatException)
                    {
                        Console.Beep(200, 300);
                        formatFalse formatFalse1 = new formatFalse();
                        formatFalse1.StartPosition = FormStartPosition.CenterParent;
                        formatFalse1.ShowDialog();
                        
                        return;
                    }                  
                }

                //建立连接
                string connect = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database1.accdb";//第二个参数为文件的路径，记得在字符串前加@
                OleDbConnection connection = new OleDbConnection(connect);
                connection.Open();

                //获取所有借书记录
                string select1 = "SELECT *FROM borrowData";
                OleDbDataAdapter accessData = new OleDbDataAdapter(select1, connection);
                DataSet dataSet = new DataSet();
                accessData.Fill(dataSet);

                //将所有过期记录全部在右侧文本框显示出来
                ArrayList outDateRecords = new ArrayList();
                int tableLength = dataSet.Tables[0].Rows.Count;

                DateTime outBorrowDate = dateNow.AddMonths(-1);
                for (int i = 0; i < tableLength; i++)
                {
                    //比较时间是否超时
                    DateTime borrowDate = Convert.ToDateTime(dataSet.Tables[0].Rows[i]["borrowDate"].ToString());
                    if (borrowDate < outBorrowDate)
                    {
                        outDateRecords.Add("书名:" + dataSet.Tables[0].Rows[i]["bookName"].ToString());
                        outDateRecords.Add("借书证号:" + dataSet.Tables[0].Rows[i]["cardNumber"].ToString() + " " + "借书人：" + dataSet.Tables[0].Rows[i]["borrowerName"].ToString());
                        outDateRecords.Add("借书日期：" + borrowDate.ToLongDateString().ToString());
                        outDateRecords.Add("");
                    }
                }

                 //将内容展示至左侧文本框
                int listLength = outDateRecords.Count;
                if (listLength == 0)
                {
                    //如果没有过期者
                    string[] nothing = new string[1];
                    nothing[0] = "没有过期者";
                    richTextBox1.Lines = nothing;
                    Console.Beep(1200, 300);
                    connection.Close();
                    return;
                }
                string[] show = new string[listLength];
                for (int i = 0; i < listLength; i++)
                {
                    show[i] = outDateRecords[i].ToString();
                }
                Console.Beep(1200, 300);
                richTextBox1.Lines = show;
                connection.Close();
            }
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "借书人")
            {
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;

                label2.Text = "作者姓名";
                label3.Text = "例：刘慈欣";
                label4.Text = "该作者所著图书借出情况";
            }
            if (comboBox1.Text == "借书情况")
            {
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;

                label2.Text = "借书证号";
                label3.Text = "例：1101";
                label4.Text = "该借书证号借书情况";
            }
            if (comboBox1.Text == "过期者")
            {
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;

                label2.Text = "过期日期";
                label3.Text = "例：2020-6-6 不输入则默认为现在电脑时间";
                label4.Text = "借书过期者";
            }

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
