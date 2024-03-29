﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using SAM2020.Models;

namespace SAM2020.Pages
{
    public class SubmitPaperModel : PageModel
    {
        [BindProperty]
        public Paper paper { get; set; }
        [BindProperty]
        [Required]
        public IFormFile File { set; get; }
        public async Task<IActionResult> OnGet() {
          string user = HttpContext.Session.GetString("userID");

          if (string.IsNullOrEmpty(user)) {
            return RedirectToPage(Routes.LOGIN);
          }

          int userRole = -1;
          if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userRole"))) {
            userRole = Convert.ToInt32(HttpContext.Session.GetString("userRole"));
          }

          // If the user is not a PCM or Admin, redirect to the main menu
          if (userRole != Roles.AUTHOR & userRole != Roles.PCM & userRole != Roles.ADMIN) {
            return RedirectToPage(Routes.INDEX);
          }

          return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int  userID  = Convert.ToInt32(HttpContext.Session.GetString("userID"));
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // If all the information on the form is valid.
            var fileExtension = System.IO.Path.GetExtension(File.FileName);
            var referenceGuid = Guid.NewGuid().ToString();
            paper.status = 1;
            paper.author = userID;
            paper.version = 1;
            paper.finalReview = "";
            paper.referenceName = referenceGuid;
            paper.fileReference = referenceGuid + "-" + paper.version.ToString() + fileExtension;
            paper.submissionDate = DateTime.Now;

            // Save information
            Papers papers = new Papers();

            int status = papers.addNewPaper(paper);

            // Create the file.
            if (status != -1) {
                var filePath = "wwwroot/papers/" + paper.fileReference;
                File.CopyTo(new FileStream(filePath, FileMode.Create));

                Notification notificationManager = new Notification();
                //Notify PCC new paper has been submitted
                notificationManager.sendNotifiactionAll(" New Paper Submitted by " + HttpContext.Session.GetString("userEmail"), Roles.PCC, userID);
                //Notify Author his paper has been submitted
                notificationManager.sendNotifiactionToOneUser("Your paper has been submitted successfully", userID, userID);
                return RedirectToPage(Routes.SUBMIT_PAPER, new { id = 1 }); 
            }

            return RedirectToPage(Routes.SUBMIT_PAPER, new { id = 2 }); 
        }
    }
}

