using AAO.Common.BCSCSelfAssessment;
using AAO.DTO.BCSCSelfAssessment;
using AAO.SQLHelper.BCSCSelfAssessment;
using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAO.DAL.BCSCSelfAssessment
{
    public class NotificationMessageSettingDAL
    {
        public static List<NotificationMessageSetting> GetAllNotificationMessageSetting(SearchParameters queDetails)
        {
            List<NotificationMessageSetting> performanceList = new List<NotificationMessageSetting>();
            SqlParameter[] objSqlParameter =
            {
                                                  new SqlParameter("@OffsetValue", queDetails.OffsetValue),
                                                  new SqlParameter("@PagingSize", queDetails.PagingSize),
                                                  new SqlParameter("@Search", queDetails.Search),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAllNotificationMessageSetting", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    NotificationMessageSetting reportListBO = new NotificationMessageSetting();
                    object NotificationMessageSettingIdObj = objSqlDataReader["NotificationMessageSettingId"];
                    reportListBO.NotificationMessageSettingId = NotificationMessageSettingIdObj is DBNull ? 0 : Convert.ToInt32(NotificationMessageSettingIdObj);

                    object UserIdObj = objSqlDataReader["UserId"];
                    reportListBO.UserId = UserIdObj is DBNull ? (int?)null : Convert.ToInt32(UserIdObj);

                    object NotificationMessageObj = objSqlDataReader["NotificationMessage"];
                    reportListBO.NotificationMessage = NotificationMessageObj is DBNull ? null : Convert.ToString(NotificationMessageObj);

                    object NoOfTimeObj = objSqlDataReader["NoOfTime"];
                    reportListBO.NoOfTime = NoOfTimeObj is DBNull ? 0 : Convert.ToInt32(NoOfTimeObj);

                    object OnholdScreenTimeObj = objSqlDataReader["OnholdScreenTime"];
                    reportListBO.OnholdScreenTime = OnholdScreenTimeObj is DBNull ? 0 : Convert.ToInt32(OnholdScreenTimeObj);

                    object IsUnableObj = objSqlDataReader["IsUnable"];
                    reportListBO.IsUnable = IsUnableObj is DBNull ? false : Convert.ToBoolean(IsUnableObj);

                    object ISDoYouWishToSeeThisMessageAgainObj = objSqlDataReader["ISDoYouWishToSeeThisMessageAgain"];
                    reportListBO.ISDoYouWishToSeeThisMessageAgain = ISDoYouWishToSeeThisMessageAgainObj is DBNull ? false : Convert.ToBoolean(ISDoYouWishToSeeThisMessageAgainObj);

                    object TotalObj = objSqlDataReader["Total"];
                    reportListBO.Total = TotalObj is DBNull ? 0 : Convert.ToInt32(TotalObj);

                    performanceList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return performanceList;
        }

        public static List<NotificationMessageSetting> GetAllMessageSettingByUser(SearchParameters queDetails)
        {
            List<NotificationMessageSetting> performanceList = new List<NotificationMessageSetting>();
            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@UserId", queDetails.UserId),
            };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAllNotificationMessageSettingByUser", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    NotificationMessageSetting reportListBO = new NotificationMessageSetting();
                    object NotificationMessageSettingIdObj = objSqlDataReader["NotificationMessageSettingId"];
                    reportListBO.NotificationMessageSettingId = NotificationMessageSettingIdObj is DBNull ? 0 : Convert.ToInt32(NotificationMessageSettingIdObj);

                    object UserIdObj = objSqlDataReader["UserId"];
                    reportListBO.UserId = UserIdObj is DBNull ? (int?)null : Convert.ToInt32(UserIdObj);

                    object NotificationMessageObj = objSqlDataReader["NotificationMessage"];
                    reportListBO.NotificationMessage = NotificationMessageObj is DBNull ? null : Convert.ToString(NotificationMessageObj);

                    object NoOfTimeObj = objSqlDataReader["NoOfTime"];
                    reportListBO.NoOfTime = NoOfTimeObj is DBNull ? 0 : Convert.ToInt32(NoOfTimeObj);

                    object OnholdScreenTimeObj = objSqlDataReader["OnholdScreenTime"];
                    reportListBO.OnholdScreenTime = OnholdScreenTimeObj is DBNull ? 0 : Convert.ToInt32(OnholdScreenTimeObj);

                    object IsUnableObj = objSqlDataReader["IsUnable"];
                    reportListBO.IsUnable = IsUnableObj is DBNull ? false : Convert.ToBoolean(IsUnableObj);

                    object ISDoYouWishToSeeThisMessageAgainObj = objSqlDataReader["ISDoYouWishToSeeThisMessageAgain"];
                    reportListBO.ISDoYouWishToSeeThisMessageAgain = ISDoYouWishToSeeThisMessageAgainObj is DBNull ? false : Convert.ToBoolean(ISDoYouWishToSeeThisMessageAgainObj);


                    performanceList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return performanceList;
        }


        public static NotificationMessageSetting GetNotificationMessageSetting(SearchParameters queDetails)
        {
            List<NotificationMessageSetting> performanceList = new List<NotificationMessageSetting>();
            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@id", queDetails.Id)
            };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetNotificationMessageSettingById", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    NotificationMessageSetting reportListBO = new NotificationMessageSetting();
                    object NotificationMessageSettingIdObj = objSqlDataReader["NotificationMessageSettingId"];
                    reportListBO.NotificationMessageSettingId = NotificationMessageSettingIdObj is DBNull ? 0 : Convert.ToInt32(NotificationMessageSettingIdObj);

                    object UserIdObj = objSqlDataReader["UserId"];
                    reportListBO.UserId = UserIdObj is DBNull ? (int?)null : Convert.ToInt32(UserIdObj);

                    object NotificationMessageObj = objSqlDataReader["NotificationMessage"];
                    reportListBO.NotificationMessage = NotificationMessageObj is DBNull ? null : Convert.ToString(NotificationMessageObj);

                    object NoOfTimeObj = objSqlDataReader["NoOfTime"];
                    reportListBO.NoOfTime = NoOfTimeObj is DBNull ? 0 : Convert.ToInt32(NoOfTimeObj);

                    object OnholdScreenTimeObj = objSqlDataReader["OnholdScreenTime"];
                    reportListBO.OnholdScreenTime = OnholdScreenTimeObj is DBNull ? 0 : Convert.ToInt32(OnholdScreenTimeObj);

                    object IsUnableObj = objSqlDataReader["IsUnable"];
                    reportListBO.IsUnable = IsUnableObj is DBNull ? false : Convert.ToBoolean(IsUnableObj);

                    object ISDoYouWishToSeeThisMessageAgainObj = objSqlDataReader["ISDoYouWishToSeeThisMessageAgain"];
                    reportListBO.ISDoYouWishToSeeThisMessageAgain = ISDoYouWishToSeeThisMessageAgainObj is DBNull ? false : Convert.ToBoolean(ISDoYouWishToSeeThisMessageAgainObj);

                    performanceList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return performanceList.FirstOrDefault();
        }


        public static void SaveNotificationMessageSetting(NotificationMessageSetting notificationMessageSetting)
        {
            try
            {
                SqlParameter[] objSqlParameter =
                {
                new SqlParameter("@NotificationMessageSettingId", notificationMessageSetting.NotificationMessageSettingId),
                new SqlParameter("@NotificationMessage", notificationMessageSetting.NotificationMessage),
                new SqlParameter("@NoOfTime", Convert.ToInt32(notificationMessageSetting.NoOfTime)),
                new SqlParameter("@OnholdScreenTime", notificationMessageSetting.OnholdScreenTime),
                new SqlParameter("@IsUnable", notificationMessageSetting.IsUnable),
                new SqlParameter("@UserId", notificationMessageSetting.UserId),
                new SqlParameter("@ISDoYouWishToSeeThisMessageAgain", notificationMessageSetting.ISDoYouWishToSeeThisMessageAgain),
            };

                int result = SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_CreateOrUpdateNotificationMessageSetting", objSqlParameter);
            }
            catch (Exception e)
            {
                // to do
            }
        }

        public static bool DeleteMessageSetting(SearchParameters queDetails)
        {
            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@NotificationMessageSettingId", queDetails.Id),
            };

            int count = SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_DeleteNotificationMessageSetting", objSqlParameter);

            return count > 0 ? true : false;
        }

        public static bool InsertOrUpdateUserMessageSetting(SearchParameters queDetails)
        {
            SqlParameter[] objSqlParameter =
            {
                new SqlParameter("@UserId", queDetails.UserId),
            };

            int count = SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_InsertUserNotificationMessageSetting", objSqlParameter);

            return true;
        }

        public static ExamDTO GetExamData(SearchParameters loginDetails)
        {
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@UserId", Convert.ToInt32(loginDetails.UserId)),
                                             };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetExamData", objSqlParameter))
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
    }
}
