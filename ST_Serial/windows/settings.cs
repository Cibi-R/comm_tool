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

namespace comm_tool.windows
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

            /* Serial cannot be configured while the port is in open condition. */
            if (code.serialport.IsPortOpen())
            {
                MessageBox.Show("Close the port before proceed to configure","Configuration info",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }

            else if (TempPort != null)
            {
                code.serialport.SerialPort_Configure(TempPort);

                Store_Configured_Values();

                Properties.Settings.Default.IsConfigured = true;

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }

            else
            {
                Properties.Settings.Default.IsConfigured = false;

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


            if (Properties.Settings.Default.IsConfigured)
            {
                Retrieve_Configured_Values();
            }

            else
            {
                /* Initial values for the combo box if not configured. */
                comboBox2.SelectedIndex = 1;
                comboBox3.SelectedIndex = 0;
                comboBox4.SelectedIndex = 0;
            }
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

        private void Store_Configured_Values()
        {
            Properties.Settings.Default.Port = comboBox1.Text;
            Properties.Settings.Default.Baud_Rate = comboBox2.Text;
            Properties.Settings.Default.Stop_Bit = comboBox3.Text;
            Properties.Settings.Default.Parity_Bit = comboBox4.Text;
        }

        private void Retrieve_Configured_Values()
        {
            /* Port */
            if (Get_Available_Ports().Any(Properties.Settings.Default.Port.Contains))
            {
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(Properties.Settings.Default.Port);
            }else { /* Do nothing */ }

            /* Baud Rate */
            comboBox2.SelectedIndex = comboBox2.Items.IndexOf(Properties.Settings.Default.Baud_Rate);

            /* Stop Bit */
            comboBox3.SelectedIndex = comboBox3.Items.IndexOf(Properties.Settings.Default.Stop_Bit);

            /* Parity Bit */
            comboBox4.SelectedIndex = comboBox4.Items.IndexOf(Properties.Settings.Default.Parity_Bit);
        }
    }
}
