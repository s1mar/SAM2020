using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;

namespace SAM2020.Models
{
    public class Admin : User {
        public List<User> getUsers() {
          List<User> usersList = new List<User>();
      
          try {
            MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
            DBconnection.Open();
            MySqlCommand SQLCommand = DBconnection.CreateCommand();
            MySqlDataReader dataReader;
            SQLCommand.CommandText = @"
              SELECT user_id, name, username, role_id, password
              FROM user";
            dataReader = SQLCommand.ExecuteReader();

            try {
              while(dataReader.Read()) {
                User user = new User();
                user.id = dataReader.GetInt32(0);
                user.name = dataReader.GetString(1);
                user.userEmail = dataReader.GetString(2);
                user.role = dataReader.GetString(3);
                user.password = dataReader.GetString(4);

                usersList.Add(user);
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
              sqlCommand.CommandText = @"
                INSERT INTO user(username, name, password, role_id)
                VALUES(@username, @name, @password, @role)";
              sqlCommand.Parameters.AddWithValue("@username", user.userEmail);
              sqlCommand.Parameters.AddWithValue("@name", user.name);
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

        public int deleteUser(string userEmail) {
            int operationStatus = -1;

            try {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand sqlCommand = DBconnection.CreateCommand();
              sqlCommand.CommandText = @"
                DELETE FROM USER
                WHERE username=@username";
              sqlCommand.Parameters.AddWithValue("@username", userEmail);
              operationStatus = sqlCommand.ExecuteNonQuery();
              DBconnection.Close(); 
            }
            catch (Exception e) {
              Console.WriteLine(e.Message);
            }

          return operationStatus;
        }


        public int modifyUser(User user) {
            int operationStatus = -1;

            try {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand sqlCommand = DBconnection.CreateCommand();
              sqlCommand.CommandText = @"
                UPDATE user
                SET username=@username, name=@name, password=@password, role_id=@role
                WHERE user_id=@userId";
              sqlCommand.Parameters.AddWithValue("@username", user.userEmail);
              sqlCommand.Parameters.AddWithValue("@name", user.name);
              sqlCommand.Parameters.AddWithValue("@password", user.password);
              sqlCommand.Parameters.AddWithValue("@role", user.role);
              sqlCommand.Parameters.AddWithValue("@userId", user.id);
              operationStatus = sqlCommand.ExecuteNonQuery();
              DBconnection.Close(); 
            }
            catch (Exception e) {
              Console.WriteLine(e.Message);
            }
          
          return operationStatus;
        }

    }
}
