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
using System.Collections.Generic;
using com.sun.org.apache.xml.@internal.resolver.helpers;
using System.Net;
using com.sun.org.apache.xerces.@internal.impl;
using sun.swing;
using System.Web.Hosting;


public partial class TTProjectRelatedDoc : System.Web.UI.Page
{
    string strProjectID, strProjectName, strProjectType;
    string strLangCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode, strUserName, strDepartCode;
        string strFromProjectID, strFromProjectPlanVerID;

        strLangCode = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strUserName = GetUserName(strUserCode);
        strDepartCode = GetDepartCode(strUserCode);

        strProjectID = Request.QueryString["ProjectID"];
        LB_ProjectID.Text = strProjectID;


        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            Response.Redirect("TTProjectRelatedDocSAAS.aspx?ProjectID=" + strProjectID);
        }


        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        strProjectName = project.ProjectName.Trim();
        strProjectType = project.ProjectType.Trim();

        if (lst.Count > 0)
        {
            //this.Title = LanguageHandle.GetWord("Project").ToString().Trim() + strProjectID + " " + strProjectName + "的相关文档";
        }

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandlerForSpecialPopWindow();", true);
        if (Page.IsPostBack == false)
        {
            ShareClass.InitialDocTypeTree(TreeView1, strUserCode, "Project", strProjectID, strProjectName);
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY").ToString().Trim();

            ShareClass.InitialUserDocTypeTree(TreeView3, strUserCode);

            ShareClass.LoadProjectActorGroupForDropDownList(DL_Visible, strProjectID);

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

                ShareClass.InitialDocTypeTree(TreeView4, strUserCode, "ProjectType", "0", strProjectType);
                LoadProjectTypeRelatedDoc("ProjectType", strProjectType);

                LB_TemplateProjectID.Text = strFromProjectID;
                LB_TemplatePlanVerID.Text = strFromProjectPlanVerID;


                //设置缺省的工作流模板
                ShareClass.SetDefaultWorkflowTemplateByRelateName("Project", strProjectID, strProjectName, DL_TemName);
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
                strHQL += " or (document.Visible in ( 'Department','Entire')))";   
                strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))))";

                strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
                strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + "))";

                strHQL += "or (document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  
                strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
                strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
                strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + ")))";

                strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + "))";  
                strHQL += " and ((document.Visible in ('Meeting','Department') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) ";  
                strHQL += " or (document.Visible = 'Entire' )))))";   
                strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

                LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLX").ToString().Trim() + strDocType;

                //设置缺省的文件类型
                ShareClass.SetDefaultDocType(strDocType, LB_DocTypeID, TB_DocType);

                ////按文件类型设置缺省的工作流模板树
                //ShareClass.SetDefaultWorkflowTemplate(strDocType, DL_TemName);
            }
        }
        else
        {
            LB_DocTypeID.Text = "";
            TB_DocType.Text = "";

            strHQL = "from Document as document where (((document.RelatedType = 'Project' and document.RelatedID = " + strProjectID + ")";
            strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( 'Department','Entire')))";   
            strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))))";

            strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
            strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + "))";

            strHQL += "or (document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  
            strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
            strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
            strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + ")))";

            strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + "))";  
            strHQL += " and ((document.Visible in ('Meeting','Department') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) ";  
            strHQL += " or (document.Visible = 'Entire' )))))";   
            strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY").ToString().Trim();
        }

        lst2 = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst2;
        DataGrid1.DataBind();

        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst2.Count.ToString();

        //根据文档有无工作流情况隐藏删除按钮
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

                    //删除更多文档
                    ShareClass.DeleteMoreDocByDataGrid(DataGrid1);

                    LoadRelatedDoc();
                    ShareClass.InitialDocTypeTree(TreeView1, strUserCode, "Project", strProjectID, strProjectName);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFFCZNBNSCBRSCDWJ").ToString().Trim() + "')", true);
                }
            }

            //if (e.CommandName == "Download")
            //{
            //    DownloadMoreDocByDataGrid(DataGrid1);
            //}

            if (e.CommandName == "Review")
            {
                MultiView1.ActiveViewIndex = 1;
                RadioButtonList1.SelectedIndex = 1;

                LB_DocID.Text = strDocID;

                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                TB_WLName.Text = LanguageHandle.GetWord("PingShen").ToString().Trim() + LanguageHandle.GetWord("WenJian").ToString().Trim() + strDocID + strDocName;

                BT_SubmitApply.Enabled = true;

                LoadRelatedWL("DocumentReview", "Document", int.Parse(strDocID));
            }
        }
    }

    //下载更多文档
    public static void DownloadMoreDocByDataGrid(DataGrid dataGrid1)
    {
        string strHQL;
        string strDocID, strURL, strFileName, strExtendName;

        var client = new WebClient();

        for (int i = 0; i < dataGrid1.Items.Count; i++)
        {
            if (((CheckBox)(dataGrid1.Items[i].FindControl("CB_Select"))).Checked == true)
            {
                strDocID = dataGrid1.Items[i].Cells[0].Text.Trim();
                strHQL = "Select Address From T_Document Where DocID =" + strDocID;
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Document");
                strURL = ds.Tables[0].Rows[0]["Address"].ToString().Trim();

                strFileName = Path.GetFileName(HttpContext.Current.Server.MapPath(strURL));
                strExtendName = System.IO.Path.GetExtension(strFileName);//获取扩展名

                string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath;
                string fullUrl = baseUrl + "/" + strURL.Replace("\\", "/");

                new System.Threading.Thread(delegate ()
                {
                    client.DownloadFile(new Uri(fullUrl), strFileName);

                }).Start();
            }
        }
    }

    

    //删除更多文档
    public static void DeleteMoreDocByDataGrid(DataGrid dataGrid1)
    {
        string strHQL;

        string strDocID;

        for (int i = 0; i < dataGrid1.Items.Count; i++)
        {
            if (((CheckBox)(dataGrid1.Items[i].FindControl("CB_Select"))).Checked == true)
            {
                strDocID = dataGrid1.Items[i].Cells[0].Text.Trim();

                try
                {
                    strHQL = "Update T_Document Set Status = 'Deleted' Where DocID = " + strDocID;
                    ShareClass.RunSqlCommand(strHQL);
                }
                catch
                {
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

            strFileName1 = this.AttachFile.FileName;//获取上传文件的文件名,包括后缀
            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

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
                    msg.SendMSM("Message", strRelatedUserCode, strMsg, strUserCode);
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

        //根据文档有无工作流情况隐藏删除按钮
        ShareClass.HideDataGridDeleteButtonForDocUploadPage(DataGrid1);
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;
        string strUserCode = LB_UserCode.Text.Trim();

        while (i < RadioButtonList1.Items.Count)
        {
            if (RadioButtonList1.Items[i].Selected == true)
            {
                MultiView1.ActiveViewIndex = int.Parse(RadioButtonList1.Items[i].Value);
            }
            i++;
        }
    }

    protected void BT_SubmitApply_Click(object sender, EventArgs e)
    {
        string strWFID, strWLName, strWLType, strTemName, strXMLFileName, strXMLFile1, strXMLFile2;
        string strDescription, strCreatorCode, strCreatorName, strUserCode;
        string strCmdText, strDocID;
        DateTime dtCreateTime;

        strDocID = LB_DocID.Text.Trim();

        XMLProcess xmlProcess = new XMLProcess();

        strWLName = TB_WLName.Text.Trim();
        strWLType = DL_WFType.SelectedValue.Trim();
        strTemName = DL_TemName.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strCreatorCode = LB_UserCode.Text.Trim();
        strCreatorName = GetUserName(strCreatorCode);
        dtCreateTime = DateTime.Now;

        strXMLFileName = strWLType + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
        strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;

        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        WorkFlow workFlow = new WorkFlow();

        workFlow.WLName = strWLName;
        workFlow.WLType = strWLType;
        workFlow.XMLFile = strXMLFile2;
        workFlow.TemName = strTemName;
        workFlow.Description = strDescription;
        workFlow.CreatorCode = strCreatorCode;
        workFlow.CreatorName = strCreatorName;
        workFlow.CreateTime = dtCreateTime;
        workFlow.Status = "New";
        workFlow.RelatedType = "Document";
        workFlow.RelatedID = int.Parse(strDocID);
        workFlow.DIYNextStep = "YES"; workFlow.IsPlanMainWorkflow = "NO";

        if (CB_RequiredSMS.Checked == true)
        {
            workFlow.ReceiveSMS = "YES";
        }
        else
        {
            workFlow.ReceiveSMS = "NO";
        }

        if (CB_RequiredMail.Checked == true)
        {
            workFlow.ReceiveEMail = "YES";
        }
        else
        {
            workFlow.ReceiveEMail = "NO";
        }

        try
        {
            workFlowBLL.AddWorkFlow(workFlow);

            strUserCode = LB_UserCode.Text.Trim();
            strWFID = ShareClass.GetMyCreatedWorkFlowID(strUserCode);

            strCmdText = "select * from T_Document where DocID = " + strDocID;
            strXMLFile2 = Server.MapPath(strXMLFile2);

            xmlProcess.DbToXML(strCmdText, "T_Document", strXMLFile2);

            //自动附加要评审的工作流文件
            ShareClass.AddWLDocumentForUploadDocPage(strDocID, int.Parse(strWFID));
            //自动附加其它已选择的要评审的工作流文件
            ShareClass.AddMoreWLSelectedDocumentForUploadDocPage(DataGrid1, int.Parse(strWFID), strDocID);

            LoadRelatedWL("DocumentReview", "Document", int.Parse(strDocID));

            //工作流模板是否是自动激活状态
            if (ShareClass.GetWorkflowTemplateIsAutoActiveStatus(strTemName) == "NO")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWJPSSSCDGZLGLYMJHCGZLS").ToString().Trim() + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGZLFQCG").ToString().Trim() + "')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWJPSSSB").ToString().Trim() + "')", true);
        }
    }

    protected int GetWLID(string strWLName, string strWLType, string strXMLFile, string strCreatorCode, DateTime dtCreateTime)
    {
        string strHQL;
        int intWLID;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLName = " + "'" + strWLName + "'";
        strHQL += " and workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.XMLFile = ";
        strHQL += "'" + strXMLFile + "'" + " and workFlow.CreatorCode = " + "'" + strCreatorCode + "'" + " and to_char(workFlow.CreateTime,'yyyy-mm-dd hh:mi:ss') = " + "'" + dtCreateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        WorkFlow workFlow = (WorkFlow)lst[0];

        intWLID = workFlow.WLID;

        return intWLID;
    }

    protected void BT_Refrash_Click(object sender, EventArgs e)
    {
        //设置缺省的工作流模板
        ShareClass.SetDefaultWorkflowTemplateByRelateName("Project", strProjectID, strProjectName, DL_TemName);

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

    protected void LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    public string LoadRelatedDoc()
    {
        string strHQL, strUserCode, strDepartCode, strProjectType;
        IList lst;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = GetDepartCode(strUserCode);

        strProjectType = "EngineeringProject";   
        strHQL = string.Format(@"Select * From (Select * from T_Document as document where (((document.RelatedType = 'Project' and document.RelatedID = {0})
                   and (((document.UploadManCode = '{1}' and document.DepartCode = '{2}')
                   or (document.Visible in ( 'Department','Entire')))
                   or (document.Visible in (select actorGroupDetail.GroupName from T_ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = '{1}'))))
                   or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from T_RelatedReq as relatedReq where relatedReq.ProjectID = {0}))
                   or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From T_WorkFlow as workFlow Where workFlow.RelatedType = 'Project' and workFlow.RelatedID = {0}))
                   or (document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from T_ProjectRisk as projectRisk where projectRisk.ProjectID = {0}))
                   or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from T_ProjectTask as projectTask where projectTask.ProjectID = {0}))
                   or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID From T_ImplePlan as workPlan where workPlan.ProjectID = {0}))
                   or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From T_WorkFlow as workFlow Where workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID From T_ImplePlan as workPlan where workPlan.ProjectID = {0})))
                   or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from T_Meeting as meeting where meeting.RelatedID = {0}))
                   and ((document.Visible in ('Meeting','Department') and document.DepartCode = '{2}' ) 
                   or (document.Visible = 'Entire' )))))
                   and rtrim(ltrim(document.Status)) <> 'Deleted') A, (Select * From T_Document Where RelatedType = 'ProjectType' and RelatedName = '{3}' ) B  Where  position(B.DocName in A.DocName) > 0.", strProjectID, strUserCode, strDepartCode, strProjectType);   

        strHQL = "from Document as document where (((document.RelatedType = 'Project' and document.RelatedID = " + strProjectID + ")";
        strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
        strHQL += " or (document.Visible in ( 'Department','Entire')))";   
        strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))))";

        strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
        strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + "))";

        strHQL += "or (document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  
        strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
        strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
        strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + ")))";

        strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + "))";  
        strHQL += " and ((document.Visible in ('Meeting','Department') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) ";  
        strHQL += " or (document.Visible = 'Entire' )))))";   

        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst.Count.ToString();

        //根据文档有无工作流情况隐藏删除按钮
        ShareClass.HideDataGridDeleteButtonForDocUploadPage(DataGrid1);

        return lst.Count.ToString();
    }

    protected void TreeView4_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDocTypeID, strHQL, strUserCode, strDepartCode, strDocType;
        IList lst1, lst2;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView4.SelectedNode;

        strDocTypeID = treeNode.Target.Trim();

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        lst1 = docTypeBLL.GetAllDocTypes(strHQL);

        DocumentBLL documentBLL = new DocumentBLL();

        if (strDocTypeID != "0")
        {
            DocType docType = (DocType)lst1[0];
            strDocType = docType.Type.Trim();

            strHQL = " from Document as document where document.RelatedType = 'ProjectType' and document.RelatedName = " + "'" + strProjectType + "'" + " and  document.DocType = " + "'" + strDocType + "'" + " and document.Status <> 'Deleted' Order by document.DocID DESC";
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLX").ToString().Trim() + strDocType;
        }
        else
        {
            strHQL = " from Document as document where document.RelatedType =  'ProjectType' and document.RelatedName = " + "'" + strProjectType + "'" + " and document.Status <> 'Deleted' Order by document.DocID DESC";
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY").ToString().Trim();
        }

        lst2 = documentBLL.GetAllDocuments(strHQL);
        DataGrid3.DataSource = lst2;
        DataGrid3.DataBind();

        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst2.Count.ToString();
    }

    protected void LoadProjectTypeRelatedDoc(string strRelatedType, string strRelatedName)
    {
        string strHQL;
        IList lst;

        strHQL = " from Document as document where document.RelatedType = 'ProjectType' and document.RelatedName = " + "'" + strRelatedName + "'" + " and document.Status <> 'Deleted' Order by document.DocID DESC";
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
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
