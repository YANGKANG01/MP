using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace MP.Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class ProjectInfo
    {
        public ProjectInfo()
        {


        }
        /// <summary>
        /// Desc:项目编号
        /// Default:
        /// Nullable:False
        /// </summary>        
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Desc:项目名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:所属公司
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Company { get; set; }

        /// <summary>
        /// Desc:项目职责
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Duties { get; set; }

        /// <summary>
        /// Desc:项目职位
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Place { get; set; }

        /// <summary>
        /// Desc:项目技能
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Skill { get; set; }

        /// <summary>
        /// Desc:项目规模
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Scale { get; set; }

        /// <summary>
        /// Desc:项目描述
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Describe { get; set; }

        /// <summary>
        /// Desc:项目时长
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ProjectTime { get; set; }

        /// <summary>
        /// Desc:项目开始时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Desc:修改时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// Desc:该字段表示是否被删除(0-删除,1-未删除)，默认为1
        /// Default:b'1'
        /// Nullable:True
        /// </summary>           
        public int Valid { get; set; } = 1;

    }
}
