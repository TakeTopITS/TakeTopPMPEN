using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZSupplierTemplateFileList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();

            BindSupplierWorkFlowData();
        }
    }


    private void DataBinder()
    {
        string strSupplierTemplateHQL = @"select * from T_WZSupplierTemplateFile";
        DataTable dtSupplierTemplate = ShareClass.GetDataSetFromSql(strSupplierTemplateHQL, "SupplierTemplate").Tables[0];

        DG_List.DataSource = dtSupplierTemplate;
        DG_List.DataBind();

        LB_RecordCount.Text = dtSupplierTemplate.Rows.Count.ToString();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZSupplierTemplateFileBLL wZSupplierTemplateFileBLL = new WZSupplierTemplateFileBLL();
                string strWZSupplierTemplateFileSql = "from WZSupplierTemplateFile as wZSupplierTemplateFile where ID = " + cmdArges;
                IList supplierTemplateFileList = wZSupplierTemplateFileBLL.GetAllWZSupplierTemplateFiles(strWZSupplierTemplateFileSql);
                if (supplierTemplateFileList != null && supplierTemplateFileList.Count == 1)
                {
                    WZSupplierTemplateFile wZSupplierTemplateFile = (WZSupplierTemplateFile)supplierTemplateFileList[0];


                    wZSupplierTemplateFileBLL.DeleteWZSupplierTemplateFile(wZSupplierTemplateFile);

                    //重新加载列表
                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);
                }

            }
        }
    }


    //加载列表
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder();
    }



    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strID = HF_ID.Value;
        string strTemplateContent = TXT_TemplateContent.Value.Trim();

        if (string.IsNullOrEmpty(strTemplateContent))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSRRWLCNR+"')", true);
            return;
        }
        if (!ShareClass.CheckStringRight(strTemplateContent))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZRWLCNRBNWFFZFC+"')", true);
            return;
        }

        //strTemplateContent = strTemplateContent.StartsWith("<br />") ? strTemplateContent : "<br />" + strTemplateContent;

        if (!string.IsNullOrEmpty(strID))
        {
            //修改
            try
            {
                string strSupplierWorkFlowSQL = string.Format(@"update T_WZSupplierWorkFlow set TemplateContent = '{0}' where ID = {1}", strTemplateContent, strID);
                ShareClass.RunSqlCommand(strSupplierWorkFlowSQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
            }
            catch (Exception ex) { }
        }
        else
        {
            //新增
            try
            {
                string strSupplierWorkFlowSQL = string.Format(@"insert into T_WZSupplierWorkFlow(TemplateContent,CreateTime) values('{0}',now())", strTemplateContent);
                ShareClass.RunSqlCommand(strSupplierWorkFlowSQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTJCG+"')", true);
            }
            catch (Exception ex) { }
        }
    }




    private void BindSupplierWorkFlowData()
    {
        string strWZSupplierWorkFlowSql = @"select * from T_WZSupplierWorkFlow";
        DataTable dtWZSupplierWorkFlow = ShareClass.GetDataSetFromSql(strWZSupplierWorkFlowSql, "SupplierWorkFlow").Tables[0];
        if (dtWZSupplierWorkFlow != null && dtWZSupplierWorkFlow.Rows.Count > 0)
        {
            DataRow drSupplierWorkFlow = dtWZSupplierWorkFlow.Rows[0];

            string strID = ShareClass.ObjectToString(drSupplierWorkFlow["ID"]);
            string strTemplateContent = ShareClass.ObjectToString(drSupplierWorkFlow["TemplateContent"]);

            HF_ID.Value = strID;
            TXT_ID.Text = strID;
            TXT_TemplateContent.Value = strTemplateContent;
        }
    }



}