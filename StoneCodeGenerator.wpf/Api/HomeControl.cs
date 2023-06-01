using Furion.DatabaseAccessor;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Furion.RemoteRequest.Extensions;
using HandyControlDemo.UserControl;
using StoneCodeGenerator.Lib.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HandyControlDemo.api
{
public class FurionAppService : IDynamicApiController
    {
  

        public List<Codess> GetData()
        {
          return  Codes._code.SelectMongodb();
        }

        public async Task<string> GetRemote(string id)
        {
            var data = await $"https://furion.baiqian.ltd/data?id={id}".GetAsAsync<string>();
            return data;
        }
    }
}