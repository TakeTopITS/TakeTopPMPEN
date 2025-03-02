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
    // ��̬�ֵ䣬�洢�������Ե���Դ
    private static readonly Dictionary<string, Dictionary<string, string>> AllLanguageResources;

    // ��̬����������������������Դ�ļ�
    static LanguageHandle()
    {
        // ���������ļ�����App_GlobalResourcesĿ¼��LanguageĿ¼���Ա�֤ÿ�θ���������Դ�ļ�ʱ��LanguageĿ¼�е��ļ�Ҳ�����
        CopyLanguageFilesToLanguageDirctory();


        // ��ʼ��ȫ����Դ�ֵ�
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
        try
        {
            HttpContext context = HttpContext.Current;
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }


            // ��ȡ��ǰ���Դ���
            string langCode = context.Session["LangCode"] as string;
            if (string.IsNullOrEmpty(langCode))
            {
                langCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
                context.Session["LangCode"] = langCode;
            }


            // ��ȫ����Դ�ֵ��л�ȡ��Ӧ���Ե���Դ
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

            //����Ҳ�����Ӧ����Դ��������Ĭ�����Ե���Դ
            langCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
            // ��ȫ����Դ�ֵ��л�ȡ��Ӧ���Ե���Դ
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

    //���������ļ�����App_GlobalResourcesĿ¼��LanguageĿ¼���Ա�֤ÿ�θ���������Դ�ļ�ʱ��LanguageĿ¼�е��ļ�Ҳ�����
    public static void CopyLanguageFilesToLanguageDirctory()
    {
        try
        {
            string sourceDirectory = HttpContext.Current.Server.MapPath("App_GlobalResources");
            string targetDirectory = HttpContext.Current.Server.MapPath("Language");

            // ȷ��ԴĿ¼����
            if (!Directory.Exists(sourceDirectory))
            {
                //Console.WriteLine("ԴĿ¼�����ڣ�");
                return;
            }

            // ȷ��Ŀ��Ŀ¼���ڣ�����������򴴽�
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            // ��ȡԴĿ¼�µ�������չ��Ϊ .resx ���ļ�
            string[] files = Directory.GetFiles(sourceDirectory, "*.resx");

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(targetDirectory, fileName);

                // �����ļ�������Ŀ��Ŀ¼�е�ͬ���ļ�
                File.Copy(file, destFile, true);
            }
        }
        catch (Exception ex)
        {
            LogClass.WriteLogFile($"Resource file copy failed: {ex.Message}");
        }
    }
}