using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST_Serial
{
    public partial class mainwindow : Form
    {
        public mainwindow()
        {
            InitializeComponent();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            windows.settings NewSettingWindow = new windows.settings();

            if (NewSettingWindow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
            }

            else
            {
                
            }
        }
    }
}
