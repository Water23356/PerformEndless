using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace PerformEndless
{
    /// <summary>
    /// 演出数据包
    /// </summary>
    public abstract class PerformData : PerformObject
    {
        /// <summary>
        /// 所属演出数据库对象
        /// </summary>
        protected DataStore owner;
        /// <summary>
        /// 规定标签
        /// </summary>
        public new string tag = "Data";
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="status">指令参数</param>
        /// <returns></returns>
        public abstract Dictionary<string, string> GetData(out string type,int status = 0);
    }

    /// <summary>
    /// 演出数据包
    /// </summary>
    public class DataPackage : PerformData
    {
        #region 属性
        /// <summary>
        /// 数据单元
        /// </summary>
        public List<int> cells = new List<int>();
        #endregion

        public DataPackage(int id,DataStore ownerp)
        {
            ID = id;
            owner = ownerp;
        }
        /// <summary>
        /// 获取演出数据
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public override Dictionary<string, string> GetData(out string type,int status = 0)
        {
            DataCell cell = null;
            type = "";
            if (status >= 0 && status < cells.Count)
            {
                int cell_id = cells[status];
                cell = (DataCell)owner.Find(cell_id);
                if (cell == null) { return null; }
            }
            else
            {
                return null;
            }
            Dictionary<string, string> dct = cell.GetData(out type);
            return dct;
        }
        /// <summary>
        /// 获取一个没有修改的数据包
        /// </summary>
        /// <returns></returns>
        public DataPackage Copy()
        {
            List<int> cells_copy = new List<int>();
            foreach(int c in cells)
            {
                cells_copy.Add(c);
            }
            return new DataPackage(ID, owner) 
            { 
                Name = Name,
                cells = cells_copy
            };
        }

        /// <summary>
        /// 获取此对象的csv数据字符串
        /// </summary>
        /// <returns></returns>
        public string GetCsv()
        {
            string csv = "";
            csv += "$,";
            csv += ToolMore.ToCsv(Name) + ",";
            csv += ToolMore.ToCsv(ID + "") + ",";
            string item = "";
            bool start = false;
            foreach(int c in cells)
            {
                if (!start) { start = true; }
                else
                {
                    item += ",";
                }
                item += c;
            }
            csv += ToolMore.ToCsv(item) + "\n";
            return csv;
        }
    }

    /// <summary>
    /// 演出数据
    /// </summary>
    public class DataCell : PerformData
    {
        #region 属性
        /// <summary>
        /// 数据类型
        /// </summary>
        public string type;
        /// <summary>
        /// 数据内容(json)
        /// </summary>
        public Dictionary<string,string> body = new Dictionary<string, string>();
        #endregion

        public DataCell(int id,DataStore ownerp,string typep)
        {
            ID = id;
            owner = ownerp;
            type = typep;
        }
        /// <summary>
        /// 获取演出数据
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public override Dictionary<string, string> GetData(out string type,int status = 0)
        {
            type = this.type;
            return body;
        }
        public DataCell Copy()
        {
            return new DataCell(ID, owner,type) { body=body,Name=Name};
        }

        /// <summary>
        /// 获取此对象的csv数据字符串
        /// </summary>
        /// <returns></returns>
        public string GetCsv()
        {
            string csv = "";
            csv += "#,";
            csv += ToolMore.ToCsv(type) + ",";
            csv += ToolMore.ToCsv(Name) + ",";
            csv += ToolMore.ToCsv(ID + "") + ",";
            string item = "";
            bool start = false;
            foreach(string key in body.Keys)
            {
                if (!start) { start = true; }
                else { item += ","; }
                item += key +":"+body[key];
            }
            csv += ToolMore.ToCsv(item) + "\n";
            return csv;
        }
    }
}
