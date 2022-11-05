using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    namespace Display
    {
        internal class BranchParser : IEventParser
        {
            public int IDPefdata { get ; set; }

            public void Parse(PerformData data, Action<int> result)
            {
                for(int i=0;i<data.Data.Length;i++)
                {
                    Console.Write("<"+(i+1)+">"+data.Data[i]+"\t");
                }
                Console.WriteLine();
                int getT = -1;
                while (getT < 0 || getT > data.Data.Length) 
                {
                    getT = Convert.ToInt32(Console.ReadLine())-1;
                }
                result(getT);
            }
        }
    }
}
