using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    public class Test
    {
        static void test1()
        {
            ScriptStore scriptStore = new ScriptStore();
            DataStore dataStore = new DataStore();

            scriptStore.Output = Output;
            dataStore.Output = Output;
            
            PerformInstruction ist1 = scriptStore.Creat();
            PerformInstruction ist2_1 = scriptStore.Creat();
            PerformInstruction ist2_2 = scriptStore.Creat();
            PerformInstruction ist2_3 = scriptStore.Creat();
            PerformInstruction ist3 = scriptStore.Creat();
            PerformInstruction ist4 = scriptStore.Creat();
            PerformInstruction ist5 = scriptStore.Creat();
            PerformInstruction ist6 = scriptStore.Creat();
            PerformInstruction ist7 = scriptStore.Creat();
            PerformInstruction ist8 = scriptStore.Creat();

            

            
            
            DataCell dc1 = (DataCell)dataStore.Creat("test_input");
            DataCell dc2 = (DataCell)dataStore.Creat("test");
            DataCell dc3 = (DataCell)dataStore.Creat("test");
            DataCell dc4 = (DataCell)dataStore.Creat("test");
            DataCell dc5 = (DataCell)dataStore.Creat("test");
            DataCell dc6 = (DataCell)dataStore.Creat("test");
            DataCell dc7 = (DataCell)dataStore.Creat("test");
            DataCell dc8 = (DataCell)dataStore.Creat("test");
            DataPackage dc9 = (DataPackage)dataStore.Creat();

            dc9.cells.Add(dc6.ID);
            dc9.cells.Add(dc7.ID);
            dc9.cells.Add(dc8.ID);

            ist1.Next = ist2_1.ID;
            ist1.Item = dc1.ID;

            ist2_1.Next = ist2_2.ID;
            ist2_1.Item = ist3.ID;
            ist2_1.Condizione = 1;

            ist2_2.Next = ist2_3.ID;
            ist2_2.Item = ist4.ID;
            ist2_2.Condizione = 2;

            ist2_3.Item = ist5.ID;
            ist2_3.Condizione = 3;

            ist3.Next = ist6.ID;
            ist3.Item = dc2.ID;
            ist6.Item = dc3.ID;

            ist4.Next = ist7.ID;
            ist4.Item = dc4.ID;
            ist7.Item = dc5.ID;

            ist5.Next = ist8.ID;
            ist5.Item = dc6.ID;
            ist8.Item = dc7.ID;

            dc1.body.Add("test", "AAA");
            dc2.body.Add("test", "BBB");
            dc3.body.Add("test", "CCC");
            dc4.body.Add("test", "DDD");
            dc5.body.Add("test", "EEE");
            dc6.body.Add("test", "FFF");
            dc7.body.Add("test", "GGG");
            dc8.body.Add("test", "HHH");

            scriptStore.Path = "E:\\VSTest\\testScriptStore.scpt";
            //scriptStore.Save();
            scriptStore.Load();

            PerformDisplayer pdp = new PerformDisplayer(scriptStore, dataStore, -1);
            TestParser testParser = new TestParser("test");
            Test_InputParser testParserI = new Test_InputParser("test_input");
            pdp.AddParser(testParser);
            pdp.AddParser(testParserI);
             pdp.Start(0);
        }

        public static void Main(string[] args)
        {

            test1();
           


        }

        static void Output(string txt)
        {
            Console.WriteLine(txt);
        }
    }
}
