
using System;
using System.Collections;
using System.Data;
using System.Web.UI;

using TakeTopCore;
using TakeTopWF;
using TakeTopInfoPathSoft.Service;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class _TTWorkFlowInfoPathDataView : System.Web.UI.Page
{
    string PublishUrl = null;
    string uri = null;

    public string strUserCode, strWLID, strWFName, strStepID, strID, strWLTemName, strWFXMLFile, strNewWFXMLFile;
    int intMainTableID;

    public string strXSNFile;
    public string strXMLFile;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strWFStatus, strStepStatus, strStepDetailStatus;
        string strEnablEdit, strAllowFullEdit;
        string strEditField;
        string strUnVisibleFieldXMLFile;

        strWLID = Request.QueryString["WLID"].Trim();

        if (Request.QueryString["XSNFile"] != null)
        {
            strXSNFile = Request.QueryString["XSNFile"].Trim();
        }
        else
        {
            strXSNFile = Request.QueryString["XSNFile"];
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGXSNFILEWKJC").ToString().Trim() + "');</script>");
            return;
        }
        LB_XSNFile.Text = strXSNFile;


        if (Request.QueryString["XMLFile"] != null)
        {
            strXMLFile = Request.QueryString["XMLFile"].Trim();
        }
        else
        {
            strXMLFile = Request.QueryString["XMLFile"];
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGXMLFILEWKJC").ToString().Trim() + "');</script>");
            return;
        }

        strStepID = Request.QueryString["StepID"].Trim();
        strID = Request.QueryString["ID"].Trim();

        strUserCode = Session["UserCode"].ToString();

        try
        {
            DataSet ds = GetWorkFlow(strWLID);
            strWFXMLFile = ds.Tables[0].Rows[0]["XMLFile"].ToString().Trim();
            strWLTemName = ds.Tables[0].Rows[0]["TemName"].ToString().Trim();
            strWFName = ds.Tables[0].Rows[0]["WLName"].ToString().Trim();

            LB_WLID.Text = strWLID;

            //装载相应的业务数据
            string strRelatedType, strRelatedID;
            strRelatedType = ds.Tables[0].Rows[0]["RelatedType"].ToString().Trim();
            strRelatedID = ds.Tables[0].Rows[0]["RelatedID"].ToString().Trim();
            LoadRelatedBusinessData(strRelatedType, strRelatedID);

            //如果数据库表中存在此工作流的数据，那么把表中数据附加到表单中
            intMainTableID = GetWorkflowMainTableID(strWLID);
            strNewWFXMLFile = TakeTopXML.TableConvertToFormByMainID(strWLID, intMainTableID, strWLTemName, Server.MapPath(strWFXMLFile));
            if (strNewWFXMLFile != "")
            {
                strWFXMLFile = strNewWFXMLFile;
            }

            ClientScript.RegisterStartupScript(this.GetType(), "clickParentA", "<script>aHandlerForWorkflowCommonFormDataPopWindow();</script>");
            if (Page.IsPostBack == false)
            {

                if (ShareClass.IsMobileDeviceCheckAgent())
                {
                    IMG_LRArrow.Visible = false;
                    aPrintAll.Visible = false;
                    aPrintForm.Visible = false;
                }

                strWFStatus = ds.Tables[0].Rows[0]["Status"].ToString().Trim();

                //附加用户自定义的JSCode到页面
                string strWFCreatorCode = ds.Tables[0].Rows[0]["CreatorCode"].ToString().Trim();
                WFShareClass.AttachUserJSCodeFromWFTemplate(strWLTemName, LIT_AttachUserJSCode, strWFCreatorCode, "", "0", "");

                //附加工作流步骤用户自定义的JSCode到页面
                if (strStepID != "0")
                {
                    string strCurrentStepSortNumber = ShareClass.GetWorkFlowCurrentStepSortNumber(strStepID).ToString();
                    WFShareClass.AttachUserJSCodeFromWFTemplateStep(strWLTemName, strCurrentStepSortNumber, LIT_AttachUserWFStepJSCode, strWFCreatorCode);
                }


                //取最新流程表单数据文件，并在替换不可视域
                WFDataHandle wfDataHandle = new WFDataHandle();
                strUnVisibleFieldXMLFile = wfDataHandle.GetXMLFileAfterReplaceWFXmlUNVisibleFieldNode(strWLID, strID, strWFXMLFile);

                LB_UnVisibleFieldXMLFile.Text = strUnVisibleFieldXMLFile;

                //注册流程表单模板并装载流程表单数据
                wfDataHandle.RegisterWFTemplateAndLoadWFFormData(strXSNFile, strUnVisibleFieldXMLFile, this.uri, PublishUrl, xdoc, this.Context);

                PublishUrl = wfDataHandle.wfPublishUrl.ToString();
                uri = wfDataHandle.wfUri.ToString();

                LB_PublishUrl.Text = PublishUrl;
                LB_Uri.Text = uri;

                strEnablEdit = GetWFEnableEdit(strWLTemName);
                strAllowFullEdit = IsAllowFullEdit(strID);

                strStepStatus = GetWorkFlowStepStatus(strStepID);


                if ((strStepStatus == "InProgress" | strStepStatus == "New") & (strWFStatus != "Passed" & strWFStatus != "CaseClosed") & (strEnablEdit == "YES"))
                {
                    strEditField = GetEditFieldList(strID);

                    if (strEditField == "" & strID == "0")
                    {
                        if (GetWorkFlowStepCount(strWLID) > 0)
                        {
                            BT_SaveXMLFile.Visible = false;
                            BT_BackupSaveXMLFile.Visible = false;
                        }
                        else
                        {
                            BT_SaveXMLFile.Visible = true;
                            BT_BackupSaveXMLFile.Visible = true;
                        }
                    }
                    else
                    {
                        if (strAllowFullEdit == "NO")
                        {
                            if (strEditField != "" & strID != "0")
                            {
                                BT_SaveXMLFile.Visible = true;
                                BT_BackupSaveXMLFile.Visible = true;
                            }
                            else
                            {
                                BT_SaveXMLFile.Visible = false;
                                BT_BackupSaveXMLFile.Visible = false;
                            }
                        }
                        else
                        {
                            BT_SaveXMLFile.Visible = true;
                            BT_BackupSaveXMLFile.Visible = true;
                        }
                    }
                }

                try
                {
                    strStepDetailStatus = ShareClass.GetWorkFlowStepDetail(strID).Status.Trim();
                }
                catch
                {
                    strStepDetailStatus = "";
                }
                if (strStepDetailStatus == "Approved")
                {
                    BT_SaveXMLFile.Visible = false;
                    BT_BackupSaveXMLFile.Visible = false;
                }

            }

            try
            {
                LB_WorkflowStatus.Text = GetStatusHomeNameByWorkflowStatus(strWLID);

                LoadRelatedDoc(strWLID);
                LoadWorkFlowRelatedOtherDoc(strUserCode, strWLID);
                LoadWorkflowApproveRecord(strWLID);

                //取和上级流程的审批记录
                string strParentWFID, strParentWFStepID;
                strParentWFID = ShareClass.GetParentWorklowID(strWLID);
                strParentWFStepID = ShareClass.GetParentWorklowStepID(strWLID);
                if (strParentWFID != "0")
                {
                    HL_ParentWorkflowApproveRecord.Visible = true;
                    HL_ParentWorkflowApproveRecord.NavigateUrl = "TTWorkFlowViewMain.aspx?WLID=" + strParentWFID;
                }
                else
                {
                    HL_ParentWorkflowApproveRecord.Visible = false;
                }

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
            catch (Exception err)
            {
                //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }

        }
        catch (Exception err)
        {
            //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGCGZLMBBCZQJC").ToString().Trim() + "');</script>");
        }
    }

    //装载相应的业务数据
    protected void LoadRelatedBusinessData(string strRelateType,string strRelatedID)
    {
        string strHQL;

        DataSet ds;

        if(strRelateType == "合同") 
        {
            strHQL = "select * from T_Constract where ConstractID = " + strRelatedID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_Constract");

            DataList1.DataSource = ds;
            DataList1.DataBind();
        }

        if (strRelateType == "Project")
        {
            strHQL = "select * from T_Project where ProjectID = " + strRelatedID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

            DataList2.DataSource = ds;
            DataList2.DataBind();
        }


        if (strRelateType == "Plan")
        {
            strHQL = "select * from T_ImplePlan where ID = " + strRelatedID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");

            DataList3.DataSource = ds;
            DataList3.DataBind();
        }

        if (strRelateType == "投标") 
        {
            strHQL = "select * from T_Tender_HYYQ where ID = " + strRelatedID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_Tender_HYYQ");

            DataList4.DataSource = ds;
            DataList4.DataBind();
        }

    }

    protected void BT_SaveXMLFile_Click(object sender, EventArgs e)
    {
        string strXMLFile, strWFName;

        strWFName = GetWorkFlow(strWLID).Tables[0].Rows[0]["WLName"].ToString().Trim();

        if (PublishUrl == null)
        {
            PublishUrl = LB_PublishUrl.Text.Trim();
        }

        if (this.uri == null)
        {
            this.uri = LB_Uri.Text.Trim();
        }

        WFSubmitHandle wfSubmitHandle = new WFSubmitHandle();

        try
        {
            WFDataHandle wfDataHandle = new WFDataHandle();
            strXMLFile = wfDataHandle.GetWorkFlowXMLFile(strWFName, PublishUrl, xdoc, this.Context);

            //检查必填项不为空
            if (CheckIsNotNullForCanNotNullField(strID, strWLTemName, strXMLFile))
            {
                int intResult = wfDataHandle.SaveWorkFlowXMLFile(strWLID, strID, strXMLFile, strWFXMLFile, "Common");
                if (intResult > 0)
                {
                    //赋全局变量给工作流XML文件
                    try
                    {
                        wfSubmitHandle.AddGlobalVariable(strWLTemName, strWLID, "", "", "");
                    }
                    catch (Exception ex)
                    {
                    }

                    if (intResult == 1)
                    {
                        try
                        {
                            //保存表单数据到数据库，用于开发平台一般处理程序方式
                            ClientScript.RegisterStartupScript(this.GetType(), "SaveData",  "<script>saveWFFormDataToDatabase(" + strWLID + ");</script>");
                        }
                        catch
                        {
                        }

                        try
                        {
                            //保存自定义表单数据到对应数据表,用于XML节点与字段对应方式
                            TakeTopXML.FormConvertToTable(int.Parse(strWLID), 0);
                        }
                        catch
                        {
                        }

                        try
                        {
                            //保存工作流数据文件到流程明细表作为记录用
                            SaveWorkflowXMLFileToDetailTable(strID, strXMLFile);
                        }
                        catch
                        {
                        }

                        new System.Threading.Thread(delegate ()
                        {
                            try
                            {
                                try
                                {
                                    //保存自定义表单数据到统一流程数据表，用于数据分析用
                                    XmlDbWorker.AddFormFromXml(Server.MapPath(strWFXMLFile), int.Parse(strWLID), strWLTemName);

                                }
                                catch
                                {
                                }
                            }
                            catch
                            {
                            }
                        }).Start();

                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "');</script>");
                    }
                }
                else
                {
                    try
                    {
                        wfSubmitHandle.ReLoadFormDataBeforeSubmit(this.Context, LB_PublishUrl.Text.Trim(), LB_Uri.Text.Trim(), xdoc);
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGZXJZBCSJSBJC").ToString().Trim() + "');</script>");
                    }

                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "');</script>");
                }
            }
            else
            {
                string strCanNotNullFieldList = GetCanNotNullFieldList(strID, strWLTemName, strXMLFile);
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZTJSBBTXSTRCANNOTNULLFIELDLISTBNWKJC").ToString().Trim() + "');</script>");
            }
        }
        catch
        {
            try
            {
                wfSubmitHandle.ReLoadFormDataBeforeSubmit(this.Context, LB_PublishUrl.Text.Trim(), LB_Uri.Text.Trim(), xdoc);
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGZXJZBCSJSBJC").ToString().Trim() + "');</script>");
            }
        }
    }

    protected void BT_BackupSaveXMLFile_Click(object sender, EventArgs e)
    {
        string strXMLFile, strWFName;

        strWFName = GetWorkFlow(strWLID).Tables[0].Rows[0]["WLName"].ToString().Trim();

        if (PublishUrl == null)
        {
            PublishUrl = LB_PublishUrl.Text.Trim();
        }

        if (this.uri == null)
        {
            this.uri = LB_Uri.Text.Trim();
        }

        WFSubmitHandle wfSubmitHandle = new WFSubmitHandle();

        try
        {
            WFDataHandle wfDataHandle = new WFDataHandle();
            strXMLFile = wfDataHandle.GetWorkFlowXMLFile(strWFName, PublishUrl, xdoc, this.Context);

            //检查必填项不为空
            if (CheckIsNotNullForCanNotNullField(strID, strWLTemName, strXMLFile))
            {
                int intResult = wfDataHandle.SaveWorkFlowXMLFile(strWLID, strID, strXMLFile, strWFXMLFile, "Common");
                if (intResult > 0)
                {
                    //赋全局变量给工作流XML文件
                    try
                    {
                        wfSubmitHandle.AddGlobalVariable(strWLTemName, strWLID, "", "", "");
                    }
                    catch (Exception ex)
                    {
                    }

                    if (intResult == 1)
                    {
                        try
                        {
                            //保存表单数据到数据库，用于开发平台一般处理程序方式
                            ClientScript.RegisterStartupScript(this.GetType(), "SaveData",  "<script>saveWFFormDataToDatabase(" + strWLID + ");</script>");
                        }
                        catch
                        {
                        }

                        try
                        {
                            //保存自定义表单数据到对应数据表,用于XML节点与字段对应方式
                            TakeTopXML.FormConvertToTable(int.Parse(strWLID), 0);
                        }
                        catch
                        {
                        }

                        try
                        {
                            //保存工作流数据文件到流程明细表作为记录用
                            SaveWorkflowXMLFileToDetailTable(strID, strXMLFile);
                        }
                        catch
                        {
                        }

                        new System.Threading.Thread(delegate ()
                        {
                            try
                            {
                                try
                                {
                                    //保存自定义表单数据到统一流程数据表，用于数据分析用
                                    XmlDbWorker.AddFormFromXml(Server.MapPath(strWFXMLFile), int.Parse(strWLID), strWLTemName);
                                }
                                catch
                                {
                                }
                            }
                            catch
                            {
                            }
                        }).Start();
                    }
                }
                else
                {
                    try
                    {
                        wfSubmitHandle.ReLoadFormDataBeforeSubmit(this.Context, LB_PublishUrl.Text.Trim(), LB_Uri.Text.Trim(), xdoc);
                    }
                    catch
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGZXJZBCSJSBJC").ToString().Trim() + "');</script>");
                    }

                  /*  ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "');</script>")*/;
                }
            }
            else
            {
                string strCanNotNullFieldList = GetCanNotNullFieldList(strID, strWLTemName, strXMLFile);
                //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZTJSBBTXSTRCANNOTNULLFIELDLISTBNWKJC").ToString().Trim() + "');</script>");
            }
        }
        catch
        {
            try
            {
                wfSubmitHandle.ReLoadFormDataBeforeSubmit(this.Context, LB_PublishUrl.Text.Trim(), LB_Uri.Text.Trim(), xdoc);
            }
            catch
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGZXJZBCSJSBJC").ToString().Trim() + "');</script>");
            }
        }
    }

    //保存工作流数据文件到流程明细表作为记录用
    protected void SaveWorkflowXMLFileToDetailTable(string strDetailID, string strXMLFile)
    {
        string strHQL;

        strHQL = "Update T_WorkFlowStepDetail Set XMLFile = '" + strXMLFile + "', DetailXMLFile = ''  Where ID = " + strDetailID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected bool CheckIsNotNullForCanNotNullField(string strID, string strTemName, string strXMLFile)
    {
        string strCanNotNullFieldList;

        WFDataHandle wfDataHandle = new WFDataHandle();

        //检查必填项不为空
        if (strID == "0")
        {
            strCanNotNullFieldList = WFShareClass.GetCanNotNullFieldListFromWFTemplate(strWLTemName);
        }
        else
        {
            strCanNotNullFieldList = WFShareClass.GetCanNotNullFieldListFromWFStepDetail(strID);
        }

        return wfDataHandle.CheckCanNotNullField(strCanNotNullFieldList, strXMLFile);

    }

    protected string GetCanNotNullFieldList(string strID, string strTemName, string strXMLFile)
    {
        string strCanNotNullFieldList;

        WFDataHandle wfDataHandle = new WFDataHandle();

        //检查必填项不为空
        if (strID == "0")
        {
            strCanNotNullFieldList = WFShareClass.GetCanNotNullFieldListFromWFTemplate(strWLTemName);
        }
        else
        {
            strCanNotNullFieldList = WFShareClass.GetCanNotNullFieldListFromWFStepDetail(strID);
        }

        return strCanNotNullFieldList;

    }

    protected string GetEditFieldList(string strID)
    {
        string strHQL, strEditFieldList;
        DataSet ds;

        strHQL = "Select * from T_WorkFlowStepDetail where ID = " + strID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * from T_WorkFlowStepDetailBackup where ID = " + strID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetailBackup");
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            strEditFieldList = ds.Tables[0].Rows[0]["EditFieldList"].ToString().Trim();

            if (strEditFieldList == null)
            {
                strEditFieldList = "";
            }

            return strEditFieldList.Trim();
        }
        else
        {
            return "";
        }
    }

    protected string IsAllowFullEdit(string strID)
    {
        string strHQL;
        DataSet ds;

        string strAllowFullEdit;


        strHQL = "Select * from T_WorkFlowStepDetail where ID = " + strID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * from T_WorkFlowStepDetailBackup where ID = " + strID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetailBackup");
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            strAllowFullEdit = ds.Tables[0].Rows[0]["AllowFullEdit"].ToString().Trim();

            if (strAllowFullEdit == null)
            {
                strAllowFullEdit = "NO";
            }

            return strAllowFullEdit;
        }
        else
        {
            return "NO";
        }
    }

    protected string GetWFEnableEdit(string strTemName)
    {
        IList lst;
        string strHQL;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        return workFlowTemplate.EnableEdit.Trim();
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

    protected int GetWorkFlowStepCount(string strWLID)
    {
        string strHQL;
        DataSet ds;

        strHQL = "Select * from T_WorkFlowStepDetail where WLID = " + strWLID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * from T_WorkFlowStepDetailBackup where WLID = " + strWLID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetailBackup");
        }

        return ds.Tables[0].Rows.Count;
    }

    protected string GetWorkFlowStatus(string strWLID)
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

        return ds.Tables[0].Rows[0]["Status"].ToString().Trim();
    }


    //取得工作流关联的MainTableID
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

    protected string GetWorkFlowTemName(string strWLID)
    {
        string strHQL;
        DataSet ds;

        strHQL = "Select * from T_WorkFlow where WLID = " + strWLID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select Status From T_WorkFlowBackup Where WLID = " + strWLID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkflowBackup");
        }

        return ds.Tables[0].Rows[0]["TemName"].ToString().Trim();
    }

    protected string GetWorkFlowStepStatus(string strStepID)
    {
        string strHQL;
        DataSet ds;

        strHQL = "Select * from T_WorkFlowStep where StepID = " + strStepID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStep");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * from T_WorkFlowStepBackup where StepID = " + strStepID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepBackup");
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0]["Status"].ToString().Trim();
        }
        else
        {
            return "New";
        }
    }

    protected string GetStatusHomeNameByWorkflowStatus(string strWLID)
    {
        string strHQL;
        string strStatus;
        DataSet ds;

        strHQL = "Select Status From T_WorkFlow Where WLID = " + strWLID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_Workflow");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select Status From T_WorkFlowBackup Where WLID = " + strWLID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkflowBackup");
        }

        strStatus = ds.Tables[0].Rows[0][0].ToString();

        return ShareClass.GetStatusHomeNameByWorkflowStatus(strStatus);
    }

    protected void LoadWorkflowApproveRecord(string strWFID)
    {
        string strHQL;
        DataSet ds;

        try
        {
            strHQL = "Select A.*,B.SortNumber,B.StepName from T_WorkFlowStepDetail A,T_WorkFlowStep B where A.StepID = B.StepID and  (A.WLID = " + strWFID;
            strHQL += " or A.WLID in (Select WFChildID From T_WFStepRelatedWF Where WFID = " + strWFID + " ))";
            strHQL += " and COALESCE(A.Operation,'') <> ''";
            strHQL += " Order by A.ID DESC";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ApproveFlow");
            if (ds.Tables[0].Rows.Count == 0)
            {
                strHQL = "Select A.*,B.SortNumber,B.StepName from T_WorkFlowStepDetailBackup A,T_WorkFlowStepBackup B where A.StepID = B.StepID and  (A.WLID = " + strWFID;
                strHQL += " or A.WLID in (Select WFChildID From T_WFStepRelatedWF Where WFID = " + strWFID + " ))";
                strHQL += " and COALESCE(A.Operation,'') <> ''";
                strHQL += " Order by A.ID DESC";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ApproveFlowBackup");
            }

            DataList29.DataSource = ds;
            DataList29.DataBind();
        }
        catch
        {
        }
    }

    protected void LoadRelatedDoc(string strWLID)
    {
        string strHQL, strUserCode, strDepartCode;
        IList lst;

        strUserCode = Session["UserCode"].ToString();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = "from Document as document where document.Status <> 'Deleted' ";
        strHQL += " and (document.RelatedType = 'Workflow' and (document.RelatedID = " + strWLID + " Or document.RelatedID in (Select wfStepRelatedWF.WFChildID From WFStepRelatedWF as wfStepRelatedWF Where wfStepRelatedWF.WFID = " + strWLID + ")";
        strHQL += " Or document.RelatedID in (Select wfStepRelatedWF.WFID From WFStepRelatedWF as wfStepRelatedWF Where wfStepRelatedWF.WFChildID = " + strWLID + "))";
        strHQL += "or ((document.RelatedType = '会议' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Workflow' and (meeting.RelatedID =" + strWLID + " or meeting.RelatedID in (Select wfStepRelatedWF.WFChildID From WFStepRelatedWF as wfStepRelatedWF Where wfStepRelatedWF.WFID = " + strWLID + "))))"; 
        strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
        strHQL += " or ( document.Visible = '会议')))"; 
        strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
        strHQL += " or (document.Visible in ( '部门','全体')))"; 
        strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))))";
        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadWorkFlowRelatedOtherDoc(string strUserCode, string strWLID)
    {
        string strHQL, strDepartCode, strProjectID;
        IList lst;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        DataSet ds = GetWorkFlowByWLID(strWLID);

        if (ds == null)
        {
            return;
        }

        if (ds.Tables[0].Rows[0]["RelatedType"].ToString().Trim() == "Project")
        {
            strProjectID = ds.Tables[0].Rows[0]["RelatedID"].ToString().Trim();

            strHQL = "from Document as document where (((document.RelatedType = 'Project' and document.RelatedID = " + strProjectID + ")";
            strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( '部门','全体')))"; 
            strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))))";

            strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
            //strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + "))";

            strHQL += "or (document.RelatedType = '风险' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))"; 
            strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
            strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
            //strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + ")))";

            strHQL += "or (document.RelatedType = '会议' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + "))"; 
            strHQL += " and ((document.Visible in ('会议','部门') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) "; 
            strHQL += " or (document.Visible = '全体' )))))"; 
            strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

            DocumentBLL documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            LB_ProjectDocNumber.Text = lst.Count.ToString();
        }
    }



    protected DataSet GetWorkFlowByWLID(string strWLID)
    {
        string strHQL;

        strHQL = "Select * From T_WorkFlow Where WLID  = " + strWLID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * From T_WorkFlowBackup Where WLID  = " + strWLID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowBackup");
        }

        if (ds.Tables[0].Rows.Count == 0)
        {
            return null;
        }
        else
        {
            return ds;
        }
    }

}
