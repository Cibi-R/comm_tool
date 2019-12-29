using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST_Serial.code
{
    class files
    {
        public static void Parse_HexStream(FileStream NewStream)
        {
            /* To store a single record while sending records to the MCU */
            List<byte> SingleRecord = new List<byte>();

            BinaryReader NewBReader = new BinaryReader(NewStream);

            bool RecordStart = false, EndReading = false;

            string Byte = null;

            int RawValue;

            /*************************   Code Coupled with Serialport class  *****************************/

            if (code.serialport.CurrentSerialAPP == (int)Application.APP_NONE)
            {
                code.serialport.CurrentSerialAPP = (int)Application.APP_FLASH;
            }

            else
            {
                MessageBox.Show("Cannot use Serial port righ now!,[" + code.serialport.CurrentSerialAPP.ToString() +
                    "] using the serial port", "Serial Port Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                /* Return incase if other applications using the serial port */
                return;
            }

            /*********************************************************************************************/

            try
            {
                while (((RawValue = NewBReader.ReadChar()) != -1) && !EndReading)
                {
                    if ((char)RawValue == ':')
                    {
                        RecordStart = true;

                        SingleRecord.Add(0X7B);   /*< Add Start of Text */
                    }

                    else if (((char)RawValue == '\r') || ((char)RawValue == '\n'))
                    {
                        if (RecordStart)
                        {
                            SingleRecord.Add(0X7D);  /*< Add End of Record. */

                            bool IsRecordSent = code.serialport.SerialPort_WriteByteArray(SingleRecord.ToArray());

                            System.Threading.Thread.Sleep(50);

                            if (!Check_Host_Response(code.serialport.SerialPort_ReadByte()))
                            {
                                bool Val = false;
                            }

                            SingleRecord = new List<byte>();
                        }

                        RecordStart = false;
                    }

                    else if (RecordStart)
                    {
                        Byte += ((char)RawValue).ToString();

                        if (Byte.Length == 2)
                        {
                            byte Val = code.lib.ConvertStringToByte(Byte);

                            if ((Val == 0x7B) || (Val == 0x7D) || (Val == 0x05))
                            {
                                /* Insert Escape character */
                                SingleRecord.Add(0x05);
                            }

                            SingleRecord.Add(Val);

                            Byte = null;
                        }
                    }

                    else
                    {
                        MessageBox.Show("Logical error!");

                        /* End reading incase of error */
                        EndReading = true;
                    }
                }
            }

            catch
            {
                MessageBox.Show("Something went wrong while parsing Hex!","Parse Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

            /* Reset current application using the serail port. */
            code.serialport.CurrentSerialAPP = (int)Application.APP_NONE;
        }

        private static bool Check_Host_Response(byte[] Data)
        {
            bool RetVal = true;
            byte[] Response = new byte[] { 0X7B,0XA0,0X02,0X00,0X00,0X7D};

            if (Data.Length == 6)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (Data[i] != Response[i])
                    {
                        RetVal = false;
                        break;
                    }
                }
            }

            else
            {
                RetVal = false;
            }

            return RetVal;
        }
    }
}
