﻿using System;
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
    public class AssignPapersModel : PageModel
    {
        public List<User> allPcms { get; set; }
        
        public async Task<IActionResult> OnGet() {
          string user = HttpContext.Session.GetString("userID");
          int userRole = (int)HttpContext.Session.GetInt32("userRole");

          // If the user is not logged in
          if (string.IsNullOrEmpty(user)) {
            return RedirectToPage(Routes.INDEX);
          }

          // If the user is not a PCC or Admin, redirect to the main menu
          if (userRole != Roles.PCC & userRole != Roles.ADMIN) {
            return RedirectToPage(Routes.CONTROL_PANEL);
          }

          Users users = new Users();
          allPcms = users.getUsersByRole(Roles.PCM);

          return null;
        }
    }
}

