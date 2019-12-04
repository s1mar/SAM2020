using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAM2020.Models;
using Microsoft.AspNetCore.Http;

namespace SAM2020.Pages
{
    public class CreateNotificationModel : PageModel
    {

        [BindProperty]
        public Notification notification { get; set; }
        public List<User> userList { get; set; }

        public async Task<IActionResult> OnGet() 
        {
          string user = HttpContext.Session.GetString("userID");

          if (string.IsNullOrEmpty(user)) {
            return RedirectToPage(Routes.LOGIN);
          }

          int userRole = -1;
          if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userRole"))) {
            userRole = Convert.ToInt32(HttpContext.Session.GetString("userRole"));
          }

          Users users = new Users();
          userList = users.getAllUsers();

          return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Notification notificationManager = new Notification();
            int userID = Convert.ToInt32(HttpContext.Session.GetString("userID"));
            int status = 0;

            if (String.IsNullOrEmpty(notification.message))
            {
              return RedirectToPage(Routes.CREATE_NOTIFICATION, new { id = 3 });
            }

            if (notification.recipientId < 0)
            {
              status = notificationManager.sendNotifiactionAll(notification.message, notification.recipientId, userID);
            } else {
              status = notificationManager.sendNotifiactionToOneUser(notification.message, notification.recipientId, userID);
            }

            if (status == 0)
            {
              return RedirectToPage(Routes.CREATE_NOTIFICATION, new { id = status });
            }

            return RedirectToPage(Routes.CREATE_NOTIFICATION, new { id = 1 });
        }

    }
}