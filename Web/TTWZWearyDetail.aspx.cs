using System; using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Data;
using System.Text;
using System.Collections;
using System.Drawing;

public partial class TTWZWearyDetail : System.Web.UI.Page
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
            DataWearyBinder();
        }
    }

    private void DataWearyBinder()
    {
        
        string strWZWearyHQL = string.Format(@"select w.*,m.UserName as MainLeaderName,r.UserName as MarkerName from T_WZWeary w
                    left join T_ProjectMember m on w.MainLeader = m.UserCode
                    left join T_ProjectMember r on w.Marker = r.UserCode 
                    where w.MainLeader = '{0}' 
                    order by w.PlanTime desc", strUserCode);
        DataTable dtWeary = ShareClass.GetDataSetFromSql(strWZWearyHQL, "Weary").Tables[0];

        DG_List.DataSource = dtWeary;
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
                WZWearyBLL wZWearyBLL = new WZWearyBLL();
                string strWZWearyHQL = "from WZWeary as wZWeary where WearyCode = '" + cmdArges + "'";
                IList listWZWeary = wZWearyBLL.GetAllWZWearys(strWZWearyHQL);
                if (listWZWeary != null && listWZWeary.Count == 1)
                {
                    WZWeary wZWeary = (WZWeary)listWZWeary[0];

                    if (wZWeary.Process != LanguageHandle.GetWord("BaoPi").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWBPZTBNPZ").ToString().Trim()+"')", true);
                        return;
                    }

                    wZWeary.Process = "Approved";

                    wZWearyBLL.UpdateWZWeary(wZWeary, wZWeary.WearyCode);

                    //重新加载列表
                    DataWearyBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPZCG").ToString().Trim()+"')", true);
                }
            }
        }
    }
}