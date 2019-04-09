using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace net_second
{
    public partial class Form1 : Form
    {
        List<string> listfiles = new List<string>();   //用来存储音乐文件的全路径
        XmlDocument xml = new XmlDocument();//用于读取xml文件
        ListViewItem listViewItem;
        ListViewItem.ListViewSubItem listViewSubItem;
        XmlTextWriter textWriter;
        XmlNode Xmlroot;
        public Form1()
        {
            InitializeComponent();
        }
        //打开文件
        private void button1_Click(object sender,EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择xml文件";      //打开对话框的标题
            ofd.InitialDirectory = @"D:\";    //设置打开对话框的初始设置目录
            ofd.Multiselect = true; //设置多选
            ofd.Filter = @"xml文件|*.xml|所有文件|*.*";    //设置文件格式筛选
            ofd.ShowDialog();   //显示打开对话框
            string[] path = ofd.FileNames;       //获得在文件夹中选择的所有文件的全路径
            //在TreeView中显示
            TreeNode parentNode = null;
            for (int i = 0; i < path.Length; i++)
            {
                listfiles.Add(path[i]);
                String[] paths = listfiles[i].Split('\\');
                int length = paths.Length;
                int end = paths.Length;
                while (length != 0)
                {
                    if (length == end)
                    {
                        parentNode = treeView1.Nodes.Add(paths[end - length]);
                    }
                    else
                    {
                        parentNode = parentNode.Nodes.Add(paths[end - length]);
                    }
                    length--;
                }
                //读取xml文件
                //将XML文件加载进来,并在listView中显示xml文件中内容
                xml.Load(path[i]);
                XmlNodeReader reader = new XmlNodeReader(xml);
                String str = "";
                reader.Read();
                reader.Read();
                while(reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            str = reader.Name;
                            break;
                        case XmlNodeType.Text:
                            if (str == "ID")
                            {
                                listViewItem = listView1.Items.Add(reader.Value);
                                break;
                            }
                            else
                            {
                                listViewSubItem = listViewItem.SubItems.Add(reader.Value);
                            }
                            break;

                    }
                }
            }
        }
        //保存文件
        private void button2_Click(object sender, EventArgs e)
        {
            //选择保存路径与文件名
            string localFilePath = "";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "请选择保存路径";//对话框标题
            sfd.InitialDirectory = @"D:\";
            sfd.Filter = @"xml文件|*.xml|所有文件|*.*";//保存文件类型
            //点了保存按钮进入 
            if (sfd.ShowDialog() == DialogResult.OK)
            {
               localFilePath = sfd.FileName.ToString(); //获得文件路径 
               Console.WriteLine(localFilePath);
               //保存创建好的xml文件
               //如果xml文件为空则创建一个进行保存
               if (xml.DocumentElement == null)
               {
                   xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", ""));//xml文件头
                   XmlElement Root = xml.CreateElement("Root");//根元素
                   xml.AppendChild(Root);
               }
               else
               {
                   TreeToXML(treeView1,localFilePath);
               }
            }
        }

        public int TreeToXML(TreeView TheTreeView, string XMLFilePath)
        {
            //-------初始化转换环境变量
            TreeView thetreeview = TheTreeView;
            String xmlfilepath = XMLFilePath;
            textWriter = new XmlTextWriter(xmlfilepath, null);

            //-------创建XML写操作对象
            textWriter.Formatting = Formatting.Indented;

            //-------开始写过程，调用WriteStartDocument方法
            textWriter.WriteStartDocument();

            //-------添加第一个根节点
            textWriter.WriteStartElement("TreeExXMLCls");
            textWriter.WriteEndElement();

            //------ 写文档结束，调用WriteEndDocument方法
            textWriter.WriteEndDocument();

            //-----关闭输入流
            textWriter.Close();

            //-------创建XMLDocument对象
            xml.Load(xmlfilepath);

            //------选中根节点
            XmlElement Xmlnode = xml.CreateElement(thetreeview.Nodes[0].Text.Replace(":",""));
            Xmlroot = xml.SelectSingleNode("TreeExXMLCls");

            //------遍历原treeview控件，并生成相应的XML
            TransTreeSav(thetreeview.Nodes, (XmlElement)Xmlroot,xmlfilepath);


            return 0;


        }

        private int TransTreeSav(TreeNodeCollection nodes, XmlElement ParXmlnode,String xmlfilepath)
        {

            //-------遍历树的各个故障节点，同时添加节点至XML
            XmlElement xmlnode;
            Xmlroot = xml.SelectSingleNode("TreeExXMLCls");

            foreach (TreeNode node in nodes)
            {
                xmlnode = xml.CreateElement(node.Text.Replace(":",""));
                ParXmlnode.AppendChild(xmlnode);

                if (node.Nodes.Count > 0)
                {
                    TransTreeSav(node.Nodes, xmlnode,xmlfilepath);
                }
            }
            xml.Save(xmlfilepath);

            return 0;
        }


        //设置树单选,就是只能有一个树节点被选中
        private void SetNodeCheckStatus(TreeNode tn, TreeNode node)
        {
            if (tn == null)
                return;
            if (tn != node)
            {
                tn.Checked = false;
            }
            // Check children nodes
            foreach (TreeNode tnChild in tn.Nodes)
            {
                if (tnChild != node)
                {
                    tnChild.Checked = false;
                }
                SetNodeCheckStatus(tnChild, node);
            }
        }
        //在树节点被选中后触发
        private void treeView1_AfterCheacked(object sender, TreeViewEventArgs e)
        {
            //过滤不是鼠标选中的其它事件，防止死循环
            if (e.Action != TreeViewAction.Unknown)
            {
                //Event call by mouse or key-press
                foreach (TreeNode tnChild in treeView1.Nodes)
                    SetNodeCheckStatus(tnChild, e.Node);
                string sName = e.Node.Text;
            }
        }
        //获得选择节点
        private void GetSelectNode(TreeNode tn)
        {
            if (tn == null)
                return;
            if (tn.Checked == true)
            {
                m_NodeName = tn.Text;
                return;
            }
            // Check children nodes
            foreach (TreeNode tnChild in tn.Nodes)
            {
                GetSelectNode(tnChild);
            }
        }
        //选择树的节点并点击右键，触发事件
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)//判断你点的是不是右键
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode CurrentNode = treeView1.GetNodeAt(ClickPoint);
                if (CurrentNode != null && CurrentNode is TreeNode)//判断你点的是不是一个节点
                {
                    CurrentNode.ContextMenuStrip = contextMenuStrip1;
                    treeView1.SelectedNode = CurrentNode;//选中这个节点
                }
            }
        }
        private String m_NodeName = null;
        //右键设置节点可以重命名
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //窗体的LabelEdir为false，因此每次要BeginEdit时都要先自LabelEdit为true
            treeView1.LabelEdit = true;
            treeView1.SelectedNode.BeginEdit();
        }
        //右键添加节点
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //在Tree选择节点的同一级添加节点
            treeView1.LabelEdit = true;
            TreeNode CurrentNode = treeView1.SelectedNode.Nodes.Add("Node1");
            //更新选择节点
            treeView1.SelectedNode.Checked = false;
            CurrentNode.Checked = true;
            //使添加的树节点处于可编辑的状态
            CurrentNode.BeginEdit();
        }
        //右键删除节点
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode.Remove();
        }
        //添加根节点
        private void toolStripMenuItem4_Click(object sender,EventArgs e)
        {
            //添加根节点
            treeView1.Nodes.Add("Node1");
        }
    }
}
