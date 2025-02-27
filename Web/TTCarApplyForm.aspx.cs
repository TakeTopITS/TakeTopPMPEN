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

public partial class TTCarApplyForm : System.Web.UI.Page
{
    string strUserCode;

    ArrayList hour, m;
    int i;

    string strRelatedType, strRelatedID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        strUserCode = Session["UserCode"].ToString();

        if (ShareClass.IsMobileDeviceCheckAgent() & Session["IsMobileDevice"].ToString() == "YES")
        {
            Response.Redirect("TTAPPCarApplyForm.aspx");
        }

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "VehicleRequest", strUserCode);
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

            LoadCarType();

            InitialCalendar();

            LoadCarApplyForm(strUserCode);

            ShareClass.LoadWFTemplate(strUserCode, "VehicleRequest", DL_TemName);

            BT_SubmitApply.Enabled = false;

            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = ShareClass.GetUserName(strUserCode);

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
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
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddCarAppply();
        }
        else
        {
            UpdateCarAppply();
        }
    }

    protected void AddCarAppply()
    {
        string strID, strDepartCode, strDepartName, strApplicantCode, strApplicantName, strMakeUserCode;
        string strCarType, strBoardingSite, strDestination, strAttendant, strApplyReason, strStatus;
        DateTime dtDepartTime, dtBacktime;

        strDepartCode = TB_DepartCode.Text.Trim();
        strDepartName = ShareClass.GetDepartName(strDepartCode);
        strApplicantCode = LB_UserCode.Text.Trim();
        strApplicantName = ShareClass.GetUserName(strApplicantCode);
        strCarType = DL_CarType.SelectedValue.Trim();
        strBoardingSite = TB_BoardingSite.Text.Trim();
        strDestination = TB_Destination.Text.Trim();
        strAttendant = TB_Attendant.Text.Trim();
        strApplyReason = TB_ApplyReason.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();
        strMakeUserCode = strUserCode;

        dtDepartTime = DateTime.Parse(DateTime.Parse(DLC_DepartTime.Text).ToString("yyyy/MM/dd ") + DL_DepartBeginHour.SelectedValue + ":" + DL_DepartBeginMinute.SelectedValue);
        dtBacktime = DateTime.Parse(DateTime.Parse(DLC_BackTime.Text).ToString("yyyy/MM/dd ") + DL_BackBeginHour.SelectedValue + ":" + DL_BackBeginMinute.SelectedValue);

        if (dtBacktime < dtDepartTime)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHCSJBNXYYCSJJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        try
        {
            CarApplyFormBLL carApplyFormBLL = new CarApplyFormBLL();
            CarApplyForm carApplyForm = new CarApplyForm();

            carApplyForm.DepartCode = strDepartCode;
            carApplyForm.DepartName = strDepartName;
            carApplyForm.ApplicantCode = strApplicantCode;
            carApplyForm.ApplicantName = strApplicantName;
            carApplyForm.CarType = strCarType;
            carApplyForm.Attendant = strAttendant;
            carApplyForm.BoardingSite = strBoardingSite;
            carApplyForm.Destination = strDestination;
            carApplyForm.ApplyReason = strApplyReason;
            carApplyForm.DepartTime = dtDepartTime;
            carApplyForm.BackTime = dtBacktime;
            carApplyForm.Status = strStatus;
            carApplyForm.MakeUserCode = strMakeUserCode;

            carApplyFormBLL.AddCarApplyForm(carApplyForm);

            strID = ShareClass.GetMyCreatedMaxCarApplyFormID();

            LB_ID.Text = strID;

            BT_SubmitApply.Enabled = true;

            LoadCarApplyForm(strUserCode);

            LoadRelatedWL("CarApply" + strID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateCarAppply()
    {
        string strID, strDepartCode, strApplicantCode, strApplicantName, strMakeUserCode;
        string strCarType, strBoardingSite, strDestination, strAttendant, strApplyReason, strStatus;
        DateTime dtDepartTime, dtBacktime;

        strID = LB_ID.Text.Trim();
        strDepartCode = TB_DepartCode.Text.Trim();
        strApplicantCode = strUserCode;
        strApplicantName = ShareClass.GetUserName(strApplicantCode);
        strCarType = DL_CarType.SelectedValue.Trim();
        strBoardingSite = TB_BoardingSite.Text.Trim();
        strDestination = TB_Destination.Text.Trim();
        strAttendant = TB_Attendant.Text.Trim();
        strApplyReason = TB_ApplyReason.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();
        strMakeUserCode = strUserCode;

        dtDepartTime = DateTime.Parse(DateTime.Parse(DLC_DepartTime.Text).ToString("yyyy/MM/dd ") + DL_DepartBeginHour.SelectedValue + ":" + DL_DepartBeginMinute.SelectedValue);
        dtBacktime = DateTime.Parse(DateTime.Parse(DLC_BackTime.Text).ToString("yyyy/MM/dd ") + DL_BackBeginHour.SelectedValue + ":" + DL_BackBeginMinute.SelectedValue);

        if (dtBacktime < dtDepartTime)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHCSJBNXYYCSJJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        try
        {
            CarApplyFormBLL carApplyFormBLL = new CarApplyFormBLL();
            CarApplyForm carApplyForm = new CarApplyForm();

            carApplyForm.ID = int.Parse(strID);
            carApplyForm.DepartCode = strDepartCode;
            carApplyForm.DepartName = ShareClass.GetDepartName(strDepartCode);
            carApplyForm.ApplicantCode = strApplicantCode;
            carApplyForm.ApplicantName = strApplicantName;
            carApplyForm.CarType = strCarType;
            carApplyForm.Attendant = strAttendant;
            carApplyForm.BoardingSite = strBoardingSite;
            carApplyForm.Destination = strDestination;
            carApplyForm.ApplyReason = strApplyReason;
            carApplyForm.DepartTime = dtDepartTime;
            carApplyForm.BackTime = dtBacktime;
            carApplyForm.Status = strStatus;
            carApplyForm.MakeUserCode = strMakeUserCode;

            carApplyFormBLL.UpdateCarApplyForm(carApplyForm, int.Parse(strID));

            LoadCarApplyForm(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected string SubmitApply()
    {
        string strHQL, strCmdText;
        string strID, strWLID, strXMLFileName, strXMLFile2;
        string strTemName;


        strID = LB_ID.Text.Trim();
        strWLID = "0";

        strTemName = DL_TemName.SelectedValue.Trim();
        if (strTemName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWLCMBBNWKJC").ToString().Trim() + "')", true);
            return strWLID;
        }

        XMLProcess xmlProcess = new XMLProcess();

        strHQL = "Update T_CarApplyForm Set Status = 'InProgress' Where ID = " + strID;


        try
        {
            ShareClass.RunSqlCommand(strHQL);

            strXMLFileName = "VehicleRequest" + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
            strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;

            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            WorkFlow workFlow = new WorkFlow();

            workFlow.WLName = "VehicleRequest";
            workFlow.WLType = "VehicleRequest";
            workFlow.Status = "New";
            workFlow.TemName = DL_TemName.SelectedValue.Trim();
            workFlow.CreateTime = DateTime.Now;
            workFlow.CreatorCode = strUserCode;
            workFlow.CreatorName = ShareClass.GetUserName(strUserCode);
            workFlow.Description = TB_ApplyReason.Text.Trim();
            workFlow.XMLFile = strXMLFile2;

            if (strRelatedType != "" & strRelatedType != null)
            {
                workFlow.RelatedType = strRelatedType;
                workFlow.RelatedID = int.Parse(strRelatedID);
            }
            else
            {
                workFlow.RelatedType = "Other";
                workFlow.RelatedID = int.Parse(strID);
            }
            workFlow.RelatedCode = "CarApply" + strID;


            workFlow.DIYNextStep = "YES";
            workFlow.IsPlanMainWorkflow = "NO";

            if (CB_SMS.Checked == true)
            {
                workFlow.ReceiveSMS = "YES";
            }
            else
            {
                workFlow.ReceiveSMS = "NO";
            }

            if (CB_Mail.Checked == true)
            {
                workFlow.ReceiveEMail = "YES";
            }
            else
            {
                workFlow.ReceiveEMail = "NO";
            }

            try
            {
                workFlowBLL.AddWorkFlow(workFlow);
                strWLID = ShareClass.GetMyCreatedWorkFlowID(strUserCode);

                strCmdText = "select * from T_CarApplyForm where ID = " + strID;

                strXMLFile2 = Server.MapPath(strXMLFile2);
                xmlProcess.DbToXML(strCmdText, "T_CarApplyForm", strXMLFile2);

                LoadRelatedWL("CarApply" + strID);

                DL_Status.SelectedValue = "InProgress";

                BT_SubmitApply.Enabled = false;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCLSSCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                strWLID = "0";

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCLSSCSBKNSMCZSGDJC").ToString().Trim() + "')", true);
            }

            LoadCarApplyForm(strUserCode);
        }
        catch
        {
            strWLID = "0";
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }

        return strWLID;
    }

    protected void BT_ActiveYes_Click(object sender, EventArgs e)
    {
        string strWLID = SubmitApply();

        if (strWLID != "0")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "popShowByURL('TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID + "','workflow','99%','99%',window.location);", true);
        }
    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        SubmitApply();
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        int intWLNumber;
        string strID, strHQL;
        IList lst;

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

                strHQL = "from CarApplyForm as carApplyForm Where carApplyForm.ID = " + strID;
                CarApplyFormBLL carApplyFormBLL = new CarApplyFormBLL();
                lst = carApplyFormBLL.GetAllCarApplyForms(strHQL);
                CarApplyForm carApplyForm = (CarApplyForm)lst[0];


                LB_ID.Text = strID;

                TB_DepartCode.Text = carApplyForm.DepartCode.Trim();
                LB_UserCode.Text = carApplyForm.ApplicantCode.Trim();
                LB_UserName.Text = carApplyForm.ApplicantName.Trim();
                DL_CarType.SelectedValue = carApplyForm.CarType;
                TB_BoardingSite.Text = carApplyForm.BoardingSite.Trim();
                TB_Destination.Text = carApplyForm.Destination.Trim();

                DLC_DepartTime.Text = carApplyForm.DepartTime.ToString("yyyy-MM-dd");
                DL_DepartBeginHour.SelectedValue = carApplyForm.DepartTime.Hour.ToString();
                DL_DepartBeginMinute.SelectedValue = carApplyForm.DepartTime.Minute.ToString();

                DLC_BackTime.Text = carApplyForm.BackTime.ToString("yyyy-MM-dd");
                DL_BackBeginHour.SelectedValue = carApplyForm.BackTime.Hour.ToString();
                DL_BackBeginMinute.SelectedValue = carApplyForm.BackTime.Minute.ToString();

                TB_Attendant.Text = carApplyForm.Attendant.Trim();
                TB_ApplyReason.Text = carApplyForm.ApplyReason.Trim();
                DL_Status.SelectedValue = carApplyForm.Status.Trim();

                BT_SubmitApply.Enabled = true;

                LoadRelatedWL("CarApply" + strID);

                intWLNumber = GetRelatedWorkFlowNumber("VehicleRequest", "Other", strID);

                if (intWLNumber > 0)
                {
                    BT_SubmitApply.Enabled = false;
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
                    CarApplyFormBLL carApplyFormBLL = new CarApplyFormBLL();
                    CarApplyForm carApplyForm = new CarApplyForm();

                    carApplyForm.ID = int.Parse(strID);


                    carApplyFormBLL.DeleteCarApplyForm(carApplyForm);


                    BT_SubmitApply.Enabled = false;

                    LoadCarApplyForm(strUserCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBKNFYMCHYTZSGDJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        CarApplyFormBLL carApplyFormBLL = new CarApplyFormBLL();
        IList lst = carApplyFormBLL.GetAllCarApplyForms(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }


    protected void BT_Reflash_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'VehicleRequest'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popWFApplyWindow','true') ", true);
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
            strHQL = "Update T_CarApplyForm Set Status = " + "'" + strSatus + "'" + " where ID = " + strID;


            try
            {
                ShareClass.RunSqlCommand(strHQL);

                LoadCarApplyForm(strUserCode);
            }
            catch
            {
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void LoadCarApplyForm(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From CarApplyForm as carApplyForm Where carApplyForm.MakeUserCode = " + "'" + strUserCode + "'";
        strHQL += " Order By carApplyForm.DepartTime DESC";
        CarApplyFormBLL carApplyFormBLL = new CarApplyFormBLL();
        lst = carApplyFormBLL.GetAllCarApplyForms(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }


    protected void LoadRelatedWL(string strRelatedCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.RelatedCode = '" + strRelatedCode + "'";
        strHQL += " Order by workFlow.WLID DESC";
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


    protected void InitialCalendar()
    {
        hour = new ArrayList();
        m = new ArrayList();

        for (i = 0; i <= 23; i++)
            hour.Add(i.ToString());
        for (i = 00; i <= 59; i++)
            m.Add(i.ToString());

        DL_DepartBeginHour.DataSource = hour;
        DL_DepartBeginHour.DataBind();
        DL_DepartBeginHour.Text = System.DateTime.Now.Hour.ToString();

        DL_DepartBeginMinute.DataSource = m;
        DL_DepartBeginMinute.DataBind();
        DL_DepartBeginMinute.Text = System.DateTime.Now.Minute.ToString();


        DL_BackBeginHour.DataSource = hour;
        DL_BackBeginHour.DataBind();
        DL_BackBeginHour.Text = System.DateTime.Now.Hour.ToString();

        DL_BackBeginMinute.DataSource = m;
        DL_BackBeginMinute.DataBind();
        DL_BackBeginMinute.Text = System.DateTime.Now.Minute.ToString();
    }
}
