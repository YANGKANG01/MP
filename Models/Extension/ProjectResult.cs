using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace MP.Models.Extension
{
    public class ProjectResult
    {
        /// <summary>
        /// 项目信息
        /// </summary>
        public ProjectInfo ProjectInfo { get; set; }
        /// <summary>
        /// 项目效果图
        /// </summary>
        public IList<ProjectEffect> EffectResults { get; set; }
    }
   
}
