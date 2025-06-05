using System;
using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTKPITemplateSetForDepartPosition : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;

        strUserCode = Session["UserCode"].ToString();
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();

        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "职称KPI模板设置", strUserCode);

        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "职称KPI模板设置---" + System.Configuration.ConfigurationManager.AppSettings["SystemName"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LoadKPIType();

            TakeTopCore.CoreShareClass.InitialDepartmentPositionTreeByUserInfor(LanguageHandle.GetWord("ZZJGT"),TreeView1, strUserCode);

            ShareClass.InitialKPITree(TreeView2);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strPosition;
        string strUserCode = Session["UserCode"].ToString();

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            if (treeNode.ChildNodes.Count == 0)
            {
                strDepartCode = treeNode.Parent.Target.Trim();
                strPosition = treeNode.Text.Trim();

                LB_DepartCode.Text = strDepartCode;
                LB_Position.Text = strPosition;

                LoadKPI(strDepartCode, strPosition);
            }
            else
            {
                try
                {
                    strDepartCode = treeNode.Target.Trim();
                }
                catch
                {

                }
            }
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;

        string strKPIID, strKPI;
        string strDepartCode, strPosition;

        strDepartCode = LB_DepartCode.Text.Trim();
        strPosition = LB_Position.Text.Trim();

        if (strDepartCode != "" & strPosition != "")
        {
            TreeNode treeNode = new TreeNode();
            treeNode = TreeView2.SelectedNode;

            if (treeNode.Depth == 2)
            {
                strKPIID = treeNode.Target.Trim();
                strKPI = treeNode.Text.Trim();

                strHQL = "Insert Into T_KPITemplateForDepartPosition(Departcode,Position,SortNumber,KPI,KPIType,Definition,KPIFunction,Formula,SqlCode,UnitSqlPoint,Source,Weight)";
                strHQL += " Select " + "'" + strDepartCode + "'" + "," + "'" + strPosition + "'" + ",SortNumber,KPI,KPIType,Definition,KPIFunction,Formula,SqlCode,UnitSqlPoint,Source,0 From T_KPILibrary Where ID = " + strKPIID;
                strHQL += " and KPI not in (Select KPI From T_KPITemplateForDepartPosition Where DepartCode = " + "'" + strDepartCode + "'" + " and Position = " + "'" + strPosition + "'" + ")";

                ShareClass.RunSqlCommand(strHQL);

                LoadKPI(strDepartCode, strPosition);
            }
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            string strDepartCode = LB_DepartCode.Text.Trim();
            string strPosition = LB_Position.Text.Trim();

            string strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                strHQL = "From KPITemplateForDepartPosition as kpiTemplateForDepartPosition Where kpiTemplateForDepartPosition.ID = " + strID;
                KPITemplateForDepartPositionBLL kpiTemplateForDepartPositionBLL = new KPITemplateForDepartPositionBLL();
                lst = kpiTemplateForDepartPositionBLL.GetAllKPITemplateForDepartPositions(strHQL);

                KPITemplateForDepartPosition kpiTemplateForDepartPosition = (KPITemplateForDepartPosition)lst[0];

                LB_KPIID.Text = strID;
                TB_KPI.Text = kpiTemplateForDepartPosition.KPI.Trim();
                DL_KPIType.SelectedValue = kpiTemplateForDepartPosition.KPIType;
                TB_Definition.Text = kpiTemplateForDepartPosition.Definition.Trim();
                NB_Weight.Amount = kpiTemplateForDepartPosition.Weight;
                TB_Formula.Text = kpiTemplateForDepartPosition.Formula.Trim();
                TB_SqlCode.Text = kpiTemplateForDepartPosition.SqlCode.Trim();
                NB_UnitSqlPoint.Amount = kpiTemplateForDepartPosition.UnitSqlPoint;
                TB_Source.Text = kpiTemplateForDepartPosition.Source.Trim();
                NB_SortNubmer.Amount = kpiTemplateForDepartPosition.SortNumber;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                strDepartCode = LB_DepartCode.Text.Trim();
                strPosition = LB_Position.Text.Trim();

                strHQL = "Delete From T_KPITemplateForDepartPosition Where ID = " + strID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    LoadKPI(strDepartCode, strPosition);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;
        IList lst;

        KPITemplateForDepartPositionBLL kpiTemplateForDepartPositionBLL = new KPITemplateForDepartPositionBLL();
        lst = kpiTemplateForDepartPositionBLL.GetAllKPITemplateForDepartPositions(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_KPIID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strKPIID;

        strKPIID = LB_KPIID.Text.Trim();

        if (strKPIID == "")
        {
            AddKPI();
        }
        else
        {
            UpdateKPI();
        }
    }

    protected void AddKPI()
    {
        string strDepartCode, strPosition;
        string strKPIID, strKPI, strKPIType, strDefinition, strSqlCode, strFormula, strSource;
        int intSortNumber;
        decimal deWeight;

        strKPI = TB_KPI.Text.Trim();
        strKPIType = DL_KPIType.SelectedValue.Trim();
        strDefinition = TB_Definition.Text.Trim();
        strFormula = TB_Formula.Text.Trim();
        strSqlCode = TB_SqlCode.Text.Trim();
        strSource = TB_Source.Text.Trim();
        deWeight = NB_Weight.Amount;
        if (deWeight > 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBNDY1JC") + "')", true);
            return;
        }

        if (strSqlCode.ToLower().Contains("delete") || strSqlCode.ToLower().Contains("update") || strSqlCode.ToLower().Contains("drop")
           || strSqlCode.ToLower().Contains("insert") || strSqlCode.ToLower().Contains("alter"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSQLDMBNHDELETEUPDATEDROPINSERTALTERYJJC") + "')", true);
            return;
        }

        intSortNumber = int.Parse(NB_SortNubmer.Amount.ToString());

        strDepartCode = LB_DepartCode.Text.Trim();
        strPosition = LB_Position.Text.Trim();

        if (strKPI != "" | strKPIType != "" | strDefinition != "" | strDepartCode != "" | strPosition != "")
        {
            KPITemplateForDepartPositionBLL kpiTemplateForDepartPositionBLL = new KPITemplateForDepartPositionBLL();
            KPITemplateForDepartPosition kpiTemplateForDepartPosition = new KPITemplateForDepartPosition();

            try
            {
                kpiTemplateForDepartPosition.KPI = strKPI;
                kpiTemplateForDepartPosition.KPIType = strKPIType;
                kpiTemplateForDepartPosition.Definition = strDefinition;
                kpiTemplateForDepartPosition.Formula = strFormula;
                kpiTemplateForDepartPosition.SqlCode = strSqlCode;
                kpiTemplateForDepartPosition.UnitSqlPoint = NB_UnitSqlPoint.Amount;

                kpiTemplateForDepartPosition.Source = strSource;
                kpiTemplateForDepartPosition.SortNumber = intSortNumber;
                kpiTemplateForDepartPosition.Weight = deWeight;

                kpiTemplateForDepartPosition.DepartCode = strDepartCode;
                kpiTemplateForDepartPosition.Position = strPosition;


                kpiTemplateForDepartPositionBLL.AddKPITemplateForDepartPosition(kpiTemplateForDepartPosition);

                strKPIID = ShareClass.GetMyCreatedMaxKPIDepartPositionTemplateID();
                LB_KPIID.Text = strKPIID;

                LoadKPI(strDepartCode, strPosition);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDXBNWKJC") + "')", true);
        }
    }


    protected void UpdateKPI()
    {
        string strDepartCode, strPosition;
        string strKPIID, strKPI, strKPIType, strDefinition, strSqlCode, strFormula, strSource;
        int intSortNumber;
        decimal deWeight;

        strKPIID = LB_KPIID.Text.Trim();
        strKPI = TB_KPI.Text.Trim();
        strKPIType = DL_KPIType.SelectedValue.Trim();
        strDefinition = TB_Definition.Text.Trim();
        strFormula = TB_Formula.Text.Trim();
        strSqlCode = TB_SqlCode.Text.Trim();

        strSource = TB_Source.Text.Trim();

        deWeight = NB_Weight.Amount;
        if (deWeight > 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBNDY1JC") + "')", true);
            return;
        }

        if (strSqlCode.ToLower().Contains("create") || strSqlCode.ToLower().Contains("execute") || strSqlCode.ToLower().Contains("delete") || strSqlCode.ToLower().Contains("update") || strSqlCode.ToLower().Contains("drop")
           || strSqlCode.ToLower().Contains("insert") || strSqlCode.ToLower().Contains("alter"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSQLDMBNHDELETEUPDATEDROPINSERTALTERYJJC") + "')", true);
            return;
        }

        intSortNumber = int.Parse(NB_SortNubmer.Amount.ToString());

        strDepartCode = LB_DepartCode.Text.Trim();
        strPosition = LB_Position.Text.Trim();

        if (strKPI != "" | strKPIType != "" | strDefinition != "")
        {
            KPITemplateForDepartPositionBLL kpiTemplateForDepartPositionBLL = new KPITemplateForDepartPositionBLL();
            KPITemplateForDepartPosition kpiTemplateForDepartPosition = new KPITemplateForDepartPosition();

            try
            {
                kpiTemplateForDepartPosition.KPI = strKPI;
                kpiTemplateForDepartPosition.KPIType = strKPIType;
                kpiTemplateForDepartPosition.Definition = strDefinition;

                kpiTemplateForDepartPosition.Formula = strFormula;
                kpiTemplateForDepartPosition.SqlCode = strSqlCode;
                kpiTemplateForDepartPosition.UnitSqlPoint = NB_UnitSqlPoint.Amount;

                kpiTemplateForDepartPosition.Source = strSource;
                kpiTemplateForDepartPosition.SortNumber = intSortNumber;
                kpiTemplateForDepartPosition.Weight = deWeight;

                kpiTemplateForDepartPosition.DepartCode = strDepartCode;
                kpiTemplateForDepartPosition.Position = strPosition;

                kpiTemplateForDepartPositionBLL.UpdateKPITemplateForDepartPosition(kpiTemplateForDepartPosition, int.Parse(strKPIID));

                LoadKPI(strDepartCode, strPosition);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDXBNWKJC") + "')", true);
        }
    }


    protected void LoadKPI(string strDepartCode, string strPosition)
    {
        string strHQL;
        IList lst;

        strHQL = "From KPITemplateForDepartPosition as kpiTemplateForDepartPosition where kpiTemplateForDepartPosition.DepartCode = " + "'" + strDepartCode + "'" + " and kpiTemplateForDepartPosition.Position = " + "'" + strPosition + "'";
        strHQL += " Order by kpiTemplateForDepartPosition.ID ASC";

        KPITemplateForDepartPositionBLL kpiTemplateForDepartPositionBLL = new KPITemplateForDepartPositionBLL();
        lst = kpiTemplateForDepartPositionBLL.GetAllKPITemplateForDepartPositions(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;

        LB_Weight.Text = SumKPIWeight(strDepartCode, strPosition); ;
    }

    protected string SumKPIWeight(string strDepartCode, string strPosition)
    {
        string strHQL;
        strHQL = "Select Sum(Weight) From T_KPITemplateForDepartPosition where DepartCode = " + "'" + strDepartCode + "'" + " and Position = " + "'" + strPosition + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "_KPITemplateForDepartPosition");

        if(ds.Tables[0].Rows.Count> 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    protected void LoadKPIType()
    {
        string strHQL;
        IList lst;

        strHQL = "From KPIType as kpiType Order By kpiType.SortNumber ASC";
        KPITypeBLL kpiTypeBLL = new KPITypeBLL();
        lst = kpiTypeBLL.GetAllKPITypes(strHQL);

        DL_KPIType.DataSource = lst;
        DL_KPIType.DataBind();
    }

}
