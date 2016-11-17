using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Widget;
using Android.Bluetooth;
using System.Threading;

namespace Dynamic_Controler
{
    /// <summary>
    /// Check state bluetooth of this device moblie, handle bond or rebond. If bonded to a device, excute SendDataTo device.
    /// </summary>
    class BluetoothHandler
    {
        public BluetoothHandler()
        {

        }

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

        public bool CheckBondState(BluetoothDevice device)
        {
            bool BondState = false;
            if (device.BondState == Bond.Bonded)
            {
                BondState = true;
            }
            else
            {
                BondState = false;
            }
            return BondState;
        }

        public void BondedDevice(BluetoothDevice device)
        {
            try
            {
                MainActivity.bluetoothAdapter.CancelDiscovery();
                BluetoothSocket tmpSocket = null;
                Timer t = null;
                tmpSocket = device.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
                tmpSocket.Connect();
                t = new Timer(x => MainActivity._DataCollector.AutoGetData(tmpSocket), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
                MainActivity.mBondedDevice = device;
                MainActivity.mTimer = t;
                MainActivity.mSocket = tmpSocket;
                MainActivity.mBondedDevice = device;
                showToast("State connection: " + MainActivity.mSocket.IsConnected);
            }
            catch
            {
                showToast("Can't bond this device. Please Re-scan bluetooth device.");
            }
        }

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
        public void UnBondedAllDevice(BluetoothDevice device)
        {
            ICollection<BluetoothDevice> btcollect = MainActivity.bluetoothAdapter.BondedDevices;
            List<BluetoothDevice> allDevice = btcollect.ToList();
            if (allDevice.Count != 0)
            {
                for (int i = 0; i < allDevice.Count; i++)
                {
                    try
                    {
                        Java.Lang.Reflect.Method method = allDevice[i].Class.GetMethod("removeBond", null);
                        method.Invoke(allDevice[i], null);
                    }
                    catch (Exception)
                    {
                        showToast("Can't unbond this device");
                    }
                }

            }

        }
    }
}