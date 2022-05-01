using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Definition { get; set; }

        [Required]
        public string? Subject { get; set; }
    }
}