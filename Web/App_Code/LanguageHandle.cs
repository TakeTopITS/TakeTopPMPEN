using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Resources;
using System.Web;

public static class LanguageHandle
{
    // 静态字典，存储所有语言的资源
    private static readonly Dictionary<string, Dictionary<string, string>> AllLanguageResources;

    // 静态构造器，加载所有语言资源文件
    static LanguageHandle()
    {
        // 复制语言文件，从App_GlobalResources目录到Language目录，以保证每次更新语言资源文件时，Language目录中的文件也会更新
        CopyLanguageFilesToLanguageDirctory();


        // 初始化全局资源字典
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
        try
        {
            HttpContext context = HttpContext.Current;
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }


            // 获取当前语言代码
            string langCode = context.Session["LangCode"] as string;
            if (string.IsNullOrEmpty(langCode))
            {
                langCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
                context.Session["LangCode"] = langCode;
            }


            // 从全局资源字典中获取对应语言的资源
            var resourceCache = new Dictionary<string, string>();
            string value;
            if (AllLanguageResources.TryGetValue(langCode, out resourceCache))
            {
                if (resourceCache.TryGetValue(key, out value))
                {
                    //LogClass.WriteLogFile(value + ":" + value);
                    return value.Trim();
                }
            }

            //如果找不到对应的资源键，则用默认语言的资源
            langCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
            // 从全局资源字典中获取对应语言的资源
            var resourceCacheDefault = new Dictionary<string, string>();
            string valueDefault;
            if (AllLanguageResources.TryGetValue(langCode, out resourceCacheDefault))
            {
                if (resourceCacheDefault.TryGetValue(key, out valueDefault))
                {
                    //LogClass.WriteLogFile(value + ":" + value);
                    return valueDefault.Trim();
                }
            }

            return ""; 
        }
        catch
        {
            return "";
        }
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

    //复制语言文件，从App_GlobalResources目录到Language目录，以保证每次更新语言资源文件时，Language目录中的文件也会更新
    public static void CopyLanguageFilesToLanguageDirctory()
    {
        try
        {
            string sourceDirectory = HttpContext.Current.Server.MapPath("App_GlobalResources");
            string targetDirectory = HttpContext.Current.Server.MapPath("Language");

            // 确保源目录存在
            if (!Directory.Exists(sourceDirectory))
            {
                //Console.WriteLine("源目录不存在！");
                return;
            }

            // 确保目标目录存在，如果不存在则创建
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            // 获取源目录下的所有扩展名为 .resx 的文件
            string[] files = Directory.GetFiles(sourceDirectory, "*.resx");

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(targetDirectory, fileName);

                // 复制文件并覆盖目标目录中的同名文件
                File.Copy(file, destFile, true);
            }
        }
        catch (Exception ex)
        {
            LogClass.WriteLogFile($"Resource file copy failed: {ex.Message}");
        }
    }
}