using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {      
        info INFO;
        SetINFO setinfo;
        Thread INFOThread;
        public Form1()
        {
            InitializeComponent();
            INFO = new info();
            setinfo = new SetINFO();
            setinfo.ShowINFO = 0;//默认简易
            setinfo.satus = 1;
            //调用INFO线程后台刷新数据
            INFOThread = new Thread(() => INFO.Main(setinfo));
            INFOThread.Start();
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
 
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            
        }

        private void Form1_PaddingChanged(object sender, EventArgs e)
        {

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
            else
            {
                this.notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e)
        {                              
            this.notifyIcon1.Text = INFO.SysInfo;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //退出按钮
            this.notifyIcon1.Dispose();
            setinfo.satus = 0;
            INFOThread.Abort();
            this.Close();
            this.Dispose();
            Application.Exit();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            this.notifyIcon1.Dispose();
            setinfo.satus = 0;
            INFOThread.Abort();
            this.Close();
            this.Dispose();
            Application.Exit();
        }

        private void cPUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //显示按钮
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void 简易ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setinfo.ShowINFO = 0;
        }

        private void 详细ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setinfo.ShowINFO = 1;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDown(object sender, MouseEventArgs e)
        {
            this.notifyIcon1.Text = INFO.SysInfo;
        }

        private void notifyIcon1_MouseUp(object sender, MouseEventArgs e)
        {
            this.notifyIcon1.Text = INFO.SysInfo;
        }
    }
}
