using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYAK.Common.NetCore
{
   
    public delegate T GetEntityHandler<T>(object id);  

    public delegate Result<List<T>> SelectHandler<T>(string where,Dictionary<string,object> prm,SelectType selectType);

    public delegate List<T> SelectListHandler<T>(int id);
    
    //ViewDelegates
    /// <summary>
    /// Long ID alıp geriye View döndüren methodlarda kullanılır.
    /// SingleView -> Verilen ID nin kendisi getirileceğinde
    /// RelatedView -> Verilen ID nin ilişkili olduğu Kayıt getirileceğinde 
    /// </summary>
    /// <typeparam name="T"> Geriye döneceği tip.</typeparam>
    /// <param name="id">getireceği kayıt ile ilgili ID (ilişkili veya kendi id'si)</param>
    /// <returns></returns>
    public delegate T GetRelatedViewHandler<T>(long id,int operationID=0);
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    public delegate T GetViewHandler<T>(long id);



    /// <summary>
    /// Verilen Id nin ilişkili olduğu kayıt listesini getirir. 
    /// View methodu bağlanır.
    /// </summary>
    /// <typeparam name="T">Geriye dönecek listenin içindeki kayıtların Tipi</typeparam>
    /// <param name="id">Getirilen kayıtlarla ilişkili ID</param>
    /// <returns></returns>
    public delegate List<T> SelectRelatedViewListHandler<T>(long id,int operationID=0);


    public delegate Result<List<T>> SelectRelatedBusinessEntity<T>(long primaryId, int typeID, int operation);






    ///Common Entity ile ilgili olan delegeler
    public delegate T GetCommonEntityHandler<T>(int typeId, long beid);
    public delegate T GetCommonEntitiesHandler<T>(int typeId, long beid);

}
