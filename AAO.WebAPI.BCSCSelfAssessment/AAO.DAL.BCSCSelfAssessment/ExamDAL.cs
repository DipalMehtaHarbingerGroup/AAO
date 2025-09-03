namespace AAO.DAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;

    public static class ExamDAL
    {
        public static List<ExamHistoryVM> GetExamListView(int? userId)
        {
            int examID = 0;
            List<ExamHistoryVM> examSummaryList = new List<ExamHistoryVM>();
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@UserId", Convert.ToInt32(userId)),
                                             };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetExamSummary", objSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        ExamHistoryVM examHistory = new ExamHistoryVM();
                        object examIdObj = objSqlDataReader["Examid"];
                        examHistory.ExamId = examID = examIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Examid"]);
                        object examNameObj = objSqlDataReader["ExamName"];
                        examHistory.ExamName = examNameObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ExamName"]);
                        object attemptQuestionObj = objSqlDataReader["AttempedQuestion"];
                        examHistory.AttempedQuestionCount = attemptQuestionObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["AttempedQuestion"]);
                        object noOfQuestionObj = objSqlDataReader["NoofQuestions"];
                        examHistory.TotalQuestionCount = noOfQuestionObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["NoofQuestions"]);
                        object examTypeObj = objSqlDataReader["ExamType"];
                        examHistory.ExamType = examTypeObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ExamType"]);
                        object examLastAttemptDateObj = objSqlDataReader["ExamLastAttemptDate"];
                        examHistory.LastAttempedDate = examLastAttemptDateObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ExamLastAttemptDate"]);
                        object lastQuestionIdObj = objSqlDataReader["LastQuestionID"];
                        examHistory.LastAttempedQuestionId = lastQuestionIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["LastQuestionID"]);
                        object nextQuestionIDObj = objSqlDataReader["NextQuestionID"];
                        examHistory.NextAttemptQuestionId = nextQuestionIDObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["NextQuestionID"]);
                        object examCountObj = objSqlDataReader["ExamCount"];
                        examHistory.ExamCount = examCountObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ExamCount"]);
                        examSummaryList.Add(examHistory);
                    }

                    objSqlDataReader.Close();
                }
            }

            return examSummaryList;
        }

        // get default Question
        public static int GetExamQuestionDefault(int? examID)
        {
            int examId = 0;
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@ExamId", Convert.ToInt32(examID)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetDefaultQuestion", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object allExamObj = objSqlDataReader["QuestionId"];
                        examId = allExamObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionId"]);
                    }

                    objSqlDataReader.Close();
                }
            }

            return examId;
        }

        public static int CreateExam(ExamDTO exam)
        {
            SqlParameter[] arrSqlParameter =
            {
                new SqlParameter("@ExamName", exam.ExamName),
                new SqlParameter("@ExamCreateDate ", DateTime.Now),
                new SqlParameter("@ExamType", exam.ExamType),
                new SqlParameter("@ExamMode", exam.ExamMode),
                new SqlParameter("@UserId", exam.UserId),
                new SqlParameter("@NoofQuestions", exam.NoofQuestions),
                new SqlParameter("@ExamTimeType", exam.ExamTimeType),
                new SqlParameter("@IsDeleted", exam.IsDeleted),
                new SqlParameter("@ExamAnswerToShow", exam.ExamAnswerToShow),
                new SqlParameter("@TypeofQuestion", exam.TypeofQuestion),
                new SqlParameter("@TypeofCategory", exam.TypeofCategory),
                new SqlParameter("@RoundNumber", exam.RoundNumber),
                new SqlParameter("@SessionId", exam.SessionId),
                new SqlParameter("@StartDate", exam.StartDate),
                new SqlParameter("@EndDate", exam.EndDate),
                new SqlParameter("@EmailIdToWhomExamIsShared", exam.EmailId),
                new SqlParameter("@ShowDetailedAnswers", exam.ShowDetailedAnswers),
                new SqlParameter("@ShowAnsAfterExamCompletion", exam.ShowAnsAfterExamCompletion),
                new SqlParameter("@id", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                }
            };

            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_CreateExam", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        public static ExamDTO GetExamBYId(int? examId)
        {
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                                             };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetExamById", objSqlParameter))
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
                    examObj.StartDate = objSqlDataReader["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(objSqlDataReader["StartDate"]);
                    examObj.StartDate = objSqlDataReader["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(objSqlDataReader["EndDate"]);
                    examObj.EmailId = Convert.ToString(objSqlDataReader["EmailIdToWhomExamIsShared"]);
                }

                return examObj;
            }
        }

        public static List<ExamQuestionDTO> GetAttemptedQuestionDetails_SR(ExamDTO examObj, string selectedQuestionList_string)
        {
            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@ExamID", Convert.ToInt32(examObj.ExamId)),
                new SqlParameter("@RoundNumber", Convert.ToInt32(examObj.RoundNumber)),
                //new SqlParameter("@selectedQuestionList_string", Convert.ToString(selectedQuestionList_string)),
                new SqlParameter("@specialityId", Convert.ToString(examObj.TypeofCategory)),
            };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAttemptedQuestionDetails_SR", objSqlParameter))
            {
                List<ExamQuestionDTO> attemptedQuestionDetails = new List<ExamQuestionDTO>();

                while (objSqlDataReader.Read())
                {
                    ExamQuestionDTO examQuestionDTO = new ExamQuestionDTO
                    {
                        ExamId = Convert.ToInt32(objSqlDataReader["ExamId"]),
                        QuestionId = Convert.ToInt32(objSqlDataReader["QuestionId"]),
                    };
                    attemptedQuestionDetails.Add(examQuestionDTO);
                }

                return attemptedQuestionDetails;
            }
        }

        public static List<QuestionTypeCountDTO> GetQuestionTypeCount(int? userId)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@UserId", Convert.ToInt32(userId)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQuestionType", arrSqlParameter))
            {
                List<QuestionTypeCountDTO> questionTypeCountList = new List<QuestionTypeCountDTO>();

                while (objSqlDataReader.Read())
                {
                    QuestionTypeCountDTO questionTypeCount = new QuestionTypeCountDTO();
                    questionTypeCount.TotalCount = Convert.ToInt32(objSqlDataReader["TotalCounts"]);
                    questionTypeCount.MarkQuestionCount = Convert.ToInt32(objSqlDataReader["MarkQuestionCounts"]);
                    questionTypeCount.IncorrectAnswerCount = Convert.ToInt32(objSqlDataReader["IncorrectAsnwerCounts"]);
                    questionTypeCount.ExamSkipQuestionCount = Convert.ToInt32(objSqlDataReader["ExamSkipQuestionCounts"]);
                    questionTypeCountList.Add(questionTypeCount);
                }

                objSqlDataReader.Close();
                return questionTypeCountList;
            }
        }

        public static List<int> GetExamQuestionIdList(int? specialityId, int userId, List<SelectedQuestionTypeId> selectedQuestionTypeIdList, int topCount)
        {
            List<int> examQuestionList = new List<int>();
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string sqlQueryAllquestion = string.Empty;
            string sqlQueryUnAnswered = string.Empty;
            string sqlQueryIncorrect = string.Empty;
            string sqlQueryMarked = string.Empty;
            if (specialityId != null)
            {
                // SqlQueryAllquestion = @"select top " + topCount;
                sqlQueryAllquestion += @"select Max(question.QuestionId) As ID from Question question
                                        inner join QuestionTopicXwalk questionTopicXWalk on question.QuestionId = questionTopicXWalk.QuestionId
                                        inner join SubspecialtyTopicXWalk subspecialtyTopicXWalk on questionTopicXWalk.TopicId = subspecialtyTopicXWalk.TopicId
                                        inner join Subspecialty subSpeciality on subspecialtyTopicXWalk.SubspecialtyId = subSpeciality.SubspecialtyId 
                                        where subSpeciality.SubspecialtyId in (" + specialityId + ") and question.Status = 7 and question.IsActive=1 group by question.QuestionId ORDER BY NEWID()";

                // SqlQueryUnAnswered = @"select top " + topCount;
                sqlQueryUnAnswered = @" Declare @examid int;
                                        Declare @attemptid int;
                                        set @examid= (select isnull(max(ExamId),0) from Exam with(nolock) where UserId=" + userId + " and ExamType=3)" +
                                        "set @attemptid= (select isnull(max(ExamAttemptId),0) from ExamAttempt with(nolock) where UserId=" + userId + " and Examid=@examid)" +
                                        "select Max(question.QuestionId) As ID from Question question   " +
                                        "join QuestionTopicXwalk QTX on question.QuestionId=QTX.QuestionId " +
                                        "join  SubspecialtyTopicXWalk SPTX on QTX.TopicId=SPTX.TopicId " +
                                        "join Subspecialty SP on SP.SubspecialtyId=sptx.SubspecialtyId " +
                                        "where  question.Status=7 and question.IsActive=1 and question.IsDeleted=0  and question.QuestionId not in" +
                                        "(Select QuestionId from ExamAttemptAsnwer EAA where EAA.UserId=" + userId + " and EAA.Isdeleted=0 " +
                                        "and EAA.ExamAttemptId <> @attemptid and (EAA.RoundNumber = 0 or EAA.RoundNumber is NULL)) " +
                                        "and SP.SubspecialtyId in (" + specialityId + ")  group by question.QuestionId ORDER BY NEWID()";

                //Old Query for getting unanswered Questions
                /*sqlQueryUnAnswered = @"select Max(question.QuestionId) As ID from Question question   
		                                join QuestionTopicXwalk QTX on question.QuestionId=QTX.QuestionId  
		                                join  SubspecialtyTopicXWalk SPTX on QTX.TopicId=SPTX.TopicId 
		                                join Subspecialty SP on SP.SubspecialtyId=sptx.SubspecialtyId 				         
		                                where  question.Status=7 and question.IsActive=1 and question.IsDeleted=0  and question.QuestionId not in
                                        (Select QuestionId from ExamAttemptAsnwer EAA where EAA.UserId=" + userId + " and EAA.Isdeleted=0 
                                         and RoundNumber = 0) and SP.SubspecialtyId in (" + specialityId + ")  
                                         group by question.QuestionId ORDER BY NEWID()";*/

                // SqlQueryIncorrect = @"select top " + topCount;
                sqlQueryIncorrect = @"select Max(question.QuestionId) As ID from Question question 
                                    inner join QuestionTopicXwalk questionTopicXWalk on question.QuestionId = questionTopicXWalk.QuestionId 
                                    inner join SubspecialtyTopicXWalk subspecialtyTopicXWalk on questionTopicXWalk.TopicId = subspecialtyTopicXWalk.TopicId 
                                    inner join Subspecialty subSpeciality on subspecialtyTopicXWalk.SubspecialtyId = subSpeciality.SubspecialtyId
                                    inner join IncorrectQuestion incorrectQuestion on incorrectQuestion.QuestionId= question.QuestionId
                                    AND incorrectQuestion.QuestionId = question.QuestionId and incorrectQuestion.IsDeleted = 0 
                                    and incorrectQuestion.UserId = " + userId + " where subSpeciality.SubspecialtyId in (" + specialityId + ") and question.Status=7 and question.IsActive=1 group by question.QuestionId ORDER BY NEWID()";

                // SqlQueryMarked = @"select top " + topCount;
                sqlQueryMarked = @"select Max(question.QuestionId) As ID from Question question 
                                 inner join QuestionTopicXwalk questionTopicXWalk on question.QuestionId = questionTopicXWalk.QuestionId 
                                 inner join SubspecialtyTopicXWalk subspecialtyTopicXWalk on questionTopicXWalk.TopicId = subspecialtyTopicXWalk.TopicId 
                                 inner join Subspecialty subSpeciality on subspecialtyTopicXWalk.SubspecialtyId = subSpeciality.SubspecialtyId
                                 inner join MarkQuestion markQuestion on markQuestion.QuestionId=question.QuestionId and markQuestion.IsDeleted=0
                                 and markQuestion.UserId=" + userId + " where subSpeciality.SubspecialtyId in (" + specialityId + ") and question.Status=7 and question.IsActive=1 group by question.QuestionId ORDER BY NEWID()";
            }

            foreach (var item in selectedQuestionTypeIdList)
            {
                if (item.questionTypeId == 0)
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sqlQueryAllquestion, connection);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                ExamQuestionId examid = new ExamQuestionId();
                                object idObj = reader.GetOrdinal("ID");
                                examid.examQuestionId = idObj is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
                                examQuestionList.Add(examid.examQuestionId);
                            }

                            reader.Close();
                        }
                    }

                    connection.Close();
                }
                else if (item.questionTypeId == 1)
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sqlQueryUnAnswered, connection);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ExamQuestionId examid = new ExamQuestionId();
                                object idObj = reader.GetOrdinal("ID");
                                examid.examQuestionId = idObj is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
                                examQuestionList.Add(examid.examQuestionId);
                            }

                            reader.Close();
                        }
                    }

                    connection.Close();
                }
                else if (item.questionTypeId == 2)
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sqlQueryIncorrect, connection);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ExamQuestionId examid = new ExamQuestionId();
                                object idObj = reader.GetOrdinal("ID");
                                examid.examQuestionId = idObj is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
                                examQuestionList.Add(examid.examQuestionId);
                            }

                            reader.Close();
                        }
                    }

                    connection.Close();
                }
                else if (item.questionTypeId == 3)
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sqlQueryMarked, connection);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ExamQuestionId examid = new ExamQuestionId();
                                object idObj = reader.GetOrdinal("ID");
                                examid.examQuestionId = idObj is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
                                examQuestionList.Add(examid.examQuestionId);
                            }

                            reader.Close();
                        }
                    }

                    connection.Close();
                }
            }

            List<int> listOfexamQuestion = new List<int>();
            listOfexamQuestion = examQuestionList.Distinct().ToList();
            return listOfexamQuestion;
        }

        public static void UpdateExam(int examId, int userId, int noofQuestion, string columnName)
        {
            try
            {
                SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
                connection.Open();
                using (SqlCommand cmd =
                    new SqlCommand(
                        "UPDATE Exam SET " + columnName + "=@NoofQuestion" +
                        " WHERE UserId=@UserId and ExamId=@ExamId", connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ExamId", examId);
                    cmd.Parameters.AddWithValue("@NoofQuestion", noofQuestion);
                    int rows = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.Write(ex.Message);
            }
        }

        public static void DeleteExam(int examId, int userId)
        {
            try
            {
                SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Exam " + "WHERE UserId=@UserId and ExamId=@ExamId", connection))
                {
                    cmd.Parameters.AddWithValue("@ExamId", examId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    int rows = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.Write(ex.Message);
            }
        }

        public static int GetExamIdBYUserId(int userId)
        {
            int examId = 0;
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string sqlQueryAllquestion = string.Empty;
            sqlQueryAllquestion = "Select Max(ExamId)As ID FROM Exam WHERE UserId= " + @userId;
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

        // This Method Will Get Exam Status Count On Dashboard
        public static ExamStatusCountVM GetExamStatusCount(ServiceCallVM examService)
        {
            ExamStatusCountVM examStatusCount = new ExamStatusCountVM();
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@UserId", examService.userId),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAllExamDetails", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object allExamObj = objSqlDataReader["AllExam"];
                        examStatusCount.allExam = allExamObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["AllExam"]);
                        object completedObj = objSqlDataReader["Completed"];
                        examStatusCount.completed = completedObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Completed"]);
                        object inProgressObj = objSqlDataReader["InProgress"];
                        examStatusCount.inProgress = inProgressObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["InProgress"]);
                        object notStartedObj = objSqlDataReader["NotStarted"];
                        examStatusCount.notStarted = notStartedObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["NotStarted"]);
                    }
                }
            }

            return examStatusCount;
        }

        // This Method Will Get Exam Score On Dashboard
        public static ExamScoreDetailVM GetExamScore(ServiceCallVM examService)
        {
            ExamScoreDetailVM examScore = new ExamScoreDetailVM();
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@UserId", examService.userId),
                     new SqlParameter("@ExamType", examService.examType),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAvrageExamDetails", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object avgScoreObj = objSqlDataReader["AvgScore"];
                        examScore.averageScore = avgScoreObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["AvgScore"]);
                        object attempedQuestiondObj = objSqlDataReader["AttemptedQuestion"];
                        examScore.totalAttempedQuestion = attempedQuestiondObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["AttemptedQuestion"]);
                        object correctQuestionObj = objSqlDataReader["CorrectQuestion"];
                        examScore.correctAnswer = correctQuestionObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["CorrectQuestion"]);
                        object inCorrectQuestionObj = objSqlDataReader["InCorrectQuestion"];
                        examScore.inCorrectAnswer = inCorrectQuestionObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["InCorrectQuestion"]);
                    }

                    objSqlDataReader.Close();
                }
            }

            return examScore;
        }

        // This Method Will Get Dashboard CumulativeChartDetails
        public static List<DashboardChartVM> GetCumulativeChartDetail(ChartParameterVM chartParameters)
        {
            List<DashboardChartVM> chartDetailsList = new List<DashboardChartVM>();
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@UserId", chartParameters.UserId),
                     new SqlParameter("@ExamType", chartParameters.ExamType),
                     new SqlParameter("@FROMdate", chartParameters.FromDate),
                     new SqlParameter("@TOdate", chartParameters.ToDate),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetCumulativePerformanceBySection", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        DashboardChartVM chartDetail = new DashboardChartVM();
                        object subspecialtyIdObj = objSqlDataReader["SubspecialtyId"];
                        chartDetail.SubSpecialityId = subspecialtyIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);
                        object subspecialtyNameObj = objSqlDataReader["Subspecialty_Name"];
                        chartDetail.SubSpecialityName = subspecialtyNameObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Subspecialty_Name"]);
                        object correctObj = objSqlDataReader["correct"];
                        chartDetail.Correct = correctObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["correct"]);
                        object inCorrectObj = objSqlDataReader["InCorrect"];
                        chartDetail.Incorrect = inCorrectObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["InCorrect"]);
                        object remainingObj = objSqlDataReader["remaining"];
                        chartDetail.Remaining = remainingObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["remaining"]);
                        object totalQuestionObj = objSqlDataReader["TotalQuestion"];
                        chartDetail.Total = totalQuestionObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["TotalQuestion"]);
                        chartDetailsList.Add(chartDetail);
                    }

                    objSqlDataReader.Close();
                }
            }

            return chartDetailsList;
        }

        // This Method Will Get Dashboard PeerChartDetail
        public static List<DashboardChartVM> GetPeerChartDetail(ChartParameterVM chartParameters)
        {
            List<DashboardChartVM> chartDetailsList = new List<DashboardChartVM>();
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@UserId", chartParameters.UserId),
                     new SqlParameter("@ExamType", chartParameters.ExamType),
                     new SqlParameter("@FROMdate", chartParameters.FromDate),
                     new SqlParameter("@TOdate", chartParameters.ToDate),
                   };
            try
            {
                using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                    SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetCumulativePerformanceBySectionWithPeer", arrSqlParameter))
                {
                    if (objSqlDataReader.HasRows)
                    {
                        while (objSqlDataReader.Read())
                        {
                            DashboardChartVM chartDetail = new DashboardChartVM();
                            object subspecialtyIdObj = objSqlDataReader["SubspecialtyId"];
                            chartDetail.SubSpecialityId = subspecialtyIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);
                            object subspecialtyNameObj = objSqlDataReader["Subspecialty_Name"];
                            chartDetail.SubSpecialityName = subspecialtyNameObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Subspecialty_Name"]);
                            object selfScoreObj = objSqlDataReader["self"];
                            chartDetail.SelfScore = selfScoreObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["self"]);
                            object peerScoreObj = objSqlDataReader["peer"];
                            chartDetail.PeerScore = peerScoreObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["peer"]);
                            chartDetailsList.Add(chartDetail);
                        }

                        objSqlDataReader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            return chartDetailsList;
        }

        // Get Available Exam Name
        public static string CheckExamNameAvailable(ExamNameVM examName)
        {
            string result = string.Empty;
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@ExamName", examName.ExamName),
                     new SqlParameter("@UserId", examName.UserId),
            };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAvailableExamName", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object examNameObj = objSqlDataReader["ExamName"];
                        result = examNameObj is DBNull ? "Available" : "Not Available";
                    }

                    objSqlDataReader.Close();
                }
            }

            return result;
        }

        // Get  ExamType Count on base of ExamType and userid
        public static ExamCountOnExamTypeVM GetExamCountOnExamType(int userId)
        {
            ExamCountOnExamTypeVM examTypeCount = new ExamCountOnExamTypeVM();
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@UserId", userId),
            };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetExamTypeCountOnUserId", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object customCountObj = objSqlDataReader["CustomCount"];
                        examTypeCount.CustomModeCount = customCountObj is DBNull ? 1 : Convert.ToInt32(objSqlDataReader["CustomCount"]) + 1;
                        object simulatedCountObj = objSqlDataReader["SimulatedCount"];
                        examTypeCount.SimulatedModeCount = simulatedCountObj is DBNull ? 1 : (Convert.ToInt32(objSqlDataReader["SimulatedCount"])) + 1;
                        object quickCountObj = objSqlDataReader["QuickCount"];
                        examTypeCount.QuickModeCount = quickCountObj is DBNull ? 1 : (Convert.ToInt32(objSqlDataReader["QuickCount"])) + 1;
                        object sharedCountObj = objSqlDataReader["SharedCount"];
                        examTypeCount.SharedModeCount = sharedCountObj is DBNull ? 1 : (Convert.ToInt32(objSqlDataReader["SharedCount"])) + 1;
                    }

                    objSqlDataReader.Close();
                }
            }

            return examTypeCount;
        }

        // Get Simulated Question List
        public static List<int> GetSimulatedExamQuestionList(int totalInputQuestion)
        {
            List<int> questionList = new List<int>();
            ExamCountOnExamTypeVM examTypeCount = new ExamCountOnExamTypeVM();
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@TotalInputQuestion", totalInputQuestion),
            };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetSimulatedExamQuestionList", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        int questionId = 0;
                        object customCountObj = objSqlDataReader["QuestionId"];
                        questionId = customCountObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionId"]);
                        questionList.Add(questionId);
                    }

                    objSqlDataReader.Close();
                }
            }

            return questionList;
        }

        // Get UserStatus Is FirstUser
        public static int GetUserStatusIsFirst(ServiceCallVM userService)
        {
            int isLogfirst = 0;
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@UserId", userService.userId),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetIsFirstUser", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object isLogFirstObj = objSqlDataReader["IsFirst"];
                        isLogfirst = isLogFirstObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["IsFirst"]);
                    }

                    objSqlDataReader.Close();
                }
            }

            return isLogfirst;
        }

        // Update UserStatus Is FirstUser
        public static void UpdateUserStatusIsFirst(ServiceCallVM userService)
        {
            SqlParameter[] arrSqlParameter =
                {
                    new SqlParameter("@UserId", userService.userId),
                   };
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateIsFirstUser", arrSqlParameter);
        }

        // Get  QuestionType Count on Section
        public static QuestionCountOnSection GetQuestionTypeCountBySection(QuestionCountOnSection sectionValue)
        {
            QuestionCountOnSection qustionCount = new QuestionCountOnSection();
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@UserId", Convert.ToInt32(sectionValue.UserId)),
                     new SqlParameter("@Filter", Convert.ToString(sectionValue.SectionId)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQuestionCountSectionWiseWithType", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object total_questionObj = objSqlDataReader["total_question"];
                        qustionCount.TotalCount = total_questionObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["total_question"]);

                        object skip_questionObj = objSqlDataReader["skip_question"];
                        qustionCount.SkipCount = skip_questionObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["skip_question"]);

                        object incorrectAsnwerCountsObj = objSqlDataReader["IncorrectAsnwerCounts"];
                        qustionCount.InCorrectCount = incorrectAsnwerCountsObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["IncorrectAsnwerCounts"]);

                        object markQuestionCountsObj = objSqlDataReader["MarkQuestionCounts"];
                        qustionCount.MarkCount = markQuestionCountsObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["MarkQuestionCounts"]);
                    }

                    objSqlDataReader.Close();
                }
            }

            return qustionCount;
        }

        // Get UserStatus Is FirstUser
        public static string GetUserOptInStatus(ServiceCallVM userService)
        {
            string optInStatus = null;
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@UserId", userService.userId),
            };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetOptInStatus", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object isLogFirstObj = objSqlDataReader["OptIn"];
                        optInStatus = isLogFirstObj is DBNull ? null : Convert.ToString(objSqlDataReader["OptIn"]);
                    }

                    objSqlDataReader.Close();
                }
            }

            return optInStatus;
        }

        // Update UserOptIn
        public static int UpdateUserOptIn(ServiceCallVM userService)
        {
            SqlParameter[] arrSqlParameter =
                {
                    new SqlParameter("@UserId", userService.userId),
                    new SqlParameter("@OptIn", userService.OptIn),
                   };
            int rows = SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateUserOptIn", arrSqlParameter);
            return rows;
        }
    }
}