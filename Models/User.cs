﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace SAM2020.Models
{
    public class User
    {
        public int id { get; set; }
        [BindProperty]
        [Required]
        public string userEmail { get; set; }
        [BindProperty]
        [Required]
        public string name { get; set; }
        [BindProperty]
        [Required]
        public string password { get; set; }
        [BindProperty]
        [Required]
        public string role { get; set; }
     
        public int addNewUser(String userEmail , String password,String name)
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
                comm.CommandText = "INSERT INTO user(username,password,role_id,name) VALUES(@username, @password, @role_id,@name)";
                comm.Parameters.AddWithValue("@username", userEmail);
                comm.Parameters.AddWithValue("@password", password);
                comm.Parameters.AddWithValue("@name", name);
                comm.Parameters.AddWithValue("@role_id", Roles.AUTHOR); // role author by default
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
            int userID = -1;

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

        public int getUserRole(String userID)
        {
            int userRole = 0;



            try
            {
                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select role_id from user where user_id=@user_id";
                comm.Parameters.AddWithValue("@user_id", userID);
                userRole = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }

            return userRole;
        }

        public string getUserNameByID(String userID)
        {
            string name = "";



            try
            {
                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select name from user where user_id=@user_id";
                comm.Parameters.AddWithValue("@user_id", userID);
                name = Convert.ToString(comm.ExecuteScalar());
                conn.Close();
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }

            return name;
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


        /*
         *  Get list of users IDS that has specific rule such us all authors or PCCs, or PCMs
         * */
        public List<string> getAllUsersIDs(int roleType)
        {
            List<string> usersList = new List<string>();

            try
            {
                MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
                DBconnection.Open();
                MySqlCommand SQLCommand = DBconnection.CreateCommand();
                MySqlDataReader dataReader;
                SQLCommand.CommandText = "SELECT user_id  FROM USER where role_id=@role_id";
                SQLCommand.Parameters.AddWithValue("@role_id", roleType);
                dataReader = SQLCommand.ExecuteReader();

                try
                {
                    while (dataReader.Read())
                    {
                        usersList.Add(dataReader.GetString(0));
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

            return usersList;
        }
    }
}
