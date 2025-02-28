using System; using System.Resources;
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
using System.IO;

public partial class TTWZCompactCheckListNewDetail : System.Web.UI.Page
{
    string strUserCode;
    string strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        strUserName = Session["UserName"] == null ? "" : Session["UserName"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                string strID = Request.QueryString["ID"];

                HF_ID.Value = strID;
                DataCompactCheckBinder(strID);

                HF_Checker.Value = strUserCode;
                TXT_Checker.Text = strUserName;
                TXT_CheckerDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
    }

    private void DataCompactCheckBinder(string strID)
    {
        //查询检号列表
        string strCompactCheckHQL = string.Format(@"select * from T_WZCompactCheck
                        where ID = {0}", strID);
        DataTable dtCompactCheck = ShareClass.GetDataSetFromSql(strCompactCheckHQL, "CompactCheck").Tables[0];

        if (dtCompactCheck != null && dtCompactCheck.Rows.Count > 0)
        {
            DataRow drCompactCheck = dtCompactCheck.Rows[0];


            TXT_CheckCode.Text = ShareClass.ObjectToString(drCompactCheck["CheckCode"]);
            TXT_ArrivalGoodsNumber.Text = ShareClass.ObjectToString(drCompactCheck["ArrivalGoodsNumber"]);
            TXT_ArrivalGoodsName.Text = ShareClass.ObjectToString(drCompactCheck["ArrivalGoodsName"]);
            TXT_ArrivalGoodsModel.Text = ShareClass.ObjectToString(drCompactCheck["ArrivalGoodsModel"]);
            TXT_Factory.Text = ShareClass.ObjectToString(drCompactCheck["Factory"]);
            TXT_ExecutionStandard.Text = ShareClass.ObjectToString(drCompactCheck["ExecutionStandard"]);
            TXT_BatchNo.Text = ShareClass.ObjectToString(drCompactCheck["BatchNo"]);

            string strCheckDocument = ShareClass.ObjectToString(drCompactCheck["CheckDocument"]);
            string strCheckDocumentURL = ShareClass.ObjectToString(drCompactCheck["CheckDocumentURL"]);
            HF_CheckDocument.Value = strCheckDocument;
            HF_CheckDocumentURL.Value = strCheckDocumentURL;
            LT_CheckDocument.Text = "<a href=\"" + strCheckDocumentURL + "\" class=\"notTab\" target=\"_blank\">" + strCheckDocument + "</a>";

            TXT_DeliveryStatus.Text = ShareClass.ObjectToString(drCompactCheck["DeliveryStatus"]);

            string strReinspectionRecord = ShareClass.ObjectToString(drCompactCheck["ReinspectionRecord"]);
            string strReinspectionRecordURL = ShareClass.ObjectToString(drCompactCheck["ReinspectionRecordURL"]);
            HF_ReinspectionRecord.Value = strReinspectionRecord;
            HF_ReinspectionRecordURL.Value = strReinspectionRecordURL;
            LT_ReinspectionRecord.Text = "<a href=\"" + strReinspectionRecordURL + "\" class=\"notTab\" target=\"_blank\">" + strReinspectionRecord + "</a>";

            TXT_Remark.Text = ShareClass.ObjectToString(drCompactCheck["Remark"]);


            TXT_CheckCode.BackColor = Color.CornflowerBlue;
            TXT_ArrivalGoodsNumber.BackColor = Color.CornflowerBlue;
            TXT_ArrivalGoodsName.BackColor = Color.CornflowerBlue;
            TXT_ArrivalGoodsModel.BackColor = Color.CornflowerBlue;
            TXT_Factory.BackColor = Color.CornflowerBlue;
            TXT_ExecutionStandard.BackColor = Color.CornflowerBlue;
            TXT_BatchNo.BackColor = Color.CornflowerBlue;

            TXT_DeliveryStatus.BackColor = Color.CornflowerBlue;

            TXT_Remark.BackColor = Color.CornflowerBlue;

        }
    }
    
    protected void BT_Save_Click(object sender, EventArgs e)
    {
        //查询检号列表
        string strID = HF_ID.Value;
        WZCompactCheckBLL wZCompactCheckBLL = new WZCompactCheckBLL();
        string strCompactCheckHQL = string.Format(@" from WZCompactCheck as wZCompactCheck where ID = {0}", strID);
        IList lstCompactCheck = wZCompactCheckBLL.GetAllWZCompactChecks(strCompactCheckHQL);
        if (lstCompactCheck != null && lstCompactCheck.Count > 0)
        {
            WZCompactCheck wZCompactCheck = (WZCompactCheck)lstCompactCheck[0];

            string strCheckCode = TXT_CheckCode.Text.Trim();

            if (string.IsNullOrEmpty(strCheckCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('检号不能为空，请补充！');", true);
                return;
            }

            string strArrivalGoodsNumber = TXT_ArrivalGoodsNumber.Text.Trim();
            string strArrivalGoodsName = TXT_ArrivalGoodsName.Text.Trim();
            string strArrivalGoodsModel = TXT_ArrivalGoodsModel.Text.Trim();
            string strFactory = TXT_Factory.Text.Trim();
            string strExecutionStandard = TXT_ExecutionStandard.Text.Trim();
            string strBatchNo = TXT_BatchNo.Text.Trim();

            string strCheckDocument = HF_CheckDocument.Value.Trim();
            string strCheckDocumentURL = HF_CheckDocumentURL.Value.Trim();

            string strDeliveryStatus = TXT_DeliveryStatus.Text.Trim();

           string strReinspectionRecord = HF_ReinspectionRecord.Value;
            string strReinspectionRecordURL = HF_ReinspectionRecordURL.Value;

            string strRemark = TXT_Remark.Text.Trim();

            wZCompactCheck.CheckCode = strCheckCode;
            decimal decimalArrivalGoodsNumber = 0;
            decimal.TryParse(strArrivalGoodsNumber, out decimalArrivalGoodsNumber);
            wZCompactCheck.ArrivalGoodsNumber = decimalArrivalGoodsNumber;
            wZCompactCheck.ArrivalGoodsName = strArrivalGoodsName;
            wZCompactCheck.ArrivalGoodsModel = strArrivalGoodsModel;
            wZCompactCheck.Factory = strFactory;
            wZCompactCheck.ExecutionStandard = strExecutionStandard;
            wZCompactCheck.BatchNo = strBatchNo;
            if (!string.IsNullOrEmpty(strCheckDocument) && !string.IsNullOrEmpty(strCheckDocumentURL))
            {
                wZCompactCheck.CheckDocument = strCheckDocument;
                wZCompactCheck.CheckDocumentURL = strCheckDocumentURL;
            }
            wZCompactCheck.DeliveryStatus = strDeliveryStatus;
            if (!string.IsNullOrEmpty(strReinspectionRecord) && !string.IsNullOrEmpty(strReinspectionRecordURL))
            {
                wZCompactCheck.ReinspectionRecord = strReinspectionRecord;
                wZCompactCheck.ReinspectionRecordURL = strReinspectionRecordURL;
            }
            wZCompactCheck.Remark = strRemark;

            wZCompactCheck.CheckerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");
            wZCompactCheck.Progress = "材检";

            wZCompactCheckBLL.UpdateWZCompactCheck(wZCompactCheck, wZCompactCheck.ID);


            //检号〈材检日期〉＝“系统日期”												
            //检号〈进度〉＝“材检”												
            //合同明细〈检号〉＝检号〈检号〉												
            //合同明细〈材检标志〉＝“-1”												
            string strUpdateCompactDetailSQL = string.Format(@"update T_WZCompactDetail
                        set CheckCode = '{0}',
                        IsCheck = -1
                        where ID = {1}", strCheckCode, wZCompactCheck.CompactDetailID);
            ShareClass.RunSqlCommand(strUpdateCompactDetailSQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('检号不存在，请点击编辑进来！');", true);
            return;
        }
    }

    protected void BT_CheckDocument_Click(object sender, EventArgs e)
    {
        string strID = HF_ID.Value;

        if (!string.IsNullOrEmpty(strID))
        {
            try
            {
                string strCheckDocument = FUP_CheckDocument.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strCheckDocument))
                {
                    string strExtendName = System.IO.Path.GetExtension(strCheckDocument);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strCheckDocument);
                    string strExtName = Path.GetExtension(strFileName2);

                    string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

                    string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


                    FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

                    if (fi.Exists)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+Resources.lang.ZZCZTMWJSCSBGMHZSC+"');</script>");
                        return;
                    }

                    if (Directory.Exists(strDocSavePath) == false)
                    {
                        //如果不存在就创建file文件夹{
                        Directory.CreateDirectory(strDocSavePath);
                    }

                    FUP_CheckDocument.SaveAs(strDocSavePath + strFileName3);


                    string strUrl = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                    LT_CheckDocument.Text = "<a href=\"" + strUrl + "\" class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_CheckDocument.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_CheckDocumentURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    WZCompactCheckBLL wZCompactCheckBLL = new WZCompactCheckBLL();
                    string strCompactCheckHQL = "from WZCompactCheck as wZCompactCheck where ID = " + strID;
                    IList listCompactCheck = wZCompactCheckBLL.GetAllWZCompactChecks(strCompactCheckHQL);
                    if (listCompactCheck != null && listCompactCheck.Count > 0)
                    {
                        WZCompactCheck wZCompactCheck = (WZCompactCheck)listCompactCheck[0];
                        wZCompactCheck.CheckDocument = Path.GetFileNameWithoutExtension(strFileName2);
                        wZCompactCheck.CheckDocumentURL = strUrl;

                        wZCompactCheckBLL.UpdateWZCompactCheck(wZCompactCheck, wZCompactCheck.ID);
                    }

                    //重新加载报价文件列表
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('上传材检资料成功！');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择要上传的文件！');", true);
                    return;
                }
            }
            catch (Exception ex) { }
        }
        else
        {
            try
            {
                string strCompactText = FUP_CheckDocument.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strCompactText))
                {
                    string strExtendName = System.IO.Path.GetExtension(strCompactText);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strCompactText);
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

                    FUP_CheckDocument.SaveAs(strDocSavePath + strFileName3);

                    LT_CheckDocument.Text = "<a href=\"" + "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3 + "\"  class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_CheckDocument.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_CheckDocumentURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    //重新加载报价文件列表
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('上传材检资料成功！');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择要上传的文件！');", true);
                    return;
                }
            }
            catch (Exception ex) { }
        }
    }










    protected void BT_ReinspectionRecord_Click(object sender, EventArgs e)
    {
        string strID = HF_ID.Value;

        if (!string.IsNullOrEmpty(strID))
        {
            try
            {
                string strReinspectionRecord = FUP_ReinspectionRecord.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strReinspectionRecord))
                {
                    string strExtendName = System.IO.Path.GetExtension(strReinspectionRecord);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strReinspectionRecord);
                    string strExtName = Path.GetExtension(strFileName2);

                    string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

                    string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


                    FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

                    if (fi.Exists)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+Resources.lang.ZZCZTMWJSCSBGMHZSC+"');</script>");
                        return;
                    }

                    if (Directory.Exists(strDocSavePath) == false)
                    {
                        //如果不存在就创建file文件夹{
                        Directory.CreateDirectory(strDocSavePath);
                    }

                    FUP_ReinspectionRecord.SaveAs(strDocSavePath + strFileName3);


                    string strUrl = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                    LT_ReinspectionRecord.Text = "<a href=\"" + strUrl + "\" class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    WZCompactCheckBLL wZCompactCheckBLL = new WZCompactCheckBLL();
                    string strCompactCheckHQL = "from WZCompactCheck as wZCompactCheck where ID = " + strID;
                    IList listWZCompactCheck = wZCompactCheckBLL.GetAllWZCompactChecks(strCompactCheckHQL);
                    if (listWZCompactCheck != null && listWZCompactCheck.Count > 0)
                    {
                        WZCompactCheck wZCompactCheck = (WZCompactCheck)listWZCompactCheck[0];
                        wZCompactCheck.ReinspectionRecord = Path.GetFileNameWithoutExtension(strFileName2);
                        wZCompactCheck.ReinspectionRecordURL = strUrl;

                        wZCompactCheckBLL.UpdateWZCompactCheck(wZCompactCheck, wZCompactCheck.ID);
                    }

                    //重新加载报价文件列表
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('上传报价文件成功！');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择要上传的文件！');", true);
                    return;
                }
            }
            catch (Exception ex) { }
        }
        else
        {
            try
            {
                string strCompactText = FUP_ReinspectionRecord.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strCompactText))
                {
                    string strExtendName = System.IO.Path.GetExtension(strCompactText);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strCompactText);
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

                    FUP_ReinspectionRecord.SaveAs(strDocSavePath + strFileName3);

                    LT_ReinspectionRecord.Text = "<a href=\"" + "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3 + "\"  class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_ReinspectionRecord.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_ReinspectionRecordURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    //重新加载报价文件列表
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('上传采购文件成功！');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择要上传的文件！');", true);
                    return;
                }
            }
            catch (Exception ex) { }
        }
    }











}