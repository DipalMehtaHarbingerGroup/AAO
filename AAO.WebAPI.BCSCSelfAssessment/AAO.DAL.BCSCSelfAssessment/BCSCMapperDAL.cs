namespace AAO.DAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;

    public class BCSCMapperDAL
    {
        public static List<QuestionContentDTO> GetQuestions(int pageIndex, int noOfRecords)
        {
            List<QuestionContentDTO> questionUser = new List<QuestionContentDTO>();
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@PageIndex", pageIndex),
                                                new SqlParameter("@PageSize", noOfRecords),
                                                };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQestions", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    QuestionContentDTO objQuestionsBO = new QuestionContentDTO();

                    objQuestionsBO.QuestionId = (int)objSqlDataReader["QuestionId"];
                    objQuestionsBO.Stem = Convert.ToString(objSqlDataReader["Stem"]);
                    objQuestionsBO.RowNumber = Convert.ToInt32(objSqlDataReader["RowNumber"]);
                    objQuestionsBO.RecordCount = Convert.ToInt32(objSqlDataReader["RecordCount"]);
                    objQuestionsBO.FriendlyId = Convert.ToInt32(objSqlDataReader["Friendlyid"]);
                    questionUser.Add(objQuestionsBO);
                }

                objSqlDataReader.Close();
            }

            return questionUser;
        }

        public static List<QuestionContentDTO> GetQuestionContent(int? questionId)
        {
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@QuestionId", questionId),
                                                };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetProgramToTopicDetails", objSqlParameter))
            {
                List<QuestionContentDTO> questionContentList = new List<QuestionContentDTO>();
                while (objSqlDataReader.Read())
                {
                    QuestionContentDTO questionContentObj = new QuestionContentDTO();
                    questionContentObj.programId = (int)objSqlDataReader["programId"];
                    questionContentObj.program = Convert.ToString(objSqlDataReader["program"]);
                    questionContentObj.SubprogramId = (int)objSqlDataReader["SubprogramId"];
                    questionContentObj.subprogram = Convert.ToString(objSqlDataReader["subprogram"]);
                    questionContentObj.SubspecialtyId = (int)objSqlDataReader["SubspecialtyId"];
                    questionContentObj.Subspecialty = Convert.ToString(objSqlDataReader["Subspecialty"]);
                    questionContentObj.TopicId = (int)objSqlDataReader["TopicId"];
                    questionContentObj.TopicName = Convert.ToString(objSqlDataReader["TopicName"]);
                    questionContentList.Add(questionContentObj);
                }

                objSqlDataReader.Close();
                return questionContentList;
            }
        }

        public static bool GetButtonStatus(int? questionId, int? subSpecialityId, int? topicId)
        {
            bool result = false;
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@QuestionId", questionId),
                                                new SqlParameter("@SubspecialtyId", subSpecialityId),
                                                new SqlParameter("@TopicId", topicId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_Get_BCSCMappingStatus", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    result = Convert.ToBoolean(objSqlDataReader["Result"]);
                }
                objSqlDataReader.Close();
                return result;
            }
        }

        public static List<ChapterDTO> GetChapter(int? subspecialtyId)
        {
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@SubspecialtyId", subspecialtyId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetChapterTDetailsOfSection", objSqlParameter))
            {
                List<ChapterDTO> chapterList = new List<ChapterDTO>();

                while (objSqlDataReader.Read())
                {
                    ChapterDTO chapterObj = new ChapterDTO();
                    chapterObj.Chapter = Convert.ToString(objSqlDataReader["Output1"]);
                    chapterObj.ChapterName = Convert.ToString(objSqlDataReader["Output2"]);
                    chapterList.Add(chapterObj);
                }
                objSqlDataReader.Close();
                return chapterList;
            }
        }

        public static int InsertContent(TopicContentDTO addTopicContent)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@QuestionId", addTopicContent.QuestionId),
                    new SqlParameter("@SubspecialtyId", addTopicContent.SubspecialtyId),
                    new SqlParameter("@Chapter", addTopicContent.Chapter),
                    new SqlParameter("@TopicId ", addTopicContent.TopicId),
                    new SqlParameter("@Content", addTopicContent.Content),
                    new SqlParameter("@IsUpdated", true),
        };
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_InsertBCSCContent", arrSqlParameter);
            return 1;
        }

        public static List<TopicContentDTO> GetUpdateContent(int? questionId, int? subSpecialityId, int? topicId)
        {
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@QuestionId", questionId),
                                                new SqlParameter("@SubspecialtyId", subSpecialityId),
                                                new SqlParameter("@TopicId", topicId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetBCSCcontent", objSqlParameter))
            {
                List<TopicContentDTO> updateContentList = new List<TopicContentDTO>();
                while (objSqlDataReader.Read())
                {
                    TopicContentDTO updateContentObj = new TopicContentDTO();
                    updateContentObj.QuestionId = Convert.ToInt32(objSqlDataReader["QuestionId"]);
                    updateContentObj.SubspecialtyId = Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);
                    updateContentObj.ChapterName = Convert.ToString(objSqlDataReader["Chapter"]);
                    updateContentObj.Content = Convert.ToString(objSqlDataReader["Content"]);

                    DateTime? lastUpdateOn_IsUpdated = objSqlDataReader["LastUpdateOn_IsUpdated"] as DateTime?;
                    updateContentObj.LastUpdateOn_IsUpdated = lastUpdateOn_IsUpdated.HasValue ? lastUpdateOn_IsUpdated.Value.Date.ToString("MM-dd-yyyy") : "No Date";

                    DateTime? lastUpdateOn_BookUpdateAPI = objSqlDataReader["LastUpdateOn_BookUpdateAPI"] as DateTime?;
                    updateContentObj.LastUpdateOn_BookUpdateAPI = lastUpdateOn_BookUpdateAPI.HasValue ? lastUpdateOn_BookUpdateAPI.Value.Date.ToString("MM-dd-yyyy") : "No Date";

                    updateContentList.Add(updateContentObj);
                }

                objSqlDataReader.Close();
                return updateContentList;
            }
        }

        public static List<QuestionContentDTO> GetQuestionById(int? questionId)
        {
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@QuestionId", questionId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQestionByQuestionId", objSqlParameter))
            {
                List<QuestionContentDTO> questionList = new List<QuestionContentDTO>();

                while (objSqlDataReader.Read())
                {
                    QuestionContentDTO questionObj = new QuestionContentDTO();
                    questionObj.QuestionId = Convert.ToInt32(objSqlDataReader["QuestionId"]);
                    questionObj.Stem = Convert.ToString(objSqlDataReader["Stem"]);
                    questionObj.RowNumber = Convert.ToInt32(objSqlDataReader["RowNumber"]);
                    questionObj.FriendlyId = Convert.ToInt32(objSqlDataReader["Friendlyid"]);
                    questionList.Add(questionObj);
                }

                objSqlDataReader.Close();
                return questionList;
            }
        }

        public static List<QuestionContentDTO> GetQuestionByTitle(string questionText, int pageIndex, int noOfRecords, string radioButtonState)
        {
            List<QuestionContentDTO> questionsList = new List<QuestionContentDTO>();
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@QuestionText", questionText),
                                                new SqlParameter("@PageIndex", pageIndex),
                                                new SqlParameter("@PageSize", noOfRecords),
                                                new SqlParameter("@radioButtonState", radioButtonState),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQuestionByTitle", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    QuestionContentDTO questionsObj = new QuestionContentDTO();
                    questionsObj.QuestionId = (int)objSqlDataReader["QuestionId"];
                    questionsObj.Stem = Convert.ToString(objSqlDataReader["Stem"]);
                    questionsObj.RowNumber = Convert.ToInt32(objSqlDataReader["RowNumber"]);
                    questionsObj.RecordCount = Convert.ToInt32(objSqlDataReader["RecordCount"]);
                    questionsObj.FriendlyId = Convert.ToInt32(objSqlDataReader["Friendlyid"]);

                    questionsList.Add(questionsObj);
                }

                objSqlDataReader.Close();
            }

            return questionsList;
        }

        public static List<ProgramDTO> GetProgram()
        {
            List<ProgramDTO> programUser = new List<ProgramDTO>();

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetPrograms"))
            {
                while (objSqlDataReader.Read())
                {
                    ProgramDTO objProgramBO = new ProgramDTO();

                    objProgramBO.programId = (int)objSqlDataReader["ProgramId"];
                    objProgramBO.program = (string)objSqlDataReader["program"];

                    programUser.Add(objProgramBO);
                }
                objSqlDataReader.Close();
            }

            return programUser;
        }

        public static List<ProgramDTO> GetSubProgram(int? programId)
        {
            List<ProgramDTO> subProgramUser = new List<ProgramDTO>();
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@programId", programId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetSubprogram", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    ProgramDTO objSubProgramBO = new ProgramDTO();

                    objSubProgramBO.subprogramId = (int)objSqlDataReader["subprogramId"];
                    objSubProgramBO.subprogram = (string)objSqlDataReader["subprogram"];

                    subProgramUser.Add(objSubProgramBO);
                }
                objSqlDataReader.Close();
            }

            return subProgramUser;
        }

        public static List<ProgramDTO> GetSubSpeciality(int? subProgramId)
        {
            List<ProgramDTO> subSpecialityUser = new List<ProgramDTO>();
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@subprogramId", subProgramId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetSubSpeciality", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    ProgramDTO objSubSpecialityBO = new ProgramDTO();

                    objSubSpecialityBO.SubspecialtyId = (int)objSqlDataReader["SubspecialtyId"];
                    objSubSpecialityBO.Subspecialty = (string)objSqlDataReader["Subspecialty"];

                    subSpecialityUser.Add(objSubSpecialityBO);
                }
                objSqlDataReader.Close();
            }

            return subSpecialityUser;
        }

        public static List<ProgramDTO> GetTopicBySubSpec(int? subSpecialityId)
        {
            List<ProgramDTO> topicUser = new List<ProgramDTO>();
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@SubspecialtyId", subSpecialityId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetTopicBySubSpec", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    ProgramDTO objTopicBO = new ProgramDTO();

                    objTopicBO.TopicId = (int)objSqlDataReader["TopicId"];
                    objTopicBO.TopicName = (string)objSqlDataReader["TopicName"];

                    topicUser.Add(objTopicBO);
                }

                objSqlDataReader.Close();
            }

            return topicUser;
        }

        public static List<QuestionContentDTO> GetQuesByTopic(int? topicId, int? subSpecialtyId, int? programId, int? subProgramId, int pageIndex, int noOfRecords)
        {
            List<QuestionContentDTO> questionList = new List<QuestionContentDTO>();

            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@TopicId", topicId),
                                                new SqlParameter("@SubspecialtyId", subSpecialtyId),
                                                new SqlParameter("@ProgramId", programId),
                                                new SqlParameter("@subprogramId", subProgramId),
                                                new SqlParameter("@PageIndex", pageIndex),
                                                new SqlParameter("@PageSize", noOfRecords),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQuestionByTopicSubSpeciality", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    QuestionContentDTO questionObj = new QuestionContentDTO();
                    questionObj.QuestionId = (int)objSqlDataReader["QuestionId"];
                    questionObj.Stem = Convert.ToString(objSqlDataReader["Stem"]);
                    questionObj.RowNumber = Convert.ToInt32(objSqlDataReader["RowNumber"]);
                    questionObj.RecordCount = Convert.ToInt32(objSqlDataReader["RecordCount"]);
                    questionObj.FriendlyId = Convert.ToInt32(objSqlDataReader["Friendlyid"]);
                    questionList.Add(questionObj);
                }

                objSqlDataReader.Close();
                return questionList;
            }
        }

        public static List<QuestionData> GetBCSCQuestDetails(int? questionId, int? subSpecialityId)
        {
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@QuestionId", questionId),
                                                };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetBCSCQuestionDetails", objSqlParameter))
            {
                List<QuestionData> questionContentList = new List<QuestionData>();
                while (objSqlDataReader.Read())
                {
                    QuestionData questionContentObj = new QuestionData();
                    questionContentObj.ChoiceList = AssessmentDAL.GetChoiceBYQuestionIdForBCSCMapper(Convert.ToInt32(questionId));
                    questionContentObj.RightChoiceId = AssessmentDAL.GetRightChoiceByQuestionId(Convert.ToInt32(questionId));
                    questionContentObj.Discussion = Convert.ToString(objSqlDataReader["Discussion"]);
                    questionContentObj.References = Convert.ToString(objSqlDataReader["References"]);
                    questionContentObj.RelatedContents = Convert.ToString(objSqlDataReader["RelatedContents"]);
                    questionContentList.Add(questionContentObj);
                }

                objSqlDataReader.Close();
                return questionContentList;
            }
        }

        public static List<SubSpecialityDTO> GetBCSCSectionNumber(int? subSpecialityId)
        {
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@SubspecialtyId", subSpecialityId),
                                                };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_Get_BCSCPage", objSqlParameter))
            {
                List<SubSpecialityDTO> questionContentList = new List<SubSpecialityDTO>();
                while (objSqlDataReader.Read())
                {
                    SubSpecialityDTO qsubSpecObj = new SubSpecialityDTO();
                    qsubSpecObj.BCSCSectionNumber = Convert.ToInt32(objSqlDataReader["BCSCSectionNumber"]);
                    qsubSpecObj.BCSCSectionTitle = Convert.ToString(objSqlDataReader["BCSCSectionTitle"]);
                    questionContentList.Add(qsubSpecObj);
                }

                objSqlDataReader.Close();
                return questionContentList;
            }
        }

        public static List<ProgramDTO> GetSubSpecFromSpecialityId(int? subSpecialityId)
        {
            List<ProgramDTO> topicUser = new List<ProgramDTO>();
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@SubSpecialityId", subSpecialityId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetSubSpecialityFromSpecialityId", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    ProgramDTO objTopicBO = new ProgramDTO();
                    objTopicBO.SubspecialtyId = Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);
                    objTopicBO.Subspecialty = Convert.ToString(objSqlDataReader["Subspecialty"]);
                    topicUser.Add(objTopicBO);
                }

                objSqlDataReader.Close();
            }

            return topicUser;
        }

        public static List<ProgramDTO> GetTopicByTopicId(int? topicId)
        {
            List<ProgramDTO> topicUser = new List<ProgramDTO>();
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@TopicId", topicId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetTopicByTopicId", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    ProgramDTO objTopicBO = new ProgramDTO();
                    objTopicBO.TopicId = Convert.ToInt32(objSqlDataReader["TopicId"]);
                    objTopicBO.TopicName = Convert.ToString(objSqlDataReader["TopicName"]);
                    topicUser.Add(objTopicBO);
                }

                objSqlDataReader.Close();
            }

            return topicUser;
        }
    }
}
