using System; using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Data;
using System.Drawing;

public partial class TTDWMatchList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataMatchBinder();

            DataMatchTypeBinder();
        }
    }


    private void DataMatchBinder()
    {
        DG_Match.CurrentPageIndex = 0;

        string strDWMatchHQL = @"select dWMatch.*,dWMatchType.MatchType as MatchTypeName from T_DWMatch as dWMatch
                           left join T_DWMatchType as dWMatchType on dWMatch.MatchType = dWMatchType.ID
                           order by dWMatch.ID desc";
        DataTable dtMatch = ShareClass.GetDataSetFromSql(strDWMatchHQL, "Match").Tables[0];

        DG_Match.DataSource = dtMatch;
        DG_Match.DataBind();

        LB_MatchSql.Text = strDWMatchHQL;
        #region ×¢ÊÍ
        //DWMatchBLL dWMatchBLL = new DWMatchBLL();
        //string strDWMatchHQL = "from DWMatch as dWMatch order by dWMatch.ID desc";
        //IList listDWMatch = dWMatchBLL.GetAllDWMatchs(strDWMatchHQL);

        //DG_Match.DataSource = listDWMatch;
        //DG_Match.DataBind();

        //LB_MatchSql.Text = strDWMatchHQL;
        #endregion
    }


    private void DataMatchTypeBinder()
    {
        DWMatchTypeBLL dWMatchTypeBLL = new DWMatchTypeBLL();
        string strDWMatchTypeHQL = "from DWMatchType as dWMatchType order by dWMatchType.ID desc";
        IList listDWMatchType = dWMatchTypeBLL.GetAllDWMatchTypes(strDWMatchTypeHQL);

        DDL_MatchType.DataSource = listDWMatchType;
        DDL_MatchType.DataBind();

        DDL_MatchType.Items.Insert(0, new ListItem("", ""));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DWMatchBLL dWMatchBLL = new DWMatchBLL();
            DWMatch dWMatch = new DWMatch();
            string strMatchName = TXT_MatchName.Text.Trim();
            string strMatchType = DDL_MatchType.SelectedValue;
            string strMaterialPrice = TXT_MaterialPrice.Text.Trim();

            if (string.IsNullOrEmpty(strMatchName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYLMCBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMatchName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYLMCBNSFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strMatchType))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZYLLX+"')", true);
                return;
            }

            string strCheckMatchHQL = string.Format("from DWMatch as dWMatch where MatchName = '{0}'", strMatchName);
            IList lstCheckMatch = dWMatchBLL.GetAllDWMatchs(strCheckMatchHQL);
            if (lstCheckMatch != null && lstCheckMatch.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYLMCYJCZGG+"')", true);
                return;
            }

            dWMatch.MatchName = strMatchName;
            int intMatchType = 0;
            int.TryParse(strMatchType, out intMatchType);
            dWMatch.MatchType = intMatchType;

            dWMatchBLL.AddDWMatch(dWMatch);

            string strSelectMaxHQL = "select ID from T_DWMatch order by ID desc limit 1";
            DataTable dtMaxID = ShareClass.GetDataSetFromSql(strSelectMaxHQL, "strSelectMaxHQL").Tables[0];
            if (dtMaxID != null && dtMaxID.Rows.Count > 0)
            {
                int intID = 0;
                int.TryParse(dtMaxID.Rows[0]["ID"] == DBNull.Value ? "0" : dtMaxID.Rows[0]["ID"].ToString(), out intID);
                TXT_ID.Text = intID.ToString();
            }

            DataMatchBinder();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
        }
        catch (Exception ex) { }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_Match.Items.Count; i++)
        {
            DG_Match.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "";
        TXT_ID.Text = "";
        TXT_MatchName.Text = "";
        DDL_MatchType.SelectedValue = "";
        TXT_MaterialPrice.Text = "";
    }


    protected void DG_Match_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            string cmdName = e.CommandName;
            if (cmdName == "del")
            {
                string strCmdArgu = e.CommandArgument.ToString();

                DWMatchBLL dWMatchBLL = new DWMatchBLL();
                string strDWMatchHQL = string.Format(@"from DWMatch as dWMatch where ID = " + strCmdArgu);
                IList lstDWMatch = dWMatchBLL.GetAllDWMatchs(strDWMatchHQL);
                if (lstDWMatch != null && lstDWMatch.Count > 0)
                {
                    DWMatch dWMatch = (DWMatch)lstDWMatch[0];

                    if (dWMatch.MaterialPrice != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCGBYJTXYLJGBNSC+"')", true);
                        return;
                    }

                    dWMatchBLL.DeleteDWMatch(dWMatch);

                    DataMatchBinder();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);
                }

            }
        }
        catch (Exception ex) { }
    }


    protected void DG_Match_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_Match.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_MatchSql.Text.Trim();
        DataTable dtMatch = ShareClass.GetDataSetFromSql(strHQL, "match").Tables[0];

        DG_Match.DataSource = dtMatch;
        DG_Match.DataBind();
        #region ×¢ÊÍ
        //DG_Match.CurrentPageIndex = e.NewPageIndex;
        //string strHQL = LB_MatchSql.Text.Trim();
        //DWMatchBLL dWMatchBLL = new DWMatchBLL();
        //IList listDWMatch = dWMatchBLL.GetAllDWMatchs(strHQL);

        //DG_Match.DataSource = listDWMatch;
        //DG_Match.DataBind();
        #endregion
    }


    protected void BT_MatchTypeAsc_Click(object sender, EventArgs e)
    {
        DG_Match.CurrentPageIndex = 0;

        string strDWMatchHQL = @"select dWMatch.*,dWMatchType.MatchType as MatchTypeName from T_DWMatch as dWMatch
                           left join T_DWMatchType as dWMatchType on dWMatch.MatchType = dWMatchType.ID
                           order by dWMatch.MatchType asc";
        DataTable dtMatch = ShareClass.GetDataSetFromSql(strDWMatchHQL, "Match").Tables[0];

        DG_Match.DataSource = dtMatch;
        DG_Match.DataBind();

        LB_MatchSql.Text = strDWMatchHQL;
    }


    protected void BT_MatchTypeDesc_Click(object sender, EventArgs e)
    {
        DG_Match.CurrentPageIndex = 0;

        string strDWMatchHQL = @"select dWMatch.*,dWMatchType.MatchType as MatchTypeName from T_DWMatch as dWMatch
                           left join T_DWMatchType as dWMatchType on dWMatch.MatchType = dWMatchType.ID
                           order by dWMatch.MatchType desc";
        DataTable dtMatch = ShareClass.GetDataSetFromSql(strDWMatchHQL, "Match").Tables[0];

        DG_Match.DataSource = dtMatch;
        DG_Match.DataBind();

        LB_MatchSql.Text = strDWMatchHQL;
    }
}