using System; using System.Resources;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;

public partial class TTQueryTime : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","数据查询用时测试", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
        }
    }

    protected void BT_Check_Click(object sender, EventArgs e)
    {
        //string strHQLBefore = "declare begin_date timestamp without time zone; declare end_date timestamp without time zone; select begin_date=now(); ";
        //string strHQLQuery = TB_SQLCode.Text.Trim();
        //string strHQLBack = " Select end_date=now(); Select extract(epoch FROM (end_date-begin_date))*1000 as usetimes;";
        //string strHQL = strHQLBefore + strHQLQuery + strHQLBack;

        DateTime dtHQLBefore = DateTime.Now;
        string strHQLQuery = TB_SQLCode.Text.Trim();

        if (strHQLQuery == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSQLYJBNWKJC").ToString().Trim() + "')", true);
            return;
        }

        if (TB_SQLCode.Text.Trim().ToLower().Contains("create ") || TB_SQLCode.Text.Trim().ToLower().Contains("execute ") || TB_SQLCode.Text.Trim().ToLower().Contains("delete ") || TB_SQLCode.Text.Trim().ToLower().Contains("update") || TB_SQLCode.Text.Trim().ToLower().Contains("drop ")
         || TB_SQLCode.Text.Trim().ToLower().Contains("insert ") || TB_SQLCode.Text.Trim().ToLower().Contains("alter "))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZNSSELECTCYJBNSDELETEUPDATEINSERTCREATEDROPYJJC").ToString().Trim() + "')", true);
            TB_SQLCode.Focus();
            return;
        }

        try
        {
            DataSet ds = ShareClass.GetDataSetFromSql(strHQLQuery, "TestTable");

            DateTime dtHQLBack = DateTime.Now;

            TimeSpan ts = dtHQLBack - dtHQLBefore;

            lbl_Time.Text = ts.TotalSeconds.ToString() + LanguageHandle.GetWord("Miao").ToString().Trim();

            LB_Sql.Text = strHQLQuery;

            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGYFCWJC").ToString().Trim() + "')", true);
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "TestTable");

        DataGrid1.DataSource = ds ;
        DataGrid1.DataBind();
    }

}