using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Base.Models
{

    /// <summary>
    /// 菜单信息
    /// </summary>
    [SugarTable("SysMenu")]
    public class BaseSysMenu
    {
        /// <summary>
        /// 菜单编号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 控制器名
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// 执行函数名称
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 父菜单标识
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 菜单位置(排序)
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// 菜单备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 该字段表示是否被删除(0-删除,1-未删除)，默认为1
        /// </summary>
        public int Valid { get; set; } = 1;
    }
}
