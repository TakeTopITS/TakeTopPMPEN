using System; using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Drawing;
using System.Data;

public partial class TTWZCompactLawList : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();
        }
    }


    private void DataBinder()
    {
        string strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        string strCompactHQL = string.Format(@"select c.*,s.SupplierName,
                    p.UserName as PurchaseEngineerName,
                    m.UserName as ControlMoneyName,
                    j.UserName as JuridicalPersonName,
                    d.UserName as DelegateAgentName,
                    t.UserName as CompacterName,
                    k.UserName as SafekeepName,
                    h.UserName as CheckerName
                    from T_WZCompact c
                    left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                    left join T_ProjectMember p on c.PurchaseEngineer = p.UserCode
                    left join T_ProjectMember m on c.ControlMoney = m.UserCode
                    left join T_ProjectMember j on c.JuridicalPerson = j.UserCode
                    left join T_ProjectMember d on c.DelegateAgent = d.UserCode
                    left join T_ProjectMember t on c.Compacter = t.UserCode
                    left join T_ProjectMember k on c.Safekeep = k.UserCode
                    left join T_ProjectMember h on c.Checker = h.UserCode 

                    where c.Progress in ('批准') 
                    order by c.MarkTime desc", strUserCode);
        //where c.JuridicalPerson = '{0}' 
        //and c.Progress in ('生效') 
        //order by c.MarkTime desc", strUserCode);
        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            for (int i = 0; i < DG_List.Items.Count; i++)
            {
                DG_List.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdName = e.CommandName;
            if (cmdName == "approve")
            {
                //批准
                string cmdArges = e.CommandArgument.ToString();
                WZCompactBLL wZCompactBLL = new WZCompactBLL();
                string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
                if (listCompact != null && listCompact.Count == 1)
                {
                    WZCompact wZCompact = (WZCompact)listCompact[0];
                    if (wZCompact.Progress == "生效")
                    {
                        wZCompact.Progress = "批准";
                        wZCompact.ApproveTime = DateTime.Now.ToString("yyyy-MM-dd");

                        wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZPZCG+"')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZHTJDBSCBZTBNPZ+"')", true);
                        return;
                    }
                }
            }
            else if (cmdName == "notApprove")
            {
                //退回
                string cmdArges = e.CommandArgument.ToString();
                WZCompactBLL wZCompactBLL = new WZCompactBLL();
                string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
                if (listCompact != null && listCompact.Count == 1)
                {
                    WZCompact wZCompact = (WZCompact)listCompact[0];
                    if (wZCompact.Progress == "生效")
                    {
                        wZCompact.Progress = "草签";
                        wZCompact.ApproveTime = "";

                        wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTHCG+"')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZHTJDBSCBZTBNTH+"')", true);
                        return;
                    }
                }
            }
        }
    }
}