using BusinessLayer.BLException;
using BusinessLayer.Interface;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.RLException;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class NotesBL : INotesBL
    {
        private readonly INotesRL _notesRL;
        private readonly IDistributedCache _cache;
        private List<UserNote> noteList;

        public NotesBL(INotesRL notesRL, IDistributedCache cache)
        {
            _notesRL = notesRL;
            _cache = cache;
        }

        public async Task<UserNote> AddNote(NoteCreationModel notesModel, int userId)
        {
            try
            {
                var dbnote = await _notesRL.AddNote(notesModel, userId);
                await CacheUserNoteAsync(dbnote, _cache);
                
                return dbnote;
            }
            catch (RepositoryLayerException ex)
            {
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public async Task<List<UserNote>> ViewNotes(int userId)
        {
            try
            {
                var uId = getUserIdForCache(userId);
                var cachedNotesJson = await _cache.GetStringAsync(uId);
                if (cachedNotesJson != null)
                {
                    return JsonSerializer.Deserialize<List<UserNote>>(cachedNotesJson);
                }
                else
                {
                    var notes = _notesRL.ViewNotes(userId);
                    await _cache.SetStringAsync(uId, JsonSerializer.Serialize(notes));
                    return notes;
                }
                
            }
            catch (RepositoryLayerException ex)
            {
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public UserNote ViewNotebyId(int userId, NotesIdModel noteIdmodel)
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

        public UserNote EditNote(EditNotesModel notesModel, int userId)
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

        private async Task CacheUserNoteAsync(UserNote dbnote, IDistributedCache cache)
        {
            try
            {
                await _cache.SetStringAsync(Convert.ToString(dbnote.NoteId), JsonSerializer.Serialize(dbnote));
                
                var userid = getUserIdForCache(dbnote.UserId);
                var userIdCacheNote = await cache.GetStringAsync(userid);

                if (userIdCacheNote == null)
                {
                    noteList = new List<UserNote> { dbnote };
                    await _cache.SetStringAsync(Convert.ToString(userid), JsonSerializer.Serialize(noteList));
                }
                else
                {
                    List<UserNote> noteListDeserialize = JsonSerializer.Deserialize<List<UserNote>>(userIdCacheNote);
                    if (noteListDeserialize != null)
                    {
                        noteListDeserialize.Add(dbnote);
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
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        private string getUserIdForCache(int userId)
        {
            return $"User_{userId}";
        }
    }
}

