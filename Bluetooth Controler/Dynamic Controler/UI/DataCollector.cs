using System;
using Android.Content;
using Android.Widget;
using Android.Bluetooth;
using System.Threading.Tasks;

namespace Dynamic_Controler
{
    public class DataCollector
    {
        Context _Context;
        ConvertData mConverterData;
        public DataCollector (Context _context)
        {
            _Context = _context;
        }
        public async void AutoGetData(BluetoothSocket socket)
        {
            Task data = Task.Factory.StartNew(() =>
            {
                ReceiveData2(socket);
            });
            await data;
        }
        public void showToast(string str)
        {
            Toast.MakeText(_Context, str, ToastLength.Short).Show();
        }
        public void ReceiveData2(BluetoothSocket socket)
        {
            byte[] resultArray = new byte[14];
            resultArray[0] = 126;
            resultArray[resultArray.Length - 1] = 03;
            byte[] temp = new byte[30];
            float result = 0.0f;
            try
            {
                socket.InputStream.Read(temp, 0, temp.Length);
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i] == 126)
                    {
                        System.Array.Copy(temp, i + 1, resultArray, 1, 12);
                        result = mConverterData.ConvertHexToFloat(mConverterData.ConvertByteToHex(resultArray, 0));
                        MainActivity.txtHumidity.Text = Math.Round(result, 2).ToString();

                        result = mConverterData.ConvertHexToFloat(mConverterData.ConvertByteToHex(resultArray, 1));
                        MainActivity.txtFlow.Text = Math.Round(result, 2).ToString();

                        result = mConverterData.ConvertHexToFloat(mConverterData.ConvertByteToHex(resultArray, 2));
                        MainActivity.txtTemperature.Text = Math.Round(result, 2).ToString();
                        break;
                    }
                }
            }
            catch
            {
                showToast("Can't read mesage. Please check your bluetooth.");
            }

        }


       /* public string ReceiveData(BluetoothSocket socket)
        {
            string result = "";
            try
            {
                if (MainActivity.bluetoothAdapter.IsEnabled)
                {
                    float data = (float)socket.InputStream.ReadByte();
                    result = data.ToString() + "%";
                }
                else
                {
                    result = "Bluetooth is disable";
                }
            }
            catch
            {
                result = "";
            }


            return result;
        } */
    }
}