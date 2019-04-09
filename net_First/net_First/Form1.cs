using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;	//Path类用到
using compent;
namespace net_First
{
    public partial class Form1 : Form
    {
        List<string> listsongs = new List<string>();   //用来存储音乐文件的全路径
        String state = "stop";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //实现打开文件
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择音乐文件";      //打开对话框的标题
            ofd.InitialDirectory = @"F:\music";    //设置打开对话框的初始设置目录
            ofd.Multiselect = true; //设置多选
            ofd.Filter = @"音乐文件|*.mp3||*.wav|所有文件|*.*";    //设置文件格式筛选
            ofd.ShowDialog();   //显示打开对话框
            string[] pa_th = ofd.FileNames;       //获得在文件夹中选择的所有文件的全路径
            for (int i = 0; i < pa_th.Length;i++ )
            {
                listBox1.Items.Add(Path.GetFileName(pa_th[i]));  //将音乐文件的文件名加载到listBox中
                listsongs.Add(pa_th[i]);    //将音乐文件的全路径存储到泛型集合中
            }
            compent.Compent comp = new Compent();
            comp.updateListView(listView1, ofd, pa_th, listsongs);
            comp.updateTreeView(treeView1,pa_th, listsongs);
        }
        

        //播放
        private void button2_Click(object sender, EventArgs e)
        {
            if (state == "stop")
            {
                axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedIndex];
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            else if (state == "pause")
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }

        //暂停
        private void button3_Click(Object sender, EventArgs e)
        {
            state = "pause";
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        //停止
        private void button4_Click(Object sender, EventArgs e)
        {
            state = "stop";
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        //加速
        private void button5_Click(Object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.rate += 0.1;
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition += 1;
        }

        //减速
        private void button6_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.rate -= 1;
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition += 1;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}
