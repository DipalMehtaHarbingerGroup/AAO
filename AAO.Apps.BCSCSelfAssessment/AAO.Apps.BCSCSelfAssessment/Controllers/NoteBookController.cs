namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Mvc;
    using Common.BCSCSelfAssessment;
    using Newtonsoft.Json;
    using Proxy;

    public class NoteBookController : BaseResidentController
    {
        // GET: NoteBook
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetQuestions(string chapterName, int? subSpecialtyId, int? noOfRecords = 5, int? pageNo = 1)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                NoteBookRequest notesRequest = new NoteBookRequest();
                notesRequest.UserId = user._userId;
                notesRequest.SubSpecialtyId = subSpecialtyId;
                if (chapterName == null || chapterName == string.Empty || chapterName == "0")
                {
                    chapterName = null;
                }
                else
                {
                    notesRequest.ChapterName = Convert.ToString(chapterName);
                }

                notesRequest.NoOfRecords = Convert.ToInt32(noOfRecords);
                notesRequest.PageNo = Convert.ToInt32(pageNo);

                string notesPostDataJson = JsonConvert.SerializeObject(notesRequest);

                string noteBookurl = AAOGlobalConstants.SiteWebAPIUrl + "NoteBook/GetNoteBookDetails";
                string noteBookResult = HttpProxy.HttpPost(noteBookurl, notesPostDataJson, "application/json; charset=utf-8", "POST");
                SectionsList noteBookList = new SectionsList();
                noteBookList = JsonConvert.DeserializeObject<SectionsList>(noteBookResult);
                ViewBag.noteBookList = noteBookList.NoteSectionsLists;

                if (noteBookList.NoteSectionsLists.Count > 0 && noteBookList.NoteSectionsLists[0].ChapterList.Count > 0)
                {
                    ViewBag.FirstSerialNum = noteBookList.NoteSectionsLists[0].ChapterList[0].ChapterSerialNumber;
                    ViewBag.LastSerialNum = noteBookList.NoteSectionsLists[0].ChapterList[noteBookList.NoteSectionsLists[0].ChapterList.Count - 1].ChapterSerialNumber;
                }

                ViewBag.ChapterCount = noteBookList.NoteSectionsLists[0].ChapterList[0].ChapterCount;
                ViewBag.SubSpecialtyId = subSpecialtyId;
                ViewBag.chapterName = chapterName;
                ViewBag.NoOfRecords = noOfRecords;
                ViewBag.PageNo = pageNo;

                ViewBag.viewCurrentRecordcount = noteBookList.NoteSectionsLists.Count;
                ViewBag.viewAssessmentdetailscount = noteBookList.NoteSectionsLists.Count > 0 ? noteBookList.NoteSectionsLists[0].QuestionCount : 0;

                return View("GetQuestions");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ActionResult GetSectionList()
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                NoteBookRequest notesRequest = new NoteBookRequest();
                notesRequest.UserId = user._userId;
                string notesPostDataJson = JsonConvert.SerializeObject(notesRequest);

                string sectionurl = AAOGlobalConstants.SiteWebAPIUrl + "NoteBook/GetSectionList";
                string sectionResult = HttpProxy.HttpPost(sectionurl, notesPostDataJson, "application/json; charset=utf-8", "POST");
                List<NoteSections> sectionList = new List<NoteSections>();
                sectionList = JsonConvert.DeserializeObject<List<NoteSections>>(sectionResult);
                ViewBag.SectionList = sectionList;
                return Json(sectionList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ActionResult GetChapterList(int? subSpecialtyId)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                NoteBookRequest notesRequest = new NoteBookRequest();
                notesRequest.UserId = user._userId;
                notesRequest.SubSpecialtyId = Convert.ToInt32(subSpecialtyId);
                string notesPostDataJson = JsonConvert.SerializeObject(notesRequest);

                string chapterurl = AAOGlobalConstants.SiteWebAPIUrl + "NoteBook/GetChapterList";
                string chapterResult = HttpProxy.HttpPost(chapterurl, notesPostDataJson, "application/json; charset=utf-8", "POST");
                List<ChapterDetails> chapterList = new List<ChapterDetails>();
                chapterList = JsonConvert.DeserializeObject<List<ChapterDetails>>(chapterResult);

                ViewBag.ChapterList = chapterList;
                return Json(chapterList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ActionResult AddNotes(int? questionId, string notesText)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                NotesRequest notesRequest = new NotesRequest();
                notesRequest.UserId = user._userId;
                notesRequest.ExamId = 0;
                notesRequest.ExamAttemptId = 0;
                notesRequest.NotesCreatedDate = DateTime.Now;
                notesRequest.NotesUpdatedDate = DateTime.Now;
                notesRequest.NotesText = Convert.ToString(WebUtility.UrlDecode(notesText));
                notesRequest.QuestionId = Convert.ToInt32(questionId);
                string notesPostDataJson = JsonConvert.SerializeObject(notesRequest);

                string editNotesurl = AAOGlobalConstants.SiteWebAPIUrl + "NoteBook/AddNotes";
                string editNotesResult = HttpProxy.HttpPost(editNotesurl, notesPostDataJson, "application/json; charset=utf-8", "POST");

                List<Notes> notesList = new List<Notes>();
                notesList = JsonConvert.DeserializeObject<List<Notes>>(editNotesResult);
                ViewBag.NotesList = notesList;
                return Json(notesList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ActionResult UpdateNotes(int? notesId, int? questionId, string notesText)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                NotesRequest notesRequest = new NotesRequest();
                notesRequest.UserId = user._userId;
                notesRequest.NotesId = Convert.ToInt32(notesId);
                notesRequest.NotesUpdatedDate = DateTime.Now;
                notesRequest.NotesText = Convert.ToString(WebUtility.UrlDecode(notesText));
                notesRequest.QuestionId = Convert.ToInt32(questionId);
                string notesPostDataJson = JsonConvert.SerializeObject(notesRequest);

                string editNotesurl = AAOGlobalConstants.SiteWebAPIUrl + "NoteBook/UpdateNotes";
                string editNotesResult = HttpProxy.HttpPost(editNotesurl, notesPostDataJson, "application/json; charset=utf-8", "POST");

                List<Notes> notesList = new List<Notes>();
                notesList = JsonConvert.DeserializeObject<List<Notes>>(editNotesResult);
                ViewBag.NotesList = notesList;
                return Json(notesList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ActionResult DeleteNotes(int? notesId, int? questionId)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                NotesRequest notesRequest = new NotesRequest();
                notesRequest.UserId = user._userId;
                notesRequest.NotesUpdatedDate = DateTime.Now;
                notesRequest.NotesId = Convert.ToInt32(notesId);
                notesRequest.QuestionId = Convert.ToInt32(questionId);
                string notesPostDataJson = JsonConvert.SerializeObject(notesRequest);

                string deleteNotesurl = AAOGlobalConstants.SiteWebAPIUrl + "NoteBook/DeleteNotes";
                string deleteNotesResult = HttpProxy.HttpPost(deleteNotesurl, notesPostDataJson, "application/json; charset=utf-8", "POST");

                List<Notes> notesList = new List<Notes>();
                notesList = JsonConvert.DeserializeObject<List<Notes>>(deleteNotesResult);
                ViewBag.NotesList = notesList;
                return Json(notesList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ActionResult GetNotesList(string chapterName, int? subSpecialtyId)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                NoteBookRequest notesRequest = new NoteBookRequest();
                notesRequest.UserId = user._userId;
                notesRequest.SubSpecialtyId = subSpecialtyId;
                if (chapterName == null || chapterName == string.Empty || chapterName == "0")
                {
                    chapterName = null;
                }
                else
                {
                    notesRequest.ChapterName = Convert.ToString(chapterName);
                }

                notesRequest.NoOfRecords = Convert.ToInt32(100);
                notesRequest.PageNo = Convert.ToInt32(1);

                string notesPostDataJson = JsonConvert.SerializeObject(notesRequest);

                string noteBookurl = AAOGlobalConstants.SiteWebAPIUrl + "NoteBook/GetNoteBookDetails";
                string noteBookResult = HttpProxy.HttpPost(noteBookurl, notesPostDataJson, "application/json; charset=utf-8", "POST");
                SectionsList noteBookList = new SectionsList();
                noteBookList = JsonConvert.DeserializeObject<SectionsList>(noteBookResult);
                ViewBag.noteBookList = noteBookList.NoteSectionsLists;

                if (noteBookList.NoteSectionsLists.Count > 0)
                {
                    ViewBag.FirstSerialNum = noteBookList.NoteSectionsLists[0].ChapterList[0].ChapterSerialNumber;
                    ViewBag.LastSerialNum = noteBookList.NoteSectionsLists[0].ChapterList[noteBookList.NoteSectionsLists[0].ChapterList.Count - 1].ChapterSerialNumber;
                    ViewBag.ChapterCount = noteBookList.NoteSectionsLists[0].ChapterList[0].ChapterCount;
                }

                ViewBag.SubSpecialtyId = subSpecialtyId;
                ViewBag.chapterName = chapterName;
                ViewBag.NoOfRecords = notesRequest.NoOfRecords;
                ViewBag.PageNo = notesRequest.PageNo;
                ViewBag.viewCurrentRecordcount = noteBookList.NoteSectionsLists.Count;
                ViewBag.viewAssessmentdetailscount = noteBookList.NoteSectionsLists.Count > 0 ? noteBookList.NoteSectionsLists[0].QuestionCount : 0;

                return View("GetNotesList");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                // return JavaScript("window.close();");
                return View("Close");

                // return null;
            }
        }

        public ActionResult SubmitForm()
        {
            return View("Close");
        }
    }
}