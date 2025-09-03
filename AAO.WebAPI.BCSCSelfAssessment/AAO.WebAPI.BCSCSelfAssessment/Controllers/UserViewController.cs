using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    public class UserViewController : ApiController
    {

        [Route("api/UserView/GetQuestionById")]
        [HttpPost]
        public QuestionDetails GetQuestionById(UpdateSkipAnswered question)
        {
            return UserViewBL.GetQuestionById(question.questionId, question.userId);
        }
    }
}
