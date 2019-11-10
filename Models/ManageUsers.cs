using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace SAM2020.Models
{
    public class ManageUsers
    {
        public List<string> getUsers() {
          
          List<string> users = new List<string>();
      
          try {
                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                MySqlDataReader myReader;
                comm.CommandText = "SELECT * FROM USER";
                myReader = comm.ExecuteReader();
                try {
                  while(myReader.Read()) {
                    users.Add(myReader.GetString(1));
                  }
                } finally {
                  myReader.Close();
                  conn.Close();
                }
            } catch (Exception e) {
              Console.WriteLine(e);
            }

            return users;
        }
    }
}