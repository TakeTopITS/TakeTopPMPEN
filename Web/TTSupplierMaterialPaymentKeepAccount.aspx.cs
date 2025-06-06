using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using System.Data.SqlClient;

using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTSupplierMaterialPaymentKeepAccount : System.Web.UI.Page
{
    string strProjectID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserName, strDepartString;

        strProjectID = "1";

        string strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "物料采购付款记账", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);
            LB_DepartString.Text = strDepartString;

            LoadSupplierMaterialPaymentApplicant(strUserCode);
        }
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strAOID, strAccountCode, strAccountName;
            int intReAndPayalbeID;
            string strHQL;
            IList lst;

            strAOID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid5.Items.Count; i++)
                {
                    DataGrid5.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                ProjectMaterialPaymentApplicant projectMaterialPaymentApplicant = GetProjectMaterialPaymentApplicant(strAOID);

                //插入应收应付数据到应收应付表
                //decimal deAmount = projectMaterialPaymentApplicant.CurrentTotalPaymentAmount;
                string strCurrencyType = projectMaterialPaymentApplicant.CurrencyType;
                string strReAndPayer = projectMaterialPaymentApplicant.PartA.Trim();
                string strStatus = projectMaterialPaymentApplicant.Status.Trim();
                int intProjectID = projectMaterialPaymentApplicant.ProjectID;
                string strApplicantCode = projectMaterialPaymentApplicant.UserCode.Trim();
                string strApplicantName = projectMaterialPaymentApplicant.UserName.Trim();
                string strPayMethod = projectMaterialPaymentApplicant.PaymentMethod.Trim();

                decimal deDetailAmount;

                if (strStatus != "Recorded")
                {
                    strHQL = "From ProjectMaterialPaymentApplicantDetail as projectMaterialPaymentApplicantDetail Where projectMaterialPaymentApplicantDetail.AOID = " + strAOID;
                    ProjectMaterialPaymentApplicantDetailBLL projectMaterialPaymentApplicantDetailBLL = new ProjectMaterialPaymentApplicantDetailBLL();
                    lst = projectMaterialPaymentApplicantDetailBLL.GetAllProjectMaterialPaymentApplicantDetails(strHQL);
                    ProjectMaterialPaymentApplicantDetail projectMaterialPaymentApplicantDetail = new ProjectMaterialPaymentApplicantDetail();

                    for (int i = 0; i < lst.Count; i++)
                    {
                        projectMaterialPaymentApplicantDetail = (ProjectMaterialPaymentApplicantDetail)lst[i];
                        strAccountName = projectMaterialPaymentApplicantDetail.AccountName;
                        strAccountCode = projectMaterialPaymentApplicantDetail.AccountCode;
                        deDetailAmount = projectMaterialPaymentApplicantDetail.Amount;

                        intReAndPayalbeID = ShareClass.InsertReceivablesOrPayableByAccount("Payables", "GoodsPUPO", "GoodsPUPO", strAOID, strAOID, strAccountCode, strAccountName, deDetailAmount, strCurrencyType, strReAndPayer, strApplicantCode, intProjectID);
                        ShareClass.InsertReceivablesOrPayableRecord("Payables", intReAndPayalbeID, deDetailAmount, strCurrencyType, strPayMethod, strReAndPayer, strApplicantCode, intProjectID);

                        strHQL = "Update T_ConstractPayable Set OutOfPocketAccount = " + deDetailAmount.ToString() + ",UNPayAmount = 0 Where ID = " + intReAndPayalbeID.ToString();
                        ShareClass.RunSqlCommand(strHQL);

                        //把申报费用列入项目费用
                        if (intProjectID > 1)
                        {
                            ShareClass.AddConstractPayAmountToProExpense(intProjectID.ToString(), intReAndPayalbeID.ToString(), strAccountCode, strAccountName, LanguageHandle.GetWord("WuZiCaiGouFeiYong"), deDetailAmount, strCurrencyType, strApplicantCode, strApplicantName);
                        }
                    }

                    strHQL = "Update T_ProjectMaterialPaymentApplicant Set Status = 'Recorded' Where AOID = " + strAOID;
                    ShareClass.RunSqlCommand(strHQL);


                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJZCG") + "')", true);


                    LoadSupplierMaterialPaymentApplicant(strUserCode);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCSQDYJJZBNCFJC") + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBBNZFJZ") + "')", true);
            }

        }

    }

    protected int GetProjectMaterialPaymentApplicantCodeCount(string strAOID, string strGoodsPOCode)
    {
        string strHQL;

        if (strGoodsPOCode != "")
        {
            strHQL = "Select * From T_ProjectMaterialPaymentApplicant Where AOID <> " + strAOID + " and AOName =  " + "'" + strGoodsPOCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMaterialPaymentApplicant");

            return ds.Tables[0].Rows.Count;
        }
        else
        {
            return 0;
        }
    }


    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        ProjectMaterialPaymentApplicantBLL puchaseOrderBLL = new ProjectMaterialPaymentApplicantBLL();
        IList lst = puchaseOrderBLL.GetAllProjectMaterialPaymentApplicants(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }


    protected void LoadSupplierMaterialPaymentApplicant(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectMaterialPaymentApplicant as projectMaterialPaymentApplicant where (projectMaterialPaymentApplicant.UserCode = " + "'" + strUserCode + "'";
        strHQL += " or projectMaterialPaymentApplicant.UserCode in (select  memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ")) ";
        strHQL += " and projectMaterialPaymentApplicant.ProjectID = 1 ";
        strHQL += " Order by projectMaterialPaymentApplicant.AOID DESC";
        ProjectMaterialPaymentApplicantBLL projectMaterialPaymentApplicantBLL = new ProjectMaterialPaymentApplicantBLL();
        lst = projectMaterialPaymentApplicantBLL.GetAllProjectMaterialPaymentApplicants(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }


    protected ProjectMaterialPaymentApplicant GetProjectMaterialPaymentApplicant(string strAOID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ProjectMaterialPaymentApplicant as projectMaterialPaymentApplicant Where projectMaterialPaymentApplicant.AOID = " + strAOID;
        ProjectMaterialPaymentApplicantBLL projectMaterialPaymentApplicantBLL = new ProjectMaterialPaymentApplicantBLL();
        lst = projectMaterialPaymentApplicantBLL.GetAllProjectMaterialPaymentApplicants(strHQL);
        ProjectMaterialPaymentApplicant projectMaterialPaymentApplicant = (ProjectMaterialPaymentApplicant)lst[0];

        return projectMaterialPaymentApplicant;
    }


}

