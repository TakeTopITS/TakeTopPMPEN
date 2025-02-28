using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZObjectCodeImport : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            //BindObjectData();
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
            lineNumber++;
            try
            {
                //string strDLCode = ShareClass.ObjectToString(row["Major Category Code"]);
                //string strZLCode = ShareClass.ObjectToString(row["中类代码"]);
                //string strXLCode = ShareClass.ObjectToString(row["小类代码"]);
                string strObjectCode = ShareClass.ObjectToString(row[LanguageHandle.GetWord("WuZiDaiMa").ToString().Trim()]);

                string strObjectName = ShareClass.ObjectToString(row[LanguageHandle.GetWord("WuZiMingChen").ToString().Trim()]);



                string strModel = ShareClass.ObjectToString(row[LanguageHandle.GetWord("GuiGeXingHao").ToString().Trim()]);

                if (string.IsNullOrEmpty(strObjectCode) && string.IsNullOrEmpty(strObjectName) && string.IsNullOrEmpty(strModel))
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


                //if (string.IsNullOrEmpty(strDLCode) && string.IsNullOrEmpty(strZLCode) && string.IsNullOrEmpty(strXLCode) && string.IsNullOrEmpty(strObjectCode) && string.IsNullOrEmpty(strObjectName))
                //{
                //    break;
                //}

                //if (string.IsNullOrEmpty(strXLCode))
                //{
                //    resultMsg += string.Format("第{0}行，小类代码不能为空<br/>", lineNumber);
                //    continue;
                //}

                //因为多了一位，去掉一位  5209005
                //string strNewXLCode1 = strXLCode.Substring(0, 4);
                //string strNewXLCode2 = strXLCode.Substring(5, 2);

                //string strNewXLCode = strNewXLCode1 + strNewXLCode2;

                string strNewXLCode = strObjectCode.Substring(0, 6);


                string strXLCodeHQL = "select count(1) from T_WZMaterialXL where XLCode = '" + strNewXLCode + "'";
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
                if (strUnit == LanguageHandle.GetWord("QianKe").ToString().Trim())
                {
                    strUnit = "kg";
                }
                string strUnitHQL = "select * from T_WZSpan where UnitName = '" + strUnit + "'";
                DataTable dtUnit = ShareClass.GetDataSetFromSql(strUnitHQL, "strUnitHQL").Tables[0];
                if (dtUnit == null || dtUnit.Rows.Count == 0)
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("Di0HangJiLiangChanWeiZaiJiLian").ToString().Trim(), lineNumber);
                    continue;
                }

                string strConvertUnit = ShareClass.ObjectToString(row[LanguageHandle.GetWord("HuanSuanChanWei").ToString().Trim()]);
                if (strConvertUnit == LanguageHandle.GetWord("QianKe").ToString().Trim())
                {
                    strConvertUnit = "kg";
                }
                string strConvertUnitHQL = "select * from T_WZSpan where UnitName = '" + strConvertUnit + "'";
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
        //string strDeleteObjectCodeHQL = "truncate table T_WZObject";
        //ShareClass.RunSqlCommand(strDeleteObjectCodeHQL);

        WZObjectBLL wZObjectBLL = new WZObjectBLL();

        int successCount = 0;
        int lineNumber = 0;

        foreach (DataRow row in dtExcel.Rows)
        {
            string strDLCode = string.Empty;
            string strZLCode = string.Empty;
            string strXLCode = string.Empty;
            string strObjectCode = string.Empty;
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

            try
            {
                //strDLCode = ShareClass.ObjectToString(row["Major Category Code"]);
                //strZLCode = ShareClass.ObjectToString(row["中类代码"]);
                //strXLCode = ShareClass.ObjectToString(row["小类代码"]);
                strObjectCode = ShareClass.ObjectToString(row[LanguageHandle.GetWord("WuZiDaiMa").ToString().Trim()]);

                strDLCode = strObjectCode.Substring(0, 2);
                strZLCode = strObjectCode.Substring(0, 4);
                strXLCode = strObjectCode.Substring(0, 6);

                strObjectName = ShareClass.ObjectToString(row[LanguageHandle.GetWord("WuZiMingChen").ToString().Trim()]);

                if (string.IsNullOrEmpty(strDLCode) && string.IsNullOrEmpty(strZLCode) && string.IsNullOrEmpty(strXLCode) && string.IsNullOrEmpty(strObjectCode) && string.IsNullOrEmpty(strObjectName))
                {
                    break;
                }

                strModel = ShareClass.ObjectToString(row[LanguageHandle.GetWord("GuiGeXingHao").ToString().Trim()]);
                strCriterion = ShareClass.ObjectToString(row[LanguageHandle.GetWord("BiaoZhun").ToString().Trim()]);
                strGrade = ShareClass.ObjectToString(row[LanguageHandle.GetWord("JiBie").ToString().Trim()]);
                strUnit = ShareClass.ObjectToString(row[LanguageHandle.GetWord("JiLiangChanWei").ToString().Trim()]);

                if (strUnit == LanguageHandle.GetWord("QianKe").ToString().Trim())
                {
                    strUnit = "kg";
                }

                string strUnitHQL = "select ID from T_WZSpan where UnitName = '" + strUnit + "'";
                DataTable dtUnit = ShareClass.GetDataSetFromSql(strUnitHQL, "strUnitHQL").Tables[0];

                if (dtUnit == null || dtUnit.Rows.Count == 0)
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("brChanWei0BuCunZai").ToString().Trim(), strUnit);
                }

                int intUnit = 0;
                int.TryParse(dtUnit.Rows[0]["ID"].ToString(), out intUnit);
                strConvertUnit = ShareClass.ObjectToString(row[LanguageHandle.GetWord("HuanSuanChanWei").ToString().Trim()]);

                if (strConvertUnit == LanguageHandle.GetWord("QianKe").ToString().Trim())
                {
                    strConvertUnit = "kg";
                }

                string strConvertUnitHQL = "select ID from T_WZSpan where UnitName = '" + strConvertUnit + "'";
                DataTable dtConvertUnit = ShareClass.GetDataSetFromSql(strConvertUnitHQL, "strConvertUnitHQL").Tables[0];

                if (dtConvertUnit == null || dtConvertUnit.Rows.Count == 0)
                {
                    resultMsg += string.Format(LanguageHandle.GetWord("brHuanSuanChanWei0BuCunZai").ToString().Trim(), strConvertUnit);
                }

                int intConvertUnit = 0;
                int.TryParse(dtConvertUnit.Rows[0]["ID"].ToString(), out intConvertUnit);
                strConvertRatio = ShareClass.ObjectToString(row[LanguageHandle.GetWord("HuanSuanJiShu").ToString().Trim()]);
                decimal decimalConvertRatio = 0;
                decimal.TryParse(strConvertRatio, out decimalConvertRatio);
                strReferDesc = ShareClass.ObjectToString(row[LanguageHandle.GetWord("DuiZhaoMiaoShu").ToString().Trim()]);
                strReferStandard = ShareClass.ObjectToString(row[LanguageHandle.GetWord("DuiZhaoBiaoZhun").ToString().Trim()]);

                string strCreater = ShareClass.ObjectToString(row[LanguageHandle.GetWord("ChuangJianRen").ToString().Trim()]);
                string strMarket = ShareClass.ObjectToString(row[LanguageHandle.GetWord("ShiChangHangQing").ToString().Trim()]);
                string strCollectTime = ShareClass.ObjectToString(row[LanguageHandle.GetWord("CaiJiRiJi").ToString().Trim()]);

                decimal decimalMarket = 0;
                decimal.TryParse(strMarket, out decimalMarket);

                DateTime dtCollectTime = DateTime.Now;
                DateTime.TryParse(strCollectTime, out dtCollectTime);


                WZObject wZObject = new WZObject();

                //因为多了一位，去掉一位  5209005
                //string strNewXLCode1 = strXLCode.Substring(0, 4);
                //string strNewXLCode2 = strXLCode.Substring(5, 2);

                //string strNewXLCode = strNewXLCode1 + strNewXLCode2;

                wZObject.DLCode = strDLCode;
                wZObject.ZLCode = strZLCode;
                wZObject.XLCode = strXLCode;
                wZObject.ObjectCode = strObjectCode;
                wZObject.ObjectName = strObjectName;
                wZObject.Model = strModel;
                wZObject.Criterion = strCriterion;
                wZObject.Grade = strGrade;
                wZObject.Unit = intUnit;
                wZObject.ConvertUnit = intConvertUnit;
                wZObject.ConvertRatio = decimalConvertRatio;
                wZObject.ReferDesc = strReferDesc;
                wZObject.ReferStandard = strReferStandard;
                wZObject.Market = decimalMarket;
                wZObject.CollectTime = dtCollectTime;

                wZObject.Creater = strCreater;

                wZObjectBLL.AddWZObject(wZObject);

                //修改小类代码的使用标记
                ShareClass.UpdateXLCodeStatus(strXLCode);

                successCount++;
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile(this.GetType().BaseType.Name + "："  + LanguageHandle.GetWord("ZZJGDRSBJC").ToString().Trim() + " : " + LanguageHandle.GetWord("HangHao").ToString().Trim() + ": " + (lineNumber + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa").ToString().Trim() + ": " + strObjectCode + " : " + err.Message.ToString());
            }
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
            //BindObjectData();

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



    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim();
        DataTable dtObjectRefer = ShareClass.GetDataSetFromSql(strHQL, "strHQL").Tables[0];

        DG_List.DataSource = dtObjectRefer;
        DG_List.DataBind();
    }


    private void BindObjectData()
    {
        DG_List.CurrentPageIndex = 0;

        string strObjectHQL = @"select r.*,s.UnitName as UnitName,p.UnitName as ConvertUnitName from T_WZObject r
                    left join T_WZSpan s on r.Unit = s.ID
                    left join T_WZSpan p on r.ConvertUnit = p.ID";
        DataTable dtObject = ShareClass.GetDataSetFromSql(strObjectHQL, "strObjectHQL").Tables[0];

        DG_List.DataSource = dtObject;
        DG_List.DataBind();

        LB_Sql.Text = strObjectHQL;
    }


}