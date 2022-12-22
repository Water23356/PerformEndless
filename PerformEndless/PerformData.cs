using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    /// <summary>
    /// 演出数据
    /// </summary>
    public class PerformData : PerformConnection
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        public string type;
        /// <summary>
        /// 数据内容(json)
        /// </summary>
        public string body;

        /// <summary>
        /// 数据体名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据体ID
        /// </summary>
        public int ID { get; private set; }

        public PerformData(int id)
        {
            ID = id;
        }

        public PerformInstruction Active(out PerformData data, int input = 0)
        {
            data = this;
            return null;
        }

        public PerformData Copy()
        {
            return new PerformData(ID) { type = type, body = body, Name = Name };
        }
    }
}
