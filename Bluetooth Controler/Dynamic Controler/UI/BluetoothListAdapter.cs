using System.Collections.Generic;
using Android.Content;
using Android.Bluetooth;

namespace Dynamic_Controler
{
    /// <summary>
    /// Show list device bluetooth
    /// </summary>
    class BluetoothListAdapter
    {
        Context _Context;
        BluetoothHandler _BluetoothHandler = new BluetoothHandler();
        public BluetoothListAdapter(Context c)
        {
            _Context = c;
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
        /// <summary>
        /// 
        /// If a device have in current but haven't in old list,
        /// update old list by add this device in old list.
        /// 
        /// </summary>
        public void SaveToListOld()
        {
            for (int i = 0; i < MainActivity.mListDeviceCurrent.Count; i++)
            {
                if (CheckIsHave(MainActivity.mListDeviceCurrent[i], MainActivity.mListDeviceOld))
                {
                    continue;
                }
                else
                {
                    MainActivity.mListDeviceOld.Add(MainActivity.mListDeviceCurrent[i]);
                }
            }
        }

        public void DeleteDeviceDisable()
        {
            for (int i = 0; i < MainActivity.mListDeviceOld.Count; i++)
            {
                if (CheckIsHave(MainActivity.mListDeviceOld[i], MainActivity.mListDeviceCurrent))
                {
                    continue;
                }
                else
                {
                    MainActivity.mListDeviceOld.RemoveAt(i);
                }
            }
        }
    }
}