using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static Form2 form2 = new Form2();
        public Form1()
        {
            InitializeComponent();
        }

        //弹出新窗口
        private void button11_Click(object sender, EventArgs e)
        {
            form2.Show();
            Thread t1 = new Thread(new ThreadStart(maopao));
            t1.Start();
            Thread t2 = new Thread(new ThreadStart(selectsort));
            t2.Start();
            Thread t3 = new Thread(new ThreadStart(quicksort));
            t3.Start();
        }

        //冒泡排序方法
        private void maopao()
        {
            //赋值
            double[] length = new double[10];
            Button[] buttons = new Button[10];
            buttons[0] = this.button1;
            buttons[1] = this.button2;
            buttons[2] = this.button3;
            buttons[3] = this.button4;
            buttons[4] = this.button5;
            buttons[5] = this.button6;
            buttons[6] = this.button7;
            buttons[7] = this.button8;
            buttons[8] = this.button9;
            buttons[9] = this.button10;
            for (int i = 0; i < 10; i++)
            {
                length[i] = buttons[i].Width;
            }
            int value = 0;
            form2.progressBar1.Maximum = 45;
            //进行排序
            for (int i = 0; i < length.Length - 1; i++)
            {
                for (int j = length.Length - 1; j > i; j--)
                {
                    if (length[j] > length[j - 1])
                    {
                        length[j] = length[j] + length[j - 1];
                        length[j - 1] = length[j] - length[j - 1];
                        length[j] = length[j] - length[j - 1];
                        var location = buttons[j - 1].Location;
                        buttons[j - 1].Location = buttons[j].Location;
                        buttons[j].Location = location;
                        var button = buttons[j - 1];
                        buttons[j - 1] = buttons[j];
                        buttons[j] = button;
                    }
                    value++;
                    form2.progressBar1.Value = 0;
                    form2.progressBar1.Value = value;
                    Thread.Sleep(500);
                }
            }
        }

        //选择排序
        private void selectsort()
        {
            //赋值
            double[] length = new double[10];
            Button[] buttons = new Button[10];
            buttons[0] = this.button12;
            buttons[1] = this.button13;
            buttons[2] = this.button14;
            buttons[3] = this.button15;
            buttons[4] = this.button16;
            buttons[5] = this.button17;
            buttons[6] = this.button18;
            buttons[7] = this.button19;
            buttons[8] = this.button20;
            buttons[9] = this.button21;
            for (int i = 0; i < 10; i++)
            {
                length[i] = buttons[i].Width;
            }
            //排序
            form2.progressBar2.Maximum = 9;
            for (int i = 0; i < length.Length; i++)
            {
                int index = i;//最小值
                for (int j = i + 1; j < length.Length; j++)
                {
                    if (length[index] < length[j])
                    {
                        index = j;
                    }
                }
                if (index != i)
                {
                    var temp = length[index];
                    length[index] = length[i];
                    length[i] = temp;
                    var location = buttons[index].Location;
                    buttons[index].Location = buttons[i].Location;
                    buttons[i].Location = location;
                    var button = buttons[index];
                    buttons[index] = buttons[i];
                    buttons[i] = button;
                }
                form2.progressBar2.Value = 0;
                form2.progressBar2.Value = i;
                Thread.Sleep(500);
            }
        }

        //快速排序
        private void quicksort()
        {
            //赋值
            double[] length = new double[10];
            Button[] buttons = new Button[10];
            buttons[0] = this.button22;
            buttons[1] = this.button23;
            buttons[2] = this.button24;
            buttons[3] = this.button25;
            buttons[4] = this.button26;
            buttons[5] = this.button27;
            buttons[6] = this.button28;
            buttons[7] = this.button29;
            buttons[8] = this.button30;
            buttons[9] = this.button31;
            for (int i = 0; i < 10; i++)
            {
                length[i] = buttons[i].Width;
            }
            //排序
            form2.progressBar3.Maximum=6;
            quick(length, buttons, 0, 9);
        }
        private static int count = 0;
        private void quick(double[] data, Button[] buttons, int left, int right)
        {
            if (left < right)
            {
                int i = Division(data, buttons, left, right);
                //对枢轴的左边部分进行排序
                quick(data, buttons, i + 1, right);
                //对枢轴的右边部分进行排序
                quick(data, buttons, left, i - 1);
            }
        }
        private static int Division(double[] list,Button[]buttons, int left, int right)
        {
            while (left < right)
            {
                double num = list[left]; //将首元素作为枢轴
                if (num < list[left + 1])
                {
                    list[left] = list[left + 1];
                    list[left + 1] = num;
                    var location = buttons[left].Location;
                    var button = buttons[left];
                    buttons[left].Location = buttons[left + 1].Location;
                    buttons[left + 1].Location = location;
                    buttons[left] = buttons[left + 1];
                    buttons[left + 1] = button;
                    left++;
                }
                else
                {
                    double temp = list[right];
                    list[right] = list[left + 1];
                    list[left + 1] = temp;
                    var location = buttons[right].Location;
                    var button = buttons[right];
                    buttons[right].Location = buttons[left + 1].Location;
                    buttons[left + 1].Location = location;
                    buttons[right] = buttons[left + 1];
                    buttons[left + 1] = button;
                    right--;
                }
            }
            count = count +1 ;
            form2.progressBar3.Value = 0;
            form2.progressBar3.Value=count;
            Thread.Sleep(500);
            return left; //指向的此时枢轴的位置
        }
    }
}
