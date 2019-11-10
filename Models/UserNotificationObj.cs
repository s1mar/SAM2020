using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAM2020.Models
{
    public class UserNotificationObj
    {
        public string message;
        public string notificationSenderID;
        public UserNotificationObj(string message, string notificationSenderID)
        {
            this.message = message;
            this.notificationSenderID = notificationSenderID;
        }
    }
}
