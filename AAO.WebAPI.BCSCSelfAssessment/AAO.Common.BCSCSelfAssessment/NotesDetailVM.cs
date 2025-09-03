namespace AAO.Common.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SectionsList
    {
        public List<NoteSections> NoteSectionsLists { get; set; }
    }

    public class NoteSections
    {
        public int SubSpecialtyId { get; set; }

        public string SubSpecialty { get; set; }

        public int BCSCSectionNumber { get; set; }

        public string BCSCSectionTitle { get; set; }

        public List<ChapterDetails> ChapterList { get; set; }

        public int? SerialNumber { get; set; }

        public int QuestionCount { get; set; }
    }

    public class ChapterDetails
    {
        public string Chapter { get; set; }

        public string ChapterName { get; set; }

        public int TopicId { get; set; }

        public string TopicName { get; set; }

        public List<QuestionDetail> QuestionList { get; set; }

        public int ChapterCount { get; set; }

        public int ChapterSerialNumber { get; set; }
    }

    public class QuestionDetail
    {
        public int QuestionId { get; set; }

        public string Stem { get; set; }

        public List<Notes> NotesList { get; set; }

        public List<Choice> ChoiceList { get; set; }

        public int RightChoiceId { get; set; }

        public int UserChoiceId { get; set; }

        public List<QuestionImage> QuestionImageList { get; set; }

        public string TopicName { get; set; }

        public string Topic { get; set; }
    }

    public class NoteBookRequest
    {
        public int UserId { get; set; }

        public int? SubSpecialtyId { get; set; }

        public string SubSpecialty { get; set; }

        public int BCSCSectionNumber { get; set; }

        public string BCSCSectionTitle { get; set; }

        public string Chapter { get; set; }

        public string ChapterName { get; set; }

        public int TopicId { get; set; }

        public string TopicName { get; set; }

        public int NoOfRecords { get; set; }

        public int PageNo { get; set; }
    }
    public class Notes
    {
        public int NotesId { get; set; }

        public string NotesText { get; set; }

        public int QuestionId { get; set; }
    }

    public class NotesRequest
    {
        public int QuestionId { get; set; }

        public int UserId { get; set; }

        public int NotesId { get; set; }

        public int ExamId { get; set; }

        public int ExamAttemptId { get; set; }

        public DateTime NotesCreatedDate { get; set; }

        public DateTime NotesUpdatedDate { get; set; }

        public string NotesText { get; set; }
    }

}
