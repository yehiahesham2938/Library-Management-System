using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace LibraryManagementSystem.Pages
{
        public class Borrowing
    {
        public int BorrowingId { get; set; }
        public int BookId { get; set; }
        public  string StudentName { get; set; } = string .Empty;
        public  int StudentId { get; set; }
        public  string StudentEmail { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Borrowing() {}
    }
    public class BorrowPageModel : PageModel
    {
        private readonly string ConnectionString;
        private readonly SqlConnection con;

        public List<Book> Books { get; private set; } = new List<Book>();
        public List<Borrowing> Borrowings { get; private set; } = new List<Borrowing>();

        public BorrowPageModel()
        {
            ConnectionString = "Data Source = YEHIA-HESHAM; Initial Catalog = LibraryManagementSystem; Integrated Security = True";
            con = new SqlConnection(ConnectionString);
        }

        public async Task<IActionResult> LoadBooksAsync()
        {
            try
            {
                await con.OpenAsync();

                string bookQuery = "SELECT BookID, Title FROM Books";
                using (SqlCommand bookCommand = new SqlCommand(bookQuery, con))
                using (SqlDataReader bookReader = await bookCommand.ExecuteReaderAsync())
                {
                    while (await bookReader.ReadAsync())
                    {
                        Books.Add(new Book
                        {
                            Id = bookReader.GetInt32(0),
                            Title = bookReader.GetString(1)
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
                con.Close();
            }
        }

        public async Task<IActionResult> OnGetsAsync()
        {
        try
        {
                await con.OpenAsync();

                string query = "SELECT BorrowingID, BookID, StudentName, StudentID, StudentEmail, BorrowDate, ReturnDate FROM Borrowings";
                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Borrowings.Add(new Borrowing
                        {
                            BorrowingId = reader.GetInt32(0),
                            BookId = reader.GetInt32(1),
                            StudentName = reader.GetString(2),
                            StudentId = reader.GetInt32(3),
                            StudentEmail = reader.GetString(4),
                            BorrowDate = reader.GetDateTime(5),
                            ReturnDate = reader.GetDateTime(6)
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
        public async Task<IActionResult> OnPostBorrowBookAsync(int bookId, string studentName, int studentId, string studentEmail, DateTime borrowDate, DateTime returnDate)
        {
            try
            {
                con.Open();

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
                    return RedirectToPage("/BorrowPage");
                }
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

        public async Task<IActionResult> OnPostReturnedBookAsync(int borrowingId, int bookId, string studentName, int studentId, DateTime returnDate)
        {
            try
            {
                await con.OpenAsync();
                    using (SqlTransaction transaction = con.BeginTransaction())
                {   
                    string insertQuery = "INSERT INTO Returned (BorrowingID, BookID, StudentName, StudentID, ReturnDate) " +
                                        "VALUES (@BorrowingId, @BookId, @StudentName, @StudentId, @ReturnDate)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BorrowingId", borrowingId);
                        cmd.Parameters.AddWithValue("@BookId", bookId);
                        cmd.Parameters.AddWithValue("@StudentName", studentName);
                        cmd.Parameters.AddWithValue("@StudentId", studentId);
                        cmd.Parameters.AddWithValue("@ReturnDate", returnDate);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    string deleteQuery = "DELETE FROM Borrowings WHERE BorrowingID = @BorrowingId";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, con, transaction))
                    {
                        deleteCmd.Parameters.AddWithValue("@BorrowingId", borrowingId);
                        await deleteCmd.ExecuteNonQueryAsync();
                    }
                transaction.Commit();
                }

                return RedirectToPage("/ReturnedBooks");
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
