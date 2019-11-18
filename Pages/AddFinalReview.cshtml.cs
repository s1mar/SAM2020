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
    public class AddFinalReviewModel : PageModel
    {
        [BindProperty]
        public List<Paper> papersList { get; set; }
        public Dictionary<string, List<Review>> papersReviews {get; set;}

        public async Task<IActionResult> OnGet() {
          string user = HttpContext.Session.GetString("userID");
          int userRole = (int)HttpContext.Session.GetInt32("userRole");

          if (string.IsNullOrEmpty(user)) {
            return RedirectToPage(Routes.INDEX);
          }

          // If the user is not a PCM or Admin, redirect to the main menu
          if (userRole != (int)UserRole.PCM & userRole != (int)UserRole.Admin) {
            return RedirectToPage(Routes.CONTROL_PANEL);
          }

          Reviews reviews = new Reviews();
          Papers papers = new Papers();
          papersReviews = reviews.getPapersReviews();
          papersList = papers.getReviewsPapers();

          return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var paperId = Request.Form["paper_id"];
            var reviewText = Request.Form["review_text"];
            var paperStatus = Request.Form["status"];

            if (String.IsNullOrEmpty(reviewText))
            {
                return RedirectToPage(Routes.ADD_FINAL_REVIEW, new { id = 2 });
            }

            Papers papers = new Papers();

            int status = papers.updateReview(paperId, reviewText, paperStatus);

            if (status != -1) {
                //Notify Authorabout paper decision
                Notification notificationManager = new Notification();
                int statusPaper = notificationManager.sendNotifiactionToOneUser("Your paper has been "+(paperStatus=="2"?"approved":"reject"), papers.getAuthorByPaperID(paperId), Convert.ToInt32(HttpContext.Session.GetString("userID")));

                return RedirectToPage(Routes.ADD_FINAL_REVIEW, new { id = 1 });
            }

            return RedirectToPage(Routes.ADD_FINAL_REVIEW, new { id = 3 });
        }
    }
}

