using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface INotesBL
    {

        public Task<UserNote> AddNote(NoteCreationModel notesModel, int userId);
        public Task<List<UserNote>> ViewNotes(int userId);

        public UserNote ViewNotebyId(int userId, NotesIdModel noteIdmodel);

        public UserNote EditNote(EditNotesModel editModel, int userId);

        public bool DeleteNote(int userId, NotesIdModel notenoteIdmodelId);

        public bool ArchUnarchived(int userId, NotesIdModel noteIdmodel);

        public bool TrashUnTrash(int userId, NotesIdModel noteIdmodel);
    }
}
