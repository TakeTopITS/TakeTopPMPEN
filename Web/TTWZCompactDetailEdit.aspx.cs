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

public partial class TTWZCompactDetailEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (Request.QueryString["CompactDetailID"] != null)
            {
                string strCompactDetailID = Request.QueryString["CompactDetailID"];

                HF_CompactDetailID.Value = strCompactDetailID;

                DataCompactDetailBinder(strCompactDetailID);

            }
        }
    }



    private void DataCompactDetailBinder(string strCompactDetailID)
    {
        WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
        string strWZCompactDetailHQL = string.Format(@" from WZCompactDetail wZCompactDetail
                            where wZCompactDetail.ID = {0}", strCompactDetailID);
        IList lstCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);

        if (lstCompactDetail != null && lstCompactDetail.Count > 0)
        {
            WZCompactDetail wZCompactDetail = (WZCompactDetail)lstCompactDetail[0];

            TXT_CompactNumber.Text = wZCompactDetail.CompactNumber.ToString();
            TXT_CompactPrice.Text = wZCompactDetail.CompactPrice.ToString();
            TXT_CompactMoney.Text = wZCompactDetail.CompactMoney.ToString();
            TXT_Factory.Text = wZCompactDetail.Factory;
            TXT_StandardCode.Text = wZCompactDetail.StandardCode;
            TXT_Remark.Text = wZCompactDetail.Remark;
        }

    }







    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strCompactDetailID = HF_CompactDetailID.Value;
        string strCompactCode = HF_CompactCode.Value;
        if (!string.IsNullOrEmpty(strCompactDetailID))
        {
            string strCompactNumber = TXT_CompactNumber.Text.Trim();
            string strStandardCode = TXT_StandardCode.Text.Trim();
            string strFactory = TXT_Factory.Text.Trim();
            string strRemark = TXT_Remark.Text.Trim();

            if (!ShareClass.CheckIsNumber(strCompactNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHTSLZNWSZ").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strStandardCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGGSBHBNBHFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFactory))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCJBNBHFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRemark))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBZBNBHFFZF").ToString().Trim()+"')", true);
                return;
            }

            decimal decimalCompactNumber = 0;
            decimal.TryParse(strCompactNumber, out decimalCompactNumber);

            int intCompactDetailID = 0;
            int.TryParse(strCompactDetailID, out intCompactDetailID);
            WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
            string strWZCompactDetailHQL = string.Format(@"from WZCompactDetail as wZCompactDetail 
                        where ID = {0}", intCompactDetailID);
            IList listWZCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
            if (listWZCompactDetail != null && listWZCompactDetail.Count > 0)
            {
                WZCompactDetail wZCompactDetail = (WZCompactDetail)listWZCompactDetail[0];

                wZCompactDetail.CompactNumber = decimalCompactNumber;


                wZCompactDetail.StandardCode = strStandardCode;
                wZCompactDetail.Factory = strFactory;
                wZCompactDetail.Remark = strRemark;

                wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);

                //重新加载合同明细
                //DataCompactDetailBinder(strCompactCode);

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZYXGDHTMX").ToString().Trim()+"')", true);
            return;
        }
    }



}