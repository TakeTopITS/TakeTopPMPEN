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

public partial class TTConstractGoodsDeliveryRecordView : System.Web.UI.Page
{
    string strDeliveryPlanID, strConstractCode;
    decimal deReceivablesNumber = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        strDeliveryPlanID = Request.QueryString["DeliveryPlanID"];

        strHQL = "from ConstractGoodsDeliveryPlan as constractGoodsDeliveryPlan where constractGoodsDeliveryPlan.ID = " + strDeliveryPlanID;
        ConstractGoodsDeliveryPlanBLL constractGoodsDeliveryPlanBLL = new ConstractGoodsDeliveryPlanBLL();
        lst = constractGoodsDeliveryPlanBLL.GetAllConstractGoodsDeliveryPlans(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        ConstractGoodsDeliveryPlan constractGoodsDeliveryPlan = (ConstractGoodsDeliveryPlan)lst[0];
        strConstractCode = constractGoodsDeliveryPlan.ConstractCode.Trim();
        deReceivablesNumber = constractGoodsDeliveryPlan.Number;

        //this.Title = "收货计划：" + strDeliveryPlanID + "执行情况";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LB_DeliveryGoodsType.Text = constractGoodsDeliveryPlan.Type.Trim();
            TB_DeliveryGoodsCode.Text = constractGoodsDeliveryPlan.GoodsCode;
            TB_DeliveryGoodsName.Text = constractGoodsDeliveryPlan.GoodsName;
            TB_DeliveryGoodsModelNumber.Text = constractGoodsDeliveryPlan.ModelNumber;
            TB_DeliveryGoodsSpec.Text = constractGoodsDeliveryPlan.Spec;
            LB_DeliveryGoodsType.Text = constractGoodsDeliveryPlan.Type;
            LB_DeliveryGoodsUnit.Text = constractGoodsDeliveryPlan.Unit;
            NB_DeliveryGoodsNumber.Amount = constractGoodsDeliveryPlan.Number;
            NB_DeliveryGoodsPrice.Amount = constractGoodsDeliveryPlan.Price;

            DLC_DeliveryGoodsTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_DeliveryAddress.Text = constractGoodsDeliveryPlan.DeliveryAddress;

            LoadConstractGoodsDeliveryRecordList(strDeliveryPlanID);

            CountGoodsDeliveryNumber(strDeliveryPlanID);
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

            strHQL = "from ConstractGoodsDeliveryRecord as constractGoodsDeliveryRecord Where constractGoodsDeliveryRecord.DeliveryPlanID = " + "'" + strDeliveryPlanID + "'";

            ConstractGoodsDeliveryRecordBLL constractGoodsDeliveryRecordBLL = new ConstractGoodsDeliveryRecordBLL();
            lst = constractGoodsDeliveryRecordBLL.GetAllConstractGoodsDeliveryRecords(strHQL);
            ConstractGoodsDeliveryRecord constractGoodsDeliveryRecord = (ConstractGoodsDeliveryRecord)lst[0];

            TB_DeliveryGoodsCode.Text = constractGoodsDeliveryRecord.GoodsCode;
            TB_DeliveryGoodsName.Text = constractGoodsDeliveryRecord.GoodsName;
            TB_DeliveryGoodsModelNumber.Text = constractGoodsDeliveryRecord.ModelNumber;
            TB_DeliveryGoodsSpec.Text = constractGoodsDeliveryRecord.Spec;
            LB_DeliveryGoodsType.Text = constractGoodsDeliveryRecord.Type;
            LB_DeliveryGoodsUnit.Text = constractGoodsDeliveryRecord.Unit;
            NB_DeliveryGoodsNumber.Amount = constractGoodsDeliveryRecord.Number;
            NB_DeliveryGoodsPrice.Amount = constractGoodsDeliveryRecord.Price;
            DLC_DeliveryGoodsTime.Text = constractGoodsDeliveryRecord.DeliveryTime.ToString("yyyy-MM-dd");
            TB_DeliveryAddress.Text = constractGoodsDeliveryRecord.DeliveryAddress;
        }
    }

    protected void BT_AddDeliveryGoodsRecord_Click(object sender, EventArgs e)
    {
        string strRecordID, strConstractCode, strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strStatus;
        string strUnitName;
        decimal decNumber;
        DateTime dtDeliveryTime;
        decimal dePrice;


        strConstractCode = LB_ConstractCode.Text.Trim();
        strType = LB_DeliveryGoodsType.Text.Trim();
        strGoodsCode = TB_DeliveryGoodsCode.Text.Trim();
        strGoodsName = TB_DeliveryGoodsName.Text.Trim();
        strUnitName = LB_DeliveryGoodsUnit.Text.Trim();
        strModelNumber = TB_DeliveryGoodsModelNumber.Text.Trim();
        decNumber = NB_DeliveryGoodsNumber.Amount;
        strSpec = TB_DeliveryGoodsSpec.Text.Trim();
        dePrice = NB_DeliveryGoodsPrice.Amount;
        dtDeliveryTime = DateTime.Parse(DLC_DeliveryGoodsTime.Text);


        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim()+"')", true);
        }
        else
        {
            ConstractGoodsDeliveryRecordBLL constractGoodsDeliveryRecordBLL = new ConstractGoodsDeliveryRecordBLL();
            ConstractGoodsDeliveryRecord constractGoodsDeliveryRecord = new ConstractGoodsDeliveryRecord();

            constractGoodsDeliveryRecord.DeliveryPlanID = int.Parse(strDeliveryPlanID);
            constractGoodsDeliveryRecord.Type = strType;
            constractGoodsDeliveryRecord.GoodsCode = strGoodsCode;
            constractGoodsDeliveryRecord.GoodsName = strGoodsName;

            constractGoodsDeliveryRecord.ModelNumber = strModelNumber;
            constractGoodsDeliveryRecord.Spec = strSpec;

            constractGoodsDeliveryRecord.Number = decNumber;
            constractGoodsDeliveryRecord.Unit = strUnitName;
            constractGoodsDeliveryRecord.Number = decNumber;
            constractGoodsDeliveryRecord.Price = dePrice;
            constractGoodsDeliveryRecord.Amount = decNumber * dePrice;

            constractGoodsDeliveryRecord.DeliveryTime = dtDeliveryTime;

            constractGoodsDeliveryRecord.DeliveryAddress = TB_DeliveryAddress.Text;


            try
            {
                constractGoodsDeliveryRecordBLL.AddConstractGoodsDeliveryRecord(constractGoodsDeliveryRecord);

                strRecordID = ShareClass.GetMyCreatedMaxConstractGoodsDeliveryRecordID(strDeliveryPlanID);
                LB_ID.Text = strRecordID;

                LoadConstractGoodsDeliveryRecordList(strDeliveryPlanID);

                CountGoodsDeliveryNumber(strDeliveryPlanID);


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXJCCJC").ToString().Trim()+"')", true);
            }
        }
    }


    protected void BT_UpdateDeliveryGoodsRecord_Click(object sender, EventArgs e)
    {
        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strStatus;
        string strUnitName;

        DateTime dtDeliveryTime;
        decimal dePrice, deNumber;

        string strID, strConstractCode;
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text;

        strID = LB_ID.Text.Trim();
        strConstractCode = LB_ConstractCode.Text.Trim();
        strType = LB_DeliveryGoodsType.Text.Trim();
        strGoodsCode = TB_DeliveryGoodsCode.Text.Trim();
        strGoodsName = TB_DeliveryGoodsName.Text.Trim();
        strUnitName = LB_DeliveryGoodsUnit.Text.Trim();
        strModelNumber = TB_DeliveryGoodsModelNumber.Text.Trim();
        deNumber = NB_DeliveryGoodsNumber.Amount;
        strSpec = TB_DeliveryGoodsSpec.Text.Trim();
        dePrice = NB_DeliveryGoodsPrice.Amount;
        dtDeliveryTime = DateTime.Parse(DLC_DeliveryGoodsTime.Text);


        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim()+"')", true);
        }
        else
        {
            ConstractGoodsDeliveryRecordBLL constractGoodsDeliveryRecordBLL = new ConstractGoodsDeliveryRecordBLL();
            strHQL = "from ConstractGoodsDeliveryRecord as constractGoodsDeliveryRecord where constractGoodsDeliveryRecord.ID = " + strID;
            lst = constractGoodsDeliveryRecordBLL.GetAllConstractGoodsDeliveryRecords(strHQL);
            ConstractGoodsDeliveryRecord constractGoodsDeliveryRecord = (ConstractGoodsDeliveryRecord)lst[0];

            constractGoodsDeliveryRecord.DeliveryPlanID = int.Parse(strDeliveryPlanID);
            constractGoodsDeliveryRecord.Type = strType;
            constractGoodsDeliveryRecord.GoodsCode = strGoodsCode;
            constractGoodsDeliveryRecord.GoodsName = strGoodsName;
            constractGoodsDeliveryRecord.ModelNumber = strModelNumber;
            constractGoodsDeliveryRecord.Spec = strSpec;
            constractGoodsDeliveryRecord.Number = deNumber;
            constractGoodsDeliveryRecord.Unit = strUnitName;
            constractGoodsDeliveryRecord.Price = dePrice;

            constractGoodsDeliveryRecord.Amount = deNumber * dePrice;
            constractGoodsDeliveryRecord.DeliveryTime = dtDeliveryTime;
            constractGoodsDeliveryRecord.DeliveryAddress = TB_DeliveryAddress.Text;

            try
            {
                constractGoodsDeliveryRecordBLL.UpdateConstractGoodsDeliveryRecord(constractGoodsDeliveryRecord, int.Parse(strID));

                LoadConstractGoodsDeliveryRecordList(strDeliveryPlanID);

                CountGoodsDeliveryNumber(strDeliveryPlanID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_DeleteDeliveryGoodsRecord_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strConstractCode = LB_ConstractCode.Text.Trim();

        string strHQL;
        IList lst;

        ConstractGoodsDeliveryRecordBLL constractGoodsDeliveryRecordBLL = new ConstractGoodsDeliveryRecordBLL();
        strHQL = "from ConstractGoodsDeliveryRecord as constractGoodsDeliveryRecord where constractGoodsDeliveryRecord.ID = " + strID;
        lst = constractGoodsDeliveryRecordBLL.GetAllConstractGoodsDeliveryRecords(strHQL);
        ConstractGoodsDeliveryRecord constractGoodsDeliveryRecord = (ConstractGoodsDeliveryRecord)lst[0];

        try
        {
            constractGoodsDeliveryRecordBLL.DeleteConstractGoodsDeliveryRecord(constractGoodsDeliveryRecord);

            LoadConstractGoodsDeliveryRecordList(strDeliveryPlanID);

            CountGoodsDeliveryNumber(strDeliveryPlanID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void LoadConstractGoodsDeliveryRecordList(string strDeliveryPlanID)
    {
        string strHQL;
        IList lst;


        strHQL = "Select * From T_ConstractGoodsDeliveryRecord Where DeliveryPlanID = " + strDeliveryPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractGoodsDeliveryRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void CountGoodsDeliveryNumber(string strDeliveryPlanID)
    {
        string strHQL;
        IList lst;

        decimal deReceiverNumber = 0;

        strHQL = "from ConstractGoodsDeliveryRecord as constractGoodsDeliveryRecord where constractGoodsDeliveryRecord.DeliveryPlanID = " + strDeliveryPlanID;
        ConstractGoodsDeliveryRecordBLL constractGoodsDeliveryRecordBLL = new ConstractGoodsDeliveryRecordBLL();
        lst = constractGoodsDeliveryRecordBLL.GetAllConstractGoodsDeliveryRecords(strHQL);

        ConstractGoodsDeliveryRecord constractGoodsDeliveryRecord = new ConstractGoodsDeliveryRecord();

        for (int i = 0; i < lst.Count; i++)
        {
            constractGoodsDeliveryRecord = (ConstractGoodsDeliveryRecord)lst[i];

            deReceiverNumber += constractGoodsDeliveryRecord.Number;
        }

        LB_ReceivablesNumber.Text = deReceivablesNumber.ToString();
        LB_ReceiverNubmer.Text = deReceiverNumber.ToString();
        LB_UNReceiverNumber.Text = (deReceivablesNumber - deReceiverNumber).ToString();

        strHQL = "Update T_ConstractGoodsDeliveryPlan Set DeliveredNumber = " + deReceiverNumber.ToString() + " where ID =" + strDeliveryPlanID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_ConstractGoodsDeliveryPlan Set UNDeliveredNumber = " + (deReceivablesNumber - deReceiverNumber).ToString() + " where ID =" + strDeliveryPlanID;
        ShareClass.RunSqlCommand(strHQL);
    }
}

