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

public partial class TTDWProductTypeList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {

            DataProductTypeBinder();
        }
    }


    private void DataProductTypeBinder()
    {
        DG_ProductType.CurrentPageIndex = 0;

        DWProductTypeBLL dWProductTypeBLL = new DWProductTypeBLL();
        string strDWProductTypeHQL = "from DWProductType as dWProductType order by dWProductType.ID desc";
        IList listDWProductType = dWProductTypeBLL.GetAllDWProductTypes(strDWProductTypeHQL);

        DG_ProductType.DataSource = listDWProductType;
        DG_ProductType.DataBind();

        LB_ProductTypeSql.Text = strDWProductTypeHQL;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DWProductTypeBLL dWProductTypeBLL = new DWProductTypeBLL();
            DWProductType dWProductType = new DWProductType();
            string strProductType = TXT_ProductType.Text.Trim();
            string strProductDesc = TXT_ProductDesc.Text.Trim();

            if (string.IsNullOrEmpty(strProductType))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCPLXBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strProductType))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCPLXBNBHFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strProductDesc))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCPMSBNBHFFZF+"')", true);
                return;
            }

            if (!string.IsNullOrEmpty(HF_ID.Value) && HF_ID.Value != "0")
            {
                string strID = HF_ID.Value;
                string strProductTypeHQL = string.Format(@"from DWProductType as dWProductType where ID = " + strID);
                IList lstProductType = dWProductTypeBLL.GetAllDWProductTypes(strProductTypeHQL);
                if (lstProductType != null && lstProductType.Count > 0)
                {
                    dWProductType = (DWProductType)lstProductType[0];

                    dWProductType.ProductType = strProductType;
                    dWProductType.ProductDesc = strProductDesc;

                    dWProductTypeBLL.UpdateDWProductType(dWProductType, int.Parse(strID));


                }
            }
            else
            {
                dWProductType.ProductType = strProductType;
                dWProductType.ProductDesc = strProductDesc;

                dWProductTypeBLL.AddDWProductType(dWProductType);

                string strSelectMaxHQL = "select ID from T_DWProductType order by ID desc limit 1";
                DataTable dtMaxID = ShareClass.GetDataSetFromSql(strSelectMaxHQL, "strSelectMaxHQL").Tables[0];
                if (dtMaxID != null && dtMaxID.Rows.Count > 0)
                {
                    int intID = 0;
                    int.TryParse(dtMaxID.Rows[0]["ID"] == DBNull.Value ? "0" : dtMaxID.Rows[0]["ID"].ToString(), out intID);
                    TXT_ID.Text = intID.ToString();
                }
            }

            DataProductTypeBinder();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
        }
        catch (Exception ex) { }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_ProductType.Items.Count; i++)
        {
            DG_ProductType.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "";
        TXT_ID.Text = "";

        TXT_ProductType.Text = "";
        TXT_ProductDesc.Text = "";

    }


    protected void DG_ProductType_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                for (int i = 0; i < DG_ProductType.Items.Count; i++)
                {
                    DG_ProductType.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strCmdArgu = e.CommandArgument.ToString();

                DWProductTypeBLL dWProductTypeBLL = new DWProductTypeBLL();
                string strProductTypeHQL = string.Format(@"from DWProductType as dWProductType where ID = " + strCmdArgu);
                IList lstProductType = dWProductTypeBLL.GetAllDWProductTypes(strProductTypeHQL);
                if (lstProductType != null && lstProductType.Count > 0)
                {
                    DWProductType dWProductType = (DWProductType)lstProductType[0];

                    HF_ID.Value = dWProductType.ID.ToString();
                    TXT_ID.Text = dWProductType.ID.ToString();

                    TXT_ProductType.Text = dWProductType.ProductType;
                    TXT_ProductDesc.Text = dWProductType.ProductDesc;
                }
            }
            else if (cmdName == "del")
            {
                string strCmdArgu = e.CommandArgument.ToString();

                DWProductTypeBLL dWProductTypeBLL = new DWProductTypeBLL();
                string strProductTypeHQL = string.Format(@"from DWProductType as dWProductType where ID = " + strCmdArgu);
                IList lstProductType = dWProductTypeBLL.GetAllDWProductTypes(strProductTypeHQL);
                if (lstProductType != null && lstProductType.Count > 0)
                {
                    DWProductType dWProductType = (DWProductType)lstProductType[0];

                    dWProductTypeBLL.DeleteDWProductType(dWProductType);

                    DataProductTypeBinder();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
                }
            }
        }
        catch (Exception ex) { }
    }


    protected void DG_ProductType_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_ProductType.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_ProductTypeSql.Text.Trim();
        DWProductTypeBLL dWProductTypeBLL = new DWProductTypeBLL();
        IList listDWProductType = dWProductTypeBLL.GetAllDWProductTypes(strHQL);

        DG_ProductType.DataSource = listDWProductType;
        DG_ProductType.DataBind();
    }
}