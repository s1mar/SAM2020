using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SAM2020.Models
{
    public class Reports
    {
        public int getTotalSubmittedPapers()
        {
            int totalSubmittedPapers = 0;


            try
            {

                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "SELECT  count(*) FROM paper ORDER BY version DESC";

                totalSubmittedPapers = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();
                 
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
             

            return totalSubmittedPapers;
        }


        public int getTotalNotReviewedPapers()
        {
            //TODO: Add code for not review papers
            return -1;
        }

        public int getTotalReviewedPapers()
        {
            //TODO: Add code for review papers
            return -1;
        }



    }
}
