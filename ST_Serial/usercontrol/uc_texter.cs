using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace comm_tool.usercontrol
{
    public partial class uc_texter : UserControl
    {
        int KeyCount = -1;
        bool PlaceHifen = false;

        Stack<char> TextChar = new Stack<char>();

        private static Mutex serialPortMutex = new Mutex();

        public uc_texter(string GroupboxNo)
        {
            InitializeComponent();

            Initialize_TexterUserControl(GroupboxNo);
        }

        void Initialize_TexterUserControl(string GroupboxNo)
        {
            /* Disable Set time button. */
            button2.Enabled = false;
            button2.Visible = false;

            /* Append the Groupbox number to groupbox name. */
            groupBox1.Text += GroupboxNo;

            /* Set the label text to null (used to indicate the staus of the sent data)*/
            label1.Text = "";
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                button1.Text = "start";
                button2.Enabled = true;
                button2.Visible = true;
            }

            else
            {
                button1.Text = "send";
                button2.Enabled = false;
                button2.Visible = false;
            }

            /* Stop the timer on both cases. */
            timer1.Stop();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            /* send/start button */
            if (!(code.serialport.IsPortOpen() && code.serialport.SerialPort_IsConfigured()))
            {
                MessageBox.Show("please configure and open the port!", "Texting Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (!string.IsNullOrEmpty(richTextBox1.Text))
            {
                if (button1.Text == "start")
                {
                    if (timer1.Interval > 0)
                    {
                        timer1.Start();

                        button1.Text = "stop";
                    }
                    else
                    {
                        MessageBox.Show("invalid periodicity value", "Texting Information",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                else if (button1.Text == "stop")
                {
                    timer1.Stop();

                    button1.Text = "start";
                }

                else
                {
                    // button - send
                    Text_Data(richTextBox1.Text);
                }
            }

            else
            {
                label1.Text = "no data";
                label1.ForeColor = System.Drawing.Color.Blue;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            /**
             * If the set time button is pressed when texter is already sending serial data, it is prompted to stop the onging
             * communication before setting the new periocity value.
             **/
            if(button1.Text == "start")
            {
                windows.timing newTimingWindow = new windows.timing(timer1.Interval);

                if (newTimingWindow.ShowDialog() == DialogResult.OK)
                {
                    timer1.Interval = newTimingWindow.Timer_Interval;
                }
            }
            else
            {
                MessageBox.Show("stop the ongoing communication","Texting Information",
                    MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char pressedChar = e.KeyChar;

            /**
             * Having empty text box and non empty TextChar stack on keypress event will result in undesired printing of hifen
             **/
            if (string.IsNullOrEmpty(richTextBox1.Text) && (TextChar.Count > 0))
            {
                TextChar.Clear();
            }
            else
            {
                /*< Do nothing */
            }

            if (!("0123456789ABCDEFabcdef\b".Contains(pressedChar)))
            {
                e.Handled = true;
            }

            else if (('\b' == pressedChar) && (!string.IsNullOrEmpty(richTextBox1.Text)))
            {
              if (1 == TextChar.Count)
                {
                    TextChar.Push('r');
                    TextChar.Push('r');

                    System.Windows.Forms.SendKeys.Send("\b");
                }
                else
                {
                    TextChar.Pop();
                }
            }
            else
            {
                if (2 == TextChar.Count)
                {
                    richTextBox1.Text += "-";

                    TextChar.Clear();
                }

                TextChar.Push(pressedChar);

                /* Placing the cursor at the last position. */
                richTextBox1.Focus();
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
            }
        }

        private void Text_Data(string StringData)
        {
            List<byte> txData = new List<byte>();

            txData = code.lib.ConvertStringAToByteA(StringData.Split('-'));

            /* Send data serially */
            if (code.serialport.SerialPort_WriteByteArray(txData.ToArray()))
            {
                label1.Text = "sent";
                label1.ForeColor = System.Drawing.Color.Green;
            }

            else
            {
                label1.Text = "failed";
                label1.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            if (serialPortMutex.WaitOne(100))
            {
                Text_Data(richTextBox1.Text);
            }

            timer1.Start();
        }
    }
}
