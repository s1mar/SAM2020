using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SAM2020.Modles;

namespace SAM2020.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        [BindProperty]
        public User user { get; set; }
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            
        }


        public async Task<IActionResult> OnPostAsync()
        {
            User userManage = new User();
            int status = userManage.addNewUser(user.userEmail, user.password);
            if(status >0)
            {
                return RedirectToPage("/index", new { id = 2 });
                
            }
            else
            {
                return RedirectToPage("/Register", new { id = status });
            }
           
        }
    }
}
