using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Resources;
using System.Web;

public static class LanguageHandleBackup
{
    // ʹ�� Lazy<T> �ӳٳ�ʼ��ֻ���ֵ�
    private static readonly Lazy<Dictionary<string, Dictionary<string, string>>> AllLanguageResources =
        new Lazy<Dictionary<string, Dictionary<string, string>>>(LoadLanguageResources, isThreadSafe: true);

    // ����Ĭ�����Դ���
    private static readonly string DefaultLangCode = ConfigurationManager.AppSettings["DefaultLang"];

    // ��̬����������ѡ�������Ҫ��ʼ��������Դ��
    static LanguageHandleBackup()
    {
        // ������������� CopyLanguageFilesToLanguageDirctory() �����Ҫ
        CopyLanguageFilesToLanguageDirctory();
    }

    // ����������Դ
    private static Dictionary<string, Dictionary<string, string>> LoadLanguageResources()
    {
        var tempResources = new Dictionary<string, Dictionary<string, string>>();

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

            tempResources[langCode] = resourceCache;
        }

        return tempResources;
    }

    // ��ȡ��ǰ���Դ���
    private static string GetCurrentLangCode()
    {
        return HttpContext.Current.Session["LangCode"] as string ?? DefaultLangCode;
    }

    // ��ȡ��Դֵ
    public static string GetWord(string key)
    {
        try
        {
            string langCode = GetCurrentLangCode();

            var resourceCache = new Dictionary<string, string>();
            var value = "";

            // ��ȫ����Դ�ֵ��л�ȡ��Ӧ���Ե���Դ
            if (AllLanguageResources.Value.TryGetValue(langCode, out resourceCache))
            {
                if (resourceCache.TryGetValue(key, out value))
                {
                    return value.Trim();
                }
            }

            var resourceCacheDefault = new Dictionary<string, string>();
            var valueDefault = "";

            // ����Ҳ�����Ӧ����Դ��������Ĭ�����Ե���Դ
            if (AllLanguageResources.Value.TryGetValue(DefaultLangCode, out resourceCacheDefault))
            {
                if (resourceCacheDefault.TryGetValue(key, out valueDefault))
                {
                    return valueDefault.Trim();
                }
            }

            return ""; // ����Ҳ�����Դ�������ؿ��ַ���
        }
        catch
        {
            return ""; // �����쳣ʱ���ؿ��ַ���
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

    // ���������ļ��������Ҫ��
    public static void CopyLanguageFilesToLanguageDirctory()
    {
        try
        {
            string sourceDirectory = HttpContext.Current.Server.MapPath("App_GlobalResources");
            string targetDirectory = HttpContext.Current.Server.MapPath("Language");

            // ȷ��ԴĿ¼����
            if (!Directory.Exists(sourceDirectory))
            {
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
            // ��¼��־�����쳣
        }
    }
}