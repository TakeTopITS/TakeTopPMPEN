using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZObjectMiddleImport : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            //BindObjectData();
        }
    }


    protected void btnImport_Click(object sender, EventArgs e)
    {
        string strObjectBigDocument = fileExcel.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
        if (!string.IsNullOrEmpty(strObjectBigDocument))
        {
            string strExtendName = System.IO.Path.GetExtension(strObjectBigDocument);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间
            string strFileName2 = System.IO.Path.GetFileName(strObjectBigDocument);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + Resources.lang.ZZCZTMWJSCSBGMHZSC + "');</script>");
            }

            if (Directory.Exists(strDocSavePath) == false)
            {
                //如果不存在就创建file文件夹{
                Directory.CreateDirectory(strDocSavePath);
            }

            string strAllUrl = strDocSavePath + strFileName3;
            fileExcel.SaveAs(strAllUrl);

            DataTable dtExcel = null;
            string resultMsg = string.Empty;
            try
            {
                dtExcel = ExcelUtils.ReadExcel(strAllUrl, "Sheet1").Tables[0];
                bool isSuccess = ValidateData(dtExcel, ref resultMsg);
                if (isSuccess)
                {
                    Import(dtExcel, ref resultMsg);
                }

                lblMsg.Text = string.Format("<span style='color:red' >{0}</span>", resultMsg);
            }
            catch (Exception ex)
            {
                lblMsg.Text = string.Format("<span style='color:red' >导入时出现以下错误: {0}!</span>", ex.Message);
            }
        }
    }


    /// <summary>
    /// 验证数据合法性.
    /// </summary>
    /// <param name="dtExcel"></param>
    /// <param name="resultMsg"></param>
    /// <returns></returns>
    private bool ValidateData(DataTable dtExcel, ref string resultMsg)
    {
        int lineNumber = 1;
        foreach (DataRow row in dtExcel.Rows)
        {
            lineNumber++;
            try
            {
                string strDLCode = ShareClass.ObjectToString(row["大类代码"]);
                string strZLCode = ShareClass.ObjectToString(row["中类代码"]);

                string strObjectName = ShareClass.ObjectToString(row["中类名称"]);
                string strModel = ShareClass.ObjectToString(row["中类说明"]);

                if (string.IsNullOrEmpty(strDLCode) && string.IsNullOrEmpty(strZLCode) && string.IsNullOrEmpty(strObjectName) && string.IsNullOrEmpty(strModel))
                {
                    break;
                }

                if (string.IsNullOrEmpty(strDLCode))
                {
                    resultMsg += string.Format("第{0}行，大类代码不能为空<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strZLCode))
                {
                    resultMsg += string.Format("第{0}行，中类代码不能为空<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strObjectName))
                {
                    resultMsg += string.Format("第{0}行，小类名称不能为空<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strModel))
                {
                    resultMsg += string.Format("第{0}行，小类说明不能为空<br/>", lineNumber);
                    continue;
                }

            }
            catch (Exception ex)
            {
                lblMsg.Text = string.Format("<span style='color:red' >导入时出现以下错误: {0}!</span>", ex.Message);
            }

        }
        if (!string.IsNullOrEmpty(resultMsg)) return false;
        return true;
    }


    private bool Import(DataTable dtExcel, ref string resultMsg)
    {
        //先清空代码对照表
        //string strDeleteObjectCodeHQL = "truncate table T_WZMaterialZL";
        //ShareClass.RunSqlCommand(strDeleteObjectCodeHQL);

        WZMaterialZLBLL WZMaterialZLBLL = new WZMaterialZLBLL();

        int successCount = 0;
        int lineNumber = 0;

        foreach (DataRow row in dtExcel.Rows)
        {
            string strDLCode = string.Empty;
            string strZLCode = string.Empty;
            string strZLName = string.Empty;
            string strZLDesc = string.Empty;
            string strIsmark = string.Empty;
            string strCreateProgress = string.Empty;
            string strCreater = string.Empty;
            string strCreateTitle = string.Empty;

            lineNumber++;

            try
            {
                strDLCode = ShareClass.ObjectToString(row["大类代码"]);
                strZLCode = ShareClass.ObjectToString(row["中类代码"]);
                strZLName = ShareClass.ObjectToString(row["中类名称"]);
                strZLDesc = ShareClass.ObjectToString(row["中类说明"]);

                if (string.IsNullOrEmpty(strDLCode) && string.IsNullOrEmpty(strZLCode) && string.IsNullOrEmpty(strZLName) && string.IsNullOrEmpty(strZLDesc))
                {
                    break;
                }


                strIsmark = ShareClass.ObjectToString(row["使用标记"]);
                strCreateProgress = ShareClass.ObjectToString(row["创建进度"]);
                strCreater = ShareClass.ObjectToString(row["创建人"]);

                strCreateTitle = ShareClass.ObjectToString(row["创建标志"]);

                WZMaterialZL wZMaterialZL = new WZMaterialZL();

                wZMaterialZL.DLCode = strDLCode;
                wZMaterialZL.ZLCode = strZLCode;
                wZMaterialZL.ZLName = strZLName;
                wZMaterialZL.ZLDesc = strZLDesc;
                int intIsmark = 0;
                int.TryParse(strIsmark, out intIsmark);
                wZMaterialZL.IsMark = intIsmark;
                wZMaterialZL.CreateProgress = strCreateProgress;
                wZMaterialZL.Creater = strCreater;
                int intCreateTitle = 0;
                int.TryParse(strCreateTitle, out intCreateTitle);
                wZMaterialZL.CreateTitle = intCreateTitle;

                WZMaterialZLBLL.AddWZMaterialZL(wZMaterialZL);
                successCount++;
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile(this.GetType().BaseType.Name + "："  + Resources.lang.ZZJGDRSBJC + " : " + Resources.lang.HangHao + ": " + (lineNumber + 2).ToString() + " , " + Resources.lang.DaiMa + ": " + strZLCode + " : " + err.Message.ToString());
            }
        }

        if (successCount > 0)
        {
            if (successCount == dtExcel.Rows.Count)
            {
                resultMsg += string.Format("<br/>已成功导入 {0} 条数据", successCount);
            }
            else
            {
                resultMsg += string.Format("<br/>已成功导入 {0} 条数据， 共有 {1} 条数据验证失败", successCount, dtExcel.Rows.Count - successCount);
            }

            //重新加载列表
            //BindObjectData();

            return true;
        }
        else
        {
            resultMsg += string.Format("<br/>未导入数据， 共有 {0} 条数据验证失败", dtExcel.Rows.Count - successCount);
        }

        return false;
    }

    protected void lbTemplate_Click(object sender, EventArgs e)
    {
        // 下载项目对应相应模板.
        try
        {
            string templatePath = Server.MapPath("Doc/Templates/对照代码.xls");


            FileUtils.Download(templatePath, string.Format("{0}.xls", "对照代码"), Response, false);
        }
        catch (Exception ex)
        { }
    }


}