using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    /// <summary>
    /// 演出连接体
    /// </summary>
    public class PerformObject
    {
        /// <summary>
        /// 连接体的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 连接体的ID
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 标签,用于区分演出数据和演出指令
        /// </summary>
        public string tag;
    }
}
