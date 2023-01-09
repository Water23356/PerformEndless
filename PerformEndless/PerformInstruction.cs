namespace PerformEndless
{
    /// <summary>
    /// 演出指令
    /// </summary>
    public class PerformInstruction: PerformObject
    {
        #region 属性
        /// <summary>
        /// 所属剧本库
        /// </summary>
        public ScriptStore owner;
        /// <summary>
        /// 所属指令包
        /// </summary>
        public int script_Onwer;
        /// <summary>
        /// 规定标签
        /// </summary>
        public new string tag = "Instruction";
        /// <summary>
        /// 连接的物体
        /// </summary>
        public int Item { get; set; }
        /// <summary>
        /// 一下条指令
        /// </summary>
        public int Next { get; set; }
        /// <summary>
        /// 触发连接体的条件,为0表示无限制条件
        /// </summary>
        public int Condizione { get; set; }
        /// <summary>
        /// 调用参数(在获取数据时传入)
        /// </summary>
        public int parameter;

        #endregion

        public PerformInstruction(int id,ScriptStore ownerp)
        {
            ID = id;
            owner = ownerp;
            Item = 0;
            Next = 0;
            Condizione = 0;
        }

        /// <summary>
        /// 执行此条指令,并返回数据,以及下一条指令的ID
        /// </summary>
        /// <param name="id_data"></param>
        /// <returns></returns>
        public int Ist(out int id_data,int input = 0)
        {
            if(Condizione==0 || input == Condizione)
            {
                if (Item > 0)//大于0的id为数据
                {
                    id_data = Item;
                    return Next;
                }
                else if (Item < 0)//小于0的id为指令
                {
                    id_data = 0;
                    return Item;
                }
            }
            id_data = 0;
            return Next;
        }

        /// <summary>
        /// 获取一个自身副本
        /// </summary>
        /// <returns></returns>
        public PerformInstruction Copy()
        {
            return new PerformInstruction(ID,owner)
            {
                Name = Name,
                Item = Item,
                Next = Next,
                Condizione = Condizione,
                parameter=parameter,
                script_Onwer = script_Onwer
            }; 
        }
        /// <summary>
        /// 创建一个新的指令对象,其预设属性为自身的副本
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PerformInstruction Copy(int id)
        {
            return new PerformInstruction(id,owner)
            {
                Name = Name,
                Item = Item,
                Next = Next,
                Condizione = Condizione,
                script_Onwer= script_Onwer
            };
        }
        /// <summary>
        /// 获取此对象的csv数据字符串
        /// </summary>
        /// <returns></returns>
        public string GetCsv()
        {
            string csv = "%,";
            csv += ToolMore.ToCsv(Name) + ",";
            csv += ToolMore.ToCsv(ID+"") + ",";
            csv += ToolMore.ToCsv(Item + "") + ",";
            csv += ToolMore.ToCsv(Next + "") + ",";
            csv += ToolMore.ToCsv(Condizione + "") + ",";
            csv += ToolMore.ToCsv(parameter + "") + "\n";
            return csv;
        }
    }
}