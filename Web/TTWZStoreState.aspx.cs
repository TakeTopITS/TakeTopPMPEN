using System; using System.Resources;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZStoreState : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataYearMonthBinder();

            BindStateData();
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strCYear = DDL_CYear.SelectedValue;
            string strCMonth = DDL_CMonth.SelectedValue;
            string strCPath = TXT_CPath.Text.Trim();
            string strPass = TXT_Pass.Text.Trim();
            string strID = TXT_ID.Text;

            if (!string.IsNullOrEmpty(strID))
            {
                //修改
                string strWZStateSql = string.Format(@"update T_WZState 
                                set CYear = '{0}',
                                CMonth = '{1}',
                                CPath = '{2}',
                                Pass = '{3}'
                                where ID = {4}", strCYear, strCMonth, strCPath, strPass, strID);

                ShareClass.RunSqlCommand(strWZStateSql);
            }
            else
            {
                //增加
                string strWZStateSql = string.Format(@"insert into T_WZState(CYear,CMonth,CPath,Pass)
                                values('{0}','{1}','{2}','{3}')", strCYear, strCMonth, strCPath, strPass);

                ShareClass.RunSqlCommand(strWZStateSql);

                string strWZStateHQL = @"select * from T_WZState order by ID desc limit 1";
                DataTable dtState = ShareClass.GetDataSetFromSql(strWZStateHQL, "State").Tables[0];
                if (dtState != null && dtState.Rows.Count > 0)
                {
                    DataRow drState = dtState.Rows[0];

                    TXT_ID.Text = ShareClass.ObjectToString(drState["ID"]);
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
        }
        catch (Exception ex)
        { }
    }


    private void BindStateData()
    {
        string strWZStateHQL = @"select * from T_WZState order by ID desc limit 1";
        DataTable dtState = ShareClass.GetDataSetFromSql(strWZStateHQL, "State").Tables[0];
        if (dtState != null && dtState.Rows.Count > 0)
        {
            DataRow drState = dtState.Rows[0];


            DDL_CYear.SelectedValue = ShareClass.ObjectToString(drState["CYear"]);
            DDL_CMonth.SelectedValue = ShareClass.ObjectToString(drState["CMonth"]);
            TXT_CPath.Text = ShareClass.ObjectToString(drState["CPath"]);
            TXT_Pass.Text = ShareClass.ObjectToString(drState["Pass"]);
            TXT_ID.Text = ShareClass.ObjectToString(drState["ID"]);

        }
    }


    private void DataYearMonthBinder()
    {
        DDL_CYear.Items.Add(new ListItem(DateTime.Now.AddYears(-2).Year.ToString(), DateTime.Now.AddYears(-2).Year.ToString()));
        DDL_CYear.Items.Add(new ListItem(DateTime.Now.AddYears(-1).Year.ToString(), DateTime.Now.AddYears(-1).Year.ToString()));
        DDL_CYear.Items.Add(new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString()));
        DDL_CYear.Items.Add(new ListItem(DateTime.Now.AddYears(1).Year.ToString(), DateTime.Now.AddYears(1).Year.ToString()));
        DDL_CYear.Items.Add(new ListItem(DateTime.Now.AddYears(2).Year.ToString(), DateTime.Now.AddYears(2).Year.ToString()));
        DDL_CYear.SelectedValue = DateTime.Now.Year.ToString();

        for (int i = 1; i <= 12; i++)
        {
            if (i < 10)
            {
                DDL_CMonth.Items.Add(new ListItem("0" + i, "0" + i));
            }
            else
            {
                DDL_CMonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }
        string strCurrentMonth = DateTime.Now.Month.ToString();
        if (strCurrentMonth.Length > 1)
        {
            DDL_CMonth.SelectedValue = strCurrentMonth;
        }
        else
        {
            DDL_CMonth.SelectedValue = "0" + strCurrentMonth;
        }
    }

}