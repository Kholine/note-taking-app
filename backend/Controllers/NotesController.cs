using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;

        public NotesController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        // GET: api/notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes(
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            try
            {
                const int userId = 1; // For demo purposes
                var notes = await _noteRepository.GetAllNotesAsync(userId, search, sortBy, isDescending);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving notes", error = ex.Message });
            }
        }

        // GET: api/notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            try
            {
                const int userId = 1; // For demo purposes
                var note = await _noteRepository.GetNoteByIdAsync(id, userId);
                
                if (note == null)
                {
                    return NotFound(new { message = "Note not found" });
                }

                return Ok(note);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving note", error = ex.Message });
            }
        }

        // POST: api/notes
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote(CreateNoteRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return BadRequest(new { message = "Title is required" });
                }

                const int userId = 1; // For demo purposes
                var note = await _noteRepository.CreateNoteAsync(request, userId);
                
                return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating note", error = ex.Message });
            }
        }

        // PUT: api/notes/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Note>> UpdateNote(int id, UpdateNoteRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return BadRequest(new { message = "Title is required" });
                }

                const int userId = 1; // For demo purposes
                var note = await _noteRepository.UpdateNoteAsync(id, request, userId);
                
                if (note == null)
                {
                    return NotFound(new { message = "Note not found" });
                }

                return Ok(note);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating note", error = ex.Message });
            }
        }

        // DELETE: api/notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            try
            {
                const int userId = 1; // For demo purposes
                var success = await _noteRepository.DeleteNoteAsync(id, userId);
                
                if (!success)
                {
                    return NotFound(new { message = "Note not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting note", error = ex.Message });
            }
        }
    }
} 