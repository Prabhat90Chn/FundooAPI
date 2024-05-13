using BusinessLayer.BLException;
using BusinessLayer.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.RLException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class NotesBL : INotesBL
    {
        private readonly INotesRL _notesRL;
        private readonly IDistributedCache _cache;
        private readonly ILogger<NotesBL> _logger;
        private List<NoteModel> noteList;
        private NoteModel noteModel = null;

        public NotesBL(INotesRL notesRL, IDistributedCache cache, ILogger<NotesBL> logger)
        {
            _notesRL = notesRL;
            _cache = cache;
            _logger = logger;
        }

        public async Task<NoteModel> AddNote(NoteCreationModel notesModel, int userId)
        {
            try
            {
                var dbnote = await _notesRL.AddNote(notesModel, userId);
                if (dbnote != null)
                {
                    noteModel = ConvertToNoteModel(dbnote);
                    await CacheUserNoteAsync(noteModel, _cache);
                }
                return noteModel;
            }
            catch (RepositoryLayerException ex)
            {
                LogException(ex, "Error occurred in AddNote method.");
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public async Task<List<NoteModel>> ViewNotes(int userId)
        {
            try
            {
                var uId = GetUserCacheKey(userId);
                var cachedNotesJson = await _cache.GetStringAsync(uId);

                if (!string.IsNullOrEmpty(cachedNotesJson))
                {
                    return JsonSerializer.Deserialize<List<NoteModel>>(cachedNotesJson);
                }
                else
                {
                    var listOfNotes = await _notesRL.ViewNotes(userId);
                    if (listOfNotes != null)
                    {
                        noteList = ConvertToNoteModels(listOfNotes);
                        await _cache.SetStringAsync(uId, JsonSerializer.Serialize(noteList));
                    }
                    return noteList;
                }

            }
            catch (RepositoryLayerException ex)
            {
                LogException(ex, "Error occurred in ViewNotes method.");
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public async Task<NoteModel> ViewNotebyId(int userId, NotesIdModel noteIdmodel)
        {
            try
            {
                var uId = GetUserCacheKey(userId);

                var noteIdNote = await _cache.GetStringAsync(noteIdmodel.NoteId);
                if (!string.IsNullOrEmpty(noteIdNote))
                {
                    return JsonSerializer.Deserialize<NoteModel>(noteIdNote);
                }

                var noteId = GetIntNotesId(noteIdmodel.NoteId);

                var dbNote = await _notesRL.ViewNotebyId(userId, noteId);
                if (dbNote != null)
                {
                    noteModel = ConvertToNoteModel(dbNote);
                    await _cache.SetStringAsync(noteIdmodel.NoteId, JsonSerializer.Serialize(noteModel));
                }
                return noteModel;
            }
            catch (RepositoryLayerException ex)
            {
                LogException(ex, "Error occurred in ViewNotebyId method.");
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public async Task<NoteModel> EditNote(EditNotesModel notesModel, int userId)
        {
            try
            {
                var dbNote = await _notesRL.EditNote(notesModel, userId);

                if (dbNote != null)
                {
                    var uId = GetUserCacheKey(userId);
                    var nId = GetIntNotesId(notesModel.NoteId);

                    noteModel = ConvertToNoteModel(dbNote);

                    await _cache.SetStringAsync(notesModel.NoteId, JsonSerializer.Serialize(noteModel));

                    var userNotes = await ViewNotes(userId);
                    var noteToUpdate = userNotes.FirstOrDefault(n => n.NoteId == nId);
                    if (noteToUpdate != null)
                    {
                        userNotes.Remove(noteToUpdate);
                        userNotes.Add(noteModel);
                        await _cache.SetStringAsync(uId, JsonSerializer.Serialize(userNotes));
                    }
                }
                return noteModel;
            }
            catch (RepositoryLayerException ex)
            {
                LogException(ex, "Error occurred in EditNote method.");
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteNote(int userId, NotesIdModel noteIdmodel)
        {
            try
            {
                var uId = GetUserCacheKey(userId);
                var noteId = GetIntNotesId(noteIdmodel.NoteId);
                var result = await _notesRL.DeleteNote(userId, noteId);

                if (result)
                {
                    await _cache.RemoveAsync(noteIdmodel.NoteId);

                    var userNotes = await ViewNotes(userId);
                    var noteToRemove = userNotes.FirstOrDefault(n => n.NoteId == noteId);

                    if (noteToRemove != null)
                    {
                        userNotes.Remove(noteToRemove);
                        await _cache.SetStringAsync(uId, JsonSerializer.Serialize(userNotes));
                    }
                }
                return result;
            }
            catch (RepositoryLayerException ex)
            {
                LogException(ex, "Error occurred in DeleteNote method.");
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public async Task<bool> ArchUnarchived(int userId, NotesIdModel noteIdmodel)
        {
            try
            {
                var noteId = GetIntNotesId(noteIdmodel.NoteId);
                return await _notesRL.ArchUnarchived(userId, noteId);
            }
            catch (RepositoryLayerException ex)
            {
                LogException(ex, "Error occurred in ArchUnarchived method.");
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public async Task<bool> TrashUnTrash(int userId, NotesIdModel noteIdmodel)
        {
            try
            {
                var noteId = GetIntNotesId(noteIdmodel.NoteId);
                return await _notesRL.TrashUnTrash(userId, noteId);
            }
            catch (RepositoryLayerException ex)
            {
                LogException(ex, "Error occurred in TrashUnTrash method.");
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        private async Task CacheUserNoteAsync(NoteModel note, IDistributedCache cache)
        {
            try
            {
                await _cache.SetStringAsync(Convert.ToString(note.NoteId), JsonSerializer.Serialize(note));

                var userid = GetUserCacheKey(note.UserId);
                var userIdCacheNote = await cache.GetStringAsync(userid);

                if (userIdCacheNote == null)
                {
                    noteList = new List<NoteModel> { note };
                    await _cache.SetStringAsync(Convert.ToString(userid), JsonSerializer.Serialize(noteList));
                }
                else
                {
                    List<NoteModel> noteListDeserialize = JsonSerializer.Deserialize<List<NoteModel>>(userIdCacheNote);
                    if (noteListDeserialize != null)
                    {
                        noteListDeserialize.Add(note);
                        await _cache.SetStringAsync(Convert.ToString(userid), JsonSerializer.Serialize(noteListDeserialize));
                    }
                    else
                    {
                        throw new BusinessLayerException("Unable to deserialize list of notes");
                    }

                }
            }
            catch (Exception ex)
            {
                LogException(ex, "Error occurred in CacheUserNoteAsync method.");
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        private int GetIntNotesId(string id)
        {
            if (int.TryParse(id, out int intValue))
            {
                return intValue;
            }
            return 0;
        }

        private string GetUserCacheKey(int userId)
        {
            return $"User_{userId}";
        }

        private NoteModel ConvertToNoteModel(UserNote userNote)
        {
            NoteModel noteModel = new NoteModel()
            {
                UserId = userNote.UserId,
                NoteId = userNote.NoteId,
                Title = userNote.Title,
                Description = userNote.Description,
                Colour = userNote.Colour
            };
            return noteModel;
        }

        private List<NoteModel> ConvertToNoteModels(List<UserNote> listOfNotes)
        {
            return listOfNotes.Select(ConvertToNoteModel).ToList();
        }

        private void LogException(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }
    }
}

