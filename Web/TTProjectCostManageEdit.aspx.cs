using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTProjectCostManageEdit : System.Web.UI.Page
{
    string strUserCode, strProjectID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strProjectID = Request.QueryString["ProjectID"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            LoadProjectCostManageList(strProjectID);
        }
    }

    protected void LoadProjectCostManageList(string strProjectID)
    {
        string strHQL = "Select * From T_ProjectCostManage Where ProjectID='" + strProjectID + "' and Type='Base' ";

        if (!string.IsNullOrEmpty(TB_RemarkQuery.Text.Trim()))
        {
            strHQL += " and (Remark like '%" + TB_RemarkQuery.Text.Trim() + "%' or Code like '%" + TB_RemarkQuery.Text.Trim() + "%' or Name like '%" + TB_RemarkQuery.Text.Trim() + "%' " +
                "or Quantities like '%" + TB_RemarkQuery.Text.Trim() + "%' or Price like '%" + TB_RemarkQuery.Text.Trim() + "%' or Unit like '%" + TB_RemarkQuery.Text.Trim() + "%' or " +
                "Total like '%" + TB_RemarkQuery.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCostManage");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }


    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddCost();
        }
        else
        {
            UpdateCost();
        }
    }

    protected void AddCost()
    {
        if (string.IsNullOrEmpty(TB_Code.Text) || TB_Code.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSBMBNWKJC")+"')", true);
            TB_Code.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        if (string.IsNullOrEmpty(TB_Name.Text) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoChengBenLeiXingBuNeng")+"')", true);
            TB_Name.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        if (IsProjectCostManageExits(string.Empty, TB_Code.Text.Trim(), TB_Name.Text.Trim(), strProjectID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGEDXMYCZJC")+"')", true);
            TB_Code.Focus();
            TB_Name.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        ProjectCostManageBLL projectCostManageBLL = new ProjectCostManageBLL();
        ProjectCostManage projectCostManage = new ProjectCostManage();
        projectCostManage.Code = TB_Code.Text.Trim();
        projectCostManage.Creater = strUserCode.Trim();
        projectCostManage.Name = TB_Name.Text.Trim();
        projectCostManage.Price = NB_Price.Amount;
        projectCostManage.ProjectID = int.Parse(strProjectID);
        projectCostManage.Quantities = NB_Quantities.Amount;
        projectCostManage.Remark = TB_Remark.Text.Trim();
        NB_Total.Amount = projectCostManage.Quantities * projectCostManage.Price;
        projectCostManage.Total = NB_Total.Amount;
        projectCostManage.Type = "Base";
        projectCostManage.Unit = TB_Unit.Text.Trim();
        try
        {
            projectCostManageBLL.AddProjectCostManage(projectCostManage);
            LB_ID.Text = GetMaxProjectCostManageID(projectCostManage).ToString();

            AddBDBaseData(LB_ID.Text.Trim(), projectCostManage.Total);

            LoadProjectCostManageList(projectCostManage.ProjectID.ToString());

        

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB")+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    /// <summary>
    /// 新增时，获取表T_ProjectCostManage中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxProjectCostManageID(ProjectCostManage bmbf)
    {
        string strHQL = "Select ID From T_ProjectCostManage where Creater='" + bmbf.Creater.Trim() + "' and ProjectID='" + bmbf.ProjectID.ToString() + "' and Type='Base' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCostManage").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 存在，则返回true；否则，返回false
    /// </summary>
    /// <param name="strID"></param>
    /// <param name="strCode"></param>
    /// <param name="strName"></param>
    /// <param name="strprojectID"></param>
    /// <returns></returns>
    protected bool IsProjectCostManageExits(string strID, string strCode, string strName,string strprojectID)
    {
        bool flag = true;
        ProjectCostManageBLL projectCostManageBLL = new ProjectCostManageBLL();
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "from ProjectCostManage as projectCostManage where projectCostManage.Code = '" + strCode + "' and projectCostManage.Name='" + strName + "' and projectCostManage.ProjectID='" + strprojectID + "' and projectCostManage.Type='Base' ";
        }
        else
        {
            strHQL = "from ProjectCostManage as projectCostManage where projectCostManage.Code = '" + strCode + "' and projectCostManage.Name='" + strName + "' and projectCostManage.ProjectID='" + strprojectID + "' and projectCostManage.Type='Base' and projectCostManage.ID<>'" + strID + "' ";
        }
        IList lst = projectCostManageBLL.GetAllProjectCostManages(strHQL);
        if (lst.Count > 0 && lst != null)
            flag = true;
        else
            flag = false;
        return flag;
    }

    protected void AddBDBaseData(string strprocostID, decimal strTotal)
    {
        BDBaseDataBLL bMBaseDataBLL = new BDBaseDataBLL();
        BDBaseData bMBaseData = new BDBaseData();

        bMBaseData.AccountName = "";
        bMBaseData.DepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode.Trim());
        bMBaseData.DepartName = ShareClass.GetDepartName(bMBaseData.DepartCode.Trim());
        bMBaseData.MoneyNum = strTotal;
        bMBaseData.MonthNum = 0;
        bMBaseData.YearNum = DateTime.Now.Year;
        bMBaseData.EnterCode = strUserCode.Trim();
        bMBaseData.Type = "Budget";
        bMBaseData.ProjectCostID = int.Parse(strprocostID);

        bMBaseDataBLL.AddBDBaseData(bMBaseData);
    }

    protected void UpdateBDBaseData(string strprocostID, decimal strTotal)
    {
        string strHQL = "From BDBaseData as bDBaseData where bDBaseData.ProjectCostID = '" + strprocostID + "'";
        BDBaseDataBLL bMBaseDataBLL = new BDBaseDataBLL();
        IList lst = bMBaseDataBLL.GetAllBDBaseDatas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BDBaseData bMBaseData = (BDBaseData)lst[0];

            bMBaseData.MoneyNum = strTotal;
            bMBaseData.YearNum = DateTime.Now.Year;

            bMBaseDataBLL.UpdateBDBaseData(bMBaseData, bMBaseData.ID);
        }
    }

    protected void UpdateCost()
    {
        if (string.IsNullOrEmpty(TB_Code.Text) || TB_Code.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSBMBNWKJC")+"')", true);
            TB_Code.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        if (string.IsNullOrEmpty(TB_Name.Text) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoChengBenLeiXingBuNeng")+"')", true);
            TB_Name.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        if (IsProjectCostManageExits(LB_ID.Text.Trim(), TB_Code.Text.Trim(), TB_Name.Text.Trim(), strProjectID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGEDXMYCZJC")+"')", true);
            TB_Code.Focus();
            TB_Name.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        ProjectCostManageBLL projectCostManageBLL = new ProjectCostManageBLL();
        string strHQL = "from ProjectCostManage as projectCostManage where projectCostManage.ID = '" + LB_ID.Text.Trim() + "' ";
        IList lst = projectCostManageBLL.GetAllProjectCostManages(strHQL);
        ProjectCostManage projectCostManage = (ProjectCostManage)lst[0];
        projectCostManage.Code = TB_Code.Text.Trim();
        projectCostManage.Name = TB_Name.Text.Trim();
        projectCostManage.Price = NB_Price.Amount;
        projectCostManage.Quantities = NB_Quantities.Amount;
        projectCostManage.Remark = TB_Remark.Text.Trim();
        NB_Total.Amount = projectCostManage.Quantities * projectCostManage.Price;
        projectCostManage.Total = NB_Total.Amount;
        projectCostManage.Unit = TB_Unit.Text.Trim();

        try
        {
            projectCostManageBLL.UpdateProjectCostManage(projectCostManage, projectCostManage.ID);

            UpdateBDBaseData(projectCostManage.ID.ToString(), projectCostManage.Total);

            LoadProjectCostManageList(projectCostManage.ProjectID.ToString());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB")+"')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadProjectCostManageList(strProjectID);
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID;

            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strHQL = " from ProjectCostManage as projectCostManage where projectCostManage.ID = '" + strID + "' ";

                ProjectCostManageBLL projectCostManageBLL = new ProjectCostManageBLL();
                IList lst = projectCostManageBLL.GetAllProjectCostManages(strHQL);
                ProjectCostManage projectCostManage = (ProjectCostManage)lst[0];

                LB_ID.Text = projectCostManage.ID.ToString();
                TB_Code.Text = projectCostManage.Code.Trim();
                TB_Name.Text = projectCostManage.Name.Trim();
                TB_Remark.Text = projectCostManage.Remark.Trim();
                TB_Unit.Text = projectCostManage.Unit.Trim();
                NB_Price.Amount = projectCostManage.Price;
                NB_Quantities.Amount = projectCostManage.Quantities;
                NB_Total.Amount = projectCostManage.Total;

                if (projectCostManage.Creater.Trim() == strUserCode.Trim())
                {
                    //BT_Update.Visible = true;
                    //BT_Delete.Visible = true;
                    //BT_Update.Enabled = true;
                    //BT_Delete.Enabled = true;
                }
                else
                {
                    //BT_Update.Visible = false;
                    //BT_Delete.Visible = false;
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strHQL = "delete from T_ProjectCostManage where ID = " + strID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "delete from T_BDBaseData where ProjectCostID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);

                    LoadProjectCostManageList(strProjectID);

                

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSB") + "')", true);
                }
            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = lbl_sql.Text.Trim();

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCostManage");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected string GetUserName(string usercode)
    {
        return ShareClass.GetUserName(usercode.Trim());
    }
}