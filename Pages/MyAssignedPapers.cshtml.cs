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
    public class MyAssignedPapersModel : PageModel
    {
        [BindProperty]
        public List<Paper> reviewsPapers { get; set; }
        public Dictionary<string, Review> userReviews { get; set; }

        public async Task<IActionResult> OnGet() {
          string user = HttpContext.Session.GetString("userID");
          int userRole = (int)HttpContext.Session.GetInt32("userRole");

          if (string.IsNullOrEmpty(user)) {
            return RedirectToPage(Routes.LOGIN);
          }

          // If the user is not a PCM or Admin, redirect to the main menu
          if (userRole != Roles.PCM & userRole != Roles.ADMIN) {
            return RedirectToPage(Routes.CONTROL_PANEL);
          }

          Reviews reviews = new Reviews();
          Papers papers = new Papers();
          userReviews = reviews.getReviewsIdsByUser(user);
          reviewsPapers = papers.getReviewsPapersByUser(user);

          return null;
        }

        public async Task<IActionResult> OnPostAsync(IFormFile newFile)
        {
            var reviewId = Int32.Parse(Request.Form["review_id"]);
            var reviewText = Request.Form["review_text"];

            if (String.IsNullOrEmpty(reviewText))
            {
                return RedirectToPage(Routes.MY_ASSIGNED_PAPERS, new { id = 2 });
            }

            Reviews reviews = new Reviews();
            Review review = new Review();
            review.id = reviewId;
            review.text = reviewText;

            int status = reviews.updateReview(review);

            if (status != -1) {
                return RedirectToPage(Routes.MY_ASSIGNED_PAPERS, new { id = 1 });
            }

            return RedirectToPage(Routes.MY_ASSIGNED_PAPERS, new { id = 3 });
        }
    }
}

