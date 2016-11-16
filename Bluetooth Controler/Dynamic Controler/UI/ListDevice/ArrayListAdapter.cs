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
using Android.Graphics;

namespace Dynamic_Controler
{
    public class ArrayListAdapter : ArrayAdapter<BluetoothDevice>
    {
        Activity _Context;
        List<BluetoothDevice> _Title = new List<BluetoothDevice>();

        public ArrayListAdapter(Activity context, List<BluetoothDevice> title) : base(context, Resource.Layout.row_In_List_Device, title)
        {
            _Context = context;
            _Title = title;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = _Context.LayoutInflater;
            View rowView = inflater.Inflate(Resource.Layout.row_In_List_Device, null, true);
            TextView txt = rowView.FindViewById<TextView>(Resource.Id.nameDevice);
            txt.Text = _Title[position].Name;
            return rowView;
        }
    }
}