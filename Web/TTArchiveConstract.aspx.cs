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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTArchiveConstract : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //Taketop Software 2006－2012

        string strUserCode = Session["UserCode"].ToString();
        string strHQL;
        IList lst;

        string strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "合同归档", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        LB_UserCode.Text = strUserCode;
        strUserName = Session["UserName"].ToString();
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);

            strHQL = "from ConstractBigType as constractBigType Order by constractBigType.SortNumber ASC";
            ConstractBigTypeBLL constractBigTypeBLL = new ConstractBigTypeBLL();
            lst = constractBigTypeBLL.GetAllConstractBigTypes(strHQL);
            RBL_ConstractClass.DataSource = lst;
            RBL_ConstractClass.DataBind();

            strHQL = "from ConstractType as constractType Order by constractType.SortNumber ASC";
            ConstractTypeBLL constractTypeBLL = new ConstractTypeBLL();
            lst = constractTypeBLL.GetAllConstractTypes(strHQL);
            DL_ConstractType.DataSource = lst;
            DL_ConstractType.DataBind();
            DL_ConstractType.Items.Insert(0, new ListItem("--Select--", ""));

            string strDepartCode = GetDepartCode(strUserCode);
            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);

            LB_ConstractOwner.Text = LanguageHandle.GetWord("WoDeSuoYouGeTongLieBiao").ToString().Trim();

            strHQL = "from Constract as constract where constract.Status not in ('Cancel','Deleted') ";
            strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";
            ConstractBLL constractBLL = new ConstractBLL();
            lst = constractBLL.GetAllConstracts(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName, strHQL;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            LB_ConstractOwner.Text = strDepartName + LanguageHandle.GetWord("QianDingDeGeTongLieBiao").ToString().Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);

            LB_ConstractOwner.Text = strDepartName + LanguageHandle.GetWord("QianDingDeGeTongLieBiao").ToString().Trim();
            strHQL = "from Constract as constract where constract.ConstractCode in (Select constractSales.ConstractCode From ConstractSales as constractSales Where constractSales.SalesName in (select projectMember.UserName from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + "))  and constract.Status not in ('Archived','Deleted')";
            strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";
            ConstractBLL constractBLL = new ConstractBLL();
            lst = constractBLL.GetAllConstracts(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;
        }
    }

    protected void RBL_ConstractClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strConstractClass, strUserCode;

        strUserCode = LB_UserCode.Text.Trim();
        strConstractClass = RBL_ConstractClass.SelectedItem.Text.Trim();

        strHQL = "from Constract as constract where constract.ConstractClass = " + "'" + strConstractClass + "'" + " and constract.Status not in ('Archived','Deleted') ";
        strHQL += " and constract.ConstractCode in (select constractRelatedUser.ConstractCode from ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";

        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        LB_ConstractOwner.Text = LanguageHandle.GetWord("DaLei").ToString().Trim() + strConstractClass + LanguageHandle.GetWord("DeGeTong").ToString().Trim();
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strConstractID, strHQL, strStatus;
        IList lst;

        if (e.CommandName != "Page")
        {
            strConstractID = e.Item.Cells[0].Text.Trim();
            strHQL = "from Constract as constract where constract.ConstractID = " + strConstractID;
            ConstractBLL constractBLL = new ConstractBLL();
            lst = constractBLL.GetAllConstracts(strHQL);
            Constract constract = (Constract)lst[0];

            strStatus = constract.Status.Trim();

            if (e.CommandName == "Archive")
            {
                if (strStatus == "Archived")
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCHTYGDBYZJXGDDZLJC").ToString().Trim() + "')", true);
                    return;
                }

                if (strStatus != "Completed" & strStatus != LanguageHandle.GetWord("YiWanCheng").ToString().Trim())
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCHTHMYJABNGDJC").ToString().Trim() + "')", true);
                    return;
                }

                constract.ArchiveTime = DateTime.Now.ToString("yyyy-MM-dd HH:MM");
                constract.Status = "Archived";

                try
                {
                    constractBLL.UpdateConstract(constract, int.Parse(strConstractID));

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGDCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGDSBJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                if (strStatus != "Archived")
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCHTZTBWGDBNCXJC").ToString().Trim() + "')", true);

                    return;
                }

                constract.ArchiveTime = "";
                constract.Status = "Completed";

                try
                {
                    constractBLL.UpdateConstract(constract, int.Parse(strConstractID));

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZCheXiaoLanguageHandleGetWord").ToString().Trim()+"')", true); 
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZCheXiaoLanguageHandleGetWord").ToString().Trim()+"')", true); 
                }
            }

            LoadConstractList(LB_Sql.Text.Trim());
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strDepartCode = ((Button)e.Item.FindControl("BT_DepartCode")).Text.Trim();
            string strDepartName = ((Button)e.Item.FindControl("BT_DepartName")).Text.Trim();

            LB_ConstractOwner.Text = strDepartName + LanguageHandle.GetWord("QianDingDeGeTongLieBiao").ToString().Trim();
            strHQL = "from Constract as constract where constract.ConstractCode in ";
            strHQL += " (select constractSales.ConstractCode from ConstractSales as constractSales Where constractSales.SalesName ";
            strHQL += " in ( Select projectMember.UserName  Where from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + "))";
            strHQL += " and constract.Status not in ('Archived','Deleted')";
            strHQL += " Order by constract.SignDate  DESC,constract.ConstractCode DESC";
            ConstractBLL constractBLL = new ConstractBLL();
            lst = constractBLL.GetAllConstracts(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        LB_ConstractOwner.Text = strUserName + LanguageHandle.GetWord("QianDingDeGeTongLieBiao").ToString().Trim();

        strHQL = "from Constract as constract where constract.ConstractCode in (Select constractSales.ConstractCode from ConstractSales as constractSales Where constractSales.SalesName = " + "'" + strUserName + "'" + ")  and constract.Status not in ('Archived','Deleted')";
        strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";
        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strType = "%" + DL_ConstractType.SelectedValue + "%";
        string strSalesName = "%" + TB_SalesName.Text.Trim() + "%";
        string strConstractCode = "%" + TB_ConstractCode.Text.Trim() + "%";
        string strConstractName = "%" + TB_ConstractName.Text.Trim() + "%";
        string strPartA = "%" + TB_PartA.Text.Trim() + "%";


        LB_ConstractOwner.Text = LanguageHandle.GetWord("LeiXing").ToString().Trim() + strType + LanguageHandle.GetWord("DeGeTong").ToString().Trim();

        if (strPartA != "%%")
        {
            strHQL = "from Constract as constract where ";
            strHQL += " constract.Type Like " + "'" + strType + "'";
            strHQL += " and constract.ConstractName like " + "'" + strConstractName + "'";
            strHQL += " and constract.PartA like " + "'" + strPartA + "'";
            strHQL += " and constract.ConstractCode in (Select constractSales.ConstractCode from ConstractSales as constractSales Where constractSales.SalesName like " + "'" + strSalesName + "'" + ")";

            strHQL += " and constract.Status not in ('Cancel','Deleted') ";
            strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";
        }
        else
        {
            strHQL = "from Constract as constract where ";
            strHQL += " constract.Type Like " + "'" + strType + "'";
            strHQL += " and constract.ConstractCode like " + "'" + strConstractCode + "'";
            strHQL += " and constract.ConstractName like " + "'" + strConstractName + "'";
            strHQL += " and constract.PartA like " + "'" + strPartA + "'";
            strHQL += " and constract.Status not in ('Cancel','Deleted') ";
            strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";
        }
        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        ConstractBLL constractBLL = new ConstractBLL();
        IList lst = constractBLL.GetAllConstracts(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadConstractList(string strHQL)
    {
        IList lst;

        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected string GetDepartCode(string strUserCode)
    {
        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];

        string strDepartCode = projectMember.DepartCode;

        return strDepartCode;
    }
}
