using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryManagementSystem.Pages;
public class BorrowPage : PageModel
{
    private readonly ILogger<StudentModel> _logger;

    public BorrowPage(ILogger<StudentModel> logger)
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
    public IActionResult OnPostReturnedBooksPost()
    {
        return RedirectToPage("/ReturnedBooks");
    }
    public IActionResult OnPostLostBooksPost()
    {
        return RedirectToPage("/LostBooks");
    }
}

