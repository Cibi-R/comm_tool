using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_tool.code
{
    class files
    {
        const byte STX = 0XFD;
        const byte ETX = 0XFE;
        const byte DTX = 0XFF;

        public static List<List<byte>> Parse_HexStream(FileStream newFileStream)
        {
            /*< to store a single record */
            List<byte> hexRecord = new List<byte>();

            /*< to the entire hex file (collection of hex records) */
            List<List<byte>> hexRecordList = new List<List<byte>>();

            BinaryReader newBinaryReader = new BinaryReader(newFileStream);

            bool recordStart = false;
            bool endReading = false;

            string stringByte = null;

            char rawValue;

            try
            {
                while ((newBinaryReader.PeekChar() != -1) && !endReading)
                {
                    rawValue = newBinaryReader.ReadChar();

                    if (rawValue == ':')
                    {
                        recordStart = true;

                        /*< insert start of the packet */
                        hexRecord.Add(STX);
                    }

                    else if (((char)rawValue == '\r') || ((char)rawValue == '\n'))
                    {
                        if (recordStart)
                        {
                            /*< insert end of the packet */
                            hexRecord.Add(ETX);

                            hexRecordList.Add(hexRecord);

                            hexRecord = new List<byte>();

                            recordStart = false;
                        }
                    }

                    else if (recordStart)
                    {
                        stringByte += rawValue.ToString();

                        if (2 == stringByte.Length)
                        {
                            byte acculatedByte = code.lib.ConvertStringToByte(stringByte);

                            if ((STX == acculatedByte) || (ETX == acculatedByte) || (DTX == acculatedByte))
                            {
                                /* Insert Escape character */
                                hexRecord.Add(DTX);
                            }

                            hexRecord.Add(acculatedByte);

                            stringByte = null;
                        }
                    }

                    else
                    {
                        /*< byte allignment in the file is not proper */
                        MessageBox.Show("invalid file!");

                        endReading = true;
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            newBinaryReader.Dispose();

            hexRecordList.Insert(0, HexRecord_AddSize(hexRecordList.Count));

            return hexRecordList;
        }

        private static List<byte> HexRecord_AddSize(int count)
        {
            List<byte> sizeFrame = new List<byte>();

            byte b1 = (byte)(count & 0XFF);
            byte b2 = (byte)((count >> 8) & 0XFF);

            /*< start of frame */
            sizeFrame.Add(STX);

            /*< data bytes */
            if ((b1 == STX) || (b1 == ETX) | (b1 == DTX))
            {
                sizeFrame.Add(DTX);
            }
            sizeFrame.Add(b1);

            if ((b2 == STX) || (b2 == ETX) | (b2 == DTX))
            {
                sizeFrame.Add(DTX);
            }
            sizeFrame.Add(b2);

            /*< end of frame */
            sizeFrame.Add(ETX);

            return sizeFrame;
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
