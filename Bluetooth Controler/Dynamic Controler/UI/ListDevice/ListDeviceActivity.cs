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
using System.Threading.Tasks;
using System.Threading;
using Java.Lang;

namespace Dynamic_Controler
{
    [Activity(Theme = "@style/Theme.Example",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.SensorPortrait)]

    public class ListDeviceActivity : Activity
    {
        ListView _lvDevice;
        public List<BluetoothDevice> arrTitle = new List<BluetoothDevice>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.List_Device_layout);
            

            _lvDevice = FindViewById<ListView>(Resource.Id.listDevice);

            update();

            LayoutInflater inflater = (LayoutInflater) GetSystemService(Context.LayoutInflaterService);
            View customView = inflater.Inflate(Resource.Layout.Custom_Actionbar_layout,null);

            LayoutParams layout = new LayoutParams(LayoutParams.FillParent, LayoutParams.FillParent);

            ActionBar.SetCustomView(customView, layout);
            ActionBar.SetDisplayShowCustomEnabled(true);
            ActionBar.SetDisplayShowHomeEnabled(false);

            ImageButton btnRefresh = customView.FindViewById<ImageButton>(Resource.Id.btnRefresh);
            btnRefresh.Click += delegate {
                Toast.MakeText(this,"@@",ToastLength.Short).Show();
            };

            ImageButton btnBack = customView.FindViewById<ImageButton>(Resource.Id.btnBack);
            btnBack.Click += delegate {
                MainActivity.bluetoothAdapter.CancelDiscovery();
                Finish();
                
            };
        }

        private void update()
        {

            Timer a = new Timer(x => { MainActivity.bluetoothAdapter.StartDiscovery(); }, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(4));
            Timer c = new Timer(x => { MainActivity.mListDeviceCurrent.Clear(); MainActivity.bluetoothAdapter.CancelDiscovery(); }, null, TimeSpan.FromSeconds(3.5), TimeSpan.FromSeconds(4));
            arrTitle = MainActivity.mListDeviceCurrent;
            Timer b = new Timer(x => { updatescreen(); }, null, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(4));
        }

        private void updatescreen()
        {
            RunOnUiThread(() => {
                if (MainActivity.mListDeviceCurrent.Count > 0)
                {
                    for (int i = 0; i < MainActivity.mListDeviceCurrent.Count; i++)
                    {
                        if (CheckIsHave(arrTitle[i], MainActivity.mListDeviceCurrent))
                        {
                            continue;
                        }
                        else
                        {

                        }
                    }
                    MainActivity.mListDeviceCurrent.Sort((x, y) => string.Compare(x.Name, y.Name));
                    ArrayListAdapter adapter = new ArrayListAdapter(this, MainActivity.mListDeviceCurrent);
                    _lvDevice.Adapter = adapter;
                }

            });
        }

        public bool CheckIsHave(BluetoothDevice device, List<BluetoothDevice> listDevice)
        {
            bool isHave = false;
            for (int i = 0; i < listDevice.Count; i++)
            {
                if (device.Equals(listDevice[i]))
                {
                    isHave = true;
                    break;
                }
            }
            return isHave;
        }

    }
}