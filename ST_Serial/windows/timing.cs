using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace comm_tool.windows
{
    public partial class timing : Form
    {
        public int Timer_Interval = 0;
        public timing(int CurrentValue)
        {
            InitializeComponent();

            Init_Timing_Window_UI(CurrentValue);
        }

        private void Init_Timing_Window_UI(int CurrentValue)
        {
            textBox1.Text = CurrentValue.ToString();

            label1.Text = "Enter value in Milliseconds!";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char PressedChar = e.KeyChar;

            if (!("0123456789\b".Contains(PressedChar)))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* Set Time button. */
            bool IsParsed = int.TryParse(textBox1.Text, out Timer_Interval);

            if ((!IsParsed) || (Timer_Interval < 1))
            {
                MessageBox.Show("Enter Valid Value!", "Set Time Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
