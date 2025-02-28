using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTCarOilTypeRecord : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","车辆加油记录", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            FillInDropDownList(ddl_OilInformation, true);//绑定石油

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

            LoadCarInformation(strDepartString);
        }
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;
        string CarNo = LB_ID.Text.Trim();//车牌号
        if (e.CommandName != "Page")
        {
            string strID = e.Item.Cells[0].Text.Trim();

            CarOilTypeRecordBLL carOilTypeRecordBLL = new CarOilTypeRecordBLL();
            CarOilTypeRecord carOilTypeRecord = new CarOilTypeRecord();

            carOilTypeRecord.ID = int.Parse(strID);
            carOilTypeRecordBLL.DeleteCarOilTypeRecord(carOilTypeRecord);

            strHQL = "From CarOilTypeRecord as carOilTypeRecord Where carOilTypeRecord.CarNo = '" + CarNo + "' Order By carOilTypeRecord.CreateTime DESC";
            lst = carOilTypeRecordBLL.GetAllCarOilTypeRecord(strHQL);

            DataGrid2.CurrentPageIndex = 0;
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string CarNo = LB_ID.Text.Trim();//车牌号

        string strHQL = "From CarOilTypeRecord as carOilTypeRecord Where carOilTypeRecord.CarNo = '" + CarNo + "' Order By carOilTypeRecord.CreateTime DESC";

        CarOilTypeRecordBLL carOilTypeRecordBLL = new CarOilTypeRecordBLL();
        IList lst = carOilTypeRecordBLL.GetAllCarOilTypeRecord(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        string strStatus;

        if (e.CommandName != "Page")
        {
            strID = ((Button)e.Item.FindControl("BT_CarCode")).Text.Trim();//车牌号

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from CarInformation as carInformation Where carInformation.CarCode = '" + strID + "' ";
            CarInformationBLL carInformationBLL = new CarInformationBLL();
            lst = carInformationBLL.GetAllCarInformations(strHQL);

            CarInformation carInformation = (CarInformation)lst[0];
            lbl_CarNo.Text = carInformation.CarCode;
            lbl_DepartName.Text = carInformation.BelongDepartName;
            lbl_DepartCode.Text = carInformation.BelongDepartCode;

            strStatus = carInformation.Status.Trim();

            if (strStatus == "Scrapped")
            {
                BT_UpdateDetail.Enabled = false;
            }
            else
            {
                BT_UpdateDetail.Enabled = true;
            }

           LB_ID.Text = strID;

            LoadCarOilTypeRecord(strID);
        }
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

    //    string strHQL = "From CarInformation as CarInformation Where CarInformation.BelongDepartCode in " + strDepartString + " Order By CarInformation.PurchaseTime DESC";

        string strHQL = "From CarInformation as carInformation Where carInformation.Status<>'Scrapped' Order By carInformation.PurchaseTime DESC";

        CarInformationBLL carInformationBLL = new CarInformationBLL();
        IList lst = carInformationBLL.GetAllCarInformations(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void BT_UpdateDetail_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        CarOilTypeRecordBLL carOilTypeRecordBLL = new CarOilTypeRecordBLL();
        CarOilTypeRecord carOilTypeRecord = new CarOilTypeRecord();
        if (string.IsNullOrEmpty(ddl_OilInformation.SelectedValue) || ddl_OilInformation.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGYLBNWK").ToString().Trim()+"')", true);
            return;
        }
        carOilTypeRecord.CarNo = LB_ID.Text.Trim();//车牌号
        carOilTypeRecord.CreateTime = DateTime.Now;
        carOilTypeRecord.OilTypeId = int.Parse(ddl_OilInformation.SelectedValue);
        carOilTypeRecord.OilName = GetOilTypeName(int.Parse(ddl_OilInformation.SelectedValue)).Substring(0, GetOilTypeName(int.Parse(ddl_OilInformation.SelectedValue)).IndexOf("@"));
        carOilTypeRecord.Type = GetOilTypeName(int.Parse(ddl_OilInformation.SelectedValue)).Substring(GetOilTypeName(int.Parse(ddl_OilInformation.SelectedValue)).IndexOf("@") + 1);
        carOilTypeRecord.DepartCode = lbl_DepartCode.Text;
        carOilTypeRecord.OilNum = decimal.Parse(string.IsNullOrEmpty(NB_OilVolume.Text) ? "0" : NB_OilVolume.Text);
        carOilTypeRecord.OilPrice = decimal.Parse(string.IsNullOrEmpty(NB_OilPrice.Text) ? "0" : NB_OilPrice.Text);
        carOilTypeRecord.OilMoney = carOilTypeRecord.OilNum * carOilTypeRecord.OilPrice;

        try
        {
            carOilTypeRecordBLL.AddCarOilTypeRecord(carOilTypeRecord);

            UpdateCarAssignFormOil(carOilTypeRecord.CarNo);

            strHQL = "from CarOilTypeRecord as carOilTypeRecord Where carOilTypeRecord.CarNo = '" + carOilTypeRecord.CarNo + "' ";
            lst = carOilTypeRecordBLL.GetAllCarOilTypeRecord(strHQL);

            DataGrid2.CurrentPageIndex = 0;
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSB").ToString().Trim()+"')", true);
        }
    }

    protected void UpdateCarAssignFormOil(string strCarCode)
    {
        CarOilTypeRecordBLL carOilTypeRecordBLL = new CarOilTypeRecordBLL();
        string strHQL = "from CarOilTypeRecord as carOilTypeRecord Where carOilTypeRecord.CarNo = '" + strCarCode + "' ";
        IList lst = carOilTypeRecordBLL.GetAllCarOilTypeRecord(strHQL);
        decimal OilNum = 0;
        decimal OilMoney = 0;
        for (int i = 0; i < lst.Count; i++)
        {
            CarOilTypeRecord carOilTypeRecord = (CarOilTypeRecord)lst[i];
            OilNum += carOilTypeRecord.OilNum;
            OilMoney += carOilTypeRecord.OilMoney;
        }

        strHQL = "from CarAssignForm as carAssignForm Where carAssignForm.CarCode = '" + strCarCode + "' Order by carAssignForm.ID Desc ";
        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        IList lst1 = carAssignFormBLL.GetAllCarAssignForms(strHQL);
        if (lst1.Count > 0)
        {
            CarAssignForm carAssignForm = (CarAssignForm)lst1[0];
            carAssignForm.OilVolume = OilNum;
            carAssignForm.OilCharge = OilMoney;
            carAssignFormBLL.UpdateCarAssignForm(carAssignForm, carAssignForm.ID);   
        }
    }

    protected void LoadCarInformation(string departcode)
    {
        string strHQL;
        IList lst;

    //    strHQL = "From CarInformation as CarInformation Where CarInformation.BelongDepartCode in " + departcode + " Order By CarInformation.PurchaseTime DESC";
        strHQL = "From CarInformation as carInformation Where carInformation.Status<>'Scrapped' Order By carInformation.PurchaseTime DESC";
        CarInformationBLL carInformationBLL = new CarInformationBLL();
        lst = carInformationBLL.GetAllCarInformations(strHQL);

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadCarOilTypeRecord(string carno)
    {
        string strHQL;
        IList lst;

        strHQL = "From CarOilTypeRecord as carOilTypeRecord Where carOilTypeRecord.CarNo = '" + carno + "' Order By carOilTypeRecord.CreateTime DESC";

        CarOilTypeRecordBLL carOilTypeRecordBLL = new CarOilTypeRecordBLL();
        lst = carOilTypeRecordBLL.GetAllCarOilTypeRecord(strHQL);

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void FillInDropDownList(DropDownList ddl, bool bFirstItem)
    {
        OilTypeBLL oilTypeBLL = new OilTypeBLL();
        string strHQL = "From OilType as oilType Order By oilType.ID";
        IList lst = oilTypeBLL.GetAllOilType(strHQL);

        if (lst.Count > 0 && lst != null)
        {
            ddl.Items.Clear();
            ddl.DataSource = lst;
            ddl.DataTextField = "OilName";
            ddl.DataValueField = "ID";
            ddl.DataBind();

            if (bFirstItem)
                ddl.Items.Insert(0, new ListItem("-Select-", ""));
        }
    }

    protected string GetOilTypeName(int Id)
    {
        string strHQL;
        IList lst;
        string result = string.Empty;

        strHQL = "From OilType as oilType where oilType.ID='" + Id + "' ";
        OilTypeBLL oilTypeBLL = new OilTypeBLL();
        lst = oilTypeBLL.GetAllOilType(strHQL);

        OilType oilType = (OilType)lst[0];
        result = oilType.OilName;

        return result;
    }

    protected void btn_ExcelToDataBase_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim()+"')", true);
            return;
        }
        string IsXls = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();
        if (IsXls != ".xls" & IsXls != ".xlsx")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim()+"')", true);
            return;
        }

        CarOilTypeRecordBLL carOilTypeRecordBLL = new CarOilTypeRecordBLL();
        CarOilTypeRecord carOilTypeRecord = new CarOilTypeRecord();

        string filename = FileUpload1.FileName.ToString();  //获取Execle文件名
        string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
        string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";
        FileInfo fi = new FileInfo(strDocSavePath + newfilename);
        if (fi.Exists)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim()+"');</script>");
        }
        else
        {
            FileUpload1.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
            string strpath = strDocSavePath + newfilename;

            //DataSet ds = ExcelToDataSet(strpath, filename);
            //DataRow[] dr = ds.Tables[0].Select();
            //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
            //int rowsnum = ds.Tables[0].Rows.Count;

            DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
            DataRow[] dr = dt.Select();                        //定义一个DataRow数组
            int rowsnum = dt.Rows.Count;
            if (rowsnum == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim()+"')", true);
            }
            else
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    carOilTypeRecord.CarNo = dr[i][LanguageHandle.GetWord("ChePaiHao").ToString().Trim()].ToString();
                    carOilTypeRecord.OilTypeId = int.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("YouLiaoBianHao").ToString().Trim()].ToString()) ? "0" : dr[i][LanguageHandle.GetWord("YouLiaoBianHao").ToString().Trim()].ToString());
                    carOilTypeRecord.OilName = dr[i][LanguageHandle.GetWord("YouLiaoMingChen").ToString().Trim()].ToString();
                    carOilTypeRecord.Type = dr[i]["FuelModel"].ToString();   
                    carOilTypeRecord.DepartCode = dr[i][LanguageHandle.GetWord("GuiShuBuMenBianMa").ToString().Trim()].ToString();
                    carOilTypeRecord.OilNum = decimal.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("JiaYouShuLiang").ToString().Trim()].ToString()) ? "0" : dr[i][LanguageHandle.GetWord("JiaYouShuLiang").ToString().Trim()].ToString());
                    carOilTypeRecord.OilPrice = decimal.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("ChanJia").ToString().Trim()].ToString()) ? "0" : dr[i][LanguageHandle.GetWord("ChanJia").ToString().Trim()].ToString());
                    carOilTypeRecord.OilMoney = decimal.Parse(dr[i][LanguageHandle.GetWord("JinE").ToString().Trim()].ToString());
                    carOilTypeRecord.CreateTime = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("ChuangJianShiJian").ToString().Trim()].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("ChuangJianShiJian").ToString().Trim()].ToString());
                    try
                    {
                        carOilTypeRecordBLL.AddCarOilTypeRecord(carOilTypeRecord);
                        UpdateCarAssignFormOil(carOilTypeRecord.CarNo);
                    }
                    catch
                    {
                        int num = i + 1;
                        string Msg = LanguageHandle.GetWord("JingGaoExcelBiaoBuFenShuJuDaoR").ToString().Trim() + num + LanguageHandle.GetWord("TiaoKaiShiDaoRuShiBaiQingXianB").ToString().Trim();
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZMSG").ToString().Trim()+"')", true);
                        return;
                    }
                }
                Response.Write(LanguageHandle.GetWord("scriptalertExcleBiaoDaoRuCheng").ToString().Trim());
            }
        }
    }
}
