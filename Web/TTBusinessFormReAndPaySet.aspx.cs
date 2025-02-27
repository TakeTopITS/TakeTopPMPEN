using System; using System.Resources;
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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTBusinessFormReAndPaySet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","业务单应收应付设置", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "业务单应收应付设置---" + System.Configuration.ConfigurationManager.AppSettings["SystemName"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            LoadBusinessFormReAndpay();
        }
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strFormCode, strReOrPay, strAccountCode, strAccountName;

        try
        {

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                strFormCode = DataGrid3.Items[i].Cells[0].Text;
                strReOrPay = ((DropDownList)DataGrid3.Items[i].FindControl("DL_ReceiveOrPay")).SelectedValue;
                strAccountCode = ((DropDownList)DataGrid3.Items[i].FindControl("DL_RelatedAccount")).SelectedValue;
                strAccountName = ShareClass.GetAccountName(strAccountCode);

                strHQL = "Update T_BusinessFormReAndPay Set ReceiveOrPay = " + "'" + strReOrPay + "'";
                strHQL += ",RelatedAccountCode = " + "'" + strAccountCode + "'";
                strHQL += ",RelatedAccount = " + "'" + strAccountName + "'";
                strHQL += " Where FormCode = " + "'" + strFormCode + "'";

                ShareClass.RunSqlCommand(strHQL);
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void LoadBusinessFormReAndpay()
    {
        string strHQL1;
        IList lst1;

        string strReAndPay, strAccount;

        strHQL1 = "From BusinessFormReAndPay as businessFormReAndPay ";
        BusinessFormReAndPayBLL businessFormReAndPayBLL = new BusinessFormReAndPayBLL();
        lst1 = businessFormReAndPayBLL.GetAllBusinessFormReAndPays(strHQL1);

        DataGrid3.DataSource = lst1;
        DataGrid3.DataBind();

        for (int i = 0; i < lst1.Count; i++)
        {
            ShareClass.LoadAccountForDDL(((DropDownList)DataGrid3.Items[i].FindControl("DL_RelatedAccount")));

            try
            {
                ((DropDownList)DataGrid3.Items[i].FindControl("DL_RelatedAccount")).SelectedValue = ((BusinessFormReAndPay)lst1[i]).RelatedAccountCode.Trim();
            }
            catch
            {
            }
        }

        for (int i = 0; i < lst1.Count; i++)
        {
            strReAndPay = ((BusinessFormReAndPay)lst1[i]).ReceiveOrPay.Trim();
            ((DropDownList)DataGrid3.Items[i].FindControl("DL_ReceiveOrPay")).SelectedValue = strReAndPay;

            strAccount = ((BusinessFormReAndPay)lst1[i]).RelatedAccount;
            ((DropDownList)DataGrid3.Items[i].FindControl("DL_RelatedAccount")).SelectedValue = strAccount;
        }
    }
}
