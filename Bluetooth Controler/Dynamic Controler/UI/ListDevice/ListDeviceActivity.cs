using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using static Android.App.ActionBar;
using System.Threading;

namespace Dynamic_Controler
{
    [Activity(Theme = "@style/Theme.Example",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.SensorPortrait)]
    public class ListDeviceActivity : Activity
    {
        ListView _lvDevice;
        BluetoothHandler _BluetoothHandler = new BluetoothHandler();

        public void showToast(string str)
        {
            Toast.MakeText(Application.Context, str, ToastLength.Short).Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.List_Device_layout);

            //MainActivity.mListDeviceCurrent.Clear();
            _lvDevice = FindViewById<ListView>(Resource.Id.listDevice);
            ArrayListAdapter adapter = new ArrayListAdapter(this, MainActivity.mListDeviceCurrent);
            _lvDevice.Adapter = adapter;

            LayoutInflater inflater = (LayoutInflater)GetSystemService(Context.LayoutInflaterService);
            View customView = inflater.Inflate(Resource.Layout.Custom_Actionbar_layout, null);
            LayoutParams layout = new LayoutParams(LayoutParams.FillParent, LayoutParams.FillParent);
            ActionBar.SetCustomView(customView, layout);
            ActionBar.SetDisplayShowCustomEnabled(true);
            ActionBar.SetDisplayShowHomeEnabled(false);

            ImageButton btnRefresh = customView.FindViewById<ImageButton>(Resource.Id.btnRefresh);
            btnRefresh.Click += delegate
            {
                btnRefresh.StartAnimation(MainActivity.AminationRefresh);

            };

            ImageButton btnBack = customView.FindViewById<ImageButton>(Resource.Id.btnBack);
            btnBack.Click += delegate
            {
                MainActivity.bluetoothAdapter.CancelDiscovery();
                Finish();
            };

            _lvDevice.ItemClick += (s, e) =>
            {

                if (_BluetoothHandler.CheckBondState(MainActivity.mListDeviceCurrent[e.Position]))
                {
                    try
                    {
                        MainActivity.mBondedDevice = MainActivity.mListDeviceCurrent[e.Position];
                        _BluetoothHandler.UnBondedAllDevice(MainActivity.mBondedDevice);//unbond this device.
                        _BluetoothHandler.BondedDevice(MainActivity.mBondedDevice);//re-bond this device.
                        Finish();
                    }
                    catch
                    {
                        Finish();
                    }

                }
                else
                {
                    _BluetoothHandler.BondedDevice(MainActivity.mListDeviceCurrent[e.Position]);
                    Finish();
                }
            };
        }



    }
}