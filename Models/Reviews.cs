using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;

namespace SAM2020.Models
{
    public class Reviews
    {
      public int createReviews(int reviewerId, Array papers)
        {
            int operationStatus = -1;
            var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string baseSqlCommand = "INSERT INTO review(paper_reference_name, reviewer_id, created_date, edited_date, text) values";

            foreach (string paperReferenceName in papers)
            {
                string insertion = "('"+ paperReferenceName + "', '" + reviewerId + "', '" + date + "', '" + date + "', ''),";
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

      public Dictionary<string, Review> getReviewsIdsByUser(string reviewerId)
      {
          Dictionary<string, Review> userReviews = new Dictionary<string, Review>() {};

          try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              MySqlDataReader dataReader;
              SQLCommand.CommandText = @"
                SELECT review_id, paper_reference_name, edited_date, text
                FROM review
                WHERE reviewer_id=@reviewerId";
              SQLCommand.Parameters.AddWithValue("@reviewerId", reviewerId);
              dataReader = SQLCommand.ExecuteReader();

              try
              {
                  while (dataReader.Read())
                  {
                    Review review = new Review();

                    review.id = dataReader.GetInt32(0);
                    review.paperReferenceName = dataReader.GetString(1);
                    review.editedDate = dataReader.GetDateTime(2);
                    review.text = dataReader.GetString(3);

                    userReviews[review.paperReferenceName] = review;
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

          return userReviews;
      }

      public int updateReview(Review review)
      {
          int operationResult = -1;

          try
          {
              MySqlConnection DBconnection = new MySqlConnection(DBConnect.MyConString);
              DBconnection.Open();
              MySqlCommand SQLCommand = DBconnection.CreateCommand();
              SQLCommand.CommandText = @"
                UPDATE review
                SET text=@reviewText
                WHERE review_id=@reviewId;";
              SQLCommand.Parameters.AddWithValue("@reviewText", review.text);
              SQLCommand.Parameters.AddWithValue("@reviewId", review.id);
              operationResult = SQLCommand.ExecuteNonQuery();
              DBconnection.Close();
          }
          catch (Exception e)
          {
              Console.WriteLine(e.Message);
          }

          return operationResult;
      }

    }
}
