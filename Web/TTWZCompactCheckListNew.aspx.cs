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

public partial class TTWZCompactCheckListNew : System.Web.UI.Page
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
            DataCompactCheckBinder();

            DataCompactBinder();

            DataCheckCodeBinder();
        }
    }

    private void DataCompactCheckBinder()
    {
        string strCompactCheckHQL = string.Format(@"select c.*,s.UnitName from T_WZCompactCheck c
                    left join T_WZSpan s on c.Unit = s.ID
                    where c.Checker = '{0}' ", strUserCode);

        string strCompactCode = DDL_Compact.SelectedValue;
        if (!string.IsNullOrEmpty(strCompactCode))
        {
            strCompactCheckHQL += " and c.CompactCode = '" + strCompactCode + "' ";
        }
        string strCheckCode = DDL_Check.SelectedValue;
        if (!string.IsNullOrEmpty(strCheckCode))
        {
            strCompactCheckHQL += " and c.CheckCode = '" + strCheckCode + "' ";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactCheckHQL, "Compact").Tables[0];

        DataGrid1.DataSource = dtCompact;
        DataGrid1.DataBind();

        LB_Sql.Text = strCompactCheckHQL;

        LB_CheckRecord.Text = dtCompact.Rows.Count.ToString();
    }

    public void DataCompactBinder()
    {
        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strWZCompactHQL = "from WZCompact as wZCompact where Progress in ('生效','材检') and Checker = '" + strUserCode + "' order by CompactCode desc";   //ChineseWord
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactHQL);

        LB_Compact.DataSource = listCompact;
        LB_Compact.DataValueField = "CompactCode";
        LB_Compact.DataTextField = "CompactCode";
        LB_Compact.DataBind();

        DDL_Compact.DataSource = listCompact;
        DDL_Compact.DataValueField = "CompactCode";
        DDL_Compact.DataTextField = "CompactCode";
        DDL_Compact.DataBind();

        DDL_Compact.Items.Insert(0, new ListItem("", ""));
    }

    public void DataCheckCodeBinder()
    {
        string strCheckSQL = @"select CheckCode from T_WZCompactCheck
                    group by CheckCode";
        DataTable dtCheck = ShareClass.GetDataSetFromSql(strCheckSQL, "Check").Tables[0];

        DDL_Check.DataSource = dtCheck;
        DDL_Check.DataValueField = "CheckCode";
        DDL_Check.DataTextField = "CheckCode";
        DDL_Check.DataBind();

        DDL_Check.Items.Insert(0, new ListItem("", ""));
    }

    public void DataCompactDetailBinder()
    {
        string strCompactCode = LB_Compact.SelectedValue;
        if (!string.IsNullOrEmpty(strCompactCode))
        {
            string strWZCompactDetailHQL = string.Format(@"select d.*,o.ObjectName,o.Model,o.Grade,o.Criterion,p.PurchaseCode,l.PlanCode,s.UnitName from T_WZCompactDetail d
                            left join T_WZObject o on d.ObjectCode = o.ObjectCode 
                            left join T_WZPurchaseDetail p on d.PurchaseDetailID = p.ID
                            left join T_WZPickingPlanDetail l on d.PlanDetailID = l.ID
                            left join T_WZSpan s on o.Unit = s.ID
                            where d.CompactCode = '{0}'
                            order by o.DLCode,o.ObjectName,o.Model", strCompactCode);
            //and d.IsCheck = 0
            DataTable dtCompactDetail = ShareClass.GetDataSetFromSql(strWZCompactDetailHQL, "CompactDetail").Tables[0];

            DG_CompactDetail.DataSource = dtCompactDetail;
            DG_CompactDetail.DataBind();

            LB_CompactNumber.Text = dtCompactDetail.Rows.Count.ToString();
        }
    }

    protected void LB_Compact_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(LB_Compact.SelectedValue))
        {
            DataCompactDetailBinder();
        }
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {

        for (int i = 0; i < DataGrid1.Items.Count; i++)
        {
            DataGrid1.Items[i].ForeColor = Color.Black;
        }

        e.Item.ForeColor = Color.Red;

        string cmdName = e.CommandName;
        if (cmdName == "click")
        {
            //操作
            string cmdArges = e.CommandArgument.ToString();
            WZCompactCheckBLL wZCompactCheckBLL = new WZCompactCheckBLL();
            string strWZCompactCheckSql = "from WZCompactCheck as wZCompactCheck where ID = " + cmdArges;
            IList listWZCompactCheck = wZCompactCheckBLL.GetAllWZCompactChecks(strWZCompactCheckSql);
            if (listWZCompactCheck != null && listWZCompactCheck.Count == 1)
            {
                WZCompactCheck wZCompactCheck = (WZCompactCheck)listWZCompactCheck[0];

                ControlStatusChange(wZCompactCheck.CompactDetailID.ToString(), wZCompactCheck.Progress);

                HF_NewID.Value = wZCompactCheck.ID.ToString();
                HF_NewProgress.Value = wZCompactCheck.Progress;
                HF_NewCompactDetailID.Value = wZCompactCheck.CompactDetailID.ToString();
            }
        }
        else if (cmdName == "check")
        {
            //材检
            string cmdArges = e.CommandArgument.ToString();
            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
            IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
            if (listCompact != null && listCompact.Count == 1)
            {
                WZCompact wZCompact = (WZCompact)listCompact[0];
                if (wZCompact.Progress == LanguageHandle.GetWord("ShengXiao").ToString().Trim())
                {
                    wZCompact.CheckIsMark = -1;
                    wZCompact.Progress = LanguageHandle.GetWord("CaiJian").ToString().Trim();

                    wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                    DataCompactCheckBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCJCG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTJDBSSXZTBNCJ").ToString().Trim() + "')", true);
                    return;
                }
            }
        }
        else if (cmdName == "notCheck")
        {
            //退回
            string cmdArges = e.CommandArgument.ToString();
            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
            IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
            if (listCompact != null && listCompact.Count == 1)
            {
                WZCompact wZCompact = (WZCompact)listCompact[0];
                if (wZCompact.Progress == LanguageHandle.GetWord("CaiJian").ToString().Trim())
                {
                    wZCompact.CheckIsMark = 0;
                    wZCompact.Progress = LanguageHandle.GetWord("ShengXiao").ToString().Trim();

                    wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                    DataCompactCheckBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTHCG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTJDBSCJZTBNTH").ToString().Trim() + "')", true);
                    return;
                }
            }
        }
        else if (cmdName == "detail")
        {
            //明细
            string cmdArges = e.CommandArgument.ToString();
            Response.Redirect("TTWZCompactCheckDetail.aspx?CompactCode=" + cmdArges);
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        string strCompactHQL = LB_Sql.Text;
        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DataGrid1.DataSource = dtCompact;
        DataGrid1.DataBind();

        ControlStatusCloseChange();
    }

    protected void DG_CompactDetail_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        for (int i = 0; i < DG_CompactDetail.Items.Count; i++)
        {
            DG_CompactDetail.Items[i].ForeColor = Color.Black;
        }

        e.Item.ForeColor = Color.Red;

        string cmdName = e.CommandName;
        if (cmdName == "add")
        {
            //添加
            string cmdArges = e.CommandArgument.ToString();
            WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
            string strCompactDetailSql = "from WZCompactDetail as wZCompactDetail where ID = " + cmdArges;
            IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strCompactDetailSql);
            if (listCompactDetail != null && listCompactDetail.Count == 1)
            {
                WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

                string strCompactSql = @"select c.*,s.SupplierName,p.ProjectName from T_WZCompact c
                            left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                            left join T_WZProject p on c.ProjectCode = p.ProjectCode 
                            where CompactCode = '" + wZCompactDetail.CompactCode + "'";
                DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactSql, "Compact").Tables[0];
                if (dtCompact != null && dtCompact.Rows.Count == 1)
                {
                    DataRow drCompact = dtCompact.Rows[0];

                    WZCompactCheckBLL wZCompactCheckBLL = new WZCompactCheckBLL();
                    WZCompactCheck wZCompactCheck = new WZCompactCheck();

                    wZCompactCheck.CompactCode = wZCompactDetail.CompactCode;
                    wZCompactCheck.PlanCode = GetWZPickingPlanCode( wZCompactDetail.PlanDetailID.ToString());
                    wZCompactCheck.ProjectCode = ShareClass.ObjectToString(drCompact["ProjectCode"]);
                    wZCompactCheck.ProjectName = ShareClass.ObjectToString(drCompact["ProjectName"]);
                    wZCompactCheck.SupplierCode = ShareClass.ObjectToString(drCompact["SupplierCode"]);
                    wZCompactCheck.SupplierName = ShareClass.ObjectToString(drCompact["SupplierName"]);
                    wZCompactCheck.ObjectCode = wZCompactDetail.ObjectCode;
                    wZCompactCheck.ObjectName = wZCompactDetail.ObjectName;
                    wZCompactCheck.Model = wZCompactDetail.Model;
                    wZCompactCheck.Criterion = wZCompactDetail.Criterion;
                    wZCompactCheck.Grade = wZCompactDetail.Grade;
                    wZCompactCheck.Unit = wZCompactDetail.Unit;
                    wZCompactCheck.CompactNumber = wZCompactDetail.CompactNumber;
                    wZCompactCheck.ArrivalGoodsNumber = wZCompactDetail.CompactNumber;
                    wZCompactCheck.ArrivalGoodsName = wZCompactDetail.ObjectName;
                    wZCompactCheck.ArrivalGoodsModel = wZCompactDetail.Model;
                    wZCompactCheck.ExecutionStandard = wZCompactDetail.Criterion;
                    wZCompactCheck.Checker = strUserCode;
                    wZCompactCheck.CheckerDate = "";
                    wZCompactCheck.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();

                    wZCompactCheck.CompactDetailID = wZCompactDetail.ID;

                    wZCompactCheckBLL.AddWZCompactCheck(wZCompactCheck);
                }


                wZCompactDetail.CheckCode = LanguageHandle.GetWord("ZhengZaiCaiJian").ToString().Trim();
                wZCompactDetail.IsCheck = 1;

                wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);

                //重新加载检号列表
                DataCompactCheckBinder();

                //重新加载合同明细
                DataCompactDetailBinder();

                ControlStatusCloseChange();
            }
        }

    }

    protected string GetWZPickingPlanCode(string strPlanDetailID)
    {
        string strWZCompactDetailHQL = string.Format(@"select l.PlanCode from T_WZCompactDetail d
                            left join T_WZPickingPlanDetail l on d.PlanDetailID = l.ID
                            where d.PlanDetailID = {0}",strPlanDetailID);
                           
        DataTable dtCompactDetail = ShareClass.GetDataSetFromSql(strWZCompactDetailHQL, "CompactDetail").Tables[0];

        return dtCompactDetail.Rows[0][0].ToString();
    }




    protected void BT_SortPlanCode_Click(object sender, EventArgs e)
    {
        DataGrid1.CurrentPageIndex = 0;

        string strCompactCheckHQL = string.Format(@"select c.*,s.UnitName from T_WZCompactCheck c
                    left join T_WZSpan s on c.Unit = s.ID
                    where c.Checker = '{0}' ", strUserCode);

        string strCompactCode = DDL_Compact.SelectedValue;
        if (!string.IsNullOrEmpty(strCompactCode))
        {
            strCompactCheckHQL += " and c.CompactCode = '" + strCompactCode + "' ";
        }
        string strCheckCode = DDL_Check.SelectedValue;
        if (!string.IsNullOrEmpty(strCheckCode))
        {
            strCompactCheckHQL += " and c.CheckCode = '" + strCheckCode + "' ";
        }

        if (!string.IsNullOrEmpty(HF_SortPlanCode.Value))
        {
            strCompactCheckHQL += " order by c.PlanCode desc";

            HF_SortPlanCode.Value = "";
        }
        else
        {
            strCompactCheckHQL += " order by c.PlanCode asc";

            HF_SortPlanCode.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactCheckHQL, "CompactCheck").Tables[0];

        DataGrid1.DataSource = dtCompact;
        DataGrid1.DataBind();

        LB_Sql.Text = strCompactCheckHQL;

        LB_CheckRecord.Text = dtCompact.Rows.Count.ToString();

    }
    protected void BT_SortObjectName_Click(object sender, EventArgs e)
    {
        DataGrid1.CurrentPageIndex = 0;

        string strCompactCheckHQL = string.Format(@"select c.*,s.UnitName from T_WZCompactCheck c
                    left join T_WZSpan s on c.Unit = s.ID
                    where c.Checker = '{0}' ", strUserCode);

        string strCompactCode = DDL_Compact.SelectedValue;
        if (!string.IsNullOrEmpty(strCompactCode))
        {
            strCompactCheckHQL += " and c.CompactCode = '" + strCompactCode + "' ";
        }
        string strCheckCode = DDL_Check.SelectedValue;
        if (!string.IsNullOrEmpty(strCheckCode))
        {
            strCompactCheckHQL += " and c.CheckCode = '" + strCheckCode + "' ";
        }

        if (!string.IsNullOrEmpty(HF_SortObjectName.Value))
        {
            strCompactCheckHQL += " order by c.ProjectCode desc";

            HF_SortObjectName.Value = "";
        }
        else
        {
            strCompactCheckHQL += " order by c.ProjectCode asc";

            HF_SortObjectName.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactCheckHQL, "Compact").Tables[0];

        DataGrid1.DataSource = dtCompact;
        DataGrid1.DataBind();

        LB_Sql.Text = strCompactCheckHQL;

        LB_CheckRecord.Text = dtCompact.Rows.Count.ToString();

        ControlStatusCloseChange();

    }

    protected void BT_NewRecordDelete_Click(object sender, EventArgs e)
    {
        //记录删除
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDJHLB").ToString().Trim() + "')", true);
            return;
        }

        WZCompactCheckBLL wZCompactCheckBLL = new WZCompactCheckBLL();
        string strWZCompactCheckSql = "from WZCompactCheck as wZCompactCheck where ID = " + strEditID;
        IList listWZCompactCheck = wZCompactCheckBLL.GetAllWZCompactChecks(strWZCompactCheckSql);
        if (listWZCompactCheck != null && listWZCompactCheck.Count == 1)
        {
            WZCompactCheck wZCompactCheck = (WZCompactCheck)listWZCompactCheck[0];

            wZCompactCheckBLL.DeleteWZCompactCheck(wZCompactCheck);

            //把合同明细<检号>为空
            string strCompactDetailSQL = "update T_WZCompactDetail set CheckCode ='' where id = " + wZCompactCheck.CompactDetailID;
            ShareClass.RunSqlCommand(strCompactDetailSQL);

            //删除成功
            DataCompactCheckBinder();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);

            ControlStatusCloseChange();
        }
    }

    protected void BT_NewCheckCodeEdit_Click(object sender, EventArgs e)
    {
        //检号编辑
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDJHLB").ToString().Trim() + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZCompactCheckListNewDetail.aspx?ID=" + strEditID + "');", true);
        return;
    }

    protected void BT_NewCheckCodeReturn_Click(object sender, EventArgs e)
    {
        //检号退回
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDJHLB").ToString().Trim() + "')", true);
            return;
        }

        WZCompactCheckBLL wZCompactCheckBLL = new WZCompactCheckBLL();
        string strWZCompactCheckSql = "from WZCompactCheck as wZCompactCheck where ID = " + strEditID;
        IList listWZCompactCheck = wZCompactCheckBLL.GetAllWZCompactChecks(strWZCompactCheckSql);
        if (listWZCompactCheck != null && listWZCompactCheck.Count == 1)
        {
            WZCompactCheck wZCompactCheck = (WZCompactCheck)listWZCompactCheck[0];

            wZCompactCheck.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
            wZCompactCheck.CheckCode = "";

            wZCompactCheckBLL.UpdateWZCompactCheck(wZCompactCheck, wZCompactCheck.ID);

            //合同明细〈检号〉＝“正在材检” 												
            //合同明细〈材检标志〉＝“0”												

            string strCompactDetailSQL = "update T_WZCompactDetail set CheckCode ='正在材检',IsCheck=0 where id = " + wZCompactCheck.CompactDetailID;   //ChineseWord
            ShareClass.RunSqlCommand(strCompactDetailSQL);

            //检号退回成功
            DataCompactCheckBinder();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJianHaoLanguageHandleGetWord").ToString().Trim()+"')", true); 

            ControlStatusCloseChange();
        }
    }

    private void ControlStatusChange(string strCompactDetailID, string strProgress)
    {
        if (strProgress == LanguageHandle.GetWord("LuRu").ToString().Trim())
        {
            BT_NewRecordDelete.Enabled = true;
            BT_NewCheckCodeEdit.Enabled = true;

        }
        else
        {
            BT_NewRecordDelete.Enabled = false;
            BT_NewCheckCodeEdit.Enabled = false;
        }


        //查询下合同明细使用标记是否为0
        string strCompactDetailSQL = @"select * from T_WZCompactDetail
                        where ID = " + strCompactDetailID;
        DataTable dtCompactDetail = ShareClass.GetDataSetFromSql(strCompactDetailSQL, "CompactDetail").Tables[0];
        int intIsMark = 0;
        if (dtCompactDetail != null && dtCompactDetail.Rows.Count > 0)
        {
            int.TryParse(ShareClass.ObjectToString(dtCompactDetail.Rows[0]["IsMark"]), out intIsMark);
        }

        if (strProgress == LanguageHandle.GetWord("CaiJian").ToString().Trim() && intIsMark == 0)
        {
            BT_NewCheckCodeReturn.Enabled = true;
        }
        else
        {
            BT_NewCheckCodeReturn.Enabled = false;
        }
    }

    private void ControlStatusCloseChange()
    {
        BT_NewRecordDelete.Enabled = false;
        BT_NewCheckCodeEdit.Enabled = false;
        BT_NewCheckCodeReturn.Enabled = false;
    }

    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataCompactCheckBinder();
    }

    /// <summary>
    ///  重新加载列表
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {

        DataCompactCheckBinder();
    }


    protected void BT_NewCompactCheck_Click(object sender, EventArgs e)
    {
        //合同材检
        string strCompactCode = LB_Compact.SelectedValue;
        if (!string.IsNullOrEmpty(strCompactCode))
        {
            WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
            string strWZCompactDetailHQL = string.Format(@"from WZCompactDetail as wZCompactDetail
                            where CompactCode = '{0}'", strCompactCode);
            IList lstCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);

            if (lstCompactDetail != null && lstCompactDetail.Count > 0)
            {
                for (int i = 0; i < lstCompactDetail.Count; i++)
                {
                    WZCompactDetail wZCompactDetail = (WZCompactDetail)lstCompactDetail[i];

                    wZCompactDetail.IsCheck = -1;

                    wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);
                }

                DataCompactDetailBinder();
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZGeTongLanguageHandleGetWordZ").ToString().Trim()+"')", true); 
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZMYFHTJDHTMXJL").ToString().Trim() + "')", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZHT").ToString().Trim() + "')", true);
            return;
        }
    }

    protected void BT_NewCompactCheckReturn_Click(object sender, EventArgs e)
    {
        //合同材检退回
        string strCompactCode = LB_Compact.SelectedValue;
        if (!string.IsNullOrEmpty(strCompactCode))
        {
            WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
            string strWZCompactDetailHQL = string.Format(@"from WZCompactDetail as wZCompactDetail
                            where CompactCode = '{0}'
                            and (CheckCode = '' or CheckCode = '正在材检')
                            and IsMark = 0", strCompactCode);   //ChineseWord
            IList lstCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);

            if (lstCompactDetail != null && lstCompactDetail.Count > 0)
            {
                for (int i = 0; i < lstCompactDetail.Count; i++)
                {
                    WZCompactDetail wZCompactDetail = (WZCompactDetail)lstCompactDetail[i];

                    wZCompactDetail.IsCheck = 0;

                    wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);
                }

                DataCompactDetailBinder();
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZGeTongCaiJianLanguageHandleG").ToString().Trim()+"')", true); 
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZMYFHTJDHTMXJL").ToString().Trim() + "')", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZHT").ToString().Trim() + "')", true);
            return;
        }
    }
}
