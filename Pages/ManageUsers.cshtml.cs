using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAM2020.Models;

namespace SAM2020.Pages
{
    public class ManageUsersModel : PageModel
    {
        public List<string> userList { get; set; }
        public void OnGet()
        {
          Admin admin = new Admin();
          userList = admin.getUsers();
        }

        public async Task<IActionResult> OnPostDeleteAsync(String selectedUser) {      
          Admin admin = new Admin();
          int operationResult = admin.deleteUser(selectedUser);

          return RedirectToPage("/manageusers");
        }
    }
}