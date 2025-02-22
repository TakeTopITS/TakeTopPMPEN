using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZPurchasePlanListDecisionRecord : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString().Trim() : "";
        strUserName = Session["UserName"] != null ? Session["UserName"].ToString().Trim() : "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["PurchaseCode"]))
            {
                string strPurchaseCode = Request.QueryString["PurchaseCode"];
                HF_PurchaseCode.Value = strPurchaseCode;
                DataPurchaseBinder(strPurchaseCode);
            }
        }
    }

    private void DataPurchaseBinder(string strPurchaseCode)
    {
        string strPurchaseHQL = string.Format(@"select d.*,p.Progress from  T_WZSupplierApplyComment d 
                        left join T_WZPurchase p on d.PurchaseCode = p.PurchaseCode
                        where d.PurchaseCode = '{0}'", strPurchaseCode);
        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];
        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        strPurchaseHQL = string.Format(@"select d.SupplierCode1,h.UserName as SupplierName1,d.SupplierCode2,i.UserName as SupplierName2,d.SupplierCode3,j.UserName as SupplierName3 from  T_WZSupplierApplyComment d 
                        left join T_WZPurchase p on d.PurchaseCode = p.PurchaseCode
                        left join T_ProjectMember h on d.SupplierCode1 = h.UserCode
                        Left Join T_ProjectMember i on d.SupplierCode2 = i.UserCode
                        Left Join T_ProjectMember j on d.SupplierCode3 = j.UserCode
                        where d.PurchaseCode = '{0}'", strPurchaseCode);
        dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        if (dtPurchase != null && dtPurchase.Rows.Count > 0)
        {
            DataRow drPurchaseDecision = dtPurchase.Rows[0];

            TXT_PurchaseCode.Text = strPurchaseCode;

            DL_Supplier1.DataSource = dtPurchase;
            DL_Supplier1.DataBind();

            DL_Supplier2.DataSource = dtPurchase;
            DL_Supplier2.DataBind();

            DL_Supplier3.DataSource = dtPurchase;
            DL_Supplier3.DataBind();

            //HF_ExpertCode.Value = ShareClass.ObjectToString(drPurchaseDecision["ExpertCode1"]);
            //TXT_ExpertCode.Text = strUserName;

            TXT_SignTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            TXT_Progress.Text = "录入";
        }
        else
        {
            TXT_PurchaseCode.Text = "";
            TXT_Suggest.Text = "";

            TXT_ExpertCode.Text = strUserName;
            HF_ExpertCode.Value = strUserCode;
            TXT_SignTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            TXT_Progress.Text = "";
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strPurchaseCode = TXT_PurchaseCode.Text.Trim();
            string strSuggest = TXT_Suggest.Text.Trim();

            string strSupplierCode1 = DL_Supplier1.SelectedValue;
            string strSupplierCode2 = DL_Supplier1.SelectedValue;
            string strSupplierCode3 = DL_Supplier1.SelectedValue;

            string strExpertCode = HF_ExpertCode.Value.Trim();

            string strSignTime = TXT_SignTime.Text.Trim();
            string strProgress = TXT_Progress.Text.Trim();

            if (string.IsNullOrEmpty(strPurchaseCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('采购编号不能为空，请补充！');", true);
                return;
            }
            //if (string.IsNullOrEmpty(strSuggest))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('决策意见不能为空，请补充！');", true);
            //    return;
            //}
            //if (string.IsNullOrEmpty(strSupplierCode1))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('标段一不能为空，请补充！');", true);
            //    return;
            //}
            //if (string.IsNullOrEmpty(strSupplierCode2))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('标段二不能为空，请补充！');", true);
            //    return;
            //}
            //if (string.IsNullOrEmpty(strSupplierCode3))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('标段三不能为空，请补充！');", true);
            //    return;
            //}

            string strPurchaseHQL = string.Format(@"select d.*,p.Progress from T_WZPurchaseDecision d
                        left join T_WZPurchase p  on d.PurchaseCode = p.PurchaseCode
                        where d.PurchaseCode = '{0}'
                        ", strPurchaseCode, strUserCode);

            DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];
            if (dtPurchase != null && dtPurchase.Rows.Count >= 1)
            {
                //修改
                string strUpdateSQL = string.Format(@"update T_WZPurchaseDecision
                            set Decision = '{0}',
                            SupplierCode1='{1}',
                            SupplierCode2='{2}',
                            SupplierCode3='{3}' 
                            where PurchaseCode='{4}' 
                            and Decision='{5}'", strSuggest, strSupplierCode1, strSupplierCode2, strSupplierCode3, strPurchaseCode, strUserCode);
                ShareClass.RunSqlCommand(strUpdateSQL);
            }
            else
            {
                //新增
                string strInsertSQL = string.Format(@"insert into T_WZPurchaseDecision
                            (PurchaseCode,Decision,SupplierCode1,SupplierCode2,SupplierCode3,DecisionDesc,DecisionTime)
                            values('{0}','{1}','{2}','{3}','{4}','{5}',now())",
                            strPurchaseCode, strUserCode, strSupplierCode1, strSupplierCode2, strSupplierCode3, strSuggest);
                ShareClass.RunSqlCommand(strInsertSQL);
            }

            ////重新加载
            //DataPurchaseBinder(HF_PurchaseCode.Value);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');", true);
        }
        catch (Exception ex)
        {
        }
    }


    protected void BT_SortSupplierCode1_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strPurchaseHQL = string.Format(@"select d.*,p.Progress from T_WZPurchase p
                        left join T_WZSupplierApplyComment d on p.PurchaseCode = d.PurchaseCode
                        where p.PurchaseCode = '{0}'", HF_PurchaseCode.Value);

        if (!string.IsNullOrEmpty(HF_SortSupplierCode1.Value))
        {
            strPurchaseHQL += " order by d.SupplierCode1 desc";

            HF_SortSupplierCode1.Value = "";
        }
        else
        {
            strPurchaseHQL += " order by d.SupplierCode1 asc";

            HF_SortSupplierCode1.Value = "asc";
        }

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

    }



    protected void BT_SortSupplierCode2_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strPurchaseHQL = string.Format(@"select d.*,p.Progress from T_WZPurchase p
                        left join T_WZSupplierApplyComment d on p.PurchaseCode = d.PurchaseCode
                        where p.PurchaseCode = '{0}'", HF_PurchaseCode.Value);

        if (!string.IsNullOrEmpty(HF_SortSupplierCode2.Value))
        {
            strPurchaseHQL += " order by d.SupplierCode2 desc";

            HF_SortSupplierCode2.Value = "";
        }
        else
        {
            strPurchaseHQL += " order by d.SupplierCode2 asc";

            HF_SortSupplierCode2.Value = "asc";
        }

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();
    }


    protected void BT_SortSupplierCode3_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strPurchaseHQL = string.Format(@"select d.*,p.Progress from T_WZPurchase p
                        left join T_WZSupplierApplyComment d on p.PurchaseCode = d.PurchaseCode
                        where p.PurchaseCode = '{0}'", HF_PurchaseCode.Value);

        if (!string.IsNullOrEmpty(HF_SortSupplierCode3.Value))
        {
            strPurchaseHQL += " order by d.SupplierCode3 desc";

            HF_SortSupplierCode3.Value = "";
        }
        else
        {
            strPurchaseHQL += " order by d.SupplierCode3 asc";

            HF_SortSupplierCode3.Value = "asc";
        }

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();
    }

}