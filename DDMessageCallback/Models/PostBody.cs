using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDMessageCallback.Models
{
    public class PostBody
    {
        //{ "encrypt":"1ojQf0N..." }回调消息体使用Post请求body格式传递
        public string encrypt { get; set; }
    }
}