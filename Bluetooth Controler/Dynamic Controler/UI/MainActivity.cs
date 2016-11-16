using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Runtime;
using System;
using System.Collections.Generic;
using Android.Views.Animations;
using System.Linq;
using System.Threading;

namespace Dynamic_Controler
{
    [Activity(Label = "Pump Controler", MainLauncher = true, Icon = "@drawable/icon",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.SensorPortrait)]
    public class MainActivity : Activity
    {
        public MainActivity ()
        {

        }

        public static Activity _Activity;
        public static Button btnScanDevice;

        public static ImageView btnRefresh;
        public static ImageButton btnController;

        public static Animation AminationRefresh;
        public static TextView txtHumidity, txtTemperature, txtFlow, txtToday, txtTime;

        List<BluetoothDevice> btArrayAdapter;
        
        public static List<BluetoothDevice> mListDeviceOld = new List<BluetoothDevice>();
        public static List<BluetoothDevice> mListDeviceCurrent = new List<BluetoothDevice>();
        public static PopupMenu mMenuOld;
        public static BluetoothAdapter bluetoothAdapter;
        public static bool mFlagRefresh = false;
        public static bool mFlagStatusPump = false;
        public static BluetoothDevice mBondedDevice;
        public static BluetoothSocket mSocket;
        public static byte[] OffPumpASCII = { 126, 0, 03 };
        public static byte[] OnPumpASCII = { 126, 1, 03 };
        public static byte[] mResultGetData = new byte[10];
        public static Timer mTimer;


        ConvertData mConverterData = new ConvertData();
        ActionFoundReceiver _ActionFoundReceiver;
        public static DataCollector _DataCollector;
        BluetoothHandler _BluetoothHandler = new BluetoothHandler();
        BluetoothListAdapter _BluetoothListAdapter;
        ViewController _ViewController;


        public void showToast(string str)
        {
            Toast.MakeText(this, str, ToastLength.Short).Show();
        }

        /// <summary>
        /// Send data to pump.
        /// </summary>
        /// <param name="signal"></param>

        private void UpdateTime()
        {
            this.RunOnUiThread(() =>
            txtTime.Text = (string.Format("{0:HH:mm:ss tt}", DateTime.Now)).ToUpper());
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {


            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            _Activity = this;

            _ActionFoundReceiver = new ActionFoundReceiver(btArrayAdapter, btnScanDevice, this);
            _DataCollector = new DataCollector(this);
            _BluetoothListAdapter = new BluetoothListAdapter(this);
            _ViewController = new ViewController(this);
            _ViewController.Init();
            mResultGetData[0] = 126;//header
            mResultGetData[mResultGetData.Length - 1] = 03;//ender

            

            DateTime today = DateTime.Now;
            AminationRefresh = AnimationUtils.LoadAnimation(this, Resource.Animation.rotate_centre);
            Timer t = new Timer(x => UpdateTime(), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            ICollection<BluetoothDevice> btcollect = bluetoothAdapter.BondedDevices;
            List<BluetoothDevice> btlistbond = btcollect.ToList();

            if (btlistbond.Count != 0)
            {
                mBondedDevice = btlistbond[0];
                _BluetoothHandler.ReBondedDevice(mBondedDevice);
                showToast("Bonded: " + mBondedDevice.Name);
            }

            LinearLayout layout = (LinearLayout)FindViewById(Resource.Id.mainlayout);
            Intent discoverableIntent = new Intent(BluetoothAdapter.ActionRequestDiscoverable);
            discoverableIntent.PutExtra(BluetoothAdapter.ExtraDiscoverableDuration, 300);
            StartActivity(discoverableIntent);

            btArrayAdapter = new List<BluetoothDevice>();
            mMenuOld = new PopupMenu(this, btnScanDevice);


            RegisterReceiver(_ActionFoundReceiver, new IntentFilter(BluetoothDevice.ActionFound));
            _BluetoothHandler.CheckBlueToothState();
            MainActivity.bluetoothAdapter.StartDiscovery();
            mMenuOld.DismissEvent += delegate
            {
                mMenuOld.Menu.Clear();
            };
            _ViewController.Controller();
            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterReceiver(_ActionFoundReceiver);
        }

        /// <summary>
        /// Check this device enable or disable bluetooth.
        /// </summary>
        

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == 1)
            {
                _BluetoothHandler.CheckBlueToothState();
            }

        }


    }
}

