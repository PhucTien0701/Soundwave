using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Soundwave_Music.Common.Helper
{
    public static class Helperr
    {
		public static string GetName(this IIdentity identity)
		{
			return identity.GetUserData().Full_name;
		}
		public static int GetUserId(this IIdentity identity)
		{
			return identity.GetUserData().User_id;
		}
		public static string GetAvartar(this IIdentity identity)
		{
			return identity.GetUserData().Avatar;
		}
		public static string GetEmail(this IIdentity identity)
		{
			return identity.GetUserData().Email;
		}
		public static string GetPhoneNumber(this IIdentity identity)
		{
			return identity.GetUserData().Phone_number;
		}
		public static int GetRole(this IIdentity identity)
		{
			return identity.GetUserData().Role_id;
		}
		public static string GetRoleName(this IIdentity identity)
		{
			return identity.GetUserData().Role_name;
		}
		public static bool Permiss_Create(this IIdentity identity)
		{
			return identity.GetUserData().Permission_create;
		}
		public static bool Permiss_View(this IIdentity identity)
		{
			return identity.GetUserData().Permission_view;
		}
		public static bool Permiss_Update(this IIdentity identity)
		{
			return identity.GetUserData().Permission_update;
		}
		public static bool Permiss_Edit(this IIdentity identity)
		{
			return identity.GetUserData().Permission_edit;
		}
		public static bool Permiss_Delete(this IIdentity identity)
		{
			return identity.GetUserData().Permission_delete;
		}
		public static bool Permiss_Access(this IIdentity identity)
		{
			return identity.GetUserData().Permission_access;
		}
		public static SignInUserSavedData GetUserData(this IIdentity identity)
		{
			var jsonUserData = HttpContext.Current.User.Identity.Name;
			var userData = JsonConvert.DeserializeObject<SignInUserSavedData>(jsonUserData);
			return userData;
		}
	}
}