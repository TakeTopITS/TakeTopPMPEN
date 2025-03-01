using System;
using System.Resources;
using System.IO;
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
using System.Data.OleDb;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopCore;

public partial class TTItemDataAndBomSet : System.Web.UI.Page
{
    string strUserCode, strChildItemCodes = "";
    decimal deSumItemBomCost;

    string strRelatedType, strRelatedID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strItemCode;

        //Session["UserCode"] = "C7094";
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "入库单", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        if (strRelatedType == null)
        {
            strRelatedType = "SYSTEM";
            strRelatedID = "0";
        }

        if (strRelatedType == "PROJECT")
        {
            //this.Title = "项目:" + strRelatedID + "元素定义与结构设置！";
        }
        else
        {
            //this.Title = "元素定义与结构设置！";
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");

        if (Page.IsPostBack != true)
        {
            LoadUnit();
            LoadProductProcess();
            LoadPackingType();

            if (LLB_ItemCode.Items.Count > 0)
            {
                strItemCode = LLB_ItemCode.Items[0].Text.Trim();

                LoadItemBomVersion(strItemCode);
            }

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            BT_AddToBom.Enabled = false;
            BT_UpdateFormBom.Enabled = false;
            BT_DeleteFormBom.Enabled = false;

            DL_RelatedType.SelectedValue = strRelatedType;
            LoadItemByItemType(strRelatedType, strRelatedID, DL_ProjectItemType.SelectedValue.Trim(), "");
            LoadCurrencyType();

            DL_BigType.Items.Insert(0, new ListItem("--Select--", ""));
            DL_SmallType.Items.Insert(0, new ListItem("--Select--", ""));

            //初始化Item BOM数据
            TakeTopBOM.AddDataToItemBom();
        }
    }

    protected void DL_RelatedType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strItemRelatedType, strItemType;

        strItemRelatedType = DL_RelatedType.SelectedValue.Trim();
        strItemType = DL_ProjectItemType.SelectedValue.Trim();

        LoadItemByItemType(strItemRelatedType, strRelatedID, strItemType, "");
    }

    protected void DL_ProjectItemType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strItemRelatedType, strItemType;

        strItemRelatedType = DL_RelatedType.SelectedValue.Trim();
        strItemType = DL_ProjectItemType.SelectedValue.Trim();

        LoadItemByItemType(strItemRelatedType, strRelatedID, strItemType, "");
    }

    protected void BT_FindItemName_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strItemRelatedType, strItemType;

        strItemRelatedType = DL_RelatedType.SelectedValue.Trim();
        strItemType = DL_ProjectItemType.SelectedValue.Trim();

        string strItemCode = "%" + TB_ProjectItemCode.Text.Trim() + "%";
        string strItemName = "%" + TB_ProjectItemName.Text.Trim() + "%";
        string strModelNumber = "%" + TB_FindModelNumber.Text.Trim() + "%";
        string strBrand = "%" + TB_FindBrand.Text.Trim() + "%";
        string strItemSpec = "%" + TB_ProjectItemSpec.Text.Trim() + "%";

        strHQL = "Select ItemCode, ItemCode || '  ' ||  ItemName as ProjectItemName From T_Item Where";
        strHQL += " ItemCode Like " + "'" + strItemCode + "'";
        strHQL += " and ItemName Like " + "'" + strItemName + "'";
        strHQL += " and ModelNumber Like " + "'" + strModelNumber + "'";
        strHQL += " and Brand Like " + "'" + strBrand + "'";
        strHQL += " and Specification Like " + "'" + strItemSpec + "'";
        strHQL += " and Type = " + "'" + strItemType + "'";
        strHQL += " and RelatedType =" + "'" + strItemRelatedType + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectItem");

        LLB_ItemCode.DataSource = ds;
        LLB_ItemCode.DataBind();
    }

    protected void LLB_ItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strItemCode;
        string strVerID, strVerType, strBigType;
        string strEditStatus;
        string strItemBomRelatedType, strItemBomRelatedID;
        string strFromItemCode, strToItemCode;
        int intTreeDepth;

        try
        {
            intTreeDepth = int.Parse(LB_TreeDepth.Text.Trim());
            strEditStatus = DL_EditStatus.SelectedValue.Trim();
            strItemCode = LLB_ItemCode.SelectedValue.Trim();

            strHQL = "From Item as item where item.ItemCode = " + "'" + strItemCode + "'";
            ItemBLL itemBLL = new ItemBLL();
            lst = itemBLL.GetAllItems(strHQL);
            Item item = (Item)lst[0];

            strBigType = item.BigType.Trim();
            LoadSmallTypeList(strBigType);

            TB_ItemCode.Text = item.ItemCode.Trim();
            TB_ItemName.Text = item.ItemName.Trim();
            DL_ItemType.SelectedValue = item.Type.Trim();

            try
            {
                DL_BigType.SelectedValue = item.BigType.Trim();
            }
            catch
            {
                DL_BigType.SelectedValue = "";
            }

            try
            {
                DL_SmallType.SelectedValue = item.SmallType;
            }
            catch
            {
                DL_SmallType.SelectedValue = "";
            }

            DL_Unit.SelectedValue = item.Unit;
            TB_ModelNumber.Text = item.ModelNumber.Trim();
            TB_Specification.Text = item.Specification.Trim();
            TB_Brand.Text = item.Brand;
            NB_PULeadTime.Amount = item.PULeadTime;
            NB_MFLeadTime.Amount = item.MFLeadTime;

            NB_PurchasePrice.Amount = item.PurchasePrice;
            NB_SalePrice.Amount = item.SalePrice;
            DL_CurrencyType.SelectedValue = item.CurrencyType.Trim();

            NB_HRCost.Amount = item.HRCost;
            NB_MFCost.Amount = item.MFCost;
            NB_MTCost.Amount = item.MTCost;
            NB_SafetyStock.Amount = item.SafetyStock;

            TB_DefaultProcess.Text = item.DefaultProcess.Trim();

            LB_ChildItemCode.Text = item.ItemCode.Trim();
            LB_ChildItemName.Text = item.ItemName.Trim();

            DL_ChildItemUnitToBom.SelectedValue = item.Unit;

            TB_ChildDefaultProcess.Text = item.DefaultProcess.Trim();
            NB_WarrantyPeriod.Amount = item.WarrantyPeriod;
            NB_LossRate.Amount = item.LossRate;
            NB_BomItemLossRate.Amount = item.LossRate;

            IM_ItemPhoto.ImageUrl = item.PhotoURL.Trim();
            HL_ItemPhoto.NavigateUrl = item.PhotoURL.Trim();

            TB_RegistrationNumber.Text = item.RegistrationNumber.Trim();

            TB_PackingType.Text = item.PackingType.Trim();

            strItemBomRelatedType = item.RelatedType.Trim();
            strItemBomRelatedID = item.RelatedID.ToString();

            BT_New.Enabled = true;

            if (strEditStatus == "NO")
            {
                LoadItemBomVersion(strItemCode);

                if (DL_VersionID.Items.Count > 0)
                {
                    try
                    {
                        //检查相应版本的BOM是否存在，如没有则初始化
                        ChecAndSetItemBom(strItemCode, DL_VersionID.SelectedItem.Text.Trim());
                    }
                    catch
                    {
                    }

                    DL_ToItemVersionID.SelectedItem.Text = DL_VersionID.SelectedItem.Text.Trim();
                    strVerID = DL_VersionID.SelectedItem.Text.Trim();
                    strVerType = GetItemBomVersionType(strItemCode, strVerID);
                    DL_ChangeVersionType.SelectedValue = strVerType;
                    DL_ChangeChildVersionType.SelectedValue = strVerType;

                    try
                    {
                        TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView1);
                        LB_ItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString("F2");


                        TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView2);
                        LB_ChildItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString("F2");
                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWXXHCWKNSZXZWWX").ToString().Trim() + "')", true);
                    }
                }
                else
                {
                    strVerID = "0";
                    try
                    {
                        TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView1);
                        LB_ItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString("F2");


                        TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView2);
                        LB_ChildItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString("F2");

                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWXXHCWKNSZXZWWX").ToString().Trim() + "')", true);
                    }
                }

                LB_TopItemCode.Text = strItemCode;
                LB_TopItemName.Text = GetItemName(strItemCode);


                LB_ToItemCode.Text = strItemCode;
                //LB_ToItemVerID.Text = strVerID;

                BT_AddToBom.Enabled = false;
            }
            else
            {
                LoadItemBomVersion(strItemCode);

                if (DL_ChildVersionID.Items.Count > 0)
                {
                    try
                    {
                        //检查相应版本的BOM是否存在，如没有则初始化
                        ChecAndSetItemBom(strItemCode, DL_ChildVersionID.SelectedItem.Text.Trim());
                    }
                    catch
                    {
                    }

                    strVerID = DL_ChildVersionID.SelectedItem.Text.Trim();
                    strVerType = GetItemBomVersionType(strItemCode, strVerID);
                    DL_ChangeChildVersionType.SelectedValue = strVerType;

                    try
                    {
                        TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView2);
                        LB_ChildItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString("F2");


                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWXXHCWKNSZXZWWX").ToString().Trim() + "')", true);
                    }
                }
                else
                {
                    strVerID = "0";
                    try
                    {
                        TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView2);
                        LB_ChildItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString("F2");


                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWXXHCWKNSZXZWWX").ToString().Trim() + "')", true);
                    }
                }
            }

            LB_FromItemCode.Text = strItemCode;

            strFromItemCode = LB_FromItemCode.Text.Trim();
            strToItemCode = LB_ToItemCode.Text.Trim();

            if (BT_CopyVersionAB.Enabled)
            {
                if (strFromItemCode == strToItemCode)
                {
                    BT_CopyVersionAB.Enabled = false;
                }
                else
                {
                    BT_CopyVersionAB.Enabled = true;
                }
            }

            if (strRelatedType == strItemBomRelatedType & strRelatedID == strItemBomRelatedID)
            {
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
                BT_DeleteVersion.Enabled = true;
            }
            else
            {
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
                BT_DeleteVersion.Enabled = false;
            }

            BT_NewVersion.Enabled = true;

            BT_TakePhoto.Enabled = true;
            BT_DeletePhoto.Enabled = true;

            BT_ExportToExcel.Enabled = true;
            BT_CreateBOMReport.Enabled = true;
        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
        }

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

    }

    //查检BOM是否存在
    protected void ChecAndSetItemBom(string strItemCode, string strVerID)
    {
        string strHQL;

        string strType = DL_VersionType.SelectedValue.Trim();
        strItemCode = LLB_ItemCode.SelectedValue.Trim();
        string strUnit = DL_Unit.SelectedValue.Trim();
        string strKeyWord = DateTime.Now.ToString("yyyyMMddHHMMssff");

        Item item = GetItem(strItemCode);

        strHQL = "Select * From T_ItemBom Where BelongItemCode = '" + strItemCode + "' And VerID = " + strVerID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemBom");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Insert Into T_ItemBom(ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,ChildItemModelNumber,ChildItemPhotoURL,Number,Unit,DefaultProcess,ChildItemVerID,VerID,BelongItemCode,BelongVerId,KeyWord,ParentKeyWord)";
            strHQL += " Values(" + "'" + strItemCode + "'" + "," + "'" + strItemCode + "'" + "," + "'" + strItemCode + "','" + item.ItemName + "','" + item.SmallType + "','" + item.Specification + "','" + item.ModelNumber + "','" + item.PhotoURL + "',1," + "'" + strUnit + "'" + "," + "' '" + "," + strVerID + "," + strVerID + ",'" + strItemCode + "'," + strVerID + ",'" + strKeyWord + "','" + strKeyWord + "')";

            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected void BT_NewVersion_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strID, strType, strItemCode, strUnit, strKeyWord;
        int intVerID;

        intVerID = int.Parse(NB_NewVerID.Amount.ToString());
        strType = DL_VersionType.SelectedValue.Trim();
        strItemCode = LLB_ItemCode.SelectedValue.Trim();
        strUnit = DL_Unit.SelectedValue.Trim();

        strKeyWord = DateTime.Now.ToString("yyyyMMddHHMMssff");

        if (GetItemBomVersionCount(strItemCode, intVerID.ToString()) == 0)
        {
            ItemBomVersionBLL itemBomVersionBLL = new ItemBomVersionBLL();
            ItemBomVersion itemBomVersion = new ItemBomVersion();

            itemBomVersion.ItemCode = strItemCode;
            itemBomVersion.VerID = intVerID;
            itemBomVersion.Type = strType;
            itemBomVersion.RelatedType = strRelatedType;
            itemBomVersion.RelatedID = int.Parse(strRelatedID);

            Item item = GetItem(strItemCode);

            try
            {
                itemBomVersionBLL.AddItemBomVersion(itemBomVersion);

                strHQL = "Insert Into T_ItemBom(ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,ChildItemModelNumber,ChildItemPhotoURL,Number,Unit,DefaultProcess,ChildItemVerID,VerID,BelongItemCode,BelongVerId,KeyWord,ParentKeyWord)";
                strHQL += " Values(" + "'" + strItemCode + "'" + "," + "'" + strItemCode + "'" + "," + "'" + strItemCode + "','" + item.ItemName + "','" + item.SmallType + "','" + item.Specification + "','" + item.ModelNumber + "','" + item.PhotoURL + "',1," + "'" + strUnit + "'" + "," + "' '" + "," + intVerID.ToString() + "," + intVerID.ToString() + ",'" + strItemCode + "'," + intVerID.ToString() + ",'" + strKeyWord + "','" + strKeyWord + "')";
                ShareClass.RunSqlCommand(strHQL);

                LoadItemBomVersion(strItemCode);

                if (DL_EditStatus.SelectedValue.Trim() == "NO")
                {
                    TakeTopBOM.InitialItemBomTreeForNew(strItemCode, intVerID.ToString(), strItemCode, intVerID.ToString(), TreeView1);
                    LB_ItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, intVerID.ToString(), strItemCode, intVerID.ToString()).ToString("F2");


                }

                TakeTopBOM.InitialItemBomTreeForNew(strItemCode, intVerID.ToString(), strItemCode, intVerID.ToString(), TreeView2);
                LB_ChildItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, intVerID.ToString(), strItemCode, intVerID.ToString()).ToString();

                strID = DL_VersionID.SelectedValue.Trim();
                strType = DL_ChangeVersionType.SelectedValue.Trim();

                if (strType == "InUse")
                {
                    strHQL = "update T_ItemBomVersion Set Type = 'Backup' where Type = 'InUse' and ItemCode = " + "'" + strItemCode + "'";
                    ShareClass.RunSqlCommand(strHQL);
                }

                if (strType == "Baseline")
                {
                    strHQL = "update T_ItemBomVersion Set Type = 'Backup' where Type = 'Baseline' and ItemCode = " + "'" + strItemCode + "'";
                    ShareClass.RunSqlCommand(strHQL);
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBCYSZKNCZCBBHJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeleteVersion_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strType, strVerID, strItemCode;

        if (DL_VersionID.Items.Count == 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBMYSBXBLYGJHBB").ToString().Trim() + "')", true);
            return;
        }

        strVerID = NB_NewVerID.Amount.ToString();
        strType = DL_VersionType.SelectedValue.Trim();
        strItemCode = LLB_ItemCode.SelectedValue.Trim();

        try
        {
            strHQL = "Delete From T_ItemBomVersion Where ItemCode = " + "'" + strItemCode + "'" + " and VerID = " + strVerID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_ItemBom Where ItemCode = " + "'" + strItemCode + "'" + " and VerID = " + strVerID;
            strHQL += " And BelongItemCode = '" + strItemCode + "' And BelongVerID =" + strVerID;
            ShareClass.RunSqlCommand(strHQL);

            LoadItemBomVersion(strItemCode);

            if (DL_VersionID.Items.Count > 0)
            {
                strVerID = DL_VersionID.SelectedItem.Text.Trim();

                TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView1);
                LB_ItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString("F2");


            }

            if (DL_ChildVersionID.Items.Count > 0)
            {
                strVerID = DL_ChildVersionID.SelectedItem.Text.Trim();

                TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView2);
                LB_ChildItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString();


            }


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DL_OldVersionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strVerID, strItemCode;
        string strVerType;

        strVerID = DL_OldVersionID.SelectedItem.Text.Trim();
        strItemCode = LB_ChildItemCode.Text.Trim();

        DL_ChildVersionID.SelectedValue = DL_OldVersionID.SelectedValue;

        strVerType = GetItemBomVersionType(strItemCode, strVerID);
        DL_ChangeChildVersionType.SelectedValue = strVerType;

        try
        {
            TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView2);
            LB_ChildItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString();

        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
        }
    }

    protected void DL_NewVersionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strNewVerID, strItemCode;
        string strItemBomRelatedType, strItemBomRelatedID;

        strNewVerID = DL_NewVersionID.SelectedItem.Text.Trim();
        strItemCode = LLB_ItemCode.SelectedValue.Trim();

        ItemBomVersion itemBomVersion = GetItemBomVersion(strItemCode, strNewVerID);

        strItemBomRelatedType = itemBomVersion.RelatedType.Trim();
        strItemBomRelatedID = itemBomVersion.RelatedID.ToString();

        if (strRelatedType == strItemBomRelatedType & strRelatedID == strItemBomRelatedID)
        {
            BT_CopyVersion.Enabled = true;
        }
        else
        {
            BT_CopyVersion.Enabled = false;
        }
    }


    protected void DL_FromVersionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strVerID, strItemCode;
        string strVerType;

        strVerID = DL_FromVersionID.SelectedItem.Text.Trim();
        strItemCode = LB_ChildItemCode.Text.Trim();

        DL_ChildVersionID.SelectedValue = DL_FromVersionID.SelectedValue;

        strVerType = GetItemBomVersionType(strItemCode, strVerID);
        DL_ChangeChildVersionType.SelectedValue = strVerType;

        try
        {
            TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView2);
            LB_ChildItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString();

        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
        }
    }

    protected void BT_CopyVersion_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strOldVerID, strNewVerID;
        string strItemCode, strKeyWord;

        strOldVerID = DL_OldVersionID.SelectedItem.Text.Trim();
        strNewVerID = DL_NewVersionID.SelectedItem.Text.Trim();

        strItemCode = LLB_ItemCode.SelectedValue.Trim();

        strKeyWord = DateTime.Now.ToString("yyyyMMddHHMMssff");

        if (strOldVerID == strNewVerID)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYFZDBBHBFZDBBBNXTJC").ToString().Trim() + "')", true);
            return;
        }

        try
        {
            strHQL = "delete from T_ItemBom where VerID = " + strNewVerID + " and ItemCode = " + "'" + strItemCode + "'";
            strHQL += " and BelongItemCode = '" + strItemCode + "' And BelongVerID = " + strNewVerID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = @" Insert Into T_ItemBom (ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,ChildItemModelNumber,ChildItemBrand,ChildItemPhotoURL,Number,Unit,ReservedNumber,LossRate,DefaultProcess,ChildItemVerID,VerID,BelongItemCode,BelongVerID,KeyWord,ParentKeyWord,
               PULeadTime
              ,MFLeadTime
              ,HRCost
              ,MTCost
              ,MFCost
              ,PurchasePrice
              ,SalePrice
              ,SortNumber)";

            strHQL += @" Select ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,ChildItemModelNumber,ChildItemBrand,ChildItemPhotoURL,Number,Unit,ReservedNumber,LossRate,DefaultProcess," + strNewVerID + "," + strNewVerID + ",BelongItemCode," + strNewVerID + ",KeyWord +" + "'" + strKeyWord + "'" + ",ParentKeyWord +" + "'" + strKeyWord + "'" + ",PULeadTime ,MFLeadTime  ,HRCost ,MTCost ,MFCost,PurchasePrice,SalePrice  ,SortNumber From T_ItemBom ";
            strHQL += " Where ItemCode = " + "'" + strItemCode + "'" + " and ParentItemCode = ChildItemCode and VerID = " + strOldVerID;
            strHQL += " and BelongItemCode = '" + strItemCode + "' And BelongVerID = " + strOldVerID;
            strHQL += " Order By ID ASC";
            ShareClass.RunSqlCommand(strHQL);



            strHQL = @" Insert Into T_ItemBom (ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,ChildItemModelNumber,ChildItemBrand,ChildItemPhotoURL,Number,Unit,ReservedNumber,LossRate,DefaultProcess,ChildItemVerID,VerID,BelongItemCode,BelongVerID,KeyWord,ParentKeyWord,
               PULeadTime
              ,MFLeadTime
              ,HRCost
              ,MTCost
              ,MFCost
              ,PurchasePrice
              ,SalePrice
              ,SortNumber)";

            strHQL += " Select ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,ChildItemModelNumber,ChildItemBrand,ChildItemPhotoURL,Number,Unit,ReservedNumber,LossRate,DefaultProcess,ChildItemVerID," + strNewVerID + ",BelongItemCode," + strNewVerID + ",KeyWord +" + "'" + strKeyWord + "'" + ",ParentKeyWord +" + "'" + strKeyWord + "'" + ",PULeadTime ,MFLeadTime  ,HRCost ,MTCost ,MFCost,PurchasePrice,SalePrice  ,SortNumber From T_ItemBom "; 
            strHQL += " Where ParentItemCode <> ChildItemCode";
            strHQL += " and BelongItemCode = '" + strItemCode + "' And BelongVerID = " + strOldVerID;
            strHQL += " Order By ID ASC";
            ShareClass.RunSqlCommand(strHQL);

            try
            {
                DL_VersionID.SelectedValue = DL_NewVersionID.SelectedValue;
                DL_ToItemVersionID.SelectedValue = DL_NewVersionID.SelectedValue;
                TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strNewVerID, strItemCode, strNewVerID, TreeView1);
                LB_ItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strNewVerID, strItemCode, strNewVerID).ToString("F2");

            }
            catch
            {
            }

            DL_ChildVersionID.SelectedValue = DL_NewVersionID.SelectedValue;
            TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strNewVerID, strItemCode, strNewVerID, TreeView2);
            LB_ChildItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strNewVerID, strItemCode, strNewVerID).ToString();

            LB_TopItemCode.Text = strItemCode;
            LB_TopItemName.Text = GetItemName(strItemCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZCG").ToString().Trim() + "')", true);
        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_CopyVersionAB_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strFromItemVerID, strToItemVerID;
        string strFromItemCode, strToItemCode;
        string strUnit;
        string strDefaultProcess, strKeyWord, strFromKeyWord, strFromParentKeyWord;
        decimal deReservedNumber;

        strKeyWord = DateTime.Now.ToString("yyyyMMddHHMMssff");

        strFromItemVerID = DL_FromVersionID.SelectedItem.Text.Trim();
        strToItemVerID = DL_ToItemVersionID.SelectedItem.Text.Trim();

        strFromItemCode = LB_ChildItemCode.Text.Trim();
        strToItemCode = LB_TopItemCode.Text.Trim();

        strUnit = DL_ChildItemUnitToBom.SelectedValue.Trim();

        strDefaultProcess = TB_DefaultProcess.Text.Trim();
        deReservedNumber = NB_ChildItemNumberToBom.Amount;

        DataSet ds = GetFromItemBomDataSet(strFromItemCode, strFromItemVerID);
        strFromKeyWord = ds.Tables[0].Rows[0]["KeyWord"].ToString().Trim() + strKeyWord;
        strFromParentKeyWord = ds.Tables[0].Rows[0]["ParentKeyWord"].ToString().Trim() + strKeyWord;

        Item item = GetItem(strToItemCode);

        try
        {
            strHQL = "delete from T_ItemBom where VerID = " + strToItemVerID + " and ItemCode = " + "'" + strToItemCode + "'";
            strHQL += " and BelongItemCode = '" + strToItemCode + "' And BelongVerID = " + strToItemVerID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = @" Insert Into T_ItemBom (ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,ChildItemModelNumber,ChildItemBrand,ChildItemPhotoURL,Number,Unit,DefaultProcess,ReservedNumber,LossRate,ChildItemVerID,VerID,BelongItemCode,BelongVerID,KeyWord,ParentKeyWord,
               PULeadTime            
              ,MFLeadTime
              ,HRCost
              ,MTCost
              ,MFCost
              ,PurchasePrice
              ,SalePrice
              ,SortNumber)";
            strHQL += " Values(" + "'" + strToItemCode + "'" + "," + "'" + strToItemCode + "'" + "," + "'" + strToItemCode + "','" + item.ItemName + "','" + item.SmallType + "','" + item.Specification + "','" + item.ModelNumber + "','" + item.Brand + "','" + item.PhotoURL + "',1," + "'" + strUnit + "'" + "," + "'" + strDefaultProcess + "'" + "," + deReservedNumber.ToString() + ",0," + strToItemVerID + "," + strToItemVerID + ",'" + strToItemCode + "'," + strToItemVerID + ",'" + strFromKeyWord + "','" + strFromParentKeyWord + "'," + item.PULeadTime.ToString() + "," + item.MFLeadTime.ToString() +"," + item.HRCost.ToString() +"," + item.MTCost.ToString() +"," + item.MFCost.ToString() + "," +item.PurchasePrice.ToString() + ","+item.SalePrice.ToString() + ",0)";

            ShareClass.RunSqlCommand(strHQL);

            strHQL = @" Insert Into T_ItemBom (ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,ChildItemModelNumber,ChildItemBrand,ChildItemPhotoURL,Number,Unit,ReservedNumber,LossRate,DefaultProcess,ChildItemVerID,VerID,BelongItemCode,BelongVerID,KeyWord,ParentKeyWord,
               PULeadTime            
              ,MFLeadTime
              ,HRCost
              ,MTCost
              ,MFCost
              ,PurchasePrice
              ,SalePrice
              ,SortNumber)";
            strHQL += " Select " + "'" + strToItemCode + "'" + "," + "'" + strToItemCode + "'" + ",ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,ChildItemModelNumber,ChildItemBrand,ChildItemPhotoURL,Number,Unit,ReservedNumber,LossRate,DefaultProcess,ChildItemVerID," + strToItemVerID + ",'" + strToItemCode + "'," + strToItemVerID + ",KeyWord +" + "'" + strKeyWord + "'" + ",ParentKeyWord +" + "'" + strKeyWord + "'" + ",PULeadTime,MFLeadTime,HRCost ,MTCost ,MFCost ,PurchasePrice ,SalePrice ,SortNumber From T_ItemBom ";
            strHQL += " Where ParentItemCode <> ChildItemCode ";
            strHQL += " and BelongItemCode = '" + strFromItemCode + "' And BelongVerID = " + strFromItemVerID;
            strHQL += " Order By ID ASC";
            ShareClass.RunSqlCommand(strHQL);

            TakeTopBOM.InitialItemBomTreeForNew(strToItemCode, strToItemVerID, strToItemCode, strToItemVerID, TreeView1);
            LB_ItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strToItemCode, strToItemVerID, strToItemCode, strToItemVerID).ToString("F2");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected DataSet GetFromItemBomDataSet(string strFromItemCode, string strFromItemVerID)
    {
        string strHQL;

        strHQL = "Select KeyWord,ParentKeyWord From T_ItemBom ";
        strHQL += " Where ItemCode = " + "'" + strFromItemCode + "'" + " and ItemCode = ParentItemCode And ParentItemCode = ChildItemCode and VerID = " + strFromItemVerID;
        strHQL += " and BelongItemCode = '" + strFromItemCode + "' And BelongVerID = " + strFromItemVerID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemBom");

        return ds;
    }

    protected void DL_VersionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strVerID, strItemCode;
        string strVerType, strItemBomRelatedType, strItemBomRelatedID;

        strID = DL_VersionID.SelectedValue.Trim();
        strVerID = DL_VersionID.SelectedItem.Text.Trim();
        strItemCode = LB_TopItemCode.Text.Trim();

        //LB_ToItemVerID.Text = strVerID;
        DL_ToItemVersionID.SelectedValue = strID;

        ItemBomVersion itemBomVersion = GetItemBomVersion(strItemCode, strVerID);

        if (itemBomVersion != null)
        {
            strItemBomRelatedType = itemBomVersion.RelatedType.Trim();
            strItemBomRelatedID = itemBomVersion.RelatedID.ToString();
            if (strRelatedType == strItemBomRelatedType & strRelatedID == strItemBomRelatedID)
            {
                BT_CopyVersionAB.Enabled = true;
            }
            else
            {
                BT_CopyVersionAB.Enabled = false;
            }

            strVerType = itemBomVersion.Type.Trim();
            DL_ChangeVersionType.SelectedValue = strVerType;
        }

        HL_ItemRelatedDoc.NavigateUrl = "TTItemRelatedDoc.aspx?ItemBomID=" + DL_VersionID.SelectedValue.Trim();

        try
        {
            TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView1);

            LB_ItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString("F2");
        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
        }
    }


    protected void DL_ToItemVersionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strVerID, strItemCode;
        string strVerType, strItemBomRelatedType, strItemBomRelatedID;

        strID = DL_ToItemVersionID.SelectedValue.Trim();
        strVerID = DL_ToItemVersionID.SelectedItem.Text.Trim();
        strItemCode = LB_TopItemCode.Text.Trim();

        DL_VersionID.SelectedValue = strID;

        //LB_ToItemVerID.Text = strVerID;

        ItemBomVersion itemBomVersion = GetItemBomVersion(strItemCode, strVerID);

        strItemBomRelatedType = itemBomVersion.RelatedType.Trim();
        strItemBomRelatedID = itemBomVersion.RelatedID.ToString();
        if (strRelatedType == strItemBomRelatedType & strRelatedID == strItemBomRelatedID)
        {
            BT_CopyVersionAB.Enabled = true;
        }
        else
        {
            BT_CopyVersionAB.Enabled = false;
        }

        strVerType = itemBomVersion.Type.Trim();
        DL_ChangeVersionType.SelectedValue = strVerType;

        try
        {
            TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView1);
            LB_ItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString("F2");

        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
        }
    }

    protected void DL_ChangeVersionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strType, strItemCode;
        string strHQL;

        if (DL_VersionID.Items.Count > 0)
        {
            strID = DL_VersionID.SelectedValue.Trim();
            strType = DL_ChangeVersionType.SelectedValue.Trim();

            strItemCode = LLB_ItemCode.SelectedValue.Trim();

            if (strType == "InUse")
            {
                strHQL = "update T_ItemBomVersion Set Type = 'Backup' where Type = 'InUse' and ItemCode = " + "'" + strItemCode + "'";
                ShareClass.RunSqlCommand(strHQL);
            }

            if (strType == "Baseline")
            {
                strHQL = "update T_ItemBomVersion Set Type = 'Backup' where Type = 'Baseline' and ItemCode = " + "'" + strItemCode + "'";
                ShareClass.RunSqlCommand(strHQL);
            }


            strHQL = "Update T_ItemBomVersion Set Type = " + "'" + strType + "'" + " Where ID = " + strID;

            try
            {
                ShareClass.RunSqlCommand(strHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBBLXYGG").ToString().Trim() + "')", true);
            }
            catch
            {
            }
        }
    }

    protected void DL_ExpandType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strVerID, strItemCode;

        strItemCode = LLB_ItemCode.SelectedValue.Trim();

        strVerID = DL_VersionID.SelectedItem.Text.Trim();

        try
        {
            TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView1);
            LB_ItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString("F2");

        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
        }
    }

    protected void DL_ChildVersionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strVerID, strItemCode;
        string strVerType;

        strID = DL_ChildVersionID.SelectedValue.Trim();
        strVerID = DL_ChildVersionID.SelectedItem.Text.Trim();
        strItemCode = LB_ChildItemCode.Text.Trim();

        HL_ItemRelatedDoc.NavigateUrl = "TTItemRelatedDoc.aspx?ItemBomID=" + DL_ChildVersionID.SelectedValue.Trim();

        strVerType = GetItemBomVersionType(strItemCode, strVerID);
        DL_ChangeChildVersionType.SelectedValue = strVerType;

        try
        {
            TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView2);
            LB_ChildItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString();

        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strVerID;
        string strEditStatus;

        int intDepth;

        try
        {
            strEditStatus = DL_EditStatus.SelectedValue.Trim();
            strVerID = DL_VersionID.SelectedItem.Text;

            TreeNode treeNode = new TreeNode();
            treeNode = TreeView1.SelectedNode;

            intDepth = treeNode.Depth;
            LB_TreeDepth.Text = intDepth.ToString();

            strID = treeNode.ToolTip;

            strHQL = "From ItemBom as itemBom where itemBom.ID = " + strID;
            ItemBomBLL itemBomBLL = new ItemBomBLL();
            ItemBom itemBom = new ItemBom();
            lst = itemBomBLL.GetAllItemBoms(strHQL);
            itemBom = (ItemBom)lst[0];

            LB_ItemBomID.Text = strID;
            NB_SelectBOMNumber.Amount = itemBom.Number;
            NB_SelectBOMReservedNumber.Amount = itemBom.ReservedNumber;
            DL_SelectBOMUnit.SelectedValue = itemBom.Unit;

            LB_SelectItemCode.Text = itemBom.ChildItemCode;
            LB_SelectItemName.Text = itemBom.ChildItemName;
            NB_SelectItemLossRate.Amount = itemBom.LossRate;
            NB_SortNumber.Amount = itemBom.SortNumber;

            LB_SelectKeyWord.Text = itemBom.KeyWord.Trim();
            LB_SelectParentKeyWord.Text = itemBom.ParentKeyWord.Trim();

            TB_BomDefaultProcess.Text = itemBom.DefaultProcess.Trim();

            try
            {
                LB_SelectType.Text = GetItem(strChildItemCodes).Type;
            }
            catch
            {
            }
            TB_SelectModelNumber.Text = itemBom.ChildItemModelNumber;
            TB_SelectSpecification.Text = itemBom.ChildItemSpecification;
            TB_SelectBrand.Text = itemBom.ChildItemBrand;

            HL_ItemBomRelatedDoc.NavigateUrl = "TTDocumentTreeView.aspx?RelatedType=BOM&RelatedItemCode=" + itemBom.ChildItemCode + "&RelatedID=" + itemBom.VerID.ToString();

            if (strEditStatus == "YES")
            {
                BT_AddToBom.Enabled = true;
                BT_UpdateFormBom.Enabled = true;
                BT_DeleteFormBom.Enabled = true;
            }
            else
            {
                BT_AddToBom.Enabled = false;
                BT_UpdateFormBom.Enabled = false;
                BT_DeleteFormBom.Enabled = false;
            }
        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
        }
    }

    protected void DL_ProductProcess1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strProcessName = DL_ProductProcess1.SelectedValue.Trim();

        string strBomProcess = TB_DefaultProcess.Text.Trim();

        if (strBomProcess != "")
        {
            TB_DefaultProcess.Text = strBomProcess + "," + strProcessName;
        }
        else
        {
            TB_DefaultProcess.Text = strProcessName;
        }
    }

    protected void DL_BigType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strBigType;

        strBigType = DL_BigType.SelectedValue.Trim();

        LoadSmallTypeList(strBigType);
    }

    protected void LoadSmallTypeList(string strBigType)
    {
        string strHQL;
        IList lst;

        if (strBigType == "Goods")
        {
            strHQL = "From GoodsType as goodsType Order By SortNumber ASC";
            GoodsTypeBLL goodsTypeBLL = new GoodsTypeBLL();
            lst = goodsTypeBLL.GetAllGoodsTypes(strHQL);

            DL_SmallType.DataSource = lst;
            DL_SmallType.DataBind();
        }

        if (strBigType == "Asset" | strBigType == "Material")
        {
            strHQL = "From AssetType as assetType Order By SortNumber ASC";
            AssetTypeBLL assetTypeBLL = new AssetTypeBLL();
            lst = assetTypeBLL.GetAllAssetTypes(strHQL);

            DL_SmallType.DataSource = lst;
            DL_SmallType.DataBind();
        }

        DL_SmallType.Items.Insert(0, new ListItem("--Select--", ""));

    }

    protected void BT_TakePhoto_Click(object sender, EventArgs e)
    {
        Panel2.Visible = true;
    }

    protected void BT_DeletePhoto_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strItemCode = TB_ItemCode.Text.Trim();

        try
        {
            strHQL = "Update T_Item Set PhotoURL = '' Where ItemCode = " + "'" + strItemCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_ItemPhoto.ImageUrl = "";
            HL_ItemPhoto.NavigateUrl = "";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_UploadPhoto_Click(object sender, EventArgs e)
    {
        if (this.FUP_File.PostedFile != null)
        {
            string strFileName1 = FUP_File.PostedFile.FileName.Trim();
            string strLoginUserCode = Session["UserCode"].ToString().Trim();
            string strItemCode = TB_ItemCode.Text.Trim();

            string strHQL;
            int i;

            if (strFileName1 != "")
            {
                //获取初始文件名
                i = strFileName1.LastIndexOf("."); //取得文件名中最后一个"."的索引
                string strNewExt = strFileName1.Substring(i); //获取文件扩展名

                DateTime dtUploadNow = DateTime.Now; //获取系统时间

                string strFileName2 = System.IO.Path.GetFileName(strFileName1);
                string strExtName = Path.GetExtension(strFileName2);
                strFileName2 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtName;

                string strDocSavePath = Server.MapPath("Doc") + "\\UserPhoto\\";
                string strFileName3 = "Doc\\UserPhoto\\" + strFileName2;
                string strFileName4 = strDocSavePath + strFileName2;

                FileInfo fi = new FileInfo(strFileName4);

                if (fi.Exists)
                {
                    fi.Delete();
                }

                try
                {
                    FUP_File.PostedFile.SaveAs(strFileName4);

                    strHQL = "Update T_Item Set PhotoURL = " + "'" + strFileName3 + "'" + " Where ItemCode = " + "'" + strItemCode + "'";
                    ShareClass.RunSqlCommand(strHQL);

                    IM_ItemPhoto.ImageUrl = strFileName3;
                    HL_ItemPhoto.NavigateUrl = strFileName3;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSHANGCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
        }
    }

    protected void BT_SavePhoto_Click(object sender, EventArgs e)
    {
        string strItemCode;
        string strItemPhotoString;

        strItemCode = TB_ItemCode.Text.Trim();

        strItemPhotoString = TB_PhotoString1.Text.Trim();
        strItemPhotoString += TB_PhotoString2.Text.Trim();
        strItemPhotoString += TB_PhotoString3.Text.Trim();
        strItemPhotoString += TB_PhotoString4.Text.Trim();

        if (strItemPhotoString != "")
        {
            var binaryData = Convert.FromBase64String(strItemPhotoString);

            string strDateTime = DateTime.Now.ToString("yyyyMMddHHMMssff");
            string strItemPhotoURL = "Doc\\" + "UserPhoto\\" + strItemCode + strDateTime + ".jpg";
            var imageFilePath = Server.MapPath("Doc") + "\\UserPhoto\\" + strItemCode + strDateTime + ".jpg";

            if (File.Exists(imageFilePath))
            { File.Delete(imageFilePath); }
            var stream = new System.IO.FileStream(imageFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            stream.Write(binaryData, 0, binaryData.Length);
            stream.Close();

            string strHQL = "Update T_Item Set PhotoURL = " + "'" + strItemPhotoURL + "'" + " Where ItemCode = " + "'" + strItemCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_ItemPhoto.ImageUrl = strItemPhotoURL;
            HL_ItemPhoto.NavigateUrl = strItemPhotoURL;
        }
    }

    protected string GetItemPhotoURL(string strItemCode)
    {
        string strHQL = " from Item as item where item.ItemCode = " + "'" + strItemCode + "'";
        ItemBLL itemBLL = new ItemBLL();
        IList lst = itemBLL.GetAllItems(strHQL);
        Item item = (Item)lst[0];

        return item.PhotoURL.Trim();
    }

    protected void DL_PackingType_SelectedIndexChanged(object sender, EventArgs e)
    {
        TB_PackingType.Text = DL_PackingType.SelectedValue;
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strTopItemCode;
        string strVerType;
        string strItemCode, strItemName, strType, strBigType, strSmallType, strModelNumber, strSpecification, strBrand, strUnit, strDefaultProcess, strCurrencyType;
        decimal dePULeadTime, deMFLeadTime, dePurchasePrice, deSalePrice, deHRCost, deMTCost, deMFCost, deSafetyStock, deLossRate;

        string strParentCode, strKeyWord;
        int intWarrantyPeriod;


        strTopItemCode = LLB_ItemCode.SelectedValue.Trim();

        strParentCode = LB_SelectItemCode.Text.Trim();

        strKeyWord = DateTime.Now.ToString("yyyyMMddHHMMssff");


        strVerType = DL_VersionType.SelectedValue.Trim();

        strItemCode = TB_ItemCode.Text.Trim();
        strItemName = TB_ItemName.Text.Trim();
        strType = DL_ItemType.SelectedValue.Trim();
        strBigType = DL_BigType.SelectedValue.Trim();
        strSmallType = DL_SmallType.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpecification = TB_Specification.Text.Trim();
        strBrand = TB_Brand.Text.Trim();
        strUnit = DL_Unit.SelectedValue.Trim();

        dePurchasePrice = NB_PurchasePrice.Amount;
        deSalePrice = NB_SalePrice.Amount;
        deSafetyStock = NB_SalePrice.Amount;
        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
        dePULeadTime = NB_PULeadTime.Amount;
        deMFLeadTime = NB_MFLeadTime.Amount;
        deHRCost = NB_HRCost.Amount;
        deMTCost = NB_MTCost.Amount;
        deMFCost = NB_MFCost.Amount;

        deSafetyStock = NB_SafetyStock.Amount;
        deLossRate = NB_LossRate.Amount;

        strDefaultProcess = TB_DefaultProcess.Text.Trim();
        intWarrantyPeriod = int.Parse(NB_WarrantyPeriod.Amount.ToString());

        //检查是否包含非法字符
        if (CheckInvalidCharForString(strItemName) || CheckInvalidCharForString(strSpecification) || CheckInvalidCharForString(strModelNumber))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCGGXHBNBHDYHSYHBFBHQJC").ToString().Trim() + "')", true);
            return;
        }

        ItemBLL itemBLL = new ItemBLL();
        Item item = new Item();

        item.ItemCode = strItemCode;
        item.ItemName = strItemName;
        item.Type = strType;
        item.BigType = strBigType;
        item.SmallType = strSmallType;
        item.ModelNumber = strModelNumber;
        item.Specification = strSpecification;
        item.Brand = strBrand;
        item.Unit = strUnit;

        item.PULeadTime = dePULeadTime;
        item.MFLeadTime = deMFCost;

        item.PurchasePrice = dePurchasePrice;
        item.SalePrice = deSalePrice;
        item.CurrencyType = strCurrencyType;

        item.HRCost = deHRCost;
        item.MFCost = deMFCost;
        item.MTCost = deMTCost;
        item.MFLeadTime = deMFLeadTime;

        item.SafetyStock = deSafetyStock;
        item.LossRate = deLossRate;

        item.DefaultProcess = strDefaultProcess;
        item.WarrantyPeriod = intWarrantyPeriod;

        item.PhotoURL = "";

        item.RelatedType = strRelatedType;
        item.RelatedID = int.Parse(strRelatedID);

        item.RegistrationNumber = TB_RegistrationNumber.Text;
        item.PackingType = TB_PackingType.Text;


        try
        {
            itemBLL.AddItem(item);

            strHQL = "Insert Into T_ItemBomVersion(ItemCode,VerID,Type,RelatedType,RelatedID) Values(" + "'" + strItemCode + "'" + ",1,'Baseline'," + "'" + strRelatedType + "'" + "," + strRelatedID + ")";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = @"Insert Into T_ItemBom(ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,
                       ChildItemModelNumber,ChildItemBrand,Number,ReservedNumber,LossRate,Unit,ChildItemPhotoURL,DefaultProcess,PULeadTime,MFLeadTime,HRCost,MTCost,MFCost,
                       PurchasePrice,SalePrice,ChildItemVerID,VerID,BelongItemCode,BelongVerID,KeyWord,ParentKeyWord,SortNumber)";
            strHQL += " Values('" + strItemCode + "','" + strItemCode + "','" + strItemCode + "','" +strItemName + "','" + item.SmallType.Trim() + "','" + item.Specification.Trim() + "','" + item.ModelNumber.Trim() + "','" + item.Brand.Trim()  + "',1,0,0,"  + "'" + strUnit + "','" +item.PhotoURL.Trim() + "','" + strDefaultProcess + "'," + item.PULeadTime.ToString() + "," + item.MFLeadTime.ToString() + "," + deHRCost.ToString() + "," + deMTCost.ToString() + ","  + deMFCost.ToString() + "," +dePurchasePrice.ToString() + "," + deSalePrice.ToString()  + ",1,1,'" + strItemCode + "',1,'" + strItemCode + "1" + strItemCode + "1" + "','" + strItemCode + "1" + strItemCode + "1" + "',0)";
            ShareClass.RunSqlCommand(strHQL);

            LoadItemByItemType(strRelatedType, strRelatedID, strType, strItemCode);

            BT_AddToBom.Enabled = true;

            BT_TakePhoto.Enabled = true;
            BT_DeletePhoto.Enabled = true;

            IM_ItemPhoto.ImageUrl = "";
            HL_ItemPhoto.NavigateUrl = "";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBDMZFJC").ToString().Trim() + "')", true);
        }
    }

    //检查是否包含非法字符
    protected bool CheckInvalidCharForString(string strString)
    {
        if (strString.IndexOf("'") >= 0 | strString.IndexOf('\"') > 0 | strString.IndexOf("%") >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strOldItemName;

        string strItemCode, strItemName, strType, strBigType, strSmallType, strModelNumber, strSpecification, strBrand, strUnit, strDefaultProcess, strCurrencyType;
        decimal dePULeadTime, deMFLeadTime, dePurchasePrice, deSalePrice, deHRCost, deMTCost, deMFCost, deSafetyStock, deLossRate;
        int intWarrantyPeriod;

        strItemCode = TB_ItemCode.Text.Trim();
        strItemName = TB_ItemName.Text.Trim();
        strType = DL_ItemType.SelectedValue.Trim();
        strBigType = DL_BigType.SelectedValue.Trim();
        strSmallType = DL_SmallType.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpecification = TB_Specification.Text.Trim();
        strBrand = TB_Brand.Text.Trim();
        strUnit = DL_Unit.SelectedValue.Trim();

        dePULeadTime = NB_PULeadTime.Amount;
        deMFLeadTime = NB_MFLeadTime.Amount;

        dePurchasePrice = NB_PurchasePrice.Amount;
        deSalePrice = NB_SalePrice.Amount;
        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();

        deHRCost = NB_HRCost.Amount;
        deMTCost = NB_MTCost.Amount;
        deMFCost = NB_MFCost.Amount;

        deSafetyStock = NB_SafetyStock.Amount;
        deLossRate = NB_LossRate.Amount;

        strDefaultProcess = TB_DefaultProcess.Text.Trim();
        intWarrantyPeriod = int.Parse(NB_WarrantyPeriod.Amount.ToString());

        //检查是否包含非法字符
        if (CheckInvalidCharForString(strItemName) || CheckInvalidCharForString(strSpecification) || CheckInvalidCharForString(strModelNumber))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCGGXHBNBHDYHSYHBFBHQJC").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "From Item as item Where item.ItemCode = " + "'" + strItemCode + "'";
        ItemBLL itemBLL = new ItemBLL();
        lst = itemBLL.GetAllItems(strHQL);
        Item item = (Item)lst[0];

        strOldItemName = item.ItemName.Trim();

        item.ItemCode = strItemCode;
        item.ItemName = strItemName;
        item.Type = strType;
        item.BigType = strBigType;
        item.SmallType = strSmallType;
        item.ModelNumber = strModelNumber;
        item.Specification = strSpecification;
        item.Brand = strBrand;
        item.Unit = strUnit;

        item.PULeadTime = dePULeadTime;
        item.MFLeadTime = deMFCost;

        item.PurchasePrice = dePurchasePrice;
        item.SalePrice = deSalePrice;
        item.CurrencyType = strCurrencyType;

        item.HRCost = deHRCost;
        item.MTCost = deMTCost;
        item.MFCost = deMFCost;
        item.MFLeadTime = deMFLeadTime;

        item.SafetyStock = deSafetyStock;
        item.DefaultProcess = strDefaultProcess;
        item.WarrantyPeriod = intWarrantyPeriod;
        item.LossRate = deLossRate;

        item.RegistrationNumber = TB_RegistrationNumber.Text;
        item.PackingType = TB_PackingType.Text;

        item.RelatedType = strRelatedType;
        item.RelatedID = int.Parse(strRelatedID);

        try
        {
            itemBLL.UpdateItem(item, strItemCode);

            LoadItemByItemType(strRelatedType, strRelatedID, strType, strItemCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGXGSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strItemCode, strItemType;

        strItemCode = TB_ItemCode.Text.Trim();

        strHQL = "From ItemBom as itemBom where itemBom.ItemCode <> '" + strItemCode + "'";
        strHQL += " and (itemBom.ParentItemCode = '" + strItemCode + "'";
        strHQL += " Or itemBom.ChildItemCode = '" + strItemCode + "')";
        ItemBomBLL itemBomBLL = new ItemBomBLL();
        lst = itemBomBLL.GetAllItemBoms(strHQL);

        if (lst.Count <= 1)
        {
            strHQL = "Delete From T_Item Where ItemCode = " + "'" + strItemCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_ItemBomVersion Where ItemCode = " + "'" + strItemCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_ItemBom Where BelongItemCode = " + "'" + strItemCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            BT_TakePhoto.Enabled = false;
            BT_DeletePhoto.Enabled = false;

            BT_ExportToExcel.Enabled = false;

            strItemType = DL_ProjectItemType.SelectedValue.Trim();
            LoadItemByItemType(strRelatedType, strRelatedID, strItemType, "");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCWLCZYBOMDZBNSCJC").ToString().Trim() + "')", true);
        }
    }

    protected void btn_ExcelToDataTrainingForItem_Click(object sender, EventArgs e)
    {
        string strTopItemCode;
        string strVerType, strBigType;
        string strItemCode, strDefaultProcess, strCurrencyType;
        decimal dePULeadTime, deMFLeadTime, dePurchasePrice, deSalePrice, deHRCost, deMTCost, deMFCost, deSafetyStock, deLossRate;

        string strParentCode;
        int intWarrantyPeriod;

        if (ExelToDBTestForItem() == -1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click1111", "alert('" + LanguageHandle.GetWord("ZZDRSBEXECLBLDSJYCJC").ToString().Trim() + "')", true);
            Label58.Text = LanguageHandle.GetWord("ZZDRSBEXECLBLDSJYCJC").ToString().Trim();
            Label58.ForeColor = Color.Red;
            return;
        }
        else
        {
            strTopItemCode = LLB_ItemCode.SelectedValue.Trim();
            strParentCode = LB_SelectItemCode.Text.Trim();
            strVerType = DL_VersionType.SelectedValue.Trim();

            dePurchasePrice = NB_PurchasePrice.Amount;
            deSalePrice = NB_SalePrice.Amount;
            deSafetyStock = NB_SalePrice.Amount;
            strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
            dePULeadTime = NB_PULeadTime.Amount;
            deMFLeadTime = NB_MFLeadTime.Amount;
            deHRCost = NB_HRCost.Amount;
            deMTCost = NB_MTCost.Amount;
            deMFCost = NB_MFCost.Amount;

            deSafetyStock = NB_SafetyStock.Amount;
            deLossRate = NB_LossRate.Amount;

            strDefaultProcess = TB_DefaultProcess.Text.Trim();
            intWarrantyPeriod = int.Parse(NB_WarrantyPeriod.Amount.ToString());

            if (FileUpload_Item.HasFile == false)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click2222", "alert('" + LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim() + "')", true);
                return;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Item.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click3333", "alert('" + LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim() + "')", true);
                return;
            }
            string filename = FileUpload_Item.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim() + "');</script>");
            }
            else
            {
                FileUpload_Item.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
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
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click4444", "alert('" + LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() + "')", true);
                }
                else
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        strItemCode = dr[i][LanguageHandle.GetWord("DaiMa").ToString().Trim()].ToString().Trim();

                        if (strItemCode != "")
                        {
                            ItemBLL itemBLL = new ItemBLL();
                            string strHQL = "From Item as item Where item.ItemCode = " + "'" + strItemCode + "'";
                            IList lst = itemBLL.GetAllItems(strHQL);
                            if (lst != null && lst.Count > 0)//存在，则不操作
                            {

                            }
                            else//新增
                            {
                                Item item = new Item();

                                item.ItemCode = dr[i][LanguageHandle.GetWord("DaiMa").ToString().Trim()].ToString().Trim();
                                item.ItemName = dr[i][LanguageHandle.GetWord("MingChen").ToString().Trim()].ToString().Trim();
                                item.Type = dr[i][""+LanguageHandle.GetWord("ShuXing")+"（"+LanguageHandle.GetWord("CaiGouJian")+"、"+LanguageHandle.GetWord("ZiZhiJian")+"、"+LanguageHandle.GetWord("WaiXieJian")+"、"+LanguageHandle.GetWord("JiaoFuJian")+"）"].ToString().Trim();   
                                strBigType = dr[i][""+LanguageHandle.GetWord("DaLei")+"（"+LanguageHandle.GetWord("WuLiao")+"、"+LanguageHandle.GetWord("ZiChan")+"）"].ToString().Trim();   

                                item.BigType = "";
                                if (strBigType == LanguageHandle.GetWord("WuLiao").ToString().Trim())
                                {
                                    item.BigType = "Goods";
                                }
                                if (strBigType == "Assets")
                                {
                                    item.BigType = "Asset";
                                }


                                item.SmallType = dr[i][""+LanguageHandle.GetWord("XiaoLei")+"（"+LanguageHandle.GetWord("WuLiaoHuoZiChanLeiXing")+"）"].ToString().Trim();   
                                item.Specification = dr[i][LanguageHandle.GetWord("GuiGe").ToString().Trim()].ToString().Trim();
                                item.ModelNumber = dr[i][LanguageHandle.GetWord("XingHao").ToString().Trim()].ToString().Trim();
                                item.Brand = dr[i][LanguageHandle.GetWord("PinPai").ToString().Trim()].ToString().Trim();
                                item.Unit = dr[i][LanguageHandle.GetWord("ChanWei").ToString().Trim()].ToString().Trim();

                                item.RegistrationNumber = dr[i][LanguageHandle.GetWord("ZhuCeZhengHao").ToString().Trim()].ToString().Trim();
                                item.PackingType = dr[i]["PackagingMethod"].ToString().Trim();   

                                item.PULeadTime = dePULeadTime;
                                item.MFLeadTime = deMFCost;

                                item.PurchasePrice = dePurchasePrice;
                                item.SalePrice = deSalePrice;
                                item.CurrencyType = strCurrencyType;

                                item.HRCost = deHRCost;
                                item.MFCost = deMFCost;
                                item.MTCost = deMTCost;
                                item.MFLeadTime = deMFLeadTime;

                                item.SafetyStock = deSafetyStock;
                                item.LossRate = deLossRate;

                                item.DefaultProcess = strDefaultProcess;
                                item.WarrantyPeriod = intWarrantyPeriod;

                                item.PhotoURL = "";



                                item.RelatedType = strRelatedType;
                                item.RelatedID = int.Parse(strRelatedID);

                                itemBLL.AddItem(item);

                                strHQL = "Insert Into T_ItemBomVersion(ItemCode,VerID,Type,RelatedType,RelatedID) Values(" + "'" + strItemCode + "'" + ",1,'Baseline'," + "'" + strRelatedType + "'" + "," + strRelatedID + ")";
                                ShareClass.RunSqlCommand(strHQL);


                                strHQL = @"Insert Into T_ItemBom(ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,
                                        ChildItemModelNumber,ChildItemBrand,Number,ReservedNumber,LossRate,Unit,ChildItemPhotoURL,DefaultProcess,PULeadTime,MFLeadTime,HRCost,MTCost,MFCost,
                                        PurchasePrice,SalePrice,ChildItemVerID,VerID,BelongItemCode,BelongVerID,KeyWord,ParentKeyWord,SortNumber)";
                                strHQL += " Values('" + strItemCode + "','" + strItemCode + "','" + strItemCode + "','" + item.ItemName.Trim()+ "','" + item.SmallType.Trim() + "','" + item.Specification.Trim() + "','" + item.ModelNumber.Trim() + "','" + item.Brand.Trim() + "',1,0,0," + "'" + dr[i][LanguageHandle.GetWord("ChanWei").ToString().Trim()].ToString().Trim() + "','" + item.PhotoURL.Trim() + "','" + strDefaultProcess + "'," + item.PULeadTime.ToString() + "," + item.MFLeadTime.ToString() + "," + deHRCost.ToString() + "," + deMTCost.ToString() + "," + deMFCost.ToString() + "," + dePurchasePrice.ToString() + "," + deSalePrice.ToString() + ",1,1,'" + strItemCode + "',1,'" + strItemCode + "1" + strItemCode + "1" + "','" + strItemCode + "1" + strItemCode + "1" + "',0)";
                                ShareClass.RunSqlCommand(strHQL);

                            }
                            continue;
                        }
                    }

                    LoadItemByRelatedItemType(strRelatedType, strRelatedID);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click5555", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRCG").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected int ExelToDBTestForItem()
    {
        int j = 0;

        string strTopItemCode;
        string strVerType;
        string strItemCode, strDefaultProcess, strCurrencyType;
        decimal dePULeadTime, deMFLeadTime, dePurchasePrice, deSalePrice, deHRCost, deMTCost, deMFCost, deSafetyStock, deLossRate;

        string strParentCode;
        int intWarrantyPeriod;

        strTopItemCode = LLB_ItemCode.SelectedValue.Trim();
        strParentCode = LB_SelectItemCode.Text.Trim();
        strVerType = DL_VersionType.SelectedValue.Trim();

        dePurchasePrice = NB_PurchasePrice.Amount;
        deSalePrice = NB_SalePrice.Amount;
        deSafetyStock = NB_SalePrice.Amount;
        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
        dePULeadTime = NB_PULeadTime.Amount;
        deMFLeadTime = NB_MFLeadTime.Amount;
        deHRCost = NB_HRCost.Amount;
        deMTCost = NB_MTCost.Amount;
        deMFCost = NB_MFCost.Amount;

        deSafetyStock = NB_SafetyStock.Amount;
        deLossRate = NB_LossRate.Amount;

        strDefaultProcess = TB_DefaultProcess.Text.Trim();
        intWarrantyPeriod = int.Parse(NB_WarrantyPeriod.Amount.ToString());

        if (FileUpload_Item.HasFile == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click111", "alert('" + LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim() + "')", true);
            j = -1;
        }
        string IsXls = System.IO.Path.GetExtension(FileUpload_Item.FileName).ToString().ToLower();
        if (IsXls != ".xls" & IsXls != ".xlsx")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click222", "alert('" + LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim() + "')", true);
            j = -1;
        }
        string filename = FileUpload_Item.FileName.ToString();  //获取Execle文件名
        string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
        string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
        FileInfo fi = new FileInfo(strDocSavePath + newfilename);
        if (fi.Exists)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim() + "');</script>");
            j = -1;
        }
        else
        {
            FileUpload_Item.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click333", "alert('" + LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() + "')", true);
                j = -1;
            }
            else
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    strItemCode = dr[i][LanguageHandle.GetWord("DaiMa").ToString().Trim()].ToString().Trim();

                    if (strItemCode != "")
                    {
                        ItemBLL itemBLL = new ItemBLL();
                        string strHQL = "From Item as item Where item.ItemCode = " + "'" + strItemCode + "'";
                        IList lst = itemBLL.GetAllItems(strHQL);
                        if (lst != null && lst.Count > 0)//存在，则不操作
                        {

                        }
                        else//新增
                        {
                            CheckAndAddUnit(dr[i][LanguageHandle.GetWord("ChanWei").ToString().Trim()].ToString().Trim());

                            string strBigType = dr[i][""+LanguageHandle.GetWord("DaLei")+"（"+LanguageHandle.GetWord("WuLiao")+"、"+LanguageHandle.GetWord("ZiChan")+"）"].ToString().Trim();   
                            if (strBigType != LanguageHandle.GetWord("WuLiao").ToString().Trim() & strBigType != "Assets")
                            {
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click444", "alert('" +LanguageHandle.GetWord("ZZDaoRuShiBaiDaLeiZhiNengSheWe").ToString().Trim()+"')", true);
                                j = -1;
                            }

                            string strSmallType = dr[i][""+LanguageHandle.GetWord("XiaoLei")+"（"+LanguageHandle.GetWord("WuLiaoHuoZiChanLeiXing")+"）"].ToString().Trim();   
                            if (CheckSmallType(strSmallType, strBigType) == 0)
                            {
                                if (strBigType != LanguageHandle.GetWord("WuLiao").ToString().Trim())
                                {
                                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click555", "alert('" +LanguageHandle.GetWord("ZZDaoRuShiBaiXiaoLeistrSmallTy").ToString().Trim()+"')", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click666", "alert('" +LanguageHandle.GetWord("ZZDaoRuShiBaiXiaoLeistrSmallTy").ToString().Trim()+"')", true);
                                }
                                j = -1;
                            }
                        }
                        continue;
                    }
                }
            }
        }

        return j;
    }

    protected void btn_ExcelToDataTrainingForItemBOM_Click(object sender, EventArgs e)
    {
        string strItemCode, strParentItemCode, strChildItemCode, strItemBOMVerID, strChildItemBOMVerID, strDefaultProcess, strUnit;
        string strBelongItemCode = "", strBelongVerID = "1";

        decimal deNumber, deReservedNumber, deLossRate;
        string strHQL;
        string strKeyWord;

        strKeyWord = DateTime.Now.ToString("yyyyMMddHHMMssff");

        ItemBomBLL itemBomBLL = new ItemBomBLL();
        ItemBLL itemBLL = new ItemBLL();

        DataSet ds1, ds2, ds3;

        if (ExelToDBTestForItemBOM() == -1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click1111", "alert('" + LanguageHandle.GetWord("ZZDRSBEXECLBLDSJYCJC").ToString().Trim() + "')", true);

            Label59.Text = LanguageHandle.GetWord("ZZDRSBEXECLBLDSJYCJC").ToString().Trim();
            Label59.ForeColor = Color.Red;
            return;
        }
        else
        {
            if (FileUpload_ItemBom.HasFile == false)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click2222", "alert('" + LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim() + "')", true);
                return;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_ItemBom.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click3333", "alert('" + LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim() + "')", true);
                return;
            }
            string filename = FileUpload_ItemBom.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim() + "');</script>");
            }
            else
            {
                FileUpload_ItemBom.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                string strpath = strDocSavePath + newfilename;

                DataTable dt1;
                dt1 = new DataTable();
                dt1 = MSExcelHandler.ReadExcelToDataTable(strpath, "");
                DataRow[] dr = dt1.Select();  //定义一个DataRow数组
                int rowsnum = dt1.Rows.Count;

                if (rowsnum == 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click4444", "alert('" + LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() + "')", true);
                }
                else
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        strItemCode = dr[i][LanguageHandle.GetWord("WuLiaoDaiMa").ToString().Trim()].ToString().Trim();

                        if (strItemCode != "")
                        {
                            strItemBOMVerID = dr[i][LanguageHandle.GetWord("BOMBanBenHao").ToString().Trim()].ToString().Trim();
                            strParentItemCode = dr[i][LanguageHandle.GetWord("FuWuLiaoDaiMa").ToString().Trim()].ToString().Trim();
                            strChildItemCode = dr[i][LanguageHandle.GetWord("ZiWuLiaoDaiMa").ToString().Trim()].ToString().Trim();
                            strChildItemBOMVerID = dr[i][LanguageHandle.GetWord("ZiWuLiaoBOMBanBenHao").ToString().Trim()].ToString().Trim();
                            deNumber = decimal.Parse(dr[i][LanguageHandle.GetWord("ZiWuLiaoShuLiang").ToString().Trim()].ToString().Trim());
                            deReservedNumber = decimal.Parse(dr[i][LanguageHandle.GetWord("BaoLiuLiang").ToString().Trim()].ToString().Trim());
                            deLossRate = decimal.Parse(dr[i][LanguageHandle.GetWord("SunHaoLv").ToString().Trim()].ToString().Trim());
                            strUnit = dr[i][LanguageHandle.GetWord("ChanWei").ToString().Trim()].ToString().Trim();
                            strDefaultProcess = dr[i][LanguageHandle.GetWord("QueShengGongYi").ToString().Trim()].ToString().Trim();
                            strBelongItemCode = dr[i][LanguageHandle.GetWord("GuiShuWuLiaoDaiMa").ToString().Trim()].ToString().Trim();
                            strBelongVerID = dr[i][LanguageHandle.GetWord("GuiShuWuLiaoBOMBanBen").ToString().Trim()].ToString().Trim();

                            ItemBom itemBom = new ItemBom();

                            itemBom.ItemCode = strItemCode;
                            itemBom.ParentItemCode = strParentItemCode;
                            itemBom.ChildItemCode = strChildItemCode;
                            itemBom.VerID = int.Parse(strItemBOMVerID);
                            itemBom.ChildItemVerID = int.Parse(strChildItemBOMVerID);
                            itemBom.Number = deNumber;
                            itemBom.ReservedNumber = deReservedNumber;
                            itemBom.LossRate = deLossRate;
                            itemBom.Unit = strUnit;
                            itemBom.DefaultProcess = strDefaultProcess;
                            itemBom.BelongItemCode = strBelongItemCode;
                            itemBom.BelongVerID = int.Parse(strBelongVerID);
                            itemBom.KeyWord = strChildItemCode + strChildItemBOMVerID + strBelongItemCode + strBelongVerID;
                            itemBom.ParentKeyWord = strParentItemCode + strItemBOMVerID + strBelongItemCode + strBelongVerID;

                            Item childItem = GetItem(strChildItemCode);

                            itemBom.ChildItemName = childItem.ItemName.Trim();
                            itemBom.ChildItemType = childItem.SmallType.Trim();
                            itemBom.ChildItemSpecification = childItem.Specification.Trim();
                            itemBom.ChildItemModelNumber = childItem.ModelNumber.Trim();
                            itemBom.ChildItemBrand = childItem.Brand;
                            itemBom.ChildItemPhotoURL = childItem.PhotoURL.Trim();

                            itemBom.PurchasePrice = childItem.PurchasePrice;
                            itemBom.SalePrice = childItem.SalePrice;
                            itemBom.HRCost = childItem.HRCost;
                            itemBom.MFCost = childItem.MFCost;
                            itemBom.MTCost = childItem.MTCost;
                            itemBom.PULeadTime = childItem.PULeadTime;
                            itemBom.MFLeadTime = childItem.MFLeadTime;

                            itemBom.SortNumber = 1;

                            itemBomBLL.AddItemBom(itemBom);

                            strHQL = "Select * From T_ItemBomVersion Where ItemCode =" + "'" + strItemCode + "'" + " and VerID = " + strItemBOMVerID;
                            ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_ItemBomVersion");
                            if (ds2.Tables[0].Rows.Count == 0)
                            {
                                strHQL = "Insert Into T_ItemBomVersion(ItemCode,VerID,Type,RelatedType,RelatedID) Values(" + "'" + strItemCode + "'" + "," + strItemBOMVerID + ",'Baseline'," + "'" + strRelatedType + "'" + "," + strRelatedID + ")";
                                ShareClass.RunSqlCommand(strHQL);
                            }

                            strHQL = "Select * From T_ItemBomVersion Where ItemCode =" + "'" + strChildItemCode + "'" + " and VerID = " + strChildItemBOMVerID;
                            ds3 = ShareClass.GetDataSetFromSql(strHQL, "T_ItemBomVersion");
                            if (ds3.Tables[0].Rows.Count == 0)
                            {
                                strHQL = "Insert Into T_ItemBomVersion(ItemCode,VerID,Type,RelatedType,RelatedID) Values(" + "'" + strChildItemCode + "'" + "," + strChildItemBOMVerID + ",'Baseline'," + "'" + strRelatedType + "'" + "," + strRelatedID + ")";
                                ShareClass.RunSqlCommand(strHQL);
                            }
                        }

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click5555", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRCG").ToString().Trim() + "')", true);
                    }
                }
            }
        }

        strHQL = @"Insert Into T_ItemBOMVersion(ItemCode,VerID,Type,RelatedType,RelatedID)
                       Select ItemCode,1,'Baseline','SYSTEM',0 From T_Item 
                       Where ItemCode Not In (Select ItemCode From T_ItemBOMVersion)";
        ShareClass.RunSqlCommand(strHQL);

        //strHQL = @"Insert Into T_ItemBom(ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,
        //               ChildItemModelNumber,ChildItemBrand,Number,ReservedNumber,LossRate,Unit,ChildItemPhotoURL,DefaultProcess,PULeadTime,MFLeadTime,HRCost,MTCost,MFCost,
        //               PurchasePrice,SalePrice,ChildItemVerID,VerID,BelongItemCode,BelongVerID,KeyWord,ParentKeyWord)";
     

        strHQL = @"Insert Into T_ItemBOM(ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,
                       ChildItemModelNumber,ChildItemBrand,Number,Unit,VerID,ChildItemVerID,ReservedNumber,LossRate,ChildItemPhotoURL,DefaultProcess,PULeadTime,MFLeadTime,HRCost,MTCost,MFCost,
                       PurchasePrice,SalePrice,SortNumber,BelongItemCode,BelongVerID,KeyWord,ParentKeyWord) 
                 
                  Select ItemCode,ItemCode,ItemCode,ItemName,ItemType,Specification,
                       ModelNumber,Brand,1,Unit,1,1,0,0,PhotoURL,DefaultProcess,PULeadTime,MFLeadTime,HRCost,MTCost,MFCost,
                       PurchasePrice,SalePrice,0,ItemCode,1,ItemCode ||'1' || ItemCode ||'1',ItemCode || '1' || ItemCode || '1' From T_Item 
                       Where ItemCode Not in (Select ItemCode From T_ItemBOM Where ItemCode = ParentItemCode and ItemCode = ChildItemCode And KeyWord = ParentKeyWord )";

       
        ShareClass.RunSqlCommand(strHQL);
    }

    protected int ExelToDBTestForItemBOM()
    {
        string strItemCode, strParentItemCode, strChildItemCode, strItemBOMVerID, strChildItemBOMVerID, strDefaultProcess, strUnit;
        decimal deNumber, deReservedNumber, deLossRate;
        string strHQL;

        ItemBomBLL itemBomBLL = new ItemBomBLL();
        ItemBLL itemBLL = new ItemBLL();
        IList lst1, lst2, lst3;

        DataSet ds1;


        if (FileUpload_ItemBom.HasFile == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click1111", "alert('" + LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim() + "')", true);
            return -1;
        }
        string IsXls = System.IO.Path.GetExtension(FileUpload_ItemBom.FileName).ToString().ToLower();
        if (IsXls != ".xls" & IsXls != ".xlsx")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click2222", "alert('" + LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim() + "')", true);
            return -1;
        }
        string filename = FileUpload_ItemBom.FileName.ToString();  //获取Execle文件名
        string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
        string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
        FileInfo fi = new FileInfo(strDocSavePath + newfilename);
        if (fi.Exists)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim() + "');</script>");
            return -1;
        }
        else
        {
            FileUpload_ItemBom.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);

            string strpath = strDocSavePath + newfilename;
            DataTable dt1;
            dt1 = new DataTable();
            dt1 = MSExcelHandler.ReadExcelToDataTable(strpath, "");
            DataRow[] dr = dt1.Select();  //定义一个DataRow数组
            int rowsnum = dt1.Rows.Count;

            if (rowsnum == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click3333", "alert('" + LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() + "')", true);
                return -1;
            }
            else
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    strItemCode = dr[i][LanguageHandle.GetWord("WuLiaoDaiMa").ToString().Trim()].ToString().Trim();

                    if (strItemCode != "")
                    {
                        strItemBOMVerID = dr[i][LanguageHandle.GetWord("BOMBanBenHao").ToString().Trim()].ToString().Trim();
                        strParentItemCode = dr[i][LanguageHandle.GetWord("FuWuLiaoDaiMa").ToString().Trim()].ToString().Trim();
                        strChildItemCode = dr[i][LanguageHandle.GetWord("ZiWuLiaoDaiMa").ToString().Trim()].ToString().Trim();
                        strChildItemBOMVerID = dr[i][LanguageHandle.GetWord("ZiWuLiaoBOMBanBenHao").ToString().Trim()].ToString().Trim();

                        try
                        {
                            deNumber = decimal.Parse(dr[i][LanguageHandle.GetWord("ZiWuLiaoShuLiang").ToString().Trim()].ToString().Trim());
                            deReservedNumber = decimal.Parse(dr[i][LanguageHandle.GetWord("BaoLiuLiang").ToString().Trim()].ToString().Trim());
                            deLossRate = decimal.Parse(dr[i][LanguageHandle.GetWord("SunHaoLv").ToString().Trim()].ToString().Trim());
                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click4444", "alert('" + strItemCode + LanguageHandle.GetWord("ZZZIsNotNumber").ToString().Trim() + "')", true);
                            return -1;
                        }

                        strUnit = dr[i][LanguageHandle.GetWord("ChanWei").ToString().Trim()].ToString().Trim();
                        if (CheckUnit(strUnit) == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click5555", "alert('" + LanguageHandle.GetWord("ZZJGJCSJLBCZCDWDRIDWTOSTRINGTRIMJC").ToString().Trim() + "')", true);
                            return -1;
                        }

                        strDefaultProcess = dr[i][LanguageHandle.GetWord("QueShengGongYi").ToString().Trim()].ToString().Trim();

                        strHQL = "From Item as item Where item.ItemCode = " + "'" + strItemCode + "'";
                        lst1 = itemBLL.GetAllItems(strHQL);
                        if (lst1.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click6666", "alert('" + strItemCode + LanguageHandle.GetWord("IsNotExist").ToString().Trim() + "')", true);
                            return -1;
                        }

                        strHQL = "From Item as item Where item.ItemCode = " + "'" + strParentItemCode + "'";
                        lst2 = itemBLL.GetAllItems(strHQL);
                        if (lst2.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click7777", "alert('" + strParentItemCode + LanguageHandle.GetWord("IsNotExist").ToString().Trim() + "')", true);
                            return -1;
                        }
                        strHQL = "From Item as item Where item.ItemCode = " + "'" + strChildItemCode + "'";
                        lst3 = itemBLL.GetAllItems(strHQL);
                        if (lst3.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click8888", "alert('" + strChildItemCode + LanguageHandle.GetWord("IsNotExist").ToString().Trim() + "')", true);
                            return -1;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click9999", "alert('" + LanguageHandle.GetWord("ZZWLDMBNWK").ToString().Trim() + "')", true);
                        return -1;
                    }
                }
            }

            return 1;
        }
    }

    protected void BT_BOMItemFind_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strBOMChildItemCode = "%" + TB_BOMItemCode.Text.Trim() + "%";
        string strBOMChildItemName = "%" + TB_BOMItemName.Text.Trim() + "%";
        string strItemType = DL_BOMItemType.SelectedValue.Trim();


        if (strBOMChildItemCode != "")
        {
            strHQL = "Select ItemCode, ItemCode || '  ' ||  ItemName as ProjectItemName From T_Item Where";
            strHQL += " ItemCode in (Select ParentItemCode From T_ItemBOM Where ChildItemCode Like '" + strBOMChildItemCode + "')";
            strHQL += " and ItemName in (Select B.ItemName From T_ItemBOM A,T_Item B Where A.ChildItemCode = B.ItemCode and A.ChildItemCode Like '" + strBOMChildItemName + "')";
            strHQL += " and Type = " + "'" + strItemType + "'";

            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectItem");

            LLB_ItemCode.DataSource = ds;
            LLB_ItemCode.DataBind();

            LB_ResultNumber.Text = ds.Tables[0].Rows.Count.ToString();
        }
    }

    protected int CheckSmallType(string strType, string strBigType)
    {
        string strHQL;

        if (strBigType == LanguageHandle.GetWord("WuLiao").ToString().Trim())
        {
            strHQL = "Select Type From T_GoodsType Where Type = '" + strType + "'";
        }
        else
        {
            strHQL = "Select Type From T_AssetType Where Type = '" + strType + "'";
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsType");

        return ds.Tables[0].Rows.Count;
    }

    protected void CheckAndAddUnit(string strUnitName)
    {
        string strHQL;
        IList lst;

        strHQL = "from JNUnit as jnUnit Where jnUnit.UnitName = " + "'" + strUnitName + "'";
        JNUnitBLL jnUnitBLL = new JNUnitBLL();
        lst = jnUnitBLL.GetAllJNUnits(strHQL);

        JNUnit jnUnit = new JNUnit();

        if (lst.Count == 0)
        {
            jnUnit.UnitName = strUnitName;
            jnUnit.SortNumber = 100;

            jnUnitBLL.AddJNUnit(jnUnit);
        }
    }

    protected int CheckUnit(string strUnitName)
    {
        string strHQL;
        IList lst;

        strHQL = "from JNUnit as jnUnit Where jnUnit.UnitName = " + "'" + strUnitName + "'";
        JNUnitBLL jnUnitBLL = new JNUnitBLL();
        lst = jnUnitBLL.GetAllJNUnits(strHQL);

        return lst.Count;
    }

    protected void DL_ProductProcess2_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strProcessName = DL_ProductProcess2.SelectedValue.Trim();

        string strChildBomProcess = TB_ChildDefaultProcess.Text.Trim();

        TB_ChildDefaultProcess.Text = strChildBomProcess + "," + strProcessName;
    }

    protected void BT_AddToBom_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strItemCode;
        string strVerID, strVerType;
        string strChildItemCode, strChildItemName, strUnit, strDefaultProcess, strKeyWord, strSelectKeyWord, strSelectParentKeyWord;
        decimal deBomNumber, deReservedNumber, deBomItemLossRate;

        string strParentCode;
        string strEditStatus;

        string strChildVerID;

        strEditStatus = DL_EditStatus.SelectedValue.Trim();

        if (strEditStatus == "NO")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCBOMBCYBJZTBNJRJC").ToString().Trim() + "')", true);
            return;
        }

        strItemCode = LB_TopItemCode.Text.Trim();
        strParentCode = LB_SelectItemCode.Text.Trim();

        strVerID = DL_VersionID.SelectedItem.Text.Trim();
        strVerType = DL_VersionType.SelectedValue.Trim();

        strChildItemCode = LB_ChildItemCode.Text.Trim();
        strChildItemName = LB_ChildItemName.Text.Trim();
        deBomNumber = NB_ChildItemNumberToBom.Amount;
        strUnit = DL_ChildItemUnitToBom.SelectedValue.Trim();
        deReservedNumber = NB_ReservedNumber.Amount;

        strDefaultProcess = TB_ChildDefaultProcess.Text.Trim();
        deBomItemLossRate = NB_BomItemLossRate.Amount;

        strSelectKeyWord = LB_SelectKeyWord.Text.Trim();
        strSelectParentKeyWord = LB_SelectParentKeyWord.Text.Trim();

        strKeyWord = DateTime.Now.ToString("yyyyMMddHHMMssff");

        strChildItemCodes = "";

        try
        {
            GetNodes(TreeView2.Nodes[0], strChildItemCodes);
        }
        catch
        {
        }

        Item childItem = GetItem(strChildItemCode);

        if (strChildItemCodes.IndexOf(strItemCode + ",") < 0)
        {
            if (DL_ChildVersionID.Items.Count == 0)
            {
                if (CB_IncludeChildItem.Checked == false)
                {
                    strHQL = @"Insert Into T_ItemBom(ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,
                       ChildItemModelNumber,ChildItemBrand,ChildItemPhotoURL,PULeadTime,MFLeadTime,PurchasePrice,SalePrice,HRCost,MTCost,MFCost
                       ,Number,ReservedNumber,Unit,LossRate,DefaultProcess,ChildItemVerID,VerID,BelongItemCode,BelongVerID,KeyWord,ParentKeyWord)";
                    strHQL += " Values(" + "'" + strItemCode + "'" + "," + "'" + strParentCode + "'" + "," + "'" + strChildItemCode + "','" + childItem.ItemName + "','"
                        + childItem.SmallType + "','" + childItem.Specification + "','" + childItem.ModelNumber + "','" + childItem.Brand + "','" + childItem.PhotoURL + "',"
                        + childItem.PULeadTime + "," + childItem.MFLeadTime + "," + childItem.PurchasePrice + "," + childItem.SalePrice + "," + childItem.HRCost.ToString() + ","
                        + childItem.MTCost.ToString() + "," + childItem.MFCost.ToString() + "," + deBomNumber.ToString() + "," + deReservedNumber.ToString() + ","
                        + "'" + strUnit + "'" + "," + deBomItemLossRate.ToString() + "," + "'" + strDefaultProcess + "'" + ",1," + strVerID + ",'" + strItemCode + "',"
                        + strVerID + ",'" + strKeyWord + "','" + strSelectKeyWord + "')";

                    ShareClass.RunSqlCommand(strHQL);
                }
                else
                {
                    TakeTopBOM.AddItemWholeBomDataToTreeForNew(strChildItemCode, "1", strParentCode, strVerID, strItemCode, strVerID, strSelectKeyWord);
                }
            }
            else
            {
                strChildVerID = DL_ChildVersionID.SelectedItem.Text.Trim();

                if (CB_IncludeChildItem.Checked == false)
                {
                    strHQL = @" Insert Into T_ItemBom (ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,ChildItemModelNumber,ChildItemBrand,
                      ChildItemPhotoURL
                       ,PULeadTime,MFLeadTime,PurchasePrice,SalePrice,HRCost,MTCost,MFCost 
                       ,Number,ReservedNumber,Unit,LossRate,DefaultProcess,ChildItemVerID,VerID,BelongItemCode,BelongVerID,KeyWord,ParentKeyWord)";
                    strHQL += " Values (" + "'" + strItemCode + "'" + "," + "'" + strParentCode + "'" + "," + "'" + strChildItemCode + "','" + childItem.ItemName + "','"
                        + childItem.SmallType + "','" + childItem.Specification + "','" + childItem.ModelNumber + "','" + childItem.Brand + "','" + childItem.PhotoURL + "',"
                        + childItem.PULeadTime + "," + childItem.MFLeadTime + "," + childItem.PurchasePrice + "," + childItem.SalePrice + "," + childItem.HRCost.ToString() + ","
                        + childItem.MTCost.ToString() + "," + childItem.MFCost.ToString() + "," + deBomNumber.ToString() + "," + deReservedNumber.ToString() + ","
                        + "'" + strUnit + "'" + "," + deBomItemLossRate.ToString() + "," + "'" + strDefaultProcess + "'" + "," + strChildVerID + "," + strVerID + ",'"
                        + strItemCode + "'," + strVerID + ",'" + strKeyWord + "','" + strSelectKeyWord + "')";
                 
                    ShareClass.RunSqlCommand(strHQL);
                }
                else
                {
                    TakeTopBOM.AddItemWholeBomDataToTreeForNew(strChildItemCode, strChildVerID, strParentCode, strVerID, strItemCode, strVerID, strSelectKeyWord);
                }
            }

            try
            {
                TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView1);
                LB_ItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strItemCode, strVerID, strItemCode, strVerID).ToString("F2");

            }
            catch (Exception err)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCCPBNZWYJRDCPBOMDZJYWBOMZJBHCYJRBOMDCPFJBNZWZJDZJJC").ToString().Trim() + "')", true);
        }
    }
    protected void BT_ExportToExcel_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strItemCode;
        string strVerID, strVerType;
        string strChildItemCode, strChildItemName, strUnit, strDefaultProcess;
        decimal deBomNumber, deReservedNumber, deBomItemLossRate;

        string strParentCode;
        string strEditStatus;

        strEditStatus = DL_EditStatus.SelectedValue.Trim();

        strItemCode = LB_TopItemCode.Text.Trim();
        strParentCode = LB_SelectItemCode.Text.Trim();

        strVerID = DL_VersionID.SelectedItem.Text.Trim();
        strVerType = DL_VersionType.SelectedValue.Trim();

        strChildItemCode = LB_ChildItemCode.Text.Trim();
        strChildItemName = LB_ChildItemName.Text.Trim();
        deBomNumber = NB_ChildItemNumberToBom.Amount;
        strUnit = DL_ChildItemUnitToBom.SelectedValue.Trim();
        deReservedNumber = NB_ReservedNumber.Amount;

        strDefaultProcess = TB_ChildDefaultProcess.Text.Trim();
        deBomItemLossRate = NB_BomItemLossRate.Amount;

        string strRelatedType, strRelatedID;
        string strFileName;

        strRelatedType = strChildItemCode;
        strRelatedID = strVerID;

        strHQL = "Delete From T_ItemRelatedOrderBomToExpendDetailData Where RelatedType = " + "'" + strRelatedType + "'" + " and RelatedID = " + strRelatedID;
        ShareClass.RunSqlCommand(strHQL);

        try
        {
            TakeTopPlan.ItemMRPPExtendDetail(strChildItemCode, strVerID, 1, strUnit, strRelatedType, strRelatedID, strChildItemCode, strVerID);
        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
        }

        strFileName = "ITEM-" + strChildItemCode + "-Ver-" + " BOMData.xls";
        strHQL = "Select ItemCode as  \""+LanguageHandle.GetWord("TeLiaoDaiMa")+"\",ItemName as  \""+LanguageHandle.GetWord("WuLiaoMingCheng")+"\",Type as \""+LanguageHandle.GetWord("LeiXing")+"\",ModelNumber as \""+LanguageHandle.GetWord("XingHao")+"\",Specification as \""+LanguageHandle.GetWord("GuiGe")+"\",Brand as \""+LanguageHandle.GetWord("PinPai")+"\",Number as  \""+LanguageHandle.GetWord("YongLiang")+"\", Unit as \""+LanguageHandle.GetWord("DanWei")+"\" From T_ItemRelatedOrderBomToExpendDetailData Where RelatedType = " + "'" + strRelatedType + "'" + " and RelatedID = " + strRelatedID;   
        strHQL += " Order By OrderTime DESC";
        MSExcelHandler.DataTableToExcel(strHQL, strFileName);
    }

    protected void BT_CreateBOMReport_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strItemCode;
        string strVerID, strVerType;
        string strChildItemCode, strChildItemName, strUnit, strDefaultProcess;
        decimal deBomNumber, deReservedNumber, deBomItemLossRate;

        string strParentCode;
        string strEditStatus;

        strEditStatus = DL_EditStatus.SelectedValue.Trim();

        strItemCode = LB_TopItemCode.Text.Trim();
        strParentCode = LB_SelectItemCode.Text.Trim();

        strVerID = DL_VersionID.SelectedItem.Text.Trim();
        strVerType = DL_VersionType.SelectedValue.Trim();

        strChildItemCode = LB_ChildItemCode.Text.Trim();
        strChildItemName = LB_ChildItemName.Text.Trim();
        deBomNumber = NB_ChildItemNumberToBom.Amount;
        strUnit = DL_ChildItemUnitToBom.SelectedValue.Trim();
        deReservedNumber = NB_ReservedNumber.Amount;

        strDefaultProcess = TB_ChildDefaultProcess.Text.Trim();
        deBomItemLossRate = NB_BomItemLossRate.Amount;

        string strRelatedType, strRelatedID;

        strRelatedType = strChildItemCode;
        strRelatedID = strVerID;

        strHQL = "Delete From T_ItemRelatedOrderBomToExpendDetailData Where RelatedType = " + "'" + strRelatedType + "'" + " and RelatedID = " + strRelatedID;
        ShareClass.RunSqlCommand(strHQL);

        try
        {
            TakeTopPlan.ItemMRPPExtendDetail(strChildItemCode, strVerID, 1, strUnit, strRelatedType, strRelatedID, strChildItemCode, strVerID);
        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
        }

        string strURL = "TTItemBOMView.aspx?RelatedType=" + strRelatedType + "&RelatedID=" + strRelatedID;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "popShowByURL('" + strURL + "', 'Report'title, 800, 600,window.location);", true);
    }

    private void GetNodes(TreeNode tnParent, string strChildItemCode)
    {
        if (tnParent != null)
        {
            foreach (TreeNode tn in tnParent.ChildNodes)
            {
                strChildItemCode = GetItemCodeFormBom(tn.ToolTip) + ",";

                strChildItemCodes += strChildItemCode;

                GetNodes(tn, strChildItemCode);
            }
        }
    }

    private string GetItemCodeFormBom(string strBomID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ItemBom as itemBom Where itemBom.ID = " + strBomID;
        ItemBomBLL itemBomBLL = new ItemBomBLL();
        lst = itemBomBLL.GetAllItemBoms(strHQL);

        ItemBom itemBom = (ItemBom)lst[0];

        return itemBom.ChildItemCode.Trim();
    }

    protected void BT_DeleteFormBom_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strID, strVerID, strTopItemCode, strChildItemCode, strKeyWord;

        strVerID = DL_VersionID.SelectedItem.Text.Trim();
        strID = LB_ItemBomID.Text.Trim();
        strTopItemCode = LB_TopItemCode.Text.Trim();
        strChildItemCode = LB_ChildItemCode.Text.Trim();
        strKeyWord = TreeView1.SelectedNode.Target;

        try
        {
            strHQL = "Delete From T_ItemBom Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            //删除分支
            DeleteItemWholeBomDataToTreeForNew(strKeyWord);

            TakeTopBOM.InitialItemBomTreeForNew(strTopItemCode, strVerID, strTopItemCode, strVerID, TreeView1);
            if (strTopItemCode == strChildItemCode)
            {
                TakeTopBOM.InitialItemBomTreeForNew(strTopItemCode, strVerID, strTopItemCode, strVerID, TreeView2);
                LB_ItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strTopItemCode, strVerID, strTopItemCode, strVerID).ToString("F2");

            }

            BT_AddToBom.Enabled = false;
            BT_UpdateFormBom.Enabled = false;
            BT_DeleteFormBom.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }


    //删除引用BOM的整个分支节点
    public static void DeleteItemWholeBomDataToTreeForNew(string strParentKeyWord)
    {
        string strHQL, strHQLPart;
        IList lst;

        string strKeyWord;

        Item item = new Item();

        strHQL = "From ItemBom as itemBom where itemBom.ParentKeyWord = " + "'" + strParentKeyWord + "'";
        ItemBomBLL itemBomBLL = new ItemBomBLL();
        lst = itemBomBLL.GetAllItemBoms(strHQL);
        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                ItemBom itemBom = (ItemBom)lst[i];
                strKeyWord = itemBom.KeyWord.Trim();

                try
                {
                    strHQLPart = "Delete from T_ItemBom Where ID = " + itemBom.ID.ToString();
                    CoreShareClass.RunSqlCommand(strHQLPart);
                }
                catch
                {

                }

                DeleteItemWholeBomDataToTreeForNew(strKeyWord);
            }
        }
    }

    protected void DL_ProductProcess3_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strProcessName = DL_ProductProcess3.SelectedValue.Trim();
        string strBOMProcess = TB_BomDefaultProcess.Text.Trim();

        if (strBOMProcess != "")
        {
            TB_BomDefaultProcess.Text = strBOMProcess + "," + strProcessName;
        }
        else
        {
            TB_BomDefaultProcess.Text = strProcessName;
        }
    }

    protected void BT_UpdateFormBom_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strVerID, strTopItemCode, strUnit, strDefaultProcess;
        decimal deNumber, deReservedNumber, deLossRate;
        int intSortNumber;


        strVerID = DL_VersionID.SelectedItem.Text.Trim();
        strID = LB_ItemBomID.Text.Trim();
        strTopItemCode = LB_TopItemCode.Text.Trim();

        deNumber = NB_SelectBOMNumber.Amount;
        deReservedNumber = NB_SelectBOMReservedNumber.Amount;
        strUnit = DL_SelectBOMUnit.SelectedValue.Trim();

        strDefaultProcess = TB_BomDefaultProcess.Text.Trim();
        deLossRate = NB_SelectItemLossRate.Amount;
        intSortNumber = int.Parse(NB_SortNumber.Amount.ToString());

        try
        {
            strHQL = "From ItemBom as itemBom where itemBom.ID = " + strID;
            ItemBomBLL itemBomBLL = new ItemBomBLL();
            lst = itemBomBLL.GetAllItemBoms(strHQL);

            ItemBom itemBom = (ItemBom)lst[0];

            itemBom.Number = deNumber;
            itemBom.ReservedNumber = deReservedNumber;
            itemBom.Unit = strUnit;

            itemBom.DefaultProcess = strDefaultProcess;
            itemBom.LossRate = deLossRate;
            itemBom.SortNumber = intSortNumber;

            itemBomBLL.UpdateItemBom(itemBom, int.Parse(strID));

            TakeTopBOM.InitialItemBomTreeForNew(strTopItemCode, strVerID, strTopItemCode, strVerID, TreeView1);
            LB_ItemBomCost.Text = TakeTopBOM.SumItemBomCostForNew(strTopItemCode, strVerID, strTopItemCode, strVerID).ToString("F2");

            BT_AddToBom.Enabled = false;
            BT_DeleteFormBom.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGXGSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void LoadItemBomVersion(string strItemCode)
    {
        string strHQL;
        IList lst;

        string strChildItemCode, strTopItemCode;
        string strEditStatus;
        string strItemBomRelatedType, strItemBomRelatedID, strVerID;
        string strToItemCode, strToItemVerID;

        strChildItemCode = LB_ChildItemCode.Text.Trim();
        strTopItemCode = LB_TopItemCode.Text.Trim();

        strEditStatus = DL_EditStatus.SelectedValue.Trim();

        strHQL = "From ItemBomVersion as itemBomVersion Where itemBomVersion.ItemCode = " + "'" + strItemCode + "'" + " Order by itemBomVersion.VerID DESC";
        ItemBomVersionBLL itemBomVersionBLL = new ItemBomVersionBLL();
        lst = itemBomVersionBLL.GetAllItemBomVersions(strHQL);

        DL_OldVersionID.DataSource = lst;
        DL_OldVersionID.DataBind();

        DL_NewVersionID.DataSource = lst;
        DL_NewVersionID.DataBind();

        DL_FromVersionID.DataSource = lst;
        DL_FromVersionID.DataBind();

        DL_ChildVersionID.DataSource = lst;
        DL_ChildVersionID.DataBind();

        if (lst.Count > 0)
        {
            try
            {
                ItemBomVersion itemBomVersion = (ItemBomVersion)lst[0];
                DL_ChangeVersionType.SelectedValue = itemBomVersion.Type.Trim();
            }
            catch
            {
            }


            HL_ItemRelatedDoc.NavigateUrl = "TTItemRelatedDoc.aspx?ItemBomID=" + DL_ChildVersionID.SelectedValue.Trim();

        }

        if (strEditStatus == "NO")
        {
            DL_VersionID.DataSource = lst;
            DL_VersionID.DataBind();

            DL_ToItemVersionID.DataSource = lst;
            DL_ToItemVersionID.DataBind();
        }
        else
        {
            if (strTopItemCode == strChildItemCode)
            {
                DL_VersionID.DataSource = lst;
                DL_VersionID.DataBind();

                DL_ToItemVersionID.DataSource = lst;
                DL_ToItemVersionID.DataBind();
            }
        }

        if (lst.Count > 0)
        {
            strVerID = DL_NewVersionID.SelectedItem.Text.Trim();
            ItemBomVersion itemBomVersion = GetItemBomVersion(strItemCode, strVerID);

            strItemBomRelatedType = itemBomVersion.RelatedType.Trim();
            strItemBomRelatedID = itemBomVersion.RelatedID.ToString();
            if (strRelatedType == strItemBomRelatedType & strRelatedID == strItemBomRelatedID)
            {
                BT_CopyVersion.Enabled = true;
            }
            else
            {
                BT_CopyVersion.Enabled = false;
            }

            strToItemCode = LB_ToItemCode.Text.Trim();
            strToItemVerID = DL_ToItemVersionID.SelectedItem.Text.Trim();
            if (strToItemCode != "" & strToItemVerID != "")
            {
                if (strToItemVerID != "0")
                {
                    itemBomVersion = GetItemBomVersion(strToItemCode, strToItemVerID);

                    if (itemBomVersion != null)
                    {
                        strItemBomRelatedType = itemBomVersion.RelatedType.Trim();
                        strItemBomRelatedID = itemBomVersion.RelatedID.ToString();
                        if (strRelatedType == strItemBomRelatedType & strRelatedID == strItemBomRelatedID)
                        {
                            BT_CopyVersionAB.Enabled = true;
                        }
                        else
                        {
                            BT_CopyVersionAB.Enabled = false;
                        }
                    }
                }
            }

        }
    }

    protected int GetItemBomVersionCount(string strItemCode, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ItemBomVersion as itemBomVersion where itemBomVersion.ItemCode = " + "'" + strItemCode + "'" + " and itemBomVersion.VerID =" + strVerID;
        ItemBomVersionBLL itemBomVersionBLL = new ItemBomVersionBLL();
        lst = itemBomVersionBLL.GetAllItemBomVersions(strHQL);

        return lst.Count;
    }

    protected void LoadProductProcess()
    {
        string strHQL = "From ProductProcess as productProcess Order By productProcess.SortNumber ASC";
        ProductProcessBLL productProcessBLL = new ProductProcessBLL();
        IList lst = productProcessBLL.GetAllProductProcesss(strHQL);

        DL_ProductProcess1.DataSource = lst;
        DL_ProductProcess1.DataBind();

        DL_ProductProcess2.DataSource = lst;
        DL_ProductProcess2.DataBind();

        DL_ProductProcess3.DataSource = lst;
        DL_ProductProcess3.DataBind();
    }

    protected void LoadProjectItemBomVersion(string strItemCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From ItemBomVersion s itemBomVersion Where ItemCode = " + "'" + strItemCode + "'" + " Order by ID DESC";
        ItemBomVersionBLL itemBomVersionBLL = new ItemBomVersionBLL();
        lst = itemBomVersionBLL.GetAllItemBomVersions(strHQL);

        DL_VersionID.DataSource = lst;
        DL_VersionID.DataBind();

        DL_ToItemVersionID.DataSource = lst;
        DL_ToItemVersionID.DataBind();

        DL_OldVersionID.DataSource = lst;
        DL_OldVersionID.DataBind();

        DL_NewVersionID.DataSource = lst;
        DL_NewVersionID.DataBind();
    }

    protected void LoadItemByItemType(string strItemRelatedType, string strItemRelatedID, string strItemType, string strItemCode)
    {
        string strHQL;

        strHQL = "Select ItemCode, ItemCode || '  ' ||  ItemName as ProjectItemName From T_Item Where Type =" + "'" + strItemType + "'";


        if (strItemRelatedType == "SYSTEM")
        {
            strHQL += " and RelatedType = " + "'" + strItemRelatedType + "'";
            strHQL += " and RelatedID = '0'";
        }

        if (strItemRelatedType == "PROJECT")
        {
            if (strRelatedID != "0")
            {
                strHQL += " and RelatedType = " + "'" + strItemRelatedType + "'";
                strHQL += " and RelatedID = " + strItemRelatedID;
            }
            else
            {
                strHQL += " and RelatedType = " + "'" + strItemRelatedType + "'";
            }
        }

        if (strItemRelatedType == "OTHER")
        {
            strHQL += " and RelatedType != 'SYSTEM' and RelatedID != " + strItemRelatedID;
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectItem");

        LLB_ItemCode.DataSource = ds;
        LLB_ItemCode.DataBind();

        try
        {
            for (int i = 0; i < LLB_ItemCode.Items.Count; i++)
            {
                if (LLB_ItemCode.Items[i].Value.Trim() == strItemCode.Trim())
                {
                    LLB_ItemCode.Items[i].Selected = true;
                }
            }
        }
        catch
        {

        }
    }

    protected void LoadItemByRelatedItemType(string strItemRelatedType, string strItemRelatedID)
    {
        string strHQL;

        strHQL = "Select ItemCode, ItemCode || '  ' ||  ItemName as ProjectItemName From T_Item Where 1=1";


        if (strItemRelatedType == "SYSTEM")
        {
            strHQL += " and RelatedType = " + "'" + strItemRelatedType + "'";
            strHQL += " and RelatedID = '0'";
        }

        if (strItemRelatedType == "PROJECT")
        {
            if (strRelatedID != "0")
            {
                strHQL += " and RelatedType = " + "'" + strItemRelatedType + "'";
                strHQL += " and RelatedID = " + strItemRelatedID;
            }
            else
            {
                strHQL += " and RelatedType = " + "'" + strItemRelatedType + "'";
            }
        }

        if (strItemRelatedType == "OTHER")
        {
            strHQL += " and RelatedType != 'SYSTEM' and RelatedID != " + strItemRelatedID;
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectItem");

        LLB_ItemCode.DataSource = ds;
        LLB_ItemCode.DataBind();
    }

    protected void LoadUnit()
    {
        string strHQL;
        IList lst;

        strHQL = "from JNUnit as jnUnit order by jnUnit.SortNumber ASC";
        JNUnitBLL jnUnitBLL = new JNUnitBLL();
        lst = jnUnitBLL.GetAllJNUnits(strHQL);
        DL_Unit.DataSource = lst;
        DL_Unit.DataBind();

        DL_SelectBOMUnit.DataSource = lst;
        DL_SelectBOMUnit.DataBind();

        DL_ChildItemUnitToBom.DataSource = lst;
        DL_ChildItemUnitToBom.DataBind();
    }

    protected void LoadCurrencyType()
    {
        string strHQL;
        IList lst;

        strHQL = "From CurrencyType as currencyType Order By currencyType.SortNo ASC";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);

        DL_CurrencyType.DataSource = lst;
        DL_CurrencyType.DataBind();
    }

    protected void LoadPackingType()
    {
        string strHQL;

        strHQL = "Select Type From T_PackingType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_PackingType");

        DL_PackingType.DataSource = ds;
        DL_PackingType.DataBind();

        DL_PackingType.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected string GetItemBomVersionType(string strItemCode, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ItemBomVersion as itemBomVersion where itemBomVersion.ItemCode = " + "'" + strItemCode + "'" + " and itemBomVersion.VerID = " + strVerID;
        ItemBomVersionBLL itemBomVersionBLL = new ItemBomVersionBLL();
        lst = itemBomVersionBLL.GetAllItemBomVersions(strHQL);
        ItemBomVersion itemBomVersion = (ItemBomVersion)lst[0];

        return itemBomVersion.Type.Trim();
    }

    protected ItemBomVersion GetItemBomVersion(string strItemCode, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ItemBomVersion as itemBomVersion where itemBomVersion.ItemCode = " + "'" + strItemCode + "'" + " and itemBomVersion.VerID = " + strVerID;
        ItemBomVersionBLL itemBomVersionBLL = new ItemBomVersionBLL();
        lst = itemBomVersionBLL.GetAllItemBomVersions(strHQL);
        if (lst.Count > 0)
        {
            ItemBomVersion itemBomVersion = (ItemBomVersion)lst[0];
            return itemBomVersion;
        }
        else
        {
            return null;
        }
    }

    protected string GetItemBomID(string strItemCode, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ItemBomVersion as itemBomVersion where itemBomVersion.ItemCode = " + "'" + strItemCode + "'" + " and itemBomVersion.VerID = " + strVerID;
        ItemBomVersionBLL itemBomVersionBLL = new ItemBomVersionBLL();
        lst = itemBomVersionBLL.GetAllItemBomVersions(strHQL);
        ItemBomVersion itemBomVersion = (ItemBomVersion)lst[0];

        return itemBomVersion.ID.ToString();
    }

    protected static string GetItemName(string strItemCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Item as item where item.ItemCode = " + "'" + strItemCode + "'";
        ItemBLL itemBLL = new ItemBLL();
        lst = itemBLL.GetAllItems(strHQL);

        if (lst.Count > 0)
        {
            Item item = (Item)lst[0];
            return item.ItemName.Trim();
        }
        else
        {
            return "";
        }
    }

    public static decimal GetItemCost(string strItemCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Item as item where item.ItemCode = " + "'" + strItemCode + "'";
        ItemBLL itemBLL = new ItemBLL();
        lst = itemBLL.GetAllItems(strHQL);

        Item item = (Item)lst[0];

        return item.HRCost + item.MFCost + item.MTCost;
    }

    public static Item GetItem(string strItemCode)
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
