using System;

namespace AspNetCoreStarter.Dtos
{
    public class UserReadDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
