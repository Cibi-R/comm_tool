using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST_Serial.code
{
    class serialport
    {
        private static SerialPort MySerialPort = new SerialPort();

        public static void SerialPort_Configure(SerialPort PortData)
        {
            MySerialPort.PortName = PortData.PortName;
            MySerialPort.BaudRate = PortData.BaudRate;
            MySerialPort.StopBits = PortData.StopBits;
            MySerialPort.Parity = PortData.Parity;
        }

        public static bool SerialPort_Open()
        {
            bool PortOpened = true;

            try
            {
                MySerialPort.Open();
            }
            catch
            {
                PortOpened = false;
            }

            return PortOpened;
        }

        public static bool SerialPort_Close()
        {
            bool PortClosed = true;

            try
            {
                MySerialPort.Close();
            }
            catch
            {
                PortClosed = false;
            }

            return PortClosed;
        }

        private static bool IsPortOpen()
        {
            return MySerialPort.IsOpen;
        }
    }
}
