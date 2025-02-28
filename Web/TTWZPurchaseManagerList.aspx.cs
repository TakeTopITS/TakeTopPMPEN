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

public partial class TTWZPurchaseManagerList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();
        }
    }

    private void DataBinder()
    {
        string strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        string strPurchaseHQL = string.Format(@"select p.*,e.UserName as PurchaseEngineerName,
                        u.UserName as UpLeaderName,
                        m.UserName as PurchaseManagerName
                        from T_WZPurchase p
                        left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
                        left join T_ProjectMember u on p.UpLeader = u.UserCode
                        left join T_ProjectMember m on p.PurchaseManager = m.UserCode 
                        where p.PurchaseManager = '{0}' 
                        and p.Progress in ('Submit','Approval','Approved','询价') 
                        order by p.MarkTime desc", strUserCode);   //ChineseWord
        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        LB_PurchaseSql.Text = strPurchaseHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;

            for (int i = 0; i < DG_List.Items.Count; i++)
            {
                DG_List.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdArges = e.CommandArgument.ToString();
            WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
            string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + cmdArges + "'";
            IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
            if (listPurchase != null && listPurchase.Count == 1)
            {
                WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

                if (cmdName == "price")
                {
                    //询价
                    //if (wZPurchase.Progress == LanguageHandle.GetWord("BaoPi").ToString().Trim() || wZPurchase.Progress == "Approved")
                    //{
                        //报价开始 < 报价截止，提示
                        //if (DateTime.Now < DateTime.Parse(wZPurchase.PurchaseEndTime))
                        //{
                            wZPurchase.Progress = LanguageHandle.GetWord("XunJia").ToString().Trim();
                            wZPurchase.PurchaseStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                            //采购清单的进度为询价
                            WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
                            string strWZPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where PurchaseCode= '" + wZPurchase.PurchaseCode + "'";
                            IList listWZPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strWZPurchaseDetailHQL);
                            if (listWZPurchaseDetail != null && listWZPurchaseDetail.Count > 0)
                            {
                                for (int i = 0; i < listWZPurchaseDetail.Count; i++)
                                {
                                    WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)listWZPurchaseDetail[i];
                                    wZPurchaseDetail.Progress = LanguageHandle.GetWord("XunJia").ToString().Trim();

                                    wZPurchaseDetailBLL.UpdateWZPurchaseDetail(wZPurchaseDetail, wZPurchaseDetail.ID);
                                }
                            }

                            //重新加载列表
                            DataBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJCG").ToString().Trim()+"')", true);
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBJJZRXYDSJXG").ToString().Trim()+"')", true);
                        //    return;
                        //}
                    //}
                    //else {
                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGWJJDBSBPHZPZBNJ").ToString().Trim()+"')", true);
                    //    return;
                    //}
                }
                else if (cmdName == "report")
                {
                    ////呈报
                    //if (wZPurchase.Progress == LanguageHandle.GetWord("BaoPi").ToString().Trim())
                    //{
                        string strUpLeader = HF_UpLeader.Value;
                        //if (string.IsNullOrEmpty(strUpLeader))
                        //{
                        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSJLD").ToString().Trim()+"')", true);
                        //    return;
                        //}

                        wZPurchase.Progress = LanguageHandle.GetWord("ShangBao").ToString().Trim();
                        wZPurchase.PurchaseStartTime = "-";
                        wZPurchase.UpLeader = strUpLeader;

                        wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                        //重新加载列表
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCBCG").ToString().Trim()+"')", true);
                    //}
                    //else {
                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGWJJDBSBPBNCB").ToString().Trim()+"')", true);
                    //    return;
                    //}
                }
                else if (cmdName == "return")
                {
                    ////退回
                    //if (wZPurchase.Progress == LanguageHandle.GetWord("BaoPi").ToString().Trim())
                    //{
                        wZPurchase.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                        wZPurchase.PurchaseStartTime = "-";

                        wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                        //重新加载列表
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTHCG").ToString().Trim()+"')", true);
                    //}
                    //else {
                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGWJJDBSBPBNTHLR").ToString().Trim()+"')", true);
                    //    return;
                    //}
                }
                else if (cmdName == "cancel")
                {
                    ////取消
                    //if (wZPurchase.Progress == LanguageHandle.GetWord("XunJia").ToString().Trim())
                    //{
                        wZPurchase.Progress = LanguageHandle.GetWord("BaoPi").ToString().Trim();

                        wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                        //采购清单的进度为录入
                        WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
                        string strWZPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where PurchaseCode= '" + wZPurchase.PurchaseCode + "'";
                        IList listWZPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strWZPurchaseDetailHQL);
                        if (listWZPurchaseDetail != null && listWZPurchaseDetail.Count > 0)
                        {
                            for (int i = 0; i < listWZPurchaseDetail.Count; i++)
                            {
                                WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)listWZPurchaseDetail[i];
                                wZPurchaseDetail.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();

                                wZPurchaseDetailBLL.UpdateWZPurchaseDetail(wZPurchaseDetail, wZPurchaseDetail.ID);
                            }
                        }

                        //重新加载列表
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXCG").ToString().Trim()+"')", true);
                    //}
                    //else {
                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGWJJDBSJBNXBP").ToString().Trim()+"')", true);
                    //    return;
                    //}
                }
                //else if (cmdName == "report")
                //{
                //    //呈报
                //    //if (wZPurchase.Progress == LanguageHandle.GetWord("BaoPi").ToString().Trim())
                //    //{


                //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickReport('" + wZPurchase.PurchaseCode+ "')", true);
                //    //}
                //    //else {
                //    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGWJJDBSBPBNXCB").ToString().Trim()+"')", true);
                //    //    return;
                //    //}
                //}
            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_PurchaseSql.Text.Trim();
        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();
    }



    protected void BT_Report_Click(object sender, EventArgs e)
    {
        string strPurchaseCode = HF_PurchaseCode.Value;
        string strUpLeader = HF_UpLoaderCodeName.Value;

        string[] arrUpLeader = strUpLeader.Split('|');

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            wZPurchase.Progress = LanguageHandle.GetWord("ChengBao").ToString().Trim();
            wZPurchase.PurchaseStartTime = "-";
            wZPurchase.UpLeader = arrUpLeader[0];

            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCBCG").ToString().Trim()+"')", true);
        }
    }
}
