using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGoodsImport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = Session["UserCode"].ToString();
        string strUserName;

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            DLC_BuyTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strHQL = "from JNUnit as jnUnit order by jnUnit.SortNumber ASC";
            JNUnitBLL jnUnitBLL = new JNUnitBLL();
            lst = jnUnitBLL.GetAllJNUnits(strHQL);
            DL_Unit.DataSource = lst;
            DL_Unit.DataBind();

            strHQL = "from GoodsType as goodsType Order by goodsType.SortNumber ASC";
            GoodsTypeBLL goodsTypeBLL = new GoodsTypeBLL();
            lst = goodsTypeBLL.GetAllGoodsTypes(strHQL);
            DL_Type.DataSource = lst;
            DL_Type.DataBind();
            DL_Type.Items.Insert(0, new ListItem("--Select--", ""));

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);
            ShareClass.InitialWarehouseTreeByAuthorityAsset(TreeView3, strUserCode, strDepartString);

            //LoadWareHouseListByAuthority(strUserCode);

            LB_OwnerCode.Text = strUserCode;
            LB_OwnerName.Text = strUserName;

            //2013-07-18 Liujp 
            BindGoodsData(DL_Type.SelectedValue.Trim(), TB_WHName.Text.Trim());
        }
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView3.SelectedNode;

        if (treeNode.Target != "")
        {
            TB_WHName.Text = treeNode.Target.Trim();
        }
    }

    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        BindGoodsData(DL_Type.SelectedValue.Trim(), TB_WHName.Text.Trim());
    }

    protected void btn_ExcelToDataTraining_Click(object sender, EventArgs e)
    {
        string strGoodsCode = "";
        int i = 0;

        string strUserCode = LB_UserCode.Text;

        if (ExcelToDBTest() == -1)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZDRSBEXECLBLDSJYCJC");
            return;
        }
        else
        {
            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ") ;
                return;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ");
                return;
            }
            string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB") ;
            }
            else
            {
                FileUpload_Training.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
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
                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ") ;
                }
                else
                {
                    GoodsBLL goodsBLL = new GoodsBLL();
                    Goods goods = new Goods();

                    for (i = 0; i < dr.Length; i++)
                    {
                        strGoodsCode = dr[i][LanguageHandle.GetWord("DaiMa")].ToString().Trim();

                        if (strGoodsCode != "")
                        {
                            try
                            {
                                goods.GoodsCode = dr[i][LanguageHandle.GetWord("DaiMa")].ToString().Trim();
                                goods.GoodsName = dr[i][LanguageHandle.GetWord("MingChen")].ToString().Trim();
                                goods.Type = dr[i][LanguageHandle.GetWord("LeiXing")].ToString().Trim();
                                goods.Spec = dr[i][LanguageHandle.GetWord("GuiGe")].ToString().Trim();
                                goods.Manufacturer = dr[i][LanguageHandle.GetWord("PinPai")].ToString().Trim();
                                goods.ModelNumber = dr[i][LanguageHandle.GetWord("XingHao")].ToString().Trim();
                                goods.Number = decimal.Parse(dr[i][LanguageHandle.GetWord("ShuLiang")].ToString().Trim());
                                goods.Price = decimal.Parse(dr[i][LanguageHandle.GetWord("ChanJia")].ToString().Trim());
                                goods.IsTaxPrice = dr[i][LanguageHandle.GetWord("HanShui")].ToString().Trim();
                                goods.CurrencyType = dr[i][LanguageHandle.GetWord("BiBie")].ToString().Trim();
                                goods.UnitName = dr[i][LanguageHandle.GetWord("ChanWei")].ToString().Trim();
                                goods.Position = dr[i][LanguageHandle.GetWord("CunFangCangKu")].ToString().Trim();
                                goods.Manufacturer = dr[i]["Supplier"].ToString().Trim();
                                goods.OwnerCode = strUserCode;
                                goods.OwnerName = ShareClass.GetUserName(strUserCode);
                                goods.BuyTime = DateTime.Now;
                                goods.Memo = "";
                                goods.PhotoURL = "";
                                goods.Status = "InUse";

                                goods.BatchNumber = dr[i][LanguageHandle.GetWord("PiHao")].ToString().Trim();
                                goods.ProductDate = DateTime.Parse(dr[i][LanguageHandle.GetWord("ShengChanRiJi")].ToString());
                                goods.ExpiryDate = DateTime.Parse(dr[i][LanguageHandle.GetWord("ShiXiaoRiJi")].ToString()); ;
                                goods.ProductionEquipmentNumber = dr[i][LanguageHandle.GetWord("ShengChanSheBeiHao")].ToString().Trim();
                                goods.MaterialFormNumber = dr[i][LanguageHandle.GetWord("CaiZhiChanHao")].ToString().Trim();
                                goods.SN = dr[i][LanguageHandle.GetWord("XuLieHao")].ToString().Trim();

                                goodsBLL.AddGoods(goods);
                            }
                            catch (Exception err)
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGDRSBJC") + " : " + LanguageHandle.GetWord("HangHao") + ": " + (i + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa") + ": " + strGoodsCode + " : " + err.Message.ToString() + "<br/>"; ;
                            }
                        }
                    }

                    //2013-07-18 Liujp 
                    BindGoodsData(DL_Type.SelectedValue.Trim(), TB_WHName.Text.Trim());

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRCG") + "')", true);
                }
            }
        }
    }

    protected int ExcelToDBTest()
    {
        string strCIOID, strID, strGoodsCode, strManufacturer, strMemo, strIsTaxPrice;
        string strUserCode = LB_UserCode.Text;

        int j = 0;

        try
        {
            strCIOID = lbl_CheckId.Text.Trim();
            strID = LB_ID.Text.Trim();

            strManufacturer = TB_Manufacturer.Text.Trim();
            strMemo = TB_Memo.Text.Trim();

            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text +=
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ");
                j = -1;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ");
                j = -1;
            }
            string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB");
                j = -1;
            }
            else
            {
                FileUpload_Training.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
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
                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ");
                    j = -1;
                }
                else
                {
                    GoodsBLL goodsBLL = new GoodsBLL();
                    Goods goods = new Goods();

                    for (int i = 0; i < dr.Length; i++)
                    {
                        strGoodsCode = dr[i][LanguageHandle.GetWord("DaiMa")].ToString().Trim();

                        if (strGoodsCode.Length > 20)
                        {
                            LB_ErrorText.Text += LanguageHandle.GetWord("DaiMa") + ": " + strGoodsCode + " " + LanguageHandle.GetWord("ZZCDDY20WDRSB");
                            j = -1;

                            continue;
                        }

                        if (dr[i][LanguageHandle.GetWord("DaiMa")].ToString().Trim() != "")
                        {
                            goods.GoodsCode = dr[i][LanguageHandle.GetWord("DaiMa")].ToString().Trim();
                            goods.GoodsName = dr[i][LanguageHandle.GetWord("MingChen")].ToString().Trim();

                            if (CheckGoodsType(dr[i][LanguageHandle.GetWord("LeiXing")].ToString().Trim()) > 0)
                            {
                                goods.Type = dr[i][LanguageHandle.GetWord("LeiXing")].ToString().Trim();
                            }
                            else
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("DaiMa") + ": " + strGoodsCode + " " + LanguageHandle.GetWord("ZZJGJCSJLBCZCLPLXDRILXTOSTRINGTRIMJC") + " " + dr[i][LanguageHandle.GetWord("LeiXing")].ToString().Trim();
                                j = -1;
                            }
                            goods.Spec = dr[i][LanguageHandle.GetWord("GuiGe")].ToString().Trim();
                            goods.ModelNumber = dr[i][LanguageHandle.GetWord("XingHao")].ToString().Trim();
                            goods.Manufacturer = dr[i][LanguageHandle.GetWord("PinPai")].ToString().Trim();

                            try
                            {
                                goods.Number = decimal.Parse(dr[i][LanguageHandle.GetWord("ShuLiang")].ToString().Trim());
                                goods.Price = decimal.Parse(dr[i][LanguageHandle.GetWord("ChanJia")].ToString().Trim());
                            }
                            catch
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("DaiMa") + ": " + strGoodsCode + " " + LanguageHandle.GetWord("ZZJGSLHDJBSSZJC");
                                j = -1;
                            }

                            strIsTaxPrice = dr[i][LanguageHandle.GetWord("HanShui")].ToString().Trim();
                            if (strIsTaxPrice != "YES" & strIsTaxPrice != "NO")
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("DaiMa") + ": " + strGoodsCode + " " + LanguageHandle.GetWord("ZZDJHSXBXSYESHNO") + " ')";
                                j = -1;
                            }

                            if (CheckUnit(dr[i][LanguageHandle.GetWord("ChanWei")].ToString().Trim()) > 0)
                            {
                                goods.UnitName = dr[i][LanguageHandle.GetWord("ChanWei")].ToString().Trim();
                            }
                            else
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("DaiMa") + ": " + strGoodsCode + " " + LanguageHandle.GetWord("ZZJGJCSJLBCZCDWDRIDWTOSTRINGTRIMJC") + " " + dr[i][LanguageHandle.GetWord("ChanWei")].ToString().Trim();
                                j = -1;
                            }

                            if (CheckCurrency(dr[i][LanguageHandle.GetWord("BiBie")].ToString().Trim()) > 0)
                            {
                                goods.CurrencyType = dr[i][LanguageHandle.GetWord("BiBie")].ToString().Trim();
                            }
                            else
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("DaiMa") + ": " + strGoodsCode + " " + LanguageHandle.GetWord("ZZJGJCSJLBCZCBBDRIBBTOSTRINGTRIMJC");
                                j = -1;
                            }

                            if (CheckWareHouse(dr[i][LanguageHandle.GetWord("CunFangCangKu")].ToString().Trim()) > 0)
                            {
                                goods.Position = dr[i][LanguageHandle.GetWord("CunFangCangKu")].ToString().Trim();
                            }
                            else
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("DaiMa") + ": " + strGoodsCode + " " + LanguageHandle.GetWord("ZZJGJCSJLBCZCCKDRICFCKTOSTRINGTRIMJC") + " " + dr[i][LanguageHandle.GetWord("CunFangCangKu")].ToString().Trim();
                                j = -1;
                            }

                            try
                            {
                                DateTime.Parse(dr[i][LanguageHandle.GetWord("ShengChanRiJi")].ToString().Trim());
                            }
                            catch
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("DaiMa") + ": " + strGoodsCode + LanguageHandle.GetWord("ShengChanRiJiRiJiGeShiBuDuiHuo");
                                j = -1;
                            }

                            try
                            {
                                DateTime.Parse(dr[i][LanguageHandle.GetWord("ShiXiaoRiJi")].ToString().Trim());
                            }
                            catch
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("DaiMa") + ": " + strGoodsCode + LanguageHandle.GetWord("ShiXiaoRiJiRiJiGeShiBuDuiHuoWe");
                                j = -1;
                            }

                            goods.Manufacturer = dr[i]["Supplier"].ToString().Trim();

                            goods.OwnerCode = strUserCode;
                            goods.OwnerName = ShareClass.GetUserName(strUserCode);

                            goods.BuyTime = DateTime.Now;

                            goods.Memo = "";
                            goods.Status = "InUse";

                            continue;
                        }
                    }
                }
            }

            LogClass.WriteLogFile(LB_ErrorText.Text);
        }
        catch (Exception err)
        {
            LB_ErrorText.Text += err.Message.ToString() + "<br/>"; ;

            LogClass.WriteLogFile(LB_ErrorText.Text);

            j = -1;
        }

        return j;
    }

    protected int CheckGoodsType(string strGoodsType)
    {
        string strHQL;
        IList lst;

        strHQL = "From GoodsType as goodsType Where goodsType.Type = " + "'" + strGoodsType + "'";
        GoodsTypeBLL goodsTypeBLL = new GoodsTypeBLL();

        lst = goodsTypeBLL.GetAllGoodsTypes(strHQL);

        return lst.Count;
    }

    protected int CheckWareHouse(string strWareHouseName)
    {
        string strHQL;


        strHQL = "Select * From T_WareHouse Where WHName = " + "'" + strWareHouseName + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WareHouse");

        return ds.Tables[0].Rows.Count;
    }

    protected int CheckCurrency(string strCurrency)
    {
        string strHQL;
        IList lst;

        strHQL = "From CurrencyType as currencyType Where currencyType = " + "'" + strCurrency + "'";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);

        return lst.Count;
    }

    protected int CheckUnit(string strUnit)
    {
        string strHQL;

        strHQL = "Select * From T_JNUnit Where UnitName = " + "'" + strUnit + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_JNUnit");

        return ds.Tables[0].Rows.Count;
    }

    protected void BindGoodsData(string goodsType, string goodsWareHouse)
    {
        string strHQL;
        IList lst;

        string strGoodsType, strGoodsCode, strGoodsName, strModelNumber, strSpec;
        string strWareHouse;
        string strUserCode, strDepartString;

        DataGrid4.CurrentPageIndex = 0;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strGoodsType = DL_Type.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        strWareHouse = TB_WHName.Text.Trim();

        strHQL = "From Goods as goods Where goods.Type = '" + strGoodsType + "' ";//and goods.Position='" + strWareHouse + "' ";
        strHQL += " and goods.OwnerCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " and goods.Number > 0 and goods.Status = 'InUse' ";

        if (!string.IsNullOrEmpty(strGoodsCode))
        {
            strHQL += " and goods.GoodsCode Like '%" + strGoodsCode + "%' ";
        }
        if (!string.IsNullOrEmpty(strGoodsName))
        {
            strHQL += " and goods.GoodsName like '%" + strGoodsName + "%' ";
        }
        if (!string.IsNullOrEmpty(strModelNumber))
        {
            strHQL += " and goods.ModelNumber Like '%" + strModelNumber + "%' ";
        }
        if (!string.IsNullOrEmpty(strSpec))
        {
            strHQL += " and goods.Spec Like '%" + strSpec + "%' ";
        }
        strHQL += " Order by goods.BuyTime DESC ";
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        TB_GoodsCode.Text = "";
        TB_GoodsName.Text = "";
        TB_ModelNumber.Text = "";
        TB_Spec.Text = "";
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strGoodsCode;
            string strUserCode = LB_UserCode.Text;

            strID = e.Item.Cells[0].Text;
            strGoodsCode = ((Button)e.Item.FindControl("BT_GoodsCode")).Text.Trim();

            strHQL = "from Goods as goods where goods.ID = " + "'" + strID + "'";
            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            GoodsBLL goodsBLL = new GoodsBLL();
            lst = goodsBLL.GetAllGoodss(strHQL);
            Goods goods = (Goods)lst[0];

            LB_ID.Text = goods.ID.ToString();
            TB_GoodsCode.Text = goods.GoodsCode.Trim();
            LB_OwnerCode.Text = goods.OwnerCode;
            LB_OwnerName.Visible = true;
            LB_OwnerName.Text = ShareClass.GetUserName(goods.OwnerCode);

            TB_GoodsCode.Text = goods.GoodsCode;
            TB_GoodsName.Text = goods.GoodsName;
            TB_ModelNumber.Text = goods.ModelNumber;
            NB_Number.Amount = goods.Number;
            DL_Unit.SelectedValue = goods.UnitName;
            DL_Type.SelectedValue = goods.Type;
            TB_ModelNumber.Text = goods.ModelNumber;
            TB_Spec.Text = goods.Spec;
            TB_Brand.Text = goods.Manufacturer;

            TB_WHName.Text = goods.Position.Trim();

            NB_Price.Amount = goods.Price;
            DLC_BuyTime.Text = goods.BuyTime.ToString("yyyy-MM-dd");

            TB_Manufacturer.Text = goods.Manufacturer;
            TB_Memo.Text = goods.Memo.Trim();
        }
    }

    protected void DL_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGoodsData(DL_Type.SelectedValue.Trim(), TB_WHName.Text.Trim());
    }

}







