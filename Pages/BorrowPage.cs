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
    public class BorrowPage : PageModel
{
   
        private readonly string ConnectionString;
        private readonly SqlConnection con;
        public List<Book> Books { get; private set; } = new List<Book>();

         public BorrowPage()
        {
            ConnectionString = "Data Source=YEHIA-HESHAM;Initial Catalog=LibraryManagementSystem;Integrated Security=True";
            con = new SqlConnection(ConnectionString);
        }
        public async Task OnGetAsync()
        {
            try
            {
                await con.OpenAsync();

                string query = "SELECT BookID, Title FROM Books";
                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Books.Add(new Book
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await con.CloseAsync();
            }
        }
        public async Task OnPostBorrowBookAsync(int bookId, string studentName, string studentId, string studentEmail, DateTime borrowDate, DateTime returnDate)
        {
            try
            {
                await con.OpenAsync();

                string query = "INSERT INTO Borrowings (BookID, StudentName, StudentID, StudentEmail, BorrowDate, ReturnDate) VALUES (@BookID, @StudentName, @StudentID, @StudentEmail, @BorrowDate, @ReturnDate)";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@BookID", bookId);
                    command.Parameters.AddWithValue("@StudentName", studentName);
                    command.Parameters.AddWithValue("@StudentID", studentId);
                    command.Parameters.AddWithValue("@StudentEmail", studentEmail);
                    command.Parameters.AddWithValue("@BorrowDate", borrowDate);
                    command.Parameters.AddWithValue("@ReturnDate", returnDate);

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await con.CloseAsync();
            }
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

}