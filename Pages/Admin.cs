using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LibraryManagementSystem.Pages
{
    public class AdminModel : PageModel
    {
        private readonly string ConnectionString; 
        private readonly SqlConnection con;

        public AdminModel()
        {
            ConnectionString = "Data Source = YEHIA-HESHAM; Initial Catalog = LibraryManagementSystem; Integrated Security = True";
            con = new SqlConnection(ConnectionString);
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostInsertBookAsync(string title, string isbn, string author, DateTime publishedDate, IFormFile photo)
        {
            try
            {
                con.Open(); 
                byte[]? photoBytes = null;

                if (photo != null && photo.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await photo.CopyToAsync(ms);
                        photoBytes = ms.ToArray();
                    }
                }

                string query = "INSERT INTO Books (Title, ISBN, Author, PublishedDate, Photo) VALUES (@Title, @ISBN, @Author, @PublishedDate, @Photo)";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@ISBN", isbn);
                command.Parameters.AddWithValue("@Author", author);
                command.Parameters.AddWithValue("@PublishedDate", publishedDate);
                SqlParameter photoParam = new SqlParameter("@Photo", photoBytes != null ? (object)photoBytes : DBNull.Value);
                command.Parameters.Add(photoParam);

                await command.ExecuteNonQueryAsync();

                return RedirectToPage("/Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Page();
            }
            finally
            {
                con.Close();
            }
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
}
