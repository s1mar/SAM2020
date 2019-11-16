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
              sqlCommand.CommandText = "INSERT INTO paper(author_id, title, reference_name, topic, co_authors, version, file_reference, submission_date, status) VALUES(@authorId, @title, @referenceName, @topic, @coAuthors, @version, @fileReference, @submissionDate, @status)";
              sqlCommand.Parameters.AddWithValue("@authorId", paper.author);
              sqlCommand.Parameters.AddWithValue("@title", paper.title);
              sqlCommand.Parameters.AddWithValue("@referenceName", paper.referenceName);
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

      public List<Paper> getAllPapers()
      {
          List<Paper> userPapers = new List<Paper>();

          try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              MySqlDataReader dataReader;
              SQLCommand.CommandText = "SELECT p.paper_id, p.reference_name, p.title, p.co_authors, p.topic, p.author_id, p.version, p.file_reference, p.submission_date, p.status, u.username FROM paper p, user u Where p.author_id = u.user_id";
              dataReader = SQLCommand.ExecuteReader();

              try
              {
                  while (dataReader.Read())
                  {
                      Paper paper = new Paper();
                      paper.id = dataReader.GetInt32(0);
                      paper.referenceName = dataReader.GetString(1);
                      paper.title = dataReader.GetString(2);
                      paper.coAuthors = dataReader.GetString(3);
                      paper.topic = dataReader.GetString(4);
                      paper.author = dataReader.GetInt32(5);
                      paper.version = dataReader.GetInt32(6);
                      paper.fileReference = dataReader.GetString(7);
                      paper.submissionDate = dataReader.GetDateTime(8);
                      paper.status = dataReader.GetInt32(9);
                      paper.authorName = dataReader.GetString(10);
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

      public Dictionary<string, List<string>> getPcmsPreselections()
      {
        Dictionary<string, List<string>> pcmPreselections = new Dictionary<string, List<string>>() {};
        try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              MySqlDataReader dataReader;
              SQLCommand.CommandText = "SELECT user_id, paper_id  FROM preselection";
              dataReader = SQLCommand.ExecuteReader();

              try
              {
                  while (dataReader.Read())
                  {
                    var userId = dataReader.GetString(0);
                    var paperId = dataReader.GetString(1);

                    if(pcmPreselections.ContainsKey(userId))
                    {
                      List<string> authorSelection = pcmPreselections[userId];
                      authorSelection.Add(paperId);
                      pcmPreselections[userId] = authorSelection;
                    }
                    else
                    {
                      pcmPreselections[userId] = new List<string>(){ paperId };
                    }
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
          return pcmPreselections;
      }

      public List<int> getUserPreselection(int userId)
      {
        List<int> userPreselection = new List<int>();
        try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              MySqlDataReader dataReader;
              SQLCommand.CommandText = "SELECT paper_id FROM preselection WHERE user_id=@userId";
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
      public List<Paper> getUserAssignedPapers(int userId)
      {
          List<Paper> userPapers = new List<Paper>();
          List<string> paperIds = new List<string>();

          try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              MySqlDataReader dataReader;
              SQLCommand.CommandText = @"
                SELECT DISTINCT
                  r.review_id, p.paper_id, r.paper_reference_name, p.title, u.name, p.version
                FROM
                  review r, paper p, user u
                WHERE
                  r.reviewer_id=4 AND r.paper_reference_name = p.reference_name AND p.author_id = u.user_id
                ORDER BY p.version DESC";
              SQLCommand.Parameters.AddWithValue("@reviewerId", userId);
              dataReader = SQLCommand.ExecuteReader();

              try
              {
                  while (dataReader.Read())
                  {
                    if (!paperIds.Contains(dataReader.GetString(2)))
                    {
                      Paper paper = new Paper();
                      paper.id = dataReader.GetInt32(1);
                      paper.referenceName = dataReader.GetString(2);
                      paperIds.Add(paper.referenceName);
                      paper.title = dataReader.GetString(3);                      
                      paper.authorName = dataReader.GetString(4);
                      paper.version = dataReader.GetInt32(5);
                      userPapers.Add(paper);
                    }
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
