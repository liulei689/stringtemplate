using Flurl.Http;
using JinianNet.JNTemplate;
using StoneCodeGenerator.Lib.Model;
using System.Globalization;

namespace StoneCodeGenerator.Lib;
    public static class StoneCode
    {
    #region 生成方法
    /// <summary>
    /// 生成方法
    /// </summary>
    /// <param name="FunActionCn">方法中文作用</param>
    /// <returns></returns>
    public static async Task<string> GMethod(FunCode FunActionCn)
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

}
