using System;
using System.Collections.Generic;
using System.Text;
using MP.Models;

namespace MP.Models.Extension
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class RoleResult : SysRole
    {
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreaterName { get; set; }
    }
}
