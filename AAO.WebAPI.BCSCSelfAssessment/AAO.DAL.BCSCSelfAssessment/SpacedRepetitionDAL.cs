namespace AAO.DAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;

    public class SpacedRepetitionDAL
    {
        // This method will call SpacedRepetition Exam
        public static ExamDTO GetSpacedRepetionExam(int userId, string typeofCategory)
        {
            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@UserID", Convert.ToInt32(userId)),
                new SqlParameter("@TypeofCategory", Convert.ToString(typeofCategory)), // Added for HG-459
            };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetSpacedRepetitionExam", objSqlParameter))
            {
                ExamDTO examObj = new ExamDTO();
                while (objSqlDataReader.Read())
                {
                    examObj.ExamId = Convert.ToInt32(objSqlDataReader["ExamId"]);
                    examObj.ExamName = Convert.ToString(objSqlDataReader["ExamName"]);
                    examObj.ExamCreateDate = Convert.ToDateTime(objSqlDataReader["ExamCreateDate"]);
                    examObj.IsDeleted = (bool)(objSqlDataReader["IsDeleted"]);
                    examObj.ExamType = Convert.ToInt32(objSqlDataReader["ExamType"]);
                    examObj.NoofQuestions = Convert.ToInt32(objSqlDataReader["NoofQuestions"]);
                    examObj.ExamTimeType = (bool)(objSqlDataReader["ExamTimeType"]);
                    examObj.ExamMode = (bool)(objSqlDataReader["ExamMode"]);
                    examObj.ExamAnswerToShow = (bool)(objSqlDataReader["ExamAnswerToShow"]);
                    examObj.TypeofQuestion = Convert.ToString(objSqlDataReader["TypeofQuestion"]);
                    examObj.TypeofCategory = Convert.ToString(objSqlDataReader["TypeofCategory"]);
                    examObj.UserId = Convert.ToInt32(objSqlDataReader["UserId"]);
                    examObj.RoundNumber = Convert.ToInt32(objSqlDataReader["RoundNumber"]);
                    examObj.SessionId = Convert.ToInt32(objSqlDataReader["SessionId"]);
                }

                return examObj;
            }
        }

        // This Method Will Get ExamId based on UserId for Spaced Repetition
        public static int GetExamIdBYUserIdSR(int userId)
        {
            int examId = 0;
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string sqlQueryAllquestion = string.Empty;
            sqlQueryAllquestion = "Select ExamId As ID FROM Exam WHERE UserId= " + @userId + "AND ExamType = 3";
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlQueryAllquestion, connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        object objExamId = reader["ID"];
                        examId = objExamId is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
                    }
                }
            }
            connection.Close();
            return examId;
        }

        // This Method Will Get Exam Status Count On Spaced Repetition Landing Page
        public static ExamStatusCountSRVM GetExamStatusCountSR(int examId)
        {
            ExamStatusCountSRVM examStatusCountSRVM = new ExamStatusCountSRVM();
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                     };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetSpacedRepetitionExamCounters", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object totalansweredQuestionobj = objSqlDataReader["TotalAnsweredQuestions"];
                        examStatusCountSRVM.TotalAnsweredQuestions = totalansweredQuestionobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["TotalAnsweredQuestions"]);
                        object topicCountobj = objSqlDataReader["TopicCount"];
                        examStatusCountSRVM.TopicCount = topicCountobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["TopicCount"]);
                        object answeredCorrectlyObj = objSqlDataReader["AnsweredCorrectly"];
                        examStatusCountSRVM.AnsweredCorrectly = answeredCorrectlyObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["AnsweredCorrectly"]);
                        object answeredInCorrectlyObj = objSqlDataReader["AnsweredInCorrectly"];
                        examStatusCountSRVM.AnsweredInCorrectly = answeredInCorrectlyObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["AnsweredInCorrectly"]);
                    }
                }
            }

            return examStatusCountSRVM;
        }

        // This Method Will Get Section Data On SR Landing Page Performance Overview
        public static List<SRPerformanceOverviewVM> GetSectionPerformanceOverviewSR(int examId)
        {
            List<SRPerformanceOverviewVM> sRPerformanceOverviewVMList = new List<SRPerformanceOverviewVM>();
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@ExamId", Convert.ToInt32(examId)),
            };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_SpacedRepetitionPerformanceOverview", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        SRPerformanceOverviewVM sRPerformanceOverviewVM = new SRPerformanceOverviewVM();
                        object subspecialityIdobj = objSqlDataReader["SubspecialtyId"];
                        sRPerformanceOverviewVM.SubspecialtyId = subspecialityIdobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);
                        object sectionTitleobj = objSqlDataReader["SectionTitle"];
                        sRPerformanceOverviewVM.SectionTitle = sectionTitleobj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["SectionTitle"]);
                        object sectionNumberobj = objSqlDataReader["BCSCSectionNumber"];
                        sRPerformanceOverviewVM.SectionNumber = sectionNumberobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["BCSCSectionNumber"]);
                        object correctConfidentobj = objSqlDataReader["CorrectConfident"];
                        sRPerformanceOverviewVM.CorrectConfident = correctConfidentobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["CorrectConfident"]);
                        object correctNotconfidentObj = objSqlDataReader["CorrectNotconfident"];
                        sRPerformanceOverviewVM.CorrectNotconfident = correctNotconfidentObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["CorrectNotconfident"]);
                        object incorrectConfidentObj = objSqlDataReader["IncorrectConfident"];
                        sRPerformanceOverviewVM.IncorrectConfident = incorrectConfidentObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["IncorrectConfident"]);
                        object incorrectNotconfidentobj = objSqlDataReader["IncorrectNotconfident"];
                        sRPerformanceOverviewVM.IncorrectNotconfident = incorrectNotconfidentobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["IncorrectNotconfident"]);
                        object questionCountObj = objSqlDataReader["QuestionCount"];
                        sRPerformanceOverviewVM.QuestionCount = questionCountObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionCount"]);
                        sRPerformanceOverviewVM.CorrectConfidentPercent = ((Convert.ToDouble(correctConfidentobj) * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewVM.CorrectConfidentPercent = Math.Round(sRPerformanceOverviewVM.CorrectConfidentPercent, 2);
                        sRPerformanceOverviewVM.CorrectNotconfidentPercent = ((Convert.ToDouble(correctNotconfidentObj) * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewVM.CorrectNotconfidentPercent = Math.Round(sRPerformanceOverviewVM.CorrectNotconfidentPercent, 2);
                        sRPerformanceOverviewVM.IncorrectConfidentPercent = ((Convert.ToDouble(incorrectConfidentObj) * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewVM.IncorrectConfidentPercent = Math.Round(sRPerformanceOverviewVM.IncorrectConfidentPercent, 2);
                        sRPerformanceOverviewVM.IncorrectNotconfidentPercent = ((Convert.ToDouble(incorrectNotconfidentobj) * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewVM.IncorrectNotconfidentPercent = Math.Round(sRPerformanceOverviewVM.IncorrectNotconfidentPercent, 2);
                        sRPerformanceOverviewVM.UnAnswered = (Convert.ToInt32(questionCountObj) - ((Convert.ToInt32(correctConfidentobj) + Convert.ToInt32(correctNotconfidentObj) + Convert.ToInt32(incorrectConfidentObj) + Convert.ToInt32(incorrectNotconfidentobj))));
                        sRPerformanceOverviewVM.UnAnsweredPercent = (Convert.ToDouble(sRPerformanceOverviewVM.UnAnswered * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewVM.UnAnsweredPercent = Math.Round(sRPerformanceOverviewVM.UnAnsweredPercent, 2) - 0.1;
                        sRPerformanceOverviewVMList.Add(sRPerformanceOverviewVM);
                    }
                }
            }

            return sRPerformanceOverviewVMList;
        }

        // This Method Will Get Chapter Data On SR Landing Page Performance Overview  
        public static List<SRPerformanceOverviewChapterVM> GetChapterPerformanceOverviewSR(int examId, int subspecialityId)
        {
            List<SRPerformanceOverviewChapterVM> sRPerformanceOverviewChapterVMList = new List<SRPerformanceOverviewChapterVM>();
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                     new SqlParameter("@SubspecialityId", Convert.ToInt32(subspecialityId)),
            };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_SpacedRepetitionPerformanceOverviewChapter", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        SRPerformanceOverviewChapterVM sRPerformanceOverviewChapterVM = new SRPerformanceOverviewChapterVM();
                        object subspecialityIdobj = objSqlDataReader["SubspecialtyId"];
                        sRPerformanceOverviewChapterVM.SubspecialtyId = subspecialityIdobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);
                        object sectionNumberobj = objSqlDataReader["ChapterNumber"];
                        sRPerformanceOverviewChapterVM.ChapterNumber = subspecialityIdobj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ChapterNumber"]);
                        object sectionTitleobj = objSqlDataReader["ChapterName"];
                        sRPerformanceOverviewChapterVM.ChapterName = sectionTitleobj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ChapterName"]);
                        object correctConfidentobj = objSqlDataReader["CorrectConfident"];
                        sRPerformanceOverviewChapterVM.CorrectConfident = correctConfidentobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["CorrectConfident"]);
                        object correctNotconfidentObj = objSqlDataReader["CorrectNotconfident"];
                        sRPerformanceOverviewChapterVM.CorrectNotconfident = correctNotconfidentObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["CorrectNotconfident"]);
                        object incorrectConfidentObj = objSqlDataReader["IncorrectConfident"];
                        sRPerformanceOverviewChapterVM.IncorrectConfident = incorrectConfidentObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["IncorrectConfident"]);
                        object incorrectNotconfidentobj = objSqlDataReader["IncorrectNotconfident"];
                        sRPerformanceOverviewChapterVM.IncorrectNotconfident = incorrectNotconfidentobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["IncorrectNotconfident"]);
                        object questionCountObj = objSqlDataReader["QuestionCount"];
                        sRPerformanceOverviewChapterVM.QuestionCount = questionCountObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionCount"]);
                        sRPerformanceOverviewChapterVM.CorrectConfidentPercent = ((Convert.ToDouble(correctConfidentobj) * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewChapterVM.CorrectConfidentPercent = Math.Round(sRPerformanceOverviewChapterVM.CorrectConfidentPercent, 2);
                        sRPerformanceOverviewChapterVM.CorrectNotconfidentPercent = ((Convert.ToDouble(correctNotconfidentObj) * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewChapterVM.CorrectNotconfidentPercent = Math.Round(sRPerformanceOverviewChapterVM.CorrectNotconfidentPercent, 2);
                        sRPerformanceOverviewChapterVM.IncorrectConfidentPercent = ((Convert.ToDouble(incorrectConfidentObj) * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewChapterVM.IncorrectConfidentPercent = Math.Round(sRPerformanceOverviewChapterVM.IncorrectConfidentPercent, 2);
                        sRPerformanceOverviewChapterVM.IncorrectNotconfidentPercent = ((Convert.ToDouble(incorrectNotconfidentobj) * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewChapterVM.IncorrectNotconfidentPercent = Math.Round(sRPerformanceOverviewChapterVM.IncorrectNotconfidentPercent, 2);
                        sRPerformanceOverviewChapterVM.UnAnswered = (Convert.ToInt32(questionCountObj) - ((Convert.ToInt32(correctConfidentobj) + Convert.ToInt32(correctNotconfidentObj) + Convert.ToInt32(incorrectConfidentObj) + Convert.ToInt32(incorrectNotconfidentobj))));
                        sRPerformanceOverviewChapterVM.UnAnsweredPercent = (Convert.ToDouble(sRPerformanceOverviewChapterVM.UnAnswered * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewChapterVM.UnAnsweredPercent = Math.Round(sRPerformanceOverviewChapterVM.UnAnsweredPercent, 2) - 0.1;
                        sRPerformanceOverviewChapterVMList.Add(sRPerformanceOverviewChapterVM);
                    }
                }
            }

            return sRPerformanceOverviewChapterVMList;
        }

        // This Method Will Get Topics Data On SR Landing Page Performance Overview  
        public static List<SRPerformanceOverviewTopicsVM> GetTopicsPerformanceOverviewSR(int examId, string chapterNumber, int subspecialityId, string ChapterName)
        {
            List<SRPerformanceOverviewTopicsVM> sRPerformanceOverviewTopicsVMList = new List<SRPerformanceOverviewTopicsVM>();
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@ExamID", Convert.ToInt32(examId)),
                     new SqlParameter("@ChapterNumber", Convert.ToString(chapterNumber)),
                     new SqlParameter("@SubspecialityId", Convert.ToInt32(subspecialityId)),
                     new SqlParameter("@ChapterName",Convert.ToString(ChapterName)),
            };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_SpacedRepetitionPerformanceOverviewTopic", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        SRPerformanceOverviewTopicsVM sRPerformanceOverviewTopicsVM = new SRPerformanceOverviewTopicsVM();
                        object subspecialityIdobj = objSqlDataReader["SubspecialtyId"];
                        sRPerformanceOverviewTopicsVM.SubspecialtyId = subspecialityIdobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);
                        object topicNameobj = objSqlDataReader["TopicName"];
                        sRPerformanceOverviewTopicsVM.TopicName = topicNameobj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["TopicName"]);
                        object topicIdobj = objSqlDataReader["TopicId"];
                        sRPerformanceOverviewTopicsVM.TopicId = topicIdobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["TopicId"]);
                        object chapterNumberobj = objSqlDataReader["ChapterNumber"];
                        sRPerformanceOverviewTopicsVM.ChapterNumber = chapterNumberobj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ChapterNumber"]);
                        object chapterNameobj = objSqlDataReader["ChapterName"];
                        sRPerformanceOverviewTopicsVM.ChapterName = chapterNameobj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ChapterName"]);
                        object correctConfidentobj = objSqlDataReader["CorrectConfident"];
                        sRPerformanceOverviewTopicsVM.CorrectConfident = correctConfidentobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["CorrectConfident"]);
                        object correctNotconfidentObj = objSqlDataReader["CorrectNotconfident"];
                        sRPerformanceOverviewTopicsVM.CorrectNotconfident = correctNotconfidentObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["CorrectNotconfident"]);
                        object incorrectConfidentObj = objSqlDataReader["IncorrectConfident"];
                        sRPerformanceOverviewTopicsVM.IncorrectConfident = incorrectConfidentObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["IncorrectConfident"]);
                        object incorrectNotconfidentobj = objSqlDataReader["IncorrectNotconfident"];
                        sRPerformanceOverviewTopicsVM.IncorrectNotconfident = incorrectNotconfidentobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["IncorrectNotconfident"]);
                        object questionCountObj = objSqlDataReader["QuestionCount"];
                        sRPerformanceOverviewTopicsVM.QuestionCount = questionCountObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionCount"]);
                        sRPerformanceOverviewTopicsVM.CorrectConfidentPercent = ((Convert.ToDouble(correctConfidentobj) * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewTopicsVM.CorrectConfidentPercent = Math.Round(sRPerformanceOverviewTopicsVM.CorrectConfidentPercent, 2);
                        sRPerformanceOverviewTopicsVM.CorrectNotconfidentPercent = ((Convert.ToDouble(correctNotconfidentObj) * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewTopicsVM.CorrectNotconfidentPercent = Math.Round(sRPerformanceOverviewTopicsVM.CorrectNotconfidentPercent, 2);
                        sRPerformanceOverviewTopicsVM.IncorrectConfidentPercent = ((Convert.ToDouble(incorrectConfidentObj) * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewTopicsVM.IncorrectConfidentPercent = Math.Round(sRPerformanceOverviewTopicsVM.IncorrectConfidentPercent, 2);
                        sRPerformanceOverviewTopicsVM.IncorrectNotconfidentPercent = ((Convert.ToDouble(incorrectNotconfidentobj) * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewTopicsVM.IncorrectNotconfidentPercent = Math.Round(sRPerformanceOverviewTopicsVM.IncorrectNotconfidentPercent, 2);
                        sRPerformanceOverviewTopicsVM.UnAnswered = (Convert.ToInt32(questionCountObj) - ((Convert.ToInt32(correctConfidentobj) + Convert.ToInt32(correctNotconfidentObj) + Convert.ToInt32(incorrectConfidentObj) + Convert.ToInt32(incorrectNotconfidentobj))));
                        sRPerformanceOverviewTopicsVM.UnAnsweredPercent = (Convert.ToDouble(sRPerformanceOverviewTopicsVM.UnAnswered * 100) / Convert.ToDouble(questionCountObj));
                        sRPerformanceOverviewTopicsVM.UnAnsweredPercent = Math.Round(sRPerformanceOverviewTopicsVM.UnAnsweredPercent, 2) - 0.1;
                        sRPerformanceOverviewTopicsVMList.Add(sRPerformanceOverviewTopicsVM);
                    }
                }
            }

            return sRPerformanceOverviewTopicsVMList;
        }

        // This Method Will Get Recommended Topics On SR Landing Page 
        public static List<SRRecommendedTopicsVM> GetRecommendedTopicsSR(int examId, int UserId)
        {
            List<SRRecommendedTopicsVM> sRRecommendedTopicsVMList = new List<SRRecommendedTopicsVM>();
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                    new SqlParameter("@UserId", Convert.ToInt32(UserId)),
            };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_SpacedRepetitionRecommendedTopics", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        SRRecommendedTopicsVM sRRecommendedTopicsVM = new SRRecommendedTopicsVM();
                        object sectionNameobj = objSqlDataReader["SectionName"];
                        sRRecommendedTopicsVM.SectionName = sectionNameobj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["SectionName"]);
                        object chapterNameobj = objSqlDataReader["ChapterName"];
                        sRRecommendedTopicsVM.ChapterName = chapterNameobj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ChapterName"]);
                        object topicNameobj = objSqlDataReader["TopicName"];
                        sRRecommendedTopicsVM.TopicName = topicNameobj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["TopicName"]);
                        object excerptobj = objSqlDataReader["Excerpt"];
                        sRRecommendedTopicsVM.Excerpt = excerptobj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Excerpt"]);
                        object sectionNumberobj = objSqlDataReader["SectionNumber"];
                        sRRecommendedTopicsVM.SectionNumber = sectionNumberobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["SectionNumber"]);
                        object ExamIdbj = objSqlDataReader["ExamId"];
                        sRRecommendedTopicsVM.ExamId= ExamIdbj is DBNull ?0: Convert.ToInt32(objSqlDataReader["ExamId"]);
                        sRRecommendedTopicsVMList.Add(sRRecommendedTopicsVM);
                    }
                }
            }

            return sRRecommendedTopicsVMList;
        }

        // This Method Will Get Recommended Topics Export On SR Landing Page 
        public static List<SRRecommendedTopicsExportVM> GetRecommendedTopicsExportSR(int ExamId)
        {
            List<SRRecommendedTopicsExportVM> topicList = new List<SRRecommendedTopicsExportVM>();
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AAODBConnection"].ConnectionString))
            {
                SqlCommand sqlComm = new SqlCommand("AAO_SpacedRepetitionRecommendedTopics", conn);
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.CommandTimeout = 60;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                da.Fill(ds);
                conn.Dispose();
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                SRRecommendedTopicsExportVM topicListBO = new SRRecommendedTopicsExportVM();
                topicListBO.SectionName = Convert.ToString(ds.Tables[0].Rows[i]["SectionName"]);
                topicListBO.ChapterName = Convert.ToString(ds.Tables[0].Rows[i]["ChapterName"]);
                topicListBO.TopicName = Convert.ToString(ds.Tables[0].Rows[i]["TopicName"]);
                topicList.Add(topicListBO);
            }

            return topicList;
        }

        public static int DeleteRecommendedTopic(SRRecommendedTopicsDeletedVM delRecommendedTopic)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@SectionName", delRecommendedTopic.SectionName),
                    new SqlParameter("@ChapterName", delRecommendedTopic.ChapterName),
                    new SqlParameter("@TopicName", delRecommendedTopic.TopicName),
                    new SqlParameter("@UserId", delRecommendedTopic.UserId),
                    new SqlParameter("@ExamId", delRecommendedTopic.ExamId),
                    new SqlParameter("@id", SqlDbType.Int)
            };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_DelRecommendedTopic", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }
    }
}