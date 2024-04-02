using Microsoft.AspNetCore.Identity;

namespace StoryDragon.Models
{
    public class User : IdentityUser
    {
        // public string UserName { get; set; }
        public string? Bio { get; set; }
        public ICollection<Character> Characters { get; set; } = new List<Character>();
        public ICollection<Story> Stories { get; set; } = new List<Story>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
