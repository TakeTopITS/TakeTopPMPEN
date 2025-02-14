using Newtonsoft.Json.Linq;

using System.IO;
using System.Text;
using System.Xml;
using System.Web;

public class LangHandler
{

    public static XmlDocument xmlLanguageDoc;
    public static JObject jAppSetingJsonObject;
    public static bool isFirstCopyModelXMLFile = false;

    static LangHandler()
    {
        xmlLanguageDoc = GetLanguageDoc();
        jAppSetingJsonObject = GetAppsettingsJsonObject();
    }

    //改变语言
    public static void ChangeSystemLanguage(string strLangCode)
    {
        string SystemLangCode = strLangCode;
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

    //取得appsettings.json中的节点值
    public static string getAppSettingJsonNodeValue(string strNodeName1, string strNodeName2)
    {
        // 获取"ConnectionStrings"下的"CatalogDBContext"值
        string connectionString;

        if (jAppSetingJsonObject == null)
        {
            connectionString = GetAppsettingsJsonObject()[strNodeName1][strNodeName2].ToString();
        }
        else
        {
            connectionString = jAppSetingJsonObject[strNodeName1][strNodeName2].ToString();
        }

        return connectionString;
    }


    //取得AppsetingJsonObject到缓存，不用重复读文档
    public static JObject GetAppsettingsJsonObject()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

        // 读取appsettings.json文件内容  
        string jsonContent = File.ReadAllText(filePath);

        // 解析JSON内容为JObject  
        JObject jsonObject = JObject.Parse(jsonContent);

        return jsonObject;
    }


    //取得语言文件到缓存，不用每次重复读文档
    public static XmlDocument GetLanguageDoc()
    {
        string resxFile;

        string SystemLangCode = HttpContext.Current.Session["LangCode"].ToString();

        if (SystemLangCode == "")
        {
            SystemLangCode = HttpContext.Current.Session["LangCode"].ToString();
        }

        if (SystemLangCode != null && SystemLangCode != "")
        {
            resxFile = "/Language/" + $"lang.{SystemLangCode}.resx";
            if (!File.Exists(resxFile))
            {
                resxFile = "Language/lang.default.resx";
            }
        }
        else
        {
            resxFile = "Language/lang.default.resx";
        }

        // 创建一个XmlDocument对象并加载XML文件  
        XmlDocument doc = new XmlDocument();
        doc.Load(resxFile);

        return doc;
    }


}

