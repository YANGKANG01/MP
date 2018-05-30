using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace MP.Models
{
    ///<summary>
    ///项目效果图表
    ///</summary>
    public partial class ProjectEffect
    {
        public ProjectEffect()
        {


        }
        /// <summary>
        /// Desc:项目效果图标识
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? Id { get; set; }

        /// <summary>
        /// Desc:项目标识
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? ProjectId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Desc:路径地址
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Path { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
        /// <summary>
        /// Desc:该字段表示是否被删除(0-删除,1-未删除)，默认为1
        /// Default:b'1'
        /// Nullable:True
        /// </summary>           
        [SugarColumn(IsIgnore = true)]
        public int Valid { get; set; }

    }
}
