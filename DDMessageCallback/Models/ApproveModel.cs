using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDMessageCallback.Models
{
    public class ApproveModel
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public string EventType { get; set; }
        /// <summary>
        /// 审批实例id
        /// </summary>
        public string processInstanceId { get; set; }
        /// <summary>
        /// 审批实例对应的企业
        /// </summary>
        public string corpId { get; set; }
        /// <summary>
        /// 实例创建时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 实例标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 类型  start
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 发起审批实例的员工
        /// </summary>
        public string staffId { get; set; }
        /// <summary>
        /// 审批实例url，可在钉钉内跳转到审批页面
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 审批模板的唯一码
        /// </summary>
        public string processCode { get; set; }
    }
}