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
            for (int i = 0; i < pa_th.Length; i++)
            {
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
          
        }
    }
}
