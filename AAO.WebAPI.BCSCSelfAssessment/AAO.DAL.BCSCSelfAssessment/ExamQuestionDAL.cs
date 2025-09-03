namespace AAO.DAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using AAO.DTO.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;


    public static class ExamQuestionDAL
    {
        public static int AddExamQuestion(List<ExamQuestionDTO> examQuestionList, ExamDTO examObj)
        {
            List<SelectedQuestion> questionList = new List<SelectedQuestion>();
            foreach (ExamQuestionDTO examQuestion in examQuestionList)
            {
                questionList.Add(new SelectedQuestion { ExamId = examQuestion.ExamId, QuestionId = examQuestion.QuestionId });
            }

            string tmpTable = "create table #question_selected (ID BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED (ID ASC), ExamId int, QuestionId int)";

            // Create a datatable that matches the temp table exactly. (WARNING: order of columns must match the order in the table)
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("ID", typeof(long)));
            table.Columns.Add(new DataColumn("ExamId", typeof(int)));
            table.Columns.Add(new DataColumn("QuestionId", typeof(int)));

            // Add prices in our list to our DataTable
            int id = 1;
            foreach (SelectedQuestion questionListitem in questionList)
            {
                DataRow row = table.NewRow();
                row["ID"] = id;
                row["ExamId"] = questionListitem.ExamId;
                row["QuestionId"] = questionListitem.QuestionId;
                table.Rows.Add(row);
                id++;
            }

            // Connect to DB
            string conString = SqlConnectionProvider.GetConnectionString(DataAccessType.Write);
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                // Execute the command to make a temp table
                SqlCommand cmd = new SqlCommand(tmpTable, con);
                cmd.ExecuteNonQuery();

                // BulkCopy the data in the DataTable to the temp table
                using (SqlBulkCopy bulk = new SqlBulkCopy(con))
                {
                    bulk.DestinationTableName = "#question_selected";
                    bulk.WriteToServer(table);
                }

                string mergeSql = string.Empty;
                if (examObj.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition))
                {
                    mergeSql = " DELETE FROM ExamQuestion WHERE ExamId = " + examObj.ExamId + " INSERT INTO ExamQuestion (ExamId,QuestionId) SELECT ExamId,QuestionId FROM #question_selected order by ID; ";
                }
                else
                {
                    // Now use the merge command to upsert from the temp table to the production table
                    mergeSql = "merge into ExamQuestion as Target " +
                                      "using #question_selected as Source " +
                                      "on " +
                                      "Target.ExamId=Source.ExamId " +
                                      "and Target.QuestionId = Source.QuestionId " +
                                      "when matched then " +
                                      "update set Target.QuestionId=Source.QuestionId " +
                                      "when not matched then " +
                                      "insert (ExamId,QuestionId) values (Source.ExamId,Source.QuestionId);";
                }
                cmd.CommandText = mergeSql;
                cmd.ExecuteNonQuery();

                // Additional condition for examObj.ExamType == 4 (Shared Exams)
                if (examObj.ExamType == 4)
                {
                    // Get all related exam IDs that were created by the stored procedure for shared users
                    // Use a safer approach to find related exams
                    string getRelatedExamsSql = @"SELECT ExamId FROM Exam 
                                         WHERE ExamName = @ExamName 
                                         AND ExamType = @ExamType 
                                         AND ExamId != @OriginalExamId
                                         AND ABS(ExamId - @OriginalExamId) <= 100"; // Safety check to avoid getting unrelated exams

                    cmd.CommandText = getRelatedExamsSql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ExamName", examObj.ExamName ?? "");
                    cmd.Parameters.AddWithValue("@ExamType", examObj.ExamType);
                    cmd.Parameters.AddWithValue("@OriginalExamId", examObj.ExamId);

                    List<int> relatedExamIds = new List<int>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            relatedExamIds.Add(reader.GetInt32(0));
                        }
                    }

                    // For each related exam, add the same questions
                    foreach (int relatedExamId in relatedExamIds)
                    {
                        // Use direct INSERT approach to avoid temp table issues
                        foreach (SelectedQuestion questionListitem in questionList)
                        {
                            string directInsertSql = "INSERT INTO ExamQuestion (ExamId, QuestionId) VALUES (@ExamId, @QuestionId)";
                            cmd.CommandText = directInsertSql;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@ExamId", relatedExamId);
                            cmd.Parameters.AddWithValue("@QuestionId", questionListitem.QuestionId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                // Clean up the first temp table
                cmd.CommandText = "drop table #question_selected";
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return 1;
        }
    }

    internal class SelectedQuestion
    {
        public int ExamId { get; set; }

        public int QuestionId { get; set; }
    }
}