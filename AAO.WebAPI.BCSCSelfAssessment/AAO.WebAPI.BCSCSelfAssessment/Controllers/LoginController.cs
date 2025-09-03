namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    // [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [Route("api/login/ValidateUser")]
        [HttpPost]
        public List<UserDTO> ValidateUser(UserDTO user)
        {
            return UserBL.ValidateUser(user.UserEmail, user.Password);
        }

        [Route("api/login/UpdateUser")]
        [HttpPost]
        public int UpdateUsers(UserDTO user)
        {
            return UserBL.UpdateUser(user);
        }

        [Route("api/login/GetUserByRole")]
        [HttpPost]
        public string GetUserByRole(UserVM userName)
        {
            return UserBL.GetUserByRole(userName.UserName);
        }
    }
}
