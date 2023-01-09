using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace PerformEndless
{
    public class DataStore
    {
        public List<PerformData> datas = new List<PerformData>();
        public string Path;
        public Action<string> Output;
        

        /// <summary>
        /// 根据ID获取一个数据体的副本
        /// </summary>
        /// <param name="id">数据体的ID</param>
        /// <returns></returns>
        public PerformData Find(int id)
        {
            if (id > 0)
            {
                foreach (PerformData d in datas)
                {
                    if (d.ID == id)
                    {
                        if(d is DataCell)
                        {
                            return (d as DataCell).Copy();
                        }
                        else if(d is DataPackage)
                        {
                            return (d as DataPackage).Copy();
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 创建一个新的演出数据对象
        /// </summary>
        /// <param name="type">数据类型,为空表示为一个数据包</param>
        /// <returns></returns>
        public PerformData Creat(string type = "")
        {
            Random random_seed = new Random();
            int seed = random_seed.Next(0, 9999);
            int times = 0;
            int id = 1;
            bool next = true;
            while (next)
            {
                id = new Random(id + times + seed).Next(0, 99999);
                if (id < 0) { id *= -1; }

                next = false;
                if (id == 0) { next = true; }
                else
                {
                    foreach (PerformData u in datas)
                    {
                        if (u.ID == id) { next = true; break; }
                    }
                }
            }

            PerformData pd = null;
            if(type == "")
            {
                pd = new DataPackage(id, this)
                {
                    Name = "Package" + datas.Count
                };
            }
            else
            {
                pd = new DataCell(id, this, type)
                {
                    Name = "PData" + datas.Count
                };
            }
            datas.Add(pd);
            return pd;
        }

        private void Creat(string[] txtLine)
        {
            if (txtLine[0] == "$")//创建包
            {
                DataPackage dp = new DataPackage(Convert.ToInt32(txtLine[2]), this) {Name = txtLine[1] };
                string[] ids = ToolMore.SplitCsv(txtLine[3]);
                foreach(string s in ids)
                {
                    dp.cells.Add(Convert.ToInt32(s));
                }
                datas.Add(dp);
            }
            else if(txtLine[0] == "#")//创建数据
            {
                DataCell dc = new DataCell(Convert.ToInt32(txtLine[3]), this, txtLine[1]) { Name = txtLine[2] };
                string[] keyValues = ToolMore.SplitCsv(txtLine[4]);
                foreach(string s in keyValues)
                {
                    string key = "";
                    string value = "";
                    bool kv = true;
                    foreach(char c in s)
                    {
                        if(kv)
                        {
                            if (c == ':') { kv = false; }
                            else { key += c; }
                        }
                        else
                        {
                            value += c;
                        }
                    }
                    dc.body.Add(key, value);
                }
                datas.Add(dc);
            }
        }

        public void Save()
        {
            if (Path == "Null")
            {
                if (Output != null)
                {
                    Output("未指定路径,无法保存数据!");
                }
                return;
            }

            string text = "";
            foreach (PerformData pd in datas)
            {
                if(pd is DataCell)
                {
                    text += (pd as DataCell).GetCsv();
                }
                else if(pd is DataPackage) 
                {
                    text += (pd as DataPackage).GetCsv();
                }
            }

            if (!File.Exists(Path)) { File.Create(Path).Close(); }//确认文件的存在
            File.WriteAllText(Path, text);
            if (Output != null)
            {
                Output($"数据写入完毕:{Path}");
            }
        }

        /// <summary>
        /// 从本地读取数据
        /// </summary>
        public void Load()
        {
            if (Path == "Null")
            {
                if (Output != null)
                {
                    Output("未指定路径,无法保存数据!");
                }
                return;
            }
            string datas = "";
            if (!File.Exists(Path)) //确认文件的存在
            {
                if (Output != null)
                {
                    Output($"路径不存在:{Path}");
                }
                return;
            }
            try
            {
                datas = File.ReadAllText(Path);
            }
            catch
            {
                if (Output != null)
                {
                    Output($"文件读取失败:{Path}");
                }
                return;
            }

            string[] txtLine = datas.Split('\n');
            foreach (string line in txtLine)
            {
                string[] cells = ToolMore.SplitCsv(line);
                Creat(cells);
            }
        }
    }
}
