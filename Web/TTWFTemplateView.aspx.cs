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

using System.Text;
using System.IO;
using System.Web.Mail;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTWFTemplateView : System.Web.UI.Page
{
    string strUserCode, strMakeUserCode, strIdentifyString, strTemName, strDesignType;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strWFXML;

        strUserCode = Session["UserCode"].ToString();
        strIdentifyString = Request.QueryString["IdentifyString"].Trim();
        strDesignType = Request.QueryString["DesignType"];
        if (strDesignType == "JS")
        {
            BT_SaveWFDefinition.Visible = false;
        }

        strTemName = GetWLTemplate(strIdentifyString);
        strMakeUserCode = GetWFTemplateCreatorCode(strIdentifyString);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", " aHandler();", true);

        Page.ClientScript.RegisterStartupScript(this.GetType(), "test", "javascript:document.forms[0]['TB_WFXML'].value=window.parent.document.getElementById('TB_WFXML').value;", true);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "test1", "javascript:document.forms[0]['TB_WFChartString'].value=window.parent.document.getElementById('TB_WFChartString').value;", true);

        if (Page.IsPostBack == false)
        {
            strWFXML = GetWFDefinitionXML(strIdentifyString);

            if (Session["SuperWFAdmin"].ToString() == "YES" | strUserCode == strMakeUserCode)
            {
                BT_SaveWFDefinition.Enabled = true;

                DataGrid3.Enabled = true;

                BT_Add.Enabled = true;
            }
            else
            {
                BT_SaveWFDefinition.Enabled = false;

                DataGrid3.Enabled = false;

                BT_Add.Enabled = false;
            }

            LoadWorkFlowTStep(strTemName);
            LB_WorkFlow.Text = strTemName;

            LoadWFTemplateXMLNodeGlobalVariable(strTemName);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clic2323412k", strDesignType, true);

            HL_XMLFile.NavigateUrl = ShareClass.GetWorkFlowLastestXMLFile(strTemName);
        }
    }


    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL, strGUID, strStepID, strWFXML;
        IList lst;

        if (e.CommandName != "Page")
        {
            strStepID = ((Button)e.Item.FindControl("BT_StepID")).Text.ToString();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.StepID = " + strStepID;
            WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
            lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);
            WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

            LB_StepID.Text = strStepID;
            LB_StepName.Text = workFlowTStep.StepName.Trim();
            LB_SortNumber.Text = workFlowTStep.SortNumber.ToString();

            LoadWorkFlowTStepOperator(strStepID);

            strWFXML = GetWFDefinitionXML(strIdentifyString);

            strGUID = workFlowTStep.GUID.Trim();


            if (e.CommandName == "Update")
            {
                LoadWorkFlowTStep(strTemName);
                LoadWorkFlowTStepOperator(strStepID);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                if (strWFXML.IndexOf(strGUID) > -1)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click11", "alert('" +LanguageHandle.GetWord("ZZJingGaoLiuChengTuZhongYouCiB") + LanguageHandle.GetWord("ZZSBJC") + "')", true); 
                    return;
                }
                else
                {
                    if (Session["SuperWFAdmin"].ToString() == "YES" | strUserCode == strMakeUserCode)
                    {
                        try
                        {
                            ShareClass.RunSqlCommand("Delete From T_WorkFlowTStep Where StepID = " + strStepID);
                            ShareClass.RunSqlCommand("Delete From T_WorkFlowTStepOperator Where StepID = " + strStepID);

                            LoadWorkFlowTStep(strTemName);
                            LoadWorkFlowTStepOperator(strStepID);

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click22", "alert('" +LanguageHandle.GetWord("ZZShanChuChengGong")+"')", true); 
                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click33", "alert('" +LanguageHandle.GetWord("ZZShanChuShiBaiQingJianCha")+"')", true); 
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click44", "alert('" +LanguageHandle.GetWord("ZZJingGaoCiLiuChengBuShiNiChua") + LanguageHandle.GetWord("ZZSBJC") + "')", true); 
                        return;
                    }
                }
            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_SqlWL.Text;

        ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
        IList lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void BT_SaveWFDefinition_Click(object sender, EventArgs e)
    {
        if (Session["SuperWFAdmin"].ToString() == "YES" | strUserCode == strMakeUserCode)
        {
            SaveWFTemplateDefinationXML(strIdentifyString, "0");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZBaoCunShiBaiNiBuShiCiLiuChen")+LanguageHandle.GetWord("ZZSBJC") + "')", true); 
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;

        string strID;
        string strXMLNode, strGlobalVariable;

        if (e.CommandName != "Page")
        {
            strID = ((Button)e.Item.FindControl("BT_ID")).Text.ToString();

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "Select XMLNode,GlobalVariable From T_WFTemplateXMLNodeGlobalVariable Where ID = " + strID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WFTemplateXMLNodeGlobalVariable");

            strXMLNode = ds.Tables[0].Rows[0][0].ToString();
            strGlobalVariable = ds.Tables[0].Rows[0][1].ToString();

            LB_XMLNodeID.Text = strID;
            TB_XMLNode.Text = strXMLNode;
            DL_GlobalVariable.SelectedValue = strGlobalVariable;

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
        }
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strXMLNode, strGlobalVariable;

        strXMLNode = TB_XMLNode.Text.Trim();
        strGlobalVariable = DL_GlobalVariable.SelectedValue.Trim();

        strHQL = "Insert Into T_WFTemplateXMLNodeGlobalVariable(TemName,XMLNode,GlobalVariable) ";
        strHQL += " Values('" + strTemName + "','" + strXMLNode + "','" + strGlobalVariable + "')";
        ShareClass.RunSqlCommand(strHQL);

        LB_XMLNodeID.Text = ShareClass.GetMyCreatedMaxWFTemplateXMLNodeGlobalVariableID();

        LoadWFTemplateXMLNodeGlobalVariable(strTemName);

        BT_Update.Enabled = true;
        BT_Delete.Enabled = true;
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID, strXMLNode, strGlobalVariable;

        strID = LB_XMLNodeID.Text.Trim();
        strXMLNode = TB_XMLNode.Text.Trim();
        strGlobalVariable = DL_GlobalVariable.SelectedValue.Trim();

        strHQL = "Update T_WFTemplateXMLNodeGlobalVariable Set XMLNode = '" + strXMLNode + "',GlobalVariable='" + strGlobalVariable + "' Where ID = " + strID;
        ShareClass.RunSqlCommand(strHQL);

        LoadWFTemplateXMLNodeGlobalVariable(strTemName);
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID, strXMLNode, strGlobalVariable;

        strID = LB_XMLNodeID.Text.Trim();
        strXMLNode = TB_XMLNode.Text.Trim();
        strGlobalVariable = DL_GlobalVariable.SelectedValue.Trim();

        strHQL = "Delete From T_WFTemplateXMLNodeGlobalVariable Where ID = " + strID;
        ShareClass.RunSqlCommand(strHQL);

        LoadWFTemplateXMLNodeGlobalVariable(strTemName);

        BT_Update.Enabled = false;
        BT_Delete.Enabled = false;
    }

    protected void LoadWFTemplateXMLNodeGlobalVariable(string strTemName)
    {
        string strHQL;

        strHQL = "Select * From  T_WFTemplateXMLNodeGlobalVariable Where TemName = '" + strTemName + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, " T_WFTemplateXMLNodeGlobalVariable");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }

    protected void LoadWorkFlowTStep(string strTemName)
    {
        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
        string strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.TemName =" + "'" + strTemName + "'" + " order by workFlowTStep.SortNumber ASC";
        IList lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadWorkFlowTStepOperator(string strStepID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTStepOperator as workFlowTStepOperator where workFlowTStepOperator.StepID = " + strStepID;
        WorkFlowTStepOperatorBLL workFlowTStepOperatorBLL = new WorkFlowTStepOperatorBLL();

        lst = workFlowTStepOperatorBLL.GetAllWorkFlowTStepOperators(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void SaveWFTemplateDefinationXML(string strIdentifyString, string strMark)
    {
        string strHQL;

        string strXML = TB_WFXML.Text.Trim();
        string strWFChartString = TB_WFChartString.Text.Trim();

        try
        {
            if (strXML != "")
            {
                strHQL = "update T_WorkFlowTemplate Set WFDefinition = " + "'" + strXML + "'" + " Where IdentifyString = " + "'" + strIdentifyString + "'";
                ShareClass.RunSqlCommand(strHQL);
            }

            SaveWFChart(strWFChartString);

            if (strMark == "0")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWBCSBJC") + "')", true);
        }
    }

    protected void SaveWFChart(string strWFChartString)
    {
        if (strWFChartString != "")
        {
            var binaryData = Convert.FromBase64String(strWFChartString);

            string strDateTime = DateTime.Now.ToString("yyyyMMddHHMMssff");
            string strChartURL = "Doc\\" + "WorkFlowTemplate\\" + strTemName + strDateTime + ".jpg";
            var imageFilePath = Server.MapPath("Doc") + "\\" + "WorkFlowTemplate\\" + strTemName + strDateTime + ".jpg";


            if (File.Exists(imageFilePath))
            { File.Delete(imageFilePath); }
            var stream = new System.IO.FileStream(imageFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            stream.Write(binaryData, 0, binaryData.Length);
            stream.Close();

            string strHQL = "Update T_WorkFlowTemplate Set ChartURL = " + "'" + strChartURL + "'" + " Where TemName = " + "'" + strTemName + "'";
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected string GetWFTemplateCreatorCode(string strIdentityString)
    {
        string strHQL;
        IList lst;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.IdentifyString = " + "'" + strIdentityString + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        return workFlowTemplate.CreatorCode.Trim();
    }

    protected string GetWLTemplate(string strIdentityString)
    {
        string strHQL;
        IList lst;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.IdentifyString = " + "'" + strIdentityString + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        return workFlowTemplate.TemName.Trim();
    }

    protected string GetWFDefinitionXML(string strIdentityString)
    {
        string strHQL;
        IList lst;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.IdentifyString = " + "'" + strIdentityString + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        try
        {
            return workFlowTemplate.WFDefinition.Trim();
        }
        catch
        {
            return "";
        }
    }

    protected string GetWorkFlowTemplateStepGUID(string strStepID)
    {
        string strHQL, strGUID;
        IList lst;

        strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.StepID = " + strStepID;
        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
        lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);
        WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

        try
        {
            strGUID = workFlowTStep.GUID.Trim();
        }
        catch
        {
            strGUID = "";
        }

        return strGUID;
    }

}
