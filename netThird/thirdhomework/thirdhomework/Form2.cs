using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace thirdhomework
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        DataSet dataset;
        //xml节点
        private Dictionary<TreeNode, XmlNode> Tree = new Dictionary<TreeNode, XmlNode>();
        private XmlDocument xml = new XmlDocument();
        //读取数据进行加载内容,传入参数数据库与表名称
        public void loadData(string database, string table)
        {
            treeView1.LabelEdit = true;
            MySqlConnection mySqlConnection = new MySqlConnection("server=localhost;User Id=root;password=123123;Database=" + database);
            mySqlConnection.Open();
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("select * from " + table, mySqlConnection);
            dataset = new DataSet();
            mySqlDataAdapter.Fill(dataset, "data");
            mySqlConnection.Close();
            dataGridView1.DataSource = dataset.Tables["data"];
            xml.LoadXml(dataset.GetXml());
            treeView1.Nodes.Clear();
            TreeNode treeRoot = new TreeNode();
            XmlNode xmlRoot = xml.DocumentElement;
            if (xmlRoot != null)
            {
                Tree.Add(treeRoot, xmlRoot);
                treeRoot.Tag = xmlRoot.NodeType.ToString();
                treeRoot.Text = xmlRoot.Name;
                loadNodes(treeRoot, xmlRoot);
            }
            treeView1.Nodes.Add(treeRoot);
        }

        //加载树节点
        private void loadNodes(TreeNode treeNode, XmlNode xmlNode)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {

                TreeNode treeNode1 = new TreeNode();
                if (node.NodeType == XmlNodeType.Text)
                {
                    treeNode1.Text = node.InnerText;
                    treeNode1.BackColor = Color.Red;
                }
                else
                {
                    treeNode1.Text = node.Name;
                }
                treeNode1.Tag = node.NodeType.ToString();
                Tree.Add(treeNode1, node);
                treeNode.Nodes.Add(treeNode1);
                loadNodes(treeNode1, node);
            }
        }

        //增加节点
        private void button1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag.ToString() == "Element")
            {
                TreeNode currentNode = new TreeNode();
                currentNode.Text = "节点1";
                treeView1.SelectedNode.Nodes.Add(currentNode);
                currentNode.Tag = "Element";
                XmlNode xmlNode = xml.CreateElement(currentNode.Text);
                Tree[treeView1.SelectedNode].AppendChild(xmlNode);
                Tree.Add(currentNode, xmlNode);
                currentNode.BeginEdit();
            }
        }

        //删除节点
        private void button2_Click(object sender, EventArgs e)
        {
            XmlNode node = Tree[treeView1.SelectedNode];
            node.ParentNode.RemoveChild(node);
            treeView1.SelectedNode.Remove();
        }

        //保存树
        private void button3_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.Filter = "txt文件(*.txt)|*.txt";
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = File.Create(this.saveFileDialog1.FileName);
                BinaryFormatter bf = new BinaryFormatter();
                xuliehua temp = new xuliehua();
                temp.XmlDocument = xml;
                bf.Serialize(fs, temp);
                fs.Close();
            }
        }

        //加载树
        private void button4_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "txt文件(*.txt)|*.txt";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fileStream = File.OpenRead(openFileDialog1.FileName);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                xuliehua temp = (xuliehua)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
                Tree = new Dictionary<TreeNode, XmlNode>();
                treeView1.Nodes.Clear();
                TreeNode treeRoot = new TreeNode();
                XmlNode xmlRoot = temp.XmlDocument.DocumentElement;
                if (xmlRoot != null)
                {
                    Tree.Add(treeRoot, xmlRoot);
                    treeRoot.Tag = xmlRoot.NodeType.ToString();
                    treeRoot.Text = xmlRoot.Name;
                    loadNodes(treeRoot, xmlRoot);
                }
                treeView1.Nodes.Add(treeRoot);
            }
        }

        //保存为xml
        private void button5_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.Filter = "XML文件(*.xml)|*.xml";
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                dataset.WriteXml(this.saveFileDialog1.FileName);
            }
        }
    }
}
