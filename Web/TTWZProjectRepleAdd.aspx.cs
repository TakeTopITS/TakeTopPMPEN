using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZProjectRepleAdd : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();
        strUserName = Session["UserName"] == null ? "" : Session["UserName"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            //BindStockData();

            if (!string.IsNullOrEmpty(Request.QueryString["strProjectCode"]))
            {
                string strProjectCode = Request.QueryString["strProjectCode"].ToString();
                HF_ID.Value = strProjectCode;

                BindProjectData(strProjectCode);

                TXT_SupplementEditor.Text = strUserName;
                HF_SupplementEditor.Value = strUserCode;
            }
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strProjectName = TXT_ProjectName.Text.Trim();
            string strProjectManager = HF_ProjectManager.Value;
            string strStartTime = TXT_StartTime.Text.Trim();
            string strPowerPurchase = TXT_PowerPurchase.Text.Trim();
            string strDelegateAgent = HF_DelegateAgent.Value; //TXT_DelegateAgent.Text.Trim();
            string strProjectDesc = TXT_ProjectDesc.Text.Trim();
            string strStoreRoom = TXT_StoreRoom.Text; //DDL_StoreRoom.SelectedValue;
            string strPurchaseManager = HF_PurchaseManager.Value; //TXT_PurchaseManager.Text.Trim();
            string strPurchaseEngineer = HF_PurchaseEngineer.Value; //TXT_PurchaseEngineer.Text.Trim();
            string strContracter = HF_Contracter.Value;//TXT_Contracter.Text.Trim();
            string strChecker = HF_Checker.Value; //TXT_Checker.Text.Trim();
            string strSafekeep = HF_Safekeep.Value; //TXT_Safekeep.Text.Trim();

            string strSupplementEditor = HF_SupplementEditor.Value;



            if (string.IsNullOrEmpty(strProjectName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMMCBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strProjectName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMMCBNBHFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strProjectManager))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMJLBYXWKBC+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strStartTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKGRBYXWKBC+"')", true);
                return;
            }



            WZProjectBLL wZProjectBLL = new WZProjectBLL();

            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //修改
                string strProjectCode = HF_ID.Value;
                string strWZProjectSql = "from WZProject as wZProject where ProjectCode = '" + strProjectCode + "'";
                IList projectList = wZProjectBLL.GetAllWZProjects(strWZProjectSql);
                if (projectList != null && projectList.Count > 0)
                {
                    WZProject wZProject = (WZProject)projectList[0];
                    if (wZProject.Progress == "立项")
                    {
                        if (wZProject.PowerPurchase == "有")
                        {
                            //if (string.IsNullOrEmpty(strStoreRoom))
                            //{
                            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKBBYXWKBC+"')", true);
                            //    return;
                            //}
                            //if (string.IsNullOrEmpty(strDelegateAgent))
                            //{
                            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZWTDLRBYXWKBC+"')", true);
                            //    return;
                            //}
                            //if (string.IsNullOrEmpty(strPurchaseManager))
                            //{
                            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCGJLBYXWKBC+"')", true);
                            //    return;
                            //}
                            //if (string.IsNullOrEmpty(strPurchaseEngineer))
                            //{
                            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCGGCSBYXWKBC+"')", true);
                            //    return;
                            //}
                            //if (string.IsNullOrEmpty(strContracter))
                            //{
                            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZHTYBYXWKBC+"')", true);
                            //    return;
                            //}
                            //if (string.IsNullOrEmpty(strChecker))
                            //{
                            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCJYBYXWKBC+"')", true);
                            //    return;
                            //}
                            //if (string.IsNullOrEmpty(strSafekeep))
                            //{
                            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBGYBYXWKBC+"')", true);
                            //    return;
                            //}
                        }
                        else {
                            if (string.IsNullOrEmpty(strPurchaseEngineer))
                            {
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCGGCSBYXWKBC+"')", true);
                                return;
                            }
                        }
                        wZProject.StoreRoom = strStoreRoom;
                        wZProject.DelegateAgent = strDelegateAgent;
                        wZProject.PurchaseManager = strPurchaseManager;
                        wZProject.PurchaseEngineer = strPurchaseEngineer;
                        wZProject.Contracter = strContracter;
                        wZProject.Checker = strChecker;
                        wZProject.Safekeep = strSafekeep;
                        wZProject.SupplementEditor = strSupplementEditor;

                        wZProjectBLL.UpdateWZProject(wZProject, strProjectCode);

                        //修改库别的使用标记
                        string strUpdateStockHQL = string.Format(@"update T_WZStock
                                        set IsMark = -1
                                        where StockCode = '{0}'", strStoreRoom);
                        ShareClass.RunSqlCommand(strUpdateStockHQL);
                    }
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBNBJLX+"')", true);
                return;
            }


            //Response.Redirect("TTWZProjectReple.aspx");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
        catch (Exception ex)
        { }
    }


    private void BindProjectData(string strProjectCode)
    {
        string strWZProjectHQL = string.Format(@"select p.*,
                        pp.UserName as ProjectManagerName,
                        pd.UserName as DelegateAgentName,
                        pm.UserName as PurchaseManagerName,
                        pe.UserName as PurchaseEngineerName,
                        pc.UserName as ContracterName,
                        pk.UserName as CheckerName,
                        ps.UserName as SafekeepName,
                        pa.UserName as MarkerName,
                        pu.UserName as SupplementEditorName
                        from T_WZProject p
                        left join T_ProjectMember pp on p.ProjectManager = pp.UserCode
                        left join T_ProjectMember pd on p.DelegateAgent = pd.UserCode
                        left join T_ProjectMember pm on p.PurchaseManager = pm.UserCode
                        left join T_ProjectMember pe on p.PurchaseEngineer = pe.UserCode
                        left join T_ProjectMember pc on p.Contracter = pc.UserCode
                        left join T_ProjectMember pk on p.Checker = pk.UserCode
                        left join T_ProjectMember ps on p.Safekeep = ps.UserCode
                        left join T_ProjectMember pa on p.Marker = pa.UserCode
                        left join T_ProjectMember pu on p.SupplementEditor = pu.UserCode
                        where p.ProjectCode = '{0}'", strProjectCode);
        DataTable dtProject = ShareClass.GetDataSetFromSql(strWZProjectHQL, "Project").Tables[0];
        if (dtProject != null && dtProject.Rows.Count > 0)
        {
            DataRow drProject = dtProject.Rows[0];

            LB_ProjectCode.Text = ShareClass.ObjectToString(drProject["ProjectCode"]);
            TXT_ProjectName.Text = ShareClass.ObjectToString(drProject["ProjectName"]);
            TXT_ProjectManager.Text = ShareClass.ObjectToString(drProject["ProjectManagerName"]);
            HF_ProjectManager.Value = ShareClass.ObjectToString(drProject["ProjectManager"]);
            TXT_StartTime.Text = ShareClass.ObjectToString(drProject["StartTime"]);
            string strPowerPurchase = ShareClass.ObjectToString(drProject["PowerPurchase"]);
            TXT_PowerPurchase.Text = strPowerPurchase;
            TXT_DelegateAgent.Text = ShareClass.ObjectToString(drProject["DelegateAgentName"]);
            HF_DelegateAgent.Value = ShareClass.ObjectToString(drProject["DelegateAgent"]);
            TXT_ProjectDesc.Text = ShareClass.ObjectToString(drProject["ProjectDesc"]);
            //DDL_StoreRoom.SelectedValue = ShareClass.ObjectToString(drProject["StoreRoom"]);
            TXT_StoreRoom.Text = ShareClass.ObjectToString(drProject["StoreRoom"]);
            TXT_PurchaseManager.Text = ShareClass.ObjectToString(drProject["PurchaseManagerName"]);
            HF_PurchaseManager.Value = ShareClass.ObjectToString(drProject["PurchaseManager"]);
            TXT_PurchaseEngineer.Text = ShareClass.ObjectToString(drProject["PurchaseEngineerName"]);
            HF_PurchaseEngineer.Value = ShareClass.ObjectToString(drProject["PurchaseEngineer"]);
            TXT_Contracter.Text = ShareClass.ObjectToString(drProject["ContracterName"]);
            HF_Contracter.Value = ShareClass.ObjectToString(drProject["Contracter"]);
            TXT_Checker.Text = ShareClass.ObjectToString(drProject["CheckerName"]);
            HF_Checker.Value = ShareClass.ObjectToString(drProject["Checker"]);
            TXT_Safekeep.Text = ShareClass.ObjectToString(drProject["SafekeepName"]);
            HF_Safekeep.Value = ShareClass.ObjectToString(drProject["Safekeep"]);

            string strOldProjectCode = ShareClass.ObjectToString(drProject["ProjectCode"]);


            string strProgress = ShareClass.ObjectToString(drProject["Progress"]);

            TXT_MarkTime.Text = ShareClass.ObjectToString(drProject["MarkTime"]);
            TXT_Marker.Text = ShareClass.ObjectToString(drProject["MarkerName"]);


            if (strPowerPurchase.Trim() == "有")
            {
                //DDL_StoreRoom.Enabled = true;
                //DDL_StoreRoom.BackColor = Color.CornflowerBlue;
                //TXT_StoreRoom.ReadOnly = true;
                TXT_StoreRoom.BackColor = Color.CornflowerBlue;
                //TXT_Checker.ReadOnly = true;
                TXT_Checker.BackColor = Color.CornflowerBlue;
                //TXT_Safekeep.ReadOnly = true;
                TXT_Safekeep.BackColor = Color.CornflowerBlue;

                BT_StoreBie.Attributes.Remove("disabled");
                btnChecker.Attributes["disabled"] = "disabled";
            }else{
                //DDL_StoreRoom.Enabled = false;
                //DDL_StoreRoom.BackColor = Color.White;
                //TXT_StoreRoom.ReadOnly = false;
                TXT_StoreRoom.BackColor = Color.White;
                //TXT_Checker.ReadOnly = true;
                TXT_Checker.BackColor = Color.White;
                //TXT_Safekeep.ReadOnly = true;
                TXT_Safekeep.BackColor = Color.White;

                BT_StoreBie.Attributes["disabled"] = "disabled";
                btnChecker.Attributes.Remove("disabled");
            }
            TXT_DelegateAgent.BackColor = Color.CornflowerBlue;
            TXT_PurchaseManager.BackColor = Color.CornflowerBlue;
            TXT_PurchaseEngineer.BackColor = Color.CornflowerBlue;
            TXT_Contracter.BackColor = Color.CornflowerBlue;
            
        }
    }





    //private void BindStockData()
    //{
    //    WZStockBLL wZStockBLL = new WZStockBLL();
    //    string strStockHQL = "from WZStock as wZStock";
    //    IList lstStock = wZStockBLL.GetAllWZStocks(strStockHQL);

    //    DDL_StoreRoom.DataSource = lstStock;
    //    DDL_StoreRoom.DataBind();

    //    DDL_StoreRoom.Items.Insert(0, new ListItem("-", ""));
    //}


//    protected void DDL_StoreRoom_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        string strStoreRoomSelectedValue = DDL_StoreRoom.SelectedValue;
//        if (!string.IsNullOrEmpty(strStoreRoomSelectedValue))
//        {
//            string strStockHQL = string.Format(@"select s.*,p.UserName as SafekeepName,m.UserName as CheckerName from T_WZStock s
//                            left join T_ProjectMember p on s.Safekeep = p.UserCode
//                            left join T_ProjectMember m on s.Checker = m.UserCode
//                            where s.StockCode = '{0}'", strStoreRoomSelectedValue);
//            DataTable dtStock = ShareClass.GetDataSetFromSql(strStockHQL, "Stock").Tables[0];

//            if (dtStock != null && dtStock.Rows.Count > 0)
//            {
//                DataRow drProject = dtStock.Rows[0];

//                TXT_Checker.Text = ShareClass.ObjectToString(drProject["CheckerName"]);
//                HF_Checker.Value = ShareClass.ObjectToString(drProject["Checker"]);
//                TXT_Safekeep.Text = ShareClass.ObjectToString(drProject["SafekeepName"]);
//                HF_Safekeep.Value = ShareClass.ObjectToString(drProject["Safekeep"]);

//            }
//        }
//    }
}