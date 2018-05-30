using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace MP.Models
{
    ///<summary>
    ///用户角色
    ///</summary>
    public partial class SysRole
    {
        public SysRole()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Desc:角色名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Name { get; set; }
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// Desc:创建人
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? Creater { get; set; }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Desc:该字段表示是否被删除(0-删除,1-未删除)，默认为1
        /// Default:b'1'
        /// Nullable:True
        /// </summary>           
        public int? Valid { get; set; }

    }
}
