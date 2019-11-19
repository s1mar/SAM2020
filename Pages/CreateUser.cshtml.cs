using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using SAM2020.Models;

namespace SAM2020.Pages
{
    public class CreateUserModel : PageModel
    {
        private readonly ILogger<CreateUserModel> _logger;

        [BindProperty]
        public User user { get; set; }
        public CreateUserModel(ILogger<CreateUserModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
          string user = HttpContext.Session.GetString("userID");
          int userRole = (int)HttpContext.Session.GetInt32("userRole");

          if (string.IsNullOrEmpty(user)) {
            return RedirectToPage(Routes.INDEX);
          }

          // If the user is not an Admin, redirect to the main menu
          if (userRole != Roles.ADMIN) {
            return RedirectToPage(Routes.CONTROL_PANEL);
          }

          return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Admin admin = new Admin();
            int status = admin.createUser(user);

            // To Do: Add validation, if user is valid.
            if (status != -1) {
              return RedirectToPage(Routes.MANAGE_USERS, new { id = 0 });
            }
            else {
              return RedirectToPage(Routes.CREATE_USER, new { id = 0 });
            }
           
        }
    }
}
