namespace AAO.DAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;

    public class ViewAssessmentDAL
    {
        public static List<int> ViewAssessmentDetails(int? examId)
        {
            List<int> questionList = new List<int>();

            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@ExamId", examId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQuestionId", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    int objExamListBO = 0;

                    // objExamListBO.UserId = Convert.ToInt32(objSqlDataReader["UserId"]);
                    objExamListBO = Convert.ToInt32(objSqlDataReader["QuestionId"]);
                    questionList.Add(objExamListBO);
                }

                objSqlDataReader.Close();
            }

            return questionList;
        }

        public static List<QuestionIdWithCountVM> FilterByQuestions(int? examId, string filter, int noOfRecords, int pageNo, int userId)
        {
            List<QuestionIdWithCountVM> questionList = new List<QuestionIdWithCountVM>();

            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@ExamId", examId),
                                                new SqlParameter("@Filter", filter),
                                                new SqlParameter("@PageSize", noOfRecords),
                                                new SqlParameter("@PageIndex", pageNo),
                                                new SqlParameter("@Userid", userId),
                                             };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetExamQuestionByFilter", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    QuestionIdWithCountVM objExamListBO = new QuestionIdWithCountVM();
                    objExamListBO.QuestionId = Convert.ToInt32(objSqlDataReader["QuestionId"]);
                    objExamListBO.QuestionIdCount = Convert.ToInt32(objSqlDataReader["RecordCount"]);
                    objExamListBO.QuestionNo = Convert.ToInt32(objSqlDataReader["RowNumber"]);
                    questionList.Add(objExamListBO);
                }

                objSqlDataReader.Close();
            }

            return questionList;
        }

        public static List<int> ViewDetails(int? examId)
        {
            List<int> questionList = new List<int>();

            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@ExamId", examId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQuestionId", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    int objExamListBO = 0;

                    // objExamListBO.UserId = Convert.ToInt32(objSqlDataReader["UserId"]);
                    objExamListBO = Convert.ToInt32(objSqlDataReader["QuestionId"]);
                    questionList.Add(objExamListBO);
                }

                objSqlDataReader.Close();
            }

            return questionList;
        }

        public static List<QuestionIdWithExamId> SearchByQuestions(string userId, string searchterm, string filter, int noOfRecords, int pageNo)
        {
            List<QuestionIdWithExamId> questionList = new List<QuestionIdWithExamId>();

            SqlParameter[] objSqlParameter =
            { new SqlParameter("@Searchterm", searchterm),
                            new SqlParameter("@UserId", userId ),
                            new SqlParameter("@Filter", filter ),
                            new SqlParameter("@PageSize", noOfRecords),
                            new SqlParameter("@PageIndex", pageNo),
    };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_Search_Question_Procedure", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    QuestionIdWithExamId objExamListBO = new QuestionIdWithExamId();
                    objExamListBO.QuestionId = Convert.ToInt32(objSqlDataReader["QuestionId"]);
                    objExamListBO.ExamId = filter != "All" ? Convert.ToInt32(objSqlDataReader["ExamId"]) : 0;
                    objExamListBO.QuestionIdCount = Convert.ToInt32(objSqlDataReader["RecordCount"]);
                    objExamListBO.QuestionNo = Convert.ToInt32(objSqlDataReader["RowNumber"]);
                    questionList.Add(objExamListBO);
                }

                objSqlDataReader.Close();
            }

            return questionList;
        }
    }
}
