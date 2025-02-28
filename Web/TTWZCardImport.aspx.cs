using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;

public partial class TTWZCardImport : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataTurnDetailBinder();
            DataCardImportBinder();
            DataCardBinder();
        }
    }


    private void DataTurnDetailBinder()
    {
        DG_TurnDetail.CurrentPageIndex = 0;

        string strWZTurnDetailHQL = @"select d.*,o.ObjectName,pd.PlanCode as PickingPlanCode ,
                                m.UserName as MaterialPersonName
                                from T_WZTurnDetail d
                                left join T_WZObject o on d.ObjectCode = o.ObjectCode
                                left join T_WZPickingPlanDetail pd on d.PlanCode = pd.ID 
                                left join T_ProjectMember m on d.MaterialPerson = m.UserCode
                                where d.IsMark= -1 
                                and d.CardIsMark =0 
                                order by d.NoCode";
        DataTable dtWZTurnDetail = ShareClass.GetDataSetFromSql(strWZTurnDetailHQL, "TurnDetail").Tables[0];

        DG_TurnDetail.DataSource = dtWZTurnDetail;
        DG_TurnDetail.DataBind();

        LB_TurnDetailSql.Text = strWZTurnDetailHQL;


        //DG_TurnDetail.CurrentPageIndex = 0;

        //WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
        //string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where IsMark= -1 and CardIsMark =0 order by NoCode";
        //IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);

        //DG_TurnDetail.DataSource = listWZTurnDetail;
        //DG_TurnDetail.DataBind();

        //LB_TurnDetailSql.Text = strWZTurnDetailHQL;
    }


    protected void DG_TurnDetail_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_TurnDetail.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_TurnDetailSql.Text.Trim();
        DataTable dtWZTurnDetail = ShareClass.GetDataSetFromSql(strHQL, "TurnDetail").Tables[0];

        DG_TurnDetail.DataSource = dtWZTurnDetail;
        DG_TurnDetail.DataBind();

        //DG_TurnDetail.CurrentPageIndex = e.NewPageIndex;

        //string strHQL = LB_TurnDetailSql.Text.Trim();
        //WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
        //IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strHQL);

        //DG_TurnDetail.DataSource = listWZTurnDetail;
        //DG_TurnDetail.DataBind();
    }


    private void DataCardImportBinder()
    {
        DG_CardImportList.CurrentPageIndex = 0;

        WZCardImportBLL wZCardImportBLL = new WZCardImportBLL();
        string strWZCardImportHQL = "from WZCardImport as wZCardImport  order by NoCode";
        IList listWZCardImport = wZCardImportBLL.GetAllWZCardImports(strWZCardImportHQL);

        DG_CardImportList.DataSource = listWZCardImport;
        DG_CardImportList.DataBind();

        LB_CardImportListSql.Text = strWZCardImportHQL;
    }


    protected void DG_CardImportList_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_CardImportList.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_CardImportListSql.Text.Trim();
        WZCardImportBLL wZCardImportBLL = new WZCardImportBLL();
        IList listWZCardImport = wZCardImportBLL.GetAllWZCardImports(strHQL);

        DG_CardImportList.DataSource = listWZCardImport;
        DG_CardImportList.DataBind();
    }

    private void DataCardBinder()
    {
        WZCardBLL wZCardBLL = new WZCardBLL();
        string strWZCardHQL = "from WZCard as wZCard order by CardCode desc";
        IList listWZCard = wZCardBLL.GetAllWZCards(strWZCardHQL);

        DDL_Card.DataSource = listWZCard;
        DDL_Card.DataBind();

        DDL_Card.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void BT_Compare_Click(object sender, EventArgs e)
    {
        string strCardCode = DDL_Card.SelectedValue;
        if (string.IsNullOrEmpty(strCardCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCWPZ").ToString().Trim() + "')", true);
            return;
        }
        //比对
        //凭证导入明细
        WZCardImportBLL wZCardImportBLL = new WZCardImportBLL();
        string strWZCardImportHQL = "from WZCardImport as wZCardImport order by ID asc";
        IList listWZCardImport = wZCardImportBLL.GetAllWZCardImports(strWZCardImportHQL);
        if (listWZCardImport != null && listWZCardImport.Count > 0)
        {
            for (int i = 0; i < listWZCardImport.Count; i++)
            {
                WZCardImport wZCardImport = (WZCardImport)listWZCardImport[i];
                //凭证导入〈No编号〉＝移交明细〈No编号〉,凭证导入〈出库数量〉＝移交明细〈实领数量〉, 凭证导入〈出库金额〉＝移交明细〈开票金额〉
                //移交单明细
                WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
                string strWZTurnDetailHQL = string.Format(@"from WZTurnDetail as wZTurnDetail 
                                where IsMark= -1 and CardIsMark =0
                                and NoCode = '{0}'
                                and ActualNumber = {1}
                                and TicketMoney = {2}", wZCardImport.NoCode, wZCardImport.OutNumber, wZCardImport.OutMoney);
                IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);
                if (listWZTurnDetail != null && listWZTurnDetail.Count == 1)
                {
                    WZCardBLL wZCardBLL = new WZCardBLL();
                    string strWZCardHQL = "from WZCard as wZCard where CardCode = '" + strCardCode + "'";
                    IList listCard = wZCardBLL.GetAllWZCards(strWZCardHQL);
                    string strCardMarker = string.Empty;
                    WZCard wZCard = (WZCard)listCard[0];
                    strCardMarker = wZCard.CardMarker;

                    WZTurnDetail wZTurnDetail = (WZTurnDetail)listWZTurnDetail[0];
                    //当 3 项条件相等且唯一时
                    //写凭证导入表单
                    wZCardImport.ImportStatus = "ok";
                    wZCardImport.CardCode = strCardCode;
                    wZCardImport.PickingCode = wZTurnDetail.PickingCode;
                    wZCardImport.TurnCode = wZTurnDetail.TurnCode;
                    wZCardImport.MaterialPerson = wZTurnDetail.MaterialPerson;

                    wZCardImportBLL.UpdateWZCardImport(wZCardImport, wZCardImport.ID);
                    //写移交明细表单
                    wZTurnDetail.PlanMoney = wZCardImport.PlanMoney;
                    if (wZTurnDetail.ActualNumber != 0)
                    {
                        wZTurnDetail.PlanPrice = wZTurnDetail.PlanMoney / wZTurnDetail.ActualNumber;
                    }
                    wZTurnDetail.CardCode = strCardCode;
                    wZTurnDetail.CardPerson = strCardMarker;
                    wZTurnDetail.CardIsMark = -1;

                    wZTurnDetailBLL.UpdateWZTurnDetail(wZTurnDetail, wZTurnDetail.ID);
                    //写财务凭证表单
                    wZCard.CardMoney += wZTurnDetail.PlanMoney;
                    wZCard.DetailMoney += wZTurnDetail.TicketMoney;
                    wZCard.RowNumber += 1;
                    wZCard.IsMark = -1;

                    wZCardBLL.UpdateWZCard(wZCard, wZCard.CardCode);

                    continue;
                }
                else if (listWZTurnDetail != null && listWZTurnDetail.Count > 1)
                {
                    //当〈No编号〉相等，其余 2 项条件相等但不唯一时

                    wZCardImport.ImportStatus = "记录重复";   //ChineseWord

                    wZCardImportBLL.UpdateWZCardImport(wZCardImport, wZCardImport.ID);

                    continue;
                }

                //当〈No编号〉相等，其余 2 项条件不等时
                string strWZTurnDetailHQL2 = string.Format(@"from WZTurnDetail as wZTurnDetail 
                                where IsMark= -1 and CardIsMark =0
                                and NoCode = '{0}'
                                and ActualNumber != {1}
                                and TicketMoney != {2}", wZCardImport.NoCode, wZCardImport.OutNumber, wZCardImport.OutMoney);
                IList listWZTurnDetail2 = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL2);
                if (listWZTurnDetail2 != null && listWZTurnDetail2.Count > 0)
                {
                    wZCardImport.ImportStatus = "数据错误";   //ChineseWord

                    wZCardImportBLL.UpdateWZCardImport(wZCardImport, wZCardImport.ID);

                    continue;
                }

                //当找不到〈No编号〉时
                string strWZTurnDetailHQL3 = string.Format(@"from WZTurnDetail as wZTurnDetail 
                                where IsMark= -1 and CardIsMark =0
                                and NoCode = '{0}'", wZCardImport.NoCode);
                IList listWZTurnDetail3 = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL3);
                if (listWZTurnDetail3 == null || listWZTurnDetail3.Count == 0)
                {
                    wZCardImport.ImportStatus = "无记录";   //ChineseWord

                    wZCardImportBLL.UpdateWZCardImport(wZCardImport, wZCardImport.ID);

                    continue;
                }
            }
        }

        //把凭证导入<导入状态> = OK的记录删除
        string strDeleteCardImportHQL = "delete T_WZCardImport where ImportStatus = 'ok'";
        ShareClass.RunSqlCommand(strDeleteCardImportHQL);

        //重新加载列表
        DataTurnDetailBinder();
        DataCardImportBinder();

        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZBDWC").ToString().Trim() + "');</script>");
    }

    protected void BT_Export_Click(object sender, EventArgs e)
    {
        //导出
        string strCardCode = DDL_Card.SelectedValue;
        if (string.IsNullOrEmpty(strCardCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCWPZ").ToString().Trim() + "')", true);
            return;
        }
        string strFileName = strCardCode + LanguageHandle.GetWord("HaoCaiWuPingZhengWeiDuiLiaoCha").ToString().Trim();
        string strWZCardHQL = "select * from T_WZCardImport";
        DataTable dtWZCard = ShareClass.GetDataSetFromSql(strWZCardHQL, "WZCard").Tables[0];
        //ExcelUtils.Export2Excel(DG_CardImportList, strFileName);

        Export3Excel(dtWZCard, strFileName);

    }

    protected void BT_Import_Click(object sender, EventArgs e)
    {
        string strCardFile = fileExcel.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
        if (!string.IsNullOrEmpty(strCardFile))
        {
            string strExtendName = System.IO.Path.GetExtension(strCardFile);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间
            string strFileName2 = System.IO.Path.GetFileName(strCardFile);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "');</script>");
            }

            if (Directory.Exists(strDocSavePath) == false)
            {
                //如果不存在就创建file文件夹{
                Directory.CreateDirectory(strDocSavePath);
            }

            string strAllUrl = strDocSavePath + strFileName3;
            fileExcel.SaveAs(strAllUrl);

            DataTable dtExcel = null;
            string resultMsg = string.Empty;
            try
            {
                dtExcel = ExcelUtils.ReadExcel(strAllUrl, "Sheet1").Tables[0];
                bool isSuccess = ValidateData(dtExcel, ref resultMsg);
                if (isSuccess)
                {
                    Import(dtExcel, ref resultMsg);
                }

                lblMsg.Text = string.Format("<span style='color:red' >{0}</span>", resultMsg);


                //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZDRCG").ToString().Trim()+"');</script>");
            }
            catch (Exception ex)
            {
                lblMsg.Text = string.Format(LanguageHandle.GetWord("spanstylecolorredDaoRuShiChuXi").ToString().Trim(), ex.Message);
            }
        }
    }


    /// <summary>
    /// 验证数据合法性.
    /// </summary>
    /// <param name="dtExcel"></param>
    /// <param name="resultMsg"></param>
    /// <returns></returns>
    private bool ValidateData(DataTable dtExcel, ref string resultMsg)
    {
        int lineNumber = 1;
        foreach (DataRow row in dtExcel.Rows)
        {
            lineNumber++;
            try
            {
                string strID = ShareClass.ObjectToString(row[LanguageHandle.GetWord("XuHao").ToString().Trim()]);
                string strNoCode = ShareClass.ObjectToString(row[LanguageHandle.GetWord("NoBianHao").ToString().Trim()]);
                string strObjectName = ShareClass.ObjectToString(row[LanguageHandle.GetWord("WuZiMingChen").ToString().Trim()]);
                string strOutNumber = ShareClass.ObjectToString(row[LanguageHandle.GetWord("ChuKuShuLiang").ToString().Trim()]);
                string strOutPrice = ShareClass.ObjectToString(row[LanguageHandle.GetWord("ChuKuChanJia").ToString().Trim()]);
                string strOutMoney = ShareClass.ObjectToString(row[LanguageHandle.GetWord("ChuKuJinE").ToString().Trim()]);
                string strPlanMoney = ShareClass.ObjectToString(row[LanguageHandle.GetWord("YuSuanJinE").ToString().Trim()]);

                if (string.IsNullOrEmpty(strID) && string.IsNullOrEmpty(strNoCode) && string.IsNullOrEmpty(strObjectName)
                    && string.IsNullOrEmpty(strOutNumber) && string.IsNullOrEmpty(strOutPrice)
                    && string.IsNullOrEmpty(strOutMoney) && string.IsNullOrEmpty(strPlanMoney))
                {
                    break;
                }
                if (string.IsNullOrEmpty(strID))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangXuHaoBuNengWeiKongbr").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strNoCode))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangNoBianHaoBuNengWeiKongb").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strObjectName))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangWuZiMingChenBuNengWeiKo").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strOutNumber))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangChuKuShuLiangBuNengWeiK").ToString().Trim(), lineNumber);
                    continue;
                }
                else
                {
                    if (!ShareClass.CheckIsNumber(strOutNumber))
                    {
                        resultMsg += string.Format(LanguageHandle.GetWord("Di0HangChuKuShuLiangBiXuWeiShu").ToString().Trim(), lineNumber);
                        continue;
                    }
                }
                if (string.IsNullOrEmpty(strOutPrice))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangChuKuChanJiaBuNengWeiKo").ToString().Trim(), lineNumber);
                    continue;
                }
                else
                {
                    if (!ShareClass.CheckIsNumber(strOutPrice))
                    {
                        resultMsg += string.Format(LanguageHandle.GetWord("Di0HangChuKuChanJiaBiXuWeiXiao").ToString().Trim(), lineNumber);
                        continue;
                    }
                }
                if (string.IsNullOrEmpty(strOutMoney))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangChuKuJinEBuNengWeiKongb").ToString().Trim(), lineNumber);
                    continue;
                }
                else
                {
                    if (!ShareClass.CheckIsNumber(strOutMoney))
                    {
                        resultMsg += string.Format(LanguageHandle.GetWord("Di0HangChuKuJinEBiXuWeiXiaoShu").ToString().Trim(), lineNumber);
                        continue;
                    }
                }
                if (string.IsNullOrEmpty(strPlanMoney))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangYuSuanJinEBuNengWeiKong").ToString().Trim(), lineNumber);
                    continue;
                }
                else
                {
                    if (!ShareClass.CheckIsNumber(strPlanMoney))
                    {
                        resultMsg += string.Format(LanguageHandle.GetWord("Di0HangYuSuanJinEBiXuWeiXiaoSh").ToString().Trim(), lineNumber);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = string.Format(LanguageHandle.GetWord("spanstylecolorredDaoRuShiChuXi").ToString().Trim(), ex.Message);
            }

        }
        if (!string.IsNullOrEmpty(resultMsg)) return false;
        return true;
    }


    private bool Import(DataTable dtExcel, ref string resultMsg)
    {
        //先清空代码对照表
        string strDeleteCardImportHQL = "truncate table T_WZCardImport";
        ShareClass.RunSqlCommand(strDeleteCardImportHQL);

        WZCardImportBLL wZCardImportBLL = new WZCardImportBLL();

        int successCount = 0;
        int lineNumber = 0;

        foreach (DataRow row in dtExcel.Rows)
        {
            int intID = 0;
            string strNoCode = string.Empty;
            string strObjectName = string.Empty;
            decimal decimalOutNumber = 0;
            decimal decimalOutPrice = 0;
            decimal decimalOutMoney = 0;
            decimal decimalPlanMoney = 0;

            try
            {
                lineNumber++;
                int.TryParse(ShareClass.ObjectToString(row[LanguageHandle.GetWord("XuHao").ToString().Trim()]), out intID);
                strNoCode = ShareClass.ObjectToString(row[LanguageHandle.GetWord("NoBianHao").ToString().Trim()]);
                strObjectName = ShareClass.ObjectToString(row[LanguageHandle.GetWord("WuZiMingChen").ToString().Trim()]);
                decimal.TryParse(ShareClass.ObjectToString(row[LanguageHandle.GetWord("ChuKuShuLiang").ToString().Trim()]), out decimalOutNumber);
                decimal.TryParse(ShareClass.ObjectToString(row[LanguageHandle.GetWord("ChuKuChanJia").ToString().Trim()]), out decimalOutPrice);
                decimal.TryParse(ShareClass.ObjectToString(row[LanguageHandle.GetWord("ChuKuJinE").ToString().Trim()]), out decimalOutMoney);
                decimal.TryParse(ShareClass.ObjectToString(row[LanguageHandle.GetWord("YuSuanJinE").ToString().Trim()]), out decimalPlanMoney);

                if (intID == 0 && string.IsNullOrEmpty(strNoCode) && string.IsNullOrEmpty(strObjectName)
                        && decimalOutNumber == 0 && decimalOutPrice == 0
                        && decimalOutMoney == 0 && decimalPlanMoney == 0)
                {
                    break;
                }

                WZCardImport wZCardImport = new WZCardImport();
                wZCardImport.ID = intID;
                wZCardImport.NoCode = strNoCode;
                wZCardImport.ObjectName = strObjectName;
                wZCardImport.OutNumber = decimalOutNumber;
                wZCardImport.OutPrice = decimalOutPrice;
                wZCardImport.OutMoney = decimalOutMoney;
                wZCardImport.PlanMoney = decimalPlanMoney;

                wZCardImport.ImportStatus = "x";
                wZCardImport.CardCode = "x";
                wZCardImport.PickingCode = "x";
                wZCardImport.MaterialPerson = "x";
                wZCardImport.TurnCode = "x";

                wZCardImportBLL.AddWZCardImport(wZCardImport);

                successCount++;
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile(this.GetType().BaseType.Name + "：" + LanguageHandle.GetWord("ZZJGDRSBJC").ToString().Trim() + " : " + LanguageHandle.GetWord("HangHao").ToString().Trim() + ": " + (lineNumber + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa").ToString().Trim() + ": " + strNoCode + " : " + err.Message.ToString());
            }
        }

        if (successCount > 0)
        {
            if (successCount == dtExcel.Rows.Count)
            {
                resultMsg += string.Format(LanguageHandle.GetWord("brYiChengGongDaoRu0TiaoShuJu").ToString().Trim(), successCount);
            }
            //else
            //{
            //    resultMsg += string.Format("<br/>已成功导入 {0} 条数据， 共有 {1} 条数据验证失败", successCount, dtExcel.Rows.Count - successCount);
            //}

            //重新加载列表
            DataCardImportBinder();

            return true;
        }
        else
        {
            resultMsg += string.Format(LanguageHandle.GetWord("brWeiDaoRuShuJuGongYou0TiaoShu").ToString().Trim(), dtExcel.Rows.Count - successCount);
        }

        return false;
    }


    protected void DDL_Card_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataCardImportBinder();
    }



    public void Export3Excel(DataTable dtData, string strFileName)
    {
        DataGrid dgControl = new DataGrid();
        dgControl.DataSource = dtData;
        dgControl.DataBind();


        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + strFileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/shlnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dgControl.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();

    }
}
