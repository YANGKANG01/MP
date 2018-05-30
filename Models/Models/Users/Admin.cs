using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Models.Users
{
    /// <summary>
    /// Admin
    /// </summary>
    public class Admin
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public virtual string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }
        /// <summary>
        /// 用户凭证
        /// </summary>
        public virtual string Token { get; set; }
        /// <summary>
        /// 角色标识
        /// </summary>
        public virtual int RoleId { get; set; }
        /// <summary>
        /// 用户状态(100-正常,101-禁用)
        /// </summary>
        public virtual int State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }
        /// <summary>
        /// 是否删除，默认为1
        /// </summary>
        public virtual int Valid { get; set; }

    }
}
