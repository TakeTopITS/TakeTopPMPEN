using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZObjectBigEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

         if (!IsPostBack)
        {
            DataTemplateFileBinder();
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
                string strDLName = ShareClass.ObjectToString(row[LanguageHandle.GetWord("DaLeiMingChen").ToString().Trim()]);
                string strDLDesc = ShareClass.ObjectToString(row[LanguageHandle.GetWord("DaLeiShuiMing").ToString().Trim()]);
                if (string.IsNullOrEmpty(strDLCode))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangDaLeiDaiMaBuNengWeiKong").ToString().Trim(), lineNumber);
                    continue;
                }
                if (!ShareClass.CheckStringRight(strDLCode))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangDaLeiDaiMaBuNengBaoHanF").ToString().Trim(), lineNumber);
                    continue;
                }
                if (strDLCode.Length != 2)
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangDaLeiDaiMaZhiNengWei2Ge").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strDLName))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangDaLeiMingChenBuNengWeiK").ToString().Trim(), lineNumber);
                    continue;
                }
                if (!ShareClass.CheckStringRight(strDLName))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangDaLeiMingChenBuNengBaoH").ToString().Trim(), lineNumber);
                    continue;
                }
                if (strDLCode.Length > 22)
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangDaLeiMingChenBuNengChao").ToString().Trim(), lineNumber);
                    continue;
                }
                if (strDLDesc.Length > 30)
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangDaLeiShuiMingBuNengChao").ToString().Trim(), lineNumber);
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
        //先清空大类表
        string strDeleteMaterialDLHQL = "truncate table T_WZMaterialDL";
        ShareClass.RunSqlCommand(strDeleteMaterialDLHQL);

        WZMaterialDLBLL wZMaterialDLBLL = new WZMaterialDLBLL();

        int successCount = 0;
        int lineNumber = 0;

        foreach (DataRow row in dtExcel.Rows)
        {
            string strDLCode = string.Empty;
            string strDLName = string.Empty;
            string strDLDesc = string.Empty;
            lineNumber++;
            strDLCode = ShareClass.ObjectToString(row[LanguageHandle.GetWord("DaLeiDaiMa").ToString().Trim()]);
            strDLName = ShareClass.ObjectToString(row[LanguageHandle.GetWord("DaLeiMingChen").ToString().Trim()]);
            strDLDesc = ShareClass.ObjectToString(row[LanguageHandle.GetWord("DaLeiShuiMing").ToString().Trim()]);

            WZMaterialDL wZMaterialDL = new WZMaterialDL();
            wZMaterialDL.DLCode = strDLCode;
            wZMaterialDL.DLName = strDLName;
            wZMaterialDL.DLDesc = strDLDesc;

            wZMaterialDLBLL.AddWZMaterialDL(wZMaterialDL);

            successCount++;
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
            string templatePath = Server.MapPath(LanguageHandle.GetWord("DocTemplatesDaLeiDaiMaxls").ToString().Trim());


            FileUtils.Download(templatePath, string.Format("{0}.xls", LanguageHandle.GetWord("DaLeiDaiMa").ToString().Trim()), Response, false);
        }
        catch (Exception ex)
        { }
    }


    protected void BT_Template_Click(object sender, EventArgs e)
    {
        try
        {
            string strTemplateDocument = FUP_Template.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
            if (!string.IsNullOrEmpty(strTemplateDocument))
            {
                string strExtendName = System.IO.Path.GetExtension(strTemplateDocument);//获取扩展名

                DateTime dtUploadNow = DateTime.Now; //获取系统时间
                string strFileName2 = System.IO.Path.GetFileName(strTemplateDocument);
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

                FUP_Template.SaveAs(strDocSavePath + strFileName3);


                string strUrl = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                string strName = Path.GetFileNameWithoutExtension(strFileName2);
                LT_Template.Text = "<a href=\"" + strUrl + "\" class=\"notTab\" target=\"_blank\">" + strName + "</a>";

                string strCheckTemplateFileHQL = string.Format("select * from T_WZTemplateFile where TemplateType = 'MajorCategory'");   //ChineseWord
                DataTable dtCheckTemplateFile = ShareClass.GetDataSetFromSql(strCheckTemplateFileHQL, "TemplateFile").Tables[0];
                if (dtCheckTemplateFile != null && dtCheckTemplateFile.Rows.Count > 0)
                {
                    //已经存在，替换
                    string strID = ShareClass.ObjectToString(dtCheckTemplateFile.Rows[0]["ID"]);

                    string strUpdateTemplateFileSQL = string.Format(@"update T_WZTemplateFile
                                    set TemplateName = '{0}',
                                    TemplateUrl = '{1}'
                                    where ID = {2}", strName,strUrl, strID);
                    ShareClass.RunSqlCommand(strUpdateTemplateFileSQL);
                }
                else {
                    //不存在，添加
                    string strInsertTemplateFileSQL = string.Format(@"insert into T_WZTemplateFile(TemplateType,TemplateName,TemplateUrl)
                                    values('{0}', '{1}','{2}')", "MajorCategory", strName, strUrl);   //ChineseWord
                    ShareClass.RunSqlCommand(strInsertTemplateFileSQL);
                }

                Response.Write(LanguageHandle.GetWord("scriptalertShangChuanChengGong").ToString().Trim());
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCBJWJCG").ToString().Trim()+"')", true);
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim()+"')", true);
                Response.Write("<script>alert('"+LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim()+"');</script>");
                return;
            }
        }
        catch (Exception ex) { }
    }



    private void DataTemplateFileBinder()
    {
        string strTemplateFileHQL = string.Format("select * from T_WZTemplateFile where TemplateType = 'MajorCategory'");   //ChineseWord
        DataTable dtTemplateFile = ShareClass.GetDataSetFromSql(strTemplateFileHQL, "TemplateFile").Tables[0];

        if (dtTemplateFile != null && dtTemplateFile.Rows.Count > 0)
        {
            string strTemplateName = ShareClass.ObjectToString(dtTemplateFile.Rows[0]["TemplateName"]);
            string strTemplateUrl = ShareClass.ObjectToString(dtTemplateFile.Rows[0]["TemplateUrl"]);

            LT_Template.Text = "<a href=\"" + strTemplateUrl + "\" class=\"notTab\" target=\"_blank\">" + strTemplateName + "</a>"; ;
        }
    }
}
