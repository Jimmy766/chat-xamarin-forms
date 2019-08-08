using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    // cliente de firebase .. aqui se establece la conexion con firebase y metodos utilitarios 
    class DBFire
    {
        // instancia del cliente
        FirebaseClient fbClient;

        // constructor
        public DBFire()
        {
            // conexion con firebase usando el link que proporciona
            fbClient = new FirebaseClient("https://chat-xamarin-forms.firebaseio.com/");
        }

        // obtiene la lista de instancias de chat 
        public async Task<List<Room>> getRoomList()
        {
            return (await fbClient
                .Child("ChatApp") // "ChatApp es el nombre global de todas las instancias"
                .OnceAsync<Room>()) // obtine la coleccion de forma asincrona
                .Select((item) => // iteracion para generar una lista formateada a la clase Room
                new Room // formatea los datos
                {
                    Key = item.Key,
                    Name=User.UserName=="Conductor"? item.Object.Cliente:item.Object.Conductor,
                    Conductor = item.Object.Conductor,
                    Cliente= item.Object.Cliente,
                    Activo= item.Object.Activo
                }

                       ).ToList(); // devuelve un iterable (List<Room>)

        }

        // desactiva el chat rm.. 
        public async Task desactivar(Room rm)
        {
            // obtiene la lista de chats
            List<Room> salas = await getRoomList();
            foreach (var item in salas) // itera para buscar el chat rm
            {
                if(item.Key== rm.Key) // una vez encontrado se marca la instancia como falso 
                {
                    rm.Activo = false;
                    // se actualiza la instancia en firebase
                    await fbClient.Child("ChatApp/" + rm.Key).Child("Activo").PutAsync(false);
                    // con la marca en false se puede usar para desactivar el chat u omitirlo en alguna comprobacion futura
                }
            }
        }

        // guarda una instancia de chat
        public async Task saveRoom(Room rm)
        {
            await fbClient.Child("ChatApp")
                    .PostAsync(rm);

        }

       

        // guarda un mensaje dentro de una instancia de chat _room
        public async Task saveMessage(Chat _ch, string _room)
        {
            await fbClient.Child("ChatApp/" + _room + "/Message")
                    .PostAsync(_ch);
        }

        //  obtiene la coleccion de mensajes dentro de una instancia de chat _roomKEY
        // lo obtiene como observable para la base de datos en tiempo real de firebase
        public ObservableCollection<Chat> subChat(string _roomKEY)
        {

            return fbClient.Child("ChatApp/" + _roomKEY + "/Message")
                           .AsObservable<Chat>()
                           .AsObservableCollection<Chat>();
        }
    }
}
