using System.ComponentModel.DataAnnotations;

namespace AspNetCoreStarter.Models
{
    public class Todo : Entity
    {
        [Required]
        [MaxLength(256)]
        public string Text { get; set; }

        public bool Checked { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
