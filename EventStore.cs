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
        /// 事件库
        /// </summary>
        public class EventStore
        {
            #region 属性
            /// <summary>
            /// 事件库所在的目录
            /// </summary>
            public string? Path { get; set; }
            #endregion 属性

            #region 管理属性
            /// <summary>
            /// 事件文件引用组
            /// </summary>
            public SimpleList<FileInfo> Files { get; private set; }
            /// <summary>
            /// 与此事件库关联的管理器
            /// </summary>
            public PerformManager? Manager { get; set; }
            #endregion 管理属性

            #region 构造函数
            /// <summary>
            /// 初始化一个标准的事件库
            /// </summary>
            /// <param name="path">事件库所在的文件夹路径</param>
            public EventStore(string path)
            {
                Files = new SimpleList<FileInfo>();
                Path = path;
                UpdataFromFile();
            }
            #endregion 构造函数

            #region 功能
            /// <summary>
            /// 通过ID检索指定事件
            /// </summary>
            /// <param name="id">事件ID</param>
            /// <returns>返回加载后的事件对象</returns>
            public PerformEvent? FindID(int id)
            {
                for(int i=0;i<Files.Count;i++)
                {
                    int aId = Convert.ToInt32(Files[i].Name.Split("_")[0]);
                    if(id == aId)
                    {
                        return PerformEventDataSL.ToObjectByJsonFile(Files[i].FullName, this);
                    }
                }
                return null;
            }
            /// <summary>
            /// 更新目录（从文件到对象更新）
            /// </summary>
            public void UpdataFromFile()
            {
                if(Path != null)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Path);
                    if(directoryInfo.Exists)
                    {
                        Files.RemoveAll();//清除旧对象
                        FileInfo[] files = directoryInfo.GetFiles();
                        for(int i=0;i<files.Length;i++)
                        {
                            if (files[i].Extension == ".ejson")//检查后缀名是否正确
                            {
                                Files.Add(files[i]);//将正确格式文件加入列表中
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// 向事件库提交新的事件对象并保存，如果ID存在冲突则以旧文件为主
            /// </summary>
            /// <param name="performEvent">欲保存的 PerformEvent 对象</param>
            public void AddSave(PerformEvent performEvent)
            {
                for(int i=0;i<Files.Count;i++)
                {
                    int id = Convert.ToInt32(Files[i].Name.Split("_")[0]);
                    if(performEvent.ID() == id)//如果存在ID冲突
                    {
                        return;
                    }
                }
                PerformEventDataSL.ToJsonTextFileAuto(performEvent,Path);
                UpdataFromFile();
            }
            /// <summary>
            /// 向事件库提交新的事件对象并保存，如果ID存在冲突则以新文件覆盖
            /// <param name="performEvent">欲保存的 PerformEvent 对象</param>
            /// </summary>
            public void ChangeSave(PerformEvent performEvent)
            {
                PerformEventDataSL.ToJsonTextFileAuto(performEvent, Path);
                UpdataFromFile();
            }
            /// <summary>
            /// 从事件库中删除指定事件，并删除其源文件
            /// <param name="id">欲删除的事件的ID</param>
            /// </summary>
            public void DeleteSave(int id)
            {
                for(int i=0;i<Files.Count;i++)
                {
                    int aId = Convert.ToInt32(Files[i].Name.Split("_")[0]);
                    if (id == aId)
                    {
                        Files[i].Delete();
                    }
                }
            }
            /// <summary>
            /// 终止事件（通知管理器将事件移除缓存区）
            /// </summary>
            public void EndEvent(PerformEvent performEvent)
            {
                if (Manager == null) { return; }
                Manager.UnLoadEvent(performEvent);
            }
            /// <summary>
            /// 向控制台输出自身的描述
            /// </summary>
            public void WriteSelf()
            {
                Console.WriteLine("[事件库]<事件数量 = "+Files.Count+">");
                Console.WriteLine("[已登录的事件ID]{");
                for(int i=0;i<Files.Count;i++)
                {
                    string name = Files[i].Name;
                    Console.Write("("+name.Substring(0,name.Length-6)+")");
                }
                Console.WriteLine("}");
            }
            #endregion 功能

        }
    }
}
