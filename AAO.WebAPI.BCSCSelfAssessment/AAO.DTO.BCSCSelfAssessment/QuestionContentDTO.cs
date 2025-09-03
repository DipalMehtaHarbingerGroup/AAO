using System.Collections.Generic;

namespace AAO.DTO.BCSCSelfAssessment
{
    public class QuestionContentDTO
    {
        public int QuestionId { get; set; }

        public int Question_QAS_id { get; set; }

        public string Stem { get; set; }

        public int programId { get; set; }

        public string program { get; set; }

        public int SubprogramId { get; set; }

        public string subprogram { get; set; }

        public int SubspecialtyId { get; set; }

        public string Subspecialty { get; set; }

        public int TopicId { get; set; }

        public string TopicName { get; set; }

        public int ChapterId { get; set; }

        public string ChapterName { get; set; }

        public int PageIndex { get; set; }

        public int NoOfRecords { get; set; }

        public int RowNumber { get; set; }

        public int RecordCount { get; set; }

        public string radioButtonState { get; set; }

        public int FriendlyId { get; set; }

        public List<string> RelatedContents { get; set; }
    }
}
