using PerformEndless.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    public class Test
    {
        public static void Main(string[] args)
        {
            test7();
        }

        static void test8()
        {
            EventStore store = new EventStore(@"E:\VSTest\testStore");
            store.WriteSelf();
            store.DeleteSave(1);
        }

        static void test7()
        {
            EventStore store = new EventStore(@"E:\VSTest\testStore");
            store.WriteSelf();
            PerformManager? manager = PerformManagerDataSL.ToObjectByJsonFile(@"E:\VSTest\testScene",0);
            if (manager == null) { return; }

            manager.EventStore = store;
            //manager.WriteSelf();

            PerformDisplayer displayer = new PerformDisplayer(manager);
            displayer.AddParser(new DialogueParser() { IDPefdata = 1});//添加对话事件解析器
            displayer.AddParser(new BranchParser() { IDPefdata = 2 });//添加分支事件解析器
            //Console.WriteLine("IDIDIDID:" + displayer.ParserList[0].IDPefdata);
            displayer.Active = true;
        }

        static void test6()//写剧本
        {
            PerformScript_JsonTemplate script = new PerformScript_JsonTemplate();
            script.Description = new NormalDescription("剧本0","无描述",0);
            script.InstructionLsit = new PerformEventInstructions_JsonTemplate[]
            {
                new PerformEventInstructions_JsonTemplate(0, PEventInstructions.Start),
                new PerformEventInstructions_JsonTemplate(0, PEventInstructions.Continue),
                new PerformEventInstructions_JsonTemplate(0, PEventInstructions.Continue),
                new PerformEventInstructions_JsonTemplate(0, PEventInstructions.Continue),
                new PerformEventInstructions_JsonTemplate(0, PEventInstructions.Continue),
                new PerformEventInstructions_JsonTemplate(0, PEventInstructions.Continue),//对话事件
                new PerformEventInstructions_JsonTemplate(0, PEventInstructions.Continue),
                new PerformEventInstructions_JsonTemplate(1, PEventInstructions.Start),//分支
                new PerformEventInstructions_JsonTemplate(1, PEventInstructions.Continue)
            };

            PerformScript_JsonTemplate script1 = new PerformScript_JsonTemplate();
            script1.Description = new NormalDescription("剧本1", "无描述", 1);
            script1.InstructionLsit = new PerformEventInstructions_JsonTemplate[]
            {
                new PerformEventInstructions_JsonTemplate(2, PEventInstructions.Start),
                new PerformEventInstructions_JsonTemplate(2, PEventInstructions.Continue)
            };

            PerformScript_JsonTemplate script2 = new PerformScript_JsonTemplate();
            script2.Description = new NormalDescription("剧本2", "无描述", 2);
            script2.InstructionLsit = new PerformEventInstructions_JsonTemplate[]
            {
                new PerformEventInstructions_JsonTemplate(3, PEventInstructions.Start),
                new PerformEventInstructions_JsonTemplate(3, PEventInstructions.Continue)
            };



            PerformManager_JsonTemplate jsonTemplate
                = new PerformManager_JsonTemplate();
            jsonTemplate.Description = new NormalDescription("序章","-和平的异变-",0);
            jsonTemplate.PFScriptList = new PerformScript_JsonTemplate[]
            {
                script,script1,script2
            };
            PerformManagerDataSL.ToJsonTextFileAuto(jsonTemplate, @"E:\VSTest\testScene");
        }

        static void test5()//写事件
        {
            //新建一个事件库
            EventStore store = new EventStore(@"E:\VSTest\testStore");
            #region 事件0
            PerformEvent_JsonTemplate eventJson
                = new PerformEvent_JsonTemplate();
            eventJson.IDPefevent = 1;
            eventJson.Description = new NormalDescription("对话事件1","无描述",0);
            eventJson.Data = new PerformData[]
            {
                new PerformData(){ IDPefdata = 1, Data = new string[]
                {"博丽灵梦","一般","嗯？塞钱箱里貌似有奇怪的声音" } },
                new PerformData(){ IDPefdata = 1, Data = new string[]
                {"博丽灵梦","惊喜","（难道今天有参拜客塞钱了，快让我看看有多少）" } },
                new PerformData(){ IDPefdata = 1, Data = new string[]
                {"","","灵梦走向塞钱箱" } },
                new PerformData(){ IDPefdata = 1, Data = new string[]
                {"东风谷早苗","嘲讽脸","哟哟哟，这不是博丽的巫女么，几天不见这么拉了~" } },
                new PerformData(){ IDPefdata = 1, Data = new string[]
                {"东风谷早苗","嘲讽脸","要我说，赶紧把你这破神社拆了，反正也没什么参拜客吧（嘻" } },
                new PerformData(){ IDPefdata = 1, Data = new string[]
                {"东风谷早苗","嘲讽脸","不如合并到我们的守矢神社，来供伺我早苗大人吧" } },
                new PerformData(){ IDPefdata = 1, Data = new string[]
                {"","","塞钱箱大破！！" } },
            };
            PerformEvent performEvent = new PerformEvent(eventJson,store);
            #endregion 事件0

            #region 事件1
            PerformEvent_JsonTemplate eventJson1
                = new PerformEvent_JsonTemplate();
            eventJson1.IDPefevent = 2;
            eventJson1.Description = new NormalDescription("节点分支事件","",1);
            eventJson1.StartIndex = 2;
            eventJson1.Data = new PerformData[]//设分支事件的ID为-2
            {
                new PerformData(){IDPefdata = -2,
                Data = new string[]{ ScriptPosition.GetJsonText(1,0)}},//数据1，跳到剧本1
                new PerformData(){IDPefdata = -2,
                Data = new string[]{ ScriptPosition.GetJsonText(2,0)}}, //数据2，跳到剧本2
                new PerformData(){IDPefdata = 2,
                Data = new string[]{ "灵梦战胜","灵梦战败"}},
                new PerformData(){IDPefdata = 0 }
            };
            PerformEvent performEvent1 = new PerformEvent(eventJson1, store);
            performEvent1.WriteSelf();
            #endregion 事件1

            #region 事件2
            PerformEvent_JsonTemplate eventJson2
                = new PerformEvent_JsonTemplate();
            eventJson2.IDPefevent = 1;
            eventJson2.Description = new NormalDescription("对话事件2", "无描述", 2);
            eventJson2.Data = new PerformData[]
            {
                new PerformData(){ IDPefdata = 1, Data = new string[]
                {"博丽灵梦","赤色杀人魔","只要你乖乖把钱叫出来，我就考虑不把你和你的神社扬了" } },
                new PerformData(){ IDPefdata = 1, Data = new string[]
                {"东方谷早苗","战损","呜~好可怕！" } }
            };
            PerformEvent performEvent2 = new PerformEvent(eventJson2, store);
            #endregion 事件2

            #region 事件3
            PerformEvent_JsonTemplate eventJson3
                = new PerformEvent_JsonTemplate();
            eventJson3.IDPefevent = 1;
            eventJson3.Description = new NormalDescription("对话事件3", "无描述", 3);
            eventJson3.Data = new PerformData[]
            {
                new PerformData(){ IDPefdata = 1, Data = new string[]
                {"东方谷早苗","傲气","果然只有这种水平么，幻想乡的巫女只要我守矢风祝就够了" } },
                new PerformData(){ IDPefdata = 1, Data = new string[]
                {"博丽灵梦","战损","这，怎么会这样" } }
            };
            PerformEvent performEvent3 = new PerformEvent(eventJson3, store);
            #endregion 事件3
            store.ChangeSave(performEvent);
            store.ChangeSave(performEvent1);
            store.ChangeSave(performEvent2);
            store.ChangeSave(performEvent3);
        }

        static void test4()
        {
            PerformManager? manager
                = PerformManagerDataSL.ToObjectByJsonFile(@"E:\VSTest\testScene", 1);
            if (manager == null) { return; }

            
            manager.WriteSelf();
            EventStore store = new EventStore(@"E:\VSTest\testStore");
            manager.EventStore = store;

            Console.WriteLine("-----------------------");
            store.FindID(1).WriteSelf();
            PerformData data = manager.Start(0);
            Console.WriteLine("-----------------------");
            data.WriteSelf();

            data = manager.Continue(0);
            Console.WriteLine("-----------------------");
            data.WriteSelf();

            data = manager.Continue(0);
            Console.WriteLine("-----------------------");
            data.WriteSelf();

            data = manager.Continue(0);
            Console.WriteLine("-----------------------");
            data.WriteSelf();

            data = manager.Continue(0);
            Console.WriteLine("-----------------------");
            data.WriteSelf();

            data = manager.Continue(0);
            Console.WriteLine("-----------------------");
            data.WriteSelf();

            data = manager.Continue(0);
            Console.WriteLine("-----------------------");
            data.WriteSelf();

            data = manager.Continue(0);
            Console.WriteLine("-----------------------");
            data.WriteSelf();

            data = manager.Continue(0);
            Console.WriteLine("-----------------------");
            data.WriteSelf();
        }
        static void test3()
        {
            PerformManager_JsonTemplate jsonTemplate
                = new PerformManager_JsonTemplate();
            jsonTemplate.Description = new NormalDescription("剧场1","剧场描述1",1);



            PerformScript_JsonTemplate performScript = new PerformScript_JsonTemplate();
            performScript.Description = new NormalDescription("剧本1","剧本描述1",0);
            SimpleList<PerformEventInstructions_JsonTemplate>
                list = new SimpleList<PerformEventInstructions_JsonTemplate>();
            list.Add(new PerformEventInstructions_JsonTemplate(1,PEventInstructions.Start));
            list.Add(new PerformEventInstructions_JsonTemplate(2, PEventInstructions.Start));
            list.Add(new PerformEventInstructions_JsonTemplate(2, PEventInstructions.Continue));
            list.Add(new PerformEventInstructions_JsonTemplate(1, PEventInstructions.Continue));
            list.Add(new PerformEventInstructions_JsonTemplate(2, PEventInstructions.Continue));
            list.Add(new PerformEventInstructions_JsonTemplate(1, PEventInstructions.Continue));
            list.Add(new PerformEventInstructions_JsonTemplate(1, PEventInstructions.Continue));
            performScript.InstructionLsit = list.ToArray();

            jsonTemplate.PFScriptList 
                = new PerformScript_JsonTemplate[] { performScript };

            PerformManagerDataSL.ToJsonTextFileAuto(jsonTemplate, @"E:\VSTest\testScene");
        }

        static void test2()
        {
            EventStore store = new EventStore(@"E:\VSTest\testStore");
            store.WriteSelf();

            PerformEvent_JsonTemplate jsonTemplate = new PerformEvent_JsonTemplate();
            jsonTemplate.IDPefevent = 1;
            jsonTemplate.Description = new NormalDescription("事件名称2", "事件描述2", 2);
            jsonTemplate.Data = new PerformData[]
            {
                new PerformData(1,"2数据1"),
                new PerformData(1,"2数据888888"),
                new PerformData(1,"2数据3")
            };
            PerformEvent performEvent = new PerformEvent(jsonTemplate, store);
            store.ChangeSave(performEvent);
        }

        static void test1()
        {
            EventStore store = new EventStore(@"E:\VSTest\testStore");
            store.WriteSelf();

            PerformEvent? performEvent = store.FindID(1);
            if (performEvent != null) 
            {
                performEvent.WriteSelf();
            }
            /*PerformEvent_JsonTemplate jsonTemplate = new PerformEvent_JsonTemplate();
            jsonTemplate.IDPefevent = 1;
            jsonTemplate.Description = new NormalDescription("事件名称1","事件描述1",1);
            jsonTemplate.Data = new PerformData[]
            {
                new PerformData(1,"数据1"),
                new PerformData(1,"数据2"),
                new PerformData(1,"数据3")
            };
            PerformEvent performEvent = new PerformEvent(jsonTemplate, store);
            store.AddSave(performEvent);

            store.WriteSelf();*/
        }
    }
}
