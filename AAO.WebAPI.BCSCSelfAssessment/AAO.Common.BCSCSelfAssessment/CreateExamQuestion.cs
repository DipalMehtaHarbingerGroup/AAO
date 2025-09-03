namespace AAO.Common.BCSCSelfAssessment
{

   public class SpecialityId
    {
        public int specialityId { get; set; }
    }

   public class SelectedQuestionTypeId
    {
        public int questionTypeId { get; set; }
    }

   public class ExamQuestionId
    {
        public int examQuestionId { get; set; }
    }

   public class SpecialityQuestion
    {
        public int LspecialityId { get; set; }

        public int LQuestionvalue { get; set; }

        public int CurrentCount { get; set; }
    }

   public class SpecialityQuestionList
    {
        public int LspecialityId { get; set; }

        public int LQuestionvalue { get; set; }

        public int CurrentCount { get; set; }
    }

   public class TypeOfQuestion
    {
        public int LquestionTypeId { get; set; }

        public int LQuestionvalue { get; set; }

        public int CurrentCount { get; set; }
    }

   public class ResponseStatusVM
    {
        public int UserEnteredQuestionCount { get; set; }

        public int SystemGeneratedQuestionCount { get; set; }

        public int ErrorCode { get; set; }

        public string Errormessage { get; set; }

        public int ExamId { get; set; }
    }
}
