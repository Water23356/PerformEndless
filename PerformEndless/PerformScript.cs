using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    /// <summary>
    /// 指令包
    /// </summary>
    public class PerformScript:PerformObject
    {
        /// <summary>
        /// 此指令包所包含指令
        /// </summary>
        public List<int> ists = new List<int>();

        public PerformScript(int id) { ID = id; }

        public string GetCsv()
        {
            string csv = "*,";
            csv += ToolMore.ToCsv(Name) + ",";
            csv += ToolMore.ToCsv(ID + "") + ",";
            string item = "";
            bool start = false;
            foreach(int i in ists)
            {
                if (!start) { start = true; }
                else { item += ","; }
                item+= i;
            }
            csv += ToolMore.ToCsv(item + "") + "\n";
            return csv;
        }
    }
}
