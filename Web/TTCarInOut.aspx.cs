using System;
using System.Resources;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTCarInOut : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","车辆出入管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        Timer1.Interval = 300000;

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            LoadCarAssignFormByGuard(strUserCode);
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        LoadCarAssignFormByGuard(strUserCode);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        string strStatus;

        if (e.CommandName != "Page")
        {
            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from CarAssignForm as carAssignForm Where carAssignForm.ID = " + strID;
            CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
            lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

            DataList1.DataSource = lst;
            DataList1.DataBind();

            CarAssignForm carAssignForm = (CarAssignForm)lst[0];
            strStatus = carAssignForm.Status.Trim();

            if (strStatus == "Closed" | strStatus == "Cancel")
            {
                BT_CarIn.Enabled = false;
                BT_CarOut.Enabled = false;
            }
            else
            {
                BT_CarIn.Enabled = true;
                BT_CarOut.Enabled = true;
            }

            if (strStatus == "")
            {
                BT_CarIn.Enabled = false;
            }

            if (strStatus == "Departure")
            {
                BT_CarOut.Enabled = false;
                BT_CarIn.Enabled = true;
            }

            if (strStatus == "Return")
            {
                BT_CarOut.Enabled = false;
                BT_CarIn.Enabled = false;
            }

            LB_ID.Text = strID;
        }
    }

    protected void BT_CarOut_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        int j = 0;

        string strID, strStatus;

        strID = LB_ID.Text.Trim();


        strHQL = "from CarAssignForm as carAssignForm Where carAssignForm.ID = " + strID;
        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

        CarAssignForm carAssignForm = (CarAssignForm)lst[0];

        strStatus = carAssignForm.Status.Trim();

        carAssignForm.RealDepartTime = DateTime.Now;
        carAssignForm.Status = "Departure";

        try
        {
            carAssignFormBLL.UpdateCarAssignForm(carAssignForm, int.Parse(strID));

            BT_CarIn.Enabled = true;
            BT_CarOut.Enabled = false;

            LoadCarAssignForm(strID);
            LoadCarAssignFormByGuard(strUserCode);

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                if (strID == ((Button)DataGrid1.Items[i].FindControl("BT_ID")).Text.Trim())
                {
                    DataGrid1.Items[i].ForeColor = Color.Red;
                }
                else
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click2", "alert('"+LanguageHandle.GetWord("ZZCCCG").ToString().Trim()+"')", true);
        }
        catch
        {
        }
    }

    protected void BT_CarIn_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strStatus;
        int j = 0;

        strID = LB_ID.Text.Trim();

        strHQL = "from CarAssignForm as carAssignForm Where carAssignForm.ID = " + strID;
        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

        CarAssignForm carAssignForm = (CarAssignForm)lst[0];

        strStatus = carAssignForm.Status.Trim();

        carAssignForm.RealBackTime = DateTime.Now;
        carAssignForm.Status = "Return";

        try
        {
            carAssignFormBLL.UpdateCarAssignForm(carAssignForm, int.Parse(strID));

            BT_CarOut.Enabled = false;
            BT_CarIn.Enabled = false;

            LoadCarAssignForm(strID);
            LoadCarAssignFormByGuard(strUserCode);

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                if (strID == ((Button)DataGrid1.Items[i].FindControl("BT_ID")).Text.Trim())
                {
                    DataGrid1.Items[i].ForeColor = Color.Red;
                }
                else
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click2", "alert('"+LanguageHandle.GetWord("ZZSHOUCHECG").ToString().Trim() + "')", true);
        }
        catch
        {
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = "From CarAssignForm as carAssignForm Where carAssignForm.GuardCode = " + "'" + strUserCode + "'" + " And carAssignForm.Status<>'Cancel' And carAssignForm.Status<>'Closed' Order By carAssignForm.DepartTime DESC";//过滤掉 取消或关闭 状态 Liujp 2013-07-16

        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        IList lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadCarAssignForm(string strID)
    {
        string strHQL;
        IList lst;

        strHQL = "from CarAssignForm as carAssignForm Where carAssignForm.ID = " + strID;
        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

        DataList1.DataSource = lst;
        DataList1.DataBind();
    }

    protected void LoadCarAssignFormByGuard(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From CarAssignForm as carAssignForm Where carAssignForm.GuardCode = " + "'" + strUserCode + "'" + " And carAssignForm.Status<>'Cancel' And carAssignForm.Status<>'Closed' Order By carAssignForm.DepartTime DESC";//过滤掉 取消或关闭 状态 Liujp 2013-07-16
        CarAssignFormBLL carAssignFormBLL = new CarAssignFormBLL();
        lst = carAssignFormBLL.GetAllCarAssignForms(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }
}
