using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SAM2020.Models;

namespace SAM2020.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public User user { get; set; }

        public void OnGet()
        {
            HttpContext.Session.Clear();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            User userManage = new User();
            int userID = userManage.login(user.userEmail, user.password);
            if (userID == -1)
            {
                return RedirectToPage(Routes.LOGIN, new { id = 0 });
            }
            else
            {
                HttpContext.Session.SetString("userID", userID.ToString());
                HttpContext.Session.SetString("userName", user.getUserNameByID(userID.ToString()));
                int userRole = userManage.getUserRole(userID.ToString());
                HttpContext.Session.SetString("userRole", userRole.ToString());

                return RedirectToPage(Routes.INDEX);
            }

           
        }
    }
}
