using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using ProjectMgt.BLL;
using ProjectMgt.Model;


public partial class TTUpLoadDoc : System.Web.UI.Page
{
    string strUserCode, strUserName, strDepartCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        //ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "员工档案设置", strUserCode);
        //if (blVisible == false)
        //{
        //    Response.Redirect("TTDisplayErrors.aspx");
        //    return;
        //}

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandlerForSpecialPopWindow();", true);
        if (Page.IsPostBack == false)
        {
            ShareClass.InitialDocTypeTree(TreeView1, strUserCode, LanguageHandle.GetWord("ZhiShiGuanLi").ToString().Trim(), "0", "0");
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY").ToString().Trim();

            lbl_Number.Text = (64 - strUserCode.Trim().Length).ToString();

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);

            LoadDocument(strUserCode, "");

            ShareClass.InitialUserDocTypeTree(TreeView3, strUserCode);

            ShareClass.LoadActorGroupDropDownList(DL_Visible, strUserCode);

            strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'DocumentReview'";
            strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
            WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
            lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
            DL_TemName.DataSource = lst;
            DL_TemName.DataBind();

            TB_Author.Text = strUserName;
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
                strHQL += " and document.RelatedType = 'KnowledgeManagement'";   //ChineseWord
                strHQL += " and document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'";
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
            strHQL += " document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'";
            strHQL += " and document.RelatedType = 'KnowledgeManagement'";   //ChineseWord
            strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY").ToString().Trim();
        }

        lst2 = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst2;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst2.Count.ToString();


        //根据文档有无工作流情况隐藏删除按钮
        ShareClass.HideDataGridDeleteButtonForDocUploadPage(DataGrid1);
    }

    protected void BT_LoadDoc_Click(object sender, EventArgs e)
    {
        LoadDocument(Session["UserCode"].ToString().Trim(), "");
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


                strHQL = "from Document as document where document.DocID = " + strDocID;
                DocumentBLL documentBLL = new DocumentBLL();
                lst = documentBLL.GetAllDocuments(strHQL);
                Document document = (Document)lst[0];

                LB_DocID.Text = document.DocID.ToString();
                LB_DocTypeID.Text = document.DocTypeID.ToString();
                TB_DocType.Text = document.DocType;
                TB_Author.Text = document.Author;

                try
                {
                    DL_Visible.SelectedValue = document.Visible.Trim();
                }
                catch
                {
                }

                //显示关联公司名称
                GetDocRelatedDepartment(strDocID);

                TB_WLName.Text = LanguageHandle.GetWord("PingShen").ToString().Trim() + LanguageHandle.GetWord("WenJian").ToString().Trim() + strDocID + strDocName;


                BT_SubmitApply.Enabled = true;

                LoadRelatedWL("DocumentReview", "Document", int.Parse(strDocID));

                BT_EditSave.Enabled = true;
                BT_SubmitApply.Enabled = true;
            }

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

                    LoadDocument(strUserCode, LB_Sql.Text.Trim());
                    ShareClass.InitialDocTypeTree(TreeView1, strUserCode, LanguageHandle.GetWord("ZhiShiGuanLi").ToString().Trim(), "0", "0");

                    LB_DocID.Text = "";

                    BT_EditSave.Enabled = false;
                    BT_SubmitApply.Enabled = false;
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZFFCZNBNSCBRSCDWJ").ToString().Trim() + "');</script>");
                }
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

        LB_RelatedDepartCode.Visible = true;
        LB_RelatedDepartCode.Text = strDepartCode;
        TB_RelatedDepartName.Text = strDepartName;
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDocTypeID = TreeView3.SelectedNode.Target;
        string strDocType = GetDocTypeName(strDocTypeID);

        LB_DocTypeID.Text = strDocTypeID;
        TB_DocType.Text = strDocType;
    }

    protected void BtnUP_Click(object sender, EventArgs e)
    {
        if (AttachFile.HasFile)
        {
            string strHQL;

            string strUserCode = LB_UserCode.Text.Trim();
            string strProjectID = LB_ProjectID.Text.Trim();
            string strAuthor = TB_Author.Text.Trim();

            string strDocTypeID = LB_DocTypeID.Text;
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
                }
            }

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
                //    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGBNSCDYSZDWJ").ToString().Trim() + "');</script>");
                //    return;
                //}
            }

            if (fi.Exists)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "');</script>");
            }
            else
            {
                DocumentBLL documentBLL = new DocumentBLL();
                Document document = new Document();

                document.RelatedType = LanguageHandle.GetWord("ZhiShiGuanLi").ToString().Trim();
                document.RelatedID = 0;
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
                document.RelatedName = "";

                try
                {
                    documentBLL.AddDocument(document);

                    strDocID = ShareClass.GetMyCreatedMaxDocID(strUserCode);

                    LB_DocID.Text = strDocID;

                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);

                    LoadDocument(strUserCode, LB_Sql.Text.Trim());
                    ShareClass.InitialDocTypeTree(TreeView1, strUserCode, LanguageHandle.GetWord("ZhiShiGuanLi").ToString().Trim(), "0", "0");

                    if (strVisible == LanguageHandle.GetWord("GongSi").ToString().Trim())
                    {
                        strHQL = "Insert Into T_DocRelatedDepartment(DocID,DepartCode,DepartName) Values(" + strDocID + "," + "'" + strRelatedDepartCode + "'" + "," + "'" + strRelatedDepartName + "'" + ")";
                        ShareClass.RunSqlCommand(strHQL);
                    }


                    BT_SubmitApply.Enabled = true;

                    TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("ShangChuanLeWenJian").ToString().Trim() + strFileName2 + LanguageHandle.GetWord("QingJiShiChaKan").ToString().Trim();

                    TB_WLName.Text = LanguageHandle.GetWord("PingShen").ToString().Trim() + LanguageHandle.GetWord("WenJian").ToString().Trim() + strDocID + strFileName2;
                    LoadRelatedWL("DocumentReview", "Document", int.Parse(strDocID));
                }
                catch (Exception err)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + err.Message.ToString() + "');</script>");

                    //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "');</script>");
                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "');</script>");
        }
    }

    protected void BT_RelatedUser_Click(object sender, EventArgs e)
    {
        string strDocID, strIMTitle;
        string strJavaScriptFuntion;

        strDocID = LB_DocID.Text.Trim();

        strIMTitle = LanguageHandle.GetWord("WenDang").ToString().Trim() + strDocID + LanguageHandle.GetWord("KeShiYongHu").ToString().Trim();

        strJavaScriptFuntion = "popShowByURL('TTDocRelatedUser.aspx?DocID=" + strDocID + "', '" + strIMTitle + "', 800, 600,window.location)";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", strJavaScriptFuntion, true);
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
                    msg.SendMSM("Message", strGroupUserCode, strMsg, strUserCode);
                }

                if (CB_Mail.Checked == true)
                {
                    msg.SendMail(strGroupUserCode, strSubject, strMsg, strUserCode);
                }
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSWB").ToString().Trim() + "')", true);
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

            LoadDocument(strUserCode, LB_Sql.Text.Trim());
            ShareClass.InitialUserDocTypeTree(TreeView1, strUserCode);

            BT_SubmitApply.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
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

    protected void BT_SubmitApply_Click(object sender, EventArgs e)
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
            return;
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

            //自动附加要评审的工作流文件
            ShareClass.AddWLDocumentForUploadDocPage(strDocID, int.Parse(strWLID));
            //自动附加其它已选择的要评审的工作流文件
            ShareClass.AddMoreWLSelectedDocumentForUploadDocPage(DataGrid1, int.Parse(strWLID), strDocID);

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
            strWLID = "0";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWJPSSSB").ToString().Trim() + "')", true);
        }
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

    protected void LoadDocument(string strUserCode, string strHQL)
    {
        IList lst;

        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        if (strHQL == "")
        {
            strHQL = "from Document as document where document.UploadManCode = " + "'" + strUserCode + "'";
            //strHQL += " and document.RelatedType = 'KnowledgeManagement'";
            strHQL += " and document.DepartCode = " + "'" + strDepartCode + "'" + " and document.Status <> 'Deleted'";
            strHQL += " Order by document.DocID DESC";
        }

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();


        LB_Sql.Text = strHQL;
        LB_TotalCount.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst.Count.ToString();

        //根据文档有无工作流情况隐藏删除按钮
        ShareClass.HideDataGridDeleteButtonForDocUploadPage(DataGrid1);
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

    protected void GetDocRelatedDepartment(string strDocID)
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
            LB_SelectCompany.Visible = false;
            LB_RelatedDepartCode.Visible = false;
            TB_RelatedDepartName.Visible = false;

            LB_RelatedDepartCode.Text = "";
            TB_RelatedDepartName.Text = "";
        }
    }

}
