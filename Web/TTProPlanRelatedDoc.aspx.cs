using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using TakeTopCore;

using ProjectMgt.BLL;
using ProjectMgt.DAL;
using ProjectMgt.Model;

using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using com.sun.org.apache.xalan.@internal.xsltc.dom;


public partial class TTProPlanRelatedDoc : System.Web.UI.Page
{
    string strProjectID, strPlanID, strPlanName, strPlanVerID, strProjectType;
    string strLangCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strLangCode = Session["LangCode"].ToString();

        string strUserCode, strUserName, strDepartCode;
        string strFromProjectID, strFromProjectPlanVerID;

        strUserCode = Session["UserCode"].ToString();
        strUserName = GetUserName(strUserCode);
        strDepartCode = GetDepartCode(strUserCode);

        strPlanID = Request.QueryString["PlanID"];

        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            Response.Redirect("TTProPlanRelatedDocSAAS.aspx?PlanID=" + strPlanID);
        }

        //if (ShareClass.CheckUserCanControlProjectPlan(strPlanID, strUserCode) == false)
        //{
        //    Response.Redirect("TTDisplayCustomErrorMessage.aspx?ErrorMsg='" + LanguageHandle.GetWord("ZZJGZYXMJLJHYJHCJRHLXZJHFZRCNJXZCZQJC") + "'");
        //}

        strHQL = "from WorkPlan as workPlan where workPlan.ID = " + strPlanID;
        WorkPlanBLL workPlanBLL = new WorkPlanBLL();
        WorkPlan workPlan = new WorkPlan();
        lst = workPlanBLL.GetAllWorkPlans(strHQL);
        workPlan = (WorkPlan)lst[0];

        strPlanName = workPlan.Name.Trim();
        strProjectID = workPlan.ProjectID.ToString().Trim();
        strPlanVerID = workPlan.VerID.ToString().Trim();


        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        strProjectType = project.ProjectType.Trim();

        LB_PlanID.Text = strPlanID;
        LB_ProjectID.Text = strProjectID;
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandlerForSpecialPopWindow();", true);
        if (Page.IsPostBack == false)
        {
            ShareClass.InitialDocTypeTree(TreeView1, strUserCode, "Plan", strPlanID, strPlanName);
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY");

            LoadRelatedDoc(strPlanID, strProjectID);

            ShareClass.InitialUserDocTypeTree(TreeView3, strUserCode);

            ShareClass.LoadProjectActorGroupForDropDownList(DL_Visible, strProjectID);

            TB_Author.Text = strUserName;

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
            }

            LoadProPlanRelatedDocForTemplate(strPlanID);
            HL_DocumentForProjectPlanTemplate.NavigateUrl = "TTProPlanRelatedDocForTemplateView.aspx?PlanID=" + strPlanID;

            //设置对象相关缺省的工作流模板
            SetDefaultWorkflowTemplateByRelateName("ProjectPlan", strPlanID, strPlanName, DL_TemName);
        }
    }

    //设置对象相关缺省的工作流模板
    public static void SetDefaultWorkflowTemplateByRelateName(string strRelatedType, string strRelatedID, string strRelateName, DropDownList DL_WorkFlowTemName)
    {
        string strHQL;

        DocTypeBLL docTypeBLL = new DocTypeBLL();

        strHQL = string.Format(@"select * from T_WorkFlowTemplate  where (TemName in (Select wftemplatename From T_RelatedWorkflowTemplate Where 
													 ((RelatedType = '{0}' and RelatedID = {1})
                                 Or (wftemplatename in (Select PlanStartupRelatedWorkflowTemplate From T_Project 
													 Where ProjectID in (Select ProjectID From T_ImplePlan Where ID = {1})))))
                                 Or (Type = 'DocumentReview' and Authority = 'All' )) and Visible = 'YES' Order By SortNumber", strRelatedType, strRelatedID);
        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        DL_WorkFlowTemName.DataSource = ds1;
        DL_WorkFlowTemName.DataBind();

        strHQL = string.Format(@"select * from T_WorkFlowTemplate  where (TemName in (Select wftemplatename From T_RelatedWorkflowTemplate Where RelatedType = '{0}' and RelatedID = {1}  and '{2}' like '%'|| wftemplatename ||'%' )
                              Or (Type = 'DocumentReview' and Authority = 'All' ))
                              and Visible = 'YES'", strRelatedType, strRelatedID, strRelateName);
        DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        if (ds2.Tables[0].Rows.Count > 0)
        {
            DL_WorkFlowTemName.SelectedValue = ds2.Tables[0].Rows[0]["TemName"].ToString().Trim();

        }
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

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        lst1 = docTypeBLL.GetAllDocTypes(strHQL);

        if (strDocTypeID != "0")
        {
            DocType docType = (DocType)lst1[0];
            strDocType = docType.Type.Trim();

            strHQL = " Select * from T_Document as document where document.RelatedType = 'Plan' and document.RelatedID = " + strPlanID + " and  document.DocType = " + "'" + strDocType + "'" + " and document.Status <> 'Deleted' Order by document.DocID DESC";
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLX") + strDocType;

            //设置缺省的文件类型
            ShareClass.SetDefaultDocType(strDocType, LB_DocTypeID, TB_DocType);

            ////设置缺省的工作流模板树
            //ShareClass.SetDefaultWorkflowTemplate(strDocType, DL_TemName);

            //////设置缺省的工作流模板树
            //SetDefaultWorkflowTemplateByPlanName("ProjectPlan", strPlanID, strPlanName);
        }
        else
        {
            strHQL = " Select * from T_Document as document where document.RelatedType = 'Plan' and document.RelatedID = " + strPlanID + " and document.Status <> 'Deleted' Order by document.DocID DESC";
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY");

            strDocType = "";
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Doc");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + ds.Tables[0].Rows.Count.ToString();

        //根据文档有无工作流情况隐藏删除按钮
        ShareClass.HideDataGridDeleteButtonForDocUploadPage(DataGrid1);

        try
        {
            //按文件类型列出模板名
            DL_ProPlanRelatedDocForTemplate.SelectedValue = GetProPlanRelatedDocForTemplateByDocType(strPlanID, strDocType);
        }
        catch
        {
        }

        //列出要传没传的文档
        LoadProjectPlanUnloadDoc(strProjectID);
    }



    protected void BT_LoadDoc_Click(object sender, EventArgs e)
    {
        LoadRelatedDoc(strPlanID, strProjectID);
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode = LB_UserCode.Text.Trim();
        string strDepartCode = GetDepartCode(strUserCode);
        IList lst;
        DocumentBLL documentBLL = new DocumentBLL();

        string strDocName = TB_DocName.Text.Trim();
        strDocName = "%" + strDocName + "%";

        if (strPlanID != null)
        {
            strHQL = " Select * from T_Document as document where ";
            strHQL += " (document.RelatedType = 'Plan' and document.RelatedID = " + strPlanID;
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( 'Department','Entire'))))";   
            strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted'";
        }
        else
        {
            strHQL = " Select * from T_Document as document where ";
            strHQL += " (document.RelatedType = 'Plan' and document.RelatedID in (Select workPlan.ID from T_ImplePlan as workPlan where workPlan.ProjectID = " + strProjectID + " and workPlan.VerID = " + strPlanVerID + ")";
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( 'Department','Entire'))))";   
            strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted'";
        }

        strHQL += " and (document.DocID || '_' || document.DocName) Like '" + strDocName + "'";
        strHQL += "  Order by document.DocID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Doc");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + ds.Tables[0].Rows.Count.ToString();

        //根据文档有无工作流情况隐藏删除按钮
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

            strHQL = " Select * from T_Document as document where document.RelatedType = 'Plan' and document.RelatedID = " + strPlanID + " and document.Status <> 'Deleted' Order by document.DocID DESC";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Doc");
            DataGrid2.DataSource = ds;
            DataGrid2.DataBind();
        }
        catch
        {
            strPlanVerID = LB_TemplatePlanVerID.Text.Trim();
            LoadProjectPlanDoc(strProjectID, strPlanVerID);
        }

        LB_TemplatePlanID.Text = strPlanID;
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
            string strDocName = ((HyperLink)e.Item.Cells[4].Controls[0]).Text.Trim();
            string strUploadMan = e.Item.Cells[6].Text.Trim();
            string strUploadManCode;
            string strDepartCode = GetDepartCode(strUserCode);

            if (e.CommandName == "Delete")
            {
                strHQL = "from Document as document where document.DocID = " + strDocID;
                DocumentBLL documentBLL = new DocumentBLL();
                lst = documentBLL.GetAllDocuments(strHQL);
                Document document = (Document)lst[0];

                strUploadManCode = document.UploadManCode.Trim();

                if (strUserCode == strUploadManCode)
                {
                    document.Status = "Deleted";

                    documentBLL.UpdateDocument(document, int.Parse(strDocID));

                    //删除更多文档
                    ShareClass.DeleteMoreDocByDataGrid(DataGrid1);

                    LoadRelatedDoc(strPlanID, strProjectID);
                    ShareClass.InitialDocTypeTree(TreeView1, strUserCode, "Plan", strPlanID, strPlanName);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFFCZNBNSCBRSCDWJ") + "')", true);
                }
            }

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

                TB_WLName.Text = LanguageHandle.GetWord("WenJian") + strDocID + strDocName + LanguageHandle.GetWord("PingShen");

                BT_SubmitApply.Enabled = true;

                LoadRelatedWL("DocumentReview", "Plan", int.Parse(strPlanID));
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

            strHQL = "Select * from T_Document as document where document.RelatedType = 'ProjectType' and document.RelatedName = " + "'" + strProjectType + "'" + " and  document.DocType = " + "'" + strDocType + "'" + " and document.Status <> 'Deleted' Order by document.DocID DESC";
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLX") + strDocType;
        }
        else
        {
            strHQL = "Select * from T_Document as document where document.RelatedType =  'ProjectType' and document.RelatedName = " + "'" + strProjectType + "'" + " and document.Status <> 'Deleted' Order by document.DocID DESC";
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY");
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Doc");
        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();

        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + ds.Tables[0].Rows.Count.ToString();
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGWDLXBNWKJC") + "')", true);
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
                //    if (this.AttachFile.ContentLength > 1024)
                //    {
                //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBNSCDYSZDWJ") + "')", true);
                //        return;
                //    }
            }

            if (fi.Exists)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC") + "')", true);
            }
            else
            {
                DocumentBLL documentBLL = new DocumentBLL();
                Document document = new Document();

                document.RelatedType = "Plan";
                document.DocTypeID = int.Parse(strDocTypeID);
                document.DocType = strDocType;
                document.RelatedID = int.Parse(strPlanID);
                document.Author = strAuthor;
                document.DocName = strFileName2;
                document.Address = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                document.UploadManCode = strUserCode;
                document.UploadManName = GetUserName(strUserCode);
                document.UploadTime = DateTime.Now;
                document.Visible = strVisible;
                document.DepartCode = strDepartCode;
                document.DepartName = ShareClass.GetDepartName(strDepartCode);
                document.Status = "InProgress";
                document.RelatedName = "";

                try
                {
                    documentBLL.AddDocument(document);


                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);

                    //保存必传文件名
                    string strDocID = ShareClass.GetMyCreatedMaxDocID(strUserCode);
                    AddMustBeUploadDocName(strDocID);

                    LoadRelatedDoc(strPlanID, strProjectID);
                    ShareClass.InitialDocTypeTree(TreeView1, strUserCode, "Plan", strPlanID, strPlanName);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ") + "')", true);
        }
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
        workFlow.RelatedType = "Plan";
        workFlow.RelatedID = int.Parse(strPlanID);
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

            LoadRelatedWL("DocumentReview", "Plan", int.Parse(strPlanID));

            LoadRelatedDoc(strPlanID, strProjectID);

            //工作流模板是否是自动激活状态
            if (ShareClass.GetWorkflowTemplateIsAutoActiveStatus(strTemName) == "NO")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWJPSSSCDGZLGLYMJHCGZLS") + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGZLFQCG") + "')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWJPSSSB") + "')", true);
        }
    }

    protected void BT_Refrash_Click(object sender, EventArgs e)
    {
        //设置对象相关缺省的工作流模板
        ShareClass.SetDefaultWorkflowTemplateByRelateName("ProjectPlan", strPlanID, strPlanName, DL_TemName);
    }

    //保存必传文件名
    protected void AddMustBeUploadDocName(string strDocID)
    {
        string strHQL;
        string strMustUploadDoc = DL_ProPlanRelatedDocForTemplate.SelectedValue.Trim();

        strHQL = string.Format(@"Update T_Document Set MustUploadDoc = '{0}' Where DocID = {1}", strMustUploadDoc, strDocID);
        ShareClass.RunSqlCommand(strHQL);
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

    protected void LoadRelatedDoc(string strPlanID, string strProjectID)
    {
        string strHQL;
        string strUserCode = LB_UserCode.Text.Trim();
        string strDepartCode = GetDepartCode(strUserCode);

        if (strPlanID != null)
        {
            strHQL = "Select * from T_Document as document where ";
            strHQL += " (document.RelatedType = 'Plan' and document.RelatedID = " + strPlanID;
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( 'Department','Entire'))))";   
            strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";
        }
        else
        {
            strHQL = "Select * from T_Document as document where ";
            strHQL += " (document.RelatedType = 'Plan' and document.RelatedID in (Select workPlan.ID from T_ImplePlan as workPlan where workPlan.ProjectID = " + strProjectID + " and workPlan.VerID = " + strPlanVerID + ")";
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( 'Department','Entire'))))";   
            strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Doc");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + ds.Tables[0].Rows.Count.ToString();

        //根据文档有无工作流情况隐藏删除按钮
        ShareClass.HideDataGridDeleteButtonForDocUploadPage(DataGrid1);
    }

    protected void LoadProjectTypeRelatedDoc(string strRelatedType, string strRelatedName)
    {
        string strHQL;

        strHQL = "Select * from T_Document as document where document.RelatedType = 'ProjectType' and document.RelatedName = " + "'" + strRelatedName + "'" + " and document.Status <> 'Deleted' Order by document.DocID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Doc");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void LoadProjectPlanDoc(string strProjectID, string strPlanVerID)
    {
        string strHQL;

        strHQL = "Select * from T_Document as document where document.RelatedType = 'Plan'";
        strHQL += " and document.RelatedID in (Select workPlan.ID from T_ImplePlan as workPlan where workPlan.ProjectID = " + strProjectID + " and workPlan.VerID = " + strPlanVerID + ")";
        strHQL += " order by document.DocID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Doc");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        //列出要传没传的文档
        LoadProjectPlanUnloadDoc(strProjectID);
    }

    //列出要传没传的文档
    protected void LoadProjectPlanUnloadDoc(string strProjectID)
    {
        string strHQL;

        strHQL = string.Format(@"select t1.* FROM  t_documentForProjectPlanTemplate t1
                    where t1.RelatedType='Plan' and t1.Status<> 'Deleted' and t1.RelatedID in (Select ID From T_ImplePlan Where ProjectID = {0})
			        and t1.DocName Not in (Select t2.MustUploadDoc From T_Document t2 Where t2.RelatedType='Plan' and t2.Status<> 'Deleted' and t2.RelatedID in (Select ID From T_ImplePlan Where ProjectID = {0})
			    )", strProjectID);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Document");
        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();

        LB_UnUploadMustDocCount.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + ds.Tables[0].Rows.Count.ToString();

        TR_UnUploadForMustDocList.Visible = true;
    }

    protected void LoadProPlanRelatedDocForTemplate(string strPlanID)
    {
        string strHQL;

        strHQL = "Select  distinct DocName  From t_documentForProjectPlanTemplate where RelatedType = 'Plan' and Status <> 'Deleted' and RelatedID = " + strPlanID + " Order By DocName ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "t_documentForProjectPlanTemplate");

        DL_ProPlanRelatedDocForTemplate.DataSource = ds;
        DL_ProPlanRelatedDocForTemplate.DataBind();
    }

    //按文件类型取得模板名
    protected string GetProPlanRelatedDocForTemplateByDocType(string strPlanID, string strDocType)
    {
        string strHQL;

        strHQL = "Select  distinct DocName  From t_documentForProjectPlanTemplate where RelatedType = 'Plan' and Status <> 'Deleted' and RelatedID = " + strPlanID;
        strHQL += " and DocType = '" + strDocType + "'";
        strHQL += " Order By DocName ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "t_documentForProjectPlanTemplate");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return strDocType;
        }
    }

    protected string GetDocTypeName(string strDocTypeID)
    {
        DocTypeBLL docTypeBLL = new DocTypeBLL();

        string strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        DocType docType = (DocType)lst[0];

        return docType.Type.Trim();
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
