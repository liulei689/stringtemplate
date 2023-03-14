using Flurl.Http;
using JinianNet.JNTemplate;
using StoneCodeGenerator.Lib.Model;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection;

namespace StoneCodeGenerator.Lib;
    public static class StoneCode
    {
    #region 方法模板_默认_1 生成代码
    /// <summary>
    /// 方法模板_默认_1 生成代码
    /// </summary>
    /// <param name="FunActionCn">方法参数</param>
    /// <returns></returns>
    public static async Task<string> GMethod(FunCode_Defult_1 FunActionCn)
    {
        Engine.UseInterpretationEngine();
        Engine.Configure((c) =>
        {
            c.OutMode = OutMode.Auto;
        });
        var content = File.ReadAllText(FunActionCn.FTmpPath);
        string FunName= await ChangeChineseToKeyWordsAsync(FunActionCn.FUseFor);
        var template = Engine.CreateTemplate(content);
        template.Set("ZhushiS", FunActionCn.FCment);
        template.Set("Zhushi", FunActionCn.FSout);
        template.Set("FunName", FunName);
        var result = template.Render();
        return result;
    }
    #endregion
    #region 中文转关键词
    /// <summary>
    /// 中文转关键词
    /// </summary>
    /// <param name="ActionCn">方法中文作用</param>
    /// <returns></returns>
    public static async Task<string> ChangeChineseToKeyWordsAsync(string ActionCn)
    {
        return await Task.Run(() =>
        {
            try
            {
                string base_url_one = "https://yuanxiapi.cn/api/translation/?text=" + ActionCn;
                var reslutdata1 = base_url_one.GetJsonAsync().Result;
                string data = reslutdata1.result;
                data = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.ToLower());
                data = data.Replace(" ", "").Replace(".", "").Replace("-", "").Replace
                (",", "");
                return data;
            }
            catch { return "异常"; }
        });
    }
    #endregion
    #region 获取方法模板列表
    /// <summary>
    /// 获取方法模板列表
    /// </summary>
    public static string[] GetTheListOfMethodTemplates()=>Array.FindAll(Directory.GetFiles("模板"), ele => ele.Contains("方法") || ele.Contains("function"));
    #endregion
    #region 获取模板类型
    public static List<object> list;
    public static Dictionary<string, Dictionary<string, int>> dics;
    /// <summary>
    /// 获取模板类型
    /// </summary>
    public static void GetTemplateTypes()
    {
         list = new List<object>() 
        {
        new FunCode_Defult_1(),
        new FunCode_AsyncDefult_2()
        };
        dics = new Dictionary<string, Dictionary<string, int>>();
        for (int i=0;i<list.Count;i++)
        {
           string[] datas= list[i].GetDescriptionFromO().Split('_');
            string lei = datas[0];
            string usin = datas[1];
            if (!dics.ContainsKey(lei))
            {
                Dictionary<string, int> dic = new Dictionary<string, int>
                {
                    { usin, i }
                };
                dics.Add(lei, dic);
            }
            else
            {             
                dics[lei].Add(usin, i);
            }        
        }
    }
    public static List<string> GetTypeLists()
    {
        if (dics == null) GetTemplateTypes();
        if (dics != null)
            return dics.Keys.ToList();
        else return new List<string>();
    }
    public static Dictionary<string, int> GetTypeContentListsByType(string type)
    {
        if (dics == null) GetTemplateTypes();
        if (dics != null)
        {
            if (dics.TryGetValue(type, out Dictionary<string, int> dic))
            {
                return dic;
            }
        }
        return new Dictionary<string, int>();
    }
    #endregion
    #region 获取描述
    /// <summary>
    /// 获取描述
    /// </summary>
    public static string GetDescriptionFromO(this object o)
    {
    var data= o.GetType().GetCustomAttribute<DescriptionAttribute>();
        if (data != null) 
            return data.Description;
        else return "";
    }
    #endregion

}
