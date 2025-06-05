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


using System.Data.SqlClient;

using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopCore;

public partial class TTGoodsMRPMainPlan : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserName;
        strUserCode = Session["UserCode"].ToString();

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "资产登记入库", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_PlanStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_DeliveryDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityAsset(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            ShareClass.LoadUnitForDropDownList(DL_Unit);

            strHQL = "from GoodsType as goodsType Order by goodsType.SortNumber ASC";
            GoodsTypeBLL goodsTypeBLL = new GoodsTypeBLL();
            lst = goodsTypeBLL.GetAllGoodsTypes(strHQL);
            DL_GoodsType.DataSource = lst;
            DL_GoodsType.DataBind();
            DL_GoodsType.Items.Insert(0, new ListItem("--Select--", ""));

            LB_CreatorCode.Text = strUserCode;
            TB_CreatorName.Text = strUserName;

            LoadItemMainPlan(strUserCode);
            LoadGoodsApplication(strUserCode);
            LoadGoodsSaleOrder(strUserCode);
            LoadProject(strUserCode);

            LoadUsingConstract(strUserCode);
        }
    }

    protected void DL_SourceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strSourceType;

        strSourceType = DL_SourceType.SelectedValue.Trim();

        if (strSourceType == "Other")
        {
            TabContainer1.ActiveTabIndex = 0;
            NB_SourceID.Amount = 0;
        }

        if (strSourceType == "GoodsAO")
        {
            TabContainer1.ActiveTabIndex = 2;
            BT_SelectAO.Visible = true;
            NB_SourceID.Amount = 0;
        }
        else
        {
            BT_SelectAO.Visible = false;
        }

        if (strSourceType == "GoodsSO")
        {
            TabContainer1.ActiveTabIndex = 3;
            BT_SelectSO.Visible = true;
            NB_SourceID.Amount = 0;
        }
        else
        {
            BT_SelectSO.Visible = false;
        }

        if (strSourceType == "GoodsPJ")
        {
            TabContainer1.ActiveTabIndex = 4;
            BT_SelectPJ.Visible = true;
            NB_SourceID.Amount = 0;
        }
        else
        {
            BT_SelectPJ.Visible = false;
        }

        if (strSourceType == "Contract")
        {
            NB_SourceID.Amount = 0;
            BT_SelectCS.Visible = true;
            TabContainer1.ActiveTabIndex = 10;
        }
        else
        {
            BT_SelectCS.Visible = false;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;

            string strPlanVerID;

            strPlanVerID = e.Item.Cells[2].Text.Trim();
            LB_PlanVerID.Text = strPlanVerID;

            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                DataGrid5.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Update")
            {
                strHQL = "From ItemMainPlan as itemMainPlan Where PlanVerID = " + strPlanVerID;
                ItemMainPlanBLL itemMainPlanBLL = new ItemMainPlanBLL();
                IList lst = itemMainPlanBLL.GetAllItemMainPlans(strHQL);
                ItemMainPlan itemMainPlan = (ItemMainPlan)lst[0];

                LB_PlanVerID.Text = itemMainPlan.PlanVerID.ToString();
                DL_PlanType.SelectedValue = itemMainPlan.PlanType.Trim();
                TB_PlanVerName.Text = itemMainPlan.PlanVerName;
                LB_BelongDepartCode.Text = itemMainPlan.BelongDepartCode;
                TB_BelongDepartName.Text = itemMainPlan.BelongDepartName;
                TB_CreatorName.Text = itemMainPlan.CreatorName;
                LB_CreatorCode.Text = itemMainPlan.CreatorCode;
                LB_CreateTime.Text = itemMainPlan.CreateTime.ToShortDateString();
                DL_Status.SelectedValue = itemMainPlan.Status.Trim();

                LoadItemMainPlanDetail(strPlanVerID);

                //BT_UpdateMainPlan.Enabled = true;
                //BT_DeleteMainPlan.Enabled = true;

                //BT_Update.Enabled = false;
                //BT_Delete.Enabled = false;
                //BT_New.Enabled = true;

                BT_CopyDetailFromPlan.Enabled = true;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                try
                {
                    strHQL = "Delete From T_ItemMainPlan Where PlanVerID = " + strPlanVerID;
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_ItemMainPlanDetail Where PlanVerID = " + strPlanVerID;
                    ShareClass.RunSqlCommand(strHQL);

                    LoadItemMainPlan(strUserCode);
                    LoadItemMainPlanDetail("0");

                    BT_CopyDetailFromPlan.Enabled = false;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC") + "')", true);
                }
            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql2.Text;

        GoodsBLL goodsBLL = new GoodsBLL();
        IList lst = goodsBLL.GetAllGoodss(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlan");

        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strAAID;

        if (e.CommandName != "Page")
        {
            strAAID = ((Button)e.Item.FindControl("BT_AAID")).Text.Trim();

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            LB_AOID.Text = strAAID;

            NB_SourceID.Amount = int.Parse(strAAID);

            LoadGoodsApplicationDetail(strAAID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid6_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strSOID;

        if (e.CommandName != "Page")
        {
            strSOID = ((Button)e.Item.FindControl("BT_SOID")).Text.Trim();

            for (int i = 0; i < DataGrid6.Items.Count; i++)
            {
                DataGrid6.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            LB_SOID.Text = strSOID;

            NB_SourceID.Amount = int.Parse(strSOID);

            LoadGoodsSaleOrderDetail(strSOID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }
    }

    protected void DataGrid8_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strProjectID;
        string strUserName;

        if (e.CommandName != "Page")
        {
            strUserName = LB_UserName.Text.Trim();

            strProjectID = ((Button)e.Item.FindControl("BT_ProjectID")).Text;

            for (int i = 0; i < DataGrid8.Items.Count; i++)
            {
                DataGrid8.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            LB_ProjectID.Text = strProjectID;
            NB_SourceID.Amount = int.Parse(strProjectID);

            LoadProjectRelatedItem(strProjectID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }



    protected void DataGrid23_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strConstractCode;

            strConstractCode = ((Button)e.Item.FindControl("BT_ConstractCode")).Text.Trim();

            LB_ConstractCode.Text = strConstractCode;

            LoadConstractRelatedGoodsList(strConstractCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql3.Text;

        GoodsApplicationBLL goodsApplicationBLL = new GoodsApplicationBLL();
        IList lst = goodsApplicationBLL.GetAllGoodsApplications(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }


    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            LB_BelongDepartCode.Text = strDepartCode;
            TB_BelongDepartName.Text = ShareClass.GetDepartName(strDepartCode);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }


    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }


    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_PlanVerID.Text = "";

        BT_NewMain.Visible = true;
        BT_NewDetail.Visible = true;

        LoadItemMainPlanDetail("0");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_NewMain_Click(object sender, EventArgs e)
    {
        string strPlanVerID;

        strPlanVerID = LB_PlanVerID.Text.Trim();

        if (strPlanVerID == "")
        {
            AddMain();
        }
        else
        {
            UpdateMain();
        }
    }

    protected void AddMain()
    {
        string strPlanVerID, strPlanType, strPlanVerName, strBlongDepartName, strBlongDepartCode, strCreatorCode, strCreatorName, strStatus, strSourceType;
        DateTime dtCreateTime;
        int intSourceID;

        strPlanType = DL_PlanType.SelectedValue.Trim();
        strPlanVerName = TB_PlanVerName.Text.Trim();
        strBlongDepartCode = LB_BelongDepartCode.Text.Trim();
        strBlongDepartName = TB_BelongDepartName.Text.Trim();
        strCreatorCode = LB_CreatorCode.Text.Trim();
        strCreatorName = TB_CreatorName.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();
        dtCreateTime = DateTime.Now;
        strSourceType = DL_SourceType.SelectedValue.Trim();
        intSourceID = int.Parse(NB_SourceID.Amount.ToString());

        LB_CreateTime.Text = dtCreateTime.ToString("yyyy-MM-dd");

        if (strBlongDepartName == "" & strBlongDepartCode == "" & strPlanVerName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGJHMCGSBMBNWKJC") + "')", true);
            return;
        }

        try
        {
            ItemMainPlanBLL itemMainPlanBLL = new ItemMainPlanBLL();
            ItemMainPlan itemMainPlan = new ItemMainPlan();

            itemMainPlan.PlanType = strPlanType;
            itemMainPlan.PlanVerName = strPlanVerName;
            itemMainPlan.BelongDepartCode = strBlongDepartCode;
            itemMainPlan.BelongDepartName = strBlongDepartName;
            itemMainPlan.CreateTime = DateTime.Now;
            itemMainPlan.CreatorCode = strCreatorCode;
            itemMainPlan.CreatorName = strCreatorName;
            itemMainPlan.Status = strStatus;

            itemMainPlanBLL.AddItemMainPlan(itemMainPlan);

            strPlanVerID = ShareClass.GetMyCreatedMaxGoodsMrpMainPlanVerID(strUserCode).ToString();
            LB_PlanVerID.Text = strPlanVerID;

            LB_CreateTime.Text = DateTime.Now.ToString();

            LoadItemMainPlan(strUserCode);

            //BT_UpdateMainPlan.Enabled = true;
            //BT_DeleteMainPlan.Enabled = true;

            //BT_New.Enabled = true;
            BT_CopyDetailFromPlan.Enabled = true;

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCCKNSCMCZD50GHZHBZZSZD100GHZGDJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    protected void UpdateMain()
    {
        string strHQL;
        IList lst;

        string strPlanVerID, strPlanType, strPlanVerName, strBlongDepartName, strBlongDepartCode, strCreatorCode, strCreatorName, strStatus, strSourceType;
        DateTime dtCreateTime;
        int intSourceID;

        strPlanVerID = LB_PlanVerID.Text.Trim();
        strPlanType = DL_PlanType.SelectedValue.Trim();
        strPlanVerName = TB_PlanVerName.Text.Trim();
        strBlongDepartCode = LB_BelongDepartCode.Text.Trim();
        strBlongDepartName = TB_BelongDepartName.Text.Trim();
        strCreatorCode = LB_CreatorCode.Text.Trim();
        strCreatorName = TB_CreatorName.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();
        dtCreateTime = DateTime.Now;
        strSourceType = DL_SourceType.SelectedValue.Trim();
        intSourceID = int.Parse(NB_SourceID.Amount.ToString());

        if (strBlongDepartName == "" & strBlongDepartCode == "" & strPlanVerName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGJHMCGSBMBNWKJC") + "')", true);
            return;
        }

        try
        {
            strHQL = "From ItemMainPlan as itemMainPlan Where itemMainPlan.PlanVerID = " + strPlanVerID;
            ItemMainPlanBLL itemMainPlanBLL = new ItemMainPlanBLL();
            lst = itemMainPlanBLL.GetAllItemMainPlans(strHQL);

            ItemMainPlan itemMainPlan = (ItemMainPlan)lst[0];

            itemMainPlan.PlanType = strPlanType;
            itemMainPlan.PlanVerName = strPlanVerName;
            itemMainPlan.BelongDepartCode = strBlongDepartCode;
            itemMainPlan.BelongDepartName = strBlongDepartName;
            itemMainPlan.Status = strStatus;

            itemMainPlanBLL.UpdateItemMainPlan(itemMainPlan, int.Parse(strPlanVerID));

            LoadItemMainPlan(strUserCode);


        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCCKNSCMCZD50GHZHBZZSZD100GHZGDJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }


    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID;

            strID = e.Item.Cells[2].Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Update")
            {
                strHQL = "From ItemMainPlanDetail as itemMainPlanDetail Where itemMainPlanDetail.ID = " + strID;
                ItemMainPlanDetailBLL itemMainPlanDetailBLL = new ItemMainPlanDetailBLL();
                lst = itemMainPlanDetailBLL.GetAllItemMainPlanDetails(strHQL);

                ItemMainPlanDetail itemMainPlanDetail = (ItemMainPlanDetail)lst[0];

                LoadItemBomVersion(itemMainPlanDetail.ItemCode.Trim(), DL_BomVerID);

                LB_ID.Text = strID;
                TB_GoodsCode.Text = itemMainPlanDetail.ItemCode.Trim();
                TB_GoodsName.Text = itemMainPlanDetail.ItemName.Trim();

                try
                {
                    DL_GoodsType.SelectedValue = itemMainPlanDetail.Type;
                }
                catch
                {
                    DL_GoodsType.SelectedValue = "";
                }


                TB_ModelNumber.Text = itemMainPlanDetail.ModelNumber.Trim();
                TB_Spec.Text = itemMainPlanDetail.Specification.Trim();
                TB_Brand.Text = itemMainPlanDetail.Brand;

                NB_PlanNumber.Amount = itemMainPlanDetail.PlanNumber;
                NB_FinishedNUmber.Amount = itemMainPlanDetail.FinishedNumber;

                DL_Unit.SelectedValue = itemMainPlanDetail.Unit;
                DL_BomVerID.SelectedValue = itemMainPlanDetail.BomVerID.ToString();

                DLC_PlanStartDate.Text = itemMainPlanDetail.PlanStartDate.ToString("yyyy-MM-dd");

                DLC_DeliveryDate.Text = itemMainPlanDetail.DeliveryDate.ToString("yyyy-MM-dd");

                DL_RecordSourceType.SelectedValue = itemMainPlanDetail.SourceType.Trim();
                NB_RecordSourceID.Amount = itemMainPlanDetail.SourceRecordID;

                //BT_New.Enabled = true;
                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;

                string strItemCode = TB_GoodsCode.Text.Trim();
                string strVerID = DL_BomVerID.SelectedValue.Trim();
                if (strVerID != "0")
                {
                    try
                    {
                        TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView2);
                    }
                    catch
                    {

                    }
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }

            if (e.CommandName == "BOM")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From ItemMainPlanDetail as itemMainPlanDetail Where itemMainPlanDetail.ID = " + strID;
                ItemMainPlanDetailBLL itemMainPlanDetailBLL = new ItemMainPlanDetailBLL();
                lst = itemMainPlanDetailBLL.GetAllItemMainPlanDetails(strHQL);
                ItemMainPlanDetail itemMainPlanDetail = (ItemMainPlanDetail)lst[0];

                string strItemCode, strVerID;

                strItemCode = itemMainPlanDetail.ItemCode.Trim();
                strVerID = itemMainPlanDetail.BomVerID.ToString();
                if (strVerID != "0")
                {
                    TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView3);
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popBOMWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                try
                {
                    strHQL = "Delete From T_ItemMainPlanDetail Where ID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);

                    LoadItemMainPlanDetail(LB_PlanVerID.Text.Trim());

                    //BT_Update.Enabled = false;
                    //BT_Delete.Enabled = false;

                    LB_ID.Text = "0";

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
                }
            }
        }
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popBOMWindow') ", true);
    }

    protected void DL_RecordSourceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        NB_RecordSourceID.Amount = 0;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strGoodsCode, strGoodsName, strSpec;
        string strDepartString;

        TabContainer1.ActiveTabIndex = 0;

        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();

        strSpec = TB_Spec.Text.Trim();

        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";

        strSpec = "%" + strSpec + "%";

        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "Select * From T_Goods  Where GoodsCode Like " + "'" + strGoodsCode + "'" + " and GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and  Spec Like " + "'" + strSpec + "'";
        strHQL += " and Number > 0";

        strHQL += " and Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
        strHQL += " Order by Number DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        strHQL = "Select * From T_Item as item Where item.ItemCode Like " + "'" + strGoodsCode + "'" + " and item.ItemName like " + "'" + strGoodsName + "'";
        strHQL += " and item.Specification Like " + "'" + strSpec + "'";
        strHQL += " and item.BigType = 'Goods'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_Item");
        DataGrid9.DataSource = ds;
        DataGrid9.DataBind();
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        TB_GoodsCode.Text = "";
        TB_GoodsName.Text = "";

        TB_Spec.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DL_BomVerID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strItemCode, strVerID;

        strItemCode = TB_GoodsCode.Text.Trim();
        strVerID = DL_BomVerID.SelectedValue.Trim();
        if (strVerID != "0")
        {
            TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView2);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid9_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strItemCode;

            strID = e.Item.Cells[0].Text;
            strItemCode = ((Button)e.Item.FindControl("BT_ItemCode")).Text.Trim();

            for (int i = 0; i < DataGrid9.Items.Count; i++)
            {
                DataGrid9.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Item as item where ItemCode = " + "'" + strItemCode + "'";
            ItemBLL itemBLL = new ItemBLL();
            lst = itemBLL.GetAllItems(strHQL);

            if (lst.Count > 0)
            {
                Item item = (Item)lst[0];

                TB_GoodsCode.Text = item.ItemCode;
                TB_GoodsName.Text = item.ItemName;

                try
                {
                    DL_GoodsType.SelectedValue = item.SmallType;
                }
                catch
                {
                    DL_GoodsType.SelectedValue = "";
                }

                TB_ModelNumber.Text = item.ModelNumber.Trim();
                TB_Spec.Text = item.Specification;
                TB_Brand.Text = item.Brand;

                DL_Unit.SelectedValue = item.Unit;

                if (LB_SourceRelatedID.Text.Trim() == "0")
                {
                    DL_RecordSourceType.SelectedValue = "Other";
                    NB_RecordSourceID.Amount = 0;
                }

                LoadItemBomVersion(item.ItemCode.Trim(), DL_BomVerID);
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID;

            strID = e.Item.Cells[0].Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From Goods as goods where goods.ID = " + strID;
            GoodsBLL goodsBLL = new GoodsBLL();
            lst = goodsBLL.GetAllGoodss(strHQL);

            Goods goods = (Goods)lst[0];

            TB_GoodsCode.Text = goods.GoodsCode.Trim();
            TB_GoodsName.Text = goods.GoodsName.Trim();

            try
            {
                DL_GoodsType.SelectedValue = goods.Type;
            }
            catch
            {
                DL_GoodsType.SelectedValue = "";
            }

            TB_ModelNumber.Text = goods.ModelNumber.Trim();
            TB_Spec.Text = goods.Spec.Trim();
            TB_Brand.Text = goods.Manufacturer;

            DL_Unit.SelectedValue = goods.UnitName;

            LoadItemBomVersion(goods.GoodsCode.Trim(), DL_BomVerID);

            if (LB_SourceRelatedID.Text.Trim() == "0")
            {
                DL_RecordSourceType.SelectedValue = "Other";
                NB_RecordSourceID.Amount = 0;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from GoodsApplicationDetail as goodsApplicationDetail where goodsApplicationDetail.ID = " + strID;
            GoodsApplicationDetailBLL goodsApplicationDetailBLL = new GoodsApplicationDetailBLL();
            lst = goodsApplicationDetailBLL.GetAllGoodsApplicationDetails(strHQL);

            if (lst.Count > 0)
            {
                try
                {
                    GoodsApplicationDetail goodsApplicationDetail = (GoodsApplicationDetail)lst[0];

                    TB_GoodsCode.Text = goodsApplicationDetail.GoodsCode;
                    TB_GoodsName.Text = goodsApplicationDetail.GoodsName.Trim();

                    try
                    {
                        DL_GoodsType.SelectedValue = goodsApplicationDetail.Type;
                    }
                    catch
                    {
                        DL_GoodsType.SelectedValue = "";
                    }

                    TB_ModelNumber.Text = goodsApplicationDetail.ModelNumber.Trim();
                    TB_Spec.Text = goodsApplicationDetail.Spec.Trim();
                    TB_Brand.Text = goodsApplicationDetail.Brand;

                    NB_PlanNumber.Amount = goodsApplicationDetail.Number;
                    DL_Unit.SelectedValue = goodsApplicationDetail.Unit;


                    LB_SourceRelatedID.Text = goodsApplicationDetail.AAID.ToString();
                    DL_RecordSourceType.SelectedValue = "GoodsAORecord";
                    NB_RecordSourceID.Amount = goodsApplicationDetail.AAID;

                    LoadItemBomVersion(goodsApplicationDetail.GoodsCode.Trim(), DL_BomVerID);
                }
                catch
                {

                }
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid7_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid7.Items.Count; i++)
            {
                DataGrid7.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from GoodsSaleRecord as goodsSaleRecord where goodsSaleRecord.ID = " + strID;
            GoodsSaleRecordBLL goodsSaleRecordBLL = new GoodsSaleRecordBLL();
            lst = goodsSaleRecordBLL.GetAllGoodsSaleRecords(strHQL);

            if (lst.Count > 0)
            {
                try
                {
                    GoodsSaleRecord goodsSaleRecord = (GoodsSaleRecord)lst[0];

                    TB_GoodsCode.Text = goodsSaleRecord.GoodsCode;
                    TB_GoodsName.Text = goodsSaleRecord.GoodsName.Trim();

                    try
                    {
                        DL_GoodsType.SelectedValue = goodsSaleRecord.Type;
                    }
                    catch
                    {
                        DL_GoodsType.SelectedValue = "";
                    }

                    TB_ModelNumber.Text = goodsSaleRecord.ModelNumber.Trim();
                    TB_Spec.Text = goodsSaleRecord.Spec.Trim();
                    TB_Brand.Text = goodsSaleRecord.Brand;

                    NB_PlanNumber.Amount = goodsSaleRecord.Number;
                    DL_Unit.SelectedValue = goodsSaleRecord.Unit;

                    LB_SourceRelatedID.Text = goodsSaleRecord.SOID.ToString();
                    DL_RecordSourceType.SelectedValue = "GoodsSORecord";
                    NB_RecordSourceID.Amount = goodsSaleRecord.ID;

                    LoadItemBomVersion(goodsSaleRecord.GoodsCode.Trim(), DL_BomVerID);
                }
                catch
                {

                }
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid10_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strItemCode, strItemName, strBomVerID, strUnit, strDefaultProcess;
            decimal deNumber, deReservedNumber;

            for (int i = 0; i < DataGrid10.Items.Count; i++)
            {
                DataGrid10.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            strHQL = "From ProjectRelatedItem as projectRelatedItem where projectRelatedItem.ID = " + strID;
            ProjectRelatedItemBLL projectRelatedItemBLL = new ProjectRelatedItemBLL();
            lst = projectRelatedItemBLL.GetAllProjectRelatedItems(strHQL);

            ProjectRelatedItem projectRelatedItem = (ProjectRelatedItem)lst[0];

            strItemCode = projectRelatedItem.ItemCode.Trim();
            strItemName = projectRelatedItem.ItemName.Trim();
            strBomVerID = projectRelatedItem.BomVersionID.ToString();
            strUnit = projectRelatedItem.Unit;
            deNumber = projectRelatedItem.Number;
            deReservedNumber = projectRelatedItem.ReservedNumber;
            strDefaultProcess = projectRelatedItem.DefaultProcess.Trim();

            TB_GoodsCode.Text = strItemCode;
            TB_GoodsName.Text = strItemName;

            try
            {
                DL_GoodsType.SelectedValue = projectRelatedItem.ItemType;
            }
            catch
            {
            }

            TB_Spec.Text = projectRelatedItem.Specification;
            TB_ModelNumber.Text = projectRelatedItem.ModelNumber;
            TB_Brand.Text = projectRelatedItem.Brand;


            NB_PlanNumber.Amount = deNumber;

            DL_Unit.SelectedValue = strUnit;

            LB_SourceRelatedID.Text = projectRelatedItem.ProjectID.ToString();
            DL_RecordSourceType.SelectedValue = "GoodsPJRecord";
            NB_RecordSourceID.Amount = projectRelatedItem.ID;

            LoadItemBomVersion(strItemCode, DL_BomVerID);
            DL_BomVerID.SelectedValue = strBomVerID;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }



    protected void DataGrid24_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID, strConstractCode;
            IList lst;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            LB_ID.Text = strID;

            strConstractCode = LB_ConstractCode.Text.Trim();

            for (int i = 0; i < DataGrid24.Items.Count; i++)
            {
                DataGrid24.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from ConstractRelatedGoods as constractRelatedGoods Where constractRelatedGoods.ConstractCode = " + "'" + strConstractCode + "'";

            ConstractRelatedGoodsBLL constractRelatedGoodsBLL = new ConstractRelatedGoodsBLL();
            lst = constractRelatedGoodsBLL.GetAllConstractRelatedGoodss(strHQL);
            ConstractRelatedGoods constractRelatedGoods = (ConstractRelatedGoods)lst[0];

            TB_GoodsCode.Text = constractRelatedGoods.GoodsCode;
            TB_GoodsName.Text = constractRelatedGoods.GoodsName;
            TB_ModelNumber.Text = constractRelatedGoods.ModelNumber;
            TB_Spec.Text = constractRelatedGoods.Spec;
            TB_Brand.Text = constractRelatedGoods.Brand;

            DL_GoodsType.SelectedValue = constractRelatedGoods.Type;
            DL_Unit.SelectedValue = constractRelatedGoods.Unit;
            NB_PlanNumber.Amount = constractRelatedGoods.Number;


            DL_Unit.SelectedValue = constractRelatedGoods.Unit;

            LB_SourceRelatedID.Text = constractRelatedGoods.ID.ToString();
            DL_RecordSourceType.SelectedValue = "GoodsCSRecord";
            NB_RecordSourceID.Amount = constractRelatedGoods.ID;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void BT_CreateDetail_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popDetailWindow') ", true);
    }

    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        string strPlanVerID;

        strPlanVerID = LB_PlanVerID.Text.Trim();

        if (strPlanVerID == "")
        {
            AddMain();
        }
        else
        {
            UpdateMain();
        }


        string strDetailID;

        strDetailID = LB_ID.Text.Trim();

        if (strDetailID == "")
        {
            AddDetail();
        }
        else
        {
            UpdateDetail();
        }
    }


    protected void AddDetail()
    {
        string strPlanVerID, strGoodsCode, strGoodsName, strGoodsType, strSpec, strBomVerID, strUnitName;
        string strRecordSourceType, strRecordSourceID, strModelNumber;
        decimal dePlanNumber, deFinishedNumber, deUNFinishedNumber;
        DateTime dtPlanStartDate, dtPlanMakeDate, dtDeliveryDate;


        strPlanVerID = LB_PlanVerID.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();

        strGoodsType = DL_GoodsType.SelectedValue.Trim();
        strSpec = TB_Spec.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strBomVerID = DL_BomVerID.SelectedValue.Trim();

        dePlanNumber = NB_PlanNumber.Amount;
        deFinishedNumber = NB_FinishedNUmber.Amount;
        deUNFinishedNumber = dePlanNumber - deFinishedNumber;

        dtPlanStartDate = DateTime.Parse(DLC_PlanStartDate.Text.Trim());
        dtPlanMakeDate = DateTime.Now;
        dtDeliveryDate = DateTime.Parse(DLC_DeliveryDate.Text.Trim());

        strUnitName = DL_Unit.SelectedValue.Trim();


        strRecordSourceType = DL_RecordSourceType.SelectedValue.Trim();
        strRecordSourceID = NB_RecordSourceID.Amount.ToString();


        if (strGoodsCode == "" | strGoodsName == "" | strSpec == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
        }
        else
        {
            ItemMainPlanDetailBLL itemMainPlanDetailBLL = new ItemMainPlanDetailBLL();
            ItemMainPlanDetail itemMainPlanDetail = new ItemMainPlanDetail();

            itemMainPlanDetail.PlanVerID = int.Parse(strPlanVerID);

            itemMainPlanDetail.ItemCode = strGoodsCode;
            itemMainPlanDetail.ItemName = strGoodsName;
            itemMainPlanDetail.Type = strGoodsType;
            itemMainPlanDetail.ModelNumber = strModelNumber;
            itemMainPlanDetail.Specification = strSpec;
            itemMainPlanDetail.Brand = TB_Brand.Text;

            itemMainPlanDetail.BomVerID = int.Parse(strBomVerID);
            itemMainPlanDetail.PlanNumber = dePlanNumber;
            itemMainPlanDetail.FinishedNumber = deFinishedNumber;
            itemMainPlanDetail.UNFinishedNumber = deUNFinishedNumber;
            itemMainPlanDetail.Unit = strUnitName;
            itemMainPlanDetail.PlanStartDate = dtPlanStartDate;

            itemMainPlanDetail.DeliveryDate = dtDeliveryDate;
            itemMainPlanDetail.CreateDate = DateTime.Now;
            itemMainPlanDetail.ModifyDate = DateTime.Now;

            itemMainPlanDetail.SourceType = strRecordSourceType;
            itemMainPlanDetail.SourceRecordID = int.Parse(strRecordSourceID);

            try
            {

                itemMainPlanDetailBLL.AddItemMainPlanDetail(itemMainPlanDetail);
                LB_ID.Text = ShareClass.GetMyCreatedMaxGoodsMrpMainPlanVerDetailID(strPlanVerID).ToString();

                LoadItemMainPlanDetail(strPlanVerID);

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }
        }
    }

    protected void UpdateDetail()
    {
        string strHQL;
        IList lst;

        string strID, strPlanVerID, strGoodsCode, strGoodsType, strGoodsName, strSpec, strBomVerID, strUnitName;
        string strSourceType, strSourceID, strSourceRelatedID, strModelNumber;
        decimal dePlanNumber, deFinishedNumber, deUNFinishedNumber;
        DateTime dtPlanStartDate, dtPlanMakeDate, dtDeliveryDate;


        strID = LB_ID.Text.Trim();
        strPlanVerID = LB_PlanVerID.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();

        strGoodsType = DL_GoodsType.SelectedValue.Trim();

        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        strBomVerID = DL_BomVerID.SelectedValue.Trim();

        dePlanNumber = NB_PlanNumber.Amount;
        deFinishedNumber = NB_FinishedNUmber.Amount;
        deUNFinishedNumber = dePlanNumber - deFinishedNumber;

        dtPlanStartDate = DateTime.Parse(DLC_PlanStartDate.Text.Trim());
        dtPlanMakeDate = DateTime.Now;
        dtDeliveryDate = DateTime.Parse(DLC_DeliveryDate.Text.Trim());

        strUnitName = DL_Unit.SelectedValue.Trim();

        strSourceRelatedID = LB_SourceRelatedID.Text.Trim();
        strSourceType = DL_RecordSourceType.SelectedValue.Trim();
        strSourceID = NB_RecordSourceID.Amount.ToString();


        if (strGoodsCode == "" | strGoodsName == "" | strSpec == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
        }
        else
        {
            strHQL = "From ItemMainPlanDetail as itemMainPlanDetail Where itemMainPlanDetail.ID =" + strID;
            ItemMainPlanDetailBLL itemMainPlanDetailBLL = new ItemMainPlanDetailBLL();
            lst = itemMainPlanDetailBLL.GetAllItemMainPlanDetails(strHQL);
            ItemMainPlanDetail itemMainPlanDetail = (ItemMainPlanDetail)lst[0];

            itemMainPlanDetail.PlanVerID = int.Parse(strPlanVerID);
            itemMainPlanDetail.SourceType = strSourceType;
            itemMainPlanDetail.SourceRecordID = int.Parse(strSourceID);
            itemMainPlanDetail.ItemCode = strGoodsCode;
            itemMainPlanDetail.ItemName = strGoodsName;
            itemMainPlanDetail.Type = strGoodsType;
            itemMainPlanDetail.ModelNumber = strModelNumber;
            itemMainPlanDetail.Specification = strSpec;
            itemMainPlanDetail.Brand = TB_Brand.Text;

            itemMainPlanDetail.BomVerID = int.Parse(strBomVerID);
            itemMainPlanDetail.PlanNumber = dePlanNumber;
            itemMainPlanDetail.FinishedNumber = deFinishedNumber;
            itemMainPlanDetail.UNFinishedNumber = deUNFinishedNumber;
            itemMainPlanDetail.Unit = strUnitName;
            itemMainPlanDetail.PlanStartDate = dtPlanStartDate;
            itemMainPlanDetail.DeliveryDate = dtDeliveryDate;
            itemMainPlanDetail.ModifyDate = DateTime.Now;

            try
            {

                itemMainPlanDetailBLL.UpdateItemMainPlanDetail(itemMainPlanDetail, int.Parse(strID));

                LoadItemMainPlanDetail(strPlanVerID);

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }
        }
    }


    protected void CB_CheckProductionDetailItem_CheckedChanged(object sender, EventArgs e)
    {
        int i = 0;
        if (CB_CheckProductionDetailItem.Checked == true)
        {
            for (i = 0; i < DataGrid10.Items.Count; i++)
            {
                ((CheckBox)DataGrid10.Items[i].FindControl("CB_CheckItem")).Checked = true;
            }
        }
        else
        {
            for (i = 0; i < DataGrid10.Items.Count; i++)
            {
                ((CheckBox)DataGrid10.Items[i].FindControl("CB_CheckItem")).Checked = false;
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }


    protected void IB_AdProductionDetailItem_Click(object sender, ImageClickEventArgs e)
    {
        string strHQL;

        string strSourceID, strItemCode, strRelatedID;


        strRelatedID = LB_ProjectID.Text.Trim();

        string strPlanVerID = LB_PlanVerID.Text.Trim();


        for (int i = 0; i < DataGrid10.Items.Count; i++)
        {
            if (((CheckBox)DataGrid10.Items[i].FindControl("CB_CheckItem")).Checked == true)
            {
                strItemCode = DataGrid10.Items[i].Cells[2].Text;
                strSourceID = ((Button)DataGrid10.Items[i].FindControl("BT_ID")).Text.Trim();

                try
                {
                    strHQL = "Insert Into T_ItemMainPlanDetail(PlanVerID,ItemCode,ItemName,Type,ModelNumber,Specification,Brand,BomVerID,PlanNumber,FinishedNumber,UNFinishedNumber,Unit,";
                    strHQL += "PlanStartDate,DeliveryDate,CreateDate,ModifyDate,SourceType,SourceRecordID)";
                    strHQL += " Select " + strPlanVerID + ",A.ItemCode,A.ItemName,B.SmallType,B.ModelNumber,B.Specification,A.Brand,A.BomVersionID,A.Number,0,A.Number,A.Unit,now(),now(),now(),now(),'GoodsPJRecord',A.ID ";
                    strHQL += " From T_ProjectRelatedItem A, T_Item B  Where  A.ItemCode = B.ItemCode  and A.ID = " + strSourceID;
                    strHQL += " and A.ID Not In (Select SourceRecordID From T_ItemMainPlanDetail Where SourceType = 'GoodsPJRecord' and PlanVerID = " + strPlanVerID + ")";

                    ShareClass.RunSqlCommand(strHQL);
                }
                catch
                {

                }
            }
        }

        LoadItemMainPlanDetail(strPlanVerID);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }


    protected void BT_CopyDetailFromPlan_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strFromPlanVerID = NB_FromPlanVerID.Amount.ToString();
        string strToPlanVerID = LB_PlanVerID.Text.Trim();

        strHQL = "Insert Into T_ItemMainPlanDetail(PlanVerID,ItemCode,ItemName,Type,ModelNumber,Specification,Brand,BOMVerID,PlanNumber,FinishedNumber,UNFinishedNumber,Unit,PlanStartDate,MakeDate,DeliveryDate,CreateDate,Modifydate,SourceType,SourceRecordID)";
        strHQL += "Select " + strToPlanVerID + ",ItemCode,ItemName,Type,ModelNumber,Specification,Brand,BOMVerID,PlanNumber,FinishedNumber,UNFinishedNumber,Unit,PlanStartDate,MakeDate,DeliveryDate,CreateDate,Modifydate,SourceType,SourceRecordID From T_ItemMainPlanDetail";
        strHQL += " Where PlanVerID  = " + strFromPlanVerID;
        strHQL += " Order By ID ASC";
        ShareClass.RunSqlCommand(strHQL);

        LoadItemMainPlanDetail(strToPlanVerID);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }


    protected void UpdateGoodsNumberForAdd(string strFromGoodsID, decimal deNumber)
    {
        string strHQL;

        strHQL = "Update T_Goods Set Number = Number - " + deNumber.ToString() + " Where ID = " + strFromGoodsID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected void UpdateGoodsNumberForUpdate(string strFromGoodsID, decimal deNumber, decimal deOldNumber)
    {
        string strHQL;

        strHQL = "Update T_Goods Set Number = Number - " + (deNumber - deOldNumber).ToString() + " Where ID = " + strFromGoodsID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected void UpdateGoodsNumberForDelete(string strFromGoodsID, decimal deNumber)
    {
        string strHQL;

        strHQL = "Update T_Goods Set Number = Number + " + deNumber.ToString() + " Where ID = " + strFromGoodsID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected void UpdateGoodsSOOrAONumber(string strSourceType, string strSourceID)
    {
        string strHQL;
        decimal deSumNumber;

        if (strSourceType == "GoodsSORecord")
        {
            strHQL = "Select  COALESCE(Sum(Number),0) From T_GoodsShipmentDetail Where SourceType = 'GoodsSORecord' And SourceID=" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsShipmentDetail");

            try
            {
                deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deSumNumber = 0;
            }

            strHQL = "Update T_GoodsSaleRecord Set CheckOutNumber = " + deSumNumber.ToString() + " Where ID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }

        if (strSourceType == "GoodsAORecord")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsShipmentDetail Where SourceType = 'GoodsAORecord' And SourceID=" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsShipmentDetail");

            try
            {
                deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deSumNumber = 0;
            }

            strHQL = "Update T_GoodsApplicationDetail Set CheckOutNumber = " + deSumNumber.ToString() + " Where AAID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected bool CheckShipNumber(string strFromGoodsID, decimal deShipNumber)
    {
        string strHQL;
        IList lst;

        decimal deGoodsNumber;

        strHQL = "From Goods as goods where goods.ID = " + strFromGoodsID;
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);

        Goods goods = (Goods)lst[0];

        deGoodsNumber = goods.Number;

        if (deGoodsNumber < deShipNumber)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void LoadItemMainPlan(string strUserCode)
    {
        string strHQL;

        strHQL = "Select * From T_ItemMainPlan Where CreatorCode = " + "'" + strUserCode + "'";
        strHQL += " Or CreatorCode in (Select UserCode From T_ProjectMember Where DepartCode in  " + LB_DepartString.Text.Trim() + ")";
        strHQL += " Order By PlanVerID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlan");

        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void LoadItemMainPlanDetail(string strPlanVerID)
    {
        string strHQL;

        strHQL = "Select * From T_ItemMainPlanDetail Where PlanVerID = " + strPlanVerID;
        strHQL += " Order By ID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanDetail");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql1.Text = strHQL;
    }

    protected void LoadGoodsShipmentDetail(string strShipmentNO)
    {
        string strHQL;
        IList lst;

        strHQL = "From GoodsShipmentDetail as goodsShipmentDetail Where goodsShipmentDetail.ShipmentNO = " + strShipmentNO;
        strHQL += " Order By goodsShipmentDetail.ID ASC";
        GoodsShipmentDetailBLL goodsShipmentDetailBLL = new GoodsShipmentDetailBLL();
        lst = goodsShipmentDetailBLL.GetAllGoodsShipmentDetails(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql1.Text = strHQL;
    }

    protected void LoadGoods(string strGoodsCode, string strWareHouse)
    {
        string strHQL;
        IList lst;

        strHQL = "From Goods as goods Where goods.Number > 0 and goods.GoodsCode = " + "'" + strGoodsCode + "'";
        strHQL += " and goods.Position = " + "'" + strWareHouse + "'";
        strHQL += " Order by goods.ID ASC";
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        TB_GoodsName.Focus();

        LB_Sql2.Text = strHQL;
    }


    protected void LoadProjectRelatedItem(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ProjectRelatedItem as projectRelatedItem where projectRelatedItem.ProjectID = " + strProjectID + " Order by projectRelatedItem.ID ASC";
        ProjectRelatedItemBLL projectRelatedItemBLL = new ProjectRelatedItemBLL();
        lst = projectRelatedItemBLL.GetAllProjectRelatedItems(strHQL);

        DataGrid10.DataSource = lst;
        DataGrid10.DataBind();
    }

    protected void LoadAllGoodsByWareHouse(string strWareHouse)
    {
        string strHQL;
        IList lst;

        strHQL = "From Goods as goods Where goods.Number > 0 and goods.Position = " + "'" + strWareHouse + "'";
        strHQL += " Order by goods.ID ASC";
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql2.Text = strHQL;
    }

    protected void LoadItemBomVersion(string strItemCode, DropDownList DL_VersionID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ItemBomVersion as itemBomVersion Where itemBomVersion.ItemCode = " + "'" + strItemCode + "'" + " Order by itemBomVersion.VerID DESC";
        ItemBomVersionBLL itemBomVersionBLL = new ItemBomVersionBLL();
        lst = itemBomVersionBLL.GetAllItemBomVersions(strHQL);

        DL_VersionID.DataSource = lst;
        DL_VersionID.DataBind();

        DL_VersionID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void LoadGoodsApplication(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartString;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "from GoodsApplication as goodsApplication where ";
        strHQL += " (goodsApplication.ApplicantCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " or goodsApplication.ApplicantCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ")) ";
        strHQL += " Order by goodsApplication.AAID DESC";
        GoodsApplicationBLL goodsApplicationBLL = new GoodsApplicationBLL();
        lst = goodsApplicationBLL.GetAllGoodsApplications(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        LB_Sql3.Text = strHQL;
    }

    protected void LoadGoodsSaleOrder(string strOperatorCode)
    {
        string strHQL;
        IList lst;

        string strDepartString;
        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "from GoodsSaleOrder as goodsSaleOrder where ( goodsSaleOrder.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " or goodsSaleOrder.OperatorCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ") ";
        strHQL += " or goodsSaleOrder.OperatorCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " or goodsSaleOrder.SalesCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
        strHQL += " Order by goodsSaleOrder.SOID DESC";
        GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
        lst = goodsSaleOrderBLL.GetAllGoodsSaleOrders(strHQL);

        DataGrid6.DataSource = lst;
        DataGrid6.DataBind();

        LB_Sql8.Text = strHQL;
    }

    protected void LoadProject(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartString;
        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "from Project as project";
        strHQL += " Where project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " order by project.ProjectID DESC ";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        DataGrid8.DataSource = lst;
        DataGrid8.DataBind();
    }


    protected void LoadUsingConstract(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where  constract.Status not in ('Archived','Deleted') ";
        strHQL += " and (constract.RecorderCode = " + "'" + strUserCode + "'" + " Or constract.ConstractCode in (select constractRelatedUser.ConstractCode from ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = " + "'" + strUserCode + "'" + "))";
        strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";

        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);

        DataGrid23.DataSource = lst;
        DataGrid23.DataBind();
    }

    protected void LoadGoodsApplicationDetail(string strAAID)
    {
        string strHQL;
        IList lst;

        if (strAAID != "")
        {
            strHQL = "from GoodsApplicationDetail as goodsApplicationDetail where goodsApplicationDetail.AAID = " + strAAID;
            GoodsApplicationDetailBLL goodsApplicationDetailBLL = new GoodsApplicationDetailBLL();
            lst = goodsApplicationDetailBLL.GetAllGoodsApplicationDetails(strHQL);

            DataGrid4.DataSource = lst;
            DataGrid4.DataBind();

            LB_Sql4.Text = strHQL;
        }
    }

    protected void LoadGoodsSaleOrderDetail(string strSOID)
    {
        LB_GoodsOwner.Text = LanguageHandle.GetWord("XiaoShouChan") + strSOID + LanguageHandle.GetWord("MingXi");

        if (strSOID != "")
        {
            string strHQL = "Select * from T_GoodsSaleRecord where SOID = " + strSOID + " Order by ID DESC";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleRecord");

            DataGrid7.DataSource = ds;
            DataGrid7.DataBind();

            LB_Sql7.Text = strHQL;
        }
    }


    protected void LoadConstractRelatedGoodsList(string strConstractCode)
    {
        string strHQL;
        IList lst;

        ConstractRelatedGoodsBLL constractRelatedGoodsBLL = new ConstractRelatedGoodsBLL();
        strHQL = "from ConstractRelatedGoods as constractRelatedGoods where constractRelatedGoods.ConstractCode = " + "'" + strConstractCode + "'";
        lst = constractRelatedGoodsBLL.GetAllConstractRelatedGoodss(strHQL);

        DataGrid24.DataSource = lst;
        DataGrid24.DataBind();
    }

    protected int GetRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString();
        strHQL += " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        return lst.Count;
    }


    protected Item GetItem(string strItemCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Item as item where item.ItemCode = " + "'" + strItemCode + "'";
        ItemBLL itemBLL = new ItemBLL();
        lst = itemBLL.GetAllItems(strHQL);
        Item item = (Item)lst[0];

        return item;
    }


}
