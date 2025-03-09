using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Resources;
using System.Web;

public static class LanguageHandleBackup
{
    // 使用 Lazy<T> 延迟初始化只读字典
    private static readonly Lazy<Dictionary<string, Dictionary<string, string>>> AllLanguageResources =
        new Lazy<Dictionary<string, Dictionary<string, string>>>(LoadLanguageResources, isThreadSafe: true);

    // 缓存默认语言代码
    private static readonly string DefaultLangCode = ConfigurationManager.AppSettings["DefaultLang"];

    // 静态构造器（可选，如果需要初始化其他资源）
    static LanguageHandleBackup()
    {
        // 可以在这里调用 CopyLanguageFilesToLanguageDirctory() 如果需要
        CopyLanguageFilesToLanguageDirctory();
    }

    // 加载语言资源
    private static Dictionary<string, Dictionary<string, string>> LoadLanguageResources()
    {
        var tempResources = new Dictionary<string, Dictionary<string, string>>();

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

            tempResources[langCode] = resourceCache;
        }

        return tempResources;
    }

    // 获取当前语言代码
    private static string GetCurrentLangCode()
    {
        return HttpContext.Current.Session["LangCode"] as string ?? DefaultLangCode;
    }

    // 获取资源值
    public static string GetWord(string key)
    {
        try
        {
            string langCode = GetCurrentLangCode();

            var resourceCache = new Dictionary<string, string>();
            var value = "";

            // 从全局资源字典中获取对应语言的资源
            if (AllLanguageResources.Value.TryGetValue(langCode, out resourceCache))
            {
                if (resourceCache.TryGetValue(key, out value))
                {
                    return value.Trim();
                }
            }

            var resourceCacheDefault = new Dictionary<string, string>();
            var valueDefault = "";

            // 如果找不到对应的资源键，则用默认语言的资源
            if (AllLanguageResources.Value.TryGetValue(DefaultLangCode, out resourceCacheDefault))
            {
                if (resourceCacheDefault.TryGetValue(key, out valueDefault))
                {
                    return valueDefault.Trim();
                }
            }

            return ""; // 如果找不到资源键，返回空字符串
        }
        catch
        {
            return ""; // 发生异常时返回空字符串
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

    // 复制语言文件（如果需要）
    public static void CopyLanguageFilesToLanguageDirctory()
    {
        try
        {
            string sourceDirectory = HttpContext.Current.Server.MapPath("App_GlobalResources");
            string targetDirectory = HttpContext.Current.Server.MapPath("Language");

            // 确保源目录存在
            if (!Directory.Exists(sourceDirectory))
            {
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
            // 记录日志或处理异常
        }
    }
}