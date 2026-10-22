using CRUD_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace CRUD_Razor
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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

        public IActionResult OnPost(int id)
        {
            string cs = _configuration
                .GetConnectionString("DefaultConnection")!;

            using SqlConnection con = new SqlConnection(cs);
            con.Open();

            string query = "DELETE FROM dbo.books WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                return NotFound();

            return RedirectToPage("Index");
        }
    }
}