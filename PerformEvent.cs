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
    /// 演出事件；用于存储一类事件的数据包
    /// </summary>
    public class PerformEvent
    {
        #region 属性
        /// <summary>
        /// 该事件类型的唯一事件类型ID；建议在具体使用时，定义一个全局枚举来替代数字ID；
        /// 且事件使用的数据的类型ID 和事件类型ID 需保持一致；
        /// </summary>
        public readonly int IDPefevent = 0;
        /// <summary>
        /// 该事件的描述
        /// </summary>
        public NormalDescription Description { get; private set; }
        /// <summary>
        /// 该事件所引用的数据包
        /// </summary>
        public SimpleList<PerformData> Data { get; private set; }
        /// <summary>
        /// 指针的开始位置
        /// </summary>
        public int StartIndex { get; private set; }
        #endregion 属性

        #region 管理属性
        /// <summary>
        /// 该事件当前执行的进度（用于事件阻塞后继续执行事件）
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 所属演出系统，此演出信息将通知给该系统
        /// </summary>
        public EventStore Owner { get; private set; }
        #endregion 管理属性

        #region 构造函数
        /// <summary>
        /// 初始化一个空事件
        /// </summary>
        /// <param name="owner"></param>
        public PerformEvent(EventStore owner) 
        {
            Description = new NormalDescription("空事件","无描述",-1);
            Owner = owner;
            Data = new SimpleList<PerformData>();
            StartIndex = 0;
        }
        /// <summary>
        /// 根据 PerformEvent_JsonTemplate 生成一个标准的事件
        /// </summary>
        /// <param name="jsonTemplate">Json模板</param>
        /// <param name="owner">所属管理器对象</param>
        public PerformEvent(PerformEvent_JsonTemplate jsonTemplate, EventStore owner)
        {
            IDPefevent = jsonTemplate.IDPefevent;
            Description = jsonTemplate.Description;
            StartIndex = jsonTemplate.StartIndex;
            Data = new SimpleList<PerformData>(jsonTemplate.Data);
            Owner = owner;
        }
        #endregion 构造函数

        #region 功能
        /// <summary>
        /// 检查事件是否已经到结尾（当前指针位置，没有数据
        /// </summary>
        /// <returns></returns>
        public bool IsToEnd()
        {
            if (Index > Data.Count - 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 得到一个自身的Json模板对象，其属性为自身属性的浅拷贝
        /// </summary>
        /// <returns></returns>
        public PerformEvent_JsonTemplate ToJsonTemplate()
        {
            return new PerformEvent_JsonTemplate(IDPefevent, Description, Data.ToArray(), StartIndex);
        }
        /// <summary>
        /// 向控制台输出自己的详细描述
        /// </summary>
        public void WriteSelf()
        {
            Console.WriteLine("[事件]<IDpe = "+IDPefevent+">["+Description.Name+"]"
                +Description.Description+"<id = "+Description.ID+">StartForm:"+StartIndex);
            for(int i=0;i<Data.Count;i++)
            {
                Data[i].WriteSelf();
            }
        }
        /// <summary>
        /// 获取该事件在全剧场中的唯一标识ID
        /// </summary>
        /// <returns>事件的ID</returns>
        public int ID()
        {
            return Description.ID;
        }
        /// <summary>
        /// 重新开始该事件的执行
        /// </summary>
        /// <param name="input">来自展示器的输入值</param>
        /// <returns>返回该事件需要的数据</returns>
        public PerformData StartPlay(int input)
        {
            //WriteSelf();
           // Console.WriteLine("<id = " + ID() + ">");
            if (input == -1)
            {
                // Console.WriteLine("[id = "+ID()+"]");
                Index = StartIndex;
               // Console.WriteLine("Index:" + Index+"  Count:"+Data.Count);
                if (Data.Count == 0) //如果数据包为空的话，则结束事件
                {
                    EndPlay();
                    return PerformData.SkipData;
                }
                PerformData? data = Data[Index];
                if (data == null) { return PerformData.SkipData; }
                Index++;
                return data;
            }
            else
            {
                PerformData? data = Data[input];
                if (data == null) { return PerformData.SkipData; }
                return data;
            }
        }
        /// <summary>
        /// 解除该事件的阻塞状态，继续执行
        /// </summary>
        /// <param name="input">来自展示器的输入值</param>
        /// <returns>返回该事件需要的数据</returns>
        public PerformData ContinuePlay(int input)
        {
            Console.WriteLine("INPUT:" + input);
            if (input == -1)
            {
                if (Index > Data.Count)
                {
                    EndPlay();
                    return PerformData.SkipData;
                }
                PerformData? data = Data[Index];
                Index++;
                if (data == null)
                {
                    return PerformData.SkipData;
                }
                return data;
            }
            else
            {
                //Console.WriteLine("INPUT:"+input);
                //Console.WriteLine(Data[input] == null);
                PerformData? data = Data[input];
                //Data[input].WriteSelf();
                if (data == null) { return PerformData.SkipData; }
                return data;
            }
            }
        /// <summary>
        /// 强制结束该事件的执行，通知所属的管理类将自身移除缓存区
        /// </summary>
        public void EndPlay()
        {
            Owner.EndEvent(this);
        }

        #endregion 功能
    }

    /// <summary>
    /// PerformEvent 的Json模板类
    /// </summary>
    public class PerformEvent_JsonTemplate
    {
        #region 属性
        /// <summary>
        /// 事件类型ID
        /// </summary>
        public int IDPefevent { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public NormalDescription Description { get; set; }
        /// <summary>
        /// 数据包
        /// </summary>
        public PerformData[]? Data { get; set; }
        /// <summary>
        /// 指针的开始位置
        /// </summary>
        public int StartIndex { get; set; }
        #endregion 属性

        #region 构造函数
        /// <summary>
        /// 生成一个空数据 PerformEvent_JsonTemplate 对象
        /// </summary>
        public PerformEvent_JsonTemplate()
        {
            Description = new NormalDescription("未知事件","没有描述",0);
        }
        /// <summary>
        /// 生成一个标准的 PerformEvent_JsonTemplate 对象
        /// </summary>
        /// <param name="iDPefevent">事件类型ID</param>
        /// <param name="description">描述</param>
        /// <param name="data">数据包组</param>
        /// <param name="startIndex">初始指针位置</param>
        public PerformEvent_JsonTemplate(int iDPefevent, NormalDescription description, PerformData[]? data,int startIndex)
        {
            IDPefevent = iDPefevent;
            Description = description;
            Data = data;
            StartIndex = startIndex;
        }
        #endregion 构造函数
    }
    /// <summary>
    /// PerformEvent 的专用读写类
    /// </summary>
    public class PerformEventDataSL
    {
        //以下是对文本处理

        #region 写入
        /// <summary>
        /// 将一个 PerformEvent 对象序列化成 Json文本
        /// </summary>
        /// <param name="performEvent">目标 PerformEvent 对象</param>
        /// <returns>Json 文本</returns>
        public static string ToJsonText(PerformEvent performEvent)
        {
            PerformEvent_JsonTemplate jsonTemplate
                = performEvent.ToJsonTemplate();
            return JsonConvert.SerializeObject(jsonTemplate);
        }
        /// <summary>
        /// 将一个 PerformEvent 对象序列化成 Json文本
        /// </summary>
        /// <param name="jsonTemplate">目标 PerformEvent 对象的 Json模板副本对象</param>
        /// <returns>Json 文本</returns>
        public static string ToJsonText(PerformEvent_JsonTemplate jsonTemplate)
        {
            return JsonConvert.SerializeObject(jsonTemplate);
        }
        #endregion 写入

        #region 读取
        /// <summary>
        /// 根据 Json 文本反序列化得到一个 PerformEvent 对象
        /// </summary>
        /// <param name="jsonText">使用的 Json 文本</param>
        /// <param name="store">所属的管理器对象</param>
        /// <returns>一个新的 PerformEvent 对象</returns>
        public static PerformEvent? ToObjectByJson(string jsonText,EventStore store)
        {
            PerformEvent_JsonTemplate? jsonTemplate
                = JsonConvert.DeserializeObject<PerformEvent_JsonTemplate>(jsonText);
            if (jsonTemplate == null) { return null; }
            return new PerformEvent(jsonTemplate,store);
        }
        #endregion 读取

        //以下是对文件处理

        #region 读取
        /// <summary>
        /// 通过读取 Json 文件，得到一个 PerformEvent 对象
        /// </summary>
        /// <param name="path">文件路径（包括文件名）</param>
        /// <param name="store">所属的管理器对象</param>
        /// <returns>一个新的 PerfornEvent 对象</returns>
        public static PerformEvent? ToObjectByJsonFile(string? path, EventStore store)
        {
            if (path == null) { return null; }
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists && fileInfo.Extension == ".ejson")
            {
                string jsonText = File.ReadAllText(path);
                PerformEvent? perform = ToObjectByJson(jsonText,store);
                return perform;
            }
            else
            {
                Console.WriteLine("路径不存在或者文件格式错误！");
                return null;
            }
        }
        #endregion 读取

        #region 写入
        /// <summary>
        /// 将一个 PerformEvent 对象保存进Json文本文件
        /// </summary>
        /// <param name="performEvent">需保存的 PerformEvent 对象</param>
        /// <param name="path">保存文件的路径（包含文件名）</param>
        public static void ToJsonTextFile(PerformEvent performEvent, string? path)
        {
            if (path == null) { return; }
            FileInfo fileInfo = new FileInfo(path);
            string jsonText = ToJsonText(performEvent);
            if (fileInfo.Exists && fileInfo.Extension == ".ejson")
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
                newPath += ".ejson";
            }
            else
            {
                newPath = path + ".ejson";
            }
            jsonText = ToJsonText(performEvent);
            File.WriteAllText(newPath, jsonText);//写入文件
        }
        /// <summary>
        /// 将一个 PerformEvent 对象保存进Json文本文件
        /// 且以"事件类型ID"+"_"+"事件名称"+"_"+"事件对象ID"的形式命名;
        /// 如果存在同名文件，则覆盖保存
        /// </summary>
        /// <param name="performEvent">需保存的 PerformEvent 对象</param>
        /// <param name="path">保存文件的父文件夹的路径</param>
        public static void ToJsonTextFileAuto(PerformEvent performEvent, string? path)
        {
            if (path == null) { return; }
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists) { directoryInfo.Create(); }
            string pathD = path + @"\" + performEvent.Description.ID +"_"+ performEvent.IDPefevent + ".ejson";
            string jsonText = ToJsonText(performEvent);
            FileInfo file = new FileInfo(pathD);
            File.WriteAllText(pathD, jsonText);
        }
        /// <summary>
        /// 将一个 PerformEvent 对象保存进Json文本文件
        /// 且以"事件类型ID"+"_"+"事件名称"+"_"+"事件对象ID"的形式命名;
        /// 如果存在同名文件，则覆盖保存
        /// </summary>
        /// <param name="performEvent">需保存的 PerformEvent 对象的 Json模板副本</param>
        /// <param name="path">保存文件的父文件夹的路径</param>
        
        #endregion 写入
    }
}
