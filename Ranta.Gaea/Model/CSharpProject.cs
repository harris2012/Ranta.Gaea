using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranta.Gaea.Model
{
    public class CSharpProject
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// 项目名称 Ranta.Translation
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 全名称 Ranta.Translation.Net40
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// .net版本
        /// </summary>
        public ProjectType ProjectType { get; set; }

        /// <summary>
        /// 是否需要编译并打包到nuget package
        /// </summary>
        public bool Compile { get; set; }

        /// <summary>
        /// 是否需要测试
        /// </summary>
        public bool NeedTest { get; set; }
    }
}
