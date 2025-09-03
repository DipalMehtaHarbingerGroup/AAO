using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;
using Newtonsoft.Json;

namespace AAO.Apps.BCSCSelfAssessment.Proxy
{
    public static class HttpProxy
    {
        public static string HttpPost(string uri, string data, string contentType, string method = "POST")
        {
            HttpWebResponse response = null;
            try
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.ContentLength = dataBytes.Length;
                request.ContentType = contentType;
                request.Method = method;
                request.Timeout = 5000000;

                using (Stream requestBody = request.GetRequestStream())
                {
                    requestBody.Write(dataBytes, 0, dataBytes.Length);
                }
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }

                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    response = (HttpWebResponse)e.Response;
                    // Console.Write("Errorcode: {0}", (int)response.StatusCode); 
                    return null;
                }
                else
                {
                    Console.Write("Error: {0}", e.Status);
                    return null;
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    //return response;
                }
                //return 0;
            }
        }

        //GET METHOD

        public static string HttpGET(string uri, string contentType, string method = "GET")
        {
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.ContentType = contentType;
                request.Method = method;
                request.Timeout = 5000000;

                //using (Stream requestBody = request.GetRequestStream())
                //{
                //    //requestBody.Write(dataBytes, 0, dataBytes.Length);
                //}
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }

                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    response = (HttpWebResponse)e.Response;
                    //Console.Write("Errorcode: {0}", (int)response.StatusCode); 
                    return null;
                }
                else
                {
                    Console.Write("Error: {0}", e.Status);
                    return null;
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    //return response;
                }
                //return 0;
            }
        }

        //internal static string HttpGET(string url, string questuionTypeDataJson, string v1, string v2)
        //{
        //    throw new NotImplementedException();
        //}


        public static async Task<ServiceResponse> RoutePostRequest(string endpoint, string method, string content)
        {
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



                    var response = await client.PostAsync(endpoint, contentbyte);
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
    }

    public class ServiceResponse
    {
        public string JsonContent { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; }

        public static implicit operator string(ServiceResponse v)
        {
            throw new NotImplementedException();
        }
    }
}
