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

        private void button1_Click(object sender, EventArgs e)
        {
            /* send/start button */

            if ((!checkBox1.Checked) && (button1.Text == "send"))
            {
                MessageBox.Show("Data sent!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /* set time button */
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
                if (PressedChar == '\b')
                {
                    if ((richTextBox1.Text != null) && (KeyCount != -1))
                    {
                        //KeyCount
                    }
                }

                else
                {
                    KeyCount++;

                    if (KeyCount == 2)
                    {
                        KeyCount = 0;
                        richTextBox1.Text += "-";

                        /* Placing the cursor at the last position. */
                        richTextBox1.Focus();
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    }
                }
            }
        }
    }
}
