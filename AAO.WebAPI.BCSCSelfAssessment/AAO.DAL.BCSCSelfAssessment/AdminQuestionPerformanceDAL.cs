namespace AAO.DAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;
    using Serilog;

    public class AdminQuestionPerformanceDAL
    {
        //private static string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        //private static Serilog.Core.Logger log = new LoggerConfiguration().WriteTo.RollingFile(_sAttrLogPath, retainedFileCountLimit: 7).CreateLogger();

        public static List<AdminQuestionPerformanceVM> AdminQuestionPerformanceDetails(AdminQuestionPerformanceVM queDetails)
        {
            List<AdminQuestionPerformanceVM> performanceList = new List<AdminQuestionPerformanceVM>();

            SqlParameter[] objSqlParameter =
            {
                                                  new SqlParameter("@StartDate", queDetails.ExamStartDate),
                                                  new SqlParameter("@EndDate", queDetails.ExamCompletedDate),
                                                  new SqlParameter("@UserYear", queDetails.Year),
                                                  new SqlParameter("@PageIndex", queDetails.PageNo),
                                                  new SqlParameter("@PageSize", queDetails.NoOfRecords),
                                                  new SqlParameter("@OrderColumn",  queDetails.OrderBy),
                                                  new SqlParameter("@OrderSequence", queDetails.Seq),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAdminQuestionListReport", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    AdminQuestionPerformanceVM reportListBO = new AdminQuestionPerformanceVM();
                    object stemObj = objSqlDataReader["Stem"];
                    reportListBO.Stem = stemObj is DBNull ? null : Convert.ToString(objSqlDataReader["Stem"]);

                    object questionIdObj = objSqlDataReader["QuestionId"];
                    reportListBO.QuestionId = questionIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionId"]);

                    object subspecialtyObj = objSqlDataReader["Subspecialty"];
                    reportListBO.Subspecialty = subspecialtyObj is DBNull ? null : Convert.ToString(objSqlDataReader["Subspecialty"]);

                    object chapterObj = objSqlDataReader["Chapter"];
                    reportListBO.Chapter = chapterObj is DBNull ? null : Convert.ToString(objSqlDataReader["Chapter"]);

                    object topicObj = objSqlDataReader["Topic"];
                    reportListBO.Topic = topicObj is DBNull ? null : Convert.ToString(objSqlDataReader["Topic"]);

                    object userAnsweredcorrectlyObj = objSqlDataReader["UserAnsweredcorrectly"];
                    reportListBO.UserAnsweredcorrectly = userAnsweredcorrectlyObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["UserAnsweredcorrectly"]);

                    object userAnsweredincorrectlyObj = objSqlDataReader["UserAnsweredincorrectly"];
                    reportListBO.UserAnsweredincorrectly = userAnsweredincorrectlyObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["UserAnsweredincorrectly"]);

                    object usersRepeatedSRModeObj = objSqlDataReader["UsersRepeatedSRMode"];
                    reportListBO.UsersRepeatedSRMode = usersRepeatedSRModeObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["UsersRepeatedSRMode"]);

                    object percentagecorrectlyObj = objSqlDataReader["Percentagecorrectly"];
                    reportListBO.Percentagecorrectly = percentagecorrectlyObj is DBNull ? null : Convert.ToString(objSqlDataReader["Percentagecorrectly"]);

                    object percentageIncorrectlyObj = objSqlDataReader["PercentageIncorrectly"];
                    reportListBO.PercentageIncorrectly = percentageIncorrectlyObj is DBNull ? null : Convert.ToString(objSqlDataReader["PercentageIncorrectly"]);

                    object correctAnsObj = objSqlDataReader["CorrectAnswer"];
                    reportListBO.CorrectAnswer = correctAnsObj is DBNull ? null : Convert.ToString(objSqlDataReader["CorrectAnswer"]);

                    object distroctor1Obj = objSqlDataReader["Distractor_1"];
                    reportListBO.Distractor_1 = distroctor1Obj is DBNull ? null : Convert.ToString(objSqlDataReader["Distractor_1"]);

                    object perDistroctor1Obj = objSqlDataReader["user_chose_distractor_1"];
                    reportListBO.User_chose_distractor_1 = perDistroctor1Obj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["User_chose_distractor_1"]);

                    object distroctor2Obj = objSqlDataReader["distractor_2"];
                    reportListBO.Distractor_2 = distroctor2Obj is DBNull ? null : Convert.ToString(objSqlDataReader["Distractor_2"]);

                    object perDistroctor2Obj = objSqlDataReader["user_chose_distractor_2"];
                    reportListBO.User_chose_distractor_2 = perDistroctor2Obj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["User_chose_distractor_2"]);

                    object distroctor3Obj = objSqlDataReader["distractor_3"];
                    reportListBO.Distractor_3 = distroctor3Obj is DBNull ? null : Convert.ToString(objSqlDataReader["Distractor_3"]);

                    object perDistroctor3Obj = objSqlDataReader["user_chose_distractor_3"];
                    reportListBO.User_chose_distractor_3 = perDistroctor3Obj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["User_chose_distractor_3"]);

                    object avg_times_incorrect_before_correct = objSqlDataReader["Avg_times_incorrect_before_correct"];
                    reportListBO.Avg_times_incorrect_before_correct = avg_times_incorrect_before_correct is DBNull ? null : Convert.ToString(objSqlDataReader["Avg_times_incorrect_before_correct"]);

                    object users_correct_first_try = objSqlDataReader["users_correct_first_try"];
                    reportListBO.Users_correct_first_try = users_correct_first_try is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["users_correct_first_try"]);

                    performanceList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return performanceList;
        }

        public static int GetAllQuestionCount(AdminQuestionPerformanceVM questionDetails)
        {
            int count = 0;
            SqlParameter[] objSqlParameter =
            {
                                                  new SqlParameter("@StartDate", questionDetails.ExamStartDate),
                                                  new SqlParameter("@EndDate", questionDetails.ExamCompletedDate),
                                                  new SqlParameter("@UserYear", questionDetails.Year),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAdminQuestionCountReport", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    object cntObj = objSqlDataReader["count"];
                    count = cntObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["count"]);
                }

                objSqlDataReader.Close();
            }

            return count;
        }

        public static List<AdminQuestionPerformanceVM> GetAllQuestionExport(AdminQuestionPerformanceVM questionDetails)
        {
            List<AdminQuestionPerformanceVM> quePerformanceList = new List<AdminQuestionPerformanceVM>();
            SqlParameter[] objSqlParameter =
            {
                                                  new SqlParameter("@StartDate", questionDetails.ExamStartDate),
                                                  new SqlParameter("@EndDate", questionDetails.ExamCompletedDate),
                                                  new SqlParameter("@UserYear", questionDetails.Year),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAdminQuestionExportReport", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    AdminQuestionPerformanceVM queReportListBO = new AdminQuestionPerformanceVM();
                    object stemObj = objSqlDataReader["Stem"];
                    queReportListBO.Stem = stemObj is DBNull ? null : Convert.ToString(objSqlDataReader["Stem"]);

                    object questionIdObj = objSqlDataReader["QuestionId"];
                    queReportListBO.QuestionId = questionIdObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["QuestionId"]);

                    object subspecialtyObj = objSqlDataReader["Subspecialty"];
                    queReportListBO.Subspecialty = subspecialtyObj is DBNull ? null : Convert.ToString(objSqlDataReader["Subspecialty"]);

                    object chapterObj = objSqlDataReader["Chapter"];
                    queReportListBO.Chapter = chapterObj is DBNull ? null : Convert.ToString(objSqlDataReader["Chapter"]);

                    object topicObj = objSqlDataReader["Topic"];
                    queReportListBO.Topic = topicObj is DBNull ? null : Convert.ToString(objSqlDataReader["Topic"]);

                    object userAnsweredcorrectlyObj = objSqlDataReader["UserAnsweredcorrectly"];
                    queReportListBO.UserAnsweredcorrectly = userAnsweredcorrectlyObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["UserAnsweredcorrectly"]);

                    object userAnsweredincorrectlyObj = objSqlDataReader["UserAnsweredincorrectly"];
                    queReportListBO.UserAnsweredincorrectly = userAnsweredincorrectlyObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["UserAnsweredincorrectly"]);

                    object usersRepeatedSRModeObj = objSqlDataReader["UsersRepeatedSRMode"];
                    queReportListBO.UsersRepeatedSRMode = usersRepeatedSRModeObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["UsersRepeatedSRMode"]);

                    object percentagecorrectlyObj = objSqlDataReader["Percentagecorrectly"];
                    queReportListBO.Percentagecorrectly = percentagecorrectlyObj is DBNull ? null : Convert.ToString(objSqlDataReader["Percentagecorrectly"]);

                    object percentageIncorrectlyObj = objSqlDataReader["PercentageIncorrectly"];
                    queReportListBO.PercentageIncorrectly = percentageIncorrectlyObj is DBNull ? null : Convert.ToString(objSqlDataReader["PercentageIncorrectly"]);

                    object correctAnsObj = objSqlDataReader["CorrectAnswer"];
                    queReportListBO.CorrectAnswer = correctAnsObj is DBNull ? null : Convert.ToString(objSqlDataReader["CorrectAnswer"]);

                    object distroctor1Obj = objSqlDataReader["Distractor_1"];
                    queReportListBO.Distractor_1 = distroctor1Obj is DBNull ? null : Convert.ToString(objSqlDataReader["Distractor_1"]);

                    object perDistroctor1Obj = objSqlDataReader["user_chose_distractor_1"];
                    queReportListBO.User_chose_distractor_1 = perDistroctor1Obj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["User_chose_distractor_1"]);

                    object distroctor2Obj = objSqlDataReader["distractor_2"];
                    queReportListBO.Distractor_2 = distroctor2Obj is DBNull ? null : Convert.ToString(objSqlDataReader["Distractor_2"]);

                    object perDistroctor2Obj = objSqlDataReader["user_chose_distractor_2"];
                    queReportListBO.User_chose_distractor_2 = perDistroctor2Obj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["User_chose_distractor_2"]);

                    object distroctor3Obj = objSqlDataReader["distractor_3"];
                    queReportListBO.Distractor_3 = distroctor3Obj is DBNull ? null : Convert.ToString(objSqlDataReader["Distractor_3"]);

                    object perDistroctor3Obj = objSqlDataReader["user_chose_distractor_3"];
                    queReportListBO.User_chose_distractor_3 = perDistroctor3Obj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["User_chose_distractor_3"]);

                    object avg_times_incorrect_before_correct = objSqlDataReader["Avg_times_incorrect_before_correct"];
                    queReportListBO.Avg_times_incorrect_before_correct = avg_times_incorrect_before_correct is DBNull ? null : Convert.ToString(objSqlDataReader["Avg_times_incorrect_before_correct"]);

                    object users_correct_first_try = objSqlDataReader["users_correct_first_try"];
                    queReportListBO.Users_correct_first_try = users_correct_first_try is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["users_correct_first_try"]);

                    quePerformanceList.Add(queReportListBO);
                }

                objSqlDataReader.Close();
            }

            return quePerformanceList;
        }

        public static List<AdminExamQuestionReport> _quePerformanceList = new List<AdminExamQuestionReport>();

        public static async Task<List<AdminExamQuestionReport>> GetAllAnsweredQuestionExport(AdminQuestionPerformanceVM questionDetails)
        {
            _quePerformanceList.Clear();
            int tablecount = 3;

            try
            {
                //SqlParameter[] objSqlParameter =
                //{
                //                                  new SqlParameter("@StartDate", questionDetails.ExamStartDate),
                //                                  new SqlParameter("@EndDate", questionDetails.ExamCompletedDate),
                //                                  new SqlParameter("@UserYear", questionDetails.Year),
                //                                  new SqlParameter("@OptOutValue", questionDetails.OptOutValue),
                //                             };

                //using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                //    SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAllAdminQuestionExportReport", objSqlParameter))
                //{
                //    while (objSqlDataReader.Read())
                //    {
                //        tablecount = Convert.ToInt32(objSqlDataReader["NoOfTables"]);
                //    }

                //    objSqlDataReader.Close();
                //}

                long startCount = 1;
                long endCount = 100;
                var taskList = new List<Task>();
                for (int i = 1; i <= tablecount; i++)
                {
                    var task = new Task(new Action(async () => await GetQuestionTable(startCount, endCount)));
                    //GetQuestionTable(startCount, endCount);
                    task.Start();
                    taskList.Add(task);
                    long startCount1 = startCount;
                    startCount = endCount;
                    endCount = endCount + startCount1;

                }
                Task.WaitAll(taskList.ToArray());
                // _quePerformanceList.GroupBy(x => x.ID).Select(x => x.First()).ToList();
                //_quePerformanceList.Select(x => x.ID).AsParallel().Distinct().ToList(); 
                //_quePerformanceList.Distinct(x => x.ID).ToList();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.Write(ex.InnerException);
            }
            return _quePerformanceList;
        }

        private static async Task<List<AdminExamQuestionReport>> GetQuestionTable(long startCount, long endCount)
        // private static async void GetQuestionTable(long startCount, long endCount)

        {
            string connectionString = ConfigurationManager.ConnectionStrings["AAODBConnection"].ConnectionString;
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objSqlParameter =
                {
                                                  new SqlParameter("@StartCount", startCount),
                                                  new SqlParameter("@EndCount", endCount),
                                             };

                using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                    SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAllAdminQuestionExportInExcel", objSqlParameter))
                {
                    await Task.Run(() =>
                    {

                        while (objSqlDataReader.Read())
                        {
                            _quePerformanceList.Add(new AdminExamQuestionReport
                            {
                                ID = Convert.ToInt32(objSqlDataReader["row_num"]),

                                Email = Convert.ToString(objSqlDataReader["Email"]),

                                MasterCustomerID = Convert.ToString(objSqlDataReader["Master_Customer_ID"]),

                                UserName = Convert.ToString(objSqlDataReader["User_Name"]),

                                OptInStatus = Convert.ToString(objSqlDataReader["Opt_In_Status"]),

                                ExamId = Convert.ToInt32(objSqlDataReader["ExamId"]),

                                //ExamStartDate = Convert.ToDateTime(objSqlDataReader["Exam_StartDate"]),
                                ExamStartDate = Convert.ToDateTime(objSqlDataReader["Exam_StartDate"]).ToString("MM-dd-yyyy"),

                                ExamType = Convert.ToString(objSqlDataReader["ExamType"]),

                                SelectedSubspeciality = Convert.ToString(objSqlDataReader["Selected_Subspeciality"]),

                                NoOfQuestions = Convert.ToInt32(objSqlDataReader["NoofQuestions"]),

                                TypeOfQuestion = Convert.ToString(objSqlDataReader["TypeofQuestion"]),

                                ExamMode = Convert.ToString(objSqlDataReader["ExamMode"]),

                                ExamAnswerToShow = Convert.ToString(objSqlDataReader["Show_Correct_Answers"]),

                                ExamTimeType = Convert.ToString(objSqlDataReader["ExamTimeType"]),

                                QuestionId = Convert.ToInt32(objSqlDataReader["Question_Id"]),

                                Stem = Convert.ToString(objSqlDataReader["Stem"]),

                                ChallengeModeTypedAnswer = Convert.ToString(objSqlDataReader["Challenge_Mode_Typed_Answer"]),

                                ChallengeModeAnswerText = Convert.ToString(objSqlDataReader["Challenge_Mode_Answer_Text"]),

                                BCSCSectionTitle = Convert.ToString(objSqlDataReader["BCSCSectionTitle"]),

                                ChapterName = Convert.ToString(objSqlDataReader["chapterName"]),

                                TopicName = Convert.ToString(objSqlDataReader["TopicName"]),

                                UserSelectedChoice = Convert.ToString(objSqlDataReader["User_Selected_Choice"]),

                                UserAnsweredcorrectly = Convert.ToInt32(objSqlDataReader["#_Users_Answered_Correctly"]),

                                UserAnsweredincorrectly = Convert.ToInt32(objSqlDataReader["#_Users_Answered_Incorrectly"]),

                                UsersRepeatedSRMode = Convert.ToInt32(objSqlDataReader["#Users_Repeated_Atleast_Once_in_SR_Mode"]),

                                Percentagecorrectly = Convert.ToString(objSqlDataReader["Percent_Users_Answered_Correctly"]),

                                PercentageIncorrectly = Convert.ToString(objSqlDataReader["Percent_Users_Answered_Incorrectly"]),

                                CorrectAnswer = Convert.ToString(objSqlDataReader["Correct_Answer"]),

                                Distractor_1 = Convert.ToString(objSqlDataReader["Distractor_1"]),

                                User_chose_distractor_1 = Convert.ToInt32(objSqlDataReader["#_Who_Chose_Distractor_1"]),

                                Distractor_2 = Convert.ToString(objSqlDataReader["Distractor_2"]),

                                User_chose_distractor_2 = Convert.ToInt32(objSqlDataReader["#_Who_Chose_Distractor_2"]),

                                Distractor_3 = Convert.ToString(objSqlDataReader["Distractor_3"]),

                                User_chose_distractor_3 = Convert.ToInt32(objSqlDataReader["#_Who_Chose_Distractor_3"]),

                            });
                        }
                    });
                    objSqlDataReader.Close();
                }

                //using (SqlConnection conn = new SqlConnection(connectionString))
                //{
                //    SqlCommand sqlComm = new SqlCommand("AAO_GetAllAdminQuestionExportInExcel", conn);
                //    sqlComm.Parameters.AddWithValue("@StartCount", startCount);
                //    sqlComm.Parameters.AddWithValue("@EndCount", endCount);
                //    sqlComm.CommandTimeout = 0;
                //    sqlComm.CommandType = CommandType.StoredProcedure;
                //    SqlDataAdapter sda = new SqlDataAdapter();
                //    sda.SelectCommand = sqlComm;

                //    sda.Fill(ds);
                //}
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.Write(ex.InnerException);
            }

            return _quePerformanceList;
        }

        public static List<AdminExamQuestionReport> GetAllAnsweredQuestionExport1(AdminQuestionPerformanceVM questionDetails)
        {
            List<AdminExamQuestionReport> reportList = new List<AdminExamQuestionReport>();

            try
            {
                SqlParameter[] objSqlParameter =
                {
                                                  new SqlParameter("@StartCount", questionDetails.StartCount),
                                                  new SqlParameter("@EndCount", questionDetails.EndCount),
                                             };

                using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                    SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAllAdminQuestionExportInExcel", objSqlParameter))
                {
                    while (objSqlDataReader.Read())
                    {
                        reportList.Add(new AdminExamQuestionReport
                        {
                            ID = Convert.ToInt32(objSqlDataReader["row_num"]),

                            Email = Convert.ToString(objSqlDataReader["Email"]),

                            MasterCustomerID = Convert.ToString(objSqlDataReader["Master_Customer_ID"]),

                            UserName = Convert.ToString(objSqlDataReader["User_Name"]),

                            OptInStatus = Convert.ToString(objSqlDataReader["Opt_In_Status"]),

                            ExamId = Convert.ToInt32(objSqlDataReader["ExamId"]),

                            ExamStartDate = Convert.ToDateTime(objSqlDataReader["Exam_StartDate"]).ToString("MM-dd-yyyy"),

                            ExamType = Convert.ToString(objSqlDataReader["ExamType"]),

                            SelectedSubspeciality = Convert.ToString(objSqlDataReader["Selected_Subspeciality"]),

                            NoOfQuestions = Convert.ToInt32(objSqlDataReader["NoofQuestions"]),

                            TypeOfQuestion = Convert.ToString(objSqlDataReader["TypeofQuestion"]),

                            ExamMode = Convert.ToString(objSqlDataReader["ExamMode"]),

                            ExamAnswerToShow = Convert.ToString(objSqlDataReader["Show_Correct_Answers"]),

                            ExamTimeType = Convert.ToString(objSqlDataReader["ExamTimeType"]),

                            QuestionId = Convert.ToInt32(objSqlDataReader["Question_Id"]),

                            Stem = Convert.ToString(objSqlDataReader["Stem"]),

                            ChallengeModeTypedAnswer = Convert.ToString(objSqlDataReader["Challenge_Mode_Typed_Answer"]),

                            ChallengeModeAnswerText = Convert.ToString(objSqlDataReader["Challenge_Mode_Answer_Text"]),

                            BCSCSectionTitle = Convert.ToString(objSqlDataReader["BCSCSectionTitle"]),

                            ChapterName = Convert.ToString(objSqlDataReader["chapterName"]),

                            TopicName = Convert.ToString(objSqlDataReader["TopicName"]),

                            UserSelectedChoice = Convert.ToString(objSqlDataReader["User_Selected_Choice"]),

                            UserAnsweredcorrectly = Convert.ToInt32(objSqlDataReader["#_Users_Answered_Correctly"]),

                            UserAnsweredincorrectly = Convert.ToInt32(objSqlDataReader["#_Users_Answered_Incorrectly"]),

                            UsersRepeatedSRMode = Convert.ToInt32(objSqlDataReader["#Users_Repeated_Atleast_Once_in_SR_Mode"]),

                            Percentagecorrectly = Convert.ToString(objSqlDataReader["Percent_Users_Answered_Correctly"]),

                            PercentageIncorrectly = Convert.ToString(objSqlDataReader["Percent_Users_Answered_Incorrectly"]),

                            CorrectAnswer = Convert.ToString(objSqlDataReader["Correct_Answer"]),

                            Distractor_1 = Convert.ToString(objSqlDataReader["Distractor_1"]),

                            User_chose_distractor_1 = Convert.ToInt32(objSqlDataReader["#_Who_Chose_Distractor_1"]),

                            Distractor_2 = Convert.ToString(objSqlDataReader["Distractor_2"]),

                            User_chose_distractor_2 = Convert.ToInt32(objSqlDataReader["#_Who_Chose_Distractor_2"]),

                            Distractor_3 = Convert.ToString(objSqlDataReader["Distractor_3"]),

                            User_chose_distractor_3 = Convert.ToInt32(objSqlDataReader["#_Who_Chose_Distractor_3"]),
                        });
                    }

                    objSqlDataReader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.Write(ex.InnerException);
            }
            finally
            {
                Console.Write("Called for  StartCount:{0} EndCount:{1} ", questionDetails.StartCount, questionDetails.EndCount);
                //log.Information("Called for  StartCount:{0} EndCount:{1} ",questionDetails.StartCount, questionDetails.EndCount);
            }

            return reportList;
        }

        public static int GetTableCountExport(AdminQuestionPerformanceVM questionDetails)
        {
            int tablecount = 0;

            try
            {
                SqlParameter[] objSqlParameter =
                {
                                                  new SqlParameter("@StartDate", questionDetails.ExamStartDate),
                                                  new SqlParameter("@EndDate", questionDetails.ExamCompletedDate),
                                                  new SqlParameter("@UserYear", questionDetails.Year),
                                                  new SqlParameter("@OptOutValue", questionDetails.OptOutValue),
                                             };

                using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                    SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetTableCountExportReport", objSqlParameter))
                {
                    while (objSqlDataReader.Read())
                    {
                        tablecount = Convert.ToInt32(objSqlDataReader["NoOfTables"]);
                    }

                    objSqlDataReader.Close();
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.Write(ex.InnerException);
            }
            finally
            {
                Console.Write("Table count :{0} StartDate:{1} EndDate:{2}", tablecount, questionDetails.ExamStartDate, questionDetails.ExamCompletedDate);
                // log.Information("Table count :{0} StartDate:{1} EndDate:{2}", tablecount,questionDetails.ExamStartDate, questionDetails.ExamCompletedDate);
            }
            return tablecount;
        }

    }
}
