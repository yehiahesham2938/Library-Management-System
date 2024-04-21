using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryManagementSystem.Pages;
public class LostBooks : PageModel
{
    private readonly ILogger<StudentModel> _logger;

    public LostBooks(ILogger<StudentModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
    public IActionResult OnPostHomePagePost()
    {
        return RedirectToPage("/Admin");
    }
        public IActionResult OnPostBorrowPagePost()
    {
        return RedirectToPage("/BorrowPage");
    }
        public IActionResult OnPostReturnedBooksPost()
    {
        return RedirectToPage("/ReturnedBooks");
    }
}

