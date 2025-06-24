using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using backend.Models;

namespace backend.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly string _connectionString;

        public NoteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException("Connection string not found");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Note>> GetAllNotesAsync(int userId, string? searchTerm = null, string? sortBy = null, bool isDescending = false)
        {
            using var connection = CreateConnection();
            
            var sql = "SELECT * FROM Notes WHERE UserId = @UserId";
            object parameters = new { UserId = userId };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                sql += " AND (Title LIKE @SearchTerm OR Content LIKE @SearchTerm)";
                parameters = new { UserId = userId, SearchTerm = $"%{searchTerm}%" };
            }

            // Add ordering
            var orderBy = sortBy?.ToLower() switch
            {
                "title" => "Title",
                "createdat" => "CreatedAt",
                "updatedat" => "UpdatedAt",
                _ => "UpdatedAt"
            };

            sql += $" ORDER BY {orderBy} {(isDescending ? "DESC" : "ASC")}";

            return await connection.QueryAsync<Note>(sql, parameters);
        }

        public async Task<Note?> GetNoteByIdAsync(int id, int userId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Notes WHERE Id = @Id AND UserId = @UserId";
            return await connection.QueryFirstOrDefaultAsync<Note>(sql, new { Id = id, UserId = userId });
        }

        public async Task<Note> CreateNoteAsync(CreateNoteRequest request, int userId)
        {
            using var connection = CreateConnection();
            var now = DateTime.UtcNow;
            
            var sql = @"
                INSERT INTO Notes (Title, Content, CreatedAt, UpdatedAt, UserId) 
                OUTPUT INSERTED.*
                VALUES (@Title, @Content, @CreatedAt, @UpdatedAt, @UserId)";
            
            var note = await connection.QuerySingleAsync<Note>(sql, new 
            { 
                request.Title, 
                request.Content, 
                CreatedAt = now, 
                UpdatedAt = now, 
                UserId = userId 
            });
            
            return note;
        }

        public async Task<Note?> UpdateNoteAsync(int id, UpdateNoteRequest request, int userId)
        {
            using var connection = CreateConnection();
            
            var sql = @"
                UPDATE Notes 
                SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt 
                OUTPUT INSERTED.*
                WHERE Id = @Id AND UserId = @UserId";
            
            var note = await connection.QueryFirstOrDefaultAsync<Note>(sql, new 
            { 
                request.Title, 
                request.Content, 
                UpdatedAt = DateTime.UtcNow, 
                Id = id, 
                UserId = userId 
            });
            
            return note;
        }

        public async Task<bool> DeleteNoteAsync(int id, int userId)
        {
            using var connection = CreateConnection();
            var sql = "DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, UserId = userId });
            return rowsAffected > 0;
        }
    }
} 