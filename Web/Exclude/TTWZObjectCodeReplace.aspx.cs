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

public partial class TTWZObjectCodeReplace : System.Web.UI.Page
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
        foreach (DataRow row in dtExcel.Rows)
        {
            lineNumber++;
            try
            {
                string strOldObjectCode = ShareClass.ObjectToString(row["原物资代码"]);
                string strNewObjectCode = ShareClass.ObjectToString(row["新物资代码"]);

                if (string.IsNullOrEmpty(strOldObjectCode) && string.IsNullOrEmpty(strNewObjectCode))
                {
                    break;
                }

                if (string.IsNullOrEmpty(strOldObjectCode))
                {
                    resultMsg += string.Format("第{0}行，原物资代码不能为空<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strNewObjectCode))
                {
                    resultMsg += string.Format("第{0}行，新物资代码不能为空<br/>", lineNumber);
                    continue;
                }

                string strOldBigCode = strOldObjectCode.Substring(0, 2);
                string strOldMiddleCode = strOldObjectCode.Substring(0, 4);
                string strOldSmallCode = strOldObjectCode.Substring(0, 6);


                string strNewBigCode = strNewObjectCode.Substring(0, 2);
                string strNewMiddleCode = strNewObjectCode.Substring(0, 4);
                string strNewSmallCode = strNewObjectCode.Substring(0, 6);


                if (strOldBigCode != strNewBigCode)
                {
                    resultMsg += string.Format("第{0}行，原物资代码与新物资代码大类不相等<br/>", lineNumber);
                    continue;
                }
                //if (strOldObjectCode != strNewObjectCode)
                //{
                //    resultMsg += string.Format("第{0}行，原物资代码与新物资代码相等，请输入正确的新物资代码<br/>", lineNumber);
                //    continue;
                //}
                if (strOldSmallCode == strNewSmallCode)
                {
                    resultMsg += string.Format("第{0}行，原物资代码与新物资代码在同一个小类下面，不允许替换，只能替换不同中类，小类下面的物资代码<br/>", lineNumber);
                    continue;
                }

                string strNewZLCodeHQL = "select count(1) from T_WZMaterialZL where ZLCode = '" + strNewMiddleCode + "'";
                DataTable dtNewZLCode = ShareClass.GetDataSetFromSql(strNewZLCodeHQL, "NewZLCode").Tables[0];
                if (dtNewZLCode == null || dtNewZLCode.Rows.Count == 0)
                {
                    resultMsg += string.Format("第{0}行，新物资代码在中类基础数据表中不存在<br/>", lineNumber);
                    continue;
                }


                string strNewXLCodeHQL = "select count(1) from T_WZMaterialXL where XLCode = '" + strNewSmallCode + "'";
                DataTable dtNewXLCode = ShareClass.GetDataSetFromSql(strNewXLCodeHQL, "NewXLCode").Tables[0];
                if (dtNewXLCode == null || dtNewXLCode.Rows.Count == 0)
                {
                    resultMsg += string.Format("第{0}行，新物资代码在小类基础数据表中不存在<br/>", lineNumber);
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
        //先清空物资代码替换表
        string strDeleteObjectReplaceHQL = "truncate table T_WZObjectReplace";
        ShareClass.RunSqlCommand(strDeleteObjectReplaceHQL);

        WZObjectReplaceBLL wZObjectReplaceBLL = new WZObjectReplaceBLL();

        int successCount = 0;
        int lineNumber = 0;

        foreach (DataRow row in dtExcel.Rows)
        {
            string strOldObjectCode = string.Empty;
            string strNewObjectCode = string.Empty;

            lineNumber++;
            strOldObjectCode = ShareClass.ObjectToString(row["原物资代码"]);
            strNewObjectCode = ShareClass.ObjectToString(row["新物资代码"]);

            if (string.IsNullOrEmpty(strOldObjectCode) && string.IsNullOrEmpty(strNewObjectCode))
            {
                break;
            }

            WZObjectReplace wZObjectReplace = new WZObjectReplace();

            wZObjectReplace.OldObjectCode = strOldObjectCode;
            wZObjectReplace.NewObjectCode = strNewObjectCode;

            wZObjectReplaceBLL.AddWZObjectReplace(wZObjectReplace);

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
            //BindObjectData();

            return true;
        }
        else
        {
            resultMsg += string.Format("<br/>未导入数据， 共有 {0} 条数据验证失败", dtExcel.Rows.Count - successCount);
        }

        return false;
    }




    private void BindObjectData()
    {
        DG_List.CurrentPageIndex = 0;

        string strObjectHQL = @"select * from T_WZObjectReplace";
        DataTable dtObject = ShareClass.GetDataSetFromSql(strObjectHQL, "strObjectHQL").Tables[0];

        DG_List.DataSource = dtObject;
        DG_List.DataBind();
    }



    protected void BT_Pass_Click(object sender, EventArgs e)
    {
        string resultMsg = string.Empty;
        try
        {
            string strObjectReplaceHQL = "select * from T_WZObjectReplace";
            DataTable dtObjectReplace = ShareClass.GetDataSetFromSql(strObjectReplaceHQL, "ObjectReplace").Tables[0];
            if (dtObjectReplace != null && dtObjectReplace.Rows.Count > 0)
            {
                foreach (DataRow drObjectRefer in dtObjectReplace.Rows)
                {
                    string strOldObjectCode = ShareClass.ObjectToString(drObjectRefer["OldObjectCode"], "");
                    string strNewObjectCode = ShareClass.ObjectToString(drObjectRefer["NewObjectCode"], "");

                    string strObjectCodeHQL = string.Format(@"select * from T_WZObject
                    where ObjectCode = '{0}'", strNewObjectCode);
                    DataTable dtObjectCode = ShareClass.GetDataSetFromSql(strObjectCodeHQL, "Object").Tables[0];
                    if (dtObjectCode != null && dtObjectCode.Rows.Count > 0)
                    {
                        //替换一
                        //操作步骤：												
                        //① 依次打开“计划明细、采购清单、移交明细、合同明细、收料单、库存、发料单”等与物资代码有关的表单												
                        //② 如果相关表单〈物资代码〉＝代码替换〈原物资代码〉												
                        //     写记录：相关表单〈物资代码〉＝代码替换〈新物资代码〉												
                        //     如本表单有多条符合条件的记录，应逐条替换												
                        //③ 循环操作，直到最后一个表单替换结束时为止												
                        //④ 删除 物资代码〈物资代码〉＝代码替换〈原物资代码〉的记录												
                        //⑤ 按上述步骤继续进行第2条记录的替换，直至代码替换表单最后一条记录为止												

                        #region 新的物资代码存在，直接替换，然后删除原来的物资代码
                        //计划明细
                        string strUpdatePlanDetailHQL = string.Format(@"update T_WZPickingPlanDetail 
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

                        //移交单
                        string strUpdateTurnDetailHQL = string.Format(@"update T_WZTurnDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdateTurnDetailHQL);

                        //采购清单
                        string strUpdatePurchaseDetailHQL = string.Format(@"update T_WZPurchaseDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdatePurchaseDetailHQL);

                        //合同明细
                        string strUpdateCompactDetailHQL = string.Format(@"update T_WZCompactDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdateCompactDetailHQL);

                        //收料单
                        string strUpdateCollectHQL = string.Format(@"update T_WZCollect
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdateCollectHQL);

                        //发料单
                        string strUpdateSendHQL = string.Format(@"update T_WZSend
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdateSendHQL);

                        //库别
                        string strUpdateStoreHQL = string.Format(@"update T_WZStore
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdateStoreHQL);

                        //删除原物资代码记录
                        string strDeleteObjectSQL = "delete T_WZObject where ObjectCode ='" + strOldObjectCode + "'";
                        ShareClass.RunSqlCommand(strDeleteObjectSQL);
                        #endregion
                    }
                    else
                    {
                        WZObjectBLL wZObjectBLL = new WZObjectBLL();
                        string strObjectSql = "from WZObject as wZObject where ObjectCode = '" + strOldObjectCode + "'";
                        IList objectList = wZObjectBLL.GetAllWZObjects(strObjectSql);
                        if (objectList != null && objectList.Count == 1)
                        {
                            WZObject wZObject = (WZObject)objectList[0];

                            string strNewBigCode = strNewObjectCode.Substring(0, 2);
                            string strNewMiddleCode = strNewObjectCode.Substring(0, 4);
                            string strNewSmallCode = strNewObjectCode.Substring(0, 6);

                            string strObjectName = wZObject.ObjectName;
                            string strCriterion = wZObject.Criterion;
                            string strGrade = wZObject.Grade;
                            string strModel = wZObject.Model;
                            int intUnit = wZObject.Unit;

                            string strCheckObjectHQL = string.Format(@"select * from T_WZObject 
                            where ObjectName = '{0}'
                            and Model = '{1}'
                            and Criterion = '{2}'
                            and Grade = '{3}'
                            and Unit = {4}
                            and XLCode = '{5}'", strObjectName, strModel, strCriterion, strGrade, intUnit, strNewSmallCode);
                            DataTable dtCheckObject = ShareClass.GetDataSetFromSql(strCheckObjectHQL, "CheckObject").Tables[0];
                            if (dtCheckObject != null && dtCheckObject.Rows.Count > 0)
                            {
                                DataRow drNewObject = dtCheckObject.Rows[0];

                                string strNewNewObjectCode = ShareClass.ObjectToString(drNewObject["ObjectCode"]);

                                #region 根据原物资名称，规格型号，标准，级别，单位，新物资代码的小类代码，如果存在，则直接修改
                                
                                //计划明细
                                string strUpdatePlanDetailHQL = string.Format(@"update T_WZPickingPlanDetail 
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

                                //移交单
                                string strUpdateTurnDetailHQL = string.Format(@"update T_WZTurnDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateTurnDetailHQL);

                                //采购清单
                                string strUpdatePurchaseDetailHQL = string.Format(@"update T_WZPurchaseDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdatePurchaseDetailHQL);

                                //合同明细
                                string strUpdateCompactDetailHQL = string.Format(@"update T_WZCompactDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateCompactDetailHQL);

                                //收料单
                                string strUpdateCollectHQL = string.Format(@"update T_WZCollect
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateCollectHQL);

                                //发料单
                                string strUpdateSendHQL = string.Format(@"update T_WZSend
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateSendHQL);

                                //库别
                                string strUpdateStoreHQL = string.Format(@"update T_WZStore
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateStoreHQL);

                                //删除原物资代码记录
                                string strDeleteObjectSQL = "delete T_WZObject where ObjectCode ='" + strOldObjectCode + "'";
                                ShareClass.RunSqlCommand(strDeleteObjectSQL);
                                #endregion
                            }
                            else
                            {
                                #region 根据原物资名称，规格型号，标准，级别，单位，新物资代码的小类代码，如果不存在，先修改原物资代码的中类，小类，物资代码，然后再继续修改计划明细，移交单等等，不用再删除原物资代码
                                //修改物资代码
                                string strUpdateObjectCodeSQL = string.Format(@"update T_WZObject set ZLCode = '{0}',XLCode ='{1}',ObjectCode='{2}' where ObjectCode='{3}'",
                                    strNewMiddleCode, strNewSmallCode, strNewObjectCode, strOldObjectCode);
                                ShareClass.RunSqlCommand(strUpdateObjectCodeSQL);

                                //计划明细
                                string strUpdatePlanDetailHQL = string.Format(@"update T_WZPickingPlanDetail 
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

                                //移交单
                                string strUpdateTurnDetailHQL = string.Format(@"update T_WZTurnDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateTurnDetailHQL);

                                //采购清单
                                string strUpdatePurchaseDetailHQL = string.Format(@"update T_WZPurchaseDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdatePurchaseDetailHQL);

                                //合同明细
                                string strUpdateCompactDetailHQL = string.Format(@"update T_WZCompactDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateCompactDetailHQL);

                                //收料单
                                string strUpdateCollectHQL = string.Format(@"update T_WZCollect
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateCollectHQL);

                                //发料单
                                string strUpdateSendHQL = string.Format(@"update T_WZSend
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateSendHQL);

                                //库别
                                string strUpdateStoreHQL = string.Format(@"update T_WZStore
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateStoreHQL);
                                #endregion
                            }
                        }
                    }
                }
            }

            resultMsg += "新物资代码替换原物资代码成功<br/>";
        }
        catch (Exception ex)
        {
            lblMsg.Text = string.Format("<span style='color:red' >导入时出现以下错误: {0}!</span>", ex.Message);
        }
    }
    

}