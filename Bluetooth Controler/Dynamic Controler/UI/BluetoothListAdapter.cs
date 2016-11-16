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

        public void ShowListOld()
        {
            //MainActivity.mListDeviceOld.Sort((x, y) => string.Compare(x.Name, y.Name));
            //PopupMenu menu = new PopupMenu(_Context, MainActivity.btnScanDevice);
            //menu.Inflate(Resource.Menu.menu_device);
            //if (MainActivity.mMenuOld != null)
            //{
            //    MainActivity.mMenuOld.Dismiss();
            //    for (int i = 0; i < MainActivity.mListDeviceOld.Count; i++)
            //    {
            //        menu.Menu.Add(MainActivity.mListDeviceOld[i].Name);
            //    }
            //    menu.Show();
            //    menu.MenuItemClick += (s, e) =>
            //    {
            //        _BluetoothHandler.BondedDevice(MainActivity.mListDeviceCurrent, e.Item);
            //    };
            //    MainActivity.mMenuOld = menu;
            //}

        }

        public void ShowListCurrent()
        {
            //MainActivity.mListDeviceOld.Sort((x, y) => string.Compare(x.Name, y.Name));
            //PopupMenu menu = new PopupMenu(_Context, MainActivity.btnScanDevice);
            //menu.Inflate(Resource.Menu.menu_device);
            //if (MainActivity.mMenuOld != null)
            //{
            //    MainActivity.mMenuOld.Dismiss();
            //    for (int i = 0; i < MainActivity.mListDeviceCurrent.Count; i++)
            //    {
            //        menu.Menu.Add(MainActivity.mListDeviceCurrent[i].Name);
            //    }
            //    menu.Show();
            //    menu.MenuItemClick += (s, e) =>
            //    {
            //        _BluetoothHandler.BondedDevice(MainActivity.mListDeviceCurrent, e.Item);
            //    };
            //    MainActivity.mMenuOld = menu;
            //}

        }

        /// <summary>
        /// 
        /// If a device have in old list but haven't in current list, It was disabled bluetooth.
        ///
        /// </summary>
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