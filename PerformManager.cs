using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PerformEndless.Display;

namespace PerformEndless
{
    /// <summary>
    /// 演出管理类；
    /// 包含了一次连续演出的所有内容，是一个完整的剧场；
    /// </summary>
    public class PerformManager
    {
        #region 属性
        /// <summary>
        /// 该剧场的描述
        /// </summary>
        public NormalDescription Description { get; private set; }
        /// <summary>
        /// 该剧场的剧本列表
        /// </summary>
        public SimpleList<PerformScript> PFScriptList { get; private set; }
        /// <summary>
        /// 场景文件所在文件夹的路径
        /// </summary>
        public string? Path { get; set; }
        #endregion 属性

        #region 管理属性
        /// <summary>
        /// 事件缓存区
        /// </summary>
        private SimpleList<PerformEvent> pfEventList = new SimpleList<PerformEvent>();
        /// <summary>
        /// 当前指令的位置
        /// </summary>
        private ScriptPosition scriptPosition;
        /// <summary>
        /// 当前管理类所加载的事件的数量
        /// </summary>
        public int OnLoadEventCount 
        {
            get
            {
                return pfEventList.Count;
            }
        }
        /// <summary>
        /// 这个管理器所引用的 事件库 对象
        /// </summary>
        public EventStore? EventStore { get; set; }
        #endregion 管理属性

        #region 构造函数
        /// <summary>
        /// 初始化一个空剧场
        /// </summary>
        public PerformManager()
        {
            scriptPosition = ScriptPosition.zero;
            Description = new NormalDescription("空剧场","无描述",-1);
            PFScriptList = new SimpleList<PerformScript>();
            Path = "NULL";
        }
        /// <summary>
        /// 根据 PerformManager_JsonTemplate 创建一个 PerformManager 对象；
        /// PerformManager 的属性是 PerformManager_JsonTemplate 的浅拷贝
        /// </summary>
        /// <param name="jsonTemplate"></param>
        public PerformManager(PerformManager_JsonTemplate jsonTemplate)
        {
            scriptPosition = ScriptPosition.zero;
            Description = jsonTemplate.Description;
            PerformScript_JsonTemplate[]? array = jsonTemplate.PFScriptList;
            if (array == null) { PFScriptList = new SimpleList<PerformScript>(); }
            else
            {
                PerformScript[] list = new PerformScript[array.Length];
                for(int i = 0;i < list.Length;i++)//将剧本模板转化为剧本对象
                {
                    list[i] = new PerformScript(array[i]);
                }
                PFScriptList = new SimpleList<PerformScript>(list);
            }
            Path = "NULL";
        }
        #endregion 构造函数

        #region 内部函数
        /// <summary>
        /// 从缓存区抽取指定ID的事件（事件不存在则返回null）
        /// </summary>
        /// <param name="id">事件ID</param>
        /// <returns>返回的事件对象</returns>
        private PerformEvent? Get(int id)
        {
            for (int i = 0; i < pfEventList.Count; i++)
            {
                if (pfEventList[i].ID() == id)
                {
                    return pfEventList[i];
                }
            }
            return null;
        }
        /// <summary>
        /// 根据事件ID，将指定事件加入缓存区
        /// </summary>
        /// <param name="id"></param>
        private void LoadEvent(int id)
        {
            
            if (EventStore != null)
            {
                
                PerformEvent? performEvent = EventStore.FindID(id);//从事件库内获取对应事件
               // performEvent.WriteSelf();
                if(performEvent != null)
                {
                    pfEventList.Add(performEvent);
                }
            }
        }
        /// <summary>
        /// 检查指定事件是否存在于缓存区中
        /// </summary>
        /// <param name="performEvent">事件对象</param>
        /// <returns>是否存在</returns>
        private bool Checkout(PerformEvent performEvent)
        {
            for(int i=0;i<pfEventList.Count;i++)
            {
                if (pfEventList[i] == performEvent)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 检查指定ID的事件是否存储在缓存区
        /// </summary>
        /// <returns>是否存在</returns>
        private bool Checkout(int id)
        {
            for(int i=0;i<pfEventList.Count;i++)
            {
                if (pfEventList[i].ID() == id)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 执行当前指针所在位置的指令，并根据事件返回演出所需要的数据包
        /// </summary>
        /// <returns>演出数据包</returns>
        private PerformData Next(int input)
        {
            //scriptPosition.WriteSelf();
            Console.WriteLine("指令位置：[剧本 = " + scriptPosition.scriptID + "]" + "[指令 = " + scriptPosition.instructionID + "]");
            PerformEventInstructions? instructions = null;
            for(int i=0;i<PFScriptList.Count;i++)
            {
                if (PFScriptList[i].Description.ID == scriptPosition.scriptID)//查询对应ID的剧本
                {
                    PerformScript? script = PFScriptList[i];
                    if (script == null) { break; }
                    instructions = script.InstructionLsit[scriptPosition.instructionID];
                    break;
                }
            }

            //Console.WriteLine(instructions == null);

            if (instructions == null) { return PerformData.ExitData; }//无指令时返回终止演出的数据包

            /*if (scriptPosition.instructionID == 8) //***********测试用-**************
            {
                Console.WriteLine(!Checkout(instructions.PerformEventID));
            }*/
            if(!Checkout(instructions.PerformEventID))//检查缓存区是否存在该事件
            {
                if (instructions.Instrction == PEventInstructions.Start)//仅在开始事件时重新加载事件
                {
                    LoadEvent(instructions.PerformEventID);//加载该事件
                }
            }
            scriptPosition++;//指针跳转至下一个指令
            PerformEvent? performEvent = Get(instructions.PerformEventID);//获取事件对象
            if (performEvent == null) //若无该事件，说明事件加载异常，终止演出
            {
                throw new Exception("事件对象获取出错！");
            }
            PerformData? data = null;
            switch(instructions.Instrction)//读取数据包
            {
                case PEventInstructions.Start:
                    data =  performEvent.StartPlay(input);
                    break;
                case PEventInstructions.Continue:
                    data = performEvent.ContinuePlay(input);
                    break;
                case PEventInstructions.End:
                    UnLoadEvent(performEvent);
                    return Next(input);//结束当前事件，并跳转至下一条指令
                default:
                    Console.WriteLine("指令枚举出错！！已跳过当前指令的执行");
                    return Next(-1);//执行到这段代码说明枚举输入存在问题
            }
            if (performEvent.IsToEnd()) { UnLoadEvent(performEvent); }//如果事件结束了，则将之移除缓存区
            


            switch(data.IDPefdata)
            {
                case -2://跳转数据包
                   // data.WriteSelf();
                    ScriptPosition? position = JsonConvert.DeserializeObject<ScriptPosition>(data.Data[0]);
                    //position.WriteSelf();
                    if (position != null)//设置指令指针的新位置
                    {
                        if(position.scriptID < 0)//则不改变读取的剧本
                        {
                            if(position.instructionID >= 0)//只改变读取的指令
                            {
                                scriptPosition.instructionID = position.instructionID;
                            }
                        }
                        else if(position.instructionID >= 0)//改变剧本位置时，必须改变读取指令的位置
                        {
                            scriptPosition = position;
                        }
                    }
                    return Next(-1);//返回跳转位置的事件数据包
                case -1://终止演出数据包
                    return data;
            }
            return data;
        }
        #endregion 内部函数

        #region 功能
        /// <summary>
        /// 将指定的事件从缓存区中移除
        /// </summary>
        /// <param name="performEvent">指定的事件对象</param>
        /// <returns>执行是否成功</returns>
        public bool UnLoadEvent(PerformEvent performEvent)
        {
            if (performEvent == null) { return false; }
            if (!Checkout(performEvent)) { return false; }
            pfEventList.Remove(performEvent);
            return true;
        }
        /// <summary>
        /// 将指定ID的事件从缓存区中移除
        /// </summary>
        /// <param name="id">事件ID</param>
        /// <returns>执行是否成功</returns>
        public bool UnLoadEvent(int id)
        {
            PerformEvent? performEvent = Get(id);//获取事件对象
            if (performEvent == null) { return false; }
            if (!Checkout(performEvent)) { return false; }
            pfEventList.Remove(performEvent);
            return true;
        }
        /// <summary>
        /// 重新开始演出（初次请求）
        /// </summary>
        /// <param name="input">初次请求附加的初始输入值</param>
        /// <returns>返回演出事件所需的数据包</returns>
        public PerformData Start(int input)
        {
            scriptPosition = ScriptPosition.zero;
            return Next(input);
        }
        public PerformData Continue(int input)
        {
            return Next(input);
        }

        /// <summary>
        /// 获取该对象的 PerformManagerJson 副本，其属性为自己属性的 浅拷贝
        /// </summary>
        /// <returns>自己的 PerformManagerJson 副本</returns>
        public PerformManager_JsonTemplate ToJsonTemplate()
        {
            return new PerformManager_JsonTemplate(this);
        }
        /// <summary>
        /// 向这个剧场里添加剧本（测试用）
        /// </summary>
        /// <param name="performScript">所要添加的剧本对象</param>
        public void AddScript(PerformScript performScript)
        {
            PFScriptList.Add(performScript);
        }
        /// <summary>
        /// 这个剧场的描述
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            return "{[剧场]"+Description.ToString() + "[剧本数量]：" + PFScriptList.Count+"}";
        }
        /// <summary>
        /// 向控制台发送详细的自我描述
        /// </summary>
        public void WriteSelf()
        {
            Console.WriteLine(ToString());
            for(int i=0;i<PFScriptList.Count;i++)
            {
                PFScriptList[i].WriteSelf();
            }
        }
        #endregion 功能
    }
    /// <summary>
    /// 作为 PerformManager 的副本，拥有 PerformManager 的基础属性，用于 Json中的序列化和反序列化
    /// </summary>
    public class PerformManager_JsonTemplate
    {
        #region 属性
        /// <summary>
        /// 该剧场的描述
        /// </summary>
        public NormalDescription Description { get; set; }
        /// <summary>
        /// 该剧场的剧本列表
        /// </summary>
        public PerformScript_JsonTemplate[]? PFScriptList { get; set; }
        #endregion 属性

        #region 构造函数
        /// <summary>
        /// 生成一个 未知的 空副本
        /// </summary>
        public PerformManager_JsonTemplate()
        {
            Description = new NormalDescription("空副本","无描述",-1);
        }
        /// <summary>
        /// 以 performManager 为源，生成一个副本；
        /// 其元素是源对象的元素浅拷贝
        /// </summary>
        /// <param name="performManager">源对象</param>
        public PerformManager_JsonTemplate(PerformManager performManager)
        {
            Description = performManager.Description;
            PerformScript[]? array = performManager.PFScriptList.ToArray();
            if (array == null) { PFScriptList = null; }
            else
            {
                PFScriptList = new PerformScript_JsonTemplate[array.Length];
                for(int i=0;i<PFScriptList.Length;i++)
                {
                    PFScriptList[i] = array[i].ToJsonTemplate();
                }
            }
        }

        #endregion 构造函数
    }

/// <summary>
/// PerformManager类的专属数据读写类
/// </summary>
    public class PerformManagerDataSL
    {
        //以下是文本处理

        #region 写入
        /// <summary>
        /// 将一个 PerformManager 对象序列化成 Json 文本（使用 PerformManager_JsonTemplate ）
        /// </summary>
        /// <param name="performManager">需要序列化的 PerformManager 对象</param>
        /// <returns>Json 文本</returns>
        public static string ToJsonText(PerformManager performManager)
        {
            PerformManager_JsonTemplate jsonTemplate = performManager.ToJsonTemplate();
            return JsonConvert.SerializeObject(jsonTemplate);
        }
        /// <summary>
        /// 将一个 PerformManager 对象序列化成 Json 文本（使用 PerformManager_JsonTemplate ）
        /// </summary>
        /// <param name="jsonTemplate">需要序列化的 PerformManager 的Json模板类</param>
        /// <returns>Json 文本</returns>
        public static string ToJsonText(PerformManager_JsonTemplate jsonTemplate)
        {
            return JsonConvert.SerializeObject(jsonTemplate);
        }
        #endregion 写入

        #region 读取
        /// <summary>
        /// 根据 Json 文本，返回序列化一个 PerformManager 对象（使用 PerformManager_JsonTemplate ）
        /// </summary>
        /// <param name="jsonText">使用的 Json 文本</param>
        /// <returns>一个新的 PerformManager 对象</returns>
        public static PerformManager? ToObjectByJson(string jsonText)
        {
            PerformManager_JsonTemplate? jsonTemplate
                = JsonConvert.DeserializeObject<PerformManager_JsonTemplate>(jsonText);
            if (jsonTemplate == null) { return null; }
            return new PerformManager(jsonTemplate);
        }
        #endregion 读取

        //以下为文件处理
        #region 读取
        /// <summary>
        /// 通过读取 Json 文本文件，得到一个 PerformManager 对象
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>一个新的 PerformManager 对象</returns>
        public static PerformManager? ToObjectByJsonFile(string? path)
        {
            if (path == null) { return null; }
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists && fileInfo.Extension == ".mjson")
            {
                string jsonText = File.ReadAllText(path);
                PerformManager? perform = ToObjectByJson(jsonText);
                if(perform != null) 
                {
                    perform.Path = fileInfo.DirectoryName; 
                }
                return perform;
            }
            else
            {
                Console.WriteLine("路径不存在或者文件格式错误！");
                return null;
            }
        }
        /// <summary>
        /// 通过读取Json文本文件，得到一个 PerformManager 对象
        /// </summary>
        /// <param name="path">Json文本的父文件夹路径</param>
        /// <param name="ID">剧场的ID</param>
        /// <returns>一个新的 PerformManager 对象，如果没有对应ID的场景则返回null</returns>
        public static PerformManager? ToObjectByJsonFile(string? path,int ID)
        {
            if (path == null) { return null; }
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if(directoryInfo.Exists)
            {
                FileInfo[] fileInfos = directoryInfo.GetFiles();
                for(int i=0;i<fileInfos.Length;i++)
                {
                    if (fileInfos[i].Extension == ".mjson")//检查文件格式
                    {
                        string[] name = fileInfos[i].Name.Split("_");
                        if (Convert.ToInt32(name[0]) == ID)//查询对应的ID
                        {
                            return ToObjectByJsonFile(fileInfos[i].FullName);
                        }
                    }
                }
            }
            return null;
        }
        #endregion 读取

        #region 写入
        /// <summary>
        /// 将一个 PerformManager 对象以 Json 模板类的形式保存至一个 Json 文本文件里,
        /// 文件扩展名：.mjson
        /// </summary>
        /// <param name="performManager">欲保存的 PerforManager 对象</param>
        /// <param name="path">保存的路径（包括了文件名）</param>
        public static void ToJsonTextFile(PerformManager performManager, string? path)
        {
            if (path == null) { return; }
            FileInfo fileInfo = new FileInfo(path);
            string jsonText = ToJsonText(performManager);
            if (fileInfo.Exists && fileInfo.Extension == ".mjson")
            {
                File.WriteAllText(path, jsonText);//写入文件
            }
            //对输入路径的处理(如果最后5字符不是.json)
            string[] text = path.Split(@"\");//路径分隔
            int index = 0;
            string newPath;//处理后的新路径
            if ((index = text[text.Length - 1].IndexOf(".")) > 0)//如果文件路径文件名存在扩展名的话
            {
                text[text.Length - 1] = text[text.Length - 1].Substring(0, index);
                newPath = text[0];
                for (int i = 1; i < text.Length; i++)
                {
                    newPath += @"\";
                    newPath += text[i];
                }
                newPath += ".mjson";
            }
            else
            {
                newPath = path + ".mjson";
            }
            jsonText = ToJsonText(performManager);
            File.WriteAllText(newPath, jsonText);//写入文件
        }
        /// <summary>
        /// 将一个 PerformManager 对象 以 Json模板类的形式保存至一个Json文本文件内，自动命名为:
        /// 剧场ID_m.mjson
        /// </summary>
        /// <param name="performManager">需保存的 PerformManager 对象</param>
        /// <param name="path">所在文件夹路径</param>
        public static void ToJsonTextFileAuto(PerformManager performManager, string? path)
        {
            if (path == null) { return; }
            string pathFull = path + @"\" + performManager.Description.ID + "_" + "m.mjson";
            string text = ToJsonText(performManager);
            File.WriteAllText(pathFull, text);
        }
        /// <summary>
        /// 将一个 PerformManager 对象 以 Json模板类的形式保存至一个Json文本文件内，自动命名为:
        /// 剧场ID_m.mjson
        /// </summary>
        /// <param name="jsonTemplate">一个 PerformManager 对象的Json模板副本</param>
        /// <param name="path">保存路径（父文件夹）</param>
        public static void ToJsonTextFileAuto(PerformManager_JsonTemplate jsonTemplate, string? path)
        {
            if (path == null) { return; }
            string pathFull = path + @"\" + jsonTemplate.Description.ID + "_" + "m.mjson";
            string text = ToJsonText(jsonTemplate);
            File.WriteAllText(pathFull, text);
        }
        #endregion 写入
    }
}
