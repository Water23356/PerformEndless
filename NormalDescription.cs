using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    /// <summary>
    /// 常规描述
    /// </summary>
    public class NormalDescription
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 初始化一个标准的描述类
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="description">详细描述</param>
        /// <param name="id">ID编号</param>
        public NormalDescription(string name,string description,int id)
        {
            Name = name;
            Description = description;
            ID = id;
        }
        /// <summary>
        /// 自我描述
        /// </summary>
        /// <returns>描述文本</returns>
        public new string ToString()
        {
            return "[" + Name + "]" + Description + "<id = " + ID + ">";
        }
    }
}
