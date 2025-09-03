namespace AAO.Common.BCSCSelfAssessment
{
    public class SubSpecialityDetailVM
    {
        public long SpecialityId { get; set; }

        public string SpecialityName { get; set; }

        public bool IsActive { get; set; }

        public int TotalCount { get; set; }

        public int MarkQuestionCount { get; set; }

        public int IncorrectAsnwerCount { get; set; }

        public int ExamSkipQuestionCount { get; set; }
    }
}
