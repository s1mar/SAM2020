using System;
using MySql.Data.MySqlClient;

namespace SAM2020.Models
{
    public class Preference
    {
      private static Preference instance = null;
      public DateTime paperSubmission { get; set; }
      public DateTime reviewSubmission { get; set; }
      public DateTime reviewChoice { get; set; }
      public DateTime authorNotification { get; set; }
      public static Preference Instance
        {  
            get  
            {  
                if (instance == null)  
                {  
                    instance = new Preference(); 
                }  
                return instance;  
            }  
        }  
        public void getPreferences() {  
          try {
                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                MySqlDataReader myReader;
                comm.CommandText = "SELECT * FROM PREFERENCES LIMIT 1";
                myReader = comm.ExecuteReader();
                try {
                  while(myReader.Read()) {
                    paperSubmission = myReader.GetDateTime(1);
                    reviewSubmission = myReader.GetDateTime(2);
                    reviewChoice = myReader.GetDateTime(3);
                    authorNotification = myReader.GetDateTime(4);
                  }
                } finally {
                  myReader.Close();
                  conn.Close();
                }
            } catch (Exception e) {
              Console.WriteLine(e);
            }
        }

        public int updatePreferences() {
          int result = -1;

          try
            {
              MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
              conn.Open();
              MySqlCommand comm = conn.CreateCommand();
              comm.CommandText = "UPDATE PREFERENCES SET paper_submission=@paperSubmission, review_submission=@reviewSubmission, review_choice=@reviewChoice, author_notification=@authorNotification WHERE preference_id=1";
              comm.Parameters.AddWithValue("@paperSubmission", paperSubmission);
              comm.Parameters.AddWithValue("@reviewSubmission", reviewSubmission);
              comm.Parameters.AddWithValue("@reviewChoice", reviewChoice);
              comm.Parameters.AddWithValue("@authorNotification", authorNotification);
              result = comm.ExecuteNonQuery();
              conn.Close();
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }

          return result;
        }
    }
}