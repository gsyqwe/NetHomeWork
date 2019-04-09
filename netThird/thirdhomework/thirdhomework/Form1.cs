using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace thirdhomework
{
    public partial class Form1 : Form
    {
        DataSet dataSet = new DataSet();
        MySqlConnection mySqlConnection;
        public Form1()
        {
            InitializeComponent();
            mySqlConnection = new MySqlConnection("server=localhost;User Id=root;password=123123;Database=j2eebig");
            mySqlConnection.Open();
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("show databases", mySqlConnection);
            mySqlDataAdapter.Fill(dataSet, "databases");
            mySqlConnection.Close();
            dataGridView1.DataSource = dataSet.Tables["databases"];
        }

        private string nowdatabase;//当前被选中的数据库

        //得到nowdatabase
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            nowdatabase = dataGridView1.CurrentCell.Value.ToString();
            mySqlConnection = new MySqlConnection("server=localhost;User Id=root;password=123123;Database=" + nowdatabase);
            mySqlConnection.Open();
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("show tables", mySqlConnection);
            DataSet set = new DataSet();
            mySqlDataAdapter.Fill(set, "tables");
            mySqlConnection.Close();
            dataGridView2.DataSource = set.Tables["tables"];
        }

        private string lastdatabase = "";//旧得到数据库
        //创建、修改数据库名
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string newDatabase = dataGridView1.CurrentCell.Value.ToString();
            mySqlConnection = new MySqlConnection("server=localhost;User Id=root;password=123123;Database=j2eebig");
            mySqlConnection.Open();
            MySqlCommand mySqlCommand = new MySqlCommand("create database " + newDatabase, mySqlConnection);
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
            if (lastdatabase != "")
            {
                mySqlConnection = new MySqlConnection("server=localhost;User Id=root;password=123123;Database=" + lastdatabase);
                mySqlConnection.Open();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("show tables", mySqlConnection);
                mySqlDataAdapter.Fill(dataSet, "tables");
                foreach (DataRow dataRow in dataSet.Tables["tables"].Rows)
                {
                    string tableName = dataRow.ItemArray[0].ToString();
                    MySqlCommand command = new MySqlCommand("rename table " + lastdatabase + "." + tableName + " to " + newDatabase + "." + tableName, mySqlConnection);
                    command.ExecuteNonQuery();

                }
                MySqlCommand mySqlCommand2 = new MySqlCommand("drop database " + lastdatabase, mySqlConnection);
                mySqlCommand2.ExecuteNonQuery();
                mySqlConnection.Close();
            }
        }
        //开始进行编辑,将名字改成新设计的名字
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            lastdatabase = dataGridView1.CurrentCell.Value.ToString();
        }

        //删除数据库
        private void button1_Click(object sender, EventArgs e)
        {
            string databasetodelete= dataGridView1.CurrentCell.Value.ToString();//得到要删除的名称
            mySqlConnection = new MySqlConnection("server=localhost;User Id=root;password=123123;Database=" + "j2eebig");
            mySqlConnection.Open();
            MySqlCommand mySqlCommand = new MySqlCommand("drop database " + databasetodelete, mySqlConnection);
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
        }

        //删除表
        private void button2_Click(object sender, EventArgs e)
        {
            string tabletodelete = dataGridView2.CurrentCell.Value.ToString();//得到要删除的表名称
            mySqlConnection = new MySqlConnection("server=localhost;User Id=root;password=123123;Database=" + nowdatabase);
            mySqlConnection.Open();
            MySqlCommand mySqlCommand = new MySqlCommand("drop table " +tabletodelete, mySqlConnection);
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
            dataGridView2.Rows.Remove(dataGridView2.CurrentRow);
        }
        //进入Form2
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.loadData(nowdatabase, dataGridView2.CurrentCell.Value.ToString());
            form2.Show();
        }

        //创建、修改数据库表名
        private string nowtable = "";
        //当前选中的表
        private void dataGridView2_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            nowtable = dataGridView2.CurrentCell.Value.ToString();
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string newtable = dataGridView2.CurrentCell.Value.ToString();
            mySqlConnection = new MySqlConnection("server=localhost;User Id=root;password=123123;Database=" + nowdatabase);
            mySqlConnection.Open();
            MySqlCommand mySqlCommand;
            if (nowtable == "")
            {
                mySqlCommand = new MySqlCommand("create table " + newtable + "(id int(11))", mySqlConnection);//创建表
            }
            else
            {
                mySqlCommand = new MySqlCommand("alter table " + nowtable + " rename to " + newtable, mySqlConnection);//修改表
            }
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
