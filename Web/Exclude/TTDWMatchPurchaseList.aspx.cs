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

public partial class TTDWMatchPurchaseList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {

            DataYearMonthBinder();

            DataMatchBinder();
            //DDL_MatchType_SelectedIndexChanged(null, null);

            DataMatchTypeBinder();


            
        }

        
    }


    private void DataMatchBinder()
    {
        //DG_Match.CurrentPageIndex = 0;

        string strDWMatchHQL = @"select dWMatch.*,dWMatchType.MatchType as MatchTypeName from T_DWMatch as dWMatch
                           left join T_DWMatchType as dWMatchType on dWMatch.MatchType = dWMatchType.ID
                           order by dWMatch.ID desc";
        DataTable dtMatch = ShareClass.GetDataSetFromSql(strDWMatchHQL, "Match").Tables[0];

        DG_Match.DataSource = dtMatch;
        DG_Match.DataBind();

        LB_MatchSql.Text = strDWMatchHQL;
        #region 注释
        //DG_Match.CurrentPageIndex = 0;

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


        DDL_RMatchType.DataSource = listDWMatchType;
        DDL_RMatchType.DataBind();

        DDL_RMatchType.Items.Insert(0, new ListItem("", ""));
    }




    private void DataMatchHistoryTimeBinder()
    {
        string strDWMatchTypeHQL = "select * from T_DWMatchHistoryTime";
        DataTable dtDWMatchType = ShareClass.GetDataSetFromSql(strDWMatchTypeHQL, "MatchType").Tables[0];

        if (dtDWMatchType != null && dtDWMatchType.Rows.Count > 0)
        {
            string strYear = ShareClass.ObjectToString(dtDWMatchType.Rows[0]["HistoryYear"]);
            string strMonth = ShareClass.ObjectToString(dtDWMatchType.Rows[0]["HistoryMonth"]);

            DDL_HistoryYear.SelectedValue = strYear;
            DDL_HistoryMonth.SelectedValue = strMonth;
        }
    }


    private void DataYearMonthBinder()
    {
        DDL_HistoryYear.Items.Add(new ListItem(DateTime.Now.AddYears(-2).Year.ToString(), DateTime.Now.AddYears(-2).Year.ToString()));
        DDL_HistoryYear.Items.Add(new ListItem(DateTime.Now.AddYears(-1).Year.ToString(), DateTime.Now.AddYears(-1).Year.ToString()));
        DDL_HistoryYear.Items.Add(new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString()));
        DDL_HistoryYear.Items.Add(new ListItem(DateTime.Now.AddYears(1).Year.ToString(), DateTime.Now.AddYears(1).Year.ToString()));
        DDL_HistoryYear.Items.Add(new ListItem(DateTime.Now.AddYears(2).Year.ToString(), DateTime.Now.AddYears(2).Year.ToString()));
        DDL_HistoryYear.SelectedValue = DateTime.Now.Year.ToString();

        for (int i = 1; i <= 12; i++)
        {
            if (i < 10)
            {
                DDL_HistoryMonth.Items.Add(new ListItem("0" + i, "0" + i));
            }
            else
            {
                DDL_HistoryMonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }
        string strCurrentMonth = DateTime.Now.Month.ToString();
        if (strCurrentMonth.Length > 1)
        {
            DDL_HistoryMonth.SelectedValue = strCurrentMonth;
        }
        else
        {
            DDL_HistoryMonth.SelectedValue = "0" + strCurrentMonth;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            if (!string.IsNullOrEmpty(HF_ID.Value) && HF_ID.Value != "0")
            {
                string strID = HF_ID.Value;

                DWMatchBLL dWMatchBLL = new DWMatchBLL();
                DWMatch dWMatch = new DWMatch();
                string strMaterialPrice = TXT_MaterialPrice.Text.Trim();

                if (string.IsNullOrEmpty(strMaterialPrice))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYLJGBYXWKBC+"')", true);
                    return;
                }
                if (!ShareClass.CheckIsNumber(strMaterialPrice))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYLJGBXWXSHZZS+"')", true);
                    return;
                }

                string strWriteYear = DDL_HistoryYear.SelectedValue;
                string strWriteMonth = DDL_HistoryMonth.SelectedValue;
                

                string strDWMatchHQL = string.Format(@"from DWMatch as dWMatch where ID = " + strID);
                IList lstDWMatch = dWMatchBLL.GetAllDWMatchs(strDWMatchHQL);
                if (lstDWMatch != null && lstDWMatch.Count > 0)
                {
                    dWMatch = (DWMatch)lstDWMatch[0];

                    decimal decimalMaterialPrice = 0;
                    decimal.TryParse(strMaterialPrice, out decimalMaterialPrice);
                    dWMatch.MaterialPrice = decimalMaterialPrice;

                    dWMatchBLL.UpdateDWMatch(dWMatch, int.Parse(strID));



                    //把原料价格保存到历史数据表
                    DateTime dtWriteDate = DateTime.Now;
                    DateTime.TryParse(strWriteYear + "-" + strWriteMonth+"-01", out dtWriteDate);


                    string strCheckHistoryHQL = string.Format(@"select * from T_DWMatchHistory where to_char( CreateTime, 'yyyy-mm-dd' ) like '{0}%' and MatchID = {1}", dtWriteDate.ToString("yyyy-MM"), strID);
                    DataTable dtCheckHistory = ShareClass.GetDataSetFromSql(strCheckHistoryHQL, "CheckHistory").Tables[0];
                    if (dtCheckHistory != null && dtCheckHistory.Rows.Count > 0)
                    {
                        string strDWMatchHistoryID = ShareClass.ObjectToString(dtCheckHistory.Rows[0]["ID"]);

                        string strUpdateHistoryHQL = string.Format(@"update T_DWMatchHistory set MaterialPrice = {0}, CreateTime = '{1}' where ID = {2}", dWMatch.MaterialPrice, dtWriteDate, strDWMatchHistoryID);
                        ShareClass.RunSqlCommand(strUpdateHistoryHQL);
                    }
                    else {
                        string strInsertHistoryHQL = string.Format(@"insert into T_DWMatchHistory(MatchName,MatchType,MatchID,MaterialPrice,CreateTime,Remark)
                                                values('{0}',{1},{2},{3},'{4}','')", dWMatch.MatchName, dWMatch.MatchType, dWMatch.ID, dWMatch.MaterialPrice, dtWriteDate);
                        ShareClass.RunSqlCommand(strInsertHistoryHQL);
                    }



                    string strCheckHistoryTimeHQL = @"select * from T_DWMatchHistoryTime";
                    DataTable dtCheckHistoryTime = ShareClass.GetDataSetFromSql(strCheckHistoryTimeHQL, "CheckHistoryTime").Tables[0];
                    if (dtCheckHistoryTime != null && dtCheckHistoryTime.Rows.Count > 0)
                    {
                        string strDWMatchHistoryTimeID = ShareClass.ObjectToString(dtCheckHistoryTime.Rows[0]["ID"]);

                        string strUpdateHistoryTimeHQL = string.Format(@"update T_DWMatchHistoryTime set HistoryYear = '{0}', HistoryMonth = '{1}' where ID = {2}", strWriteYear, strWriteMonth, strDWMatchHistoryTimeID);
                        ShareClass.RunSqlCommand(strUpdateHistoryTimeHQL);
                    }
                    else
                    {
                        string strInsertHistoryTimeHQL = string.Format(@"insert into T_DWMatchHistoryTime(HistoryYear,HistoryMonth)
                                                values('{0}','{1}')", strWriteYear, strWriteMonth);
                        ShareClass.RunSqlCommand(strInsertHistoryTimeHQL);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXZYXGDYLJGLB+"')", true);
                return;
                #region 注释
                //string strCheckMatchHQL = string.Format("from DWMatch as dWMatch where MatchName = '{0}'", strMatchName);
                //IList lstCheckMatch = dWMatchBLL.GetAllDWMatchs(strCheckMatchHQL);
                //if (lstCheckMatch != null && lstCheckMatch.Count > 0)
                //{
                //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYLMCYJCZGG+"')", true);
                //    return;
                //}

                //dWMatch.MatchName = strMatchName;
                //decimal decimalMaterialPrice = 0;
                //decimal.TryParse(strMaterialPrice, out decimalMaterialPrice);
                //dWMatch.MaterialPrice = decimalMaterialPrice;

                //dWMatchBLL.AddDWMatch(dWMatch);

                //string strSelectMaxHQL = "select top 1 ID from T_DWMatch order by ID desc";
                //DataTable dtMaxID = ShareClass.GetDataSetFromSql(strSelectMaxHQL, "strSelectMaxHQL").Tables[0];
                //if (dtMaxID != null && dtMaxID.Rows.Count > 0)
                //{
                //    int intID = 0;
                //    int.TryParse(dtMaxID.Rows[0]["ID"] == DBNull.Value ? "0" : dtMaxID.Rows[0]["ID"].ToString(), out intID);
                //    TXT_ID.Text = intID.ToString();
                //}
                #endregion
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
        TXT_MaterialPrice.Text = "";
        DDL_MatchType.SelectedValue = "";
    }


    protected void DG_Match_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                for (int i = 0; i < DG_Match.Items.Count; i++)
                {
                    DG_Match.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strCmdArgu = e.CommandArgument.ToString();

                DWMatchBLL dWMatchBLL = new DWMatchBLL();
                string strDWMatchHQL = string.Format(@"from DWMatch as dWMatch where ID = " + strCmdArgu);
                IList lstDWMatch = dWMatchBLL.GetAllDWMatchs(strDWMatchHQL);
                if (lstDWMatch != null && lstDWMatch.Count > 0)
                {
                    DWMatch dWMatch = (DWMatch)lstDWMatch[0];

                    HF_ID.Value = dWMatch.ID.ToString();
                    TXT_ID.Text = dWMatch.ID.ToString();
                    TXT_MatchName.Text = dWMatch.MatchName;
                    DDL_RMatchType.SelectedValue = dWMatch.MatchType.ToString();
                    TXT_MaterialPrice.Text = dWMatch.MaterialPrice.ToString();

                    //历史价格时间
                    DataMatchHistoryTimeBinder();


                    string strSelectHistoryHQL = string.Format(@"select * from T_DWMatchHistory
                                where (DATE_PART('year', CreateTime::date) - DATE_PART('year',now()::date))*12+(DATE_PART('month',CreateTime::date)-DATE_PART('month',now()::date)) <= 5
                                and MatchID = {0}
                                order by CreateTime desc", dWMatch.ID);
                    DataTable dtHistory = ShareClass.GetDataSetFromSql(strSelectHistoryHQL, "History").Tables[0];

                    RPT_History.DataSource = dtHistory;
                    RPT_History.DataBind();
                }
            }
            else if (cmdName == "del")
            {
                string strCmdArgu = e.CommandArgument.ToString();

                DWMatchBLL dWMatchBLL = new DWMatchBLL();
                string strDWMatchHQL = string.Format(@"from DWMatch as dWMatch where ID = " + strCmdArgu);
                IList lstDWMatch = dWMatchBLL.GetAllDWMatchs(strDWMatchHQL);
                if (lstDWMatch != null && lstDWMatch.Count > 0)
                {
                    DWMatch dWMatch = (DWMatch)lstDWMatch[0];

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
        #region 注释
        //DG_Match.CurrentPageIndex = e.NewPageIndex;
        //string strHQL = LB_MatchSql.Text.Trim();
        //DWMatchBLL dWMatchBLL = new DWMatchBLL();
        //IList listDWMatch = dWMatchBLL.GetAllDWMatchs(strHQL);

        //DG_Match.DataSource = listDWMatch;
        //DG_Match.DataBind();
        #endregion
    }


    protected void DDL_MatchType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strMatchType = DDL_MatchType.SelectedValue;
        DG_Match.CurrentPageIndex = 0;

        DWMatchBLL dWMatchBLL = new DWMatchBLL();
        string strDWMatchHQL = @"select dWMatch.*,dWMatchType.MatchType as MatchTypeName from T_DWMatch as dWMatch
                           left join T_DWMatchType as dWMatchType on dWMatch.MatchType = dWMatchType.ID";
        if (!string.IsNullOrEmpty(strMatchType))
        {
            strDWMatchHQL += " where dWMatch.MatchType = " + strMatchType;
        }
        strDWMatchHQL += " order by dWMatch.ID desc";
        DataTable dtMatch = ShareClass.GetDataSetFromSql(strDWMatchHQL, "Match").Tables[0];

        DG_Match.DataSource = dtMatch;
        DG_Match.DataBind();

        LB_MatchSql.Text = strDWMatchHQL;
        #region 注释
        //string strMatchType = DDL_MatchType.SelectedValue;
        //DG_Match.CurrentPageIndex = 0;

        //DWMatchBLL dWMatchBLL = new DWMatchBLL();
        //string strDWMatchHQL = "from DWMatch as dWMatch ";
        //if (!string.IsNullOrEmpty(strMatchType))
        //{
        //    strDWMatchHQL += " where MatchType = " + strMatchType;
        //}
        //strDWMatchHQL += " order by dWMatch.ID desc";
        //IList listDWMatch = dWMatchBLL.GetAllDWMatchs(strDWMatchHQL);

        //DG_Match.DataSource = listDWMatch;
        //DG_Match.DataBind();

        //LB_MatchSql.Text = strDWMatchHQL;
        #endregion
    }


    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        try
        {
            string strClearHQL = "truncate table T_DWMatchHistory";

            ShareClass.RunSqlCommand(strClearHQL);


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);


        }
        catch (Exception ex)
        { 
        
        }
    }


    protected void BT_ClearMatch_Click(object sender, EventArgs e)
    {
        try
        {
            string strUpdateHQL = "update T_DWMatch set MaterialPrice = 0";

            ShareClass.RunSqlCommand(strUpdateHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKYLJGCG+"')", true);

            //重新加载列表
            DataMatchBinder();
        }
        catch (Exception ex)
        { }
    }
}