using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FMHJJService.Common.Enum
{
    /// <summary>
    /// 短信验证码枚举
    /// </summary>
    public enum SMSType
    {
        // 广告类
        Advertisement = 0,

        //验证码类
        VerificationCode = 1
    }

    public enum UserType
    {
        //匿名类
        Anonym = 0,

        //公司侧
        User = 1,

        //客户侧
        Customer = 2
    }

    public enum UserState
    {
        //正常
        Normal = 0,

        //已注销
        Canceled = 1       
    }

    public enum NoticeType
    {
        //竞价公告
        Notice = 1,
        //竞价规则
        Rule = 2
    }

    public enum SmsType
    {        
        //发送短信
        Send = 1,
        //删除短信
        Delete = 2
    }
}
