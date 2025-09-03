using AAO.Apps.BCSCSelfAssessment.Proxy;
using AAO.Common.BCSCSelfAssessment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    public class NotificationMessageSettingController : BaseAdminController
    {
        // GET: NotificationMessageSetting
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetAllNotificationMessageSetting()
        {
            try
            {
                int count = 0;
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);

                SearchParameters searchParameters = new SearchParameters()
                {
                    OffsetValue = skip,
                    PagingSize = pageSize,
                    Search = searchValue,
                };

                JsonResult dataresult = new JsonResult();
                string examPostDataJson = JsonConvert.SerializeObject(searchParameters);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "NotificationMessageSetting/GetAllNotificationMessageSetting";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                List<NotificationMessageSetting> report = new List<NotificationMessageSetting>();
                report = JsonConvert.DeserializeObject<List<NotificationMessageSetting>>(result);
                if (report.Any())
                {
                    count = report.FirstOrDefault().Total;
                }
                int totalRecords = count;
                int recFilter = count;
                report = report.Skip(startRec).Take(pageSize).ToList();
                dataresult = Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = report }, JsonRequestBehavior.AllowGet);

                return (dataresult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        public ActionResult GetMessageSettingForm(int id)
        {
            NotificationMessageSetting notificationMessage = new NotificationMessageSetting();
            notificationMessage.NotificationMessageSettingId = 0;
            if (id != 0)
            {
                SearchParameters searchParameters = new SearchParameters()
                {
                    Id = id
                };
                string examPostDataJson = JsonConvert.SerializeObject(searchParameters);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "NotificationMessageSetting/GetNotificationMessageSetting";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                notificationMessage = JsonConvert.DeserializeObject<NotificationMessageSetting>(result);
            }
            else
            {
                notificationMessage.IsUnable = true;
            }
            return View(notificationMessage);
        }

        [HttpPost]
        public ActionResult DeleteMessageSetting(int id)
        {
            bool isSucess = true;
            var msg = string.Empty;
            if (id != 0)
            {
                SearchParameters searchParameters = new SearchParameters()
                {
                    Id = id,
                };
                string examPostDataJson = JsonConvert.SerializeObject(searchParameters);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "NotificationMessageSetting/DeleteMessageSetting";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                bool isDelete = JsonConvert.DeserializeObject<bool>(result);
                if (isDelete)
                {
                    msg = "Message settings deleted successfully.";
                }
                else
                {
                    isSucess = false;
                    msg = "Error while deleting message setting.";
                }
            }
            else
            {
                isSucess = false;
                msg = "Error while deleting message setting.";
            }
            return Json(new { msg, isSucess });
        }


        [HttpPost]
        public ActionResult SaveNotificationMessageSetting(int NotificationMessageSettingId, string NotificationMessage, int NoOfTime, int OnholdScreenTime, int IsEnable) //int NotificationMessageSettingId,string NotificationMessage,int NoOfTime, int OnholdScreenTime, bool IsEnable
        {
            bool isSucess = true;
            var msg = string.Empty;
            if (!string.IsNullOrEmpty(NotificationMessage))
            {
                NotificationMessageSetting notificationMessage = new NotificationMessageSetting()
                {
                    NotificationMessageSettingId = NotificationMessageSettingId,
                    NotificationMessage = NotificationMessage,
                    NoOfTime = NoOfTime,
                    OnholdScreenTime = 0,
                    IsUnable = IsEnable == 1 ? true : false,
                    UserId = (int?)null
                };
                string examPostDataJson = JsonConvert.SerializeObject(notificationMessage);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "NotificationMessageSetting/SaveNotificationMessageSetting";
                HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
            }
            return Json(new { msg, isSucess });
        }
    }
}