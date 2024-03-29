using System;
using System.Collections.Generic;
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
              sqlCommand.CommandText = @"
                INSERT INTO paper(author_id, title, reference_name, topic, co_authors, version, file_reference, submission_date, status, final_review)
                VALUES(@authorId, @title, @referenceName, @topic, @coAuthors, @version, @fileReference, @submissionDate, @status, @finalReview)";
              sqlCommand.Parameters.AddWithValue("@authorId", paper.author);
              sqlCommand.Parameters.AddWithValue("@title", paper.title);
              sqlCommand.Parameters.AddWithValue("@referenceName", paper.referenceName);
              sqlCommand.Parameters.AddWithValue("@topic", paper.topic);
              sqlCommand.Parameters.AddWithValue("@coAuthors", paper.coAuthors);
              sqlCommand.Parameters.AddWithValue("@version", paper.version);
              sqlCommand.Parameters.AddWithValue("@fileReference", paper.fileReference);
              sqlCommand.Parameters.AddWithValue("@submissionDate", paper.submissionDate);
              sqlCommand.Parameters.AddWithValue("@status", paper.status);
              sqlCommand.Parameters.AddWithValue("@finalReview", paper.finalReview);
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
          Dictionary<string, List<string>> paperFiles = new Dictionary<string, List<string>>() {};

          try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              MySqlDataReader dataReader;
              SQLCommand.CommandText = @"
                SELECT paper_id, title, co_authors, topic, author_id, version, file_reference, submission_date, status, reference_name, final_review
                FROM paper
                WHERE author_id=@authorId
                ORDER BY version DESC";
              SQLCommand.Parameters.AddWithValue("@authorId", authorId);
              dataReader = SQLCommand.ExecuteReader();

              try
              {
                  while (dataReader.Read())
                  {
                    Paper paper = new Paper();
                    string paperId = dataReader.GetString(9);
                    string fileReference = dataReader.GetString(6);

                    if (!paperFiles.ContainsKey(paperId))
                    {
                      paper.id = dataReader.GetInt32(0);
                      paper.title = dataReader.GetString(1);
                      paper.coAuthors = dataReader.GetString(2);
                      paper.topic = dataReader.GetString(3);
                      paper.author = dataReader.GetInt32(4);
                      paper.version = dataReader.GetInt32(5);
                      paper.fileReference = dataReader.GetString(6);
                      paper.submissionDate = dataReader.GetDateTime(7);
                      paper.status = dataReader.GetInt32(8);
                      paper.referenceName = dataReader.GetString(9);
                      paper.finalReview = dataReader.GetString(10);
                      // Add paper to the user papers list
                      userPapers.Add(paper);
                      // Add file reference to
                      paperFiles[paperId] = new List<string>(){ paper.fileReference };
                    } else {
                      List<string> files = paperFiles[paperId];
                      files.Add(fileReference);
                      paperFiles[paperId] = files;
                    }

                    paper.fileList = paperFiles[paperId];
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
          // Sort papers in by version ASC.
          foreach(var paper in userPapers) {
            paper.fileList.Reverse();
          }

          return userPapers;
      }

      public List<Paper> getAllPapers()
      {
          List<Paper> userPapers = new List<Paper>();
          List<string> papersIds = new List<string>();

          try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              MySqlDataReader dataReader;
              SQLCommand.CommandText = @"
              SELECT p.paper_id, p.reference_name, p.title, p.co_authors, p.topic, p.author_id, p.version, p.file_reference, p.submission_date, p.status, u.name
              FROM paper p, user u
              WHERE p.author_id = u.user_id
              ORDER BY p.version DESC";
              dataReader = SQLCommand.ExecuteReader();

              try
              {
                  while (dataReader.Read())
                  {
                    if (!papersIds.Contains(dataReader.GetString(1)))
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
                      papersIds.Add(dataReader.GetString(1));
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
              SQLCommand.CommandText = @"
                SELECT paper_id
                FROM preselection
                WHERE user_id=@userId";
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

      public List<Paper> getReviewsPapersByUser(string reviewerId)
      {
          List<Paper> userPapers = new List<Paper>();
          Dictionary<string, List<string>> paperFiles = new Dictionary<string, List<string>>() {};

          try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              MySqlDataReader dataReader;
              SQLCommand.CommandText = @"
                SELECT p.paper_id, p.title, p.co_authors, p.topic, p.author_id, p.version, p.file_reference, p.submission_date, p.status, p.reference_name
                FROM paper p, review r
                WHERE r.reviewer_id=@reviewerId AND p.reference_name=r.paper_reference_name
                ORDER BY version DESC";
              SQLCommand.Parameters.AddWithValue("@reviewerId", reviewerId);
              dataReader = SQLCommand.ExecuteReader();

              try
              {
                  while (dataReader.Read())
                  {
                    Paper paper = new Paper();
                    string paperId = dataReader.GetString(9);
                    string fileReference = dataReader.GetString(6);

                    if (!paperFiles.ContainsKey(paperId))
                    {
                      paper.id = dataReader.GetInt32(0);
                      paper.title = dataReader.GetString(1);
                      paper.coAuthors = dataReader.GetString(2);
                      paper.topic = dataReader.GetString(3);
                      paper.author = dataReader.GetInt32(4);
                      paper.version = dataReader.GetInt32(5);
                      paper.fileReference = dataReader.GetString(6);
                      paper.submissionDate = dataReader.GetDateTime(7);
                      paper.status = dataReader.GetInt32(8);
                      paper.referenceName = dataReader.GetString(9);
                      // Add paper to the user papers list
                      userPapers.Add(paper);
                      // Add file reference to
                      paperFiles[paperId] = new List<string>(){ paper.fileReference };
                    } else {
                      List<string> files = paperFiles[paperId];
                      files.Add(fileReference);
                      paperFiles[paperId] = files;
                    }

                    paper.fileList = paperFiles[paperId];
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
          // Sort papers in by version ASC.
          foreach(var paper in userPapers) {
            paper.fileList.Reverse();
          }

          return userPapers;
      }

      public List<Paper> getReviewsPapers()
      {
          List<Paper> userPapers = new List<Paper>();
          Dictionary<string, List<string>> paperFiles = new Dictionary<string, List<string>>() {};

          try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              MySqlDataReader dataReader;
              SQLCommand.CommandText = @"
                SELECT p.paper_id, p.title, p.co_authors, p.topic, p.author_id, p.version, p.file_reference, p.submission_date, p.status, p.reference_name, p.final_review
                FROM paper p, review r
                WHERE p.reference_name=r.paper_reference_name
                ORDER BY version DESC";
              dataReader = SQLCommand.ExecuteReader();

              try
              {
                  while (dataReader.Read())
                  {
                    Paper paper = new Paper();
                    string paperId = dataReader.GetString(9);
                    string fileReference = dataReader.GetString(6);

                    if (!paperFiles.ContainsKey(paperId))
                    {
                      paper.id = dataReader.GetInt32(0);
                      paper.title = dataReader.GetString(1);
                      paper.coAuthors = dataReader.GetString(2);
                      paper.topic = dataReader.GetString(3);
                      paper.author = dataReader.GetInt32(4);
                      paper.version = dataReader.GetInt32(5);
                      paper.fileReference = dataReader.GetString(6);
                      paper.submissionDate = dataReader.GetDateTime(7);
                      paper.status = dataReader.GetInt32(8);
                      paper.referenceName = dataReader.GetString(9);
                      paper.finalReview = dataReader.GetString(10);
                      // Add paper to the user papers list
                      userPapers.Add(paper);
                      // Add file reference to
                      paperFiles[paperId] = new List<string>(){ paper.fileReference };
                    } else {
                      List<string> files = paperFiles[paperId];
                      files.Add(fileReference);
                      paperFiles[paperId] = files;
                    }

                    paper.fileList = paperFiles[paperId];
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
          // Sort papers in by version ASC.
          foreach(var paper in userPapers) {
            paper.fileList.Reverse();
          }

          return userPapers;
      }

      public int updateReview(string paperId, string finalReview, string status)
      {
          int operationResult = -1;

          try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              SQLCommand.CommandText = @"
                UPDATE paper
                SET final_review=@finalReview, status=@status
                WHERE paper_id=@paperId;";
              SQLCommand.Parameters.AddWithValue("@finalReview", finalReview);
              SQLCommand.Parameters.AddWithValue("@paperId", paperId);
              SQLCommand.Parameters.AddWithValue("@status", status);
              operationResult = SQLCommand.ExecuteNonQuery();
              DBconnection.Close();
          }
          catch (Exception e)
          {
              Console.WriteLine(e.Message);
          }

          return operationResult;
      }

        public int getAuthorByPaperID(string paperId)
        {
            int userID = 0;



            try
            {
                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select author_id from paper where  paper_id=@paperId";
                comm.Parameters.AddWithValue("@paper_id", paperId);
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
