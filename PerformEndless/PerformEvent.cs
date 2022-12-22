using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    /// <summary>
    /// 演出事件包 (包含若干个数据包)
    /// </summary>
    public class PerformEvent: PerformConnection
    {
        /// <summary>
        /// 事件数据包
        /// </summary>
        private List<PerformData> datas;
        /// <summary>
        /// 事件体名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 事件体ID
        /// </summary>
        public int ID { get;private set; }
        /// <summary>
        /// 当前事件包执行进度
        /// </summary>
        public int Index { get; private set; }

        public PerformEvent(int id)
        {
            ID = id;
            Index = 0;
        }

        /// <summary>
        /// 重置事件进度
        /// </summary>
        public void Reset() { Index = 0; }
        /// <summary>
        /// 激活事件,得到事件当前进度的事件数据包
        /// </summary>
        /// <returns></returns>
        public PerformInstruction Active(out PerformData data, int input = 0)
        {
            if(Index < datas.Count)
            {
                data = datas[Index];
                Index++;
            }
            data = null;
            return null;
        }
        /// <summary>
        /// 得到此事件包的一个副本对象
        /// </summary>
        /// <param name="id">副本的id</param>
        public PerformEvent Copy(int id)
        {
            List<PerformData> datas_new = new List<PerformData>();
            foreach(PerformData d in datas)
            {
                datas_new.Add(d.Copy());
            }
            return new PerformEvent(id) { Name = Name,datas = datas_new};
        }
    }
}
