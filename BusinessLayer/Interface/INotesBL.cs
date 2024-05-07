using ModelLayer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface INotesBL
    {

        public Task<NoteModel> AddNote(NoteCreationModel notesModel, int userId);
        public Task<List<NoteModel>> ViewNotes(int userId);

        public Task<NoteModel> ViewNotebyId(int userId, NotesIdModel noteIdmodel);

        public Task<NoteModel> EditNote(EditNotesModel editModel, int userId);

        public Task<bool> DeleteNote(int userId, NotesIdModel notenoteIdmodelId);

        public Task<bool> ArchUnarchived(int userId, NotesIdModel noteIdmodel);

        public Task<bool> TrashUnTrash(int userId, NotesIdModel noteIdmodel);
    }
}
