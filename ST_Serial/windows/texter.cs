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
    public partial class texter : Form
    {
        public texter()
        {
            InitializeComponent();

            Init_TexterWindow_UI();
        }

        private void Init_TexterWindow_UI()
        {
            /* Initially add one control to the panel. */
            usercontrol.uc_texter NewControl = new usercontrol.uc_texter(flowLayoutPanel1.Controls.Count.ToString());
            flowLayoutPanel1.Controls.Add(NewControl);
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* Add button */
            if (flowLayoutPanel1.Controls.Count < 10)
            {
                /* To add user control at runtime. */
                usercontrol.uc_texter NewControl = new usercontrol.uc_texter(flowLayoutPanel1.Controls.Count.ToString());
                flowLayoutPanel1.Controls.Add(NewControl);
            }
            else
            {
                MessageBox.Show("Maximum Limit Reached!","", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Update_Texter();
        }

        ~texter()
        {
          
        }

        private void Update_Texter()
        {
            if (code.serialport.IsPortOpen())
            {
                if (code.serialport.SerialPort_AvailBytes() > 0)
                {
                    string Val = "";
                    foreach (byte Data in code.serialport.SerialPort_ReadByte())
                    {
                        Val += (Data.ToString() + " "); 
                    }
                    MessageBox.Show(Val);
                }
            }
        }
    }
}
