using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    /// <summary>
    /// 演出数据，用于封装演出事件所需数据的类
    /// </summary>
    public class PerformData
    {
        #region 属性
        /// <summary>
        /// 此类数据包的唯一标识ID，建议使用时用枚举封装，用于数据分类，发送给不同种类的事件实现器；
        /// </summary>
        public int IDPefdata = 0;
        /// <summary>
        /// 数据以字符串数组的形式储存，具体解析根据数据种类（由ID区分种类）；
        /// 演出实现器会根据ID，发送给不同的事件实现器中解析；
        /// 数据解析器，放置于事件实现器中；
        /// </summary>
        public string[]? Data { get; set; }
        #endregion 属性

        #region 构造函数
        /// <summary>
        /// 初始化一个空的演出数据
        /// </summary>
        public PerformData() { Data = new string[0]; }
        /// <summary>
        /// 初始化一个标准的演出数据
        /// </summary>
        /// <param name="iDPefdata">数据类型ID</param>
        /// <param name="data">数据表</param>
        public PerformData(int iDPefdata, string[]? data)
        {
            IDPefdata = iDPefdata;
            Data = data;
        }
        public PerformData(int iDPefdata, string? headData)
        {
            IDPefdata = iDPefdata;
            if (headData == null) { Data = null; }
            else
            {
                Data = new string[] { headData};
            }
        }
        #endregion 构造函数

        #region 功能
        /// <summary>
        /// 向控制台输出自身的描述
        /// </summary>
        public void WriteSelf()
        {
            string dataAll = "{";
            if(Data != null)
            {
                for (int i = 0;i < Data.Length;i++)
                {
                    dataAll += "[" + Data[i] + "]";
                }
            }
            dataAll += "}";
            Console.WriteLine("[数据包]<id = " + IDPefdata + ">" + dataAll );
        }
        /// <summary>
        /// 判断两个数据包是否相同：
        /// 数据ID 和 数据内容相同 则使其相同
        /// </summary>
        /// <param name="data">比较的另一个 PerformData 对象</param>
        /// <returns>比较结果</returns>
        public bool Equals(PerformData data)
        {
            if(IDPefdata == data.IDPefdata)
            {
                if (Data == null && data.Data == null) { return true; }
                if(Data != null && data.Data != null)
                {
                    if(Data == data.Data)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取头数据（如果头数据不存在，则返回(下一个位置的Json文本)）
        /// </summary>
        /// <returns>头数据</returns>
        public string GetDataHead()
        {
            if(Data != null)
            {
                if(Data.Length > 0)
                {
                    return Data[0];
                }
            }
            return "{\"scriptID\":-1,\"instructionID\":-1}";
        }
        #endregion 功能

        #region 静态对象
        /// <summary>
        /// 演出数据指令-关闭演出
        /// </summary>
        public static PerformData ExitData => new(-1, (string?)null);
        /// <summary>
        /// 跳过该条指令，并跳转至指定位置
        /// </summary>
        public static PerformData SkipData => new(-2, (string?)null);
        #endregion 静态对象
    }

}
