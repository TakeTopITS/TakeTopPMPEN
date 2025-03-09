using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Resources;
using System.Web;
using System.Xml;


public static class LanguageHandle
{
    private static ConcurrentDictionary<string, XmlDocument> languageDocCache = new ConcurrentDictionary<string, XmlDocument>();

    static LanguageHandle()
    {
        // 复制语言文件（如果需要）
        CopyLanguageFilesToLanguageDirectory();
    }

    // 取得语言文件的关键词的值
    public static string GetWord(string strKeyword)
    {
        // 获取当前会话的语言代码
        string systemLangCode = HttpContext.Current.Session["LangCode"]?.ToString();

        if (string.IsNullOrEmpty(systemLangCode))
        {
            systemLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
        }

        // 获取对应的语言资源文件
        XmlDocument doc = GetLanguageDoc(systemLangCode);

        // 使用XPath选择器定位到指定节点  
        XmlNode dataNode = doc.SelectSingleNode("/root/data[@name='" + strKeyword + "']/value");
        if (dataNode != null)
        {
            // 获取节点的值并返回给客户端  
            return dataNode.InnerText;
        }
        else
        {
            // 节点不存在，则取默认语言文件的节点值
            string defaultLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
            doc = GetLanguageDoc(defaultLangCode);

            // 使用XPath选择器定位到指定节点  
            dataNode = doc.SelectSingleNode("/root/data[@name='" + strKeyword + "']/value");
            if (dataNode != null)
            {
                // 获取节点的值并返回给客户端  
                return dataNode.InnerText;
            }
            else
            {
                return ""; // 如果默认语言文件也没有找到，返回空字符串
            }
        }
    }

    // 从缓存中获取语言文件，如果缓存中没有则加载
    public static XmlDocument GetLanguageDoc(string langCode)
    {
        // 检查缓存中是否已经存在该语言的资源文件
        XmlDocument cachedDoc;
        if (languageDocCache.TryGetValue(langCode, out cachedDoc))
        {
            return cachedDoc; // 如果缓存中存在，直接返回
        }

        // 如果缓存中不存在，加载对应的语言文件
        string resxFile = HttpContext.Current.Server.MapPath($"Language/lang.{langCode}.resx");

        // 如果指定的语言文件不存在，则使用默认语言文件
        if (!File.Exists(resxFile))
        {
            string defaultLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
            resxFile = HttpContext.Current.Server.MapPath($"Language/lang.{defaultLangCode}.resx");

            // 如果默认语言文件也不存在，返回空的 XmlDocument
            if (!File.Exists(resxFile))
            {
                return new XmlDocument();
            }
        }

        // 创建一个XmlDocument对象并加载XML文件  
        XmlDocument doc = new XmlDocument();
        doc.Load(resxFile);

        // 将加载的资源文件缓存起来
        languageDocCache[langCode] = doc;

        return doc;
    }

    // 复制语言文件（如果需要）
    public static void CopyLanguageFilesToLanguageDirectory()
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

