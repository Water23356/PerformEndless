using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    namespace Display
    {
        /// <summary>
        /// 对话解析器（演示用）
        /// </summary>
        public class DialogueParser:IEventParser
        {
            public int IDPefdata { get; set; }
            public void Parse(PerformData data,Action<int> result)
            {
                string name = data.Data[0];
                string img = data.Data[1];
                string txt = data.Data[2];

                Console.WriteLine("[名称 = "+name+"][贴图 = "+img+"]{"+txt+"}");
                Console.Write("<按a继续>");
                string? getT = "";
                while(getT != "a")
                {
                    getT = Console.ReadLine();
                }
                result(-1);//通知展示器
            }
        }
    }
}
