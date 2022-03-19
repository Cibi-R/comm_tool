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
                foreach(List<byte> record in HexFileList)
                {
                    if (!code.serialport.SerialPort_WriteByteArray(record.ToArray()))
                    {
                        richTextBox1.Text += "> hex record send failed" + Environment.NewLine;
                    }

                    System.Threading.Thread.Sleep(100);
                }
            }
        }
    }
}
