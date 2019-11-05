using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAM2020.Modles;
using Microsoft.AspNetCore.Http;
namespace SAM2020.Pages
{
    public class NotificationManagerModel : PageModel
    {

        [BindProperty]
        public Notification notification { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            Notification notificationManager = new Notification();
            int  userID = Convert.ToInt32(HttpContext.Session.GetString("userID"));
            int status= notificationManager.sendNotifiactionAll(notification.message, Convert.ToInt32(notification.userRole), userID);
            return RedirectToPage("/NotificationManager", new { id = status });
        }

    }
}