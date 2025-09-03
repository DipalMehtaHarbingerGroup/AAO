using AAO.BAL.BCSCSelfAssessment;
using AAO.Common.BCSCSelfAssessment;
using AAO.DTO.BCSCSelfAssessment;
using System.Collections.Generic;
using System.Web.Http;

namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    public class NotificationMessageSettingController : ApiController
    {
        // GET: NotificationMessageSetting
        [Route("api/NotificationMessageSetting/GetAllNotificationMessageSetting")]
        [HttpPost]
        public List<NotificationMessageSetting> GetAllNotificationMessageSetting(SearchParameters queDetails)
        {
            return NotificationMessageSettingBL.GetAllNotificationMessageSetting(queDetails);
        }

        [Route("api/NotificationMessageSetting/GetAllMessageSettingByUser")]
        [HttpPost]
        public List<NotificationMessageSetting> GetAllMessageSettingByUser(SearchParameters queDetails)
        {
            return NotificationMessageSettingBL.GetAllMessageSettingByUser(queDetails);
        }

        [Route("api/NotificationMessageSetting/GetNotificationMessageSetting")]
        [HttpPost]
        public NotificationMessageSetting GetNotificationMessageSetting(SearchParameters queDetails)
        {
            return NotificationMessageSettingBL.GetNotificationMessageSetting(queDetails);
        }

        [Route("api/NotificationMessageSetting/SaveNotificationMessageSetting")]
        [HttpPost]
        public void SaveNotificationMessageSetting(NotificationMessageSetting notificationMessageSetting)
        {
            NotificationMessageSettingBL.SaveNotificationMessageSetting(notificationMessageSetting);
        }

        [Route("api/NotificationMessageSetting/DeleteMessageSetting")]
        [HttpPost]
        public bool DeleteMessageSetting(SearchParameters queDetails)
        {
            return NotificationMessageSettingBL.DeleteMessageSetting(queDetails);
        }

        [Route("api/NotificationMessageSetting/InsertOrUpdateUserMessageSetting")]
        [HttpPost]
        public bool InsertOrUpdateUserMessageSetting(SearchParameters queDetails)
        {
            return NotificationMessageSettingBL.InsertOrUpdateUserMessageSetting(queDetails);
        }

        [Route("api/NotificationMessageSetting/GetExamData")]
        [HttpPost]
        public ExamDTO GetExamData(SearchParameters loginDetails)
        {
            return NotificationMessageSettingBL.GetExamData(loginDetails);
        }
    }
}