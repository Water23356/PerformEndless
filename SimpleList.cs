using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PerformEndless
{
    /// <summary>
    /// 一个简单链表，用于存储管理 同类型的 可示例化的 对象；
    /// 链表不直接管理存储对象，只管理所包含的链结；
    /// 链结相互连接，且储存着具体对象的引用；
    /// </summary>
    /// <typeparam name="T">所管理元素的类型</typeparam>
    public class SimpleList<T> where T : class
    {
        #region 链表属性
        /// <summary>
        /// 头链结，表头，不用于存储对象
        /// </summary>
        private SimpleListNode headNode;
        /// <summary>
        /// 尾链结，存在主要便利查询
        /// </summary>
        private SimpleListNode tailNode;
        /// <summary>
        /// 链表长度（链表包含节点的个数）
        /// </summary>
        private int length;
        /// <summary>
        /// 链表所包含元素的个数
        /// </summary>
        public int Count { get { return length-1; } }
        /// <summary>
        /// 链表的名字
        /// </summary>
        public String Name { get; set; }
        #endregion 链表属性

        #region 构造函数
        /// <summary>
        /// 初始化一个默认的空链表
        /// </summary>
        public SimpleList() 
        {
            Name = "SimpleList";
            headNode = new SimpleListNode();
            tailNode = headNode;
        }
        /// <summary>
        ///  初始化一个默认的空链表，并预设链表名称
        /// </summary>
        /// <param name="name">链表名称</param>
        public SimpleList(String name)
        {
            Name = name;
            headNode = new SimpleListNode();
            tailNode = headNode;
        }
        /// <summary>
        /// 通过数组的形式创建一个链表，链表里的元素为源数组的浅拷贝
        /// </summary>
        /// <param name="array">源数组</param>
        public SimpleList(T[]? array)
        {
            Name = "SimpleList";
            headNode = new SimpleListNode();
            tailNode = headNode;
            if (array != null)
            {
                foreach (var item in array)
                {
                    Add(item);
                }
            }
        }
        #endregion 构造函数

        #region 内部函数
        /// <summary>
        /// 更新链表属性，在链表长度发生变化时调用
        /// </summary>
        private void Update()
        {
            length = 0;
            tailNode = headNode;
            while(true)
            {
                if(tailNode != null)
                {
                    length++;
                    if(tailNode.nextNode != null)
                    {
                        tailNode = tailNode.nextNode;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 通过索引值获取指定链结（头链结对应的索引值为-1）（不会返回头链结，因为头链结不存储元素，且不能被删除）
        /// </summary>
        /// <param name="index">索引值</param>
        /// <returns></returns>
        private SimpleListNode? Node(int index)
        {
            if (index < 0 || index > Count-1) { return null; }
            SimpleListNode? node = headNode;//当前选取的链结
            for(int i=-1;i<index;i++)
            {
                if(node != null)
                {
                    node = node.nextNode;
                }
                else
                {
                    break;
                }
            }
            return node;
        }

        #endregion 内部函数

        #region 功能函数
        /// <summary>
        /// 向链表末尾添加一个新元素
        /// </summary>
        /// <param name="item">添加的元素</param>
        public void Add(T item)
        {
            new SimpleListNode(item, tailNode);
            Update();
        }
        /// <summary>
        /// 向链表指定链结后面添加一个新元素；新元素添加进表后对应索引值为(index+1)；如果指定链结不存在则会返回false；
        /// </summary>
        /// <param name="item">添加的元素</param>
        /// <param name="index">链结索引值</param>
        /// <returns>执行是否成功</returns>
        public bool Add(T item,int index)
        {
            SimpleListNode? node = Node(index);
            if (node == null) { return false; }
            SimpleListNode? node2 = node.nextNode;
            if(node2 != null)//入口出口都存在
            {
                new SimpleListNode(item, node, node2);
            }
            else
            {
                new SimpleListNode(item, node);
            }
            Update();
            return true;
        }
        /// <summary>
        /// 移除链表中最后的元素，注意此函数不会自动调用 Update 更新链表属性
        /// </summary>
        /// <returns>是否执行成功</returns>
        private bool Remove()
        {
            if (tailNode == null || tailNode == headNode) { return false; }
            SimpleListNode? node = tailNode.lastNode;
            if (node == null) { return false; }
            tailNode.lastNode = null;
            node.nextNode = null;
            tailNode = node;
            return true;
        }
        /// <summary>
        /// 根据索引值，将指定元素移出链表；
        /// 如果成功移出该元素则返回true，否则返回false；
        /// </summary>
        /// <param name="index">索引值</param>
        /// <returns>执行是否成功</returns>
        public bool Remove(int index)
        {
            SimpleListNode? nodeFind = Node(index);//获取目标链结
            if (nodeFind == null) { return false; }
            SimpleListNode? node1 = nodeFind.lastNode;//入口
            SimpleListNode? node2 = nodeFind.nextNode;//出口

            nodeFind.lastNode = null;
            nodeFind.nextNode = null;

            if (node1 != null)
            {
                node1.nextNode = node2;
            }
            if(node2 != null)
            {
                node2.lastNode = node1;
            }
            Update();
            return true;
        }
        /// <summary>
        /// 查询目标元素在链表中的索引值
        /// </summary>
        /// <param name="item">查询元素</param>
        /// <returns>查询结果，如果元素不在链表中则返回-1</returns>
        public int FindIndex(T item)
        {
            SimpleListNode? nowNode = headNode;
            int index = -1;
            while(true)
            {
                if(nowNode.nextNode != null)
                {
                    nowNode = nowNode.nextNode;
                    index++;
                }
                else
                {
                    return -1;
                }
                if (nowNode.item == item)
                {
                    return index;
                }
            }
        }
        /// <summary>
        /// 移除指定元素，如果元素原本不存在链表中则返回false
        /// </summary>
        /// <param name="item">删除的元素</param>
        /// <returns>执行是否成功</returns>
        public bool Remove(T item)
        {
            SimpleListNode? nowNode = headNode;
            while (true)
            {
                if (nowNode.nextNode != null)
                {
                    nowNode = nowNode.nextNode;
                }
                else
                {
                    return false;
                }
                if (nowNode.item == item)
                {
                    SimpleListNode? node1 = nowNode.lastNode;
                    SimpleListNode? node2 = nowNode.nextNode;

                    nowNode.lastNode = null;
                    nowNode.nextNode = null;
                    if (node1 != null)
                    {
                        node1.nextNode = node2;
                    }
                    if(node2 != null)
                    {
                        node2.lastNode = node1;
                    }
                    Update();
                    return true;
                }
            }
        }
        /// <summary>
        /// 根据索引值，查询获取指定元素；
        /// 如果元素不存在，则返回null
        /// </summary>
        /// <param name="index">索引值</param>
        /// <returns>目标元素</returns>
        public T? Find(int index)
        {
            SimpleListNode? node = Node(index);
            if (node == null) { return null; }
            return node.item;
        }
        /// <summary>
        /// 根据索引值，查询获取指定元素（Find函数替代品）
        /// 如果元素不存在，则返回null
        /// </summary>
        /// <param name="index">索引值</param>
        /// <returns>目标元素</returns>
        public T? this[int index]
        {
            get
            {
                return Find(index);
            }
            set 
            {
                SimpleListNode? node = Node(index);
                if (node == null || value == null) { return; }
                node.item = value;
            }
        }
        /// <summary>
        /// 移除链表里的所有元素
        /// </summary>
        public void RemoveAll()
        {
            bool next = true;
            while(next)
            {
                next = Remove();
            }
            Update();
        }
        /// <summary>
        /// 通过浅拷贝将这个链表转化成一个数组，如果数组长度为0，则返回null
        /// </summary>
        /// <returns>结果数组</returns>
        public T[]? ToArray()
        {
            if (Count > 0) 
            {
                T[] array = new T[Count];
                for(int i = 0;i<Count;i++)
                {
                    T? item = this[i];
                    if(item != null) { array[i] = item; }
                }
                return array;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 筛选指定元素
        /// </summary>
        /// <param name="Where">筛选条件</param>
        /// <returns>返回符合条件的所有元素</returns>
        public T[]? Select(Func<T,bool> Where)
        {
            SimpleList<T> aimList = new SimpleList<T>();
            SimpleListNode? node = headNode.nextNode;
            while (node != null)
            {
                if(node.item != null )
                {
                    if(Where(node.item))
                    {
                        aimList.Add(node.item);
                    }
                }
                node = node.nextNode;
            }
            return aimList.ToArray();
        }
        #endregion 功能函数
        /// <summary>
        /// 这个链表的描述
        /// </summary>
        /// <returns>描述文本</returns>
        public new string ToString()
        {
            return "[链表]"+Name+"[链长]"+length+"[元素个数]"+Count;
        }

        /// <summary>
        /// 链结；用于连接其他链结；关联需要的对象
        /// </summary>
        private class SimpleListNode
        {
            #region 属性
            /// <summary>
            /// 该链结所存储的对象
            /// </summary>
            public T? item;
            /// <summary>
            /// 上一个链结（入口）
            /// </summary>
            public SimpleListNode? lastNode;
            /// <summary>
            /// 下一个链结（出口）
            /// </summary>
            public SimpleListNode? nextNode;

            #endregion 属性

            #region 构造函数

            /// <summary>
            /// 初始化一个空链结（未与具体对象建立连接）
            /// </summary>
            public SimpleListNode()
            {
                item = null;
            }
            /// <summary>
            /// 初始化一个链结，并与指定对象建立连接
            /// </summary>
            /// <param name="item">要求连接的对象（不可为null）</param>
            public SimpleListNode(T item)
            {
                this.item = item;
            }
            /// <summary>
            /// 初始化一个链结，并与指定对象建立连接，并设定这个连接的入口
            /// </summary>
            /// <param name="item">要求连接的对象</param>
            /// <param name="lastNode">入口节点</param>
            public SimpleListNode(T item, SimpleListNode lastNode)
            {
                this.item = item;
                this.lastNode = lastNode;
                lastNode.nextNode = this;
            }
            /// <summary>
            /// 初始化一个链结，并与指定对象建立连接，并设定这个连接的入口和出口
            /// </summary>
            /// <param name="item">要求连接的对象</param>
            /// <param name="lastNode">入口节点</param>
            /// <param name="nextNode">出口节点</param>
            public SimpleListNode(T item, SimpleListNode lastNode, SimpleListNode nextNode)
            {
                this.item = item;
                this.lastNode = lastNode;
                lastNode.nextNode = this;
                this.nextNode = nextNode;
                nextNode.lastNode = this;
            }

            #endregion 构造函数
            ~SimpleListNode()
            {
                item = null;
                lastNode = null;
                nextNode = null;
            }
        }
    }
    /// <summary>
    /// SimpleList 数据的专用读写类
    /// </summary>
    public static class SimpleListDataSL
    {
        /// <summary>
        /// 将一个指定 SimpleList<T> 转化为Json文本
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToJsonText<T>(SimpleList<T> list) where T : class
        {
            T[]? array = list.ToArray();
            if (array == null) { return ""; }
            return JsonConvert.SerializeObject(array);
        }
         
        /// <summary>
        /// 根据 Json 文本反序列化得到一个 SimpleList 对象
        /// </summary>
        /// <typeparam name="T">该链表所管理元素的类型</typeparam>
        /// <param name="jsonText">使用的 Json 文本</param>
        /// <returns>一个新 SimpleList 对象</returns>
        public static SimpleList<T>? ToSimpleListByJson<T>(string jsonText) where T : class
        {
            T[]? array = JsonConvert.DeserializeObject<T[]>(jsonText);
            if (array == null) { return null; }
            return new SimpleList<T>(array);
        }
    }

}
