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

public partial class TTWZSupplierApprove : System.Web.UI.Page
{
    string strUserCode;
    string strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";
        strUserName = Session["UserName"] != null ? Session["UserName"].ToString() : "";
        HF_UserCode.Value = strUserCode;
        HF_UserName.Value = strUserName;

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
            DataBinder();
        }
    }


    private void DataBinder()
    {
        DG_List.CurrentPageIndex = 0;

        string strSupplierHQL = string.Format(@"select s.*,m.UserName as AuditorName,q.UserName as QualityEngineerName,p.UserName as PushPersonName,
                                                a.UserName as CompetentMaterialsName,
                                                c.UserName as ContractWhoseName,
                                                l.UserName as CompetentLeadershipName
                                                from T_WZSupplier s
                                                left join T_ProjectMember m on s.Auditor = m.UserCode 
                                                left join T_ProjectMember p on s.PushPerson = p.UserCode 
                                                left join T_ProjectMember q on s.QualityEngineer = q.UserCode
                                                left join T_ProjectMember a on s.CompetentMaterials = a.UserCode
                                                left join T_ProjectMember c on s.ContractWhose = c.UserCode
                                                left join T_ProjectMember l on s.CompetentLeadership = l.UserCode
                                              
                                                where s.Progress in('提交','Approved','登记') 
                                               "
                                                 ); 
        //and (s.QualityEngineer = '{0}' or COALESCE(s.QualityEngineer,'') = '')", strUserCode);

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strSupplierHQL += " and s.Progress = '" + strProgress + "'";
        }

        strSupplierHQL += " order by s.SupplierNumber desc";

        //Label1.Text = strSupplierHQL;

        DataTable dtSupplier = ShareClass.GetDataSetFromSql(strSupplierHQL, "Supplier").Tables[0];

        DG_List.DataSource = dtSupplier;
        DG_List.DataBind();

        LB_Sql.Text = strSupplierHQL;

        LB_Record.Text = dtSupplier.Rows.Count.ToString();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            for (int i = 0; i < DG_List.Items.Count; i++)
            {
                DG_List.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdName = e.CommandName;
            if (cmdName == "click")
            {
                string cmdArges = e.CommandArgument.ToString();
                string[] arrOperate = cmdArges.Split('|');

                string strEditID = arrOperate[0].Trim();
                string strProgress = arrOperate[1].Trim();
                string strQualityEngineer = arrOperate[2].Trim();

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "','" + strQualityEngineer + "','"+strUserCode+"');", true);

                HF_NewID.Value = strEditID;
                HF_NewProgress.Value = strProgress;
                HF_NewQualityEngineer.Value = strQualityEngineer;
                //}
                //else if (cmdName == "approve")
                //{
                //    string cmdArges = e.CommandArgument.ToString();
                string strSupplierSql = string.Format(@"select s.*,m.UserName as AuditorName,p.UserName as QualityEngineerName from T_WZSupplier s
                                                left join T_ProjectMember m on s.Auditor = m.UserCode 
                                                left join T_ProjectMember p on s.QualityEngineer = p.UserCode 
                                                where id = {0}", strEditID);
                DataTable dtSupplier = ShareClass.GetDataSetFromSql(strSupplierSql, "Supplier").Tables[0];
                if (dtSupplier != null && dtSupplier.Rows.Count == 1)
                {
                    DataRow drSupplier = dtSupplier.Rows[0];

                    //if (ShareClass.ObjectToString(drSupplier["Progress"]) != "Approved")
                    //{
                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGYSJDBWPZBNSH").ToString().Trim()+"')", true);
                    //    return;
                    //}

                    HF_ID.Value = ShareClass.ObjectToString(drSupplier["ID"]);
                    HF_SupplierCode.Value = ShareClass.ObjectToString(drSupplier["SupplierCode"]);
                    TXT_SupplierNumber.Text = ShareClass.ObjectToString(drSupplier["SupplierNumber"]);
                    TXT_SupplierName.Text = ShareClass.ObjectToString(drSupplier["SupplierName"]);
                    TXT_MainSupplier.Text = ShareClass.ObjectToString(drSupplier["MainSupplier"]);
                    DDL_Grade.SelectedValue = ShareClass.ObjectToString(drSupplier["Grade"]);
                    TXT_QualityEngineer.Text = ShareClass.ObjectToString(drSupplier["QualityEngineerName"]);
                    HF_QualityEngineer.Value = ShareClass.ObjectToString(drSupplier["QualityEngineer"]);
                    TXT_Auditor.Text = ShareClass.ObjectToString(drSupplier["AuditorName"]);
                    HF_Auditor.Value = ShareClass.ObjectToString(drSupplier["Auditor"]);
                    TXT_ReviewDate.Text = ShareClass.ObjectToString(drSupplier["ReviewDate"]);

                    DDL_Progress.SelectedValue = ShareClass.ObjectToString(drSupplier["Progress"]);

                    LoadRelatedWL("SupplierManagement", "Supplier", int.Parse(strEditID));

                    if (DDL_Progress.SelectedValue.Trim() == LanguageHandle.GetWord("DengJi").ToString().Trim())
                    {
                        BT_NewEdit.Enabled = false;
                        BT_NewRegister.Enabled = false;
                    }
                    else
                    {
                        BT_NewEdit.Enabled = true;
                        BT_NewRegister.Enabled = true;
                    }
                }
            }
        }
    }

    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim();
        DataTable dtSupplier = ShareClass.GetDataSetFromSql(strHQL, "Supplier").Tables[0];

        DG_List.DataSource = dtSupplier;
        DG_List.DataBind();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strID = HF_ID.Value;
        if (!string.IsNullOrEmpty(strID))
        {
            string strMainSupplier = TXT_MainSupplier.Text.Trim();
            string strGrade = DDL_Grade.SelectedValue;
            string strAuditor = HF_Auditor.Value; //TXT_Auditor.Text;
            string strReviewDate = TXT_ReviewDate.Text;
            string strQualityEngineer = HF_QualityEngineer.Value; //TXT_QualityEngineer.Text;

            string strNewProgress = HF_NewProgress.Value;
            string strNewQualityEngineer = HF_NewQualityEngineer.Value;

            if (string.IsNullOrEmpty(strMainSupplier))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择主供物资！');ControlStatusChange('" + strNewProgress + "','" + strNewQualityEngineer + "','" + strUserCode + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strGrade))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择级别！');ControlStatusChange('" + strNewProgress + "','" + strNewQualityEngineer + "','" + strUserCode + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strAuditor))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择责任工程师！');ControlStatusChange('" + strNewProgress + "','" + strNewQualityEngineer + "','" + strUserCode + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strReviewDate))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择复审日期！');ControlStatusChange('" + strNewProgress + "','" + strNewQualityEngineer + "','" + strUserCode + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strQualityEngineer))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择质保工程师！');ControlStatusChange('" + strNewProgress + "','" + strNewQualityEngineer + "','" + strUserCode + "');", true); 
                return;
            }

            WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
            string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strID;
            IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
            if (supplierList != null && supplierList.Count == 1)
            {
                WZSupplier wZSupplier = (WZSupplier)supplierList[0];

                wZSupplier.MainSupplier = strMainSupplier;
                wZSupplier.Grade = strGrade;
                wZSupplier.Auditor = strAuditor;
                wZSupplier.ReviewDate = strReviewDate;
                wZSupplier.QualityEngineer = strQualityEngineer;

                wZSupplier.Progress = "Approved";
                wZSupplier.ApproveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

                //重新加载列表
                DataBinder();

                //HF_ID.Value = "";
                //HF_SupplierCode.Value = "";
                //TXT_SupplierNumber.Text = "";
                //TXT_SupplierName.Text = "";
                //TXT_MainSupplier.Text = "";
                //DDL_Grade.SelectedValue = "";
                //TXT_QualityEngineer.Text = "";
                //HF_QualityEngineer.Value = "";
                //TXT_Auditor.Text = "";
                //HF_Auditor.Value = "";
                //TXT_ReviewDate.Text = "";

                //TXT_MainSupplier.BackColor = Color.White;
                //DDL_Grade.BackColor = Color.White;
                //TXT_Auditor.BackColor = Color.White;
                //TXT_ReviewDate.BackColor = Color.White;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatusCloseChange();", true); 
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewQualityEngineer + "','" + strUserCode + "');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择要审核的供应商！');ControlStatusCloseChange();", true); 
            return;
        }
    }

    protected void BT_QualityEngineer_Click(object sender, EventArgs e)
    {
        TXT_QualityEngineer.Text = strUserName;
        HF_QualityEngineer.Value = strUserCode;

        if (!string.IsNullOrEmpty(HF_NewID.Value))
        {
            string strNewProgress = HF_NewProgress.Value;
            string strNewQualityEngineer = HF_NewQualityEngineer.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewQualityEngineer + "','" + strUserCode + "');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }

    protected void BT_Cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "";
        HF_SupplierCode.Value = "";
        TXT_SupplierNumber.Text = "";
        TXT_SupplierName.Text = "";
        TXT_MainSupplier.Text = "";
        DDL_Grade.SelectedValue = "";
        TXT_QualityEngineer.Text = "";
        HF_QualityEngineer.Value = "";
        TXT_Auditor.Text = "";
        HF_Auditor.Value = "";
        TXT_ReviewDate.Text = "";

        TXT_MainSupplier.BackColor = Color.White;
        DDL_Grade.BackColor = Color.White;
        TXT_Auditor.BackColor = Color.White;
        TXT_ReviewDate.BackColor = Color.White;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }

    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim() + "')", true);
            return;
        }

        string strSupplierSql = string.Format(@"select s.*,m.UserName as AuditorName,p.UserName as QualityEngineerName from T_WZSupplier s
                                                left join T_ProjectMember m on s.Auditor = m.UserCode 
                                                left join T_ProjectMember p on s.QualityEngineer = p.UserCode 
                                                where id = {0}", strEditID);
        DataTable dtSupplier = ShareClass.GetDataSetFromSql(strSupplierSql, "Supplier").Tables[0];
        if (dtSupplier != null && dtSupplier.Rows.Count == 1)
        {
            DataRow drSupplier = dtSupplier.Rows[0];

            HF_ID.Value = ShareClass.ObjectToString(drSupplier["ID"]);
            HF_SupplierCode.Value = ShareClass.ObjectToString(drSupplier["SupplierCode"]);
            TXT_SupplierNumber.Text = ShareClass.ObjectToString(drSupplier["SupplierNumber"]);
            TXT_SupplierName.Text = ShareClass.ObjectToString(drSupplier["SupplierName"]);
            TXT_MainSupplier.Text = ShareClass.ObjectToString(drSupplier["MainSupplier"]);
            DDL_Grade.SelectedValue = ShareClass.ObjectToString(drSupplier["Grade"]);
            TXT_Auditor.Text = ShareClass.ObjectToString(drSupplier["AuditorName"]);
            HF_Auditor.Value = ShareClass.ObjectToString(drSupplier["Auditor"]);
            TXT_ReviewDate.Text = ShareClass.ObjectToString(drSupplier["ReviewDate"]);
            TXT_QualityEngineer.Text = ShareClass.ObjectToString(drSupplier["QualityEngineerName"]);
            HF_QualityEngineer.Value = ShareClass.ObjectToString(drSupplier["QualityEngineer"]);

            TXT_MainSupplier.BackColor = Color.CornflowerBlue;
            DDL_Grade.BackColor = Color.CornflowerBlue;
            TXT_Auditor.BackColor = Color.CornflowerBlue;
            TXT_ReviewDate.BackColor = Color.CornflowerBlue;

            string strNewProgress = HF_NewProgress.Value;
            string strNewQualityEngineer = HF_NewQualityEngineer.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewQualityEngineer + "','" + strUserCode + "');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }



    protected void BT_NewRegister_Click(object sender, EventArgs e)
    {
        //登记
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim() + "')", true);
            return;
        }


        string strMainSupplier = TXT_MainSupplier.Text.Trim();
        string strGrade = DDL_Grade.SelectedValue.Trim();
        string strReviewDate = TXT_ReviewDate.Text.Trim();
        string strQualityEngineer = TXT_QualityEngineer.Text.Trim();
        string strAuditor = TXT_Auditor.Text.Trim();

        if (string.IsNullOrEmpty(strMainSupplier) | string.IsNullOrEmpty(strGrade) | string.IsNullOrEmpty(strMainSupplier) | string.IsNullOrEmpty(strReviewDate) | string.IsNullOrEmpty(strQualityEngineer) | string.IsNullOrEmpty(strAuditor))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZWZJBFSRQZRGCSZBGCSBNWKQJC").ToString().Trim() + "')", true);
            return;
        }



        WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
        string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strEditID;
        IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
        if (supplierList != null && supplierList.Count == 1)
        {
            WZSupplier wZSupplier = (WZSupplier)supplierList[0];

            wZSupplier.Progress = LanguageHandle.GetWord("DengJi").ToString().Trim();
            wZSupplier.ApproveTime = DateTime.Now.ToString("yyyy-MM-dd");

            wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

            //重新加载列表
            DataBinder();

            DDL_Progress.SelectedValue = LanguageHandle.GetWord("DengJi").ToString().Trim();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('登记成功！');ControlStatusCloseChange();", true); 
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }


    protected void BT_NewRegisterReturn_Click(object sender, EventArgs e)
    {
        //登记退回
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim() + "')", true);
            return;
        }

        WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
        string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strEditID;
        IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
        if (supplierList != null && supplierList.Count == 1)
        {
            WZSupplier wZSupplier = (WZSupplier)supplierList[0];

            wZSupplier.Progress = "Approved";
            wZSupplier.ApproveTime = "";

            wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

            //重新加载列表
            DataBinder();

            DDL_Progress.SelectedValue = "Approved";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('登记退回成功！');ControlStatusCloseChange();", true); 
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }


    protected void BT_NewBrowse_Click(object sender, EventArgs e)
    {
        //浏览
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim() + "')", true);
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewQualityEngineer = HF_NewQualityEngineer.Value;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZSupplierBrowse.aspx?id=" + strEditID + "');ControlStatusChange('" + strNewProgress + "','" + strNewQualityEngineer + "','" + strUserCode + "');", true);
    }

    protected void BT_NewChange_Click(object sender, EventArgs e)
    {
        //信息变更
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim() + "')", true);
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewQualityEngineer = HF_NewQualityEngineer.Value;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZSupplierEdit.aspx?id=" + strEditID + "');ControlStatusChange('" + strNewProgress + "','" + strNewQualityEngineer + "','" + strUserCode + "');", true);
    }



    protected void DDL_Progress_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBinder();

        string strProgress = DDL_Progress.SelectedValue.Trim();
        if(strProgress == "Approved")
        {
            BT_NewChange.Enabled = false;
        }
        else
        {
            BT_NewChange.Enabled = true;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    //加载列表
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }

    protected int LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType In (" + "'" + strRelatedType + "','Other')" + " and workFlow.RelatedID = " + intRelatedID.ToString();
        strHQL += " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        return lst.Count;
    }
}
