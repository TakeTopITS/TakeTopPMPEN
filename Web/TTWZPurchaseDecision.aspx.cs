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

public partial class TTWZPurchaseDecision : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (Request.QueryString["PurchaseCode"] != null)
            {
                string strPurchaseCode = Request.QueryString["PurchaseCode"];

                HF_PurchaseCode.Value = strPurchaseCode;

                DataBinder(strPurchaseCode);
            }
        }
    }

    private void DataBinder(string strPurchaseCode)
    {


        string strPurchaseHQL = string.Format(@"select p.*,a.SupplierCode,a.SumApplyMoney,COALESCE(c.ExpertCode, '没有') as IsSelect,c.Suggest,sl.SupplierName,ps.DocumentName as SupplierDocumentName,ps.DocumentURL as SupplierDocumentURL from
                                                (
                                                select t.SupplierCode,t.PurchaseCode,Sum(t.ApplyMoney) as SumApplyMoney from
                                                (
                                                select ld.SupplierCode,ld.PurchaseCode,COALESCE(ld.ApplyMoney,0)*lp.PurchaseNumber  as ApplyMoney  from T_WZSupplierApplyDetail ld
                                                left join T_WZPurchaseDetail lp on ld.PurchaseDetailID = lp.ID
                                                ) t
                                                --where t.SupplierCode = '{1}'
                                                group by t.PurchaseCode,t.SupplierCode
                                                ) a 
                                                left join T_WZPurchase p on a.PurchaseCode = p.PurchaseCode
                                                left join T_WZSupplierApplyComment c on c.PurchaseCode = a.PurchaseCode
                                                and c.SupplierCode = a.SupplierCode
                                                and c.ExpertCode = '{1}'
                                                left join T_WZSupplier sl on a.SupplierCode = sl.SupplierCode
                                                left join T_WZPurchaseSupplier ps on a.SupplierCode = ps.SupplierCode
                                                and a.PurchaseCode = ps.PurchaseCode
                                                where p.PurchaseCode = '{0}'", strPurchaseCode, strUserCode); 
        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();
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

            if (cmdName == "select")
            {
                string cmdArges = e.CommandArgument.ToString();

                //判断是否的在采购报名截止日期和开始时间内
                string strCheckPurchaseHQL = string.Format(@"select * from T_WZPurchase 
                                    where (PurchaseEndTime < now() or PurchaseStartTime > now())
                                    and PurchaseCode = '{0}'", HF_PurchaseCode.Value);
                DataTable dtCheckPurchase = ShareClass.GetDataSetFromSql(strCheckPurchaseHQL, "CheckPurchase").Tables[0];
                if (dtCheckPurchase != null && dtCheckPurchase.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGBJBZJZRHKSSJN").ToString().Trim()+"')", true);
                    return;
                }

                WZSupplierApplyCommentBLL wZSupplierApplyCommentBLL = new WZSupplierApplyCommentBLL();
                string strWZSupplierApplyCommentHQL = "from WZSupplierApplyComment as wZSupplierApplyComment where PurchaseCode = '" + HF_PurchaseCode.Value + "' and ExpertCode = '" + strUserCode + "'";
                IList lstWZSupplierApplyComment = wZSupplierApplyCommentBLL.GetAllWZSupplierApplyComments(strWZSupplierApplyCommentHQL);
                if (lstWZSupplierApplyComment != null && lstWZSupplierApplyComment.Count == 1)
                {
                    ////修改
                    //WZSupplierApplyComment wZSupplierApplyComment = (WZSupplierApplyComment)lstWZSupplierApplyComment[0];
                    //if (wZSupplierApplyComment.SupplierCode == cmdArges)
                    //{
                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYJZLDGYSBXYZFDJ").ToString().Trim()+"')", true);
                    //    return;
                    //}

                    //wZSupplierApplyComment.SupplierCode = cmdArges;

                    //wZSupplierApplyCommentBLL.UpdateWZSupplierApplyComment(wZSupplierApplyComment, wZSupplierApplyComment.ID);
                }
                else
                {
                    //新增
                    //WZSupplierApplyComment wZSupplierApplyComment = new WZSupplierApplyComment();
                    //wZSupplierApplyComment.PurchaseCode = HF_PurchaseCode.Value;
                    //wZSupplierApplyComment.ExpertCode = strUserCode;
                    //wZSupplierApplyComment.SupplierCode = cmdName;
                    //wZSupplierApplyComment.Suggest = "";
                    //wZSupplierApplyComment.SignTime = DateTime.Now;

                    //wZSupplierApplyCommentBLL.AddWZSupplierApplyComment(wZSupplierApplyComment);
                }

                DataBinder(HF_PurchaseCode.Value);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZCG").ToString().Trim()+"')", true);


            }
            else if (cmdName == "request")
            {
                string cmdArges = e.CommandArgument.ToString();
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickSign('" + cmdArges + "')", true);
            }
        }
    }


    protected void BT_HiddenButton_Click(object sender, EventArgs e)
    {
        string strSupplierCode = HF_SupplierCode.Value;
        string strSuggest = HF_Suggest.Value;

        //判断是否的在采购报名截止日期和开始时间内
        string strCheckPurchaseHQL = string.Format(@"select * from T_WZPurchase 
                                    where (PurchaseEndTime < now() or PurchaseStartTime > now())
                                    and PurchaseCode = '{0}'", HF_PurchaseCode.Value);
        DataTable dtCheckPurchase = ShareClass.GetDataSetFromSql(strCheckPurchaseHQL, "CheckPurchase").Tables[0];
        if (dtCheckPurchase != null && dtCheckPurchase.Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGBJBZJZRHKSSJN").ToString().Trim()+"')", true);
            return;
        }

        WZSupplierApplyCommentBLL wZSupplierApplyCommentBLL = new WZSupplierApplyCommentBLL();
        string strWZSupplierApplyCommentHQL = "from WZSupplierApplyComment as wZSupplierApplyComment where PurchaseCode = '" + HF_PurchaseCode.Value + "' and ExpertCode = '" + strUserCode + "'";
        IList lstWZSupplierApplyComment = wZSupplierApplyCommentBLL.GetAllWZSupplierApplyComments(strWZSupplierApplyCommentHQL);
        if (lstWZSupplierApplyComment != null && lstWZSupplierApplyComment.Count == 1)
        {
            ////修改
            //WZSupplierApplyComment wZSupplierApplyComment = (WZSupplierApplyComment)lstWZSupplierApplyComment[0];
            //if (wZSupplierApplyComment.SupplierCode == strSupplierCode)
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYJZLDGYSBXYZFDJ").ToString().Trim()+"')", true);
            //    return;
            //}

            //wZSupplierApplyComment.SupplierCode = strSupplierCode;
            //wZSupplierApplyComment.Suggest = strSuggest;
            //wZSupplierApplyComment.SignTime = DateTime.Now;

            //wZSupplierApplyCommentBLL.UpdateWZSupplierApplyComment(wZSupplierApplyComment, wZSupplierApplyComment.ID);
        }
        else
        {
            ////新增
            //WZSupplierApplyComment wZSupplierApplyComment = new WZSupplierApplyComment();
            //wZSupplierApplyComment.PurchaseCode = HF_PurchaseCode.Value;
            //wZSupplierApplyComment.ExpertCode = strUserCode;
            //wZSupplierApplyComment.SupplierCode = strSupplierCode;
            //wZSupplierApplyComment.Suggest = strSuggest;
            //wZSupplierApplyComment.SignTime = DateTime.Now;

            //wZSupplierApplyCommentBLL.AddWZSupplierApplyComment(wZSupplierApplyComment);
        }

        DataBinder(HF_PurchaseCode.Value);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZCG").ToString().Trim()+"')", true);
    }
}
