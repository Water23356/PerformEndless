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
    public interface PerformConnection
    {
        /// <summary>
        /// 连接体的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 连接体的ID
        /// </summary>
        public int ID { get; }
        /// <summary>
        /// 激活连接体
        /// </summary>
        public PerformInstruction Active(out PerformData data, int input = 0);
    }
}
