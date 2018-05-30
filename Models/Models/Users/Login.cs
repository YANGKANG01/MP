using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MP.Models.Users
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public class Login
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "请输入邮箱")]
        [EmailAddress(ErrorMessage = "邮箱格式非法")]
        [Display(Name = "邮箱")]
        public string Email { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        [MinLength(6, ErrorMessage = "密码长度不能小于6")]
        public string Password { get; set; }
        /// <summary>
        /// 记住我
        /// </summary>
        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }
}
