using System;

namespace AAO.DTO.BCSCSelfAssessment
{
    public class TopicContentDTO
    {
        public int QuestionId { get; set; }

        public int TopicId { get; set; }

        public int SubspecialtyId { get; set; }

        public int SubprogramId { get; set; }

        public string Chapter { get; set; }

        public string ChapterName { get; set; }

        public string Content { get; set; }

        public string LastUpdateOn_IsUpdated { get; set; }

        public string LastUpdateOn_BookUpdateAPI { get; set; }
    }
}
