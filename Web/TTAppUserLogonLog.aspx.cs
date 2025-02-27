using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class TTAppUserLogonLog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;
        string strUserName;
        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        if (Page.IsPostBack != true)
        {
            string strHQL1, strHQL2;
            DateTime dtSelectedDate = DateTime.Now;

            strHQL1 = "Select * From T_LogonLog where to_char(LoginTime,'yyyymmdd') = " + "'" + dtSelectedDate.ToString("yyyyMMdd") + "'" + " Order by ID DESC";
            DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_logonLog");
            DataGrid1.DataSource = ds1;
            DataGrid1.DataBind();

            strHQL2 = "Select Distinct(UserCode) From T_LogonLog where to_char(LoginTime,'yyyymmdd') = " + "'" + dtSelectedDate.ToString("yyyyMMdd") + "'" + " Group By UserCode";
            DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL2, "T_LogonLog");

            LB_QueryScope.Text = dtSelectedDate.ToShortDateString() + "," + LanguageHandle.GetWord("CiShu").ToString().Trim() + ds1.Tables[0].Rows.Count.ToString() + "," + LanguageHandle.GetWord("RenShu").ToString().Trim() + ds2.Tables[0].Rows.Count.ToString();
        }
    }
}
