namespace AAO.DAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;

    public static class ImportDAL
    {
        // This Method will insert data into Question Table And return QuestionId
        public static int SaveQuestionData(QuestionVM questioData)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@QuestionType", Convert.ToBoolean(questioData.QuestionType)),
                    new SqlParameter("@Question_QAS_id ", questioData.Id),
                    new SqlParameter("@Question_References", questioData.References),
                    new SqlParameter("@Status", Convert.ToInt32(questioData.Status)),
                    new SqlParameter("@Stem", questioData.Stem),
                    new SqlParameter("@Friendlyid", questioData.FriendlyId),
                    new SqlParameter("@IsActive", Convert.ToBoolean(questioData.Active)),
                    new SqlParameter("@createdon", questioData.Createdon),
                    new SqlParameter("@modifiedon", questioData.ModifiedOn),
                    new SqlParameter("@IsDeleted", Convert.ToBoolean(questioData.Deleted)),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_SaveQuestionData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will insert data into Images Table accordint to passed table name  And return ImageId
        public static int InsertImageData(ImageVM image, int referenceId, string tableName)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@TableName", tableName),
                    new SqlParameter("@Caption", image.Caption),
                    new SqlParameter("@Comments", image.Comments),
                    new SqlParameter("@Credit", image.Credit),
                    new SqlParameter("@Figure", image.Figure),
                    new SqlParameter("@ImagePath", image.Path),
                    new SqlParameter("@Source", image.Source),
                    new SqlParameter("@Title", image.Title),
                    new SqlParameter("@ReferenceId", referenceId),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_InsertImageData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will insert data into Discussion Table And return DiscussionId
        public static int SaveDiscussionData(DiscussionVM discussionData)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@Discussion_text", discussionData.DiscussionText),
                    new SqlParameter("@QuestionId", discussionData.QuestionId),
                    new SqlParameter("@Discussion_type", discussionData.DiscussionType),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_SaveDiscussionData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will insert data into Choice Table And return ChoiceId
        public static int SaveAnswerData(AnswerVM answerVM)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@IsRightChoice", answerVM.Correct),
                    new SqlParameter("@QuestionId", answerVM.QuestionId),
                    new SqlParameter("@ChoiceText", answerVM.Text),
                    new SqlParameter("@ChoiceSequence", answerVM.Sequence),
                    new SqlParameter("@AnswerType", answerVM.AnswerType),
                    new SqlParameter("@QAS_Choice_id", answerVM.Id),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_SaveAnswerData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will insert data into Topic Table And return TopicId
        public static int SaveTopicData(TopicContentVM topicContent)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@QAS_topicid", topicContent.TopicId),
                    new SqlParameter("@code", topicContent.Code),
                    new SqlParameter("@deleted", Convert.ToBoolean(topicContent.Deleted)),
                    new SqlParameter("@modifiedon", topicContent.ModifiedOn),
                    new SqlParameter("@title", topicContent.Title),
                    new SqlParameter("@chapter", topicContent.Chapter),
                    new SqlParameter("@chapterName", topicContent.ChapterName),
                    new SqlParameter("@Topic", topicContent.Topic),
                    new SqlParameter("@TopicName", topicContent.TopicName),
                    new SqlParameter("@IsActive", Convert.ToBoolean(topicContent.Active)),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_SaveTopicData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will insert data into QuestionTopicXwalk Table And return QuestionTopicXwalkId
        public static int SaveQuestionTopicXWalkData(QuestionTopicXWalkVM questionTopicXWalk)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@QuestionId", questionTopicXWalk.QuestionId),
                    new SqlParameter("@QAS_question_id", questionTopicXWalk.QASQuestionId),
                    new SqlParameter("@QAS_topicId", questionTopicXWalk.QASTopicId),
                    new SqlParameter("@TopicId", questionTopicXWalk.TopicId),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_SaveQuestionTopicXWalkData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will insert data into Program Table And return ProgramId
        public static int SaveProgramData(ProgramVM program)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@program", program.Program),
                    new SqlParameter("@QAS_programid", program.QASProgramid),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_SaveProgramData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will insert data into SubProgram Table And return SubProgramId
        public static int SaveSubProgramData(SubProgramVM subProgram)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@subprogram", subProgram.Subprogram),
                    new SqlParameter("@QAS_subprogramid", subProgram.QASSubprogramid),
                    new SqlParameter("@ProgramId", subProgram.ProgramId),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_SaveSubProgramData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will insert data into SubSpeciality Table And return SubSpecialityId
        public static int SaveSubSpecialityData(SubProgramVM subProgram)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@subprogram", subProgram.Subprogram),
                    new SqlParameter("@QAS_subprogramid", subProgram.QASSubprogramid),
                    new SqlParameter("@ProgramId", subProgram.ProgramId),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_SaveSubProgramData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will insert data into SubSpeciality Table And return SubSpecialityId
        public static int SaveSubSpecialityData(SubSpecialityVM subSpeciality)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@Subspecialty", subSpeciality.Subspecialty),
                    new SqlParameter("@QAS_subspecialtyid", subSpeciality.QASSubspecialtyid),
                    new SqlParameter("@SubprogramId", subSpeciality.SubprogramId),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_SaveSubSpecialityData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will insert data into SubspecialtyTopicXWalk Table And return SubspecialtyTopicXWalkId
        public static int SaveSubspecialtyTopicXWalkData(SubspecialtyTopicXWalkVM subspecialtyTopicXWalk)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@SubspecialtyId", subspecialtyTopicXWalk.SubSpecialityId),
                    new SqlParameter("@TopicId", subspecialtyTopicXWalk.TopicId),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_SaveSubspecialtyTopicXWalkVMData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // Retired Operation For QAS DateBase Start from Here
        // This Method will Retire Question into Question Table
        public static int RetiredQuestionData(QuestionVM questioData)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@Question_QAS_id ", questioData.Id),
                    new SqlParameter("@Status", Convert.ToInt32(questioData.Status)),
                    new SqlParameter("@IsActive", Convert.ToBoolean(questioData.Active)),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_RetiredQuestionData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // Update Operation For QAS DateBase Start from Here
        // This Method will Update data into Question Table And return QuestionId
        public static int UpdateQuestionData(QuestionVM questioData)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@QuestionType", Convert.ToBoolean(questioData.QuestionType)),
                    new SqlParameter("@Question_QAS_id ", questioData.Id),
                    new SqlParameter("@Question_References", questioData.References),
                    new SqlParameter("@Status", Convert.ToInt32(questioData.Status)),
                    new SqlParameter("@Stem", questioData.Stem),
                    new SqlParameter("@Friendlyid", questioData.FriendlyId),
                    new SqlParameter("@IsActive", Convert.ToBoolean(questioData.Active)),
                    new SqlParameter("@createdon", questioData.Createdon),
                    new SqlParameter("@modifiedon", questioData.ModifiedOn),
                    new SqlParameter("@IsDeleted", Convert.ToBoolean(questioData.Deleted)),
                    new SqlParameter("@NotRandomizeAnswers", Convert.ToBoolean(questioData.NotRandomizeAnswers)),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateQuestionData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        //call SP (AAO_UpdateorInsertRelatedContents) for update or insert Contents
        public static int UpdateorInsertRelatedContents(QuestionVM questioData, int questionId)
        {
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@Friendlyid", questioData.FriendlyId),
                     new SqlParameter("@QuestionId", questionId),
                     new SqlParameter("@Question_qas_Id", questioData.Id),
                     new SqlParameter("@RelatedContents", Convert.ToString(questioData.RelatedContents)),
                     new SqlParameter("@Que_createdon", questioData.Createdon),
                     new SqlParameter("@Que_modifiedon", questioData.ModifiedOn),
                     new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateorInsertRelatedContents", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will Update data into Topic Table And return Topic
        public static int UpdateTopicData(TopicContentVM topicContent)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@QAS_topicid", topicContent.TopicId),
                    new SqlParameter("@code", topicContent.Code),
                    new SqlParameter("@deleted", Convert.ToBoolean(topicContent.Deleted)),
                    new SqlParameter("@modifiedon", topicContent.ModifiedOn),
                    new SqlParameter("@title", topicContent.Title),
                    new SqlParameter("@chapter", topicContent.Chapter),
                    new SqlParameter("@chapterName", topicContent.ChapterName),
                    new SqlParameter("@Topic", topicContent.Topic),
                    new SqlParameter("@TopicName", topicContent.TopicName),
                    new SqlParameter("@IsActive", Convert.ToBoolean(topicContent.Active)),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateTopicData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will Update data into QuestionTopicXwalk Table And return QuestionTopicXwalkId
        public static int UpdateQuestionTopicXWalkData(QuestionTopicXWalkVM questionTopicXWalk)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@QuestionId", questionTopicXWalk.QuestionId),
                    new SqlParameter("@QAS_question_id", questionTopicXWalk.QASQuestionId),
                    new SqlParameter("@QAS_topicId", questionTopicXWalk.QASTopicId),
                    new SqlParameter("@TopicId", questionTopicXWalk.TopicId),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateQuestionTopicXWalkData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will Update data into Program Table And return ProgramId
        public static int UpdateProgramData(ProgramVM program)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@program", program.Program),
                    new SqlParameter("@QAS_programid", program.QASProgramid),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateProgramData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will Update data into SubProgram Table And return SubProgramId
        public static int UpdateSubProgramData(SubProgramVM subProgram)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@subprogram", subProgram.Subprogram),
                    new SqlParameter("@QAS_subprogramid", subProgram.QASSubprogramid),
                    new SqlParameter("@ProgramId", subProgram.ProgramId),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateSubProgramData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will Update data into SubSpeciality Table And return SubSpecialityId
        public static int UpdateSubSpecialityData(SubSpecialityVM subSpeciality)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@Subspecialty", subSpeciality.Subspecialty),
                    new SqlParameter("@QAS_subspecialtyid", subSpeciality.QASSubspecialtyid),
                    new SqlParameter("@SubprogramId", subSpeciality.SubprogramId),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateSubSpecialityData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will Update data into SubspecialtyTopicXWalk Table And return SubspecialtyTopicXWalkId
        public static int UpdateSubspecialtyTopicXWalkData(SubspecialtyTopicXWalkVM subspecialtyTopicXWalk)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@SubspecialtyId", subspecialtyTopicXWalk.SubSpecialityId),
                    new SqlParameter("@TopicId", subspecialtyTopicXWalk.TopicId),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateSubspecialtyTopicXWalkVMData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will Update data into Discussion Table And return DiscussionId
        public static int UpdateDiscussionData(DiscussionVM discussionData)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@Discussion_text", discussionData.DiscussionText),
                    new SqlParameter("@QuestionId", discussionData.QuestionId),
                    new SqlParameter("@Discussion_type", discussionData.DiscussionType),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateDiscussionData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will Update data into Choice Table And return ChoiceId
        public static int UpdateAnswerData(AnswerVM answerVM)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@IsRightChoice", Convert.ToBoolean(answerVM.Correct)),
                    new SqlParameter("@QuestionId", answerVM.QuestionId),
                    new SqlParameter("@ChoiceText", answerVM.Text),
                    new SqlParameter("@ChoiceSequence", Convert.ToInt32(answerVM.Sequence)),
                    new SqlParameter("@AnswerType", answerVM.AnswerType),
                    new SqlParameter("@QAS_Choice_id", answerVM.Id),
                    new SqlParameter("@id", SqlDbType.Int),
                   };
            arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateAnswerData", arrSqlParameter);
            return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
        }

        // This Method will Delete data From Image Table
        public static void DeleteReferenceData(int referenceId, string tableName)
        {
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@ReferenceId", referenceId),
                     new SqlParameter("@TableName", tableName),
                   };

            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_DeleteReferenceData", arrSqlParameter);
        }

        public static int GetQuestionCount(ServiceCallVM serviceCall)
        {
            int questionCount = 0;
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@Status", serviceCall.Status),
                   };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_QuestionCount", arrSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    object countObj = objSqlDataReader["QuestionCount"];
                    questionCount = countObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionCount"]);
                }

                objSqlDataReader.Close();
            }

            return questionCount;
        }

        // This Method will Delete data From ExamAttemptAsnwer,ExamSkipQuestion,MarkQuestion,ExamQuestion,IncorrectQuestion Table
        public static void RetiredQuestionFromRelatedTable(int questionId)
        {
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@QuestionId", questionId),
                   };
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_DeleteRetiredQuestionData", arrSqlParameter);
        }

        public static List<int> GetQuestionList()
        {
            List<int> questionList = new List<int>();
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string sqlQueryAllquestion = string.Empty;
            sqlQueryAllquestion = "Select QuestionId As ID FROM Question";
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlQueryAllquestion, connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        int questionId;
                        object objID = reader["ID"];
                        if (objID is DBNull)
                        {
                            questionId = 0;
                        }
                        else
                        {
                            questionId = reader.GetInt32(reader.GetOrdinal("ID"));
                        }

                        questionList.Add(questionId);
                    }

                    reader.Close();
                }
            }

            connection.Close();
            return questionList;
            }

        public static List<BCSCContentVM> GetBCSCSectionChapterTopicDetails(int questionId)
        {
            List<BCSCContentVM> bCSCContents = new List<BCSCContentVM>();
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@QuestionId", questionId),
                   };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetBCSCSectionChapterTopicDetails", arrSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    BCSCContentVM bCSCContent = new BCSCContentVM();

                    object questionIdObj = objSqlDataReader["QuestionId"];
                    bCSCContent.QuestionId = questionIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionId"]);

                    object subspecialtyIdObj = objSqlDataReader["SubspecialtyId"];
                    bCSCContent.SubSpecialityId = subspecialtyIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);

                    object bCSCSectionNumberObj = objSqlDataReader["BCSCSectionNumber"];
                    bCSCContent.BCSCSectionNumber = bCSCSectionNumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["BCSCSectionNumber"]);

                    object bCSCSectionTitleObj = objSqlDataReader["BCSCSectionTitle"];
                    bCSCContent.BCSCSectionTitle = bCSCSectionTitleObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["BCSCSectionTitle"]);

                    object chapterObj = objSqlDataReader["chapter"];
                    bCSCContent.Chapter = questionIdObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["chapter"]);

                    object chapterNameObj = objSqlDataReader["chapterName"];
                    bCSCContent.ChapterName = chapterNameObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["chapterName"]);

                    object topicObj = objSqlDataReader["TopicId"];
                    bCSCContent.TopicId = topicObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["TopicId"]);

                    object topicNameObj = objSqlDataReader["TopicName"];
                    bCSCContent.TopicName = topicNameObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["TopicName"]);
                    bCSCContents.Add(bCSCContent);
                }

                objSqlDataReader.Close();
            }

            return bCSCContents;
        }

        public static void InsertContentInBCSCMapper(BCSCContentVM bCSCContent)
        {
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@QuestionId", bCSCContent.QuestionId),
                     new SqlParameter("@SubspecialtyId", bCSCContent.SubSpecialityId),
                     new SqlParameter("@Chapter", bCSCContent.ChapterName),
                     new SqlParameter("@TopicId", bCSCContent.TopicId),
                     new SqlParameter("@Content", bCSCContent.Content),
                     new SqlParameter("@BCSCWordCount", bCSCContent.BCSCbookWordCount),
                   };
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_InsertBCSCContentFromHTML", arrSqlParameter);
        }

        public static void DeleteContentFromBCSCMapper(int question, int topicId)
        {
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@QuestionId", question),
                     new SqlParameter("@TopicId", topicId),
                   };
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_DeleteBCSCContentOnUpdate", arrSqlParameter);
        }

        public static void DeleteContentFromBCSCMapperOnRetired(int question)
        {
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@QuestionId", question),
                   };
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_DeleteBCSCContentOnRetire", arrSqlParameter);
        }

        public static List<int> GetQASTopicIdByQuestionId(int? questionId)
        {
            List<int> dBtopicIdList = new List<int>();

            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@QuestionId", questionId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQASTopicIdByQuestionId", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    int dbToipicId = Convert.ToInt32(objSqlDataReader["TopicId"]);
                    dBtopicIdList.Add(dbToipicId);
                }

                objSqlDataReader.Close();
            }

            return dBtopicIdList;
        }

        public static void UpdateBCSCMapperContentField(BCSCContentVM bCSCContent)
        {
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@QuestionId", bCSCContent.QuestionId),
                     new SqlParameter("@SubspecialtyId", bCSCContent.SubSpecialityId),
                     new SqlParameter("@Chapter", bCSCContent.ChapterName),
                     new SqlParameter("@TopicId", bCSCContent.TopicId),
                     new SqlParameter("@Content", bCSCContent.Content),
                     new SqlParameter("@BCSCWordCount", bCSCContent.BCSCbookWordCount),
                   };
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateBCSCMapperContentField", arrSqlParameter);
        }

        public static void InsertContentInBCSCMapperIfNotInDb(BCSCContentVM bCSCContent)
        {
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@QuestionId", bCSCContent.QuestionId),
                     new SqlParameter("@SubspecialtyId", bCSCContent.SubSpecialityId),
                     new SqlParameter("@Chapter", bCSCContent.ChapterName),
                     new SqlParameter("@TopicId", bCSCContent.TopicId),
                     new SqlParameter("@Content", bCSCContent.Content),
                     new SqlParameter("@BCSCWordCount", bCSCContent.BCSCbookWordCount),
                   };
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_InsertContentInBCSCMapperIfNotInDb", arrSqlParameter);
        }

        public static void UpdateBCSCMapperContentOnBookChange(BCSCContentVM bCSCContent)
        {
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@QuestionId", bCSCContent.QuestionId),
                     new SqlParameter("@SubspecialtyId", bCSCContent.SubSpecialityId),
                     new SqlParameter("@Chapter", bCSCContent.ChapterName),
                     new SqlParameter("@TopicId", bCSCContent.TopicId),
                     new SqlParameter("@Content", bCSCContent.Content),
                     new SqlParameter("@BCSCWordCount", bCSCContent.BCSCbookWordCount),
                   };
            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateBCSCMapperContentOnBookChangeFromHTML", arrSqlParameter);
        }

        public static void DeleteOldTopicData()
        {
            SqlParameter[] arrSqlParameter =

                {};

            SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_Delete_Old_Topics ", arrSqlParameter);
        }
    }
}
