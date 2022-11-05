using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    /// <summary>
    /// 描述一个指令在演出管理器的位置
    /// </summary>
    public class ScriptPosition
    {
        #region 属性
        /// <summary>
        /// 所属剧本 的 位置
        /// </summary>
        public int scriptID;
        /// <summary>
        /// 该指令在剧本中的位置
        /// </summary>
        public int instructionID;
        #endregion 属性

        #region 构造函数
        /// <summary>
        /// 初始化一个标准的事件位置
        /// </summary>
        /// <param name="scriptID">事件所属剧本的位置</param>
        /// <param name="instructionID">该指令的位置</param>
        public ScriptPosition(int scriptID, int instructionID)
        {
            this.scriptID = scriptID;
            this.instructionID = instructionID;
        }
        #endregion 构造函数

        #region 静态对象
        /// <summary>
        /// 事件初始位置 0号剧本 0号位置
        /// </summary>
        public static ScriptPosition zero = new ScriptPosition(0, 0);
        /// <summary>
        /// 表示相对位置 同一剧本的下一个位置
        /// </summary>
        public static ScriptPosition next = new ScriptPosition(-1, -1);
        #endregion 静态对象

        #region 功能
        /// <summary>
        /// 使 ScriptPosition 对象的指令指针向后移动一位
        /// </summary>
        /// <param name="scriptPosition">目标对象</param>
        /// <returns>对象自身</returns>
        public static ScriptPosition operator ++(ScriptPosition scriptPosition)
        {
            scriptPosition.instructionID++;
            return scriptPosition;
        }
        /// <summary>
        /// 得到一个自我描述文本
        /// </summary>
        /// <returns>描述文本</returns>
        public new string ToString()
        {
            return "[指针位置]("+scriptID+","+instructionID+")";
        }
        /// <summary>
        /// 向控制台输出自我描述
        /// </summary>
        public void WriteSelf()
        {
            Console.WriteLine(ToString());
        }
        /// <summary>
        /// 获取一个位置类的Json文本
        /// </summary>
        /// <param name="sId"></param>
        /// <param name="iId"></param>
        /// <returns></returns>
        public static string GetJsonText(int sId,int iId)
        {
            string text = "{\"scriptID\":" + sId + ",\"instructionID\":" + iId + "}";
            Console.WriteLine(text);
            return text;
        }
        #endregion 功能
    }
}
