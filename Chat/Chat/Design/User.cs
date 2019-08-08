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


		private User(){}

	}
}
