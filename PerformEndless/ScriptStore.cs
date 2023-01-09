using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    /// <summary>
    /// 剧本库
    /// 指令对象的ID皆小于0
    /// </summary>
    public class ScriptStore
    {
        /// <summary>
        /// 包库
        /// </summary>
        public List<PerformScript> scripts = new List<PerformScript>();
        /// <summary>
        /// 剧本库
        /// </summary>
        public List<PerformInstruction> instructions = new List<PerformInstruction>();
        /// <summary>
        /// 消息输出委托
        /// </summary>
        public Action<string> Output;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path = "Null";
        /// <summary>
        /// 当前所在的包的ID
        /// </summary>
        public int script_id;

        /// <summary>
        /// 获取指令包
        /// </summary>
        /// <param name="id_script"></param>
        /// <returns></returns>
        public PerformScript FindPackage(int id_script)
        {
            foreach (PerformScript sp in scripts)
            {
                if (sp.ID == id_script) { return sp; }
            }
            return null;
        }
        /// <summary>
        /// 根据包ID获取所有相关的指令
        /// </summary>
        /// <param name="id_script"></param>
        /// <returns></returns>
        public PerformInstruction[] FindWithPackage(int id_script)
        {
            List<PerformInstruction> performInstructions = new List<PerformInstruction>();
            PerformScript sc = FindPackage(id_script);
            List<int> ids = sc.ists;
            foreach (PerformInstruction p in instructions)
            {
                foreach (int i in ids)
                {
                    if (p.ID == i) { performInstructions.Add(p); break; }
                }
            }
            return performInstructions.ToArray();
        }
        /// <summary>
        /// 通过ID获取指令对象的副本
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PerformInstruction Find(int id)
        {
            if (id < 0)
            {
                foreach (PerformInstruction p in instructions)
                {
                    if (p.ID == id)
                    {
                        return p.Copy();
                    }
                }
            }
            if (Output != null) { Output($"未查找到指定指令对象,ID所对应的指令不存在: {id}"); }
            return null;
        }
        /// <summary>
        /// 移除指定ID对应的指令
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            if (id < 0)
            {
                foreach (PerformInstruction p in instructions)
                {
                    if (p.ID == id)
                    {
                        instructions.Remove(p);
                        return;
                    }
                }
            }
            if (Output != null) { Output($"未查找到指定指令对象,ID所对应的指令不存在: {id}"); }
        }
        /// <summary>
        /// 创建一个新的指令对象
        /// </summary>
        /// <returns></returns>
        public PerformInstruction Creat()
        {

            Random random_seed = new Random();
            int seed = random_seed.Next(0, 9999);
            int times = 0;
            int id = -1;
            bool next = true;
            while (next)
            {
                id = new Random(id + times + seed).Next(0, 99999);
                if (id > 0) { id *= -1; }

                next = false;
                if (id == 0) { next = true; }
                else
                {
                    foreach (PerformInstruction u in instructions)
                    {
                        if (u.ID == id) { next = true; break; }
                    }
                }
            }

            PerformInstruction ist = new PerformInstruction(id, this)
            {
                Name = "新指令",
                script_Onwer = script_id
            };
            instructions.Add(ist);
            return ist;
        }
        /// <summary>
        /// 根据id创建新的指令对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private void Creat(string name, int id, int item, int next, int condizione, int parameter)
        {
            PerformInstruction ist = new PerformInstruction(id, this)
            {
                Name = name,
                Item = item,
                Next = next,
                Condizione = condizione,
                parameter = parameter
            };
            instructions.Add(ist);
        }

        /// <summary>
        /// 创建一个新的指令包
        /// </summary>
        public PerformScript CreatPackage()
        {
            Random random_seed = new Random();
            int seed = random_seed.Next(0, 9999);
            int times = 0;
            int id = 1;
            bool next = true;
            while (next)
            {
                id = new Random(id + times + seed).Next(0,99999);
                if (id < 0) { id *= -1; }

                next = false;
                if (id == 0) { next = true; }
                else
                {
                    foreach (PerformScript u in scripts)
                    {
                        if (u.ID == id) { next = true; break; }
                    }
                }
            }

            PerformScript scp = new PerformScript(id)
            {
                Name = "新指令包"
            };
            scripts.Add(scp);
            return scp;
        }
        /// <summary>
        /// 根据ID创建指令包
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="csv"></param>
        private void CreatPackage(string name, int id, string csv)
        {
            PerformScript ps = new PerformScript(id)
            {
                Name = name,
            };
            string[] cells = ToolMore.SplitCsv(csv);
            foreach (string s in cells)
            {
                ps.ists.Add(Convert.ToInt32(s));
            }
            scripts.Add(ps);
        }
        /// <summary>
        /// 更新包的归属
        /// </summary>
        private void UpdatePackage()
        {
            foreach (PerformScript s in scripts)
            {
                for (int i = 0; i < s.ists.Count; i++)
                {
                    PerformInstruction ist = Find(s.ists[i]);
                    if (ist != null) { ist.script_Onwer = s.ID; }
                }
            }
        }
        /// <summary>
        /// 保存数据至本地
        /// </summary>
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

            string datas = "";
            foreach (PerformScript ps in scripts)
            {
                datas += ps.GetCsv();
            }
            foreach (PerformInstruction ist in instructions)
            {
                datas += ist.GetCsv();
            }

            if (!File.Exists(Path)) { File.Create(Path).Close(); }//确认文件的存在
            File.WriteAllText(Path, datas);
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
                if (line == "") { break; }
                string[] cells = ToolMore.SplitCsv(line, 4);

                //Console.WriteLine(cells[0]+" "+
                //    cells[1]+ " " +
                //    cells[2]+ " " +
                //    cells[3]+ " " +
                //    cells[4]+ " " +
                //    cells[5]);
                if (cells[0] == "*")
                {
                    CreatPackage(cells[1],
                        Convert.ToInt32(cells[2]),
                        cells[3]);
                }
                else if (cells[0] == "%")
                {
                    Creat(cells[1],
                    Convert.ToInt32(cells[2]),
                    Convert.ToInt32(cells[3]),
                    Convert.ToInt32(cells[4]),
                    Convert.ToInt32(cells[5]),
                    Convert.ToInt32(cells[6]));
                }
            }
            UpdatePackage();
        }
    }
}
