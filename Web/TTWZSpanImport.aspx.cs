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

public partial class TTWZSpanImport : System.Web.UI.Page
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
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + Resources.lang.ZZCZTMWJSCSBGMHZSC + "');</script>");
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
                string strDLCode = ShareClass.ObjectToString(row["������λ"]);

                if (string.IsNullOrEmpty(strDLCode))
                {
                    break;
                }

                if (string.IsNullOrEmpty(strDLCode))
                {
                    resultMsg += string.Format("��{0}�У�������λ����Ϊ��<br/>", lineNumber);
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
        //����յ�λ��
        string strDeleteSpanHQL = "truncate table T_WZSpan";
        ShareClass.RunSqlCommand(strDeleteSpanHQL);

        WZSpanBLL wZSpanBLL = new WZSpanBLL();

        int successCount = 0;
        int lineNumber = 0;

        foreach (DataRow row in dtExcel.Rows)
        {
            string strUnitName = string.Empty;



            try
            {
                lineNumber++;
                strUnitName = ShareClass.ObjectToString(row["������λ"]);

                if (string.IsNullOrEmpty(strUnitName))
                {
                    break;
                }

                WZSpan wZSpan = new WZSpan();

                wZSpan.UnitName = strUnitName;

                wZSpanBLL.AddWZSpan(wZSpan);

                successCount++;
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile(this.GetType().BaseType.Name + "��"  + Resources.lang.ZZJGDRSBJC + " : " + Resources.lang.HangHao + ": " + (lineNumber + 2).ToString() + " , " + Resources.lang.DaiMa + ": " + strUnitName + " : " + err.Message.ToString());
            }
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

    protected void lbTemplate_Click(object sender, EventArgs e)
    {
        // ������Ŀ��Ӧ��Ӧģ��.
        try
        {
            string templatePath = Server.MapPath("Doc/Templates/���մ���.xls");


            FileUtils.Download(templatePath, string.Format("{0}.xls", "���մ���"), Response, false);
        }
        catch (Exception ex)
        { }
    }


}