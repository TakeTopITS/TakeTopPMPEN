using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Resources;
using System.Web;

public static class LanguageHandle
{
    // 静态字典，存储所有语言的资源
    private static readonly Dictionary<string, Dictionary<string, string>> AllLanguageResources;

    // 静态构造器，加载所有语言资源文件
    static LanguageHandle()
    {
        AllLanguageResources = new Dictionary<string, Dictionary<string, string>>();

        // 从数据库获取所有语言代码
        var supportedLanguages = GetSupportedLanguagesFromDatabase();
        
        foreach (var langCode in supportedLanguages)
        {
            string langFile = HttpContext.Current.Server.MapPath($"Language/lang.{langCode}.resx");
            var resourceCache = new Dictionary<string, string>();

            using (var resxReader = new ResXResourceReader(langFile))
            {
                var dict = resxReader.GetEnumerator();

                while (dict.MoveNext())
                {
                    resourceCache[dict.Key.ToString()] = dict.Value?.ToString();
                }
            }

            AllLanguageResources[langCode] = resourceCache;
        }
    }

    // 获取资源值
    public static string GetWord(string key)
    {
        HttpContext context = HttpContext.Current;
        if (context == null)
        {
            throw new InvalidOperationException("HttpContext is not available.");
        }

        //LogClass.WriteLogFile(key + ":" + key);

        // 获取当前语言代码
        string langCode = context.Session["LangCode"] as string;
        if (string.IsNullOrEmpty(langCode))
        {
            langCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
            context.Session["LangCode"] = langCode;
        }

        //LogClass.WriteLogFile(langCode + ":" + langCode);

        // 从全局资源字典中获取对应语言的资源
        var resourceCache = new Dictionary<string, string>();
        string value;
        if (AllLanguageResources.TryGetValue(langCode, out resourceCache))
        {
            if (resourceCache.TryGetValue(key, out  value))
            {
                //LogClass.WriteLogFile(value + ":" + value);
                return value;
            }
        }

        return null; // 如果找不到对应的资源键，返回null
    }

    // 从数据库获取支持的语言类型
    private static List<string> GetSupportedLanguagesFromDatabase()
    {
        var supportedLanguages = new List<string>();

        string query = "SELECT LangCode FROM t_systemlanguage";
        DataSet ds = ShareClass.GetDataSetFromSql(query, "t_systemlanguage");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

        {
            supportedLanguages.Add(ds.Tables[0].Rows[i][0].ToString().Trim());
        }


        return supportedLanguages;
    }
}