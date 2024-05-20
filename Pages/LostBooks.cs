using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Pages
{
    public class LostBook
    {
        public int LostBookId { get; set; }
        public int BorrowingId { get; set; }
        public int BookId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public LostBook(){ }
    }
    public class LostBooks : PageModel
{
        private readonly string ConnectionString;
        private readonly SqlConnection con;

        public LostBooks()
        {
            ConnectionString = "Data Source = YEHIA-HESHAM; Initial Catalog = LibraryManagementSystem; Integrated Security = True";
            con = new SqlConnection(ConnectionString);
        }

    public List<LostBook> Losts { get; private set; } = new List<LostBook>();


        public async Task<IActionResult> OnGetsAsync()
        {
        try
        {
            await con.OpenAsync();
            string query = "SELECT LostBookID, BorrowingID, BookID, StudentName, StudentID FROM Lost";
            using (SqlCommand command = new SqlCommand(query, con))
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Losts.Add(new LostBook
                    {
                        LostBookId = reader.GetInt32(0),
                        BorrowingId = reader.GetInt32(1),
                        BookId = reader.GetInt32(2),
                        StudentName = reader.GetString(3),
                        StudentId = reader.GetInt32(4),
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
                
                string deleteQuery = "DELETE FROM Lost WHERE LostBookID = @LostBookId";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, con))
                {
                    deleteCmd.Parameters.AddWithValue("@LostBookId", id);
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
        public IActionResult OnPostReturnedBooksPost()
    {
        return RedirectToPage("/ReturnedBooks");
    }
}

}