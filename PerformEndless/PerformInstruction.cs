namespace PerformEndless
{
    /// <summary>
    /// 演出指令
    /// </summary>
    public class PerformInstruction: PerformConnection
    {
        /// <summary>
        /// 数据体名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据体ID
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 连接的物体
        /// </summary>
        public PerformConnection Item { get; set; }
        /// <summary>
        /// 一下条指令
        /// </summary>
        public PerformInstruction Next { get; set; }
        /// <summary>
        /// 触发连接体的条件
        /// </summary>
        public int Condizione { get; set; }
        public PerformInstruction(int id)
        {
            ID = id;
        }

        public PerformInstruction Active(out PerformData data,int input = 0)
        {
            PerformData pdata = null;
            if (input == Condizione)
            {
                
                if(Item is PerformInstruction)
                {
                    data = pdata;
                    return (PerformInstruction)Item;
                }
                else
                {
                    Item.Active(out pdata,input);
                }
            }
            data = pdata;
            return Next;
        }


    }
}