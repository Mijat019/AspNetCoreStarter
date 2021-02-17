using System.ComponentModel.DataAnnotations;

namespace AspNetCoreStarter.Dtos
{
    public class SignInDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(32)]
        public string Password { get; set; }
    }
}
