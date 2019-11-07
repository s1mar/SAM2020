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
                File.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            return Page();
        }
    }
}
