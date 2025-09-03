namespace AAO.DTO.BCSCSelfAssessment
{
    public class ExamSummaryViewModelDTO
    {
        public int ExamId { get; set; }

        public string ExamName { get; set; }

        public string ExamCreateDate { get; set; }

        public int ExamType { get; set; }

        public int ExamMode { get; set; }

        public int UserId { get; set; }

        public int NoofQuestions { get; set; }

        public bool ExamTimeType { get; set; }

        public bool ExamAnswerToShow { get; set; }

        public string ExamLastAttemptDate { get; set; }

        public int ExamStatus { get; set; }
    }
}
