using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Resources;
using System.Web;

using TakeTopCore;


public static class LanguageHandle
{

    //// 获取语言资源值
    public static string GetWord(string key)
    {
        return TakeTopCore.LanguageHandle.GetWord(key);
    }

   
}
