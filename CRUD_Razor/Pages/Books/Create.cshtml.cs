using CRUD_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace CRUD_Razor
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public Book Book { get; set; } = new();

        public void OnGet()
        {
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
                INSERT INTO dbo.books
                (title, author, description, publisher, status, category)
                VALUES
                (@title, @author, @description, @publisher, @status, @category)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@title", (object?)Book.title ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@author", (object?)Book.author ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@description", (object?)Book.description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@publisher", (object?)Book.publisher ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@status", (object?)Book.status ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@category", (object?)Book.category ?? DBNull.Value);

            cmd.ExecuteNonQuery();

            return RedirectToPage("Index");
        }
    }
}