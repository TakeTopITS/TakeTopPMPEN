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

public partial class TTWZStockList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();

            
        }

    }

    private void DataBinder()
    {
        DG_List.CurrentPageIndex = 0;

        string strWZStockHQL = @"select s.*,p.UserName as SafekeepName,m.UserName as CheckerName from T_WZStock s
                                        left join T_ProjectMember p on s.Safekeep = p.UserCode
                                        left join T_ProjectMember m on s.Checker = m.UserCode
                                        order by s.ID desc";
        DataTable dtStock = ShareClass.GetDataSetFromSql(strWZStockHQL, "Stock").Tables[0];

        DG_List.DataSource = dtStock;
        DG_List.DataBind();

        LB_Sql.Text = strWZStockHQL;

        LB_RecordCount.Text = dtStock.Rows.Count.ToString();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZStockBLL wZStockBLL = new WZStockBLL();
                string strWZStockSql = "from WZStock as wZStock where id = " + cmdArges;
                IList listStock = wZStockBLL.GetAllWZStocks(strWZStockSql);
                if (listStock != null && listStock.Count == 1)
                {
                    WZStock wZStock = (WZStock)listStock[0];
                    if (wZStock.StockCode == LanguageHandle.GetWord("ZhiShuZongKu").ToString().Trim() || wZStock.StockCode == LanguageHandle.GetWord("RongQiYiKu").ToString().Trim() || wZStock.StockCode == LanguageHandle.GetWord("RongQiErKu").ToString().Trim() || wZStock.StockCode == LanguageHandle.GetWord("JiLiangKu").ToString().Trim() || wZStock.StockCode == LanguageHandle.GetWord("ErJiKu").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZSZKRYKREKJLKEJKBYXSC").ToString().Trim()+"')", true);
                        return;
                    }
                    if (wZStock.IsMark == -1)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBJW1BYXSC").ToString().Trim()+"')", true);
                        return;
                    }

                    wZStockBLL.DeleteWZStock(wZStock);

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
                string strStockHQL = string.Format(@"select s.*,p.UserName as SafekeepName,m.UserName as CheckerName from T_WZStock s
                                        left join T_ProjectMember p on s.Safekeep = p.UserCode
                                        left join T_ProjectMember m on s.Checker = m.UserCode 
                                        where s.id = {0}" ,cmdArges);
                DataTable dtStock = ShareClass.GetDataSetFromSql(strStockHQL, "Stock").Tables[0];
                if (dtStock != null && dtStock.Rows.Count > 0)
                {
                    DataRow drStock = dtStock.Rows[0];

                    TXT_ID.Text = ShareClass.ObjectToString(drStock["ID"]);
                    LB_StockCode.Text = ShareClass.ObjectToString(drStock["StockCode"]);
                    TXT_StockDesc.Text = ShareClass.ObjectToString(drStock["StockDesc"]);
                    TXT_Safekeep.Text = ShareClass.ObjectToString(drStock["SafekeepName"]);
                    HF_Safekeep.Value = ShareClass.ObjectToString(drStock["Safekeep"]);
                    TXT_Checker.Text = ShareClass.ObjectToString(drStock["CheckerName"]);
                    HF_Checker.Value = ShareClass.ObjectToString(drStock["Checker"]);

                    TXT_Safekeep.BackColor = Color.CornflowerBlue;
                    TXT_Checker.BackColor = Color.CornflowerBlue;
                    TXT_StockDesc.BackColor = Color.CornflowerBlue;
                }
            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim();
        DataTable dtStock = ShareClass.GetDataSetFromSql(strHQL, "Stock").Tables[0];

        DG_List.DataSource = dtStock;
        DG_List.DataBind();

        LB_RecordCount.Text = dtStock.Rows.Count.ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            WZStockBLL wZStockBLL = new WZStockBLL();
            string strStockDesc = TXT_StockDesc.Text.Trim();
            string strSafekeep = HF_Safekeep.Value;
            string strChecker = HF_Checker.Value;

            if (string.IsNullOrEmpty(strSafekeep))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGJSJK").ToString().Trim()+"')", true);
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBGYBNWKZ").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strChecker))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGJSJK").ToString().Trim()+"')", true);
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCLYBNWKZ").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strStockDesc))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGJSJK").ToString().Trim()+"')", true);
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKBSMBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            else
            {
                if (!ShareClass.CheckStringRight(strStockDesc))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKBSMBNWFFZFCXG").ToString().Trim()+"')", true);
                    return;
                }
                if (strStockDesc.Length > 30)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKBSMBNCG30GZFXG").ToString().Trim()+"')", true);
                    return;
                }
            }


            if (!string.IsNullOrEmpty(TXT_ID.Text) && TXT_ID.Text != "0")
            {
                int intID = 0;
                int.TryParse(TXT_ID.Text, out intID);
                //修改
                string strStockHQL = "from WZStock as wZStock where id = " + intID;
                IList lstStock = wZStockBLL.GetAllWZStocks(strStockHQL);
                if (lstStock != null && lstStock.Count > 0)
                {
                    WZStock wZStock = (WZStock)lstStock[0];
                    wZStock.StockDesc = strStockDesc;
                    wZStock.Safekeep = strSafekeep;
                    wZStock.Checker = strChecker;

                    wZStockBLL.UpdateWZStock(wZStock, intID);
                }
            }
            else
            {
                //新增
                WZStock wZStock = new WZStock();
                string strNewStockCode = CreateNewStockCode();
                wZStock.StockCode = strNewStockCode;
                wZStock.StockDesc = strStockDesc;
                wZStock.Safekeep = strSafekeep;
                wZStock.Checker = strChecker;

                wZStockBLL.AddWZStock(wZStock);

                LB_StockCode.Text = strNewStockCode;

                //将材检员，保管员同步到项目工程中
                //string strUpdateProjectSQL = string.Format("update ";

                ////查询出最大的ID
                //string strSelectStockHQL = string.Format("select top 1 * from T_WZStock order by ID desc");
                //DataTable dtSelectStock = ShareClass.GetDataSetFromSql(strSelectStockHQL, "strSelectStockHQL").Tables[0];
                //if (dtSelectStock != null && dtSelectStock.Rows.Count > 0)
                //{
                //    //TXT_ID.Text = dtSelectStock.Rows[0]["ID"].ToString();
                //    LB_StockCode.Text = dtSelectStock.Rows[0]["StockCode"].ToString();
                //    TXT_StockDesc.Text = dtSelectStock.Rows[0]["StockDesc"] == DBNull.Value ? "" : dtSelectStock.Rows[0]["StockDesc"].ToString();
                //}
            }

            DataBinder();
        }
        catch (Exception ex) { }

        TXT_ID.Text = "";
        LB_StockCode.Text = "";
        TXT_StockDesc.Text = "";
        TXT_Safekeep.Text = "";
        HF_Safekeep.Value = "";
        TXT_Checker.Text = "";
        HF_Checker.Value = "";

        TXT_Safekeep.BackColor = Color.White;
        TXT_Checker.BackColor = Color.White;
        TXT_StockDesc.BackColor = Color.White;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatus()", true);   //ChineseWord
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        TXT_ID.Text = "";
        LB_StockCode.Text = "";
        TXT_StockDesc.Text = "";
        TXT_Safekeep.Text = "";
        HF_Safekeep.Value = "";
        TXT_Checker.Text = "";
        HF_Checker.Value = "";

        TXT_Safekeep.BackColor = Color.CornflowerBlue;
        TXT_Checker.BackColor = Color.CornflowerBlue;
        TXT_StockDesc.BackColor = Color.CornflowerBlue;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatus()", true);

        //try
        //{
        //    WZStockBLL wZStockBLL = new WZStockBLL();
        //    string strStockDesc = TXT_StockDesc.Text.Trim();
        //    string strSafekeep = HF_Safekeep.Value;
        //    string strSafekeepName = TXT_Safekeep.Text;
        //    string strChecker = HF_Checker.Value;
        //    string strCheckerName = TXT_Checker.Text;

        //    if (!ShareClass.CheckStringRight(strStockDesc))
        //    {
        //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKBSMBNWFFZFCXG").ToString().Trim()+"')", true);
        //        return;
        //    }
        //    if (strStockDesc.Length > 30)
        //    {
        //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKBSMBNCG30GZFXG").ToString().Trim()+"')", true);
        //        return;
        //    }


        //    //新增
        //    WZStock wZStock = new WZStock();
        //    string strNewStockCode = CreateNewStockCode();
        //    wZStock.StockCode = strNewStockCode;
        //    wZStock.StockDesc = strStockDesc;
        //    wZStock.Safekeep = strSafekeep;
        //    wZStock.Checker = strChecker;

        //    wZStockBLL.AddWZStock(wZStock);

        //    LB_StockCode.Text = strNewStockCode;


        //    //查询出最大的ID
        //    string strSelectStockHQL = string.Format("select top 1 * from T_WZStock order by ID desc");
        //    DataTable dtSelectStock = ShareClass.GetDataSetFromSql(strSelectStockHQL, "strSelectStockHQL").Tables[0];
        //    if (dtSelectStock != null && dtSelectStock.Rows.Count > 0)
        //    {
        //        TXT_ID.Text = ShareClass.ObjectToString(dtSelectStock.Rows[0]["ID"]);
        //    }


        //    //HF_Safekeep.Value= strSafekeep;
        //    //TXT_Safekeep.Text= strSafekeepName;
        //    //HF_Checker.Value = strChecker;
        //    //TXT_Checker.Text = strCheckerName;


        //    DataBinder();
        //}
        //catch (Exception ex) { }
        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatus()", true);
    }
    

    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        try
        {
            WZStockBLL wZStockBLL = new WZStockBLL();
            string strStockDesc = TXT_StockDesc.Text.Trim();
            string strSafekeep = HF_Safekeep.Value;
            string strSafekeepName = TXT_Safekeep.Text;
            string strChecker = HF_Checker.Value;
            string strCheckerName = TXT_Checker.Text;

            if (!ShareClass.CheckStringRight(strStockDesc))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKBSMBNWFFZFCXG").ToString().Trim()+"')", true);
                return;
            }
            if (strStockDesc.Length > 30)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKBSMBNCG30GZFXG").ToString().Trim()+"')", true);
                return;
            }

            if (!string.IsNullOrEmpty(TXT_ID.Text) && TXT_ID.Text != "0")
            {
                int intID = 0;
                int.TryParse(TXT_ID.Text, out intID);
                //修改
                string strStockHQL = "from WZStock as wZStock where id = " + intID;
                IList lstStock = wZStockBLL.GetAllWZStocks(strStockHQL);
                if (lstStock != null && lstStock.Count > 0)
                {
                    WZStock wZStock = (WZStock)lstStock[0];
                    wZStock.StockDesc = strStockDesc;
                    wZStock.Safekeep = strSafekeep;
                    wZStock.Checker = strChecker;

                    wZStockBLL.UpdateWZStock(wZStock, intID);

                    //HF_Safekeep.Value = strSafekeep;
                    //TXT_Safekeep.Text = strSafekeepName;
                    //HF_Checker.Value = strChecker;
                    //TXT_Checker.Text = strCheckerName;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZYXGDKB").ToString().Trim()+"')", true);
                return;
            }

            DataBinder();
        }
        catch (Exception ex) { }
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatus()", true);   //ChineseWord
    }


    protected void BT_RepeatMark_Click(object sender, EventArgs e)
    {
      //检查是否有 库别〈库别〉＝工程项目〈库别〉 的记录												
      //有，则写记录：库别〈使用标记〉＝“-1”，然后继续做下一条												
      //无，则写记录：库别〈使用标记〉＝“0”，然后继续做下一条												
      //循环检查，直到库别表单最后一条记录后结束												

        try
        {

            string strStockHQL = @"select s.*,COALESCE(p.ProjectCode,'no') as IsExist from T_WZStock s
                        left join T_WZProject p on s.StockCode = p.StoreRoom";
            DataTable dtStock = ShareClass.GetDataSetFromSql(strStockHQL, "Stock").Tables[0];
            if (dtStock != null && dtStock.Rows.Count > 0)
            {
                foreach (DataRow drStock in dtStock.Rows)
                {
                    string strID = ShareClass.ObjectToString(drStock["ID"]);
                    string strIsExist = ShareClass.ObjectToString(drStock["IsExist"]);
                    int intIsMark = 0;
                    if (strIsExist.Trim() == "no")
                    {
                        intIsMark = 0;
                    }
                    else {
                        intIsMark = -1;
                    }

                    string strUpdateStockSQL = string.Format(@"update T_WZStock set IsMark = {0} where ID  = {1}", intIsMark, strID);

                    ShareClass.RunSqlCommand(strUpdateStockSQL);
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZZSYBJWC").ToString().Trim()+"')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZSMYKCLB").ToString().Trim()+"')", true);
                return;
            }
        }
        catch (Exception ex) { }
    }
    

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        TXT_ID.Text = "";
        LB_StockCode.Text = "";
        TXT_StockDesc.Text = "";
        TXT_Safekeep.Text = "";
        HF_Safekeep.Value = "";
        TXT_Checker.Text = "";
        HF_Checker.Value = "";

        LB_StockCode.BackColor = Color.White;
        TXT_StockDesc.BackColor = Color.White;
        TXT_Safekeep.BackColor = Color.White;
        TXT_Checker.BackColor = Color.White;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatus()", true);
    }


    protected void BT_Start_Click(object sender, EventArgs e)
    {
        string strResult = string.Empty;
        string strTotalHQL = "select * from T_WZStock where StockCode = 'Direct General Warehouse'";   //ChineseWord
        DataTable dtTotal = ShareClass.GetDataSetFromSql(strTotalHQL, "strTotalHQL").Tables[0];
        if (dtTotal == null || dtTotal.Rows.Count == 0)
        {
            //创建 直属总库
            string strInsertTotalHQL = string.Format(@"insert into T_WZStock(StockCode,StockDesc,IsMark,IsCancel)
                            values('{0}','{1}',-1,0)", LanguageHandle.GetWord("ZhiShuZongKu").ToString().Trim(), "用于各行政单位自营项目（包括消耗、劳保、维修及自用料等）");   //ChineseWord
            ShareClass.RunSqlCommand(strInsertTotalHQL);
            strResult += LanguageHandle.GetWord("ZhiShuZongKuChuangJianChengGon").ToString().Trim();
        }
        else {
            strResult += LanguageHandle.GetWord("ZhiShuZongKuYiJingCunZai").ToString().Trim();
        }


        string strContainerOneHQL = "select * from T_WZStock where StockCode = 'Container Warehouse One'";   //ChineseWord
        DataTable dtContainerOne = ShareClass.GetDataSetFromSql(strContainerOneHQL, "ContainerOne").Tables[0];
        if (dtContainerOne == null || dtContainerOne.Rows.Count == 0)
        {
            //创建 容器一库
            string strInsertContainerOneHQL = string.Format(@"insert into T_WZStock(StockCode,StockDesc,IsMark,IsCancel)
                            values('{0}','{1}',-1,0)", LanguageHandle.GetWord("RongQiYiKu").ToString().Trim(), "Used for Pressure Vessel Manufacturing Projects of Container Manufacturing Plant");   //ChineseWord
            ShareClass.RunSqlCommand(strInsertContainerOneHQL);
            strResult += LanguageHandle.GetWord("RongQiYiKuChuangJianChengGong").ToString().Trim();
        }
        else
        {
            strResult += LanguageHandle.GetWord("RongQiYiKuYiJingCunZai").ToString().Trim();
        }

        string strContainerTwoHQL = "select * from T_WZStock where StockCode = 'Container Warehouse Two'";   //ChineseWord
        DataTable dtContainerTwo = ShareClass.GetDataSetFromSql(strContainerTwoHQL, "ContainerTwo").Tables[0];
        if (dtContainerTwo == null || dtContainerTwo.Rows.Count == 0)
        {
            //创建 容器二库
            string strInsertContainerTwoHQL = string.Format(@"insert into T_WZStock(StockCode,StockDesc,IsMark,IsCancel)
                            values('{0}','{1}',-1,0)", LanguageHandle.GetWord("RongQiErKu").ToString().Trim(), "Used for Pressure Vessel Manufacturing Projects of Lanjiang Company");   //ChineseWord
            ShareClass.RunSqlCommand(strInsertContainerTwoHQL);
            strResult += LanguageHandle.GetWord("RongQiErKuChuangJianChengGong").ToString().Trim();
        }
        else
        {
            strResult += LanguageHandle.GetWord("RongQiErKuYiJingCunZai").ToString().Trim();
        }

        string strAmountHQL = "select * from T_WZStock where StockCode = 'Measurement Warehouse'";   //ChineseWord
        DataTable dtAmount = ShareClass.GetDataSetFromSql(strAmountHQL, "Amount").Tables[0];
        if (dtAmount == null || dtAmount.Rows.Count == 0)
        {
            //创建 计量库
            string strInsertContainerTwoHQL = string.Format(@"insert into T_WZStock(StockCode,StockDesc,IsMark,IsCancel)
                            values('{0}','{1}',-1,0)", LanguageHandle.GetWord("JiLiangKu").ToString().Trim(), "Used for Pressure Vessel Manufacturing Projects of Lanjiang Company");   //ChineseWord
            ShareClass.RunSqlCommand(strInsertContainerTwoHQL);
            strResult += LanguageHandle.GetWord("JiLiangKuChuangJianChengGong").ToString().Trim();
        }
        else
        {
            strResult += LanguageHandle.GetWord("JiLiangKuYiJingCunZai").ToString().Trim();
        }

        string strSecondLevelHQL = "select * from T_WZStock where StockCode = 'Secondary Warehouse'";   //ChineseWord
        DataTable dtSecondLevel = ShareClass.GetDataSetFromSql(strSecondLevelHQL, "SecondLevel").Tables[0];
        if (dtSecondLevel == null || dtSecondLevel.Rows.Count == 0)
        {
            //创建 二级库
            string strInsertContainerTwoHQL = string.Format(@"insert into T_WZStock(StockCode,StockDesc,IsMark,IsCancel)
                            values('{0}','{1}',-1,0)", LanguageHandle.GetWord("ErJiKu").ToString().Trim(), "Specifically for the Calculation of Labor Protection Supplies Purchased by Lanzhou Petrochemical Company");   //ChineseWord
            ShareClass.RunSqlCommand(strInsertContainerTwoHQL);
            strResult += LanguageHandle.GetWord("ErJiKuChuangJianChengGong").ToString().Trim();
        }
        else
        {
            strResult += LanguageHandle.GetWord("ErJiKuYiJingCunZai").ToString().Trim();
        }

        string strUpdateSQL = "update T_WZStock set IsMark = -1 where StockCode in ('Direct General Warehouse','Container Warehouse One','Container Warehouse Two','Measurement Warehouse','Secondary Warehouse')";   //ChineseWord
        ShareClass.RunSqlCommand(strUpdateSQL);

        DataBinder();
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + strResult + "！');ControlStatus()", true);
    }



    /// <summary>
    ///  写记录
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BT_WriteRecord_Click(object sender, EventArgs e)
    {
        //写记录条件：库别〈使用标记〉＝“-1”，库别〈库别〉＝工程项目〈库别〉；写记录：库别〈材检员〉〈保管员〉＝工程项目〈材检员〉〈保管员〉
        try
        {
            string strStockHQL = @"select p.*,s.ID as StockID,
                        s.Checker as StockChecker,
                        s.Safekeep as StockSafekeep
                        from T_WZProject p
                        inner join T_WZStock s on p.StoreRoom = s.StockCode
                        where s.IsMark = -1";
            DataTable dtStock = ShareClass.GetDataSetFromSql(strStockHQL, "Stock").Tables[0];
            if (dtStock != null && dtStock.Rows.Count > 0)
            {
                StringBuilder sbSql = new StringBuilder();

                foreach (DataRow drStock in dtStock.Rows)
                {
                    //string strStockID = ShareClass.ObjectToString(drStock["StockID"]);
                    //string strChecker = ShareClass.ObjectToString(drStock["Checker"]);
                    //string strSafekeep = ShareClass.ObjectToString(drStock["Safekeep"]);

                    //sbSql.AppendFormat(@"update T_WZStock set Checker = '{0}',Safekeep='{1}' where ID  = {2}; ", strChecker, strSafekeep, strStockID);

                    string strProjectCode = ShareClass.ObjectToString(drStock["ProjectCode"]);
                    string strStockChecker = ShareClass.ObjectToString(drStock["StockChecker"]);
                    string strStockSafekeep = ShareClass.ObjectToString(drStock["StockSafekeep"]);

                    sbSql.AppendFormat(@" update T_WZProject set Checker = '{0}',Safekeep = '{1}' where ProjectCode = '{2}'; ", strStockChecker, strStockSafekeep, strProjectCode);
                }

                if (!string.IsNullOrEmpty(sbSql.ToString()))
                {
                    ShareClass.RunSqlCommand(sbSql.ToString());

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZZSYBJWC").ToString().Trim()+"')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZSMYKCLB").ToString().Trim()+"')", true);
                return;
            }
        }
        catch (Exception ex) { 
        
        }
    }





    /// <summary>
    ///  生成库别Code
    /// </summary>
    private string CreateNewStockCode()
    {
        string strNewStockCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                string strStockCodeHQL = "select COUNT(1) as RowNumber from T_WZStock where StockCode like '自营%'";   //ChineseWord
                DataTable dtStock = ShareClass.GetDataSetFromSql(strStockCodeHQL, "Stock").Tables[0];
                int intStockCodeNumber = 0;
                int.TryParse(dtStock.Rows[0]["RowNumber"].ToString(), out intStockCodeNumber);
                intStockCodeNumber = intStockCodeNumber + 1;
                do
                {
                    strNewStockCode = LanguageHandle.GetWord("ZiYing").ToString().Trim() + intStockCodeNumber.ToString() + LanguageHandle.GetWord("Ku").ToString().Trim();

                    //验证新的库别Code是否存在
                    string strCheckNewStockCodeHQL = "select count(1) as RowNumber from T_WZStock where StockCode = '" + strNewStockCode + "'";
                    DataTable dtCheckNewStockCode = ShareClass.GetDataSetFromSql(strCheckNewStockCodeHQL, "NewStockCode").Tables[0];
                    int intCheckNewStockCode = int.Parse(dtCheckNewStockCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewStockCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intStockCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewStockCode;
    }
}
