using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGoodsProductionOrderKeepAcccount : System.Web.UI.Page
{
    private string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserName, strDepartString;

        strUserCode = Session["UserCode"].ToString();

        //this.Title = "MaterialProduction";

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "��ҵ���ü���", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);
            LB_DepartString.Text = strDepartString;

            //ȡ�û�ƿ�Ŀ�б�
            ShareClass.LoadAccountForDDL(DL_Account);

            LoadGoodsProductionOrder(strUserCode);
        }
    }


    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strPDID;


            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid5.Items.Count; i++)
                {
                    DataGrid5.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;
            }

            strPDID = e.Item.Cells[2].Text.Trim();
            LB_PDID.Text = strPDID;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void BT_KeepAccount_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text;
        string strPDID, strAccountCode, strAccountName;
        int intReAndPayalbeID;
        string strHQL;

        strPDID = LB_PDID.Text.Trim();

        strAccountCode = lbl_AccountCode.Text.Trim();
        strAccountName = ShareClass.GetAccountName(strAccountCode);

        GoodsProductionOrder goodsProductionOrder = GetGoodsProductionOrder(strPDID);

        //����Ӧ��Ӧ�����ݵ�Ӧ��Ӧ����
        string strCurrencyType = goodsProductionOrder.CurrencyType;
        string strReAndPayer = goodsProductionOrder.BelongDepartName.Trim();
        string strStatus = goodsProductionOrder.Status.Trim();
        int intRelatedID = goodsProductionOrder.RelatedID;
        string strRelatedType = goodsProductionOrder.RelatedType.Trim();
        string strApplicantCode = goodsProductionOrder.CreatorCode.Trim();
        string strApplicantName = goodsProductionOrder.CreatorName.Trim();
        string strPayMethod = TB_PaymentMethod.Text.Trim();

        decimal deDetailAmount = goodsProductionOrder.Amount;

        if (strStatus != "Recorded")
        {
            intReAndPayalbeID = ShareClass.InsertReceivablesOrPayableByAccount("Payables", "GoodsPD", "GoodsPD", strPDID, strPDID, strAccountCode, strAccountName, deDetailAmount, strCurrencyType, strReAndPayer, strApplicantCode, intRelatedID);
            ShareClass.InsertReceivablesOrPayableRecord("Payables", intReAndPayalbeID, deDetailAmount, strCurrencyType, strPayMethod, strReAndPayer, strApplicantCode, intRelatedID);

            strHQL = "Update T_ConstractPayable Set OutOfPocketAccount = " + deDetailAmount.ToString() + ",UNPayAmount = 0 Where ID = " + intReAndPayalbeID.ToString();
            ShareClass.RunSqlCommand(strHQL);

            //���걨����������Ŀ����
            if (strRelatedType == "Project" & intRelatedID > 1)
            {
                ShareClass.AddConstractPayAmountToProExpense(intRelatedID.ToString(), intReAndPayalbeID.ToString(), strAccountCode, strAccountName, "�����ӹ�����", deDetailAmount, strCurrencyType, strApplicantCode, strApplicantName);
            }


            strHQL = "Update T_GoodsProductionOrder Set Status = 'Recorded' Where PDID = " + strPDID;
            ShareClass.RunSqlCommand(strHQL);


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJZCG + "')", true);

            LoadGoodsProductionOrder(strUserCode);

        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSBCSQDYJJZBNCFJC + "')", true);
        }
    }

    protected void DL_Account_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strAccountCode = DL_Account.SelectedValue.Trim();
        lbl_AccountCode.Text = strAccountCode;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DL_PaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        TB_PaymentMethod.Text = DL_PaymentMethod.SelectedValue;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        GoodsProductionOrderBLL puchaseOrderBLL = new GoodsProductionOrderBLL();
        IList lst = puchaseOrderBLL.GetAllGoodsProductionOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }

    protected GoodsProductionOrder GetGoodsProductionOrder(string strPDID)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsProductionOrder as goodsProductionOrder where goodsProductionOrder.PDID = " + strPDID;
        GoodsProductionOrderBLL goodsProductionOrderBLL = new GoodsProductionOrderBLL();
        lst = goodsProductionOrderBLL.GetAllGoodsProductionOrders(strHQL);

        return (GoodsProductionOrder)lst[0];
    }

    protected void LoadGoodsProductionOrder(string strUserCode)
    {
        string strHQL;
        IList lst;

        //Workflow,���������ģ�����ж�

        strHQL = "from GoodsProductionOrder as goodsProductionOrder where goodsProductionOrder.CreatorCode = " + "'" + strUserCode + "'";
        strHQL += " or goodsProductionOrder.CreatorCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ") ";
        strHQL += " Order by goodsProductionOrder.PDID DESC";
        GoodsProductionOrderBLL goodsProductionOrderBLL = new GoodsProductionOrderBLL();
        lst = goodsProductionOrderBLL.GetAllGoodsProductionOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

}