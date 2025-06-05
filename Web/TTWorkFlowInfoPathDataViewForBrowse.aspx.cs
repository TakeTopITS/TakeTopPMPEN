using TakeTopInfoPathSoft.Service;
using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using TakeTopCore;
using TakeTopWF;

public partial class TTWorkFlowInfoPathDataViewForBrowse : System.Web.UI.Page
{
    string PublishUrl = null;
    string uri = null;

    string strUserCode, strWLID, strWFName, strStepID, strID, strWLTemName, strWFXMLFile, strNewWFXMLFile;
    int intMainTableID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strWFStatus;
        string strUnVisibleFieldXMLFile;
        string strXSNFile;

        strUserCode = Session["UserCode"].ToString();

        strWLID = Request.QueryString["WLID"].Trim();
        strStepID = Request.QueryString["StepID"].Trim();
        strID = Request.QueryString["ID"].Trim();

        if (Request.QueryString["XSNFile"] != null)
        {
            strXSNFile = Request.QueryString["XSNFile"].Trim();
        }
        else
        {
            strXSNFile = Request.QueryString["XSNFile"];
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGXSNFILEWKJC") + "');</script>");
            return;
        }
        LB_XSNFile.Text = strXSNFile;

        try
        {
            strWFXMLFile = GetWorkflowStepDetailXMLFile(strID);
            if (strWFXMLFile == null | strWFXMLFile == "")
            {
                strWFXMLFile = ShareClass.GetWorkflowXMLFile(strWLID);
            }

            DataSet ds = GetWorkFlow(strWLID);
            strWLTemName = ds.Tables[0].Rows[0]["TemName"].ToString().Trim();
            strWFName = ds.Tables[0].Rows[0]["WLName"].ToString().Trim();

            //������ݿ���д��ڴ˹����������ݣ���ô�ѱ������ݸ��ӵ�����
            intMainTableID = GetWorkflowMainTableID(strWLID);
            strNewWFXMLFile = TakeTopXML.TableConvertToFormByMainID(strWLID, intMainTableID, strWLTemName, Server.MapPath(strWFXMLFile));
            if (strNewWFXMLFile != "")
            {
                strWFXMLFile = strNewWFXMLFile;
            }

            if (Page.IsPostBack == false)
            {
                strWFStatus = ds.Tables[0].Rows[0]["Status"].ToString().Trim();

                //ȡ�������̱������ļ��������滻��������
                WFDataHandle wfDataHandle = new WFDataHandle();
                strUnVisibleFieldXMLFile = wfDataHandle.GetXMLFileAfterReplaceWFXmlUNVisibleFieldNode(strWLID, strID, strWFXMLFile);

                LB_UnVisibleFieldXMLFile.Text = strUnVisibleFieldXMLFile;

                //ע�����̱�ģ�岢װ�����̱�����
                wfDataHandle.RegisterWFTemplateAndLoadWFFormData(strXSNFile, strUnVisibleFieldXMLFile, this.uri, PublishUrl, xdoc, this.Context);

                PublishUrl = wfDataHandle.wfPublishUrl.ToString();
                uri = wfDataHandle.wfUri.ToString();

                LB_PublishUrl.Text = PublishUrl;
                LB_Uri.Text = uri;


                try
                {
                    string str, strCopyRight;

                    strCopyRight = TakeTopInfoPathService.CopyRight;
                    str = "<span id='LB_CopyRight' style='display:none;'>" + strCopyRight + "</span>";
                    this.Controls.Add(new System.Web.UI.LiteralControl(str));
                }
                catch (Exception err)
                {
                    //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                }
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGCGZLMBBCZQJC") + "');</script>");
        }
    }

    protected DataSet GetWorkFlow(string strWLID)
    {
        string strHQL;
        DataSet ds;

        strHQL = "Select * from T_WorkFlow where WLID = " + strWLID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * from T_WorkFlowBackup where WLID = " + strWLID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowBackup");
        }

        return ds;
    }

    //ȡ�ù�����������MainTableID
    protected int GetWorkflowMainTableID(string strWFID)
    {
        string strHQL;
        DataSet ds;

        strHQL = "Select MainTableID From T_WorkFlow Where WLID = " + strWFID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_Workflow");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select MainTableID From T_WorkFlowBackup Where WLID = " + strWFID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkflowBackup");
        }

        try
        {
            return int.Parse(ds.Tables[0].Rows[0][0].ToString().Trim());
        }
        catch
        {
            return 0;
        }
    }

    //ȡ�ô˲��������ļ�
    protected string GetWorkflowStepDetailXMLFile(string strID)
    {
        string strHQL;

        strHQL = "Select XMLFile From T_WorkFlowStepDetail Where ID =" + strID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select XMLFile From T_WorkFlowStepDetailBackup Where ID =" + strID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetailBackup");
        }

        return ds.Tables[0].Rows[0][0].ToString();
    }
}
