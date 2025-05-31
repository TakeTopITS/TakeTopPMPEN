using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZPurchasePlanListVolume : System.Web.UI.Page
{
    string strUserCode;
    string strPurchaseCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";
        strPurchaseCode = Request.QueryString["PurchaseCode"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(strPurchaseCode))
            {
                HF_PurchaseCode.Value = strPurchaseCode;
                DataPurchaseBinder(strPurchaseCode);
            }

            DDL_PurchaseMethod.BackColor = Color.CornflowerBlue;
            TXT_DisciplinarySupervision.BackColor = Color.CornflowerBlue;
            TXT_PurchaseStartTime.BackColor = Color.CornflowerBlue;
            TXT_ControlMoney.BackColor = Color.CornflowerBlue;
            TXT_PurchaseEndTime.BackColor = Color.CornflowerBlue;
        }
    }

    private void DataPurchaseBinder(string strPurchaseCode)
    {
        string strPurchaseHQL = string.Format(@"select p.*,
                        d.UserName as DisciplinarySupervisionName,
                        c.UserName as ControlMoneyName
                        from T_WZPurchase p
                        left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
                        left join T_ProjectMember c on p.ControlMoney = c.UserCode
                        where p.PurchaseCode = '{0}'", strPurchaseCode);

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        if (dtPurchase != null && dtPurchase.Rows.Count > 0)
        {
            DataRow drPurchase = dtPurchase.Rows[0];

            DDL_PurchaseMethod.SelectedValue = ShareClass.ObjectToString(drPurchase["PurchaseMethod"]);
            LT_PurchaseDocument.Text = "";

            string strPurchaseDocument = ShareClass.ObjectToString(drPurchase["PurchaseDocument"]);
            string strPurchaseDocumentURL = ShareClass.ObjectToString(drPurchase["PurchaseDocumentURL"]);
            LT_PurchaseDocument.Text = "<a href='" + strPurchaseDocumentURL + "' class=\"notTab\" target=\"_blank\">" + strPurchaseDocument + "</a>";
            HF_PurchaseDocument.Value = strPurchaseDocument;
            HF_PurchaseDocumentURL.Value = strPurchaseDocumentURL;

            TXT_DisciplinarySupervision.Text = ShareClass.ObjectToString(drPurchase["DisciplinarySupervisionName"]);
            HF_DisciplinarySupervision.Value = ShareClass.ObjectToString(drPurchase["DisciplinarySupervision"]);

            string strAssessmentDocument = ShareClass.ObjectToString(drPurchase["AssessmentDocument"]);
            string strAssessmentDocumentURL = ShareClass.ObjectToString(drPurchase["AssessmentDocumentURL"]);
            LT_AssessmentDocument.Text = "<a href='" + strAssessmentDocumentURL + "' class=\"notTab\" target=\"_blank\">" + strAssessmentDocument + "</a>";
            HF_AssessmentDocument.Value = strAssessmentDocument;
            HF_AssessmentDocumentURL.Value = strAssessmentDocumentURL;

            TXT_ControlMoney.Text = ShareClass.ObjectToString(drPurchase["ControlMoneyName"]);
            HF_ControlMoney.Value = ShareClass.ObjectToString(drPurchase["ControlMoney"]);

            try
            {
                TXT_PurchaseStartTime.Text = ShareClass.ObjectToString(DateTime.Parse(drPurchase["PurchaseStartTime"].ToString()).ToString("yyyy/MM/dd"));
            }
            catch
            {
                TXT_PurchaseStartTime.Text = DateTime.Now.ToString("yyyy/MM/dd");
            }
            try
            {
                TXT_PurchaseEndTime.Text = ShareClass.ObjectToString(DateTime.Parse(drPurchase["PurchaseEndTime"].ToString()).ToString("yyyy/MM/dd"));
            }
            catch
            {
                TXT_PurchaseEndTime.Text = DateTime.Now.ToString("yyyy/MM/dd");

            }

            //TXT_PurchaseStartTime.Text = ShareClass.ObjectToString(drPurchase["PurchaseStartTime"]);
            //TXT_PurchaseEndTime.Text = ShareClass.ObjectToString(drPurchase["PurchaseEndTime"]);

            HF_Supplier1.Value = ShareClass.ObjectToString(drPurchase["SupplierCode1"]);
            TXT_Supplier1.Text = GetSupplierName(ShareClass.ObjectToString(drPurchase["SupplierCode1"]));

            HF_Supplier2.Value = ShareClass.ObjectToString(drPurchase["SupplierCode2"]);
            TXT_Supplier2.Text = GetSupplierName(ShareClass.ObjectToString(drPurchase["SupplierCode2"]));

            HF_Supplier3.Value = ShareClass.ObjectToString(drPurchase["SupplierCode3"]);
            TXT_Supplier3.Text = GetSupplierName(ShareClass.ObjectToString(drPurchase["SupplierCode3"]));

            HF_Supplier4.Value = ShareClass.ObjectToString(drPurchase["SupplierCode4"]);
            TXT_Supplier4.Text = GetSupplierName(ShareClass.ObjectToString(drPurchase["SupplierCode4"]));

            HF_Supplier5.Value = ShareClass.ObjectToString(drPurchase["SupplierCode5"]);
            TXT_Supplier5.Text = GetSupplierName(ShareClass.ObjectToString(drPurchase["SupplierCode5"]));

            HF_Supplier6.Value = ShareClass.ObjectToString(drPurchase["SupplierCode6"]);
            TXT_Supplier6.Text = GetSupplierName(ShareClass.ObjectToString(drPurchase["SupplierCode6"]));



            //加载专家
            WZPurchaseExpertBLL wZPurchaseExpertBLL = new WZPurchaseExpertBLL();
            string strPurchaseExpertHQL = "from WZPurchaseExpert as wZPurchaseExpert where PurchaseCode = '" + strPurchaseCode + "'";
            IList lstPurchaseExpert = wZPurchaseExpertBLL.GetAllWZPurchaseExperts(strPurchaseExpertHQL);
            if (lstPurchaseExpert != null && lstPurchaseExpert.Count > 0)
            {
                for (int i = 0; i < lstPurchaseExpert.Count; i++)
                {
                    WZPurchaseExpert wZPurchaseExpert = (WZPurchaseExpert)lstPurchaseExpert[i];
                    if (string.IsNullOrEmpty(TXT_Expert1.Text))
                    {
                        TXT_Expert1.Text = wZPurchaseExpert.ExpertName;
                        HF_Expert1.Value = wZPurchaseExpert.ExpertCode;
                        continue;
                    }
                    if (string.IsNullOrEmpty(TXT_Expert2.Text))
                    {
                        TXT_Expert2.Text = wZPurchaseExpert.ExpertName;
                        HF_Expert2.Value = wZPurchaseExpert.ExpertCode;
                        continue;
                    }
                    if (string.IsNullOrEmpty(TXT_Expert3.Text))
                    {
                        TXT_Expert3.Text = wZPurchaseExpert.ExpertName;
                        HF_Expert3.Value = wZPurchaseExpert.ExpertCode;
                        continue;
                    }
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "CloseAutoExpert();", true);
            }
        }

        //打开4-6号供应商▼按钮，下拉选项为“空”，允许选定后删除该供应商；
        BT_SelectSupplier1.Enabled = true;
        BT_SelectSupplier2.Enabled = true;
        BT_SelectSupplier3.Enabled = true;
        BT_SelectSupplier4.Enabled = true;
        BT_SelectSupplier5.Enabled = true;
        BT_SelectSupplier6.Enabled = true;

        BT_DeleteSupplier1.Enabled = true;
        BT_DeleteSupplier2.Enabled = true;
        BT_DeleteSupplier3.Enabled = true;
        BT_DeleteSupplier4.Enabled = true;
        BT_DeleteSupplier5.Enabled = true;
        BT_DeleteSupplier6.Enabled = true;
    }

    protected string GetSupplierName(string strSupplierCode)
    {
        string strHQL;

        if (strSupplierCode != "" & strSupplierCode != null)
        {
            strHQL = "Select SupplierName From T_WZSupplier Where SupplierCode = " + "'" + strSupplierCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZSupplier");

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strPurchaseMethod = DDL_PurchaseMethod.SelectedValue;
            string strPurchaseStartTime = TXT_PurchaseStartTime.Text.Trim();
            DateTime dtPurchaseStartTime = DateTime.Now;
            DateTime.TryParse(strPurchaseStartTime, out dtPurchaseStartTime);
            string strPurchaseEndTime = TXT_PurchaseEndTime.Text.Trim();
            DateTime dtPurchaseEndTime = DateTime.Now;
            DateTime.TryParse(strPurchaseEndTime, out dtPurchaseEndTime);

            string strDisciplinarySupervision = HF_DisciplinarySupervision.Value;
            string strControlMoney = HF_ControlMoney.Value;

            string strSupplierName1 = TXT_Supplier1.Text;
            string strSupplier1 = HF_Supplier1.Value;
            string strSupplierName2 = TXT_Supplier2.Text;
            string strSupplier2 = HF_Supplier2.Value;
            string strSupplierName3 = TXT_Supplier3.Text;
            string strSupplier3 = HF_Supplier3.Value;

            string strSupplierName4 = TXT_Supplier4.Text;
            string strSupplier4 = HF_Supplier4.Value;
            string strSupplierName5 = TXT_Supplier5.Text;
            string strSupplier5 = HF_Supplier5.Value;
            string strSupplierName6 = TXT_Supplier6.Text;
            string strSupplier6 = HF_Supplier6.Value;

            string strExpertName1 = TXT_Expert1.Text;
            string strExpert1 = HF_Expert1.Value;
            string strExpertName2 = TXT_Expert2.Text;
            string strExpert2 = HF_Expert2.Value;
            string strExpertName3 = TXT_Expert3.Text;
            string strExpert3 = HF_Expert3.Value;


            if (string.IsNullOrEmpty(strPurchaseMethod))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('供应方式不能为空，请补充！');", true);
                return;
            }
            if (string.IsNullOrEmpty(strControlMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('价格监审不能为空，请补充！');", true);
                return;
            }

            if (strPurchaseMethod == "询比价" || strPurchaseMethod == "招标" || strPurchaseMethod == "框架")
            {
                //当采购文件〈采购方式〉＝询比价、招标、框架时，供应商个数≥3
                bool IsDaiYu = true;
                if (string.IsNullOrEmpty(strSupplierName1) | string.IsNullOrEmpty(strSupplierName2) | string.IsNullOrEmpty(strSupplierName3))
                {
                    IsDaiYu = false;
                }

                if (IsDaiYu == false)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('警告，保存失败，当采购文件〈采购方式〉＝询比价、招标、框架时，供应商个数要≥3，请检查！');", true);
                    return;
                }
            }

            WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();

            string strPurchaseDocumentURL = HF_PurchaseDocumentURL.Value;
            string strPurchaseDocumentName = HF_PurchaseDocument.Value;

            string strAssessmentDocumentURL = HF_AssessmentDocumentURL.Value;
            string strAssessmentDocument = HF_AssessmentDocument.Value;

            WZPurchase wZPurchase = new WZPurchase();

            if (!string.IsNullOrEmpty(HF_PurchaseCode.Value))
            {
                string strPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + HF_PurchaseCode.Value + "'";
                IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseHQL);
                if (listPurchase != null && listPurchase.Count > 0)
                {
                    wZPurchase = (WZPurchase)listPurchase[0];

                    if (wZPurchase.Progress != "提交")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('采购文件进度不为提交，不允许修改！');", true);
                        return;
                    }

                    if (string.IsNullOrEmpty(strControlMoney))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('价格监审不能为空，请补充！');", true);
                        return;
                    }

                    //修改
                    wZPurchase.PurchaseMethod = strPurchaseMethod;
                    wZPurchase.PurchaseStartTime = dtPurchaseStartTime.ToString();
                    wZPurchase.PurchaseEndTime = dtPurchaseEndTime.ToString();
                    if (!string.IsNullOrEmpty(strPurchaseDocumentName) && !string.IsNullOrEmpty(strPurchaseDocumentURL))
                    {
                        wZPurchase.PurchaseDocument = strPurchaseDocumentName;
                        wZPurchase.PurchaseDocumentURL = strPurchaseDocumentURL;
                    }
                    if (!string.IsNullOrEmpty(strAssessmentDocument) && !string.IsNullOrEmpty(strAssessmentDocumentURL))
                    {
                        wZPurchase.AssessmentDocument = strAssessmentDocument;
                        wZPurchase.AssessmentDocumentURL = strAssessmentDocumentURL;
                    }

                    wZPurchase.DisciplinarySupervision = strDisciplinarySupervision;
                    wZPurchase.ControlMoney = strControlMoney;

                    wZPurchase.SupplierCode1 = strSupplier1;
                    wZPurchase.SupplierCode2 = strSupplier2;
                    wZPurchase.SupplierCode3 = strSupplier3;
                    wZPurchase.SupplierCode4 = strSupplier4;
                    wZPurchase.SupplierCode5 = strSupplier5;
                    wZPurchase.SupplierCode6 = strSupplier6;

                    wZPurchase.ExpertCode1 = strExpert1;
                    wZPurchase.ExpertCode2 = strExpert2;
                    wZPurchase.ExpertCode3 = strExpert3;



                    wZPurchaseBLL.UpdateWZPurchase(wZPurchase, HF_PurchaseCode.Value);


                    //保存供应商
                    #region 供应商1-6逐一保存
                    string strDeleteSupplierSQL = string.Format(@"delete T_WZPurchaseSupplier where PurchaseCode = '{0}'", HF_PurchaseCode.Value);
                    ShareClass.RunSqlCommand(strDeleteSupplierSQL);

                    WZPurchaseSupplierBLL wZPurchaseSupplierBLL = new WZPurchaseSupplierBLL();
                    if (!string.IsNullOrEmpty(strSupplierName1) && !string.IsNullOrEmpty(strSupplier1))
                    {
                        string strCheckPurchaseSupplierHQL = string.Format(@"select * from T_WZPurchaseSupplier
                                        where PurchaseCode = '{0}'
                                        and SupplierCode = '{1}'", wZPurchase.PurchaseCode, strSupplier1);
                        DataTable dtCheckPurchaseSupplier = ShareClass.GetDataSetFromSql(strCheckPurchaseSupplierHQL, "PurchaseSupplier").Tables[0];
                        if (dtCheckPurchaseSupplier == null || dtCheckPurchaseSupplier.Rows.Count <= 0)
                        {
                            WZPurchaseSupplier wZPurchaseSupplier = new WZPurchaseSupplier();
                            wZPurchaseSupplier.PurchaseCode = wZPurchase.PurchaseCode;

                            wZPurchaseSupplier.SupplierCode = strSupplier1;
                            wZPurchaseSupplier.SupplierName = strSupplierName1;
                            wZPurchaseSupplier.IsConfirm = "0";

                            wZPurchaseSupplierBLL.AddWZPurchaseSupplier(wZPurchaseSupplier);

                            //修改供应商使用标记
                            string strUpdateSupplierHQL = "update T_WZSupplier set IsMark = -1 where SupplierCode = '" + strSupplier1 + "'";
                            ShareClass.RunSqlCommand(strUpdateSupplierHQL);
                        }
                    }
                    if (!string.IsNullOrEmpty(strSupplierName2) && !string.IsNullOrEmpty(strSupplier2))
                    {
                        string strCheckPurchaseSupplierHQL = string.Format(@"select * from T_WZPurchaseSupplier
                                        where PurchaseCode = '{0}'
                                        and SupplierCode = '{1}'", wZPurchase.PurchaseCode, strSupplier2);
                        DataTable dtCheckPurchaseSupplier = ShareClass.GetDataSetFromSql(strCheckPurchaseSupplierHQL, "PurchaseSupplier").Tables[0];
                        if (dtCheckPurchaseSupplier == null || dtCheckPurchaseSupplier.Rows.Count <= 0)
                        {
                            WZPurchaseSupplier wZPurchaseSupplier = new WZPurchaseSupplier();
                            wZPurchaseSupplier.PurchaseCode = wZPurchase.PurchaseCode;

                            wZPurchaseSupplier.SupplierCode = strSupplier2;
                            wZPurchaseSupplier.SupplierName = strSupplierName2;
                            wZPurchaseSupplier.IsConfirm = "0";

                            wZPurchaseSupplierBLL.AddWZPurchaseSupplier(wZPurchaseSupplier);

                            //修改供应商使用标记
                            string strUpdateSupplierHQL = "update T_WZSupplier set IsMark = -1 where SupplierCode = '" + strSupplier2 + "'";
                            ShareClass.RunSqlCommand(strUpdateSupplierHQL);
                        }
                    }
                    if (!string.IsNullOrEmpty(strSupplierName3) && !string.IsNullOrEmpty(strSupplier3))
                    {
                        string strCheckPurchaseSupplierHQL = string.Format(@"select * from T_WZPurchaseSupplier
                                        where PurchaseCode = '{0}'
                                        and SupplierCode = '{1}'", wZPurchase.PurchaseCode, strSupplier3);
                        DataTable dtCheckPurchaseSupplier = ShareClass.GetDataSetFromSql(strCheckPurchaseSupplierHQL, "PurchaseSupplier").Tables[0];
                        if (dtCheckPurchaseSupplier == null || dtCheckPurchaseSupplier.Rows.Count <= 0)
                        {
                            WZPurchaseSupplier wZPurchaseSupplier = new WZPurchaseSupplier();
                            wZPurchaseSupplier.PurchaseCode = wZPurchase.PurchaseCode;

                            wZPurchaseSupplier.SupplierCode = strSupplier3;
                            wZPurchaseSupplier.SupplierName = strSupplierName3;
                            wZPurchaseSupplier.IsConfirm = "0";

                            wZPurchaseSupplierBLL.AddWZPurchaseSupplier(wZPurchaseSupplier);

                            //修改供应商使用标记
                            string strUpdateSupplierHQL = "update T_WZSupplier set IsMark = -1 where SupplierCode = '" + strSupplier3 + "'";
                            ShareClass.RunSqlCommand(strUpdateSupplierHQL);
                        }
                    }
                    if (!string.IsNullOrEmpty(strSupplierName4) && !string.IsNullOrEmpty(strSupplier4))
                    {
                        string strCheckPurchaseSupplierHQL = string.Format(@"select * from T_WZPurchaseSupplier
                                        where PurchaseCode = '{0}'
                                        and SupplierCode = '{1}'", wZPurchase.PurchaseCode, strSupplier4);
                        DataTable dtCheckPurchaseSupplier = ShareClass.GetDataSetFromSql(strCheckPurchaseSupplierHQL, "PurchaseSupplier").Tables[0];
                        if (dtCheckPurchaseSupplier == null || dtCheckPurchaseSupplier.Rows.Count <= 0)
                        {
                            WZPurchaseSupplier wZPurchaseSupplier = new WZPurchaseSupplier();
                            wZPurchaseSupplier.PurchaseCode = wZPurchase.PurchaseCode;

                            wZPurchaseSupplier.SupplierCode = strSupplier4;
                            wZPurchaseSupplier.SupplierName = strSupplierName4;
                            wZPurchaseSupplier.IsConfirm = "0";

                            wZPurchaseSupplierBLL.AddWZPurchaseSupplier(wZPurchaseSupplier);

                            //修改供应商使用标记
                            string strUpdateSupplierHQL = "update T_WZSupplier set IsMark = -1 where SupplierCode = '" + strSupplier4 + "'";
                            ShareClass.RunSqlCommand(strUpdateSupplierHQL);
                        }
                    }
                    if (!string.IsNullOrEmpty(strSupplierName5) && !string.IsNullOrEmpty(strSupplier5))
                    {
                        string strCheckPurchaseSupplierHQL = string.Format(@"select * from T_WZPurchaseSupplier
                                        where PurchaseCode = '{0}'
                                        and SupplierCode = '{1}'", wZPurchase.PurchaseCode, strSupplier5);
                        DataTable dtCheckPurchaseSupplier = ShareClass.GetDataSetFromSql(strCheckPurchaseSupplierHQL, "PurchaseSupplier").Tables[0];
                        if (dtCheckPurchaseSupplier == null || dtCheckPurchaseSupplier.Rows.Count <= 0)
                        {
                            WZPurchaseSupplier wZPurchaseSupplier = new WZPurchaseSupplier();
                            wZPurchaseSupplier.PurchaseCode = wZPurchase.PurchaseCode;

                            wZPurchaseSupplier.SupplierCode = strSupplier5;
                            wZPurchaseSupplier.SupplierName = strSupplierName5;
                            wZPurchaseSupplier.IsConfirm = "0";

                            wZPurchaseSupplierBLL.AddWZPurchaseSupplier(wZPurchaseSupplier);

                            //修改供应商使用标记
                            string strUpdateSupplierHQL = "update T_WZSupplier set IsMark = -1 where SupplierCode = '" + strSupplier5 + "'";
                            ShareClass.RunSqlCommand(strUpdateSupplierHQL);
                        }
                    }
                    if (!string.IsNullOrEmpty(strSupplierName6) && !string.IsNullOrEmpty(strSupplier6))
                    {
                        string strCheckPurchaseSupplierHQL = string.Format(@"select * from T_WZPurchaseSupplier
                                        where PurchaseCode = '{0}'
                                        and SupplierCode = '{1}'", wZPurchase.PurchaseCode, strSupplier6);
                        DataTable dtCheckPurchaseSupplier = ShareClass.GetDataSetFromSql(strCheckPurchaseSupplierHQL, "PurchaseSupplier").Tables[0];
                        if (dtCheckPurchaseSupplier == null || dtCheckPurchaseSupplier.Rows.Count <= 0)
                        {
                            WZPurchaseSupplier wZPurchaseSupplier = new WZPurchaseSupplier();
                            wZPurchaseSupplier.PurchaseCode = wZPurchase.PurchaseCode;

                            wZPurchaseSupplier.SupplierCode = strSupplier6;
                            wZPurchaseSupplier.SupplierName = strSupplierName6;
                            wZPurchaseSupplier.IsConfirm = "0";

                            wZPurchaseSupplierBLL.AddWZPurchaseSupplier(wZPurchaseSupplier);

                            //修改供应商使用标记
                            string strUpdateSupplierHQL = "update T_WZSupplier set IsMark = -1 where SupplierCode = '" + strSupplier6 + "'";
                            ShareClass.RunSqlCommand(strUpdateSupplierHQL);
                        }
                    }
                    #endregion
                    //保存专家
                    #region 专家1-3逐一保存
                    string strDeleteExpertSQL = string.Format(@"delete T_WZPurchaseExpert where PurchaseCode = '{0}'", HF_PurchaseCode.Value);
                    ShareClass.RunSqlCommand(strDeleteExpertSQL);

                    WZPurchaseExpertBLL wZPurchaseExpertBLL = new WZPurchaseExpertBLL();
                    if (!string.IsNullOrEmpty(strExpertName1) && !string.IsNullOrEmpty(strExpert1))
                    {
                        string strCheckPurchaseExpertHQL = string.Format(@"select * from T_WZPurchaseExpert
                                                        where PurchaseCode = '{0}'
                                                        and ExpertCode = '{1}'", wZPurchase.PurchaseCode, strExpert1);
                        DataTable dtCheckPurchaseExpert = ShareClass.GetDataSetFromSql(strCheckPurchaseExpertHQL, "PurchaseExpert").Tables[0];
                        if (dtCheckPurchaseExpert == null || dtCheckPurchaseExpert.Rows.Count <= 0)
                        {

                            WZPurchaseExpert wZPurchaseExpert = new WZPurchaseExpert();
                            wZPurchaseExpert.PurchaseCode = wZPurchase.PurchaseCode;

                            wZPurchaseExpert.ExpertCode = strExpert1;
                            wZPurchaseExpert.ExpertName = strExpertName1;

                            wZPurchaseExpertBLL.AddWZPurchaseExpert(wZPurchaseExpert);
                        }
                    }
                    if (!string.IsNullOrEmpty(strExpertName2) && !string.IsNullOrEmpty(strExpert2))
                    {
                        string strCheckPurchaseExpertHQL = string.Format(@"select * from T_WZPurchaseExpert
                                                        where PurchaseCode = '{0}'
                                                        and ExpertCode = '{1}'", wZPurchase.PurchaseCode, strExpert2);
                        DataTable dtCheckPurchaseExpert = ShareClass.GetDataSetFromSql(strCheckPurchaseExpertHQL, "PurchaseExpert").Tables[0];
                        if (dtCheckPurchaseExpert == null || dtCheckPurchaseExpert.Rows.Count <= 0)
                        {

                            WZPurchaseExpert wZPurchaseExpert = new WZPurchaseExpert();
                            wZPurchaseExpert.PurchaseCode = wZPurchase.PurchaseCode;

                            wZPurchaseExpert.ExpertCode = strExpert2;
                            wZPurchaseExpert.ExpertName = strExpertName2;

                            wZPurchaseExpertBLL.AddWZPurchaseExpert(wZPurchaseExpert);
                        }
                    }
                    if (!string.IsNullOrEmpty(strExpertName3) && !string.IsNullOrEmpty(strExpert3))
                    {
                        string strCheckPurchaseExpertHQL = string.Format(@"select * from T_WZPurchaseExpert
                                                        where PurchaseCode = '{0}'
                                                        and ExpertCode = '{1}'", wZPurchase.PurchaseCode, strExpert3);
                        DataTable dtCheckPurchaseExpert = ShareClass.GetDataSetFromSql(strCheckPurchaseExpertHQL, "PurchaseExpert").Tables[0];
                        if (dtCheckPurchaseExpert == null || dtCheckPurchaseExpert.Rows.Count <= 0)
                        {

                            WZPurchaseExpert wZPurchaseExpert = new WZPurchaseExpert();
                            wZPurchaseExpert.PurchaseCode = wZPurchase.PurchaseCode;

                            wZPurchaseExpert.ExpertCode = strExpert3;
                            wZPurchaseExpert.ExpertName = strExpertName3;

                            wZPurchaseExpertBLL.AddWZPurchaseExpert(wZPurchaseExpert);
                        }
                    }
                    #endregion
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('采购文件不存在！');", true);
                return;
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');", true);
        }
        catch (Exception ex)
        {
        }
    }


    protected void BT_Upload_Click(object sender, EventArgs e)
    {
        string strPurchaseCode = HF_PurchaseCode.Value;
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            try
            {
                string strPurchaseOfferDocument = FUP_PurchaseOfferDocument.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strPurchaseOfferDocument))
                {
                    string strExtendName = System.IO.Path.GetExtension(strPurchaseOfferDocument);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strPurchaseOfferDocument);
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

                    FUP_PurchaseOfferDocument.SaveAs(strDocSavePath + strFileName3);

                    string strUrl = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                    LT_AssessmentDocument.Text = "<a href=\"" + "Doc\\" + strUrl + "\" class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_AssessmentDocument.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_AssessmentDocumentURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    //写入到采购报价文件表中
                    WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
                    string strPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
                    IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseHQL);
                    if (listPurchase != null && listPurchase.Count > 0)
                    {
                        WZPurchase wZPurchase = (WZPurchase)listPurchase[0];
                        wZPurchase.AssessmentDocument = Path.GetFileNameWithoutExtension(strFileName2);
                        wZPurchase.AssessmentDocumentURL = strUrl;

                        wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);
                    }

                    //加载报价文件


                    //重新加载报价文件列表

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('上传评标文件成功！');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择要上传的文件！');", true);
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先保存采购文件，再编辑报价文件！');", true);
            return;
        }
    }

    protected void BT_DeleteAssessmentDocument_Click(object sender, EventArgs e)
    {
        //写入到采购报价文件表中
        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseHQL);
        if (listPurchase != null && listPurchase.Count > 0)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];
            wZPurchase.AssessmentDocument = "";
            wZPurchase.AssessmentDocumentURL = "";

            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

            HF_AssessmentDocument.Value = "";
            HF_AssessmentDocumentURL.Value = "";
            LT_AssessmentDocument.Text = "";
        }
    }

    protected void BT_PurchaseFile_Click(object sender, EventArgs e)
    {
        string strPurchaseCode = HF_PurchaseCode.Value;
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            try
            {
                string strPurchaseOfferDocument = FUP_PurchaseDocument.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strPurchaseOfferDocument))
                {
                    string strExtendName = System.IO.Path.GetExtension(strPurchaseOfferDocument);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strPurchaseOfferDocument);
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

                    FUP_PurchaseDocument.SaveAs(strDocSavePath + strFileName3);


                    string strUrl = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                    LT_PurchaseDocument.Text = "<a href=\"" + "Doc\\" + strUrl + "\" class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
                    string strPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
                    IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseHQL);
                    if (listPurchase != null && listPurchase.Count > 0)
                    {
                        WZPurchase wZPurchase = (WZPurchase)listPurchase[0];
                        wZPurchase.PurchaseDocument = Path.GetFileNameWithoutExtension(strFileName2);
                        wZPurchase.PurchaseDocumentURL = strUrl;

                        wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);
                    }

                    //重新加载报价文件列表

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('上传招标文件成功！');", true);
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
                string strPurchaseOfferDocument = FUP_PurchaseDocument.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strPurchaseOfferDocument))
                {
                    string strExtendName = System.IO.Path.GetExtension(strPurchaseOfferDocument);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strPurchaseOfferDocument);
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

                    FUP_PurchaseOfferDocument.SaveAs(strDocSavePath + strFileName3);

                    LT_PurchaseDocument.Text = "<a href=\"" + "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3 + "\" class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_PurchaseDocument.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_PurchaseDocumentURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

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



    protected void BT_DeletePurchaseFile_Click(object sender, EventArgs e)
    {
        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseHQL);
        if (listPurchase != null && listPurchase.Count > 0)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];
            wZPurchase.PurchaseDocument = "";
            wZPurchase.PurchaseDocumentURL = "";

            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

            HF_PurchaseDocument.Value = "";
            HF_PurchaseDocumentURL.Value = "";
            LT_PurchaseDocument.Text = "";
        }
    }


    protected void BT_SuppierSystem_Click(object sender, EventArgs e)
    {
        //自动选择供应商
        string strPurchaseCode = HF_PurchaseCode.Value;
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            try
            {
                //判断是否已经提交
                string strPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
                WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
                IList lstPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseHQL);
                if (lstPurchase != null && lstPurchase.Count > 0)
                {
                    WZPurchase wZPurchase = (WZPurchase)lstPurchase[0];

                    if (wZPurchase.Progress != "提交")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('采购文件进度不为录入，不允许选择供应商了！');", true);
                        return;
                    }
                }

                //查询出采购清单里面的大类代码数组
                string strPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where PurchaseCode = '" + strPurchaseCode + "'";
                WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
                IList lstPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strPurchaseDetailHQL);
                if (lstPurchaseDetail != null && lstPurchaseDetail.Count > 0)
                {
                    string strPurchaseDetails = string.Empty;
                    ArrayList arrayPurchase = new ArrayList();
                    for (int i = 0; i < lstPurchaseDetail.Count; i++)
                    {
                        WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)lstPurchaseDetail[i];
                        string strObjectCode = wZPurchaseDetail.ObjectCode;
                        string strDLCode = strObjectCode.Substring(0, 2);
                        if (!arrayPurchase.Contains(strDLCode))
                        {
                            strPurchaseDetails += " and MainSupplier like '%" + strDLCode + "%'";

                            arrayPurchase.Add(strDLCode);
                        }
                    }

                    string strSupplierHQL = string.Format(@"select *  from T_WZSupplier 
                                                            where Grade <> '禁用'
                                                            and Progress in('登记','批准')
                                                            and SupplierCode in (Select UserCode From T_ProjectMember)
                                                            and ReviewDate::timestamp  > now()  
                                                             {0} 
                                                            order by random() limit 6", strPurchaseDetails);
                    DataTable dtSupplier = ShareClass.GetDataSetFromSql(strSupplierHQL, "Supplier").Tables[0];
                    if (dtSupplier != null && dtSupplier.Rows.Count > 0)
                    {
                        if (dtSupplier.Rows.Count == 1)
                        {
                            foreach (DataRow drSupplier in dtSupplier.Rows)
                            {
                                string strSupplierCode = ShareClass.ObjectToString(drSupplier["SupplierCode"]);
                                string strSupplierName = ShareClass.ObjectToString(drSupplier["SupplierName"]);
                                if (string.IsNullOrEmpty(TXT_Supplier1.Text))
                                {
                                    HF_Supplier1.Value = strSupplierCode;
                                    TXT_Supplier1.Text = strSupplierName;
                                    continue;
                                }
                            }
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('找到 1 家符合条件的供应商！');", true);
                            return;
                        }
                        if (dtSupplier.Rows.Count == 2)
                        {
                            foreach (DataRow drSupplier in dtSupplier.Rows)
                            {
                                string strSupplierCode = ShareClass.ObjectToString(drSupplier["SupplierCode"]);
                                string strSupplierName = ShareClass.ObjectToString(drSupplier["SupplierName"]);
                                if (string.IsNullOrEmpty(TXT_Supplier1.Text))
                                {
                                    HF_Supplier1.Value = strSupplierCode;
                                    TXT_Supplier1.Text = strSupplierName;

                                    continue;
                                }
                                else if (string.IsNullOrEmpty(TXT_Supplier2.Text))
                                {
                                    HF_Supplier2.Value = strSupplierCode;
                                    TXT_Supplier2.Text = strSupplierName;

                                    continue;
                                }
                            }

                            //打开3号供应商▼按钮，允许点击打开供方表单，手动选择供应商，或直接修改采购方式           OpenSupplier3
                            BT_SelectSupplier3.Enabled = true;
                            BT_SuppierSystem.Enabled = false;

                            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "OpenSupplier3();", true);
                            //return;
                        }
                        if (dtSupplier.Rows.Count == 3)
                        {
                            foreach (DataRow drSupplier in dtSupplier.Rows)
                            {
                                string strSupplierCode = ShareClass.ObjectToString(drSupplier["SupplierCode"]);
                                string strSupplierName = ShareClass.ObjectToString(drSupplier["SupplierName"]);
                                if (string.IsNullOrEmpty(TXT_Supplier1.Text))
                                {
                                    HF_Supplier1.Value = strSupplierCode;
                                    TXT_Supplier1.Text = strSupplierName;

                                    continue;
                                }
                                else if (string.IsNullOrEmpty(TXT_Supplier2.Text))
                                {
                                    HF_Supplier2.Value = strSupplierCode;
                                    TXT_Supplier2.Text = strSupplierName;

                                    continue;
                                }
                                else if (string.IsNullOrEmpty(TXT_Supplier3.Text))
                                {
                                    HF_Supplier3.Value = strSupplierCode;
                                    TXT_Supplier3.Text = strSupplierName;

                                    continue;
                                }
                            }

                            BT_SuppierSystem.Enabled = false;
                        }
                        if (dtSupplier.Rows.Count > 3)
                        {
                            foreach (DataRow drSupplier in dtSupplier.Rows)
                            {
                                string strSupplierCode = ShareClass.ObjectToString(drSupplier["SupplierCode"]);
                                string strSupplierName = ShareClass.ObjectToString(drSupplier["SupplierName"]);
                                if (string.IsNullOrEmpty(TXT_Supplier1.Text))
                                {
                                    HF_Supplier1.Value = strSupplierCode;
                                    TXT_Supplier1.Text = strSupplierName;

                                    continue;
                                }
                                else if (string.IsNullOrEmpty(TXT_Supplier2.Text))
                                {
                                    HF_Supplier2.Value = strSupplierCode;
                                    TXT_Supplier2.Text = strSupplierName;

                                    continue;
                                }
                                else if (string.IsNullOrEmpty(TXT_Supplier3.Text))
                                {
                                    HF_Supplier3.Value = strSupplierCode;
                                    TXT_Supplier3.Text = strSupplierName;

                                    continue;
                                }
                                else if (string.IsNullOrEmpty(TXT_Supplier4.Text))
                                {
                                    HF_Supplier4.Value = strSupplierCode;
                                    TXT_Supplier4.Text = strSupplierName;

                                    continue;
                                }
                                else if (string.IsNullOrEmpty(TXT_Supplier5.Text))
                                {
                                    HF_Supplier5.Value = strSupplierCode;
                                    TXT_Supplier5.Text = strSupplierName;

                                    continue;
                                }
                                else if (string.IsNullOrEmpty(TXT_Supplier6.Text))
                                {
                                    HF_Supplier6.Value = strSupplierCode;
                                    TXT_Supplier6.Text = strSupplierName;

                                    continue;
                                }
                            }

                            //打开4-6号供应商▼按钮，下拉选项为“空”，允许选定后删除该供应商；

                            BT_SelectSupplier4.Enabled = true;
                            BT_SelectSupplier5.Enabled = true;
                            BT_SelectSupplier6.Enabled = true;
                            BT_SuppierSystem.Enabled = false;

                            BT_DeleteSupplier4.Enabled = true;
                            BT_DeleteSupplier5.Enabled = true;
                            BT_DeleteSupplier6.Enabled = true;

                            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "OpenSupplier456();", true);
                            //return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('未找到符合条件的供应商！');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先编辑采购清单，再选择供应商！');", true);
                    return;
                }
            }
            catch (Exception ex) { }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先保存采购文件，再编辑供应商！');", true);
            return;
        }
    }


    protected void BT_ExpertSystem_Click(object sender, EventArgs e)
    {
        //自动选择专家
        string strPurchaseCode = HF_PurchaseCode.Value;
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            try
            {
                HF_Expert1.Value = "";
                TXT_Expert1.Text = "";

                HF_Expert2.Value = "";
                TXT_Expert2.Text = "";

                HF_Expert3.Value = "";
                TXT_Expert3.Text = "";

                //查询出采购清单里面的专业类别
                string strPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
                WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
                IList lstPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseHQL);
                if (lstPurchase != null && lstPurchase.Count > 0)
                {
                    WZPurchase wZPurchase = (WZPurchase)lstPurchase[0];

                    if (wZPurchase.Progress != "提交")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('采购文件进度不为录入，不允许选择专家了！');", true);
                        return;
                    }

                    string strPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where PurchaseCode = '" + strPurchaseCode + "'";
                    WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
                    IList lstPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strPurchaseDetailHQL);
                    if (lstPurchaseDetail != null && lstPurchaseDetail.Count > 0)
                    {
                        //选择专业范围相同的专家
                        string strPurchaseDetailSQL = string.Empty;
                        ArrayList arrayMajor = new ArrayList();


                        for (int i = 0; i < lstPurchaseDetail.Count; i++)
                        {
                            WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)lstPurchaseDetail[i];
                            string strMajorType = wZPurchaseDetail.MajorType;
                            if (!string.IsNullOrEmpty(strMajorType))
                            {
                                if (!arrayMajor.Contains(strMajorType))
                                {
                                    arrayMajor.Add(strMajorType);

                                    strPurchaseDetailSQL += string.Format(@" select * from (select ExpertCode,Name from T_WZExpert where ExpertType like '%{0}%' order by random() limit 1) t union", strMajorType);
                                    strPurchaseDetailSQL += string.Format(@" select * from (select ExpertCode,Name from T_WZExpert where ExpertType2 like '%{0}%' order by random() limit 1) t union", strMajorType);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('采购清单的专业类别为空，不允许为空，请先补充！');", true);
                                return;
                            }
                        }

                        if (!string.IsNullOrEmpty(strPurchaseDetailSQL))
                        {
                            if (strPurchaseDetailSQL.EndsWith("union"))
                            {
                                strPurchaseDetailSQL = strPurchaseDetailSQL.Substring(0, strPurchaseDetailSQL.LastIndexOf("union"));

                                DataTable dtPurchaseExpert = ShareClass.GetDataSetFromSql(strPurchaseDetailSQL, "PurchaseDetail").Tables[0];
                                if (dtPurchaseExpert != null && dtPurchaseExpert.Rows.Count > 0)
                                {
                                    foreach (DataRow drPurchaseExert in dtPurchaseExpert.Rows)
                                    {
                                        string strExpertCode = ShareClass.ObjectToString(drPurchaseExert["ExpertCode"]);
                                        string strName = ShareClass.ObjectToString(drPurchaseExert["Name"]);

                                        if (string.IsNullOrEmpty(TXT_Expert1.Text))
                                        {
                                            HF_Expert1.Value = strExpertCode;
                                            TXT_Expert1.Text = strName;
                                            continue;
                                        }

                                        else if (string.IsNullOrEmpty(TXT_Expert2.Text))
                                        {
                                            HF_Expert2.Value = strExpertCode;
                                            TXT_Expert2.Text = strName;
                                            continue;
                                        }

                                        else if (string.IsNullOrEmpty(TXT_Expert3.Text))
                                        {
                                            HF_Expert3.Value = strExpertCode;
                                            TXT_Expert3.Text = strName;
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    BT_SelectExpert1.Enabled = true;
                                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('采购清单的专业类别找不到相应的专家！');", true);
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先编辑采购清单，再选择专家！');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('采购编号不存在！');", true);
                    return;
                }
            }
            catch (Exception ex) { }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先保存采购文件，再编辑采购文件！');", true);
            return;
        }
    }

    protected void BT_SelectSupplier1_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "SelectSupplier('" + HF_Supplier1.Value + "','TXT_Supplier1','1','" + strPurchaseCode + "');", true);
    }

    protected void BT_SelectSupplier2_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "SelectSupplier('" + HF_Supplier2.Value + "','TXT_Supplier2','2','" + strPurchaseCode + "');", true);
    }

    protected void BT_SelectSupplier3_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "SelectSupplier('" + HF_Supplier3.Value + "','TXT_Supplier3','3','" + strPurchaseCode + "');", true);
    }

    protected void BT_SelectSupplier4_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "SelectSupplier('" + HF_Supplier4.Value + "','TXT_Supplier4','4','" + strPurchaseCode + "');", true);
    }

    protected void BT_SelectSupplier5_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "SelectSupplier('" + HF_Supplier5.Value + "','TXT_Supplier5','5','" + strPurchaseCode + "');", true);
    }

    protected void BT_SelectSupplier6_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "SelectSupplier('" + HF_Supplier6.Value + "','TXT_Supplier6','6','" + strPurchaseCode + "');", true);
    }

    protected void BT_SelectExpert1_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "SelectExpert('" + HF_Expert1.Value + "','TXT_Expert1');", true);
    }

    protected void BT_DeleteSupplier1_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strPurchaseCode, strSupplierCode;

        strPurchaseCode = HF_PurchaseCode.Value;
        strSupplierCode = HF_Supplier1.Value;

        strHQL = "Update T_WZPurchase Set SupplierCode4 = '' Where PurchaseCode = " + "'" + strPurchaseCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Delete From T_WZPurchaseSupplier  Where SupplierCode = " + "'" + strSupplierCode + "'" + " and PurchaseCode = " + "'" + strPurchaseCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        HF_Supplier1.Value = "";
        TXT_Supplier1.Text = "";
    }

    protected void BT_DeleteSupplier2_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strPurchaseCode, strSupplierCode;

        strPurchaseCode = HF_PurchaseCode.Value;
        strSupplierCode = HF_Supplier2.Value;

        strHQL = "Update T_WZPurchase Set SupplierCode4 = '' Where PurchaseCode = " + "'" + strPurchaseCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Delete From T_WZPurchaseSupplier  Where SupplierCode = " + "'" + strSupplierCode + "'" + " and PurchaseCode = " + "'" + strPurchaseCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        HF_Supplier2.Value = "";
        TXT_Supplier2.Text = "";
    }

    protected void BT_DeleteSupplier3_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strPurchaseCode, strSupplierCode;

        strPurchaseCode = HF_PurchaseCode.Value;
        strSupplierCode = HF_Supplier3.Value;

        strHQL = "Update T_WZPurchase Set SupplierCode4 = '' Where PurchaseCode = " + "'" + strPurchaseCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Delete From T_WZPurchaseSupplier  Where SupplierCode = " + "'" + strSupplierCode + "'" + " and PurchaseCode = " + "'" + strPurchaseCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        HF_Supplier3.Value = "";
        TXT_Supplier3.Text = "";
    }

    protected void BT_DeleteSupplier4_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strPurchaseCode, strSupplierCode;

        strPurchaseCode = HF_PurchaseCode.Value;
        strSupplierCode = HF_Supplier4.Value;

        strHQL = "Update T_WZPurchase Set SupplierCode4 = '' Where PurchaseCode = " + "'" + strPurchaseCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Delete From T_WZPurchaseSupplier  Where SupplierCode = " + "'" + strSupplierCode + "'" + " and PurchaseCode = " + "'" + strPurchaseCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        HF_Supplier4.Value = "";
        TXT_Supplier4.Text = "";
    }

    protected void BT_DeleteSupplier5_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strPurchaseCode, strSupplierCode;

        strPurchaseCode = HF_PurchaseCode.Value;
        strSupplierCode = HF_Supplier5.Value;


        strHQL = "Update   T_WZPurchase Set SupplierCode5 = '' Where PurchaseCode = " + "'" + strPurchaseCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Delete From T_WZPurchaseSupplier  Where SupplierCode = " + "'" + strSupplierCode + "'" + " and PurchaseCode = " + "'" + strPurchaseCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        HF_Supplier5.Value = "";
        TXT_Supplier5.Text = "";
    }

    protected void BT_DeleteSupplier6_Click(object sender, EventArgs e)
    {

        string strHQL;
        string strPurchaseCode, strSupplierCode;

        strPurchaseCode = HF_PurchaseCode.Value;
        strSupplierCode = HF_Supplier6.Value;

        strHQL = "Update   T_WZPurchase Set SupplierCode5 = '' Where PurchaseCode = " + "'" + strPurchaseCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Delete From T_WZPurchaseSupplier  Where SupplierCode = " + "'" + strSupplierCode + "'" + " and PurchaseCode = " + "'" + strPurchaseCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        HF_Supplier6.Value = "";
        TXT_Supplier6.Text = "";
    }


}