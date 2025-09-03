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
    using System.Configuration;

    public class UtilizationReportsDAL
    {
        public static List<UtilizationReportsVM> UtilizationReports(UtilizationReportsVM utilizationReports)
        {
            string ordering = string.Empty;
            if (utilizationReports.OrderBy != null)
            {
                ordering = " order by " + utilizationReports.OrderBy + " " + utilizationReports.Seq;
            }

            List<UtilizationReportsVM> reportList = new List<UtilizationReportsVM>();
            SqlParameter[] objSqlParameter =
            {
                                         new SqlParameter("@StartDate", utilizationReports.ExamStartDate),
                                         new SqlParameter("@EndDate", utilizationReports.ExamCompletedDate),
                                         new SqlParameter("@UserYear ", utilizationReports.Year),
                                         new SqlParameter("@Order", ordering),
                                     };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetUtilizationReports", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    UtilizationReportsVM reportListBO = new UtilizationReportsVM();
                    reportListBO.UserName = Convert.ToString(objSqlDataReader["UserName"]);

                    object customerIdObj = objSqlDataReader["CustomerID"];
                    reportListBO.CustomerId = customerIdObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["CustomerID"]);
                    reportListBO.QuestionAnswered = Convert.ToInt32(objSqlDataReader["QuestionAnswered"]);
                    reportListBO.QuestionsCustom = Convert.ToInt32(objSqlDataReader["QuestionsCustom"]);
                    reportListBO.QuestionsSimulated = Convert.ToInt32(objSqlDataReader["QuestionsSimulated"]);
                    reportListBO.QuestionsQuick = Convert.ToInt32(objSqlDataReader["QuestionsQuick"]);
                    reportListBO.QuestionsChallenged = Convert.ToInt32(objSqlDataReader["QuestionsChallenged"]);
                    reportListBO.QuestionsSpacedRepetition = Convert.ToInt32(objSqlDataReader["QuestionsSpacedRepetition"]);
                    reportListBO.QuestionsSharedExam = Convert.ToInt32(objSqlDataReader["QuestionsSharedExam"]);
                    reportList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return reportList;
        }

        public static UtilizationReportsVM AtAGlance(UtilizationReportsVM utilizationReports)
        {
            UtilizationReportsVM atAGlance = new UtilizationReportsVM();
            SqlParameter[] objSqlParameter =
            {
                                                 new SqlParameter("@StartDate", utilizationReports.ExamStartDate),
                                                 new SqlParameter("@EndDate", utilizationReports.ExamCompletedDate),
                                                 new SqlParameter("@UserYear ", utilizationReports.Year),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAtAGlance", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    atAGlance.TotalUserCount = Convert.ToInt32(objSqlDataReader["Total"]);
                }

                objSqlDataReader.Close();
            }

            return atAGlance;
        }
    }
}
