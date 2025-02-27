using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Text;
using System.IO;
using System.Web.Mail;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTUploadFileWithDialog : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strWLID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);


        strWLID = Request.QueryString["WLID"];
        if (strWLID == "null")
        {
            strWLID = "0";
        }

        if (Page.IsPostBack == false)
        {
            TB_Author.Text = strUserName;

            ShareClass.InitialUserDocTypeTree(TreeView3, strUserCode);
            ShareClass.LoadWorkflowActorGroupDropDownList(DL_Visible, strUserCode);
        }
    }

    protected void BtnUP_Click(object sender, EventArgs e)
    {
        if (AttachFile.HasFile)
        {
            string strFileName1, strExtendName;
            string strDocTypeID = LB_DocTypeID.Text;
            if (strDocTypeID == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGWDLXBNWKJC").ToString().Trim() + "');</script>");
                return;
            }

            string strDocType = GetDocTypeName(strDocTypeID);
            string strVisible = DL_Visible.SelectedValue.Trim();
            string strAuthor = TB_Author.Text.Trim();

            strFileName1 = this.AttachFile.FileName;//获取上传文件的文件名,包括后缀

            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";

            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "');</script>");
            }
            else
            {
                DocumentBLL documentBLL = new DocumentBLL();
                Document document = new Document();

                document.RelatedType = "Workflow";
                document.RelatedID = int.Parse(strWLID);
                document.DocType = strDocType;
                document.DocTypeID = int.Parse(strDocTypeID);
                document.Author = strAuthor;
                document.DocName = strFileName2;
                document.Address = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                document.UploadManCode = strUserCode;
                document.UploadManName = ShareClass.GetUserName(strUserCode);
                document.UploadTime = DateTime.Now;
                document.Visible = strVisible;
                document.DepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
                document.DepartName = ShareClass.GetDepartName(ShareClass.GetDepartCodeFromUserCode(strUserCode));
                document.Status = "InProgress";
                document.RelatedName = "";

                try
                {
                    documentBLL.AddDocument(document);

                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                    txtReturnValue.Value = strFileName2 + "|" + AttachFile.Size / 1024 + "|" + "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                    ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>toFather();</script>");
                }
                catch
                {

                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "');</script>");
        }
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDocTypeID = TreeView3.SelectedNode.Target;
        string strDocType = GetDocTypeName(strDocTypeID);

        LB_DocTypeID.Text = strDocTypeID;
        TB_DocType.Text = strDocType;
    }

    //绑定角色组，全体适用
    public static void LoadActorGroupDropDownList(DropDownList DL_Visible, string strUserCode)
    {
        string strHQL;

        string strDepartString, strDepartCode, strLangCode;

        strLangCode = HttpContext.Current.Session["LangCode"].ToString();

        strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = "Select GroupName,HomeName from T_ActorGroup where GroupName <> '全体' and  Type = 'All' "; 
        strHQL += " and (BelongDepartCode in (select ParentDepartCode from F_GetParentDepartCode(" + "'" + strDepartCode + "'" + "))";
        strHQL += " Or BelongDepartCode in " + strDepartString + ")";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";
        strHQL += " Order by SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ActorGroup");
        DL_Visible.DataSource = ds;
        DL_Visible.DataBind();
    }

    protected string GetDocTypeName(string strDocTypeID)
    {
        DocTypeBLL docTypeBLL = new DocTypeBLL();

        string strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        DocType docType = (DocType)lst[0];

        return docType.Type.Trim();
    }
}
