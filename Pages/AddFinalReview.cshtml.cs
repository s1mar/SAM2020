using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
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

          if (string.IsNullOrEmpty(user)) {
            return RedirectToPage(Routes.LOGIN);
          }

          int userRole = -1;
          if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userRole"))) {
            userRole = Convert.ToInt32(HttpContext.Session.GetString("userRole"));
          }

          // If the user is not a PCM or Admin, redirect to the main menu
          if (userRole != Roles.PCC & userRole != Roles.ADMIN) {
            return RedirectToPage(Routes.INDEX);
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
        public async Task<IActionResult> OnPostConflictAsync()
        {
            Reviews reviews = new Reviews();
            Papers papers = new Papers();
            papersReviews = reviews.getPapersReviews();
            var selectedReviewer = Request.Form["selectedReviewer"];
            string paperTitle = Request.Form["paper_title"];
            string paperReference = Request.Form["paper_reference"];
            string conflictMessage = Request.Form["conflict_message"];
            string senderId = HttpContext.Session.GetString("userID");
            string message = @"
              Your review has conflicting views.
              Paper: @paperTitle
              Message: @conflictMessage
              Users: ";

            List<string> users = new List<string>();

            foreach(Review review in papersReviews[paperReference])
            {
              if (Array.IndexOf(selectedReviewer, review.reviewerId) != -1) {
                string username = review.reviewerName + " - " + review.reviewerEmail;
                users.Add(username);
              }

            }

            message = message + String.Join(", ", users.ToArray());;
            message = message.Replace("@paperTitle", paperTitle);
            message = message.Replace("@conflictMessage", conflictMessage);

            if (String.IsNullOrEmpty(selectedReviewer)) {
              return RedirectToPage(Routes.ADD_FINAL_REVIEW, new { id = 5 });
            }

            Notification notification = new Notification();

            int status = notification.notifyConflict(senderId, message, selectedReviewer);

            if (status == -1)
            {
              return RedirectToPage(Routes.ADD_FINAL_REVIEW, new { id = 6 });
            }

            return RedirectToPage(Routes.ADD_FINAL_REVIEW, new { id = 4 });
        }
    }
}

