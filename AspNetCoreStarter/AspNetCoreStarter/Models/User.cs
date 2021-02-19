using AspNetCoreStarter.Contracts.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreStarter.Models
{
    public class User : Entity
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }

        [Required]
        public Role Role { get; set; }

        public IEnumerable<Todo> Todos { get; set; }
    }
}
