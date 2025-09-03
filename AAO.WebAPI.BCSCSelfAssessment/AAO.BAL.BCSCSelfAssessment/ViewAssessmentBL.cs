namespace AAO.BAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using Serilog;

    public static class ViewAssessmentBL
    {
        private static string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        private static Serilog.Core.Logger log = new LoggerConfiguration().WriteTo.RollingFile(_sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();

        public static List<QuestionDetails> ViewAssessmentDetails(ExamQuestionDTO examque)
        {
            List<QuestionDetails> examQuestionDetailsList = new List<QuestionDetails>();
            List<int> questionList = new List<int>();
            questionList = ViewAssessmentDAL.ViewAssessmentDetails(Convert.ToInt32(examque.ExamId));
            foreach (int item in questionList)
            {
                QuestionDetails examQuestionDetails = new QuestionDetails();
                examQuestionDetails = AssessmentBL.GetExamQuestion((Convert.ToInt32(examque.ExamId)), item, Convert.ToInt32(examque.UserId));
                examQuestionDetailsList.Add(examQuestionDetails);
            }

            return examQuestionDetailsList;
        }

        public static List<QuestionDetails> FilterByQuestions(AssesmentDetailVM assDetail)
        {
            List<QuestionDetails> examQuestionDetailsList = new List<QuestionDetails>();
            List<QuestionIdWithCountVM> questionList = new List<QuestionIdWithCountVM>();
            questionList = ViewAssessmentDAL.FilterByQuestions(Convert.ToInt32(assDetail.ExamId), Convert.ToString(assDetail.Filter), assDetail.NoOfRecords, assDetail.PageNo, assDetail.UserId);
            foreach (QuestionIdWithCountVM item in questionList)
            {
                QuestionDetails examQuestionDetails = new QuestionDetails();
                //examQuestionDetails = AssessmentBL.GetExamQuestion((Convert.ToInt32(assDetail.ExamId)), item.QuestionId, Convert.ToInt32(assDetail.UserId));
                examQuestionDetails = GetExamQuestion((Convert.ToInt32(assDetail.ExamId)), item.QuestionId, Convert.ToInt32(assDetail.UserId));
                examQuestionDetails.QuestionCount = item.QuestionIdCount;
                examQuestionDetails.serialNumber = item.QuestionNo;
                examQuestionDetailsList.Add(examQuestionDetails);
            }

            return examQuestionDetailsList;
        }

        public static List<QuestionDetails> ViewDetails(ExamQuestionDTO examque)
        {
            List<QuestionDetails> examQuestionDetailsList = new List<QuestionDetails>();
            List<int> questionList = new List<int>();
            questionList = ViewAssessmentDAL.ViewDetails(Convert.ToInt32(examque.ExamId));
            foreach (int item in questionList)
            {
                QuestionDetails examQuestionDetails = new QuestionDetails();
                examQuestionDetails = AssessmentBL.GetExamQuestion((Convert.ToInt32(examque.ExamId)), item, Convert.ToInt32(examque.UserId));
                examQuestionDetailsList.Add(examQuestionDetails);
            }

            return examQuestionDetailsList;
        }

        public static List<QuestionDetails> SearchByQuestions(AssesmentDetailVM assDetail)
        {
            List<QuestionDetails> examQuestionDetailsList = new List<QuestionDetails>();
            List<QuestionIdWithExamId> questionList = new List<QuestionIdWithExamId>();
            questionList = ViewAssessmentDAL.SearchByQuestions(Convert.ToString(assDetail.UserId), Convert.ToString(assDetail.SearchTerm), Convert.ToString(assDetail.Filter), assDetail.NoOfRecords, assDetail.PageNo);

            foreach (QuestionIdWithExamId item in questionList)
            {
                QuestionDetails examQuestionDetails = new QuestionDetails(); //GetExamQuestion_Search GetExamQuestion_Search1
                examQuestionDetails = AssessmentBL.GetExamQuestion_Search((Convert.ToInt32(item.ExamId)), item.QuestionId, Convert.ToInt32(assDetail.UserId));
                examQuestionDetails.QuestionCount = item.QuestionIdCount;
                examQuestionDetails.serialNumber = item.QuestionNo;
                examQuestionDetailsList.Add(examQuestionDetails);
            }

            return examQuestionDetailsList;

            /*Parallel.ForEach(questionList, (item) =>
            {
                QuestionDetails examQuestionDetails = new QuestionDetails();
                examQuestionDetails = AssessmentBL.GetExamQuestion_Search((Convert.ToInt32(item.ExamId)), item.QuestionId, Convert.ToInt32(assDetail.UserId));
                examQuestionDetails.QuestionCount = item.QuestionIdCount;
                examQuestionDetails.serialNumber = item.QuestionNo;
                examQuestionDetailsList.Add(examQuestionDetails);
            });

            return examQuestionDetailsList.OrderBy(x => x.serialNumber).ToList();*/
        }

        public static QuestionDetails GetExamQuestion(int? examId, int? questionId, int? userId)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            connection.Open();

            try
            {
                Log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetExamQuestion", examId, questionId, userId);
                // Examination ExamQuestionList = new Examination();
                ExamDTO examObj = AssessmentBL.GetExamBYId(examId);
                List<Choice> choiceList = new List<Choice>();
                QuestionDetails questionDetails = new QuestionDetails();
                if (Convert.ToInt32(questionId) == 0)
                {
                    log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetFirstQuestion", examId, questionId, userId);
                    questionId = AssessmentDAL.GetFirstQuestion(examObj);
                    log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetFirstQuestion", examId, questionId, userId);
                }

                // get QuestionDetail with Next and Previous Id
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionDetails", examId, questionId, userId);
                questionDetails = AssessmentDAL.GetQuestionDetails(Convert.ToInt32(examId), Convert.ToInt32(questionId));
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionDetails", examId, questionId, userId);
                // Get Question Choice by Question Id
                // choiceList = AssessmentDAL.GetChoiceBYQuestionId(Convert.ToInt32(questionId));
                // questionDetails.choiceList = choiceList;

                // Get Right Choice for Given Question Id
                questionDetails.rightChoiceId = AssessmentDAL.GetRightChoiceByQuestionId(Convert.ToInt32(questionId), connection);

                // questionDetails.questionText = StripHTML(questionDetails.questionText);
                questionDetails.examStatus = examObj.ExamStatus;
                questionDetails.examAttemptId = examObj.ExamAttemptId;
                questionDetails.examTimeType = examObj.ExamTimeType;
                questionDetails.examMode = examObj.ExamMode;
                questionDetails.examAnswerToShow = examObj.ExamAnswerToShow;
                questionDetails.noOfQuestion = examObj.NoofQuestions;
                questionDetails.ExamName = examObj.ExamName;
                questionDetails.ExamType = examObj.ExamType;
                questionDetails.userId = examObj.UserId;
                // Get Question Disscussion on Question Id
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionDiscussion", examId, questionId, userId);
                questionDetails.questionDiscussion = AssessmentDAL.GetQuestionDiscussion(Convert.ToInt32(questionId), connection);
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionDiscussion", examId, questionId, userId);

                // Get References from Question Table on base on Question Id
                questionDetails.references = AssessmentDAL.GetReferences(Convert.ToInt32(questionId), connection);

                // Get Notes from Notes On Base on QuestionId and ExamId
                questionDetails.notes = AssessmentDAL.GetNotes(Convert.ToInt32(questionId), Convert.ToInt32(userId), Convert.ToInt32(examId), connection);

                // Get RelatedContent from RelatedCOntent On Base on QuestionId
                questionDetails.RelatedContents = AssessmentDAL.GetRelatedContent(Convert.ToInt32(questionId), connection);

                // Get IsAttemped this Question in this Exam
                questionDetails.examAttemptAnswerDTO = AssessmentDAL.GetExamAttemptAnswer(Convert.ToInt32(questionId), Convert.ToInt32(examObj.ExamAttemptId), connection);

                // GetExamAttemptText
                questionDetails.examAttemptText = AssessmentDAL.GetExamAttemptAnswerText(Convert.ToInt32(examId), Convert.ToInt32(questionId));

                // Get Mark Status
                questionDetails.markCheck = AssessmentDAL.GetMarkStatus(Convert.ToInt32(examId), Convert.ToInt32(questionId), Convert.ToInt32(userId));
                questionDetails.examId = Convert.ToInt32(examId);
                questionDetails.CurrentExamAttemptTime = DateTime.Now;

                // Get Question Choice by Question Id
                choiceList = AssessmentDAL.GetChoiceBYQuestionId(Convert.ToInt32(questionId), examId, questionDetails.examAttemptAnswerDTO.ChoiceId, questionDetails.examAttemptId, questionDetails.NotRandomizeAnswer, connection);
                questionDetails.choiceList = choiceList;

                // Get PeerDetails for this Question
                // questionDetails.PeerDetails = AssessmentDAL.GetPeerDetails(Convert.ToInt32(questionId), Convert.ToInt32(userId), questionDetails.examAttemptId);
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetPeerDetails", examId, questionId, userId);
                var peerdata = AssessmentDAL.GetPeerDetails(Convert.ToInt32(questionId), Convert.ToInt32(userId), questionDetails.examAttemptId);
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetPeerDetails", examId, questionId, userId);
                questionDetails.PeerDetails = peerdata.Where(s => s.ChoiceId != 0).OrderBy(s => s.ChoiceOption).ToList();

                // Question Count for this Question
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionCount", examId, questionId, userId);
                questionDetails.QuestionCount = AssessmentDAL.GetQuestionCount(Convert.ToInt32(examId));
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionCount", examId, questionId, userId);

                // Get BCSCExcerpt for this Question
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetBCSCExcerptText", examId, questionId, userId);
                questionDetails.BCSCExcerpt = AssessmentDAL.GetBCSCExcerptText(Convert.ToInt32(questionId));
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetBCSCExcerptText", examId, questionId, userId);

                questionDetails.ExcerptHeadings = AssessmentBL.GetExcerptHeadings(questionDetails.BCSCExcerpt);

                // Get Topic for this Question
                log.Information("start method :{0} examId:{1} questionId:{2} userId:{3}", "GetTopicByQuestion", examId, questionId, userId);
                questionDetails.TopicName = AssessmentDAL.GetTopicByQuestion(Convert.ToInt32(questionId));
                log.Information("end method :{0} examId:{1} questionId:{2} userId:{3}", "GetTopicByQuestion", examId, questionId, userId);
                connection.Close();
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetExamQuestion", examId, questionId, userId);
                return questionDetails;
            }
            catch (Exception e)
            {
                log.Error(e.Message + " method:GetExamQuestion" + " examId:" + examId + " UserId:" + userId);
                Console.Write(e.Message);
                return null;
            }
        }
    }
}
