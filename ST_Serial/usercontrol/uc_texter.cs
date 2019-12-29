using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST_Serial.usercontrol
{
    public partial class uc_texter : UserControl
    {
        int KeyCount = -1;
        bool PlaceHifen = false;
        bool BreakTimer = false;
        public uc_texter(string GroupboxNo)
        {
            InitializeComponent();

            Init_TexterUserControl(GroupboxNo);
        }

        void Init_TexterUserControl(string GroupboxNo)
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
                MessageBox.Show("Port not cofigured or opened!", "Texting Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if ((!checkBox1.Checked) && (button1.Text == "send"))
            {
                Text_Data(richTextBox1.Text);
            }

            else if (checkBox1.Checked)
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
                        MessageBox.Show("Interval has not been set","Texting Information",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                else if (button1.Text == "stop")
                {
                    if (timer1.Interval > 0)
                    {
                        timer1.Stop();

                        button1.Text = "start";
                    }
                }
            }

            else
            {
                /* Do Nothing */
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            /* SetTime button */
            
            if(button1.Text == "start")
            {
                windows.timing NewTimingWindow = new windows.timing(timer1.Interval);

                if (NewTimingWindow.ShowDialog() == DialogResult.OK)
                {
                    timer1.Interval = NewTimingWindow.Timer_Interval;
                }
                else
                {
                    MessageBox.Show("Previous value retained for timer!", "Texting Information", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            char PressedChar = e.KeyChar;

            if (!("0123456789ABCDEFabcdef\b".Contains(PressedChar)))
            {
                e.Handled = true;
            }

            else
            {
                /* Behaviour of textbox while pressing backspace to be implemented. */
                if (PressedChar == '\b')
                {

                }

                else
                {
                    KeyCount++;

                    if (PlaceHifen)
                    {
                        PlaceHifen = false;

                        richTextBox1.Text += "-";

                        /* Placing the cursor at the last position. */
                        richTextBox1.Focus();
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    }

                    if (KeyCount == 1)
                    {
                        KeyCount = -1;
                        PlaceHifen = true;
                    }
                }
            }
        }

        private void Text_Data(string StringData)
        {
            List<byte> ConvertedData = new List<byte>();

            ConvertedData = code.lib.ConvertStringAToByteA(StringData.Split('-'));

            if (ConvertedData.Count == 0)
            {
                label1.Text = "Value Empty";
                label1.ForeColor = System.Drawing.Color.Blue;
            }
            /* Send data serially */
            else if (code.serialport.SerialPort_WriteByteArray(ConvertedData.ToArray()))
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
                                                         // APP.NONE
            while ((!(code.serialport.CurrentSerialAPP == (int)0)) && (!BreakTimer))
            {
                if (!timer2.Enabled)
                {
                    timer2.Enabled = true;
                    timer2.Start();
                }
                else { /* Do Nothing */ }

                if (timer1.Enabled)
                {
                    timer1.Stop();
                    timer1.Enabled = false;
                }
                else { /* Do Nothing */ }
            }

            timer1.Enabled = true;
            timer1.Start();

            timer2.Stop();
            timer2.Enabled = false;

            if (!BreakTimer)
            {
                /* Set the serial port resource to texter  */
                code.serialport.CurrentSerialAPP = (int)3;

                Text_Data(richTextBox1.Text);

                /* release the serial port resource from texter */
                code.serialport.CurrentSerialAPP = (int)0;
            }
            else
            {
                BreakTimer = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            BreakTimer = true;
        }
    }
}
