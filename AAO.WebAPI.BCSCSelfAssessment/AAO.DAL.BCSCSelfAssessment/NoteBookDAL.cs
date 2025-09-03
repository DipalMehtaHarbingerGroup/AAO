namespace AAO.DAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Common.BCSCSelfAssessment;
    using SQLHelper.BCSCSelfAssessment;
    using SQLHelper.BCSCSelfAssessment.DataAccessProvider;

    public static class NoteBookDAL
    {
        public static List<NoteSections> GetNotesSection()
        {
            List<NoteSections> noteSectionsList = new List<NoteSections>();

            SqlParameter[] objSqlParameter =
            {
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetNotesSection", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    NoteSections noteSections = new NoteSections();

                    object subspecialtyIdObj = objSqlDataReader["SubspecialtyId"];
                    noteSections.SubSpecialtyId = subspecialtyIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);

                    object subspecialtyObj = objSqlDataReader["Subspecialty"];
                    noteSections.SubSpecialty = subspecialtyObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Subspecialty"]);

                    object bCSCSectionNumberObj = objSqlDataReader["BCSCSectionNumber"];
                    noteSections.BCSCSectionNumber = bCSCSectionNumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["BCSCSectionNumber"]);

                    object bCSCSectionTitleObj = objSqlDataReader["BCSCSectionTitle"];
                    noteSections.BCSCSectionTitle = bCSCSectionTitleObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["BCSCSectionTitle"]);

                    noteSectionsList.Add(noteSections);
                }

                objSqlDataReader.Close();
            }

            return noteSectionsList;
        }

        public static NoteSections GetSectionOnSubspecilty(int subSpecialtyId)
        {
      NoteSections noteSectionsList = new NoteSections();

      SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@SubspecialtyId", Convert.ToInt32(subSpecialtyId)),
                                             };

      using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetSectionOnFilter", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    object subspecialtyIdObj = objSqlDataReader["SubspecialtyId"];
                    noteSectionsList.SubSpecialtyId = subspecialtyIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);

                    object subspecialtyObj = objSqlDataReader["Subspecialty"];
                    noteSectionsList.SubSpecialty = subspecialtyObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Subspecialty"]);

                    object bCSCSectionNumberObj = objSqlDataReader["BCSCSectionNumber"];
                    noteSectionsList.BCSCSectionNumber = bCSCSectionNumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["BCSCSectionNumber"]);

                    object bCSCSectionTitleObj = objSqlDataReader["BCSCSectionTitle"];
                    noteSectionsList.BCSCSectionTitle = bCSCSectionTitleObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["BCSCSectionTitle"]);
                }

                objSqlDataReader.Close();
            }

      return noteSectionsList;
        }

        public static List<ChapterDetails> GetChapter(int? subSpecialtyId, NoteBookRequest noteBookRequest)
        {
            List<ChapterDetails> chapterList = new List<ChapterDetails>();

            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@SubspecialtyId", Convert.ToInt32(subSpecialtyId)),
                new SqlParameter("@PageSize", noteBookRequest.NoOfRecords),
                new SqlParameter("@PageIndex", noteBookRequest.PageNo),
                new SqlParameter("@UserId", Convert.ToInt32(noteBookRequest.UserId)),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetChapterDetailsOnSubSpecialty", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    ChapterDetails chapter = new ChapterDetails();
                    object chapterNameObj = objSqlDataReader["ChapterName"];
                    chapter.ChapterName = chapterNameObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ChapterName"]);

                    object chapterObj = objSqlDataReader["Chapter"];
                    chapter.Chapter = chapterObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Chapter"]);

                    object recordCountObj = objSqlDataReader["RecordCount"];
                    chapter.ChapterCount = recordCountObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["RecordCount"]);

                    object rowNumberObj = objSqlDataReader["RowNumber"];
                    chapter.ChapterSerialNumber = rowNumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["RowNumber"]);

                    chapterList.Add(chapter);
                }

                objSqlDataReader.Close();
            }

            chapterList = chapterList.DistinctBy(i => i.ChapterName).ToList();
            return chapterList;
        }

        public static List<ChapterDetails> GetChapter_withoutPagination(int? subSpecialtyId, NoteBookRequest noteBookRequest)
        {
            List<ChapterDetails> chapterList = new List<ChapterDetails>();

            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@SubspecialtyId", Convert.ToInt32(subSpecialtyId)),
                new SqlParameter("@PageSize", noteBookRequest.NoOfRecords),
                new SqlParameter("@PageIndex", noteBookRequest.PageNo),
                new SqlParameter("@UserId", Convert.ToInt32(noteBookRequest.UserId)),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetChapterDetailsOnSubSpecialty_withoutPagination", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    ChapterDetails chapter = new ChapterDetails();
                    object chapterNameObj = objSqlDataReader["ChapterName"];
                    chapter.ChapterName = chapterNameObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ChapterName"]);

                    object chapterObj = objSqlDataReader["Chapter"];
                    chapter.Chapter = chapterObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Chapter"]);

                    object recordCountObj = objSqlDataReader["RecordCount"];
                    chapter.ChapterCount = recordCountObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["RecordCount"]);

                    object rowNumberObj = objSqlDataReader["RowNumber"];
                    chapter.ChapterSerialNumber = rowNumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["RowNumber"]);

                    chapterList.Add(chapter);
                }

                objSqlDataReader.Close();
            }

            chapterList = chapterList.DistinctBy(i => i.ChapterName).ToList();
            return chapterList;
        }

        public static List<ChapterDetails> GetChapterList(int? subSpecialtyId)
        {
            List<ChapterDetails> chapterList = new List<ChapterDetails>();

            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@SubspecialtyId", Convert.ToInt32(subSpecialtyId)),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetChapterDetails", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    ChapterDetails chapter = new ChapterDetails();
                    object chapterNameObj = objSqlDataReader["ChapterName"];
                    chapter.ChapterName = chapterNameObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ChapterName"]);

                    object chapterObj = objSqlDataReader["Chapter"];
                    chapter.Chapter = chapterObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Chapter"]);

                    chapterList.Add(chapter);
                }

                objSqlDataReader.Close();
            }

            chapterList = chapterList.DistinctBy(i => i.ChapterName).ToList();
            return chapterList;
        }

        public static List<QuestionDetail> GetNotesQuestion(int? subSpecialityId, string chapterName, int userId)
        {
            List<QuestionDetail> notesDetailList = new List<QuestionDetail>();

            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@SubspecialtyId", Convert.ToInt32(subSpecialityId)),
                new SqlParameter("@ChapterName", chapterName),
                new SqlParameter("@UserId", Convert.ToInt32(userId)),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetNotesQuestionDetails", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    QuestionDetail notesDetail = new QuestionDetail();
                    object questionIdObj = objSqlDataReader["QuestionId"];
                    notesDetail.QuestionId = questionIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionId"]);

                    object stemObj = objSqlDataReader["Stem"];
                    notesDetail.Stem = stemObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Stem"]);

                    object topicNameObj = objSqlDataReader["TopicName"];
                    notesDetail.TopicName = topicNameObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["TopicName"]);

                    object topicObj = objSqlDataReader["Topic"];
                    notesDetail.Topic = topicObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Topic"]);
                    notesDetailList.Add(notesDetail);
                }

                objSqlDataReader.Close();
            }

            notesDetailList = notesDetailList.DistinctBy(i => i.QuestionId).ToList();
            notesDetailList = notesDetailList.OrderBy(x => x.Topic).ToList();
            return notesDetailList;
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return enumerable.GroupBy(keySelector).Select(grp => grp.First());
        }

        public static List<Notes> GetNotesList(int questionId,  int userId)
        {
            List<Notes> notesList = new List<Notes>();

            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@QuestionId", Convert.ToInt32(questionId)),
                new SqlParameter("@UserId", Convert.ToInt32(userId)),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetNotesList", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    Notes notes = new Notes();
                    object notesIdObj = objSqlDataReader["notesId"];
                    notes.NotesId = notesIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["notesId"]);

                    object notesObj = objSqlDataReader["notes"];
                    notes.NotesText = notesObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["notes"]);
                    notesList.Add(notes);
                }

                objSqlDataReader.Close();
            }

            return notesList;
        }

        public static List<Choice> GetChoiceList(int questionId)
        {
            List<Choice> choiceList = new List<Choice>();

            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@QuestionId", Convert.ToInt32(questionId)),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetChoiceList", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    Choice choice = new Choice();
                    object iDObj = objSqlDataReader["ID"];
                    choice.choiceId = iDObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ID"]);
                    object textObj = objSqlDataReader["Text"];
                    choice.choiceText = textObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Text"]);
                    choiceList.Add(choice);
                }

                objSqlDataReader.Close();
            }

            return choiceList;
        }

        public static List<Notes> AddNotes(NotesRequest notesRequest)
        {
            List<Notes> notesList = new List<Notes>();

            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@QuestionId", Convert.ToInt32(notesRequest.QuestionId)),
                new SqlParameter("@UserId", Convert.ToInt32(notesRequest.UserId)),
                new SqlParameter("@ExamId", Convert.ToInt32(notesRequest.ExamId)),
                new SqlParameter("@ExamAttemptId", Convert.ToInt32(notesRequest.ExamAttemptId)),
                new SqlParameter("@NotesText", Convert.ToString(notesRequest.NotesText)),
                new SqlParameter("@NotesCreatedDate", notesRequest.NotesCreatedDate),
                new SqlParameter("@NotesUpdatedDate", notesRequest.NotesUpdatedDate),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_AddAndGetNotesList", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    Notes notes = new Notes();
                    object notesIdObj = objSqlDataReader["notesId"];
                    notes.NotesId = notesIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["notesId"]);

                    object notesObj = objSqlDataReader["notes"];
                    notes.NotesText = notesObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["notes"]);

                    object questionIdObj = objSqlDataReader["questionId"];
                    notes.QuestionId = questionIdObj is DBNull ? 0 : Convert.ToInt16(objSqlDataReader["questionId"]);
                    notesList.Add(notes);
                }

                objSqlDataReader.Close();
            }

            return notesList;
        }

        public static List<Notes> UpdateNotes(NotesRequest notesRequest)
        {
            List<Notes> notesList = new List<Notes>();

            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@QuestionId", Convert.ToInt32(notesRequest.QuestionId)),
                new SqlParameter("@UserId", Convert.ToInt32(notesRequest.UserId)),
                new SqlParameter("@NotesId", Convert.ToInt32(notesRequest.NotesId)),
                new SqlParameter("@NotesText", Convert.ToString(notesRequest.NotesText)),
                new SqlParameter("@NotesUpdatedDate", notesRequest.NotesUpdatedDate),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_UpdateAndGetNotesList", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    Notes notes = new Notes();
                    object notesIdObj = objSqlDataReader["notesId"];
                    notes.NotesId = notesIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["notesId"]);

                    object notesObj = objSqlDataReader["notes"];
                    notes.NotesText = notesObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["notes"]);

                    object questionIdObj = objSqlDataReader["questionId"];
                    notes.QuestionId = questionIdObj is DBNull ? 0 : Convert.ToInt16(objSqlDataReader["questionId"]);
                    notesList.Add(notes);
                }

                objSqlDataReader.Close();
            }

            return notesList;
        }

        public static List<Notes> DeleteNotes(NotesRequest notesRequest)
        {
            List<Notes> notesList = new List<Notes>();

            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@QuestionId", Convert.ToInt32(notesRequest.QuestionId)),
                new SqlParameter("@UserId", Convert.ToInt32(notesRequest.UserId)),
                new SqlParameter("@NotesId", Convert.ToInt32(notesRequest.NotesId)),
                new SqlParameter("@NotesUpdatedDate", notesRequest.NotesUpdatedDate),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_DeleteAndGetNotesList", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    Notes notes = new Notes();
                    object notesIdObj = objSqlDataReader["notesId"];
                    notes.NotesId = notesIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["notesId"]);

                    object notesObj = objSqlDataReader["notes"];
                    notes.NotesText = notesObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["notes"]);

                    object questionIdObj = objSqlDataReader["questionId"];
                    notes.QuestionId = questionIdObj is DBNull ? 0 : Convert.ToInt16(objSqlDataReader["questionId"]);
                    notesList.Add(notes);
                }

                objSqlDataReader.Close();
            }

            return notesList;
        }

        public static ChapterDetails GetChapterByName(NoteBookRequest noteBookRequest)
        {
            ChapterDetails chapterDetails = new ChapterDetails();

            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@SubspecialtyId", Convert.ToInt32(noteBookRequest.SubSpecialtyId)),
                new SqlParameter("@ChapterName", noteBookRequest.ChapterName),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetChapterDetailsByName", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    object chapterNameObj = objSqlDataReader["ChapterName"];
                    chapterDetails.ChapterName = chapterNameObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["ChapterName"]);

                    object chapterObj = objSqlDataReader["Chapter"];
                    chapterDetails.Chapter = chapterObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Chapter"]);
                }

                objSqlDataReader.Close();
            }

            return chapterDetails;
        }
    }
}
