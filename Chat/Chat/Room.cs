using System;
using System.Collections.Generic;
using System.Text;

namespace Chat
{

    // modelo de una instancia de chat
    class Room
    {
        // nombre de la instancia
        public string Name
        {
            get;
            set;
        }

        // nombre o id del conductor 
        public string Conductor
        {
            get;
            set;
        }
        // nombre o id del cliente 
        public string Cliente
        {
            get;
            set;
        }
        // llave unica de la instancia
        public string Key
        {
            get;
            set;
        }
        // marcador que indica si el chat esta activo o no
        public bool Activo
        {
            get;
            set;
        }
    }
}
