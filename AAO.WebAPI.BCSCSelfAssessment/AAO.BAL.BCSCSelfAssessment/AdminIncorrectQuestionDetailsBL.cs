namespace AAO.BAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    public class AdminIncorrectQuestionDetailsBL
    {
        public static List<QuestionDetails> AdminIncorrectQuestionDetails(AdminReportVM incorrectReportDetails)
        {
            List<QuestionDetails> reportQuestionDetailsList = new List<QuestionDetails>();
            List<IncorrectQuestionDetailsDTO> questionList = new List<IncorrectQuestionDetailsDTO>();
            questionList = AdminIncorrectQuestionDetailsDAL.AdminIncorrectQuestionDetails(incorrectReportDetails.Year, incorrectReportDetails.UserId, incorrectReportDetails.SubspecialtyId, incorrectReportDetails.ExamStartDate, incorrectReportDetails.ExamCompletedDate, incorrectReportDetails.NoOfRecords, incorrectReportDetails.PageNo);
            foreach (IncorrectQuestionDetailsDTO item in questionList)
            {
                QuestionDetails reportQuestionDetails = new QuestionDetails();

                if (item.ExamType == 3)
                {
                    reportQuestionDetails = GetAdminQuestion_SR(item.ExamId, item.QuestionId, item.UserId);
                }
                else
                {
                    reportQuestionDetails = GetAdminQuestion(item.ExamId, item.QuestionId, item.UserId);
                }

                reportQuestionDetails.QuestionCount = item.QuestionIdCount;
                reportQuestionDetails.serialNumber = item.Rownumber;
                reportQuestionDetails.SubSpeciality = item.Subspecialty;
                reportQuestionDetails.Section = item.Section;
                reportQuestionDetailsList.Add(reportQuestionDetails);
            }

            return reportQuestionDetailsList;
        }

        public static QuestionDetails GetAdminQuestion(int? examId, int? questionId, int? userId)
        {
            try
            {
                // Examination ExamQuestionList = new Examination();
                ExamDTO examObj = new ExamDTO();
                List<Choice> choiceList = new List<Choice>();
                QuestionDetails questionDetails = new QuestionDetails();
                if (Convert.ToInt32(questionId) == 0)
                {
                    questionId = AdminIncorrectQuestionDetailsDAL.GetAdminFirstQuestion(Convert.ToInt32(examId));
                }

                // get QuestionDetail with Next and Previous Id
                questionDetails = AdminIncorrectQuestionDetailsDAL.GetAdminQuestionDetails(Convert.ToInt32(examId), Convert.ToInt32(questionId));

                // Get Right Choice for Given Question Id
                questionDetails.rightChoiceId = AssessmentDAL.GetRightChoiceByQuestionId(Convert.ToInt32(questionId));

                // Get Created Exam Object for Exam Status and Exam AttemptId
                examObj = GetAdminExamBYId(examId);

                questionDetails.examStatus = examObj.ExamStatus;
                questionDetails.examAttemptId = examObj.ExamAttemptId;
                questionDetails.examTimeType = examObj.ExamTimeType;
                questionDetails.examMode = examObj.ExamMode;
                questionDetails.examAnswerToShow = examObj.ExamAnswerToShow;
                questionDetails.noOfQuestion = examObj.NoofQuestions;
                questionDetails.ExamName = examObj.ExamName;
                questionDetails.ExamType = examObj.ExamType;

                // Get Question Disscussion on Question Id
                questionDetails.questionDiscussion = AssessmentDAL.GetQuestionDiscussion(Convert.ToInt32(questionId));

                // Get References from Question Table on base on Question Id
                questionDetails.references = AdminIncorrectQuestionDetailsDAL.GetAdminReferences(Convert.ToInt32(questionId));

                // Get References from Question Table on base on Question Id
                questionDetails.RelatedContents = AdminIncorrectQuestionDetailsDAL.GetAdminRelatedContent(Convert.ToInt32(questionId));

                // Get Notes from Notes On Base on QuestionId and ExamId
                questionDetails.notes = AdminIncorrectQuestionDetailsDAL.GetAdminNotes(Convert.ToInt32(questionId), Convert.ToInt32(userId));

                // Get IsAttemped this Question in this Exam
                questionDetails.examAttemptAnswerDTO = AssessmentDAL.GetExamAttemptAnswer(Convert.ToInt32(questionId), Convert.ToInt32(examObj.ExamAttemptId));

                // GetExamAttemptText
                questionDetails.examAttemptText = AdminIncorrectQuestionDetailsDAL.GetAdminExamAttemptAnswerText(Convert.ToInt32(examId), Convert.ToInt32(questionId));

                // Get Mark Status
                questionDetails.markCheck = AdminIncorrectQuestionDetailsDAL.GetAdminMarkStatus(Convert.ToInt32(examId), Convert.ToInt32(questionId));
                questionDetails.examId = Convert.ToInt32(examId);
                questionDetails.CurrentExamAttemptTime = DateTime.Now;

                // Get Question Choice by Question Id
                choiceList = AssessmentDAL.GetChoiceBYQuestionId(Convert.ToInt32(questionId), examId, questionDetails.examAttemptAnswerDTO.ChoiceId, questionDetails.examAttemptId, questionDetails.NotRandomizeAnswer);
                questionDetails.choiceList = choiceList;

                // Get PeerDetails for this Question
                questionDetails.PeerDetails = AdminIncorrectQuestionDetailsDAL.GetAdminPeerDetails(Convert.ToInt32(questionId), Convert.ToInt32(userId));

                // Question Count for this Question
                questionDetails.QuestionCount = AdminIncorrectQuestionDetailsDAL.GetAdminQuestionCount(Convert.ToInt32(examId));

                // Get BCSCExcerpt for this Question
                questionDetails.BCSCExcerpt = AssessmentDAL.GetBCSCExcerptText(Convert.ToInt32(questionId));

                questionDetails.ExcerptHeadings = AssessmentBL.GetExcerptHeadings(questionDetails.BCSCExcerpt);
                // Get Topic for this Question
                questionDetails.TopicName = AssessmentDAL.GetTopicByQuestion(Convert.ToInt32(questionId));
                return questionDetails;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        public static QuestionDetails GetAdminQuestion_SR(int? examId, int? questionId, int? userId)
        {
            try
            {
                ExamDTO examObj = new ExamDTO();
                List<Choice> choiceList = new List<Choice>();
                QuestionDetails questionDetails = new QuestionDetails();
                if (Convert.ToInt32(questionId) == 0)
                {
                    questionId = AdminIncorrectQuestionDetailsDAL.GetAdminFirstQuestion(Convert.ToInt32(examId));
                }

                // get QuestionDetail with Next and Previous Id
                //questionDetails = AdminIncorrectQuestionDetailsDAL.GetAdminQuestionDetails(Convert.ToInt32(examId), Convert.ToInt32(questionId));

                // get QuestionDetail with Next and Previous Id
                questionDetails = AdminIncorrectQuestionDetailsDAL.GetAdminQuestionDetails_SR(Convert.ToInt32(examId), Convert.ToInt32(questionId));

                // Get Right Choice for Given Question Id
                questionDetails.rightChoiceId = AssessmentDAL.GetRightChoiceByQuestionId(Convert.ToInt32(questionId));

                // Get Created Exam Object for Exam Status and Exam AttemptId
                examObj = GetAdminExamBYId(examId);

                questionDetails.examStatus = examObj.ExamStatus;
                questionDetails.examAttemptId = examObj.ExamAttemptId;
                questionDetails.examTimeType = examObj.ExamTimeType;
                questionDetails.examMode = examObj.ExamMode;
                questionDetails.examAnswerToShow = examObj.ExamAnswerToShow;
                questionDetails.noOfQuestion = examObj.NoofQuestions;
                questionDetails.ExamName = examObj.ExamName;
                questionDetails.ExamType = examObj.ExamType;

                // Get Question Disscussion on Question Id
                questionDetails.questionDiscussion = AssessmentDAL.GetQuestionDiscussion(Convert.ToInt32(questionId));

                // Get References from Question Table on base on Question Id
                questionDetails.references = AdminIncorrectQuestionDetailsDAL.GetAdminReferences(Convert.ToInt32(questionId));

                // Get References from Question Table on base on Question Id
                questionDetails.RelatedContents = AdminIncorrectQuestionDetailsDAL.GetAdminRelatedContent(Convert.ToInt32(questionId));

                // Get Notes from Notes On Base on QuestionId and ExamId
                questionDetails.notes = AdminIncorrectQuestionDetailsDAL.GetAdminNotes(Convert.ToInt32(questionId), Convert.ToInt32(userId));

                // Get IsAttemped this Question in this Exam
                questionDetails.examAttemptAnswerDTO = AssessmentDAL.GetExamAttemptAnswer(Convert.ToInt32(questionId), Convert.ToInt32(examObj.ExamAttemptId));

                // GetExamAttemptText
                questionDetails.examAttemptText = AdminIncorrectQuestionDetailsDAL.GetAdminExamAttemptAnswerText(Convert.ToInt32(examId), Convert.ToInt32(questionId));

                // Get Mark Status
                questionDetails.markCheck = AdminIncorrectQuestionDetailsDAL.GetAdminMarkStatus(Convert.ToInt32(examId), Convert.ToInt32(questionId));
                questionDetails.examId = Convert.ToInt32(examId);
                questionDetails.CurrentExamAttemptTime = DateTime.Now;

                // Get Question Choice by Question Id
                choiceList = AssessmentDAL.GetChoiceBYQuestionId(Convert.ToInt32(questionId), examId, questionDetails.examAttemptAnswerDTO.ChoiceId, questionDetails.examAttemptId, questionDetails.NotRandomizeAnswer);
                questionDetails.choiceList = choiceList;

                // Get PeerDetails for this Question
                questionDetails.PeerDetails = AdminIncorrectQuestionDetailsDAL.GetAdminPeerDetails(Convert.ToInt32(questionId), Convert.ToInt32(userId));

                // Question Count for this Question
                questionDetails.QuestionCount = AdminIncorrectQuestionDetailsDAL.GetAdminQuestionCount(Convert.ToInt32(examId));

                // Get BCSCExcerpt for this Question
                questionDetails.BCSCExcerpt = AssessmentDAL.GetBCSCExcerptText(Convert.ToInt32(questionId));

                questionDetails.ExcerptHeadings = AssessmentBL.GetExcerptHeadings(questionDetails.BCSCExcerpt);
                // Get Topic for this Question
                questionDetails.TopicName = AssessmentDAL.GetTopicByQuestion(Convert.ToInt32(questionId));
                return questionDetails;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        public static ExamDTO GetAdminExamBYId(int? examId)
        {
            return AdminIncorrectQuestionDetailsDAL.GetAdminExamBYId(examId);
        }
    }
}
