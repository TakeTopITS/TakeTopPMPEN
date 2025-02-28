using System; using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Drawing;
using System.Data;

public partial class TTWZCompactCheckDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (Request.QueryString["CompactCode"] != null)
            {
                string strCompactCode = Request.QueryString["CompactCode"];

                HF_CompactCode.Value = strCompactCode;

                DataCompactDetailBinder(strCompactCode);
            }
        }
    }



    private void DataCompactDetailBinder(string strCompactCode)
    {
        string strWZCompactDetailHQL = string.Format(@"select cd.*,pd.PlanCode,hd.PurchaseCode,o.ObjectName from T_WZCompactDetail cd
                                        left join T_WZPickingPlanDetail pd on cd.PlanDetailID = pd.ID
                                        left join T_WZPurchaseDetail hd on cd.PurchaseDetailID = hd.ID 
                                        left join T_WZObject o on cd.ObjectCode = o.ObjectCode
                                        where cd.CompactCode = '{0}'", strCompactCode);
        DataTable dtCompact = ShareClass.GetDataSetFromSql(strWZCompactDetailHQL, "CompactDetail").Tables[0];

        DG_CompactDetail.DataSource = dtCompact;
        DG_CompactDetail.DataBind();

        LB_CompactDetailSql.Text = strWZCompactDetailHQL;
        #region 注释
        //WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
        //string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where CompactCode = '" + strCompactCode + "'";
        //IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);

        //DG_CompactDetail.DataSource = listCompactDetail;
        //DG_CompactDetail.DataBind();
        #endregion
    }


    protected void DG_CompactDetail_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_CompactDetail.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_CompactDetailSql.Text.Trim();
        DataTable dtCompact = ShareClass.GetDataSetFromSql(strHQL, "CompactDetail").Tables[0];

        DG_CompactDetail.DataSource = dtCompact;
        DG_CompactDetail.DataBind();
    }


    protected void DG_CompactDetail_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            string strCompactCode = HF_CompactCode.Value;
            if (cmdName == "edit")
            {
                for (int i = 0; i < DG_CompactDetail.Items.Count; i++)
                {
                    DG_CompactDetail.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();                             //ID|CheckCode|ObjectCode
                string[] arrArges = cmdArges.Split('|');

                WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                string strWZCompactDetailHQL = string.Format(@"from WZCompactDetail as wZCompactDetail 
                        where ID = {0}", arrArges[0]);
                IList listWZCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
                if (listWZCompactDetail != null && listWZCompactDetail.Count > 0)
                {
                    WZCompactDetail wZCompactDetail = (WZCompactDetail)listWZCompactDetail[0];

                    HF_CompactDetailID.Value = wZCompactDetail.ID.ToString();
                    TXT_CheckCode.Text = wZCompactDetail.CheckCode;

                    TXT_ObjectCode.Text = arrArges[2];
                }
            }
        }
    }


    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strCompactDetailID = HF_CompactDetailID.Value;
        string strCompactCode = HF_CompactCode.Value;
        if (!string.IsNullOrEmpty(strCompactDetailID))
        {
            string strCheckCode = TXT_CheckCode.Text.Trim();

            if (!ShareClass.CheckStringRight(strCheckCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJHBNBHFFZF+"')", true);
                return;
            }

            int intCompactDetailID = 0;
            int.TryParse(strCompactDetailID, out intCompactDetailID);
            WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
            string strWZCompactDetailHQL = string.Format(@"from WZCompactDetail as wZCompactDetail 
                        where ID = {0}", intCompactDetailID);
            IList listWZCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
            if (listWZCompactDetail != null && listWZCompactDetail.Count > 0)
            {
                WZCompactDetail wZCompactDetail = (WZCompactDetail)listWZCompactDetail[0];

                wZCompactDetail.CheckCode = strCheckCode;

                wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);

                //重新加载合同明细
                DataCompactDetailBinder(strCompactCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXZYXGDHTMX+"')", true);
            return;
        }
    }

    protected void BT_Reset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_CompactDetail.Items.Count; i++)
        {
            DG_CompactDetail.Items[i].ForeColor = Color.Black;
        }

        HF_CompactDetailID.Value = "";
        TXT_CheckCode.Text = "";

        TXT_ObjectCode.Text = "";

    }
}