using DDMessageCallback.Common;
using DDMessageCallback.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDMessageCallback.Controllers
{
    public class SuiteController : Controller
    {
        /// <summary>
        /// 注册回调事件 这里主要注册两种事件
        /// bpms_task_change :  审批任务开始，结束，转交
        /// bpms_instance_change：审批实例开始，结束
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Register()
        {
            DDHelper DDHelper = new DDHelper();
            //获取钉钉token
            string access_token = DDHelper.GetAccessToken();
            //注册时填写的token、aes_key、suitekey
            string token = ConfigurationManager.AppSettings["SuiteToken"];
            //token = "123456";//钉钉测试文档中的token
            string aes_key = ConfigurationManager.AppSettings["Suiteaes_key"];
            //aes_key = "4g5j64qlyl3zvetqxz5jiocdr586fn2zvjpa8zls3ij";//钉钉测试文档中的aes_key
            //准备钉钉回调事件类型 
            //bpms_task_change :  审批任务开始，结束，转交
            //bpms_instance_change：审批实例开始，结束
            string data = "{\"call_back_tag\":[\"bpms_task_change\",\"bpms_instance_change\"],\"token\":\"" + token + "\",\"aes_key\":\"" + aes_key + "\",\"url\":\"http://***/Suite/Receive\"}";
            string json = new DDHelper().Request("user/create?access_token=" + access_token, data, "POST");

            return Json(json);
        }
        /// <summary>
        /// 钉钉消息回调接口
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Receive(string signature, string timestamp, string nonce, PostBody body)
        {
            LogHelper.WriteProgramLogInFolder(DateTime.Now.ToString() + " 接收回调！", "DDCallBack");
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(900001, "加密明文文本非法");
            dic.Add(900002, "加密时间戳参数非法");
            dic.Add(900003, "加密随机字符串参数非法");
            dic.Add(900004, "不合法的aeskey");
            dic.Add(900005, "签名不匹配");
            dic.Add(900006, "计算签名错误");
            dic.Add(900007, "计算加密文字错误");
            dic.Add(900008, "计算解密文字错误");
            dic.Add(900009, "计算解密文字长度不匹配");
            dic.Add(900010, "计算解密文字suiteKey不匹配");

            //接收encrypt参数
            string encryptStr = body.encrypt.Replace("{\"encrypt\":\"", "").Replace("\"}", "");

            //注册时填写的token、aes_key、suitekey
            string token = ConfigurationManager.AppSettings["SuiteToken"];
            //token = "123456";//钉钉测试文档中的token
            string aes_key = ConfigurationManager.AppSettings["Suiteaes_key"];
            //aes_key = "4g5j64qlyl3zvetqxz5jiocdr586fn2zvjpa8zls3ij";//钉钉测试文档中的aes_key
            string suitekey = ConfigurationManager.AppSettings["DD_corpid"];
            //suitekey = "suite4xxxxxxxxxxxxxxx";//钉钉测试文档中的suitekey

            #region 验证回调的url
            DingTalkCrypt dingTalk = new DingTalkCrypt(token, aes_key, suitekey);
            string sEchoStr = "";
            int ret = dingTalk.VerifyURL(signature, timestamp, nonce, encryptStr, ref sEchoStr);
            if (ret != 0)
            {
                string message = "";
                if (dic.ContainsKey(ret))
                    message = dic.Where(e => e.Key == ret).FirstOrDefault().Value;

                LogHelper.WriteProgramLogInFolder(DateTime.Now.ToString() + " 验证回调发生错误，错误代码为：" + ret + " " + message, "DDCallBack");
                return Json(new object());
            }
            #endregion

            #region 解密接受信息，进行事件处理
            string plainText = "";
            ret = dingTalk.DecryptMsg(signature, timestamp, nonce, encryptStr, ref plainText);
            if (ret != 0)
            {
                string message = "";
                if (dic.ContainsKey(ret))
                    message = dic.Where(e => e.Key == ret).FirstOrDefault().Value;

                LogHelper.WriteProgramLogInFolder(DateTime.Now.ToString() + " 解密信息发生错误，错误代码为：" + ret + " " + message, "DDCallBack");
                return Json(new object());
            }

            Hashtable tb = (Hashtable)JsonConvert.DeserializeObject(plainText, typeof(Hashtable));
            string eventType = tb["EventType"].ToString();
            string res = "success";

            LogHelper.WriteProgramLogInFolder(DateTime.Now.ToString() + " 接收到的事件类型为：" + eventType, "DDCallBack");
            switch (eventType)
            {
                case "bpms_task_change"://审批任务开始，结束，转交
                    #region 执行代码 在此处添加业务逻辑代码，处理获取的审批单信息
                    ApproveModel modelTask = JsonConvert.DeserializeObject<ApproveModel>(plainText);
                    #endregion
                    break;
                case "bpms_instance_change"://审批实例开始，结束
                    #region 执行代码 在此处添加业务逻辑代码，处理获取的审批单信息
                    ApproveModel modelInstance = JsonConvert.DeserializeObject<ApproveModel>(plainText);
                    #endregion
                    break;
                default:
                    break;
            }

            timestamp = DateTime.Now.GetTimeStamp().ToString();
            string encrypt = "";
            string signature2 = "";
            dingTalk = new DingTalkCrypt(token, aes_key, suitekey);
            ret = dingTalk.EncryptMsg(res, timestamp, nonce, ref encrypt, ref signature2);
            if (ret != 0)
            {
                string message = "";
                if (dic.ContainsKey(ret))
                    message = dic.Where(e => e.Key == ret).FirstOrDefault().Value;

                LogHelper.WriteProgramLogInFolder(DateTime.Now.ToString() + " 解密信息发生错误，错误代码为：" + ret + " " + message, "DDUserInfoUpdate");
                return Json(new object());
            }

            Hashtable jsonMap = new Hashtable
                {
                    {"msg_signature", signature2},
                    {"encrypt", encrypt},
                    {"timeStamp", timestamp},
                    {"nonce", nonce}
                };

            return Json(jsonMap);
            #endregion
        }
    }
}