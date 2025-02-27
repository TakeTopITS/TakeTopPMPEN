using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTBMPerformanceEvaluation : System.Web.UI.Page
{
    string strUserCode, strBidPlanID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strBidPlanID = Request.QueryString["BidPlanID"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            GetExpertList(strBidPlanID);
            LoadBMPerformanceEvaluationList(strBidPlanID);
        }
    }

    protected void GetExpertList(string strBidPlanID)
    {
        string strHQL = "From BMBidPlan as bMBidPlan where bMBidPlan.ID = '" + strBidPlanID + "'";
        BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
        IList lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
            if (string.IsNullOrEmpty(bMBidPlan.UserCodeList) || bMBidPlan.UserCodeList.Trim() == "")
            {
                if (string.IsNullOrEmpty(bMBidPlan.AddUserCodeList) || bMBidPlan.AddUserCodeList.Trim() == "")
                {
                    ddl_ExpertID.DataSource = null;
                    ddl_ExpertID.DataBind();
                    ddl_ExpertID.Items.Insert(0, new ListItem("--Select--", "0"));
                }
                else
                {
                    strHQL = "From WZExpert as wZExpert where wZExpert.ID in (" + bMBidPlan.AddUserCodeList.Trim() + ")";
                    WZExpertBLL wZExpertBLL = new WZExpertBLL();
                    lst = wZExpertBLL.GetAllWZExperts(strHQL);
                    ddl_ExpertID.DataSource = lst;
                    ddl_ExpertID.DataBind();
                    ddl_ExpertID.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(bMBidPlan.AddUserCodeList) || bMBidPlan.AddUserCodeList.Trim() == "")
                {
                    strHQL = "From WZExpert as wZExpert where wZExpert.ID in (" + bMBidPlan.UserCodeList.Trim() + ")";
                    WZExpertBLL wZExpertBLL = new WZExpertBLL();
                    lst = wZExpertBLL.GetAllWZExperts(strHQL);
                    ddl_ExpertID.DataSource = lst;
                    ddl_ExpertID.DataBind();
                    ddl_ExpertID.Items.Insert(0, new ListItem("--Select--", "0"));
                }
                else
                {
                    strHQL = "Select distinct ID,Name From T_WZExpert Where ID in (" + bMBidPlan.AddUserCodeList.Trim() + ") or ID in (" + bMBidPlan.UserCodeList.Trim() + ") Order By ID ";
                    DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZExpert");
                    ddl_ExpertID.DataSource = ds;
                    ddl_ExpertID.DataBind();
                    ddl_ExpertID.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
        }
    }

    protected void LoadBMPerformanceEvaluationList(string strBidPlanID)
    {
        string strHQL;

        strHQL = "Select * From T_BMPerformanceEvaluation Where BidPlanID='" + strBidPlanID + "' ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (CooperateDegree like '%" + TextBox1.Text.Trim() + "%' or Remark like '%" + TextBox1.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMPerformanceEvaluation");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text;

        if (strID == "")
        {
            Add();
        }
        else
        {
            Update();
        }
    }

    protected void Add()
    {
        if (string.IsNullOrEmpty(ddl_ExpertID.SelectedValue) || ddl_ExpertID.SelectedValue.Trim() == "0")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCPZJBNWKCZSBJC").ToString().Trim() + "')", true);
            ddl_ExpertID.Focus();
            return;
        }

        BMPerformanceEvaluationBLL bMPerformanceEvaluationBLL = new BMPerformanceEvaluationBLL();
        BMPerformanceEvaluation bMPerformanceEvaluation = new BMPerformanceEvaluation();
        bMPerformanceEvaluation.BidPlanID = int.Parse(strBidPlanID);
        bMPerformanceEvaluation.BidPlanName = GetBMBidPlanName(strBidPlanID);
        bMPerformanceEvaluation.CooperateDegree = DL_CooperateDegree.SelectedValue.Trim();
        bMPerformanceEvaluation.CreateTime = DateTime.Now;
        bMPerformanceEvaluation.EnterCode = strUserCode.Trim();
        bMPerformanceEvaluation.ExpertID = int.Parse(ddl_ExpertID.SelectedValue.Trim());
        bMPerformanceEvaluation.Remark = TB_Remark.Text.Trim();

        try
        {
            bMPerformanceEvaluationBLL.AddBMPerformanceEvaluation(bMPerformanceEvaluation);
            LB_ID.Text = GetMaxBMPerformanceEvaluationID(bMPerformanceEvaluation).ToString();

            UpdateWZExpertWorkingPoint(bMPerformanceEvaluation.ExpertID.ToString(), "1");

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            GetExpertList(bMPerformanceEvaluation.BidPlanID.ToString().Trim());
            LoadBMPerformanceEvaluationList(bMPerformanceEvaluation.BidPlanID.ToString().Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void UpdateWZExpertWorkingPoint(string struserid, string strType)
    {
        if (struserid.Trim() != "0" && !string.IsNullOrEmpty(struserid))
        {
            WZExpertBLL wZExpertBLL = new WZExpertBLL();
            string strHQL = "From WZExpert as wZExpert Where wZExpert.ID='" + struserid + "' ";
            IList lst = wZExpertBLL.GetAllWZExperts(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                WZExpert wZExpert = (WZExpert)lst[0];
                if (strType == "1")
                {
                    wZExpert.WorkingPoint = wZExpert.WorkingPoint + 1;
                }
                else if (strType == "-1")
                {
                    wZExpert.WorkingPoint = wZExpert.WorkingPoint - 1;
                }
                wZExpertBLL.UpdateWZExpert(wZExpert, wZExpert.ID);
            }
        }
    }

    protected int GetMaxBMPerformanceEvaluationID(BMPerformanceEvaluation bmbf)
    {
        string strHQL = "Select ID From T_BMPerformanceEvaluation where EnterCode='" + bmbf.EnterCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMPerformanceEvaluation").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected string GetBMBidPlanName(string strID)
    {
        string strHQL;
        IList lst;
        //绑定招标计划名称
        strHQL = "From BMBidPlan as bMBidPlan Where bMBidPlan.ID='" + strID + "' ";
        BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
        lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
            return bMBidPlan.Name.Trim();
        }
        else
            return "";
    }

    protected string GetExpertName(string strID)
    {
        string strHQL;
        IList lst;
        //绑定招标计划名称
        strHQL = "From WZExpert as wZExpert Where wZExpert.ID='" + strID + "' ";
        WZExpertBLL wZExpertBLL = new WZExpertBLL();
        lst = wZExpertBLL.GetAllWZExperts(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            WZExpert wZExpert = (WZExpert)lst[0];
            return wZExpert.Name.Trim();
        }
        else
            return "";
    }

    protected void Update()
    {
        string strHQL;
        IList lst;

        if (string.IsNullOrEmpty(ddl_ExpertID.SelectedValue) || ddl_ExpertID.SelectedValue.Trim() == "0")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCPZJBNWKCZSBJC").ToString().Trim() + "')", true);
            ddl_ExpertID.Focus();
            return;
        }

        BMPerformanceEvaluationBLL bMPerformanceEvaluationBLL = new BMPerformanceEvaluationBLL();
        strHQL = "from BMPerformanceEvaluation as bMPerformanceEvaluation where bMPerformanceEvaluation.ID = '" + LB_ID.Text.Trim() + "' ";
        lst = bMPerformanceEvaluationBLL.GetAllBMPerformanceEvaluations(strHQL);
        BMPerformanceEvaluation bMPerformanceEvaluation = (BMPerformanceEvaluation)lst[0];
        int strExpertIDOld = bMPerformanceEvaluation.ExpertID;
        bMPerformanceEvaluation.Remark = TB_Remark.Text.Trim();
        bMPerformanceEvaluation.ExpertID = int.Parse(ddl_ExpertID.SelectedValue.Trim());
        bMPerformanceEvaluation.CooperateDegree = DL_CooperateDegree.SelectedValue.Trim();

        try
        {
            bMPerformanceEvaluationBLL.UpdateBMPerformanceEvaluation(bMPerformanceEvaluation, bMPerformanceEvaluation.ID);

            if (strExpertIDOld != bMPerformanceEvaluation.ExpertID)
            {
                UpdateWZExpertWorkingPoint(strExpertIDOld.ToString(), "-1");
                UpdateWZExpertWorkingPoint(bMPerformanceEvaluation.ExpertID.ToString(), "1");
            }

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            GetExpertList(bMPerformanceEvaluation.BidPlanID.ToString().Trim());
            LoadBMPerformanceEvaluationList(bMPerformanceEvaluation.BidPlanID.ToString());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSB").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void Delete()
    {
        string strHQL;
        BMPerformanceEvaluationBLL bMPerformanceEvaluationBLL = new BMPerformanceEvaluationBLL();
        strHQL = "from BMPerformanceEvaluation as bMPerformanceEvaluation where bMPerformanceEvaluation.ID = '" + LB_ID.Text.Trim() + "' ";
        IList lst = bMPerformanceEvaluationBLL.GetAllBMPerformanceEvaluations(strHQL);
        BMPerformanceEvaluation bMPerformanceEvaluation = (BMPerformanceEvaluation)lst[0];

        strHQL = "delete from T_BMPerformanceEvaluation where ID = '" + LB_ID.Text.Trim() + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;

            UpdateWZExpertWorkingPoint(bMPerformanceEvaluation.ExpertID.ToString(), "-1");

            GetExpertList(bMPerformanceEvaluation.BidPlanID.ToString().Trim());
            LoadBMPerformanceEvaluationList(bMPerformanceEvaluation.BidPlanID.ToString());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSB").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMPerformanceEvaluationList(strBidPlanID.Trim());
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                strHQL = " from BMPerformanceEvaluation as bMPerformanceEvaluation where bMPerformanceEvaluation.ID = '" + strID + "' ";

                BMPerformanceEvaluationBLL bMPerformanceEvaluationBLL = new BMPerformanceEvaluationBLL();
                lst = bMPerformanceEvaluationBLL.GetAllBMPerformanceEvaluations(strHQL);
                BMPerformanceEvaluation bMPerformanceEvaluation = (BMPerformanceEvaluation)lst[0];

                LB_ID.Text = bMPerformanceEvaluation.ID.ToString();
                TB_Remark.Text = bMPerformanceEvaluation.Remark.Trim();
                ddl_ExpertID.SelectedValue = bMPerformanceEvaluation.ExpertID.ToString();
                DL_CooperateDegree.SelectedValue = bMPerformanceEvaluation.CooperateDegree.Trim();

                //if (bMPerformanceEvaluation.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Update.Visible = true;
                //    BT_Delete.Visible = true;
                //    BT_Update.Enabled = true;
                //    BT_Delete.Enabled = true;
                //}
                //else
                //{
                //    BT_Update.Visible = false;
                //    BT_Delete.Visible = false;
                //}
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            }


            if (e.CommandName == "Delete")
            {
                Delete();

            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = lbl_sql.Text.Trim();

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMPerformanceEvaluation");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
}