using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace PerformEndless
{
    public class Test
    {
        static void test1()
        {
            List<Template> tps = new List<Template>();

            #region mod1
            Template tp1 = new Template();
            tp1.name= "TP-1";
            List<Template_Control> controls = tp1.Controls();
            Template_Combobox tp_cbb = new Template_Combobox()
            {
                key = "testKey",
                name = "测试键",
                valueType = ValueType.String,
                description = "这是一个测试项",
            };
            tp_cbb.items.Add("选项1"); tp_cbb.values.Add("值1");
            tp_cbb.items.Add("选项2"); tp_cbb.values.Add("值2");
            tp_cbb.items.Add("选项3"); tp_cbb.values.Add("值3");
            tp_cbb.items.Add("选项4"); tp_cbb.values.Add("值4");
            controls.Add(tp_cbb);
            tp1.UpdateTxt();
            tps.Add(tp1);
            #endregion

            #region mod2
            Template tp2 = new Template();
            tp2.name = "TP-2";
            controls = tp2.Controls();
            Template_InputField tp_if = new Template_InputField()
            {
                key = "testKey",
                name = "测试键",
                valueType = ValueType.Number,
                description = "这是一个测试项",
                limit = "大于0"
            };
            controls.Add(tp_if);
            tp2.UpdateTxt();
            tps.Add(tp2);
            #endregion

            
            string txt = JsonConvert.SerializeObject(tps);
            Console.WriteLine(txt);
        }

        static void test2()
        {
            string txt = "[{\"id\":0,\"name\":\"TP-1\",\"description\":null,\"json_template\":[\"{\\\"tp_Tpye\\\":0,\\\"items\\\":[\\\"选项1\\\",\\\"选项2\\\",\\\"选项3\\\",\\\"选项4\\\"],\\\"values\\\":[\\\"值1\\\",\\\"值2\\\",\\\"值3\\\",\\\"值4\\\"],\\\"key\\\":\\\"testKey\\\",\\\"name\\\":\\\"测试键\\\",\\\"valueType\\\":0,\\\"description\\\":\\\"这是一个测试项\\\"}\"],\"json_type\":[0]},{\"id\":0,\"name\":\"TP-2\",\"description\":null,\"json_template\":[\"{\\\"tp_Tpye\\\":1,\\\"limit\\\":\\\"大于0\\\",\\\"key\\\":\\\"testKey\\\",\\\"name\\\":\\\"测试键\\\",\\\"valueType\\\":1,\\\"description\\\":\\\"这是一个测试项\\\"}\"],\"json_type\":[0]}]";
            Template[] tps = JsonConvert.DeserializeObject<Template[]>(txt);
            string txt2 = JsonConvert.SerializeObject(tps);
            Console.WriteLine(txt2);

            tps[0].UpdateFormJson();
            Console.WriteLine("模板1的项的数量:"+tps[0].Controls().Count);
            List<Template_Control> controls = tps[0].Controls();
            foreach(Template_Control tp in controls)
            {
                Console.WriteLine(tp.tp_Tpye);
                if(tp.tp_Tpye == TP_Tpye.CheckBox)
                {
                    Console.WriteLine("复选框模板");
                }
                else if(tp.tp_Tpye == TP_Tpye.Combobox)
                {
                    Console.WriteLine("下拉框模板");
                    Template_Combobox ccb = (Template_Combobox)tp;
                    Console.WriteLine("键值对:");
                    for (int i=0;i<ccb.items.Count;i++)
                    {
                        Console.WriteLine($"<{ccb.items[i]}:{ccb.values[i]}>");
                    }
                }
                else if(tp.tp_Tpye == TP_Tpye.InputField)
                {
                    Console.WriteLine("输入框模板");
                    Template_InputField tif = (Template_InputField)tp;
                    Console.WriteLine("Limit:"+tif.limit);
                }
            }
        }
        static void test3()
        {
            List<Template_Control> cs = new List<Template_Control>();
            Template_Combobox tp_cbb = new Template_Combobox()
            {
                key = "testKey",
                name = "测试键",
                valueType = ValueType.String,
                description = "这是一个测试项",
            };
            tp_cbb.items.Add("选项1"); tp_cbb.values.Add("值1");
            tp_cbb.items.Add("选项2"); tp_cbb.values.Add("值2");
            tp_cbb.items.Add("选项3"); tp_cbb.values.Add("值3");
            tp_cbb.items.Add("选项4"); tp_cbb.values.Add("值4");
            cs.Add(tp_cbb);
            string txt = JsonConvert.SerializeObject(cs[0]);
            Console.WriteLine(txt);
        }
        public static void Main(string[] args)
        {

            Console.WriteLine("开始测试");
            test2();



        }

        static void Output(string txt)
        {
            Console.WriteLine(txt);
        }
    }
}
