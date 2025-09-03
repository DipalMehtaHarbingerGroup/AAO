namespace AAO.DAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;
    using SQLHelper.BCSCSelfAssessment;

    public static class SpecialityDAL
    {
        public static List<SubSpecialityDetailVM> GetSpecialityList(int? userId)
        {
            SqlParameter[] arrSqlParameter =
            {
                    new SqlParameter("@UserId", Convert.ToInt32(userId)),
                   };
            List<SubSpecialityDetailVM> lstSpeciality = new List<SubSpecialityDetailVM>();

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetQuestionTypeCountOnCategory", arrSqlParameter))
            {
                if (objSqlDataReader.HasRows)
                {
                    while (objSqlDataReader.Read())
                    {
                        SubSpecialityDetailVM objSpecialityBO = new SubSpecialityDetailVM();

                        object subspecialtyIdCount = objSqlDataReader["SubspecialtyId"];
                        objSpecialityBO.SpecialityId = subspecialtyIdCount is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);
                        object subspecialtyCount = objSqlDataReader["Subspecialty"];
                        objSpecialityBO.SpecialityName = subspecialtyCount is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["Subspecialty"]);
                        object examSkipQuestionCountsCount = objSqlDataReader["ExamSkipQuestionCounts"];
                        objSpecialityBO.ExamSkipQuestionCount = examSkipQuestionCountsCount is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["ExamSkipQuestionCounts"]);
                        object incorrectAsnwerCountsCount = objSqlDataReader["IncorrectAsnwerCounts"];
                        objSpecialityBO.IncorrectAsnwerCount = incorrectAsnwerCountsCount is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["IncorrectAsnwerCounts"]);
                        object markQuestionCountsCount = objSqlDataReader["MarkQuestionCounts"];
                        objSpecialityBO.MarkQuestionCount = markQuestionCountsCount is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["MarkQuestionCounts"]);
                        object totalCountsCount = objSqlDataReader["TotalCount"];
                        objSpecialityBO.TotalCount = totalCountsCount is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["TotalCount"]);
                        lstSpeciality.Add(objSpecialityBO);
                    }

                    objSqlDataReader.Close();
                }
            }

            return lstSpeciality;
        }
    }
}