using System;
using System.Collections.Generic;
using System.Text;

namespace App2
{
    class Mes
    {
        public string to;
        public Noti notification;
        public Mes(string to, Noti notification)
        {
            this.to = to;
            this.notification = notification;
        }
    }
}
