using AAO.Common.BCSCSelfAssessment;
using AAO.DAL.BCSCSelfAssessment;
using AAO.DTO.BCSCSelfAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAO.BAL.BCSCSelfAssessment
{
    public class NotificationMessageSettingBL
    {
        public static List<NotificationMessageSetting> GetAllNotificationMessageSetting(SearchParameters queDetails)
        {
            return NotificationMessageSettingDAL.GetAllNotificationMessageSetting(queDetails);
        }
        public static List<NotificationMessageSetting> GetAllMessageSettingByUser(SearchParameters queDetails)
        {
            return NotificationMessageSettingDAL.GetAllMessageSettingByUser(queDetails);
        }

        public static NotificationMessageSetting GetNotificationMessageSetting(SearchParameters queDetails)
        {
            return NotificationMessageSettingDAL.GetNotificationMessageSetting(queDetails);
        }
        public static void SaveNotificationMessageSetting(NotificationMessageSetting notificationMessageSetting)
        {
            NotificationMessageSettingDAL.SaveNotificationMessageSetting(notificationMessageSetting);
        }

        public static bool DeleteMessageSetting(SearchParameters queDetails)
        {
            return NotificationMessageSettingDAL.DeleteMessageSetting(queDetails);
        }
        public static bool InsertOrUpdateUserMessageSetting(SearchParameters queDetails)
        {
            return NotificationMessageSettingDAL.InsertOrUpdateUserMessageSetting(queDetails);
        }

        public static ExamDTO GetExamData(SearchParameters loginDetails)
        {
            return NotificationMessageSettingDAL.GetExamData(loginDetails);
        }
    }
}
