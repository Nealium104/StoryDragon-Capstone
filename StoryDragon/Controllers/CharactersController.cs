using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using StoryDragon.Data;
using StoryDragon.Models;

namespace StoryDragon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly StoryDragonContext _context;

        // Dependency Inversion Principle (DIP): Depends on abstraction (StoryDragonContext) through constructor injection
        public CharactersController(StoryDragonContext context)
        {
            _context = context;
        }

        // Single Responsibility Principle (SRP): Focused on handling GET requests for Characters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharacters()
        {
            return await _context.Characters.ToListAsync();
        }

        // Single Responsibility Principle (SRP): Focused on handling GET requests for a specific Character
        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacter(Guid id)
        {
            var character = await _context.Characters.FindAsync(id);

            if (character == null)
            {
                return NotFound();
            }

            return character;
        }

        // Single Responsibility Principle (SRP): Focused on handling PUT requests for updating a Character
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacter(Guid id, Character character)
        {
            if (id != character.CharacterId)
            {
                return BadRequest();
            }

            _context.Entry(character).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacterExists(id))
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

        // Single Responsibility Principle (SRP): Focused on handling POST requests for creating a character
        [HttpPost]
        public async Task<ActionResult<Character>> PostCharacters(Character character)
        {
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCharacter", new { id = character.CharacterId }, character);
        }

        // Single Responsibility Principle (SRP): Focused on handling DELETE requests for deleting a character
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCharacter(Guid id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CharacterExists(Guid id)
        {
            return _context.Characters.Any(e => e.CharacterId == id);
        }
    }
}