using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST_Serial.code
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
            byte ConvertedData = 0x00;

            if (Data.Length == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    if ((Data[i] >= 'A') && (Data[i] <= 'F'))
                    {
                        ConvertedData |= (byte)((Data[i] - 0x37) << (i * 4));
                    }

                    else if ((Data[i] >= '0') && (Data[i] <= '9'))
                    {
                        ConvertedData |= (byte)((Data[i] - 0x30) << (i * 4));
                    }

                    else
                    {
                        ConvertedData |= (byte)(0 << (i * 4));
                    }
                }
            }

            return ConvertedData;
        }
    }
}
