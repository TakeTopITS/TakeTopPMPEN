using System; using System.Resources;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTCarOutMileageExpense : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "资产登记入库", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); 
        if (Page.IsPostBack != true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            LoadCarAssignForm(strUserCode);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        string strStatus;

        if (e.CommandName != "Page")
        {
            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from CarAssignForm as carAssignForm Where carAssignForm.ID = " + strID;
            CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
            lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

            DataList1.DataSource = lst;
            DataList1.DataBind();

            CarAssignForm carAssignForm = (CarAssignForm)lst[0];
            NB_Mileage.Amount = carAssignForm.Mileage;
            NB_ParkingCharge.Amount = carAssignForm.ParkingCharge;
            NB_RoadToll.Amount = carAssignForm.RoadToll;

            NB_OilVolume.Amount = carAssignForm.OilVolume;
            NB_OilCharge.Amount = carAssignForm.OilCharge;

       
            strStatus = carAssignForm.Status.Trim();

            if (strStatus == "Closed" | strStatus == "Cancel")
            {
                BT_UpdateDetail.Enabled = false;
            }
            else
            {
                BT_UpdateDetail.Enabled = true;
            }

            LB_ID.Text = strID;          
        }
    }

    protected void BT_UpdateDetail_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strCarCode;
        decimal deMileage, deRoadToll, deParkingCharge, deOilVolume, deOilCharge;
        decimal dePriorMileage;

        strID = LB_ID.Text.Trim();

        strHQL = "from CarAssignForm as carAssignForm Where carAssignForm.ID = " + strID;
        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);
        CarAssignForm carAssignForm = (CarAssignForm)lst[0];      

        deMileage = NB_Mileage.Amount;
        deRoadToll = NB_RoadToll.Amount;
        deParkingCharge = NB_ParkingCharge.Amount;

        deOilVolume = NB_OilVolume.Amount;
        deOilCharge = NB_OilCharge.Amount;

        strCarCode = carAssignForm.CarCode.Trim();
        dePriorMileage = GetPriorCarAssignFromMileage(strID,strCarCode);

        try
        {          
            carAssignForm.Mileage = deMileage;
            carAssignForm.RoadToll = deRoadToll;
            carAssignForm.ParkingCharge = deParkingCharge;

            carAssignForm.OilVolume = deOilVolume;
            carAssignForm.OilCharge = deOilCharge;

            carAssignForm.CurrentMileage = deMileage - dePriorMileage;

            carAssignFormBLL.UpdateCarAssignForm(carAssignForm, int.Parse(strID));            

           // LoadCarAssignForm(strUserCode);

            strHQL = "from CarAssignForm as carAssignForm Where carAssignForm.ID = " + strID;         
            lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

            DataList1.DataSource = lst;
            DataList1.DataBind();


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG")+"')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXGSBKNFYMCHYTZSGDJC")+"')", true);
        }
    }

    protected decimal GetPriorCarAssignFromMileage(string strCarAssignID, string strCarCode)
    {
        string strHQL;

        decimal deMileage;

        strHQL = "Select A.Mileage From T_CarAssignForm A Where A.CarCode = " + "'" + strCarCode + "'" + "and A.RealDepartTime = ";
        strHQL += " (Select Max(B.RealDepartTime) From T_CarAssignForm B Where B.CarCode = " + "'" + strCarCode + "'" + " and B.RealDepartTime < ";
        strHQL += " (Select C.RealDepartTime From T_CarAssignForm C Where C.CarCode = " + "'" + strCarCode + "'" + " and C.ID = " + strCarAssignID + "))";
        strHQL += " Order By A.Mileage DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CarAssignForm");
        if (ds.Tables[0].Rows.Count > 0)
        {
            deMileage = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            strHQL = "Select InitialMileage From T_CarInformation Where CarCode = " + "'" + strCarCode + "'";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_CarInformation");
            deMileage = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }

        return deMileage;
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = "From CarAssignForm as carAssignForm Where carAssignForm.DriverCode = " + "'" + strUserCode + "'" + " And carAssignForm.Status<>'Cancel' And carAssignForm.Status<>'Closed' Order By carAssignForm.ID DESC";//过滤掉 取消或关闭 "+LanguageHandle.GetWord("ZhuangTai")+" Liujp 2013-07-16

        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        IList lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadCarAssignForm(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From CarAssignForm as carAssignForm Where carAssignForm.DriverCode = " + "'" + strUserCode + "'" + " And carAssignForm.Status<>'Cancel' And carAssignForm.Status<>'Closed' Order By carAssignForm.ID DESC";//过滤掉 取消或关闭 "+LanguageHandle.GetWord("ZhuangTai")+" Liujp 2013-07-16
        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }
}
