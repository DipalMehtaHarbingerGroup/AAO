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

    public class AdminIncorrectQuestionDetailsDAL
    {
        public static List<IncorrectQuestionDetailsDTO> AdminIncorrectQuestionDetails(int year, int? userId, int? subspecialtyId, DateTime? examStartDate, DateTime? examCompletedDate, int? noOfRecords, int? pageNo)
        {
            List<IncorrectQuestionDetailsDTO> incorrectReportList = new List<IncorrectQuestionDetailsDTO>();
            SqlParameter[] objSqlParameter =
            {
                                                    new SqlParameter("@UserId", null),
                                                    new SqlParameter("@SubspecialtyId", Convert.ToInt32(subspecialtyId)),
                                                    new SqlParameter("@StartDate", examStartDate),
                                                    new SqlParameter("@EndDate", examCompletedDate),
                                                    new SqlParameter("@PageSize", noOfRecords),
                                                    new SqlParameter("@PageIndex", pageNo),
                                                    new SqlParameter("@UserYear", year),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetInCorrectQuestionBytSection", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    IncorrectQuestionDetailsDTO inCorrectListBO = new IncorrectQuestionDetailsDTO();
                    object examidObj = objSqlDataReader["Examid"];
                    inCorrectListBO.ExamId = examidObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Examid"]);

                    object questionIdObj = objSqlDataReader["QuestionId"];
                    inCorrectListBO.QuestionId = questionIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionId"]);

                    object questionIdCountObj = objSqlDataReader["RecordCount"];
                    inCorrectListBO.QuestionIdCount = questionIdCountObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["RecordCount"]);

                    object subspecialtyObj = objSqlDataReader["subspecialty"];
                    inCorrectListBO.Subspecialty = questionIdCountObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["subspecialty"]);

                    object sectionObj = objSqlDataReader["section"];
                    inCorrectListBO.Section = sectionObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["section"]);

                    object rownumberObj = objSqlDataReader["rownumber"];
                    inCorrectListBO.Rownumber = rownumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["rownumber"]);

                    object examtypeObj = objSqlDataReader["examtype"];
                    inCorrectListBO.ExamType = examtypeObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["examtype"]);

                    incorrectReportList.Add(inCorrectListBO);
                }

                 objSqlDataReader.Close();
            }

            return incorrectReportList;
        }

        public static int GetAdminFirstQuestion(int? examId)
        {
            int questionId = 0;
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string sqlQueryAllquestion = string.Empty;
            sqlQueryAllquestion = "Select Top(1) QuestionId As ID FROM ExamQuestion WHERE ExamId= " + @examId + " ORDER BY QuestionId ASC";
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlQueryAllquestion, connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        object objID = reader["ID"];
                        questionId = objID is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
                    }

                    reader.Close();
                }
            }

            connection.Close();
            return questionId;
        }

        public static List<string> GetAdminReferences(int questionId)
        {
            List<string> referencesList = new List<string>();
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string sqlQueryReference = string.Empty;
            sqlQueryReference = "Select Question_References As reference  FROM Question WHERE QuestionId= " + @questionId;
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlQueryReference, connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        string refertences = string.Empty;
                        object obj = reader["reference"];
                        refertences = obj is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("reference"));
                        referencesList.Add(refertences);
                    }

                    reader.Close();
                }
            }

            connection.Close();
            return referencesList;
        }

        public static List<string> GetAdminRelatedContent(int questionId, SqlConnection gConnection = null)
        {
            List<string> relatedContent_List = new List<string>();
            SqlConnection connection = gConnection;
            if (gConnection == null)
            {
                connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
                connection.Open();
            }

            SqlCommand cmd = new SqlCommand("AAO_GetRelatedContents", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@questionId ", questionId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        string content = string.Empty;
                        object contentObj = reader["contents"];
                        content = contentObj is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("contents"));
                        relatedContent_List.Add(content);
                    }

                    reader.Close();
                }
            }

            if (gConnection == null)
            {
                connection.Close();
            }

            return relatedContent_List;
        }

        public static List<string> GetAdminNotes(int questionId, int userId)
        {
            List<string> notesList = new List<string>();
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string sqlQueryAllquestion = string.Empty;
            sqlQueryAllquestion = "Select ExamAttemptNoteText As notes  FROM ExamNotes WHERE QuestionId= " + @questionId; // + " and UserId= " + @userId;
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlQueryAllquestion, connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        string notes = string.Empty;
                        object objNotes = reader["notes"];
                        notes = objNotes is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("notes"));
                        notesList.Add(notes);
                    }

                    reader.Close();
                }
            }

            connection.Close();
            return notesList;
        }

        public static int GetAdminQuestionCount(int examId)
        {
            int result = 0;
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", examId),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAdminQuestionCount", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object objQuestionCount = objSqlDataReader["QuestionCount"];
                        result = objQuestionCount is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionCount"]);
                    }

                    objSqlDataReader.Close();
                }
            }

            return result;
        }

        public static List<PeerDetailVM> GetAdminPeerDetails(int questionId, int userId)
        {
            List<PeerDetailVM> peerDetailsList = new List<PeerDetailVM>();
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@QuestionId", questionId),
                    new SqlParameter("@UserId", userId),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAdminPeerDetails", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        PeerDetailVM peerDetail = new PeerDetailVM();
                        object choiceSequendceObj = objSqlDataReader["ChoiceSequence"];
                        peerDetail.ChoiceSequence = choiceSequendceObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ChoiceSequence"]);
                        object choiceOptionObj = objSqlDataReader["Choice"];
                        peerDetail.ChoiceOption = choiceOptionObj is DBNull ? '\0' : Convert.ToChar(objSqlDataReader["Choice"]);
                        object choiceIdObj = objSqlDataReader["Choice_Id"];
                        peerDetail.ChoiceId = choiceIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Choice_Id"]);
                        object isRightChoiceObj = objSqlDataReader["IsRightChoice"];
                        peerDetail.IsRightChoice = isRightChoiceObj is DBNull ? false : Convert.ToBoolean(objSqlDataReader["IsRightChoice"]);
                        object percentageObj = objSqlDataReader["Per_Choice"];
                        peerDetail.ChoicePercentage = percentageObj is DBNull ? 0 : Convert.ToDouble(objSqlDataReader["Per_Choice"]);
                        object usertypeObj = objSqlDataReader["usertype"];
                        peerDetail.UserType = usertypeObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["usertype"]);
                        peerDetailsList.Add(peerDetail);
                    }

                    objSqlDataReader.Close();
                }
            }

            return peerDetailsList;
        }

        public static bool GetAdminMarkStatus(int? examId, int? questionId)
        {
            bool markStatus = false;
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                    new SqlParameter("@QuestionId", Convert.ToInt32(questionId)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAdminMarkStatus", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object objMarkStatus = objSqlDataReader["MarkStatus"];
                        markStatus = objMarkStatus is DBNull ? false : Convert.ToBoolean(objSqlDataReader["MarkStatus"]);
                    }

                    objSqlDataReader.Close();
                }

                return markStatus;
            }
        }

        public static string GetAdminExamAttemptAnswerText(int? examId, int? questionId)
        {
            string examAttemptAnswerText = string.Empty;
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                    new SqlParameter("@QuestionId", Convert.ToInt32(questionId)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAdminExamAttemptAnswerText", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object objExamAttemptAnswerText = objSqlDataReader["ExamAttemptAnswerText"];
                        examAttemptAnswerText = objExamAttemptAnswerText is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ExamAttemptAnswerText"]);
                    }

                    objSqlDataReader.Close();
                }

                return examAttemptAnswerText;
            }
        }

        public static QuestionDetails GetAdminQuestionDetails(int? examId, int? questionId)
        {
            QuestionDetails questionDetail = new QuestionDetails();
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                    new SqlParameter("@QuestionId", Convert.ToInt32(questionId)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAdminQuestionDetails", arrSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    questionDetail.questionId = Convert.ToInt32(objSqlDataReader["questionid"]);
                    object prevQuestionIdObj = objSqlDataReader["prevQuestionId"];
                    questionDetail.previousId = prevQuestionIdObj is DBNull ? -1 : Convert.ToInt32(objSqlDataReader["prevQuestionId"]);
                    object nextQuestionIdObj = objSqlDataReader["nextQuestionId"];
                    questionDetail.nextId = nextQuestionIdObj is DBNull ? -2 : Convert.ToInt32(objSqlDataReader["nextQuestionId"]);
                    object serialNumberObj = objSqlDataReader["serialNumber"];
                    questionDetail.serialNumber = serialNumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["serialNumber"]);
                    object stemObj = objSqlDataReader["Stem"];
                    questionDetail.questionText = stemObj is DBNull ? null : Convert.ToString(objSqlDataReader["Stem"]);
                    object friendlyidobj = objSqlDataReader["Friendlyid"];
                    questionDetail.FriendlyId = friendlyidobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Friendlyid"]);
                    object notRandomizeAnswersobj = objSqlDataReader["NotRandomizeAnswers"];
                    questionDetail.NotRandomizeAnswer = notRandomizeAnswersobj is DBNull ? false : Convert.ToBoolean(objSqlDataReader["NotRandomizeAnswers"]);
                }

                objSqlDataReader.Close();
                List<QuestionImage> questionImageList = new List<QuestionImage>();
                questionImageList = AssessmentDAL.GetQuestionImageList(questionId);
                questionDetail.questionImage = questionImageList;
                return questionDetail;
            }
        }

        public static QuestionDetails GetAdminQuestionDetails_SR(int? examId, int? questionId)
        {
            QuestionDetails questionDetail = new QuestionDetails();
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                    new SqlParameter("@QuestionId", Convert.ToInt32(questionId)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAdminQuestionDetails_SR", arrSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    questionDetail.questionId = Convert.ToInt32(objSqlDataReader["questionid"]);
                    //object prevQuestionIdObj = objSqlDataReader["prevQuestionId"];
                    //questionDetail.previousId = prevQuestionIdObj is DBNull ? -1 : Convert.ToInt32(objSqlDataReader["prevQuestionId"]);
                    //object nextQuestionIdObj = objSqlDataReader["nextQuestionId"];
                    //questionDetail.nextId = nextQuestionIdObj is DBNull ? -2 : Convert.ToInt32(objSqlDataReader["nextQuestionId"]);
                    object serialNumberObj = objSqlDataReader["serialNumber"];
                    questionDetail.serialNumber = serialNumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["serialNumber"]);
                    object stemObj = objSqlDataReader["Stem"];
                    questionDetail.questionText = stemObj is DBNull ? null : Convert.ToString(objSqlDataReader["Stem"]);
                    object friendlyidobj = objSqlDataReader["Friendlyid"];
                    questionDetail.FriendlyId = friendlyidobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Friendlyid"]);
                }

                objSqlDataReader.Close();
                List<QuestionImage> questionImageList = new List<QuestionImage>();
                questionImageList = AssessmentDAL.GetQuestionImageList(questionId);
                questionDetail.questionImage = questionImageList;
                return questionDetail;
            }
        }

        public static ExamDTO GetAdminExamBYId(int? examId)
        {
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                                             };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAdminExamList", objSqlParameter))
            {
                ExamDTO examObj = new ExamDTO();
                while (objSqlDataReader.Read())
                {
                    object objExamId = objSqlDataReader["ExamId"];
                    examObj.ExamId = objExamId is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ExamId"]);
                    object objExamName = objSqlDataReader["ExamName"];
                    examObj.ExamName = objExamName is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ExamName"]);
                    examObj.ExamCreateDate = Convert.ToDateTime(objSqlDataReader["ExamCreateDate"]);
                    object objIsDeleted = objSqlDataReader["IsDeleted"];
                    examObj.IsDeleted = objIsDeleted is DBNull ? false : (bool)(objSqlDataReader["IsDeleted"]);
                    object objExamType = objSqlDataReader["ExamType"];
                    examObj.ExamType = objExamType is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ExamType"]);
                    object objNoofQuestions = objSqlDataReader["NoofQuestions"];
                    examObj.NoofQuestions = objNoofQuestions is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["NoofQuestions"]);
                    object objExamTimeType = objSqlDataReader["ExamTimeType"];
                    examObj.ExamTimeType = objExamTimeType is DBNull ? false : (bool)(objSqlDataReader["ExamTimeType"]);
                    object objExamMode = objSqlDataReader["ExamMode"];
                    examObj.ExamMode = objExamMode is DBNull ? false : (bool)(objSqlDataReader["ExamMode"]);
                    object objExamAnswerToShow = objSqlDataReader["ExamAnswerToShow"];
                    examObj.ExamAnswerToShow = objExamAnswerToShow is DBNull ? false : (bool)(objSqlDataReader["ExamAnswerToShow"]);
                    object objTypeofQuestion = objSqlDataReader["TypeofQuestion"];
                    examObj.TypeofQuestion = objTypeofQuestion is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["TypeofQuestion"]);
                    object objTypeofCategory = objSqlDataReader["TypeofCategory"];
                    examObj.TypeofCategory = objTypeofCategory is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["TypeofCategory"]);
                    object objUserId = objSqlDataReader["UserId"];
                    examObj.UserId = objUserId is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["UserId"]);
                    object objExamStatus = objSqlDataReader["ExamStatus"];
                    examObj.ExamStatus = objExamStatus is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ExamStatus"]);
                    object objExamAttemptId = objSqlDataReader["ExamAttemptId"];
                    examObj.ExamAttemptId = objExamAttemptId is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ExamAttemptId"]);
                }

                objSqlDataReader.Close();
                return examObj;
            }
        }
    }
}
