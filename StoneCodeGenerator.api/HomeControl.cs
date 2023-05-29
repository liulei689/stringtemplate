using Furion.DatabaseAccessor;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Furion.RemoteRequest.Extensions;
using System.ComponentModel.DataAnnotations;

namespace StoneCodeGenerator.api
{
public class FurionAppService : IDynamicApiController
    {
  

        public  string GetUser([Range(1, int.MaxValue)] int userId)
        {
            return userId.ToString();
        }

        public async Task<string> GetRemote(string id)
        {
            var data = await $"https://furion.baiqian.ltd/data?id={id}".GetAsAsync<string>();
            return data;
        }
    }
}