using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soundwave_Music.Common
{
    public class SignInUserSavedData
    {
		public int User_id { get; set; }
		public string Full_name { get; set; }
		public string Email { get; set; }
		public bool Permission_create { get; set; }
		public bool Permission_view { get; set; }
		public bool Permission_update { get; set; }
		public bool Permission_edit { get; set; }
		public bool Permission_delete { get; set; }
		public bool Permission_access { get; set; }
		public int Role_id { get; set; }
		public string Role_name { get; set; }
		public string Avatar { get; set; }
		public string Phone_number { get; set; }
	}
}