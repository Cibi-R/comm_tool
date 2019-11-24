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
            BinaryReader NewBReader = new BinaryReader(NewStream);

            bool RecordStart = false;
            string Byte = null;
            int RawValue;
            byte HexValue;

            while ((RawValue = NewBReader.ReadChar()) != -1)
            {
                if ((char)RawValue == ':')
                {
                    RecordStart = true;
                }

                else if (((char)RawValue == '\r') || ((char)RawValue == '\n'))
                {
                    RecordStart = false;
                }

                else if (RecordStart)
                {
                    Byte += ((char)RawValue).ToString();

                    if (Byte.Length == 2)
                    {
                        MessageBox.Show(Byte);

                        HexValue = code.lib.ConvertStringToByte(Byte);

                        Byte = null;
                    }
                }
            }
        }
    }
}
