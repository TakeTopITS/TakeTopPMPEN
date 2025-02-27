using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTVisitRegistration_StudentEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "查看所有项目", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["ID"] != null)
            {
                string strID = Request.QueryString["ID"];

                HF_ID.Value = strID;

                DataVisitRegistrationBander(strID);
            }
            else {
                TXT_VisitStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }


    private void DataVisitRegistrationBander(string strID)
    {
        VisitRegistration_StudentBLL visitRegistration_StudentBLL = new VisitRegistration_StudentBLL();
        string strVisitRegistrationHQL = "from VisitRegistration_Student as visitRegistration_Student where ID = " + strID;
        IList listVisitRegistration = visitRegistration_StudentBLL.GetAllVisitRegistration_Students(strVisitRegistrationHQL);
        if (listVisitRegistration != null && listVisitRegistration.Count > 0)
        {
            VisitRegistration_Student visitRegistration_Student = (VisitRegistration_Student)listVisitRegistration[0];

            TXT_VisitStartTime.Text = visitRegistration_Student.VisitStartTime.ToString("yyyy-MM-dd HH:mm:ss");
            TXT_VisitName.Text = visitRegistration_Student.VisitName;
            DDL_VisitSex.SelectedValue = visitRegistration_Student.VisitSex;
            TXT_VisitCardName.Text = visitRegistration_Student.VisitCardName;
            //TXT_VisitReason.Text = visitRegistration_Student.VisitReason;
            DDL_VisitReason.SelectedValue = visitRegistration_Student.VisitReason;
            //TXT_ReceiverName.Text = visitRegistration_Student.ReceiverName;
            DDL_ReceiverName.SelectedValue = visitRegistration_Student.ReceiverName;
            TXT_VisitEndTime.Text = visitRegistration_Student.VisitEndTime;
        }
    }


    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strID = HF_ID.Value;

        string strVisitStartTime = TXT_VisitStartTime.Text.Trim();
        string strVisitName = TXT_VisitName.Text.Trim();
        string strVisitSex = DDL_VisitSex.SelectedValue;
         string strVisitCardName = TXT_VisitCardName.Text.Trim();
        //string strVisitReason = TXT_VisitReason.Text.Trim();
         string strVisitReason = DDL_VisitReason.SelectedValue;
        //string strReceiverName = TXT_ReceiverName.Text.Trim();
         string strReceiverName = DDL_ReceiverName.SelectedValue;
        string strVisitEndTime = TXT_VisitEndTime.Text.Trim();

        if (string.IsNullOrEmpty(strVisitName))
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBFRXMBNWKBC").ToString().Trim()+"')", true);
            Response.Write("<script>alert('"+LanguageHandle.GetWord("ZZBFRXMBNWKBC").ToString().Trim()+"');</script>");
            return;
        }
        if (string.IsNullOrEmpty(strVisitCardName))
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZLFRZJBNWKBC").ToString().Trim()+"')", true);
            Response.Write("<script>alert('"+LanguageHandle.GetWord("ZZLFRZJBNWKBC").ToString().Trim()+"');</script>");
            return;
        }
        if (string.IsNullOrEmpty(strReceiverName))
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDRBNWKBC").ToString().Trim()+"')", true);
            Response.Write("<script>alert('"+LanguageHandle.GetWord("ZZJDRBNWKBC").ToString().Trim()+"');</script>");
            return;
        }

        DateTime dtVisitStartTime = DateTime.Now;
        DateTime.TryParse(strVisitStartTime, out dtVisitStartTime);

        VisitRegistration_StudentBLL visitRegistration_StudentBLL = new VisitRegistration_StudentBLL();
        if (!string.IsNullOrEmpty(strID))
        {
            string strVisitRegistrationHQL = "from VisitRegistration_Student as visitRegistration_Student where ID = " + strID;
            IList listVisitRegistration = visitRegistration_StudentBLL.GetAllVisitRegistration_Students(strVisitRegistrationHQL);
            if (listVisitRegistration != null && listVisitRegistration.Count == 1)
            {
                VisitRegistration_Student visitRegistration_Student = (VisitRegistration_Student)listVisitRegistration[0];

                visitRegistration_Student.VisitStartTime = dtVisitStartTime;
                visitRegistration_Student.VisitName = strVisitName;
                visitRegistration_Student.VisitSex = strVisitSex;
                visitRegistration_Student.VisitCardName = strVisitCardName;
                visitRegistration_Student.VisitReason = strVisitReason;
                visitRegistration_Student.ReceiverName = strReceiverName;
                visitRegistration_Student.VisitEndTime = strVisitEndTime;

                visitRegistration_StudentBLL.UpdateVisitRegistration_Student(visitRegistration_Student, visitRegistration_Student.ID);

                Response.Write("<script>alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"');</script>");
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }

        }
        else
        {
            VisitRegistration_Student visitRegistration_Student = new VisitRegistration_Student();

            visitRegistration_Student.VisitStartTime = dtVisitStartTime;
            visitRegistration_Student.VisitName = strVisitName;
            visitRegistration_Student.VisitSex = strVisitSex;
            visitRegistration_Student.VisitCardName = strVisitCardName;
            visitRegistration_Student.VisitReason = strVisitReason;
            visitRegistration_Student.ReceiverName = strReceiverName;
            visitRegistration_Student.VisitEndTime = strVisitEndTime;
            visitRegistration_Student.UserCode = strUserCode;

            visitRegistration_StudentBLL.AddVisitRegistration_Student(visitRegistration_Student);

            Response.Write("<script>alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"');</script>");
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);
        }

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        //Response.Write("<script>LoadParentLit();</script>");
    }



}