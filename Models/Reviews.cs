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
            string baseSqlCommand = "INSERT INTO review(paper_reference_name, reviewer_id, created_date, edited_date) values";

            foreach (string paperReferenceName in papers)
            {
                string insertion = "('"+ paperReferenceName + "', '" + reviewerId + "', '" + date + "', '" + date + "'),";
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

    }
}
