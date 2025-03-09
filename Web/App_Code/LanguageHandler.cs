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
        xmlLanguageDoc = GetLanguageDoc();
    }

    //取得语言文件的关键词的值
    public static string GetWord(string strKeyword)
    {
        // 创建一个XmlDocument对象并加载XML文件  
        XmlDocument doc = xmlLanguageDoc;


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
            // 节点不存在，返回错误信息  
            return "";
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

        // 创建一个XmlDocument对象并加载XML文件  
        XmlDocument doc = new XmlDocument();
        doc.Load(resxFile);

        return doc;
    }


}

