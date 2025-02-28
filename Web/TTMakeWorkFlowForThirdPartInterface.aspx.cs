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

using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;
using System.Text;



using System.IO;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTMakeWorkFlowForThirdPartInterface : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strWLID = Request.QueryString["WLID"];

        string strUserCode, strUserName;

        strUserCode = Session["UserCode"].ToString();

        //this.Title = "建立和修改审批申请";

        if (Page.IsPostBack != true)
        {

            LB_CreatorCode.Text = strUserCode;
            strUserName = GetUserName(strUserCode);
            LB_CreatorName.Text = strUserName;

            LoadWLType();

            LoadWorkFlow(strUserCode);

            HL_WLRelatedDoc.NavigateUrl = "TTWLRelatedDoc.aspx?DocType=审核&WLID=" + strWLID;   
        }
    }


    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strWLName, strWLType, strCreatorCode, strCreatorName, strDescription;
        DateTime dtCreateTime;
        string strTemName, strXMLFile;


        strWLName = TB_WLName.Text.Trim();
        strWLType = LB_WLType.Text.Trim();
        strCreatorCode = LB_CreatorCode.Text.Trim();
        strCreatorName = LB_CreatorName.Text.Trim();
        strDescription = TB_WLDescription.Text.Trim();
        dtCreateTime = DateTime.Now;
        strTemName = DL_TemName.SelectedValue.Trim();

        strXMLFile = UPLoadXMLFile(strCreatorCode);

        if (strXMLFile != "")
        {
            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            WorkFlow workFlow = new WorkFlow();

            workFlow.WLName = strWLName;
            workFlow.WLType = strWLType;
            workFlow.RelatedType = "Other";
            workFlow.RelatedID = 0;
            workFlow.CreateTime = dtCreateTime;
            workFlow.CreatorCode = strCreatorCode;
            workFlow.CreatorName = strCreatorName;
            workFlow.Description = strDescription;
            workFlow.XMLFile = strXMLFile;
            workFlow.TemName = strTemName;
            workFlow.Status = "New";
            workFlow.DIYNextStep = "YES"; workFlow.IsPlanMainWorkflow = "NO";

            try
            {
                workFlowBLL.AddWorkFlow(workFlow);
                LoadWorkFlow(strCreatorCode);

                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
                HL_WLRelatedDoc.Enabled = false;

                LB_XMLFile.Text = strXMLFile;
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "');</script>");
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZXJSBCSHSDSJWJXMLWJBNWKZ").ToString().Trim() + "');</script>");

        }
    }
    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strWLID, strWLName, strWLType, strCreatorCode, strCreatorName, strDescription;
        DateTime dtCreateTime;
        string strTemName, strXMLFile;
        string strHQL;
        IList lst;

        strWLID = LB_WLID.Text.Trim();
        strWLName = TB_WLName.Text.Trim();
        strWLType = LB_WLType.Text.Trim();
        strCreatorCode = LB_CreatorCode.Text.Trim();
        strCreatorName = LB_CreatorName.Text.Trim();
        strDescription = TB_WLDescription.Text.Trim();
        dtCreateTime = DateTime.Now;
        strTemName = DL_TemName.SelectedValue.Trim();
        strXMLFile = FUP_File.PostedFile.FileName;

        strHQL = "from Approve as approve where approve.Type = 'Review' and approve.RelatedID = " + strWLID;   
        ApproveBLL approveBLL = new ApproveBLL();
        lst = approveBLL.GetAllApproves(strHQL);

        if (lst.Count == 0)
        {
            strHQL = "from WorkFlow as workFlow where workFlow.WLID = " + strWLID;
            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            lst = workFlowBLL.GetAllWorkFlows(strHQL);

            WorkFlow workFlow = (WorkFlow)lst[0];

            workFlow.WLType = strWLType;
            workFlow.WLName = strWLName;
            workFlow.Description = strDescription;
            workFlow.TemName = strTemName;

            if (strXMLFile != "")
            {
                workFlow.XMLFile = strXMLFile;
            }

            try
            {
                workFlowBLL.UpdateWorkFlow(workFlow, int.Parse(strWLID));

                DataSet ds = new DataSet();
                ds.ReadXml(workFlow.XMLFile.Trim());

                DataGrid3.DataSource = ds;
                DataGrid3.DataBind();

                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "');</script>");

            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "');</script>");

            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZYCZSPJLBNXGLJC").ToString().Trim() + "');</script>");
        }

        HL_WLRelatedDoc.NavigateUrl = "TTWLRelatedDoc.aspx?WLID=" + strWLID;

    }
    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL, strWLID;
        IList lst;

        strWLID = LB_WLID.Text.Trim();

        strHQL = "from Approve as approve where approve.Type = 'Review' and approve.RelatedID = " + strWLID;   
        ApproveBLL approveBLL = new ApproveBLL();
        lst = approveBLL.GetAllApproves(strHQL);

        if (lst.Count == 0)
        {
            strHQL = "from WorkFlow as workFlow where workFlow.WLID = " + strWLID;
            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            lst = workFlowBLL.GetAllWorkFlows(strHQL);

            WorkFlow workFlow = (WorkFlow)lst[0];

            try
            {
                workFlowBLL.DeleteWorkFlow(workFlow);
                LoadWorkFlow(workFlow.CreatorCode);
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "');</script>");
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZYCZSPJLBNSCLJC").ToString().Trim() + "');</script>");
        }

    }

    protected void BT_ReviewData_Click(object sender, EventArgs e)
    {
        string strFileName1 = FUP_File.PostedFile.FileName.Trim();

        if (strFileName1 != "")
        {
            DataSet ds = new DataSet();
            ds.ReadXml(strFileName1);

            DataGrid3.DataSource = ds;
            DataGrid3.DataBind();

            LB_CheckData.Visible = true;

            LB_XMLFile.Text = strFileName1;
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strWLType = ((Button)e.Item.FindControl("BT_WLType")).Text.Trim();

        LB_WLType.Text = strWLType;

        LoadWLTemplate(strWLType);
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strWLID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strWLID = ((Button)e.Item.FindControl("BT_WLID")).Text;

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from WorkFlow as workFlow where workFlow.WLID = " + strWLID;
            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            lst = workFlowBLL.GetAllWorkFlows(strHQL);

            WorkFlow workFlow = (WorkFlow)lst[0];

            LoadWLTemplate(workFlow.WLType.Trim());

            LB_WLID.Text = workFlow.WLID.ToString();
            LB_WLType.Text = workFlow.WLType.Trim();
            TB_WLName.Text = workFlow.WLName.Trim();
            TB_WLDescription.Text = workFlow.Description.Trim();
            DL_TemName.SelectedValue = workFlow.TemName.Trim();
            LB_CreateTime.Text = workFlow.CreateTime.ToString();
            LB_Status.Text = workFlow.Status;
            LB_XMLFile.Text = workFlow.XMLFile.Trim();

            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(workFlow.XMLFile);
                DataGrid3.DataSource = ds;
                DataGrid3.DataBind();

                LB_CheckData.Visible = true;
            }
            catch
            {
            }

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
            HL_WLRelatedDoc.Enabled = true;

            HL_WLRelatedDoc.NavigateUrl = "TTWLRelatedDoc.aspx?WLID=" + strWLID;
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql1.Text;

        WLTypeBLL wlTypeBLL = new WLTypeBLL();
        IList lst = wlTypeBLL.GetAllWLTypes(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql2.Text;

        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        IList lst = workFlowBLL.GetAllWorkFlows(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }

    protected Approve GetApprove(string strID)
    {
        string strHQL = "from Approve as approve where approve.ID = " + strID;
        ApproveBLL approveBLL = new ApproveBLL();
        IList lst = approveBLL.GetAllApproves(strHQL);
        Approve approve = (Approve)lst[0];
        return approve;
    }

    protected string GetDepartCode(string strUserCode)
    {
        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];

        return projectMember.DepartCode;
    }

    protected string GetDepartName(string strDepartCode)
    {
        string strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        IList lst = departmentBLL.GetAllDepartments(strHQL);

        Department department = (Department)lst[0];

        return department.DepartName;
    }


    protected WorkFlow GetWorkFlow(string strWLID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLID = " + strWLID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        WorkFlow workFlow = (WorkFlow)lst[0];

        return workFlow;
    }

    protected void UpdateWorkFlowStatus(string strWLID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLID = " + strWLID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        WorkFlow workFlow = (WorkFlow)lst[0];

        workFlow.Status = strStatus;

        try
        {
            workFlowBLL.UpdateWorkFlow(workFlow, int.Parse(strWLID));
        }
        catch
        {
        }
    }

    protected void LoadWorkFlow(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.CreatorCode = " + "'" + strUserCode + "'" + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql2.Text = strHQL;
    }

    protected void LoadWLType()
    {
        string strHQL;
        IList lst;

        string strLangCode = Session["LangCode"].ToString();

        strHQL = " from WLType as wlType";
        strHQL += " Where wlType.LangCode = " + "'" + strLangCode + "'";
        strHQL += " Order by wlType.SortNumber ASC";
        WLTypeBLL wlTypeBLL = new WLTypeBLL();
        lst = wlTypeBLL.GetAllWLTypes(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql1.Text = strHQL;
    }

    protected void LoadWLTemplate(string strWLType)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = " + "'" + strWLType + "'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();
    }

    protected string UPLoadXMLFile(string strUserCode)
    {
        if (this.FUP_File.PostedFile != null)
        {
            string strFileName1 = FUP_File.PostedFile.FileName.Trim();
        
            string strWholeFileName;

            int i;


            if (strFileName1 != "")
            {
                Msg msg = new Msg();

                //获取初始文件名
                i = strFileName1.LastIndexOf("."); //取得文件名中最后一个"."的索引
                string strNewExt = strFileName1.Substring(i); //获取文件扩展名

                DateTime dtUploadNow = DateTime.Now; //获取系统时间

                string strFileName2 = System.IO.Path.GetFileName(strFileName1);
                string strExtName = Path.GetExtension(strFileName2);
                string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtName;

                string strDocSavePath = Server.MapPath("Doc");

                strDocSavePath = strDocSavePath + "\\XML\\";

                strWholeFileName = strDocSavePath + strFileName3;

                FileInfo fi = new FileInfo(strWholeFileName);

                try
                {
                    FUP_File.PostedFile.SaveAs(strWholeFileName);

                    DataSet ds = new DataSet();
                    ds.ReadXml(strWholeFileName);

                    DataGrid3.DataSource = ds;
                    DataGrid3.DataBind();

                    return strWholeFileName;
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "');</script>");
                    return "";
                }
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


}
