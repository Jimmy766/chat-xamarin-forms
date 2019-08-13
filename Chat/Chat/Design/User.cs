using System;
namespace Chat
{
    // clase estatica para guardar el usuario actual... solo con fines de simular una session
	public class User
	{
		private static string uid;

		public static string UserName
		{
			get {
				return uid;
			}
			set
			{
				uid = value;
			}
		}

        public static string Token { get; set; }

        public async static void guardarToken(string token)
        {
            
            DBFire cliente = new DBFire();
            await cliente.guardarToken(token);
        }
        
		private User(){}

	}
}
