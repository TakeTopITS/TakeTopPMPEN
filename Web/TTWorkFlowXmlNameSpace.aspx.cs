using System; using System.Resources;
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

public partial class TTWorkFlowXmlNameSpace : System.Web.UI.Page
{
    string strUserCode, strMakeUserCode;

    protected void Page_Load(object sender, EventArgs e)
    { 
        string strIdentifyString = Request.QueryString["IdentifyString"].Trim();

        string strTemName = GetTemplateName(strIdentifyString);

        strUserCode = Session["UserCode"].ToString();
        strMakeUserCode = GetMakeUserCode(strTemName);


        LB_UserCode.Text = strUserCode;
        LB_TemName.Text = strTemName;

        //this.Title = "工作流模板：" + strTemName + "命名空间设置"; 

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            LoadXmlNameSpace(strTemName);

            if (strUserCode == strMakeUserCode)
            {
                BT_Add.Enabled = true;
            }
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID, strHQL;
            string strTemName;
            IList lst;

            strTemName = LB_TemName.Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            strHQL = "from XmlNameSpace as xmlNameSpace where xmlNameSpace.ID = " + strID;
            XmlNameSpaceBLL xmlNameSpaceBLL = new XmlNameSpaceBLL();
            lst = xmlNameSpaceBLL.GetAllXmlNameSpaces(strHQL);

            XmlNameSpace xmlNameSpace = (XmlNameSpace)lst[0];


            LB_ID.Text = xmlNameSpace.ID.ToString();
            TB_NameSpace.Text = xmlNameSpace.XmlNameSpaceName.Trim();
            TB_Attribute.Text = xmlNameSpace.XmlNameSpaceValue.Trim();

            if (strUserCode == strMakeUserCode)
            {
                BT_Add.Enabled = true;
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
            }
            else
            {
                BT_Add.Enabled = false;
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
            }
        }
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strID,strTemName,strNameSpace, strAttribute;
 
        strTemName = LB_TemName.Text.Trim();
        strNameSpace = TB_NameSpace.Text.Trim();
        strAttribute = TB_Attribute.Text.Trim();

        if (strNameSpace != "" & strAttribute != "")
        {
            XmlNameSpaceBLL xmlNameSpaceBLL = new XmlNameSpaceBLL();
            XmlNameSpace xmlNameSpace = new XmlNameSpace();

            xmlNameSpace.WLTemName = strTemName;
            xmlNameSpace.XmlNameSpaceName = strNameSpace;
            xmlNameSpace.XmlNameSpaceValue = strAttribute;

            try
            {
                xmlNameSpaceBLL.AddXmlNameSpace(xmlNameSpace);
               
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);
                LoadXmlNameSpace(strTemName);

                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim()+"')", true);

            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZMMKJSXBNWKJC").ToString().Trim()+"')", true);

        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {

        string strID, strTemName, strNameSpace, strAttribute;
        string strHQL;
        IList lst;


        strID = LB_ID.Text.Trim();
        strTemName = LB_TemName.Text.Trim();
        strNameSpace = TB_NameSpace.Text.Trim();
        strAttribute = TB_Attribute.Text.Trim();

        if (strNameSpace != "" & strAttribute != "")
        {
            XmlNameSpaceBLL xmlNameSpaceBLL = new XmlNameSpaceBLL();
            strHQL = "from XmlNameSpace as xmlNameSpace where xmlNameSpace.ID = " + strID;
            lst = xmlNameSpaceBLL.GetAllXmlNameSpaces(strHQL);

            XmlNameSpace xmlNameSpace = (XmlNameSpace)lst[0];

            xmlNameSpace.WLTemName = strTemName;
            xmlNameSpace.XmlNameSpaceName = strNameSpace;
            xmlNameSpace.XmlNameSpaceValue = strAttribute;

            try
            {
                xmlNameSpaceBLL.UpdateXmlNameSpace(xmlNameSpace, int.Parse(strID));

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
                LoadXmlNameSpace(strTemName);

                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);

            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZMMKJSXBNWKJC").ToString().Trim()+"')", true);

        }      

    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strID, strTemName, strNameSpace, strAttribute;
        string strHQL;
        IList lst;


        strID = LB_ID.Text.Trim();
        strTemName = LB_TemName.Text.Trim();
        strNameSpace = TB_NameSpace.Text.Trim();
        strAttribute = TB_Attribute.Text.Trim();

        if (strNameSpace != "" & strAttribute != "")
        {
            XmlNameSpaceBLL xmlNameSpaceBLL = new XmlNameSpaceBLL();
            strHQL = "from XmlNameSpace as xmlNameSpace where xmlNameSpace.ID = " + strID;
            lst = xmlNameSpaceBLL.GetAllXmlNameSpaces(strHQL);

            XmlNameSpace xmlNameSpace = (XmlNameSpace)lst[0];

            xmlNameSpace.WLTemName = strTemName;
            xmlNameSpace.XmlNameSpaceName = strNameSpace;
            xmlNameSpace.XmlNameSpaceValue = strAttribute;

            try
            {
                xmlNameSpaceBLL.DeleteXmlNameSpace(xmlNameSpace);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                LoadXmlNameSpace(strTemName);

                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);

            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZMMKJSXBNWKJC").ToString().Trim()+"')", true);

        }   
    }
    

    protected void  LoadXmlNameSpace(string strTemName)
    {
        string strHQL;
        IList lst;

        strHQL = "from XmlNameSpace as xmlNameSpace where xmlNameSpace.WLTemName = " + "'" +strTemName+ "'";
        XmlNameSpaceBLL xmlNameSpaceBLL = new XmlNameSpaceBLL ();
        lst = xmlNameSpaceBLL .GetAllXmlNameSpaces (strHQL );

        DataGrid1.DataSource = lst ;
        DataGrid1 .DataBind ();
        
    }

    protected string GetMakeUserCode(string strTemName)
    {
        string strHQL,strMakeUserCode;
        IList lst;

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName = " + "'" + strTemName + "'";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        strMakeUserCode = workFlowTemplate.CreatorCode.Trim();

        return strMakeUserCode;
    }

    protected string GetTemplateName(string strIdentifyString)
    {
        IList lst;
        string strHQL,strTemplateName;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.IdentifyString =" + "'" + strIdentifyString + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];
        strTemplateName = workFlowTemplate.TemName;

        return strTemplateName;
    }
}
