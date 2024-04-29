using BusinessLayer.BLException;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.RLException;
using System.Collections.Generic;

namespace BusinessLayer.Service
{
    public class NotesBL : INotesBL
    {
        private readonly INotesRL _notesRL;

        public NotesBL(INotesRL notesRL)
        {
            _notesRL = notesRL;
        }

        public UserNotes AddNote(NotesModel notesModel, int userId)
        {
            try
            {
                return _notesRL.AddNote(notesModel, userId);
            }
            catch (RepositoryLayerException ex)
            {
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public List<UserNotes> ViewNotes(int userId)
        {
            try
            {
                return _notesRL.ViewNotes(userId);
            }
            catch (RepositoryLayerException ex)
            {
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public UserNotes ViewNotebyId(int userId, NotesIdModel noteIdmodel)
        {
            try
            {
                var noteId = getNotesId(noteIdmodel.NoteId);
                return _notesRL.ViewNotebyId(userId, noteId);
            }
            catch (RepositoryLayerException ex)
            {
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public UserNotes EditNote(EditNotesModel notesModel, int userId)
        {
            try
            {
                return _notesRL.EditNote(notesModel, userId);
            }
            catch (RepositoryLayerException ex)
            {
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public bool DeleteNote(int userId, NotesIdModel noteIdmodel)
        {
            try
            {
                var noteId = getNotesId(noteIdmodel.NoteId);
                return _notesRL.DeleteNote(userId, noteId);
            }
            catch (RepositoryLayerException ex)
            {
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public bool ArchUnarchived(int userId, NotesIdModel noteIdmodel)
        {
            try
            {
                var noteId = getNotesId(noteIdmodel.NoteId);
                return _notesRL.ArchUnarchived(userId, noteId);
            }
            catch (RepositoryLayerException ex)
            {
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public bool TrashUnTrash(int userId, NotesIdModel noteIdmodel)
        {
            try
            {
                var noteId = getNotesId(noteIdmodel.NoteId);
                return _notesRL.TrashUnTrash(userId, noteId);
            }
            catch (RepositoryLayerException ex)
            {
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        private int getNotesId(string id)
        {
            if (int.TryParse(id, out int intValue))
            {
                return intValue;
            }
            return 0;
        }
    }
}

