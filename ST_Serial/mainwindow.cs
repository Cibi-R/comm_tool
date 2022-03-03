using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace comm_tool
{
    public partial class mainwindow : Form
    {
        public mainwindow()
        {
            InitializeComponent();

            windows.console.Console_Log_Info("TempValues");
            windows.console.Console_Log_Warning("TempValues");
            windows.console.Console_Log_Error("TempValues");
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            windows.settings NewSettingWindow = new windows.settings();

            var DialogResult = NewSettingWindow.ShowDialog();

            if (DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                
            }

            else if (DialogResult == System.Windows.Forms.DialogResult.Retry)
            {
                MessageBox.Show("Unsupported values configured for ports", "Something went wrong!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

            else
            {
                /* User pressed cancel button. */
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            code.serialport.SerialPort_Close();

            this.Close();
        }

        private void mainwindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            code.serialport.SerialPort_Close();
        }

        private void flashMCUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            windows.Flash NewFlashWindow = new windows.Flash();

            if (NewFlashWindow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }

            else
            {
                /* To be implemented. */
            }
        }

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            windows.console NewTerminalWindow = new windows.console();

            if (NewTerminalWindow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }

            else
            {
                /* To be implemented. */
            }
        }

        private void texterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            windows.texter NewTexter = new windows.texter();

            if (NewTexter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }

            else
            {
                /* To be implemented. */
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "open")
            {
                if (code.serialport.SerialPort_IsConfigured())
                {
                    code.serialport.SerialPort_Open();

                    button1.Text = "close";
                }

                else
                {
                    MessageBox.Show("Serial port is not configured!","Nofification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            else
            {
                code.serialport.SerialPort_Close();

                button1.Text = "open";
            }
        }
    }
}
