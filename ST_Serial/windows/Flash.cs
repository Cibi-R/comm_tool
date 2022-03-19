using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace comm_tool.windows
{
    public partial class Flash : Form
    {
        /*< collection of hex records are stored in this queue */
        private List<List<byte>> HexFileList = new List<List<byte>>();

        public Flash()
        {
            InitializeComponent();

            InitializeFlashWindow_UI();
        }

        private void InitializeFlashWindow_UI()
        {
            /* openFile dialogue initialization:
             * 
             * Set Defualt Extension(DefaultExt) to hex
             */
            openFileDialog1.DefaultExt = "hex";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* Write button */
            if (!string.IsNullOrEmpty(openFileDialog1.FileName))
            {
                try
                {
                    FileStream newFileStream = new FileStream(openFileDialog1.FileName, FileMode.Open);
                    HexFileList = code.files.Parse_HexStream(newFileStream);
                    newFileStream.Dispose();
                    Flash_Programming();
                }
                catch
                {
                    richTextBox1.Text += "> invalid path or file" + Environment.NewLine;
                }
            }

            else
            {
                richTextBox1.Text += "> file not selected" + Environment.NewLine;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /* Cancel button */
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /* Browse button */
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.SafeFileName;
            }
        }

        private void Flash_Programming()
        {
            if (0 != HexFileList.Count)
            {
                if (checkBox1.Checked)
                {
                    /*< if jump to application check box is checked, add a frame at the end */
                    List<byte> frame = new List<byte>()
                    {
                        0XFD, 0X06, 0XFA, 0XFE
                    };

                    HexFileList.Add(frame);
                }

                richTextBox1.Text += "> flashing started..." + Environment.NewLine;

                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            bool response_received = false;

            foreach (List<byte> record in HexFileList)
            {
                response_received = false;

                if (!code.serialport.SerialPort_WriteByteArray(record.ToArray()))
                {
                    /*< transmitting frame failed */
                    break;
                }
                else
                {
                    uint timeout = 0XFFFF;

                    while ((0 != timeout) && (!response_received))
                    {
                        if (3 < code.serialport.SerialPort_AvailBytes())
                        {
                            if (code.serialport.SerialPort_ReadByte()[1] == 0X01)
                            {
                                response_received = true;
                            }
                            else
                            {
                                timeout = 0;
                            }
                        }

                        timeout--;
                    }
                }

                if (!response_received)
                { 
                    e.Cancel = true;

                    break;
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                richTextBox1.Text += "> reflashing failed due to nack/no response" + Environment.NewLine;
            }
            else
            {
                richTextBox1.Text += "> reflashing succefully completed..." + Environment.NewLine;
            }
        }
    }
}
