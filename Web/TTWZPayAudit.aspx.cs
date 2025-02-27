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

public partial class TTWZPayAudit : System.Web.UI.Page
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
            DataPayBinder();
        }
    }

    private void DataPayBinder()
    {
        string strWZPayHQL = string.Format(@"select p.*,m.UserName as MarkerName from T_WZPay p
                    left join T_ProjectMember m on p.Marker = m.UserCode 
                    where p.Marker ='{0}' 
                    order by p.PayTime desc", strUserCode);
        DataTable dtPay = ShareClass.GetDataSetFromSql(strWZPayHQL, "Pay").Tables[0];

        DG_Pay.DataSource = dtPay;
        DG_Pay.DataBind();
    }


    protected void DG_Pay_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "audit")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZPayBLL wZPayBLL = new WZPayBLL();
                string strWZPayHQL = "from WZPay as wZPay where PayID = '" + cmdArges + "'";
                IList listWZPay = wZPayBLL.GetAllWZPays(strWZPayHQL);
                if (listWZPay != null && listWZPay.Count == 1)
                {
                    WZPay wZPay = (WZPay)listWZPay[0];

                    if (wZPay.Progress != LanguageHandle.GetWord("BaoPi").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBSBPZTBNPZ").ToString().Trim() + "')", true);
                        return;
                    }

                    wZPay.Progress = "Approved";

                    wZPayBLL.UpdateWZPay(wZPay, wZPay.PayID);

                    //重新加载预付款列表
                    DataPayBinder();
                }
            }
        }
    }
}