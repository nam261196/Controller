using System;
using System.Text;

namespace Dynamic_Controler
{
    /// <summary>
    /// Convert Data Byte to Hex, Hex to Float
    /// </summary>
    public class ConvertData
    {
        public ConvertData()
        {

        }
        public string ConvertByteToHex(byte[] strConvert, int line)
        {
            //line = 0 is read 4 byte first, line = 1 is read 4 byte continue....
            StringBuilder hex = new StringBuilder();
            switch (line)
            {
                case 0:
                    for (int i = 1; i < strConvert.Length; i++)
                    {
                        if (i < 5)
                        {
                            hex.AppendFormat("{0:x2}", strConvert[i]);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                case 1:
                    for (int i = 5; i < strConvert.Length; i++)
                    {
                        if (i < 9)
                        {
                            hex.AppendFormat("{0:x2}", strConvert[i]);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;

                //case 2:
                //    for (int i = 9; i < strConvert.Length; i++)
                //    {
                //        if (i < 13)
                //        {
                //            hex.AppendFormat("{0:x2}", strConvert[i]);
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //    break;
                default:
                    //showToast("Không thể nhận dữ liệu");
                    break;
            }


            return hex.ToString();
        }


        public float ConvertHexToFloat(string hexString)
        {
            string reverseString = "";
            reverseString = hexString.Substring(6, 2) + hexString.Substring(4, 2) + hexString.Substring(2, 2) + hexString.Substring(0, 2);
            uint num = uint.Parse(reverseString, System.Globalization.NumberStyles.AllowHexSpecifier);
            byte[] floatVals = BitConverter.GetBytes(num);
            float f = BitConverter.ToSingle(floatVals, 0);
            return f;
        }
    }
}