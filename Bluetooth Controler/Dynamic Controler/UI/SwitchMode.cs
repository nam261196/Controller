using Android.App;
using Android.Views;
using Android.Widget;

namespace Dynamic_Controler.UI
{
    public class SwitchMode
    {
        public SwitchMode() { }

        public void showToast(string str)
        {
            Toast.MakeText(Application.Context, str, ToastLength.Short).Show();
        }

        public void SwitchToManual()
        {
            MainActivity.btnAuto.SetImageResource(Resource.Drawable.Off_Auto);
            showToast("Switch to Manual mode - control by hand");
            MainActivity.txtMode.Visibility = ViewStates.Visible;
            MainActivity.btnController.Visibility = ViewStates.Visible;
            MainActivity._FlagCheckAuto = false;
        }

        public void SwitchToAuto()
        {
            MainActivity.btnAuto.SetImageResource(Resource.Drawable.On_Auto);
            showToast("Switch to Auto mode");
            MainActivity.txtMode.Visibility = ViewStates.Invisible;
            MainActivity.btnController.Visibility = ViewStates.Invisible;
            MainActivity._FlagCheckAuto = true;
        }
    }
}