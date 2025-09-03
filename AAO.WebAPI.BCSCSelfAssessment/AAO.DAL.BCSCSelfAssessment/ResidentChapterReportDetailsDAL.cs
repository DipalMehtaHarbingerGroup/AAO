namespace AAO.DAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using AAO.DTO.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;

    public static class ResidentChapterReportDetailsDAL
    {
        public static List<ResidentChapterReportDetailsDTO> ResidentChapterReportDetails(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            List<ResidentChapterReportDetailsDTO> reportList = new List<ResidentChapterReportDetailsDTO>();

            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@UserId", reportChapterDetails.UserId),
                                                new SqlParameter("@SubspecialtyId", reportChapterDetails.SubspecialtyId),
                                                new SqlParameter("@FROMdate", reportChapterDetails.ExamStartDate),
                                                new SqlParameter("@TOdate", reportChapterDetails.ExamCompletedDate),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetResidentSectionChapterReport", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    ResidentChapterReportDetailsDTO reportListBO = new ResidentChapterReportDetailsDTO();
                    object subspecialtyObj = objSqlDataReader["Subspecialty"];
                    reportListBO.Subspecialty = subspecialtyObj is DBNull ? null : Convert.ToString(objSqlDataReader["Subspecialty"]);
                    object chapterNameObj = objSqlDataReader["chapterName"];
                    reportListBO.ChapterName = chapterNameObj is DBNull ? null : Convert.ToString(objSqlDataReader["chapterName"]);
                    object correctObj = objSqlDataReader["Correct"];
                    reportListBO.Correct = correctObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Correct"]);
                    object inCorrectObj = objSqlDataReader["InCorrect"];
                    reportListBO.InCorrect = inCorrectObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["InCorrect"]);
                    object scoreObj = objSqlDataReader["RemainingQuestion"];
                    reportListBO.RemainingQuestion = scoreObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["RemainingQuestion"]);
                    object bCSCSectionNumberObj = objSqlDataReader["BCSCSectionNumber"];
                    reportListBO.BCSCSectionNumber = bCSCSectionNumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["BCSCSectionNumber"]);
                    reportList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return reportList;
        }
    }
}
