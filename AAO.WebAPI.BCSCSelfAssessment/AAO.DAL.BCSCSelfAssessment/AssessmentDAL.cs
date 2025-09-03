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


    public class AssessmentDAL
    {
        public static List<Choice> GetChoiceBYQuestionIdForBCSCMapper(int? questionId, SqlConnection gConnection = null)
        {
            List<Choice> choiceList = new List<Choice>();
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string sqlQueryChoice = string.Empty;

            int notRandomizeAnswer = 0;
            SqlConnection aconnection = gConnection;
            if (gConnection == null)
            {
                aconnection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
                aconnection.Open();
            }

            SqlCommand cmd_a = new SqlCommand("AAO_GetNotRandomizeAnswer_Value", aconnection);
            cmd_a.CommandType = CommandType.StoredProcedure;
            cmd_a.Parameters.Add(new SqlParameter("@questionId ", questionId));
            using (SqlDataReader reader = cmd_a.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        object objID = reader["NotRan_Value"];
                        notRandomizeAnswer = objID is DBNull ? 0 : Convert.ToInt32(reader["NotRan_Value"]);
                    }

                    reader.Close();
                }
            }
            if (gConnection == null)
            {
                aconnection.Close();
            }

            if (notRandomizeAnswer == 1)
            {
                sqlQueryChoice = "Select Choice_Id As ID, ChoiceText as Text FROM QuestionChoice WHERE QuestionId= " + @questionId + " ORDER BY ChoiceSequence ASC ";
            }
            else
            {
                sqlQueryChoice = "Select Choice_Id As ID, ChoiceText as Text FROM QuestionChoice WHERE QuestionId= " + @questionId;
            }

            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlQueryChoice, connection);
            int sequenceNo = 64;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        Choice choice = new Choice();
                        choice.choiceId = reader.GetInt32(reader.GetOrdinal("ID"));
                        object textObj = reader["Text"];
                        choice.choiceText = textObj is DBNull ? null : reader.GetString(reader.GetOrdinal("Text"));

                        // // System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");
                        //    // choice.choiceText = rx.Replace(reader.GetString(reader.GetOrdinal("Text")), "");
                        //    choice.choiceText = reader.GetString(reader.GetOrdinal("Text"));
                        sequenceNo++;
                        choice.choiceSequence = (char)sequenceNo;
                        choice.ChoiceImage = GetChoiceImageList(choice.choiceId);
                        choiceList.Add(choice);
                    }

                    reader.Close();
                }
            }

            connection.Close();
            return choiceList;
        }

        public static List<Choice> GetChoiceBYQuestionId(int? questionId, int? examId, int? choiceID, int? examAttemptId, bool? notRandomizeAnswer, SqlConnection gConnection = null)
        {
            List<Choice> choiceList = new List<Choice>();
            SqlConnection connection = gConnection;
            SqlCommand cmd = new SqlCommand();
            if (gConnection == null)
            {
                connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            }

            string sqlQueryChoice = string.Empty;
            string sqlQueryAttempt = string.Empty;
            string userExamState = string.Empty;
            if (choiceID > 0)
            {
                //sqlQueryAttempt = "select UserExamState from ExamAttemptAsnwer where ExamAttemptId=" + @examAttemptId + " and QuestionId = " + @questionId;
                if (gConnection == null)
                {
                    connection.Open();
                }
                //SqlCommand cmdAttempt = new SqlCommand(sqlQueryAttempt, connection);

                SqlCommand cmdAttempt = new SqlCommand("AAO_GetChoiceBYQuestionId_if", connection);
                cmdAttempt.CommandType = CommandType.StoredProcedure;
                cmdAttempt.Parameters.Add(new SqlParameter("@examAttemptId", examAttemptId));
                cmdAttempt.Parameters.Add(new SqlParameter("@questionId", questionId));

                using (SqlDataReader reader = cmdAttempt.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        // Read advances to the next row.
                        while (reader.Read())
                        {
                            object obj = reader["UserExamState"];
                            if (obj is DBNull)
                            {
                                userExamState = null;

                                // sqlQueryChoice = "Select Choice_Id As ID, ChoiceText as Text FROM QuestionChoice WHERE QuestionId= " + @questionId + " ORDER BY NEWID()";
                                //sqlQueryChoice = "Select Choice_Id As ID, ChoiceText as Text FROM QuestionChoice WHERE QuestionId= " + @questionId;

                                cmd = new SqlCommand("AAO_GetChoiceBYQuestionId_if_if", connection);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@questionId", questionId));

                            }
                            else
                            {
                                userExamState = reader.GetString(reader.GetOrdinal("UserExamState"));

                                // sqlQueryChoice = "Select Choice_Id As ID, ChoiceText as Text FROM QuestionChoice WHERE Choice_Id IN(" + userExamState + " )";
                                string sq = string.Empty;
                                sq = "'" + userExamState + "'";
                                sqlQueryChoice = " Select Choice_Id As ID, ChoiceText  as Text FROM QuestionChoice WHERE Choice_Id IN(" + userExamState + " ) ORDER BY CHARINDEX(CAST(Choice_Id AS VARCHAR), " + sq + " )";
                                cmd = new SqlCommand(sqlQueryChoice, connection);

                                /*cmd = new SqlCommand("AAO_GetChoiceBYQuestionId_if_else1", connection);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@userExamState", userExamState));
                                cmd.Parameters.Add(new SqlParameter("@sq", sq));*/
                            }
                        }

                        reader.Close();
                    }
                }
            }
            else
            {
                if (notRandomizeAnswer == true)
                {
                    cmd = new SqlCommand("AAO_GetChoiceBYQuestionId_notRandomize", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@questionId ", questionId));
                }
                else
                {
                    //sqlQueryChoice = "Select Choice_Id As ID, ChoiceText as Text FROM QuestionChoice WHERE QuestionId= " + @questionId + " ORDER BY NEWID()";

                    cmd = new SqlCommand("AAO_GetChoiceBYQuestionId_else", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@questionId ", questionId));
                }
            }

            // sqlQueryChoice = "Select Choice_Id As ID, ChoiceText as Text FROM QuestionChoice WHERE QuestionId= " + @questionId ;
            if (gConnection == null)
            {
                connection.Close();
            }
            if (gConnection == null)
            {
                connection.Open();
            }
            //SqlCommand cmd = new SqlCommand(sqlQueryChoice, connection);
            //cmd = new SqlCommand(sqlQueryChoice, connection);

            int sequenceNo = 64;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        Choice choice = new Choice();
                        choice.choiceId = reader.GetInt32(reader.GetOrdinal("ID"));
                        object textObj = reader["Text"];
                        choice.choiceText = textObj is DBNull ? null : reader.GetString(reader.GetOrdinal("Text"));

                        // // System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");
                        //    // choice.choiceText = rx.Replace(reader.GetString(reader.GetOrdinal("Text")), "");
                        //    choice.choiceText = reader.GetString(reader.GetOrdinal("Text"));
                        sequenceNo++;
                        choice.choiceSequence = (char)sequenceNo;
                        choice.ChoiceImage = GetChoiceImageList(choice.choiceId);
                        choiceList.Add(choice);
                    }

                    reader.Close();
                }
            }
            if (gConnection == null)
            {
                connection.Close();
            }
            return choiceList;
        }

        public static int GetFirstQuestion(ExamDTO examObj)
        {
            int questionId = 0;
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            /*string sqlQueryAllquestion = string.Empty;
            if (examObj.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition))
            {
                sqlQueryAllquestion = "SELECT TOP(1) QuestionId As ID FROM ExamQuestion WHERE ExamId= " + examObj.ExamId + " AND Isdeleted= 0 ORDER BY ExamQuestionId ASC";
            }
            else
            {
                sqlQueryAllquestion = "Select Top(1) QuestionId As ID FROM ExamQuestion WHERE ExamId= " + examObj.ExamId + " and Isdeleted= 0 ORDER BY ExamQuestionId ASC";
            }
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlQueryAllquestion, connection);*/

            connection.Open();
            SqlCommand cmd = new SqlCommand("AAO_GetFirstQuestion", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@examId ", examObj.ExamId));

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
                }
            }

            connection.Close();
            return questionId;
        }

        public static QuestionDetails GetQuestionDetails(int? examId, int? questionId)
        {
            QuestionDetails questionDetail = new QuestionDetails();
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                    new SqlParameter("@QuestionId", Convert.ToInt32(questionId)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQuestionDetails", arrSqlParameter))
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
                    object examType = objSqlDataReader["ExamType"];
                    questionDetail.ExamType = examType is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ExamType"]);
                    object notRandomizeAnswerobj = objSqlDataReader["notRandomizeAnswers"];
                    questionDetail.NotRandomizeAnswer = notRandomizeAnswerobj is DBNull ? false : Convert.ToBoolean(objSqlDataReader["notRandomizeAnswers"]);
                }

                objSqlDataReader.Close();

                List<QuestionImage> questionImageList = new List<QuestionImage>();
                questionImageList = GetQuestionImageList(questionId);
                questionDetail.questionImage = questionImageList;
                return questionDetail;
            }
        }

        //Added for User Report if ExamType=3 then fetch Ques from there
        public static QuestionDetails GetQuestionDetails_SR(int? examId, int? questionId)
        {
            QuestionDetails questionDetail = new QuestionDetails();
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                    new SqlParameter("@QuestionId", Convert.ToInt32(questionId)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQuestionDetails_SR", arrSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    questionDetail.questionId = Convert.ToInt32(objSqlDataReader["questionid"]);
                    //object prevQuestionIdObj = objSqlDataReader["prevQuestionId"];
                    //questionDetail.previousId = prevQuestionIdObj is DBNull ? -1 : Convert.ToInt32(objSqlDataReader["prevQuestionId"]);
                    //object nextQuestionIdObj = objSqlDataReader["nextQuestionId"];
                    //questionDetail.nextId = nextQuestionIdObj is DBNull ? -2 : Convert.ToInt32(objSqlDataReader["nextQuestionId"]);
                    //object serialNumberObj = objSqlDataReader["serialNumber"];
                    //questionDetail.serialNumber = serialNumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["serialNumber"]);
                    object stemObj = objSqlDataReader["Stem"];
                    questionDetail.questionText = stemObj is DBNull ? null : Convert.ToString(objSqlDataReader["Stem"]);
                    object friendlyidobj = objSqlDataReader["Friendlyid"];
                    questionDetail.FriendlyId = friendlyidobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Friendlyid"]);
                }

                objSqlDataReader.Close();

                List<QuestionImage> questionImageList = new List<QuestionImage>();
                questionImageList = GetQuestionImageList(questionId);
                questionDetail.questionImage = questionImageList;
                return questionDetail;
            }
        }

        public static List<QuestionImage> GetQuestionImageList(int? questionId)
        {
            List<QuestionImage> questionImageList = new List<QuestionImage>();
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string sqlQueryQuestionImage = string.Empty;
            sqlQueryQuestionImage = "Select * FROM Question_Image WHERE QuestionId= " + @questionId + " ORDER BY Figure, Question_Image_id DESC";
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlQueryQuestionImage, connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        QuestionImage questionImage = new QuestionImage();
                        object objQuestionImageid = reader["Question_Image_id"];
                        questionImage.QuestionImageId = objQuestionImageid is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("Question_Image_id"));
                        object objCaption = reader["Caption"];
                        questionImage.Caption = objCaption is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Caption"));
                        object objComments = reader["Comments"];
                        questionImage.Comments = objComments is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Comments"));
                        object objCredit = reader["Credit"];
                        questionImage.Credit = objCredit is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Credit"));
                        object objFigure = reader["Figure"];
                        questionImage.Figure = objFigure is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Figure"));
                        object objImagePath = reader["ImagePath"];
                        questionImage.ImagePath = objImagePath is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("ImagePath"));
                        object objSource = reader["Source"];
                        questionImage.Source = objSource is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Source"));
                        object objTitle = reader["Title"];
                        questionImage.Title = objTitle is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Title"));
                        object objQuestionId = reader["QuestionId"];
                        questionImage.QuestionId = objQuestionId is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("QuestionId"));
                        questionImageList.Add(questionImage);
                    }

                    reader.Close();
                }
            }

            connection.Close();
            return questionImageList;
        }

        public static List<ChoiceImage> GetChoiceImageList(int? choiceId)
        {
            List<ChoiceImage> choiceImageList = new List<ChoiceImage>();
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            /*string sqlQueryGetChoiceImage = string.Empty;
            sqlQueryGetChoiceImage = "Select * FROM Answer_Choice_Image WHERE Choice_Id= " + @choiceId + " ORDER BY Answer_Choice_Image_id ASC";
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlQueryGetChoiceImage, connection);*/

            connection.Open();
            SqlCommand cmd = new SqlCommand("AAO_GetChoiceImageList", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@choiceId", choiceId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        ChoiceImage questionImage = new ChoiceImage();
                        object objAnswerChoiceImageid = reader["Answer_Choice_Image_id"];
                        questionImage.AnswerChoiceImageId = objAnswerChoiceImageid is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("Answer_Choice_Image_id"));
                        object objCaption = reader["Caption"];
                        questionImage.Caption = objCaption is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Caption"));
                        object objCredit = reader["Credit"];
                        questionImage.Credit = objCredit is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Credit"));
                        object objFigure = reader["Figure"];
                        questionImage.Figure = objFigure is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Figure"));
                        object objImagePath = reader["ImagePath"];
                        questionImage.ImagePath = objImagePath is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("ImagePath"));
                        object objSource = reader["Source"];
                        questionImage.Source = objSource is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Source"));
                        object objTitle = reader["Title"];
                        questionImage.Title = objTitle is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Title"));
                        object objChoiceId = reader["Choice_Id"];
                        questionImage.ChoiceId = objChoiceId is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("Choice_Id"));
                        choiceImageList.Add(questionImage);
                    }

                    reader.Close();
                }
            }

            connection.Close();
            return choiceImageList;
        }

        public static int GetRightChoiceByQuestionId(int questionId, SqlConnection gConnection=null)
        {

            int rightChoiceId = 0;
            SqlConnection connection = gConnection;
            if (gConnection == null)
            {
                connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
                connection.Open();
            }

            /*string sqlQueryGetChoice = string.Empty;
            sqlQueryGetChoice = "select Choice_Id as ID from QuestionChoice where QuestionId =" + @questionId + " and IsRightChoice =" + 1;*/

            SqlCommand cmd = new SqlCommand("AAO_GetRightChoiceByQuestionId", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@questionId ", questionId));
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        object objID = reader["ID"];
                        rightChoiceId = objID is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
                    }

                    reader.Close();
                }
            }
            if (gConnection == null)
            {
                connection.Close();
            }
            return rightChoiceId;
        }

        // public static int UpdateExamAttempt(int examId, int userId, int examStatus)
        // {
        //    int status = 0;
        //    SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
        //    string sqlQueryUpdateGet = string.Empty;
        //    sqlQueryUpdateGet = "update ExamAttempt SET ExamLastAttemptDate=GETDATE(),ExamStatus= " + examStatus + " OUTPUT INSERTED.ExamStatus as ID where ExamId =" + examId + " and UserId= " + userId;
        //    connection.Open();
        //    SqlCommand cmd = new SqlCommand(sqlQueryUpdateGet, connection);
        //    using (SqlDataReader reader = cmd.ExecuteReader())
        //    {
        //        if (reader.HasRows)
        //        {
        //            // Read advances to the next row.
        //            while (reader.Read())
        //            {
        //                status = reader.GetInt32(reader.GetOrdinal("ID"));
        //            }
        //        }
        //    }

        // connection.Close();
        //    return status;
        // }
        public static int UpdateSkipQuestion(ExamSkipQuestionDTO skipQuestion)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", skipQuestion.ExamId),
                    new SqlParameter("@QuestionId ", skipQuestion.QuestionId),
                    new SqlParameter("@ExamAttemptId", skipQuestion.ExamAttemptId),
                    new SqlParameter("@UserId", skipQuestion.UserId),
                    new SqlParameter("@ExamCreatedDate", skipQuestion.ExamCreatedDate),
                    new SqlParameter("@ExamLastAttemptDate", DateTime.Now),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateSkipQuestion", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        public static int UpdateAttemptAnswer(ExamAttemptAnswerDTO attemptAnswer)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ChoiceId", attemptAnswer.ChoiceId),
                    new SqlParameter("@IsRight", attemptAnswer.IsRight),
                    new SqlParameter("@ExamAttemptDate", attemptAnswer.ExamAttemptDate),
                    new SqlParameter("@UserId", attemptAnswer.UserId),
                    new SqlParameter("@QuestionId ", attemptAnswer.QuestionId),
                    new SqlParameter("@ExamAttemptId", attemptAnswer.ExamAttemptId),
                    new SqlParameter("@IsYourAnswer", attemptAnswer.IsYourAnswer),
                    new SqlParameter("@UserExamState", attemptAnswer.UserExamState),
                    new SqlParameter("@RoundNumber", attemptAnswer.RoundNumber),
                    new SqlParameter("@EasinessFactor", attemptAnswer.EasinessFactor),
                    new SqlParameter("@NewEasinessFactor", attemptAnswer.NewEasinessFactor),
                    new SqlParameter("@Quality", attemptAnswer.Quality),
                    new SqlParameter("@Iteration", attemptAnswer.Iteration),
                    new SqlParameter("@SessionId", attemptAnswer.SessionId),
                    //new SqlParameter("@IsDeleted", false),
                    new SqlParameter("@id", SqlDbType.Int),
            };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateAttemptAnswer", arrSqlParameter);
            object objResult = arrSqlParameter[arrSqlParameter.Length - 1];
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        public static int UpdateAttemptAnswer_SR_05Mar24(ExamAttemptAnswerDTO attemptAnswer)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ChoiceId", attemptAnswer.ChoiceId),
                    new SqlParameter("@IsRight", attemptAnswer.IsRight),
                    new SqlParameter("@ExamAttemptDate", attemptAnswer.ExamAttemptDate),
                    new SqlParameter("@UserId", attemptAnswer.UserId),
                    new SqlParameter("@QuestionId ", attemptAnswer.QuestionId),
                    new SqlParameter("@ExamAttemptId", attemptAnswer.ExamAttemptId),
                    new SqlParameter("@IsYourAnswer", attemptAnswer.IsYourAnswer),
                    new SqlParameter("@UserExamState", attemptAnswer.UserExamState),
                    new SqlParameter("@RoundNumber", attemptAnswer.RoundNumber),
                    new SqlParameter("@EasinessFactor", attemptAnswer.EasinessFactor),
                    new SqlParameter("@NewEasinessFactor", attemptAnswer.NewEasinessFactor),
                    new SqlParameter("@Quality", attemptAnswer.Quality),
                    new SqlParameter("@Iteration", attemptAnswer.Iteration),
                    new SqlParameter("@SessionId", attemptAnswer.SessionId),
                    //new SqlParameter("@IsDeleted", false),
                    new SqlParameter("@id", SqlDbType.Int),
            };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateAttemptAnswer_SR_05Mar24", arrSqlParameter);
            object objResult = arrSqlParameter[arrSqlParameter.Length - 1];
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        public static QuestionDiscussion GetQuestionDiscussion(int questionId, SqlConnection gConnection = null)
        {
            QuestionDiscussion questionDiscussion = new QuestionDiscussion();
            SqlConnection connection = gConnection;
            if (gConnection == null)
            {
                connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
                connection.Open();
            }

            /*string sqlQueryGetDiscussion = string.Empty;
            sqlQueryGetDiscussion = "Select Discussion_id As ID, Discussion_text as discussion_text, Discussion_type as discussion_type FROM Question_Discussion WHERE QuestionId= " + @questionId;
            SqlCommand cmd = new SqlCommand(sqlQueryGetDiscussion, connection);*/

            SqlCommand cmd = new SqlCommand("AAO_GetQuestionDiscussion", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@questionId", questionId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        object objID = reader["ID"];
                        questionDiscussion.discussionId = objID is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
                        object objDiscussionText = reader["discussion_text"];
                        questionDiscussion.discussionText = objDiscussionText is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("discussion_text"));
                        object objDiscussiontype = reader["discussion_type"];
                        questionDiscussion.discussionType = objDiscussiontype is DBNull ? false : reader.GetBoolean(reader.GetOrdinal("discussion_type"));
                        questionDiscussion.discussionImage = GetDiscussionImageList(questionDiscussion.discussionId);
                    }

                    reader.Close();
                }
            }
            if (gConnection == null)
            {
                connection.Close();
            }
            return questionDiscussion;
        }

        public static List<DiscussionImage> GetDiscussionImageList(int discussionId)
        {
            List<DiscussionImage> discussionImageList = new List<DiscussionImage>();
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string sqlQueryGetDiscussionImage = string.Empty;
            sqlQueryGetDiscussionImage = "Select * FROM Discussion_Image WHERE Discussion_id = " + @discussionId + " ORDER BY Figure ASC";
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlQueryGetDiscussionImage, connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        DiscussionImage discussionImage = new DiscussionImage();
                        object objDiscussionImageId = reader["Discussion_Image_id"];
                        discussionImage.discussionImageId = objDiscussionImageId is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("Discussion_Image_id"));
                        object objCaption = reader["Caption"];
                        discussionImage.Caption = objCaption is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Caption"));
                        object objComments = reader["Comments"];
                        discussionImage.Comments = objComments is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Comments"));
                        object objCredit = reader["Credit"];
                        discussionImage.Credit = objCredit is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Credit"));
                        object objFigure = reader["Figure"];
                        discussionImage.Figure = objFigure is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Figure"));
                        object objImagePath = reader["ImagePath"];
                        discussionImage.ImagePath = objImagePath is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("ImagePath"));
                        object objSource = reader["Source"];
                        discussionImage.Source = objSource is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Source"));
                        object objTitle = reader["Title"];
                        discussionImage.Title = objTitle is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("Title"));
                        object objChoiceId = reader["Discussion_id"];
                        discussionImage.discussionid = objChoiceId is DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("Discussion_id"));
                        discussionImageList.Add(discussionImage);
                    }

                    reader.Close();
                }
            }

            connection.Close();
            return discussionImageList;
        }

        public static List<string> GetReferences(int questionId, SqlConnection gConnection=null)
        {
            List<string> referencesList = new List<string>();
            SqlConnection connection = gConnection;
            if (gConnection == null)
            {
                connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
                connection.Open();
            }

            /*string sqlQueryReference = string.Empty;
            sqlQueryReference = "Select Question_References As reference  FROM Question WHERE QuestionId= " + @questionId + " AND IsDeleted = 0 ";
            SqlCommand cmd = new SqlCommand(sqlQueryReference, connection);*/

            SqlCommand cmd = new SqlCommand("AAO_GetReferences", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@questionId ", questionId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        string refertences = string.Empty;
                        object referenceObj = reader["reference"];
                        refertences = referenceObj is DBNull ? string.Empty : reader.GetString(reader.GetOrdinal("reference"));
                        referencesList.Add(refertences);
                    }

                    reader.Close();
                }
            }

            if (gConnection == null)
            {
                connection.Close();
            }

            return referencesList;
        }

        public static List<string> GetNotes(int questionId, int userId, int examId, SqlConnection  gConnection=null)
        {
            List<string> notesList = new List<string>();
            SqlConnection connection = gConnection;
            if (gConnection == null)
            {
                connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
                connection.Open();
            }

            /*string sqlQueryAllquestion = string.Empty;
            if (examId == 0 && examId != null)
            {
                sqlQueryAllquestion = "Select ExamAttemptNoteText As notes  FROM ExamNotes WHERE QuestionId= " + questionId + " and UserId= " + userId + " and IsDeleted = 0";
            }
            else
            {
                sqlQueryAllquestion = "Select ExamAttemptNoteText As notes  FROM ExamNotes WHERE QuestionId= " + questionId + " and UserId= " + userId + " and IsDeleted = 0 and ExamId= " + examId;
            }

            SqlCommand cmd = new SqlCommand(sqlQueryAllquestion, connection);*/

            SqlCommand cmd = new SqlCommand("AAO_GetNotes", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@questionId", questionId));
            cmd.Parameters.Add(new SqlParameter("@userId", userId));
            cmd.Parameters.Add(new SqlParameter("@examId", examId));

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
            if (gConnection == null)
            {
                connection.Close();
            }
            if (notesList.Count == 0)
            {
                notesList.Add("You have not created a note for this question.");
            }
            return notesList;
        }

        public static List<string> GetRelatedContent(int questionId, SqlConnection gConnection = null)
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

        public static ExamDTO GetExamBYId(int? examId)
        {
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                                             };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetExamList", objSqlParameter))
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
                    examObj.ExamType = Convert.ToInt32(objSqlDataReader["ExamType"]);
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

                    object objExamType = objSqlDataReader["ExamType"];
                    examObj.ExamType = objExamType is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ExamType"]);
                }

                objSqlDataReader.Close();
                return examObj;
            }
        }

        public static ExamAttemptAnswerDTO GetExamAttemptAnswer(int questionId, int examAttemptId, SqlConnection gConnection=null)
        {
            ExamAttemptAnswerDTO examAttemptAnswer = new ExamAttemptAnswerDTO();
            SqlConnection connection = gConnection;
            if (gConnection == null)
            {
                connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
                connection.Open();
            }

            /*string sqlQueryGetExamAttemptAnswer = string.Empty;
            sqlQueryGetExamAttemptAnswer = "Select * FROM ExamAttemptAsnwer WHERE QuestionId= " + @questionId + " and ExamAttemptId= " + @examAttemptId;
            SqlCommand cmd = new SqlCommand(sqlQueryGetExamAttemptAnswer, connection);*/

            SqlCommand cmd = new SqlCommand("AAO_GetExamAttemptAnswer", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@questionId", questionId));
            cmd.Parameters.Add(new SqlParameter("@examAttemptId", examAttemptId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        examAttemptAnswer.ExamAttemptAsnwerId = reader.GetInt32(reader.GetOrdinal("ExamAttemptAsnwerId"));
                        examAttemptAnswer.ExamAttemptDate = reader.GetDateTime(reader.GetOrdinal("ExamAttemptDate"));
                        examAttemptAnswer.ChoiceId = reader.GetInt32(reader.GetOrdinal("ChoiceId"));
                        examAttemptAnswer.IsRight = reader.GetBoolean(reader.GetOrdinal("IsRight"));
                        examAttemptAnswer.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
                        examAttemptAnswer.QuestionId = reader.GetInt32(reader.GetOrdinal("QuestionId"));
                        examAttemptAnswer.ExamAttemptId = reader.GetInt32(reader.GetOrdinal("ExamAttemptId"));
                        examAttemptAnswer.IsYourAnswer = reader.GetInt32(reader.GetOrdinal("IsYourAnswer"));
                    }

                    reader.Close();
                }
            }
            if (gConnection == null)
            {
                connection.Close();
            }

            return examAttemptAnswer;
        }

        public static void AddExamAttemptText(UpdateSkipAnswered notesAnswer)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamAttemptAnswerText", notesAnswer.examAttemptText),
                    new SqlParameter("@ExamCreatedDate ", DateTime.Now),
                    new SqlParameter("@ExamLastAttemptDate", DateTime.Now),
                    new SqlParameter("@ExamId", notesAnswer.examId),
                    new SqlParameter("@UserId", notesAnswer.userId),
                    new SqlParameter("@QuestionId", notesAnswer.currentQuestionId),
                    new SqlParameter("@ExamAttemptId", Convert.ToInt32(notesAnswer.examAttemptId)),
                   };
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_AddAttemptAnswerText", arrSqlParameter);
        }

        public static string GetExamAttemptAnswerText(int? examId, int? questionId)
        {
            string examAttemptAnswerText = string.Empty;
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                    new SqlParameter("@QuestionId", Convert.ToInt32(questionId)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetExamAttemptAnswerText", arrSqlParameter))
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

        public static void AddExamQuestionNotes(UpdateSkipAnswered notesAnswer)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamAttemptNoteText", notesAnswer.examAttemptText),
                    new SqlParameter("@ExamCreatedDate ", DateTime.Now),
                    new SqlParameter("@ExamLastAttemptDate", DateTime.Now),
                    new SqlParameter("@ExamId", notesAnswer.examId),
                    new SqlParameter("@UserId", notesAnswer.userId),
                    new SqlParameter("@QuestionId", notesAnswer.currentQuestionId),
                    new SqlParameter("@ExamAttemptId", Convert.ToInt32(notesAnswer.examAttemptId)),
                   };
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_AddExamQuestionNotes", arrSqlParameter);
        }

        public static bool GetMarkStatus(int? examId, int? questionId, int userId)
        {
            bool markStatus = false;
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", Convert.ToInt32(examId)),
                    new SqlParameter("@QuestionId", Convert.ToInt32(questionId)),
                    new SqlParameter("@UserId", Convert.ToInt32(userId)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetMarkStatus", arrSqlParameter))
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

        public static bool AddOrDeleteMarkedQuestion(UpdateSkipAnswered updateMark)
        {
            // string examAttemptAnswerText = string.Empty;
            bool markStatus = false;
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamCreatedDate ", DateTime.Now),
                    new SqlParameter("@ExamLastAttemptDate", DateTime.Now),
                    new SqlParameter("@ExamId", updateMark.examId),
                    new SqlParameter("@UserId", updateMark.userId),
                    new SqlParameter("@QuestionId", updateMark.currentQuestionId),
                    new SqlParameter("@ExamAttemptId", Convert.ToInt32(updateMark.examAttemptId)),
                    new SqlParameter("@markedStatus", updateMark.markedStatus),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_InsertOrDeleteMark", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object objMarkStatus = objSqlDataReader["ChangedStatus"];
                        markStatus = objMarkStatus is DBNull ? false : Convert.ToBoolean(objSqlDataReader["ChangedStatus"]);
                    }

                    objSqlDataReader.Close();
                }

                return markStatus;
            }
        }

        public static int SetTimeForQuestion(int? attemptId)
        {
            int time = 0;
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamAttemptId", Convert.ToInt32(attemptId)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_SetExamTime", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object objExamTime = objSqlDataReader["ExamTime"];
                        time = objExamTime is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ExamTime"]);
                    }

                    objSqlDataReader.Close();
                }
            }

            return time;
        }

        public static void UpdateConsumedTime(int? attemptId, double consumedTime)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamAttemptId", Convert.ToInt32(attemptId)),
                    new SqlParameter("@ConsumedTime", Convert.ToInt32(consumedTime)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_UpdateExamTime", arrSqlParameter))
            {
            }
        }

        public static int SubmitExam(UpdateSkipAnswered submitExam)
        {
            int result = 0;
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", submitExam.examId),
                    new SqlParameter("@UserId", submitExam.userId),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_SubmitExam", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object objResult = objSqlDataReader["Result"];
                        result = objResult is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Result"]);
                    }

                    objSqlDataReader.Close();
                }
            }

            return result;
        }

        public static List<ViewProgressVM> ViewProgress(UpdateSkipAnswered viewProgressData)
        {
            List<ViewProgressVM> viewProgressList = new List<ViewProgressVM>();
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", viewProgressData.examId),
                    new SqlParameter("@UserId", viewProgressData.userId),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetViewProgressDetails", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    int questionSequences = 0;
                    while (objSqlDataReader.Read())
                    {
                        questionSequences++;
                        ViewProgressVM viewProgress = new ViewProgressVM();
                        viewProgress.QuestionSequence = questionSequences;
                        object objQuestionid = objSqlDataReader["Questionid"];
                        viewProgress.QuestionId = objQuestionid is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Questionid"]);
                        object selectedChoiceObj = objSqlDataReader["user_ANS"];
                        viewProgress.SelectedChoice = selectedChoiceObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["user_ANS"]);
                        object correctChoiceObj = objSqlDataReader["correct_ANS"];
                        viewProgress.CorrectChoice = correctChoiceObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["correct_ANS"]);
                        object isRightObj = objSqlDataReader["isRight"];
                        viewProgress.IsRight = isRightObj is DBNull ? false : Convert.ToBoolean(objSqlDataReader["isRight"]);
                        object markObj = objSqlDataReader["MarkQuestionText"];
                        viewProgress.IsMarked = markObj is DBNull ? false : Convert.ToBoolean(objSqlDataReader["MarkQuestionText"]);
                        object notestObj = objSqlDataReader["ExamAttemptNoteText"];
                        viewProgress.HasNotes = notestObj is DBNull ? false : Convert.ToBoolean(objSqlDataReader["ExamAttemptNoteText"]);
                        object selectedOptionObj = objSqlDataReader["User_Choice"];
                        viewProgress.SelectedOption = selectedOptionObj is DBNull ? 'u' : Convert.ToChar(objSqlDataReader["User_Choice"]);
                        object correctOptionObj = objSqlDataReader["CorrectChoice"];
                        viewProgress.CorrectOption = correctOptionObj is DBNull ? '-' : Convert.ToChar(objSqlDataReader["CorrectChoice"]);
                        viewProgressList.Add(viewProgress);
                    }

                    objSqlDataReader.Close();
                }
            }

            return viewProgressList;
        }

        // Get  PeerDetails against QuestionId
        public static List<PeerDetailVM> GetPeerDetails(int questionId, int userId, int examattemptId)
        {
            List<PeerDetailVM> peerDetailsList = new List<PeerDetailVM>();
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@QuestionId", questionId),
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@ExamAttemptId", examattemptId),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetPeerDetails", arrSqlParameter))
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

        // Get Question Count
        public static int GetQuestionCount(int examId)
        {
            int result = 0;
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", examId),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQuestionCount", arrSqlParameter))
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

        public static void UpdateOrAddIncorrectAnswer(IncorrectQuestionDTO incorrectQuestion)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@QuestionId ", incorrectQuestion.QuestionId),
                    new SqlParameter("@UserId", incorrectQuestion.UserId),
                    new SqlParameter("@ExamAttemptId", incorrectQuestion.ExamAttemptId),
                    new SqlParameter("@IsRight", incorrectQuestion.IsRight),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_UpdateOrAddIncorrectQuestion", arrSqlParameter))
            {
            }
        }

        public static void SaveIsYourAnswer(UpdateSkipAnswered isYourAnswer)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@QuestionId ", isYourAnswer.currentQuestionId),
                    new SqlParameter("@UserId", isYourAnswer.userId),
                    new SqlParameter("@ExamAttemptId", Convert.ToInt32(isYourAnswer.examAttemptId)),
                    new SqlParameter("@IsYourAnswer", isYourAnswer.isYourAnswer),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_UpdateIsYourAnswer", arrSqlParameter))
            {
            }
        }

        // Get BCSCExcerpt Text
        public static List<string> GetBCSCExcerptText(int questionId)
        {
            List<string> content = new List<string>();
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@QuestionId", questionId),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetBCSCExcerptText", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object objContent = objSqlDataReader["Content"];
                        string contentData = objContent is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Content"]);
                        content.Add(contentData);
                    }

                    objSqlDataReader.Close();
                }
            }

            return content;
        }

        // Get Topic
        public static string GetTopicByQuestion(int questionId)
        {
            List<string> topicContent = new List<string>();
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@QuestionId", questionId),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetTopicByQuestion", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object objTopicName = objSqlDataReader["TopicName"];
                        string topicName = objTopicName is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["TopicName"]);
                        topicContent.Add(topicName);
                    }

                    objSqlDataReader.Close();
                }
            }

            return string.Join<string>(",", topicContent);
        }

        public static void UpdateTime(int? attemptId, double consumedTime)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamAttemptId", Convert.ToInt32(attemptId)),
                    new SqlParameter("@ConsumedTime", Convert.ToInt32(consumedTime)),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_UpdateTime", arrSqlParameter))
            {
            }
        }

        public static QuestionDetails GetQuestionDetails_Search(int? questionId)
        {
            QuestionDetails questionDetail = new QuestionDetails();
            SqlParameter[] arrSqlParameter =
            {
            new SqlParameter("@QuestionId", Convert.ToInt32(questionId)),
           };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQuestionDetails_Search", arrSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    questionDetail.questionId = Convert.ToInt32(objSqlDataReader["questionid"]);
                    object stemObj = objSqlDataReader["Stem"];
                    questionDetail.questionText = stemObj is DBNull ? null : Convert.ToString(objSqlDataReader["Stem"]);
                    object friendlyidobj = objSqlDataReader["Friendlyid"];
                    questionDetail.FriendlyId = friendlyidobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Friendlyid"]);
                }

                objSqlDataReader.Close();

                List<QuestionImage> questionImageList = new List<QuestionImage>();
                questionImageList = GetQuestionImageList(questionId);
                questionDetail.questionImage = questionImageList;
                return questionDetail;
            }
        }

        public static ExamAttemptAnswerDTO GetExamAttemptAnswer_Search(int questionId, int userId)
        {
            ExamAttemptAnswerDTO examAttemptAnswer = new ExamAttemptAnswerDTO();
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            /*string sqlQueryGetExamAttemptAnswer = string.Empty;
            sqlQueryGetExamAttemptAnswer = "SELECT TOP (1) * from ExamAttemptAsnwer where QuestionId=" + @questionId + " and UserId= " + @userId + " ORDER BY ExamAttemptDate DESC";
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlQueryGetExamAttemptAnswer, connection);*/

            connection.Open();
            SqlCommand cmd = new SqlCommand("AAO_GetExamAttemptAnswer_Search", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@questionId ", questionId));
            cmd.Parameters.Add(new SqlParameter("@userId ", userId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        examAttemptAnswer.ExamAttemptAsnwerId = reader.GetInt32(reader.GetOrdinal("ExamAttemptAsnwerId"));
                        examAttemptAnswer.ExamAttemptDate = reader.GetDateTime(reader.GetOrdinal("ExamAttemptDate"));
                        examAttemptAnswer.ChoiceId = reader.GetInt32(reader.GetOrdinal("ChoiceId"));
                        examAttemptAnswer.IsRight = reader.GetBoolean(reader.GetOrdinal("IsRight"));
                        examAttemptAnswer.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
                        examAttemptAnswer.QuestionId = reader.GetInt32(reader.GetOrdinal("QuestionId"));
                        examAttemptAnswer.ExamAttemptId = reader.GetInt32(reader.GetOrdinal("ExamAttemptId"));
                        examAttemptAnswer.IsYourAnswer = reader.GetInt32(reader.GetOrdinal("IsYourAnswer"));
                    }

                    reader.Close();
                }
                else
                {
                    examAttemptAnswer.ExamAttemptId = 0;
                }
            }

            connection.Close();
            return examAttemptAnswer;
        }

        public static int UpdateDeleteTopic(ExamAttemptAnswerDTO attemptAnswer)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ChoiceId", attemptAnswer.ChoiceId),
                    new SqlParameter("@IsRight", attemptAnswer.IsRight),
                    new SqlParameter("@ExamAttemptDate", attemptAnswer.ExamAttemptDate),
                    new SqlParameter("@UserId", attemptAnswer.UserId),
                    new SqlParameter("@QuestionId ", attemptAnswer.QuestionId),
                    new SqlParameter("@ExamAttemptId", attemptAnswer.ExamAttemptId),

                    new SqlParameter("@id", SqlDbType.Int),
            };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateAttemptAnswer", arrSqlParameter);
            object objResult = arrSqlParameter[arrSqlParameter.Length - 1];
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }
        public static void UpdateDeleteTopic(SRRecommendedDeleteVM attemptAnswer)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@ExamId", attemptAnswer.ExamId),
                    new SqlParameter("@IsRight", attemptAnswer.IsRight),
                    new SqlParameter("@UserId", attemptAnswer.UserId),
                    new SqlParameter("@QuestionId ", attemptAnswer.QuestionId),
                    //new SqlParameter("@id", SqlDbType.Int),
            };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_UpdateDelTopicTable", arrSqlParameter))
            {
            }
            //arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            //SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateDelTopicTable", arrSqlParameter);
            //object objResult = arrSqlParameter[arrSqlParameter.Length - 1];
            //return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        public static int GetAttemptedQuestionCount(int userId, int sessionId, int examAttemptId)
        {
            int count = 0;
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@SessionId", sessionId),
                    new SqlParameter("@ExamAttemptId", examAttemptId),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAttemptedQuestionCount", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object objContent = objSqlDataReader["Count"];
                        count = objContent is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Count"]);

                    }

                    objSqlDataReader.Close();
                }
            }

            return count;
        }

        public static int GetNewEasynessFactor(int userId, int questionId, int examAttemptId)
        {
            int newEasinessFactor = 0;
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@QuestionId", questionId),
                    new SqlParameter("@ExamAttemptId", examAttemptId),
                   };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetEasinessFactor", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        object objContent = objSqlDataReader["NewEasinessFactor"];
                        newEasinessFactor = objContent is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["NewEasinessFactor"]);

                    }

                    objSqlDataReader.Close();
                }
            }

            return newEasinessFactor;
        }
    }
}
