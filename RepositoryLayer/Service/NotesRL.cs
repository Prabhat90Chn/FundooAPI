using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.RLException;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryLayer.Service
{
    public class NotesRL : INotesRL
    {
        private readonly FundooApiContext _dbContext;

        public NotesRL(FundooApiContext context)
        {
            _dbContext = context;
        }

        public UserNotes AddNote(NotesModel notesModel, int userId)
        {
            try
            {
                var note = new UserNotes
                {
                    UserId = userId,
                    Title = notesModel.Title,
                    Description = notesModel.Description,
                    Colour = notesModel.Colour
                };
                _dbContext.Notes.Add(note);
                _dbContext.SaveChanges();
                return note;
            }
            catch (Exception ex)
            {
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }

        public List<UserNotes> ViewNotes(int userId)
        {
            try
            {
                return _dbContext.Notes.Where(n => n.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }

        public UserNotes ViewNotebyId(int userId, int noteId)
        {
            try
            {
                return _dbContext.Notes.FirstOrDefault(n => n.UserId == userId && n.NoteId == noteId);
            }
            catch (Exception ex)
            {
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }

        public UserNotes EditNote(EditNotesModel editModel, int userId)
        {
            try
            {
                var note = _dbContext.Notes.FirstOrDefault(n => n.UserId == userId && n.NoteId == editModel.NoteId);
                if (note != null)
                {
                    note.Title = editModel.Title ?? note.Title;
                    note.Description = editModel.Description ?? note.Description;
                    note.Colour = editModel.Colour ?? note.Colour;
                    _dbContext.SaveChanges();
                }
                return note;
            }
            catch (Exception ex)
            {
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }

        public bool DeleteNote(int userId, int noteId)
        {
            try
            {
                UserNotes note = _dbContext.Notes.FirstOrDefault(e => e.UserId == userId && e.NoteId == noteId);
                if (note != null)
                {
                    _dbContext.Notes.Remove(note);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }

        public bool ArchUnarchived(int userId, int noteId)
        {
            try
            {
                var note = _dbContext.Notes.FirstOrDefault(n => n.UserId == userId && n.NoteId == noteId);
                if (note != null)
                {
                    note.IsArchived = !note.IsArchived;
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }

        public bool TrashUnTrash(int userId, int noteId)
        {
            try
            {
                var note = _dbContext.Notes.FirstOrDefault(n => n.UserId == userId && n.NoteId == noteId);
                if (note != null)
                {
                    note.IsDeleted = !note.IsDeleted;
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }
    }
}
