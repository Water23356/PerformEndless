using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    public static class ToolMore
    {
        /// <summary>
        /// 对一行csv数据的进行切片
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        public static string[] SplitCsv(string txtLine,int least = 0)
        {
            List<string> parts = new List<string>();
            string buffer = "";
            //状态机:  0:通常读取  1:引号抓取  2:引号转义
            int status = 0;
            foreach (char c in txtLine)
            {
                switch (status)
                {
                    case 0:
                        if (c == '"') { status = 1; }
                        else if (c == ',')
                        {
                            parts.Add(buffer);
                            buffer = "";
                            status = 0;
                        }
                        else { buffer += c; }
                        break;
                    case 1:
                        if (c == '"') { status = 2; }
                        else
                        {
                            buffer += c;
                        }
                        break;
                    case 2:
                        if (c == '"')
                        {
                            buffer += c;
                            status = 1;
                        }
                        else if (c == ',')
                        {
                            parts.Add(buffer);
                            buffer = "";
                            status = 0;
                        }
                        break;
                }
            }
            if (buffer != "") { parts.Add(buffer); }
            while(parts.Count < least)
            {
                parts.Add("");
            }
            return parts.ToArray();
        }
        /// <summary>
        /// 获取指定字符串的csv格式
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static string ToCsv(string cell)
        {
            string item = "";
            bool quotes = false;
            foreach (char c in cell)
            {
                switch(c)
                {
                    case '"': 
                        quotes = true;
                        item += '"';
                        item += '"';
                        break;
                    case ',': 
                        quotes= true;
                        item += ",";
                        break;
                    case '\n':
                        break;
                    default:
                        item += c;
                        break;
                }
            }
            if(quotes)
            {
                item = '"' + item + '"';
            }
            return item;
        }
    }
}
