using System;
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
            int totalSubmittedPapers = 0;


            try
            {

                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select count(*)  as countPaper from paper where reference_name not in( select paper_reference_name   from( select paper_reference_name, count(*) as numberOFReviews from review group by paper_reference_name) as t )";

                totalSubmittedPapers = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return totalSubmittedPapers;
        }

        public int getTotalReviewedPapers()
        {
            int totalSubmittedPapers = 0;


            try
            {

                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select count(*) as countPaper from( select paper_reference_name, count(*) as numberOFReviews from review group by paper_reference_name) as t where numberOFReviews=3";

                totalSubmittedPapers = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return totalSubmittedPapers;
        }


       public  int getTotalAlLestOneReviewedPapers()
        {

            int totalSubmittedPapers = 0;


            try
            {

                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select count(*) as countPaper from( select paper_reference_name, count(*) as numberOFReviews from review group by paper_reference_name) as t where numberOFReviews>0 and numberOFReviews<3";

                totalSubmittedPapers = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return totalSubmittedPapers;

        }

    }
}
