using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZObjectBigList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataBinder();
        }
    }

    private void DataBinder()
    {
        DG_List.CurrentPageIndex = 0;

        WZMaterialDLBLL wZMaterialDLBLL = new WZMaterialDLBLL();
        string strWZMaterialDLHQL = "from WZMaterialDL as wZMaterialDL order by DLCode";
        IList listWZMaterialDL = wZMaterialDLBLL.GetAllWZMaterialDLs(strWZMaterialDLHQL);

        DG_List.DataSource = listWZMaterialDL;
        DG_List.DataBind();

        LB_Sql.Text = strWZMaterialDLHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            for (int i = 0; i < DG_List.Items.Count; i++)
            {
                DG_List.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdName = e.CommandName;
            if (cmdName == "click")
            {
                //操作
                string cmdArges = e.CommandArgument.ToString();
                string[] arrOperate = cmdArges.Split('|');

                string strDLCode = arrOperate[0];
                string strDLName = arrOperate[1];
                string strDLDesc = arrOperate[2];

                HF_DLCode.Value = strDLCode;
                HF_DLName.Value = strDLName;
                TXT_DLDesc.Text = strDLDesc;

                TXT_DLDesc.BackColor = Color.CornflowerBlue;
            }
        }
    }

    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim(); ;
        WZMaterialDLBLL wZMaterialDLBLL = new WZMaterialDLBLL();
        IList lst = wZMaterialDLBLL.GetAllWZMaterialDLs(strHQL);

        DG_List.DataSource = lst;
        DG_List.DataBind();
    }

    protected void BT_Cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        HF_DLCode.Value = "";
        HF_DLName.Value = "";
        TXT_DLDesc.Text = "";

        TXT_DLDesc.BackColor = Color.White;
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strDLCode = HF_DLCode.Value;
        if (string.IsNullOrEmpty(strDLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZDLLB").ToString().Trim()+"')", true);
            return;
        }

        string strDLDesc = TXT_DLDesc.Text.Trim();
        if (!ShareClass.CheckStringRight(strDLDesc))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDLMSBNSFFZFC").ToString().Trim()+"')", true);
            return;
        }
        if (strDLDesc.Length > 30)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDLMSBNCG30GZFTXSY30GZFC").ToString().Trim()+"')", true);
            return;
        }


        WZMaterialDLBLL wZMaterialDLBLL = new WZMaterialDLBLL();
        string strDLName = HF_DLName.Value.Trim();


        WZMaterialDL wZMaterialDL = new WZMaterialDL();

        wZMaterialDL.DLCode = strDLCode;
        wZMaterialDL.DLName = strDLName;
        wZMaterialDL.DLDesc = strDLDesc;

        wZMaterialDLBLL.UpdateWZMaterialDL(wZMaterialDL, strDLCode);

        //重新加载列表
        DataBinder();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);

        HF_DLCode.Value = "";
        HF_DLName.Value = "";
        TXT_DLDesc.Text = "";

        TXT_DLDesc.BackColor = Color.White;
        return;
    }
}