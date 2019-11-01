using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST_Serial.windows
{
    public partial class console : Form
    {
        public console()
        {
            InitializeComponent();

            code.serialport.SerialPort_Open();

            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Update_Console();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* Clear button */
            richTextBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /* Close button */
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void Update_Console()
        {
            if (code.serialport.IsPortOpen())
            {
                if (code.serialport.SerialPort_AvailBytes() > 0)
                {
                    richTextBox1.Text += code.serialport.SerialPort_ReadString();
                }
            }
        }

        ~console()
        {
            code.serialport.SerialPort_Close();

            timer1.Enabled = false;
        }
    }
}
