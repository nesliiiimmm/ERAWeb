using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYAK.Common.NetCore
{
    public interface IViewORM<ET,VT>
    {
        /// <summary>
        /// Verilen Entity'i Delegeleri bağlayacak şekilde View'a dönüştürür.
        /// </summary>
        /// <param name="entity">ORM içindeki View Sınıfı</param>
        /// <returns>VT tipinde View Döner</returns>
        VT ConvertToView(ET entity);
        /// <summary>
        /// Girilen BusinessEntity'e ait olan View'ı getirir
        /// </summary>
        /// <param name="beid">BusinessEntity ID</param>
        /// <returns></returns>
        VT SingleView(long beid);

        /// <summary>
        /// bu tipteki tüm kayıtları view'a çevirerek getirir.
        /// </summary>
        /// <returns></returns>
        List<VT> View();


        /// <summary>
        /// Verilen BEID ne bağlı olan View'ları getirir. Select(BEID,X,Y) methodu kullanılır X=Bu ORM'in kullandığı Entity'nin BEType'dır Y çekme işleminde kullanılacak OperationID dir
        /// </summary>
        /// <param name="beid">ilişkili olduğu BEID</param>
        /// <returns></returns>
        List<VT> View(long beid,int operationID);

        /// <summary>
        /// verilen ıd'e ilişkili business entity i getirir
        /// </summary>
        /// <param name="BEID"></param>
        /// <returns></returns>
        VT RelatedView(long beid,int operationID);




    }




}
