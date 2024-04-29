using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Collections.Generic;

namespace BusinessLayer.Interface
{
    public interface INotesBL
    {

        public UserNotes AddNote(NotesModel notesModel, int userId);
        public List<UserNotes> ViewNotes(int userId);

        public UserNotes ViewNotebyId(int userId, NotesIdModel noteIdmodel);

        public UserNotes EditNote(EditNotesModel editModel, int userId);

        public bool DeleteNote(int userId, NotesIdModel notenoteIdmodelId);

        public bool ArchUnarchived(int userId, NotesIdModel noteIdmodel);

        public bool TrashUnTrash(int userId, NotesIdModel noteIdmodel);
    }
}
