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
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            /* send/start button */

            if ((!checkBox1.Checked) && (button1.Text == "send"))
            {
                List<byte> ConvertedData = new List<byte>();

                ConvertedData = code.lib.ConvertStringAToByteA(richTextBox1.Text.Split('-'));

                /* Send data serially */
                if (code.serialport.SerialPort_WriteByteArray(ConvertedData.ToArray()))
                {
                    label1.Text = "sent";
                    label1.BackColor = System.Drawing.Color.Green;
                }

                else
                {
                    label1.Text = "failed";
                    label1.BackColor = System.Drawing.Color.Red;
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            /* To be implemented. */
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
                    if (richTextBox1.Text != null)
                    {
                        
                    }
                    else
                    {

                    }
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
    }
}
