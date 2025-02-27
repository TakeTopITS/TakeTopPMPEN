using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTProjDocumentControl : System.Web.UI.Page
{
    string strUserCode, strProjectID;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strProjectID = Request.QueryString["ProjectID"];

        //this.Title = LanguageHandle.GetWord("Project").ToString().Trim() + strProjectID + " " + ShareClass.GetProjectName(strProjectID.Trim()) + "的项目文控";

        LB_UserCode.Text = strUserCode.Trim();
        LB_UserName.Text = ShareClass.GetUserName(strUserCode.Trim());
        LB_ReqID.Text = strProjectID.Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DocTypeTree(TreeView1, strUserCode, strProjectID, ShareClass.GetProjectName(strProjectID.Trim()));
            DLC_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_FigureDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_IssueDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_IssuingDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_Creator.Text = strUserCode;
            TB_Creator1.Text = strUserCode;
            TB_Distribution.Text = strUserCode;

            LoadProDocType();
            LoadProGraphRegistrationName();
            LoadProReceiptRegistrationName();
            BindInformation();
        }
    }


    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        try
        {
            string strDocTypeID = treeNode.Target.Trim();
            string strDocTypeName = treeNode.Text.Trim();
            string strParentName = treeNode.Parent == null ? "" : treeNode.Parent.Text.Trim();
            if (strDocTypeID.IndexOf("_") > -1)
            {
                strDocTypeID = strDocTypeID.Substring(0, strDocTypeID.IndexOf("_"));
            }
            lbl_ParamaValue.Text = strDocTypeID;

            if (strDocTypeName.Contains(LanguageHandle.GetWord("Project").ToString().Trim()) && strDocTypeName.Contains(LanguageHandle.GetWord("WenKongLieBiao").ToString().Trim()))
            {
                lbl_TreeViewName.Text = "";
                ddl_ParentName.Items.Clear();
                ddl_ParentName.Items.Insert(0, new ListItem("--Select--", "PleaseSelect"));
            }
            else
            {
                if (strDocTypeName.Contains("."))
                {
                    lbl_TreeViewName.Text = strDocTypeName.Substring(strDocTypeName.LastIndexOf(".") + 1);
                    if (lbl_TreeViewName.Text == LanguageHandle.GetWord("FaTuDengJi").ToString().Trim())
                    {
                        lbl_TreeViewName.Text = LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim();

                    }
                    else if (lbl_TreeViewName.Text == LanguageHandle.GetWord("FaWenDengJi").ToString().Trim())
                    {
                        lbl_TreeViewName.Text = LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim();
                    }

                    BindParentNameData(strDocTypeName.Substring(0, strDocTypeName.IndexOf(".")));
                }
                else
                {
                    lbl_TreeViewName.Text = "";
                    ddl_ParentName.Items.Clear();
                    ddl_ParentName.Items.Insert(0, new ListItem("--Select--", "PleaseSelect"));
                }
            }

            GetProDocGraphControlData(strDocTypeName, strParentName, treeNode.Target.Trim());
            BindInformation();
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }


    protected void DataGrid3_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;
        if (e.CommandName != "Page")
        {
            string strDocID = e.Item.Cells[2].Text.Trim();
            strHQL = "from ProGraphRegistration as proGraphRegistration where proGraphRegistration.ID = " + strDocID;
            ProGraphRegistrationBLL proGraphRegistrationBLL = new ProGraphRegistrationBLL();
            lst = proGraphRegistrationBLL.GetAllProGraphRegistrations(strHQL);
            ProGraphRegistration proGraphRegistration = (ProGraphRegistration)lst[0];

            if (e.CommandName == "Update")
            {
                try
                {
                    LB_GraphRegistrationID.Text = proGraphRegistration.ID.ToString();
                    TB_FileNo.Text = proGraphRegistration.FileNo.Trim();
                    TB_FileName.Text = proGraphRegistration.FileName.Trim();
                    TB_FileNum.Amount = proGraphRegistration.FileNum;
                    TB_TableNum.Amount = proGraphRegistration.TableNum;
                    NB_FigureNum.Amount = proGraphRegistration.FigureNum;
                    DLC_FigureDate.Text = proGraphRegistration.FigureDate.ToString("yyyy-MM-dd");
                    TB_Creator.Text = proGraphRegistration.Creator.Trim();
                    ddl_DocType.SelectedValue = proGraphRegistration.DocType.Trim();
                    TB_Remark.Text = string.IsNullOrEmpty(proGraphRegistration.Remark) ? "" : proGraphRegistration.Remark.Trim();
                    TB_GraphNo.Text = string.IsNullOrEmpty(proGraphRegistration.GraphNo) ? "" : proGraphRegistration.GraphNo.Trim();

                    HL_GraphRegistrationFile.NavigateUrl = proGraphRegistration.FilePath;
                    HL_GraphRegistrationFile.Text = proGraphRegistration.FilePath;

                    if (proGraphRegistration.ArchiveIdentification == 1)
                        BT_Active.Enabled = false;
                    else
                        BT_Active.Enabled = true;

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "popShow('popwindowGraphRegistration','true') ", true);
                }
                catch (Exception ex)
                {

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCYYKNS1STXGZXM2CXMFXMBCZ3XMZTLBZMYCXMDZTJC").ToString().Trim() + "')", true);
                }
            }

            if (e.CommandName == "Active")
            {
                if (proGraphRegistration.ArchiveIdentification == 1)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGDAYBGDWXZCGD").ToString().Trim() + "')", true);
                    return;
                }
                proGraphRegistration.ArchiveIdentification = 1;
                proGraphRegistrationBLL.UpdateProGraphRegistration(proGraphRegistration, proGraphRegistration.ID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGDCG").ToString().Trim() + "')", true);
            }

            if (e.CommandName == "Delete")
            {
                if (IsProSendFigureRegistrationExitFileNo(TB_FileNo.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGTYYBFFWFSCJC").ToString().Trim() + "')", true);
                    return;
                }

                strHQL = "Delete From T_ProGraphRegistration Where ID = '" + strDocID + "' ";

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    lbl_ParamaValue.Text = "1";
                    lbl_TreeViewName.Text = ddl_DocType.SelectedValue.Trim();
                    LoadProGraphRegistrationName();
                    BindInformation();

                    LB_GraphRegistrationID.Text = "";


                    BT_Active.Enabled = false;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
                }

            }

            lbl_TreeViewName.Text = proGraphRegistration.DocType.Trim();
        }
        lbl_ParamaValue.Text = "1";
        BindInformation();
    }

    protected void DataGrid3_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = lbl_sql3.Text.Trim();

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProGraphRegistration");
        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }

    protected void BT_CreateGraphRegistration_Click(object sender, EventArgs e)
    {
        LB_GraphRegistrationID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowGraphRegistration','false') ", true);
    }

    protected void BT_NewGraphRegistration_Click(object sender, EventArgs e)
    {
        string strID1;

        strID1 = LB_GraphRegistrationID.Text.Trim();

        if (strID1 == "")
        {
            AddGraphRegistration();
        }
        else
        {
            UpdateGraphRegistration();
        }
    }

    protected void AddGraphRegistration()
    {
        if (string.IsNullOrEmpty(TB_FileNo.Text.Trim()) || TB_FileNo.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDAHBNWKJC").ToString().Trim() + "')", true);
            TB_FileNo.Focus();
            return;
        }
        if (IsProGraphRegistration(TB_FileNo.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDAHZSJBZYCZJC").ToString().Trim() + "')", true);
            TB_FileNo.Focus();
            return;
        }
        string strBookImage;
        string strImage = UploadBookDesign(AttachFile, LanguageHandle.GetWord("ShouTuFuJian").ToString().Trim()).Trim();
        if (strImage.Equals("0"))//附件为空
        {
            strBookImage = "";
        }
        else if (strImage.Equals("1"))//存在同名文件，上传失败，请改名后再上传！
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZTMWJGMHZSC").ToString().Trim() + "')", true);
            return;
        }
        else if (strImage.Equals("2"))//上传"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSCSBJC").ToString().Trim() + "')", true);
            return;
        }
        else
            strBookImage = strImage;

        ProGraphRegistrationBLL proGraphRegistrationBLL = new ProGraphRegistrationBLL();
        ProGraphRegistration proGraphRegistration = new ProGraphRegistration();

        proGraphRegistration.Creator = TB_Creator.Text.Trim();
        proGraphRegistration.FigureDate = DateTime.Parse(string.IsNullOrEmpty(DLC_FigureDate.Text.Trim()) ? DateTime.Now.ToString() : DLC_FigureDate.Text.Trim());
        proGraphRegistration.FigureNum = int.Parse(NB_FigureNum.Amount.ToString());
        proGraphRegistration.FileName = TB_FileName.Text.Trim();
        proGraphRegistration.FileNo = TB_FileNo.Text.Trim();
        proGraphRegistration.FileNum = int.Parse(TB_FileNum.Amount.ToString());
        proGraphRegistration.FilePath = strImage;
        proGraphRegistration.ProjectID = int.Parse(string.IsNullOrEmpty(LB_ReqID.Text.Trim()) || LB_ReqID.Text.Trim() == "" ? "0" : LB_ReqID.Text.Trim());
        proGraphRegistration.TableNum = int.Parse(TB_TableNum.Amount.ToString());
        proGraphRegistration.ArchiveIdentification = 0;
        proGraphRegistration.DocType = ddl_DocType.SelectedValue.Trim();
        proGraphRegistration.GraphNo = TB_GraphNo.Text.Trim();
        proGraphRegistration.Remark = TB_Remark.Text.Trim();

        try
        {
            proGraphRegistrationBLL.AddProGraphRegistration(proGraphRegistration);

            LB_GraphRegistrationID.Text = GetProGraphRegistrationNowID().ToString();
            lbl_ParamaValue.Text = "1";
            lbl_TreeViewName.Text = ddl_DocType.SelectedValue.Trim();
            LoadProGraphRegistrationName();

            BindInformation();


            BT_Active.Enabled = true;


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void UpdateGraphRegistration()
    {
        if (string.IsNullOrEmpty(TB_FileNo.Text.Trim()) || TB_FileNo.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDAHBNWKJC").ToString().Trim() + "')", true);
            TB_FileNo.Focus();
            return;
        }
        if (IsProGraphRegistration(TB_FileNo.Text.Trim(), LB_GraphRegistrationID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDAHZSJBZYCZJC").ToString().Trim() + "')", true);
            TB_FileNo.Focus();
            return;
        }
        string strHQL = "from ProGraphRegistration as proGraphRegistration where proGraphRegistration.ID = '" + LB_GraphRegistrationID.Text.Trim() + "' ";
        ProGraphRegistrationBLL proGraphRegistrationBLL = new ProGraphRegistrationBLL();
        IList lst = proGraphRegistrationBLL.GetAllProGraphRegistrations(strHQL);
        ProGraphRegistration proGraphRegistration = (ProGraphRegistration)lst[0];
        string strFileNoOld = proGraphRegistration.FileNo.Trim();
        string strDocTypeOld = proGraphRegistration.DocType.Trim();
        string strImage = UploadBookDesign(AttachFile, LanguageHandle.GetWord("ShouTuFuJian").ToString().Trim()).Trim();
        if (strImage.Equals("0"))//附件为空
        {
        }
        else if (strImage.Equals("1"))//存在同名文件，上传失败，请改名后再上传！
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZTMWJGMHZSC").ToString().Trim() + "')", true);
            return;
        }
        else if (strImage.Equals("2"))//上传"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSCSBJC").ToString().Trim() + "')", true);
            return;
        }
        else
            proGraphRegistration.FilePath = strImage;

        proGraphRegistration.Creator = TB_Creator.Text.Trim();
        proGraphRegistration.FigureDate = DateTime.Parse(string.IsNullOrEmpty(DLC_FigureDate.Text.Trim()) ? DateTime.Now.ToString() : DLC_FigureDate.Text.Trim());
        proGraphRegistration.FigureNum = int.Parse(NB_FigureNum.Amount.ToString());
        proGraphRegistration.FileName = TB_FileName.Text.Trim();
        proGraphRegistration.FileNo = TB_FileNo.Text.Trim();
        proGraphRegistration.FileNum = int.Parse(TB_FileNum.Amount.ToString());
        proGraphRegistration.ProjectID = int.Parse(string.IsNullOrEmpty(LB_ReqID.Text.Trim()) || LB_ReqID.Text.Trim() == "" ? "0" : LB_ReqID.Text.Trim());
        proGraphRegistration.TableNum = int.Parse(TB_TableNum.Amount.ToString());
        proGraphRegistration.DocType = ddl_DocType.SelectedValue.Trim();
        proGraphRegistration.GraphNo = TB_GraphNo.Text.Trim();
        proGraphRegistration.Remark = TB_Remark.Text.Trim();

        try
        {
            proGraphRegistrationBLL.UpdateProGraphRegistration(proGraphRegistration, proGraphRegistration.ID);
            if (!strFileNoOld.Equals(TB_FileNo.Text.Trim()) || !strDocTypeOld.Equals(ddl_DocType.SelectedValue.Trim()))
            {
                UpdateProSendFigureRegistrationByFileNo(strFileNoOld, TB_FileNo.Text.Trim(), ddl_DocType.SelectedValue.Trim());
            }
            lbl_ParamaValue.Text = "1";
            lbl_TreeViewName.Text = ddl_DocType.SelectedValue.Trim();
            LoadProGraphRegistrationName();
            BindInformation();

            BT_Active.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Active_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TB_FileNo.Text.Trim()) || TB_FileNo.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDAHBNWKJC").ToString().Trim() + "')", true);
            TB_FileNo.Focus();
            return;
        }
        if (IsProGraphRegistration(TB_FileNo.Text.Trim(), LB_GraphRegistrationID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDAHZSJBZYCZJC").ToString().Trim() + "')", true);
            TB_FileNo.Focus();
            return;
        }
        string strHQL = "from ProGraphRegistration as proGraphRegistration where proGraphRegistration.ID = '" + LB_GraphRegistrationID.Text.Trim() + "' ";
        ProGraphRegistrationBLL proGraphRegistrationBLL = new ProGraphRegistrationBLL();
        IList lst = proGraphRegistrationBLL.GetAllProGraphRegistrations(strHQL);
        ProGraphRegistration proGraphRegistration = (ProGraphRegistration)lst[0];
        if (proGraphRegistration.ArchiveIdentification == 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGDAYGDJC").ToString().Trim() + "')", true);
            return;
        }

        string strFileNoOld = proGraphRegistration.FileNo.Trim();
        string strDocTypeOld = proGraphRegistration.DocType.Trim();
        string strImage = UploadBookDesign(AttachFile, LanguageHandle.GetWord("ShouTuFuJian").ToString().Trim()).Trim();
        if (strImage.Equals("0"))//附件为空
        {
        }
        else if (strImage.Equals("1"))//存在同名文件，上传失败，请改名后再上传！
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZTMWJGMHZSC").ToString().Trim() + "')", true);
            return;
        }
        else if (strImage.Equals("2"))//上传"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSCSBJC").ToString().Trim() + "')", true);
            return;
        }
        else
            proGraphRegistration.FilePath = strImage;

        proGraphRegistration.Creator = TB_Creator.Text.Trim();
        proGraphRegistration.FigureDate = DateTime.Parse(string.IsNullOrEmpty(DLC_FigureDate.Text.Trim()) ? DateTime.Now.ToString() : DLC_FigureDate.Text.Trim());
        proGraphRegistration.FigureNum = int.Parse(NB_FigureNum.Amount.ToString());
        proGraphRegistration.FileName = TB_FileName.Text.Trim();
        proGraphRegistration.FileNo = TB_FileNo.Text.Trim();
        proGraphRegistration.FileNum = int.Parse(TB_FileNum.Amount.ToString());
        proGraphRegistration.ProjectID = int.Parse(string.IsNullOrEmpty(LB_ReqID.Text.Trim()) || LB_ReqID.Text.Trim() == "" ? "0" : LB_ReqID.Text.Trim());
        proGraphRegistration.TableNum = int.Parse(TB_TableNum.Amount.ToString());
        proGraphRegistration.ArchiveIdentification = 1;
        proGraphRegistration.DocType = ddl_DocType.SelectedValue.Trim();
        proGraphRegistration.GraphNo = TB_GraphNo.Text.Trim();
        proGraphRegistration.Remark = TB_Remark.Text.Trim();

        try
        {
            proGraphRegistrationBLL.UpdateProGraphRegistration(proGraphRegistration, proGraphRegistration.ID);
            if (!strFileNoOld.Equals(TB_FileNo.Text.Trim()) || !strDocTypeOld.Equals(ddl_DocType.SelectedValue.Trim()))
            {
                UpdateProSendFigureRegistrationByFileNo(strFileNoOld, TB_FileNo.Text.Trim(), ddl_DocType.SelectedValue.Trim());
            }
            lbl_ParamaValue.Text = "1";
            lbl_TreeViewName.Text = ddl_DocType.SelectedValue.Trim();
            LoadProGraphRegistrationName();
            BindInformation();

            BT_Active.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGDCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZGuiDangLanguageHandleGetWord").ToString().Trim()+"')", true); 
        }
    }

    protected void DataGrid5_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;
        if (e.CommandName != "Page")
        {
            string strDocID = e.Item.Cells[2].Text.Trim();
            strHQL = "from ProSendFigureRegistration as proSendFigureRegistration where proSendFigureRegistration.ID = " + strDocID;
            ProSendFigureRegistrationBLL proSendFigureRegistrationBLL = new ProSendFigureRegistrationBLL();
            lst = proSendFigureRegistrationBLL.GetAllProSendFigureRegistrations(strHQL);
            ProSendFigureRegistration proSendFigureRegistration = (ProSendFigureRegistration)lst[0];

            if (e.CommandName == "Update")
            {
                try
                {
                    LB_SendFigureRegistrationID.Text = proSendFigureRegistration.ID.ToString();
                    ddl_FileNo.SelectedValue = proSendFigureRegistration.FileNo.Trim();
                    TB_Recipients.Text = proSendFigureRegistration.Recipients.Trim();
                    NB_FileNum.Amount = proSendFigureRegistration.FileNum;
                    NB_TableNum.Amount = proSendFigureRegistration.TableNum;
                    NB_FigureNum1.Amount = proSendFigureRegistration.FigureNum;
                    DLC_IssueDate.Text = proSendFigureRegistration.IssueDate.ToString("yyyy-MM-dd");
                    TB_Distribution.Text = proSendFigureRegistration.Distribution.Trim();
                    TB_FigurePlan.Text = proSendFigureRegistration.FigurePlan.Trim();
                    lbl_DocType.Text = proSendFigureRegistration.DocType.Trim();
                    lbl_FilePath.Visible = false;
                    lbl_FilePath.Text = proSendFigureRegistration.FilePath.Trim();
                    RP_SendGraph.DataSource = lst;
                    RP_SendGraph.DataBind();

                    GetGraphNums(proSendFigureRegistration.FileNo.Trim(), strDocID);

                    if (proSendFigureRegistration.BackPer.Trim() == "" || string.IsNullOrEmpty(proSendFigureRegistration.BackPer))
                    {
                        ddl_IsBack.SelectedValue = "NO";
                    }
                    else
                        ddl_IsBack.SelectedValue = "YES";


                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowSendFigureRegistration','false') ", true);
                }
                catch (Exception ex)
                {

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCYYKNS1STXGZXM2CXMFXMBCZ3XMZTLBZMYCXMDZTJC").ToString().Trim() + "')", true);
                }
            }

            if (e.CommandName == "Delete")
            {
                strHQL = "Delete From T_ProSendFigureRegistration Where ID = '" + strDocID + "' ";

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    lbl_TreeViewName.Text = lbl_DocType.Text.Trim();
                    lbl_ParamaValue.Text = "2";
                    BindInformation();

                    LB_SendFigureRegistrationID.Text = "";


                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
                }

            }

            if (e.CommandName == "Review")
            {
                if (!string.IsNullOrEmpty(proSendFigureRegistration.BackPer) && proSendFigureRegistration.BackPer.Trim() != "")
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGTZYBHSWXZCHS").ToString().Trim() + "')", true);
                    return;
                }
                proSendFigureRegistration.BackPer = strUserCode.Trim();
                proSendFigureRegistration.BackTime = DateTime.Now;
                proSendFigureRegistrationBLL.UpdateProSendFigureRegistration(proSendFigureRegistration, proSendFigureRegistration.ID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHSCG").ToString().Trim() + "')", true);
            }
            lbl_TreeViewName.Text = proSendFigureRegistration.DocType.Trim();
        }
        lbl_ParamaValue.Text = "2";
        BindInformation();
    }

    protected void DataGrid5_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = lbl_sql4.Text.Trim();

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProSendFigureRegistration");
        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();
    }

    protected void BT_CreateSendFigureRegistration_Click(object sender, EventArgs e)
    {
        LB_SendFigureRegistrationID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowSendFigureRegistration','false') ", true);
    }

    protected void BT_NewSendFigureRegistration_Click(object sender, EventArgs e)
    {
        string strID1;

        strID1 = LB_SendFigureRegistrationID.Text.Trim();

        if (strID1 == "")
        {
            AddSendFigureRegistration();
        }
        else
        {
            UpdateSendFigureRegistration();
        }
    }


    protected void AddSendFigureRegistration()
    {
        if (string.IsNullOrEmpty(ddl_FileNo.SelectedValue.Trim()) || ddl_FileNo.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDAMCWBXJC").ToString().Trim() + "')", true);
            ddl_FileNo.Focus();
            return;
        }
        if (int.Parse(lbl_FileOld.Text.Trim()) <= 0 || int.Parse(lbl_TableOld.Text.Trim()) <= 0 || int.Parse(lbl_FigureOld.Text.Trim()) <= 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGDAYBFFJC").ToString().Trim() + "')", true);
            ddl_FileNo.Focus();
            return;
        }
        if (int.Parse(NB_FileNum.Amount.ToString()) > int.Parse(lbl_FileOld.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGDAFSYCCKYSLJC").ToString().Trim() + "')", true);
            NB_FileNum.Focus();
            return;
        }
        if (int.Parse(NB_TableNum.Amount.ToString()) > int.Parse(lbl_TableOld.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGDAMFBSYCCKYSLJC").ToString().Trim() + "')", true);
            NB_TableNum.Focus();
            return;
        }
        if (int.Parse(NB_FigureNum1.Amount.ToString()) > int.Parse(lbl_FigureOld.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGDAMFTSYCCKYSLJC").ToString().Trim() + "')", true);
            NB_FigureNum1.Focus();
            return;
        }

        ProSendFigureRegistrationBLL proSendFigureRegistrationBLL = new ProSendFigureRegistrationBLL();
        ProSendFigureRegistration proSendFigureRegistration = new ProSendFigureRegistration();

        proSendFigureRegistration.Distribution = TB_Distribution.Text.Trim();
        proSendFigureRegistration.IssueDate = DateTime.Parse(string.IsNullOrEmpty(DLC_IssueDate.Text.Trim()) ? DateTime.Now.ToString() : DLC_IssueDate.Text.Trim());
        proSendFigureRegistration.FigureNum = int.Parse(NB_FigureNum1.Amount.ToString());
        proSendFigureRegistration.FigurePlan = TB_FigurePlan.Text.Trim();
        proSendFigureRegistration.FileName = ddl_FileNo.SelectedItem.Text;
        proSendFigureRegistration.FileNo = ddl_FileNo.SelectedValue.Trim();
        proSendFigureRegistration.FileNum = int.Parse(NB_FileNum.Amount.ToString());
        proSendFigureRegistration.FilePath = lbl_FilePath.Text.Trim();
        proSendFigureRegistration.TableNum = int.Parse(NB_TableNum.Amount.ToString());
        proSendFigureRegistration.Recipients = TB_Recipients.Text.Trim();
        proSendFigureRegistration.BackTime = DateTime.Now;
        if (ddl_IsBack.SelectedValue.Trim() == "NO")
            proSendFigureRegistration.BackPer = "";
        else
            proSendFigureRegistration.BackPer = strUserCode.Trim();
        proSendFigureRegistration.DocType = lbl_DocType.Text.Trim();

        try
        {
            proSendFigureRegistrationBLL.AddProSendFigureRegistration(proSendFigureRegistration);
            LB_SendFigureRegistrationID.Text = GetProSendFigureRegistrationNowID().ToString();

            lbl_FilePath.Visible = false;

            lbl_ParamaValue.Text = "2";
            lbl_TreeViewName.Text = lbl_DocType.Text.Trim();
            BindInformation();


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void UpdateSendFigureRegistration()
    {
        if (string.IsNullOrEmpty(ddl_FileNo.SelectedValue.Trim()) || ddl_FileNo.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDAMCWBXJC").ToString().Trim() + "')", true);
            ddl_FileNo.Focus();
            return;
        }
        if (int.Parse(lbl_FileOld.Text.Trim()) <= 0 || int.Parse(lbl_TableOld.Text.Trim()) <= 0 || int.Parse(lbl_FigureOld.Text.Trim()) <= 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGDAYBFFJC").ToString().Trim() + "')", true);
            ddl_FileNo.Focus();
            return;
        }
        if (int.Parse(NB_FileNum.Amount.ToString()) > int.Parse(lbl_FileOld.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGDAFSYCCKYSLJC").ToString().Trim() + "')", true);
            NB_FileNum.Focus();
            return;
        }
        if (int.Parse(NB_TableNum.Amount.ToString()) > int.Parse(lbl_TableOld.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGDAMFBSYCCKYSLJC").ToString().Trim() + "')", true);
            NB_TableNum.Focus();
            return;
        }
        if (int.Parse(NB_FigureNum1.Amount.ToString()) > int.Parse(lbl_FigureOld.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGDAMFTSYCCKYSLJC").ToString().Trim() + "')", true);
            NB_FigureNum1.Focus();
            return;
        }

        string strHQL = "from ProSendFigureRegistration as proSendFigureRegistration where proSendFigureRegistration.ID = '" + LB_SendFigureRegistrationID.Text.Trim() + "' ";
        ProSendFigureRegistrationBLL proSendFigureRegistrationBLL = new ProSendFigureRegistrationBLL();
        IList lst = proSendFigureRegistrationBLL.GetAllProSendFigureRegistrations(strHQL);
        ProSendFigureRegistration proSendFigureRegistration = (ProSendFigureRegistration)lst[0];

        proSendFigureRegistration.Distribution = TB_Distribution.Text.Trim();
        proSendFigureRegistration.IssueDate = DateTime.Parse(string.IsNullOrEmpty(DLC_IssueDate.Text.Trim()) ? DateTime.Now.ToString() : DLC_IssueDate.Text.Trim());
        proSendFigureRegistration.FigureNum = int.Parse(NB_FigureNum1.Amount.ToString());
        proSendFigureRegistration.FigurePlan = TB_FigurePlan.Text.Trim();
        proSendFigureRegistration.FileName = ddl_FileNo.SelectedItem.Text;
        proSendFigureRegistration.FileNo = ddl_FileNo.SelectedValue.Trim();
        proSendFigureRegistration.FileNum = int.Parse(NB_FileNum.Amount.ToString());
        proSendFigureRegistration.TableNum = int.Parse(NB_TableNum.Amount.ToString());
        proSendFigureRegistration.Recipients = TB_Recipients.Text.Trim();
        proSendFigureRegistration.BackTime = DateTime.Now;
        if (ddl_IsBack.SelectedValue.Trim() == "NO")
            proSendFigureRegistration.BackPer = "";
        proSendFigureRegistration.DocType = lbl_DocType.Text.Trim();

        try
        {
            proSendFigureRegistrationBLL.UpdateProSendFigureRegistration(proSendFigureRegistration, proSendFigureRegistration.ID);

            strHQL = "from ProSendFigureRegistration as proSendFigureRegistration where proSendFigureRegistration.ID = '" + LB_SendFigureRegistrationID.Text.Trim() + "' ";
            lst = proSendFigureRegistrationBLL.GetAllProSendFigureRegistrations(strHQL);
            lbl_FilePath.Visible = false;
            RP_SendGraph.DataSource = lst;
            RP_SendGraph.DataBind();

            lbl_ParamaValue.Text = "2";
            lbl_TreeViewName.Text = lbl_DocType.Text.Trim();
            BindInformation();


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }


    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;
        if (e.CommandName != "Page")
        {
            string strDocID = e.Item.Cells[2].Text.Trim();
            strHQL = "from ProReceiptRegistration as proReceiptRegistration where proReceiptRegistration.ID = " + strDocID;
            ProReceiptRegistrationBLL proReceiptRegistrationBLL = new ProReceiptRegistrationBLL();
            lst = proReceiptRegistrationBLL.GetAllProReceiptRegistrations(strHQL);
            ProReceiptRegistration proReceiptRegistration = (ProReceiptRegistration)lst[0];

            if (e.CommandName == "Update")
            {
                LB_ReceiptRegistrationID.Text = proReceiptRegistration.ID.ToString();
                TB_DocumentNo.Text = proReceiptRegistration.DocumentNo.Trim();
                TB_FileName1.Text = proReceiptRegistration.FileName.Trim();
                TB_DispatchDepartment.Text = proReceiptRegistration.DispatchDepartment.Trim();
                TB_FileWay.Text = proReceiptRegistration.FileWay.Trim();
                if (string.IsNullOrEmpty(proReceiptRegistration.DestroyPeople) || proReceiptRegistration.DestroyPeople.Trim() == "")
                {
                    ddl_IsDestroy.SelectedValue = "NO";
                }
                else
                    ddl_IsDestroy.SelectedValue = "YES";
                DLC_CreateDate.Text = proReceiptRegistration.CreateDate.ToString("yyyy-MM-dd");
                TB_Creator1.Text = proReceiptRegistration.Creator.Trim();
                ddl_DocType1.SelectedValue = proReceiptRegistration.DocType.Trim();

                HL_ReceiptRegistrationFile.NavigateUrl = proReceiptRegistration.FilePath;
                HL_ReceiptRegistrationFile.Text = proReceiptRegistration.FilePath;


                if (proReceiptRegistration.ArchiveIdentification == 1)
                    Button7.Enabled = false;
                else
                    Button7.Enabled = true;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowReceiptRegistration','false') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strId = LB_ReceiptRegistrationID.Text.Trim();
                if (IsProIssueRegistrationExitDocumentNo(TB_DocumentNo.Text.Trim()))//ProIssueRegistration
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGWJYBFFWFSCJC").ToString().Trim() + "')", true);
                    return;
                }

                strHQL = "Delete From T_ProReceiptRegistration Where ID = '" + strDocID + "' ";

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    lbl_ParamaValue.Text = "3";
                    lbl_TreeViewName.Text = ddl_DocType1.SelectedValue.Trim();
                    LoadProReceiptRegistrationName();
                    BindInformation();


                    Button7.Enabled = false;

                    LB_ReceiptRegistrationID.Text = "";

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
                }
            }

            if (e.CommandName == "Review")
            {
                if (!string.IsNullOrEmpty(proReceiptRegistration.DestroyPeople) && proReceiptRegistration.DestroyPeople.Trim() != "")
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGWJYBXHWXZCXH").ToString().Trim() + "')", true);
                    return;
                }
                proReceiptRegistration.DestroyPeople = strUserCode.Trim();
                proReceiptRegistration.DestructionDate = DateTime.Now;
                proReceiptRegistrationBLL.UpdateProReceiptRegistration(proReceiptRegistration, proReceiptRegistration.ID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXHCG").ToString().Trim() + "')", true);
            }

            if (e.CommandName == "Active")
            {
                if (proReceiptRegistration.ArchiveIdentification == 1)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGWJYBGDWXZCGD").ToString().Trim() + "')", true);
                    return;
                }
                proReceiptRegistration.ArchiveIdentification = 1;
                proReceiptRegistrationBLL.UpdateProReceiptRegistration(proReceiptRegistration, proReceiptRegistration.ID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGDCG").ToString().Trim() + "')", true);
            }
            lbl_TreeViewName.Text = proReceiptRegistration.DocType.Trim();
        }
        lbl_ParamaValue.Text = "3";
        BindInformation();
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        string strHQL = lbl_sql1.Text.Trim();

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProReceiptRegistration");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void BT_CreateReceiptRegistration_Click(object sender, EventArgs e)
    {
        LB_ReceiptRegistrationID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowReceiptRegistration','false') ", true);
    }

    protected void BT_NewReceiptRegistration_Click(object sender, EventArgs e)
    {
        string strID1;

        strID1 = LB_ReceiptRegistrationID.Text.Trim();

        if (strID1 == "")
        {
            AddReceiptRegistration();
        }
        else
        {
            UpdateReceiptRegistration();
        }
    }

    protected void AddReceiptRegistration()
    {
        if (string.IsNullOrEmpty(TB_DocumentNo.Text.Trim()) || TB_DocumentNo.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGWJBHBNWKJC").ToString().Trim() + "')", true);
            TB_DocumentNo.Focus();
            return;
        }
        if (IsProReceiptRegistration(TB_DocumentNo.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGWJBHZSJBZYCZJC").ToString().Trim() + "')", true);
            TB_DocumentNo.Focus();
            return;
        }
        string strBookImage;
        string strImage = UploadBookDesign(AttachDocument, LanguageHandle.GetWord("WenJianFuJian").ToString().Trim()).Trim();
        if (strImage.Equals("0"))//附件为空
        {
            strBookImage = "";
        }
        else if (strImage.Equals("1"))//存在同名文件，上传失败，请改名后再上传！
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZTMWJGMHZSC").ToString().Trim() + "')", true);
            return;
        }
        else if (strImage.Equals("2"))//上传"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSCSBJC").ToString().Trim() + "')", true);
            return;
        }
        else
            strBookImage = strImage;

        ProReceiptRegistrationBLL proReceiptRegistrationBLL = new ProReceiptRegistrationBLL();
        ProReceiptRegistration proReceiptRegistration = new ProReceiptRegistration();

        proReceiptRegistration.Creator = TB_Creator1.Text.Trim();
        proReceiptRegistration.CreateDate = DateTime.Parse(string.IsNullOrEmpty(DLC_CreateDate.Text.Trim()) ? DateTime.Now.ToString() : DLC_CreateDate.Text.Trim());
        proReceiptRegistration.DispatchDepartment = TB_DispatchDepartment.Text.Trim();
        proReceiptRegistration.DocumentNo = TB_DocumentNo.Text.Trim();
        proReceiptRegistration.FileName = TB_FileName1.Text.Trim();
        proReceiptRegistration.FileWay = TB_FileWay.Text.Trim();
        proReceiptRegistration.FilePath = strImage;
        proReceiptRegistration.ProjectID = int.Parse(string.IsNullOrEmpty(LB_ReqID.Text.Trim()) || LB_ReqID.Text.Trim() == "" ? "0" : LB_ReqID.Text.Trim());
        proReceiptRegistration.DestructionDate = DateTime.Now;
        if (ddl_IsDestroy.SelectedValue.Trim() == "NO")
            proReceiptRegistration.DestroyPeople = "";
        else
            proReceiptRegistration.DestroyPeople = strUserCode.Trim();
        proReceiptRegistration.ArchiveIdentification = 0;
        proReceiptRegistration.DocType = ddl_DocType1.SelectedValue.Trim();

        try
        {
            proReceiptRegistrationBLL.AddProReceiptRegistration(proReceiptRegistration);
            LB_ReceiptRegistrationID.Text = GetProReceiptRegistrationNowID().ToString();
            lbl_ParamaValue.Text = "3";
            lbl_TreeViewName.Text = ddl_DocType1.SelectedValue.Trim();
            LoadProReceiptRegistrationName();
            BindInformation();


            Button7.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void UpdateReceiptRegistration()
    {
        if (string.IsNullOrEmpty(TB_DocumentNo.Text.Trim()) || TB_DocumentNo.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGWJBHBNWKJC").ToString().Trim() + "')", true);
            TB_DocumentNo.Focus();
            return;
        }
        if (IsProReceiptRegistration(TB_DocumentNo.Text.Trim(), LB_ReceiptRegistrationID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGWJBHZSJBZYCZJC").ToString().Trim() + "')", true);
            TB_DocumentNo.Focus();
            return;
        }
        string strHQL = "from ProReceiptRegistration as proReceiptRegistration where proReceiptRegistration.ID = '" + LB_ReceiptRegistrationID.Text.Trim() + "' ";
        ProReceiptRegistrationBLL proReceiptRegistrationBLL = new ProReceiptRegistrationBLL();
        IList lst = proReceiptRegistrationBLL.GetAllProReceiptRegistrations(strHQL);
        ProReceiptRegistration proReceiptRegistration = (ProReceiptRegistration)lst[0];
        string strDocumentNoOld = proReceiptRegistration.DocumentNo.Trim();
        string strDocTypeOld = proReceiptRegistration.DocType.Trim();
        string strImage = UploadBookDesign(AttachDocument, LanguageHandle.GetWord("WenJianFuJian").ToString().Trim()).Trim();
        if (strImage.Equals("0"))//附件为空
        {
        }
        else if (strImage.Equals("1"))//存在同名文件，上传失败，请改名后再上传！
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZTMWJGMHZSC").ToString().Trim() + "')", true);
            return;
        }
        else if (strImage.Equals("2"))//上传"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSCSBJC").ToString().Trim() + "')", true);
            return;
        }
        else
            proReceiptRegistration.FilePath = strImage;

        proReceiptRegistration.Creator = TB_Creator1.Text.Trim();
        proReceiptRegistration.CreateDate = DateTime.Parse(string.IsNullOrEmpty(DLC_CreateDate.Text.Trim()) ? DateTime.Now.ToString() : DLC_CreateDate.Text.Trim());
        proReceiptRegistration.DispatchDepartment = TB_DispatchDepartment.Text.Trim();
        proReceiptRegistration.DocumentNo = TB_DocumentNo.Text.Trim();
        proReceiptRegistration.FileName = TB_FileName1.Text.Trim();
        proReceiptRegistration.FileWay = TB_FileWay.Text.Trim();
        proReceiptRegistration.ProjectID = int.Parse(string.IsNullOrEmpty(LB_ReqID.Text.Trim()) || LB_ReqID.Text.Trim() == "" ? "0" : LB_ReqID.Text.Trim());
        proReceiptRegistration.DestructionDate = DateTime.Now;
        if (ddl_IsDestroy.SelectedValue.Trim() == "NO")
            proReceiptRegistration.DestroyPeople = "";
        proReceiptRegistration.DocType = ddl_DocType1.SelectedValue.Trim();
        try
        {
            proReceiptRegistrationBLL.UpdateProReceiptRegistration(proReceiptRegistration, proReceiptRegistration.ID);

            if (!strDocumentNoOld.Equals(TB_DocumentNo.Text.Trim()) || !strDocTypeOld.Equals(ddl_DocType1.SelectedValue.Trim()))
            {
                UpdateProIssueRegistrationByDocumentNo(strDocumentNoOld, TB_DocumentNo.Text.Trim(), ddl_DocType1.SelectedValue.Trim());
            }

            lbl_ParamaValue.Text = "3";
            lbl_TreeViewName.Text = ddl_DocType1.SelectedValue.Trim();
            LoadProReceiptRegistrationName();
            BindInformation();

            Button7.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }


    protected void Button7_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TB_DocumentNo.Text.Trim()) || TB_DocumentNo.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGWJBHBNWKJC").ToString().Trim() + "')", true);
            TB_DocumentNo.Focus();
            return;
        }
        if (IsProReceiptRegistration(TB_DocumentNo.Text.Trim(), LB_ReceiptRegistrationID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGWJBHZSJBZYCZJC").ToString().Trim() + "')", true);
            TB_DocumentNo.Focus();
            return;
        }
        string strHQL = "from ProReceiptRegistration as proReceiptRegistration where proReceiptRegistration.ID = '" + LB_ReceiptRegistrationID.Text.Trim() + "' ";
        ProReceiptRegistrationBLL proReceiptRegistrationBLL = new ProReceiptRegistrationBLL();
        IList lst = proReceiptRegistrationBLL.GetAllProReceiptRegistrations(strHQL);
        ProReceiptRegistration proReceiptRegistration = (ProReceiptRegistration)lst[0];
        if (proReceiptRegistration.ArchiveIdentification == 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGWJYGDJC").ToString().Trim() + "')", true);
            return;
        }
        string strDocumentNoOld = proReceiptRegistration.DocumentNo.Trim();
        string strDocTypeOld = proReceiptRegistration.DocType.Trim();
        string strImage = UploadBookDesign(AttachDocument, LanguageHandle.GetWord("WenJianFuJian").ToString().Trim()).Trim();
        if (strImage.Equals("0"))//附件为空
        {
        }
        else if (strImage.Equals("1"))//存在同名文件，上传失败，请改名后再上传！
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZTMWJGMHZSC").ToString().Trim() + "')", true);
            return;
        }
        else if (strImage.Equals("2"))//上传"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSCSBJC").ToString().Trim() + "')", true);
            return;
        }
        else
            proReceiptRegistration.FilePath = strImage;

        proReceiptRegistration.Creator = TB_Creator1.Text.Trim();
        proReceiptRegistration.CreateDate = DateTime.Parse(string.IsNullOrEmpty(DLC_CreateDate.Text.Trim()) ? DateTime.Now.ToString() : DLC_CreateDate.Text.Trim());
        proReceiptRegistration.DispatchDepartment = TB_DispatchDepartment.Text.Trim();
        proReceiptRegistration.DocumentNo = TB_DocumentNo.Text.Trim();
        proReceiptRegistration.FileName = TB_FileName1.Text.Trim();
        proReceiptRegistration.FileWay = TB_FileWay.Text.Trim();
        proReceiptRegistration.ProjectID = int.Parse(string.IsNullOrEmpty(LB_ReqID.Text.Trim()) || LB_ReqID.Text.Trim() == "" ? "0" : LB_ReqID.Text.Trim());
        proReceiptRegistration.DestructionDate = DateTime.Now;
        if (ddl_IsDestroy.SelectedValue.Trim() == "NO")
            proReceiptRegistration.DestroyPeople = "";
        proReceiptRegistration.ArchiveIdentification = 1;
        proReceiptRegistration.DocType = ddl_DocType1.SelectedValue.Trim();

        try
        {
            proReceiptRegistrationBLL.UpdateProReceiptRegistration(proReceiptRegistration, proReceiptRegistration.ID);

            if (!strDocumentNoOld.Equals(TB_DocumentNo.Text.Trim()) || !strDocTypeOld.Equals(ddl_DocType1.SelectedValue.Trim()))
            {
                UpdateProIssueRegistrationByDocumentNo(strDocumentNoOld, TB_DocumentNo.Text.Trim(), ddl_DocType1.SelectedValue.Trim());
            }

            lbl_ParamaValue.Text = "3";
            lbl_TreeViewName.Text = ddl_DocType1.SelectedValue.Trim();
            LoadProReceiptRegistrationName();
            BindInformation();


            Button7.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGDCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZGuiDangLanguageHandleGetWord").ToString().Trim()+"')", true); 
        }
    }

    protected void ddl_DocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL = "From ProReceiptRegistration as proReceiptRegistration Where proReceiptRegistration.DocumentNo='" + ddl_DocumentNo.SelectedValue.Trim() + "' and proReceiptRegistration.ProjectID='" + LB_ReqID.Text.Trim() + "' ";
        ProReceiptRegistrationBLL proReceiptRegistrationBLL = new ProReceiptRegistrationBLL();
        IList lst = proReceiptRegistrationBLL.GetAllProReceiptRegistrations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProReceiptRegistration proReceiptRegistration = (ProReceiptRegistration)lst[0];
            lbl_FilePath1.Visible = true;
            lbl_FilePath1.Text = proReceiptRegistration.FilePath.Trim();
            RP_SendFile.DataSource = null;
            RP_SendFile.DataBind();
            lbl_DocType1.Text = proReceiptRegistration.DocType.Trim();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowIssueRegistration','false') ", true);

    }

    /// <summary>
    /// 检查文件是否被销毁，已销毁，返回true；否则返回false
    /// </summary>
    /// <param name="strDocumentNo"></param>
    /// <returns></returns>
    protected bool IsDestroyProReceiptRegistration(string strDocumentNo)
    {
        bool flag = true;
        string strHQL = "From ProReceiptRegistration as proReceiptRegistration Where proReceiptRegistration.DocumentNo='" + strDocumentNo + "' and proReceiptRegistration.ProjectID='" + LB_ReqID.Text.Trim() + "' and char_length(rtrim(ltrim(proReceiptRegistration.DestroyPeople))) > 0 ";
        ProReceiptRegistrationBLL proReceiptRegistrationBLL = new ProReceiptRegistrationBLL();
        IList lst = proReceiptRegistrationBLL.GetAllProReceiptRegistrations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag = true;
        }
        else
            flag = false;
        return flag;
    }



    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;
        if (e.CommandName != "Page")
        {
            string strDocID = e.Item.Cells[2].Text.Trim();
            strHQL = "from ProIssueRegistration as proIssueRegistration where proIssueRegistration.ID = " + strDocID;
            ProIssueRegistrationBLL proIssueRegistrationBLL = new ProIssueRegistrationBLL();
            lst = proIssueRegistrationBLL.GetAllProIssueRegistrations(strHQL);
            ProIssueRegistration proIssueRegistration = (ProIssueRegistration)lst[0];

            if (e.CommandName == "Update")
            {
                LB_IssueRegistrationID.Text = proIssueRegistration.ID.ToString();
                ddl_DocumentNo.SelectedValue = proIssueRegistration.DocumentNo.Trim();
                TB_Recipients1.Text = proIssueRegistration.Recipients.Trim();
                NB_Attachments.Amount = proIssueRegistration.Attachments;
                TB_ReceivingDepartment.Text = proIssueRegistration.ReceivingDepartment.Trim();
                if (string.IsNullOrEmpty(proIssueRegistration.Recycling) || proIssueRegistration.Recycling.Trim() == "")
                {
                    ddl_IsRecycle.SelectedValue = "NO";
                }
                else
                    ddl_IsRecycle.SelectedValue = "YES";
                DLC_IssuingDate.Text = proIssueRegistration.IssuingDate.ToString("yyyy-MM-dd");

                lbl_FilePath1.Visible = false;
                lbl_FilePath1.Text = proIssueRegistration.FilePath.Trim();
                RP_SendFile.DataSource = lst;
                RP_SendFile.DataBind();

                lbl_DocType1.Text = proIssueRegistration.DocType.Trim();



                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowIssueRegistration','false') ", true);
            }

            if (e.CommandName == "Delete")
            {
                strHQL = "Delete From T_ProIssueRegistration Where ID = '" + strDocID + "' ";

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    lbl_ParamaValue.Text = "4";
                    lbl_TreeViewName.Text = lbl_DocType1.Text.Trim();
                    BindInformation();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
                }
            }

            if (e.CommandName == "Review")
            {
                if (!string.IsNullOrEmpty(proIssueRegistration.Recycling) && proIssueRegistration.Recycling.Trim() != "")
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGWJYBHSWXZCHS").ToString().Trim() + "')", true);
                    return;
                }
                proIssueRegistration.Recycling = strUserCode.Trim();
                proIssueRegistration.CollectionDate = DateTime.Now;
                proIssueRegistrationBLL.UpdateProIssueRegistration(proIssueRegistration, proIssueRegistration.ID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHSCG").ToString().Trim() + "')", true);
            }
            lbl_TreeViewName.Text = proIssueRegistration.DocType.Trim();
        }
        lbl_ParamaValue.Text = "4";
        BindInformation();
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        string strHQL = lbl_sql2.Text.Trim();

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProIssueRegistration");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_CreateIssueRegistration_Click(object sender, EventArgs e)
    {
        LB_IssueRegistrationID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowIssueRegistration','false') ", true);
    }

    protected void BT_NewIssueRegistration_Click(object sender, EventArgs e)
    {
        string strID1;

        strID1 = LB_IssueRegistrationID.Text.Trim();

        if (strID1 == "")
        {
            AddIssueRegistration();
        }
        else
        {
            UpdateIssueRegistration();
        }
    }


    protected void AddIssueRegistration()
    {
        if (string.IsNullOrEmpty(ddl_DocumentNo.SelectedValue.Trim()) || ddl_DocumentNo.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGWJMCWBXJC").ToString().Trim() + "')", true);
            ddl_DocumentNo.Focus();
            return;
        }
        if (IsDestroyProReceiptRegistration(ddl_DocumentNo.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGWJYBXHJC").ToString().Trim() + "')", true);
            ddl_DocumentNo.Focus();
            return;
        }

        ProIssueRegistrationBLL proIssueRegistrationBLL = new ProIssueRegistrationBLL();
        ProIssueRegistration proIssueRegistration = new ProIssueRegistration();

        proIssueRegistration.Attachments = int.Parse(NB_Attachments.Amount.ToString());
        proIssueRegistration.IssuingDate = DateTime.Parse(string.IsNullOrEmpty(DLC_IssuingDate.Text.Trim()) ? DateTime.Now.ToString() : DLC_IssuingDate.Text.Trim());
        proIssueRegistration.DocumentNo = ddl_DocumentNo.SelectedValue.Trim();
        proIssueRegistration.FileName = ddl_DocumentNo.SelectedItem.Text;
        proIssueRegistration.ReceivingDepartment = TB_ReceivingDepartment.Text.Trim();
        proIssueRegistration.FilePath = lbl_FilePath1.Text.Trim();
        proIssueRegistration.Recipients = TB_Recipients1.Text.Trim();
        proIssueRegistration.CollectionDate = DateTime.Now;
        if (ddl_IsRecycle.SelectedValue.Trim() == "NO")
            proIssueRegistration.Recycling = "";
        else
            proIssueRegistration.Recycling = strUserCode.Trim();
        proIssueRegistration.DocType = lbl_DocType1.Text.Trim();

        try
        {
            proIssueRegistrationBLL.AddProIssueRegistration(proIssueRegistration);

            LB_IssueRegistrationID.Text = GetProIssueRegistrationNowID().ToString();

            lbl_FilePath1.Visible = false;

            lbl_ParamaValue.Text = "4";
            lbl_TreeViewName.Text = lbl_DocType1.Text.Trim();
            BindInformation();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void UpdateIssueRegistration()
    {
        if (string.IsNullOrEmpty(ddl_DocumentNo.SelectedValue.Trim()) || ddl_DocumentNo.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGWJMCWBXJC").ToString().Trim() + "')", true);
            ddl_DocumentNo.Focus();
            return;
        }
        if (IsDestroyProReceiptRegistration(ddl_DocumentNo.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGWJYBXHJC").ToString().Trim() + "')", true);
            ddl_DocumentNo.Focus();
            return;
        }

        string strHQL = "from ProIssueRegistration as proIssueRegistration where proIssueRegistration.ID = '" + LB_IssueRegistrationID.Text.Trim() + "' ";
        ProIssueRegistrationBLL proIssueRegistrationBLL = new ProIssueRegistrationBLL();
        IList lst = proIssueRegistrationBLL.GetAllProIssueRegistrations(strHQL);
        ProIssueRegistration proIssueRegistration = (ProIssueRegistration)lst[0];

        proIssueRegistration.Attachments = int.Parse(NB_Attachments.Amount.ToString());
        proIssueRegistration.IssuingDate = DateTime.Parse(string.IsNullOrEmpty(DLC_IssuingDate.Text.Trim()) ? DateTime.Now.ToString() : DLC_IssuingDate.Text.Trim());
        proIssueRegistration.DocumentNo = ddl_DocumentNo.SelectedValue.Trim();
        proIssueRegistration.FileName = ddl_DocumentNo.SelectedItem.Text;
        proIssueRegistration.ReceivingDepartment = TB_ReceivingDepartment.Text.Trim();
        proIssueRegistration.FilePath = lbl_FilePath1.Text.Trim();
        proIssueRegistration.Recipients = TB_Recipients1.Text.Trim();
        proIssueRegistration.CollectionDate = DateTime.Now;
        if (ddl_IsRecycle.SelectedValue.Trim() == "NO")
            proIssueRegistration.Recycling = "";
        proIssueRegistration.DocType = lbl_DocType1.Text.Trim();

        try
        {
            proIssueRegistrationBLL.UpdateProIssueRegistration(proIssueRegistration, proIssueRegistration.ID);
            strHQL = "from ProIssueRegistration as proIssueRegistration where proIssueRegistration.ID = '" + LB_IssueRegistrationID.Text.Trim() + "' ";
            lst = proIssueRegistrationBLL.GetAllProIssueRegistrations(strHQL);
            lbl_FilePath1.Visible = false;
            RP_SendFile.DataSource = lst;
            RP_SendFile.DataBind();

            lbl_ParamaValue.Text = "4";
            lbl_TreeViewName.Text = lbl_DocType1.Text.Trim();
            BindInformation();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }



    /// <summary>
    /// 上传图或文件电子版本
    /// </summary>
    /// <param name="struploadattach"></param>
    /// <param name="strtype">收图附件，文件附件</param>
    /// <returns></returns>
    protected string UploadBookDesign(Brettle.Web.NeatUpload.InputFile struploadattach, string strtype)
    {
        //上传规章制度附件
        if (struploadattach.HasFile)
        {
            string strFileName1, strExtendName;

            strFileName1 = struploadattach.FileName;//获取上传文件的文件名,包括后缀
            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + "_" + strtype + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";

            string bookimage = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {
                return "1";
            }
            else
            {
                try
                {
                    struploadattach.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                    return bookimage;
                }
                catch
                {
                    return "2";
                }
            }
        }
        else
        {
            return "0";
        }
    }

    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        Button BT_Review = (Button)e.Item.FindControl("BT_Review");
        Button BT_ActiveFile = (Button)e.Item.FindControl("BT_ActiveFile");
        HiddenField hfDestroyPeople = (HiddenField)e.Item.FindControl("hfDestroyPeople");
        //string hfDestroyPeople = e.Item.Cells[1].Text.Trim();
        HiddenField hfArchive = (HiddenField)e.Item.FindControl("hfArchive");
        if (hfDestroyPeople != null)
            if (hfDestroyPeople.Value.Trim() == "")
            {
                if (BT_Review != null)
                    BT_Review.Visible = true;
                else
                    BT_Review.Visible = false;
            }
            else
                if (BT_Review != null)
                BT_Review.Visible = false;
            else
                BT_Review.Visible = false;

        if (hfArchive != null)
            if (hfArchive.Value.Trim().Equals("0"))
            {
                if (BT_ActiveFile != null)
                    BT_ActiveFile.Visible = true;
                else
                    BT_ActiveFile.Visible = false;
            }
            else
                if (BT_ActiveFile != null)
                BT_ActiveFile.Visible = false;
            else
                BT_ActiveFile.Visible = false;
    }

    protected void DataGrid2_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        Button BT_Review = (Button)e.Item.FindControl("BT_Review");
        //string hfRecycling = e.Item.Cells[1].Text.Trim();
        HiddenField hfRecycling = (HiddenField)e.Item.FindControl("hfRecycling");
        if (hfRecycling != null)
        {
            if (hfRecycling.Value.Trim() == "")
            {
                if (BT_Review != null)
                    BT_Review.Visible = true;
                else
                    BT_Review.Visible = false;
            }
            else
                if (BT_Review != null)
                BT_Review.Visible = false;
            else
                BT_Review.Visible = false;
        }
    }

    protected void DataGrid3_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        Button BT_ActiveGraph = (Button)e.Item.FindControl("BT_ActiveGraph");
        HiddenField hfArchiveGraph = (HiddenField)e.Item.FindControl("hfArchiveGraph");
        if (hfArchiveGraph != null)
        {
            if (hfArchiveGraph.Value.Trim().Equals("0"))
            {
                if (BT_ActiveGraph != null)
                    BT_ActiveGraph.Visible = true;
                else
                    BT_ActiveGraph.Visible = false;
            }
            else
            {
                if (BT_ActiveGraph != null)
                    BT_ActiveGraph.Visible = false;
                else
                    BT_ActiveGraph.Visible = false;
            }
        }
    }

    protected void DataGrid5_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        Button BT_Review = (Button)e.Item.FindControl("BT_Review");
        //string hfBackPer = e.Item.Cells[1].Text.Trim();
        HiddenField hfBackPer = (HiddenField)e.Item.FindControl("hfBackPer");
        if (hfBackPer != null)
        {
            if (hfBackPer.Value.Trim() == "")
            {
                if (BT_Review != null)
                    BT_Review.Visible = true;
                else
                    BT_Review.Visible = false;
            }
            else
            {
                if (BT_Review != null)
                    BT_Review.Visible = false;
                else
                    BT_Review.Visible = false;
            }
        }
    }

    protected void BT_EditDocType_Click(object sender, EventArgs e)
    {
       
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowDocType','false') ", true);
    }

    protected void BT_NewDocType_Click(object sender, EventArgs e)
    {
        if (ddl_ParentName.SelectedValue.Trim() == "PleaseSelect" || string.IsNullOrEmpty(ddl_ParentName.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZJDDFJDJC").ToString().Trim() + "')", true);
            ddl_ParentName.Focus();
            return;
        }
        if (ddl_ParentName.SelectedValue.Trim() == LanguageHandle.GetWord("FaTuDengJi").ToString().Trim() || ddl_ParentName.SelectedValue.Trim() == LanguageHandle.GetWord("FaWenDengJi").ToString().Trim())
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGFTWDJWXTJZJDHXYDBSTWDJXDZJDDGLJC").ToString().Trim() + "')", true);
            ddl_ParentName.Focus();
            return;
        }
        if (txtClassificationName.Text.Trim() == "" || string.IsNullOrEmpty(txtClassificationName.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGJDMCBNWKJC").ToString().Trim() + "')", true);
            txtClassificationName.Focus();
            return;
        }
        if (txtClassificationName.Text.Trim().Contains("."))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGJDMCBNBHJC").ToString().Trim() + "')", true);
            txtClassificationName.Focus();
            return;
        }
        if (IsProDocGraphControl(ddl_ParentName.SelectedValue.Trim(), txtClassificationName.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGJDYCZJC").ToString().Trim() + "')", true);
            ddl_ParentName.Focus();
            txtClassificationName.Focus();
            return;
        }
        ProDocGraphControlBLL proDocGraphControlBLL = new ProDocGraphControlBLL();
        ProDocGraphControl proDocGraphControl = new ProDocGraphControl();
        proDocGraphControl.ClassificationName = txtClassificationName.Text.Trim();
        proDocGraphControl.ParentName = ddl_ParentName.SelectedValue.Trim();
        proDocGraphControl.SortNo = int.Parse(txtSortNo.Text.Trim() == "" ? "0" : txtSortNo.Text.Trim());
        try
        {
            proDocGraphControlBLL.AddProDocGraphControl(proDocGraphControl);
            lbl_NewID.Text = GetProDocGraphControlID(proDocGraphControl.ParentName.Trim(), proDocGraphControl.ClassificationName.Trim());

            if (proDocGraphControl.ParentName.Trim() == LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim())
            {
                proDocGraphControl.ParentName = LanguageHandle.GetWord("FaTuDengJi").ToString().Trim();
                proDocGraphControlBLL.AddProDocGraphControl(proDocGraphControl);
            }
            else if (proDocGraphControl.ParentName.Trim() == LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim())
            {
                proDocGraphControl.ParentName = LanguageHandle.GetWord("FaWenDengJi").ToString().Trim();
                proDocGraphControlBLL.AddProDocGraphControl(proDocGraphControl);
            }

            DocTypeTree(TreeView1, strUserCode, strProjectID, ShareClass.GetProjectName(strProjectID.Trim()));
            LoadProDocType();
            BindParentNameData(lbl_ParamaValue.Text.Trim());


            ddl_ParentName.SelectedValue = proDocGraphControl.ParentName;

            BT_NewDocType.Enabled = true;
            BT_UpdateDocType.Enabled = true;
            BT_DeleteDocType.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJieDianLanguageHandleGetWord").ToString().Trim()+"')", true); 
        }
        catch (Exception err)
        {
            //LogClass.WriteLogFile(err.Message.ToString());
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDXZSB").ToString().Trim() + "')", true);
        }
    }

    protected void BT_UpdateDocType_Click(object sender, EventArgs e)
    {
        if (ddl_ParentName.SelectedValue.Trim() == "PleaseSelect" || string.IsNullOrEmpty(ddl_ParentName.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZJDDFJDJC").ToString().Trim() + "')", true);
            ddl_ParentName.Focus();
            return;
        }
        if (txtClassificationName.Text.Trim() == "" || string.IsNullOrEmpty(txtClassificationName.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGJDMCBNWKJC").ToString().Trim() + "')", true);
            txtClassificationName.Focus();
            return;
        }
        if (lbl_NewID.Text.Trim() == "" || string.IsNullOrEmpty(lbl_NewID.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGJDSJBCZJC").ToString().Trim() + "')", true);
            return;
        }
        if (txtClassificationName.Text.Trim().Contains("."))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGJDMCBNBHJC").ToString().Trim() + "')", true);
            txtClassificationName.Focus();
            return;
        }
        if (IsProDocGraphControl(ddl_ParentName.SelectedValue.Trim(), txtClassificationName.Text.Trim(), lbl_NewID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGJDYCZJC").ToString().Trim() + "')", true);
            ddl_ParentName.Focus();
            txtClassificationName.Focus();
            return;
        }
        string strHQL = "From ProDocGraphControl as proDocGraphControl Where proDocGraphControl.ID ='" + lbl_NewID.Text.Trim() + "' ";
        ProDocGraphControlBLL proDocGraphControlBLL = new ProDocGraphControlBLL();
        IList lst = proDocGraphControlBLL.GetAllProDocGraphControls(strHQL);
        ProDocGraphControl proDocGraphControl = (ProDocGraphControl)lst[0];
        string oldclassificationname = proDocGraphControl.ClassificationName.Trim();
        proDocGraphControl.ParentName = ddl_ParentName.SelectedValue.Trim();
        proDocGraphControl.ClassificationName = txtClassificationName.Text.Trim();
        proDocGraphControl.SortNo = int.Parse(txtSortNo.Text.Trim() == "" || string.IsNullOrEmpty(txtSortNo.Text) ? "0" : txtSortNo.Text.Trim());
        try
        {
            //proDocGraphControlBLL.UpdateProDocGraphControl(proDocGraphControl, proDocGraphControl.ID);
            UpdateProDocGraphControlByclassname(oldclassificationname, proDocGraphControl.ClassificationName.Trim(), proDocGraphControl.SortNo.ToString());

            DocTypeTree(TreeView1, strUserCode, strProjectID, ShareClass.GetProjectName(strProjectID.Trim()));
            LoadProDocType();
            BindParentNameData(lbl_ParamaValue.Text.Trim());
            ddl_ParentName.SelectedValue = proDocGraphControl.ParentName.Trim();

            BT_NewDocType.Enabled = true;
            BT_UpdateDocType.Enabled = true;
            BT_DeleteDocType.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJieDianLanguageHandleGetWord").ToString().Trim()+"')", true); 
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDXGSB").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeleteDocType_Click(object sender, EventArgs e)
    {
        if (IsProDocGraphControlByDocType(txtClassificationName.Text.Trim(), lbl_ParamaValue.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGJDYBDYWFSCJC").ToString().Trim() + "')", true);
            return;
        }
        if (txtClassificationName.Text.Trim() == "" || string.IsNullOrEmpty(txtClassificationName.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGJDSJBCZJC").ToString().Trim() + "')", true);
            txtClassificationName.Focus();
            return;
        }

        string strHQL = "Delete From T_ProDocGraphControl Where ClassificationName = '" + txtClassificationName.Text.Trim() + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            DocTypeTree(TreeView1, strUserCode, strProjectID, ShareClass.GetProjectName(strProjectID.Trim()));
            LoadProDocType();
            BindParentNameData(lbl_ParamaValue.Text.Trim());

            BT_NewDocType.Enabled = true;
            BT_UpdateDocType.Enabled = false;
            BT_DeleteDocType.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    /// <summary>
    /// true代表存在，不能删除；
    /// </summary>
    /// <param name="newclname"></param>
    /// <param name="ParamaValue"></param>
    /// <returns></returns>
    protected bool IsProDocGraphControlByDocType(string newclname, string ParamaValue)
    {
        bool flag = true;
        if (ParamaValue == "1")
        {
            string strHQL = "Select * from T_ProGraphRegistration Where DocType='" + newclname + "' ";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProGraphRegistration");
            if (ds.Tables[0].Rows.Count > 0 && ds != null)
            {
                flag = true;
            }
            else
                flag = false;
        }
        if (ParamaValue == "2")
        {
            string strHQL = "Select * from T_ProSendFigureRegistration Where DocType='" + newclname + "' ";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProSendFigureRegistration");
            if (ds.Tables[0].Rows.Count > 0 && ds != null)
            {
                flag = true;
            }
            else
                flag = false;
        }
        if (ParamaValue == "3")
        {
            string strHQL = "Select * from T_ProReceiptRegistration Where DocType='" + newclname + "' ";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProReceiptRegistration");
            if (ds.Tables[0].Rows.Count > 0 && ds != null)
            {
                flag = true;
            }
            else
                flag = false;
        }
        if (ParamaValue == "4")
        {
            string strHQL = "Select * from T_ProIssueRegistration Where DocType='" + newclname + "' ";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProIssueRegistration");
            if (ds.Tables[0].Rows.Count > 0 && ds != null)
            {
                flag = true;
            }
            else
                flag = false;
        }
        return flag;
    }

    /// <summary>
    /// 更新时，同时更新图、文的收发数据
    /// </summary>
    /// <param name="oldclname"></param>
    /// <param name="newclname"></param>
    /// <param name="newsortno"></param>
    protected void UpdateProDocGraphControlByclassname(string oldclname, string newclname, string newsortno)
    {
        string strHQL = "Update T_ProDocGraphControl set ClassificationName='" + newclname + "',SortNo='" + newsortno + "' Where ClassificationName = '" + oldclname + "' ";
        ShareClass.RunSqlCommandForNOOperateLog(strHQL);

        if (lbl_ParamaValue.Text.Trim() == "1")
        {
            strHQL = "Update T_ProGraphRegistration set DocType='" + newclname + "' Where DocType = '" + oldclname + "' ";
            ShareClass.RunSqlCommandForNOOperateLog(strHQL);
        }
        if (lbl_ParamaValue.Text.Trim() == "2")
        {
            strHQL = "Update T_ProSendFigureRegistration set DocType='" + newclname + "' Where DocType = '" + oldclname + "' ";
            ShareClass.RunSqlCommandForNOOperateLog(strHQL);
        }
        if (lbl_ParamaValue.Text.Trim() == "3")
        {
            strHQL = "Update T_ProReceiptRegistration set DocType='" + newclname + "' Where DocType = '" + oldclname + "' ";
            ShareClass.RunSqlCommandForNOOperateLog(strHQL);
        }
        if (lbl_ParamaValue.Text.Trim() == "4")
        {
            strHQL = "Update T_ProIssueRegistration set DocType='" + newclname + "' Where DocType = '" + oldclname + "' ";
            ShareClass.RunSqlCommandForNOOperateLog(strHQL);
        }
    }

    /// <summary>
    /// 节点新增或更新时，检查节点是否存在，存在则返回true；否则返回false
    /// </summary>
    /// <param name="strFileNo"></param>
    /// <param name="strID"></param>
    /// <returns></returns>
    protected bool IsProDocGraphControl(string strParentName, string strClassificationName, string strID)
    {
        string strHQL;
        bool flag = true;
        if (string.IsNullOrEmpty(strID))
            strHQL = "From ProDocGraphControl as proDocGraphControl Where proDocGraphControl.ParentName ='" + strParentName + "' and proDocGraphControl.ClassificationName = '" + strClassificationName + "' ";
        else
            strHQL = "From ProDocGraphControl as proDocGraphControl Where proDocGraphControl.ParentName ='" + strParentName + "' and proDocGraphControl.ClassificationName = '" + strClassificationName + "' and proDocGraphControl.ID <>'" + strID + "' ";

        ProDocGraphControlBLL proDocGraphControlBLL = new ProDocGraphControlBLL();
        IList lst = proDocGraphControlBLL.GetAllProDocGraphControls(strHQL);
        if (lst.Count > 0 && lst != null)
            flag = true;
        else
            flag = false;
        return flag;
    }

    /// <summary>
    /// 获取即时ID
    /// </summary>
    /// <param name="strParentName"></param>
    /// <param name="strClassificationName"></param>
    /// <returns></returns>
    protected string GetProDocGraphControlID(string strParentName, string strClassificationName)
    {
        string strHQL = "From ProDocGraphControl as proDocGraphControl Where proDocGraphControl.ParentName ='" + strParentName + "' and proDocGraphControl.ClassificationName = '" + strClassificationName + "' Order By proDocGraphControl.ID Desc ";
        ProDocGraphControlBLL proDocGraphControlBLL = new ProDocGraphControlBLL();
        IList lst = proDocGraphControlBLL.GetAllProDocGraphControls(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProDocGraphControl proDocGraphControl = (ProDocGraphControl)lst[0];
            return proDocGraphControl.ID.ToString();
        }
        else
            return "0";
    }

    protected void BT_Check_Click(object sender, EventArgs e)
    {
        lbl_ParamaValue.Text = "1";
        lbl_TreeViewName.Text = ddl_DocType.SelectedValue.Trim();
        LoadProGraphRegistrationName();

        Panel_ProSendFigureRegistration.Visible = false;
        Panel_ProGraphRegistration.Visible = true;
        Panel_ProIssueRegistration.Visible = false;
        Panel_ProReceiptRegistration.Visible = false;
        LB_ProGraphRegistrationCount.Visible = true;
        LB_ProIssueRegistrationCount.Visible = false;
        LB_ProReceiptRegistrationCount.Visible = false;
        LB_ProSendFigureRegistrationCount.Visible = false;


        string strHQL = "Select * From T_ProGraphRegistration Where ProjectID='" + strProjectID + "' ";
        if (lbl_TreeViewName.Text.Trim() != "" && lbl_TreeViewName.Text.Trim() != LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim())
        {
            strHQL += " and DocType='" + lbl_TreeViewName.Text.Trim() + "' ";
        }
        if (TB_GraphInfo.Text.Trim() != "" && !string.IsNullOrEmpty(TB_GraphInfo.Text))
        {
            strHQL += " and (GraphNo like '%" + TB_GraphInfo.Text.Trim() + "%' or FileNo like '%" + TB_GraphInfo.Text.Trim() + "%' or FileName like '%" + TB_GraphInfo.Text.Trim() + "%' or Remark like '%" + TB_GraphInfo.Text.Trim() + "%')";
        }
        strHQL += "Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProGraphRegistration");

        DataGrid3.CurrentPageIndex = 0;
        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
        lbl_sql3.Text = strHQL;

        LB_FindCondition.Text = LanguageHandle.GetWord("ShouTuDengJiLieBiao").ToString().Trim();
        LB_ProGraphRegistrationCount.Text = LanguageHandle.GetWord("ZongShu").ToString().Trim() + ds.Tables[0].Rows.Count.ToString();
    }

    /// <summary>
    /// 根据情况显示右边列表信息，1 收图登记,2 发图登记,3 收文登记,4 发文登记
    /// </summary>
    protected void BindInformation()
    {
        string strtype = lbl_ParamaValue.Text.Trim();
        if (strtype.Equals("2"))
        {
            Panel_ProSendFigureRegistration.Visible = true;
            Panel_ProGraphRegistration.Visible = false;
            Panel_ProIssueRegistration.Visible = false;
            Panel_ProReceiptRegistration.Visible = false;

            LoadProSendFigureRegistration();
            LB_ProGraphRegistrationCount.Visible = false;
            LB_ProIssueRegistrationCount.Visible = false;
            LB_ProReceiptRegistrationCount.Visible = false;
            LB_ProSendFigureRegistrationCount.Visible = true;
        }
        else if (strtype.Equals("3"))
        {
            Panel_ProSendFigureRegistration.Visible = false;
            Panel_ProGraphRegistration.Visible = false;
            Panel_ProIssueRegistration.Visible = false;
            Panel_ProReceiptRegistration.Visible = true;

            LoadProReceiptRegistration();
            LB_ProGraphRegistrationCount.Visible = false;
            LB_ProIssueRegistrationCount.Visible = false;
            LB_ProReceiptRegistrationCount.Visible = true;
            LB_ProSendFigureRegistrationCount.Visible = false;
        }
        else if (strtype.Equals("4"))
        {
            Panel_ProSendFigureRegistration.Visible = false;
            Panel_ProGraphRegistration.Visible = false;
            Panel_ProIssueRegistration.Visible = true;
            Panel_ProReceiptRegistration.Visible = false;

            LoadProIssueRegistration();
            LB_ProGraphRegistrationCount.Visible = false;
            LB_ProIssueRegistrationCount.Visible = true;
            LB_ProReceiptRegistrationCount.Visible = false;
            LB_ProSendFigureRegistrationCount.Visible = false;
        }
        else
        {
            Panel_ProSendFigureRegistration.Visible = false;
            Panel_ProGraphRegistration.Visible = true;
            Panel_ProIssueRegistration.Visible = false;
            Panel_ProReceiptRegistration.Visible = false;

            LoadProGraphRegistration();
            LB_ProGraphRegistrationCount.Visible = true;
            LB_ProIssueRegistrationCount.Visible = false;
            LB_ProReceiptRegistrationCount.Visible = false;
            LB_ProSendFigureRegistrationCount.Visible = false;
        }
    }

    /// <summary>
    /// 定义文控类型树
    /// </summary>
    /// <param name="tv">控件</param>
    /// <param name="strUserCode">用户编码</param>
    /// <param name="strprojectid">项目编号</param>
    /// <param name="strprojectname">项目名称</param>
    protected void DocTypeTree(TreeView tv, string strUserCode, string strprojectid, string strprojectname)
    {
        //添加根节点
        tv.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = LanguageHandle.GetWord("Project").ToString().Trim() + strprojectid + " " + strprojectname + LanguageHandle.GetWord("WenKongLieBiao").ToString().Trim();
        node1.Target = "0";
        node1.Expanded = true;
        tv.Nodes.Add(node1);

        string namelist = LanguageHandle.GetWord("ShouTuDengJiFaTuDengJiShouWenD").ToString().Trim();
        string[] tempNameList = namelist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < tempNameList.Length; i++)
        {
            node3 = new TreeNode();
            node3.Text = (i + 1) + "." + tempNameList[i].ToString();
            node3.Target = (i + 1).ToString();
            node3.Expanded = false;
            node1.ChildNodes.Add(node3);

            GetProDocGraphControlTreeView(tempNameList[i].ToString().Trim(), node3);

            tv.DataBind();
        }
    }



    /// <summary>
    /// 根据父节点名称，获取其子节点名称
    /// </summary>
    /// <param name="strParentName"></param>
    /// <returns></returns>
    protected void GetProDocGraphControlTreeView(string strParentName, TreeNode node)
    {
        string strHQL = "From ProDocGraphControl as proDocGraphControl Where proDocGraphControl.ParentName='" + strParentName + "' Order By proDocGraphControl.SortNo ";
        ProDocGraphControlBLL proDocGraphControlBLL = new ProDocGraphControlBLL();
        IList lst = proDocGraphControlBLL.GetAllProDocGraphControls(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                ProDocGraphControl proDocGraphControl = (ProDocGraphControl)lst[i];
                TreeNode node1 = new TreeNode();
                node1.Text = node.Text.Trim().Substring(0, node.Text.Trim().LastIndexOf(".")) + "." + proDocGraphControl.SortNo.ToString() + "." + proDocGraphControl.ClassificationName.Trim();
                node1.Target = node.Target.Trim() + "_" + proDocGraphControl.ID;
                node1.Expanded = false;
                node.ChildNodes.Add(node1);

                GetProDocGraphControlTreeView(proDocGraphControl.ClassificationName.Trim(), node1);
            }
        }
    }

    /// <summary>
    /// 收图登记
    /// </summary>
    protected void LoadProGraphRegistration()
    {
        string strHQL = "Select * From T_ProGraphRegistration Where ProjectID='" + strProjectID + "' ";
        if (lbl_TreeViewName.Text.Trim() != "" && lbl_TreeViewName.Text.Trim() != LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim())
        {
            strHQL += " and DocType='" + lbl_TreeViewName.Text.Trim() + "' ";
        }
        strHQL += "Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProGraphRegistration");


        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
        lbl_sql3.Text = strHQL;

        LB_FindCondition.Text = LanguageHandle.GetWord("ShouTuDengJiLieBiao").ToString().Trim();
        LB_ProGraphRegistrationCount.Text = LanguageHandle.GetWord("ZongShu").ToString().Trim() + ds.Tables[0].Rows.Count.ToString();
    }

    /// <summary>
    /// 收图登记新增或更新时，检查档案号是否存在，存在则返回true；否则返回false
    /// </summary>
    /// <param name="strFileNo"></param>
    /// <param name="strID"></param>
    /// <returns></returns>
    protected bool IsProGraphRegistration(string strFileNo, string strID)
    {
        string strHQL;
        bool flag = true;
        if (string.IsNullOrEmpty(strID))
            strHQL = "From ProGraphRegistration as proGraphRegistration Where proGraphRegistration.FileNo ='" + strFileNo + "' ";
        else
            strHQL = "From ProGraphRegistration as proGraphRegistration Where proGraphRegistration.FileNo ='" + strFileNo + "' and proGraphRegistration.ID <>'" + strID + "' ";

        ProGraphRegistrationBLL proGraphRegistrationBLL = new ProGraphRegistrationBLL();
        IList lst = proGraphRegistrationBLL.GetAllProGraphRegistrations(strHQL);
        if (lst.Count > 0 && lst != null)
            flag = true;
        else
            flag = false;
        return flag;
    }

    /// <summary>
    /// 获取收图登记即时ID
    /// </summary>
    /// <returns></returns>
    protected int GetProGraphRegistrationNowID()
    {
        int strId = 0;
        string strHQL = "From ProGraphRegistration as proGraphRegistration Order By proGraphRegistration.ID Desc ";
        ProGraphRegistrationBLL proGraphRegistrationBLL = new ProGraphRegistrationBLL();
        IList lst = proGraphRegistrationBLL.GetAllProGraphRegistrations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProGraphRegistration proGraphRegistration = (ProGraphRegistration)lst[0];
            strId = proGraphRegistration.ID;
        }

        return strId;
    }

    /// <summary>
    /// 删除收图记录时，检查档案号是否在发图记录中存在，若存在，返回true；否则返回false
    /// </summary>
    /// <param name="strFileNo"></param>
    /// <returns></returns>
    protected bool IsProSendFigureRegistrationExitFileNo(string strFileNo)
    {
        bool flag = true;
        string strHQL = "From ProSendFigureRegistration as proSendFigureRegistration Where proSendFigureRegistration.FileNo ='" + strFileNo + "' ";
        ProSendFigureRegistrationBLL proSendFigureRegistrationBLL = new ProSendFigureRegistrationBLL();
        IList lst = proSendFigureRegistrationBLL.GetAllProSendFigureRegistrations(strHQL);
        if (lst.Count > 0 && lst != null)
            flag = true;
        else
            flag = false;
        return flag;
    }

    /// <summary>
    /// 发图登记
    /// </summary>
    protected void LoadProSendFigureRegistration()
    {
        string strHQL = "Select * From T_ProSendFigureRegistration Where FileNo in (Select FileNo From T_ProGraphRegistration Where ProjectID='" + strProjectID + "') ";
        if (lbl_TreeViewName.Text.Trim() != "" && lbl_TreeViewName.Text.Trim() != LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim())
        {
            strHQL += " and DocType='" + lbl_TreeViewName.Text.Trim() + "' ";
        }
        strHQL += "Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProSendFigureRegistration");


        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();
        lbl_sql4.Text = strHQL;

        LB_FindCondition.Text = LanguageHandle.GetWord("FaTuDengJiLieBiao").ToString().Trim();
        LB_ProSendFigureRegistrationCount.Text = LanguageHandle.GetWord("ZongShu").ToString().Trim() + ds.Tables[0].Rows.Count.ToString();
    }

    /// <summary>
    /// 获取发图登记即时ID
    /// </summary>
    /// <returns></returns>
    protected int GetProSendFigureRegistrationNowID()
    {
        int strId = 0;

        string strHQL = "Select * From T_ProSendFigureRegistration Order By ID Desc  limit 1";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProSendFigureRegistration");
        RP_SendGraph.DataSource = ds;
        RP_SendGraph.DataBind();
        if (ds.Tables[0].Rows.Count > 0 && ds != null)
        {
            strId = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
        }

        return strId;
    }

    /// <summary>
    /// 更新收图登记的档案编号时，同时相应地更改发图登记中对应的档案编号
    /// </summary>
    /// <param name="strFileNoOld"></param>
    /// <param name="strFileNoNew"></param>
    /// <param name="strDocTypeNew"></param>
    protected void UpdateProSendFigureRegistrationByFileNo(string strFileNoOld, string strFileNoNew, string strDocTypeNew)
    {
        string strHQL = "From ProSendFigureRegistration as proSendFigureRegistration Where proSendFigureRegistration.FileNo ='" + strFileNoOld + "' ";
        ProSendFigureRegistrationBLL proSendFigureRegistrationBLL = new ProSendFigureRegistrationBLL();
        IList lst = proSendFigureRegistrationBLL.GetAllProSendFigureRegistrations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                ProSendFigureRegistration proSendFigureRegistration = (ProSendFigureRegistration)lst[i];
                proSendFigureRegistration.FileNo = strFileNoNew;
                proSendFigureRegistration.DocType = strDocTypeNew;
                proSendFigureRegistrationBLL.UpdateProSendFigureRegistration(proSendFigureRegistration, proSendFigureRegistration.ID);
            }
        }
    }

    /// <summary>
    /// 收文登记
    /// </summary>
    protected void LoadProReceiptRegistration()
    {
        string strHQL = "Select * From T_ProReceiptRegistration Where ProjectID='" + strProjectID + "' ";
        if (lbl_TreeViewName.Text.Trim() != "" && lbl_TreeViewName.Text.Trim() != LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim())
        {
            strHQL += " and DocType='" + lbl_TreeViewName.Text.Trim() + "' ";
        }
        strHQL += "Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProReceiptRegistration");


        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
        lbl_sql1.Text = strHQL;

        LB_FindCondition.Text = LanguageHandle.GetWord("ShouWenDengJiLieBiao").ToString().Trim();
        LB_ProReceiptRegistrationCount.Text = LanguageHandle.GetWord("ZongShu").ToString().Trim() + ds.Tables[0].Rows.Count.ToString();
    }

    /// <summary>
    /// 收文登记新增或更新时，检查文件编号是否存在，存在则返回true；否则返回false
    /// </summary>
    /// <param name="strFileNo"></param>
    /// <param name="strID"></param>
    /// <returns></returns>
    protected bool IsProReceiptRegistration(string strDocumentNo, string strID)
    {
        string strHQL;
        bool flag = true;
        if (string.IsNullOrEmpty(strID))
            strHQL = "From ProReceiptRegistration as proReceiptRegistration Where proReceiptRegistration.DocumentNo ='" + strDocumentNo + "' ";
        else
            strHQL = "From ProReceiptRegistration as proReceiptRegistration Where proReceiptRegistration.DocumentNo ='" + strDocumentNo + "' and proReceiptRegistration.ID <>'" + strID + "' ";

        ProReceiptRegistrationBLL proReceiptRegistrationBLL = new ProReceiptRegistrationBLL();
        IList lst = proReceiptRegistrationBLL.GetAllProReceiptRegistrations(strHQL);
        if (lst.Count > 0 && lst != null)
            flag = true;
        else
            flag = false;
        return flag;
    }

    /// <summary>
    /// 获取收文登记即时ID
    /// </summary>
    /// <returns></returns>
    protected int GetProReceiptRegistrationNowID()
    {
        int strId = 0;
        string strHQL = "From ProReceiptRegistration as proReceiptRegistration Order By proReceiptRegistration.ID Desc ";
        ProReceiptRegistrationBLL proReceiptRegistrationBLL = new ProReceiptRegistrationBLL();
        IList lst = proReceiptRegistrationBLL.GetAllProReceiptRegistrations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProReceiptRegistration proReceiptRegistration = (ProReceiptRegistration)lst[0];
            strId = proReceiptRegistration.ID;
        }

        return strId;
    }

    /// <summary>
    /// 删除收文记录时，检查文件编号是否在发文记录中存在，若存在，返回true；否则返回false
    /// </summary>
    /// <param name="strFileNo"></param>
    /// <returns></returns>
    protected bool IsProIssueRegistrationExitDocumentNo(string strDocumentNo)
    {
        bool flag = true;
        string strHQL = "From ProIssueRegistration as proIssueRegistration Where proIssueRegistration.DocumentNo ='" + strDocumentNo + "' ";
        ProIssueRegistrationBLL proIssueRegistrationBLL = new ProIssueRegistrationBLL();
        IList lst = proIssueRegistrationBLL.GetAllProIssueRegistrations(strHQL);
        if (lst.Count > 0 && lst != null)
            flag = true;
        else
            flag = false;
        return flag;
    }

    /// <summary>
    /// 发文登记
    /// </summary>
    protected void LoadProIssueRegistration()
    {
        string strHQL = "Select * From T_ProIssueRegistration Where DocumentNo in (Select DocumentNo From T_ProReceiptRegistration Where ProjectID='" + strProjectID + "') ";
        if (lbl_TreeViewName.Text.Trim() != "" && lbl_TreeViewName.Text.Trim() != LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim())
        {
            strHQL += " and DocType='" + lbl_TreeViewName.Text.Trim() + "' ";
        }
        strHQL += "Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProIssueRegistration");


        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql2.Text = strHQL;

        LB_FindCondition.Text = LanguageHandle.GetWord("FaWenDengJiLieBiao").ToString().Trim();
        LB_ProIssueRegistrationCount.Text = LanguageHandle.GetWord("ZongShu").ToString().Trim() + ds.Tables[0].Rows.Count.ToString();
    }

    /// <summary>
    /// 获取发文登记即时ID
    /// </summary>
    /// <returns></returns>
    protected int GetProIssueRegistrationNowID()
    {
        int strId = 0;
        string strHQL = "Select * From T_ProIssueRegistration Order By ID Desc  limit 1";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProIssueRegistration");
        RP_SendFile.DataSource = ds;
        RP_SendFile.DataBind();
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            strId = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
        }
        return strId;
    }

    /// <summary>
    /// 更新收文登记的文件编号时，同时相应地更改发文登记中对应的文件编号
    /// </summary>
    /// <param name="strDocumentNoOld"></param>
    /// <param name="strDocumentNoNew"></param>
    /// <param name="strDocTypeNew"></param>
    protected void UpdateProIssueRegistrationByDocumentNo(string strDocumentNoOld, string strDocumentNoNew, string strDocTypeNew)
    {
        string strHQL = "From ProIssueRegistration as proIssueRegistration Where proIssueRegistration.DocumentNo ='" + strDocumentNoOld + "' ";
        ProIssueRegistrationBLL proIssueRegistrationBLL = new ProIssueRegistrationBLL();
        IList lst = proIssueRegistrationBLL.GetAllProIssueRegistrations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                ProIssueRegistration proIssueRegistration = (ProIssueRegistration)lst[i];
                proIssueRegistration.DocumentNo = strDocumentNoNew;
                proIssueRegistration.DocType = strDocTypeNew;
                proIssueRegistrationBLL.UpdateProIssueRegistration(proIssueRegistration, proIssueRegistration.ID);
            }
        }
    }

    /// <summary>
    /// 绑定收图登记中的档案号及名称
    /// </summary>
    protected void LoadProGraphRegistrationName()
    {
        string strHQL;
        IList lst;
        strHQL = "From ProGraphRegistration as proGraphRegistration Where proGraphRegistration.ProjectID='" + strProjectID + "' Order By proGraphRegistration.ID Desc";
        ProGraphRegistrationBLL proGraphRegistrationBLL = new ProGraphRegistrationBLL();
        lst = proGraphRegistrationBLL.GetAllProGraphRegistrations(strHQL);
        ddl_FileNo.DataSource = lst;
        ddl_FileNo.DataBind();
        ddl_FileNo.Items.Insert(0, new ListItem("--Select--", ""));
    }

    /// <summary>
    /// 绑定类型
    /// </summary>
    protected void LoadProDocType()
    {
        DataTable dt = GetList(LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim());
        if (dt != null && dt.Rows.Count > 0)
        {
            ddl_DocType.Items.Clear();
            ddl_DocType.Items.Insert(0, new ListItem(LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim(), LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim()));
            SetInterval(ddl_DocType, LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim(), " ");
        }
        else
        {
            ddl_DocType.Items.Clear();
            ddl_DocType.Items.Insert(0, new ListItem(LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim(), LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim()));
        }

        dt = GetList(LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim());
        if (dt != null && dt.Rows.Count > 0)
        {
            ddl_DocType1.Items.Clear();
            ddl_DocType1.Items.Insert(0, new ListItem(LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim(), LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim()));
            SetInterval(ddl_DocType1, LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim(), " ");
        }
        else
        {
            ddl_DocType1.Items.Clear();
            ddl_DocType1.Items.Insert(0, new ListItem(LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim(), LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim()));
        }
    }

    /// <summary>
    /// 获取项目文控类型
    /// </summary>
    protected DataTable GetList(string strParentName)
    {
        string strHQL = "Select * From T_ProDocGraphControl ";
        if (!string.IsNullOrEmpty(strParentName) && strParentName.Trim() != "")
        {
            strHQL += " Where ParentName='" + strParentName.Trim() + "' ";
        }
        strHQL += " Order By SortNo ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProDocGraphControl");
        return ds.Tables[0];
    }

    protected void SetInterval(DropDownList DDL, string strParentName, string interval)
    {
        interval += "├";

        DataTable list = GetList(strParentName);
        if (list.Rows.Count > 0 && list != null)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                DDL.Items.Add(new ListItem(string.Format("{0}{1}", interval, list.Rows[i]["ClassificationName"].ToString().Trim()), list.Rows[i]["ClassificationName"].ToString().Trim()));

                ///递归
                SetInterval(DDL, list.Rows[i]["ClassificationName"].ToString().Trim(), interval);
            }
        }
    }

    /// <summary>
    /// 绑定收文登记中的文件编号及名称
    /// </summary>
    protected void LoadProReceiptRegistrationName()
    {
        string strHQL;
        IList lst;
        strHQL = "From ProReceiptRegistration as proReceiptRegistration Where proReceiptRegistration.ProjectID='" + strProjectID + "' Order By proReceiptRegistration.ID Desc";
        ProReceiptRegistrationBLL proReceiptRegistrationBLL = new ProReceiptRegistrationBLL();
        lst = proReceiptRegistrationBLL.GetAllProReceiptRegistrations(strHQL);
        ddl_DocumentNo.DataSource = lst;
        ddl_DocumentNo.DataBind();
        ddl_DocumentNo.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void ddl_FileNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int filenumG = 0, tablenumG = 0, figurenumG = 0;
        int filenumU = 0, tablenumU = 0, figurenumU = 0;
        string strHQL = "From ProGraphRegistration as proGraphRegistration Where proGraphRegistration.FileNo='" + ddl_FileNo.SelectedValue.Trim() + "' and proGraphRegistration.ProjectID='" + LB_ReqID.Text.Trim() + "' ";
        ProGraphRegistrationBLL proGraphRegistrationBLL = new ProGraphRegistrationBLL();
        IList lst = proGraphRegistrationBLL.GetAllProGraphRegistrations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProGraphRegistration proGraphRegistration = (ProGraphRegistration)lst[0];
            filenumG = proGraphRegistration.FileNum;
            tablenumG = proGraphRegistration.TableNum;
            figurenumG = proGraphRegistration.FigureNum;

            if (LB_SendFigureRegistrationID.Text.Trim() == "" || string.IsNullOrEmpty(LB_SendFigureRegistrationID.Text.Trim()))
                strHQL = "From ProSendFigureRegistration as proSendFigureRegistration Where proSendFigureRegistration.FileNo='" + ddl_FileNo.SelectedValue.Trim() + "' and char_length(rtrim(ltrim(proSendFigureRegistration.BackPer))) = 0 ";
            else
                strHQL = "From ProSendFigureRegistration as proSendFigureRegistration Where proSendFigureRegistration.FileNo='" + ddl_FileNo.SelectedValue.Trim() + "' and char_length(rtrim(ltrim(proSendFigureRegistration.BackPer))) = 0 and proSendFigureRegistration.ID<>'" + LB_SendFigureRegistrationID.Text.Trim() + "' ";
            ProSendFigureRegistrationBLL proSendFigureRegistrationBLL = new ProSendFigureRegistrationBLL();
            lst = proSendFigureRegistrationBLL.GetAllProSendFigureRegistrations(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    ProSendFigureRegistration proSendFigureRegistration = (ProSendFigureRegistration)lst[i];
                    filenumU += proSendFigureRegistration.FileNum;
                    tablenumU += proSendFigureRegistration.TableNum;
                    figurenumU += proSendFigureRegistration.FigureNum;
                }
            }
            lbl_FilePath.Visible = true;
            lbl_FilePath.Text = proGraphRegistration.FilePath.Trim();
            lbl_DocType.Text = proGraphRegistration.DocType.Trim();

            RP_SendGraph.DataSource = null;
            RP_SendGraph.DataBind();
        }
        NB_FileNum.Amount = filenumG - filenumU < 0 ? 0 : filenumG - filenumU;
        NB_TableNum.Amount = tablenumG - tablenumU < 0 ? 0 : tablenumG - tablenumU;
        NB_FigureNum1.Amount = figurenumG - figurenumU < 0 ? 0 : figurenumG - figurenumU;

        lbl_FileOld.Text = (filenumG - filenumU).ToString();
        lbl_TableOld.Text = (tablenumG - tablenumU).ToString();
        lbl_FigureOld.Text = (figurenumG - figurenumU).ToString();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowSendFigureRegistration','false') ", true);

    }

    protected void GetGraphNums(string strFileNo, string strSendId)
    {
        int filenumG = 0, tablenumG = 0, figurenumG = 0;
        int filenumU = 0, tablenumU = 0, figurenumU = 0;
        string strHQL = "From ProGraphRegistration as proGraphRegistration Where proGraphRegistration.FileNo='" + strFileNo.Trim() + "' and proGraphRegistration.ProjectID='" + LB_ReqID.Text.Trim() + "' ";
        ProGraphRegistrationBLL proGraphRegistrationBLL = new ProGraphRegistrationBLL();
        IList lst = proGraphRegistrationBLL.GetAllProGraphRegistrations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProGraphRegistration proGraphRegistration = (ProGraphRegistration)lst[0];
            filenumG = proGraphRegistration.FileNum;
            tablenumG = proGraphRegistration.TableNum;
            figurenumG = proGraphRegistration.FigureNum;

            if (string.IsNullOrEmpty(strSendId))
                strHQL = "From ProSendFigureRegistration as proSendFigureRegistration Where proSendFigureRegistration.FileNo='" + strFileNo.Trim() + "' and char_length(rtrim(ltrim(proSendFigureRegistration.BackPer))) = 0 ";
            else
                strHQL = "From ProSendFigureRegistration as proSendFigureRegistration Where proSendFigureRegistration.FileNo='" + strFileNo.Trim() + "' and char_length(rtrim(ltrim(proSendFigureRegistration.BackPer))) = 0 and proSendFigureRegistration.ID<>'" + strSendId + "' ";
            ProSendFigureRegistrationBLL proSendFigureRegistrationBLL = new ProSendFigureRegistrationBLL();
            lst = proSendFigureRegistrationBLL.GetAllProSendFigureRegistrations(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    ProSendFigureRegistration proSendFigureRegistration = (ProSendFigureRegistration)lst[i];
                    filenumU += proSendFigureRegistration.FileNum;
                    tablenumU += proSendFigureRegistration.TableNum;
                    figurenumU += proSendFigureRegistration.FigureNum;
                }
            }
        }
        lbl_FileOld.Text = (filenumG - filenumU).ToString();
        lbl_TableOld.Text = (tablenumG - tablenumU).ToString();
        lbl_FigureOld.Text = (figurenumG - figurenumU).ToString();
    }


    protected void GetProDocGraphControlData(string strDocTypeName, string strParentName, string strID)
    {
        if (strID.LastIndexOf("_") > -1)
        {
            strID = strID.Substring(strID.LastIndexOf("_") + 1);
        }


        string strHQL = "From ProDocGraphControl as proDocGraphControl Where proDocGraphControl.ID =" + strID;

        ProDocGraphControlBLL proDocGraphControlBLL = new ProDocGraphControlBLL();
        IList lst = proDocGraphControlBLL.GetAllProDocGraphControls(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProDocGraphControl proDocGraphControl = (ProDocGraphControl)lst[0];
            txtSortNo.Text = proDocGraphControl.SortNo.ToString();
            lbl_NewID.Text = proDocGraphControl.ID.ToString();
        }

        if (strDocTypeName.Contains(LanguageHandle.GetWord("Project").ToString().Trim()) && strDocTypeName.Contains(LanguageHandle.GetWord("WenKongLieBiao").ToString().Trim()))
        {
            txtParentName.Text = "";
            txtClassificationName.Text = "";

            BT_NewDocType.Enabled = false;
            BT_UpdateDocType.Enabled = false;
            BT_DeleteDocType.Enabled = false;
        }
        else
        {
            if (strDocTypeName.Contains("."))//判断是否含有.
            {
                if (strDocTypeName.IndexOf(".") == strDocTypeName.LastIndexOf("."))//判断是否是包含一个.，即是4种类型，收图登记、发图登记、收文登记、发文登记
                {
                    txtParentName.Text = strDocTypeName.Substring(strDocTypeName.LastIndexOf(".") + 1);
                    ddl_ParentName.SelectedValue = txtParentName.Text.Trim();
                    txtClassificationName.Text = "";
                    BT_NewDocType.Enabled = true;
                    BT_UpdateDocType.Enabled = false;
                    BT_DeleteDocType.Enabled = false;
                }
                else
                {
                    txtParentName.Text = strParentName.Substring(strParentName.LastIndexOf(".") + 1);
                    ddl_ParentName.SelectedValue = txtParentName.Text.Trim();
                    txtClassificationName.Text = strDocTypeName.Substring(strDocTypeName.LastIndexOf(".") + 1);
                    BT_NewDocType.Enabled = true;
                    BT_UpdateDocType.Enabled = true;
                    BT_DeleteDocType.Enabled = true;
                }
            }
            else
            {
                txtParentName.Text = "";
                txtClassificationName.Text = "";
                BT_NewDocType.Enabled = false;
                BT_UpdateDocType.Enabled = false;
                BT_DeleteDocType.Enabled = false;
            }
        }
    }

    protected void BindParentNameData(string strparentnameFirst)
    {
        if (strparentnameFirst == "1")
        {
            DataTable dt = GetList(LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim());
            if (dt != null && dt.Rows.Count > 0)
            {
                DataGrid3.CurrentPageIndex = 0;

                ddl_ParentName.Items.Clear();
                ddl_ParentName.Items.Insert(0, new ListItem(LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim(), LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim()));
                SetInterval(ddl_ParentName, LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim(), " ");
            }
            else
            {
                ddl_ParentName.Items.Clear();
                ddl_ParentName.Items.Insert(0, new ListItem(LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim(), LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim()));
            }
            txtParentName.Text = LanguageHandle.GetWord("ShouTuDengJi").ToString().Trim();
        }
        else if (strparentnameFirst == "3")
        {
            DataGrid2.CurrentPageIndex = 0;

            DataTable dt = GetList(LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim());
            if (dt != null && dt.Rows.Count > 0)
            {
                DataGrid1.CurrentPageIndex = 0;

                ddl_ParentName.Items.Clear();
                ddl_ParentName.Items.Insert(0, new ListItem(LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim(), LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim()));
                SetInterval(ddl_ParentName, LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim(), " ");
            }
            else
            {
                ddl_ParentName.Items.Clear();
                ddl_ParentName.Items.Insert(0, new ListItem(LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim(), LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim()));
            }
            txtParentName.Text = LanguageHandle.GetWord("ShouWenDengJi").ToString().Trim();
        }
        else if (strparentnameFirst == "2")
        {
            DataGrid1.CurrentPageIndex = 0;

            txtParentName.Text = LanguageHandle.GetWord("FaTuDengJi").ToString().Trim();
            ddl_ParentName.Items.Clear();
            ddl_ParentName.Items.Insert(0, new ListItem(LanguageHandle.GetWord("FaTuDengJi").ToString().Trim(), LanguageHandle.GetWord("FaTuDengJi").ToString().Trim()));
        }
        else if (strparentnameFirst == "4")
        {
            DataGrid5.CurrentPageIndex = 0;


            txtParentName.Text = LanguageHandle.GetWord("FaWenDengJi").ToString().Trim();
            ddl_ParentName.Items.Clear();
            ddl_ParentName.Items.Insert(0, new ListItem(LanguageHandle.GetWord("FaWenDengJi").ToString().Trim(), LanguageHandle.GetWord("FaWenDengJi").ToString().Trim()));
        }
    }

}
