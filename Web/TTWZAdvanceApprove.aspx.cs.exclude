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

public partial class TTWZAdvanceApprove : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","预付款计划审批", strUserCode);

        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataAdvanceBinder();
        }
    }

    private void DataAdvanceBinder()
    {
        string strWZAdvanceHQL = string.Format(@"select a.*,m.UserName as MarkerName from T_WZAdvance a
                    left join T_ProjectMember m on a.Marker = m.UserCode 
                    where a.Progress ='报批' 
                    order by a.AdvanceTime desc", strUserCode);
        DataTable dtWZAdvance = ShareClass.GetDataSetFromSql(strWZAdvanceHQL, "Advance").Tables[0];

        DG_Advance.DataSource = dtWZAdvance;
        DG_Advance.DataBind();
    }



    protected void DG_Advance_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "approve")
            {
                for (int i = 0; i < DG_Advance.Items.Count; i++)
                {
                    DG_Advance.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                WZAdvanceBLL wZAdvanceBLL = new WZAdvanceBLL();
                string strWZAdvanceHQL = "from WZAdvance as wZAdvance where AdvanceCode = '" + cmdArges + "'";
                IList listWZAdvance = wZAdvanceBLL.GetAllWZAdvances(strWZAdvanceHQL);
                if (listWZAdvance != null && listWZAdvance.Count == 1)
                {
                    WZAdvance wZAdvance = (WZAdvance)listWZAdvance[0];

                    if (wZAdvance.Progress != "报批")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSYBJBW0BYXBJ + "')", true);
                        return;
                    }

                    wZAdvance.Progress = "批准";

                    wZAdvanceBLL.UpdateWZAdvance(wZAdvance, wZAdvance.AdvanceCode);

                    //重新加载列表
                    DataAdvanceBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZPZCG + "')", true);
                }
            }
        }
    }
}