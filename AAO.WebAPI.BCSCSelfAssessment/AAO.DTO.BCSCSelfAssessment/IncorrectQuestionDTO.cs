namespace AAO.DTO.BCSCSelfAssessment
{
    public class IncorrectQuestionDTO
    {
        public int IncorrectQuestionId { get; set; }

        public int QuestionId { get; set; }

        public int UserId { get; set; }

        public int ExamAttemptId { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsRight { get; set; }
    }
}
