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
        public int isRead;
        public DateTime creationDate;
        public UserNotificationObj(string message, string notificationSenderID, int isRead, DateTime creationDate)
        {
            this.message = message;
            this.notificationSenderID = notificationSenderID;
            this.isRead = isRead;
            this.creationDate = creationDate;
        }
    }
}
