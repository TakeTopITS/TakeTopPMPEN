using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZUnitList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

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
        WZSpanBLL wZSpanBLL = new WZSpanBLL();
        string strWZSpanHQL = "from WZSpan as wZSpan order by wZSpan.ID desc";
        IList listWZSpan = wZSpanBLL.GetAllWZSpans(strWZSpanHQL);

        DG_List.DataSource = listWZSpan;
        DG_List.DataBind();

        LB_SQL.Text = strWZSpanHQL;

        LB_RecordCount.Text = listWZSpan.Count.ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        WZSpanBLL wZSpanBLL = new WZSpanBLL();
        WZSpan wZSpan = new WZSpan();
        string strUnitName = TXT_UnitName.Text.Trim();
        if (string.IsNullOrEmpty(strUnitName))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWMCBNWKBC").ToString().Trim()+"')", true);
            return;
        }
        else
        {
            if (!ShareClass.CheckStringRight(strUnitName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWMCBNWFFZFCXG").ToString().Trim()+"')", true);
                return;
            }
            if (strUnitName.Length > 4)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWMCBNCG4GZFXG").ToString().Trim()+"')", true);
                return;
            }
        }

        if (!string.IsNullOrEmpty(HF_ID.Value) && HF_ID.Value != "0")
        {
            string strID = HF_ID.Value;
            string strUnitHQL = string.Format(@"from WZSpan as wZSpan where ID = " + strID);
            IList lstUnit = wZSpanBLL.GetAllWZSpans(strUnitHQL);
            if (lstUnit != null && lstUnit.Count > 0)
            {
                wZSpan = (WZSpan)lstUnit[0];

                string strCheckUnitHQL = string.Format(@"from WZSpan as wZSpan where UnitName = '{0}'", strUnitName);
                IList lstCheckUnit = wZSpanBLL.GetAllWZSpans(strCheckUnitHQL);
                if (lstCheckUnit != null && lstCheckUnit.Count > 0)
                {
                    WZSpan wZCheckSpan = (WZSpan)lstCheckUnit[0];
                    if (wZCheckSpan.ID.ToString() != strID)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWMCYJCZBYZFTJ").ToString().Trim()+"')", true);
                        return;
                    }
                }

                wZSpan.UnitName = strUnitName;
                wZSpanBLL.UpdateWZSpan(wZSpan, int.Parse(strID));

                HF_ID.Value = "";
            }
        }
        else
        {

            string strCheckUnitHQL = string.Format("select count(1) as RowNumber from T_WZSpan where UnitName = '{0}'", strUnitName);
            DataTable dtCheckUnit = ShareClass.GetDataSetFromSql(strCheckUnitHQL, "CheckUnit").Tables[0];
            int intCheckUnitCount = 0;
            int.TryParse(ShareClass.ObjectToString(dtCheckUnit.Rows[0]["RowNumber"]), out intCheckUnitCount);
            if (intCheckUnitCount > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWMCYJCZBYZFTJ").ToString().Trim()+"')", true);
                return;
            }

            wZSpan.UnitName = strUnitName;
            wZSpanBLL.AddWZSpan(wZSpan);



            string strSelectMaxHQL = "select ID from T_WZSpan order by ID desc limit 1";
            DataTable dtMaxID = ShareClass.GetDataSetFromSql(strSelectMaxHQL, "strSelectMaxHQL").Tables[0];
            if (dtMaxID != null && dtMaxID.Rows.Count > 0)
            {
                int intID = 0;
                int.TryParse(dtMaxID.Rows[0]["ID"] == DBNull.Value ? "0" : dtMaxID.Rows[0]["ID"].ToString(), out intID);
                LB_ID.Text = intID.ToString();
            }
        }

        LB_ID.Text = "";
        TXT_UnitName.Text = "";
        TXT_UnitName.BackColor = Color.White;

        DataBinder();
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "";
        LB_ID.Text = "";
        TXT_UnitName.Text = "";
        TXT_UnitName.BackColor = Color.CornflowerBlue;


        //WZSpanBLL wZSpanBLL = new WZSpanBLL();
        //WZSpan wZSpan = new WZSpan();
        //string strUnitName = TXT_UnitName.Text.Trim();
        //if (string.IsNullOrEmpty(strUnitName))
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWMCBNWKBC").ToString().Trim()+"')", true);
        //    return;
        //}
        //else
        //{
        //    if (!ShareClass.CheckStringRight(strUnitName))
        //    {
        //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWMCBNWFFZFCXG").ToString().Trim()+"')", true);
        //        return;
        //    }
        //    if (strUnitName.Length > 4)
        //    {
        //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWMCBNCG4GZFXG").ToString().Trim()+"')", true);
        //        return;
        //    }
        //}



        //string strCheckUnitHQL = string.Format("select count(1) as RowNumber from T_WZSpan where UnitName = '{0}'", strUnitName);
        //DataTable dtCheckUnit = ShareClass.GetDataSetFromSql(strCheckUnitHQL, "CheckUnit").Tables[0];
        //int intCheckUnitCount = 0;
        //int.TryParse(ShareClass.ObjectToString(dtCheckUnit.Rows[0]["RowNumber"]), out intCheckUnitCount);
        //if (intCheckUnitCount > 0)
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWMCYJCZBYZFTJ").ToString().Trim()+"')", true);
        //    return;
        //}

        //wZSpan.UnitName = strUnitName;
        //wZSpanBLL.AddWZSpan(wZSpan);



        //string strSelectMaxHQL = "select top 1 ID from T_WZSpan order by ID desc";
        //DataTable dtMaxID = ShareClass.GetDataSetFromSql(strSelectMaxHQL, "strSelectMaxHQL").Tables[0];
        //if (dtMaxID != null && dtMaxID.Rows.Count > 0)
        //{
        //    int intID = 0;
        //    int.TryParse(dtMaxID.Rows[0]["ID"] == DBNull.Value ? "0" : dtMaxID.Rows[0]["ID"].ToString(), out intID);
        //    TXT_ID.Text = intID.ToString();
        //}


        //DataBinder();
        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
    }

    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        WZSpanBLL wZSpanBLL = new WZSpanBLL();
        WZSpan wZSpan = new WZSpan();
        string strUnitName = TXT_UnitName.Text.Trim();
        if (string.IsNullOrEmpty(strUnitName))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWMCBNWKBC").ToString().Trim()+"')", true);
            return;
        }
        else
        {
            if (!ShareClass.CheckStringRight(strUnitName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWMCBNWFFZFCXG").ToString().Trim()+"')", true);
                return;
            }
            if (strUnitName.Length > 4)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWMCBNCG4GZFXG").ToString().Trim()+"')", true);
                return;
            }
        }

        if (!string.IsNullOrEmpty(HF_ID.Value) && HF_ID.Value != "0")
        {
            string strID = HF_ID.Value;
            string strUnitHQL = string.Format(@"from WZSpan as wZSpan where ID = " + strID);
            IList lstUnit = wZSpanBLL.GetAllWZSpans(strUnitHQL);
            if (lstUnit != null && lstUnit.Count > 0)
            {
                wZSpan = (WZSpan)lstUnit[0];

                string strCheckUnitHQL = string.Format(@"from WZSpan as wZSpan where UnitName = '{0}'", strUnitName);
                IList lstCheckUnit = wZSpanBLL.GetAllWZSpans(strCheckUnitHQL);
                if (lstCheckUnit != null && lstCheckUnit.Count > 0)
                {
                    WZSpan wZCheckSpan = (WZSpan)lstCheckUnit[0];
                    if (wZCheckSpan.ID.ToString() != strID)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWMCYJCZBYZFTJ").ToString().Trim()+"')", true);
                        return;
                    }
                }

                wZSpan.UnitName = strUnitName;
                wZSpanBLL.UpdateWZSpan(wZSpan, int.Parse(strID));

                HF_ID.Value = "";
            }
        }
        else
        {
            //提示先选择要修改的单位
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZDWMC").ToString().Trim()+"')", true);
            return;

        }

        DataBinder();
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "0";
        LB_ID.Text = "";
        TXT_UnitName.Text = "";

        LB_ID.BackColor = Color.White;
        TXT_UnitName.BackColor = Color.White;

    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strCommandName = e.CommandName;
        if (strCommandName != "Page")
        {
            string strCmdArgu = e.CommandArgument.ToString();

            WZSpanBLL wZSpanBLL = new WZSpanBLL();
            if (strCommandName.Trim() == "edit")
            {
                for (int i = 0; i < DG_List.Items.Count; i++)
                {
                    DG_List.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strUnitHQL = string.Format(@"from WZSpan as wZSpan where ID = " + strCmdArgu);
                IList lstUnit = wZSpanBLL.GetAllWZSpans(strUnitHQL);
                if (lstUnit != null && lstUnit.Count > 0)
                {
                    WZSpan wZSpan = (WZSpan)lstUnit[0];

                    HF_ID.Value = wZSpan.ID.ToString();
                    LB_ID.Text = wZSpan.ID.ToString();
                    TXT_UnitName.Text = wZSpan.UnitName;

                    TXT_UnitName.BackColor = Color.CornflowerBlue;
                }
            }
            else if (strCommandName.Trim() == "del")
            {
                string strUnitHQL = string.Format(@"from WZSpan as wZSpan where ID = " + strCmdArgu);
                IList lstUnit = wZSpanBLL.GetAllWZSpans(strUnitHQL);
                if (lstUnit != null && lstUnit.Count > 0)
                {
                    WZSpan wZSpan = (WZSpan)lstUnit[0];

                    wZSpanBLL.DeleteWZSpan(wZSpan);

                    //重新加载
                    DG_List.CurrentPageIndex = 0;

                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                }
            }
        }
    }

    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        WZSpanBLL wZSpanBLL = new WZSpanBLL();
        string strWZSpanHQL = LB_SQL.Text;
        IList listWZSpan = wZSpanBLL.GetAllWZSpans(strWZSpanHQL);

        DG_List.DataSource = listWZSpan;
        DG_List.DataBind();
    }
}