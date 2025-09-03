namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using BAL.BCSCSelfAssessment;
    using Common.BCSCSelfAssessment;

    public class NoteBookController : ApiController
    {
        [Route("api/NoteBook/GetNoteBookDetails")]
        [HttpPost]
        [HttpGet]
        public SectionsList GetNoteBookDetails(NoteBookRequest noteBookRequest)
        {
            return NoteBookBL.GetNoteBookDetails(noteBookRequest);
        }

        [Route("api/NoteBook/GetSectionList")]
        [HttpPost]
        [HttpGet]
        public List<NoteSections> GetSectionList(NoteBookRequest noteBookRequest)
        {
            return NoteBookBL.GetSectionList(noteBookRequest);
        }

        [Route("api/NoteBook/GetChapterList")]
        [HttpPost]
        [HttpGet]
        public List<ChapterDetails> GetChapterList(NoteBookRequest noteBookRequest)
        {
            return NoteBookBL.GetChapterList(noteBookRequest);
        }

        [Route("api/NoteBook/AddNotes")]
        [HttpPost]
        [HttpGet]
        public List<Notes> AddNotes(NotesRequest notesRequest)
        {
            return NoteBookBL.AddNotes(notesRequest);
        }

        [Route("api/NoteBook/UpdateNotes")]
        [HttpPost]
        [HttpGet]
        public List<Notes> UpdateNotes(NotesRequest notesRequest)
        {
            return NoteBookBL.UpdateNotes(notesRequest);
        }

        [Route("api/NoteBook/DeleteNotes")]
        [HttpPost]
        [HttpGet]
        public List<Notes> DeleteNotes(NotesRequest notesRequest)
        {
            return NoteBookBL.DeleteNotes(notesRequest);
        }
    }
}
