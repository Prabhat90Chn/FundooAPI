using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INotesRL
    {
        public Task<UserNote> AddNote(NoteCreationModel notesModel, int userId);
        public List<UserNote> ViewNotes(int userId);

        public UserNote ViewNotebyId(int userId, int noteId);

        public UserNote EditNote(EditNotesModel editModel, int userId);

        public bool DeleteNote(int userId, int noteId);

        public bool ArchUnarchived(int userId, int noteId);

        public bool TrashUnTrash(int userId, int noteId);
    }
}
