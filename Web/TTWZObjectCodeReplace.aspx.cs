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
        string strObjectBigDocument = fileExcel.PostedFile.FileName;   //��ȡ�ϴ��ļ����ļ���,������׺
        if (!string.IsNullOrEmpty(strObjectBigDocument))
        {
            string strExtendName = System.IO.Path.GetExtension(strObjectBigDocument);//��ȡ��չ��

            DateTime dtUploadNow = DateTime.Now; //��ȡϵͳʱ��
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
                //��������ھʹ���file�ļ���{
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
                lblMsg.Text = string.Format("<span style='color:red' >����ʱ�������´���: {0}!</span>", ex.Message);
            }
        }
    }


    /// <summary>
    /// ��֤���ݺϷ���.
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
                string strOldObjectCode = ShareClass.ObjectToString(row["ԭ���ʴ���"]);
                string strNewObjectCode = ShareClass.ObjectToString(row["�����ʴ���"]);

                if (string.IsNullOrEmpty(strOldObjectCode) && string.IsNullOrEmpty(strNewObjectCode))
                {
                    break;
                }

                if (string.IsNullOrEmpty(strOldObjectCode))
                {
                    resultMsg += string.Format("��{0}�У�ԭ���ʴ��벻��Ϊ��<br/>", lineNumber);
                    continue;
                }
                if (string.IsNullOrEmpty(strNewObjectCode))
                {
                    resultMsg += string.Format("��{0}�У������ʴ��벻��Ϊ��<br/>", lineNumber);
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
                    resultMsg += string.Format("��{0}�У�ԭ���ʴ����������ʴ�����಻���<br/>", lineNumber);
                    continue;
                }
                //if (strOldObjectCode != strNewObjectCode)
                //{
                //    resultMsg += string.Format("��{0}�У�ԭ���ʴ����������ʴ�����ȣ���������ȷ�������ʴ���<br/>", lineNumber);
                //    continue;
                //}
                if (strOldSmallCode == strNewSmallCode)
                {
                    resultMsg += string.Format("��{0}�У�ԭ���ʴ����������ʴ�����ͬһ��С�����棬�������滻��ֻ���滻��ͬ���࣬С����������ʴ���<br/>", lineNumber);
                    continue;
                }

                string strNewZLCodeHQL = "select count(1) from T_WZMaterialZL where ZLCode = '" + strNewMiddleCode + "'";
                DataTable dtNewZLCode = ShareClass.GetDataSetFromSql(strNewZLCodeHQL, "NewZLCode").Tables[0];
                if (dtNewZLCode == null || dtNewZLCode.Rows.Count == 0)
                {
                    resultMsg += string.Format("��{0}�У������ʴ���������������ݱ��в�����<br/>", lineNumber);
                    continue;
                }


                string strNewXLCodeHQL = "select count(1) from T_WZMaterialXL where XLCode = '" + strNewSmallCode + "'";
                DataTable dtNewXLCode = ShareClass.GetDataSetFromSql(strNewXLCodeHQL, "NewXLCode").Tables[0];
                if (dtNewXLCode == null || dtNewXLCode.Rows.Count == 0)
                {
                    resultMsg += string.Format("��{0}�У������ʴ�����С��������ݱ��в�����<br/>", lineNumber);
                    continue;
                }


            }
            catch (Exception ex)
            {
                lblMsg.Text = string.Format("<span style='color:red' >����ʱ�������´���: {0}!</span>", ex.Message);
            }

        }
        if (!string.IsNullOrEmpty(resultMsg)) return false;
        return true;
    }


    private bool Import(DataTable dtExcel, ref string resultMsg)
    {
        //��������ʴ����滻��
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
            strOldObjectCode = ShareClass.ObjectToString(row["ԭ���ʴ���"]);
            strNewObjectCode = ShareClass.ObjectToString(row["�����ʴ���"]);

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
                resultMsg += string.Format("<br/>�ѳɹ����� {0} ������", successCount);
            }
            else
            {
                resultMsg += string.Format("<br/>�ѳɹ����� {0} �����ݣ� ���� {1} ��������֤ʧ��", successCount, dtExcel.Rows.Count - successCount);
            }

            //���¼����б�
            //BindObjectData();

            return true;
        }
        else
        {
            resultMsg += string.Format("<br/>δ�������ݣ� ���� {0} ��������֤ʧ��", dtExcel.Rows.Count - successCount);
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
                        //�滻һ
                        //�������裺												
                        //�� ���δ򿪡��ƻ���ϸ���ɹ��嵥���ƽ���ϸ����ͬ��ϸ�����ϵ�����桢���ϵ����������ʴ����йصı�												
                        //�� �����ر������ʴ��롵�������滻��ԭ���ʴ��롵												
                        //     д��¼����ر������ʴ��롵�������滻�������ʴ��롵												
                        //     �籾���ж������������ļ�¼��Ӧ�����滻												
                        //�� ѭ��������ֱ�����һ�����滻����ʱΪֹ												
                        //�� ɾ�� ���ʴ��롴���ʴ��롵�������滻��ԭ���ʴ��롵�ļ�¼												
                        //�� ����������������е�2����¼���滻��ֱ�������滻�����һ����¼Ϊֹ												

                        #region �µ����ʴ�����ڣ�ֱ���滻��Ȼ��ɾ��ԭ�������ʴ���
                        //�ƻ���ϸ
                        string strUpdatePlanDetailHQL = string.Format(@"update T_WZPickingPlanDetail 
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

                        //�ƽ���
                        string strUpdateTurnDetailHQL = string.Format(@"update T_WZTurnDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdateTurnDetailHQL);

                        //�ɹ��嵥
                        string strUpdatePurchaseDetailHQL = string.Format(@"update T_WZPurchaseDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdatePurchaseDetailHQL);

                        //��ͬ��ϸ
                        string strUpdateCompactDetailHQL = string.Format(@"update T_WZCompactDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdateCompactDetailHQL);

                        //���ϵ�
                        string strUpdateCollectHQL = string.Format(@"update T_WZCollect
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdateCollectHQL);

                        //���ϵ�
                        string strUpdateSendHQL = string.Format(@"update T_WZSend
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdateSendHQL);

                        //���
                        string strUpdateStoreHQL = string.Format(@"update T_WZStore
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                        ShareClass.RunSqlCommand(strUpdateStoreHQL);

                        //ɾ��ԭ���ʴ����¼
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

                                #region ����ԭ�������ƣ�����ͺţ���׼�����𣬵�λ�������ʴ����С����룬������ڣ���ֱ���޸�
                                
                                //�ƻ���ϸ
                                string strUpdatePlanDetailHQL = string.Format(@"update T_WZPickingPlanDetail 
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

                                //�ƽ���
                                string strUpdateTurnDetailHQL = string.Format(@"update T_WZTurnDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateTurnDetailHQL);

                                //�ɹ��嵥
                                string strUpdatePurchaseDetailHQL = string.Format(@"update T_WZPurchaseDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdatePurchaseDetailHQL);

                                //��ͬ��ϸ
                                string strUpdateCompactDetailHQL = string.Format(@"update T_WZCompactDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateCompactDetailHQL);

                                //���ϵ�
                                string strUpdateCollectHQL = string.Format(@"update T_WZCollect
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateCollectHQL);

                                //���ϵ�
                                string strUpdateSendHQL = string.Format(@"update T_WZSend
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateSendHQL);

                                //���
                                string strUpdateStoreHQL = string.Format(@"update T_WZStore
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateStoreHQL);

                                //ɾ��ԭ���ʴ����¼
                                string strDeleteObjectSQL = "delete T_WZObject where ObjectCode ='" + strOldObjectCode + "'";
                                ShareClass.RunSqlCommand(strDeleteObjectSQL);
                                #endregion
                            }
                            else
                            {
                                #region ����ԭ�������ƣ�����ͺţ���׼�����𣬵�λ�������ʴ����С����룬��������ڣ����޸�ԭ���ʴ�������࣬С�࣬���ʴ��룬Ȼ���ټ����޸ļƻ���ϸ���ƽ����ȵȣ�������ɾ��ԭ���ʴ���
                                //�޸����ʴ���
                                string strUpdateObjectCodeSQL = string.Format(@"update T_WZObject set ZLCode = '{0}',XLCode ='{1}',ObjectCode='{2}' where ObjectCode='{3}'",
                                    strNewMiddleCode, strNewSmallCode, strNewObjectCode, strOldObjectCode);
                                ShareClass.RunSqlCommand(strUpdateObjectCodeSQL);

                                //�ƻ���ϸ
                                string strUpdatePlanDetailHQL = string.Format(@"update T_WZPickingPlanDetail 
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

                                //�ƽ���
                                string strUpdateTurnDetailHQL = string.Format(@"update T_WZTurnDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateTurnDetailHQL);

                                //�ɹ��嵥
                                string strUpdatePurchaseDetailHQL = string.Format(@"update T_WZPurchaseDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdatePurchaseDetailHQL);

                                //��ͬ��ϸ
                                string strUpdateCompactDetailHQL = string.Format(@"update T_WZCompactDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateCompactDetailHQL);

                                //���ϵ�
                                string strUpdateCollectHQL = string.Format(@"update T_WZCollect
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateCollectHQL);

                                //���ϵ�
                                string strUpdateSendHQL = string.Format(@"update T_WZSend
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strOldObjectCode, strNewObjectCode);
                                ShareClass.RunSqlCommand(strUpdateSendHQL);

                                //���
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

            resultMsg += "�����ʴ����滻ԭ���ʴ���ɹ�<br/>";
        }
        catch (Exception ex)
        {
            lblMsg.Text = string.Format("<span style='color:red' >����ʱ�������´���: {0}!</span>", ex.Message);
        }
    }
    

}