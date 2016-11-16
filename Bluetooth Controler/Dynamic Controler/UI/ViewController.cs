using System;
using Android.App;
using Android.Widget;
using Android.Content;

namespace Dynamic_Controler
{
    public class ViewController
    {
        BluetoothListAdapter _BluetoothListAdapter;
        //MainActivity _MainActivity = new MainActivity();
        Activity _Context;
        BluetoothHandler _BluetoothHandler = new BluetoothHandler();
        
        public ViewController(Activity context)
        {
            _Context = context;
        }

        public void showToast(string str)
        {
            Toast.MakeText(_Context, str, ToastLength.Short).Show();
        }
        public void SendDataTo(byte[] signal)
        {
            MainActivity.mSocket.OutputStream.Write(signal, 0, signal.Length);
        }
        public void Init()
        {
            MainActivity.btnRefresh = (ImageView)_Context.FindViewById(Resource.Id.btnrefresh);
            MainActivity.btnScanDevice = (Button)_Context.FindViewById(Resource.Id.btnscandevice);
            MainActivity.btnController = (ImageButton)_Context.FindViewById(Resource.Id.btncontroller);

            MainActivity.txtFlow = (TextView)_Context.FindViewById(Resource.Id.flowresult);
            MainActivity.txtHumidity = (TextView)_Context.FindViewById(Resource.Id.humidityresult);
            MainActivity.txtTemperature = (TextView)_Context.FindViewById(Resource.Id.temperatureresult);
            MainActivity.txtTime = (TextView)_Context.FindViewById(Resource.Id.timeresult);
            MainActivity.txtToday = (TextView)_Context.FindViewById(Resource.Id.todayresult);

            DateTime today = DateTime.Now;
            MainActivity.txtToday.Text = (today.Day + "/" + today.Month + "/" + today.Year);
        }

        public void Controller()
        {
            _BluetoothListAdapter = new BluetoothListAdapter(Application.Context);
            MainActivity.btnController.Click += delegate
            {

                //bluetooth is enable & have bond a device
                if ((MainActivity.bluetoothAdapter.IsEnabled) && (MainActivity.mBondedDevice != null))
                {
                    if (MainActivity.mFlagStatusPump)//Pump is On
                    {
                        if (MainActivity.mSocket.IsConnected)
                        {
                            SendDataTo(MainActivity.OffPumpASCII);
                            MainActivity.mFlagStatusPump = false;
                            MainActivity.btnController.SetImageResource(Resource.Drawable.Off_Enable);
                        }
                        else
                        {
                            showToast("Please scan bluetooth device and bond to a device.");
                        }
                    }
                    else//Pump is Off
                    {
                        if (MainActivity.mSocket.IsConnected)
                        {
                            SendDataTo(MainActivity.OnPumpASCII);
                            MainActivity.mFlagStatusPump = true;
                            MainActivity.btnController.SetImageResource(Resource.Drawable.On_Enable);
                        }
                        else
                        {
                            showToast("Please scan bluetooth device and bond to a device.");
                        }
                    }
                }
                else//bluetooth is disable & not have bond a device
                {
                    if (!MainActivity.bluetoothAdapter.IsEnabled)
                    {
                        showToast("Bluetooth is disable. Please enable bluetooth");
                        _BluetoothHandler.CheckBlueToothState();
                    }

                    if (MainActivity.mBondedDevice == null)
                    {
                        showToast("Not bonded to any device");
                    }

                    MainActivity.mFlagStatusPump = false;
                    MainActivity.btnController.SetImageResource(Resource.Drawable.Off_Enable);
                }
            };

            MainActivity.btnRefresh.Click += delegate
            {
                MainActivity.btnRefresh.StartAnimation(MainActivity.AminationRefresh);
                _BluetoothHandler.Initialize();
                showToast("Refresh Complete!!");
            };

            MainActivity.btnScanDevice.Click += delegate
            {
                
                //if (MainActivity.mListDeviceCurrent.Count == 0 && MainActivity.mListDeviceOld.Count == 0)
                //{
                //    showToast("No device bluetooth around here");
                //}
                //else
                //{
                    MainActivity._Activity.StartActivity(typeof(ListDeviceActivity));
                    //if (MainActivity.mFlagRefresh == true)//Refresh list device
                    //{
                    //    if (MainActivity.mListDeviceOld.Count == 0)
                    //    {
                    //        MainActivity.mListDeviceOld.AddRange(MainActivity.mListDeviceCurrent);
                    //    }
                    //    else
                    //    {
                    //        _BluetoothListAdapter.SaveToListOld();
                    //        _BluetoothListAdapter.DeleteDeviceDisable();
                    //    }
                    //    _BluetoothListAdapter.ShowListCurrent();
                    //    MainActivity.mFlagRefresh = false;
                    //}
                    //else
                    //{
                    //    _BluetoothListAdapter.ShowListOld();
                    //}

                //}
            };
        }
    }
}