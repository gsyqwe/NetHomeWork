using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace compent
{
    public class Compent
    {
        public void updateTreeView(TreeView treeView1, String[] pa_th, List<string> listsongs)
        {
            TreeNode parentNode = null;
            for (int i = 0; i < pa_th.Length; i++) { 
            String[] paths = listsongs[i].Split('\\');
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
        }
        }
        public void updateListView(ListView listView1, OpenFileDialog ofd, String[] pa_th, List<string> listsongs)
        {
            for (int i = 0; i < pa_th.Length; i++)
            {
               // listBox1.Items.Add(Path.GetFileName(pa_th[i]));  //将音乐文件的文件名加载到listBox中
                listsongs.Add(pa_th[i]);    //将音乐文件的全路径存储到泛型集合中
                //加载TreeView
                String[] paths = listsongs[i].Split('\\');
                int length = paths.Length;
                int end = paths.Length;
                TreeNode parentNode = null;
                //加载ListView
                
                listView1.BeginUpdate();
                ListViewItem listviewitem = new ListViewItem();
                listviewitem.Text = paths[end - 1].Split('.')[0];
                listviewitem.SubItems.Add(paths[end - 1].Split('.')[1]);
                System.IO.FileInfo fileinfo = new FileInfo(ofd.FileNames[i]);
                listviewitem.SubItems.Add(fileinfo.Length.ToString());
                listviewitem.SubItems.Add(fileinfo.CreationTime.ToString());
                listView1.Items.Add(listviewitem);
                listView1.EndUpdate();
            }
        }
    }
}
