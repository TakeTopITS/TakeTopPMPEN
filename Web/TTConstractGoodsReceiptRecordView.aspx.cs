using System; using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTConstractGoodsReceiptRecordView : System.Web.UI.Page
{
    string strReceiptPlanID, strConstractCode;
    decimal deReceivablesNumber = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        strReceiptPlanID = Request.QueryString["ReceiptPlanID"];

        strHQL = "from ConstractGoodsReceiptPlan as constractGoodsReceiptPlan where constractGoodsReceiptPlan.ID = " + strReceiptPlanID;
        ConstractGoodsReceiptPlanBLL constractGoodsReceiptPlanBLL = new ConstractGoodsReceiptPlanBLL();
        lst = constractGoodsReceiptPlanBLL.GetAllConstractGoodsReceiptPlans(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        ConstractGoodsReceiptPlan constractGoodsReceiptPlan = (ConstractGoodsReceiptPlan)lst[0];
        strConstractCode = constractGoodsReceiptPlan.ConstractCode.Trim();
        deReceivablesNumber = constractGoodsReceiptPlan.Number;

        //this.Title = "收货计划：" + strReceiptPlanID + "执行情况";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LB_ReceiptGoodsType.Text = constractGoodsReceiptPlan.Type.Trim();
            TB_ReceiptGoodsCode.Text = constractGoodsReceiptPlan.GoodsCode;
            TB_ReceiptGoodsName.Text = constractGoodsReceiptPlan.GoodsName;
            TB_ReceiptGoodsModelNumber.Text = constractGoodsReceiptPlan.ModelNumber;
            TB_ReceiptGoodsSpec.Text = constractGoodsReceiptPlan.Spec;
            LB_ReceiptGoodsType.Text = constractGoodsReceiptPlan.Type;
            LB_ReceiptGoodsUnit.Text = constractGoodsReceiptPlan.Unit;
            NB_ReceiptGoodsNumber.Amount = constractGoodsReceiptPlan.Number;
            NB_ReceiptGoodsPrice.Amount = constractGoodsReceiptPlan.Price;
            TB_ReceiptAddress.Text = constractGoodsReceiptPlan.ReceiptAddress;

            DLC_ReceiptGoodsTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadConstractGoodsReceiptRecordList(strReceiptPlanID);

            CountGoodsReceiptNumber(strReceiptPlanID);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID, strConstractCode;
            IList lst;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            LB_ID.Text = strID;

            strConstractCode = LB_ConstractCode.Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from ConstractGoodsReceiptRecord as constractGoodsReceiptRecord Where constractGoodsReceiptRecord.ReceiptPlanID = " + "'" + strReceiptPlanID + "'";

            ConstractGoodsReceiptRecordBLL constractGoodsReceiptRecordBLL = new ConstractGoodsReceiptRecordBLL();
            lst = constractGoodsReceiptRecordBLL.GetAllConstractGoodsReceiptRecords(strHQL);
            ConstractGoodsReceiptRecord constractGoodsReceiptRecord = (ConstractGoodsReceiptRecord)lst[0];

            TB_ReceiptGoodsCode.Text = constractGoodsReceiptRecord.GoodsCode;
            TB_ReceiptGoodsName.Text = constractGoodsReceiptRecord.GoodsName;
            TB_ReceiptGoodsModelNumber.Text = constractGoodsReceiptRecord.ModelNumber;
            TB_ReceiptGoodsSpec.Text = constractGoodsReceiptRecord.Spec;
            LB_ReceiptGoodsType.Text = constractGoodsReceiptRecord.Type;
            LB_ReceiptGoodsUnit.Text = constractGoodsReceiptRecord.Unit;
            NB_ReceiptGoodsNumber.Amount = constractGoodsReceiptRecord.Number;
            NB_ReceiptGoodsPrice.Amount = constractGoodsReceiptRecord.Price;
            DLC_ReceiptGoodsTime.Text = constractGoodsReceiptRecord.ReceiptTime.ToString("yyyy-MM-dd");
            TB_ReceiptAddress.Text = constractGoodsReceiptRecord.ReceiptAddress;
        }
    }

    protected void BT_AddReceiptGoodsRecord_Click(object sender, EventArgs e)
    {
        string strRecordID, strConstractCode, strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strStatus;
        string strUnitName;
        decimal decNumber;
        DateTime dtReceiptTime;
        decimal dePrice;


        strConstractCode = LB_ConstractCode.Text.Trim();
        strType = LB_ReceiptGoodsType.Text.Trim();
        strGoodsCode = TB_ReceiptGoodsCode.Text.Trim();
        strGoodsName = TB_ReceiptGoodsName.Text.Trim();
        strUnitName = LB_ReceiptGoodsUnit.Text.Trim();
        strModelNumber = TB_ReceiptGoodsModelNumber.Text.Trim();
        decNumber = NB_ReceiptGoodsNumber.Amount;
        strSpec = TB_ReceiptGoodsSpec.Text.Trim();
        dePrice = NB_ReceiptGoodsPrice.Amount;
        dtReceiptTime = DateTime.Parse(DLC_ReceiptGoodsTime.Text);


        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim()+"')", true);
        }
        else
        {
            ConstractGoodsReceiptRecordBLL constractGoodsReceiptRecordBLL = new ConstractGoodsReceiptRecordBLL();
            ConstractGoodsReceiptRecord constractGoodsReceiptRecord = new ConstractGoodsReceiptRecord();

            constractGoodsReceiptRecord.ReceiptPlanID = int.Parse(strReceiptPlanID);
            constractGoodsReceiptRecord.Type = strType;
            constractGoodsReceiptRecord.GoodsCode = strGoodsCode;
            constractGoodsReceiptRecord.GoodsName = strGoodsName;

            constractGoodsReceiptRecord.ModelNumber = strModelNumber;
            constractGoodsReceiptRecord.Spec = strSpec;

            constractGoodsReceiptRecord.Number = decNumber;
            constractGoodsReceiptRecord.Unit = strUnitName;
            constractGoodsReceiptRecord.Number = decNumber;
            constractGoodsReceiptRecord.Price = dePrice;
            constractGoodsReceiptRecord.Amount = decNumber * dePrice;

            constractGoodsReceiptRecord.ReceiptTime = dtReceiptTime;

            constractGoodsReceiptRecord.ReceiptAddress = TB_ReceiptAddress.Text;

            try
            {
                constractGoodsReceiptRecordBLL.AddConstractGoodsReceiptRecord(constractGoodsReceiptRecord);

                strRecordID = ShareClass.GetMyCreatedMaxConstractGoodsReceiptRecordID(strReceiptPlanID);
                LB_ID.Text = strRecordID;


                LoadConstractGoodsReceiptRecordList(strReceiptPlanID);

                CountGoodsReceiptNumber(strReceiptPlanID);


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXJCCJC").ToString().Trim()+"')", true);
            }
        }
    }


    protected void BT_UpdateReceiptGoodsRecord_Click(object sender, EventArgs e)
    {
        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strStatus;
        string strUnitName;

        DateTime dtReceiptTime;
        decimal dePrice, deNumber;

        string strID, strConstractCode;
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text;

        strID = LB_ID.Text.Trim();
        strConstractCode = LB_ConstractCode.Text.Trim();
        strType = LB_ReceiptGoodsType.Text.Trim();
        strGoodsCode = TB_ReceiptGoodsCode.Text.Trim();
        strGoodsName = TB_ReceiptGoodsName.Text.Trim();
        strUnitName = LB_ReceiptGoodsUnit.Text.Trim();
        strModelNumber = TB_ReceiptGoodsModelNumber.Text.Trim();
        deNumber = NB_ReceiptGoodsNumber.Amount;
        strSpec = TB_ReceiptGoodsSpec.Text.Trim();
        dePrice = NB_ReceiptGoodsPrice.Amount;
        dtReceiptTime = DateTime.Parse(DLC_ReceiptGoodsTime.Text);


        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim()+"')", true);
        }
        else
        {
            ConstractGoodsReceiptRecordBLL constractGoodsReceiptRecordBLL = new ConstractGoodsReceiptRecordBLL();
            strHQL = "from ConstractGoodsReceiptRecord as constractGoodsReceiptRecord where constractGoodsReceiptRecord.ID = " + strID;
            lst = constractGoodsReceiptRecordBLL.GetAllConstractGoodsReceiptRecords(strHQL);
            ConstractGoodsReceiptRecord constractGoodsReceiptRecord = (ConstractGoodsReceiptRecord)lst[0];

            constractGoodsReceiptRecord.ReceiptPlanID = int.Parse(strReceiptPlanID);
            constractGoodsReceiptRecord.Type = strType;
            constractGoodsReceiptRecord.GoodsCode = strGoodsCode;
            constractGoodsReceiptRecord.GoodsName = strGoodsName;
            constractGoodsReceiptRecord.ModelNumber = strModelNumber;
            constractGoodsReceiptRecord.Spec = strSpec;
            constractGoodsReceiptRecord.Number = deNumber;
            constractGoodsReceiptRecord.Unit = strUnitName;
            constractGoodsReceiptRecord.Price = dePrice;

            constractGoodsReceiptRecord.Amount = deNumber * dePrice;

            constractGoodsReceiptRecord.ReceiptTime = dtReceiptTime;

            constractGoodsReceiptRecord.ReceiptAddress = TB_ReceiptAddress.Text;

            try
            {
                constractGoodsReceiptRecordBLL.UpdateConstractGoodsReceiptRecord(constractGoodsReceiptRecord, int.Parse(strID));

                LoadConstractGoodsReceiptRecordList(strReceiptPlanID);

                CountGoodsReceiptNumber(strReceiptPlanID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_DeleteReceiptGoodsRecord_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strConstractCode = LB_ConstractCode.Text.Trim();

        string strHQL;
        IList lst;

        ConstractGoodsReceiptRecordBLL constractGoodsReceiptRecordBLL = new ConstractGoodsReceiptRecordBLL();
        strHQL = "from ConstractGoodsReceiptRecord as constractGoodsReceiptRecord where constractGoodsReceiptRecord.ID = " + strID;
        lst = constractGoodsReceiptRecordBLL.GetAllConstractGoodsReceiptRecords(strHQL);
        ConstractGoodsReceiptRecord constractGoodsReceiptRecord = (ConstractGoodsReceiptRecord)lst[0];

        try
        {
            constractGoodsReceiptRecordBLL.DeleteConstractGoodsReceiptRecord(constractGoodsReceiptRecord);

            LoadConstractGoodsReceiptRecordList(strReceiptPlanID);

            CountGoodsReceiptNumber(strReceiptPlanID);


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void LoadConstractGoodsReceiptRecordList(string strReceiptPlanID)
    {
        string strHQL;
        IList lst;


        strHQL = "Select * From T_ConstractGoodsReceiptRecord Where ReceiptPlanID = " + strReceiptPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractGoodsReceiptRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void CountGoodsReceiptNumber(string strReceiptPlanID)
    {
        string strHQL;
        IList lst;

        decimal deReceiverNumber = 0;

        strHQL = "from ConstractGoodsReceiptRecord as constractGoodsReceiptRecord where constractGoodsReceiptRecord.ReceiptPlanID = " + strReceiptPlanID;
        ConstractGoodsReceiptRecordBLL constractGoodsReceiptRecordBLL = new ConstractGoodsReceiptRecordBLL();
        lst = constractGoodsReceiptRecordBLL.GetAllConstractGoodsReceiptRecords(strHQL);

        ConstractGoodsReceiptRecord constractGoodsReceiptRecord = new ConstractGoodsReceiptRecord();

        for (int i = 0; i < lst.Count; i++)
        {
            constractGoodsReceiptRecord = (ConstractGoodsReceiptRecord)lst[i];

            deReceiverNumber += constractGoodsReceiptRecord.Number;
        }

        LB_ReceivablesNumber.Text = deReceivablesNumber.ToString();
        LB_ReceiverNubmer.Text = deReceiverNumber.ToString();
        LB_UNReceiverNumber.Text = (deReceivablesNumber - deReceiverNumber).ToString();

        strHQL = "Update T_ConstractGoodsReceiptPlan Set ReceiptedNumber = " + deReceiverNumber.ToString() + " where ID =" + strReceiptPlanID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_ConstractGoodsReceiptPlan Set UNReceiptedNumber = " + (deReceivablesNumber - deReceiverNumber).ToString() + " where ID =" + strReceiptPlanID;
        ShareClass.RunSqlCommand(strHQL);
    }
}

