using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

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
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            // Get the IP  
            //string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();

            string myIP = Dns.GetHostEntry(hostName).AddressList[1].MapToIPv4().ToString ();
            textBox4.Text = "TMMessage 111, `From cs`";
            textBox5.Text = myIP;

            int items = Dns.GetHostEntry(hostName).AddressList.Count();
            for (int i=0; i<items; ++i)
            {
                string ip = Dns.GetHostEntry(hostName).AddressList[i].ToString();
                comboBox1.Items.Add(ip);
            }
            comboBox1.SelectedIndex = 0;

            axETS_IPC_EX1.IpcCreateServer (textBox5.Text + "@");
            textBox1.Text = axETS_IPC_EX1.IpcGetServerName();
            
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
                    // Called From ETS as AskMailslotMessage, in this case we have 
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox5.Text = comboBox1.GetItemText(comboBox1.SelectedItem);
            RecreateServer();
        }

        private void RecreateServer()
        {
            axETS_IPC_EX1.IpcReleaseServer();
            axETS_IPC_EX1.IpcCreateServer(textBox5.Text + "@");
            textBox1.Text = axETS_IPC_EX1.IpcGetServerName();
        }
    }
}
