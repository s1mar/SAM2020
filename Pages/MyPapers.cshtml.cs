using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using SAM2020.Models;

namespace SAM2020.Pages
{
    public class MyPapersModel : PageModel
    {
        [BindProperty]
        public List<Paper> userPapers { get; set; }

        public async Task<IActionResult> OnGet() {
          string user = HttpContext.Session.GetString("userID");
          int userRole = (int)HttpContext.Session.GetInt32("userRole");

          if (string.IsNullOrEmpty(user)) {
            return RedirectToPage(Routes.LOGIN);
          }

          // If the user is not a AUTHOR, PCM and Admin redirect to the main menu
          if (userRole != Roles.AUTHOR & userRole != Roles.PCM & userRole != Roles.ADMIN) {
            return RedirectToPage(Routes.CONTROL_PANEL);
          }

          Papers papers = new Papers();
          userPapers = papers.getPapersByUser(user);

          return null;
        }

        public async Task<IActionResult> OnPostAsync(IFormFile newFile)
        {
            var fileName = Request.Form["newFile"];
            var title = Request.Form["title"];
            var referenceName = Request.Form["reference_name"];
            var coAuthors = Request.Form["co_authors"];
            var topic = Request.Form["topic"];
            var author = Int32.Parse(Request.Form["author"]);
            var version = Int32.Parse(Request.Form["version"]) + 1;

            try
            {
                if (!ModelState.IsValid | String.IsNullOrEmpty(newFile.FileName))
                {
                    return RedirectToPage(Routes.MY_PAPERS, new { id = 1 });
                }
            }
            catch (NullReferenceException e)
            {
                return RedirectToPage(Routes.MY_PAPERS, new { id = 1 });
            }

            var fileExtension = System.IO.Path.GetExtension(newFile.FileName);
            Paper paper = new Paper();
            paper.status = 1;
            paper.author = author;
            paper.version = version;
            paper.coAuthors = coAuthors;
            paper.title = title;
            paper.topic = topic;
            paper.referenceName = referenceName;
            paper.fileReference = referenceName + "-" + paper.version.ToString() + fileExtension;
            paper.submissionDate = DateTime.Now;

            // Save information
            Papers papers = new Papers();

            int status = papers.addNewPaper(paper);

            // Create the file.
            if (status != -1) {
                var filePath = "wwwroot/papers/" + paper.fileReference;
                newFile.CopyTo(new FileStream(filePath, FileMode.Create));

                return RedirectToPage(Routes.MY_PAPERS, new { id = 3 });
            }

            return RedirectToPage(Routes.MY_PAPERS, new { id = 2 });
        }
    }
}

