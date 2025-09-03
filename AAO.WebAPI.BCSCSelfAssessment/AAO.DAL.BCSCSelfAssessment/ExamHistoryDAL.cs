namespace AAO.DAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using AAO.DTO.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;
    using Common.BCSCSelfAssessment;

    public class ExamHistoryDAL
    {
        public static List<ExamHistoryDTO> ExamHistoryDetails(ExamHistoryDTO examhistory)
        {
            List<ExamHistoryDTO> examHistoryList = new List<ExamHistoryDTO>();

            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@UserId", examhistory.UserId),
                new SqlParameter("@ExamStatus", examhistory.ExamStatus),
            };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetExamHistory", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    ExamHistoryDTO objExamListBO = new ExamHistoryDTO();
                    object examIdObj = objSqlDataReader["ExamId"];
                    objExamListBO.ExamId = examIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ExamId"]);
                    object examNameObj = objSqlDataReader["ExamName"];
                    objExamListBO.ExamName = examNameObj is DBNull ? null : Convert.ToString(objSqlDataReader["ExamName"]);
                    object examTypeObj = objSqlDataReader["ExamType"];
                    objExamListBO.ExamType = examTypeObj is DBNull ? null : Convert.ToString(objSqlDataReader["ExamType"]);
                    object questionAttemptObj = objSqlDataReader["QuestionAttempt"];
                    objExamListBO.QuestionAttempt = questionAttemptObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionAttempt"]);
                    object noofQuestionsObj = objSqlDataReader["NoofQuestions"];
                    objExamListBO.NoofQuestions = noofQuestionsObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["NoofQuestions"]);
                    object scoreObj = objSqlDataReader["Score"];
                    objExamListBO.Score = scoreObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Score"]);
                    object examLastAttemptDateObj = objSqlDataReader["ExamLastAttemptDate"];
                    objExamListBO.ExamLastAttemptDate = examLastAttemptDateObj is DBNull ? null : Convert.ToString(objSqlDataReader["ExamLastAttemptDate"]);
                    object examAttemptCreatedDateobj = objSqlDataReader["ExamAttemptCreatedDate"];
                    objExamListBO.ExamCreatedDate = examAttemptCreatedDateobj is DBNull ? null : Convert.ToString(objSqlDataReader["ExamAttemptCreatedDate"]);
                    object examStatusobj = objSqlDataReader["ExamStatus"];
                    objExamListBO.ExamStatus = examStatusobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ExamStatus"]);
                    object examTimeTypeObj = objSqlDataReader["ExamTimeType"];
                    objExamListBO.ExamTimeType = examTimeTypeObj is DBNull ? false : Convert.ToBoolean(objSqlDataReader["ExamTimeType"]);
                    object examModeObj = objSqlDataReader["ExamMode"];
                    objExamListBO.ExamMode = examModeObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ExamMode"]);
                    object objLastQuestionID = objSqlDataReader["LastQuestionID"];
                    objExamListBO.LastQuestionId = objLastQuestionID is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["LastQuestionID"]);
                    object objShowDetailedAnswers = objSqlDataReader["ShowDetailedAnswers"];
                    objExamListBO.ShowDetailedAnswers = objShowDetailedAnswers is DBNull ? false : Convert.ToBoolean(objSqlDataReader["ShowDetailedAnswers"]);
                    object objShowAnsAfterExamCompletion = objSqlDataReader["ShowAnsAfterExamCompletion"];
                    objExamListBO.ShowAnsAfterExamCompletion = objShowAnsAfterExamCompletion is DBNull ? false : Convert.ToBoolean(objSqlDataReader["ShowAnsAfterExamCompletion"]);
                    //object objEmailIdToWhomExamIsShared = objSqlDataReader["EmailIdToWhomExamIsShared"];
                    //objExamListBO.EmailIdToWhomExamIsShared = objEmailIdToWhomExamIsShared is DBNull ? null : Convert.ToString(objSqlDataReader["EmailIdToWhomExamIsShared"]);
                    object objIsSharedExam = objSqlDataReader["IsSharedExam"];
                    objExamListBO.IsSharedExam = objIsSharedExam is DBNull ? false : Convert.ToBoolean(objSqlDataReader["IsSharedExam"]);
                    object objExamStartDate = objSqlDataReader["ExamStartDate"];
                    objExamListBO.ExamStartDate = objExamStartDate is DBNull ? null : Convert.ToString(objSqlDataReader["ExamStartDate"]);
                    object objExamEndDate = objSqlDataReader["ExamEndDate"];
                    objExamListBO.ExamEndDate = objExamEndDate is DBNull ? null : Convert.ToString(objSqlDataReader["ExamEndDate"]);

                    examHistoryList.Add(objExamListBO);
                }

                objSqlDataReader.Close();
            }

            return examHistoryList;
        }

        public static void DeleteExamHistoryDetails(int examId, int userId)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", examId),
                    new SqlParameter("@UserId", userId),
                   };
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_DeleteExam", arrSqlParameter);
        }

        public static PdfDetailsDataVM GetPdfDetails(ExamHistoryDTO examhistory)
        {
            PdfDetailsDataVM pdfDetailsList = new PdfDetailsDataVM();

            SqlParameter[] objSqlParameter =
            {
                                                 new SqlParameter("@ExamId", examhistory.ExamId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetExamDetailForPDF", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    object completionDate = objSqlDataReader["CompletionDate"];
                    pdfDetailsList.CompletionDate = completionDate is DBNull ? null : Convert.ToDateTime(objSqlDataReader["CompletionDate"]).ToString("MM/dd/yyyy");
                    object userName = objSqlDataReader["UserName"];
                    pdfDetailsList.UserName = userName is DBNull ? null : Convert.ToString(objSqlDataReader["UserName"]);
                    object residencyEnd = objSqlDataReader["ResidencyEnd"];
                    pdfDetailsList.ResidencyEndDate = residencyEnd is DBNull ? null : Convert.ToDateTime(objSqlDataReader["ResidencyEnd"]).ToString("MM/dd/yyyy");
                    object totalQuestion = objSqlDataReader["TotalQuestion"];
                    pdfDetailsList.TotalQuestion = totalQuestion is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["TotalQuestion"]);
                    object timedExam = objSqlDataReader["TimedExam"];
                    pdfDetailsList.TimedExam = timedExam is DBNull ? null : Convert.ToString(objSqlDataReader["TimedExam"]);
                    object examMode = objSqlDataReader["mode"];
                    pdfDetailsList.ExamMode = examMode is DBNull ? null : Convert.ToString(objSqlDataReader["mode"]);
                    object overallScore = objSqlDataReader["Score"];
                    pdfDetailsList.OverallScore = overallScore is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Score"]);
                    object correctAnswers = objSqlDataReader["CorrectAnswers"];
                    pdfDetailsList.CorrectAnswers = correctAnswers is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["CorrectAnswers"]);
                    object incorrectAnswers = objSqlDataReader["IncorrectAnswers"];
                    pdfDetailsList.IncorrectAnswers = incorrectAnswers is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["IncorrectAnswers"]);
                    object unanswered = objSqlDataReader["Unanswered"];
                    pdfDetailsList.Unanswered = unanswered is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Unanswered"]);
                    pdfDetailsList.SectionLIst = ExamHistoryDAL.GetSectionListBYExamID(examhistory.ExamId);
                }

                objSqlDataReader.Close();
            }

            return pdfDetailsList;
        }

        public static List<SectionVM> GetSectionListBYExamID(int examId)
        {
            List<SectionVM> sectionList = new List<SectionVM>();

            SqlParameter[] objSqlParameter =
            {
                                                 new SqlParameter("@ExamId", examId ),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetSectionListFromExam", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    SectionVM section = new SectionVM();
                    object sectionObj = objSqlDataReader["Section"];
                    section.SectionName = sectionObj is DBNull ? null : Convert.ToString(objSqlDataReader["Section"]);
                    sectionList.Add(section);
                }

                objSqlDataReader.Close();
            }

            return sectionList;
        }

        // Reset Exam
        public static int ResetExam(int userId)
        {
            int result = 0;
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@UserId", userId),
                };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_ResetExamOnUserId", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object resultObj = objSqlDataReader["Result"];
                        result = resultObj is DBNull ? 1 : Convert.ToInt32(objSqlDataReader["Result"]);
                    }

                    objSqlDataReader.Close();
                }
            }

            return result;
        }

        public static ExamHistoryDTO InsertorAddEmail_GetOptOutDetails(string email, ExamHistoryDTO values)
        {
            ExamHistoryDTO emaildetails = new ExamHistoryDTO();
            try
            {
                SqlParameter[] arrSqlParameter =
                {
                     new SqlParameter("@emailid", email),
                     new SqlParameter("@userid_fromwhom", values.UserId),
                };
                using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                    SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "InsertorAddEmail_GetOptOutDetails", arrSqlParameter))
                {
                    if (objSqlDataReader.HasRows)
                    {
                        while (objSqlDataReader.Read())
                        {
                            object idObj = objSqlDataReader["id"];
                            emaildetails.Id = idObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Id"]);

                            object emailObj = objSqlDataReader["emailid"];
                            emaildetails.Emailids = emailObj is DBNull ? "no Email" : Convert.ToString(objSqlDataReader["emailid"]);

                            object optOutObj = objSqlDataReader["OptOut"];
                            emaildetails.OptOut = optOutObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["OptOut"]);
                        }

                        objSqlDataReader.Close();
                    }
                }

                return emaildetails;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message + ", Class:{0}, Method:{1}, userId:{2}", "ExamHistoryDAL", "ResetExam", values.UserId);
                return emaildetails;
            }
        }

        public static string OptOut(int id)
        {
            string emailid = "Email Not Found";
            try
            {
                SqlParameter[] arrSqlParameter =
                {
                     new SqlParameter("@id", id),
                };
                using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                    SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "OptOut_email", arrSqlParameter))
                {
                    if (objSqlDataReader.HasRows)
                    {
                        while (objSqlDataReader.Read())
                        {
                            object idObj = objSqlDataReader["emailid"];
                            emailid = idObj is DBNull ? " " : Convert.ToString(objSqlDataReader["emailid"]);
                        }
                    }
                }

                return emailid;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
