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
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public byte[] Photo { get; set; } = Array.Empty<byte>();
        
        public Book() { } 
    }

    public class AdminModel : PageModel
    {
        private readonly string ConnectionString; 
        private readonly SqlConnection con;
        public List<Book> Books { get; private set; } = new List<Book>();

        public AdminModel()
        {
            ConnectionString = "Data Source = YEHIA-HESHAM; Initial Catalog = LibraryManagementSystem; Integrated Security = True";
            con = new SqlConnection(ConnectionString);
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

        public async Task<IActionResult> OnGetAsync()
{
    try
    {
        await con.OpenAsync();

        string query = "SELECT BookID, Title, ISBN, Author, PublishedDate, CONVERT(VARCHAR(MAX), Photo, 2) AS Photo FROM Books"; // Convert VARBINARY to VARCHAR(MAX) for Photo
        using (SqlCommand command = new SqlCommand(query, con))
        using (SqlDataReader reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                // Add the entire Book object to the list
                Books.Add(new Book
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    ISBN = reader.GetString(2),
                    Author = reader.GetString(3),
                    PublishedDate = reader.GetDateTime(4),
                    // Convert the hexadecimal representation of VARBINARY to byte array
                    Photo = StringToByteArray(reader.GetString(5))
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

private byte[] StringToByteArray(string hex)
{
    int numberChars = hex.Length / 2;
    byte[] bytes = new byte[numberChars];
    using (var sr = new StringReader(hex))
    {
        for (int i = 0; i < numberChars; i++)
        {
            bytes[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
        }
    }
    return bytes;
}



public async Task<IActionResult> OnPostDeleteBookAsync(int id)
{
    try
    {
        await con.OpenAsync();

        string query = "DELETE FROM Books WHERE BookID = @Id";
        using (SqlCommand command = new SqlCommand(query, con))
        {
            command.Parameters.AddWithValue("@Id", id);
            await command.ExecuteNonQueryAsync();
        }

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
