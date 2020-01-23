using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DY.Site
{
    public class RandomController
    {
        #region Member　Variables
        //待随机抽取数据集合
        public　List<int>　datas　=　new　List<int>();
        //配比
        public　List<ushort>　weights　=　new　List<ushort>(); 
        #endregion

        #region Contructors 每次抽取个数
        ///　<summary>   　　　　
        ///　构造函数   　　　　
        ///　</summary>   　　　　
        ///　<param　name="count">随机抽取个数</param>   　　　　
        public RandomController(ushort count)
        {
            if (count > 8) throw new Exception("抽取个数不能超过数据集合大小！！");
            _Count = count;
        }
        #endregion     　　　　

        #region　Method 抽取随即数
        #region 普通随机抽取
        ///　<summary>   　　　　
        ///　随机抽取   　　　　
        ///　</summary>   　　　　
        ///　<param　name="rand">随机数生成器</param>   　　　　
        ///　<returns></returns>   　　　
        public int[] RandomExtract(Random rand)
        {
            List<int> result = new List<int>();
            if (rand != null)
            {
                for (int i = Count; i > 0; )
                {
                    int item = datas[rand.Next(8)];
                    if (result.Contains(item))
                        continue;
                    else
                    {
                        result.Add(item);
                        i--;
                    }
                }
            } return result.ToArray();
        }   　　　　
        #endregion     　　　　

        #region 受控随机抽取
        private static Random r = new Random();
        public static int keyRandom()
        {
            return r.Next(1, 100);
        }
        ///　<summary>   　　　　
        ///　随机抽取   　　　　
        ///　</summary>   　　　　
        ///　<param　name="rand">随机数生成器</param>   　　　　
        ///　<returns></returns>   　　　　　
        public int ControllerRandomExtract(Random rand)
        {
            int result = 0;
            if (rand != null)
            {
                //临时变量   　　　　　　　　　　　　
                Dictionary<int, int> dict = new Dictionary<int, int>(8);
                //为每个项算一个随机数并乘以相应的权值   　　　　　　　　　　　
                for (int i = datas.Count - 1; i >= 0; i--)
                {
                    dict.Add(datas[i], keyRandom() * weights[i]);
                }
                //排序   　　　
                List<KeyValuePair<int, int>> listDict = SortByValue(dict);
                //拷贝抽取权值最大的前Count项   　　　　　　　　　
                foreach (KeyValuePair<int, int> kvp in listDict.GetRange(0, Count))
                {
                    result = kvp.Key;
                }
            } return result;
        } 　　　　
        #endregion

        #region 排序集合
        ///　<summary>   　　　　
        ///　排序集合
        ///　</summary>   　　　　
        ///　<param　name="dict"></param>   　　　　
        ///　<returns></returns>   　　　　
        private List<KeyValuePair<int, int>> SortByValue(Dictionary<int, int> dict)
        {
            List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
            if (dict != null)
            {
                list.AddRange(dict);
                list.Sort(
                    delegate(KeyValuePair<int, int> kvp1, KeyValuePair<int, int> kvp2)
                    {
                        return kvp2.Value - kvp1.Value;
                    });
            } 
            return list;
        }  　
        #endregion     　　　
        #endregion   
  　　　
        #region Properties 随机抽取个数
        private　int　_Count;   　　　　
        ///　<summary>   　　　　
        ///　随机抽取个数
        ///　</summary>   　　　　
        public int Count
        {
            get
            {
                return _Count;
            }
            set
            {
                _Count = value;
            }
        }　　　　
        #endregion
    }
}