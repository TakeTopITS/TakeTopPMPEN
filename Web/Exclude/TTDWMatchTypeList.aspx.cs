using System; using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Data;
using System.Drawing;

public partial class TTDWMatchTypeList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {

            DataMatchTypeBinder();
        }
    }


    private void DataMatchTypeBinder()
    {
        DG_MatchType.CurrentPageIndex = 0;

        DWMatchTypeBLL dWMatchTypeBLL = new DWMatchTypeBLL();
        string strDWMatchTypeHQL = "from DWMatchType as dWMatchType order by dWMatchType.ID desc";
        IList listDWMatchType = dWMatchTypeBLL.GetAllDWMatchTypes(strDWMatchTypeHQL);

        DG_MatchType.DataSource = listDWMatchType;
        DG_MatchType.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DWMatchTypeBLL dWMatchTypeBLL = new DWMatchTypeBLL();
            DWMatchType dWMatchType = new DWMatchType();
            string strMatchType = TXT_MatchType.Text.Trim();
            string strMatchDesc = TXT_MatchDesc.Text.Trim();

            if (string.IsNullOrEmpty(strMatchType))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYLLXBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMatchType))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYLLXBNBHFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMatchDesc))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYLMSBNBHFFZF+"')", true);
                return;
            }

            if (!string.IsNullOrEmpty(HF_ID.Value) && HF_ID.Value != "0")
            {
                string strID = HF_ID.Value;
                string strMatchTypeHQL = string.Format(@"from DWMatchType as dWMatchType where ID = " + strID);
                IList lstMatchType = dWMatchTypeBLL.GetAllDWMatchTypes(strMatchTypeHQL);
                if (lstMatchType != null && lstMatchType.Count > 0)
                {
                    dWMatchType = (DWMatchType)lstMatchType[0];

                    dWMatchType.MatchType = strMatchType;
                    dWMatchType.MatchDesc = strMatchDesc;

                    dWMatchTypeBLL.UpdateDWMatchType(dWMatchType, int.Parse(strID));


                }
            }
            else
            {
                dWMatchType.MatchType = strMatchType;
                dWMatchType.MatchDesc = strMatchDesc;

                dWMatchTypeBLL.AddDWMatchType(dWMatchType);

                string strSelectMaxHQL = "select ID from T_DWMatchType order by ID desc limit 1";
                DataTable dtMaxID = ShareClass.GetDataSetFromSql(strSelectMaxHQL, "strSelectMaxHQL").Tables[0];
                if (dtMaxID != null && dtMaxID.Rows.Count > 0)
                {
                    int intID = 0;
                    int.TryParse(dtMaxID.Rows[0]["ID"] == DBNull.Value ? "0" : dtMaxID.Rows[0]["ID"].ToString(), out intID);
                    TXT_ID.Text = intID.ToString();
                }
            }

            DataMatchTypeBinder();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
        }
        catch (Exception ex) { }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_MatchType.Items.Count; i++)
        {
            DG_MatchType.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "";
        TXT_ID.Text = "";

        TXT_MatchType.Text = "";
        TXT_MatchDesc.Text = "";

    }


    protected void DG_MatchType_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                for (int i = 0; i < DG_MatchType.Items.Count; i++)
                {
                    DG_MatchType.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strCmdArgu = e.CommandArgument.ToString();

                DWMatchTypeBLL dWMatchTypeBLL = new DWMatchTypeBLL();
                string strMatchTypeHQL = string.Format(@"from DWMatchType as dWMatchType where ID = " + strCmdArgu);
                IList lstMatchType = dWMatchTypeBLL.GetAllDWMatchTypes(strMatchTypeHQL);
                if (lstMatchType != null && lstMatchType.Count > 0)
                {
                    DWMatchType dWMatchType = (DWMatchType)lstMatchType[0];

                    HF_ID.Value = dWMatchType.ID.ToString();
                    TXT_ID.Text = dWMatchType.ID.ToString();

                    TXT_MatchType.Text = dWMatchType.MatchType;
                    TXT_MatchDesc.Text = dWMatchType.MatchDesc;
                }
            }
            else if (cmdName == "del")
            {
                string strCmdArgu = e.CommandArgument.ToString();

                DWMatchTypeBLL dWMatchTypeBLL = new DWMatchTypeBLL();
                string strMatchTypeHQL = string.Format(@"from DWMatchType as dWMatchType where ID = " + strCmdArgu);
                IList lstMatchType = dWMatchTypeBLL.GetAllDWMatchTypes(strMatchTypeHQL);
                if (lstMatchType != null && lstMatchType.Count > 0)
                {
                    DWMatchType dWMatchType = (DWMatchType)lstMatchType[0];

                    dWMatchTypeBLL.DeleteDWMatchType(dWMatchType);

                    DataMatchTypeBinder();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
                }
            }
        }
        catch (Exception ex) { }
    }


    protected void DG_MatchType_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_MatchType.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_MatchTypeSql.Text.Trim();
        DWMatchTypeBLL dWMatchTypeBLL = new DWMatchTypeBLL();
        IList listDWMatchType = dWMatchTypeBLL.GetAllDWMatchTypes(strHQL);

        DG_MatchType.DataSource = listDWMatchType;
        DG_MatchType.DataBind();
    }
}