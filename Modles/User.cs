﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;

namespace SAM2020.Modles
{
    public class User
    {
        [Required]
        public string userEmail { get; set; }

        [Required]
        public string password { get; set; }

        public int addNewUser(String userEmail , String password)
        {
            int addedSuccess = 0;

            // Mkae sure user not already in database
            int userID = findUser(userEmail);
            if (userID != 0)
            {
                return -1;
            }

            try
            {
                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "INSERT INTO user(username,password) VALUES(@username, @password)";
                comm.Parameters.AddWithValue("@username", userEmail);
                comm.Parameters.AddWithValue("@password", password);
                comm.ExecuteNonQuery();
                conn.Close();
                addedSuccess = 1;
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }

            return addedSuccess;
        }

        public int login(String userEmail, String password)
        {
            int userID = 0;

         

            try
            {
                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select user_id from user where username=@username and password=@password";
                comm.Parameters.AddWithValue("@username", userEmail);
                comm.Parameters.AddWithValue("@password", password);
                userID = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }

            return userID;
        }

        public int findUser(String userEmail)
        {
            int userID = 0;



            try
            {
                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select user_id from user where username=@username";
                comm.Parameters.AddWithValue("@username", userEmail);
                userID = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }

            return userID;
        }
    }
}
