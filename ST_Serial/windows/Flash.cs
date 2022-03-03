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
        public Flash()
        {
            InitializeComponent();

            Init_FlashWindow_UI();
        }

        private void Init_FlashWindow_UI()
        {
            label1.Text = "";

            /* openFile dialogue initialization:
             * 
             * Set Defualt Extension(DefaultExt) to hex
             */
            openFileDialog1.DefaultExt = "hex";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /* Browse button */
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.SafeFileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* Write button */
            if (openFileDialog1.FileName != "")
            {
                try
                {
                    FileStream NewStream = new FileStream(openFileDialog1.FileName, FileMode.Open);
                    code.files.Parse_HexStream(NewStream);
                    NewStream.Dispose();
                }
                catch
                {
                    MessageBox.Show("Something went wrong while accessing file\nInvalid path or file","File Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }

            else
            {
                MessageBox.Show("File not selected!", "Flash Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /* Cancel button */
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
