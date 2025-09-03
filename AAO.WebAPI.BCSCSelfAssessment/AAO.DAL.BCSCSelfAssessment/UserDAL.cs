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

    public static class UserDAL
    {
        public static List<UserDTO> GetByUserID(int userId)
        {
            List<UserDTO> lstUser = new List<UserDTO>();
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@userId", userId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetUsersByID", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    UserDTO objUserBO = new UserDTO();
                    objUserBO.UserId = Convert.ToInt32(objSqlDataReader["UserId"]);
                    objUserBO.FirstName = Convert.ToString(objSqlDataReader["FirstName"]);
                    objUserBO.LastName = Convert.ToString(objSqlDataReader["LastName"]);
                    objUserBO.UserEmail = Convert.ToString(objSqlDataReader["Email"]);
                    objUserBO.RegTime = (DateTime)objSqlDataReader["RegTime"];
                    objUserBO.Role = Convert.ToString(objSqlDataReader["Role"]);
                    objUserBO.isLoggedFirst = (bool)objSqlDataReader["isLoggedFirst"];
                    object renewalMessageObj = objSqlDataReader["RenewalMessage"];
                    objUserBO.RenewalMessage = renewalMessageObj is DBNull ? null : Convert.ToString(objSqlDataReader["RenewalMessage"]);
                    object renewalLinkObj = objSqlDataReader["RenewalLink"];
                    lstUser.Add(objUserBO);
                }
            }

            return lstUser;
        }

        public static List<UserDataDTO> GetByUserData(int userId)
        {
            List<UserDataDTO> lstUser = new List<UserDataDTO>();
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@userId", userId),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetUsersByID", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    UserDataDTO objUserBO = new UserDataDTO();
                    objUserBO.UserId = Convert.ToInt32(objSqlDataReader["UserId"]);
                    objUserBO.FirstName = Convert.ToString(objSqlDataReader["FirstName"]);
                    objUserBO.LastName = Convert.ToString(objSqlDataReader["LastName"]);
                    objUserBO.Email = Convert.ToString(objSqlDataReader["Email"]);
                    objUserBO.RegTime = (DateTime)objSqlDataReader["RegTime"];
                    objUserBO.Role = Convert.ToString(objSqlDataReader["Role"]);
                    objUserBO.isLoggedFirst = (bool)objSqlDataReader["isLoggedFirst"];

                    object renewalMessageObj = objSqlDataReader["RenewalMessage"];
                    objUserBO.RenewalMessage = renewalMessageObj is DBNull ? null : Convert.ToString(objSqlDataReader["RenewalMessage"]);

                    object renewalLinkObj = objSqlDataReader["RenewalLink"];
                    objUserBO.RenewalLink = renewalMessageObj is DBNull ? null : Convert.ToString(objSqlDataReader["RenewalLink"]);
                    lstUser.Add(objUserBO);
                }
            }

            return lstUser;
        }

        public static List<UserDTO> GetUsers()
        {
            List<UserDTO> lstUser = new List<UserDTO>();
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetUsers"))
            {
                while (objSqlDataReader.Read())
                {
                    UserDTO objUserBO = new UserDTO();
                    objUserBO.UserId = Convert.ToInt32(objSqlDataReader["UserId"]);
                    objUserBO.FirstName = Convert.ToString(objSqlDataReader["FirstName"]);
                    objUserBO.LastName = Convert.ToString(objSqlDataReader["LastName"]);
                    objUserBO.ProgramDirectorProgramID = Convert.ToInt32(objSqlDataReader["ProgramDirectorProgramID"]);
                    objUserBO.IsActive = (bool)objSqlDataReader["IsActive"];
                    objUserBO.RegTime = (DateTime)objSqlDataReader["RegTime"];
                    objUserBO.Role = Convert.ToString(objSqlDataReader["Role"]);
                    objUserBO.isLoggedFirst = (bool)objSqlDataReader["isLoggedFirst"];
                    lstUser.Add(objUserBO);
                }
            }

            return lstUser;
        }

        public static List<UserDTO> ValidateUser(string userName, string password)
        {
            List<UserDTO> lstUser = new List<UserDTO>();
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@UserName", userName),
                                                new SqlParameter("@Password", password),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_ValidateUser", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    UserDTO objCandidateBO = new UserDTO();
                    objCandidateBO.UserId = Convert.ToInt32(objSqlDataReader["UserId"]);
                    objCandidateBO.FirstName = Convert.ToString(objSqlDataReader["FirstName"]);
                    objCandidateBO.LastName = Convert.ToString(objSqlDataReader["LastName"]);
                    objCandidateBO.UserEmail = Convert.ToString(objSqlDataReader["Email"]);
                    objCandidateBO.ProgramDirectorProgramID = Convert.ToInt32(objSqlDataReader["ProgramDirectorProgramID"]);
                    objCandidateBO.RegTime = (DateTime)objSqlDataReader["RegTime"];
                    objCandidateBO.Role = Convert.ToString(objSqlDataReader["Role"]);
                    objCandidateBO.isLoggedFirst = (bool)objSqlDataReader["isLoggedFirst"];
                    lstUser.Add(objCandidateBO);
                }
            }

            return lstUser;
        }

        // Get Roles
        public static string GetUserByRole(string userName)
        {
            string role = string.Empty;
            List<UserDTO> lstUser = new List<UserDTO>();
            SqlParameter[] objSqlParameter =
            {
                                                new SqlParameter("@UserName", userName),
                                             };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_ValidateUser_Role", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    role = Convert.ToString(objSqlDataReader["Role"]);
                }
            }

            // return lstUser;
            return role;
        }

        public static int CreateUser(UserDTO objUser)
        {
            SqlParameter[] arrSqlParameter =
            {
                                              new SqlParameter("@UserName", objUser.FirstName),
                                              new SqlParameter("@UserName", objUser.LastName),
                                              new SqlParameter("@Email", objUser.UserEmail),
                                              new SqlParameter("@Password", "Test_1234"),
                                              new SqlParameter("@ProgramDirectorUserId", 1),
                                              new SqlParameter("@IsActive", 1),
                                              new SqlParameter("@RegTime", DateTime.Now),
                                              new SqlParameter("@Role", 'U'),
                                              new SqlParameter("@isLoggedFirst", 0),
            };

            return SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_CreateUser", arrSqlParameter);
        }

        public static int UpdateUsers(UserDTO objUser)
        {
            SqlParameter[] arrSqlParameter =
            {
                                              new SqlParameter("@isLoggedFirst", objUser.isLoggedFirst),
                                              new SqlParameter("@UserId", objUser.UserId),
            };

            return SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_UpdateUser", arrSqlParameter);
        }

        public static List<UserDataDTO> GetOrAddUser(UserJsonVM userData)
        {
            List<UserDataDTO> lstUser = new List<UserDataDTO>();
            SqlParameter[] arrSqlParameter =
            {
                     new SqlParameter("@MasterCustomerID", userData.MasterCustomerId),
                     new SqlParameter("@FirstName", userData.FirstName),
                     new SqlParameter("@LastName", userData.LastName),
                     new SqlParameter("@ResidencyStart", userData.ResidencyStart),
                     new SqlParameter("@ResidencyEnd", userData.ResidencyEnd),
                     new SqlParameter("@ResidencyProgramName", userData.ResidencyProgramName),
                     new SqlParameter("@ResidencyProgramID", userData.ResidencyProgramId),
                     new SqlParameter("@ProgramDirectorProgramName", userData.ProgramDirectorProgramName),
                     new SqlParameter("@ProgramDirectorProgramID", userData.ProgramDirectorProgramId),
                     new SqlParameter("@Email", userData.PrimaryEmail),
                     new SqlParameter("@RenewalMessage", userData.RenewalMessage),
                     new SqlParameter("@RenewalLink", userData.RenewalLink),
               };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetUsers", arrSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    UserDataDTO objUserBO = new UserDataDTO();
                    objUserBO.UserId = Convert.ToInt32(objSqlDataReader["UserId"]);
                    objUserBO.MasterCustomerId = Convert.ToString(objSqlDataReader["MasterCustomerID"]);
                    objUserBO.FirstName = Convert.ToString(objSqlDataReader["FirstName"]);
                    objUserBO.LastName = Convert.ToString(objSqlDataReader["LastName"]);

                    object residencyStartObj = objSqlDataReader["ResidencyStart"];
                    if (residencyStartObj is DBNull)
                    {
                        objUserBO.ResidencyStart = null;
                    }
                    else
                    {
                        objUserBO.ResidencyStart = (DateTime)(objSqlDataReader["ResidencyStart"]);
                    }

                    object residencyEndObj = objSqlDataReader["ResidencyEnd"];
                    if (residencyEndObj is DBNull)
                    {
                        objUserBO.ResidencyEnd = null;
                    }
                    else
                    {
                        objUserBO.ResidencyEnd = (DateTime)objSqlDataReader["ResidencyEnd"];
                    }

                    object residencyProgramNameObj = objSqlDataReader["ResidencyProgramName"];
                    objUserBO.ResidencyProgramName = residencyProgramNameObj is DBNull ? null : Convert.ToString(objSqlDataReader["ResidencyProgramName"]);
                    object residencyProgramIDObj = objSqlDataReader["ResidencyProgramID"];
                    objUserBO.ResidencyProgramId = residencyProgramIDObj is DBNull ? null : Convert.ToString(objSqlDataReader["ResidencyProgramID"]);
                    object programDirectorProgramNameObj = objSqlDataReader["ProgramDirectorProgramName"];
                    objUserBO.ProgramDirectorProgramName = programDirectorProgramNameObj is DBNull ? null : Convert.ToString(objSqlDataReader["ProgramDirectorProgramName"]);
                    object programDirectorProgramIDObj = objSqlDataReader["ProgramDirectorProgramID"];
                    objUserBO.ProgramDirectorProgramId = programDirectorProgramIDObj is DBNull ? null : Convert.ToString(objSqlDataReader["ProgramDirectorProgramID"]);
                    object emailObj = objSqlDataReader["Email"];
                    objUserBO.Email = emailObj is DBNull ? null : Convert.ToString(objSqlDataReader["Email"]);
                    objUserBO.IsActive = (bool)objSqlDataReader["IsActive"];
                    objUserBO.RegTime = (DateTime)objSqlDataReader["RegTime"];
                    objUserBO.Role = Convert.ToString(objSqlDataReader["Role"]);
                    objUserBO.isLoggedFirst = (bool)objSqlDataReader["isLoggedFirst"];
                    object renewalMessageObj = objSqlDataReader["RenewalMessage"];
                    objUserBO.RenewalMessage = renewalMessageObj is DBNull ? null : Convert.ToString(objSqlDataReader["RenewalMessage"]);
                    object renewalLinkObj = objSqlDataReader["RenewalLink"];
                    objUserBO.RenewalLink = renewalLinkObj is DBNull ? null : Convert.ToString(objSqlDataReader["RenewalLink"]);
                    lstUser.Add(objUserBO);
                }
            }

            return lstUser;
        }
    }
}