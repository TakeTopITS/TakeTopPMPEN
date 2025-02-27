using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDRTSampleEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {

            DataGDProjectBinder();

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string id = Request.QueryString["id"].ToString();
                HF_ID.Value = id;
                int intID = 0;
                int.TryParse(id, out intID);

                BindData(intID);

            }

        }
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strProjectCode = DDL_GDProject.SelectedValue;
            string strIsom_no = DDL_Isom_no.SelectedValue;
            string strJoint = TXT_Joint.Text.Trim();
            string strCover = TXT_Cover.Text.Trim();
            string strCoverDate = TXT_CoverDate.Text.Trim();
            string strRTLotNo = TXT_RTLotNo.Text.Trim();
            string strRTSampleNo = TXT_RTSampleNo.Text.Trim();
            string strRTInstrNo = TXT_RTInstrNo.Text.Trim();
            string strRTSampleSerialNo = TXT_RTSampleSerialNo.Text.Trim();
            string strRemark = TXT_Remark.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMBNWKZ").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strIsom_no))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDXTHBNWKZ").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strJoint))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJTBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strCover))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCOVERBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strCoverDate))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCOVERDATEBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRTLotNo))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZRTLOTNOBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRTSampleNo))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZRTSAMPLENOBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRTInstrNo))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZRTINSTRNOBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRTSampleSerialNo))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZRTSAMPLESERIALNOBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRemark))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZREMARKBNWFFZF").ToString().Trim()+"')", true);
                return;
            }


            GDRTSampleBLL gDRTSampleBLL = new GDRTSampleBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDRTSampleSql = "from GDRTSample as gDRTSample where id = " + intID;
                IList listGDRTSample = gDRTSampleBLL.GetAllGDRTSamples(strGDRTSampleSql);
                if (listGDRTSample != null && listGDRTSample.Count > 0)
                {
                    GDRTSample gDRTSample = (GDRTSample)listGDRTSample[0];

                    gDRTSample.ProjectCode = strProjectCode;

                    gDRTSample.Isom_no = strIsom_no;
                    gDRTSample.Joint = strJoint;
                    gDRTSample.Cover = strCover;
                    gDRTSample.CoverDate = strCoverDate;
                    gDRTSample.RTLotNo = strRTLotNo;
                    gDRTSample.RTSampleNo = strRTSampleNo;
                    gDRTSample.RTInstrNo = strRTInstrNo;
                    gDRTSample.RTSampleSerialNo = strRTSampleSerialNo;
                    gDRTSample.Remark = strRemark;

                    gDRTSampleBLL.UpdateGDRTSample(gDRTSample, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDRTSample gDRTSample = new GDRTSample();

                gDRTSample.ProjectCode = strProjectCode;

                gDRTSample.Isom_no = strIsom_no;
                gDRTSample.Joint = strJoint;
                gDRTSample.Cover = strCover;
                gDRTSample.CoverDate = strCoverDate;
                gDRTSample.RTLotNo = strRTLotNo;
                gDRTSample.RTSampleNo = strRTSampleNo;
                gDRTSample.RTInstrNo = strRTInstrNo;
                gDRTSample.RTSampleSerialNo = strRTSampleSerialNo;
                gDRTSample.Remark = strRemark;

                gDRTSample.IsMark = 0;
                gDRTSample.UserCode = strUserCode;

                gDRTSampleBLL.AddGDRTSample(gDRTSample);
            }

            Response.Redirect("TTGDRTSampleList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDRTSampleBLL gDRTSampleBLL = new GDRTSampleBLL();
        string strGDRTSampleSql = "from GDRTSample as gDRTSample where id = " + id;
        IList listGDRTSample = gDRTSampleBLL.GetAllGDRTSamples(strGDRTSampleSql);
        if (listGDRTSample != null && listGDRTSample.Count > 0)
        {
            GDRTSample gDRTSample = (GDRTSample)listGDRTSample[0];

            string strProjectCode = gDRTSample.ProjectCode;
            DDL_GDProject.SelectedValue = strProjectCode;


            string strGDLineWeldHQL = @"select * from T_GDLineWeld where ProjectCode = '" + strProjectCode + "'";
            DataTable dtGDLineWeld = ShareClass.GetDataSetFromSql(strGDLineWeldHQL, "GDLineWeld").Tables[0];

            DDL_Isom_no.DataSource = dtGDLineWeld;
            DDL_Isom_no.DataTextField = "Isom_no";
            DDL_Isom_no.DataValueField = "Isom_no";
            DDL_Isom_no.DataBind();

            DDL_Isom_no.SelectedValue = gDRTSample.Isom_no;
            TXT_Joint.Text = gDRTSample.Joint;
            TXT_Cover.Text = gDRTSample.Cover;
            TXT_CoverDate.Text = gDRTSample.CoverDate;
            TXT_RTLotNo.Text = gDRTSample.RTLotNo;
            TXT_RTSampleNo.Text = gDRTSample.RTSampleNo;
            TXT_RTInstrNo.Text = gDRTSample.RTInstrNo;
            TXT_RTSampleSerialNo.Text = gDRTSample.RTSampleSerialNo;
            TXT_Remark.Text = gDRTSample.Remark;
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

    protected void DDL_GDProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strSelectProject = DDL_GDProject.SelectedValue;

        GDLineWeldBLL gDLineWeldBLL = new GDLineWeldBLL();
        string strGDLineWeldHQL = "from GDLineWeld as gDLineWeld where ProjectCode = '" + strSelectProject + "'";
        IList listGDLineWeld = gDLineWeldBLL.GetAllGDLineWelds(strGDLineWeldHQL);

        DDL_Isom_no.DataSource = listGDLineWeld;
        DDL_Isom_no.DataTextField = "Isom_no";
        DDL_Isom_no.DataValueField = "Isom_no";
        DDL_Isom_no.DataBind();
    }
}