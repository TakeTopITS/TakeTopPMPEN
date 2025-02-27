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

public partial class TTWZCompactContractList : System.Web.UI.Page
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
            DataBinder();
        }
    }

    private void DataBinder()
    {
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
                    where c.Compacter = '{0}' ", strUserCode);

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_CompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }
        strCompactHQL += " order by c.MarkTime desc";

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_RowNumber.Text = dtCompact.Rows.Count.ToString();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        e.Item.ForeColor = Color.Red;

        string cmdName = e.CommandName;
        if (cmdName == "click")
        {
            //操作
            string cmdArges = e.CommandArgument.ToString();
            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
            IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
            if (listCompact != null && listCompact.Count == 1)
            {
                WZCompact wZCompact = (WZCompact)listCompact[0];

                HF_NewCompactCode.Value = wZCompact.CompactCode;
                HF_NewProgress.Value = wZCompact.Progress;
                HF_NewCompactMoney.Value = wZCompact.CompactMoney.ToString();
                HF_NewCollectMoney.Value = wZCompact.CollectMoney.ToString();

                ControlStatusChange(wZCompact.CompactCode, wZCompact.Progress, wZCompact.CompactMoney, wZCompact.CollectMoney,wZCompact.ReceiveTime);
            }
        }
        else if (cmdName == "sign")
        {
            //合同签收
            string cmdArges = e.CommandArgument.ToString();
            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
            IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
            if (listCompact != null && listCompact.Count == 1)
            {
                WZCompact wZCompact = (WZCompact)listCompact[0];
                if (wZCompact.Progress == LanguageHandle.GetWord("CaiJian").ToString().Trim())
                {
                    wZCompact.ReceiveTime = DateTime.Now.ToString("yyyy-MM-dd");

                    wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTJDBSCJZTBNS").ToString().Trim() + "')", true);
                    return;
                }
            }
        }
        else if (cmdName == "cancel")
        {
            //核销
            string cmdArges = e.CommandArgument.ToString();
            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
            IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
            if (listCompact != null && listCompact.Count == 1)
            {
                WZCompact wZCompact = (WZCompact)listCompact[0];
                //主观判断：合同〈合同金额〉＝合同〈收料总额〉，如不等，由合同员主观决定是否继续核销	
                //查询合同收料单，收料单<结算标记>=-1，<报销进度>=-1			

                //主观判断：合同〈合同金额〉＝合同〈收料总额〉，如不等，由合同员主观决定是否继续核销												
                //核销条件：												
                //   合同〈进度〉＝“材检”，“核销”按钮打开												
                //   合同〈合同员〉＝“操作员”												
                //   合同〈未请款额〉＝“0”												
                //   所有收料单〈结算标记〉＝“-1”												
                //   所有收料单〈报销进度〉＝“报销”				

                if (wZCompact.CompactMoney != wZCompact.CollectMoney)
                {
                    HF_AlertCompact.Value = wZCompact.CompactCode;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertCancel()", true);
                }
                else
                {

                    if (wZCompact.Progress == LanguageHandle.GetWord("CaiJian").ToString().Trim() && wZCompact.NotRequestMoney == 0 && wZCompact.Compacter == strUserCode)
                    {

                        string strCheckCollectHQL = string.Format(@"select * from T_WZCollect
                                            where CompactCode = '{0}'", wZCompact.CompactCode);
                        DataTable dtCheckCollect = ShareClass.GetDataSetFromSql(strCheckCollectHQL, "CheckCollect").Tables[0];
                        if (dtCheckCollect != null && dtCheckCollect.Rows.Count > 0)
                        {
                            foreach (DataRow drCollect in dtCheckCollect.Rows)
                            {
                                string strIsMark = ShareClass.ObjectToString(drCollect["IsMark"]);
                                string strPayProcess = ShareClass.ObjectToString(drCollect["PayProcess"]);

                                if (strIsMark != "-1" || strPayProcess != LanguageHandle.GetWord("BaoXiao").ToString().Trim())
                                {
                                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDHTSLDZYJSBJBW1HZBXJDBWBXXCLSLD").ToString().Trim() + "')", true);
                                    return;
                                }
                            }
                        }

                        wZCompact.Progress = LanguageHandle.GetWord("HeXiao").ToString().Trim();
                        wZCompact.CancelTime = DateTime.Now.ToString("yyyy-MM-dd");

                        wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHXCG").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTJDBSCJZTHZWKEBW0HTYBSCZYBNHX").ToString().Trim() + "')", true);
                        return;
                    }
                }
            }
        }
        else if (cmdName == "notCancel")
        {
            //核销退回
            string cmdArges = e.CommandArgument.ToString();
            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
            IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
            if (listCompact != null && listCompact.Count == 1)
            {
                WZCompact wZCompact = (WZCompact)listCompact[0];
                if (wZCompact.Progress == LanguageHandle.GetWord("HeXiao").ToString().Trim())
                {
                    wZCompact.Progress = LanguageHandle.GetWord("CaiJian").ToString().Trim();
                    wZCompact.CancelTime = "-";

                    wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHXTHCG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTJDBSHXZTBNHXTH").ToString().Trim() + "')", true);
                    return;
                }
            }
        }
    }


    protected void BT_CollectHandler_Click(object sender, EventArgs e)
    {
        string strCompactCode = HF_AlertCompact.Value;

        //核销条件：												
        //   合同〈进度〉＝“材检”，“核销”按钮打开												
        //   合同〈合同员〉＝“操作员”												
        //   合同〈未请款额〉＝“0”												
        //   所有收料单〈结算标记〉＝“-1”												
        //   所有收料单〈报销进度〉＝“报销”		
        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + strCompactCode + "'";
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
        if (listCompact != null && listCompact.Count == 1)
        {
            WZCompact wZCompact = (WZCompact)listCompact[0];


            if (wZCompact.Progress == LanguageHandle.GetWord("CaiJian").ToString().Trim() && wZCompact.NotRequestMoney == 0 && wZCompact.Compacter.Trim() == strUserCode)
            {

                string strCheckCollectHQL = string.Format(@"select * from T_WZCollect
                                            where CompactCode = '{0}'", wZCompact.CompactCode);
                DataTable dtCheckCollect = ShareClass.GetDataSetFromSql(strCheckCollectHQL, "CheckCollect").Tables[0];
                if (dtCheckCollect != null && dtCheckCollect.Rows.Count > 0)
                {
                    foreach (DataRow drCollect in dtCheckCollect.Rows)
                    {
                        string strIsMark = ShareClass.ObjectToString(drCollect["IsMark"]);
                        string strPayProcess = ShareClass.ObjectToString(drCollect["PayProcess"]);

                        if (strIsMark != "-1" || strPayProcess != LanguageHandle.GetWord("BaoXiao").ToString().Trim())
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDHTSLDZYJSBJBW1HZBXJDBWBXXCLSLD").ToString().Trim() + "')", true);
                            return;
                        }
                    }
                }

                wZCompact.Progress = LanguageHandle.GetWord("HeXiao").ToString().Trim();
                wZCompact.CancelTime = DateTime.Now.ToString("yyyy-MM-dd");

                wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHXCG").ToString().Trim() + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTJDBSCJZTHZWKEBW0HTYBSCZYBNHX").ToString().Trim() + "')", true);
                return;
            }
        }
    }

    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataBinder();
    }

    protected void BT_NewCompactDetail_Click(object sender, EventArgs e)
    {
        //合同明细
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDHTLB").ToString().Trim() + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZCompactDetail.aspx?CompactCode=" + strEditCompactCode + "');", true);
        return;
    }

    protected void BT_NewSign_Click(object sender, EventArgs e)
    {
        //签收
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDHTLB").ToString().Trim() + "')", true);
            return;
        }

        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + strEditCompactCode + "'";
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
        if (listCompact != null && listCompact.Count == 1)
        {
            WZCompact wZCompact = (WZCompact)listCompact[0];
            if (wZCompact.Progress == LanguageHandle.GetWord("ShengXiao").ToString().Trim() & wZCompact.Compacter.Trim() == strUserCode)
            {
                wZCompact.ReceiveTime = DateTime.Now.ToString("yyyy-MM-dd");

                wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCG").ToString().Trim() + "')", true);

                ControlStatusCloseChange();

                BT_NewSign.Enabled = false;
                BT_NewSignReturn.Enabled = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTLBBCZBNS").ToString().Trim() + "')", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTLBBCZBNS").ToString().Trim() + "')", true);
            return;
        }
    }

    protected void BT_NewSignReturn_Click(object sender, EventArgs e)
    {
        //签收退回
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDHTLB").ToString().Trim() + "')", true);
            return;
        }

        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strWZCompactSql = "from WZCompact as wZCompact where CompactCode = '" + strEditCompactCode + "'";
        IList listWZCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactSql);
        if (listWZCompact != null && listWZCompact.Count == 1)
        {
            WZCompact wZCompact = (WZCompact)listWZCompact[0];

            wZCompact.ReceiveTime = "";

            wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

            DataBinder();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSTHCG").ToString().Trim() + "')", true);

            ControlStatusCloseChange();

            BT_NewSign.Enabled = true;
            BT_NewSignReturn.Enabled = false;
        }
    }

    protected void BT_NewCancel_Click(object sender, EventArgs e)
    {
        //核销
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDHTLB").ToString().Trim() + "')", true);
            return;
        }

        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + strEditCompactCode + "'";
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
        if (listCompact != null && listCompact.Count == 1)
        {
            WZCompact wZCompact = (WZCompact)listCompact[0];
            //主观判断：合同〈合同金额〉＝合同〈收料总额〉，如不等，由合同员主观决定是否继续核销	
            //查询合同收料单，收料单<结算标记>=-1，<报销进度>=-1			

            //主观判断：合同〈合同金额〉＝合同〈收料总额〉，如不等，由合同员主观决定是否继续核销												
            //核销条件：												
            //   合同〈进度〉＝“材检”，“核销”按钮打开												
            //   合同〈合同员〉＝“操作员”												
            //   合同〈未请款额〉＝“0”												
            //   所有收料单〈结算标记〉＝“-1”												
            //   所有收料单〈报销进度〉＝“报销”		

            //收料单〈结算标记〉＝“-1”      （代表已上账，逐条检查）												
            //收料单〈报销进度〉＝“报销”    （代表已报销，逐条检查）				

            bool IsIsMark = false;
            bool IsPayProgress = false;

            string strAlertResult = string.Empty;
            strAlertResult = LanguageHandle.GetWord("BenGeTongXiaShangYouWeiShangZh").ToString().Trim();

            string strCollectMarkSQL = string.Format(@"select c.* from T_WZCollect c
                        left join T_WZCompactDetail d on c.CompactDetailID = d.ID
                        where c.IsMark = 0
                        and d.CompactCode = '{0}'", strEditCompactCode);
            DataTable dtCollectMark = ShareClass.GetDataSetFromSql(strCollectMarkSQL, "CollectCode").Tables[0];
            if (dtCollectMark != null && dtCollectMark.Rows.Count > 0)
            {
                strAlertResult += LanguageHandle.GetWord("WeiShangZhangShouLiaoChanHaobr").ToString().Trim();
                foreach (DataRow drCollectMark in dtCollectMark.Rows)
                {
                    strAlertResult += ShareClass.ObjectToString(drCollectMark["CollectCode"]);
                }
                strAlertResult += "<br />";
                IsIsMark = true;
            }

            string strPayProgressSQL = string.Format(@"select c.* from T_WZCollect c
                        left join T_WZCompactDetail d on c.CompactDetailID = d.ID
                        where c.PayProcess != '核销'
                        and d.CompactCode = '{0}'", strEditCompactCode); 
            DataTable dtPayProgress = ShareClass.GetDataSetFromSql(strPayProgressSQL, "PayProgress").Tables[0];
            if (dtPayProgress != null && dtPayProgress.Rows.Count > 0)
            {
                strAlertResult += LanguageHandle.GetWord("WeiBaoXiaoShouLiaoChanHaobr").ToString().Trim();
                foreach (DataRow drPayProgress in dtPayProgress.Rows)
                {
                    strAlertResult += ShareClass.ObjectToString(drPayProgress["CollectCode"]);
                }
                strAlertResult += "<br />";
                IsPayProgress = true;
            }

            if (IsIsMark && IsPayProgress)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSTRALERTRESULT").ToString().Trim() + "')", true);
                return;
            }

            if (wZCompact.CompactMoney != wZCompact.CollectMoney)
            {
                HF_AlertCompact.Value = wZCompact.CompactCode;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertCancel()", true);
            }
            else
            {
                if (wZCompact.Progress == LanguageHandle.GetWord("CaiJian").ToString().Trim() && wZCompact.NotRequestMoney == 0 && wZCompact.Compacter.Trim() == strUserCode)
                {

                    //                    string strCheckCollectHQL = string.Format(@"select * from T_WZCollect
                    //                                            where CompactCode = '{0}'", wZCompact.CompactCode);
                    //                    DataTable dtCheckCollect = ShareClass.GetDataSetFromSql(strCheckCollectHQL, "CheckCollect").Tables[0];
                    //                    if (dtCheckCollect != null && dtCheckCollect.Rows.Count > 0)
                    //                    {
                    //                        foreach (DataRow drCollect in dtCheckCollect.Rows)
                    //                        {
                    //                            string strIsMark = ShareClass.ObjectToString(drCollect["IsMark"]);
                    //                            string strPayProcess = ShareClass.ObjectToString(drCollect["PayProcess"]);

                    //                            if (strIsMark != "-1" || strPayProcess != LanguageHandle.GetWord("BaoXiao").ToString().Trim())
                    //                            {
                    //                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDHTSLDZYJSBJBW1HZBXJDBWBXXCLSLD").ToString().Trim()+"')", true);
                    //                                return;
                    //                            }
                    //                        }
                    //                    }

                    wZCompact.Progress = LanguageHandle.GetWord("HeXiao").ToString().Trim();
                    wZCompact.CancelTime = DateTime.Now.ToString("yyyy-MM-dd");

                    wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHXCG").ToString().Trim() + "')", true);

                    ControlStatusCloseChange();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTJDBSCJZTHZWKEBW0HTYBSCZYBNHX").ToString().Trim() + "')", true);
                    return;
                }
            }
        }
    }

    protected void BT_NewCancelReturn_Click(object sender, EventArgs e)
    {
        //核销退回
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDHTLB").ToString().Trim() + "')", true);
            return;
        }

        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + strEditCompactCode + "'";
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
        if (listCompact != null && listCompact.Count == 1)
        {
            WZCompact wZCompact = (WZCompact)listCompact[0];
            if (wZCompact.Progress == LanguageHandle.GetWord("HeXiao").ToString().Trim())
            {
                wZCompact.Progress = LanguageHandle.GetWord("ShengXiao").ToString().Trim();
                wZCompact.CancelTime = "-";

                wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHXTHCG").ToString().Trim() + "')", true);

                ControlStatusCloseChange();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTJDBSHXZTBNHXTH").ToString().Trim() + "')", true);
                return;
            }
        }
    }
    protected void BT_SortCompactCode_Click(object sender, EventArgs e)
    {
        //合同编号排序
        DG_List.CurrentPageIndex = 0;

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
                    where c.Compacter = '{0}' ", strUserCode);

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_CompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortCompactCode.Value))
        {
            strCompactHQL += " order by c.CompactCode desc";

            HF_SortCompactCode.Value = "";
        }
        else
        {
            strCompactHQL += " order by c.CompactCode asc";

            HF_SortCompactCode.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_RowNumber.Text = dtCompact.Rows.Count.ToString();

        ControlStatusCloseChange();
    }
    protected void BT_SortProjectCode_Click(object sender, EventArgs e)
    {
        //项目编码排序
        DG_List.CurrentPageIndex = 0;

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
                    where c.Compacter = '{0}' ", strUserCode);

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_CompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortProjectCode.Value))
        {
            strCompactHQL += " order by c.ProjectCode desc";

            HF_SortProjectCode.Value = "";
        }
        else
        {
            strCompactHQL += " order by c.ProjectCode asc";

            HF_SortProjectCode.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_RowNumber.Text = dtCompact.Rows.Count.ToString();

        ControlStatusCloseChange();
    }
    protected void BT_SortSupplierCode_Click(object sender, EventArgs e)
    {
        //供方编号排序
        DG_List.CurrentPageIndex = 0;

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
                    where c.Compacter = '{0}' ", strUserCode);

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_CompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortSupplierCode.Value))
        {
            strCompactHQL += " order by c.SupplierCode desc";

            HF_SortSupplierCode.Value = "";
        }
        else
        {
            strCompactHQL += " order by c.SupplierCode asc";

            HF_SortSupplierCode.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_RowNumber.Text = dtCompact.Rows.Count.ToString();

        ControlStatusCloseChange();
    }
    protected void BT_SortEffectTime_Click(object sender, EventArgs e)
    {
        //生效日期排序
        DG_List.CurrentPageIndex = 0;

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
                    where c.Compacter = '{0}' ", strUserCode);

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_CompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortEffectTime.Value))
        {
            strCompactHQL += " order by c.EffectTime desc";

            HF_SortEffectTime.Value = "";
        }
        else
        {
            strCompactHQL += " order by c.EffectTime asc";

            HF_SortEffectTime.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_RowNumber.Text = dtCompact.Rows.Count.ToString();

        ControlStatusCloseChange();
    }

    private void ControlStatusChange(string strCompactCode, string strProgress, decimal decimalCompactMoney, decimal decimalCollectMoney,string strReceiveTime)
    {
        if (strProgress == LanguageHandle.GetWord("ShengXiao").ToString().Trim() | strProgress == LanguageHandle.GetWord("CaiJian").ToString().Trim())
        {
            BT_NewCompactDetail.Enabled = true;
            BT_NewSign.Enabled = true;
        }
        else
        {
            BT_NewCompactDetail.Enabled = false;
            BT_NewSign.Enabled = false;
        }


        //查询下合同明细使用标记是否为0
        string strCompactDetailSQL = @"select * from T_WZCompactDetail
                        
                        where  IsMark = 0 and CompactCode = '" + strCompactCode + "'";
        DataTable dtCompactDetail = ShareClass.GetDataSetFromSql(strCompactDetailSQL, "CompactDetail").Tables[0];
        bool IsIsMark = false;
        if (dtCompactDetail != null && dtCompactDetail.Rows.Count > 0)
        {
            IsIsMark = true;
        }

        if (strProgress == LanguageHandle.GetWord("ShengXiao").ToString().Trim() && IsIsMark)
        {
            BT_NewSignReturn.Enabled = true;
        }
        else
        {
            BT_NewSignReturn.Enabled = false;
        }

        if (decimalCompactMoney == decimalCollectMoney && strProgress == LanguageHandle.GetWord("ShengXiao").ToString().Trim())
        {
            BT_NewCancel.Enabled = true;
        }
        else
        {
            BT_NewCancel.Enabled = false;
        }

        if (string.IsNullOrEmpty(strReceiveTime))
        {
            BT_NewSignReturn.Enabled = false;

            BT_NewCancel.Enabled = false;
            BT_NewCancelReturn.Enabled = false;
        }

        if (strProgress == LanguageHandle.GetWord("ShengXiao").ToString().Trim() & !string.IsNullOrEmpty(strReceiveTime))
        {
            BT_NewSign.Enabled = false;
            BT_NewSignReturn.Enabled = true;

            BT_NewCancel.Enabled = true;
            BT_NewCancelReturn.Enabled = false;
        }

        if (strProgress == LanguageHandle.GetWord("HeXiao").ToString().Trim())
        {
            BT_NewCancel.Enabled = false;
            BT_NewCancelReturn.Enabled = true;
        }
        else
        {
            BT_NewCancelReturn.Enabled = false;
        }
    }

    private void ControlStatusCloseChange()
    {
        BT_NewCompactDetail.Enabled = false;
        BT_NewSign.Enabled = false;
        BT_NewSignReturn.Enabled = false;
        BT_NewCancel.Enabled = false;
        BT_NewCancelReturn.Enabled = false;
    }

}
