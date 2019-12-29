using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST_Serial.code
{
    enum RecordState_EN
    {
        HEX_RECORD_IDLE,
        HEX_RECORD_PROCESSING,
        HEX_RECORD_PROCESSED,
        HEX_RECORD_CORRUPTED,
        HEX_RECORD_MAX_STATES,
    }

    class COM_Msg
    {
        public byte Id = 0x00;
        public byte DLC = 0x00;
        public byte[] Data = new byte[5];
    }
    class com_ser
    {
        const byte STX = 0X7B, ETX = 0X7D, DLE = 0X05;
        static bool Delimeter_Received = false;
        static RecordState_EN RecordState = RecordState_EN.HEX_RECORD_IDLE;
        static int FrameIndex = 0;


        static private COM_Msg CurrentFrame = new COM_Msg();

        static private bool Evaluate_Checksum(byte[] Val, int Len)
        {
            bool RetVal = false;

            return RetVal;
        }

        static private void COM_Frame_Parser()
        {
            if (Evaluate_Checksum(CurrentFrame.Data,CurrentFrame.DLC))
            {

            }
            else
            {

            }
        }
        static private void COM_Isr(byte Byte)
        {
            if ((Byte == STX) && (Delimeter_Received != true))
            {
                RecordState = RecordState_EN.HEX_RECORD_PROCESSING;
            }

            else if ((Byte == ETX) && (Delimeter_Received != true))
            {
                COM_Frame_Parser();
            }

            else
            {
                if (RecordState == RecordState_EN.HEX_RECORD_PROCESSING)
                {
                    if ((Byte == DLE) && (Delimeter_Received != true))
                    {
                        Delimeter_Received = true;

                        return;
                    }

                    if (CurrentFrame.Id == 0)
                    {
                        CurrentFrame.Id = Byte;
                    }

                    else if (CurrentFrame.DLC == 0)
                    {
                        CurrentFrame.DLC = Byte;
                    }

                    else
                    {
                        CurrentFrame.Data[FrameIndex] = Byte;
                        FrameIndex++;
                    }

                    if (Delimeter_Received == true)
                    {
                        Delimeter_Received = false;
                    }
                }
            }
        }

        static public void COM_Main(byte Data)
        {
            COM_Isr(Data);
        }

        static public COM_Msg COM_Get_Frame()
        {
            byte[] NewFrame = code.serialport.SerialPort_ReadByte();

            foreach (byte Data in NewFrame)
            {
                COM_Isr(Data);
            }

            return CurrentFrame;
        }

        static public List<COM_Msg> COM_Get_Frames()
        {
            List<COM_Msg> ReceivedFrames = null;

            return ReceivedFrames;
        }

        static public void COM_Send_Frame(COM_Msg Message)
        {

        }
    }
}
