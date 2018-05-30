using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Base.Enums
{
    public enum MsgCode
    {

        /// <summary>
        /// 请求超时
        /// </summary>
        ReuestTimeout = 996,

        /// <summary>
        /// 应用程序异常
        /// </summary>
        ServiceException = 997,
        /// <summary>
        /// 非法请求
        /// </summary>
        BadRequest = 998,
        /// <summary>
        /// 未知错误
        /// </summary>
        Unknow = 999,
        /// <summary>
        /// 请求成功
        /// </summary>
        Success = 1000,
        /// <summary>
        /// 账号冻结
        /// </summary>
        AccountFreeze = 1001,
        /// <summary>
        /// 账号未激活
        /// </summary>
        AccountNotActive = 1002,

        /// <summary>
        /// 账号已被删除
        /// </summary>
        AccountDeleted = 1027,

        /// <summary>
        /// 用户名或密码不正确
        /// </summary>
        AccountPswErr = 1003,
        /// <summary>
        /// 登录失败
        /// </summary>
        AccountLoginErr = 1004,
        /// <summary>
        /// 未登录
        /// </summary>
        NeedLogin = 1005,

        /// <summary>
        /// 其他设备登陆
        /// </summary>
        OtherLogin = 1006,
        /// <summary>
        /// 当前订单状态不能取消订单
        /// </summary>
        OrderCannotCancel = 1007,
        /// <summary>
        /// 设备名称已存在
        /// </summary>
        DeviceNameExists = 1008,
        /// <summary>
        /// 不能为空
        /// </summary>
        NonEmpty = 1009,
        /// <summary>
        /// 余额不足
        /// </summary>
        BalanceInsufficient = 1010,
        /// <summary>
        /// 不存在
        /// </summary>
        NonExistent = 1012,
        /// <summary>
        /// 账号没有注册
        /// </summary>
        NotRegister = 1020,
        /// <summary>
        /// 账号已注册
        /// </summary>
        Registered = 1021,
        /// <summary>
        /// 修改失败
        /// </summary>
        EditFail = 1022,
        /// <summary>
        /// 旧密码错误
        /// </summary>
        OldPasswordErr = 1023,
        /// <summary>
        /// 验证码超时
        /// </summary>
        CodeTimeOut = 1024,
        /// <summary>
        /// 验证码未知错误
        /// </summary>
        CodeUnknow = 1025,
        /// <summary>
        /// 密码已重置
        /// </summary>
        PasswordReset = 1026,
        /// <summary>
        /// 新密码不能与默认密码相同
        /// </summary>
        NotDefaultPsw = 1028,
        /// <summary>
        /// 请求参数错误
        /// </summary>
        RequestParameterError = 2000,
        /// <summary>
        /// 程序错误
        /// </summary>
        ProgramError = 2001,
        /// <summary>
        /// 短信条数超出（每小时只能发两条）
        /// </summary>
        SmsHourCountMore = 3000,
        /// <summary>
        /// 短信发送频率过高
        /// </summary>
        SmsFrequency = 3001,

        /// <summary>
        /// 订单已支付
        /// </summary>
        OrderPaid = 4001,

        /// <summary>
        /// 订单已取消
        /// </summary>
        OrderCancelled = 4002,

        /// <summary>
        /// 订单已结束
        /// </summary>
        OrderEnd = 4003,

        /// <summary>
        /// 支付请求成功
        /// </summary>
        AllowPayment = 4004,

        /// <summary>
        /// 支付参数错误
        /// </summary>
        PaymentParamsError = 4005,

        /// <summary>
        /// 支付失败
        /// </summary>
        PaymentFaid = 4006,
        /// <summary>
        /// 验证码错误
        /// </summary>
        IsCode = 5004,
        /// <summary>
        /// 失败
        /// </summary>
        IsFail = 5005,
        /// <summary>
        /// 是否存在
        /// </summary>
        IsExist = 5006,
        /// <summary>
        /// 没有操作权限
        /// </summary>
        NoRight = 9998,
    }
}
