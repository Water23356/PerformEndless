using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;

namespace PerformEndless
{
    public enum ValueType { String, Number, Boolean }

    /// <summary>
    /// 基础模板
    /// </summary>
    public class Template
    {
        #region 属性
        /// <summary>
        /// 类型ID
        /// </summary>
        public int id;
        /// <summary>
        /// 类型名称
        /// </summary>
        public string name;
        /// <summary>
        /// 事件类型描述
        /// </summary>
        public string description;
        /// <summary>
        /// 控件json文本
        /// </summary>
        public List<string> json_template;
        /// <summary>
        /// 控件模板对象(ED)
        /// </summary>
        private List<Template_Control> controls;
        public List<Template_Control> Controls { get { return controls; } }
        /// <summary>
        /// 根据当前json列表更新模板单元列表
        /// </summary>
        public void UpdateFormJson()
        {
            foreach(string s in json_template)
            {

            }
        }
        #endregion
    }

    public enum TP_Tpye { Combobox,InputField,CheckBox}
    /// <summary>
    /// 控件模板基础类
    /// </summary>
    public class Template_Control
    {
        /// <summary>
        /// 控件类型
        /// </summary>
        public TP_Tpye tp_Tpye;
        /// <summary>
        /// 键
        /// </summary>
        public string key;
        /// <summary>
        /// 项名称
        /// </summary>
        public string name;
        /// <summary>
        /// 值的类型
        /// </summary>
        public ValueType valueType;
        /// <summary>
        /// 此项的描述
        /// </summary>
        public string description;
    }
    /// <summary>
    /// 下拉框模板
    /// </summary>
    public class Template_Combobox : Template_Control
    {
        /// <summary>
        /// 控件类型
        /// </summary>
        public new readonly TP_Tpye tp_Tpye = TP_Tpye.Combobox;
        /// <summary>
        /// 选项名称
        /// </summary>
        public List<string> items;
        /// <summary>
        /// 值(一个选项对应一个值)
        /// </summary>
        public List<string> values;
    }

    /// <summary>
    /// 输入框模板
    /// </summary>
    public class Template_InputField : Template_Control
    {
        /// <summary>
        /// 控件类型
        /// </summary>
        public new readonly TP_Tpye tp_Tpye = TP_Tpye.InputField;
        /// <summary>
        /// 值的限制
        /// </summary>
        public string limit;
    }

    /// <summary>
    /// 复选框模板
    /// </summary>
    public class Template_CheckBox : Template_Control
    {
        /// <summary>
        /// 控件类型
        /// </summary>
        public new readonly TP_Tpye tp_Tpye = TP_Tpye.CheckBox;
    }
}
