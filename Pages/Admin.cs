using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
namespace LibraryManagementSystem.Pages;

public class AdminModel : PageModel
{
    private readonly ILogger<AdminModel> _logger;

    public AdminModel(ILogger<AdminModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
    public IActionResult OnPostBorrowPagePost()
    {
        return RedirectToPage("/BorrowPage");
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