using System.ComponentModel.DataAnnotations;

namespace CRUD_Razor.Models
{
    public class Book
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string? title { get; set; }

        [StringLength(50)]
        public string? author { get; set; }

        [StringLength(50)]
        public string? description { get; set; }

        [StringLength(50)]
        public string? publisher { get; set; }

        [StringLength(50)]
        public string? status { get; set; }

        [StringLength(50)]
        public string? category { get; set; }
    }
}
