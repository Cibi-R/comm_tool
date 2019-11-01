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

        private static bool IsSerialPortConfigured = false;

        public static void SerialPort_Configure(SerialPort PortData)
        {
            MySerialPort.PortName = PortData.PortName;
            MySerialPort.BaudRate = PortData.BaudRate;
            MySerialPort.StopBits = PortData.StopBits;
            MySerialPort.Parity = PortData.Parity;

            IsSerialPortConfigured = true;
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

        public static bool IsPortOpen()
        {
            return MySerialPort.IsOpen;
        }

        public static bool SerialPort_WriteString(string Data)
        {
            bool IsDataSent = true;
            try
            {
                MySerialPort.Write(Data);
            }
            catch
            {
                IsDataSent = false;
            }
            return IsDataSent;
        }

        public static bool SerialPort_WriteByteArray(byte[] Data)
        {
            bool IsDataSent = true;
            try
            {
                MySerialPort.Write(Data, 0, Data.Length);
            }
            catch
            {
                IsDataSent = false;
            }
            return IsDataSent;
        }

        public static byte[] SerialPort_ReadByte()
        {
            try
            {
                int ByteCount = MySerialPort.BytesToRead;

                byte[] ReceivedBytes = new byte[ByteCount];

                MySerialPort.Read(ReceivedBytes, 0, ByteCount);

                return ReceivedBytes;
            }

            catch
            {
                return null;
            }
        }

        public static string SerialPort_ReadString()
        {
            string ReceivedString = null;

            try
            {
                ReceivedString = MySerialPort.ReadExisting();
            }

            catch
            {
                /* Do nothing */
            }
            return ReceivedString;
        }

        public static bool SerialPort_IsConfigured()
        {
            return IsSerialPortConfigured;
        }

        public static int SerialPort_AvailBytes()
        {
            return MySerialPort.BytesToRead;
        }
    }
}
