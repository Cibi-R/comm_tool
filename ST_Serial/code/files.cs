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
        /*< message control characters */
        const byte STX = 0XFD;
        const byte ETX = 0XFE;
        const byte DTX = 0XFF;

        /*< message IDs */
        const byte ID_REFLASH_INFO = 0X04;
        const byte ID_REPROGRAMMING = 0X05;

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
                    }

                    else if (((char)rawValue == '\r') || ((char)rawValue == '\n'))
                    {
                        if (recordStart)
                        {
                            /*< a complete record has been accumlated, make the record as a transmitable messsage frame */
                            hexRecordList.Add(HexRecord_MakeFrame(hexRecord));

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

        private static List<byte> HexRecord_MakeFrame(List<byte> record)
        {
            byte checksum;

            /*< checksum should be calculated with id value */
            record.Insert(0, ID_REPROGRAMMING);

            /*< calculate checksum of the record */
            checksum = code.lib.Calculate_8bit_Checksum(record);

            /*< add checksum value, a complete frame has been framed. */
            record.Add(checksum);

            for (int i = 0; i < record.Count; i++)
            {
                if ((record[i] == STX) || (record[i] == ETX) || (record[i] == DTX))
                {
                    record.Insert(i, DTX);

                    /*< move the index to next position, as the index value will be increased while inserting an element */
                    i++;
                }
            }

            /*< add start and end of frame */
            record.Insert(0, STX);
            record.Add(ETX);

            return record;
        }

        private static List<byte> HexRecord_AddSize(int count)
        {
            List<byte> sizeFrame = new List<byte>();

            /*< add message id */
            sizeFrame.Add(ID_REFLASH_INFO);

            /*< add data bytes */
            sizeFrame.Add((byte)(count & 0XFF));
            sizeFrame.Add((byte)((count >> 8) & 0XFF));

            /*< add checksum */
            sizeFrame.Add(code.lib.Calculate_8bit_Checksum(sizeFrame));

            /*< add control characters */
            for (int i = 0; i < sizeFrame.Count; i++)
            {
                if ((sizeFrame[i] == STX) || (sizeFrame[i] == ETX) || (sizeFrame[i] == DTX))
                {
                    sizeFrame.Insert(i, DTX);

                    /*< move the index to next position, because the list count will be increased while inserting an element */
                    i++;
                }
            }

            /*< add start and end of frame */
            sizeFrame.Insert(0, STX);
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
