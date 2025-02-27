using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Data;
using System.Text;
using System.Drawing;

public partial class TTWZPayApproveList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataPayApproveBinder();
        }
    }

    private void DataPayApproveBinder()
    {
        DG_PayApprove.CurrentPageIndex = 0;

        string strWZPayApproveHQL = @"select a.*,m.UserName as MarkerName,p.UserName as ApproverName from T_WZPayApprove a
                        left join T_ProjectMember m on a.Marker = m.UserCode
                        left join T_ProjectMember p on a.Approver = p.UserCode 
                        order by a.ID desc";
        DataTable dtPayApprove = ShareClass.GetDataSetFromSql(strWZPayApproveHQL, "PayApprove").Tables[0];

        DG_PayApprove.DataSource = dtPayApprove;
        DG_PayApprove.DataBind();

        LB_PayApproveSql.Text = strWZPayApproveHQL;
    }


    protected void DG_PayApprove_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                for (int i = 0; i < DG_PayApprove.Items.Count; i++)
                {
                    DG_PayApprove.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                WZPayApproveBLL wZPayApproveBLL = new WZPayApproveBLL();
                string strWZPayApproveHQL = "from WZPayApprove as wZPayApprove where ID = " + cmdArges;
                IList listWZPayApprove = wZPayApproveBLL.GetAllWZPayApproves(strWZPayApproveHQL);
                if (listWZPayApprove != null && listWZPayApprove.Count == 1)
                {
                    WZPayApprove wZPayApprove = (WZPayApprove)listWZPayApprove[0];

                    if (wZPayApprove.Progress != LanguageHandle.GetWord("BaoPi").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWBPBYXBJ").ToString().Trim() + "')", true);
                        return;
                    }

                    TXT_ID.Text = wZPayApprove.ID.ToString();

                    TXT_ConfirmMoney.Text = wZPayApprove.ConfirmMoney.ToString();
                    TXT_PayTime.Text = wZPayApprove.PayTime.ToString("yyyy-MM-dd");
                }
            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZPayApproveBLL wZPayApproveBLL = new WZPayApproveBLL();
                string strWZPayApproveHQL = "from WZPayApprove as wZPayApprove where ID = " + cmdArges;
                IList listWZPayApprove = wZPayApproveBLL.GetAllWZPayApproves(strWZPayApproveHQL);
                if (listWZPayApprove != null && listWZPayApprove.Count == 1)
                {
                    WZPayApprove wZPayApprove = (WZPayApprove)listWZPayApprove[0];

                    if (wZPayApprove.Progress != LanguageHandle.GetWord("BaoPi").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWBPBYXBJ").ToString().Trim() + "')", true);
                        return;
                    }

                    wZPayApproveBLL.DeleteWZPayApprove(wZPayApprove);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
            }
            else if (cmdName == "approve")
            {
                //批准
                string cmdArges = e.CommandArgument.ToString();
                WZPayApproveBLL wZPayApproveBLL = new WZPayApproveBLL();
                string strWZPayApproveHQL = "from WZPayApprove as wZPayApprove where ID = " + cmdArges;
                IList listWZPayApprove = wZPayApproveBLL.GetAllWZPayApproves(strWZPayApproveHQL);
                if (listWZPayApprove != null && listWZPayApprove.Count == 1)
                {
                    WZPayApprove wZPayApprove = (WZPayApprove)listWZPayApprove[0];

                    if (wZPayApprove.Progress != LanguageHandle.GetWord("BaoPi").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBSBPZTBNPZ").ToString().Trim() + "')", true);
                        return;
                    }

                    wZPayApprove.Progress = "Approved";
                    wZPayApprove.Approver = strUserCode;

                    wZPayApproveBLL.UpdateWZPayApprove(wZPayApprove, wZPayApprove.ID);

                    //重新加载预付款列表
                    DataPayApproveBinder();
                }
            }
        }
    }



    protected void BT_Save_Click(object sender, EventArgs e)
    {
        try
        {
            string strID = TXT_ID.Text;
            if (!string.IsNullOrEmpty(strID))
            {
                //修改
                string strConfirmMoney = TXT_ConfirmMoney.Text.Trim();
                string strPayTime = TXT_PayTime.Text.Trim();

                if (string.IsNullOrEmpty(strConfirmMoney))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPZEDBNWKBC").ToString().Trim() + "')", true);
                    return;
                }
                if (string.IsNullOrEmpty(strPayTime))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFKRBNWKBC").ToString().Trim() + "')", true);
                    return;
                }

                decimal decimalConfirmMoney = 0;
                decimal.TryParse(strConfirmMoney, out decimalConfirmMoney);
                DateTime dtPayTime = DateTime.Now;
                DateTime.TryParse(strPayTime, out dtPayTime);

                WZPayApproveBLL wZPayApproveBLL = new WZPayApproveBLL();
                string strWZPayApproveHQL = "from WZPayApprove as wZPayApprove where ID = " + strID;
                IList listWZPayApprove = wZPayApproveBLL.GetAllWZPayApproves(strWZPayApproveHQL);
                if (listWZPayApprove != null && listWZPayApprove.Count == 1)
                {
                    WZPayApprove wZPayApprove = (WZPayApprove)listWZPayApprove[0];

                    wZPayApprove.ConfirmMoney = decimalConfirmMoney;
                    wZPayApprove.PayTime = dtPayTime;

                    wZPayApproveBLL.UpdateWZPayApprove(wZPayApprove, wZPayApprove.ID);

                    //重新加载
                    DataPayApproveBinder();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYXGDFKSPD").ToString().Trim() + "')", true);
            }
        }
        catch (Exception ex) { }
    }

    protected void BT_Reset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_PayApprove.Items.Count; i++)
        {
            DG_PayApprove.Items[i].ForeColor = Color.Black;
        }

        TXT_ID.Text = "";
        TXT_ConfirmMoney.Text = "";
        TXT_PayTime.Text = "";
    }
    protected void BT_Total_Click(object sender, EventArgs e)
    {
        try
        {
            WZPayApproveBLL wZPayApproveBLL = new WZPayApproveBLL();


            //查询当月的预付款计划
            string strAdvanceHQL = "select * from T_WZAdvance a where Progress = '报批' and SUBSTRING(to_char( AdvanceTime, 'yyyy-mm-dd'), 0, 8) = SUBSTRING(to_char( now(), 'yyyy-mm-dd'), 0, 8)"; 
            DataTable dtAdvance = ShareClass.GetDataSetFromSql(strAdvanceHQL, "Advance").Tables[0];
            if (dtAdvance != null && dtAdvance.Rows.Count > 0)
            {
                foreach (DataRow drAdvance in dtAdvance.Rows)
                {
                    WZPayApprove wZPayApprove = new WZPayApprove();

                    wZPayApprove.AdvanceCode = ShareClass.ObjectToString(drAdvance["AdvanceCode"]);
                    wZPayApprove.PayName = ShareClass.ObjectToString(drAdvance["AdvanceName"]);
                    string strProjectCode = ShareClass.ObjectToString(drAdvance["ProjectCode"]);
                    wZPayApprove.ProjectCode = strProjectCode;
                    wZPayApprove.ProjectName = GetProjectNameByProjectCode(strProjectCode);
                    decimal decimalPlanMoney = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drAdvance["AdvanceMoney"]), out decimalPlanMoney);
                    wZPayApprove.PlanMoney = decimalPlanMoney;
                    wZPayApprove.Marker = ShareClass.ObjectToString(drAdvance["Marker"]);
                    wZPayApprove.Progress = LanguageHandle.GetWord("BaoPi").ToString().Trim();
                    wZPayApprove.ConfirmMoney = 0;
                    wZPayApprove.PayTime = DateTime.Now;
                    wZPayApprove.Approver = "";

                    wZPayApproveBLL.AddWZPayApprove(wZPayApprove);
                }
            }

            //查询当月的付款计划
            string strPayHQL = "select * from T_WZPay a where Progress = '报批' and SUBSTRING(to_char( PayTime, 'yyyy-mm-dd'), 0, 8) = SUBSTRING(to_char( now(), 'yyyy-mm-dd'), 0, 8)"; 
            DataTable dtPay = ShareClass.GetDataSetFromSql(strPayHQL, "Pay").Tables[0];
            if (dtPay != null && dtPay.Rows.Count > 0)
            {
                foreach (DataRow drPay in dtPay.Rows)
                {
                    WZPayApprove wZPayApprove = new WZPayApprove();

                    wZPayApprove.AdvanceCode = ShareClass.ObjectToString(drPay["PayID"]);
                    wZPayApprove.PayName = ShareClass.ObjectToString(drPay["PayName"]);
                    string strProjectCode = ShareClass.ObjectToString(drPay["ProjectCode"]);
                    wZPayApprove.ProjectCode = strProjectCode;
                    wZPayApprove.ProjectName = GetProjectNameByProjectCode(strProjectCode);
                    decimal decimalPlanMoney = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drPay["PayTotal"]), out decimalPlanMoney);
                    wZPayApprove.PlanMoney = decimalPlanMoney;
                    wZPayApprove.Marker = ShareClass.ObjectToString(drPay["Marker"]);
                    wZPayApprove.Progress = LanguageHandle.GetWord("BaoPi").ToString().Trim();
                    wZPayApprove.ConfirmMoney = 0;
                    wZPayApprove.PayTime = DateTime.Now;
                    wZPayApprove.Approver = "";

                    wZPayApproveBLL.AddWZPayApprove(wZPayApprove);
                }
            }

            //重新加载付款审批列表
            DataPayApproveBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHZCG").ToString().Trim() + "')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHZSB").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        try
        {
            string strDeletePayApproveHQL = "truncate table T_WZPayApprove";
            ShareClass.RunSqlCommand(strDeletePayApproveHQL);

            //重新加载付款审批列表
            DataPayApproveBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSB").ToString().Trim() + "')", true);
        }
    }


    /// <summary>
    /// 根据项目编码获得项目名称
    /// </summary>
    private string GetProjectNameByProjectCode(string strProjectCode)
    {
        string strProjectName = string.Empty;
        try
        {
            WZProjectBLL wZProjectBLL = new WZProjectBLL();
            string strProjectHQL = string.Format("from WZProject as wZProject where ProjectCode = '{0}'", strProjectCode);
            IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);
            if (listProject != null && listProject.Count > 0)
            {
                WZProject wZProject = (WZProject)listProject[0];

                strProjectName = wZProject.ProjectName;
            }
        }
        catch (Exception ex) { }
        return strProjectName;
    }


    protected void DG_PayApprove_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_PayApprove.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_PayApproveSql.Text.Trim();
        DataTable dtPayApprove = ShareClass.GetDataSetFromSql(strHQL, "PayApprove").Tables[0];

        DG_PayApprove.DataSource = dtPayApprove;
        DG_PayApprove.DataBind();
    }
}
