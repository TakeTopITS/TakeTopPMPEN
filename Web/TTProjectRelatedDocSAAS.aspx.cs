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
using System.IO;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopCore;

public partial class TTProjectRelatedDocSAAS : System.Web.UI.Page
{
    string strProjectID, strProjectName;
    string strLangCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode, strUserName, strDepartCode;
        string strFromProjectID, strFromProjectPlanVerID;


        IList lst;

        strLangCode = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strUserName = GetUserName(strUserCode);
        strDepartCode = GetDepartCode(strUserCode);

        strProjectID = Request.QueryString["ProjectID"];
        LB_ProjectID.Text = strProjectID;

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        strProjectName = project.ProjectName.Trim();

        if (lst.Count > 0)
        {
            //this.Title = LanguageHandle.GetWord("Project").ToString().Trim() + strProjectID + " " + strProjectName + "������ĵ�";
        }

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandlerForSpecialPopWindow();", true);
        if (Page.IsPostBack == false)
        {
            ShareClass.InitialDocTypeTree(TreeView1, strUserCode, "Project", strProjectID, strProjectName);
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY").ToString().Trim();

            ShareClass.InitialUserDocTypeTree(TreeView3, strUserCode);

            LoadProjectActorGroupForDropDownList(DL_Visible, strProjectID);

            TB_Author.Text = strUserName;

            LoadRelatedDoc();

            strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + "'" + strProjectID + "'" + " and projectPlanVersion.Type = 'InUse'";
            ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
            lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);
            if (lst.Count > 0)
            {
                ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];
                strFromProjectID = projectPlanVersion.FromProjectID.ToString();
                strFromProjectPlanVerID = projectPlanVersion.FromProjectPlanVerID.ToString();

                TakeTopPlan.InitialProjectPlanTree(TreeView2, strFromProjectID, strFromProjectPlanVerID);

                LoadProjectPlanDoc(strFromProjectID, strFromProjectPlanVerID);

                LB_TemplateProjectID.Text = strFromProjectID;
                LB_TemplatePlanVerID.Text = strFromProjectPlanVerID;
            }
        }
    }

    protected void BT_LoadDoc_Click(object sender, EventArgs e)
    {
        LoadRelatedDoc();
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDocTypeID, strHQL, strUserCode, strDepartCode, strDocType;
        IList lst1, lst2;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = GetDepartCode(strUserCode);

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;
        strDocTypeID = treeNode.Target.Trim();

        DocumentBLL documentBLL = new DocumentBLL();

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        lst1 = docTypeBLL.GetAllDocTypes(strHQL);

        if (strDocTypeID != "0")
        {
            if (lst1.Count > 0)
            {
                DocType docType = (DocType)lst1[0];
                strDocType = docType.Type.Trim();

                LB_DocTypeID.Text = docType.ID.ToString();
                TB_DocType.Text = docType.Type.Trim();


                strHQL = "from Document as document where document.DocType = " + "'" + strDocType + "'";

                strHQL += "  and (((document.RelatedType = 'Project' and document.RelatedID = " + strProjectID + ")";
                strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or (document.Visible in ( '����','ȫ��')))"; 
                strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))))";

                strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
                strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + "))";

                strHQL += "or (document.RelatedType = '����' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))"; 
                strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
                strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
                strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + ")))";

                strHQL += "or (document.RelatedType = '����' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + "))"; 
                strHQL += " and ((document.Visible in ('����','����') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) "; 
                strHQL += " or (document.Visible = 'ȫ��' )))))"; 
                strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

                LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLX").ToString().Trim() + strDocType;

                //����ȱʡ���ļ�����
                ShareClass.SetDefaultDocType(strDocType, LB_DocTypeID, TB_DocType);
              
            }
        }
        else
        {
            LB_DocTypeID.Text = "";
            TB_DocType.Text = "";

            strHQL = "from Document as document where (((document.RelatedType = 'Project' and document.RelatedID = " + strProjectID + ")";
            strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( '����','ȫ��')))"; 
            strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))))";

            strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
            strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + "))";

            strHQL += "or (document.RelatedType = '����' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))"; 
            strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
            strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
            strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + ")))";

            strHQL += "or (document.RelatedType = '����' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + "))"; 
            strHQL += " and ((document.Visible in ('����','����') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) "; 
            strHQL += " or (document.Visible = 'ȫ��' )))))"; 
            strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY").ToString().Trim();
        }

        lst2 = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst2;
        DataGrid1.DataBind();

        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst2.Count.ToString();

        //�����ĵ����޹������������ɾ����ť
        ShareClass.HideDataGridDeleteButtonForDocUploadPage(DataGrid1);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text.Trim();
            string strUserName = GetUserName(strUserCode);
            string strDocID = e.Item.Cells[0].Text.Trim();
           string strDocName = e.Item.Cells[3].Text.Trim();
            string strUploadMan = e.Item.Cells[6].Text.Trim();
            string strDepartCode = GetDepartCode(strUserCode);

            if (e.CommandName == "Delete")
            {
                if (strUserName == strUploadMan)
                {
                    strHQL = "from Document as document where document.DocID = " + strDocID;
                    DocumentBLL documentBLL = new DocumentBLL();
                    lst = documentBLL.GetAllDocuments(strHQL);
                    Document document = (Document)lst[0];

                    document.Status = "Deleted";

                    documentBLL.UpdateDocument(document, int.Parse(strDocID));

                    //ɾ�������ĵ�
                    ShareClass.DeleteMoreDocByDataGrid(DataGrid1);

                    LoadRelatedDoc();
                    ShareClass.InitialDocTypeTree(TreeView1, strUserCode, "Project", strProjectID, strProjectName);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFFCZNBNSCBRSCDWJ").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDocTypeID = TreeView3.SelectedNode.Target;
        string strDocType = GetDocTypeName(strDocTypeID);

        LB_DocTypeID.Text = strDocTypeID;
        TB_DocType.Text = strDocType;
    }

    protected void BT_RefrehDocList_Click(object sender, EventArgs e)
    {

    }

    protected void BtnUP_Click(object sender, EventArgs e)
    {
        if (AttachFile.HasFile)
        {
            string strUserCode = LB_UserCode.Text.Trim();
            string strProjectID = LB_ProjectID.Text.Trim();
            string strAuthor = TB_Author.Text.Trim();
            string strDocTypeID = LB_DocTypeID.Text.Trim();

            if (strDocTypeID == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGWDLXBNWKJC").ToString().Trim() + "')", true);
                return;
            }
            string strDocType = GetDocTypeName(strDocTypeID);
            string strDepartCode = GetDepartCode(strUserCode);

            string strVisible = DL_Visible.SelectedValue.Trim();

            string strFileName1, strExtendName;

            strFileName1 = this.AttachFile.FileName;//��ȡ�ϴ��ļ����ļ���,������׺
            strExtendName = System.IO.Path.GetExtension(strFileName1);//��ȡ��չ��

            DateTime dtUploadNow = DateTime.Now; //��ȡϵͳʱ��

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";

            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);
            string strSystemVersionType = Session["SystemVersionType"].ToString();
            string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
            if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
            {
                //if (this.AttachFile.ContentLength > 10240000)
                //{
                //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBNSCDYSZDWJ").ToString().Trim() + "')", true);
                //    return;
                //}
            }

            if (fi.Exists)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "')", true);
            }
            else
            {
                DocumentBLL documentBLL = new DocumentBLL();
                Document document = new Document();

                document.RelatedType = "Project";
                document.DocTypeID = int.Parse(strDocTypeID);
                document.DocType = strDocType;
                document.RelatedID = int.Parse(strProjectID);
                document.Author = strAuthor;
                document.DocName = strFileName2;
                document.Address = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                document.UploadManCode = strUserCode;
                document.UploadManName = GetUserName(strUserCode);
                document.UploadTime = DateTime.Now;
                document.Visible = strVisible;
                document.DepartCode = strDepartCode; document.DepartName = ShareClass.GetDepartName(strDepartCode);
                document.Status = "InProgress";
                document.RelatedName = "";

                try
                {
                    documentBLL.AddDocument(document);

                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);

                    LB_FileName.Text = strFileName1;

                    TB_Message.Text = GetUserName(strUserCode) + LanguageHandle.GetWord("ShangChuanLeXiangMu").ToString().Trim() + strProjectID + " " + strProjectName + LanguageHandle.GetWord("DeWenJian").ToString().Trim() + strFileName1 + LanguageHandle.GetWord("QingJiShiChaYue").ToString().Trim();
                    LoadRelatedDoc();

                    ShareClass.InitialDocTypeTree(TreeView1, strUserCode, "Project", strProjectID, strProjectName);


                    TB_Message.Text = GetUserName(strUserCode) + LanguageHandle.GetWord("ShangChuanLeWenJian").ToString().Trim() + strFileName2 + LanguageHandle.GetWord("QingJiShiChaKan").ToString().Trim();
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strSubject, strMsg, strRelatedUserCode;
        string strHQL, strUserCode, strProjectID;
        IList lst;
        int i = 0;

        string strVisible = DL_Visible.SelectedValue.Trim();


        strProjectID = LB_ProjectID.Text.Trim();
        strMsg = TB_Message.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        Msg msg = new Msg();

        if (CB_SMS.Checked == true | CB_Mail.Checked == true)
        {
            RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
            RelatedUser relatedUser = new RelatedUser();
            strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID;
            lst = relatedUserBLL.GetAllRelatedUsers(strHQL);

            strSubject = LanguageHandle.GetWord("WenJianChaYueTongZhi").ToString().Trim();

            for (i = 0; i < lst.Count; i++)
            {
                relatedUser = (RelatedUser)lst[i];
                strRelatedUserCode = relatedUser.UserCode.Trim();

                if (CB_SMS.Checked == true)
                {
                    msg.SendMSM("Message",strRelatedUserCode, strMsg, strUserCode);
                }

                if (CB_Mail.Checked == true)
                {
                    msg.SendMail(strRelatedUserCode, strSubject, strMsg, strUserCode);
                }
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSWB").ToString().Trim() + "')", true);

    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DocumentBLL documentBLL = new DocumentBLL();
        IList lst = documentBLL.GetAllDocuments(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        //�����ĵ����޹������������ɾ����ť
        ShareClass.HideDataGridDeleteButtonForDocUploadPage(DataGrid1);
    }

   
    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strPlanID, strParentID, strPlanVerID;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;
        strPlanID = treeNode.Target.Trim();

        try
        {
            strParentID = treeNode.Parent.Target;

            strHQL = " from Document as document where document.RelatedType = 'Plan' and document.RelatedID = " + strPlanID + " and document.Status <> 'Deleted' Order by document.DocID DESC";
            DocumentBLL documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();
        }
        catch
        {
            strPlanVerID = LB_TemplatePlanVerID.Text.Trim();
            LoadProjectPlanDoc(strProjectID, strPlanVerID);
        }

        LB_TemplatePlanID.Text = strPlanID;
    }

    public static void LoadProjectActorGroupForDropDownList(DropDownList DL_Visible, string strProjectID)
    {
        string strHQL;
        string strLangCode, strUserCode;

        strLangCode = HttpContext.Current.Session["LangCode"].ToString();
        strUserCode = HttpContext.Current.Session["UserCode"].ToString().Trim();

        strHQL = "Select distinct GroupName,HomeName from T_ActorGroup where (GroupName = '����' or GroupName = 'ȫ��') "; 
        strHQL += " and LangCode = " + "'" + strLangCode + "'";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ActorGroup");
        DL_Visible.DataSource = ds;
        DL_Visible.DataBind();
    }

    public string LoadRelatedDoc()
    {
        string strHQL, strUserCode, strDepartCode;
        IList lst;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = GetDepartCode(strUserCode);
        strHQL = "from Document as document where (((document.RelatedType = 'Project' and document.RelatedID = " + strProjectID + ")";
        strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
        strHQL += " or (document.Visible in ( '����','ȫ��')))"; 
        strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))))";

        strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
        strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + "))";

        strHQL += "or (document.RelatedType = '����' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))"; 
        strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
        strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
        strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + ")))";

        strHQL += "or (document.RelatedType = '����' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + "))"; 
        strHQL += " and ((document.Visible in ('����','����') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) "; 
        strHQL += " or (document.Visible = 'ȫ��' )))))"; 

        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst.Count.ToString();

      
        //�����ĵ����޹������������ɾ����ť
        ShareClass.HideDataGridDeleteButtonForDocUploadPage(DataGrid1);

        return lst.Count.ToString();
    }



    protected void LoadProjectPlanDoc(string strProjectID, string strPlanVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Document as document where document.RelatedType = 'Plan'";
        strHQL += " and document.RelatedID in (Select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + " and workPlan.VerID = " + strPlanVerID + ")";
        strHQL += " order by document.DocID DESC";
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected string GetDocTypeName(string strDocTypeID)
    {
        DocTypeBLL docTypeBLL = new DocTypeBLL();

        string strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        DocType docType = (DocType)lst[0];

        return docType.Type.Trim();
    }

    protected string GetProjectStatus(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        Project project = (Project)lst[0];

        return project.Status.Trim();
    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName.Trim();
        return strUserName.Trim();
    }

    protected string GetDepartCode(string strUserCode)
    {
        string strDepartCode, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strDepartCode = projectMember.DepartCode.Trim();
        return strDepartCode;
    }
}
