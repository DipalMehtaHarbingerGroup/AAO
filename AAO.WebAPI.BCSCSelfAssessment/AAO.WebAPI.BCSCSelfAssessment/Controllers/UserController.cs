namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    public class UserController : ApiController
    {
        [Route("api/User/GetOrAddUser")]
        [HttpPost]
        public List<UserDataDTO> GetOrAddUser(UserJsonVM userDTO)
        {
            return UserBL.GetOrAddUser(userDTO);
        }
    }
}
