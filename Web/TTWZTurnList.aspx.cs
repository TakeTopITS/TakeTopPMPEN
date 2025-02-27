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

public partial class TTWZTurnList : System.Web.UI.Page
{
    string strUserCode;


    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataBinder();
            DataProjectBinder();

            //BindStockData();

            DataPickingUnitBinder();
        }
    }

    private void DataBinder()
    {
        DG_List.CurrentPageIndex = 0;

        string strWZTurnHQL = string.Format(@"select t.*,p.UserName as PurchaseEngineerName,
                        m.UserName as MaterialPersonName,
                        c.UserName as CheckPersonName
                        from T_WZTurn t
                        left join T_ProjectMember p on t.PurchaseEngineer = p.UserCode
                        left join T_ProjectMember m on t.MaterialPerson = m.UserCode
                        left join T_ProjectMember c on t.CheckPerson = c.UserCode 
                        where t.PurchaseEngineer = '{0}' 
                        order by t.TurnTime desc", strUserCode);
        DataTable dtTurn = ShareClass.GetDataSetFromSql(strWZTurnHQL, "Turn").Tables[0];

        DG_List.DataSource = dtTurn;
        DG_List.DataBind();

        
        LB_Sql.Text = strWZTurnHQL;
        LB_Record.Text = dtTurn.Rows.Count.ToString();

        try
        {
            string strProgress;
            for (int i = 0; i < dtTurn.Rows.Count; i++)
            {
                if (dtTurn.Rows[i]["Progress"] != null)
                {
                    strProgress = dtTurn.Rows[i]["Progress"].ToString();
                    if (strProgress == LanguageHandle.GetWord("LuRu").ToString().Trim())
                    {
                        DG_List.Items[i].Cells[6].Text = "";
                    }
                }
            }
        }
        catch(Exception ex)
        {
            Response.Write(ex.Message.ToString());
        }
    }


    private void DataProjectBinder()
    {
        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectHQL = "from WZProject as wZProject where Progress='开工' and PurchaseEngineer = '" + strUserCode + "' order by MarkTime desc"; 
        IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);

        DDL_Project.DataSource = listProject;
        DDL_Project.DataBind();

        DDL_Project.Items.Insert(0, new ListItem("--Select--", ""));
    }



    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZTurnBLL wZTurnBLL = new WZTurnBLL();
                string strWZTurnHQL = "from WZTurn as wZTurn where TurnCode = '" + cmdArges + "'";
                IList listWZTurn = wZTurnBLL.GetAllWZTurns(strWZTurnHQL);
                if (listWZTurn != null && listWZTurn.Count == 1)
                {
                    WZTurn wZTurn = (WZTurn)listWZTurn[0];

                    if (wZTurn.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBJBW0BNSC").ToString().Trim()+"')", true);
                        return;
                    }

                    wZTurnBLL.DeleteWZTurn(wZTurn);

                    //重新加载列表
                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                }

            }
            else if (cmdName == "edit")
            {
                for (int i = 0; i < DG_List.Items.Count; i++)
                {
                    DG_List.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                string strWZTurnHQL = string.Format(@"select t.*,m.UserName as MaterialPersonName from T_WZTurn t
                            left join T_ProjectMember m on t.MaterialPerson = m.UserCode 
                            where t.TurnCode = '{0}'", cmdArges);
                DataTable dtWZTurn = ShareClass.GetDataSetFromSql(strWZTurnHQL, "WZTurn").Tables[0];
                if (dtWZTurn != null && dtWZTurn.Rows.Count > 0)
                {
                    DataRow drTurn = dtWZTurn.Rows[0];

                    DDL_Project.SelectedValue = ShareClass.ObjectToString(drTurn["ProjectCode"]);
                    TXT_ProjectName.Text = ShareClass.ObjectToString(drTurn["ProjectName"]);
                    TXT_UnitCode.Text = ShareClass.ObjectToString(drTurn["UnitCode"]);
                    //DDL_PickingUnit.SelectedValue = ShareClass.ObjectToString(drTurn["PickingUnit"]);
                    TXT_PickingUnit.Text = ShareClass.ObjectToString(drTurn["PickingUnit"]);

                    HF_MaterialPerson.Value = ShareClass.ObjectToString(drTurn["MaterialPerson"]);
                    TXT_MaterialPerson.Text = ShareClass.ObjectToString(drTurn["MaterialPersonName"]);
                    HF_CheckPerson.Value = ShareClass.ObjectToString(drTurn["CheckPerson"]);

                    TXT_StoreRoom.Text = ShareClass.ObjectToString(drTurn["StoreRoom"]);

                    HF_TurnCode.Value = cmdArges;
                    HF_ID.Value = ShareClass.ObjectToString(drTurn["ID"]);
                    LB_TurnCode.Text = ShareClass.ObjectToString(drTurn["TurnCode"]);

                    DDL_Project.BackColor = Color.CornflowerBlue;
                    TXT_ProjectName.BackColor = Color.CornflowerBlue;
                    TXT_StoreRoom.BackColor = Color.CornflowerBlue;
                    TXT_UnitCode.BackColor = Color.CornflowerBlue;
                    //DDL_PickingUnit.BackColor = Color.CornflowerBlue;
                    TXT_PickingUnit.BackColor = Color.CornflowerBlue;


                    String strPurchaseEngineering = ShareClass.ObjectToString(drTurn["PurchaseEngineer"]);
                    if (strUserCode == strPurchaseEngineering)
                    {
                        btnTurn.Enabled = true;
                    }
                    else
                    {
                        btnTurn.Enabled = false;
                    }

                    string strTurnCode = LB_TurnCode.Text.Trim();
                    string strHQL = "Select * From T_WZTurnDetail Where TurnCode = " + "'" + strTurnCode + "'";
                    DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZTurnDetail");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        btnTurn.Enabled = true;
                    }
                    else
                    {
                        btnTurn.Enabled = false;
                    }

                    if (ShareClass.ObjectToString(drTurn["Progress"]) == LanguageHandle.GetWord("YiJiao").ToString().Trim())
                    {
                        btnTurn.Enabled = false;
                        btnTurnCancel.Enabled = true;

                        BT_Edit.Enabled = false;
                        btnSave.Enabled = false;
                        btnReset.Enabled = false;
                    }
                    else
                    {
                        btnTurn.Enabled = true;
                        btnTurnCancel.Enabled = false;

                        BT_Edit.Enabled = true;
                        btnSave.Enabled = true;
                        btnReset.Enabled = true;
                    }
                }
            }
            else if (cmdName == "detail")
            {
                string cmdArges = e.CommandArgument.ToString();
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZTurnEdit.aspx?turnCode=" + cmdArges + "');", true);
                return;
            }
        }
    }



    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim();

        DataTable dtTurn = ShareClass.GetDataSetFromSql(strHQL, "Turn").Tables[0];

        DG_List.DataSource = dtTurn;
        DG_List.DataBind();
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strProjectCode = DDL_Project.SelectedValue;
            string strProjectName = TXT_ProjectName.Text.Trim();
            string strUnitCode = TXT_UnitCode.Text.Trim();
            //string strPickingUnit = DDL_PickingUnit.SelectedValue;
            string strPickingUnit = TXT_PickingUnit.Text.Trim();
            string strMaterialPerson = HF_MaterialPerson.Value; //TXT_MaterialPerson.Text.Trim();
            string strCheckPerson = HF_CheckPerson.Value;

            //string strStoreRoom = DDL_StoreRoom.SelectedValue;
            string strStoreRoom = TXT_StoreRoom.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZGCXM").ToString().Trim()+"')", true);
                return;
            }

            if (string.IsNullOrEmpty(strStoreRoom))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJFKBBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strStoreRoom))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJFKBBNWFFZFC").ToString().Trim()+"')", true);
                return;
            }

            WZTurnBLL wZTurnBLL = new WZTurnBLL();

            if (!string.IsNullOrEmpty(HF_TurnCode.Value))
            {
                //修改
                string turnCode = HF_TurnCode.Value;

                string strWZTurnHQL = "from WZTurn as wZTurn where TurnCode = '" + turnCode + "'";
                IList listTurn = wZTurnBLL.GetAllWZTurns(strWZTurnHQL);
                if (listTurn != null && listTurn.Count > 0)
                {
                    WZTurn wZTurn = (WZTurn)listTurn[0];
                    wZTurn.ProjectCode = strProjectCode;
                    wZTurn.ProjectName = strProjectName;
                    wZTurn.UnitCode = strUnitCode;
                    wZTurn.PickingUnit = strPickingUnit;
                    wZTurn.MaterialPerson = strMaterialPerson;
                    wZTurn.CheckPerson = strCheckPerson;

                    //wZTurn.StoreRoom = DDL_StoreRoom.SelectedValue;
                    wZTurn.StoreRoom = strStoreRoom;

                    wZTurnBLL.UpdateWZTurn(wZTurn, int.Parse(HF_ID.Value));
                }
            }
            else
            {
                //移交单号：年的后二位+月份+"≈"+当月的移交单数
                string strNewTurnCode = CreateNewTurnCode();

                //增加
                WZTurn wZTurn = new WZTurn();
                wZTurn.TurnCode = strNewTurnCode;
                wZTurn.ProjectCode = strProjectCode;
                wZTurn.ProjectName = strProjectName;
                wZTurn.UnitCode = strUnitCode;
                wZTurn.PickingUnit = strPickingUnit;
                wZTurn.TurnTime = DateTime.Now;
                wZTurn.PurchaseEngineer = strUserCode;
                wZTurn.MaterialPerson = strMaterialPerson;
                wZTurn.CheckPerson = strCheckPerson;

                //wZTurn.StoreRoom = DDL_StoreRoom.SelectedValue;
                wZTurn.StoreRoom = strStoreRoom;

                wZTurn.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                wZTurn.IsMark = 0;

                //wZTurn.SingTime = DateTime.Now;
                //wZTurn.FinishTime = DateTime.Now;

                wZTurnBLL.AddWZTurn(wZTurn);
            }

            //重新加载列表
            DataBinder();


            HF_TurnCode.Value = "";
            LB_TurnCode.Text = "";
            DDL_Project.SelectedValue = "";
            TXT_ProjectName.Text = "";
            TXT_UnitCode.Text = "";
            //DDL_PickingUnit.SelectedValue = "";
            TXT_PickingUnit.Text = "";
            TXT_MaterialPerson.Text = "";
            HF_MaterialPerson.Value = "";
            HF_CheckPerson.Value = "";
            TXT_StoreRoom.Text = "";
            HF_ID.Value = "";

            DDL_Project.BackColor = Color.White;
            TXT_ProjectName.BackColor = Color.White;
            TXT_StoreRoom.BackColor = Color.White;
            TXT_UnitCode.BackColor = Color.White;
            //DDL_PickingUnit.BackColor = Color.White;
            TXT_PickingUnit.BackColor = Color.White;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatus()", true); 
        }
        catch (Exception ex)
        { }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        HF_TurnCode.Value = "";
        LB_TurnCode.Text = "";
        DDL_Project.SelectedValue = "";
        TXT_ProjectName.Text = "";
        TXT_UnitCode.Text = "";
        //DDL_PickingUnit.SelectedValue = "";
        TXT_PickingUnit.Text = "";
        TXT_MaterialPerson.Text = "";
        HF_MaterialPerson.Value = "";
        HF_CheckPerson.Value = "";
        TXT_StoreRoom.Text = "";
        HF_ID.Value = "";

        DDL_Project.BackColor = Color.CornflowerBlue;
        TXT_ProjectName.BackColor = Color.CornflowerBlue;
        TXT_StoreRoom.BackColor = Color.CornflowerBlue;
        TXT_UnitCode.BackColor = Color.CornflowerBlue;
        //DDL_PickingUnit.BackColor = Color.CornflowerBlue;
        TXT_PickingUnit.BackColor = Color.CornflowerBlue;
    }


    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        try
        {
            string strProjectCode = DDL_Project.SelectedValue;
            string strProjectName = TXT_ProjectName.Text.Trim();
            string strUnitCode = TXT_UnitCode.Text.Trim();
            //string strPickingUnit = DDL_PickingUnit.SelectedValue;
            string strPickingUnit = TXT_PickingUnit.Text.Trim();
            string strMaterialPerson = HF_MaterialPerson.Value; //TXT_MaterialPerson.Text.Trim();
            string strCheckPerson = HF_CheckPerson.Value;

            //string strStoreRoom = DDL_StoreRoom.SelectedValue;
            string strStoreRoom = TXT_StoreRoom.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZGCXM").ToString().Trim()+"')", true);
                return;
            }

            if (string.IsNullOrEmpty(strStoreRoom))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJFKBBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strStoreRoom))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJFKBBNWFFZFC").ToString().Trim()+"')", true);
                return;
            }

            WZTurnBLL wZTurnBLL = new WZTurnBLL();

            if (!string.IsNullOrEmpty(HF_TurnCode.Value))
            {
                //修改
                string turnCode = HF_TurnCode.Value;

                string strWZTurnHQL = "from WZTurn as wZTurn where TurnCode = '" + turnCode + "'";
                IList listTurn = wZTurnBLL.GetAllWZTurns(strWZTurnHQL);
                if (listTurn != null && listTurn.Count > 0)
                {
                    WZTurn wZTurn = (WZTurn)listTurn[0];
                    wZTurn.ProjectCode = strProjectCode;
                    wZTurn.ProjectName = strProjectName;
                    wZTurn.UnitCode = strUnitCode;
                    wZTurn.PickingUnit = strPickingUnit;
                    wZTurn.MaterialPerson = strMaterialPerson;
                    wZTurn.CheckPerson = strCheckPerson;

                    //wZTurn.StoreRoom = DDL_StoreRoom.SelectedValue;
                    wZTurn.StoreRoom = strStoreRoom;

                    wZTurnBLL.UpdateWZTurn(wZTurn, int.Parse(HF_ID.Value));
                }
            }
            else
            {
                //增加 提示请先选择要修改的库别
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZYXGDKB").ToString().Trim()+"')", true);
                return;
            }

            //重新加载列表
            DataBinder();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatus()", true); 
        }
        catch (Exception ex)
        { }
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        HF_TurnCode.Value = "";
        LB_TurnCode.Text = "";
        DDL_Project.SelectedValue = "";
        TXT_ProjectName.Text = "";
        TXT_UnitCode.Text = "";
        //DDL_PickingUnit.SelectedValue = "";
        TXT_PickingUnit.Text = "";
        TXT_MaterialPerson.Text = "";
        HF_MaterialPerson.Value = "";
        HF_CheckPerson.Value = "";
        TXT_StoreRoom.Text = "";
        HF_ID.Value = "";

        DDL_Project.BackColor = Color.White;
        TXT_ProjectName.BackColor = Color.White;
        TXT_StoreRoom.BackColor = Color.White;
        TXT_UnitCode.BackColor = Color.White;
        //DDL_PickingUnit.BackColor = Color.White;
        TXT_PickingUnit.BackColor = Color.White;
    }


    protected void btnTurn_Click(object sender, EventArgs e)
    {
        try
        {
            string strProjectCode = DDL_Project.SelectedValue;
            string strProjectName = TXT_ProjectName.Text.Trim();
            string strUnitCode = TXT_UnitCode.Text.Trim();
            //string strPickingUnit = DDL_PickingUnit.SelectedValue;
            string strPickingUnit = TXT_PickingUnit.Text.Trim();
            string strMaterialPerson = HF_MaterialPerson.Value; //TXT_MaterialPerson.Text.Trim();
            string strCheckPerson = HF_CheckPerson.Value;

            //string strStoreRoom = DDL_StoreRoom.SelectedValue;
            string strStoreRoom = TXT_StoreRoom.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZGCXM").ToString().Trim() + "')", true);
                return;
            }

            if (string.IsNullOrEmpty(strStoreRoom))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJFKBBNWKBC").ToString().Trim() + "')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strStoreRoom))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJFKBBNWFFZFC").ToString().Trim() + "')", true);
                return;
            }

            WZTurnBLL wZTurnBLL = new WZTurnBLL();

            if (!string.IsNullOrEmpty(HF_TurnCode.Value))
            {
                //修改
                string turnCode = HF_TurnCode.Value;

                string strWZTurnHQL = "from WZTurn as wZTurn where TurnCode = '" + turnCode + "'";
                IList listTurn = wZTurnBLL.GetAllWZTurns(strWZTurnHQL);
                if (listTurn != null && listTurn.Count > 0)
                {
                    WZTurn wZTurn = (WZTurn)listTurn[0];
                    wZTurn.ProjectCode = strProjectCode;
                    wZTurn.ProjectName = strProjectName;
                    wZTurn.UnitCode = strUnitCode;
                    wZTurn.PickingUnit = strPickingUnit;
                    wZTurn.MaterialPerson = strMaterialPerson;
                    wZTurn.CheckPerson = strCheckPerson;
                    wZTurn.TurnTime = DateTime.Now;

                    //wZTurn.StoreRoom = DDL_StoreRoom.SelectedValue;
                    wZTurn.StoreRoom = strStoreRoom;

                    wZTurn.Progress = LanguageHandle.GetWord("YiJiao").ToString().Trim();

                    wZTurnBLL.UpdateWZTurn(wZTurn, int.Parse(HF_ID.Value));

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('移交成功！');ControlStatus()", true); 

                    //string strHQL;
                    //strHQL = "Update T_WZPickingPlan Set Progress = '移交' " + " Where PlanCode in (Select PlanCode From T_WZPickingPlanDetail Where TurnCode = " + "'" + wZTurn.TurnCode + "')";
                    //ShareClass.RunSqlCommand(strHQL);

                    btnTurn.Enabled = false;
                    btnTurnCancel.Enabled = true;

                    BT_Edit.Enabled = false;
                    btnSave.Enabled = false;
                    btnReset.Enabled = false;

                    //重新加载列表
                    DataBinder();
                }
            }
         
        }
        catch (Exception ex)
        {
        }

    }


    protected void btnTurnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            string strProjectCode = DDL_Project.SelectedValue;
            string strProjectName = TXT_ProjectName.Text.Trim();
            string strUnitCode = TXT_UnitCode.Text.Trim();
            //string strPickingUnit = DDL_PickingUnit.SelectedValue;
            string strPickingUnit = TXT_PickingUnit.Text.Trim();
            string strMaterialPerson = HF_MaterialPerson.Value; //TXT_MaterialPerson.Text.Trim();
            string strCheckPerson = HF_CheckPerson.Value;

            //string strStoreRoom = DDL_StoreRoom.SelectedValue;
            string strStoreRoom = TXT_StoreRoom.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZGCXM").ToString().Trim() + "')", true);
                return;
            }

            if (string.IsNullOrEmpty(strStoreRoom))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJFKBBNWKBC").ToString().Trim() + "')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strStoreRoom))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJFKBBNWFFZFC").ToString().Trim() + "')", true);
                return;
            }

            WZTurnBLL wZTurnBLL = new WZTurnBLL();

            if (!string.IsNullOrEmpty(HF_TurnCode.Value))
            {
                //修改
                string turnCode = HF_TurnCode.Value;

                string strWZTurnHQL = "from WZTurn as wZTurn where TurnCode = '" + turnCode + "'";
                IList listTurn = wZTurnBLL.GetAllWZTurns(strWZTurnHQL);
                if (listTurn != null && listTurn.Count > 0)
                {
                    WZTurn wZTurn = (WZTurn)listTurn[0];
                    wZTurn.ProjectCode = strProjectCode;
                    wZTurn.ProjectName = strProjectName;
                    wZTurn.UnitCode = strUnitCode;
                    wZTurn.PickingUnit = strPickingUnit;
                    wZTurn.MaterialPerson = strMaterialPerson;
                    wZTurn.CheckPerson = strCheckPerson;

                    //wZTurn.StoreRoom = DDL_StoreRoom.SelectedValue;
                    wZTurn.StoreRoom = strStoreRoom;

                    wZTurn.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();

                    wZTurnBLL.UpdateWZTurn(wZTurn, int.Parse(HF_ID.Value));

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('取消移交成功！');ControlStatus()", true); 

                    //重新加载列表
                    DataBinder();

                    BT_Edit.Enabled = true;
                    btnSave.Enabled = true;
                    btnReset.Enabled = true;
                    btnTurn.Enabled = true;
                }
            }

        }
        catch (Exception ex)
        { }
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




    private void DataPickingUnitBinder()
    {
        //WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
        //string strWZGetUnitHQL = "from WZGetUnit as wZGetUnit";
        //IList listWZGetUnit = wZGetUnitBLL.GetAllWZGetUnits(strWZGetUnitHQL);

        //DDL_PickingUnit.DataSource = listWZGetUnit;
        //DDL_PickingUnit.DataBind();

        //DDL_PickingUnit.Items.Insert(0, new ListItem("--Select--", ""));
    }


    protected void DDL_Project_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strProjectSelectedValue = DDL_Project.SelectedItem.Text;
        if (!string.IsNullOrEmpty(strProjectSelectedValue))
        {
            try
            {
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                string strProjectHQL = "from WZProject as wZProject where ProjectCode = '" + strProjectSelectedValue + "'";
                IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);
                if (listProject != null && listProject.Count > 0)
                {
                    WZProject wZProject = (WZProject)listProject[0];
                    TXT_ProjectName.Text = wZProject.ProjectName;
                    //验收人 = 工程项目《材检员》
                    HF_CheckPerson.Value = wZProject.Checker;

                }
            }
            catch (Exception ex) { }
        }
    }


    protected void DDL_PickingUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strUnitSelectedValue = TXT_UnitCode.Text.Trim(); //DDL_PickingUnit.SelectedValue;
        if (!string.IsNullOrEmpty(strUnitSelectedValue))
        {
            try
            {
                //依据<单位编号>从领料单位表单带入<材料员>
                //string strGetUnitHQL = "from WZGetUnit as wZGetUnit where UnitCode = '" + strUnitSelectedValue + "'";
                //WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
                //IList listGetUnit = wZGetUnitBLL.GetAllWZGetUnits(strGetUnitHQL);
                //if (listGetUnit != null && listGetUnit.Count > 0)
                //{
                //    WZGetUnit wZGetUnit = (WZGetUnit)listGetUnit[0];

                //    TXT_UnitCode.Text = wZGetUnit.UnitCode;
                //    TXT_MaterialPerson.Text = wZGetUnit.MaterialPerson;
                //}
                string strGetUnitHQL = string.Format(@"select u.*,m.UserName as MaterialPersonName from T_WZGetUnit u
                            left join T_ProjectMember m on u.MaterialPerson = m.UserCode 
                            where u.UnitCode = '{0}'", strUnitSelectedValue);

                DataTable dtGetUnit = ShareClass.GetDataSetFromSql(strGetUnitHQL, "GetUnit").Tables[0];
                if (dtGetUnit != null && dtGetUnit.Rows.Count > 0)
                {
                    DataRow drGetUnit = dtGetUnit.Rows[0];

                    TXT_UnitCode.Text = ShareClass.ObjectToString(drGetUnit["UnitCode"]);
                    HF_MaterialPerson.Value = ShareClass.ObjectToString(drGetUnit["MaterialPerson"]);
                    TXT_MaterialPerson.Text = ShareClass.ObjectToString(drGetUnit["MaterialPersonName"]);
                }
            }
            catch (Exception ex) { }
        }
    }




    protected void BT_Department_Click(object sender, EventArgs e)
    {
        string strDepartment = HF_Department.Value;
        if (!string.IsNullOrEmpty(strDepartment))
        {

            string strGetUnitHQL = string.Format(@"select * from T_DepartRelatedWZCheckUser d
                            where d.DepartCode = '{0}'", strDepartment);

            DataTable dtGetUnit = ShareClass.GetDataSetFromSql(strGetUnitHQL, "GetUnit").Tables[0];
            if (dtGetUnit != null && dtGetUnit.Rows.Count > 0)
            {
                DataRow drGetUnit = dtGetUnit.Rows[0];

                //TXT_UnitCode.Text = ShareClass.ObjectToString(drGetUnit["UnitCode"]);
                HF_MaterialPerson.Value = ShareClass.ObjectToString(drGetUnit["UserCode"]);
                TXT_MaterialPerson.Text = ShareClass.ObjectToString(drGetUnit["UserName"]);

            }
        }
    }



    private string CreateNewTurnCode()
    {
        string strNewTurnCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                string strTurnCodeHQL = string.Format("select count(1) as RowNumber from T_WZTurn where  to_char(TurnTime,'yyyy-mm-dd')  between '{0}' and '{1}'", DateTime.Now.ToString("yyyy-MM-01"), DateTime.Now.ToString("yyyy-MM-31"));
                DataTable dtTurnCode = ShareClass.GetDataSetFromSql(strTurnCodeHQL, "strTurnCodeHQL").Tables[0];
                int intTurnCodeNumber = int.Parse(dtTurnCode.Rows[0]["RowNumber"].ToString());
                intTurnCodeNumber = intTurnCodeNumber + 1;
                do
                {
                    string strYear = DateTime.Now.Year.ToString();
                    string strMonth = DateTime.Now.Month.ToString();
                    if (strMonth.Length == 1)
                    {
                        strMonth = "0" + strMonth;
                    }
                    StringBuilder sbTurnCode = new StringBuilder();
                    for (int j = 4 - intTurnCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbTurnCode.Append("0");
                    }
                    strNewTurnCode = strYear + "" + strMonth  + sbTurnCode.ToString() + intTurnCodeNumber.ToString();

                    //验证新的移交编号是否存在
                    string strCheckNewTurnCodeHQL = "select count(1) as RowNumber from T_WZTurn where TurnCode = '" + strNewTurnCode + "'";
                    DataTable dtCheckNewTurnCode = ShareClass.GetDataSetFromSql(strCheckNewTurnCodeHQL, "NewTurnCodeHQL").Tables[0];
                    int intCheckNewTurnCode = int.Parse(dtCheckNewTurnCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewTurnCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intTurnCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }

        return strNewTurnCode;
    }


}
