using System.ComponentModel.DataAnnotations;

namespace AspNetCoreStarter.Dtos
{
    public class TodoUpdateDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Text { get; set; }

        public bool Checked { get; set; }
    }
}
