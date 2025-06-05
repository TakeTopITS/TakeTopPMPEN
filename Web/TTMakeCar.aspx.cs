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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTMakeCar : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strDepartString;

        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","车辆档案", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "车辆档案";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_BuyTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadCarType();
        
            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityAsset(LanguageHandle.GetWord("ZZJGT"),TreeView1, strUserCode);
            TB_DepartString.Text = strDepartString;

            LoadCarList(strUserCode);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            TB_DepartCode.Text = strDepartCode;
            LB_DepartName.Text = ShareClass.GetDepartName(strDepartCode);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        TB_CarCode.Text = "";
        LB_CarCode.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strCarCode;

        strCarCode = LB_CarCode.Text.Trim();

        if (strCarCode == "")
        {
            AddCar();
        }
        else
        {
            UpdateCar();
        }
    }

    protected void AddCar()
    {
        string strCarCode, strCarName, strCarBrand, strCarColor;
        string strCarType, strEngineCode, strFrameCode;
        string strVendor, strDepartCode, strStatus;
        decimal deDWT, deInitialMileage;
        int intSeatNumber;
        DateTime dtPurTime;

        strCarCode = TB_CarCode.Text.Trim();
        strCarName = TB_CarName.Text.Trim();
        strCarBrand = TB_CarBrand.Text.Trim();
        strCarColor = TB_CarColor.Text.Trim();
        strCarType = DL_CarType.SelectedValue.Trim();
        strEngineCode = TB_EngineCode.Text.Trim();
        strFrameCode = TB_FrameCode.Text.Trim();
        strVendor = TB_Vendor.Text.Trim();
        strDepartCode = TB_DepartCode.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        deDWT = NB_DWT.Amount;
        deInitialMileage = NB_InitialMileage.Amount;
        intSeatNumber = int.Parse(NB_SeatNumber.Amount.ToString());
        dtPurTime = DateTime.Parse(DLC_BuyTime.Text);

        if (strDepartCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGGSBMBNWKCZSBJC")+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }

        CarInformationBLL carInformationBLL = new CarInformationBLL();
        CarInformation carInformation = new CarInformation();

        carInformation.CarCode = strCarCode;
        carInformation.CarName = strCarName;
        carInformation.CarBrand = strCarBrand;
        carInformation.CarColor = strCarColor;
        carInformation.CarType = strCarType;
        carInformation.EngineCode = strEngineCode;
        carInformation.FrameCode = strFrameCode;
        carInformation.Vendor = strVendor;
        carInformation.BelongDepartCode  = strDepartCode;
        carInformation.BelongDepartName = ShareClass.GetDepartName(strDepartCode);
        carInformation.Status = strStatus;

        carInformation.DWT = deDWT;
        carInformation.InitialMileage = deInitialMileage;
        carInformation.SeatNumber = intSeatNumber;
        carInformation.PurchaseTime = dtPurTime;

        try
        {
            carInformationBLL.AddCarInformation(carInformation);

            LB_CarCode.Text = strCarCode;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG")+"')", true);

            LoadCarList(strUserCode);
        }
        catch
        {
           ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB")+"')", true);

           ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateCar()
    {
        string strCarCode, strCarName, strCarBrand, strCarColor;
        string strCarType, strEngineCode, strFrameCode;
        string strVendor, strDepartCode, strStatus;
        decimal deDWT, deInitialMileage;
        int intSeatNumber;
        DateTime dtPurTime;

        strCarCode = TB_CarCode.Text.Trim();
        strCarName = TB_CarName.Text.Trim();
        strCarBrand = TB_CarBrand.Text.Trim();
        strCarColor = TB_CarColor.Text.Trim();
        strCarType = DL_CarType.SelectedValue.Trim();
        strEngineCode = TB_EngineCode.Text.Trim();
        strFrameCode = TB_FrameCode.Text.Trim();
        strVendor = TB_Vendor.Text.Trim();
        strDepartCode = TB_DepartCode.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        deDWT = NB_DWT.Amount;
        deInitialMileage = NB_InitialMileage.Amount;
        intSeatNumber = int.Parse(NB_SeatNumber.Amount.ToString());
        dtPurTime = DateTime.Parse(DLC_BuyTime.Text);

        if (strDepartCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGGSBMBNWKCZSBJC")+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        CarInformationBLL carInformationBLL = new CarInformationBLL();
        CarInformation carInformation = new CarInformation();

        carInformation.CarCode = strCarCode;
        carInformation.CarName = strCarName;
        carInformation.CarBrand = strCarBrand;
        carInformation.CarColor = strCarColor;
        carInformation.CarType = strCarType;
        carInformation.EngineCode = strEngineCode;
        carInformation.FrameCode = strFrameCode;
        carInformation.Vendor = strVendor;
        carInformation.BelongDepartCode = strDepartCode;
        carInformation.BelongDepartName = ShareClass.GetDepartName(strDepartCode);
        carInformation.Status = strStatus;

        carInformation.DWT = deDWT;
        carInformation.InitialMileage = deInitialMileage;
        carInformation.SeatNumber = intSeatNumber;
        carInformation.PurchaseTime = dtPurTime;

        try
        {
            carInformationBLL.UpdateCarInformation(carInformation, strCarCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG")+"')", true);

            LoadCarList(strUserCode);
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + ex.Message.ToString() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strCarCode, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strCarCode = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From CarInformation as carInformation where carInformation.CarCode = " + "'" + strCarCode + "'";
                CarInformationBLL carInformationBLL = new CarInformationBLL();
                lst = carInformationBLL.GetAllCarInformations(strHQL);

                CarInformation carInformation = (CarInformation)lst[0];

                LB_CarCode.Text = carInformation.CarCode.Trim();
                TB_CarCode.Text = carInformation.CarCode.Trim();
                TB_CarName.Text = carInformation.CarName.Trim();
                TB_CarBrand.Text = carInformation.CarBrand.Trim();
                TB_CarColor.Text = carInformation.CarColor.Trim();
                TB_EngineCode.Text = carInformation.EngineCode.Trim();
                TB_FrameCode.Text = carInformation.FrameCode.Trim();
                TB_Vendor.Text = carInformation.Vendor.Trim();
                DL_CarType.SelectedValue = carInformation.CarType;

                NB_DWT.Amount = carInformation.DWT;
                NB_FuelConsumption.Amount = carInformation.FuelConsumption;
                NB_InitialMileage.Amount = carInformation.InitialMileage;
                NB_Price.Amount = carInformation.Price;

                TB_DepartCode.Text = carInformation.BelongDepartCode;
                LB_DepartName.Text = carInformation.BelongDepartName;
                DLC_BuyTime.Text = carInformation.PurchaseTime.ToString("yyyy-MM-dd");

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                strHQL = "Delete From T_CarInformation Where CarCode = " + "'" + strCarCode + "'";

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);

                    LoadCarList(strUserCode);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
                }
            }
        }
    }

    protected void LoadCarList(string strUserCode)
    {
        string strHQL;
        string strDepartString;

        strDepartString = TB_DepartString.Text;

        strHQL = " Select * From T_CarInformation Where ";
        strHQL += " BelongDepartCode in " + strDepartString;
        strHQL += " Order By CarCode ASC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CarInformation");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }  

    protected void LoadCarType()
    {
        string strHQL;
        IList lst;

        strHQL = "From CarType as carType Order By carType.SortNumber ASC";
        CarTypeBLL carTypeBLL = new CarTypeBLL();

        lst = carTypeBLL.GetAllCarTypes(strHQL);

        DL_CarType.DataSource = lst;
        DL_CarType.DataBind();
    }
    
}
