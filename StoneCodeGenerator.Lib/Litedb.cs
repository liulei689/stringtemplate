using LiteDB;
using StoneCodeGenerator.Lib.Model;

namespace StoneCodeGenerator.Lib
{
    public class Litedb
    {
        public ConnectionString ConnectionString = new ConnectionString();
        private LiteDatabase _db = null;
        public Litedb()
        {
            string path=  "代码库.db";
            ConnectionString.Connection = ConnectionType.Shared;
            ConnectionString.Filename = @"代码库.db";
            ConnectionString.ReadOnly = false;
            Connect();
        }
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private async Task<LiteDatabase> AsyncConnect(ConnectionString connectionString)
        {
            return await Task.Run(() =>
            {
                return new LiteDatabase(connectionString);
            });
        }
        public void Connect()
        {
            try
            {
                if (_db == null)
                {
                    _db = new LiteDatabase(ConnectionString);
                }
            }
            catch
            {
                _db?.Dispose();

                return;
            }
            finally
            {

            }
        }
        #region 同步表操作
        public string _Key = "代码库";
        public List<Codess> Selects()
        {
            if (_db != null)
            {
                // 获取文档
                var col = _db.GetCollection<Codess>(_Key).Query().ToList();
                return col;
            }
            return null;
        }
        public List<Codess> Selectby_id(string by_id)
        {
            if (_db != null)
            {
                // 获取文档
                return _db.GetCollection<Codess>(_Key).Query().ToList().FindAll(x => x._id == by_id);

            }
            return null;
        }
        public void CreatDB(List<Codess> ov)
        {
            try
            {
                if (_db != null)
                {
                    var col = _db.GetCollection<Codess>(_Key);
                    col.DeleteAll();
                    // 获取文档     
                    col.Insert(ov);
                }
            }
            catch
            {
            }
            finally
            {
            }
        }
        public int InsertListToDB(List<Codess> ov)
        {
            try
            {
                if (_db != null)
                {
                    var col = _db.GetCollection<Codess>(_Key);
                    //col.EnsureIndex(x => x.CodessId, true);
                    return col.Insert(ov);
                }
            }
            catch
            {
            }
            finally
            {
            }
            return -1;
        }
        public int InsertToDB(Codess ov)
        {
            try
            {
                if (_db != null)
                {
                    var col = _db.GetCollection<Codess>(_Key);
                    var dd = col.FindAll();
                    //col.EnsureIndex(x => x._id, true);
                    if (col.Exists(o => o._id == ov._id))
                    {
                        bool ist = col.Update(ov);
                        if (ist)
                            return 1;
                        else return -1;
                    }
                    ov.Use =(dd.Count()+1).ToString()+"."+ ov.Use;
                    ov._id = ov.Use;
                    col.Insert(ov._id, ov);
                }
            }
            catch (Exception e)
            {
                return -1;

            }
            finally
            {
            }
            return 2;
        }

        public bool DeleteOne(Codess ov)
        {
            try
            {
                if (_db != null)
                {
                    var col = _db.GetCollection<Codess>(_Key);
                    return col.Delete(ov._id);
                }
            }
            catch
            {
            }
            finally
            {
            }
            return false;
        }    
        public bool UpdateOneToDB(Codess ov)
        {
            try
            {
                if (_db != null)
                {
                    var col = _db.GetCollection<Codess>(_Key);
                    return col.Update(ov);
                }
            }
            catch
            {
            }
            finally
            {
            }
            return false;
        }
        #endregion
    }
}
