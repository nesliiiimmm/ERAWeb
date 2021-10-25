using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AYAK.Guncelleyici
{

    public class Result
    {
        public object Data { get; set; }
        public string Message { get; set; }
        public ExceptionDetail ExDetail { get; set; }
        public ResultState State { get; set; }
        public bool DataReady
        {
            get
            {
                
                    return State == ResultState.Success && Data != null ;
               
                
            }
        }
    }

    public class Result<T>
    {
        public Result()
        {
            State = ResultState.Exception;
            Data = Activator.CreateInstance<T>();
        }
        public T Data { get; set; }
        public string Message { get; set; }
        public ExceptionDetail ExDetail { get; set; }
        public ResultState State { get; set; }
        public bool DataReady
        {
            get
            {
                if (Data is IEnumerable<object>)
                    return State == ResultState.Success && Data != null&&((IEnumerable<object>)Data).Any();
                else
                return State == ResultState.Success && Data != null;

            }
        }

        /// <summary>
        /// Data hariç alanları aktarır.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        public virtual Result<K>  Convert<K>()
        {
            Type t = this.GetType();
            Result<K> result = new Result<K>();
            //foreach (PropertyInfo item in t.GetProperties())
            //{
            //    if (item.Name=="Data")
            //    {
            //        continue;
            //    }

            //    if (item.GetGetMethod()!=null&&item.GetSetMethod()!=null)
            //    {
            //        item.SetValue(result, item.GetValue(this));
            //    }
            //}
            result.ExDetail = this.ExDetail;
            result.Message = this.Message;
            result.State = this.State;
            return result;

        }
        


    }

    public delegate K ConvertHandler<K, L>(L entity);
    public class PagedResult<T> : Result<T>,IPagedResult
    {
        
        public PagedResult()
        {
            ActivePage = 1;
            PageSize = 30;
            
        }
        public int ActivePage { get; set; }
        public int PageSize { get; set; }
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                {
                    return 0;
                }
                double count = (double)TotalRowCount / (double)PageSize;
                if (count > (int)count)
                {
                    count++;
                }
                return (int)count;
            }
           
        }
        public int TotalRowCount { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }

        /// <summary>
        /// Data Hariç diğer özellikleri aktarır
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        public PagedResult<K> PagedConvert<K>()
        {
            
            PagedResult<K> r = new PagedResult<K>();
            r.ActivePage = this.ActivePage;
            r.ExDetail = this.ExDetail;
            r.Link = this.Link;
            r.Message = this.Message;
            r.PageSize = this.PageSize;
            r.State = this.State;
            r.Title = this.Title;
            r.TotalRowCount = this.TotalRowCount;
            return r;
            
        }

    }

    public class ExceptionDetail
    {
        public DBExTypes ExType { get; set; }
        public string ColumnName { get; set; }
        public string TableName { get; set; }
    }

    public interface IPagedResult
    {
         int ActivePage { get; set; }
         int PageSize { get; set; }
         int PageCount { get;  }
         int TotalRowCount { get; set; }
         string Title { get; set; }
         string Link { get; set; }
    }


    public enum ResultState
    {
        Success=1,
        Exception=0,
        TimeOut=2
    }
    public enum DBExTypes
    {
        UniqueKey,
        ForeignKey,
        PrimaryKey,
        None
    }
}
