using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace SAM2020.Modles
{
    public class DBConnect
    {
        private static String MyConString = "SERVER=localhost;DATABASE=sam2020;UID=root;PASSWORD=PASSWORD;";

        public static MySqlConnection GetConnection()
        {
            string cs = MyConString;
            MySqlConnection conn = new MySqlConnection(cs);

            return conn;
        }

        public static int InsertDocument(MySqlConnection conn, byte[] fileData, int paperId, String fileName, String extension)
        {

            MySqlCommand comm = conn.CreateCommand();
            comm.CommandText = "INSERT INTO paper_documents (paper_id, document, filename, versionNum, extension, createdDate) " +
                "VALUES (@paper_id, @document, @filename, @versionNum, @extension, @createdDate)";
            comm.Parameters.AddWithValue("@paper_id", paperId);
            comm.Parameters.AddWithValue("@document", fileData);
            comm.Parameters.AddWithValue("@filename", fileName);
            comm.Parameters.AddWithValue("@versionNum", 1);
            comm.Parameters.AddWithValue("@extension", extension);
            comm.Parameters.AddWithValue("@createdDate", fileData);
            comm.ExecuteNonQuery();

            return 1;
        }

        public static int InsertDocument(MySqlConnection conn, String filePath, int paperId, String fileName, String extension)
        {
            byte[] fileData = ReadFile(filePath);

            MySqlCommand comm = conn.CreateCommand();
            comm.CommandText = "INSERT INTO paper_documents (paper_id, document, filename, versionNum, extension, createdDate) " +
                "VALUES (@paper_id, @document, @filename, @versionNum, @extension, @createdDate)";
            comm.Parameters.AddWithValue("@paper_id", paperId);
            comm.Parameters.AddWithValue("@document", fileData);
            comm.Parameters.AddWithValue("@filename", fileName);
            comm.Parameters.AddWithValue("@versionNum", 1);
            comm.Parameters.AddWithValue("@extension", extension);
            comm.Parameters.AddWithValue("@createdDate", fileData);
            comm.ExecuteNonQuery();

            return 1;
        }

        public static byte[] ReadFile(string sPath)
        {
            //Initialize byte array with a null value initially.
            byte[] data = null;

            //Use FileInfo object to get file size.
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;

            //Open FileStream to read file
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);

            //Use BinaryReader to read file stream into byte array.
            BinaryReader br = new BinaryReader(fStream);

            //When you use BinaryReader, you need to supply number of bytes to read from file.
            //In this case we want to read entire file. So supplying total number of bytes.
            data = br.ReadBytes((int)numBytes);

            //Close BinaryReader
            br.Close();

            //Close FileStream
            fStream.Close();

            return data;
        }
    }
}
