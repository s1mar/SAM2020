using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAM2020.Models;

namespace SAM2020.Pages
{
    public class ManageUsersModel : PageModel
    {
        public List<User> userList { get; set; }
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

          Admin admin = new Admin();
          userList = admin.getUsers();

          return null;
        }

        public async Task<IActionResult> OnPostDeleteAsync(string selectedUser)
        {
          Admin admin = new Admin();
          int operationResult = admin.deleteUser(selectedUser);

          if (operationResult != -1)
          {
              return RedirectToPage(Routes.MANAGE_USERS, new { id = 1 });
          }

          return RedirectToPage(Routes.MANAGE_USERS, new { id = 2 });
        }

        public async Task<IActionResult> OnPostAsync()
        {
          Admin admin = new Admin();
          User user = new User();
          user.id = Int32.Parse(Request.Form["userid"]);
          user.userEmail = Request.Form["email"];
          user.name = Request.Form["name"];
          user.role = Request.Form["role"];
          user.password = Request.Form["password"];
          int operationResult = admin.modifyUser(user);

          if (operationResult != -1)
          {

                //Notify user his role has been Changed
                Notification notificationManager = new Notification();
                string roleNanme = Enum.GetName(typeof(UserRole), Convert.ToInt32(user.role));
                int status = notificationManager.sendNotifiactionToOneUser("Your role has been updated by "+ HttpContext.Session.GetString("userName") +" to be an "+ roleNanme, user.id, Convert.ToInt32(HttpContext.Session.GetString("userID")));
                return RedirectToPage(Routes.MANAGE_USERS, new { id = 4 });

          }

          return RedirectToPage(Routes.MANAGE_USERS, new { id = 2 });
        }
    }
}