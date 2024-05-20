using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Pages
{
    public class ReturnedBook
    {
        public int ReturnId { get; set; }

        public int BorrowingId { get; set; }
        public int BookId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public DateTime ReturnDate { get; set; }
        public ReturnedBook(){ }
    }

    public class ReturnedBooksModel : PageModel
    {
        private readonly string ConnectionString;
        private readonly SqlConnection con;

        public ReturnedBooksModel()
        {
            ConnectionString = "Data Source = YEHIA-HESHAM; Initial Catalog = LibraryManagementSystem; Integrated Security = True";
            con = new SqlConnection(ConnectionString);
        }


        public List<ReturnedBook> Returneds { get; private set; } = new List<ReturnedBook>();

        
        public async Task<IActionResult> OnGetsAsync()
        {
        try
        {
            await con.OpenAsync();
            string query = " SELECT ReturnedBookID, BorrowingID, BookID, StudentName, StudentID , ReturnDate FROM Returned";
            using (SqlCommand command = new SqlCommand(query, con))
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Returneds.Add(new ReturnedBook
                    {
                        ReturnId = reader.GetInt32(0),
                        BorrowingId = reader.GetInt32(1),
                        BookId = reader.GetInt32(2),
                        StudentName = reader.GetString(3),
                        StudentId = reader.GetInt32(4),
                        ReturnDate = reader.GetDateTime(5)

                    });
                }
            }
            return Page();

        }
        catch (Exception ex)
        {
                Console.WriteLine(ex.ToString());
                return Page();
        }
        finally
        {
                await con.CloseAsync();
        }
        }
        public async Task<IActionResult> OnGetAsync()
        {
            await OnGetsAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteBookAsync(int id)
        {
            try
            {
                await con.OpenAsync();
                
                string deleteQuery = "DELETE FROM Returned WHERE ReturnedBookID = @ReturnId";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, con))
                {
                    deleteCmd.Parameters.AddWithValue("@ReturnId", id);
                    await deleteCmd.ExecuteNonQueryAsync();
                }
                
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Page();
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

        public IActionResult OnPostBorrowPagePost()
        {
            return RedirectToPage("/BorrowPage");
        }

        public IActionResult OnPostLostBooksPost()
        {
            return RedirectToPage("/LostBooks");
        }
    }
}
