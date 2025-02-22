using System;
using System.Collections;
using System.Collections.Generic;

using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TakeTopMainSkinSelect : System.Web.UI.Page
{
    private string strUserType;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;

        string strUserCode = Session["UserCode"].ToString();

        strUserType = ShareClass.GetUserType(strUserCode);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            strHQL = "Select * From T_SystemLanguage Order By SortNumber ASC";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Language");

            RP_Language.DataSource = ds;
            RP_Language.DataBind();

            //HttpBrowserCapabilities GetBrowserCapabilities = HttpContext.Current.Request.Browser;
            //if (ShareClass.GetBrowser(GetBrowserCapabilities) == "SF")
            //{
            //    BT_Blue.Visible = false;
            //    TD_Blue.Visible = false;
                
            //}
        }
    }

    protected void BT_Blue_Click(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];

        projectMember.CssDirectory = BT_Blue.ToolTip;

        try
        {
            projectMemberBLL.UpdateProjectMember(projectMember, strUserCode);

            Session["CssDirectory"] = BT_Blue.ToolTip;

            //HttpBrowserCapabilities GetBrowserCapabilities = HttpContext.Current.Request.Browser;
            //if (ShareClass.GetBrowser(GetBrowserCapabilities) == "SF")
            //{
            //    Session["CssDirectory"] = "CssGreen";
            //    Session["CssDirectoryChangeNumber"] = "1";
            //}

            //���û�����ı�־
            ChangePageCache("Skin");

            //���´���Ӧ����ҳ����ˢ��ҳ��
            OpenTopMDIPage(strUserType, projectMember.CssDirectory.Trim() + projectMember.LangCode.Trim());
        }
        catch
        {
        }
    }

    protected void BT_Black_Click(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];

        projectMember.CssDirectory = BT_Black.ToolTip;

        try
        {
            projectMemberBLL.UpdateProjectMember(projectMember, strUserCode);

            Session["CssDirectory"] = BT_Black.ToolTip;

            //���û�����ı�־
            ChangePageCache("Skin");

            //���´���Ӧ����ҳ����ˢ��ҳ��
            OpenTopMDIPage(strUserType, projectMember.CssDirectory.Trim() + projectMember.LangCode.Trim());
        }
        catch
        {
        }
    }

    protected void BT_Green_Click(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];

        projectMember.CssDirectory = BT_Green.ToolTip;

        try
        {
            projectMemberBLL.UpdateProjectMember(projectMember, strUserCode);

            Session["CssDirectory"] = BT_Green.ToolTip;

            //���û�����ı�־
            ChangePageCache("Skin");

            //���´���Ӧ����ҳ����ˢ��ҳ��
            OpenTopMDIPage(strUserType, projectMember.CssDirectory.Trim() + projectMember.LangCode.Trim());
        }
        catch
        {
        }
    }

    protected void BT_Grey_Click(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];

        projectMember.CssDirectory = BT_Grey.ToolTip;

        try
        {
            projectMemberBLL.UpdateProjectMember(projectMember, strUserCode);

            Session["CssDirectory"] = BT_Grey.ToolTip;

            //���û�����ı�־
            ChangePageCache("Skin");

            //���´���Ӧ����ҳ����ˢ��ҳ��
            OpenTopMDIPage(strUserType, projectMember.CssDirectory.Trim() + projectMember.LangCode.Trim());
        }
        catch
        {
        }
    }

    protected void BT_Red_Click(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];

        projectMember.CssDirectory = BT_Red.ToolTip;

        try
        {
            projectMemberBLL.UpdateProjectMember(projectMember, strUserCode);

            Session["CssDirectory"] = BT_Red.ToolTip;

            //���û�����ı�־
            ChangePageCache("Skin");

            //���´���Ӧ����ҳ����ˢ��ҳ��
            OpenTopMDIPage(strUserType, projectMember.CssDirectory.Trim() + projectMember.LangCode.Trim());
        }
        catch
        {
        }
    }

    protected void BT_Gold_Click(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];

        projectMember.CssDirectory = BT_Gold.ToolTip;

        try
        {
            projectMemberBLL.UpdateProjectMember(projectMember, strUserCode);

            Session["CssDirectory"] = BT_Gold.ToolTip;

            //���û�����ı�־
            ChangePageCache("Skin");

            //���´���Ӧ����ҳ����ˢ��ҳ��
            OpenTopMDIPage(strUserType, projectMember.CssDirectory.Trim() + projectMember.LangCode.Trim());
        }
        catch
        {
        }
    }

    protected void RP_Language_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            string strLangCode;

            string strUserCode = Session["UserCode"].ToString();

            strLangCode = ((Button)e.Item.FindControl("BT_Language")).ToolTip.Trim();

            ((Button)e.Item.FindControl("BT_Language")).ForeColor = System.Drawing.Color.Black;

            strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

            ProjectMember projectMember = (ProjectMember)lst[0];

            projectMember.LangCode = strLangCode;
            try
            {
                projectMemberBLL.UpdateProjectMember(projectMember, strUserCode);

                Session["CssDirectory"] = projectMember.CssDirectory.Trim();
                Session["LangCode"] = strLangCode;
                Response.SetCookie(new HttpCookie("LangCode", strLangCode));

                //���û�����ı�־
                ChangePageCache("Language");

                //���´���Ӧ����ҳ����ˢ��ҳ��
                OpenTopMDIPage(strUserType, projectMember.CssDirectory.Trim() + projectMember.LangCode.Trim());
            }
            catch
            {
            }
        }
    }

    //���û�����ı�־����ˢ��ҳ�滺��
    protected void ChangePageCache(string strClickType)
    {
        ////���û�����ı�־
        //ShareClass.SetPageCacheMark("1");
        //Session["CssDirectoryChangeNumber"] = "1";

        //��ʱ�÷�����Ϊ�����1�Ż�ˢ��ҳ�漶��
        ShareClass.SetPageCacheMark("2");
        Session["CssDirectoryChangeNumber"] = "2";

        ChangePageCache();
    }

    //���û�����ı�־����ˢ��ҳ�滺��
    protected void ChangePageCache()
    {
        //����ҳ�滺�棬ˢ��ҳ��
        ShareClass.AddSpaceLineToFile("TTPersonalSpaceModuleFlowView.aspx", "<%--***--%>");
        ShareClass.AddSpaceLineToFile("TTModuleFlowChartViewJS.aspx", "<%--***--%>");
        ShareClass.AddSpaceLineToFile("WFDesigner/TTTakeTopMFChartViewJS.aspx", "<%--***--%>");
    }

    //����Ӧ����ҳ
    protected void OpenTopMDIPage(string strUserType, string strSkinFlag)
    {
        //�������������ӵ�URL��������ˢ�»���
        Session["SkinFlag"] = strSkinFlag;

        if (Session["SystemVersionType"].ToString() == "SAAS")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "redirectToSAASTopPage();", true);
        }
        else
        {
            if (strUserType == "INNER")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "redirectToInnerTopPage();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "redirectToOuterTopPage();", true);
            }
        }
    }


}