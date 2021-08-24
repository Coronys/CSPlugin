using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSPlugin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            axETS_IPC_EX1.IpcCreateServer ("172.29.64.1@");
            textBox1.Text = axETS_IPC_EX1.IpcGetServerName();
            textBox4.Text = "TMMessage 111, `From cs`";
            textBox5.Text = "172.29.64.1";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (axETS_IPC_EX1.IpcIsNewMessage() == 0)
                return;
            int OC = axETS_IPC_EX1.IpcGetNewMessageIndex();
            string Data = axETS_IPC_EX1.IpcGetNewMessageText();
            if (OC == 0) return;
            if (OC == 9999)
            {
                Environment.Exit(0);
            }
            textBox2.Text = OC.ToString();
            textBox3.Text = Data;
            switch (OC)
            {
                case 1:
                    break;
                case 2:
                    // From ETS called as AskMailslotMessage, in this case we have 
                    // to return value put some message into ETS runtime log
                    axETS_IPC_EX1.IpcReplyToServer(32,"TMMessage 101, `This is a message 1 to index 2`");
                    axETS_IPC_EX1.IpcReplyToServer(32,"TMMessage 102, `This is a message 1 to index 2`");
                    axETS_IPC_EX1.IpcReplyToServer(32,"TMMessage 103, `This is a message 1 to index 2`");
                    axETS_IPC_EX1.IpcReplyToServer(35,"Done");
                    break;
                case 3:
                    axETS_IPC_EX1.IpcReplyToServer(32, Data + "=`BBB`");
                    axETS_IPC_EX1.IpcReplyToServer(35,"Done");
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            axETS_IPC_EX1.IpcSendMessage(textBox5.Text + "@30000", 32, textBox4.Text);
        }
    }
}
