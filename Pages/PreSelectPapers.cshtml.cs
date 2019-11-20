using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using SAM2020.Models;

namespace SAM2020.Pages
{
    public class PreSelectPapersModel : PageModel
    {
        [BindProperty]
        public List<Paper> allPapers { get; set; }
        public List<int> userPreselection { get; set; }
        
        public async Task<IActionResult> OnGet() {
          string user = HttpContext.Session.GetString("userID");
          int userRole = (int)HttpContext.Session.GetInt32("userRole");

          // If the user is not logged in
          if (string.IsNullOrEmpty(user)) {
            return RedirectToPage(Routes.LOGIN);
          }

          // If the user is not a PCM or Admin, redirect to the main menu
          if (userRole != Roles.PCM & userRole != Roles.ADMIN) {
            return RedirectToPage(Routes.CONTROL_PANEL);
          }

          Papers papers = new Papers();
          allPapers = papers.getAllPapers();
          userPreselection = papers.getPreselectedPapers(user);

          return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Papers papers = new Papers();
            string userId = HttpContext.Session.GetString("userID");
            var selectedPapers = Request.Form["CheckedPapers"];

            if (String.IsNullOrEmpty(selectedPapers)) {
              return RedirectToPage(Routes.PRE_SELECT_PAPERS, new { id = 3 });
            }

            int result = papers.preSelectPapers(userId, selectedPapers);

            if (result == -1) {
              return RedirectToPage(Routes.PRE_SELECT_PAPERS, new { id = 2 });
            }

            return RedirectToPage(Routes.PRE_SELECT_PAPERS, new { id = 1 });
        }
    }
}

