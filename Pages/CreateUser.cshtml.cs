using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
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

        public void OnGet()
        {
            
        }


        public async Task<IActionResult> OnPostAsync()
        {
            Admin admin = new Admin();
            int status = admin.createUser(user);

            // To Do: Add validation, if user is valid.
            if (status != -1) {
              return RedirectToPage("/ManageUsers", new { id = 0 });
            }
            else {
              return RedirectToPage("/CreateUser", new { id = 0 });
            }
           
        }
    }
}
