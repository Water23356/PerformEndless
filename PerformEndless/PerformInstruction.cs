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

        public PerformInstruction(int id)
        {
            ID = id;
        }

        public PerformInstruction Active(out PerformData data)
        {
            if(Item is PerformInstruction)
            {
                data = null;
                return (PerformInstruction)Item;
            }
            PerformData pdata = null;
            Item.Active(out pdata);
            data = pdata;
            return Next;
        }

    }
}