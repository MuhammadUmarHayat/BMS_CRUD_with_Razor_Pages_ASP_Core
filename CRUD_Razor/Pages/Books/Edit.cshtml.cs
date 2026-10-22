using CRUD_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace CRUD_Razor
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public Book Book { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            string cs = _configuration
                .GetConnectionString("DefaultConnection")!;

            using SqlConnection con = new SqlConnection(cs);
            con.Open();

            string query = "SELECT * FROM dbo.books WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.Read())
                return NotFound();

            Book = new Book
            {
                Id = (int)reader["Id"],
                title = reader["title"] as string,
                author = reader["author"] as string,
                description = reader["description"] as string,
                publisher = reader["publisher"] as string,
                status = reader["status"] as string,
                category = reader["category"] as string
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            string cs = _configuration
                .GetConnectionString("DefaultConnection")!;

            using SqlConnection con = new SqlConnection(cs);
            con.Open();

            string query = @"
                UPDATE dbo.books SET
                    title = @title,
                    author = @author,
                    description = @description,
                    publisher = @publisher,
                    status = @status,
                    category = @category
                WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Id", Book.Id);
            cmd.Parameters.AddWithValue("@title", (object?)Book.title ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@author", (object?)Book.author ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@description", (object?)Book.description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@publisher", (object?)Book.publisher ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@status", (object?)Book.status ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@category", (object?)Book.category ?? DBNull.Value);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                return NotFound();

            return RedirectToPage("Index");
        }
    }
}