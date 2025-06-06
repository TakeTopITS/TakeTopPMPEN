using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTGoodsSaleReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strDepartString;

        string strUserName;
        string strUserCode = Session["UserCode"].ToString();

        //this.Title = "物料销售报表";

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "物料销售报表", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", " aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_StartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ShareClass.LoadCustomer(DL_Customer, strUserCode);
            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);
        }
    }

    protected void DL_Customer_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strCustomerName = DL_Customer.SelectedItem.Text.Trim();

        TB_CustomerName.Text = strCustomerName;
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;

        int i;
        decimal deTotalNumber = 0, deTotalAmount = 0;
        string strStartTime, strEndTime, strCustomerName;
        string strSalesName;

        string strDepartString;
        string strUserCode = LB_UserCode.Text.Trim();

        string strGoodsCode = TB_GoodsCode.Text.Trim();
        string strGoodsName = TB_GoodsName.Text.Trim();

        string strModelNumber = TB_ModelNumber.Text.Trim();
        string strSpec = TB_Spec.Text.Trim();

        strCustomerName = "%" + TB_CustomerName.Text.Trim() + "%";

        strStartTime = DateTime.Parse(DLC_StartTime.Text).ToString("yyyyMMdd");
        strEndTime = DateTime.Parse(DLC_EndTime.Text).ToString("yyyyMMdd");

        strSalesName = TB_SalesName.Text.Trim();
        strSalesName = "%" + strSalesName + "%";

        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strHQL = "Select * from T_GoodsSaleRecord A,T_GoodsSaleOrder B where A.SOID = B.SOID";
        strHQL += " and to_char(B.SaleTime,'yyyymmdd')  >= " + "'" + strStartTime + "'" + "  and to_char(B.SaleTime,'yyyymmdd') <= " + "'" + strEndTime + "'";
        strHQL += " and B.SalesName like " + "'" + strSalesName + "'";
        strHQL += " and B.CustomerName Like " + "'" + strCustomerName + "'";
        strHQL += " and A.GoodsCode Like " + "'" + strGoodsCode + "'";
        strHQL += " and A.GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and A.ModelNumber like " + "'" + strModelNumber + "'";
        strHQL += " and A.Spec Like " + "'" + strSpec + "'";
        strHQL += " and B.SalesCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")";
        strHQL += " Order by A.ID DESC";

        LB_Sql.Text = strHQL;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            deTotalNumber += decimal.Parse(ds.Tables[0].Rows[i]["Number"].ToString());
            deTotalAmount += decimal.Parse(ds.Tables[0].Rows[i]["Number"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["Price"].ToString());
        }

        LB_TotalNumber.Text = deTotalNumber.ToString();
        LB_TotalAmount.Text = deTotalAmount.ToString();

        LB_Sql.Text = strHQL;
    }

    //取得同一销售明细的退货量
    protected decimal getSaleOrderReturnNumber(string strSORecordID)
    {
        string strHQL;

        strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsReturnDetail Where SourceType = 'GoodsSORecord' and SourceID = " + strSORecordID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsReturnRecord");

        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        catch
        {
            return 0;
        }
    }

    //取得同一销售明细的退货量
    protected decimal getSaleOrderRealReceiveNumber(string strSORecordID)
    {
        string strHQL;

        strHQL = "Select COALESCE(Sum(RealReceiveNumber),0) From T_GoodsDeliveryOrderDetail Where SourceType = 'GoodsSORecord' and SourceID = " + strSORecordID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsDeliveryOrderDetail");

        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        catch
        {
            return 0;
        }
    }

    protected void BT_Export_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strStartTime, strEndTime, strCustomerName;
        string strSalesName;

        string strDepartString;
        string strUserCode = LB_UserCode.Text.Trim();

        string strGoodsCode = TB_GoodsCode.Text.Trim();
        string strGoodsName = TB_GoodsName.Text.Trim();

        string strModelNumber = TB_ModelNumber.Text.Trim();
        string strSpec = TB_Spec.Text.Trim();

        strCustomerName = "%" + TB_CustomerName.Text.Trim() + "%";

        strStartTime = DateTime.Parse(DLC_StartTime.Text).ToString("yyyyMMdd");
        strEndTime = DateTime.Parse(DLC_EndTime.Text).ToString("yyyyMMdd");

        strSalesName = TB_SalesName.Text.Trim();
        strSalesName = "%" + strSalesName + "%";

        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        //,A.NoticeOutNumber '通知出货量'
        //,A.CheckOutNumber '出库量'
        //,A.DeliveryNumber '送化量'
        //,A.RealReceiveNumber '收货量'

        strHQL = string.Format(@"Select  B.SOName '{0}' 
        ,B.SalesName '{1}'
        ,B.CustomerName 'Customer'
        ,B.SaleTime '{2}'
        ,A.ID '{3}'
        ,A.GoodsCode '{4}'
        ,A.GoodsName '{5}'
        ,A.Number '{6}'
        ,(Select COALESCE(Sum(RealReceiveNumber),0) From T_GoodsDeliveryOrderDetail Where SourceType = 'GoodsSORecord' and SourceID = A.ID) {7}
        ,A.ModelNumber '{8}'
        ,A.Spec '{9}'
        ,A.Unit '{10}'
        ,A.PackNumber '{11}'
        ,A.Price '{12}'
        ,A.Amount '{13}'
        ,B.CurrencyType '{14}'
        ,B.CarCode '{15}'
        ,B.Driver '{16}'
        ,B.OpenInvoiceTime '{17}'
        ,B.InvoiceCode '{18}'
        ,A.SaleReason '{19}'
        from T_GoodsSaleRecord A,T_GoodsSaleOrder B where A.SOID = B.SOID",
            LanguageHandle.GetWord("MingCheng"),
            LanguageHandle.GetWord("YeWuYuan"),
            LanguageHandle.GetWord("ShiJian"),
            LanguageHandle.GetWord("BianHao"),
            LanguageHandle.GetWord("DaiMa"),
            LanguageHandle.GetWord("ShangPinMingCheng"),
            LanguageHandle.GetWord("ShuLiang"),
            LanguageHandle.GetWord("ShiShouLiang"),
            LanguageHandle.GetWord("XingHao"),
            LanguageHandle.GetWord("GuiGe"),
            LanguageHandle.GetWord("DanWei"),
            LanguageHandle.GetWord("JianShu"),
            LanguageHandle.GetWord("DanJia"),
            LanguageHandle.GetWord("JinE"),
            LanguageHandle.GetWord("BiBie"),
            LanguageHandle.GetWord("CheHao"),
            LanguageHandle.GetWord("SiJi"),
            LanguageHandle.GetWord("KaiPiaoShiJian"),
            LanguageHandle.GetWord("PiaoHao"),
            LanguageHandle.GetWord("BeiZhu"));

        strHQL += " and to_char(B.SaleTime,'yyyymmdd')  >= " + "'" + strStartTime + "'" + "  and to_char(B.SaleTime,'yyyymmdd') <= " + "'" + strEndTime + "'";
        strHQL += " and B.SalesName like " + "'" + strSalesName + "'";
        strHQL += " and B.CustomerName Like " + "'" + strCustomerName + "'";
        strHQL += " and A.GoodsCode Like " + "'" + strGoodsCode + "'";
        strHQL += " and A.GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and A.ModelNumber like " + "'" + strModelNumber + "'";
        strHQL += " and A.Spec Like " + "'" + strSpec + "'";
        strHQL += " and B.SalesCode  in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")";
        strHQL += " Order by A.ID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleRecord");
        DataTable dtSaleOrder = ds.Tables[0];

        Export3Excel(dtSaleOrder, "物料销售报表.xls");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('导出成功！');", true);
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

    protected void LoadCustomer(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strHQL = "from Customer as customer ";
        strHQL += " Where (customer.CreatorCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " or (customer.CustomerCode in (Select customerRelatedUser.CustomerCode from CustomerRelatedUser as customerRelatedUser where customerRelatedUser.UserCode = " + "'" + strUserCode + "'" + "))";
        strHQL += " Or customer.CreatorCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode In  " + strDepartString + ")";
        strHQL += " Order by customer.CreateDate DESC";

        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);

        DL_Customer.DataSource = lst;
        DL_Customer.DataBind();

        DL_Customer.Items.Insert(0, new ListItem("--Select--", ""));
    }

}
