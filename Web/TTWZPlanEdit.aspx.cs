using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZPlanEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            BindStockData();
            DataProjectBinder();

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string strPlanCode = Request.QueryString["id"].ToString();
                HF_PlanCode.Value = strPlanCode;

                BindDataer(strPlanCode);
            }

            DDL_StoreRoom.Enabled = false;
            //TXT_PickingUnit.ReadOnly = true;
            //TXT_UnitCode.ReadOnly = true;
            TXT_FeeManage.ReadOnly = true;
            TXT_PurchaseEngineer.ReadOnly = true;

            TXT_SinceNumber.BackColor = Color.CornflowerBlue;
            TXT_PlanName.BackColor = Color.CornflowerBlue;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
            string strSinceNumber = TXT_SinceNumber.Text.Trim();
            string strPlanName = TXT_PlanName.Text.Trim();
            string strProjectCode = DDL_Project.SelectedItem.Text;
            string strProjectName = DDL_Project.SelectedValue;
            string strStoreRoom = DDL_StoreRoom.SelectedValue;
            string strPickingUnit = TXT_PickingUnit.Text;
            string strUnitCode = TXT_UnitCode.Text;
            string strSupplyMethod = DDL_SupplyMethod.SelectedValue;
            string strFeeManage = HF_FeeManage.Value;               //TXT_FeeManage.Text;
            string strPurchaseEngineer = HF_PurchaseEngineer.Value; //TXT_PurchaseEngineer.Text;
         
            if (string.IsNullOrEmpty(strSupplyMethod) | string.IsNullOrEmpty(strSinceNumber) | string.IsNullOrEmpty(strProjectCode) | string.IsNullOrEmpty(strPlanName) | string.IsNullOrEmpty(strPickingUnit))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSGXDBNWKQJC").ToString().Trim() + "')", true);
                return;
            }

            if (!string.IsNullOrEmpty(HF_PlanCode.Value))
            {
                //修改
                string strPlanCode = HF_PlanCode.Value;
                string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strPlanCode + "'";
                IList listPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
                if (listPickingPlan != null && listPickingPlan.Count > 0)
                {
                    WZPickingPlan wZPickingPlan = (WZPickingPlan)listPickingPlan[0];
                    wZPickingPlan.SinceNumber = strSinceNumber;
                    wZPickingPlan.PlanName = strPlanName;
                    wZPickingPlan.ProjectCode = strProjectCode;
                    wZPickingPlan.ProjectName = strProjectName;
                    wZPickingPlan.StoreRoom = strStoreRoom;
                    wZPickingPlan.PickingUnit = strPickingUnit;
                    wZPickingPlan.UnitCode = strUnitCode;
                    wZPickingPlan.SupplyMethod = strSupplyMethod;
                    wZPickingPlan.FeeManage = strFeeManage;
                    wZPickingPlan.FeeManage = strFeeManage;

                    wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, strPlanCode);
                }
            }
            else
            {
                //判断自编号是否存在
                string strCheckSinceNumberHQL = "select count(1) as RowNumber from T_WZPickingPlan where SinceNumber = '" + strSinceNumber + "'";
                DataTable dtCheckSinceNumber = ShareClass.GetDataSetFromSql(strCheckSinceNumberHQL, "strCheckSinceNumberHQL").Tables[0];
                int intCheckSinceNumber = int.Parse(dtCheckSinceNumber.Rows[0]["RowNumber"].ToString());
                if (intCheckSinceNumber > 0)
                {
                    //自编号已经存在！
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZBHYJCZGG").ToString().Trim() + "')", true);
                    return;
                }

                //增加
                WZPickingPlan wZPickingPlan = new WZPickingPlan();
                wZPickingPlan.PlanCode = CreateNewPlanID();         //生成计划编号
                //wZPickingPlan.PlanCode = GetMaxPlanCode();
                wZPickingPlan.SinceNumber = strSinceNumber;
                wZPickingPlan.PlanName = strPlanName;
                wZPickingPlan.ProjectCode = strProjectCode;
                wZPickingPlan.ProjectName = strProjectName;
                wZPickingPlan.StoreRoom = strStoreRoom;
                wZPickingPlan.PickingUnit = strPickingUnit;
                wZPickingPlan.UnitCode = strUnitCode;
                wZPickingPlan.SupplyMethod = strSupplyMethod;
                wZPickingPlan.PlanMarker = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();
                wZPickingPlan.MarkerTime = DateTime.Now;
                //wZPickingPlan.CommitTime = DateTime.Now;
                wZPickingPlan.FeeManage = strFeeManage;
                //wZPickingPlan.ApproveTime = DateTime.Now;
                wZPickingPlan.PurchaseEngineer = strPurchaseEngineer;
                //wZPickingPlan.SignTime = DateTime.Now;
                //wZPickingPlan.CancelTime = DateTime.Now;

                wZPickingPlan.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                wZPickingPlan.IsMark = 0;

                wZPickingPlanBLL.AddWZPickingPlan(wZPickingPlan);

                //工程立项使用标记为-1
                string strUpdateProjectHQL = "update T_WZProject set IsMark = -1 where ProjectCode = '" + strProjectCode + "'";
                ShareClass.RunSqlCommand(strUpdateProjectHQL);
            }

            //Response.Redirect("TTWZPlanList.aspx");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
    }

    protected string GetMaxPlanCode()
    {
        string strHQL;
        string strMaxPlanCode;

        strHQL = "Select * From T_WZPickingPlan Where PlanCode Like " + "'" + DateTime.Now.ToString("yyyyMM") + "%'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPickingPlan");

        strMaxPlanCode = (int.Parse(DateTime.Now.ToString("yyyyMM") + "0000") + ds.Tables[0].Rows.Count + 1).ToString();

        return strMaxPlanCode;
    }

    protected void BT_Department_Click(object sender, EventArgs e)
    {
        string strDepartment = HF_Department.Value;
        if (!string.IsNullOrEmpty(strDepartment))
        {
            string strHQL;
            IList lst;

            strHQL = "from DepartRelatedWZFeeUser as departRelatedWZFeeUser where departRelatedWZFeeUser.DepartCode = " + "'" + strDepartment + "'";
            DepartRelatedWZFeeUserBLL departRelatedWZFeeUserBLL = new DepartRelatedWZFeeUserBLL();
            lst = departRelatedWZFeeUserBLL.GetAllDepartRelatedWZFeeUsers(strHQL);

            if (lst != null && lst.Count > 0)
            {
                DepartRelatedWZFeeUser departRelatedWZFeeUser = (DepartRelatedWZFeeUser)lst[0];

                HF_FeeManage.Value = departRelatedWZFeeUser.UserCode;
            }
        }
    }

    private void BindDataer(string strPlanCode)
    {
        string strWZPickingPlanSql = string.Format(@"select pp.*,
                        pm.UserName as PlanMarkerName,
                        pf.UserName as FeeManageName,
                        pe.UserName as PurchaseEngineerName
                        from T_WZPickingPlan pp
                        left join T_ProjectMember pm on pp.PlanMarker = pm.UserCode
                        left join T_ProjectMember pf on pp.FeeManage = pf.UserCode
                        left join T_ProjectMember pe on pp.PurchaseEngineer = pe.UserCode
                        where pp.PlanCode = '{0}'", strPlanCode);
        DataTable dtPlan = ShareClass.GetDataSetFromSql(strWZPickingPlanSql, "PickingPlan").Tables[0];
        if (dtPlan != null && dtPlan.Rows.Count > 0)
        {
            DataRow drPlan = dtPlan.Rows[0];

            LB_PlanCode.Text = ShareClass.ObjectToString(drPlan["PlanCode"]);
            TXT_SinceNumber.Text = ShareClass.ObjectToString(drPlan["SinceNumber"]);
            TXT_PlanName.Text = ShareClass.ObjectToString(drPlan["PlanName"]);
            DDL_Project.SelectedItem.Text = ShareClass.ObjectToString(drPlan["ProjectCode"]);
            DDL_StoreRoom.SelectedValue = ShareClass.ObjectToString(drPlan["StoreRoom"]);
            TXT_PickingUnit.Text = ShareClass.ObjectToString(drPlan["PickingUnit"]);
            TXT_UnitCode.Text = ShareClass.ObjectToString(drPlan["UnitCode"]);
            DDL_SupplyMethod.SelectedValue = ShareClass.ObjectToString(drPlan["SupplyMethod"]);
            TXT_FeeManage.Text = ShareClass.ObjectToString(drPlan["FeeManageName"]);
            HF_FeeManage.Value = ShareClass.ObjectToString(drPlan["FeeManage"]);
            TXT_PurchaseEngineer.Text = ShareClass.ObjectToString(drPlan["PurchaseEngineerName"]);
            HF_PurchaseEngineer.Value = ShareClass.ObjectToString(drPlan["PurchaseEngineer"]);

            string strProgress = ShareClass.ObjectToString(drPlan["Progress"]);

            if(ShareClass.ObjectToString(drPlan["SupplyMethod"]) != LanguageHandle.GetWord("JiaGong").ToString().Trim())
            {
                DDL_SupplyMethod.Enabled = false;
            }

            if (strProgress == LanguageHandle.GetWord("LuRu").ToString().Trim())
            {
                TXT_SinceNumber.ReadOnly = false;
                TXT_PlanName.ReadOnly = false;
                DDL_Project.Enabled = true;
                DDL_StoreRoom.Enabled = true;
                TXT_PickingUnit.ReadOnly = false;
                TXT_UnitCode.ReadOnly = false;
                DDL_SupplyMethod.Enabled = true;
                TXT_FeeManage.ReadOnly = false;
                TXT_PurchaseEngineer.ReadOnly = false;
            }
            else
            {
                TXT_SinceNumber.ReadOnly = true;
                TXT_PlanName.ReadOnly = true;
                DDL_Project.Enabled = false;
                DDL_StoreRoom.Enabled = false;
                TXT_PickingUnit.ReadOnly = true;
                TXT_UnitCode.ReadOnly = true;
                DDL_SupplyMethod.Enabled = false;
                TXT_FeeManage.ReadOnly = true;
                TXT_PurchaseEngineer.ReadOnly = true;
            }

            if(GetPickingUnitType(ShareClass.ObjectToString(drPlan["ProjectCode"])) == LanguageHandle.GetWord("XiangMuBu").ToString().Trim())
            {
                btnPickingUnit.Visible = false;
            }
        }
    }

    //取得项目部门单位类型
    private string GetPickingUnitType(string strProjectCode)
    {
        string strHQL;
        string strUnitType;

        strHQL = "Select UnitType From T_WZProject Where ProjectCode = '" + strProjectCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZProject");
        if(ds.Tables [0].Rows.Count > 0)
        {
            strUnitType = ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            strUnitType = "";
        }

        return strUnitType;
    }

    private void BindStockData()
    {
        WZStockBLL wZStockBLL = new WZStockBLL();
        string strStockHQL = "from WZStock as wZStock";
        IList lstStock = wZStockBLL.GetAllWZStocks(strStockHQL);

        DDL_StoreRoom.DataSource = lstStock;
        DDL_StoreRoom.DataBind();

        DDL_StoreRoom.Items.Insert(0, new ListItem("-", ""));
    }

    private void DataProjectBinder()
    {
        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectHQL = "from WZProject as wZProject where Progress = '开工' and IsStatus != 'Closed' order by MarkTime desc"; 
        IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);

        DDL_Project.DataSource = listProject;
        DDL_Project.DataBind();

        DDL_Project.Items.Insert(0, new ListItem("--Select--", ""));
    }


    protected void DDL_Project_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strProjectSelectedValue = DDL_Project.SelectedItem.Text;
        if (!string.IsNullOrEmpty(strProjectSelectedValue))
        {
            string strProjectHQL = string.Format(@"select p.*,
                        pf.UserName as FeeManageName,
                        pp.UserName as PurchaseEngineerName
                        from T_WZProject p
                        left join T_ProjectMember pf on p.FeeManage = pf.UserCode
                        left join T_ProjectMember pp on p.PurchaseEngineer = pp.UserCode
                        where p.ProjectCode = '{0}'", strProjectSelectedValue);
            DataTable dtProject = ShareClass.GetDataSetFromSql(strProjectHQL, "Project").Tables[0];

            if (dtProject != null && dtProject.Rows.Count > 0)
            {
                DataRow drProject = dtProject.Rows[0];

                DDL_StoreRoom.SelectedValue = ShareClass.ObjectToString(drProject["StoreRoom"]);
                //TXT_PickingUnit.Text = ShareClass.ObjectToString(drProject["PickingUnit"]);
                //TXT_UnitCode.Text = ShareClass.ObjectToString(drProject["UnitCode"]);
                TXT_FeeManage.Text = ShareClass.ObjectToString(drProject["FeeManageName"]);
                HF_FeeManage.Value = ShareClass.ObjectToString(drProject["FeeManage"]);
                TXT_PurchaseEngineer.Text = ShareClass.ObjectToString(drProject["PurchaseEngineerName"]);
                HF_PurchaseEngineer.Value = ShareClass.ObjectToString(drProject["PurchaseEngineer"]);

                TXT_FeeManage.Text = ShareClass.ObjectToString(drProject["FeeManageName"]);
                HF_FeeManage.Value = ShareClass.ObjectToString(drProject["FeeManage"]);

                string strPowerPurchase = ShareClass.ObjectToString(drProject["PowerPurchase"]);

                if (strPowerPurchase == LanguageHandle.GetWord("Mo").ToString().Trim())
                {
                    DDL_SupplyMethod.SelectedValue = LanguageHandle.GetWord("JiaGong").ToString().Trim();
                    DDL_SupplyMethod.Enabled = false;
                }
                else
                {
                    DDL_SupplyMethod.SelectedValue = "";
                    DDL_SupplyMethod.Enabled = true;
                }

                string strUnitType = ShareClass.ObjectToString(drProject["UnitType"]);
                if (strUnitType == LanguageHandle.GetWord("XiangMuBu").ToString().Trim())
                {
                    TXT_PickingUnit.Text = ShareClass.ObjectToString(drProject["ProjectName"]);
                    HF_Department.Value = ShareClass.ObjectToString(drProject["ProjectCode"]);

                    TXT_UnitCode.Text = ShareClass.ObjectToString(drProject["ProjectCode"]);
                    btnPickingUnit.Visible = false;
                }
                else
                {
                    btnPickingUnit.Visible = true;
                }
            }
        }
    }

    /// <summary>
    ///  生成领料计划ID
    /// </summary>
    private string CreateNewPlanID()
    {
        string strNewPlanID = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                string strPlanIDHQL = string.Format("select count(1) as RowNumber from T_WZPickingPlan where to_char( MarkerTime, 'yyyy-mm-dd') like '%{0}%'", DateTime.Now.ToString("yyyyMM"));
                DataTable dtPlanID = ShareClass.GetDataSetFromSql(strPlanIDHQL, "PlanID").Tables[0];
                int intPlanIDNumber = 0;
                int.TryParse(ShareClass.ObjectToString(dtPlanID.Rows[0]["RowNumber"]), out intPlanIDNumber);
                intPlanIDNumber = intPlanIDNumber + 1;
                string strYear = DateTime.Now.Year.ToString();
                string strMonth = DateTime.Now.Month.ToString();
                do
                {
                    StringBuilder sbPlanID = new StringBuilder();
                    for (int j = 4 - intPlanIDNumber.ToString().Length; j > 0; j--)
                    {
                        sbPlanID.Append("0");
                    }
                    if (strMonth.Length == 1)
                    {
                        strMonth = "0" + strMonth;
                    }
                    strNewPlanID = strYear+ strMonth + sbPlanID.ToString() + intPlanIDNumber.ToString();

                    //验证新的计划ID是否存在
                    string strCheckNewPlanIDHQL = "select count(1) as RowNumber from T_WZPickingPlan where PlanCode = '" + strNewPlanID + "'";
                    DataTable dtCheckNewPlanID = ShareClass.GetDataSetFromSql(strCheckNewPlanIDHQL, "CheckNewPlanID").Tables[0];
                    int intCheckNewPlanID = int.Parse(dtCheckNewPlanID.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewPlanID == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intPlanIDNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewPlanID;
    }
}
