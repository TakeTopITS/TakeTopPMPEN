using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZNeedObjectList : System.Web.UI.Page
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
        string strNeedObjectHQL = string.Format(@"select n.*,m.UserName as PurchaseEngineerName from T_WZNeedObject n
                    left join T_ProjectMember m on n.PurchaseEngineer = m.UserCode 
                    where n.PurchaseEngineer = '{0}' 
                    order by n.PurchaseEngineer", strUserCode);
        DataTable dtNeedObject = ShareClass.GetDataSetFromSql(strNeedObjectHQL, "NeedObject").Tables[0];

        DG_List.DataSource = dtNeedObject;
        DG_List.DataBind();

        LB_RecordCount.Text = dtNeedObject.Rows.Count.ToString();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZNeedObjectBLL wZNeedObjectBLL = new WZNeedObjectBLL();
                string strNeedObjectSql = "from WZNeedObject as wZNeedObject where ID = " + cmdArges;
                IList needObjectList = wZNeedObjectBLL.GetAllWZNeedObjects(strNeedObjectSql);
                if (needObjectList != null && needObjectList.Count == 1)
                {
                    WZNeedObject wZNeedObject = (WZNeedObject)needObjectList[0];
                    if (wZNeedObject.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYBJBW0BYXSC+"')", true);
                        return;
                    }

                    wZNeedObjectBLL.DeleteWZNeedObject(wZNeedObject);

                    //重新加载列表
                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);
                }

            }
        }
    }
}