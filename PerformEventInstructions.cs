using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PerformEndless
{
    /// <summary>
    /// 指令区分枚举：Start，Continue，End
    /// </summary>
    public enum PEventInstructions {
        /// <summary>
        /// 启用该事件
        /// </summary>
        Start,
        /// <summary>
        /// 使阻塞的事件继续执行
        /// </summary>
        Continue,
        /// <summary>
        /// 强制结束该事件
        /// </summary>
        End }

    /// <summary>
    /// 演出事件指令的封装类
    /// </summary>
    public class PerformEventInstructions
    {
        #region 属性
        /// <summary>
        /// 该指令所指向的事件对象
        /// </summary>
        public int PerformEventID { get;private set; }
        /// <summary>
        /// 对于事件的操作指令
        /// </summary>
        public PEventInstructions Instrction { get;private set; }
        #endregion 属性

        #region 构造函数
        /// <summary>
        /// 初始化一个空事件指令
        /// </summary>
        public PerformEventInstructions() { }
        /// <summary>
        /// 初始化一个标准的事件指令
        /// </summary>
        /// <param name="performID">指令作用于的事件对象</param>
        /// <param name="pEventInstructions">指令区分枚举</param>
        public PerformEventInstructions(int performID,PEventInstructions pEventInstructions)
        {
            PerformEventID = performID;
            Instrction = pEventInstructions;
        }
        /// <summary>
        /// 初始化一个标准的事件指令
        /// </summary>
        /// <param name="performID">指令作用于的事件对象</param>
        /// <param name="pEventInstructions">指令区分枚举</param>
        /// <param name="scriptPosition">执行完该指令后指针所在的位置</param>
        public PerformEventInstructions(int performID, PEventInstructions performEventInstructions,ScriptPosition scriptPosition)
        {
            PerformEventID = performID;
            Instrction = performEventInstructions;
        }
        /// <summary>
        /// 根据一个自身的 Json 模板类创建一个 PerformEventInstructions 对象
        /// </summary>
        /// <param name="jsonTemplate">Json 模板对象</param>
        public PerformEventInstructions(PerformEventInstructions_JsonTemplate jsonTemplate)
        {
            PerformEventID = jsonTemplate.PerformEventID;
            Instrction = jsonTemplate.Instrction;
        }
        #endregion 构造函数

        #region 功能
        /// <summary>
        /// 得到自身的一个 Json模板类 （数据浅拷贝）
        /// </summary>
        /// <returns>Json模板类</returns>
        public PerformEventInstructions_JsonTemplate ToJsonTemplate()
        {
            return new PerformEventInstructions_JsonTemplate(PerformEventID,Instrction);
        }
        /// <summary>
        /// 获取这个对象的自描述
        /// </summary>
        /// <returns>描述文本</returns>
        public new string ToString()
        {
            String em = "";
            switch (Instrction)
            {
                case PEventInstructions.Start:
                    em = "Start";
                    break;
                case PEventInstructions.Continue:
                    em = "Continue";
                    break;
                case PEventInstructions.End:
                    em = "End";
                    break;
            }
            return "{[事件指令][对象ID]:" + PerformEventID + "<"+em+">}";
        }
        /// <summary>
        /// 向控制台发送详细的自我描述
        /// </summary>
        public void WriteSelf()
        {
            Console.WriteLine(ToString());
        }
        #endregion 功能
    }
    /// <summary>
    /// PerformEventInstructions 的Json格式副本
    /// </summary>
    public class PerformEventInstructions_JsonTemplate
    {
        #region 属性
        /// <summary>
        /// 该指令所指向的事件对象
        /// </summary>
        public int PerformEventID { get; set; }
        /// <summary>
        /// 对于事件的操作指令
        /// </summary>
        public PEventInstructions Instrction { get; set; }
        #endregion 属性

        #region 构造函数
        /// <summary>
        /// 初始化一个空副本
        /// </summary>
        public PerformEventInstructions_JsonTemplate() { }
        /// <summary>
        /// 初始化一个标准副本（数据浅拷贝）
        /// </summary>
        /// <param name="id">事件ID</param>
        /// <param name="instructions">指令枚举</param>
        /// <param name="position">执行完后指针的位置</param>
        public PerformEventInstructions_JsonTemplate(int id, PEventInstructions instructions)
        {
            PerformEventID = id;
            Instrction = instructions;
        }
        #endregion 构造函数
    }
    /// <summary>
    /// PerformEventInstructions 的专用数据读写类
    /// </summary>
    public static class PerformEventInstructionsDataSL
    {
        /// <summary>
        /// 通过 Json 文本创建一个 PerformEventInstructions 对象（使用Json模板类）
        /// </summary>
        /// <param name="jsonText">Json 文本</param>
        /// <returns>一个新的 PerformEventInstructions 对象</returns>
        public static PerformEventInstructions? ToPerformEventInstructionsByJson(string jsonText)
        {
            PerformEventInstructions_JsonTemplate? jsonTemplate
                = JsonConvert.DeserializeObject<PerformEventInstructions_JsonTemplate>(jsonText);
            if (jsonTemplate == null) { return null; }
            return new PerformEventInstructions(jsonTemplate);
        }
        /// <summary>
        /// 将一个 performEventInstructions 序列化成一段 Json 文本（使用Json模板类）
        /// </summary>
        /// <param name="performEventInstructions">转化对象</param>
        /// <returns>Json文本</returns>
        public static string ToJsonText(PerformEventInstructions performEventInstructions)
        {
            PerformEventInstructions_JsonTemplate jsonTemplate = performEventInstructions.ToJsonTemplate();
            return JsonConvert.SerializeObject(jsonTemplate);
        }
    }
}
