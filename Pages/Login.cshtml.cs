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
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public User user { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            HttpContext.Session.Clear();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            User userManage = new User();
            int userID = userManage.login(user.userEmail, user.password);
            if (userID == 0)
            {
                return RedirectToPage(Routes.INDEX, new { id = 1 });
            }
            else
            {
                HttpContext.Session.SetString("userID", userID.ToString());
                HttpContext.Session.SetString("userName", user.getUserNameByID(userID.ToString()));
                int userRole = userManage.getUserRole(userID.ToString());
                HttpContext.Session.SetInt32("userRole", userRole);
                return RedirectToPage(Routes.CONTROL_PANEL, new { id = userID });
            }

           
        }
    }
}
