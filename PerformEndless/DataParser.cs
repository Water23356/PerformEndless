using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    /// <summary>
    /// 数据解析器
    /// </summary>
    public abstract class DataParser
    {
        public string Type;
        public DataParser(string type) { Type = type; }
        /// <summary>
        /// 解析数据,实现效果
        /// </summary>
        /// <param name="data">数据包</param>
        /// <param name="reslut">回调函数</param>
        public abstract void Parse(Dictionary<string, string> data, Action<int> reslut);
    }

    public class TestParser:DataParser
    {
        public TestParser(string type):base(type) {}
        public override void Parse(Dictionary<string, string> data, Action<int> reslut)
        {
            if (data == null) { return; }
            Console.WriteLine("解析中");
            Console.WriteLine(">>>>>>>>>>>>>>>>"+data["test"]);
            Console.ReadKey();
            reslut(0);
        }
    }

    public class Test_InputParser : DataParser
    {
        public Test_InputParser(string type) : base(type) { }
        public override void Parse(Dictionary<string, string> data, Action<int> reslut)
        {
            if (data == null) { return; }
            Console.WriteLine("解析中");
            Console.WriteLine(">>>>>>>>>>>>>>>>" + data["test"]);
            string input = Console.ReadLine();
            switch (input)
            {
                case "A": reslut(1); break;
                case "B": reslut(2); break;
                case "C": reslut(3); break;
            }
            
        }
    }
}
