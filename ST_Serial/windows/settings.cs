using System;
using System.IO.Ports;
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
    public partial class settings : Form
    {
        public settings()
        {
            InitializeComponent();

            Init_Settings_UI();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SerialPort TempPort = Get_Configured_Values();

            if (TempPort != null)
            {
                code.serialport.SerialPort_Configure(TempPort);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }

            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Retry;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void Init_Settings_UI()
        {
            /* comboBox1 for com ports. */
            comboBox1.Items.AddRange(Get_Available_Ports());

            /* comboBox2 for baud rate. */
            comboBox2.Items.AddRange(new string[]{"4800","9600","19200","38400","57600" });

            /* comboBox3 for stop bits */
            comboBox3.Items.AddRange(new string[] { "1", "2" });

            /* combobox4 for parity control */
            comboBox4.Items.AddRange(new string[] { "None","Even", "Odd","Mark","Space" });


            /* Initial values for the combo box. */
            comboBox2.SelectedIndex = 1;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        private string[] Get_Available_Ports()
        {
            return SerialPort.GetPortNames();
        }

        private SerialPort Get_Configured_Values()
        {
            SerialPort TempPort = new SerialPort();

            try
            {
                TempPort.PortName = comboBox1.Text;
                TempPort.BaudRate = int.Parse(comboBox2.Text);
                TempPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBox3.Text);
                TempPort.Parity = (Parity)Enum.Parse(typeof(Parity), comboBox4.Text);
            }

            catch
            {
                TempPort = null;
            }

            return TempPort;
        }
    }
}
