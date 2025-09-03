namespace AAO.BAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;
    using System.Configuration;
    using Serilog;
    using System.Data.SqlClient;
    using AAO.SQLHelper.BCSCSelfAssessment;
    public static class UserViewBL
    {

        private static string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        private static Serilog.Core.Logger log = new LoggerConfiguration().WriteTo.RollingFile(_sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();

        // GET: ExamManager

        public static QuestionDetails GetQuestionById(int? friendlyId, int? userId)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            connection.Open();
            int questionId = UserViewDAL.GetQuestionIdByFriendlyid(friendlyId, connection);
            try
            {
                log.Information("start method :{0} questionId:{1}", "GetPeerDetails", 
                    questionId);
                List<Choice> choiceList = new List<Choice>();
                QuestionDetails questionDetails = new QuestionDetails();
                log.Information("Start GetQuestionDetails");
                // get QuestionDetail with Next and Previous Id
                questionDetails = UserViewDAL.GetQuestionDetailsByQuestionid(Convert.ToInt32(questionId));
                log.Information("End GetQuestionDetails");
                // Get Question Choice by Question Id
               // choiceList = AssessmentDAL.GetChoiceBYQuestionId(Convert.ToInt32(questionId));
                // questionDetails.choiceList = choiceList;

                // Get Right Choice for Given Question Id
                questionDetails.rightChoiceId = AssessmentDAL.GetRightChoiceByQuestionId(Convert.ToInt32(questionId), connection);
                questionDetails.examAnswerToShow = true;
                questionDetails.questionDiscussion = AssessmentDAL.GetQuestionDiscussion(Convert.ToInt32(questionId), connection);

                // Get References from Question Table on base on Question Id
                questionDetails.references = AssessmentDAL.GetReferences(Convert.ToInt32(questionId), connection);
                List<string> notesList = new List<string>();
                notesList.Add("You have not created a note for this question.");

                // Get RelatedContent from RelatedCOntent On Base on QuestionId
                questionDetails.RelatedContents = AssessmentDAL.GetRelatedContent(Convert.ToInt32(questionId), connection);

                // Get Notes from Notes On Base on QuestionId and ExamId
                questionDetails.notes = notesList;
                //AssessmentDAL.GetNotes(Convert.ToInt32(questionId), Convert.ToInt32(userId), Convert.ToInt32(examId), connection);

                // Get Question Choice by Question Id
                choiceList = UserViewDAL.GetChoiceListBYQuestionId(Convert.ToInt32(questionId), questionDetails.NotRandomizeAnswer, connection);
                questionDetails.choiceList = choiceList;

                // Get BCSCExcerpt for this Question
                log.Information("start method :{0} questionId:{1}", "GetBCSCExcerptText", questionId);
                questionDetails.BCSCExcerpt = AssessmentDAL.GetBCSCExcerptText(Convert.ToInt32(questionId));
                log.Information("end method :{0} questionId:{1}", "GetBCSCExcerptText", questionId);

                questionDetails.ExcerptHeadings = AssessmentBL.GetExcerptHeadings(questionDetails.BCSCExcerpt);

                // Get Topic for this Question
                questionDetails.TopicName = AssessmentDAL.GetTopicByQuestion(Convert.ToInt32(questionId));
                log.Information("end method :{0} questionId:{1}", "GetExamQuestion", questionId);
                connection.Close();
                return questionDetails;
            }
            catch (Exception e)
            {
                log.Error(e.Message + "questionId:" + questionId + "UserId:" + userId);
                Console.Write(e.Message);
                return null;
            }
        }
    }
}
