using System;
using System.Resources;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using TakeTopInfoPathSoft.Service;

using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;
using System.Text;
using System.IO;
using System.Collections.Generic;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopWF;
using TakeTopCore;

public partial class TTDIYModuleByWFFormReadOnly : System.Web.UI.Page
{
    private static string PublishUrl = null;
    protected string uri = null;

    private static string OldPublishUrl = null;
    protected string OldUri = null;

    public string strUserCode, strModuleName;
    public string strWFType, strTemName, strWLID, strTemIdentifyString;
    //���Ϲ���RelatedCode TODO:CAOJIAN(�ܽ�)
    string strRelatedType, strRelatedID, strRelatedCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;


        strUserCode = Session["UserCode"].ToString();
        strModuleName = Request.QueryString["ModuleName"];

        strTemIdentifyString = Request.QueryString["TemIdentifyString"].Trim();

        //���Ϲ���RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(�ܽ�)
        strRelatedType = Request.QueryString["RelatedType"];
        if (strRelatedType == null)
        {
            strRelatedType = "OTHER";
        }

        strRelatedID = Request.QueryString["RelatedID"];
        if (strRelatedID == null)
        {
            strRelatedID = "0";
        }

        strRelatedCode = Request.QueryString["RelatedCode"];
        if (strRelatedCode == null)
        {
            strRelatedCode = "";
        }

        strHQL = "select TemName,Type from T_WorkFlowTemplate Where IdentifyString = " + "'" + strTemIdentifyString + "'";
        DataSet ds = new DataSet();
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        strTemName = ds.Tables[0].Rows[0][0].ToString().Trim();
        strWFType = ds.Tables[0].Rows[0][1].ToString().Trim();

        Session["AutoActiveWorkflow"] = ShareClass.GetWorkflowTemplateAutoActive(strTemName);

        //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>aHandler();</script>");
        if (Page.IsPostBack == false)
        {
            LB_WFType.Text = strWFType;
            LB_ModuleName.Text = strModuleName;

            UploadWFDIYFormTemplate(strTemName);

            //�����û��Զ����JSCode��ҳ��
            WFShareClass.AttachUserJSCodeFromWFTemplate(strTemName, LIT_AttachUserJSCode, strUserCode, "", "0", "");
            ClientScript.RegisterStartupScript(this.GetType(), "HH77H", "<script>jqueryDocumentReady();</script>");

            //���ӹ����������û��Զ����JSCode��ҳ��
            WFShareClass.AttachUserJSCodeFromWFTemplateStep(strTemName, "0", LIT_AttachUserWFStepJSCode, strUserCode);

            //���ñ������
            ClientScript.RegisterStartupScript(this.GetType(), "HH88H", "<script>setWorkflowForm();</script>");

        }
    }

    protected void UploadWFDIYFormTemplate(string strWFTemName)
    {
        int intResult;

        if (PublishUrl != null)
        {
            TakeTopInfoPathService.Remove(PublishUrl);
            PublishUrl = null;
        }

        //���Ϲ���RelatedCode TODO:CAOJIAN(�ܽ�)
        LoadWorkFlow(strWFType, strTemName, strRelatedCode);

        WFSubmitHandle wfSubmitHandle = new WFSubmitHandle();
        intResult = wfSubmitHandle.RegisterWorkFlowTemplate(strTemName, this.Context, this.uri, PublishUrl, xdoc, LB_XSNFile);

        PublishUrl = wfSubmitHandle.wfPublishUrl.ToString();
        uri = wfSubmitHandle.wfUri.ToString();

        OldPublishUrl = null;

        if (intResult == 0)
        {
            xdoc.Text = LanguageHandle.GetWord("CuoWuXiangYingMoBanHuoMoBanWen");
        }
        else
        {
            LB_OldWFXMLFile.Text = wfSubmitHandle.GetWFXMLFile("WorkFlow", PublishUrl, this.Context, xdoc);
        }

        LB_WLID.Text = "";
        TB_WorkFlowName.Text = "";
        TB_WLDescription.Text = "";

        HL_RelatedDoc.Enabled = false;
        BT_SaveXMLFile.Enabled = false;

        BT_Download.Enabled = true;
        BT_Upload.Enabled = true;

        HL_WFXSNFile.NavigateUrl = LB_XSNFile.Text;
    }

    protected void DL_WorkFlow_SelectedIndexChanged(object sender, EventArgs e)
    {
        int intResult;

        strWLID = DL_WorkFlow.SelectedValue.Trim();
        LB_WLID.Text = strWLID;

        //������ݿ���д��ڴ˹����������ݣ���ô�ѱ������ݸ��ӵ�����
        int intMainTableID;
        string strWFXMLFile;

        intMainTableID = ShareClass.GetWorkflowMainTableID(strWLID);
        strWFXMLFile = Server.MapPath(ShareClass.GetWorkflowXMLFile(strWLID));
        TakeTopXML.TableConvertToFormByMainID(strWLID, intMainTableID, strTemName, strWFXMLFile);


        if (PublishUrl == null)
        {
            PublishUrl = LB_PublishUrl.Text.Trim();
        }

        if (this.uri == null)
        {
            this.uri = LB_Uri.Text.Trim();
        }

        WFSubmitHandle wfSubmitHandle = new WFSubmitHandle();
        intResult = wfSubmitHandle.SelectedWorkFlow(strWLID, DL_WorkFlow, LB_WLID, LB_XSNFile, TB_WorkFlowName,
            TB_WLDescription, xdoc, BT_SaveXMLFile, HL_RelatedDoc, HL_RedirectToMyWFDetail, this.Context, PublishUrl, this.uri);

        OldPublishUrl = wfSubmitHandle.wfOldPublishUrl.ToString();
        OldUri = wfSubmitHandle.wfOldUri.ToString();

        HL_OldWFXMLFile.NavigateUrl = LB_OldWFXMLFile.Text;

        if (intResult == 0)
        {
            xdoc.Text = LanguageHandle.GetWord("SBZGZLMBHSJWJBCZKNYBSCQJC");
        }

        //�����û��Զ����JSCode��ҳ��
        WFShareClass.AttachUserJSCodeFromWFTemplate(strTemName, LIT_AttachUserJSCode, strUserCode, "", "0", "");
        ClientScript.RegisterStartupScript(this.GetType(), "HH66H", "<script>jqueryDocumentReady();</script>");

        ////���ӹ����������û��Զ����JSCode��ҳ��
        //WFShareClass.AttachUserJSCodeFromWFTemplateStep(strTemName, "0", LIT_AttachUserWFStepJSCode, strUserCode);
        ////���ñ������
        //ClientScript.RegisterStartupScript(this.GetType(), "HH99H", "<script>setWorkflowForm();</script>");

        //DL_WorkFlow.Width = 500;

        //HL_RelatedDoc.NavigateUrl = "javascript:popShowByURL('" + HL_RelatedDoc.NavigateUrl + "','ChartView','99%','99%',window.location);";
    }

    protected void BT_ActiveYes_Click(object sender, EventArgs e)
    {
        string strXSNFile;
        string strWorkFlowName, strDescription;
        int intWLID;

        strWLID = "0";

        strWorkFlowName = TB_WorkFlowName.Text.Trim();
        strDescription = TB_WLDescription.Text.Trim();
        strXSNFile = GetWorkTemplateXSNFile(strTemName);

        if (PublishUrl == OldPublishUrl | OldPublishUrl == null)
        {
            WFSubmitHandle wfSubmitHandle = new WFSubmitHandle();
            intWLID = wfSubmitHandle.SubmitApplyForDIYForm(strWFType, strTemName, strXSNFile, strWLID, strWorkFlowName, strDescription, "Other", 0, strUserCode, "NO",
                 CB_SMS, CB_Mail, LB_WLID, HL_RelatedDoc, HL_RedirectToMyWFDetail, BT_SaveXMLFile, DL_WorkFlow, PublishUrl, this.uri, this.Context, xdoc);

            strWLID = intWLID.ToString();

            if (intWLID == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZTJSBJC") + "');</script>");
            }
            else
            {
                try
                {
                    //���Ϲ���RelatedCode TODO:CAOJIAN(�ܽ�)
                    if (!string.IsNullOrEmpty(strRelatedCode))
                    {
                        string strUpdateRelatedCodeHQL = string.Format("update T_WorkFlow set RelatedCode = '{0}' where WLID = {1}", strRelatedCode, strWLID);
                        ShareClass.RunSqlCommand(strUpdateRelatedCodeHQL);

                        LoadWorkFlow(strWFType, strTemName, strRelatedCode);
                    }

                    try
                    {
                        //��������ݵ����ݿ⣬���ڿ���ƽ̨һ�㴦�����ʽ
                        ClientScript.RegisterStartupScript(this.GetType(), "SaveData",  "<script>saveWFFormDataToDatabase(" + intWLID.ToString() + ");</script>");
                    }
                    catch
                    {
                    }

                    try
                    {
                        //�����Զ�������ݵ���Ӧ���ݱ�,����XML�ڵ����ֶζ�Ӧ��ʽ
                        TakeTopXML.FormConvertToTable(int.Parse(strWLID), 0);
                    }
                    catch
                    {
                    }

                    new System.Threading.Thread(delegate ()
                    {
                        try
                        {
                            //�����Զ�������ݵ�ͳһ�������ݱ��������ݷ�����
                            string strWFXMLFile = ShareClass.GetWorkflowXMLFile(strWLID);
                            XmlDbWorker.AddFormFromXml(Server.MapPath(strWFXMLFile), int.Parse(strWLID), strTemName);
                        }
                        catch
                        {
                        }

                    }).Start();

                }
                catch
                {
                }

                //HL_RelatedDoc.NavigateUrl = "javascript:popShowByURL('" + HL_RelatedDoc.NavigateUrl + "','ChartView','99%','99%',window.location);";

                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>popShowByURL('" + "TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID + "','ToWork','99%','99%',window.location);</script>");

            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZTJSBZZXMBTXGZLS") + "');</script>");
        }
    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        string strXSNFile;
        string strWorkFlowName, strDescription;
        int intWLID;

        strWLID = "0";

        strWorkFlowName = TB_WorkFlowName.Text.Trim();
        strDescription = TB_WLDescription.Text.Trim();
        strXSNFile = GetWorkTemplateXSNFile(strTemName);

        if (PublishUrl == OldPublishUrl | OldPublishUrl == null)
        {
            WFSubmitHandle wfSubmitHandle = new WFSubmitHandle();
            intWLID = wfSubmitHandle.SubmitApplyForDIYForm(strWFType, strTemName, strXSNFile, strWLID, strWorkFlowName, strDescription, "Other", 0, strUserCode, "NO",
                CB_SMS, CB_Mail, LB_WLID, HL_RelatedDoc, HL_RedirectToMyWFDetail, BT_SaveXMLFile, DL_WorkFlow, PublishUrl, this.uri, Context, xdoc);

            strWLID = intWLID.ToString();

            if (intWLID == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZTJSBJC") + "');</script>");
            }
            else
            {
                LB_WLID.Text = strWLID;

                try
                {
                    //���Ϲ���RelatedCode TODO:CAOJIAN(�ܽ�)
                    if (!string.IsNullOrEmpty(strRelatedCode))
                    {
                        string strUpdateRelatedCodeHQL = string.Format("update T_WorkFlow set RelatedCode = '{0}' where WLID = {1}", strRelatedCode, strWLID);
                        ShareClass.RunSqlCommand(strUpdateRelatedCodeHQL);

                        LoadWorkFlow(strWFType, strTemName, strRelatedCode);
                    }

                    try
                    {
                        //��������ݵ����ݿ⣬���ڿ���ƽ̨һ�㴦�����ʽ
                        ClientScript.RegisterStartupScript(this.GetType(), "SaveData",  "<script>saveWFFormDataToDatabase(" + intWLID.ToString() + ");</script>");
                    }
                    catch
                    {
                    }

                    try
                    {
                        //�����Զ�������ݵ���Ӧ���ݱ�,����XML�ڵ����ֶζ�Ӧ��ʽ
                        TakeTopXML.FormConvertToTable(int.Parse(strWLID), 0);
                    }
                    catch
                    {
                    }

                    new System.Threading.Thread(delegate ()
                    {
                        try
                        {
                            //�����Զ�������ݵ�ͳһ�������ݱ��������ݷ�����
                            string strWFXMLFile = ShareClass.GetWorkflowXMLFile(strWLID);
                            XmlDbWorker.AddFormFromXml(Server.MapPath(strWFXMLFile), int.Parse(strWLID), strTemName);
                        }
                        catch
                        {
                        }

                    }).Start();

                }
                catch
                {
                }

                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZTJGZLSCG") + "');</script>");
            }

            //HL_RelatedDoc.NavigateUrl = "javascript:popShowByURL('" + HL_RelatedDoc.NavigateUrl + "','ChartView','99%','99%',window.location);";
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZTJSBZZXMBTXGZLS") + "');</script>");
        }
    }

    protected void BT_SaveXMLFile_Click(object sender, EventArgs e)
    {
        string strWLID, strWLName, strDescription;
        int intResult;

        strWLID = LB_WLID.Text.Trim();
        strWLName = TB_WorkFlowName.Text.Trim();
        strDescription = TB_WLDescription.Text.Trim();

        WFSubmitHandle wfSubmitHandle = new WFSubmitHandle();
        intResult = wfSubmitHandle.SaveWorkFlowXMLFile(strWLID, strWLName, strDescription, xdoc, this.Context, PublishUrl, this.uri);

        if (intResult == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "');</script>");
        }
        else
        {
            try
            {
                //���Ϲ���RelatedCode TODO:CAOJIAN(�ܽ�)
                if (!string.IsNullOrEmpty(strRelatedCode))
                {
                    string strUpdateRelatedCodeHQL = string.Format("update T_WorkFlow set RelatedCode = '{0}' where WLID = {1}", strRelatedCode, strWLID);
                    ShareClass.RunSqlCommand(strUpdateRelatedCodeHQL);

                    LoadWorkFlow(strWFType, strTemName, strRelatedCode);
                }

                try
                {
                    //ͬ�������ݵ���Ӧ�����ݿ��
                    TakeTopXML.FormConvertToTable(int.Parse(strWLID), 0);

                    //��������ݵ����ݿ�
                    ClientScript.RegisterStartupScript(this.GetType(), "SaveData",  "<script>saveWFFormDataToDatabase(" + strWLID + ");</script>");
                }
                catch
                {
                }

                new System.Threading.Thread(delegate ()
                {
                    try
                    {
                        //�����Զ�������ݵ�ͳһ�������ݱ��������ݷ�����
                        string strWFXMLFile = ShareClass.GetWorkflowXMLFile(strWLID);
                        XmlDbWorker.AddFormFromXml(Server.MapPath(strWFXMLFile), int.Parse(strWLID), strTemName);
                    }
                    catch
                    {
                    }

                }).Start();
            }
            catch
            {
            }

            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZBCCG") + "');</script>");
        }
    }

    protected void BT_BackupSaveXMLFile_Click(object sender, EventArgs e)
    {
        string strWLID, strWLName, strDescription;
        int intResult;

        strWLID = LB_WLID.Text.Trim();
        strWLName = TB_WorkFlowName.Text.Trim();
        strDescription = TB_WLDescription.Text.Trim();

        WFSubmitHandle wfSubmitHandle = new WFSubmitHandle();
        intResult = wfSubmitHandle.SaveWorkFlowXMLFile(strWLID, strWLName, strDescription, xdoc, this.Context, PublishUrl, this.uri);

        if (intResult == 0)
        {

        }
        else
        {
            try
            {
                //���Ϲ���RelatedCode TODO:CAOJIAN(�ܽ�)
                if (!string.IsNullOrEmpty(strRelatedCode))
                {
                    string strUpdateRelatedCodeHQL = string.Format("update T_WorkFlow set RelatedCode = '{0}' where WLID = {1}", strRelatedCode, strWLID);
                    ShareClass.RunSqlCommand(strUpdateRelatedCodeHQL);

                    LoadWorkFlow(strWFType, strTemName, strRelatedCode);
                }

                try
                {
                    //ͬ�������ݵ���Ӧ�����ݿ��
                    TakeTopXML.FormConvertToTable(int.Parse(strWLID), 0);

                    //��������ݵ����ݿ�
                    ClientScript.RegisterStartupScript(this.GetType(), "SaveData",  "<script>saveWFFormDataToDatabase(" + strWLID + ");</script>");
                }
                catch
                {

                }

                new System.Threading.Thread(delegate ()
                {
                    try
                    {
                        //�����Զ�������ݵ�ͳһ�������ݱ��������ݷ�����
                        string strWFXMLFile = ShareClass.GetWorkflowXMLFile(strWLID);
                        XmlDbWorker.AddFormFromXml(Server.MapPath(strWFXMLFile), int.Parse(strWLID), strTemName);
                    }
                    catch
                    {
                    }

                }).Start();
            }
            catch
            {
            }
        }
    }

    protected void BT_Download_Click(object sender, EventArgs e)
    {
        string strWorkFlowName, strXMLFileName;
        strWorkFlowName = "";

        try
        {
            strWorkFlowName = DL_WorkFlow.SelectedItem.Text.Trim();
            strXMLFileName = strWorkFlowName + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
        }
        catch
        {

            strXMLFileName = strTemName + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
        }

        WFDataHandle wfDataHandle = new WFDataHandle();
        wfDataHandle.DownloadWFXMLData(strTemName, strWorkFlowName, xdoc, PublishUrl, this.Context, this.Response);
    }

    protected void BT_Upload_Click(object sender, EventArgs e)
    {
        if (PublishUrl != null)
        {
            //The method TakeTopInfoPathService.BuildFormByXML() requires the string content of an existing xml document as a parameter.
            byte[] xmlContent = new byte[XMLFile.PostedFile.ContentLength];
            XMLFile.PostedFile.InputStream.Read(xmlContent, 0, xmlContent.Length);
            string xml = System.Text.UTF8Encoding.UTF8.GetString(xmlContent);

            TakeTopInfoPathForm editForm = TakeTopInfoPathService.BuildFormByXML(Context, PublishUrl, xml);
            xdoc.Text = editForm.Xhtml;

            BT_Download.Enabled = true;
        }
        else
        {
            xdoc.Text = LanguageHandle.GetWord("QingXianZhuCeBiaoChanMoBan");
        }
    }

    protected void LoadWorkFlow(string strWFType, string strTemName, string strRelatedCode)
    {
        string strHQL;
    
        if (!string.IsNullOrEmpty(strRelatedCode))
        {
            strHQL = "Select * from T_WorkFlow as workFlow where workFlow.WLType = " + "'" + strWFType + "'" + " and workFlow.TemName = " + "'" + strTemName + "'" + " and workFlow.RelatedCode = '" + strRelatedCode + "' and char_length(rtrim(ltrim(workFlow.XSNFile)))>0 Order by workFlow.CreateTime DESC Limit 50";
        }
        else
        {
            strHQL = "Select * from T_WorkFlow as workFlow where workFlow.WLType = " + "'" + strWFType + "'" + " and workFlow.TemName = " + "'" + strTemName + "'" + " and char_length(rtrim(ltrim(workFlow.XSNFile)))>0 Order by workFlow.CreateTime DESC Limit 50";
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");

        DL_WorkFlow.DataSource = ds;
        DL_WorkFlow.DataBind();

        DL_WorkFlow.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected string GetWorkTemplateXSNFile(string strTemName)
    {
        IList lst;
        string strHQL, strXSNFile;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        try
        {
            strXSNFile = workFlowTemplate.XSNFile.Trim();
        }
        catch
        {
            strXSNFile = "";
        }

        return strXSNFile;
    }

    protected string GetWorkFlowTem(string strWFID)
    {
        string strHQL;
        IList lst;

        strHQL = "From WorkFlowPage as workFlowPage Where workFlowPage.ID = " + strWFID;
        WorkFlowPageBLL workFlowPageBLL = new WorkFlowPageBLL();
        lst = workFlowPageBLL.GetAllWorkFlowPages(strHQL);

        WorkFlowPage workFlowPage = new WorkFlowPage();

        workFlowPage = (WorkFlowPage)lst[0];

        return workFlowPage.WFName.Trim();
    }

    protected void BT_AppendXML_Click(object sender, EventArgs e)
    {
        if (PublishUrl != null)
        {
            string strTriggerID = this.HF_TriggerID.Value;
            int intTriggerID = 0;
            int.TryParse(strTriggerID, out intTriggerID);
            //д��XML�ļ�
            string strFolder = "Doc\\XML\\";
            if (!System.IO.Directory.Exists(Server.MapPath(strFolder)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(strFolder));
            }
            string strTemNameR = DL_WorkFlow.SelectedValue.Trim();
            string strFileName = strFolder + strTemNameR + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".xml";
            strFileName = Server.MapPath(strFileName);

            //������
            string strXMLHQL = string.Empty;
            strXMLHQL = "select * from T_WorkFlow WHERE TemName = '" + strTemNameR + "' order by CreateTime desc";
            DataSet dsTest = ShareClass.GetDataSetFromSql(strXMLHQL, "XMLTest");
            if (dsTest != null && dsTest.Tables[0].Rows.Count > 0)
            {
                string strXSNFile = dsTest.Tables[0].Rows[0]["XMLFile"] == DBNull.Value ? "" : dsTest.Tables[0].Rows[0]["XMLFile"].ToString();
                if (!string.IsNullOrEmpty(strXSNFile))
                {
                    strXSNFile = Server.MapPath(strXSNFile);
                    TakeTopXML.TableConvertToFormByTriggerID(intTriggerID, strTemNameR, strFileName, strXSNFile);
                    try
                    {
                        string xml = ShareClass.ReadXML(strFileName);
                        TakeTopInfoPathForm editForm = TakeTopInfoPathService.BuildFormByXML(Context, PublishUrl, xml);
                        xdoc.Text = editForm.Xhtml;
                    }
                    catch (Exception ex) { }
                }
            }
        }
        else
        {
            xdoc.Text = LanguageHandle.GetWord("QingXianZhuCeBiaoChanMoBan");
        }
    }
}