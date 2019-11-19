using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;

namespace SAM2020.Models
{
    public class Users
    {
      public List<User> getUsersByRole(int role)
      {
          List<User> userList = new List<User>();

          try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              MySqlDataReader dataReader;
              SQLCommand.CommandText = "SELECT user_id, username, name from USER WHERE role_id=@userRole";
              SQLCommand.Parameters.AddWithValue("@userRole", role);
              dataReader = SQLCommand.ExecuteReader();

              try
              {
                  while (dataReader.Read())
                  {
                      User user = new User();
                      user.id = dataReader.GetInt32(0);
                      user.userEmail = dataReader.GetString(1);
                      user.name = dataReader.GetString(2);
                      userList.Add(user);
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

          return userList;
      }

      public int preSelectPapers(string userId, Array papers)
        {
            int operationStatus = -1;
            string baseSqlCommand = "INSERT INTO preselection(user_id, paper_id) values";

            foreach (string paperId in papers)
            {
                string insertion = "("+ userId + ", " + paperId + "),";
                baseSqlCommand = baseSqlCommand + insertion;
            }

            baseSqlCommand = baseSqlCommand.Remove(baseSqlCommand.Length - 1);
            baseSqlCommand = baseSqlCommand + ';';

            try {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand sqlCommand = DBconnection.CreateCommand();
              sqlCommand.CommandText = baseSqlCommand;
              operationStatus = sqlCommand.ExecuteNonQuery();
              DBconnection.Close();
            }
            catch (Exception e) {
              Console.WriteLine(e.Message);
            }

          return operationStatus;
      }

      public List<int> getPreselectedPapers(string userId)
      {
          List<int> userPreselection = new List<int>();

          try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              MySqlDataReader dataReader;
              SQLCommand.CommandText = "SELECT paper_id  FROM preselection where user_id=@userId";
              SQLCommand.Parameters.AddWithValue("@userId", userId);
              dataReader = SQLCommand.ExecuteReader();

              try
              {
                  while (dataReader.Read())
                  {
                      userPreselection.Add(dataReader.GetInt32(0));
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

          return userPreselection;
      }

      public Boolean isValidPcmUser(int userId)
      {
          Boolean userExist = false;

          try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              MySqlDataReader dataReader;
              SQLCommand.CommandText = "SELECT * from USER WHERE user_id=@userId AND role_id=@roleId";
              SQLCommand.Parameters.AddWithValue("@userId", userId);
              SQLCommand.Parameters.AddWithValue("@roleId", Roles.PCM);
              dataReader = SQLCommand.ExecuteReader();

              try
              {
                  while (dataReader.Read())
                  {
                      userExist = true;
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

          return userExist;
      }
    }
}
