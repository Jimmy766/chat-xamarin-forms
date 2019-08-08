using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Chat

{
    public partial class AddRoomPage : ContentPage
    {
        public AddRoomPage()
        {
            InitializeComponent();
        }
        // Añade una instancia de chat entre el conductor y un cliente
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            // cliente de firebase
            var db = new DBFire();
            // añade la instancia del chat
            await db.saveRoom(new Room() { Conductor = "Conductor", Activo = true, Cliente = "Cliente", Name = _rootName.Text });
            // una vez añadido sale automaticamente de la pantalla actual
            await Navigation.PopAsync();

        }
    }
}
