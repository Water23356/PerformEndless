using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformEndless
{
    /// <summary>
    /// 演出展示器
    /// </summary>
    public class PerformDisplayer
    {
        /// <summary>
        /// 所引用的剧本库
        /// </summary>
        public ScriptStore scripts;
        /// <summary>
        /// 所引用的演出数据库
        /// </summary>
        public DataStore datas;
        /// <summary>
        /// 所使用解析器
        /// </summary>
        private List<DataParser> parsers;
        /// <summary>
        /// 初始指令ID
        /// </summary>
        public int start_id;
        /// <summary>
        /// 当前指令ID
        /// </summary>
        public int ist_id;
        /// <summary>
        /// 当前指令对象
        /// </summary>
        private PerformInstruction ist;
        /// <summary>
        /// 当前演出是否处于激活状态
        /// </summary>
        public bool active = false;
        public Action<string> Output;
        public PerformDisplayer(ScriptStore scriptStore,DataStore dataStore,int start)
        {
            scripts = scriptStore;
            datas = dataStore;
            start_id = start;
            parsers=new List<DataParser>();
        }

        public void AddParser(DataParser dataParser)
        {
            parsers.Add(dataParser);
        }

        public void ClearParser()
        {
            parsers.Clear();
        }


        public void Start(int input)
        {
            ist_id = start_id;
            ist = scripts.Find(ist_id);
            active= true;
            Continue(input);
        }
        public void Continue(int input)
        {
            if (!active) { return; }
            int data_id = 0;
            ist_id = ist.Ist(out data_id,input);
            
            if (scripts.Find(ist_id)==null)//此次为最后的指令
            {
                active = false;
                PerformData pd = datas.Find(data_id);
                string type = "";
                Dictionary<string, string> body = null;
                
                if(pd != null)
                {
                    if (pd is DataPackage)
                    {
                        body = (pd as DataPackage).GetData(out type,ist.parameter);
                    }
                    else if(pd is DataCell)
                    {
                        body = (pd as DataCell).GetData(out type);
                    }

                    foreach (DataParser dps in parsers)
                    {
                        if(dps.Type == type)
                        {
                            dps.Parse(body, Continue);
                        }
                    }
                }
            }
            else//更新当前指令对象
            {
                PerformData pd = datas.Find(data_id);
                string type = "";
                Dictionary<string, string> body = null;

                if (pd == null)
                {
                    ist = scripts.Find(ist_id);
                    Continue(input);
                    return;
                }
                else
                {
                    if (pd is DataPackage)
                    {
                        body = (pd as DataPackage).GetData(out type,ist.parameter);
                        Console.WriteLine("PackageData!!!!");
                    }
                    else if (pd is DataCell)
                    {
                        body = (pd as DataCell).GetData(out type);
                    }

                    if(body == null || body.Keys.Count == 0)
                    {
                        ist = scripts.Find(ist_id);
                        Continue(input);
                        return;
                    }
                    foreach(string key in body.Keys)
                    {
                        Console.WriteLine(key + ":" + body[key]);
                    }




                    ist = scripts.Find(ist_id);
                    foreach (DataParser dps in parsers)
                    {
                        if (dps.Type == type)
                        {
                            dps.Parse(body,Continue);
                        }
                    }
                }
            }
        }
    }
}
