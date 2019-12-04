using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;

namespace SAM2020.Models
{
    public class Notification
    {

        [Required]
        public string message { get; set; }

        [Required]
        public string userRole { get; set; }

        public int recipientId { get; set; }

        /*
         * status =0 erro
         * status =1 send
         * status=2 no record to send to
         * */
        public int sendNotifiactionAll(String message, int userRole, int senderId)
        {
            int status = 1;
            User user = new User();
            List<string> usersList = user.getAllUsersIDs(userRole);

            if(usersList.Count == 0)
            {
                return 2;
            }
            // Insert Notifiaction in main table
            long notificationID = 0;
            try
            {
                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "INSERT INTO notification(senderId,message) VALUES(@senderId,@message)";
                comm.Parameters.AddWithValue("@senderId", senderId);
                comm.Parameters.AddWithValue("@message", message);
   
                comm.ExecuteNonQuery();
                notificationID = comm.LastInsertedId;
                conn.Close();
            }
            catch (Exception e)
            {
                string msg = e.Message;
                return 0;
            }

            if(notificationID == 0)
            {
                return 0; // record didnot inserted
            }

            // send notifiaction to all subscribers
            foreach( String userID in usersList)
            {
                try
                {
                    MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();
                    comm.CommandText = "INSERT INTO notification_recipients(notification_id,recipient_id) VALUES(@notification_id,@recipient_id)";
                    comm.Parameters.AddWithValue("@notification_id", notificationID);
                    comm.Parameters.AddWithValue("@recipient_id", userID);

                    comm.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                }

            }

            return status;
        }

        public int notifyConflict(String senderId, String message, Array users)
        {
            int status = -1;
            long notificationID = 0;

            try
            {
                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "INSERT INTO notification(senderId,message) VALUES(@senderId,@message)";
                comm.Parameters.AddWithValue("@senderId", senderId);
                comm.Parameters.AddWithValue("@message", message);
   
                status = comm.ExecuteNonQuery();
                notificationID = comm.LastInsertedId;
                conn.Close();
            }
            catch (Exception e)
            {
                string msg = e.Message;
                return status;
            }

            if (notificationID == 0)
            {
                return status; // record didnot inserted
            }

            // send notifiaction to all subscribers
            foreach(String userID in users)
            {
                try
                {
                    MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();
                    comm.CommandText = "INSERT INTO notification_recipients(notification_id,recipient_id) VALUES(@notification_id,@recipient_id)";
                    comm.Parameters.AddWithValue("@notification_id", notificationID);
                    comm.Parameters.AddWithValue("@recipient_id", userID);

                    status = comm.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                }

            }

            return status;
        }

        /*
         * 
         * Use this method to send notification to one user
        * status =0 erro
        * status =1 send
        * status=2 no record to send to
        * 
        * */
        public int sendNotifiactionToOneUser(String message, int recipientID, int senderId)
        {
            int status = 1;
            
            // Insert Notifiaction in main table
            long notificationID = 0;
            try
            {
                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "INSERT INTO notification(senderId,message) VALUES(@senderId,@message)";
                comm.Parameters.AddWithValue("@senderId", senderId);
                comm.Parameters.AddWithValue("@message", message);

                comm.ExecuteNonQuery();
                notificationID = comm.LastInsertedId;
                conn.Close();
            }
            catch (Exception e)
            {
                string msg = e.Message;
                return 0;
            }

            if (notificationID == 0)
            {
                return 0; // record didnot inserted
            }

            // send notifiaction to all subscribers
                try
                {
                    MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();
                    comm.CommandText = "INSERT INTO notification_recipients(notification_id,recipient_id) VALUES(@notification_id,@recipient_id)";
                    comm.Parameters.AddWithValue("@notification_id", notificationID);
                    comm.Parameters.AddWithValue("@recipient_id", recipientID);

                    comm.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                }

            return status;
        }

        // get number of not readed notifiactions
        public int getMyNotifiactionCount(int userID)
        {
            int countNotify = 0;

            try
            {
                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select count(*) as countNotify from notification_recipients where isRead=0 and recipient_id=@recipient_id";
                comm.Parameters.AddWithValue("@recipient_id", userID);
                countNotify = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }

            return countNotify;
        }

        // get not readed notifiaction details
        public List<UserNotificationObj> getMyNotifiactionDetails(int userID)
        {
            List<UserNotificationObj> userNotificationsList = new List<UserNotificationObj>();

            try
            {
                MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
                DBconnection.Open();
                MySqlCommand SQLCommand = DBconnection.CreateCommand();
                MySqlDataReader dataReader;
                SQLCommand.CommandText = @"
                  SELECT message, notification_sender_id, isRead, createdDate
                  FROM notification inner join notification_recipients nr on notification.notification_id = nr.notification_id
                  WHERE recipient_id=@recipient_id
                  ORDER BY createdDate DESC";
                SQLCommand.Parameters.AddWithValue("@recipient_id", userID);
                dataReader = SQLCommand.ExecuteReader();

                try
                {
                    while (dataReader.Read())
                    {
                        userNotificationsList.Add(new UserNotificationObj(dataReader.GetString(0), dataReader.GetString(1), dataReader.GetInt32(2), dataReader.GetDateTime(3)));
                    }
                }
                finally
                {
                    dataReader.Close();
                    DBconnection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return userNotificationsList;
        }

        // mark readed notifiaction as readed
        public void notifiactionISReadedBy(int mnotificationSenderIDReadm, int userID)
        {
            // send notifiaction to all subscribers
            try
            {
                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "update notification_recipients set isRead=1 where  notification_sender_id=@notification_sender_id and recipient_id=@recipient_id";
                comm.Parameters.AddWithValue("@notification_sender_id", mnotificationSenderIDReadm);
                comm.Parameters.AddWithValue("@recipient_id", userID);

                comm.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
    }
}
