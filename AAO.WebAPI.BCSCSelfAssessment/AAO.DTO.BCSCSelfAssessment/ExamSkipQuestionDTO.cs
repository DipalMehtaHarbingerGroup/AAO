namespace AAO.DTO.BCSCSelfAssessment
{
    public class ExamSkipQuestionDTO
    {
        public int ExamSkipQuestionId { get; set; }

        public System.DateTime ExamCreatedDate { get; set; }

        public System.DateTime ExamLastAttemptDate { get; set; }

        public int ExamId { get; set; }

        public int UserId { get; set; }

        public int QuestionId { get; set; }

        public int ExamAttemptId { get; set; }
    }
}
