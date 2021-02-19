using System;

namespace AspNetCoreStarter.Dtos
{
    public class TodoReadDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }
        public DateTime CreatedAt{ get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
