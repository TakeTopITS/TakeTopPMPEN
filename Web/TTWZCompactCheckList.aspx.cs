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
using System.Drawing;
using System.Data;

public partial class TTWZCompactCheckList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "�ڳ����ݵ���", strUserCode);
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
                        where c.Checker = '{0}' 
                        and c.Progress in ('��Ч','�ļ�') 
                        order by c.MarkTime desc", strUserCode);
        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        e.Item.ForeColor = Color.Red;

        string cmdName = e.CommandName;
        if (cmdName == "check")
        {
            //�ļ�
            string cmdArges = e.CommandArgument.ToString();
            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
            IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
            if (listCompact != null && listCompact.Count == 1)
            {
                WZCompact wZCompact = (WZCompact)listCompact[0];
                if (wZCompact.Progress == "��Ч")
                {
                    wZCompact.CheckIsMark = -1;
                    wZCompact.Progress = "�ļ�";

                    wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZCJCG + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZHTJDBSSXZTBNCJ + "')", true);
                    return;
                }
            }
        }
        else if (cmdName == "notCheck")
        {
            //�˻�
            string cmdArges = e.CommandArgument.ToString();
            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
            IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
            if (listCompact != null && listCompact.Count == 1)
            {
                WZCompact wZCompact = (WZCompact)listCompact[0];
                if (wZCompact.Progress == "�ļ�")
                {
                    wZCompact.CheckIsMark = 0;
                    wZCompact.Progress = "��Ч";

                    wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZTHCG + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZHTJDBSCJZTBNTH + "')", true);
                    return;
                }
            }
        }
        else if (cmdName == "detail")
        {
            //��ϸ
            string cmdArges = e.CommandArgument.ToString();
            Response.Redirect("TTWZCompactCheckDetail.aspx?CompactCode=" + cmdArges);
        }
    }
}