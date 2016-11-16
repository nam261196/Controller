using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
namespace Dynamic_Controler
{
    /// <summary>
    /// When have a device turn on/turn off bluetooth, this class will excute return list device current
    /// </summary>
    public class ActionFoundReceiver : BroadcastReceiver
    {
        public List<BluetoothDevice> btArrayAdapter;
        public Button mButton;
        public Context _Context;

        public void showToast(string str)
        {
            Toast.MakeText(_Context, str, ToastLength.Short).Show();
        }

        public ActionFoundReceiver(List<BluetoothDevice> btArray, Button btn, Context context)
        {
            btArrayAdapter = btArray;
            mButton = btn;
            _Context = context;
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

        // public static readonly Java.Util.UUID myUUID = Java.Util.UUID.FromString("00001101 - 0000 - 1000 - 8000 - 00805f9b34fb");
        public override void OnReceive(Context context, Intent intent)
        {
            MainActivity.mFlagRefresh = true;
            //mListDeviceOld.Clear();
            // TODO Auto-generated method stub
            string action = intent.Action;
            if (BluetoothDevice.ActionFound.Equals(action))
            {
                BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                if (CheckIsHave(device, MainActivity.mListDeviceCurrent) == false)
                {
                    MainActivity.mListDeviceCurrent.Add(device);
                }
                //bluetoothAdapter.CancelDiscovery();
            }
            else if (BluetoothDevice.ActionBondStateChanged.Equals(action))
            {
                int curState = intent.GetIntExtra(BluetoothDevice.ExtraBondState, BluetoothDevice.Error);
                int preState = intent.GetIntExtra(BluetoothDevice.ExtraPreviousBondState, BluetoothDevice.Error);
                if (curState == (int)Bond.Bonded && preState == (int)Bond.Bonding)
                {
                    //showToast("Paired");
                }
                else if (curState == (int)Bond.None && preState == (int)Bond.Bonded)
                {
                    //showToast("UnPaired");
                }
            }
        }


    }
}