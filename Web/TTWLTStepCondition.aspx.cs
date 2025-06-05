using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTWLTStepConditon : System.Web.UI.Page
{

    string strUserCode, strMakeUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strStepID, strStepName;

        strStepID = Request.QueryString["StepID"].Trim();
        strStepName = GetStepName(strStepID);

        strMakeUserCode = GetMakeUserCode(strStepID);

        strUserCode = Session["UserCode"].ToString();

        LB_UserCode.Text = strUserCode;

        LB_StepID.Text = strStepID;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LoadStepCondition(strStepID);

            if (strUserCode == strMakeUserCode | Session["SuperWFAdmin"].ToString () == "YES")
            {
                BT_Create.Enabled = true;
                BT_CreateExpress.Enabled = true;

                LBT_New.Enabled = true;
                LBT_NewExpress.Enabled = true;
            }
            else
            {
                BT_Create.Enabled = false;
                BT_CreateExpress.Enabled = false;

                LBT_New.Enabled = false;
                LBT_NewExpress.Enabled = false;
            }

            HL_XMLFile.NavigateUrl = ShareClass.GetWorkFlowLastestXMLFile(GetTemName(strStepID));
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID, strStepID, strHQL;
            IList lst;
           
            strStepID = LB_StepID.Text.Trim();

            strID = e.Item.Cells[3].Text;

            if (e.CommandName == "Update" | e.CommandName == "Detail")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from WLTStepCondition as wlTStepCondition where wlTStepCondition.ConID = " + strID;
                WLTStepConditionBLL wlTStepConditionBLL = new WLTStepConditionBLL();
                lst = wlTStepConditionBLL.GetAllWLTStepConditions(strHQL);

                WLTStepCondition wlTStepCondition = (WLTStepCondition)lst[0];

                LB_ConID.Text = wlTStepCondition.ConID.ToString();
                TB_NextStepID.Amount = wlTStepCondition.NextSortNumber;
                TB_ConDetail.Text = wlTStepCondition.ConDetail.Trim();

                LB_ConIDExpression.Text = strID;

                LoadStepConditionExpression(strID);

                if (strUserCode == strMakeUserCode)
                {
                    //BT_Add.Enabled = true;
                    //BT_Update.Enabled = true;
                    //BT_Delete.Enabled = true;

                    //BT_AddExpression.Enabled = true;
                    //BT_UpdateExpression.Enabled = false;
                    //BT_DeleteExpression.Enabled = false;
                }
                else
                {
                    //BT_Add.Enabled = false;
                    //BT_Update.Enabled = false;
                    //BT_Delete.Enabled = false;

                    //BT_AddExpression.Enabled = false;
                    //BT_UpdateExpression.Enabled = false;
                    //BT_DeleteExpression.Enabled = false;
                }

                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }

                if (e.CommandName == "Detail")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popExpressWindow','true') ", true);
                }
            }

            if (e.CommandName == "Delete")
            {
                if (strUserCode != strMakeUserCode & Session["SuperWFAdmin"].ToString() != "YES")
                {
                    return;
                }

                WLTStepConditionBLL wlTStepConditionBLL = new WLTStepConditionBLL();
                strHQL = "from WLTStepCondition as wlTStepCondition where wlTStepCondition.ConID = " + strID;
                lst = wlTStepConditionBLL.GetAllWLTStepConditions(strHQL);

                WLTStepCondition wlTStepCondition = (WLTStepCondition)lst[0];

                try
                {
                    wlTStepConditionBLL.DeleteWLTStepCondition(wlTStepCondition);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                    LoadStepCondition(strStepID);
                    LoadStepConditionExpression("0");

                    //BT_Update.Enabled = false;
                    //BT_Delete.Enabled = false;

                    //BT_AddExpression.Enabled = false;
                    //BT_UpdateExpression.Enabled = false;
                    //BT_DeleteExpression.Enabled = false;
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ConID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strConID;

        strConID = LB_ConID.Text;

        if (strConID == "")
        {
            AddCondition();
        }
        else
        {
            UpdateCondition();
        }
    }

    protected void AddCondition()
    {
        string strConID, strStepID, strStepCondition, strTemName;
        int intNextStepID;

        strStepID = LB_StepID.Text.Trim();
        intNextStepID = int.Parse(TB_NextStepID.Amount.ToString());
        strStepCondition = TB_ConDetail.Text.Trim();

        strTemName = GetTemName(strStepID);

        if (strStepCondition != "")
        {
            WLTStepConditionBLL wlTStepConditionBLL = new WLTStepConditionBLL();
            WLTStepCondition wlTStepCondition = new WLTStepCondition();

            wlTStepCondition.StepID = int.Parse(strStepID);
            wlTStepCondition.NextSortNumber = intNextStepID;
            wlTStepCondition.ConDetail = strStepCondition;
            wlTStepCondition.TemName = strTemName;
            wlTStepCondition.GUID = DateTime.Now.ToString("yyyyMMddHHMMSS");

            try
            {
                wlTStepConditionBLL.AddWLTStepCondition(wlTStepCondition);
                strConID = ShareClass.GetMyCreatedMaxWorkFlowTStepConditionID();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
                LoadStepCondition(strStepID);
                LoadStepConditionExpression("0");

                LB_ConID.Text = strConID;

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;

                //BT_AddExpression.Enabled = true;
                //BT_UpdateExpression.Enabled = false;
                //BT_DeleteExpression.Enabled = false;

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDXMSLMBZTJBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateCondition()
    {
        string strStepID, strConID, strStepCondition;
        int intNextStepID;
        string strHQL;
        IList lst;

        strConID = LB_ConID.Text.Trim();
        strStepID = LB_StepID.Text.Trim();
        intNextStepID = int.Parse(TB_NextStepID.Amount.ToString());
        strStepCondition = TB_ConDetail.Text.Trim();

        if (strStepCondition != "")
        {
            WLTStepConditionBLL wlTStepConditionBLL = new WLTStepConditionBLL();
            strHQL = "from WLTStepCondition as wlTStepCondition where wlTStepCondition.ConID = " + strConID;
            lst = wlTStepConditionBLL.GetAllWLTStepConditions(strHQL);

            WLTStepCondition wlTStepCondition = (WLTStepCondition)lst[0];
            wlTStepCondition.NextSortNumber = intNextStepID;
            wlTStepCondition.ConDetail = strStepCondition;

            try
            {
                wlTStepConditionBLL.UpdateWLTStepCondition(wlTStepCondition, int.Parse(strConID));
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
                LoadStepCondition(strStepID);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDXMSLMBZTJBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID, strHQL;
            IList lst;

            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                strHQL = "from WLTStepConditionExpression as wlTStepConditionExpression where wlTStepConditionExpression.ID = " + strID;
                WLTStepConditionExpressionBLL wlTStepConditionExpressionBLL = new WLTStepConditionExpressionBLL();
                lst = wlTStepConditionExpressionBLL.GetAllWLTStepConditionExpressions(strHQL);

                WLTStepConditionExpression wlTStepConditionExpression = (WLTStepConditionExpression)lst[0];

                LB_ID.Text = wlTStepConditionExpression.ID.ToString();
                TB_Expression.Text = wlTStepConditionExpression.Expression.Trim();
                DL_LogicalExpression.SelectedValue = wlTStepConditionExpression.LogicalOperator.Trim();

                if (strUserCode == strMakeUserCode)
                {
                    //BT_AddExpression.Enabled = true;
                    //BT_UpdateExpression.Enabled = true;
                    //BT_DeleteExpression.Enabled = true;
                }
                else
                {
                    //BT_AddExpression.Enabled = false;
                    //BT_UpdateExpression.Enabled = false;
                    //BT_DeleteExpression.Enabled = false;
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popExpressWindow','true','popExpressDetailWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strConID, strExpression, strLogicalExpression;

                if (strUserCode != strMakeUserCode & Session["SuperWFAdmin"].ToString() != "YES")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popExpressWindow','true') ", true);
                    return;
                }

                strConID = LB_ConID.Text.Trim();
                strExpression = TB_Expression.Text.Trim();
                strLogicalExpression = DL_LogicalExpression.SelectedValue.Trim();

                strHQL = "from WLTStepConditionExpression as wlTStepConditionExpression where wlTStepConditionExpression.ID = " + strID;
                WLTStepConditionExpressionBLL wlTStepConditionExpressionBLL = new WLTStepConditionExpressionBLL();
                lst = wlTStepConditionExpressionBLL.GetAllWLTStepConditionExpressions(strHQL);

                WLTStepConditionExpression wlTStepConditionExpression = (WLTStepConditionExpression)lst[0];

                try
                {
                    wlTStepConditionExpressionBLL.DeleteWLTStepConditionExpression(wlTStepConditionExpression);

                    //BT_UpdateExpression.Enabled = false;
                    //BT_DeleteExpression.Enabled = false;

                    LoadStepConditionExpression(strConID);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popExpressWindow','true') ", true);
            }
        }
    }

    protected void BT_CreateExpress_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popExpressWindow','false','popExpressDetailWindow') ", true);
    }

    protected void BT_NewExpress_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text;

        if (strID == "")
        {
            AddConditionExpress();
        }
        else
        {
            UpdateConditionExpress();
        }
    }

    protected void AddConditionExpress()
    {
        string strID, strConID, strExpression, strLogicalExpression;

        strConID = LB_ConID.Text.Trim();
        strExpression = TB_Expression.Text.Trim();
        strLogicalExpression = DL_LogicalExpression.SelectedValue.Trim();

        if (strExpression != "")
        {
            WLTStepConditionExpressionBLL wlTStepConditionExpressionBLL = new WLTStepConditionExpressionBLL();
            WLTStepConditionExpression wlTStepConditionExpression = new WLTStepConditionExpression();

            wlTStepConditionExpression.ConID = int.Parse(strConID);
            wlTStepConditionExpression.Expression = strExpression;
            wlTStepConditionExpression.LogicalOperator = strLogicalExpression;

            try
            {
                wlTStepConditionExpressionBLL.AddWLTStepConditionExpression(wlTStepConditionExpression);

                strID = ShareClass.GetMyCreatedMaxWorkFlowTStepConditionExpressionID();

                LB_ID.Text = strID;


                //BT_UpdateExpression.Enabled = true;
                //BT_DeleteExpression.Enabled = true;

                LoadStepConditionExpression(strConID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popExpressWindow','true') ", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popExpressWindow','true','popExpressDetailWindow') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBDSBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popExpressWindow','true','popExpressDetailWindow') ", true);
        }
    }

    protected void UpdateConditionExpress()
    {
        string strHQL;
        IList lst;

        string strID, strConID, strExpression, strLogicalExpression;

        strID = LB_ID.Text.Trim();
        strConID = LB_ConID.Text.Trim();
        strExpression = TB_Expression.Text.Trim();
        strLogicalExpression = DL_LogicalExpression.SelectedValue.Trim();

        if (strExpression != "")
        {
            strHQL = "from WLTStepConditionExpression as wlTStepConditionExpression where wlTStepConditionExpression.ID = " + strID;
            WLTStepConditionExpressionBLL wlTStepConditionExpressionBLL = new WLTStepConditionExpressionBLL();
            lst = wlTStepConditionExpressionBLL.GetAllWLTStepConditionExpressions(strHQL);

            WLTStepConditionExpression wlTStepConditionExpression = (WLTStepConditionExpression)lst[0];

            wlTStepConditionExpression.ConID = int.Parse(strConID);
            wlTStepConditionExpression.Expression = strExpression;
            wlTStepConditionExpression.LogicalOperator = strLogicalExpression;

            try
            {
                wlTStepConditionExpressionBLL.UpdateWLTStepConditionExpression(wlTStepConditionExpression, int.Parse(strID));
                LoadStepConditionExpression(strConID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popExpressWindow','true') ", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popExpressWindow','true','popExpressDetailWindow') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBDSBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popExpressWindow','true','popExpressDetailWindow') ", true);
        }
    }


    protected void LBT_CloseExpressDetailWindow_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popExpressWindow','true') ", true);
    }


    protected void LoadStepCondition(string strStepID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WLTStepCondition as wlTStepCondition where wlTStepCondition.StepID = " + strStepID;
        WLTStepConditionBLL wlTStepConditionBLL = new WLTStepConditionBLL();
        lst = wlTStepConditionBLL.GetAllWLTStepConditions(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadStepConditionExpression(string strConID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WLTStepConditionExpression as wlTStepConditionExpression where wlTStepConditionExpression.ConID = " + strConID;
        WLTStepConditionExpressionBLL wlTStepConditionExpressionBLL = new WLTStepConditionExpressionBLL();
        lst = wlTStepConditionExpressionBLL.GetAllWLTStepConditionExpressions(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected string GetStepName(string strStepID)
    {
        string strHQL, strStepName;
        IList lst;

        strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.StepID = " + strStepID + " order by workFlowTStep.StepID ASC";
        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
        lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

        WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

        strStepName = workFlowTStep.StepName.Trim();

        return strStepName;
    }

    protected string GetTemName(string strStepID)
    {
        string strHQL, strTemName;
        IList lst;

        strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.StepID = " + strStepID + " order by workFlowTStep.StepID ASC";
        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
        lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

        WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

        strTemName = workFlowTStep.TemName.Trim();

        return strTemName;
    }

    protected string GetMakeUserCode(string strStepID)
    {
        string strHQL, strMakeUserCode, strTemName;
        IList lst;

        strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.StepID = " + strStepID;
        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
        lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);
        WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

        strTemName = workFlowTStep.TemName.Trim();

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        strMakeUserCode = workFlowTemplate.CreatorCode.Trim();

        return strMakeUserCode;
    }

}
