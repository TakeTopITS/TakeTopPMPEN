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

public partial class TTDocumentReviewWF : System.Web.UI.Page
{
    string strLangCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode, strUserName, strDepartCode;
        IList lst;

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();
        strUserName = GetUserName(strUserCode);

        strDepartCode = GetDepartCode(strUserCode);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        //this.Title = "我的文档上传";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandlerForSpecialPopWindow();", true);
        if (Page.IsPostBack == false)
        {
            ShareClass.InitialDocTypeTree(TreeView1, strUserCode, "Other", "0", "0");
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY");

            LoadDocument(strUserCode);

            lst = ShareClass.GetDocTypeList(strUserCode);
            DL_DocType.DataSource = lst;
            DL_DocType.DataBind();

            strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName <> 'Entire' and  actorGroup.Type = 'All' ";   
            strHQL += " and actorGroup.LangCode = " + "'" + strLangCode + "'";
            strHQL += " Order by actorGroup.SortNumber ASC";
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            lst = actorGroupBLL.GetAllActorGroups(strHQL);
            DL_Visible.DataSource = lst;
            DL_Visible.DataBind();

            ShareClass.LoadWFTemplate(strUserCode, "DocumentReview", DL_TemName);

            TB_Author.Text = strUserName;
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

        DocumentBLL documentBLL = new DocumentBLL();

        if (strDocTypeID != "0")
        {
            DocType docType = (DocType)lst1[0];
            strDocType = docType.Type.Trim();

            strHQL = " from Document as document where document.DocType = " + "'" + strDocType + "'";
            strHQL += " and document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'";
            strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLX") + strDocType;

            ////设置缺省的文件类型
            //ShareClass.SetDefaultDocType(strDocType, LB_DocTypeID, TB_DocType);
            //按文件类型设置缺省的工作流模板树
            //ShareClass.SetDefaultWorkflowTemplate(strDocType, DL_TemName);
        }
        else
        {
            strHQL = " from Document as document where ";
            strHQL += " document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'";
            strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY");
        }

        lst2 = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst2;
        DataGrid1.DataBind();

        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst2.Count.ToString();
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
            string strDocName = ((HyperLink)e.Item.Cells[3].Controls[0]).Text.Trim();
            string strUploadMan = e.Item.Cells[5].Text.Trim();
            string strProjectID = LB_ProjectID.Text.Trim();
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

                    LoadDocument(strUserCode);
                    ShareClass.InitialDocTypeTree(TreeView1, strUserCode, "Other", "0", "0");

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZFFCZNBNSCBRSCDWJ") + "');</script>");
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

                TB_WLName.Text = LanguageHandle.GetWord("PingShen") + LanguageHandle.GetWord("WenJian")  + strDocID  + strDocName;

                BT_SubmitApply.Enabled = true;

                LoadRelatedWL("DocumentReview", "Document", int.Parse(strDocID));
            }
        }
    }

    protected void BtnUP_Click(object sender, EventArgs e)
    {
        if (AttachFile.HasFile)
        {
            string strUserCode = LB_UserCode.Text.Trim();
            string strProjectID = LB_ProjectID.Text.Trim();
            string strAuthor = TB_Author.Text.Trim();
            string strDocTypeID = DL_DocType.SelectedValue.Trim();
            if (strDocTypeID == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGWDLXBNWKJC") + "');</script>");
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

            if (fi.Exists)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC") + "');</script>");
            }
            else
            {
                DocumentBLL documentBLL = new DocumentBLL();
                Document document = new Document();

                document.RelatedType = "Other";
                document.DocTypeID = int.Parse(strDocTypeID);
                document.DocType = strDocType;
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


                    LoadDocument(strUserCode);
                    ShareClass.InitialDocTypeTree(TreeView1, strUserCode, "Other", "0", "0");


                    TB_Message.Text = GetUserName(strUserCode) + LanguageHandle.GetWord("ShangChuanLeWenJian") + strFileName2 + LanguageHandle.GetWord("QingJiShiChaKan");
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "');</script>");
                }

            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZZYSCDWJ") + "');</script>");
        }
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strSubject, strMsg, strGroupUserCode;
        string strHQL, strUserCode;
        IList lst;
        int i = 0;

        string strVisible = DL_Visible.SelectedValue.Trim();

        strMsg = TB_Message.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        Msg msg = new Msg();

        if (CB_SMS.Checked == true | CB_Mail.Checked == true)
        {
            ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
            ActorGroupDetail actorGroupDetail = new ActorGroupDetail();
            strHQL = "from ActorGroupDetail as actorGroupDetail where actorGroupDetail.GroupName = " + "'" + strVisible + "'";
            lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);
            strSubject = LanguageHandle.GetWord("WenJianChaYueTongZhi");

            for (i = 0; i < lst.Count; i++)
            {
                actorGroupDetail = (ActorGroupDetail)lst[i];
                strGroupUserCode = actorGroupDetail.UserCode.Trim();

                if (CB_SMS.Checked == true)
                {
                    msg.SendMSM("Message",strGroupUserCode, strMsg, strUserCode);
                }

                if (CB_Mail.Checked == true)
                {
                    msg.SendMail(strGroupUserCode, strSubject, strMsg, strUserCode);
                }
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSWB") + "')", true);
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DocumentBLL documentBLL = new DocumentBLL();
        IList lst = documentBLL.GetAllDocuments(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected string SubmitApply()
    {
        string strWLName, strWLType, strTemName, strXMLFileName, strXMLFile1, strXMLFile2;
        string strDescription, strCreatorCode, strCreatorName;
        string strCmdText, strDocID;
        DateTime dtCreateTime;

        string strWLID, strUserCode;

        strWLID = "0";

        strUserCode = LB_UserCode.Text.Trim();

        strDocID = LB_DocID.Text.Trim();

        XMLProcess xmlProcess = new XMLProcess();

        strWLName = TB_WLName.Text.Trim();
        strWLType = DL_WFType.SelectedValue.Trim();
        strTemName = DL_TemName.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strCreatorCode = LB_UserCode.Text.Trim();
        strCreatorName = GetUserName(strCreatorCode);
        dtCreateTime = DateTime.Now;

        if (strTemName == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSSCSBLCMBBNWKJC") + "');</script>");
            return "0";
        }

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

            strWLID = ShareClass.GetMyCreatedWorkFlowID(strUserCode);

            strCmdText = "select * from T_Document where DocID = " + strDocID;
            strXMLFile2 = Server.MapPath(strXMLFile2);

            xmlProcess.DbToXML(strCmdText, "T_Document", strXMLFile2);

            LoadRelatedWL("DocumentReview", "Document", int.Parse(strDocID));

            //自动附加要评审的工作流文件
            AddWLDocument(strDocID, int.Parse(strWLID));

            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZWJPSSSCCG") + "');</script>");
        }
        catch
        {
            strWLID = "0";
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZWJPSSSCSB") + "');</script>");
        }

        return strWLID;
    }

    protected void BT_ActiveYes_Click(object sender, EventArgs e)
    {
        string strWLID = SubmitApply();

        if (strWLID != "0")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "popShowByURL('TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID + "','workflow','99%','99%',window.location);", true);
        }
    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        SubmitApply();
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

    protected void BT_Refrash_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'DocumentReview'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();
    }

    protected void AddWLDocument(string strDocID, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Document as document where document.DocID = " + strDocID;
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);

        Document document = (Document)lst[0];

        document.RelatedType = "Workflow";
        document.RelatedID = intRelatedID;
        document.RelatedName = "";


        try
        {
            documentBLL.AddDocument(document);
        }
        catch
        {
        }
    }


    protected void LoadDocument(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartCode = GetDepartCode(strUserCode);

        strHQL = "from Document as document where document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + " and document.Status <> 'Deleted'  Order by document.DocID DESC";
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();
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

    protected string GetDocTypeName(string strDocTypeID)
    {
        DocTypeBLL docTypeBLL = new DocTypeBLL();

        string strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        DocType docType = (DocType)lst[0];

        return docType.Type.Trim();
    }

    protected string GetDepartCode(string strUserCode)
    {
        string strDepartCode, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strDepartCode = projectMember.DepartCode;
        return strDepartCode;
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


}
