using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZReduceEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {

            DataReduceBinder();
        }
    }

    private void DataReduceBinder()
    {
        string strWZReduceHQL = string.Format(@"select r.*,m.UserName as MainLeaderName,p.UserName as MarkerName from T_WZReduce r
                        left join T_ProjectMember m on r.MainLeader = m.UserCode
                        left join T_ProjectMember p on r.Marker = p.UserCode 
                        where r.MainLeader = '{0}' 
                        order by r.PlanTime desc", strUserCode);
        DataTable dtReduce = ShareClass.GetDataSetFromSql(strWZReduceHQL, "Reduce").Tables[0];

        DG_List.DataSource = dtReduce;
        DG_List.DataBind();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "submit")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZReduceBLL wZReduceBLL = new WZReduceBLL();
                string strWZReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + cmdArges + "'";
                IList listWZReduce = wZReduceBLL.GetAllWZReduces(strWZReduceHQL);
                if (listWZReduce != null && listWZReduce.Count == 1)
                {
                    WZReduce wZReduce = (WZReduce)listWZReduce[0];

                    if (wZReduce.Process != LanguageHandle.GetWord("BaoPi").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWBPZTBNPZ").ToString().Trim() + "')", true);
                        return;
                    }

                    wZReduce.Process = "Approved";

                    wZReduceBLL.UpdateWZReduce(wZReduce, wZReduce.ReduceCode);

                    //重新加载列表
                    DataReduceBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPZCG").ToString().Trim() + "')", true);
                }
            }
        }
    }
}