namespace AAO.DTO.BCSCSelfAssessment
{
    public class ExamAttemptAnswerDTO
    {
        public int ExamAttemptAsnwerId { get; set; }

        public System.DateTime ExamAttemptDate { get; set; }

        public int ChoiceId { get; set; }

        public bool IsRight { get; set; }

        public int UserId { get; set; }

        public int QuestionId { get; set; }

        public int ExamAttemptId { get; set; }

        public int IsYourAnswer { get; set; }

        public string UserExamState { get; set; }

        public int RoundNumber { get; set; }

        public double EasinessFactor { get; set; }

        public double NewEasinessFactor { get; set; }

        public int Quality { get; set; }

        public int Iteration { get; set; }

        public int SessionId { get; set; }
    }
}
