using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soundwave_Music.Common.NotificationLib
{
    public class Notification
    {
        public static bool has_noti()
        {
            if (HttpContext.Current.Application["Notification"].Equals("")) return false;
            return true;
        }
        public static void set_noti(string _soundwavemess, string _soundwavemesstype)
        {
            var notification = new NotificationModel();
            notification.Soundwave_message = _soundwavemess;
            notification.Soundwave_message_type = _soundwavemesstype;
            HttpContext.Current.Application["Notification"] = notification;
        }
        public static void set_noti1s(string _soundwavemess1s, string _soundwavemesstype1s)
        {
            var notification = new NotificationModel();
            notification.Soundwave_message_1s = _soundwavemess1s;
            notification.Soundwave_message_type_1s = _soundwavemesstype1s;
            HttpContext.Current.Application["Notification"] = notification;
        }
        public static NotificationModel get_noti()
        {
            var Notification = (NotificationModel)HttpContext.Current.Application["Notification"];
            HttpContext.Current.Application["Notification"] = "";
            return Notification;
        }
    }
}