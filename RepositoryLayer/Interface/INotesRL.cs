using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Collections.Generic;

namespace RepositoryLayer.Interface
{
    public interface INotesRL
    {
        public UserNotes AddNote(NotesModel notesModel, int userId);
        public List<UserNotes> ViewNotes(int userId);

        public UserNotes ViewNotebyId(int userId, int noteId);

        public UserNotes EditNote(EditNotesModel editModel, int userId);

        public bool DeleteNote(int userId, int noteId);

        public bool ArchUnarchived(int userId, int noteId);

        public bool TrashUnTrash(int userId, int noteId);
    }
}
