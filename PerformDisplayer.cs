using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    namespace Display
    {

        /// <summary>
        /// 演出展示器；用于从 演出管理器 获取演出信息，并把相应的信息分发给不同的事件处理器；
        /// </summary>
        public class PerformDisplayer
        {
            #region 属性
            /// <summary>
            /// 这个展示器的描述
            /// </summary>
            public NormalDescription? Description { get; set; }
            /// <summary>
            /// 展示器激活状态
            /// </summary>
            private bool active = false;
            /// <summary>
            /// 当前累计执行事件的个数
            /// </summary>
            public int Count { get; private set; }
            /// <summary>
            /// 事件解析后的反馈信息(无返回时，默认值，为-1)
            /// </summary>
            private int Input { get; set; }
            #endregion 属性

            #region 管理属性
            /// <summary>
            /// 这个展示器所关联的管理器对象
            /// </summary>
            public PerformManager? Manager { get; private set; }
            /// <summary>
            /// 激活/停止这个展示器
            /// </summary>
            public bool Active
            {
                get => active;
                set
                {
                    if (value && !active)
                    {
                        active = true;
                        if (Count == 0) { Start(); }
                        else { Continue(); }
                    }
                    else
                    {
                        active = false;
                    }
                }
            }
            /// <summary>
            /// 事件解析器列表
            /// </summary>
            public SimpleList<IEventParser> ParserList { get; private set; }
            #endregion 管理属性

            #region 构造函数
            /// <summary>
            /// 初始化一个标准的 展示器（未激活）
            /// </summary>
            /// <param name="manager">与展示器关联的管理器</param>
            public PerformDisplayer(PerformManager manager)
            {
                Manager = manager;
                ParserList = new SimpleList<IEventParser>();
                active = false;
                Input = -1;
            }
            /// <summary>
            /// 初始化一个标准的 展示器（未激活）
            /// </summary>
            /// <param name="manager">与展示器关联的管理器</param>
            /// <param name="input">设置默认的输入值</param>
            public PerformDisplayer(PerformManager manager, int input)
            {
                Manager = manager;
                ParserList = new SimpleList<IEventParser>();
                active = false;
                Input = input;
            }
            #endregion 构造函数

            #region 功能
            /// <summary>
            /// 添加新的事件解析器（同类型解析器只允许存在一个）
            /// </summary>
            /// <returns>是否成功添加</returns>
            public bool AddParser(IEventParser parser)
            {
                for (int i = 0; i < ParserList.Count; i++)
                {
                    if (ParserList[i].IDPefdata == parser.IDPefdata)//如果ID相同则无法添加
                    {
                        return false;
                    }
                }
                ParserList.Add(parser);
                return true;
            }
            /// <summary>
            /// 移除 处理指定事件类型 的事件解析器
            /// </summary>
            /// <param name="IDPefdata">事件解析器处理的事件类型ID</param>
            /// <returns>是否成功移除</returns>
            public bool RemoveParser(int IDPefdata)
            {
                for (int i = 0; i < ParserList.Count; i++)
                {
                    if (ParserList[i].IDPefdata == IDPefdata)
                    {
                        ParserList.Remove(i);
                        return true;
                    }
                }
                return false;
            }
            /// <summary>
            /// 作为委托提供给事件解析器回调，通知此展示器反馈结果
            /// </summary>
            private void Result(int input)
            {
                Input = input;
                Continue();
            }
            /// <summary>
            /// 初次向管理器提交信息请求
            /// </summary>
            public void Start()
            {
                if (active)
                {
                    if (Manager != null)
                    {
                        PerformData data = Manager.Start(Input);//初次提交信息请求（默认输入值为0）
                        
                        switch (data.IDPefdata)//特殊事件处理部分
                        {
                            case -1: active = false; return;
                        }
                        for (int i = 0; i < ParserList.Count; i++)
                        {
                            if (ParserList[i].IDPefdata == data.IDPefdata)//匹配ID相同的事件解析器
                            {
                                Input = 0;//解析事件前先清空输入状态
                                ParserList[i].Parse(data, Result);//解析事件
                            }
                        }
                        Count++;
                    }
                }
            }
            /// <summary>
            /// 继续向管理器提交信息请求
            /// </summary>
            public void Continue()
            {
                if (active)
                {
                    if (Manager != null)
                    {
                        PerformData data = Manager.Continue(Input);//初次提交信息请求（默认输入值为0）
                        switch (data.IDPefdata)//特殊事件处理部分
                        {
                            case -1: active = false; return;
                        }
                        for (int i = 0; i < ParserList.Count; i++)
                        {
                            if (ParserList[i].IDPefdata == data.IDPefdata)//匹配ID相同的事件解析器
                            {
                                Input = 0;//解析事件前先清空输入状态
                                ParserList[i].Parse(data, Result);//解析事件
                            }
                        }
                        Count++;
                    }
                }
            }
            #endregion 功能
        }
    }
}
