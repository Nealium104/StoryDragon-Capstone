namespace StoryDragon.Models
{
    public class Character
    {
        public Guid CharacterId { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public User User { get; set; }
        public string CharacterName { get; set; }
        public string CharacterShortDescription { get; set; }
        public string CharacterDescription { get; set; }
        public List<string>? Tags { get; set; } = new List<string>();
        public ICollection<Story>? Stories { get; set; } = new List<Story>();
        public virtual ICollection<Post> Posts { get; set; }
    }
}
