using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTSuperUploadDoc : System.Web.UI.Page
{
    string strUserCode, strUserName, strDepartCode;
    string strLangCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;

        IList lst;

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandlerForSpecialPopWindow();", true);
        if (Page.IsPostBack == false)
        {
            ShareClass.InitialUserDocTypeTree(TreeView1, strUserCode);

            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY").ToString().Trim();

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);

            LoadDocument("");

            strHQL = "Select rtrim(GroupName) as GroupName ,rtrim(HomeName) as HomeName from T_ActorGroup where  ";
            strHQL += " LangCode = " + "'" + strLangCode + "'";
            strHQL += " Order by SortNumber ASC";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ActorGroup");
            DL_Visible.DataSource = ds;
            DL_Visible.DataBind();

            strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'DocumentReview'";
            strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
            WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
            lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
            DL_TemName.DataSource = lst;
            DL_TemName.DataBind();

            TB_Author.Text = strUserName;

            ShareClass.InitialAllDocTypeTree(TreeView3);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDocTypeID, strHQL, strUserCode, strDepartCode, strDocType;
        IList lst1, lst2;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        strDocTypeID = treeNode.Target.Trim();

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        lst1 = docTypeBLL.GetAllDocTypes(strHQL);

        DocumentBLL documentBLL = new DocumentBLL();

        if (strDocTypeID != "0")
        {
            if (lst1.Count > 0)
            {
                DocType docType = (DocType)lst1[0];
                strDocType = docType.Type.Trim();

                LB_DocTypeID.Text = docType.ID.ToString();
                TB_DocType.Text = docType.Type.Trim();

                strHQL = " from Document as document where document.DocTypeID = " + strDocTypeID;
                strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";
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

            strHQL = " from Document as document where ";
            strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY").ToString().Trim();
        }

        lst2 = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst2;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst2.Count.ToString();
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDocTypeID = TreeView3.SelectedNode.Target;
        string strDocType = GetDocTypeName(strDocTypeID);

        LB_DocTypeID.Text = strDocTypeID;
        TB_DocType.Text = strDocType;
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text.Trim();
            string strUserName = ShareClass.GetUserName(strUserCode);
            string strDocID = e.Item.Cells[0].Text.Trim();
           string strDocName = e.Item.Cells[3].Text.Trim();
            string strUploadMan = e.Item.Cells[6].Text.Trim();
            string strProjectID = LB_ProjectID.Text.Trim();
            string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

            if (e.CommandName == "Delete")
            {
                strHQL = "from Document as document where document.DocID = " + strDocID;
                DocumentBLL documentBLL = new DocumentBLL();
                lst = documentBLL.GetAllDocuments(strHQL);
                Document document = (Document)lst[0];

                document.Status = "Deleted";
                documentBLL.UpdateDocument(document, int.Parse(strDocID));

                LoadDocument(LB_Sql.Text.Trim());
                ShareClass.InitialUserDocTypeTree(TreeView1, strUserCode);

                LB_DocID.Text = "";

                BT_EditSave.Enabled = false;
                BT_SubmitApply.Enabled = false;
            }

            else if (e.CommandName == "Review")
            {
                LB_DocID.Text = strDocID;

                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;


                strHQL = "from Document as document where document.DocID = " + strDocID;
                DocumentBLL documentBLL = new DocumentBLL();
                lst = documentBLL.GetAllDocuments(strHQL);
                Document document = (Document)lst[0];

                LB_DocID.Text = document.DocID.ToString();
                LB_DocTypeID.Text = document.DocTypeID.ToString();
                TB_DocType.Text = document.DocType;

                DL_Visible.SelectedValue = document.Visible.Trim();

                TB_Author.Text = document.Author;

                //显示关联公司名称
                GetDocRelatedDepartment(strDocID, document.Visible.Trim());

                TB_WLName.Text = LanguageHandle.GetWord("PingShen").ToString().Trim() + LanguageHandle.GetWord("WenJian").ToString().Trim()  + strDocID  + strDocName ;

                BT_EditSave.Enabled = true;
                BT_SubmitApply.Enabled = true;

                LoadRelatedWL("DocumentReview", "Document", int.Parse(strDocID));

                BT_EditSave.Enabled = true;
                BT_SubmitApply.Enabled = true;
            }

        }
    }

    protected void DL_Visible_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType;

        strType = DL_Visible.SelectedValue.Trim();

        if (strType == LanguageHandle.GetWord("GongSi").ToString().Trim())
        {
            LB_SelectCompany.Visible = true;
            LB_RelatedDepartCode.Visible = true;
            TB_RelatedDepartName.Visible = true;
        }
        else
        {
            LB_SelectCompany.Visible = false;
            LB_RelatedDepartCode.Visible = false;
            TB_RelatedDepartName.Visible = false;
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        strDepartCode = TreeView2.SelectedNode.Target.Trim();
        strDepartName = ShareClass.GetDepartName(strDepartCode);

        LB_RelatedDepartCode.Text = strDepartCode;
        TB_RelatedDepartName.Text = strDepartName;
    }

    protected void BtnUP_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strUserCode = LB_UserCode.Text.Trim();
        string strProjectID = LB_ProjectID.Text.Trim();
        string strAuthor = TB_Author.Text.Trim();


        try
        {
            string strDocTypeID = LB_DocTypeID.Text.Trim();
            if (strDocTypeID == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGWDLXBNWKJC").ToString().Trim() + "');</script>");
                return;
            }
            string strDocType = GetDocTypeName(strDocTypeID);

            string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

            string strVisible = DL_Visible.SelectedValue.Trim();

            string strRelatedDepartCode = LB_RelatedDepartCode.Text.Trim();
            string strRelatedDepartName = TB_RelatedDepartName.Text.Trim();

            string strDocID;
            string strFileName1, strExtendName;

            if (strVisible == LanguageHandle.GetWord("GongSi").ToString().Trim())
            {
                if (strRelatedDepartCode == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSBXWGSDBXZGLGSJC").ToString().Trim() + "');</script>");
                    return;
                }
            }

            DocumentBLL documentBLL = new DocumentBLL();

            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFile file = Request.Files[i];
                    if (file.ContentLength > 0)
                    {
                        strFileName1 = file.FileName;//获取上传文件的文件名,包括后缀

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

                            Document document = new Document();

                            document.RelatedType = "知识管理"; 
                            document.DocTypeID = int.Parse(strDocTypeID);
                            document.DocType = strDocType;
                            document.Author = strAuthor;
                            document.DocName = strFileName2;
                            document.Address = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                            document.UploadManCode = strUserCode;
                            document.UploadManName = ShareClass.GetUserName(strUserCode);
                            document.UploadTime = DateTime.Now;
                            document.Visible = strVisible;
                            document.DepartCode = strDepartCode;
                            document.DepartName = ShareClass.GetDepartName(strDepartCode);
                            document.Status = "InProgress";
                            document.RelatedName = "";


                            documentBLL.AddDocument(document);

                            strDocID = ShareClass.GetMyCreatedMaxDocID(strUserCode);

                            LB_DocID.Text = strDocID;

                            //AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                            string strResult = FileUpLoad(file, strDocSavePath + strFileName3);

                            LoadDocument(LB_Sql.Text.Trim());
                            //ShareClass.InitialDocTypeTree(TreeView1, strUserCode, "知识管理", "0", "0");
                            ShareClass.InitialUserDocTypeTree(TreeView1, strUserCode);

                            if (strVisible == LanguageHandle.GetWord("GongSi").ToString().Trim())
                            {
                                strHQL = "Insert Into T_DocRelatedDepartment(DocID,DepartCode,DepartName) Values(" + strDocID + "," + "'" + strRelatedDepartCode + "'" + "," + "'" + strRelatedDepartName + "'" + ")";

                                ShareClass.RunSqlCommand(strHQL);
                            }


                            BT_SubmitApply.Enabled = true;

                            TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("ShangChuanLeWenJian").ToString().Trim() + strFileName2 + LanguageHandle.GetWord("QingJiShiChaKan").ToString().Trim();

                            TB_WLName.Text = LanguageHandle.GetWord("PingShen").ToString().Trim() + LanguageHandle.GetWord("WenJian").ToString().Trim() + strDocID  + strFileName2 ;
                            LoadRelatedWL("DocumentReview", "Document", int.Parse(strDocID));
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGXZYBLXSZZWDLX").ToString().Trim() + "')", true);

            return;
        }
    }


    protected void BT_EditSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strHQL;
            string strDocTypeID = LB_DocTypeID.Text.Trim();
            if (strDocTypeID == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGWDLXBNWKJC").ToString().Trim() + "');</script>");
                return;
            }
            string strDocType = GetDocTypeName(strDocTypeID);
            string strDocID = LB_DocID.Text.Trim();
            int intDocID = 0;
            int.TryParse(strDocID, out intDocID);
            string strVisible = DL_Visible.SelectedValue.Trim();

            strHQL = string.Format(@"update T_Document set DocTypeID = {0},DocType='{1}',Visible='{2}' where DocID={3}", int.Parse(strDocTypeID), strDocType, strVisible, intDocID);
            ShareClass.RunSqlCommand(strHQL);

            if (strVisible == LanguageHandle.GetWord("GongSi").ToString().Trim())
            {
                string strRelatedDepartCode = LB_RelatedDepartCode.Text.Trim();
                string strRelatedDepartName = ShareClass.GetDepartName(strRelatedDepartCode);

                strHQL = "Select * From T_DocRelatedDepartment Where DocID = " + strDocID;
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DocRelatedDepartment");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strHQL = "Delete From T_DocRelatedDepartment  Where DocID = " + strDocID;
                    ShareClass.RunSqlCommand(strHQL);
                }

                strHQL = "Insert Into T_DocRelatedDepartment(DocID,DepartCode,DepartName) Values(" + strDocID + "," + "'" + strRelatedDepartCode + "'" + "," + "'" + strRelatedDepartName + "'" + ")";
                ShareClass.RunSqlCommand(strHQL);
            }
            else
            {
                strHQL = "Delete From T_DocRelatedDepartment  Where DocID = " + strDocID;
                ShareClass.RunSqlCommand(strHQL);
            }

            LoadDocument(LB_Sql.Text.Trim());
            ShareClass.InitialUserDocTypeTree(TreeView1, strUserCode);

            BT_SubmitApply.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
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
            strSubject = LanguageHandle.GetWord("WenJianChaYueTongZhi").ToString().Trim();

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
        strCreatorName = ShareClass.GetUserName(strCreatorCode);
        dtCreateTime = DateTime.Now;

        if (strTemName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSSCSBLCMBBNWKJC").ToString().Trim() + "')", true);
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

            AddWLDocument(strDocID, int.Parse(strWLID));

            //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZWJPSSSCDGZLGLYMJHCGZLS").ToString().Trim()+"');</script>");
        }
        catch
        {
            strWLID = "0";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWJPSSSB").ToString().Trim() + "')", true);
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

    protected void LoadDocument(string strHQL)
    {
        IList lst;

        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        if (strHQL == "")
        {
            strHQL = "from Document as document where   document.Status <> 'Deleted'  Order by document.DocID DESC";
        }

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst.Count.ToString();
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

    protected void GetDocRelatedDepartment(string strDocID, string strVisible)
    {
        string strHQL;
        IList lst;

        strHQL = "From DocRelatedDepartment as docRelatedDepartment Where DocID = " + strDocID;
        DocRelatedDepartmentBLL docRelatedDepartmentBLL = new DocRelatedDepartmentBLL();
        lst = docRelatedDepartmentBLL.GetAllDocRelatedDepartments(strHQL);

        if (lst.Count > 0)
        {
            DocRelatedDepartment docRelatedDepartment = (DocRelatedDepartment)lst[0];

            LB_SelectCompany.Visible = true;
            LB_RelatedDepartCode.Visible = true;
            TB_RelatedDepartName.Visible = true;

            LB_RelatedDepartCode.Text = docRelatedDepartment.DepartCode.Trim();
            TB_RelatedDepartName.Text = docRelatedDepartment.DepartName.Trim();
        }
        else
        {
            if (strVisible == LanguageHandle.GetWord("GongSi").ToString().Trim())
            {
                LB_SelectCompany.Visible = true;
                LB_RelatedDepartCode.Visible = true;
                TB_RelatedDepartName.Visible = true;

                LB_RelatedDepartCode.Text = "";
                TB_RelatedDepartName.Text = "";
            }
            else
            {
                LB_SelectCompany.Visible = false;
                LB_RelatedDepartCode.Visible = false;
                TB_RelatedDepartName.Visible = false;

                LB_RelatedDepartCode.Text = "";
                TB_RelatedDepartName.Text = "";
            }
        }
    }


    /// <summary>
    ///  重新加载列表
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        LoadDocument("");
    }


    private string FileUpLoad(HttpPostedFile file, string savepath)
    {
        string strResult = string.Empty;
        try
        {
            string fileName, fileExtension;//文件名，文件类型
            fileName = System.IO.Path.GetFileName(file.FileName);
            fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
            int FileLen = file.ContentLength;
            Byte[] FileData = new Byte[FileLen];
            Stream sr = file.InputStream;//创建数据流对象 
            sr.Read(FileData, 0, FileLen);
            sr.Close();
            //return HeadPhotoTemp(FileData, savepath,  fileExtension);

            FileStream fs = new FileStream(savepath, FileMode.OpenOrCreate);
            fs.Write(FileData, 0, FileData.Length);
            fs.Close();

            strResult = "succ";
        }
        catch (Exception ex)
        {
            strResult = LanguageHandle.GetWord("ChuCuoLe").ToString().Trim();
        }
        return strResult;
    }
}
