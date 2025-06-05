using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTBMAnnClaFileSupplier : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            LoadBMSupplierReplyList(GetBMSupplierInfoID(strUserCode.Trim()));
        }
    }

    protected string GetBMSupplierInfoID(string strCode)
    {
        if (strCode.Trim().Contains("-"))
        {
            strCode = strCode.Trim().Substring(0, strCode.Trim().IndexOf("-"));
        }
        string strHQL = "Select * From T_BMSupplierInfo Where Code='" + strCode.Trim() + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");
        if (ds.Tables[0].Rows.Count > 0 && ds != null)
        {
            return ds.Tables[0].Rows[0]["ID"].ToString().Trim();
        }
        else
            return "0";
    }

    protected string GetBMAnnInvitationByPlanName(string strAnnClaFileID)
    {
        string strHQL = "from BMAnnClaFile as bMAnnClaFile where bMAnnClaFile.ID = '" + strAnnClaFileID.Trim() + "' ";
        BMAnnClaFileBLL bMAnnClaFileBLL = new BMAnnClaFileBLL();
        IList lst = bMAnnClaFileBLL.GetAllBMAnnClaFiles(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMAnnClaFile bMAnnClaFile = (BMAnnClaFile)lst[0];
            strHQL = "from BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.BidPlanID = '" + bMAnnClaFile.BidPlanID.ToString().Trim() + "' ";
            BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
            lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];
                return bMAnnInvitation.Name.Trim();
            }
            else
                return "";
        }
        else
            return "";
    }

    protected string GetBMAnnClaFileSendContent(string strAnnClaFileID)
    {
        string strHQL = "from BMAnnClaFile as bMAnnClaFile where bMAnnClaFile.ID = '" + strAnnClaFileID.Trim() + "' ";
        BMAnnClaFileBLL bMAnnClaFileBLL = new BMAnnClaFileBLL();
        IList lst = bMAnnClaFileBLL.GetAllBMAnnClaFiles(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMAnnClaFile bMAnnClaFile = (BMAnnClaFile)lst[0];
            return bMAnnClaFile.SendContent.Trim();
        }
        else
            return "";
    }

    protected void LoadBMSupplierReplyList(string strSupplierId)
    {
        string strHQL;

        strHQL = "Select * From T_BMSupplierReply Where SupplierId = '" + strSupplierId.Trim() + "' ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and SendContent like '%" + TextBox1.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-ReplyDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-ReplyDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierReply");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    /// <summary>
    /// 获得供应商的审核状态
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected string GetBMSupplierInfoStatus(string strCode)
    {
        if (strCode.Trim().Contains("-"))
        {
            strCode = strCode.Trim().Substring(0, strCode.Trim().IndexOf("-"));
        }
        string strHQL = "Select * From T_BMSupplierInfo Where Code='" + strCode.Trim() + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");
        if (ds.Tables[0].Rows.Count > 0 && ds != null)
        {
            return ds.Tables[0].Rows[0]["Status"].ToString().Trim();
        }
        else
            return "New";
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (!GetBMSupplierInfoStatus(strUserCode.Trim()).Trim().Equals("Qualified"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGYSXXWSHHZSHBTGBNJXTBCZJC") + "')", true);
            return;
        }

        string strHQL = "From BMSupplierReply as bMSupplierReply where bMSupplierReply.ID = '" + TB_ID.Text.Trim() + "'";
        BMSupplierReplyBLL bMSupplierReplyBLL = new BMSupplierReplyBLL();
        IList lst = bMSupplierReplyBLL.GetAllBMSupplierReplys(strHQL);
        BMSupplierReply bMSupplierReply = (BMSupplierReply)lst[0];

        bMSupplierReply.SendContent = string.IsNullOrEmpty(bMSupplierReply.SendContent) ? ShareClass.GetUserName(strUserCode.Trim()) + "：" + TB_ReplyContent.Text.Trim() : bMSupplierReply.SendContent.Trim() + "  " + ShareClass.GetUserName(strUserCode.Trim()) + "：" + TB_ReplyContent.Text.Trim();
        bMSupplierReply.ReplyDate = DateTime.Parse(DateTime.Now.ToString());

        try
        {
            bMSupplierReplyBLL.UpdateBMSupplierReply(bMSupplierReply, bMSupplierReply.ID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

            LoadBMSupplierReplyList(GetBMSupplierInfoID(strUserCode.Trim()));
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From BMSupplierReply as bMSupplierReply where bMSupplierReply.ID = '" + strId + "'";
            BMSupplierReplyBLL bMSupplierReplyBLL = new BMSupplierReplyBLL();
            lst = bMSupplierReplyBLL.GetAllBMSupplierReplys(strHQL);
            BMSupplierReply bMSupplierReply = (BMSupplierReply)lst[0];

            TB_ID.Text = bMSupplierReply.ID.ToString().Trim();
            TextBox5.Text = GetBMAnnInvitationByPlanName(bMSupplierReply.AnnClaFileID.ToString().Trim());
            TB_SendContent.Text = GetBMAnnClaFileSendContent(bMSupplierReply.AnnClaFileID.ToString().Trim());
            TB_ReplyContent.Text = string.IsNullOrEmpty(bMSupplierReply.SendContent) ? "" : bMSupplierReply.SendContent.Trim();

            BT_Update.Enabled = true;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierReply");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMSupplierReplyList(GetBMSupplierInfoID(strUserCode.Trim()));
    }
}