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

public partial class TTCarAssignForm : System.Web.UI.Page
{
    string strUserCode;
    ArrayList hour, m;
    int i;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","派车单", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {           
            DLC_BackTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_DepartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_RealDepartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_RealBackTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            InitialCalendar();

            LoadCarListByAuthority(strUserCode);

            ShareClass.LoadUnderUserByDutyAndAuthorityAsset("DRIVER", strUserCode, DL_Driver);
            ShareClass.LoadUnderUserByDutyAndAuthorityAsset("GUARD", strUserCode, DL_Guard);

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView2, strUserCode);

            LoadCarApplyFormByAuthority(strUserCode);
            LoadCarAssignForm(strUserCode);

            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = GetUserName(strUserCode);
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

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid2);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strApplyFormID;

        if (e.CommandName != "Page")
        {
            strApplyFormID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            NB_ApplyFormID.Amount = int.Parse(strApplyFormID);

            strHQL = "from CarApplyForm as carApplyForm where carApplyForm.ID = " + strApplyFormID;
            CarApplyFormBLL carApplyFormBLL = new CarApplyFormBLL();
            lst = carApplyFormBLL.GetAllCarApplyForms(strHQL);
            CarApplyForm carApplyForm = (CarApplyForm)lst[0];


            NB_ApplyFormID.Amount = decimal.Parse(strApplyFormID);
            LB_ApplicantCode.Text = carApplyForm.ApplicantCode.Trim();
            LB_ApplicantName.Text = carApplyForm.ApplicantName.Trim();
            TB_DepartCode.Text = carApplyForm.DepartCode;
            LB_DepartName.Text = ShareClass.GetDepartName(carApplyForm.DepartCode.Trim());

            DLC_DepartTime.Text = carApplyForm.DepartTime.ToString("yyyy-MM-dd");
            DLC_DepartBeginHour.SelectedValue = carApplyForm.DepartTime.Hour.ToString();
            DLC_DepartBeginMinute.SelectedValue = carApplyForm.DepartTime.Minute.ToString();

            DLC_BackTime.Text = carApplyForm.BackTime.ToString("yyyy-MM-dd");
            DLC_BackBeginHour.SelectedValue = carApplyForm.BackTime.Hour.ToString();
            DLC_BackBeginMinute.SelectedValue = carApplyForm.BackTime.Minute.ToString();

            TB_Attendant.Text = carApplyForm.Attendant.Trim();
            TB_BoardingSite.Text = carApplyForm.BoardingSite.Trim();
            TB_Destination.Text = carApplyForm.Destination.Trim();

            TB_Comment.Text = carApplyForm.ApplyReason.Trim();

            LoadRelatedWL("VehicleRequest", "Other", int.Parse(strApplyFormID));

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text;

        LB_ApplicantCode.Text = strUserCode;
        LB_ApplicantName.Text = strUserName;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddCarAssign();
        }
        else
        {
            UpdateCarAssign();
        }
    }

    protected void AddCarAssign()
    {
        string strID, strCarCode, strApplyFormID, strDepartCode, strDepartName, strApplicantCode, strApplicantName, strMakeUserCode, strDriverCode, strGuardCode;
        string strBoardingSite, strDestination, strAttendant, strComment, strStatus;
        DateTime dtDepartTime, dtBacktime;

        string strMessage;
        strApplyFormID = NB_ApplyFormID.Amount.ToString();
        strCarCode = DL_CarCode.SelectedValue;
        strDepartCode = TB_DepartCode.Text.Trim();
        strDepartName = ShareClass.GetDepartName(strDepartCode);
        strApplicantCode = LB_ApplicantCode.Text.Trim();
        strApplicantName = LB_ApplicantName.Text.Trim();

        strDriverCode = DL_Driver.SelectedValue.Trim();
        strGuardCode = DL_Guard.SelectedValue.Trim();

        strBoardingSite = TB_BoardingSite.Text.Trim();
        strDestination = TB_Destination.Text.Trim();
        strAttendant = TB_Attendant.Text.Trim();
        strComment = TB_Comment.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();
        strMakeUserCode = strUserCode;

        dtDepartTime = DateTime.Parse(DateTime.Parse(DLC_DepartTime.Text).ToString("yyyy/MM/dd ") + DLC_DepartBeginHour.SelectedValue + ":" + DLC_DepartBeginMinute.SelectedValue);
        dtBacktime = DateTime.Parse(DateTime.Parse(DLC_BackTime.Text).ToString("yyyy/MM/dd ") + DLC_BackBeginHour.SelectedValue + ":" + DLC_BackBeginMinute.SelectedValue);

        if (dtBacktime < dtDepartTime)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJHHCSJBNXYJHYCSJJC").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        if(IsCarAssignForm(strCarCode,dtDepartTime.ToString(),dtBacktime.ToString()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGCZTYDSJNYBPCYLCBNTSPCJC").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        if (!string.IsNullOrEmpty(NB_ApplyFormID.Amount.ToString().Trim()) && !NB_ApplyFormID.Amount.ToString().Trim().Equals("0"))
        {
            if (IsCarAssignFormByApplyFormID(NB_ApplyFormID.Amount.ToString().Trim(), string.Empty))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGSDYPCWXZCPCJC").ToString().Trim()+"')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                return;
            }
        }

        try
        {
            CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
            CarAssignForm carAssignForm = new CarAssignForm();

            carAssignForm.ApplyFormID = int.Parse(strApplyFormID);
            carAssignForm.CarCode = strCarCode;
            carAssignForm.DepartCode = strDepartCode;
            carAssignForm.DepartName = strDepartName;
            carAssignForm.ApplicantCode = strApplicantCode;
            carAssignForm.ApplicantName = strApplicantName;

            carAssignForm.DriverCode = strDriverCode;
            carAssignForm.DriverName = ShareClass.GetUserName(strDriverCode);

            carAssignForm.GuardCode = strGuardCode;
            carAssignForm.GuardName = ShareClass.GetUserName(strGuardCode);

            carAssignForm.Mileage = 0;
            carAssignForm.RoadToll = 0;
            carAssignForm.ParkingCharge = 0;

            carAssignForm.OilVolume = 0;
            carAssignForm.OilCharge = 0;

            carAssignForm.Attendant = strAttendant;
            carAssignForm.BoardingSite = strBoardingSite;
            carAssignForm.Destination = strDestination;
            carAssignForm.Comment = strComment;

            carAssignForm.DepartTime = dtDepartTime;
            carAssignForm.BackTime = dtBacktime;

            carAssignForm.RealDepartTime = dtDepartTime;
            carAssignForm.RealBackTime = dtBacktime;

            carAssignForm.Status = strStatus;
            carAssignForm.MakeUserCode = strMakeUserCode;

            carAssignFormBLL.AddCarAssignForm(carAssignForm);

            strID = ShareClass.GetMyCreatedMaxCarAssignFormID();

            LB_ID.Text = strID;


            BT_CarAssignFormDetail.Enabled = true;

            LoadCarAssignForm(strUserCode);

            HL_PringAssignForm.NavigateUrl = "TTCarAssignFormPrint.aspx?ID=" + strID;

            strMessage = LanguageHandle.GetWord("ChuCheTongZhi").ToString().Trim() + LanguageHandle.GetWord("MuDeDe").ToString().Trim() + strDestination + LanguageHandle.GetWord("ShenQingRen").ToString().Trim() + strApplicantName + LanguageHandle.GetWord("SuiCheRen").ToString().Trim() + strAttendant;
            strMessage += LanguageHandle.GetWord("ChuFaShiJian").ToString().Trim() + dtDepartTime.ToString("yyyy-MM-dd HH:MM") + LanguageHandle.GetWord("FanHuiShiJian").ToString().Trim() + dtBacktime.ToString("yyyy-MM-dd HH:MM");
            strMessage += LanguageHandle.GetWord("QingZhunShiChuFa1").ToString().Trim();

            TB_Message.Text = strMessage;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateCarAssign()
    {
        string strHQL;
        IList lst;

        string strID, strApplyFormID, strCarCode, strDepartCode, strDepartName, strApplicantCode, strApplicantName, strMakeUserCode, strDriverCode, strGuardCode;
        string strBoardingSite, strDestination, strAttendant, strComment, strStatus;
        DateTime dtDepartTime, dtBacktime;
      
        string strMessage;

        strID = LB_ID.Text.Trim();

        strHQL = "From CarAssignForm as carAssignForm Where carAssignForm.ID = " + strID;
        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);
        CarAssignForm carAssignForm = (CarAssignForm)lst[0];

        strApplyFormID = NB_ApplyFormID.Amount.ToString();
        strCarCode = DL_CarCode.SelectedValue;
        strApplyFormID = NB_ApplyFormID.Amount.ToString();
        strDepartCode = TB_DepartCode.Text.Trim();
        strDepartName = ShareClass.GetDepartName(strDepartCode);
        strApplicantCode = LB_ApplicantCode.Text.Trim();
        strApplicantName = LB_ApplicantName.Text.Trim();

        strDriverCode = DL_Driver.SelectedValue.Trim();
        strGuardCode = DL_Guard.SelectedValue.Trim();

        strBoardingSite = TB_BoardingSite.Text.Trim();
        strDestination = TB_Destination.Text.Trim();
        strAttendant = TB_Attendant.Text.Trim();
        strComment = TB_Comment.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();
        strMakeUserCode = strUserCode;

        dtDepartTime = DateTime.Parse(DateTime .Parse ( DLC_DepartTime.Text ).ToString("yyyy/MM/dd ") + DLC_DepartBeginHour.SelectedValue + ":" + DLC_DepartBeginMinute.SelectedValue);
        dtBacktime = DateTime.Parse(DateTime .Parse ( DLC_BackTime.Text ).ToString("yyyy/MM/dd ") + DLC_BackBeginHour.SelectedValue + ":" + DLC_BackBeginMinute.SelectedValue);

        if (dtBacktime < dtDepartTime)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJHHCSJBNXYJHYCSJJC").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        if (!carAssignForm.CarCode.Trim().Equals(strCarCode.Trim()))
        {
            if (IsCarAssignForm(strCarCode, dtDepartTime.ToString(), dtBacktime.ToString()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGCZTYDSJNYBPCYLCBNTSPCJC").ToString().Trim()+"')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                return;
            }
        }
        else
        {
            if (IsCarAssignForm2(strCarCode, dtDepartTime.ToString(), dtBacktime.ToString(), int.Parse(strID)))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGCZTYDSJNYBPCYLCBNTSPCJC").ToString().Trim()+"')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                return;
            }
        }
        if (!string.IsNullOrEmpty(NB_ApplyFormID.Amount.ToString().Trim()) && !NB_ApplyFormID.Amount.ToString().Trim().Equals("0"))
        {
            if (IsCarAssignFormByApplyFormID(NB_ApplyFormID.Amount.ToString().Trim(), LB_ID.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGSDYPCWXZCPCJC").ToString().Trim()+"')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                return;
            }
        }

        try
        {
            carAssignForm.ApplyFormID = int.Parse(strApplyFormID);
            carAssignForm.CarCode = strCarCode;
            carAssignForm.DepartCode = strDepartCode;
            carAssignForm.DepartName = strDepartName;
            carAssignForm.ApplicantCode = strApplicantCode;
            carAssignForm.ApplicantName = strApplicantName;

            carAssignForm.DriverCode = strDriverCode;
            carAssignForm.DriverName = ShareClass.GetUserName(strDriverCode);

            carAssignForm.GuardCode = strGuardCode;
            carAssignForm.GuardName = ShareClass.GetUserName(strGuardCode);

            carAssignForm.Attendant = strAttendant;
            carAssignForm.BoardingSite = strBoardingSite;
            carAssignForm.Destination = strDestination;
            carAssignForm.Comment = strComment;

            carAssignForm.DepartTime = dtDepartTime;
            carAssignForm.BackTime = dtBacktime;

            carAssignForm.Status = strStatus;
            carAssignForm.MakeUserCode = strMakeUserCode;

            carAssignFormBLL.UpdateCarAssignForm(carAssignForm, int.Parse(strID));          

            LoadCarAssignForm(strUserCode);

            strMessage = LanguageHandle.GetWord("ChuCheTongZhi").ToString().Trim() + LanguageHandle.GetWord("MuDeDe").ToString().Trim() + strDestination + LanguageHandle.GetWord("ShenQingRen").ToString().Trim() + strApplicantName + LanguageHandle.GetWord("SuiCheRen").ToString().Trim() + strAttendant;
            strMessage += LanguageHandle.GetWord("ChuFaShiJian").ToString().Trim() + dtDepartTime.ToString("yyyy-MM-dd HH:MM") + LanguageHandle.GetWord("FanHuiShiJian").ToString().Trim() + dtBacktime.ToString("yyyy-MM-dd HH:MM");
            strMessage += LanguageHandle.GetWord("QingZhunShiChuFa").ToString().Trim();

            TB_Message.Text = strMessage;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        string strMessage;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[3].Text.Trim();

            if (e.CommandName == "Update" || e.CommandName == "Assign")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                strHQL = "from CarAssignForm as carAssignForm Where carAssignForm.ID = " + strID;
                CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
                lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);
                CarAssignForm carAssignForm = (CarAssignForm)lst[0];


                try
                {
                    LB_ID.Text = strID;

                    NB_ApplyFormID.Amount = carAssignForm.ApplyFormID;

                    TB_DepartCode.Text = carAssignForm.DepartCode.Trim();
                    LB_DepartName.Text = ShareClass.GetDepartName(carAssignForm.DepartCode.Trim());

                    LB_ApplicantCode.Text = carAssignForm.ApplicantCode.Trim();
                    LB_ApplicantName.Text = carAssignForm.ApplicantName.Trim();

                    DL_Driver.SelectedValue = carAssignForm.DriverCode;
                    DL_Guard.SelectedValue = carAssignForm.GuardCode;

                    NB_Mileage.Amount = carAssignForm.Mileage;
                    NB_RoadToll.Amount = carAssignForm.RoadToll;
                    NB_ParkingCharge.Amount = carAssignForm.ParkingCharge;

                    NB_OilVolume.Amount = carAssignForm.OilVolume;
                    NB_OilCharge.Amount = carAssignForm.OilCharge;

                    DLC_DepartTime.Text = carAssignForm.DepartTime.ToString("yyyy-MM-dd");
                    DLC_DepartBeginHour.SelectedValue = carAssignForm.DepartTime.Hour.ToString();
                    DLC_DepartBeginMinute.SelectedValue = carAssignForm.DepartTime.Minute.ToString();

                    DLC_BackTime.Text = carAssignForm.BackTime.ToString("yyyy-MM-dd");
                    DLC_BackBeginHour.SelectedValue = carAssignForm.BackTime.Hour.ToString();
                    DLC_BackBeginMinute.SelectedValue = carAssignForm.BackTime.Minute.ToString();

                    DLC_RealDepartTime.Text = carAssignForm.RealDepartTime.ToString("yyyy-MM-dd");
                    DLC_RealDepartHour.SelectedValue = carAssignForm.RealDepartTime.Hour.ToString();
                    DLC_RealDepartMinute.SelectedValue = carAssignForm.RealDepartTime.Minute.ToString();

                    DLC_RealBackTime.Text = carAssignForm.RealBackTime.ToString("yyyy-MM-dd");
                    DLC_RealBackHour.SelectedValue = carAssignForm.RealBackTime.Hour.ToString();
                    DLC_RealBackMinute.SelectedValue = carAssignForm.RealBackTime.Minute.ToString();

                    TB_BoardingSite.Text = carAssignForm.BoardingSite.Trim();
                    TB_Destination.Text = carAssignForm.Destination.Trim();
                    TB_Attendant.Text = carAssignForm.Attendant.Trim();
                    TB_Comment.Text = carAssignForm.Comment.Trim();
                    DL_Status.SelectedValue = carAssignForm.Status.Trim();
                    

                    BT_CarAssignFormDetail.Enabled = true;

                    LoadRelatedWL("VehicleRequest", "Other", carAssignForm.ApplyFormID);

                    HL_PringAssignForm.NavigateUrl = "TTCarAssignFormPrint.aspx?ID=" + strID;

                    strMessage = LanguageHandle.GetWord("ChuCheTongZhi").ToString().Trim() + LanguageHandle.GetWord("MuDeDe").ToString().Trim() + carAssignForm.Destination.Trim() + LanguageHandle.GetWord("ShenQingRen").ToString().Trim() + carAssignForm.ApplicantName.Trim() + LanguageHandle.GetWord("SuiCheRen").ToString().Trim() + carAssignForm.Attendant.Trim();
                    strMessage += LanguageHandle.GetWord("ChuFaShiJian").ToString().Trim() + carAssignForm.DepartTime.ToString("yyyy-MM-dd HH:MM") + LanguageHandle.GetWord("FanHuiShiJian").ToString().Trim() + carAssignForm.BackTime.ToString("yyyy-MM-dd HH:MM");
                    strMessage += LanguageHandle.GetWord("QingZhunShiChuFa").ToString().Trim();

                    TB_Message.Text = strMessage;
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCKSCPCDDSJHMWBZXLLBZJC").ToString().Trim() + "')", true);
                }

                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }

                if (e.CommandName == "Assign")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popWFApplyWindow','true') ", true);
                }
            }

            if (e.CommandName == "Delete")
            {
                try
                {
                    CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
                    CarAssignForm carAssignForm = new CarAssignForm();

                    carAssignForm.ID = int.Parse(strID);

                    carAssignFormBLL.DeleteCarAssignForm(carAssignForm);
           

                    BT_CarAssignFormDetail.Enabled = false;

                    LoadCarAssignForm(strUserCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBKNFYMCHYTZSGDJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        IList lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void DL_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strSatus;
        string strID;

        strID = LB_ID.Text.Trim();
        strSatus = DL_Status.SelectedValue.Trim();

        if (strID != "")
        {
            strHQL = "Update T_CarAssignForm Set Status = " + "'" + strSatus + "'" + " where ID = " + strID;

            try
            {
                ShareClass.RunSqlCommand(strHQL);

                LoadCarAssignForm(strUserCode);
            }
            catch
            {
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_UpdateDetail_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strCarCode;
        DateTime dtRealDepartTime, dtRealBacktime;
        decimal deMileage, dePriorMileage, deRoadToll, deParkingCharge, deOilVolume, deOilCharge;

        strID = LB_ID.Text.Trim();

        strHQL = "from CarAssignForm as carAssignForm Where carAssignForm.ID = " + strID;
        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);
        CarAssignForm carAssignForm = (CarAssignForm)lst[0];

        dtRealDepartTime = DateTime.Parse(DateTime .Parse ( DLC_RealDepartTime.Text ).ToString("yyyy/MM/dd ") + DLC_RealDepartHour.SelectedValue + ":" + DLC_RealDepartMinute.SelectedValue);
        dtRealBacktime = DateTime.Parse(DateTime .Parse ( DLC_RealBackTime.Text ).ToString("yyyy/MM/dd ") + DLC_RealBackHour.SelectedValue + ":" + DLC_RealBackMinute.SelectedValue);

        deMileage = NB_Mileage.Amount;
        deRoadToll = NB_RoadToll.Amount;
        deParkingCharge = NB_ParkingCharge.Amount;

        deOilVolume = NB_OilVolume.Amount;
        deOilCharge = NB_OilCharge.Amount;

        strCarCode = carAssignForm.CarCode.Trim();
        dePriorMileage = GetPriorCarAssignFromMileage(strID, strCarCode);

        if (dtRealBacktime < dtRealDepartTime)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSJHCSJBNXYSJYCSJJC").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        if (IsCarAssignForm2(strCarCode, dtRealDepartTime.ToString(), dtRealBacktime.ToString(), int.Parse(strID)))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGCZTYDSJNYBPCYLCBNTSPCJC").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        try
        {
            carAssignForm.RealDepartTime = dtRealDepartTime;
            carAssignForm.RealBackTime = dtRealBacktime;

            carAssignForm.Mileage = deMileage;
            carAssignForm.RoadToll = deRoadToll;
            carAssignForm.ParkingCharge = deParkingCharge;

            carAssignForm.OilVolume = deOilVolume;
            carAssignForm.OilCharge = deOilCharge;

            carAssignForm.CurrentMileage = deMileage - dePriorMileage;

            carAssignFormBLL.UpdateCarAssignForm(carAssignForm, int.Parse(strID));

            LoadCarAssignForm(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strSubject, strMsg;
        string strOperatorCode, strUserCode;

        strUserCode = LB_UserCode.Text.Trim();

        strOperatorCode = DL_Driver.SelectedValue.Trim();

        Msg msg = new Msg();

        if (CB_SendMsg.Checked == true | CB_SendMail.Checked == true)
        {
            strSubject = LanguageHandle.GetWord("ChuCheTongZhi").ToString().Trim();
            strMsg = TB_Message.Text.Trim();

            if (CB_SendMsg.Checked == true)
            {
                msg.SendMSM("Message",strOperatorCode, strMsg, strUserCode);
            }

            if (CB_SendMail.Checked == true)
            {
                msg.SendMail(strOperatorCode, strSubject, strMsg, strUserCode);
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZFSWB").ToString().Trim()+"')", true);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void LoadCarAssignForm(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From CarAssignForm as carAssignForm Where carAssignForm.MakeUserCode = " + "'" + strUserCode + "'" + " Order By carAssignForm.DepartTime DESC";
        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected string GetWorkFlowStatus(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        WorkFlow workFlow = (WorkFlow)lst[0];

        return workFlow.Status.Trim();
    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName;

        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }

    protected int GetRelatedWorkFlowNumber(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        return lst.Count;
    }

    protected void LoadCarApplyFormByAuthority(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartString;

        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strHQL = "From CarApplyForm as carApplyForm";
        strHQL += " Where carApplyForm.ApplicantCode in ( Select projectMember.UserCode From ProjectMember as projectMember Where DepartCode in " + strDepartString + ")";
    //    strHQL += " And carApplyForm.Status<>'Cancel' And carApplyForm.ID not in (Select carAssignForm.ApplyFormID From CarAssignForm as carAssignForm Where carAssignForm.Status<>'Cancel') Order By carApplyForm.ID DESC";//取消的申请单，无法再派车 Liujp 2013-07-12  //已派车的申请单(非取消状态)，无法再派车 Liujp 2013-07-16
        strHQL += " Order By carApplyForm.ID DESC";//取消的申请单，无法再派车 Liujp 2013-07-12  //已派车的申请单(非取消状态)，无法再派车 Liujp 2013-07-16
        CarApplyFormBLL carApplyFormBLL = new CarApplyFormBLL();
        lst = carApplyFormBLL.GetAllCarApplyForms(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    protected bool IsCarAssignFormByApplyFormID(string strApplyFormId, string strId)
    {
        string strHQL;
        if (string.IsNullOrEmpty(strId))
        {
            strHQL = "from CarAssignForm as carAssignForm where carAssignForm.ApplyFormID = '" + strApplyFormId + "' and carAssignForm.Status<>'Cancel' ";
        }
        else
            strHQL = "from CarAssignForm as carAssignForm where carAssignForm.ApplyFormID = '" + strApplyFormId + "' and carAssignForm.ID<>'" + strId + "' and carAssignForm.Status<>'Cancel' ";
        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        IList lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            return true;
        }
        else
            return false;
    }

    protected void LoadCarListByAuthority(string strUserCode)
    {
        string strHQL;
        string strDepartCode, strDepartString;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);
        //增加筛选条件：报废，维修的车辆不能派；已出车的车辆不能派
        strHQL = " Select CarCode From T_CarInformation Where ";
        strHQL += " BelongDepartCode in " + strDepartString;
        strHQL += " and Status='InUse' ";
  //      strHQL += " and Status='InUse' and CarCode not in (Select CarCode From T_CarAssignForm Where Status='Departure') ";
        strHQL += " Order By PurchaseTime DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CarInformation");

        DL_CarCode.DataSource = ds;
        DL_CarCode.DataBind();

        DL_CarCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    /// <summary>
    /// 判断该车在同一段时间是否已派出，派出则返回true；否则返回false
    /// </summary>
    /// <param name="strCarCode"></param>
    /// <param name="starttime"></param>
    /// <param name="endtime"></param>
    /// <returns></returns>
    protected bool IsCarAssignForm(string strCarCode,string starttime, string endtime)
    {
        bool flag = false;
        string strHQL = "Select CarCode From T_CarAssignForm Where Status='New' and CarCode='" + strCarCode + "' and extract(epoch FROM ('" + starttime + "'::timestamp-RealDepartTime))/60<=0 and extract(epoch FROM ('" + endtime + "'::timestamp-RealBackTime))/60>=0 ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CarAssignForm");
        if (ds.Tables[0].Rows.Count > 0)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    /// <summary>
    /// 判断在同一段时间内，该车是否已派出，派出则返回true；否则返回false
    /// </summary>
    /// <param name="strCarCode"></param>
    /// <param name="starttime"></param>
    /// <param name="endtime"></param>
    /// <returns></returns>
    protected bool IsCarAssignForm2(string strCarCode, string starttime, string endtime, int Id)
    {
        bool flag = false;
        string strHQL = "Select CarCode From T_CarAssignForm Where Status='New' and ID<>'" + Id + "' and CarCode='" + strCarCode + "' and extract(epoch FROM ('" + starttime + "'::timestamp-RealBackTime))/60<=0 and extract(epoch FROM ('" + endtime + "'::timestamp-RealDepartTime))/60>=0 ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CarAssignForm");
        if (ds.Tables[0].Rows.Count > 0)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    protected void BindCarApplyFormData(string strApplyFormID)
    {
        string strHQL;
        IList lst;


        strHQL = "from CarApplyForm as carApplyForm where carApplyForm.ID = " + strApplyFormID;
        CarApplyFormBLL carApplyFormBLL = new CarApplyFormBLL();
        lst = carApplyFormBLL.GetAllCarApplyForms(strHQL);
        CarApplyForm carApplyForm = (CarApplyForm)lst[0];


        NB_ApplyFormID.Amount = decimal.Parse(strApplyFormID);
        LB_UserCode.Text = carApplyForm.ApplicantCode.Trim();
        LB_UserName.Text = carApplyForm.ApplicantName.Trim();
        TB_DepartCode.Text = carApplyForm.DepartCode;
        LB_DepartName.Text = ShareClass.GetDepartName(carApplyForm.DepartCode.Trim());
        DLC_DepartTime.Text = carApplyForm.DepartTime.ToString("yyyy-MM-dd");
        DLC_DepartBeginHour.SelectedValue = carApplyForm.DepartTime.Hour.ToString();
        DLC_DepartBeginMinute.SelectedValue = carApplyForm.DepartTime.Minute.ToString();
        DLC_BackBeginHour.SelectedValue = carApplyForm.BackTime.Hour.ToString();
        DLC_BackBeginMinute.SelectedValue = carApplyForm.BackTime.Minute.ToString();

        TB_Attendant.Text = carApplyForm.Attendant.Trim();
        TB_BoardingSite.Text = carApplyForm.BoardingSite.Trim();
        TB_Destination.Text = carApplyForm.Destination.Trim();

        TB_Comment.Text = carApplyForm.ApplyReason.Trim();

        LoadRelatedWL("VehicleRequest", "Other", int.Parse(strApplyFormID));
    }

    protected void InitialCalendar()
    {
        hour = new ArrayList();
        m = new ArrayList();


        for (i = 0; i <= 23; i++)
            hour.Add(i.ToString());
        for (i = 00; i <= 59; i++)
            m.Add(i.ToString());

        DLC_DepartBeginHour.DataSource = hour;
        DLC_DepartBeginHour.DataBind();
        DLC_DepartBeginHour.Text = System.DateTime.Now.Hour.ToString();

        DLC_DepartBeginMinute.DataSource = m;
        DLC_DepartBeginMinute.DataBind();
        DLC_DepartBeginMinute.Text = System.DateTime.Now.Minute.ToString();


        DLC_BackBeginHour.DataSource = hour;
        DLC_BackBeginHour.DataBind();
        DLC_BackBeginHour.Text = System.DateTime.Now.Hour.ToString();

        DLC_BackBeginMinute.DataSource = m;
        DLC_BackBeginMinute.DataBind();
        DLC_BackBeginMinute.Text = System.DateTime.Now.Minute.ToString();


        DLC_RealDepartHour.DataSource = hour;
        DLC_RealDepartHour.DataBind();
        DLC_RealDepartHour.Text = System.DateTime.Now.Hour.ToString();

        DLC_RealDepartMinute.DataSource = m;
        DLC_RealDepartMinute.DataBind();
        DLC_RealDepartMinute.Text = System.DateTime.Now.Minute.ToString();


        DLC_RealBackHour.DataSource = hour;
        DLC_RealBackHour.DataBind();
        DLC_RealBackHour.Text = System.DateTime.Now.Hour.ToString();

        DLC_RealBackMinute.DataSource = m;
        DLC_RealBackMinute.DataBind();
        DLC_RealBackMinute.Text = System.DateTime.Now.Minute.ToString();
    }
}
