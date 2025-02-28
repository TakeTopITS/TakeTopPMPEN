using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Resources;
using System.Web;

public static class LanguageHandle
{
    // ��̬�ֵ䣬�洢�������Ե���Դ
    private static readonly Dictionary<string, Dictionary<string, string>> AllLanguageResources;

    // ��̬����������������������Դ�ļ�
    static LanguageHandle()
    {
        AllLanguageResources = new Dictionary<string, Dictionary<string, string>>();

        // �����ݿ��ȡ�������Դ���
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

    // ��ȡ��Դֵ
    public static string GetWord(string key)
    {
        HttpContext context = HttpContext.Current;
        if (context == null)
        {
            throw new InvalidOperationException("HttpContext is not available.");
        }

        //LogClass.WriteLogFile(key + ":" + key);

        // ��ȡ��ǰ���Դ���
        string langCode = context.Session["LangCode"] as string;
        if (string.IsNullOrEmpty(langCode))
        {
            langCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
            context.Session["LangCode"] = langCode;
        }

        //LogClass.WriteLogFile(langCode + ":" + langCode);

        // ��ȫ����Դ�ֵ��л�ȡ��Ӧ���Ե���Դ
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

        return null; // ����Ҳ�����Ӧ����Դ��������null
    }

    // �����ݿ��ȡ֧�ֵ���������
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