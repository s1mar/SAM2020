using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;

namespace SAM2020.Models
{
    public class Papers
    {
        public int addNewPaper(Paper paper)
        {
            int operationStatus = -1;

            try {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand sqlCommand = DBconnection.CreateCommand();
              sqlCommand.CommandText = "INSERT INTO paper(author_id, title, topic, co_authors, version, file_reference, submission_date, status) VALUES(@authorId, @title, @topic, @coAuthors, @version, @fileReference, @submissionDate, @status)";
              sqlCommand.Parameters.AddWithValue("@authorId", paper.author);
              sqlCommand.Parameters.AddWithValue("@title", paper.title);
              sqlCommand.Parameters.AddWithValue("@topic", paper.topic);
              sqlCommand.Parameters.AddWithValue("@coAuthors", paper.coAuthors);
              sqlCommand.Parameters.AddWithValue("@version", paper.version);
              sqlCommand.Parameters.AddWithValue("@fileReference", paper.fileReference);
              sqlCommand.Parameters.AddWithValue("@submissionDate", paper.submissionDate);
              sqlCommand.Parameters.AddWithValue("@status", paper.status);
              operationStatus = sqlCommand.ExecuteNonQuery();
              DBconnection.Close(); 
            }
            catch (Exception e) {
              Console.WriteLine(e.Message);
            }
          
          return operationStatus;
      }

      public List<Paper> getPapersByUser(string authorId)
      {
          List<Paper> userPapers = new List<Paper>();

          try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              MySqlDataReader dataReader;
              SQLCommand.CommandText = "SELECT *  FROM paper where author_id=@authorId";
              SQLCommand.Parameters.AddWithValue("@authorId", authorId);
              dataReader = SQLCommand.ExecuteReader();

              try
              {
                  while (dataReader.Read())
                  {
                      Paper paper = new Paper();
                      paper.id = dataReader.GetInt32(0);
                      paper.title = dataReader.GetString(1);
                      paper.coAuthors = dataReader.GetString(2);
                      paper.topic = dataReader.GetString(3);
                      paper.author = dataReader.GetInt32(4);
                      paper.version = dataReader.GetInt32(5);
                      paper.fileReference = dataReader.GetString(6);
                      paper.submissionDate = dataReader.GetDateTime(7);
                      paper.status = dataReader.GetInt32(8);
                      userPapers.Add(paper);
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

          return userPapers;
      }
    }
}
