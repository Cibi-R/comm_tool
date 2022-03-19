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
    public partial class texter : Form
    {
        /*< to select the different color for richbox */
        static int ColorSelect = 0;

        public texter()
        {
            InitializeComponent();

            InitializeTexterWindow_UI();
        }

        private void InitializeTexterWindow_UI()
        {
            /* Initially add one user control to the flow layout panel. */
            usercontrol.uc_texter newUserControl = new usercontrol.uc_texter(flowLayoutPanel1.Controls.Count.ToString());
            flowLayoutPanel1.Controls.Add(newUserControl);
            timer1.Start();

            /*< make rich textbox1 font bold */
            richTextBox1.Font = new Font(richTextBox1.Font, FontStyle.Bold);

            /*< flash out the serial port, to avoid displaying data's from the other windows */
            code.serialport.SerialPort_Flash();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* Add button */
            if (flowLayoutPanel1.Controls.Count < 10)
            {
                /* To add user control at runtime. */
                usercontrol.uc_texter newUserControl = new usercontrol.uc_texter(flowLayoutPanel1.Controls.Count.ToString());
                flowLayoutPanel1.Controls.Add(newUserControl);
            }
            else
            {
                MessageBox.Show("transmitter window limit exceeded!!!","", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Update_Texter_ReceiveBox();
        }

        ~texter()
        {
          
        }

        private void Update_Texter_ReceiveBox()
        {
            if (code.serialport.IsPortOpen())
            {
                if (code.serialport.SerialPort_AvailBytes() > 0)
                {
                    string val = "";

                    foreach (byte Data in code.serialport.SerialPort_ReadByte())
                    {
                        val += (Data.ToString() + "-");
                    }

                    switch (ColorSelect)
                    {
                        case 0: 
                            richTextBox1.SelectionBackColor = Color.Green; break;
                        case 1:
                            richTextBox1.SelectionBackColor = Color.Red; break;
                        case 2:
                            richTextBox1.SelectionBackColor = Color.Blue; break;
                        case 4:
                            richTextBox1.SelectionBackColor = Color.Black; break;
                        case 5:
                            richTextBox1.SelectionBackColor = Color.Magenta; break;
                        default:
                        {
                            richTextBox1.SelectionBackColor = Color.Magenta;
                                ColorSelect = -1;
                            break;
                        }
                    }
                    ColorSelect++; /*< increment to select the next color */

                    richTextBox1.SelectionColor = Color.White;
                    richTextBox1.SelectedText += val;
                }
            }
        }
    }
}
