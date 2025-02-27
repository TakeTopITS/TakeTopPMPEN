
using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZStoreList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "发料单", strUserCode);
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
        DG_List.CurrentPageIndex = 0;

        string strWZStoreHQL = @"select s.*,o.ObjectName from T_WZStore s
                    left join T_WZObject o on s.ObjectCode = o.ObjectCode";
        DataTable dtStore = ShareClass.GetDataSetFromSql(strWZStoreHQL, "Store").Tables[0];

        DG_List.DataSource = dtStore;
        DG_List.DataBind();

        LB_StoreSql.Text = strWZStoreHQL;

        #region 注释
        //DG_List.CurrentPageIndex = 0;

        //WZStoreBLL wZStoreBLL = new WZStoreBLL();
        //string strWZStoreHQL = "from WZStore as wZStore";
        //IList listWZStore = wZStoreBLL.GetAllWZStores(strWZStoreHQL);

        //DG_List.DataSource = listWZStore;
        //DG_List.DataBind();

        //LB_StoreSql.Text = strWZStoreHQL;
        #endregion
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZStoreBLL wZStoreBLL = new WZStoreBLL();
                string strWZStoreHQL = "from WZStore as wZStore where id = " + cmdArges;
                IList listWZStore = wZStoreBLL.GetAllWZStores(strWZStoreHQL);
                if (listWZStore != null && listWZStore.Count == 1)
                {
                    WZStore wZStore = (WZStore)listWZStore[0];

                    if (wZStore.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSYBJBW0BYXSC").ToString().Trim() + "')", true);
                        return;
                    }

                    wZStoreBLL.DeleteWZStore(wZStore);

                    //重新加载列表
                    DataBinder();


                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }

            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_StoreSql.Text.Trim();
        WZStoreBLL wZStoreBLL = new WZStoreBLL();
        IList listWZStore = wZStoreBLL.GetAllWZStores(strHQL);

        DG_List.DataSource = listWZStore;
        DG_List.DataBind();
    }
}