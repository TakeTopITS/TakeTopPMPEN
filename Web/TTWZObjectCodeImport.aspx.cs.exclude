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
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + Resources.lang.ZZCZTMWJSCSBGMHZSC + "');</script>");
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
                lblMsg.Text = string.Format("<span style='color:red' >导入时出现以下错误: {0}!</span>", ex.Message);
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
                //string strDLCode = ShareClass.ObjectToString(row["大类代码"]);
                //string strZLCode = ShareClass.ObjectToString(row["中类代码"]);
                //string strXLCode = ShareClass.ObjectToString(row["小类代码"]);
                string strObjectCode = ShareClass.ObjectToString(row["物资代码"]);

                string strObjectName = ShareClass.ObjectToString(row["物资名称"]);



                string strModel = ShareClass.ObjectToString(row["规格型号"]);

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
                    resultMsg += string.Format("第{0}行，物资名称不能重复<br/>", lineNumber);
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
                    resultMsg += string.Format("第{0}行，小类代码在小类基础数据表中不存在<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strObjectName))
                {
                    resultMsg += string.Format("第{0}行，物资名称不能为空<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strModel))
                {
                    resultMsg += string.Format("第{0}行，规格型号不能为空<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row["标准"])))
                {
                    resultMsg += string.Format("第{0}行，标准不能为空<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row["级别"])))
                {
                    resultMsg += string.Format("第{0}行，级别不能为空<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row["计量单位"])))
                {
                    resultMsg += string.Format("第{0}行，计量单位不能为空<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row["换算单位"])))
                {
                    resultMsg += string.Format("第{0}行，计量单位不能为空<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row["换算系数"])))
                {
                    resultMsg += string.Format("第{0}行，换算系数不能为空<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row["对照描述"])))
                {
                    resultMsg += string.Format("第{0}行，对照描述不能为空<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(ShareClass.ObjectToString(row["对照标准"])))
                {
                    resultMsg += string.Format("第{0}行，对照标准不能为空<br/>", lineNumber);
                    continue;
                }

                //验证计量单位，换算单位是否在计量单位表中存在
                string strUnit = ShareClass.ObjectToString(row["计量单位"]);
                if (strUnit == "千克")
                {
                    strUnit = "kg";
                }
                string strUnitHQL = "select * from T_WZSpan where UnitName = '" + strUnit + "'";
                DataTable dtUnit = ShareClass.GetDataSetFromSql(strUnitHQL, "strUnitHQL").Tables[0];
                if (dtUnit == null || dtUnit.Rows.Count == 0)
                {
                    resultMsg += string.Format("第{0}行，计量单位在计量单位的基础表中不存在<br/>", lineNumber);
                    continue;
                }

                string strConvertUnit = ShareClass.ObjectToString(row["换算单位"]);
                if (strConvertUnit == "千克")
                {
                    strConvertUnit = "kg";
                }
                string strConvertUnitHQL = "select * from T_WZSpan where UnitName = '" + strConvertUnit + "'";
                DataTable dtConvertUnit = ShareClass.GetDataSetFromSql(strConvertUnitHQL, "strConvertUnitHQL").Tables[0];
                if (dtConvertUnit == null || dtConvertUnit.Rows.Count == 0)
                {
                    resultMsg += string.Format("第{0}行，换算单位在计量单位的基础表中不存在<br/>", lineNumber);
                    continue;
                }

                //换算系统必须为整形或者带小数点
                string strConvertRatio = ShareClass.ObjectToString(row["换算系数"]);
                bool IsBool = ShareClass.CheckIsNumber(strConvertRatio);
                if (!IsBool)
                {
                    resultMsg += string.Format("第{0}行，换算系数只能是小数<br/>", lineNumber);
                    continue;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = string.Format("<span style='color:red' >导入时出现以下错误: {0}!</span>", ex.Message);
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
                //strDLCode = ShareClass.ObjectToString(row["大类代码"]);
                //strZLCode = ShareClass.ObjectToString(row["中类代码"]);
                //strXLCode = ShareClass.ObjectToString(row["小类代码"]);
                strObjectCode = ShareClass.ObjectToString(row["物资代码"]);

                strDLCode = strObjectCode.Substring(0, 2);
                strZLCode = strObjectCode.Substring(0, 4);
                strXLCode = strObjectCode.Substring(0, 6);

                strObjectName = ShareClass.ObjectToString(row["物资名称"]);

                if (string.IsNullOrEmpty(strDLCode) && string.IsNullOrEmpty(strZLCode) && string.IsNullOrEmpty(strXLCode) && string.IsNullOrEmpty(strObjectCode) && string.IsNullOrEmpty(strObjectName))
                {
                    break;
                }

                strModel = ShareClass.ObjectToString(row["规格型号"]);
                strCriterion = ShareClass.ObjectToString(row["标准"]);
                strGrade = ShareClass.ObjectToString(row["级别"]);
                strUnit = ShareClass.ObjectToString(row["计量单位"]);

                if (strUnit == "千克")
                {
                    strUnit = "kg";
                }

                string strUnitHQL = "select ID from T_WZSpan where UnitName = '" + strUnit + "'";
                DataTable dtUnit = ShareClass.GetDataSetFromSql(strUnitHQL, "strUnitHQL").Tables[0];

                if (dtUnit == null || dtUnit.Rows.Count == 0)
                {
                    resultMsg += string.Format("<br/>单位 {0} 不存在", strUnit);
                }

                int intUnit = 0;
                int.TryParse(dtUnit.Rows[0]["ID"].ToString(), out intUnit);
                strConvertUnit = ShareClass.ObjectToString(row["换算单位"]);

                if (strConvertUnit == "千克")
                {
                    strConvertUnit = "kg";
                }

                string strConvertUnitHQL = "select ID from T_WZSpan where UnitName = '" + strConvertUnit + "'";
                DataTable dtConvertUnit = ShareClass.GetDataSetFromSql(strConvertUnitHQL, "strConvertUnitHQL").Tables[0];

                if (dtConvertUnit == null || dtConvertUnit.Rows.Count == 0)
                {
                    resultMsg += string.Format("<br/>换算单位 {0} 不存在", strConvertUnit);
                }

                int intConvertUnit = 0;
                int.TryParse(dtConvertUnit.Rows[0]["ID"].ToString(), out intConvertUnit);
                strConvertRatio = ShareClass.ObjectToString(row["换算系数"]);
                decimal decimalConvertRatio = 0;
                decimal.TryParse(strConvertRatio, out decimalConvertRatio);
                strReferDesc = ShareClass.ObjectToString(row["对照描述"]);
                strReferStandard = ShareClass.ObjectToString(row["对照标准"]);

                string strCreater = ShareClass.ObjectToString(row["创建人"]);
                string strMarket = ShareClass.ObjectToString(row["市场行情"]);
                string strCollectTime = ShareClass.ObjectToString(row["采集日期"]);

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
                LogClass.WriteLogFile(this.GetType().BaseType.Name + "："  + Resources.lang.ZZJGDRSBJC + " : " + Resources.lang.HangHao + ": " + (lineNumber + 2).ToString() + " , " + Resources.lang.DaiMa + ": " + strObjectCode + " : " + err.Message.ToString());
            }
        }

        if (successCount > 0)
        {
            if (successCount == dtExcel.Rows.Count)
            {
                resultMsg += string.Format("<br/>已成功导入 {0} 条数据", successCount);
            }
            else
            {
                resultMsg += string.Format("<br/>已成功导入 {0} 条数据， 共有 {1} 条数据验证失败", successCount, dtExcel.Rows.Count - successCount);
            }

            //重新加载列表
            //BindObjectData();

            return true;
        }
        else
        {
            resultMsg += string.Format("<br/>未导入数据， 共有 {0} 条数据验证失败", dtExcel.Rows.Count - successCount);
        }

        return false;
    }

    protected void lbTemplate_Click(object sender, EventArgs e)
    {
        // 下载项目对应相应模板.
        try
        {
            string templatePath = Server.MapPath("Doc/Templates/对照代码.xls");


            FileUtils.Download(templatePath, string.Format("{0}.xls", "对照代码"), Response, false);
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