using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZSupplierInfo : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "供应商授权", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadWZSupplierInfoList();

            ShareClass.LoadWFTemplate(strUserCode, DL_WFType.SelectedValue.Trim(), DL_TemName);
        }
    }

    protected void LoadWZSupplierInfoList()
    {
        string strHQL;

        strHQL = "Select * From T_WZSupplierInfo Where 1=1";

        if (!string.IsNullOrEmpty(TXT_SSupplierName.Text.Trim()))
        {
            strHQL += " and SupplierName like '%" + TXT_SSupplierName.Text.Trim() + "%' ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZSupplierInfo");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TXT_SupplierPass.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGYHMMBNWKCZSBJC + "')", true);
            TXT_SupplierPass.Focus();
            return;
        }
        if (string.IsNullOrEmpty(TXT_SupplierName.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGYHMBNWKCZSBJC + "')", true);
            TXT_SupplierName.Focus();
            return;
        }

        WZSupplierInfoBLL wZSupplierInfoBLL = new WZSupplierInfoBLL();
        WZSupplierInfo wZSupplierInfo = new WZSupplierInfo();

        wZSupplierInfo.SupplierCode = DateTime.Now.ToString("yyMMddHHmmssfff");         //登录代码，暂时是年月日时分秒组成
        wZSupplierInfo.SupplierPass = TXT_SupplierPass.Text.Trim();
        wZSupplierInfo.SupplierName = TXT_SupplierName.Text.Trim();
        wZSupplierInfo.CreateTime = DateTime.Now;

        try
        {
            wZSupplierInfoBLL.AddWZSupplierInfo(wZSupplierInfo);

            LoadWZSupplierInfoList();

            BT_SubmitApply.Visible = true;
            BT_SubmitApply.Enabled = true;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXZCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXZSBJC + "')", true);
        }
    }


    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(LB_SupplierCode.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGXZGYSLBZJXXG + "')", true);
            // LB_SupplierCode.Focus();
            return;
        }
        if (string.IsNullOrEmpty(TXT_SupplierPass.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGYHMMBNWKCZSBJC + "')", true);
            TXT_SupplierPass.Focus();
            return;
        }
        if (string.IsNullOrEmpty(TXT_SupplierName.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGYHMBNWKCZSBJC + "')", true);
            TXT_SupplierName.Focus();
            return;
        }

        string strHQL = "From WZSupplierInfo as wZSupplierInfo where wZSupplierInfo.ID = " + HF_ID.Value.Trim();
        WZSupplierInfoBLL wZSupplierInfoBLL = new WZSupplierInfoBLL();
        IList lst = wZSupplierInfoBLL.GetAllWZSupplierInfos(strHQL);
        WZSupplierInfo wZSupplierInfo = (WZSupplierInfo)lst[0];

        wZSupplierInfo.SupplierPass = TXT_SupplierPass.Text.Trim();
        wZSupplierInfo.SupplierName = TXT_SupplierName.Text.Trim();

        try
        {
            wZSupplierInfoBLL.UpdateWZSupplierInfo(wZSupplierInfo, wZSupplierInfo.ID);

            LoadWZSupplierInfoList();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCSBJC + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID = HF_ID.Value.Trim();
        if (string.IsNullOrEmpty(strID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGXZGYSLBZJXXG + "')", true);
            return;
        }

        strHQL = "Delete From T_WZSupplierInfo Where ID = " + strID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            BT_SubmitApply.Visible = false;

            LoadWZSupplierInfoList();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSBJC + "')", true);
        }
    }


    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From WZSupplierInfo as wZSupplierInfo where wZSupplierInfo.ID = '" + strId + "'";
            WZSupplierInfoBLL wZSupplierInfoBLL = new WZSupplierInfoBLL();
            lst = wZSupplierInfoBLL.GetAllWZSupplierInfos(strHQL);

            WZSupplierInfo wZSupplierInfo = (WZSupplierInfo)lst[0];

            HF_ID.Value = wZSupplierInfo.ID.ToString().Trim();
            LB_SupplierCode.Text = wZSupplierInfo.SupplierCode.Trim();
            TXT_SupplierPass.Text = wZSupplierInfo.SupplierPass.Trim();
            TXT_SupplierName.Text = wZSupplierInfo.SupplierName.Trim();

            BT_Update.Visible = true;
            BT_Delete.Visible = true;
            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            BT_SubmitApply.Visible = true;
            BT_SubmitApply.Enabled = true;

            LoadRelatedWL(DL_WFType.SelectedValue.Trim(), "其它", int.Parse(HF_ID.Value.Trim()));
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZSupplierInfo");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadWZSupplierInfoList();
    }

    protected int LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType In (" + "'" + strRelatedType + "','其它')" + " and workFlow.RelatedID = " + intRelatedID.ToString();
        strHQL += " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        return lst.Count;
    }

    protected string SubmitApply()
    {
        string strCmdText, strID, strWLID, strXMLFileName, strXMLFile2, strTemName;

        strID = HF_ID.Value.Trim();
        strWLID = "0";

        strTemName = DL_TemName.SelectedValue.Trim();
        if (strTemName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZCWLCMBBNWKJC + "')", true);
            return strWLID;
        }

        XMLProcess xmlProcess = new XMLProcess();

        //strHQL = "Update T_CarApplyForm Set Status = '处理中' Where ID = " + strID;

        try
        {
            //ShareClass.RunSqlCommand(strHQL);

            strXMLFileName = DL_WFType.SelectedValue.Trim() + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
            strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;

            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            WorkFlow workFlow = new WorkFlow();

            workFlow.WLName = DL_WFType.SelectedValue.Trim();
            workFlow.WLType = DL_WFType.SelectedValue.Trim();
            workFlow.Status = "新建";
            workFlow.TemName = DL_TemName.SelectedValue.Trim();
            workFlow.CreateTime = DateTime.Now;
            workFlow.CreatorCode = strUserCode;
            workFlow.CreatorName = ShareClass.GetUserName(strUserCode.Trim());
            workFlow.Description = TXT_SupplierName.Text.Trim();
            workFlow.XMLFile = strXMLFile2;
            workFlow.RelatedType = "其它";
            workFlow.RelatedID = int.Parse(strID);
            workFlow.DIYNextStep = "Yes"; workFlow.IsPlanMainWorkflow = "NO";

            if (CB_SMS.Checked == true)
            {
                workFlow.ReceiveSMS = "Yes";
            }
            else
            {
                workFlow.ReceiveSMS = "No";
            }

            if (CB_Mail.Checked == true)
            {
                workFlow.ReceiveEMail = "Yes";
            }
            else
            {
                workFlow.ReceiveEMail = "No";
            }

            try
            {
                workFlowBLL.AddWorkFlow(workFlow);
                strWLID = ShareClass.GetMyCreatedWorkFlowID(strUserCode);

                strCmdText = "select * from T_BMBidPlan where ID = " + strID;

                strXMLFile2 = Server.MapPath(strXMLFile2);
                xmlProcess.DbToXML(strCmdText, "T_BMBidPlan", strXMLFile2);

                LoadRelatedWL(DL_WFType.SelectedValue.Trim(), "其它", int.Parse(strID));

                //DL_Status.SelectedValue = "处理中";

                BT_Update.Visible = false;
                BT_Delete.Visible = false;

                BT_SubmitApply.Visible = false;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZGYSSCCG + "')", true);
            }
            catch
            {
                strWLID = "0";

                BT_SubmitApply.Visible = true;
                BT_SubmitApply.Enabled = true;
                BT_Update.Visible = true;
                BT_Update.Enabled = true;
                BT_Delete.Visible = true;
                BT_Delete.Enabled = true;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('供应商生成" + Resources.lang.ZZSBJC + "')", true);
            }

            LoadWZSupplierInfoList();
        }
        catch
        {
            strWLID = "0";

            BT_SubmitApply.Visible = true;
            BT_SubmitApply.Enabled = true;
            BT_Update.Visible = true;
            BT_Update.Enabled = true;
            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('供应商生成" + Resources.lang.ZZSBJC + "')", true);
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

    protected int GetRelatedWorkFlowNumber(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        return lst.Count;
    }


    protected void BT_Reflash_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        string strtype = DL_WFType.SelectedValue.Trim();

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = '" + strtype + "'";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        DL_TemName.Items.Clear();
        if (lst.Count > 0 && lst != null)
        {
            DL_TemName.DataSource = lst;
            DL_TemName.DataBind();
        }
        else
        {
            DL_TemName.Items.Insert(0, new ListItem("--Select--", ""));
        }

        LoadRelatedWL(strtype, "其它", int.Parse(HF_ID.Value.Trim()));

        int intWLNumber = GetRelatedWorkFlowNumber(strtype, "其它", HF_ID.Value.Trim());

        if (intWLNumber > 0)
        {
            BT_SubmitApply.Visible = false;
        }
        else
        {
            BT_SubmitApply.Visible = true;
            BT_SubmitApply.Enabled = true;
        }
    }
}