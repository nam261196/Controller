using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using System.Threading.Tasks;
using System.Threading;

namespace Dynamic_Controler
{
    class BluetoothHandler
    {
        public BluetoothHandler ()
        {

        }

        /// <summary>
        /// Bonded a device when choose from menu device.
        /// </summary>
        /// <param name="listCurrent"></param>
        /// <param name="item"></param>
        public void showToast(string str)
        {
            Toast.MakeText(Application.Context, str, ToastLength.Short).Show();
        }
        public void SendDataTo(byte[] signal)
        {
            MainActivity.mSocket.OutputStream.Write(signal, 0, signal.Length);
        }

        public void CheckBlueToothState()
        {
            if (MainActivity.bluetoothAdapter == null)
            {
                showToast("Bluetooth NOT support");
                MainActivity.btnScanDevice.Enabled = false;
            }
            else
            {
                if (MainActivity.bluetoothAdapter.IsEnabled)
                {
                    if (MainActivity.bluetoothAdapter.IsDiscovering)
                    {
                        showToast("Bluetooth is currently in device discovery process.");
                        MainActivity.btnScanDevice.Enabled = true;
                    }
                    else
                    {
                        showToast("Bluetooth is Enabled.");
                        MainActivity.btnScanDevice.Enabled = true;
                    }
                }
                else
                {
                    showToast("Bluetooth is NOT Enabled!");
                    MainActivity.bluetoothAdapter.Enable();
                    

                }
            }

        }

        public async void BondedDevice(List<BluetoothDevice> listCurrent, IMenuItem item)
        {
            //bond

            Task Bonded = Task.Factory.StartNew(() =>
            { 
                for (int i = 0; 0 < listCurrent.Count; i++)
                {
                    if (listCurrent[i].Name == item.ToString())
                    {
                        if (listCurrent[i].BondState != Bond.Bonded)
                        {

                            {
                                BluetoothSocket tmp = null;
                                Timer t = null;
                                try
                                {
                                    tmp = listCurrent[i].CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
                                    MainActivity.bluetoothAdapter.CancelDiscovery();
                                    try
                                    {
                                        tmp.Connect();
                                        t = new Timer(x => MainActivity._DataCollector.AutoGetData(tmp), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
                                        MainActivity.mBondedDevice = listCurrent[i];
                                        MainActivity.mTimer = t;
                                        MainActivity.mSocket = tmp;

                                    }
                                    catch
                                    {
                                        try
                                        {
                                            tmp.Close();
                                            t.Dispose();
                                            showToast("Socket error. Socket is closed");
                                        }
                                        catch { }
                                    }
                                }
                                catch
                                {
                                    try
                                    {
                                        tmp.Close();
                                        t.Dispose();
                                        showToast("Socket error. Socket is closed");
                                    }
                                    catch { }
                                }


                            }

                            break;
                        }
                    }
                }
            });
            await Bonded;
        }
        /// <summary>
        /// Show list device is found.
        /// </summary>
        public void Initialize()
        {

            if (MainActivity.mSocket != null)
            {
                try
                {
                    if (MainActivity.mFlagStatusPump == true)//is ON
                    {
                        try
                        {
                            MainActivity.mSocket.Connect();
                            SendDataTo(MainActivity.OffPumpASCII);
                            MainActivity.mSocket.Close();
                            MainActivity.mFlagStatusPump = false;
                            MainActivity.btnController.SetImageResource(Resource.Drawable.Off_Enable);
                            showToast("Pump is closed");
                        }
                        catch
                        {
                            showToast("Can't change status's pump");
                        }
                    }
                    else//is OFF
                    {
                        try
                        {
                            if (MainActivity.bluetoothAdapter.IsDiscovering)
                            {
                                MainActivity.bluetoothAdapter.CancelDiscovery();
                            }
                            MainActivity.mSocket.Connect();
                            SendDataTo(MainActivity.OnPumpASCII);
                            MainActivity.mSocket.Close();
                            MainActivity.mFlagStatusPump = true;
                            MainActivity.btnController.SetImageResource(Resource.Drawable.On_Enable);
                            showToast("Pump is open");
                        }
                        catch
                        {
                            showToast("Can't change status's pump");
                        }
                    }
                }
                catch
                {
                }
            }
            try
            {
                MainActivity.mListDeviceCurrent.Clear();
                MainActivity.mListDeviceOld.Clear();
                MainActivity.bluetoothAdapter.CancelDiscovery();
                MainActivity.bluetoothAdapter.Dispose();
                MainActivity.bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
                ICollection<BluetoothDevice> btcollect = MainActivity.bluetoothAdapter.BondedDevices;
                List<BluetoothDevice> btlistbond = btcollect.ToList();
                if (btlistbond.Count != 0)
                {
                    MainActivity.mBondedDevice = btlistbond[0];

                }
            }
            catch
            {
                showToast("Can't refresh");
            }

        }
        /// <summary>
        /// If have a device bonded, re-bonded this device.
        /// </summary>
        /// <param name="device"></param>
        public async void ReBondedDevice(BluetoothDevice device)
        {
            Task ReBonded = Task.Factory.StartNew(() =>
            {
                BluetoothSocket tmp = null;
                Timer t = null;
                try
                {
                    tmp = device.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
                    //listCurrent[i].CreateBond();
                    MainActivity.bluetoothAdapter.CancelDiscovery();
                    try
                    {
                        tmp.Connect();
                        byte[] temp = new byte[100];
                        //tmp.InputStream.Read(temp, 0, temp.Length);
                        ////SendDataTo(socket);
                        //GetDataFrom(temp);
                        showToast("Re-bond device: " + device.Name + " complete");
                        t = new Timer(x => MainActivity._DataCollector.AutoGetData(tmp), null, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5));
                        MainActivity.mTimer = t;
                        MainActivity.mSocket = tmp;
                    }
                    catch
                    {
                        try
                        {
                            tmp.Close();
                            t.Dispose();
                            showToast("Can't bond to this device. Socket is closed");
                        }
                        catch
                        {

                        }
                    }
                }
                catch
                {
                    try
                    {
                        tmp.Close();
                        t.Dispose();
                        showToast("Socket error. Socket is closed");
                    }
                    catch
                    {

                    }
                }

            });
        }
    }
}