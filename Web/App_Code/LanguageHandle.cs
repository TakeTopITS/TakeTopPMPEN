using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Resources;
using System.Web;
using System.Xml;


public static class LanguageHandle
{
    private static XmlDocument xmlLanguageDoc;

    static LanguageHandle()
    {
        // 可以在这里调用 CopyLanguageFilesToLanguageDirctory() 如果需要
        CopyLanguageFilesToLanguageDirctory();
    }

    //取得语言文件的关键词的值
    public static string GetWord(string strKeyword)
    {
        // 创建一个XmlDocument对象并加载XML文件  
        XmlDocument doc = GetLanguageDoc();

        // 使用XPath选择器定位到指定节点  
        XmlNode dataNode = doc.SelectSingleNode("/root/data[@name='" + strKeyword + "']/value");
        if (dataNode != null)
        {
            // 获取节点的值并返回给客户端  
            string value = dataNode.InnerText;
            return value;
        }
        else
        {
            // 节点不存在，则取默认语言文件的节点值
            string resxFile = HttpContext.Current.Server.MapPath($"Language/lang.resx");
            doc = new XmlDocument();
            doc.Load(resxFile);

            // 使用XPath选择器定位到指定节点  
            dataNode = doc.SelectSingleNode("/root/data[@name='" + strKeyword + "']/value");
            if (dataNode != null)
            {
                // 获取节点的值并返回给客户端  
                string value = dataNode.InnerText;
                return value;
            }
            else
            {
                return "";
            }
        }
    }

    //取得语言文件到缓存，不用每次重复读文档
    public static XmlDocument GetLanguageDoc()
    {
        string resxFile = "";

        string SystemLangCode = HttpContext.Current.Session["LangCode"].ToString();

        if (SystemLangCode == "" || string.IsNullOrEmpty(SystemLangCode))
        {
            SystemLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
        }

        if (SystemLangCode != null && SystemLangCode != "")
        {
            resxFile = HttpContext.Current.Server.MapPath($"Language/lang.{SystemLangCode}.resx"); 
            if (!File.Exists(resxFile))
            {
                resxFile = "Language/lang.resx";
            }
        }
        else
        {
            resxFile = "Language/lang.resx";
        }

        //LogClass.WriteLogFile(resxFile);

        // 创建一个XmlDocument对象并加载XML文件  
        XmlDocument doc = new XmlDocument();
        doc.Load(resxFile);

        return doc;
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

