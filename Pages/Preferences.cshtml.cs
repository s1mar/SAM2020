using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAM2020.Models;
using Microsoft.AspNetCore.Http;

namespace SAM2020.Pages
{
    public class PreferencesModel : PageModel
    {
      public Preference preference { get; set; } = Preference.Instance;
        public async Task<IActionResult> OnGet()
        {
          string user = HttpContext.Session.GetString("userID");
          int userRole = (int)HttpContext.Session.GetInt32("userRole");

          if (string.IsNullOrEmpty(user)) {
            return RedirectToPage(Routes.INDEX);
          }

          // If the user is not an Admin, redirect to the main menu
          if (userRole != (int)UserRole.Admin) {
            return RedirectToPage(Routes.CONTROL_PANEL);
          }

          preference.getPreferences();

          return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            Notification notificationManager = new Notification();
            int userID = Convert.ToInt32(HttpContext.Session.GetString("userID"));

            DateTime paperSubmission = Convert.ToDateTime(  Request.Form["paperSubmission"]);
            //update Paper Submission Deadline
            if (paperSubmission != preference.paperSubmission){
               
                notificationManager.sendNotifiactionAll(" New Paper Submission Deadline is " + paperSubmission.ToString(), (int)UserRole.Author, userID);

                preference.paperSubmission = paperSubmission;
            }


            DateTime reviewSubmission = Convert.ToDateTime(Request.Form["reviewSubmission"]);
            //update Review Submission Deadline
            if (reviewSubmission != preference.reviewSubmission)
            {
                notificationManager.sendNotifiactionAll(" New Review Submission Deadline is " + reviewSubmission.ToString(), (int)UserRole.PCM, userID);
                preference.reviewSubmission = reviewSubmission;
            }

            DateTime reviewChoice = Convert.ToDateTime(Request.Form["reviewChoice"]);

            //update review Choice deadline
            if (reviewChoice != preference.reviewChoice)
            {
                notificationManager.sendNotifiactionAll(" New review Choice Deadline is " + reviewChoice.ToString(), (int)UserRole.PCM, userID);
                preference.reviewChoice = reviewChoice;
            }

            DateTime authorNotification = Convert.ToDateTime(Request.Form["authorNotification"]);

            //update author Notification deadline
            if (authorNotification != preference.authorNotification)
            {
                notificationManager.sendNotifiactionAll(" New author Notification Deadline is " + authorNotification.ToString(), (int)UserRole.Author, userID);
                preference.authorNotification = authorNotification;
            }

            int result = preference.updatePreferences();

            return RedirectToPage("/Preferences", new { id = result });
        }
    }
}