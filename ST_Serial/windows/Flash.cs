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
    public partial class Flash : Form
    {
        public Flash(string ButtonName)
        {
            /* The buttonName will have the name of the button which caused this window open */
            this.Name = "Flash - " + ButtonName;

            InitializeComponent();
        }
    }
}
