using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZObjectCodeEdit : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            BindObjectReferData();
        }
    }


    protected void btnImport_Click(object sender, EventArgs e)
    {
        string strObjectBigDocument = fileExcel.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
        if (!string.IsNullOrEmpty(strObjectBigDocument))
        {
            string strExtendName = System.IO.Path.GetExtension(strObjectBigDocument);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间
            string strFileName2 = System.IO.Path.GetFileName(strObjectBigDocument);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim()+"');</script>");
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

        Hashtable htObjectName = new Hashtable();                                     //物资名称

        foreach (DataRow row in dtExcel.Rows)
        {
            string strDLCode = string.Empty;
            string strDLName = string.Empty;
            lineNumber++;
            try
            {
                string strXLCode = ShareClass.ObjectToString(row[LanguageHandle.GetWord("XiaoLeiDaiMa").ToString().Trim()]);
                string strObjectName = ShareClass.ObjectToString(row[LanguageHandle.GetWord("WuZiMingChen").ToString().Trim()]);
                string strModel = ShareClass.ObjectToString(row[LanguageHandle.GetWord("GuiGeXingHao").ToString().Trim()]);

                if (string.IsNullOrEmpty(strXLCode) && string.IsNullOrEmpty(strObjectName) && string.IsNullOrEmpty(strModel))
                {
                    break;
                }

                if (!htObjectName.Contains(strObjectName))
                {
                    htObjectName.Add(strObjectName, "");
                }
                else
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangWuZiMingChenBuNengChong").ToString().Trim(), lineNumber);
                    continue;
                }

                if (string.IsNullOrEmpty(strXLCode))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangXiaoLeiDaiMaBuNengWeiKo").ToString().Trim(), lineNumber);
                    continue;
                }
                string strXLCodeHQL = "select count(1) from T_WZMaterialXL where XLCode = '" + strXLCode + "'";
                DataTable dtXLCode = ShareClass.GetDataSetFromSql(strXLCodeHQL, "strXLCodeHQL").Tables[0];
                if (dtXLCode == null || dtXLCode.Rows.Count == 0)
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangXiaoLeiDaiMaZaiXiaoLeiJ").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strObjectName))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangWuZiMingChenBuNengWeiKo").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strModel))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangGuiGeXingHaoBuNengWeiKo").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row[LanguageHandle.GetWord("BiaoZhun").ToString().Trim()])))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangBiaoZhunBuNengWeiKongbr").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row[LanguageHandle.GetWord("JiBie").ToString().Trim()])))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangJiBieBuNengWeiKongbr").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row[LanguageHandle.GetWord("JiLiangChanWei").ToString().Trim()])))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangJiLiangChanWeiBuNengWei").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row[LanguageHandle.GetWord("HuanSuanChanWei").ToString().Trim()])))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangJiLiangChanWeiBuNengWei").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row[LanguageHandle.GetWord("HuanSuanJiShu").ToString().Trim()])))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangHuanSuanJiShuBuNengWeiK").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row[LanguageHandle.GetWord("DuiZhaoMiaoShu").ToString().Trim()])))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangDuiZhaoMiaoShuBuNengWei").ToString().Trim(), lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row[LanguageHandle.GetWord("DuiZhaoBiaoZhun").ToString().Trim()])))
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangDuiZhaoBiaoZhunBuNengWe").ToString().Trim(), lineNumber);
                    continue;
                }

                //验证计量单位，换算单位是否在计量单位表中存在
                string strUnit = ShareClass.ObjectToString(row[LanguageHandle.GetWord("JiLiangChanWei").ToString().Trim()]);
                string strUnitHQL = "select count(1) as RowNumber from T_WZSpan where UnitName = '" + strUnit + "'";
                DataTable dtUnit = ShareClass.GetDataSetFromSql(strUnitHQL, "strUnitHQL").Tables[0];
                if (dtUnit == null || dtUnit.Rows.Count == 0)
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangJiLiangChanWeiZaiJiLian").ToString().Trim(), lineNumber);
                    continue;
                }

                string strConvertUnit = ShareClass.ObjectToString(row[LanguageHandle.GetWord("HuanSuanChanWei").ToString().Trim()]);
                string strConvertUnitHQL = "select count(1) as RowNumber from T_WZSpan where UnitName = '" + strConvertUnit + "'";
                DataTable dtConvertUnit = ShareClass.GetDataSetFromSql(strConvertUnitHQL, "strConvertUnitHQL").Tables[0];
                if (dtConvertUnit == null || dtConvertUnit.Rows.Count == 0)
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangHuanSuanChanWeiZaiJiLia").ToString().Trim(), lineNumber);
                    continue;
                }

                //换算系统必须为整形或者带小数点
                string strConvertRatio = ShareClass.ObjectToString(row[LanguageHandle.GetWord("HuanSuanJiShu").ToString().Trim()]);
                bool IsBool = ShareClass.CheckIsNumber(strConvertRatio);
                if (!IsBool)
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangHuanSuanJiShuZhiNengShi").ToString().Trim(), lineNumber);
                    continue;
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
        string strDeleteObjectCodeHQL = "delete T_WZObjectRefer";
        ShareClass.RunSqlCommand(strDeleteObjectCodeHQL);

        WZObjectReferBLL wZObjectReferBLL = new WZObjectReferBLL();

        int successCount = 0;
        int lineNumber = 0;

        foreach (DataRow row in dtExcel.Rows)
        {
            string strXLCode = string.Empty;
            string strObjectName = string.Empty;
            string strModel = string.Empty;
            string strCriterion = string.Empty;
            string strGrade = string.Empty;
            string strUnit = string.Empty;
            string strConvertUnit = string.Empty;
            string strConvertRatio = string.Empty;
            string strReferDesc = string.Empty;
            string strReferStandard = string.Empty;

            lineNumber++;
            strXLCode = ShareClass.ObjectToString(row[LanguageHandle.GetWord("XiaoLeiDaiMa").ToString().Trim()]).Trim();
            strObjectName = ShareClass.ObjectToString(row[LanguageHandle.GetWord("WuZiMingChen").ToString().Trim()]);

            if (string.IsNullOrEmpty(strXLCode) && string.IsNullOrEmpty(strObjectName))
            {
                break;
            }

            strModel = ShareClass.ObjectToString(row[LanguageHandle.GetWord("GuiGeXingHao").ToString().Trim()]);
            strCriterion = ShareClass.ObjectToString(row[LanguageHandle.GetWord("BiaoZhun").ToString().Trim()]);
            strGrade = ShareClass.ObjectToString(row[LanguageHandle.GetWord("JiBie").ToString().Trim()]);
            strUnit = ShareClass.ObjectToString(row[LanguageHandle.GetWord("JiLiangChanWei").ToString().Trim()]);
            string strUnitHQL = "select ID from T_WZSpan where UnitName = '" + strUnit + "'";
            DataTable dtUnit = ShareClass.GetDataSetFromSql(strUnitHQL, "strUnitHQL").Tables[0];
            int intUnit = 0;
            int.TryParse(dtUnit.Rows[0]["ID"].ToString(), out intUnit);
            strConvertUnit = ShareClass.ObjectToString(row[LanguageHandle.GetWord("HuanSuanChanWei").ToString().Trim()]);
            string strConvertUnitHQL = "select ID from T_WZSpan where UnitName = '" + strConvertUnit + "'";
            DataTable dtConvertUnit = ShareClass.GetDataSetFromSql(strConvertUnitHQL, "strConvertUnitHQL").Tables[0];
            int intConvertUnit = 0;
            int.TryParse(dtConvertUnit.Rows[0]["ID"].ToString(), out intConvertUnit);
            strConvertRatio = ShareClass.ObjectToString(row[LanguageHandle.GetWord("HuanSuanJiShu").ToString().Trim()]);
            decimal decimalConvertRatio = 0;
            decimal.TryParse(strConvertRatio, out decimalConvertRatio);
            strReferDesc = ShareClass.ObjectToString(row[LanguageHandle.GetWord("DuiZhaoMiaoShu").ToString().Trim()]);
            strReferStandard = ShareClass.ObjectToString(row[LanguageHandle.GetWord("DuiZhaoBiaoZhun").ToString().Trim()]);

            WZObjectRefer wZObjectRefer = new WZObjectRefer();
            wZObjectRefer.ObjectCode = "-"; //BasePageOrder.module.GetQueueObjectCode(strXLCode);//物资代码
            wZObjectRefer.XLCode = strXLCode;
            wZObjectRefer.ObjectName = strObjectName;
            wZObjectRefer.Model = strModel;
            wZObjectRefer.Criterion = strCriterion;
            wZObjectRefer.Grade = strGrade;
            wZObjectRefer.Unit = intUnit;
            wZObjectRefer.ConvertUnit = intConvertUnit;
            wZObjectRefer.ConvertRatio = decimalConvertRatio;
            wZObjectRefer.ReferDesc = strReferDesc;
            wZObjectRefer.ReferStandard = strReferStandard;

            wZObjectReferBLL.AddWZObjectRefer(wZObjectRefer);

            //修改小类代码的使用标记
            ShareClass.UpdateXLCodeStatus(strXLCode);

            successCount++;
        }

        if (successCount > 0)
        {
            if (successCount == dtExcel.Rows.Count)
            {
                resultMsg += string.Format(LanguageHandle.GetWord("brYiChengGongDaoRu0TiaoShuJu").ToString().Trim(), successCount);
            }
            else
            {
                resultMsg += string.Format(LanguageHandle.GetWord("brYiChengGongDaoRu0TiaoShuJuGo").ToString().Trim(), successCount, dtExcel.Rows.Count - successCount);
            }

            //重新加载列表
            BindObjectReferData();

            return true;
        }
        else
        {
            resultMsg += string.Format(LanguageHandle.GetWord("brWeiDaoRuShuJuGongYou0TiaoShu").ToString().Trim(), dtExcel.Rows.Count - successCount);
        }

        return false;
    }

    protected void lbTemplate_Click(object sender, EventArgs e)
    {
        // 下载项目对应相应模板.
        try
        {
            string templatePath = Server.MapPath(LanguageHandle.GetWord("DocTemplatesDuiZhaoDaiMaxls").ToString().Trim());


            FileUtils.Download(templatePath, string.Format("{0}.xls", LanguageHandle.GetWord("DuiZhaoDaiMa").ToString().Trim()), Response, false);
        }
        catch (Exception ex)
        { }
    }

    protected void BT_Pass_Click(object sender, EventArgs e)
    {
        string resultMsg = string.Empty;
        try
        {
            string strObjectReferHQL = "select * from T_WZObjectRefer";
            DataTable dtObjectRefer = ShareClass.GetDataSetFromSql(strObjectReferHQL, "strObjectReferHQL").Tables[0];
            if (dtObjectRefer != null && dtObjectRefer.Rows.Count > 0)
            {
                foreach (DataRow drObjectRefer in dtObjectRefer.Rows)
                {
                    string strXLCode = ShareClass.ObjectToString(drObjectRefer["XLCode"], "");
                    string strObjectName = ShareClass.ObjectToString(drObjectRefer["ObjectName"], "");
                    string strModel = ShareClass.ObjectToString(drObjectRefer["Model"], "");
                    string strCriterion = ShareClass.ObjectToString(drObjectRefer["Criterion"], "");
                    string strGrade = ShareClass.ObjectToString(drObjectRefer["Grade"], "");
                    string strUnit = ShareClass.ObjectToString(drObjectRefer["Unit"], "0");
                    int intUnit = int.Parse(strUnit);
                    string strConvertUnit = ShareClass.ObjectToString(drObjectRefer["ConvertUnit"], "0");
                    int intConvertUnit = int.Parse(strConvertUnit);
                    string strConvertRatio = ShareClass.ObjectToString(drObjectRefer["ConvertRatio"], "0");
                    decimal decimalConvertRatio = decimal.Parse(strConvertRatio);
                    string strReferDesc = ShareClass.ObjectToString(drObjectRefer["ReferDesc"], "");
                    string strReferStandard = ShareClass.ObjectToString(drObjectRefer["ReferStandard"], "");

                    string strObjectHQL = string.Format(@"select * from T_WZObject
                    where XLCode = '{0}' 
                    and ObjectName = '{1}'
                    and Model = '{2}'
                    and Criterion = '{3}'
                    and Grade = '{4}'
                    and Unit = {5}", strXLCode, strObjectName, strModel, strCriterion, strGrade, intUnit);
                    DataTable dtObject = ShareClass.GetDataSetFromSql(strObjectHQL, "strObjectHQL").Tables[0];
                    if (dtObject != null && dtObject.Rows.Count > 0)
                    {
                        //把代码对照的物资代码改为“重复”
                        string strUpdateObjectReferHQL = string.Format(@"update T_WZObjectRefer set ObjectCode = '重复' 
                            where XLCode = '{0}' 
                            and ObjectName = '{1}'
                            and Model = '{2}'
                            and Criterion = '{3}'
                            and Grade = '{4}'
                            and Unit = {5}", strXLCode, strObjectName, strModel, strCriterion, strGrade, intUnit); 
                        ShareClass.RunSqlCommand(strUpdateObjectReferHQL);
                    }
                    else
                    {
                        //在物资代码表添加一条新的记录
                        WZObjectBLL wZObjectBLL = new WZObjectBLL();
                        WZObject wZObject = new WZObject();
                        wZObject.DLCode = strXLCode.Substring(0, 2);
                        wZObject.ZLCode = strXLCode.Substring(0, 4);
                        wZObject.XLCode = strXLCode;
                        string strObjectCode = BasePageOrder.module.GetQueueObjectCode(strXLCode);
                        wZObject.ObjectCode = strObjectCode; //生成物资代码;TXT_ObjectCode.Text;
                        wZObject.Creater = strUserCode;
                        wZObject.ObjectName = strObjectName;
                        wZObject.Criterion = strCriterion;
                        wZObject.Grade = strGrade;
                        wZObject.Model = strModel;
                        wZObject.Unit = intUnit;
                        wZObject.ConvertUnit = intConvertUnit;
                        wZObject.ConvertRatio = decimalConvertRatio;
                        wZObject.ReferDesc = strReferDesc;
                        wZObject.ReferStandard = strReferStandard;
                        wZObject.Market = 0;
                        wZObject.CollectTime = DateTime.Now;

                        wZObjectBLL.AddWZObject(wZObject);

                        //修改小类代码的使用标记
                        ShareClass.UpdateXLCodeStatus(strXLCode);
                        //把对照代码表的物资代码改为当前的物资代码
                        string strUpdateObjectReferHQL = string.Format(@"update T_WZObjectRefer set ObjectCode = '{6}' 
                            where XLCode = '{0}' 
                            and ObjectName = '{1}'
                            and Model = '{2}'
                            and Criterion = '{3}'
                            and Grade = '{4}'
                            and Unit = {5}", strXLCode, strObjectName, strModel, strCriterion, strGrade, intUnit, strObjectCode);
                        ShareClass.RunSqlCommand(strUpdateObjectReferHQL);

                        resultMsg += string.Format(LanguageHandle.GetWord("WuZiDaiMa0XiaoLeiDaiMa1WuZiMin").ToString().Trim(),
                            strObjectCode, strXLCode, strObjectName, strModel, strCriterion, strGrade);
                    }

                   
                }


                //重新加载列表
                BindObjectReferData();
            }
        }
        catch (Exception ex) {
            lblMsg.Text = string.Format(LanguageHandle.GetWord("spanstylecolorredDaoRuShiChuXi").ToString().Trim(), ex.Message);
        }
    }

    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim(); ;
        DataTable dtObjectRefer = ShareClass.GetDataSetFromSql(strHQL, "strHQL").Tables[0];

        DG_List.DataSource = dtObjectRefer;
        DG_List.DataBind();
    }


    private void BindObjectReferData()
    {
        DG_List.CurrentPageIndex = 0;

        string strObjectReferHQL = @"select r.*,s.UnitName as UnitName,p.UnitName as ConvertUnitName from T_WZObjectRefer r
                    left join T_WZSpan s on r.Unit = s.ID
                    left join T_WZSpan p on r.ConvertUnit = p.ID";
        DataTable dtObjectRefer = ShareClass.GetDataSetFromSql(strObjectReferHQL, "strObjectReferHQL").Tables[0];

        DG_List.DataSource = dtObjectRefer;
        DG_List.DataBind();

        LB_Sql.Text = strObjectReferHQL;
    }


    
}
