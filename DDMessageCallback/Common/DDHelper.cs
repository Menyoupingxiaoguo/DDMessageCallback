using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace DDMessageCallback.Common
{
    public class DDHelper
    {
        public string dd_host = ConfigurationManager.AppSettings["DDHost"];
        public string dd_corpid = ConfigurationManager.AppSettings["DD_corpid"];
        public string dd_corpsecret = ConfigurationManager.AppSettings["DD_corpsecret"];
        public string dd_accesstoken = string.Empty;

        /// <summary>
        /// 发起请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="data">数据</param>
        /// <param name="reqtype">请求类型</param>
        /// <returns></returns>
        public string Request(string url, string data, string reqtype)
        {
            if (url.IndexOf('?') == -1 && url != "gettoken")
                url += ("?access_token=" + dd_accesstoken);
            else if (url.IndexOf('?') > -1 && url.IndexOf("gettoken") == -1)
                url += ("&access_token=" + dd_accesstoken);
            HttpWebRequest web = (HttpWebRequest)HttpWebRequest.Create(dd_host + url);
            web.ContentType = "application/json";
            web.Method = reqtype;
            if (data.Length > 0 && reqtype.Trim().ToUpper() == "POST")
            {
                byte[] postBytes = Encoding.UTF8.GetBytes(data);
                web.ContentLength = postBytes.Length;
                using (Stream reqStream = web.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }
            }
            string html = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)web.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                html = streamReader.ReadToEnd();
            }
            return html;
        }
        /// <summary>
        /// 更新AccessToken
        /// </summary>
        public string GetAccessToken()
        {
            //从缓存中获取Token,如果缓存中已经过期，再从接口获取;
            object DDToken = CacheHelper.GetCache("dd_accesstoken");
            if (DDToken == null)
            {
                //获取Token;
                dd_accesstoken = JsonConvert.DeserializeObject<AccessTokenModel>(Request("gettoken?corpid=" + dd_corpid + "&corpsecret=" + dd_corpsecret, "", "GET")).access_token;
                //将Token存入缓存
                CacheHelper.AddCache("dd_accesstoken", dd_accesstoken, 115);
            }
            else
                dd_accesstoken = DDToken.ToString();

            return dd_accesstoken;
        }
    }

    public class AccessTokenModel
    {
        public string access_token { get; set; }

        public int errcode { get; set; }

        public string errmsg { get; set; }
    }
}