using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYAK.Common.NetCore
{
    public interface IORM<T>
    {
        Result<List<T>> Select();
        Result Insert(T entity);
        Result Update(T entity);
        Result Delete(T entity);
        T Select(object Id);
        Result<List<T>> Select(string where,Dictionary<string,object> p,SelectType st);

    }
   
}
