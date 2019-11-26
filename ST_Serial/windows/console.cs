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
    enum Log
    {
        INFO,
        WARNING,
        ERROR,
        NONE,
        MAX
    }

    class LogValue
    {
        public int Value;
        public string Data;

        public LogValue(int Val = 0, string Dat = "None")
        {
            Value = Val;
            Data = Dat;
        }
    };

    public partial class console : Form
    {
        private static Queue<LogValue> ConsoleData = new Queue<LogValue>();

        public console()
        {
            InitializeComponent();

            code.serialport.SerialPort_Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* Clear button */
            richTextBox1.Text = "";
            ConsoleData.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /* Close button */
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            while (ConsoleData.Count > 0)
            {
                LogValue Data = ConsoleData.Dequeue();
                richTextBox1.SelectionColor = GetColor(Data.Value);
                richTextBox1.SelectedText += Environment.NewLine;
            }
        }

        Color GetColor(int LogLevel = 0)
        {
            Color textcolor = new Color();

            switch (LogLevel)
            {
                case (int)Log.INFO:
                    textcolor = Color.Green;
                    break;

                case (int)Log.WARNING:
                    textcolor = Color.Blue;
                    break;

                case (int)Log.ERROR:
                    textcolor = Color.Red;
                    break;

                default:
                    textcolor = Color.Black;
                    break;
            }

            return textcolor;
        }

        /* Log Format - [Date] - [Time] - [LogLevel]*/

        private static string GetCurrentTimeDate()
        {
            return "[" + DateTime.UtcNow.Date.ToString("dd/MM/yyyy") + "] - " + "[" + DateTime.UtcNow.Date.ToString("dd/MM/yyyy") + "] - ";
        }
        public static void Console_Log_Info(string ConsoleString)
        {
            ConsoleData.Enqueue(new LogValue((int)Log.INFO, GetCurrentTimeDate() + "[INFO] - "+ ConsoleString));
        }

        public static void Console_Log_Warning(string ConsoleString)
        {
            ConsoleData.Enqueue(new LogValue((int)Log.WARNING, GetCurrentTimeDate() + "[WARNING] - " + ConsoleString));
        }

        public static void Console_Log_Error(string ConsoleString)
        {
            ConsoleData.Enqueue(new LogValue((int)Log.ERROR, GetCurrentTimeDate() + "[ERROR] - " + ConsoleString));
        }
    }
}
