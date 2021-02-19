using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreStarter.Dtos
{
    public class TodoCreateDto 
    {
        [Required]
        [MaxLength(256)]
        public string Text { get; set; }

        public bool Checked { get; set; }
    }
}
