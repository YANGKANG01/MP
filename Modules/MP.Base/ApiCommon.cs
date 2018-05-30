using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using static MP.Base.Models.BaseModel;

namespace MP.Base
{
    /// <summary>
    /// http请求类
    /// </summary>
    public class ApiCommon
    {
        private string _baseUrl = "http://172.16.10.54:54/";

        private readonly HttpContext _httpContext;
        public ApiCommon(string baseUrl, string token, HttpContext httpContext) : this(baseUrl, 0, token)
        {
            _httpContext = httpContext;
        }

        public ApiCommon(string baseUrl, int identify, string token)
        {
            _baseUrl = baseUrl;
            Request = new BaseRequest
            {
                ActiveUser = identify,
                Header = new ReqHeader
                {
                    Token = token,
                    DeviceId = "web",
                    DeviceName = _httpContext.Request.ToString(),
                    OsName = Environment.OSVersion.ToString(),
                    // OsVersion = HttpContext.Request.Browser.Version.ToString() + " " + HttpContext.Current.Request.Browser.Platform.ToString(),
                    Source = 4
                }
            };
            Response = new BaseResponse();
        }

        /// <summary>
        /// 请求对象
        /// </summary>
        public BaseRequest Request { get; set; }

        /// <summary>
        /// 请求响应
        /// </summary>
        public BaseResponse Response { get; set; }

        public void ExecutePost()
        {
            Execute(HttpMethod.POST);
        }
        public T ExecutePost<T>(Dictionary<String, Object> args = null)
            where T : class
        {
            return Execute<T>(HttpMethod.POST, args, string.Empty);
        }

        public T ExecutePost<T, P>(P postEntity, Dictionary<String, Object> args = null)
            where T : class
            where P : class
        {
            var body = string.Empty;
            if (postEntity != null && typeof(P).Name != "String")
            {
                body = JsonConvert.SerializeObject(postEntity);
            }
            else
            {
                body = postEntity as string;
            }
            return Execute<T>(HttpMethod.POST, args, body);
        }
        public void ExecutePost<P>(P postEntity, Dictionary<String, Object> args = null) where P : class
        {
            var body = "";
            if (postEntity != null)
            {
                body = JsonConvert.SerializeObject(postEntity);
            }
            Execute<NoResponse>(HttpMethod.POST, args, body);
        }

        public T ExecuteGet<T>(Dictionary<String, Object> args = null) where T : class
        {
            return Execute<T>(HttpMethod.GET, args, string.Empty);
        }
        public void ExecuteGet(Dictionary<String, Object> args = null)
        {
            Execute<NoResponse>(HttpMethod.GET, args, string.Empty);
        }

        private T Execute<T>(HttpMethod httpMethod, Dictionary<String, Object> args, string body) where T : class
        {
            var queryString = "";
            if (args != null)
            {
                queryString = ToQueryString(args);
            }
            if (!String.IsNullOrEmpty(queryString))
            {
                queryString = "?" + queryString;
            }
            var uri = new Uri(_baseUrl + Request.Header.Cmd + queryString);
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);

            webRequest.ContentType = "application/json";
            webRequest.Accept = "application/json";
            webRequest.Method = httpMethod.ToString();
            webRequest.Headers.Add("ActiveIdentify", Request.ActiveUser.ToString());
            //Request.Header.Timestamp = ConvertHelper.UnixTimestamp.FromDateTime(DateTime.Now);
            Request.Header.Sign = DesEncrypt(string.Format("{0}|{1}|{2}", Request.Header.Cmd, Request.ActiveUser, Request.Header.Timestamp), Request.Header.Token);
            webRequest.Headers.Add("Header", JsonConvert.SerializeObject(Request.Header));

            if (httpMethod == HttpMethod.POST && !String.IsNullOrEmpty(body))
            {
                using (Stream stream = webRequest.GetRequestStream())
                {
                    var jsonByte = Encoding.UTF8.GetBytes(body);
                    stream.Write(jsonByte, 0, jsonByte.Length);
                }
            }
            else
            {
                webRequest.ContentLength = 0;
            }
            var webResponse = webRequest.GetResponse();
            using (var myStreamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
            {
                var result = JsonConvert.DeserializeObject<RespResult>(myStreamReader.ReadToEnd());
                Response.Header = result.Header;
                if (IsBulitinType(typeof(T)))
                {
                    return (T)result.Data;
                }
                else
                {
                    return result.Data == null ? null : JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(result.Data));
                }
            }
        }
        private void Execute(HttpMethod httpMethod)
        {
            var uri = new Uri(_baseUrl + Request.Header.Cmd);
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.ContentType = "application/json";
            webRequest.Accept = "application/json";
            webRequest.Method = httpMethod.ToString();
            webRequest.Headers.Add("ActiveIdentify", Request.ActiveUser.ToString());
            // Request.Header.Timestamp = ConvertHelper.UnixTimestamp.FromDateTime(DateTime.Now);
            Request.Header.Sign = DesEncrypt(string.Format("{0}|{1}|{2}", Request.Header.Cmd, Request.ActiveUser, Request.Header.Timestamp), Request.Header.Token);
            webRequest.Headers.Add("Header", JsonConvert.SerializeObject(Request.Header));
            webRequest.ContentLength = 0;
            var webResponse = webRequest.GetResponse();
            using (var myStreamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
            {
                var result = JsonConvert.DeserializeObject<RespResult>(myStreamReader.ReadToEnd());
                Response.Header = result.Header;
            }
        }

        private string ToQueryString(Dictionary<String, Object> args)
        {
            var values = args.Where(a => a.Value != null);
            //.Select(a => String.Format("{0}={1}", a.Key, HttpUtility.UrlEncode(a.Value.ToString()))).ToArray();

            return String.Join("&", values);
        }

        private abstract class NoResponse
        {

        }

        private class RespResult
        {
            public RespHeader Header { get; set; }

            public object Data { get; set; }
        }

        static private bool IsBulitinType(Type type)
        {
            return (type == typeof(object) || Type.GetTypeCode(type) != TypeCode.Object);
        }

        /// <summary>
        /// TripleDES 加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        private static string DesEncrypt(string toEncrypt, string key)
        {
            var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            var keyArray = Convert.FromBase64String(key);
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
    public enum HttpMethod
    {
        POST,
        GET,
        PUT,
    }
}