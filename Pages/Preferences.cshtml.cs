using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAM2020.Modles;

namespace SAM2020.Pages
{
    public class PreferencesModel : PageModel
    {
      public Preference preference { get; set; } = Preference.Instance;
        public void OnGet()
        {
          preference.getPreferences();
        }

        public async Task<IActionResult> OnPostAsync()
        {        
            preference.paperSubmission = Convert.ToDateTime(Request.Form["paperSubmission"]);
            preference.reviewSubmission = Convert.ToDateTime(Request.Form["reviewSubmission"]);
            preference.reviewChoice = Convert.ToDateTime(Request.Form["reviewChoice"]);
            preference.authorNotification = Convert.ToDateTime(Request.Form["authorNotification"]);
            int result = preference.updatePreferences();

            return RedirectToPage("/Preferences", new { id = result });
        }
    }
}