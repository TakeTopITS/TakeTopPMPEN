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

public partial class TTGoodsCheckInReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserName;
        string strUserCode = Session["UserCode"].ToString();

        //this.Title = "物料入库报表";

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "物料入库报表", strUserCode);
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

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);
            ShareClass.InitialWarehouseTreeByAuthorityAsset(TreeView3, strUserCode, strDepartString);
            
            ShareClass.LoadVendorList(DL_VendorList, strUserCode);
        }
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView3.SelectedNode;

        if (treeNode.Target != "")
        {
            TB_WHName.Text = treeNode.Target.Trim();

            ShareClass.LoadWareHousePositions(TB_WHName.Text.Trim(), DL_WHPosition);
        }
    }

    protected void DL_VendorList_SelectedIndexChanged(object sender, EventArgs e)
    {
        TB_Manufacture.Text = DL_VendorList.SelectedValue.Trim();
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        int i;

        DataGrid1.CurrentPageIndex = 0;
        decimal deTotalNumber = 0, deTotalAmount = 0;

        string strUserCode = LB_UserCode.Text.Trim();
        string strDepartString;

        string strStartTime = DateTime.Parse(DLC_StartTime.Text).ToString("yyyyMMdd");
        string strEndTime = DateTime.Parse(DLC_EndTime.Text).ToString("yyyyMMdd");

        string strCheckInID = TB_CheckInID.Text.Trim();
        string strGoodsCode = TB_GoodsCode.Text.Trim();
        string strGoodsName = TB_GoodsName.Text.Trim();
        string strModelNumber = TB_ModelNumber.Text.Trim();
        string strSpec = TB_Spec.Text.Trim();
        string strPosition = TB_WHName.Text.Trim();
        string strManufacture = TB_Manufacture.Text.Trim();

        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";
        strPosition = "%" + strPosition + "%";
        strManufacture = "%" + strManufacture + "%";

        if (strCheckInID == "")
        {
            strHQL = "Select A.CheckInID,A.GoodsCode,A.GoodsName,A.Type,A.ModelNumber,A.Spec,A.Number,A.Price,A.IsTaxPrice,A.UnitName,A.Position,A.Manufacturer,A.OwnerCode,A.OwnerName,B.CheckInDate From T_GoodsCheckInOrderDetail A, T_GoodsCheckInOrder B where A.CheckInID = B.CheckInID and A.Status = 'InUse' ";
            strHQL += " and  A.GoodsCode Like " + "'" + strGoodsCode + "'";
            strHQL += " and A.GoodsName Like " + "'" + strGoodsName + "'";
            strHQL += " and A.ModelNumber Like " + "'" + strModelNumber + "'";
            strHQL += " and A.Spec Like " + "'" + strSpec + "'";
            strHQL += " and A.Position like " + "'" + strPosition + "'";
            strHQL += " and A.WHPosition Like "  + "'%" + DL_WHPosition.SelectedValue.Trim() + "%'";
            strHQL += " and A.Manufacturer Like " + "'" + strManufacture + "'";
            strHQL += " and A.Status = 'InUse' ";
            strHQL += " and A.Number > 0";
            strHQL += " and to_char(B.CheckInDate,'yyyymmdd')  >= " + "'" + strStartTime + "'" + "  and to_char(B.CheckInDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
            strHQL += " and A.OwnerCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")"; ;
            strHQL += " Order by A.Number DESC,A.ID DESC";
        }
        else
        {
            try
            {
                int.Parse(strCheckInID);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWRKDHZNSZSJC").ToString().Trim() + "')", true);
                return;
            }

            strHQL = "Select A.CheckInID,A.GoodsCode,A.GoodsName,A.Type,A.ModelNumber,A.Spec,A.Number,A.Price,A.IsTaxPrice,A.UnitName,A.Position,A.Manufacturer,A.OwnerCode,A.OwnerName,B.CheckInDate From T_GoodsCheckInOrderDetail A, T_GoodsCheckInOrder B where A.CheckInID = B.CheckInID and A.Status = 'InUse' ";
            strHQL += " and A.CheckInID = " + strCheckInID;
            strHQL += " and A.GoodsCode Like " + "'" + strGoodsCode + "'";
            strHQL += " and A.GoodsName Like " + "'" + strGoodsName + "'";
            strHQL += " and A.ModelNumber Like " + "'" + strModelNumber + "'";
            strHQL += " and A.Spec Like " + "'" + strSpec + "'";
            strHQL += " and A.Position like " + "'" + strPosition + "'";
            strHQL += " and A.Manufacturer Like " + "'" + strManufacture + "'";
            strHQL += " and A.Status = 'InUse' ";
            strHQL += " and A.Number > 0";
            strHQL += " and to_char(B.CheckInDate,'yyyymmdd')  >= " + "'" + strStartTime + "'" + "  and to_char(B.CheckInDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
            strHQL += " and A.OwnerCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")"; ;
            strHQL += " Order by A.Number DESC,A.ID DESC";
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsCheckInOrderDetail");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            deTotalNumber += decimal.Parse(ds.Tables[0].Rows[i][6].ToString());
            deTotalAmount += deTotalNumber * decimal.Parse(ds.Tables[0].Rows[i][7].ToString());
        }

        LB_TotalNumber.Text = deTotalNumber.ToString();
        LB_TotalAmount.Text = deTotalAmount.ToString();
    }


    protected void BT_Export_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        int i;

        DataGrid1.CurrentPageIndex = 0;
        decimal deTotalNumber = 0, deTotalAmount = 0;

        string strUserCode = LB_UserCode.Text.Trim();
        string strDepartString;

        string strStartTime = DateTime.Parse(DLC_StartTime.Text).ToString("yyyyMMdd");
        string strEndTime = DateTime.Parse(DLC_EndTime.Text).ToString("yyyyMMdd");

        string strCheckInID = TB_CheckInID.Text.Trim();
        string strGoodsCode = TB_GoodsCode.Text.Trim();
        string strGoodsName = TB_GoodsName.Text.Trim();
        string strModelNumber = TB_ModelNumber.Text.Trim();
        string strSpec = TB_Spec.Text.Trim();
        string strPosition = TB_WHName.Text.Trim();
        string strManufacture = TB_Manufacture.Text.Trim();

        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";
        strPosition = "%" + strPosition + "%";
        strManufacture = "%" + strManufacture + "%";

        if (strCheckInID == "")
        {
            strHQL = "Select A.CheckInID,A.GoodsCode,A.GoodsName,A.Type,A.ModelNumber,A.Spec,A.Number,A.Price,A.IsTaxPrice,A.UnitName,A.Position,A.Manufacturer,A.OwnerCode,A.OwnerName,B.CheckInDate From T_GoodsCheckInOrderDetail A, T_GoodsCheckInOrder B where A.CheckInID = B.CheckInID and A.Status = 'InUse' ";
            strHQL += " and  A.GoodsCode Like " + "'" + strGoodsCode + "'";
            strHQL += " and A.GoodsName Like " + "'" + strGoodsName + "'";
            strHQL += " and A.ModelNumber Like " + "'" + strModelNumber + "'";
            strHQL += " and A.Spec Like " + "'" + strSpec + "'";
            strHQL += " and A.Position like " + "'" + strPosition + "'";
            strHQL += " and A.Manufacturer Like " + "'" + strManufacture + "'";
            strHQL += " and A.Status = 'InUse' ";
            strHQL += " and A.Number > 0";
            strHQL += " and to_char(B.CheckInDate,'yyyymmdd')  >= " + "'" + strStartTime + "'" + "  and to_char(B.CheckInDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
            strHQL += " and A.OwnerCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")"; ;
            strHQL += " Order by A.Number DESC,A.ID DESC";
        }
        else
        {
            try
            {
                int.Parse(strCheckInID);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWRKDHZNSZSJC").ToString().Trim() + "')", true);
                return;
            }

            strHQL = "Select A.CheckInID,A.GoodsCode,A.GoodsName,A.Type,A.ModelNumber,A.Spec,A.Number,A.Price,A.IsTaxPrice,A.UnitName,A.Position,A.Manufacturer,A.OwnerCode,A.OwnerName,B.CheckInDate From T_GoodsCheckInOrderDetail A, T_GoodsCheckInOrder B where A.CheckInID = B.CheckInID and A.Status = 'InUse' ";
            strHQL += " and A.CheckInID = " + strCheckInID;
            strHQL += " and A.GoodsCode Like " + "'" + strGoodsCode + "'";
            strHQL += " and A.GoodsName Like " + "'" + strGoodsName + "'";
            strHQL += " and A.ModelNumber Like " + "'" + strModelNumber + "'";
            strHQL += " and A.Spec Like " + "'" + strSpec + "'";
            strHQL += " and A.Position like " + "'" + strPosition + "'";
            strHQL += " and A.Manufacturer Like " + "'" + strManufacture + "'";
            strHQL += " and A.Status = 'InUse' ";
            strHQL += " and A.Number > 0";
            strHQL += " and to_char(B.CheckInDate,'yyyymmdd')  >= " + "'" + strStartTime + "'" + "  and to_char(B.CheckInDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
            strHQL += " and A.OwnerCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")"; ;
            strHQL += " Order by A.Number DESC,A.ID DESC";
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsCheckInOrderDetail");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        DataTable dtSaleOrder = ds.Tables[0];

        Export3Excel(dtSaleOrder, LanguageHandle.GetWord("WuLiaoRuKuBaoBiaoxls").ToString().Trim());

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('导出成功！');", true);   //ChineseWord
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
