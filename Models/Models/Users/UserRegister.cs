using LaterTask.Module.Users.Models.Extension;
using System.ComponentModel.DataAnnotations;

namespace MP.Models.Users
{
    /// <summary>
    /// 注册信息
    /// </summary>
    public class UserRegister
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool IsAgree { get; set; }
    }
}
