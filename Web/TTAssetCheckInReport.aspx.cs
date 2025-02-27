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

public partial class TTAssetCheckInReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserName;
        string strUserCode = Session["UserCode"].ToString();

        //this.Title = "资产入库报表";

        LB_UserCode.Text = strUserCode;
        strUserName = Session["UserName"].ToString();
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_StartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ShareClass.LoadWareHouseListByAuthorityForDropDownList(strUserCode, DL_WareHouse);
        }
    }

    protected void DL_WareHouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        TB_Position.Text = DL_WareHouse.SelectedValue.Trim();
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
        string strAssetCode = TB_AssetCode.Text.Trim();
        string strAssetName = TB_AssetName.Text.Trim();
        string strModelNumber = TB_ModelNumber.Text.Trim();
        string strSpec = TB_Spec.Text.Trim();
        string strPosition = TB_Position.Text.Trim();

        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strAssetCode = "%" + strAssetCode + "%";
        strAssetName = "%" + strAssetName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";
        strPosition = "%" + strPosition + "%";

        if (strCheckInID == "")
        {
            strHQL = "from AssetCheckInOrderDetail as assetCheckInOrderDetail where ";
            strHQL += " assetCheckInOrderDetail.AssetCode Like " + "'" + strAssetCode + "'";
            strHQL += " and assetCheckInOrderDetail.AssetName Like " + "'" + strAssetName + "'";
            strHQL += " and assetCheckInOrderDetail.ModelNumber Like " + "'" + strModelNumber + "'";
            strHQL += " and assetCheckInOrderDetail.Spec Like " + "'" + strSpec + "'";
            strHQL += " and assetCheckInOrderDetail.Position like " + "'" + strPosition + "'";
            strHQL += " and assetCheckInOrderDetail.Status = 'InUse' ";
            strHQL += " and assetCheckInOrderDetail.Number > 0";
            strHQL += " and to_char(assetCheckInOrderDetail.BuyTime,'yyyymmdd')  >= " + "'" + strStartTime + "'" + "  and to_char(assetCheckInOrderDetail.BuyTime,'yyyymmdd') <= " + "'" + strEndTime + "'";
            strHQL += " and assetCheckInOrderDetail.OwnerCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")"; ;
            strHQL += " Order by assetCheckInOrderDetail.Number DESC,assetCheckInOrderDetail.ID DESC";
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

            strHQL = "from AssetCheckInOrderDetail as assetCheckInOrderDetail where ";
            strHQL += " assetCheckInOrderDetail.CheckInID = " + strCheckInID;
            strHQL += " and assetCheckInOrderDetail.AssetCode Like " + "'" + strAssetCode + "'";
            strHQL += " and assetCheckInOrderDetail.AssetName Like " + "'" + strAssetName + "'";
            strHQL += " and assetCheckInOrderDetail.ModelNumber Like " + "'" + strModelNumber + "'";
            strHQL += " and assetCheckInOrderDetail.Spec Like " + "'" + strSpec + "'";
            strHQL += " and assetCheckInOrderDetail.Position like " + "'" + strPosition + "'";
            strHQL += " and assetCheckInOrderDetail.Status = 'InUse' ";
            strHQL += " and assetCheckInOrderDetail.Number > 0";
            strHQL += " and to_char(assetCheckInOrderDetail.BuyTime,'yyyymmdd')  >= " + "'" + strStartTime + "'" + "  and to_char(assetCheckInOrderDetail.BuyTime,'yyyymmdd') <= " + "'" + strEndTime + "'";
            strHQL += " and assetCheckInOrderDetail.OwnerCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")"; ;
            strHQL += " Order by assetCheckInOrderDetail.Number DESC,assetCheckInOrderDetail.ID DESC";
        }

        AssetCheckInOrderDetailBLL assetCheckInOrderDetailBLL = new AssetCheckInOrderDetailBLL();
        lst = assetCheckInOrderDetailBLL.GetAllAssetCheckInOrderDetails(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        AssetCheckInOrderDetail assetCheckInOrderDetail = new AssetCheckInOrderDetail();

        for (i = 0; i < lst.Count; i++)
        {
            assetCheckInOrderDetail = (AssetCheckInOrderDetail)lst[i];
            deTotalNumber += assetCheckInOrderDetail.Number;
            deTotalAmount += assetCheckInOrderDetail.Number * assetCheckInOrderDetail.Price;
        }

        LB_TotalNumber.Text = deTotalNumber.ToString();
        LB_TotalAmount.Text = deTotalAmount.ToString();

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text;

        AssetCheckInOrderDetailBLL assetCheckInOrderDetailBLL = new AssetCheckInOrderDetailBLL();
        IList lst = assetCheckInOrderDetailBLL.GetAllAssetCheckInOrderDetails(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_PageIndex.Text = (e.NewPageIndex + 1).ToString();
    }

}
