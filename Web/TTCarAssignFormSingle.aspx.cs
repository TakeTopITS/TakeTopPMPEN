using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTCarAssignFormSingle : System.Web.UI.Page
{
    string strUserCode;
    ArrayList hour, m;
    int i;

    string strApplyFormID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        strApplyFormID = Request.QueryString["ApplyFormID"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            DLC_BackTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_DepartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_RealBackTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_RealDepartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            InitialCalendar();

            LoadCarListByAuthority(strUserCode);

            ShareClass.LoadUnderUserByDutyAndAuthorityAsset("DRIVER", strUserCode, DL_Driver);
            ShareClass.LoadUnderUserByDutyAndAuthorityAsset("GUARD", strUserCode, DL_Guard);

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);

            BindCarApplyFormData(strApplyFormID);
            LoadCarAssignForm(strUserCode, strApplyFormID);

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

        strApplyFormID = LB_ApplyFormID.Text.Trim();
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

            LoadCarAssignForm(strUserCode, strApplyFormID);

            HL_PringAssignForm.NavigateUrl = "TTCarAssignFormPrint.aspx?ID=" + strID;

            strMessage = LanguageHandle.GetWord("ChuCheTongZhi") + LanguageHandle.GetWord("MuDeDe") + strDestination + LanguageHandle.GetWord("ShenQingRen") + strApplicantName + LanguageHandle.GetWord("SuiCheRen") + strAttendant;
            strMessage += LanguageHandle.GetWord("ChuFaShiJian") + dtDepartTime.ToString("yyyy-MM-dd HH:MM") + LanguageHandle.GetWord("FanHuiShiJian") + dtBacktime.ToString("yyyy-MM-dd HH:MM");
            strMessage += LanguageHandle.GetWord("QingZhunShiChuFa1");

            TB_Message.Text = strMessage;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch(Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJSBKNFYMCHYTZSGDJC") + "')", true);
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

        strApplyFormID = LB_ApplyFormID.Text.Trim();
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
            
            LoadCarAssignForm(strUserCode, strApplyFormID);

            strMessage = LanguageHandle.GetWord("ChuCheTongZhi") + LanguageHandle.GetWord("MuDeDe") + strDestination + LanguageHandle.GetWord("ShenQingRen") + strApplicantName + LanguageHandle.GetWord("SuiCheRen") + strAttendant;
            strMessage += LanguageHandle.GetWord("ChuFaShiJian") + dtDepartTime.ToString("yyyy-MM-dd HH:MM") + LanguageHandle.GetWord("FanHuiShiJian") + dtBacktime.ToString("yyyy-MM-dd HH:MM");
            strMessage += LanguageHandle.GetWord("QingZhunShiChuFa");

            TB_Message.Text = strMessage;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSBKNFYMCHYTZSGDJC") + "')", true);
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        string strMessage;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[3].Text.Trim();

            if (e.CommandName == "Update" || e.CommandName == "Assign")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                strHQL = "from CarAssignForm as carAssignForm Where carAssignForm.ID = " + strID;
                CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
                lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);
                CarAssignForm carAssignForm = (CarAssignForm)lst[0];

                try
                {
                    LB_ID.Text = strID;
                    LB_ApplyFormID.Text = carAssignForm.ApplyFormID.ToString();

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

                    strMessage = LanguageHandle.GetWord("ChuCheTongZhi") + LanguageHandle.GetWord("MuDeDe") + carAssignForm.Destination.Trim() + LanguageHandle.GetWord("ShenQingRen") + carAssignForm.ApplicantName.Trim() + LanguageHandle.GetWord("SuiCheRen") + carAssignForm.Attendant.Trim();
                    strMessage += LanguageHandle.GetWord("ChuFaShiJian") + carAssignForm.DepartTime.ToString("yyyy-MM-dd HH:MM") + LanguageHandle.GetWord("FanHuiShiJian") + carAssignForm.BackTime.ToString("yyyy-MM-dd HH:MM");
                    strMessage += LanguageHandle.GetWord("QingZhunShiChuFa");

                    TB_Message.Text = strMessage;
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCKSCPCDDSJHMWBZXLLBZJC") + "')", true);
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

                    LoadCarAssignForm(strUserCode, strApplyFormID);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBKNFYMCHYTZSGDJC") + "')", true);
                }
            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        IList lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void DL_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strSatus;
        string strID, strApplyFormID;

        strID = LB_ID.Text.Trim();
        strApplyFormID = LB_ApplyFormID.Text.Trim();

        strSatus = DL_Status.SelectedValue.Trim();

        if (strID != "")
        {
            strHQL = "Update T_CarAssignForm Set Status = " + "'" + strSatus + "'" + " where ID = " + strID;


            try
            {
                ShareClass.RunSqlCommand(strHQL);

                LoadCarAssignForm(strUserCode, strApplyFormID);
            }
            catch
            {
            }
        }
    }

    protected void BT_UpdateDetail_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strCarCode, strApplyFormID;
        DateTime dtRealDepartTime, dtRealBacktime;
        decimal deMileage, dePriorMileage, deRoadToll, deParkingCharge, deOilVolume, deOilCharge;

        strID = LB_ID.Text.Trim();
        strApplyFormID = LB_ApplyFormID.Text.Trim();

        strHQL = "from CarAssignForm as carAssignForm Where carAssignForm.ID = " + strID;
        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);
        CarAssignForm carAssignForm = (CarAssignForm)lst[0];


        dtRealDepartTime = DateTime.Parse(DateTime.Parse(DLC_RealDepartTime.Text).ToString("yyyy/MM/dd ") + DLC_RealDepartHour.SelectedValue + ":" + DLC_RealDepartMinute.SelectedValue);
        dtRealBacktime = DateTime.Parse(DateTime.Parse(DLC_RealBackTime.Text).ToString("yyyy/MM/dd ") + DLC_RealBackHour.SelectedValue + ":" + DLC_RealBackMinute.SelectedValue);


        deMileage = NB_Mileage.Amount;
        deRoadToll = NB_RoadToll.Amount;
        deParkingCharge = NB_ParkingCharge.Amount;

        deOilVolume = NB_OilVolume.Amount;
        deOilCharge = NB_OilCharge.Amount;

        strCarCode = carAssignForm.CarCode.Trim();
        dePriorMileage = GetPriorCarAssignFromMileage(strID, strCarCode);


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

        

            LoadCarAssignForm(strUserCode, strApplyFormID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
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

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strSubject, strMsg;
        string strOperatorCode, strUserCode;

        strUserCode = Session["UserCode"].ToString();

        strOperatorCode = DL_Driver.SelectedValue.Trim();

        Msg msg = new Msg();

        if (CB_SendMsg.Checked == true | CB_SendMail.Checked == true)
        {
            strSubject = LanguageHandle.GetWord("ChuCheTongZhi");
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

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSWB") + "')", true);
    }

    protected void LoadCarListByAuthority(string strUserCode)
    {
        string strHQL;
        string strDepartCode, strDepartString;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strHQL = " Select CarCode From T_CarInformation Where ";
        strHQL += " BelongDepartCode in " + strDepartString;
        strHQL += " Order By PurchaseTime DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CarInformation");

        DL_CarCode.DataSource = ds;
        DL_CarCode.DataBind();
    }

    protected void LoadCarAssignForm(string strUserCode, string strApplyFormID)
    {
        string strHQL;
        IList lst;

        strHQL = "From CarAssignForm as carAssignForm Where carAssignForm.MakeUserCode = " + "'" + strUserCode + "'" + " and carAssignForm.ApplyFormID = " + strApplyFormID + " Order By carAssignForm.ID DESC";
        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
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

    protected int GetRelatedWorkFlowNumber(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        return lst.Count;
    }

    protected void BindCarApplyFormData(string strApplyFormID)
    {
        string strHQL;
        IList lst;


        strHQL = "from CarApplyForm as carApplyForm where carApplyForm.ID = " + strApplyFormID;
        CarApplyFormBLL carApplyFormBLL = new CarApplyFormBLL();
        lst = carApplyFormBLL.GetAllCarApplyForms(strHQL);
        CarApplyForm carApplyForm = (CarApplyForm)lst[0];


        LB_ApplyFormID.Text = strApplyFormID;
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
}
