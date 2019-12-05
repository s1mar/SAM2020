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
                comm.CommandText = "SELECT count(*) FROM paper ORDER BY version DESC";

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

       public  int getTotalNotSubittedReviews()
        {

            int totalReviews = 0;


            try
            {

                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select count(*) from review where text = ''";

                totalReviews = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return totalReviews;

        }


       public  int getTotalReviews()
        {

            int totalSubmittedReviews = 0;


            try
            {

                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select count(*) from review where text != ''";

                totalSubmittedReviews = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return totalSubmittedReviews;

        }
       public  int getTotalApprovedPapers()
        {

            int totalCount = 0;


            try
            {

                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select count(*) from paper where status=2";

                totalCount = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return totalCount;

        }

       public  int getTotalRejectedPapers()
        {

            int totalCount = 0;


            try
            {

                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select count(*) from paper where status=3";

                totalCount = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return totalCount;

        }


       public int getTotalFinalReviewedPapers()
        {

            int totalSubmittedPapers = 0;


            try
            {

                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select count(*) from paper where final_review != ''";

                totalSubmittedPapers = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return totalSubmittedPapers;

        }
       public int getTotalAuthors()
        {

            int totalCount = 0;


            try
            {

                MySqlConnection conn = new MySqlConnection(DBConnect.MyConString);
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select count(*) from user where role_id = 2";

                totalCount = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return totalCount;

        }
    }
}
