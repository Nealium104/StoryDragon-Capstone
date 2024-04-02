using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using StoryDragon.Data;
using StoryDragon.Models;

namespace StoryDragon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly StoryDragonContext _context;

        // Dependency Inversion Principle (DIP): Depends on abstraction (StoryDragonContext) through constructor injection
        public StoriesController(StoryDragonContext context)
        {
            _context = context;
        }

        // Single Responsibility Principle (SRP): Focused on handling GET requests for stories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Story>>> GetStories()
        {
            return await _context.Stories.ToListAsync();
        }

        // Single Responsibility Principle (SRP): Focused on handling GET requests for a specific story
        [HttpGet("{id}")]
        public async Task<ActionResult<Story>> GetStory(Guid id)
        {
            var story = await _context.Stories.FindAsync(id);

            if (story == null)
            {
                return NotFound();
            }

            return story;
        }

        // Single Responsibility Principle (SRP): Focused on handling PUT requests for updating a story
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStory(Guid id, Story story)
        {
            if (id != story.StoryID)
            {
                return BadRequest();
            }

            _context.Entry(story).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Single Responsibility Principle (SRP): Focused on handling POST requests for creating a story
        [HttpPost]
        public async Task<ActionResult<Story>> PostStory(Story story)
        {
            _context.Stories.Add(story);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStory", new { id = story.StoryID }, story);
        }

        // Single Responsibility Principle (SRP): Focused on handling DELETE requests for deleting a story
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteStory(Guid id)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story == null)
            {
                return NotFound();
            }

            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StoryExists(Guid id)
        {
            return _context.Stories.Any(e => e.StoryID == id);
        }
    }
}
