using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INotesRL
    {
        public Task<UserNote> AddNote(NoteCreationModel notesModel, int userId);
        public Task<List<UserNote>> ViewNotes(int userId);

        public Task<UserNote> ViewNotebyId(int userId, int noteId);

        public Task<UserNote> EditNote(EditNotesModel editModel, int userId);

        public Task<bool> DeleteNote(int userId, int noteId);

        public Task<bool> ArchUnarchived(int userId, int noteId);

        public Task<bool> TrashUnTrash(int userId, int noteId);
    }
}
