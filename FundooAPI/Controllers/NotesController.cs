using BusinessLayer.BLException;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UserApi.Controllers
{

    [ApiController]
    [Route("api/notes")]
    public class NotesController : ControllerBase
    {
        private INotesBL _notesBL;
        private readonly ILogger<NotesController> _logger;

        public NotesController(INotesBL notesBL, ILogger<NotesController> logger)
        {
            _notesBL = notesBL;
            _logger = logger;
        }
        /// <summary>
        /// This Api is used to create notes for the user
        /// </summary>
        /// <param name="notesModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateNote(NoteCreationModel notesModel)
        {
            try
            {
                int userId = GetUserIdFromClaims();
                var note = await _notesBL.AddNote(notesModel, userId);

                var response = new ResponseModel<NoteModel>();
                if (note != null)
                {
                    response.Success = true;
                    response.Message = "Note created succesfully";
                    response.Data = note;
                    return Created(string.Empty, response);
                }
                response.Success = false;
                response.Message = "Error creating note. Please try again";
                return BadRequest(response);
            }
            catch (BusinessLayerException ex)
            {

                LogException(ex, "Error occurred in CreateNote method.");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This Api is used to fetch all the notes for the user 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewNotes()
        {
            try
            {
                var response = new ResponseModel<List<NoteModel>>();
                int userId = GetUserIdFromClaims();
                var listOfNotes = await _notesBL.ViewNotes(userId);

                if (listOfNotes != null)
                {
                    response.Success = true;
                    response.Message = "Notes Retrieved successfully";
                    response.Data = listOfNotes;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "No Notes found for the specific user";
                return NotFound(response);
            }
            catch (BusinessLayerException ex)
            {
                LogException(ex, "Error occurred in ViewNotes method.");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This Api is used to fetch note using note id
        /// </summary>
        /// <param name="noteIdmodel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("id")]
        public async Task<IActionResult> ViewNoteByID(NotesIdModel noteIdModel)
        {
            try
            {
                var response = new ResponseModel<NoteModel>();
                int userId = GetUserIdFromClaims();
                var result = await _notesBL.ViewNotebyId(userId, noteIdModel);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Note retrieved Successfully";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = $"Note does exist for id {noteIdModel.NoteId}";
                return NotFound(response);
            }
            catch (BusinessLayerException ex)
            {
                LogException(ex, "Error occurred in ViewNoteById method.");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This Api is used to edit note
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> EditNote(EditNotesModel editNotesModel)
        {
            try
            {
                var response = new ResponseModel<NoteModel>();
                int userId = GetUserIdFromClaims();
                var result = await _notesBL.EditNote(editNotesModel, userId);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Noted Edited successfully";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Error while editing the note,Please try again";
                return NotFound(response);
            }
            catch (BusinessLayerException ex)
            {
                LogException(ex, "Error occurred in EditNote method.");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This Api is used to delete note for the given user id
        /// </summary>
        /// <param name="noteIdModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteNote(NotesIdModel noteIdModel)
        {
            try
            {
                var response = new ResponseModel<bool>();
                int userId = GetUserIdFromClaims();
                bool result = await _notesBL.DeleteNote(userId, noteIdModel);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Note Deleted successfully";
                    response.Data = true;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "There was a Error while deleting the node, Please try again";
                return NotFound(response);
            }
            catch (BusinessLayerException ex)
            {
                LogException(ex, "Error occurred in DeleteNote method.");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This Api is used to Archive and Unarchive the note
        /// </summary>
        /// <param name="noteIdModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch]
        [Route("archiving")]
        public async Task<IActionResult> ArchUnarchived(NotesIdModel noteIdModel)
        {
            try
            {
                var response = new ResponseModel<bool>();
                int userId = GetUserIdFromClaims();
                bool result = await _notesBL.ArchUnarchived(userId, noteIdModel);
                if (result)
                {
                    response.Success = true;
                    response.Message = "ArchUnarchived Performed Successfully";
                    response.Data = true;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Unable to find the note for given user, Please try again";
                return BadRequest(response);
            }
            catch (BusinessLayerException ex)
            {
                LogException(ex, "Error occurred in ArchUnarchived method.");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This Api is used to move note to trash and vice-versa.
        /// </summary>
        /// <param name="noteIdModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch]
        [Route("trashing")]
        public async Task<IActionResult> TrashUnTrash(NotesIdModel noteIdModel)
        {
            try
            {
                var response = new ResponseModel<bool>();
                int userId = GetUserIdFromClaims();
                bool result = await _notesBL.TrashUnTrash(userId, noteIdModel);
                if (result)
                {
                    response.Success = true;
                    response.Message = "TrashUnTrash Performed Successfully";
                    response.Data = true;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Unable to find the note for given user, Please try again ";
                return BadRequest(response);
            }
            catch (BusinessLayerException ex)
            {
                LogException(ex, "Error occurred in TrashUnTrash method.");
                return StatusCode(500, ex.Message);
            }
        }

        private int GetUserIdFromClaims()
        {
            string userId = User.FindFirstValue("UserId");
            return Convert.ToInt32(userId);
        }

        private void LogException(Exception ex, string message)
        {
            if (ex.InnerException != null)
                _logger.LogError(ex.InnerException, message);
            else
                _logger.LogError(ex, message);
        }
    }
}
