using Microsoft.EntityFrameworkCore;
using StoryDragon.Data;
using StoryDragon.Models;
using System.Linq;
using Xunit;

namespace Testing
{
    public class StoryTests
    {
        [Fact]
        public void CreateStory_WithCharactersAndPosts_ShouldPersistData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StoryDragonContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new StoryDragonContext(options))
            {
                var userId = "user-id";

                var story = new Story
                {
                    StoryTitle = "My Story",
                    StoryDescription = "A great story",
                    StoryText = "Once upon a time...",
                    UserId = userId,
                    Characters = new List<Character>
                    {
                        new Character {
                            UserId = userId,
                            CharacterName = "John",
                            CharacterShortDescription = "A brave warrior",
                            CharacterDescription = "John is a brave warrior with a noble heart." }
                    },
                    Posts = new List<Post>
                    {
                        new Post { Title="An adventure begins...", PostText = "Chapter 1", UserId = userId },
                        new Post { Title="A second chapter awaits...", PostText = "Chapter 2", UserId = userId }
                    }
                };

                // Act
                context.Stories.Add(story);
                context.SaveChanges();

                // Assert
                Assert.Equal(1, story.Characters.Count);
                Assert.Equal(2, story.Posts.Count);
            }
        }
        [Fact]
        public void GetPostWithCharacter_ShouldReturnPopulatedPost()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StoryDragonContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var userId = "user-id";

            using (var context = new StoryDragonContext(options))
            {
                // Create a character
                var character = new Character
                {
                    UserId = userId,
                    CharacterName = "John",
                    CharacterShortDescription = "A brave warrior",
                    CharacterDescription = "John is a brave warrior with a noble heart."
                };
                context.Characters.Add(character);

                // Create a post associated with the character
                var post = new Post
                {
                    UserId = userId,
                    Title = "John's adventure",
                    PostText = "John saw a rabbit and that rabbit was one cool cat.",
                    Character = character
                };
                context.Posts.Add(post);

                context.SaveChanges();

                // Act
                var retrievedPost = context.Posts
                    .Include(p => p.Character)
                    .FirstOrDefault(p => p.PostId == post.PostId);

                // Assert
                Assert.NotNull(retrievedPost);
                Assert.Equal(post.PostText, retrievedPost.PostText);
                Assert.NotNull(retrievedPost.Character);
                Assert.Equal(character.CharacterName, retrievedPost.Character.CharacterName);
                Assert.Equal(character.CharacterDescription, retrievedPost.Character.CharacterDescription);
            }
        }
        [Fact]
        public void GetPostWithStory_ShouldReturnPopulatedPost()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StoryDragonContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new StoryDragonContext(options))
            {
                // Create a story
                var story = new Story
                {
                    StoryTitle = "The Great Adventure",
                    StoryDescription = "An epic tale of bravery and friendship",
                    StoryText = "Once upon a time...",
                    UserId = "user-id"
                };
                context.Stories.Add(story);

                // Create a post associated with the story
                var post = new Post
                {
                    Title = "Chapter 1: The Beginning",
                    PostText = "The adventure begins with our hero setting out on a journey.",
                    Story = story,
                    UserId = "user-id",
                    PostDate = DateTime.Now
                };
                context.Posts.Add(post);

                context.SaveChanges();

                // Act
                var retrievedPost = context.Posts
                    .Include(p => p.Story)
                    .FirstOrDefault(p => p.PostId == post.PostId);

                // Assert
                Assert.NotNull(retrievedPost);
                Assert.Equal(post.Title, retrievedPost.Title);
                Assert.Equal(post.PostText, retrievedPost.PostText);
                Assert.Equal(post.UserId, retrievedPost.UserId);
                Assert.Equal(post.PostDate.Date, retrievedPost.PostDate.Date);

                Assert.NotNull(retrievedPost.Story);
                Assert.Equal(story.StoryTitle, retrievedPost.Story.StoryTitle);
                Assert.Equal(story.StoryDescription, retrievedPost.Story.StoryDescription);
                Assert.Equal(story.StoryText, retrievedPost.Story.StoryText);
                Assert.Equal(story.UserId, retrievedPost.Story.UserId);
            }
        }
    }
}