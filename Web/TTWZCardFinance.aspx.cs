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
using System.Text;

public partial class TTWZCardFinance : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","财务凭证编辑", strUserCode);

        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataTurnDetailNoCodeBinder();
            DataTurnDetailBinder("");

            DataCardBinder();
        }
    }


    private void DataTurnDetailNoCodeBinder()
    {
        string strWZTurnDetailHQL = @"select d.*,o.ObjectName,pd.PlanCode as PickingPlanCode ,
                            m.UserName as MaterialPersonName
                            from T_WZTurnDetail d
                            left join T_WZObject o on d.ObjectCode = o.ObjectCode
                            left join T_WZPickingPlanDetail pd on d.PlanCode = pd.ID 
                            left join T_ProjectMember m on d.MaterialPerson = m.UserCode
                            where d.IsMark= -1 
                            and d.CardIsMark =0 
                            order by d.NoCode";
        DataTable dtTurnDetail = ShareClass.GetDataSetFromSql(strWZTurnDetailHQL, "TurnDetail").Tables[0];

        DG_TurnDetailNoCode.DataSource = dtTurnDetail;
        DG_TurnDetailNoCode.DataBind();

        //WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
        //string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where IsMark= -1 and CardIsMark =0 order by NoCode";
        //IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);

        //DG_TurnDetailNoCode.DataSource = listWZTurnDetail;
        //DG_TurnDetailNoCode.DataBind();
    }


    private void DataTurnDetailBinder(string strCardCode)
    {
        string strWZTurnDetailHQL = string.Format(@"select d.*,o.ObjectName,pd.PlanCode as PickingPlanCode ,
                            m.UserName as MaterialPersonName
                            from T_WZTurnDetail d
                            left join T_WZObject o on d.ObjectCode = o.ObjectCode
                            left join T_WZPickingPlanDetail pd on d.PlanCode = pd.ID  
                            left join T_ProjectMember m on d.MaterialPerson = m.UserCode
                            where  d.CardCode = '{0}' 
                            order by d.NoCode", strCardCode);
        DataTable dtTurnDetail = ShareClass.GetDataSetFromSql(strWZTurnDetailHQL, "TurnDetail").Tables[0];

        DG_TurnDetail.DataSource = dtTurnDetail;
        DG_TurnDetail.DataBind();

        //WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
        //string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where  CardCode = '" + strCardCode + "' order by NoCode";
        //IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);

        //DG_TurnDetail.DataSource = listWZTurnDetail;
        //DG_TurnDetail.DataBind();
    }

    private void DataCardBinder()
    {
        WZCardBLL wZCardBLL = new WZCardBLL();
        string strWZCardHQL = "from WZCard as wZCard where CardMarker = '" + strUserCode + "' order by CardCode desc";
        IList listWZCard = wZCardBLL.GetAllWZCards(strWZCardHQL);

        LB_Card.DataSource = listWZCard;
        LB_Card.DataBind();
    }


    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strCardCode = LB_Card.SelectedValue;
        if (string.IsNullOrEmpty(strCardCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZCWPZ").ToString().Trim()+"')", true);
            return;
        }

        string strNoCode = TXT_NoCode.Text.Trim();
        if (string.IsNullOrEmpty(strNoCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSRNOBH").ToString().Trim()+"')", true);
            return;
        }

        string strTurnDetailID = HF_TurnDetailID.Value;
        if (!string.IsNullOrEmpty(strTurnDetailID))
        {
            WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
            string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where  ID = " + strTurnDetailID;
            IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);
            if (listWZTurnDetail != null && listWZTurnDetail.Count > 0)
            {
                WZTurnDetail wZTurnDetail = (WZTurnDetail)listWZTurnDetail[0];
                //财务凭证
                WZCardBLL wZCardBLL = new WZCardBLL();
                string strWZCardHQL = "from WZCard as wZCard where CardCode = '" + strCardCode + "'";
                IList listCard = wZCardBLL.GetAllWZCards(strWZCardHQL);
                WZCard wZCard = (WZCard)listCard[0];

                wZTurnDetail.CardCode = wZCard.CardCode;
                wZTurnDetail.CardPerson = wZCard.CardMarker;
                wZTurnDetail.CardIsMark = -1;
                wZTurnDetail.NoCode = strNoCode;

                wZTurnDetailBLL.UpdateWZTurnDetail(wZTurnDetail, wZTurnDetail.ID);

                //重新加载两个列表
                DataTurnDetailNoCodeBinder();
                LB_Card_SelectedIndexChanged(null, null);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZWYNOCODEDYJDMX").ToString().Trim()+"')", true);
            return;
        }
    }


    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        try
        {
            string strPlanMoney = TXT_PlanMoney.Text.Trim();
            string strPlanPrice = TXT_PlanPrice.Text.Trim();
            if (string.IsNullOrEmpty(strPlanMoney) || strPlanMoney == "0")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSJEBNWK0BC").ToString().Trim()+"')", true);
                return;
            }
            decimal decimalPlanMoney = 0;
            decimal.TryParse(strPlanMoney, out decimalPlanMoney);

            string strTurnDetailIDEdit = HF_TurnDetailIDEdit.Value;
            if (!string.IsNullOrEmpty(strTurnDetailIDEdit))
            {
                WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
                string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where  ID = " + strTurnDetailIDEdit;
                IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);
                if (listWZTurnDetail != null && listWZTurnDetail.Count > 0)
                {
                    WZTurnDetail wZTurnDetail = (WZTurnDetail)listWZTurnDetail[0];

                    wZTurnDetail.PlanMoney = decimalPlanMoney;
                    if (wZTurnDetail.ActualNumber != 0)
                    {
                        wZTurnDetail.PlanPrice = wZTurnDetail.PlanMoney / wZTurnDetail.ActualNumber;
                    }
                    wZTurnDetailBLL.UpdateWZTurnDetail(wZTurnDetail, wZTurnDetail.ID);

                    //财务凭证〈凭证金额〉＝∑移交明细〈预算金额〉												
                    //财务凭证〈明细金额〉＝∑移交明细〈开票金额〉												
                    //财务凭证〈明细条数〉＝∑移交明细记录条数												
                    //财务凭证〈使用标记〉＝“-1”												
                    string strCardCode = LB_Card.SelectedValue;
                    WZCardBLL wZCardBLL = new WZCardBLL();
                    string strWZCardHQL = "from WZCard as wZCard where CardCode = '" + strCardCode + "'";
                    IList listCard = wZCardBLL.GetAllWZCards(strWZCardHQL);
                    if (listCard != null && listCard.Count > 0)
                    {
                        WZCard wZCard = (WZCard)listCard[0];

                        wZCard.CardMoney += decimalPlanMoney;
                        wZCard.DetailMoney += wZTurnDetail.TicketMoney;
                        wZCard.RowNumber += 1;
                        wZCard.IsMark = -1;

                        wZCardBLL.UpdateWZCard(wZCard, wZCard.CardCode);
                    }

                    //重新加载列表
                    LB_Card_SelectedIndexChanged(null, null);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZBJDYJDMX").ToString().Trim()+"')", true);
                return;
            }
        }
        catch (Exception ex) { }
    }


    protected void BT_Confirm_Click(object sender, EventArgs e)
    {
        string strCardCode = LB_Card.SelectedValue;
        if (string.IsNullOrEmpty(strCardCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZCWPZ").ToString().Trim()+"')", true);
            return;
        }

        //财务凭证
        WZCardBLL wZCardBLL = new WZCardBLL();
        string strWZCardHQL = "from WZCard as wZCard where CardCode = '" + strCardCode + "'";
        IList listCard = wZCardBLL.GetAllWZCards(strWZCardHQL);
        if (listCard != null && listCard.Count > 0)
        {
            WZCard wZCard = (WZCard)listCard[0];
            wZCard.Progress = LanguageHandle.GetWord("XiaFa").ToString().Trim();
            wZCard.CardTime = DateTime.Now;

            wZCardBLL.UpdateWZCard(wZCard, wZCard.CardCode);

            string strProjectHQL = string.Format(@"select ProjectCode,SUM(PlanMoney) as TotalPlanMoney from T_WZTurnDetail
                            where CardCode = '{0}'
                            and CardIsMark = -1
                            group by ProjectCode", strCardCode);
            DataTable dtProject = ShareClass.GetDataSetFromSql(strProjectHQL, "Project").Tables[0];
            if (dtProject != null && dtProject.Rows.Count > 0)
            {
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                bool isTrue = false;
                foreach (DataRow dr in dtProject.Rows)
                {
                    string strWZProjectSql = "from WZProject as wZProject where ProjectCode = '" + ShareClass.ObjectToString(dr["ProjectCode"]) + "'";
                    IList listProject = wZProjectBLL.GetAllWZProjects(strWZProjectSql);
                    if (listProject != null && listProject.Count > 0)
                    {
                        WZProject wZProject = (WZProject)listProject[0];
                        decimal decimalTheBudget = 0;
                        decimal.TryParse(ShareClass.ObjectToString(dr["TotalPlanMoney"]), out decimalTheBudget);
                        wZProject.TheBudget = decimalTheBudget;

                        wZProjectBLL.UpdateWZProject(wZProject, wZProject.ProjectCode);
                        isTrue = true;
                    }
                }
                if (isTrue)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCG").ToString().Trim()+"')", true);
                }
            }
        }
    }

    protected void BT_Import_Click(object sender, EventArgs e)
    {
        //导出
        string strCardCode = LB_Card.SelectedValue;
        if (string.IsNullOrEmpty(strCardCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZCWPZ").ToString().Trim()+"')", true);
            return;
        }
        string strFileName = strCardCode + LanguageHandle.GetWord("HaoCaiWuPingZheng").ToString().Trim();
        string strWZTurnDetailHQL = string.Format(@"select * from T_WZTurnDetail 
                            where  CardCode = '{0}' 
                            order by NoCode", strCardCode);
        DataTable dtTurnDetail = ShareClass.GetDataSetFromSql(strWZTurnDetailHQL, "TurnDetail").Tables[0];
        //ExcelUtils.Export2Excel(DG_TurnDetail, strFileName);
        Export3Excel(dtTurnDetail, strFileName);

    }
    

    protected void LB_Card_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strCardCode = LB_Card.SelectedValue;
        if (!string.IsNullOrEmpty(strCardCode))
        {
            HF_TurnDetailIDEdit.Value = "";
            DataTurnDetailBinder(strCardCode);

            TXT_CardCode.Text = strCardCode;
        }
    }

    protected void DG_TurnDetail_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            for (int i = 0; i < DG_TurnDetail.Items.Count; i++)
            {
                DG_TurnDetail.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                string cmdArges = e.CommandArgument.ToString();             //ID|ActualNumber|PlanMoney
                string[] arrCmdArges = cmdArges.Split('|');
                HF_TurnDetailIDEdit.Value = arrCmdArges[0];
                HF_ActualNumber.Value = arrCmdArges[1];
                TXT_PlanMoney.Text = arrCmdArges[2];
                if (!string.IsNullOrEmpty(arrCmdArges[1]))
                {
                    decimal decimalActualNumber = 0;
                    decimal.TryParse(arrCmdArges[1], out decimalActualNumber);
                    decimal decimalPlanMoney = 0;
                    decimal.TryParse(arrCmdArges[2], out decimalPlanMoney);

                    if (decimalActualNumber != 0)
                    {
                        TXT_PlanPrice.Text = (decimalPlanMoney / decimalActualNumber).ToString();
                    }

                    TXT_PlanMoney.BackColor = Color.CornflowerBlue;
                    TXT_PlanPrice.BackColor = Color.CornflowerBlue;
                }
            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();

                WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
                string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where  ID = " + cmdArges;
                IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);
                if (listWZTurnDetail != null && listWZTurnDetail.Count > 0)
                {
                    WZTurnDetail wZTurnDetail = (WZTurnDetail)listWZTurnDetail[0];

                    if (wZTurnDetail.IsMark == -1)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBJW1SBNSC").ToString().Trim()+"')", true);
                        return;
                    }

                    string strCardCode = LB_Card.SelectedValue;
                    WZCardBLL wZCardBLL = new WZCardBLL();
                    string strWZCardHQL = "from WZCard as wZCard where CardCode = '" + strCardCode + "'";
                    IList listCard = wZCardBLL.GetAllWZCards(strWZCardHQL);
                    if (listCard != null && listCard.Count > 0)
                    {
                        WZCard wZCard = (WZCard)listCard[0];

                        wZCard.CardMoney -= wZTurnDetail.PlanMoney;
                        wZCard.DetailMoney -= wZTurnDetail.TicketMoney;
                        wZCard.RowNumber -= 1;
                        wZCard.IsMark = -1;

                        wZCardBLL.UpdateWZCard(wZCard, wZCard.CardCode);
                    }

                    wZTurnDetail.CardCode = "-";
                    wZTurnDetail.CardPerson = "-";
                    wZTurnDetail.CardIsMark = 0;
                    wZTurnDetail.PlanMoney = 0;
                    wZTurnDetail.PlanPrice = 0;

                    wZTurnDetailBLL.UpdateWZTurnDetail(wZTurnDetail, wZTurnDetail.ID);

                    //如果所有移交明细被删除，则财务凭证<使用标记> = 0
                    string strSelectWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where  CardCode = '" + strCardCode + "'";
                    IList listSelectWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strSelectWZTurnDetailHQL);
                    if (listSelectWZTurnDetail == null || listSelectWZTurnDetail.Count == 0)
                    {
                        string strUpdateCardHQL = "update T_WZCard set IsMark = 0 where CardCode = '" + strCardCode + "'";
                        ShareClass.RunSqlCommand(strUpdateCardHQL);
                    }
                }
            }
        }
        catch (Exception ex) { }
    }

    protected void DG_TurnDetailNoCode_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        for (int i = 0; i < DG_TurnDetailNoCode.Items.Count; i++)
        {
            DG_TurnDetailNoCode.Items[i].ForeColor = Color.Black;
        }

        e.Item.ForeColor = Color.Red;

        string cmdName = e.CommandName;
        if (cmdName == "edit")
        {
            string cmdArges = e.CommandArgument.ToString();

            string[] arrArges = cmdArges.Split('|');                    //ID|TurnCode

            HF_TurnDetailID.Value = arrArges[0];
            TXT_TurnCode.Text = arrArges[1];
        }
    }


    protected void BT_CalcPrice_Click(object sender, EventArgs e)
    {
        string strTurnDetailIDEdit = HF_TurnDetailIDEdit.Value;
        if (!string.IsNullOrEmpty(strTurnDetailIDEdit))
        {
            string strPlanMoney = TXT_PlanMoney.Text.Trim();
            if (!ShareClass.CheckIsNumber(strPlanMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSJEBXWXSHZZS").ToString().Trim()+"')", true);
                return;
            }

            decimal decimalPlanMoney = 0;
            decimal.TryParse(strPlanMoney, out decimalPlanMoney);


            WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
            string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where  ID = " + strTurnDetailIDEdit;
            IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);
            if (listWZTurnDetail != null && listWZTurnDetail.Count > 0)
            {
                WZTurnDetail wZTurnDetail = (WZTurnDetail)listWZTurnDetail[0];

                //      移交明细〈预算单价〉＝移交明细〈预算金额〉÷移交明细〈实领数量〉
                decimal decimalPlanPrice = 0;

                if (wZTurnDetail.ActualNumber != 0)
                {
                    decimalPlanPrice = decimalPlanMoney / wZTurnDetail.ActualNumber;
                }

                TXT_PlanPrice.Text = decimalPlanPrice.ToString("#0.00");
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZBJDYJDMX").ToString().Trim()+"')", true);
            return;
        }
    }


    public void Export3Excel(DataTable dtData, string strFileName)
    {
        DataGrid dgControl = new DataGrid();
        dgControl.DataSource = dtData;
        dgControl.DataBind();


        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + strFileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/shlnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dgControl.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();

    }
}