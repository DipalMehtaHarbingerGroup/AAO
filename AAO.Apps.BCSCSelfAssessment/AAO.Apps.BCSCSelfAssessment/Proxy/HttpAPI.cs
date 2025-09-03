using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AAO.Apps.BCSCSelfAssessment.Controllers;
using AAO.Common.BCSCSelfAssessment;
using Microsoft.Exchange.WebServices.Data;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace AAO.Apps.BCSCSelfAssessment.Proxy
{
    public class HttpAPI
    {
        public async Task<ServiceResponse> RoutePostRequest(string endpoint, string method, string content)
        {
            await Task.Delay(1000);
            ServiceResponse result = new ServiceResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(endpoint);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    //client.DefaultRequestHeaders.TryAddWithoutValidation("icSessionId", icSessionId);

                    var stringContent = ConvertObjectToString(content);
                    byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(stringContent);
                    var contentbyte = new ByteArrayContent(messageBytes);
                    contentbyte.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    
                    
                    
                    var response = await client.PostAsync(endpoint, contentbyte).ConfigureAwait(false);
                    result.StatusCode = response.StatusCode;
                    result.IsSuccess = response.IsSuccessStatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        result.JsonContent = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.JsonContent = ex.Message;
            }
            return result;
        }
        public static string ConvertObjectToString<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        //public Task<List<AdminExamQuestionReport>> GetList(string url,string data)
        //{
        //    List<AdminExamQuestionReport> quereport = new List<AdminExamQuestionReport>();
        //    Task<ServiceResponse> task = Task.Run(() =>  RoutePostRequest(url, "POST", data));

        // quereport = JsonConvert.DeserializeObject <Task<List<AdminExamQuestionReport>>>(task.JsonContent);
        //    return quereport;
        //}

        //public async List<AdminExamQuestionReport> DoWork(int tbl)
        //{
        //    List<AdminExamQuestionReport> quereportList = await Task.Run(() => GetData(tbl));
        //   return quereportList;
        //}

        public List<AdminExamQuestionReport> GetData(int tbl)
        {
            List<AdminExamQuestionReport> quereportList = new List<AdminExamQuestionReport>();
            try
            {
                string url = AAOGlobalConstants.SiteWebAPIUrl + "AdminQuestionPerformance/GetAllAnsweredQuestionExport1";
                int tablecount = tbl;
                int startCount = 1;
                int recordCount = Convert.ToInt32(ConfigurationManager.AppSettings["RecordPerAPICall"]);
                int endCount = recordCount;
                for (int i = 1; i <= tablecount; i++)
                {
                    AdminQuestionPerformanceVM queDetails = new AdminQuestionPerformanceVM()
                    {
                        StartCount = startCount,
                        EndCount = endCount,
                    };
                    string examPostDataJson = JsonConvert.SerializeObject(queDetails);
                    string res = RoutePostRequest1(url, "POST", examPostDataJson);
                    startCount = endCount + 1;
                    endCount = startCount + recordCount;
                    List<AdminExamQuestionReport> quereport = new List<AdminExamQuestionReport>();
                    quereport = JsonConvert.DeserializeObject<List<AdminExamQuestionReport>>(res);
                    if (quereport != null)
                    {
                        quereportList.AddRange(quereport);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new NotImplementedException();
            }
            // AdminQuestionPerformanceController aa = new AdminQuestionPerformanceController();
            // aa.ExcelGeneration(quereportList);
            return quereportList;
        }

        public string RoutePostRequest1(string endpoint, string method, string content)
        {
            Stream webStream = null;
            string result = string.Empty;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(endpoint);
                webRequest.Method = method;
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = content.Length;
                webRequest.Timeout = 60000;
                webRequest.KeepAlive = true;
                if (!string.IsNullOrEmpty(content))
                {
                    webStream = webRequest.GetRequestStream();
                    using (StreamWriter requestWriter = new StreamWriter(webStream, Encoding.ASCII))
                    {
                        requestWriter.Write(content);
                    }
                }
                HttpWebResponse webresponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader responseStream = new StreamReader(webresponse.GetResponseStream());
                result = responseStream.ReadToEnd();

            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.JsonContent = ex.Message;
            }
            return result;

        }

    }
}