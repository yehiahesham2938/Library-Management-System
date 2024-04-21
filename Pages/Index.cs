using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Pages
{
    public class IndexModel : PageModel
    {
        
        public void OnGet()
        {

        }
        public IActionResult OnPost(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                return RedirectToPage("/Admin");
            } 
            else
            {
                return RedirectToPage("/Student");
            }
        }
    }
}
