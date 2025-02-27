using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZSupplierTemplateFileEdit : System.Web.UI.Page
{
    public string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString().Trim() : "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string id = Request.QueryString["id"].ToString();
                HF_ID.Value = id;

                int intID = 0;
                int.TryParse(id, out intID);

                BindSupplierTemplateFileData(intID);
            }
        }
    }

    protected void BT_SupplierTemplateFile_Click(object sender, EventArgs e)
    {
        string strID = HF_ID.Value;
        if (!string.IsNullOrEmpty(strID))
        {
            try
            {
                string strSupplierTemplateFileDocument = FUP_SupplierTemplateFile.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strSupplierTemplateFileDocument))
                {
                    string strExtendName = System.IO.Path.GetExtension(strSupplierTemplateFileDocument);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strSupplierTemplateFileDocument);
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

                    FUP_SupplierTemplateFile.SaveAs(strDocSavePath + strFileName3);


                    string strUrl = DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                    LT_SupplierTemplateText.Text = "<a href=\"" + "Doc\\" + strUrl + "\" class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    WZSupplierTemplateFileBLL wZSupplierTemplateFileBLL = new WZSupplierTemplateFileBLL();
                    string strSupplierTemplateFileHQL = "from WZSupplierTemplateFile as wZSupplierTemplateFile where ID = " + strID;
                    IList listSupplierTemplateFile = wZSupplierTemplateFileBLL.GetAllWZSupplierTemplateFiles(strSupplierTemplateFileHQL);
                    if (listSupplierTemplateFile != null && listSupplierTemplateFile.Count > 0)
                    {
                        WZSupplierTemplateFile wZSupplierTemplateFile = (WZSupplierTemplateFile)listSupplierTemplateFile[0];
                        wZSupplierTemplateFile.TemplateFileName = Path.GetFileNameWithoutExtension(strFileName2);
                        wZSupplierTemplateFile.TemplateFileURL = strUrl;
                        wZSupplierTemplateFile.CreateTime = DateTime.Now;

                        wZSupplierTemplateFileBLL.UpdateWZSupplierTemplateFile(wZSupplierTemplateFile, wZSupplierTemplateFile.ID);
                    }


                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCGYSMBWJCG").ToString().Trim()+"')", true);


                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim()+"')", true);
                    return;
                }
            }
            catch (Exception ex) { }
        }
        else
        {
            try
            {
                string strSupplierTemplateFileDocument = FUP_SupplierTemplateFile.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strSupplierTemplateFileDocument))
                {
                    string strExtendName = System.IO.Path.GetExtension(strSupplierTemplateFileDocument);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strSupplierTemplateFileDocument);
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

                    FUP_SupplierTemplateFile.SaveAs(strDocSavePath + strFileName3);

                    string strUrl = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    LT_SupplierTemplateText.Text = "<a href=\"" + strUrl + "\" class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_TemplateFileName.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_TemplateFileURL.Value = strUrl;



                    WZSupplierTemplateFileBLL wZSupplierTemplateFileBLL = new WZSupplierTemplateFileBLL();

                    WZSupplierTemplateFile wZSupplierTemplateFile = new WZSupplierTemplateFile();

                    wZSupplierTemplateFile.TemplateFileName = Path.GetFileNameWithoutExtension(strFileName2);
                    wZSupplierTemplateFile.TemplateFileURL = strUrl;
                    wZSupplierTemplateFile.CreateTime = DateTime.Now;

                    wZSupplierTemplateFileBLL.AddWZSupplierTemplateFile(wZSupplierTemplateFile);


                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCGYSMBWJCG").ToString().Trim()+"')", true);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim()+"')", true);
                    return;
                }
            }
            catch (Exception ex) { }
        }
    }




    private void BindSupplierTemplateFileData(int ID)
    {
        string strWZSupplierTemplateFileSql = string.Format(@"select * from T_WZSupplierTemplateFile
                        where ID = {0}", ID);
        DataTable dtSupplierTemplateFile = ShareClass.GetDataSetFromSql(strWZSupplierTemplateFileSql, "SupplierTemplateFile").Tables[0];
        if (dtSupplierTemplateFile != null && dtSupplierTemplateFile.Rows.Count > 0)
        {
            DataRow drSupplierTemplateFile = dtSupplierTemplateFile.Rows[0];

            string strTemplateFileName = ShareClass.ObjectToString(drSupplierTemplateFile["TemplateFileName"]);
            string strTemplateFileURL = ShareClass.ObjectToString(drSupplierTemplateFile["TemplateFileURL"]);

            LT_SupplierTemplateText.Text = "<a href=\"" + strTemplateFileURL + "\" class=\"notTab\" target=\"_blank\">" + strTemplateFileName + "</a>";

            HF_TemplateFileName.Value = strTemplateFileName;
            HF_TemplateFileURL.Value = strTemplateFileURL;
        }
    }


}