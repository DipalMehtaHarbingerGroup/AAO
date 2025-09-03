namespace AAO.BAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using System;

    public class UserBL
    {
        public static List<UserDTO> GetUserDetails(int userId)
        {
            return UserDAL.GetByUserID(userId);
        }

        public static List<UserDTO> ValidateUser(string userName, string password)
        {
            return UserDAL.ValidateUser(userName, password);
        }

        public static string GetUserByRole(string userName)
        {
            return UserDAL.GetUserByRole(userName);
        }

        public static int CreateUser(UserDTO objUser)
        {
            return UserDAL.CreateUser(objUser);
        }

        public static List<UserDTO> GetUserList()
        {
            return UserDAL.GetUsers();
        }

        public static int UpdateUser(UserDTO objUser)
        {
            return UserDAL.UpdateUsers(objUser);
        }

        public static List<UserDataDTO> GetOrAddUser(UserJsonVM objUser)
        {
            return UserDAL.GetOrAddUser(objUser);
        }

        public static List<UserDataDTO> GetRenewal(ServiceCallVM userService)
        {
            return UserDAL.GetByUserData(Convert.ToInt32(userService.userId));
        }
    }
}
