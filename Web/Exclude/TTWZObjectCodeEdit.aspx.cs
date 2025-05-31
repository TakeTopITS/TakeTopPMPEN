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
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+Resources.lang.ZZCZTMWJSCSBGMHZSC+"');</script>");
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
            string strDLCode = string.Empty;
            string strDLName = string.Empty;
            lineNumber++;
            try
            {
                string strXLCode = ShareClass.ObjectToString(row["小类代码"]);
                string strObjectName = ShareClass.ObjectToString(row["物资名称"]);
                string strModel = ShareClass.ObjectToString(row["规格型号"]);

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
                    resultMsg += string.Format("第{0}行，物资名称不能重复<br/>", lineNumber);
                    continue;
                }

                if (string.IsNullOrEmpty(strXLCode))
                {
                    resultMsg += string.Format("第{0}行，小类代码不能为空<br/>", lineNumber);
                    continue;
                }
                string strXLCodeHQL = "select count(1) from T_WZMaterialXL where XLCode = '" + strXLCode + "'";
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
                string strUnitHQL = "select count(1) as RowNumber from T_WZSpan where UnitName = '" + strUnit + "'";
                DataTable dtUnit = ShareClass.GetDataSetFromSql(strUnitHQL, "strUnitHQL").Tables[0];
                if (dtUnit == null || dtUnit.Rows.Count == 0)
                {
                    resultMsg += string.Format("第{0}行，计量单位在计量单位的基础表中不存在<br/>", lineNumber);
                    continue;
                }

                string strConvertUnit = ShareClass.ObjectToString(row["换算单位"]);
                string strConvertUnitHQL = "select count(1) as RowNumber from T_WZSpan where UnitName = '" + strConvertUnit + "'";
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
            strXLCode = ShareClass.ObjectToString(row["小类代码"]).Trim();
            strObjectName = ShareClass.ObjectToString(row["物资名称"]);

            if (string.IsNullOrEmpty(strXLCode) && string.IsNullOrEmpty(strObjectName))
            {
                break;
            }

            strModel = ShareClass.ObjectToString(row["规格型号"]);
            strCriterion = ShareClass.ObjectToString(row["标准"]);
            strGrade = ShareClass.ObjectToString(row["级别"]);
            strUnit = ShareClass.ObjectToString(row["计量单位"]);
            string strUnitHQL = "select ID from T_WZSpan where UnitName = '" + strUnit + "'";
            DataTable dtUnit = ShareClass.GetDataSetFromSql(strUnitHQL, "strUnitHQL").Tables[0];
            int intUnit = 0;
            int.TryParse(dtUnit.Rows[0]["ID"].ToString(), out intUnit);
            strConvertUnit = ShareClass.ObjectToString(row["换算单位"]);
            string strConvertUnitHQL = "select ID from T_WZSpan where UnitName = '" + strConvertUnit + "'";
            DataTable dtConvertUnit = ShareClass.GetDataSetFromSql(strConvertUnitHQL, "strConvertUnitHQL").Tables[0];
            int intConvertUnit = 0;
            int.TryParse(dtConvertUnit.Rows[0]["ID"].ToString(), out intConvertUnit);
            strConvertRatio = ShareClass.ObjectToString(row["换算系数"]);
            decimal decimalConvertRatio = 0;
            decimal.TryParse(strConvertRatio, out decimalConvertRatio);
            strReferDesc = ShareClass.ObjectToString(row["对照描述"]);
            strReferStandard = ShareClass.ObjectToString(row["对照标准"]);

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
                resultMsg += string.Format("<br/>已成功导入 {0} 条数据", successCount);
            }
            else
            {
                resultMsg += string.Format("<br/>已成功导入 {0} 条数据， 共有 {1} 条数据验证失败", successCount, dtExcel.Rows.Count - successCount);
            }

            //重新加载列表
            BindObjectReferData();

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

                        resultMsg += string.Format("物资代码：{0}，小类代码{1}，物资名称{2}，规格型号{3}，标准{4}，级别{5}，成功同步到物资代码表<br/>",
                            strObjectCode, strXLCode, strObjectName, strModel, strCriterion, strGrade);
                    }

                   
                }


                //重新加载列表
                BindObjectReferData();
            }
        }
        catch (Exception ex) {
            lblMsg.Text = string.Format("<span style='color:red' >导入时出现以下错误: {0}!</span>", ex.Message);
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