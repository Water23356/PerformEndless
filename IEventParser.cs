using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    /// <summary>
    /// 事件解析器接口
    /// </summary>
    public interface IEventParser
    {
        /// <summary>
        /// 此解析器所解析的事件类型
        /// </summary>
        public int IDPefdata { get; set; }

        public void Parse(PerformData data,Action<int> result);
    }
}
