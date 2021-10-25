using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AYAK.Common.NetCore
{/// <summary>
/// Clustered index oluşturarak verilen Key kolonuna göre kayıtları sıralı tutar. Key kolonu üzerinden arama yapılacağında hızlı bir tarama yapar.
/// </summary>
/// <typeparam name="T">Tutulacak olan nesne Tipi</typeparam>
/// <typeparam name="K">Key kolonunun Tipi</typeparam>
    public class AList<T, K> where K : IComparable
    {
        public List<T> List { get; set; }
        public List<K> ClusteredKeys { get; set; }
        public string Key { get; set; }
        public Type Type { get; set; }
        public PropertyInfo KeyProp { get; set; }
        public AList(string key)
        {
            Key = key;
            Type = typeof(T);
            KeyProp = Type.GetProperties().FirstOrDefault(x => x.Name.ToLower() == Key.ToLower());
            if (KeyProp == null)
            {
                throw new Exception("Key özelliği bulunamadı");
            }
            ClusteredKeys = new List<K>();
            List = new List<T>();
        }
        public void Add(T item)
        {
            K value = (K)KeyProp.GetValue(item);
            if (value != null)
            {
                int i = GetIndex(value);
                ClusteredKeys.Insert(i, value);
                List.Insert(i, item);
            }
        }

        public int GetIndex(K key)
        {
            K iv = (K)key;
            int count = ClusteredKeys.Count;
            if (count > 0)
            {
                if (count == 1)//Tek kayıt varsa 
                {
                    if (iv.CompareTo(ClusteredKeys[0]) > 0)
                    {
                        return 1;
                    }
                    else//tek kayıtta gelen değer küçükse
                    {
                        return 0;
                    }
                }
                else//listede birden fazla kayıt var ise
                {
                    int min = 0;
                    int max = count;
                    int cursor = (max - min) / 2;

                    while (max - min > 5)
                    {
                        K v = (K)ClusteredKeys[cursor];
                        if (v.CompareTo(iv) > 0)//(v>iv)
                        {
                            max = cursor;
                        }
                        else if (v.CompareTo(iv) < 0) //(v<iv)
                        {
                            min = cursor;
                        }
                        else
                        {
                            return cursor;
                        }
                        cursor = min + ((max - min) / 2);
                        if (cursor == max || cursor == min)
                        {
                            return cursor;
                        }
                    }


                    for (; min < max; min++)
                    {
                        if (((IComparable)ClusteredKeys[min]).CompareTo(iv) >= 0)
                            break; ;
                    }


                    return min;


                }

            }
            else//listede hiç kayıt yok.
            {
                return 0;
            }



        }

        public int GetIndex(T item)
        {
            K keyValue = (K)KeyProp.GetValue(item);
            return GetIndex(keyValue);
        }


        public List<T> this[K key]
        {
            get
            {
                List<T> result = new List<T>();
                if (!List.Any())
                    return result;

                int i = GetIndex(key);
                while (i>0&&ClusteredKeys[i - 1].CompareTo(key) == 0)
                {
                    i--;
                }
                while (i<ClusteredKeys.Count&&ClusteredKeys[i].CompareTo(key) == 0)
                {
                    result.Add(List[i]);
                    i++;
                }

                return result;
            }
        }

        public int Count
        {
            get
            {
                return ClusteredKeys.Count;
            }
        }

    }
}
