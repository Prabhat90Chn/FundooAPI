using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.RLException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NotesRL : INotesRL
    {
        private readonly FundooApiContext _dbContext;
        private readonly ILogger<NotesRL> _logger;

        public NotesRL(FundooApiContext context, ILogger<NotesRL> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task<UserNote> AddNote(NoteCreationModel notesModel, int userId)
        {
            var note = new UserNote
            {
                UserId = userId,
                Title = notesModel.Title,
                Description = notesModel.Description,
                Colour = notesModel.Colour
            };
            try
            {
                var dbnote = await _dbContext.Notes.AddAsync(note);
                await _dbContext.SaveChangesAsync();
                return dbnote.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding note.");
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }

        public async Task<List<UserNote>> ViewNotes(int userId)
        {
            try
            {
                var dbNotes = await _dbContext.Notes.Where(n => n.UserId == userId).ToListAsync();
                return dbNotes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving notes.");
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }

        public async Task<UserNote> ViewNotebyId(int userId, int noteId)
        {
            try
            {
                var note = await _dbContext.Notes.FirstOrDefaultAsync(n => n.UserId == userId && n.NoteId == noteId);
                return note;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving note by ID.");
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }

        public async Task<UserNote> EditNote(EditNotesModel editModel, int userId)
        {
            try
            {
                var noteId = 0;
                int.TryParse(editModel.NoteId, out noteId);
                var note = await _dbContext.Notes.FirstOrDefaultAsync(n => n.UserId == userId && n.NoteId == noteId);
                if (note != null)
                {
                    note.Title = editModel.Title ?? note.Title;
                    note.Description = editModel.Description ?? note.Description;
                    note.Colour = editModel.Colour ?? note.Colour;
                    await _dbContext.SaveChangesAsync();
                }
                return note;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing note.");
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteNote(int userId, int noteId)
        {
            try
            {
                UserNote note = await _dbContext.Notes.FirstOrDefaultAsync(e => e.UserId == userId && e.NoteId == noteId);
                if (note != null)
                {
                    _dbContext.Notes.Remove(note);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting note.");
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }

        public async Task<bool> ArchUnarchived(int userId, int noteId)
        {
            try
            {
                var note = await _dbContext.Notes.FirstOrDefaultAsync(n => n.UserId == userId && n.NoteId == noteId);
                if (note != null)
                {
                    note.IsArchived = !note.IsArchived;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while archiving/unarchiving note.");
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }

        public async Task<bool> TrashUnTrash(int userId, int noteId)
        {
            try
            {
                var note = await _dbContext.Notes.FirstOrDefaultAsync(n => n.UserId == userId && n.NoteId == noteId);
                if (note != null)
                {
                    note.IsDeleted = !note.IsDeleted;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while trashing/untrashing note.");
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }
    }
}
