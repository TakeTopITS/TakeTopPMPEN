using System; using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
public partial class TTMyMemberLevelSet : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //泰顶软件2006－2012
        
        strUserCode = Session["UserCode"].ToString();
        LB_UserCode.Text = strUserCode;
      
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); 
        if (Page.IsPostBack == false)
        {
            LoadMemberList(strUserCode);
        }
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID;

        int j = 0, intSortNumber;
      
        try
        {
            for (j = 0; j < DataGrid3.Items.Count; j++)
            {
                strID = DataGrid3.Items[j].Cells[0].Text;
                intSortNumber = int.Parse(((TextBox)DataGrid3.Items[j].FindControl("TB_SortNumber")).Text);

                strHQL = "Update T_MemberLevel Set SortNumber = " + intSortNumber.ToString() + " Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);
            }

            LoadMemberList(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG")+"')", true);
           
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGBCSBJC")+"')", true);
           
        }
    }

    protected void LoadMemberList(string strUserCode)
    {
        string strHQL;

        strHQL = "Select A.UnderCode,B.UserName,* From T_MemberLevel A,T_ProjectMember B Where A.UnderCode = B.UserCode and A.UserCode = " + "'" + strUserCode + "'";
        strHQL += " Order By A.SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MemberLevel");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }

   
}
