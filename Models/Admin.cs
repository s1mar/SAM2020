using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;

namespace SAM2020.Models
{
    public class Admin : User {
        public List<string> getUsers() {
          List<string> usersList = new List<string>();
      
          try {
            MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
            DBconnection.Open();
            MySqlCommand SQLCommand = DBconnection.CreateCommand();
            MySqlDataReader dataReader;
            SQLCommand.CommandText = "SELECT * FROM USER";
            dataReader = SQLCommand.ExecuteReader();

            try {
              while(dataReader.Read()) {
                usersList.Add(dataReader.GetString(1));
              }
            } finally {
              dataReader.Close();
              DBconnection.Close();
            }
          } catch (Exception e) {
            Console.WriteLine(e.Message);
          }

          return usersList;
        }

        public int createUser(User user) {
            int operationStatus = -1;
            int userID = findUser(user.userEmail);

            if (userID != 0) {
              return operationStatus;
            }

            try {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand sqlCommand = DBconnection.CreateCommand();
              sqlCommand.CommandText = "INSERT INTO user(username,password,role_id) VALUES(@username, @password, @role)";
              sqlCommand.Parameters.AddWithValue("@username", user.userEmail);
              sqlCommand.Parameters.AddWithValue("@password", user.password);
              sqlCommand.Parameters.AddWithValue("@role", user.role);
              operationStatus = sqlCommand.ExecuteNonQuery();
              DBconnection.Close(); 
            }
            catch (Exception e) {
              Console.WriteLine(e.Message);
            }
          
          return operationStatus;
        }

        public int deleteUser(String userEmail) {
            int operationStatus = -1;
            int userID = findUser(userEmail);

            if (userID == -1) {
              return operationStatus;
            }

            try {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand sqlCommand = DBconnection.CreateCommand();
              sqlCommand.CommandText = "DELETE FROM USER WHERE username=@username";
              sqlCommand.Parameters.AddWithValue("@username", userEmail);
              operationStatus = sqlCommand.ExecuteNonQuery();
              DBconnection.Close(); 
            }
            catch (Exception e) {
              Console.WriteLine(e.Message);
            }

          return operationStatus;
        }


        public void modifyUser() {
          
        }
    }
}
