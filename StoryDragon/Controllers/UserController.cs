using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using StoryDragon.Data;
using StoryDragon.Models;
using Microsoft.AspNetCore.Identity;

namespace StoryDragon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly StoryDragonContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        // Dependency Inversion Principle (DIP): Depends on abstraction (StoryDragonContext) through constructor injection
        public UserController(StoryDragonContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Single Responsibility Principle (SRP): Focused on handling GET requests for users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }

        // Single Responsibility Principle (SRP): Focused on handling GET requests for a specific user
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // Single Responsibility Principle (SRP): Focused on handling POST requests for creating a user
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return BadRequest("Entity set 'StoryDragonContext.Users' is null.");
            }

            if (await _context.Users.AnyAsync(u => u.UserName == user.UserName || u.Email == user.Email))
            {
                return Conflict("A user with the same username or email already exists.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // Single Responsibility Principle (SRP): Focused on handling PUT requests for updating a user
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            if (!_context.Users.Any(u => u.Id == id))
            {
                return NotFound();
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Single Responsibility Principle (SRP): Focused on handling DELETE requests for deleting a user
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetUserPosts(string id)
        {
            var user = await _context.Users
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user.Posts.ToList();
        }

        [HttpGet("{id}/characters")]
        public async Task<ActionResult<IEnumerable<Character>>> GetUserCharacters(string id)
        {
            var user = await _context.Users
                .Include(u => u.Characters)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user.Characters.ToList();
        }

        [HttpGet("{id}/stories")]
        public async Task<ActionResult<IEnumerable<Story>>> GetUserStories(string id)
        {
            var user = await _context.Users
                .Include(u => u.Stories)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user.Stories.ToList();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Bio = model.Bio
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return BadRequest(ModelState);
        }
    }
}
