using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PerformEndless
{
    /// <summary>
    /// 演出剧本，仅存储单线演出；
    /// 也就是剧场的指令列表（长度可变）
    /// </summary>
    public class PerformScript
    {
        #region 属性
        /// <summary>
        /// 该剧本的描述
        /// </summary>
        public NormalDescription Description { get; private set; }
        /// <summary>
        /// 指令列表
        /// </summary>
        public SimpleList<PerformEventInstructions> InstructionLsit { get; private set; }
        #endregion 属性

        #region 构造函数
        /// <summary>
        /// 初始化一个空剧本
        /// </summary>
        public PerformScript()
        {
            InstructionLsit = new SimpleList<PerformEventInstructions>();
            Description = new NormalDescription("空剧本","无描述",-1);
        }
        /// <summary>
        /// 初始化一个标准的剧本
        /// </summary>
        /// <param name="name">剧本名称</param>
        /// <param name="description">剧本描述</param>
        /// <param name="id">剧本ID</param>
        public PerformScript(string name, string description, int id)
        {
            Description = new NormalDescription(name, description, id);
            InstructionLsit = new SimpleList<PerformEventInstructions>();
        }
        /// <summary>
        /// 初始化一个标准的剧本
        /// </summary>
        /// <param name="name">剧本名称</param>
        /// <param name="description">剧本描述</param>
        /// <param name="id">剧本ID</param>
        /// <param name="instructionLsit">剧本中的指令集</param>
        public PerformScript(string name, string description,int id, SimpleList<PerformEventInstructions> instructionLsit)
        {
            Description = new NormalDescription(name, description, id);
            InstructionLsit = instructionLsit;
        }
        /// <summary>
        /// 根据 PerformScriptDataJson 初始化一个剧本
        /// </summary>
        /// <param name="performScriptDataJson">Json数据封装类</param>
        /// <param name="instructionLsit">剧本的指令集</param>
        public PerformScript(NormalDescription normalDescription,SimpleList<PerformEventInstructions>? instructionLsit)
        {
            if (normalDescription != null) { Description = normalDescription; }
            else { Description = new NormalDescription("空剧本", "无描述", -1); }
            if (instructionLsit == null) { InstructionLsit = new SimpleList<PerformEventInstructions>(); }
            else { InstructionLsit = instructionLsit; }
            
        }
        /// <summary>
        /// 根据 PerformManager_JsonTemplate 对象创建 PerformScript 对象；
        /// 新建的 PerformScript 对象的管理元素 是 PerformManager_JsonTemplate 的浅拷贝
        /// </summary>
        /// <param name="performScriptAy">源对象</param>
        public PerformScript(PerformScript_JsonTemplate jsonTemplate)
        {
            Description = jsonTemplate.Description;
            PerformEventInstructions[] array = new PerformEventInstructions[jsonTemplate.InstructionLsit.Length];
            for(int i=0;i<array.Length;i++)
            {
                array[i] = new PerformEventInstructions(jsonTemplate.InstructionLsit[i]);
            }
            InstructionLsit = new SimpleList<PerformEventInstructions>(array);
        }
        #endregion 构造函数

        #region 功能
        /// <summary>
        /// 根据索引值获取该剧本的指定指令
        /// </summary>
        /// <param name="index">索引值</param>
        /// <returns>获取的指令</returns>
        public PerformEventInstructions? Get(int index)
        {
            return InstructionLsit[index];
        }

        /// <summary>
        /// 得到这个剧本的描述
        /// </summary>
        /// <returns>描述文本</returns>
        public new string ToString()
        {
            return "{[剧本]" + Description.ToString() + "[指令数量]：" + InstructionLsit.Count + "}";
        }
        /// <summary>
        /// 向控制台发送详细的自我描述
        /// </summary>
        public void WriteSelf()
        {
            Console.WriteLine(ToString());
            for(int i=0;i<InstructionLsit.Count;i++)
            {
                InstructionLsit[i].WriteSelf();
            }
        }
        /// <summary>
        /// 向这个剧本的末尾添加新指令（测试用）
        /// </summary>
        /// <param name="performEventInstructions">指令</param>
        public void AddInstructions(PerformEventInstructions performEventInstructions)
        {
            InstructionLsit.Add(performEventInstructions);
        }
        /// <summary>
        /// 创建一个以自己为源的 PerformScriptAy 类，其管理的指令元素来自 自己的子元素的浅拷贝
        /// </summary>
        /// <returns>作为自己副本的 PerformScriptAy 对象</returns>
        public PerformScript_JsonTemplate ToJsonTemplate()
        {
            PerformEventInstructions[]? performEventInstructions = InstructionLsit.ToArray();
            return new PerformScript_JsonTemplate(Description, performEventInstructions);
        }
        #endregion 功能

    }
    /// <summary>
    /// PerformScript 的Json存储模板副本，其属性是源对象的属性浅拷贝
    /// </summary>
    public class PerformScript_JsonTemplate
    {
        #region 属性
        /// <summary>
        /// 该剧本的描述
        /// </summary>
        public NormalDescription Description { get; set; }
        public PerformEventInstructions_JsonTemplate[]? InstructionLsit { get; set; }
        #endregion 属性

        #region 构造函数
        public PerformScript_JsonTemplate() { Description = new NormalDescription("空副本","无描述",-1); }
        /// <summary>
        /// 初始化一个标准的剧本
        /// </summary>
        /// <param name="name">剧本名称</param>
        /// <param name="description">剧本描述</param>
        /// <param name="id">剧本ID</param>
        /// <param name="instructionLsit">剧本中的指令集</param>
        public PerformScript_JsonTemplate(string name, string description, int id, PerformEventInstructions[]? instructionLsit)
        {
            Description = new NormalDescription(name, description, id);
            InstructionLsit = new PerformEventInstructions_JsonTemplate[instructionLsit.Length];
            for(int i=0;i<InstructionLsit.Length;i++)
            {
                InstructionLsit[i] = instructionLsit[i].ToJsonTemplate();
            }
        }
        /// <summary>
        /// 根据 PerformScriptDataJson 初始化一个剧本
        /// </summary>
        /// <param name="performScriptDataJson">Json数据封装类</param>
        /// <param name="instructionLsit">剧本的指令集</param>
        public PerformScript_JsonTemplate(NormalDescription? normalDescription, PerformEventInstructions[]? instructionLsit)
        {
            if (normalDescription != null) { Description = normalDescription; }
            else { Description = new NormalDescription("空剧本", "无描述", -1); }
            InstructionLsit = new PerformEventInstructions_JsonTemplate[instructionLsit.Length];
            for (int i = 0; i < InstructionLsit.Length; i++)
            {
                InstructionLsit[i] = instructionLsit[i].ToJsonTemplate();
            }
        }
        #endregion 构造函数
    }

    /// <summary>
    /// PerformScript 数据的专用读写类
    /// </summary>
    public class PerformScriptDataSL
    {
        /// <summary>
        /// 以 PerformScript_JsonTemplate 的形式使一个 PerformScript 序列化成一个 Json文本
        /// </summary>
        /// <param name="performScript">需要转化的 PerformScript 对象</param>
        /// <returns>Json 文本</returns>
        public static string ToJsonText(PerformScript performScript)
        {
            PerformScript_JsonTemplate jsonTemplate = performScript.ToJsonTemplate();
            return JsonConvert.SerializeObject(jsonTemplate);
        }
        /// <summary>
        /// 使用由 PerformScript_JsonTemplate 序列化的 Json文本 创建一个 PerformScript 对象
        /// </summary>
        /// <param name="jsonText">一个新的 PerformScript 对象</param>
        /// <returns></returns>
        public static PerformScript? ToObjectByJson(string jsonTextAy)
        {
            PerformScript_JsonTemplate? performScriptAy = JsonConvert.DeserializeObject<PerformScript_JsonTemplate>(jsonTextAy);

            Console.WriteLine(performScriptAy.InstructionLsit.Length);
            for(int i = 0;i< performScriptAy.InstructionLsit.Length;i++)
            {
                Console.WriteLine(performScriptAy.InstructionLsit[i].ToString());
            }

            if (performScriptAy == null) { return null; }
            return new PerformScript(performScriptAy);
        }
    }
}
