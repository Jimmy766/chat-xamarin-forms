using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Chat
{
	public partial class ChatPage : ContentPage
	{
        //cliente de firebase
		DBFire db = new DBFire();

        // instancia del chat actual
		Room rm = new Room();

        // marcador 
        bool visible = false;
		public ChatPage()
		{
			InitializeComponent();
            // el marcador define si la session actual es de un conductor o un cliente
            visible = User.UserName == "Conductor" ? true : false;
            if (!visible) // si es de un cliente quita el boton de desactivar chat
                ToolbarItems.Clear();

            // recupera selectRoom de la pantalla anterior(instancia del chat actual) 
            MessagingCenter.Subscribe<RoomPage, Room>(this, "RoomProp", (page, data) =>
			{
                // guarda la instancia en rm
				rm = data;
                // binding para la lista de mensajes
				_lstChat.BindingContext = db.subChat(data.Key);

                // cierra la subscripcion para evitar futuras llamadas a este callback
				MessagingCenter.Unsubscribe<RoomPage, Room>(this, "RoomProp");

			});
		}
        // envia mensaje a firebase
		async void Handle_Clicked(object sender, System.EventArgs e)
		{
			// crea el modelo cargando desde el campo de texto y el usuario actual
			var chatOBJ = new Chat { UserMessage = _etMessage.Text, UserName = User.UserName };

            // lo guarda
			await  db.saveMessage(chatOBJ, rm.Key);

            // limpia el campo de texto
            _etMessage.Text = "";


		}

        
        // boton desactivar

        private async void desactivarAsync(object sender, EventArgs e)
        {
            // desactiva el chat
            await db.desactivar(rm);
        }
    }
}
