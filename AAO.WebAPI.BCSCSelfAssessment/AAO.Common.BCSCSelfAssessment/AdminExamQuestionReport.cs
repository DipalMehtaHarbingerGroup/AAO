using System;

namespace AAO.Common.BCSCSelfAssessment
{
   public class AdminExamQuestionReport
    {
        public int ID { get; set; }

        public string Email { get; set; }

        public string MasterCustomerID { get; set; }

        public string UserName { get; set; }

        public string OptInStatus { get; set; }

        public int ExamId { get; set; }

        public string ExamStartDate { get; set; }

        public string ExamType { get; set; }

        public string SelectedSubspeciality { get; set; }

        public int NoOfQuestions { get; set; }

        public string TypeOfQuestion { get; set; }

        public string ExamMode { get; set; }

        public string ExamAnswerToShow { get; set; }

        public string ExamTimeType { get; set; }

        public int QuestionId { get; set; }

        public string Stem { get; set; }

        public string BCSCSectionTitle { get; set; }

        public string ChapterName { get; set; }

        public string TopicName { get; set; }

        public string ChallengeModeTypedAnswer { get; set; }

        public string ChallengeModeAnswerText { get; set; }

        public string UserSelectedChoice { get; set; }

        public int UserAnsweredcorrectly { get; set; }

        public int UserAnsweredincorrectly { get; set; }

        public int UsersRepeatedSRMode { get; set; }

        public string Percentagecorrectly { get; set; }

        public string PercentageIncorrectly { get; set; }

        public string CorrectAnswer { get; set; }

        public string Distractor_1 { get; set; }

        public int User_chose_distractor_1 { get; set; }

        public string Distractor_2 { get; set; }

        public int User_chose_distractor_2 { get; set; }

        public string Distractor_3 { get; set; }

        public int User_chose_distractor_3 { get; set; }

        public int Year { get; set; }

        public int NoOfRecords { get; set; }

        public int PageNo { get; set; }

        public string OrderBy { get; set; }

        public string Seq { get; set; }
    }
}
