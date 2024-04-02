using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoryDragon.Data;
using StoryDragon.Models;

namespace StoryDragon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly StoryDragonContext _context;

        // Constructor Injection (Dependency Inversion Principle)
        public PostsController(StoryDragonContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            // Single Responsibility Principle (SRP): The method has a single responsibility of retrieving all posts
            return await _context.Posts.ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(Guid id)
        {
            // Single Responsibility Principle (SRP): The method has a single responsibility of retrieving a specific post
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPost(Guid id, [FromBody] Post post)
        {
            // Single Responsibility Principle (SRP): The method has a single responsibility of updating a post
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != post.PostId)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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

        // POST: api/Posts
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Post>> PostPost([FromBody] Post post)
        {
            // Single Responsibility Principle (SRP): The method has a single responsibility of creating a new post
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.PostId }, post);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            // Single Responsibility Principle (SRP): The method has a single responsibility of deleting a post
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(Guid id)
        {
            // Single Responsibility Principle (SRP): The method has a single responsibility of checking if a post exists
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}