﻿using App2;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
                    Name = User.UserName == "Conductor" ? item.Object.Cliente : item.Object.Conductor,
                    Conductor = item.Object.Conductor,
                    Cliente = item.Object.Cliente,
                    Activo = item.Object.Activo,
                    TokenConductor=item.Object.TokenConductor,
                    TokenCliente=item.Object.TokenCliente
                }

                       ).ToList(); // devuelve un iterable (List<Room>)

        }

        public async Task guardarToken(string token)
        {
            // obtiene la lista de chats
            List<Room> salas = await getRoomList();
            foreach (var item in salas) 
            {
                string user = User.UserName == "Conductor" ? "TokenConductor" : "TokenCliente";
                    // se actualiza la instancia en firebase
               
                await fbClient.Child("ChatApp").Child(item.Key).Child(user).PutAsync<string>(User.Token);
                
            }
        }
        public async Task<string> obtenerToken()
        {
            return (await getRoomList())[0].TokenConductor;
        }
        // desactiva el chat rm.. 
        public async Task desactivar(Room rm)
        {
            // obtiene la lista de chats
            List<Room> salas = await getRoomList();
            foreach (var item in salas) // itera para buscar el chat rm
            {
                if (item.Key == rm.Key) // una vez encontrado se marca la instancia como falso 
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
            rm.TokenCliente = rm.TokenConductor = "vacio";
            User.guardarToken(User.Token);
            await fbClient.Child("ChatApp")
                    .PostAsync(rm);

        }



        // guarda un mensaje dentro de una instancia de chat _room
        public async Task saveMessage(Chat _ch, string _room)
        {
            await fbClient.Child("ChatApp/" + _room + "/Message")
                    .PostAsync(_ch);
            
            await envio2(_room);

        }

        //  obtiene la coleccion de mensajes dentro de una instancia de chat _roomKEY
        // lo obtiene como observable para la base de datos en tiempo real de firebase
        public ObservableCollection<Chat> subChat(string _roomKEY)
        {

            return fbClient.Child("ChatApp/" + _roomKEY + "/Message")
                           .AsObservable<Chat>()
                           .AsObservableCollection<Chat>();
        }


        // envia el aviso
        private async Task envio()
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://fcm.googleapis.com/fcm/send");

            string jsonData = @"{
""to"":""dj3vDWZscAE:APA91bF44CfT3KkES3LDBb3cyLjZHG6fBv7XAuaRRtUKMdhj_4_zfKW3uQTc3rcqST8e7CooIS2o-AaGoGS_JG1yNjngUJ5dKFAr0IZ-m-XMZePEXrPTCISTTFaF9y6TXoiLmALTn2sF"",
""notification"":{
""title"":""CheckthisMobile(title)"",
""body"":""RichNotificationtesting(body)"",
""mutable_content"":true,
""sound"":""Tri-tone""
}";
            try
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "AIzaSyCg2fEqo5ooHEzJFSykeuRphu7eAYNtTSc");

                HttpResponseMessage response = await client.PostAsync("https://fcm.googleapis.com/fcm/send", content);


                var result = await response.Content.ReadAsStringAsync();

            }
            catch (Exception er)
            {
                var lb = er.ToString();
                var js = "xs";
            }



        }
        public async Task envio2(string conversacion)
        {
            var con = fbClient.Child("ChatApp/" + conversacion);
            string token = User.UserName == "Conductor" ? await con.Child("TokenCliente").OnceSingleAsync<string>(): await con.Child("TokenConductor").OnceSingleAsync<string>();
            Mes mes = new Mes(token,
        new Noti("great", "yes"));
            string jsonData = JsonConvert.SerializeObject(mes);
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://fcm.googleapis.com/fcm/send");


            try
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=AIzaSyCg2fEqo5ooHEzJFSykeuRphu7eAYNtTSc");

                HttpResponseMessage response = await client.PostAsync("https://fcm.googleapis.com/fcm/send", content);


                var result = await response.Content.ReadAsStringAsync();

            }
            catch (Exception er)
            {
                var lb = er.ToString();
                var js = "xs";
            }

        }

    }
}
