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

namespace ST_Serial.windows
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

                FileStream NewStream = new FileStream(openFileDialog1.FileName, FileMode.Open);

                code.files.Parse_HexStream(NewStream);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* Write button */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /* Cancel button */
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
