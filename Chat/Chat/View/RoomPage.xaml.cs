using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace Chat
{
	public partial class RoomPage : ContentPage
	{
        // cliente firebase
		DBFire db = new DBFire();
		protected override async void OnAppearing()
		{
			base.OnAppearing();
            // obtiene las instancias de chat 
			var list = await db.getRoomList();
            // binding con la lista de instancias
			_lstx.BindingContext = list;
		}

		public RoomPage()
		{
			InitializeComponent();

		}

        // si la lista se esta refrescando llama a este metodo
		async void Handle_Refreshing(object sender, System.EventArgs e)
		{
            // rehace el binding y marca que se termino de refrescar los datos
			_lstx.BindingContext = await db.getRoomList();
			_lstx.IsRefreshing = false;
		}

        // mensaje para saber el usuario actual .. solo con fines de prueba 
		void Info_Clicked(object sender, System.EventArgs e)
		{
			DisplayAlert("Usuario: ", User.UserName, "Ok");
		}

        // inicia la pantalla para crear una nueva instancia de chat .. solo con fines de prueba
        // en produccion este metodo deberia iniciarse automaticamente cuando el chofer acepta la carrera del cliente
		void Plus_Clicked(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new AddRoomPage());
			
		}
        // inicia la pantalla del chat entre el conductor y el cliente
		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{

            // si la seleccion no es nula
			if (_lstx.SelectedItem!=null)
			{

            // castea Room por comodidad
			var selectRoom = (Room)_lstx.SelectedItem;
               // inicia la pantalla del chat
				Navigation.PushAsync(new ChatPage());
				// envia la instancia del chat seleccionado
				MessagingCenter.Send<RoomPage, Room>(this, "RoomProp", selectRoom);
				
			}

		}
        // simula un login de usuario para identificar al chofer/cliente
        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            User.UserName = usuario.Text;
            _lstx.BindingContext = await db.getRoomList();
            _lstx.IsRefreshing = false;
        }
    }
}
