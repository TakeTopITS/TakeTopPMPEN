using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZObjectSmallImport : System.Web.UI.Page
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
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim()+"');</script>");
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
                lblMsg.Text = string.Format(LanguageHandle.GetWord("spanstylecolorredDaoRuShiChuXi").ToString().Trim(), ex.Message);
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
                string strDLCode = ShareClass.ObjectToString(row[LanguageHandle.GetWord("DaLeiDaiMa").ToString().Trim()]);
                string strZLCode = ShareClass.ObjectToString(row[LanguageHandle.GetWord("ZhongLeiDaiMa").ToString().Trim()]);
                string strXLCode = ShareClass.ObjectToString(row[LanguageHandle.GetWord("XiaoLeiDaiMa").ToString().Trim()]);
             
                string strObjectName = ShareClass.ObjectToString(row[LanguageHandle.GetWord("XiaoLeiMingChen").ToString().Trim()]);
                string strModel = ShareClass.ObjectToString(row[LanguageHandle.GetWord("XiaoLeiShuiMing").ToString().Trim()]);

                if (string.IsNullOrEmpty(strDLCode) && string.IsNullOrEmpty(strZLCode) && string.IsNullOrEmpty(strXLCode) && string.IsNullOrEmpty(strObjectName) && string.IsNullOrEmpty(strModel))
                {
                    break;
                }

                if (string.IsNullOrEmpty(strDLCode))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangDaLeiDaiMaBuNengWeiKong").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strZLCode))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangZhongLeiDaiMaBuNengWeiK").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strXLCode))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangXiaoLeiDaiMaBuNengWeiKo").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strObjectName))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangXiaoLeiMingChenBuNengWe").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strModel))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangXiaoLeiShuiMingBuNengWe").ToString().Trim(), lineNumber);
                    continue;
                }
                
            }
            catch (Exception ex)
            {
                lblMsg.Text = string.Format(LanguageHandle.GetWord("spanstylecolorredDaoRuShiChuXi").ToString().Trim(), ex.Message);
            }

        }
        if (!string.IsNullOrEmpty(resultMsg)) return false;
        return true;
    }


    private bool Import(DataTable dtExcel, ref string resultMsg)
    {
        //先清空代码对照表
        //string strDeleteObjectCodeHQL = "truncate table T_WZMaterialXL";
        //ShareClass.RunSqlCommand(strDeleteObjectCodeHQL);

        WZMaterialXLBLL WZMaterialXLBLL = new WZMaterialXLBLL();

        int successCount = 0;
        int lineNumber = 0;

        foreach (DataRow row in dtExcel.Rows)
        {
            string strDLCode = string.Empty;
            string strZLCode = string.Empty;
            string strXLCode = string.Empty;
            string strXLName = string.Empty;
            string strXLRemark = string.Empty;
            string strIsmark = string.Empty;
            string strCreateProgress = string.Empty;
            string strCreater = string.Empty;
            string strCreateIsMark = string.Empty;
            
            lineNumber++;
            strDLCode = ShareClass.ObjectToString(row[LanguageHandle.GetWord("DaLeiDaiMa").ToString().Trim()]);
            strZLCode = ShareClass.ObjectToString(row[LanguageHandle.GetWord("ZhongLeiDaiMa").ToString().Trim()]);
            strXLCode = ShareClass.ObjectToString(row[LanguageHandle.GetWord("XiaoLeiDaiMa").ToString().Trim()]);
            strXLName = ShareClass.ObjectToString(row[LanguageHandle.GetWord("XiaoLeiMingChen").ToString().Trim()]);

            if (string.IsNullOrEmpty(strDLCode) && string.IsNullOrEmpty(strZLCode) && string.IsNullOrEmpty(strXLCode) && string.IsNullOrEmpty(strXLName))
            {
                break;
            }

            try
            {
                strXLRemark = ShareClass.ObjectToString(row[LanguageHandle.GetWord("XiaoLeiShuiMing").ToString().Trim()]);
                strIsmark = ShareClass.ObjectToString(row[LanguageHandle.GetWord("ShiYongBiaoJi").ToString().Trim()]);
                strCreateProgress = ShareClass.ObjectToString(row[LanguageHandle.GetWord("ChuangJianJinDu").ToString().Trim()]);
                strCreater = ShareClass.ObjectToString(row[LanguageHandle.GetWord("ChuangJianRen").ToString().Trim()]);

                strCreateIsMark = ShareClass.ObjectToString(row[LanguageHandle.GetWord("ChuangJianBiaoZhi").ToString().Trim()]);

                WZMaterialXL wZMaterialXL = new WZMaterialXL();

                wZMaterialXL.DLCode = strDLCode;
                wZMaterialXL.ZLCode = strZLCode;
                wZMaterialXL.XLCode = strXLCode;
                wZMaterialXL.XLName = strXLName;
                wZMaterialXL.XLDesc = strXLRemark;
                int intIsmark = 0;
                int.TryParse(strIsmark, out intIsmark);
                wZMaterialXL.IsMark = intIsmark;
                wZMaterialXL.CreateProgress = strCreateProgress;
                wZMaterialXL.Creater = strCreater;
                int intCreateTitle = 0;
                int.TryParse(strCreateIsMark, out intCreateTitle);
                wZMaterialXL.CreateTitle = intCreateTitle;

                WZMaterialXLBLL.AddWZMaterialXL(wZMaterialXL);

                successCount++;
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile(this.GetType().BaseType.Name + "："  + LanguageHandle.GetWord("ZZJGDRSBJC").ToString().Trim() + " : " + LanguageHandle.GetWord("HangHao").ToString().Trim() + ": " + (lineNumber + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa").ToString().Trim() + ": " + strZLCode + " : " + err.Message.ToString());
            }
        }

        if (successCount > 0)
        {
            if (successCount == dtExcel.Rows.Count)
            {
                resultMsg += string.Format(LanguageHandle.GetWord("brYiChengGongDaoRu0TiaoShuJu").ToString().Trim(), successCount);
            }
            else
            {
                resultMsg += string.Format(LanguageHandle.GetWord("brYiChengGongDaoRu0TiaoShuJuGo").ToString().Trim(), successCount, dtExcel.Rows.Count - successCount);
            }

            //重新加载列表
            //BindObjectData();

            return true;
        }
        else
        {
            resultMsg += string.Format(LanguageHandle.GetWord("brWeiDaoRuShuJuGongYou0TiaoShu").ToString().Trim(), dtExcel.Rows.Count - successCount);
        }

        return false;
    }

    protected void lbTemplate_Click(object sender, EventArgs e)
    {
        // 下载项目对应相应模板.
        try
        {
            string templatePath = Server.MapPath(LanguageHandle.GetWord("DocTemplatesDuiZhaoDaiMaxls").ToString().Trim());


            FileUtils.Download(templatePath, string.Format("{0}.xls", LanguageHandle.GetWord("DuiZhaoDaiMa").ToString().Trim()), Response, false);
        }
        catch (Exception ex)
        { }
    }




}