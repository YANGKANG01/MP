using MP.Base.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MP.Base.Models
{
    public class BaseModel
    {
        #region 基类
        public class BaseRequest
        {

            /// <summary>
            /// 登录用户
            /// </summary>
            public int ActiveUser { get; set; }

            /// <summary>
            /// 用户所属单位
            /// </summary>
            public int CompanyIdentify { get; set; }

            /// <summary>
            /// 请求Header
            /// </summary>
            public ReqHeader Header { get; set; }
        }

        [Serializable]
        [DataContract]
        public class BaseResponse
        {
            /// <summary>
            /// 响应Header
            /// </summary>
            [DataMember(Name = "header")]
            public RespHeader Header { get; set; }
        }
        public class ReqHeader
        {
            /// <summary>
            /// 设备id
            /// </summary>
            public string DeviceId { get; set; }
            /// <summary>
            /// 设备名称
            /// </summary>
            public string DeviceName { get; set; }
            /// <summary>
            /// 操作系统及cpu名称
            /// </summary>
            public string OsName { get; set; }
            /// <summary>
            /// 浏览器的信息
            /// </summary>
            public string OsVersion { get; set; }
            /// <summary>
            /// 版本名称
            /// </summary>
            public string VersionName { get; set; }
            /// <summary>
            /// 版本号
            /// </summary>
            public string VersionCode { get; set; }
            /// <summary>
            /// 凭证
            /// </summary>
            public string Token { get; set; }
            /// <summary>
            /// 接口名称
            /// </summary>
            public string Cmd { get; set; }
            /// <summary>
            /// 用户来源(1-IOS,2-Android,3-客服端,4-Web)
            /// </summary>
            public int Source { get; set; }

            /// <summary>
            /// 签名（接口名称+账号标识+时间戳,以“|”分割）
            /// </summary>
            public string Sign { get; set; }

            /// <summary>
            /// 请求时间戳
            /// </summary>
            public int Timestamp { get; set; }
        }

        [Serializable]
        [DataContract]
        public class RespHeader
        {
            /// <summary>
            /// 状态码
            /// </summary>
            [DataMember]
            public int statusCode { get; set; }
            /// <summary>
            /// 状态信息
            /// </summary>
            [DataMember]
            public string statusMsg
            {
                get
                {
                    if (String.IsNullOrEmpty(_statusMsg))
                    {
                        return Helper.MsgCodeText(_msgCode);
                    }
                    return _statusMsg;
                }
                set
                {
                    _statusMsg = value;
                }
            }
            [DataMember]
            public string cmd { get; set; }

            private string _statusMsg;

            private MsgCode _msgCode;
            [DataMember(Name = "msgCode")]
            public MsgCode MsgCode
            {
                set { statusCode = (int)value; _msgCode = value; }
                get { return _msgCode; }
            }
        }

        #endregion
    }
}