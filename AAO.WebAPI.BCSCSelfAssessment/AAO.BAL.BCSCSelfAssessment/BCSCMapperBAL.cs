namespace AAO.BAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    public class BCSCMapperBAL
    {
        public static List<QuestionContentDTO> GetQuestions(int pageIndex, int noOfRecords)
        {
            return BCSCMapperDAL.GetQuestions(pageIndex, noOfRecords);
        }

        public static List<QuestionContentDTO> GetQuestionContent(int? questionId)
        {
            return BCSCMapperDAL.GetQuestionContent(questionId);
        }

        public static bool GetButtonStatus(int? questionId, int? subSpecialityId, int? topicId)
        {
            return BCSCMapperDAL.GetButtonStatus(questionId, subSpecialityId, topicId);
        }

        public static List<ChapterDTO> GetChapter(int? subspecialtyId)
        {
            return BCSCMapperDAL.GetChapter(subspecialtyId);
        }

        public static int InsertContent(TopicContentDTO addTopicContent)
        {
            return BCSCMapperDAL.InsertContent(addTopicContent);
        }

        public static List<TopicContentDTO> GetUpdateContent(int? questionId, int? subSpecialityId, int? topicId)
        {
            return BCSCMapperDAL.GetUpdateContent(questionId, subSpecialityId, topicId);
        }

        public static List<QuestionContentDTO> GetQuestionById(int? questionId)
        {
            return BCSCMapperDAL.GetQuestionById(questionId);
        }

        public static List<QuestionContentDTO> GetQuestionByTitle(string questionText, int pageIndex, int noOfRecords, string radioButtonState)
        {
            return BCSCMapperDAL.GetQuestionByTitle(questionText, pageIndex, noOfRecords, radioButtonState);
        }

        public static List<ProgramDTO> GetProgram()
        {
           return BCSCMapperDAL.GetProgram();
        }

        public static List<ProgramDTO> GetSubProgram(int? programId)
        {
            return BCSCMapperDAL.GetSubProgram(programId);
        }

        public static List<ProgramDTO> GetSubSpeciality(int? subprogramId)
        {
            return BCSCMapperDAL.GetSubSpeciality(subprogramId);
        }

        public static List<ProgramDTO> GetTopicBySubSpec(int? subSpecialtyId)
        {
            return BCSCMapperDAL.GetTopicBySubSpec(subSpecialtyId);
        }

        public static List<QuestionContentDTO> GetQuesByTopic(int? topicId, int? subSpecialtyId, int? programId, int? subProgramId, int pageIndex, int noOfRecords)
        {
            return BCSCMapperDAL.GetQuesByTopic(topicId, subSpecialtyId, programId, subProgramId, pageIndex, noOfRecords);
        }

        public static List<QuestionData> GetBCSCQuestDetails(int? questionId, int? subSpecialtyId)
        {
            List<QuestionData> questionData = new List<QuestionData>();
            return BCSCMapperDAL.GetBCSCQuestDetails(questionId, subSpecialtyId);
        }

        public static List<SubSpecialityDTO> GetBCSCSectionNumber(int? subSpecialtyId)
        {
            return BCSCMapperDAL.GetBCSCSectionNumber(subSpecialtyId);
        }

        public static List<ProgramDTO> GetSubSpecFromSpecialityId(int? subSpecialtyId)
        {
            return BCSCMapperDAL.GetSubSpecFromSpecialityId(subSpecialtyId);
        }

        public static List<ProgramDTO> GetTopicByTopicId(int? topicId)
        {
            return BCSCMapperDAL.GetTopicByTopicId(topicId);
        }
    }
}
