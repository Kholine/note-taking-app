using backend.Models;

namespace backend.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllNotesAsync(int userId, string? searchTerm = null, string? sortBy = null, bool isDescending = false);
        Task<Note?> GetNoteByIdAsync(int id, int userId);
        Task<Note> CreateNoteAsync(CreateNoteRequest request, int userId);
        Task<Note?> UpdateNoteAsync(int id, UpdateNoteRequest request, int userId);
        Task<bool> DeleteNoteAsync(int id, int userId);
    }
} 