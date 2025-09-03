namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    public class BCSCMapperController : ApiController
    {
        [Route("api/BCSCMapper/GetQuestions")]
        [HttpPost]
        public List<QuestionContentDTO> GetQuestions(QuestionContentDTO question)
        {
            List<QuestionContentDTO> sectChaptTop = new List<QuestionContentDTO>();
            return BCSCMapperBAL.GetQuestions(Convert.ToInt32(question.PageIndex), Convert.ToInt32(question.NoOfRecords));
        }

        [Route("api/BCSCMapper/GetQuestionContent")]
        [HttpPost]
        public List<QuestionContentDTO> GetQuestionContent(QuestionContentDTO question)
        {
            List<QuestionContentDTO> questionContent = new List<QuestionContentDTO>();
            return BCSCMapperBAL.GetQuestionContent(Convert.ToInt32(question.QuestionId));
        }

        [Route("api/BCSCMapper/GetButtonStatus")]
        [HttpPost]
        public bool GetButtonStatus(QuestionContentDTO question)
        {
            return BCSCMapperBAL.GetButtonStatus(Convert.ToInt32(question.QuestionId), Convert.ToInt32(question.SubspecialtyId), Convert.ToInt32(question.TopicId));
        }

        [Route("api/BCSCMapper/GetChapter")]
        [HttpPost]
        public List<ChapterDTO> GetChapter(SubSpecialityDTO speciality)
        {
            return BCSCMapperBAL.GetChapter(Convert.ToInt32(speciality.SubspecialtyId));
        }

        [Route("api/BCSCMapper/InsertContent")]
        [HttpPost]
        public int InsertContent(TopicContentDTO addTopicContent)
        {
            return BCSCMapperBAL.InsertContent(addTopicContent);
        }

        [Route("api/BCSCMapper/GetUpdateContent")]
        [HttpPost]
        public List<TopicContentDTO> GetUpdateContent(QuestionContentDTO question)
        {
            return BCSCMapperBAL.GetUpdateContent(Convert.ToInt32(question.QuestionId), Convert.ToInt32(question.SubspecialtyId), Convert.ToInt32(question.TopicId));
        }

        [Route("api/BCSCMapper/GetQuestionById")]
        [HttpPost]
        public List<QuestionContentDTO> GetQuestionById(QuestionContentDTO question)
        {
            List<QuestionContentDTO> questionContent = new List<QuestionContentDTO>();
            return BCSCMapperBAL.GetQuestionById(Convert.ToInt32(question.QuestionId));
        }

        [Route("api/BCSCMapper/GetQuestionByTitle")]
        [HttpPost]
        public List<QuestionContentDTO> GetQuestionByTitle(QuestionContentDTO questionText)
        {
            List<QuestionContentDTO> questionContent = new List<QuestionContentDTO>();
            return BCSCMapperBAL.GetQuestionByTitle(Convert.ToString(questionText.Stem), Convert.ToInt32(questionText.PageIndex), Convert.ToInt32(questionText.NoOfRecords), Convert.ToString(questionText.radioButtonState));
        }

        [Route("api/BCSCMapper/GetProgram")]
        [HttpGet]
        public List<ProgramDTO> GetProgram()
        {
            return BCSCMapperBAL.GetProgram();
        }

        [Route("api/BCSCMapper/GetSubProgram")]
        [HttpPost]
        public List<ProgramDTO> GetSubProgram(ProgramDTO program)
        {
            return BCSCMapperBAL.GetSubProgram(Convert.ToInt32(program.programId));
        }

        [Route("api/BCSCMapper/GetSubSpeciality")]
        [HttpPost]
        public List<ProgramDTO> GetSubSpeciality(ProgramDTO subProgram)
        {
            return BCSCMapperBAL.GetSubSpeciality(Convert.ToInt32(subProgram.subprogramId));
        }

        [Route("api/BCSCMapper/GetTopicBySubSpec")]
        [HttpPost]
        public List<ProgramDTO> GetTopicBySubSpec(ProgramDTO program)
        {
            return BCSCMapperBAL.GetTopicBySubSpec(Convert.ToInt32(program.SubspecialtyId));
        }

        [Route("api/BCSCMapper/GetQuesByTopic")]
        [HttpPost]
        public List<QuestionContentDTO> GetQuesByTopic(QuestionContentDTO question)
        {
            return BCSCMapperBAL.GetQuesByTopic(Convert.ToInt32(question.TopicId), Convert.ToInt32(question.SubspecialtyId), Convert.ToInt32(question.programId), Convert.ToInt32(question.SubprogramId), Convert.ToInt32(question.PageIndex), Convert.ToInt32(question.NoOfRecords));
        }

        [Route("api/BCSCMapper/GetBCSCQuestDetails")]
        [HttpPost]
        public List<QuestionData> GetBCSCQuestDetails(QuestionData question)
        {
            return BCSCMapperBAL.GetBCSCQuestDetails(Convert.ToInt32(question.QuestionId), Convert.ToInt32(question.SubspecialtyId));
        }

        [Route("api/BCSCMapper/GetBCSCSectionNumber")]
        [HttpPost]
        public List<SubSpecialityDTO> GetBCSCSectionNumber(SubSpecialityDTO subSpec)
        {
            return BCSCMapperBAL.GetBCSCSectionNumber(Convert.ToInt32(subSpec.SubspecialtyId));
        }

        [Route("api/BCSCMapper/GetSubSpecFromSpecialityId")]
        [HttpPost]
        public List<ProgramDTO> GetSubSpecFromSpecialityId(ProgramDTO specialityId)
        {
            return BCSCMapperBAL.GetSubSpecFromSpecialityId(Convert.ToInt32(specialityId.SubspecialtyId));
        }

        [Route("api/BCSCMapper/GetTopicByTopicId")]
        [HttpPost]
        public List<ProgramDTO> GetTopicByTopicId(ProgramDTO topicId)
        {
            return BCSCMapperBAL.GetTopicByTopicId(Convert.ToInt32(topicId.TopicId));
        }
    }
}
