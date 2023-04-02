using LiteDB;
using StoneCodeGenerator.Lib.Model;

namespace StoneCodeGenerator.Lib
{
    public class Litedb
    {
        public ConnectionString ConnectionString = new ConnectionString();
        public LiteDatabase _db = null;
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
        public Codess InsertMongoToDB(Codess ov)
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
                        return null;
                    }
                    col.Insert(ov._id, ov);
                }
            }
            catch (Exception e)
            {
                return null;

            }
            finally
            {
            }
            return ov;
        }
        public bool UseIsExist(Codess ov)
        {
            try
            {
                if (_db != null)
                {
                    var col = _db.GetCollection<Codess>(_Key);
                    if (col.Exists(o => o.Use == ov.Use))
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                return false;

            }
            finally
            {
            }
            return false;
        }
        public Codess InsertToDB(Codess ov,out int status)
        {
            status = -1;
            try
            {
                if (_db != null)
                {
                    var col = _db.GetCollection<Codess>(_Key);
                    var dd = col.FindAll();
                    ov._id=(dd.Count()+1).ToString();
                    //col.EnsureIndex(x => x._id, true);
                    if (col.Exists(o => o._id == ov._id))
                    {
                       ov.CreateTime= col.Find(o => o._id == ov._id).First().CreateTime;
                        bool ist = col.Update(ov);
                        if (ist)
                        {
                            status = 1;
                            return ov;
                        }
                        else return null;
                    }
                    col.Insert(ov._id, ov);
                }
            }
            catch (Exception e)
            {
                return null;

            }
            finally
            {
            }
            status = 2;
            return ov;
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
