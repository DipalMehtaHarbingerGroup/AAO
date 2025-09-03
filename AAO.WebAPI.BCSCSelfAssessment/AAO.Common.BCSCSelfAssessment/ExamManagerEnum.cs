namespace AAO.Common.BCSCSelfAssessment
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public static class ExamManagerEnum
    {
        public enum ExamType : int
        {
            [Description("Custom Exam")]
            CustomMode,
            [Description("Simulated Exam")]
            SimulatedMode,
            [Description("Quick Exam")]
            QuickMode,
            [Description("Spaced Repetition")]
            SpacedRepetition,

            //New exam type added
            [Description("Shared Exam")]
            SharedMode,
        }

        public enum ExamMode : int
        {
            [Display(Name = "Challendged Mode")]
            Challenge = 0,

            [Display(Name = "Normal Mode")]
            Normal = 1,
        }

        public enum ExamTimeType : int
        {
            [Display(Name = "Time Mode")]
            Time = 0,

            [Display(Name = "Non-Timed Mode")]
            NonTimed = 1,
        }

        public enum ExamAnswerToShow : int
        {
            Yes = 0,
            No = 1,
        }

        public enum ExamQuestionSelectionType
        {
            [Description("All questions")]
            AllQuestions,

            [Description("Unanswered questions")]
            UnAnsweredQuestions,

            [Description("Incorrect questions")]
            IncorrectQuestions,

            [Description("Bookmarked questions")]
            MarkedQuestions,
        }

        public enum IsYourAnswer : int
        {
            Default=0,
            Yes = 1,
            No = 2,
        }

        public enum ExamStatus : int
        {
            Inprogress = 1,
            Completed = 2,
        }
    }
}
