namespace AAO.Common.BCSCSelfAssessment
{
    public class ImageVM
    {
        public string Caption { get; set; }

        public string Comments { get; set; }

        public string Credit { get; set; }

        public string Figure { get; set; }

        public string Path { get; set; }

        public string Source { get; set; }

        public string Title { get; set; }
    }

    public class AnswerVM
    {
        public bool? Correct { get; set; }

        public string Text { get; set; }

        public long? Sequence { get; set; }

        public string Id { get; set; }

        public int QuestionId { get; set; }

        public bool AnswerType { get; set; }
    }

    public class QuestionVM
    {
        public bool? QuestionType { get; set; }

        public bool? Active { get; set; }

        public System.DateTime? Createdon { get; set; }

        public bool? Deleted { get; set; }

        public string Id { get; set; }

        public string References { get; set; }

        public long? Status { get; set; }

        public string Stem { get; set; }

        public string FriendlyId { get; set; }

        public System.DateTime? ModifiedOn { get; set; }

        public bool? NotRandomizeAnswers { get; set; }

        public string RelatedContents { get; set; }
    }

    public class TopicContentVM
    {
        public bool? Active { get; set; }

        public string Code { get; set; }

        public bool? Deleted { get; set; }

        public object ModifiedOn { get; set; }

        public string Title { get; set; }

        public string TopicId { get; set; }

        public string Chapter { get; set; }

        public string ChapterName { get; set; }

        public string Topic { get; set; }

        public string TopicName { get; set; }
    }

    public class DiscussionVM
    {
        public int DiscussionId { get; set; }

        public int QuestionId { get; set; }

        public bool DiscussionType { get; set; }

        public string DiscussionText { get; set; }
    }

    public class QuestionTopicXWalkVM
    {
        public string QASTopicId { get; set; }

        public string QASQuestionId { get; set; }

        public int QuestionId { get; set; }

        public int TopicId { get; set; }
    }

    public class ProgramVM
    {
        public string Program { get; set; }

        public string QASProgramid { get; set; }

        public bool IsActive { get; set; }
    }

    public class SubProgramVM
    {
        public string Subprogram { get; set; }

        public string QASSubprogramid { get; set; }

        public int ProgramId { get; set; }

        public bool IsActive { get; set; }
    }

    public class SubSpecialityVM
    {
        public string Subspecialty { get; set; }

        public string QASSubspecialtyid { get; set; }

        public int SubprogramId { get; set; }

        public int IsActive { get; set; }
    }

    public class SubspecialtyTopicXWalkVM
    {
        public int SubSpecialityId { get; set; }

        public int TopicId { get; set; }
    }
}
