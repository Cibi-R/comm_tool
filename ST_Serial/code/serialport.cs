using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_tool.code
{
    public enum Application
    {
        /**********************************************************************************************************
                Application specific enum values to gaurd the serial port from multiple application accessing it.
        ***********************************************************************************************************/
        APP_NONE,
        APP_CONSOLE,
        APP_FLASH,
        APP_TEXTER,
        APP_UNKNOWN,
        APP_MAX,
    };
    class serialport
    {
        public static int CurrentSerialAPP = (int)Application.APP_NONE;

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

            if (!IsPortOpen())
            {
                try
                {
                    MySerialPort.Open();
                }
                catch
                {
                    PortOpened = false;
                }

            }

            return PortOpened;
        }

        public static bool SerialPort_Close()
        {
            bool PortClosed = true;

            if (IsPortOpen())
            {
                try
                {
                    MySerialPort.Close();
                }
                catch
                {
                    PortClosed = false;
                }
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

            if (IsPortOpen())
            {
                try
                {
                    MySerialPort.Write(Data);
                }
                catch
                {
                    IsDataSent = false;
                }
            }
            else { IsDataSent = false; }
            return IsDataSent;
        }

        public static bool SerialPort_WriteByteArray(byte[] Data)
        {
            bool IsDataSent = true;

            if (IsPortOpen())
            {
                try
                {
                    MySerialPort.Write(Data, 0, Data.Length);
                }
                catch
                {
                    IsDataSent = false;
                }
            } else { IsDataSent = false; }

            return IsDataSent;
        }

        public static byte[] SerialPort_ReadByte()
        {
            if (IsPortOpen())
            {
                try
                {
                    int ByteCount = MySerialPort.BytesToRead;

                    byte[] ReceivedBytes = new byte[ByteCount];

                    MySerialPort.Read(ReceivedBytes, 0, ByteCount);

                    // MySerialPort.DiscardInBuffer();

                    return ReceivedBytes;
                }

                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static string SerialPort_ReadString()
        {
            string ReceivedString = null;

            if (IsPortOpen())
            {
                try
                {
                    ReceivedString = MySerialPort.ReadExisting();
                }

                catch
                {
                    /* Do nothing */
                }
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
