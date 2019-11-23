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
    public class AssignPapersDetailModel : PageModel
    {
        [BindProperty]
        public int selectedUserId { get; set; }
        [BindProperty]
        public List<Paper> allPapers { get; set; }
        [BindProperty]
        public List<Paper> userAssignedPapers { get; set; }
        public List<int> userPreselection { get; set; }
        
        public async Task<IActionResult> OnGet() {
          string user = HttpContext.Session.GetString("userID");

          // If the user is not logged in
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

          selectedUserId = Int32.Parse(Request.Query["userid"]);
          Users users = new Users();

          if (!users.isValidPcmUser(selectedUserId)) {
            return RedirectToPage(Routes.ASSIGN_PAPERS);
          }

          Papers papers = new Papers();
          allPapers = papers.getAllPapers();
          userAssignedPapers = papers.getUserAssignedPapers(selectedUserId);
          userPreselection = papers.getUserPreselection(selectedUserId);

          return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Reviews reviews = new Reviews();
            string userId = HttpContext.Session.GetString("userID");
            var selectedPapers = Request.Form["CheckedPapers"];
            string actualUrl = Request.Path;
            selectedUserId = Int32.Parse(Request.Query["userid"]);

            if (String.IsNullOrEmpty(selectedPapers)) {
              return RedirectToPage(Routes.ASSIGN_PAPER_DETAIL, new { id = 3, userId = selectedUserId });
            }

            int result = reviews.createReviews(selectedUserId, selectedPapers);

            if (result == -1) {
      
                return RedirectToPage(Routes.ASSIGN_PAPER_DETAIL, new { id = 2, userId = selectedUserId });
            }else if (result != -1){
                //Notify Author he has paper to review
                Notification notificationManager = new Notification();
                int status = notificationManager.sendNotifiactionToOneUser("You have New assigned <a href=\"/MyAssignedPapers\">paper</a> to review", selectedUserId, Convert.ToInt32(HttpContext.Session.GetString("userID")));

            }
            // TODO: Create some constants files 
            return RedirectToPage(Routes.ASSIGN_PAPER_DETAIL, new { id = 1, userId = selectedUserId });
        }
    }
}

