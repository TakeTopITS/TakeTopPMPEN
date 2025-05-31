using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDPressureEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataGDProjectBinder();

            DataPressureObjectBinder();

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string strPressureCode = Request.QueryString["id"].ToString();
                HF_PressureCode.Value = strPressureCode;

                BindData(strPressureCode);
            }
        }
    }




    private void DataPressureObjectBinder()
    {
        GDPressureObjectBLL gDPressureObjectBLL = new GDPressureObjectBLL();
        string strGDPressureObjectHQL = "from GDPressureObject as gDPressureObject";
        IList listGDPressureObject = gDPressureObjectBLL.GetAllGDPressureObjects(strGDPressureObjectHQL);

        DDL_PressureMedium.DataSource = listGDPressureObject;
        DDL_PressureMedium.DataTextField = "PressureObject";
        DDL_PressureMedium.DataValueField = "PressureObject";
        DDL_PressureMedium.DataBind();

        DDL_PressureMedium.Items.Insert(0, new ListItem("", ""));
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strPressureCode = TXT_PressureCode.Text.Trim();
            string strOrderNumber = TXT_OrderNumber.Text.Trim();
            string strPublicTime = TXT_PublicTime.Text.Trim();
            string strPressureMedium = DDL_PressureMedium.SelectedValue;
            string strPressureTest = TXT_PressureTest.Text.Trim();
            string strMainArea = TXT_MainArea.Text.Trim();
            string strPointArea = TXT_PointArea.Text.Trim();
            string strPressureUser = TXT_PressureUser.Text.Trim();
            string strSystemCode = TXT_SystemCode.Text.Trim();
            string strMedium = TXT_Medium.Text.Trim();
            string strPipelineCheck = TXT_PipelineCheck.Text.Trim();
            string strHistoryRecord = TXT_HistoryRecord.Text.Trim();
            string strPressureTime = TXT_PressureTime.Text.Trim();
            string strRemarks = TXT_Remarks.Text.Trim();

            string strProjectCode = DDL_GDProject.SelectedValue;

            if (string.IsNullOrEmpty(strProjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZGDXM+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPressureCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYBHBNWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPressureCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYBHBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strOrderNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXLHBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPublicTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFBRBNWKBC+"')", true);
                return;
            }
            //if (!ShareClass.CheckStringRight(strPressureMedium))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYJZBNWFFZF+"')", true);
            //    return;
            //}
            if (!ShareClass.CheckStringRight(strPressureTest))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYLSYBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMainArea))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZYBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPointArea))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFYBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPressureUser))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYTBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strSystemCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXTHBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMedium))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJZBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPipelineCheck))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGXJCBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strHistoryRecord))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZLSJLBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPublicTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYRBNWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRemarks))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBZBNWFFZF+"')", true);
                return;
            }


            GDPressureBLL gDPressureBLL = new GDPressureBLL();


            if (!string.IsNullOrEmpty(HF_PressureCode.Value))
            {
                //ÐÞ¸Ä
                string strWPSNo = HF_PressureCode.Value;

                string strGDPressureSql = "from GDPressure as gDPressure where PressureCode = '" + strWPSNo + "'";
                IList listGDPressure = gDPressureBLL.GetAllGDPressures(strGDPressureSql);
                if (listGDPressure != null && listGDPressure.Count > 0)
                {
                    GDPressure gDPressure = (GDPressure)listGDPressure[0];

                    gDPressure.ProjectCode = strProjectCode;
                    gDPressure.PressureCode = strPressureCode;
                    gDPressure.OrderNumber = strOrderNumber;
                    gDPressure.PublicTime = strPublicTime;
                    gDPressure.PressureMedium = strPressureMedium;
                    gDPressure.PressureTest = strPressureTest;
                    gDPressure.MainArea = strMainArea;
                    gDPressure.PointArea = strPointArea;
                    gDPressure.PressureUser = strPressureUser;
                    gDPressure.SystemCode = strSystemCode;
                    gDPressure.Medium = strMedium;
                    gDPressure.PipelineCheck = strPipelineCheck;
                    gDPressure.HistoryRecord = strHistoryRecord;
                    gDPressure.PressureTime = strPressureTime;
                    gDPressure.Remarks = strRemarks;

                    gDPressureBLL.UpdateGDPressure(gDPressure, strWPSNo);
                }
            }
            else
            {
                //Ôö¼Ó
                GDPressure gDPressure = new GDPressure();
                gDPressure.ProjectCode = strProjectCode;
                gDPressure.PressureCode = strPressureCode;
                gDPressure.PressureCode = strPressureCode;
                gDPressure.OrderNumber = strOrderNumber;
                gDPressure.PublicTime = strPublicTime;
                gDPressure.PressureMedium = strPressureMedium;
                gDPressure.PressureTest = strPressureTest;
                gDPressure.MainArea = strMainArea;
                gDPressure.PointArea = strPointArea;
                gDPressure.PressureUser = strPressureUser;
                gDPressure.SystemCode = strSystemCode;
                gDPressure.Medium = strMedium;
                gDPressure.PipelineCheck = strPipelineCheck;
                gDPressure.HistoryRecord = strHistoryRecord;
                gDPressure.PressureTime = strPressureTime;
                gDPressure.Remarks = strRemarks;

                gDPressure.IsMark = 0;
                gDPressure.UserCode = strUserCode;

                gDPressureBLL.AddGDPressure(gDPressure);
            }

            //Response.Redirect("TTGDPressureList.aspx");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
        catch (Exception ex)
        { }
    }


    private void BindData(string strPressureCode)
    {
        GDPressureBLL gDPressureBLL = new GDPressureBLL();
        string strGDPressureSql = "from GDPressure as gDPressure where PressureCode = '" + strPressureCode + "'";
        IList listGDPressure = gDPressureBLL.GetAllGDPressures(strGDPressureSql);
        if (listGDPressure != null && listGDPressure.Count > 0)
        {
            GDPressure gDPressure = (GDPressure)listGDPressure[0];

            DDL_GDProject.SelectedValue = gDPressure.ProjectCode;

            TXT_PressureCode.Text = gDPressure.PressureCode;
            TXT_OrderNumber.Text = gDPressure.OrderNumber;
            TXT_PublicTime.Text = gDPressure.PublicTime.ToString();
            DDL_PressureMedium.SelectedValue = gDPressure.PressureMedium;
            TXT_PressureTest.Text = gDPressure.PressureTest;
            TXT_MainArea.Text = gDPressure.MainArea;
            TXT_PointArea.Text = gDPressure.PointArea;
            TXT_PressureUser.Text = gDPressure.PressureUser;
            TXT_SystemCode.Text = gDPressure.SystemCode;
            TXT_Medium.Text = gDPressure.Medium;
            TXT_PipelineCheck.Text = gDPressure.PipelineCheck;
            TXT_HistoryRecord.Text = gDPressure.HistoryRecord;
            TXT_PressureTime.Text = gDPressure.PressureTime.ToString();
            TXT_Remarks.Text = gDPressure.Remarks;
        }
    }




    private void DataGDProjectBinder()
    {
        GDProjectBLL gDProjectBLL = new GDProjectBLL();
        string strGDProjectHQL = "from GDProject as gDProject";
        IList listGDProject = gDProjectBLL.GetAllGDProjects(strGDProjectHQL);

        DDL_GDProject.DataSource = listGDProject;
        DDL_GDProject.DataTextField = "ProjectName";
        DDL_GDProject.DataValueField = "ProjectCode";
        DDL_GDProject.DataBind();

        DDL_GDProject.Items.Insert(0, new ListItem("", ""));
    }

}