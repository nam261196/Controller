using System;
using Android.Content;
using Android.Widget;
using Android.Bluetooth;
using Android.App;

namespace Dynamic_Controler
{
    /// <summary>
    /// Auto get data after bonded with a device
    /// </summary>
    public class DataHandler
    {
        Context _Context;
        ConvertData mConverterData = new ConvertData();
        Activity _Activity;
        public DataHandler(Activity activity)
        {
            _Activity = activity;
        }
        public void SendDataTo(byte[] signal)
        {
            MainActivity.mSocket.OutputStream.Write(signal, 0, signal.Length);
        }

        //public DataCollector(Context _context)
        //{
        //    _Context = _context;
        //}

        public void AutoGetData(BluetoothSocket socket)
        {

            _Activity.RunOnUiThread(() =>
            ReceiveData2(socket)
            );
        }
        public void showToast(string str)
        {
            Toast.MakeText(_Context, str, ToastLength.Short).Show();
        }
        public void ReceiveData2(BluetoothSocket socket)
        {
            byte[] resultArray = new byte[10];
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
                        System.Array.Copy(temp, i + 1, resultArray, 1, 8);
                        result = mConverterData.ConvertHexToFloat(mConverterData.ConvertByteToHex(resultArray, 0));
                        MainActivity.txtHumidity.Text = Math.Round(result, 2).ToString();

                        result = mConverterData.ConvertHexToFloat(mConverterData.ConvertByteToHex(resultArray, 1));
                        MainActivity.txtFlow.Text = Math.Round(result, 2).ToString();

                        break;
                    }
                }
            }
            catch
            {
                showToast("Can't read mesage. Please check your bluetooth.");
            }

        }


    }
}