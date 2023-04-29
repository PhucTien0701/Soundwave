using PayPal.Api;
using Soundwave_Music.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soundwave_Music.PaymentLibrary
{
    public static class PaypalConfiguration
    {
        //Variables for storing the clientID and clientSecret key
        public readonly static string clientId;
        public readonly static string clientSecret;
        private readonly static SoundwaveDbContext db = new SoundwaveDbContext();
        //Constructor
        static PaypalConfiguration()
        {
            Models.API_Key aPI_Key = db.API_Key.FirstOrDefault(m => m.id == 2 && m.active == true);
            var config = GetConfig();
            clientId = aPI_Key.client_id;
            clientSecret = aPI_Key.client_secret;
        }
        // getting properties from the web.config
        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        private static string GetAccessToken()
        {
            // getting accesstocken from paypal
            string accessToken = new OAuthTokenCredential(clientId, clientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}