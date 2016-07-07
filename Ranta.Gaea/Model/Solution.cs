using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranta.Gaea.Model
{
    public class Solution
    {
        /// <summary>
        /// Guid
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Ranta
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 解决方案名称 Translation
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目名称 {Prefix}.{Name}
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 项目主体类型 1.Library 2.Mvc
        /// </summary>
        public int ProjectType { get; set; }

        /// <summary>
        /// 项目列表
        /// </summary>
        public List<CSharpProject> ProjectList { get; set; }
    }
}
