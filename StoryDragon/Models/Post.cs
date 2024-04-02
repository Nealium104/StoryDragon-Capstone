using StoryDragon.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoryDragon.Models
{
    public enum PostType
    { 
        StoryPost,
        CharacterPost
    }


    public class RequireStoryOrCharacterAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var post = (Post)validationContext.ObjectInstance;

            if (post.StoryId == null && post.CharacterId == null)
            {
                return new ValidationResult("A Post must have either a Story or a Character.");
            }

            return ValidationResult.Success;
        }
    }
    public class Post
    {
        public Guid PostId { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        [Required]
        public string PostText { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime? EditDate { get; set; }
        public int Likes { get; set; }
        public PostType PostType { get; set; }
        public Guid? CharacterId { get; set; }
        public virtual Character Character { get; set; }
        public Guid? StoryId { get; set; }
        public virtual Story Story { get; set; }
        [RequireStoryOrCharacter]
        public string ValidateStoryOrCharacter { get; set; }
    }
}
