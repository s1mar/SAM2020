using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;
using SAM2020.Modles;
using MySql.Data.MySqlClient;

namespace SAM2020.Pages
{
    public class TestingModel : PageModel
    {
      public IFormFile File { set; get; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (File != null)
            {
                var filePath = "wwwroot/papers/" + File.FileName;
                byte[] fileData = null;

                using (var ms = new MemoryStream())
                {
                    File.CopyTo(ms);
                    fileData = ms.ToArray();
                }

                MySqlConnection conn = null;

                try
                {
                    conn = DBConnect.GetConnection();
                    conn.Open();

                    string ext = "pdf";
                    DBConnect.InsertDocument(conn, fileData, 1, File.FileName, ext);

                    // Retrieve the document again and save it to the designated folder
                    DBConnect.GetDocumentByPaperId(conn, 1);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error: {0}", ex.ToString());
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

            return Page();
        }
    }
}

