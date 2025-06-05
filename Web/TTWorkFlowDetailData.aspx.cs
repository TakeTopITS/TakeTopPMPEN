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
using System.Linq;

using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;
using System.Text;
using System.Collections.Generic;

using System.IO;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;


public partial class TTWorkFlowDetailData : System.Web.UI.Page
{
    protected string uri = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strID = Request.QueryString["ID"];
        string strStepID;

        string strWLID = Request.QueryString["WLID"];
        if (strWLID == "0")
        {
            return;
        }

        string strWLName, strTemName;
        string strHQL;
        IList lst1;

        string strUserCode;
        strUserCode = Session["UserCode"].ToString();

        string strXMLFile, strXMLFile1, strXMLFile2, strXMLFileName, strFieldList;
        string strXSNFile;
        string[] strUnVisibleField;
        int i;

        if (Request.QueryString["ID"] != null)
        {
            strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.ID = " + strID;
            WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();
            lst1 = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);
            WorkFlowStepDetail workFlowStepDetail = (WorkFlowStepDetail)lst1[0];

            strStepID = workFlowStepDetail.StepID.ToString().Trim();
            strWLID = workFlowStepDetail.WLID.ToString().Trim();
        }
        else
        {
            strStepID = "0";
        }

        strHQL = "from WorkFlow as workFlow where workFlow.WLID = " + strWLID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst1 = workFlowBLL.GetAllWorkFlows(strHQL);
        WorkFlow workFlow = (WorkFlow)lst1[0];

        strWLName = workFlow.WLName.Trim();
        strXMLFile = workFlow.XMLFile.Trim();
        strTemName = workFlow.TemName.Trim();
        strTemName = workFlow.TemName.Trim();

        if (Page.IsPostBack != true)
        {
            XmlDocument docXml = new XmlDocument();

            try
            {
                strXMLFile = Server.MapPath(strXMLFile);

                docXml.Load(strXMLFile);
            }
            catch
            {
                Response.Write(LanguageHandle.GetWord("CuoWuCiGongZuoLiuShuJuWenJianB"));
                return;
            }

            XmlNodeList xnl;
            XmlDocument document = new XmlDocument();

            document.PreserveWhitespace = true;
            document.Load(strXMLFile);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);

            XPathNavigator ipFormNav = document.CreateNavigator();
            ipFormNav.MoveToFollowing(XPathNodeType.Element);

            foreach (KeyValuePair<string, string> ns in ipFormNav.GetNamespacesInScope(XmlNamespaceScope.All))
            {
                if (ns.Key == String.Empty)
                {
                    nsmgr.AddNamespace("def", ns.Value);
                }
                else
                {
                    nsmgr.AddNamespace(ns.Key, ns.Value);
                }
            }

            if (strID == null)
            {
                strXSNFile = workFlow.XSNFile;

                if (strXSNFile == null)
                {
                    strXSNFile = "";
                }

                if (strXSNFile != "")
                {
                    strXSNFile = workFlow.XSNFile.Trim();
                    strXSNFile = Server.MapPath(strXSNFile);

                    Response.Redirect("TTWorkFlowInfoPathDataView.aspx?XSNFile=" + strXSNFile + "&XMLFile=" + strXMLFile + "&WLID=" + strWLID + "&StepID=" + strStepID + "&ID=0");
                }
                else
                {
                    Response.Redirect("TTWorkFlowCommonDataView.aspx?XMLFile=" + strXMLFile + "&TemName=" + strTemName + "&WLID=" + strWLID + "&StepID=" + strStepID + "&ID=0");
                }
            }
            else
            {
                strFieldList = GetFieldList(strID);

                try
                {
                    if (strFieldList != "")
                    {
                        strUnVisibleField = strFieldList.Split(",".ToCharArray());

                        for (i = 0; i < strUnVisibleField.Length; i++)
                        {
                            if (strUnVisibleField[i] != "")
                            {
                                xnl = docXml.SelectNodes(strUnVisibleField[i], nsmgr);

                                foreach (XmlNode xnd in xnl)
                                {
                                    xnd.InnerText = "";
                                }
                            }
                        }

                        strXMLFileName = DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
                        strXMLFile1 = "Doc\\" + "XML" + "\\" + strXMLFileName;
                        strXMLFile2 = Server.MapPath(strXMLFile1);
                        docXml.Save(strXMLFile2);
                    }
                    else
                    {
                        strXMLFile2 = strXMLFile;
                    }

                    strXSNFile = workFlow.XSNFile;

                    if (strXSNFile == null)
                    {
                        strXSNFile = "";
                    }

                    if (strXSNFile != "")
                    {
                        strXSNFile = workFlow.XSNFile.Trim();
                        strXSNFile = Server.MapPath(strXSNFile);

                        Response.Redirect("TTWorkFlowInfoPathDataView.aspx?XSNFile=" + strXSNFile + "&XMLFile=" + strXMLFile2 + "&WLID=" + strWLID + "&StepID=" + strStepID + "&ID=" + strID);
                    }
                    else
                    {
                        Response.Redirect("TTWorkFlowCommonDataView.aspx?XMLFile=" + strXMLFile2 + "&TemName=" + strTemName + "&WLID=" + strWLID + "&StepID=" + strStepID + "&ID=" + strID);
                    }
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZDKSPSJSBKNSXYGZLMBDBKSYDBDSCWJC") + "');</script>");
                }
            }
        }
    }

    protected string IsAllowFullEdit(string strID)
    {
        string strHQL;
        IList lst;

        string strAllowFullEdit;

        strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.ID = " + strID;
        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();
        lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);

        if (lst.Count > 0)
        {
            WorkFlowStepDetail workFlowStepDetail = (WorkFlowStepDetail)lst[0];

            strAllowFullEdit = workFlowStepDetail.AllowFullEdit.Trim();

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

    protected string GetFieldList(string strID)
    {
        string strHQL, strFieldList;
        IList lst;

        strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.ID = " + strID;
        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();
        lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);
        WorkFlowStepDetail workFlowStepDetail = (WorkFlowStepDetail)lst[0];

        strFieldList = workFlowStepDetail.FieldList;

        if (strFieldList == null)
        {
            strFieldList = "";
        }

        if (strFieldList != "")
        {
            strFieldList = workFlowStepDetail.FieldList.Trim();
        }

        return strFieldList;
    }

    private void IterateXmlNodes(XmlElement xmlfatherElement)
    {
        XmlNodeList childList = xmlfatherElement.ChildNodes;
        foreach (XmlElement child in childList)
        {

            // childNode.Text =  child.Attributes[0].Value;

            IterateXmlNodes(child);
        }
    }
}
