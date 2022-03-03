using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_tool.code
{
    class lib
    {
        public static List<byte> ConvertStringAToByteA(string[] Data)
        {
            List<byte> ConvertedData = new List<byte>();

            foreach (string i in Data)
            {
                if (i != "")
                {
                    ConvertedData.Add(ConvertStringToByte(i.ToUpper()));
                }
            }

            return ConvertedData;
        }

        public static byte ConvertStringToByte(string Data)
        {
            Int32 ConvertedData = 0x00;

            if (Data.Length == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    if ((Data[i] >= 'A') && (Data[i] <= 'F'))
                    {
                        ConvertedData |= (Int32)((Data[i] - 0x37) << (i * 4));
                    }

                    else if ((Data[i] >= '0') && (Data[i] <= '9'))
                    {
                        ConvertedData |= (Int32)((Data[i] - 0x30) << (i * 4));
                    }

                    else
                    {
                        ConvertedData |= (Int32)(0 << (i * 4));
                    }
                }

                ConvertedData = (ConvertedData >> 4) | (ConvertedData << 4);
            }

            return (byte)(ConvertedData & (0xFF));
        }
    }
}
