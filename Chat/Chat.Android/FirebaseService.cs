using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Iid;

namespace Chat.Droid
{
    //servicio para los mensajes
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseService : FirebaseInstanceIdService
    {
        const string TAG = "FirebaseIIDService";
        

        public override void OnTokenRefresh()
        {
            // token generado por firebase
            var nuevo = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "nuevo token: " + nuevo);
            SendRegistrationToServer(nuevo);

             
        }

        public async void SendRegistrationToServer(string token)
        {
            //registro al servidor
            User.Token = token;
        }
        
    }
}