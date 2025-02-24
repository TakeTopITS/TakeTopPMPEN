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

using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopWF;

public partial class TTRelatedWorkFlowTemplate : System.Web.UI.Page
{
    string strUserCode, strMakeUserCode;
    string strRelatedType, strRelatedID, strRelatedName;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserName;
        IList lst;

        strLangCode = Session["LangCode"].ToString();

        strRelatedType = Request.QueryString["RelatedType"].Trim();
        strRelatedID = Request.QueryString["RelatedID"].Trim();

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        if (strRelatedType == "Project")
        {
            strRelatedName = GetProjectName(strRelatedID);
            //this.Title = Resources.lang.Project + strRelatedID + " " + strRelatedName + " 的工作流模板设置";
        }

        if (strRelatedType == "ProjectTask")
        {
            strRelatedName = GetProjectTaskName(strRelatedID);
            //this.Title = "项目任务：" + strRelatedID + " " + strRelatedName + " 的工作流模板设置";
        }

        if (strRelatedType == "ProjectPlan")
        {
            strRelatedName = GetProjectPlanName(strRelatedID);
            //this.Title = "项目计划：" + strRelatedID + " " + strRelatedName + " 的工作流模板设置";
        }

        if (strRelatedType == "Req")
        {
            strRelatedName = GetRequirementName(strRelatedID);
            //this.Title  = "需求：" + strRelatedID + " " + strRelatedName + " 的工作流模板设置";
        }

        if (strRelatedType == "Meeting")
        {
            strRelatedName = GetMeetingName(strRelatedID);
            //this.Title = "会议：" + strRelatedID + " " + strRelatedName + " 的工作流模板设置";
        }

        if (strRelatedType == "ProjectRisk")
        {
            strRelatedName = GetProjectRiskName(strRelatedID);
            //this.Title = "风险：" + strRelatedID + " " + strRelatedName + " 的工作流模板设置";
        }

        if (strRelatedType == "Constract")
        {
            strRelatedName = GetConstractName(strRelatedID);
            //this.Title = "合同：" + GetConstractCode(strRelatedID) + " " + strRelatedName + "  的工作流模板设置";
        }

        if (strRelatedType == "Collaboration")
        {
            strRelatedName = GetCollaborationName(strRelatedID);
            //this.Title = "协作：" + strRelatedID + " " + strRelatedName + " 的工作流模板设置";
        }

        if (strRelatedType == "CustomerService")
        {
            strRelatedName = GetCustomerQuestionName(strRelatedID);
            //this.Title = Resources.lang.KEHUFUWU  + strRelatedID + " " + strRelatedName + " 的工作流模板设置";
        }


        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            if (strRelatedType == "Project")
            {
                TB_WFTemName.Text = Resources.lang.Project + strRelatedID + " " + "       评审流程";
            }

            if (strRelatedType == "ProjectTask")
            {
                TB_WFTemName.Text = "项目任务：" + strRelatedID + " " + "       评审流程";
            }

            if (strRelatedType == "ProjectPlan")
            {
                TB_WFTemName.Text = "项目计划：" + strRelatedID + " " + "       评审流程";

            }

            if (strRelatedType == "Req")
            {
                TB_WFTemName.Text = "需求：" + strRelatedID + " " + "       评审流程";

            }

            if (strRelatedType == "Meeting")
            {
                TB_WFTemName.Text = "会议：" + strRelatedID + " " + "       评审流程";

            }

            if (strRelatedType == "ProjectRisk")
            {
                TB_WFTemName.Text = "风险：" + strRelatedID + " " + "       评审流程";

            }

            strHQL = " from WLType as wlType";
            strHQL += " Where wlType.LangCode = " + "'" + strLangCode + "'";
            strHQL += " Order by wlType.SortNumber ASC";
            WLTypeBLL wlTypeBLL = new WLTypeBLL();
            lst = wlTypeBLL.GetAllWLTypes(strHQL);
            DL_WLType.DataSource = lst;
            DL_WLType.DataBind();
            DL_WLType.Items.Insert(0, new ListItem("--Select--", "0"));

            DL_NewWLType.DataSource = lst;
            DL_NewWLType.DataBind();
            DL_NewWLType.Items.Insert(0, new ListItem("--Select--", "0"));

            LoadRelatedWorkFlowTemplateByType(strRelatedType, strRelatedID, "0");
            LoadCommonWorkflowRelatedPage();

            Session["SuperWFAdmin"] = "NO";
        }
    }

    protected void BT_CreateWorkFlowTemplate_Click(object sender, EventArgs e)
    {
        string strWLType, strWorkFlowTemName;
        string strUserCode, strDepartCode, strDepartName;
        string strIdentifyString;


        strWLType = DL_WLType.SelectedValue.Trim();
        strWorkFlowTemName = TB_WFTemName.Text.Trim();

        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartName = ShareClass.GetDepartName(strDepartCode);

        strIdentifyString = DateTime.Now.ToString("yyyyMMddHHMMssff");

        if (strWorkFlowTemName != "")
        {
            WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
            WorkFlowTemplate workFlowTemplate = new WorkFlowTemplate();

            workFlowTemplate.TemName = strWorkFlowTemName;
            workFlowTemplate.Type = strWLType;
            workFlowTemplate.CreateTime = DateTime.Now;
            workFlowTemplate.CreatorCode = strUserCode;
            workFlowTemplate.CreatorName = ShareClass.GetUserName(strUserCode);
            workFlowTemplate.Status = "InUse";
            workFlowTemplate.Authority = "Part";
            workFlowTemplate.IdentifyString = strIdentifyString;

            workFlowTemplate.EnableEdit = "NO";
            workFlowTemplate.WFDefinition = "";

            workFlowTemplate.BelongDepartCode = strDepartCode;
            workFlowTemplate.BelongDepartName = strDepartName;

            workFlowTemplate.SortNumber = 0;
            workFlowTemplate.Visible = "YES";
            workFlowTemplate.AutoActive = "NO";
            workFlowTemplate.DesignType = "JS";

            workFlowTemplate.OverTimeAutoAgree = "NO";
            workFlowTemplate.OverTimeHourNumber = 24;

            workFlowTemplate.XSNFile = @"Template\CommonBusinessForm.xsn";
            workFlowTemplate.PageFile = "";

            workFlowTemplate.WFDefinition = "{states:{rect2:{type:'start',text:{text:'开始'}, attr:{ x:209, y:72, width:50, height:50}, props:{guid:{value:'4af6bc4b-7ed9-0b0b-e3a0-91c9d8fd92d1'},text:{value:'开始'}}}},paths:{},props:{props:{name:{value:'新建流程'},key:{value:''},desc:{value:''}}}}";


            try
            {
                workFlowTemplateBLL.AddWorkFlowTemplate(workFlowTemplate);

                RelatedWorkFlowTemplateBLL relatedWorkFlowTemplateBLL = new RelatedWorkFlowTemplateBLL();
                RelatedWorkFlowTemplate relatedWorkFlowTemplate = new RelatedWorkFlowTemplate();
                relatedWorkFlowTemplate.RelatedType = strRelatedType;
                relatedWorkFlowTemplate.RelatedID = int.Parse(strRelatedID);
                relatedWorkFlowTemplate.WFTemplateName = strWorkFlowTemName;
                relatedWorkFlowTemplate.IdentifyString = strIdentifyString;

                try
                {
                    relatedWorkFlowTemplateBLL.AddRelatedWorkFlowTemplate(relatedWorkFlowTemplate);
                    LoadRelatedWorkFlowTemplateByType(strRelatedType, strRelatedID, strWLType);

                    SelectWorkflowTemplateByTemName(strWorkFlowTemName);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ChengGong + "')", true);
                }
                catch
                {
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXZSBKNCXTMCMBJC + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZGZLMCBNWK + "')", true);
        }
    }


    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strTemName, strIdentifyString, strRelatedUserCode, strEnableEdit, strWFPage, strDesignType;

            strUserCode = LB_UserCode.Text.Trim();

            DataGrid2.CurrentPageIndex = 0;

            for (int i = 0; i < Repeater1.Items.Count; i++)
            {
                ((Button)Repeater1.Items[i].FindControl("BT_WorkFlowName")).ForeColor = Color.White;
            }
            ((Button)e.Item.FindControl("BT_WorkFlowName")).ForeColor = Color.Red;

            strTemName = ((Button)e.Item.FindControl("BT_WorkFlowName")).Text.Trim();

            WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
            strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
            lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
            WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

            strMakeUserCode = workFlowTemplate.CreatorCode.Trim();
            strIdentifyString = workFlowTemplate.IdentifyString.Trim();

            strEnableEdit = workFlowTemplate.EnableEdit.Trim();

            NB_SortNumber.Amount = workFlowTemplate.SortNumber;

            strWFPage = workFlowTemplate.PageFile.Trim();

            if (workFlowTemplate.XSNFile != "" & workFlowTemplate.XSNFile != null)
            {
                HL_XSNFile.Text = System.IO.Path.GetFileName(Server.MapPath(workFlowTemplate.XSNFile));
                HL_XSNFile.NavigateUrl = workFlowTemplate.XSNFile;
                HL_XSNFile.Target = "_blank";
            }
            else
            {
                HL_XSNFile.Text = "";
                HL_XSNFile.NavigateUrl = "";
                HL_XSNFile.Target = "_blank";
            }

            HL_Creator.NavigateUrl = "TTUserInforSimple.aspx?UserCode=" + workFlowTemplate.CreatorCode.Trim();
            HL_Creator.Text = workFlowTemplate.CreatorName.Trim();

            LoadWorkFlowStep(strTemName);
            LoadWorkFlowTStepOperator("0");

            LB_WFType.Text = workFlowTemplate.Type.Trim();

            LB_WFTemplate.Text = strTemName;
            LB_WFTemplate.Text = strTemName;

            strDesignType = workFlowTemplate.DesignType.Trim();
            if (strDesignType == "JS")
            {
                HL_WorkFlowDesigner.NavigateUrl = "TTWorkFlowDesignerJS.aspx?IdentifyString=" + strIdentifyString;
                HL_WorkFlowDesigner.Target = "_blank";
                HL_WorkFlowDesigner.Enabled = true;

                HL_WorkFlowDesigner.ForeColor = Color.Red;
            }

            if (strDesignType == "SL")
            {
                HL_WorkFlowDesigner.NavigateUrl = "TTWorkFlowDesignerSL.aspx?IdentifyString=" + strIdentifyString;
                HL_WorkFlowDesigner.Target = "_blank";
                HL_WorkFlowDesigner.Enabled = true;

                HL_WorkFlowDesigner.ForeColor = Color.Red;
            }

            LB_DesignWorkflowTemplate.Text = strTemName + " " + Resources.lang.LiuChengMuBan + Resources.lang.SheJi;

            strRelatedUserCode = LB_RelatedUserCode.Text.Trim();
            strUserCode = LB_UserCode.Text.Trim();
            LB_MakeUserCode.Text = strMakeUserCode;

            DL_EnableEdit.SelectedValue = strEnableEdit;

            DL_WFRelatedPage.SelectedValue = strWFPage;

            BT_CopyTem.Enabled = true;


            if (strUserCode != strMakeUserCode)
            {
                BT_UploadXSNFile.Enabled = false;
                BT_DeleteXSNFile.Enabled = false;

                BT_DeleteWFTemplate.Enabled = false;

                DL_EnableEdit.Enabled = false;

                DL_WFRelatedPage.Enabled = false;

                BT_SaveBelongDepartment.Enabled = false;

                BT_ChangeType.Enabled = false;
            }
            else
            {
                BT_UploadXSNFile.Enabled = true;
                BT_DeleteXSNFile.Enabled = true;

                BT_DeleteWFTemplate.Enabled = true;

                DL_EnableEdit.Enabled = true;

                DL_WFRelatedPage.Enabled = true;

                BT_SaveBelongDepartment.Enabled = true;

                BT_ChangeType.Enabled = true;
            }
        }
    }

    protected void BT_CopyTem_Click(object sender, EventArgs e)
    {
        string strOldTemName, strNewTemName, strWFType;

        strWFType = DL_WLType.SelectedValue.Trim();

        strOldTemName = LB_WFTemplate.Text.Trim();
        strNewTemName = TB_NewTemName.Text.Trim();

        if (strNewTemName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSBXMBMCBNKJC + "')", true);
            return;
        }

        try
        {
            string strNewIdentifyString = DateTime.Now.ToString("yyyyMMddHHMMssff");
            WFDataHandle.CopyWorkFlowTemplate(strWFType, strOldTemName, strNewTemName, strNewIdentifyString);

            RelatedWorkFlowTemplateBLL relatedWorkFlowTemplateBLL = new RelatedWorkFlowTemplateBLL();
            RelatedWorkFlowTemplate relatedWorkFlowTemplate = new RelatedWorkFlowTemplate();
            relatedWorkFlowTemplate.RelatedType = strRelatedType;
            relatedWorkFlowTemplate.RelatedID = int.Parse(strRelatedID);
            relatedWorkFlowTemplate.WFTemplateName = strNewTemName;
            relatedWorkFlowTemplate.IdentifyString = strNewIdentifyString;

            try
            {
                relatedWorkFlowTemplateBLL.AddRelatedWorkFlowTemplate(relatedWorkFlowTemplate);

                LoadRelatedWorkFlowTemplateByType(strRelatedType, strRelatedID, strWFType);
            }
            catch
            {
            }

            LoadRelatedWorkFlowTemplateByType(strRelatedType, strRelatedID, strWFType);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZFZCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZCWFZSBJC + "')", true);
        }
    }

    protected void BT_ChangeType_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strOldWLType, strNewWLType, strTemName;

        strOldWLType = LB_WFType.Text.Trim();
        strNewWLType = DL_NewWLType.SelectedValue.Trim();

        strTemName = LB_WFTemplate.Text.Trim();


        try
        {
            strHQL = "Update T_WorkFlowTemplate Set Type = '" + strNewWLType + "' Where TemName = '" + strTemName + "'";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Update T_WorkFlow Set WLType = '" + strNewWLType + "' Where TemName = '" + strTemName + "'";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Update T_WorkFlowBackup Set WLType = '" + strNewWLType + "' Where TemName = '" + strTemName + "'";
            ShareClass.RunSqlCommand(strHQL);

            LB_WFType.Text = strNewWLType;
            DL_WLType.SelectedValue = strNewWLType;
            LoadRelatedWorkFlowTemplateByType(strRelatedType, strRelatedID, strNewWLType);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBianGenChengGong + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBianGenShiBai + "')", true);
        }
    }

    protected void DL_EnableEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strTemName, strEnableEdit;

        strTemName = LB_WFTemplate.Text.Trim();
        strEnableEdit = DL_EnableEdit.SelectedValue.Trim();

        strHQL = "Update T_WorkFlowTemplate Set EnableEdit = " + "'" + strEnableEdit + "'" + " Where TemName = " + "'" + strTemName + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZGeiBianCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZGeiBianSBQJC + "')", true);
        }
    }

    protected void BT_DeleteWFTemplate_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strWFType, strTemName;

        strWFType = DL_WLType.SelectedValue.Trim();
        strTemName = LB_WFTemplate.Text.Trim();

        try
        {
            if (getWorkflowCountByTemNameAndCreatorCode(strTemName, strUserCode) == 0)
            {
                strHQL = "Delete From T_WorkFlowTemplate Where TemName = " + "'" + strTemName + "'" + " and Authority = 'Part'";
                ShareClass.RunSqlCommand(strHQL);
            }

            strHQL = "Delete From T_RelatedWorkFlowTemplate Where WFTemplateName = " + "'" + strTemName + "'" + " and RelatedType = " + "'" + strRelatedType + "'" + " and RelatedID = " + strRelatedID;
            ShareClass.RunSqlCommand(strHQL);

            LoadRelatedWorkFlowTemplateByType(strRelatedType, strRelatedID, strWFType);

            BT_DeleteWFTemplate.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCGZLMBCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGSCSBJC + "')", true);
        }
    }

    protected void BT_UploadXSNFile_Click(object sender, EventArgs e)
    {
        if (this.FUP_File.PostedFile != null)
        {
            string strFileName1 = FUP_File.PostedFile.FileName.Trim();
            string strTemName = LB_WFTemplate.Text.Trim();

            string strHQL;
            int i;


            if (strFileName1 != "")
            {
                //获取初始文件名
                i = strFileName1.LastIndexOf("."); //取得文件名中最后一个"."的索引
                string strNewExt = strFileName1.Substring(i); //获取文件扩展名

                DateTime dtUploadNow = DateTime.Now; //获取系统时间

                string strFileName2 = System.IO.Path.GetFileName(strFileName1);
                string strExtName = Path.GetExtension(strFileName2);
                strFileName2 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtName;

                string strDocSavePath = Server.MapPath("Doc") + "\\WorkFlowTemplate\\";
                string strFileName3 = "Doc\\" + "WorkFlowTemplate\\" + strFileName2;
                string strFileName4 = strDocSavePath + strFileName2;


                //上传加了版本号的自定义表单文件到服务器WorkFlowTemplate目录给工作流模块引用
                FileInfo fi = new FileInfo(strFileName4);
                if (fi.Exists)
                {
                    fi.Delete();
                }

                try
                {
                    FUP_File.PostedFile.SaveAs(strFileName4);

                    strHQL = "Update T_WorkFlowTemplate Set XSNFile = " + "'" + strFileName3 + "'" + " Where TemName = " + "'" + strTemName + "'";
                    ShareClass.RunSqlCommand(strHQL);

                    HL_XSNFile.Text = System.IO.Path.GetFileName(Server.MapPath(strFileName3));
                    HL_XSNFile.NavigateUrl = strFileName3;
                    HL_XSNFile.Target = "_blank";

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCHCG + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCSBJC + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZYSCDWJ + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZYSCDWJ + "')", true);
        }
    }

    protected void BT_DeleteXSNFile_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strTemName = LB_WFTemplate.Text.Trim();

        strHQL = "Update T_WorkFlowTemplate Set XSNFile = '' Where TemName =  " + "'" + strTemName + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            HL_XSNFile.Text = "";
            HL_XSNFile.NavigateUrl = "";
            HL_XSNFile.Target = "_blank";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCSBJC + "')", true);
        }
    }

    protected void DL_WFRelatedPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strTemName = LB_WFTemplate.Text.Trim();
        string strWFPageName = DL_WFRelatedPage.SelectedValue.Trim();

        if (strTemName == "")
        {
            return;
        }

        strHQL = "From WorkFlowTemplate as workFlowTemplate Where workFlowTemplate.TemName = " + "'" + strTemName + "'";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        if (strWFPageName != "")
        {
            workFlowTemplate.PageFile = strWFPageName;
            workFlowTemplate.XSNFile = "";
        }
        else
        {
            workFlowTemplate.PageFile = "";
            workFlowTemplate.XSNFile = @"Template\CommonBusinessForm.xsn";
        }

        if (workFlowTemplate.XSNFile != "" & workFlowTemplate.XSNFile != null)
        {
            HL_XSNFile.Text = System.IO.Path.GetFileName(Server.MapPath(workFlowTemplate.XSNFile));
            HL_XSNFile.NavigateUrl = workFlowTemplate.XSNFile;
            HL_XSNFile.Target = "_blank";
        }
        else
        {
            HL_XSNFile.Text = "";
            HL_XSNFile.NavigateUrl = "";
            HL_XSNFile.Target = "_blank";
        }

        try
        {
            workFlowTemplateBLL.UpdateWorkFlowTemplate(workFlowTemplate, strTemName);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCSBJC + "')", true);
        }
    }
    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID, strHQL;
            IList lst;

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            strHQL = "from WorkFlowTStepOperator as workFlowTStepOperator where workFlowTStepOperator.ID = " + strID;
            WorkFlowTStepOperatorBLL workFlowTStepOperatorBLL = new WorkFlowTStepOperatorBLL();
            lst = workFlowTStepOperatorBLL.GetAllWorkFlowTStepOperators(strHQL);
            WorkFlowTStepOperator workFlowTStepOperator = (WorkFlowTStepOperator)lst[0];

            LB_DetailID.Text = workFlowTStepOperator.ID.ToString();
            LBL_ActorGroup.Text = workFlowTStepOperator.ActorGroup;
            LBL_Requisite.Text = workFlowTStepOperator.Requisite.Trim();
            LBL_WorkDetail.Text = workFlowTStepOperator.WorkDetail;
            LBL_Actor.Text = workFlowTStepOperator.Actor.Trim();
            LBL_FinishedTime.Text = workFlowTStepOperator.LimitedTime.ToString();

            LBL_FieldList.Text = workFlowTStepOperator.FieldList.Trim();
            LBL_EditFieldList.Text = workFlowTStepOperator.EditFieldList.Trim();

            strMakeUserCode = LB_MakeUserCode.Text.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popwindowStepOperator') ", true);
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strSendSMS, strSendEMail;

        if (e.CommandName != "Page")
        {
            string strStepID = ((Button)e.Item.FindControl("BT_StepID")).Text.ToString();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string strTemName = LB_WFTemplate.Text.Trim();

            WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
            string strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.StepID =" + strStepID;
            IList lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

            WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

            LB_ID.Text = workFlowTStep.StepID.ToString();
            LB_SortNumber.Text = workFlowTStep.SortNumber.ToString();
            LBL_SortNumber.Text = workFlowTStep.SortNumber.ToString();
            LBL_StepName.Text = workFlowTStep.StepName;
            LBL_LimitedOperator.Text = workFlowTStep.LimitedOperator.ToString();
            LBL_LimitedTime.Text = workFlowTStep.LimitedTime.ToString();
            LBL_NextSortNumber.Text = workFlowTStep.NextSortNumber.ToString();

            LBL_NextStepMust.Text = workFlowTStep.NextStepMust.Trim();

            LBL_SelfReview.Text = workFlowTStep.SelfReview.Trim();
            LBL_IsPriorStepSelect.Text = workFlowTStep.IsPriorStepSelect.Trim();
            LBL_DepartRelated.Text = workFlowTStep.DepartRelated.Trim();
            LBL_PartTimeReview.Text = workFlowTStep.PartTimeReview.Trim();
            LBL_OperatorSelect.Text = workFlowTStep.OperatorSelect.Trim();
            LBL_ProjectRelated.Text = workFlowTStep.ProjectRelated.Trim();

            LBL_AllowSelfPass.Text = workFlowTStep.AllowSelfPass.Trim();
            LBL_AllowPriorOperatorPass.Text = workFlowTStep.AllowPriorOperatorPass.Trim();


            strSendSMS = workFlowTStep.SendSMS.Trim();
            strSendEMail = workFlowTStep.SendEMail.Trim();

            if (strSendSMS == "YES")
            {
                LBL_SendSMS.Text = "true";
            }
            else
            {
                LBL_SendSMS.Text = "false";
            }

            if (strSendEMail == "YES")
            {
                LBL_SendEMail.Text = "true";
            }
            else
            {
                LBL_SendEMail.Text = "false";
            }


            LB_StepID.Text = workFlowTStep.StepID.ToString().Trim();
            LB_StepName.Text = workFlowTStep.StepName.Trim();

            LoadWorkFlowTStepOperator(strStepID);

            strUserCode = LB_UserCode.Text.Trim();
            strMakeUserCode = LB_MakeUserCode.Text.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void BT_SaveBelongDepartment_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strTemName, strWFType;
        int intSortNumber;

        strWFType = DL_WLType.SelectedValue.Trim();
        strTemName = LB_WFTemplate.Text.Trim();
        intSortNumber = int.Parse(NB_SortNumber.Amount.ToString());

        try
        {
            strHQL = "Update T_WorkFlowTemplate Set SortNumber = " + intSortNumber.ToString() + " Where TemName = " + "'" + strTemName + "'";
            ShareClass.RunSqlCommand(strHQL);

            LoadRelatedWorkFlowTemplateByType(strRelatedType, strRelatedID, strWFType);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCCG + "')", true);
        }
        catch
        {
        }
    }

    protected void DL_WLType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strWLType;

        strWLType = DL_WLType.SelectedValue.Trim();

        LoadRelatedWorkFlowTemplateByType(strRelatedType, strRelatedID, strWLType);
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_SqlWL.Text;

        ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
        IList lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_SqlWL.Text;

        ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
        IList lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void SelectWorkflowTemplateByTemName(string strTemName)
    {
        string strHQL;
        IList lst;

        string strIdentifyString, strRelatedUserCode, strEnableEdit, strWFPage, strDesignType;

        strUserCode = LB_UserCode.Text.Trim();

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        strMakeUserCode = workFlowTemplate.CreatorCode.Trim();
        strIdentifyString = workFlowTemplate.IdentifyString.Trim();

        strEnableEdit = workFlowTemplate.EnableEdit.Trim();

        NB_SortNumber.Amount = workFlowTemplate.SortNumber;

        strWFPage = workFlowTemplate.PageFile.Trim();

        if (workFlowTemplate.XSNFile != "" & workFlowTemplate.XSNFile != null)
        {
            HL_XSNFile.Text = System.IO.Path.GetFileName(Server.MapPath(workFlowTemplate.XSNFile));
            HL_XSNFile.NavigateUrl = workFlowTemplate.XSNFile;
            HL_XSNFile.Target = "_blank";
        }
        else
        {
            HL_XSNFile.Text = "";
            HL_XSNFile.NavigateUrl = "";
            HL_XSNFile.Target = "_blank";
        }

        HL_Creator.NavigateUrl = "TTUserInforSimple.aspx?UserCode=" + workFlowTemplate.CreatorCode.Trim();
        HL_Creator.Text = workFlowTemplate.CreatorName.Trim();

        LoadWorkFlowStep(strTemName);
        LoadWorkFlowTStepOperator("0");

        LB_WFType.Text = workFlowTemplate.Type.Trim();

        LB_WFTemplate.Text = strTemName;
        LB_WFTemplate.Text = strTemName;

        strDesignType = workFlowTemplate.DesignType.Trim();
        if (strDesignType == "JS")
        {
            HL_WorkFlowDesigner.NavigateUrl = "TTWorkFlowDesignerJS.aspx?IdentifyString=" + strIdentifyString;
            HL_WorkFlowDesigner.Target = "_blank";
            HL_WorkFlowDesigner.Enabled = true;

            HL_WorkFlowDesigner.ForeColor = Color.Red;
        }

        if (strDesignType == "SL")
        {
            HL_WorkFlowDesigner.NavigateUrl = "TTWorkFlowDesignerSL.aspx?IdentifyString=" + strIdentifyString;
            HL_WorkFlowDesigner.Target = "_blank";
            HL_WorkFlowDesigner.Enabled = true;

            HL_WorkFlowDesigner.ForeColor = Color.Red;
        }

        LB_DesignWorkflowTemplate.Text = strTemName + " " + Resources.lang.LiuChengMuBan + Resources.lang.SheJi;

        strRelatedUserCode = LB_RelatedUserCode.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();
        LB_MakeUserCode.Text = strMakeUserCode;

        DL_EnableEdit.SelectedValue = strEnableEdit;

        DL_WFRelatedPage.SelectedValue = strWFPage;

        BT_CopyTem.Enabled = true;


        if (strUserCode != strMakeUserCode)
        {
            BT_UploadXSNFile.Enabled = false;
            BT_DeleteXSNFile.Enabled = false;

            BT_DeleteWFTemplate.Enabled = false;

            DL_EnableEdit.Enabled = false;

            DL_WFRelatedPage.Enabled = false;

            BT_SaveBelongDepartment.Enabled = false;

            BT_ChangeType.Enabled = false;
        }
        else
        {
            BT_UploadXSNFile.Enabled = true;
            BT_DeleteXSNFile.Enabled = true;

            BT_DeleteWFTemplate.Enabled = true;

            DL_EnableEdit.Enabled = true;

            DL_WFRelatedPage.Enabled = true;

            BT_SaveBelongDepartment.Enabled = true;

            BT_ChangeType.Enabled = true;
        }
    }

    //取得已用此模板的工作流数量
    protected int getWorkflowCountByTemNameAndCreatorCode(string strTemName, string strUserCode)
    {
        string strHQL;

        strHQL = string.Format(@"Select * From T_WorkFlow Where TemName = '{0}' and CreatorCode = '{1}'", strTemName, strUserCode);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");

        return ds.Tables[0].Rows.Count;
    }


    protected void LoadCommonWorkflowRelatedPage()
    {
        string strHQL;

        strHQL = "Select * From T_CommonWorkflowRelatedPage Where LangCode = '" + strLangCode + "' Order By ID ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CommonWorkflowRelatedPage");

        DL_WFRelatedPage.DataSource = ds;
        DL_WFRelatedPage.DataBind();

        DL_WFRelatedPage.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadRelatedWorkFlowTemplateByType(string strRelatedType, string strRelatedID, string strWFType)
    {
        string strHQL;
        IList lst;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();

        if (strWFType == "0")
        {
            strHQL = "from WorkFlowTemplate as workFlowTemplate where ";
            strHQL += " workFlowTemplate.TemName in (Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = " + "'" + strRelatedType + "'" + " and relatedWorkFlowTemplate.RelatedID = " + strRelatedID + ")";
            strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";

        }
        else
        {
            strHQL = "from WorkFlowTemplate as workFlowTemplate where ";
            strHQL += " workFlowTemplate.TemName in (Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = " + "'" + strRelatedType + "'" + " and relatedWorkFlowTemplate.RelatedID = " + strRelatedID + ")";
            strHQL += " and workFlowTemplate.Type = " + "'" + strWFType + "'";
            strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";

        }
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        Repeater1.DataSource = lst;
        Repeater1.DataBind();
    }


    protected void LoadWorkFlowStep(string strTemName)
    {
        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();

        string strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.TemName =" + "'" + strTemName + "'" + " order by workFlowTStep.SortNumber ASC";
        IList lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadWorkFlowTStepOperator(string strStepID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTStepOperator as workFlowTStepOperator where workFlowTStepOperator.StepID = " + strStepID;
        WorkFlowTStepOperatorBLL workFlowTStepOperatorBLL = new WorkFlowTStepOperatorBLL();

        lst = workFlowTStepOperatorBLL.GetAllWorkFlowTStepOperators(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }


    //生成角色组树
    public static void InitialActorGroupTree(TreeView TreeView, String strUserCode, string strRelatedType, string strRelatedID)
    {
        string strHQL, strActorGroupName;
        IList lst;

        //添加根节点
        TreeView.Nodes.Clear();

        TreeNode node0 = new TreeNode();
        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();
        TreeNode node3 = new TreeNode();
        TreeNode node4 = new TreeNode();

        node0.Text = "<B>角色组</B>";
        node0.Target = "0";
        node0.Expanded = true;
        TreeView.Nodes.Add(node0);

        strHQL = "from ActorGroup as actorGroup where ";
        strHQL += " actorGroup.GroupName in (Select relatedActorGroup.ActorGroupName from RelatedActorGroup as relatedActorGroup where relatedActorGroup.RelatedType = " + "'" + strRelatedType + "'" + " and relatedActorGroup.RelatedID = " + strRelatedID + ")";
        strHQL += " Order by actorGroup.IdentifyString DESC";
        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        lst = actorGroupBLL.GetAllActorGroups(strHQL);

        ActorGroup actorGroup = new ActorGroup();

        for (int i = 0; i < lst.Count; i++)
        {
            actorGroup = (ActorGroup)lst[i];

            strActorGroupName = actorGroup.GroupName.Trim();

            node3 = new TreeNode();

            node3.Text = strActorGroupName;
            node3.Target = strActorGroupName;
            node3.Expanded = true;

            node0.ChildNodes.Add(node3);
        }

        node2 = new TreeNode();
        node2.Text = "<B>所有</B>";
        node2.Target = "1";
        node2.Expanded = false;
        node0.ChildNodes.Add(node2);

        strHQL = "from ActorGroup as actorGroup where actorGroup.Type = 'All' ";
        strHQL += " Order by actorGroup.IdentifyString DESC";
        lst = actorGroupBLL.GetAllActorGroups(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            actorGroup = (ActorGroup)lst[i];

            strActorGroupName = actorGroup.GroupName.Trim();

            node4 = new TreeNode();

            node4.Text = strActorGroupName;
            node4.Target = strActorGroupName;
            node4.Expanded = true;

            node2.ChildNodes.Add(node4);
        }

        TreeView.DataBind();
    }



    protected string GetDepartName(string strDepartCode)
    {
        string strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        IList lst = departmentBLL.GetAllDepartments(strHQL);

        Department department = (Department)lst[0];

        return department.DepartName;
    }

    protected string GetMakeUserCode(string strTemName)
    {
        IList lst;
        string strHQL, strMakeUserCode;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];
        strMakeUserCode = workFlowTemplate.CreatorCode.Trim();

        return strMakeUserCode;
    }

    protected string GetIdentifyString(string strTemName)
    {
        IList lst;
        string strHQL, strIdentifyString;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];
        strIdentifyString = workFlowTemplate.IdentifyString.Trim();

        return strIdentifyString;
    }

    protected string GetWorkTemplateXSNFile(string strTemName)
    {
        IList lst;
        string strHQL, strXSNFile;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        try
        {
            strXSNFile = workFlowTemplate.XSNFile.Trim();
        }
        catch
        {
            strXSNFile = "";
        }

        return strXSNFile;
    }

    protected string GetWFDefinition(string strTemName)
    {
        IList lst;
        string strHQL, strWFDefinition;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        try
        {
            strWFDefinition = workFlowTemplate.WFDefinition.Trim();
        }
        catch
        {
            strWFDefinition = "";
        }

        return strWFDefinition;
    }

    protected string GetProjectName(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];
        string strProjectName = project.ProjectName.Trim();
        return strProjectName;
    }

    protected string GetActorGroupIdentityString(string strActorGroup)
    {
        string strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName = " + "'" + strActorGroup + "'";
        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        IList lst = actorGroupBLL.GetAllActorGroups(strHQL);

        ActorGroup actorGroup = (ActorGroup)lst[0];

        return actorGroup.IdentifyString.Trim();
    }

    protected string GetProjectTaskName(string strTaskID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        ProjectTask projectTask = (ProjectTask)lst[0];

        return projectTask.Task.Trim();
    }

    protected string GetProjectPlanName(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkPlan as workPlan where workPlan.ID = " + strPlanID;
        WorkPlanBLL workPlanBLL = new WorkPlanBLL();
        lst = workPlanBLL.GetAllWorkPlans(strHQL);
        WorkPlan workPlan = (WorkPlan)lst[0];

        return workPlan.Name.Trim();
    }

    protected string GetRequirementName(string strReqID)
    {
        string strHQL = "from Requirement as requirement where requirement.ReqID = " + strReqID;
        RequirementBLL requirementBLL = new RequirementBLL();

        IList lst = requirementBLL.GetAllRequirements(strHQL);

        Requirement requirement = (Requirement)lst[0];

        return requirement.ReqName.Trim();
    }

    protected string GetMeetingName(string strMeetingID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Meeting as meeting where meeting.ID = " + strMeetingID;
        MeetingBLL meetingBLL = new MeetingBLL();
        lst = meetingBLL.GetAllMeetings(strHQL);

        Meeting meeting = (Meeting)lst[0];

        return meeting.Name.Trim();

    }

    protected string GetProjectRiskName(string strRiskID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectRisk as projectRisk where projectRisk.ID = " + strRiskID;
        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        lst = projectRiskBLL.GetAllProjectRisks(strHQL);

        ProjectRisk projectRisk = (ProjectRisk)lst[0];

        return projectRisk.Risk.Trim();
    }

    protected string GetConstractCode(string strConstractID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where constract.ConstractID = " + strConstractID;

        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);

        Constract constract = (Constract)lst[0];

        return constract.ConstractCode.Trim();

    }

    protected string GetConstractName(string strConstractID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where constract.ConstractID = " + strConstractID;

        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);

        Constract constract = (Constract)lst[0];

        return constract.ConstractName.Trim();

    }

    protected string GetCollaborationName(string strCoID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strCoID;
        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);
        Collaboration collaboration = (Collaboration)lst[0];

        return collaboration.CollaborationName.Trim();
    }

    protected string GetCustomerQuestionName(string strQuestionID)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        return customerQuestion.Question.Trim();
    }


}
