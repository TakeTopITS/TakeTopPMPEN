using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;

public partial class TTTrainingUserInfoManage : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","ÅàÑµÈËÔ±²éÑ¯", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            IsPlayButton(strUserCode.Trim(), LanguageHandle.GetWord("PeiXunGuanLi").ToString().Trim());
            BindCheckData();
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        BindCheckData();
    }

    protected void IsPlayButton(string strusercode,string strtype)
    {
        string strHQL = " from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = '" + strusercode + "' and actorGroupDetail.GroupName='" + strtype + "' ";
        ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
        IList lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            Button1.Visible = true;
        }
        else
        {
            Button1.Visible = false;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = string.Empty;
                if (ddl_TrainingType.SelectedValue.Trim() == "PleaseSelect")
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSPXXLBBJC").ToString().Trim()+"')", true);
                    ddl_TrainingType.Focus();
                    return;
                }
                else if (ddl_TrainingType.SelectedValue.Trim() == "EmployeeTraining")
                {
                    fileName = LanguageHandle.GetWord("YuanGongPeiXun").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                else if (ddl_TrainingType.SelectedValue.Trim() == "TrainingRecord")
                {
                    fileName = LanguageHandle.GetWord("PeiXunJiLu").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                else if (ddl_TrainingType.SelectedValue.Trim() == "SpecialOperations")
                {
                    fileName = LanguageHandle.GetWord("TeChongZuoYe").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                else if (ddl_TrainingType.SelectedValue.Trim() == "SpecialEquipment")
                {
                    fileName = LanguageHandle.GetWord("TeChongSheBei").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                else if (ddl_TrainingType.SelectedValue.Trim() == "WeldingCertification")
                {
                    fileName = LanguageHandle.GetWord("HanJieChiZheng").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                else if (ddl_TrainingType.SelectedValue.Trim() == "ConstructionManagerCertificate")
                {
                    fileName = LanguageHandle.GetWord("ShiGongGuanLiYuanZheng").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                CreateExcel(getExportBookList(), fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BindCheckData()
    {
        if (ddl_TrainingType.SelectedValue.Trim() == "PleaseSelect")
        {
            Panel_Equipment.Visible = false;
            Panel_Holder.Visible = false;
            Panel_Operation.Visible = false;
            Panel_Post.Visible = false;
            Panel_TrainingInfo.Visible = false;
            Panel_TrainingRecord.Visible = false;
        }
        else if (ddl_TrainingType.SelectedValue.Trim() == "EmployeeTraining")
        {
            Panel_Equipment.Visible = false;
            Panel_Holder.Visible = false;
            Panel_Operation.Visible = false;
            Panel_Post.Visible = false;
            Panel_TrainingInfo.Visible = true;
            Panel_TrainingRecord.Visible = false;
            BindTREmployeeTrainingData();
        }
        else if (ddl_TrainingType.SelectedValue.Trim() == "TrainingRecord")
        {
            Panel_Equipment.Visible = false;
            Panel_Holder.Visible = false;
            Panel_Operation.Visible = false;
            Panel_Post.Visible = false;
            Panel_TrainingInfo.Visible = false;
            Panel_TrainingRecord.Visible = true;
            BindTRTrainingRecordEmpData();
        }
        else if (ddl_TrainingType.SelectedValue.Trim() == "SpecialOperations")
        {
            Panel_Equipment.Visible = false;
            Panel_Holder.Visible = false;
            Panel_Operation.Visible = true;
            Panel_Post.Visible = false;
            Panel_TrainingInfo.Visible = false;
            Panel_TrainingRecord.Visible = false;
            BindTRSpecialOperationsData();
        }
        else if (ddl_TrainingType.SelectedValue.Trim() == "SpecialEquipment")
        {
            Panel_Equipment.Visible = true;
            Panel_Holder.Visible = false;
            Panel_Operation.Visible = false;
            Panel_Post.Visible = false;
            Panel_TrainingInfo.Visible = false;
            Panel_TrainingRecord.Visible = false;
            BindTRSpecialEquipmentData();
        }
        else if (ddl_TrainingType.SelectedValue.Trim() == "WeldingCertification")
        {
            Panel_Equipment.Visible = false;
            Panel_Holder.Visible = true;
            Panel_Operation.Visible = false;
            Panel_Post.Visible = false;
            Panel_TrainingInfo.Visible = false;
            Panel_TrainingRecord.Visible = false;
            BindTRHolderWelderData();
        }
        else if (ddl_TrainingType.SelectedValue.Trim() == "ConstructionManagerCertificate")
        {
            Panel_Equipment.Visible = false;
            Panel_Holder.Visible = false;
            Panel_Operation.Visible = false;
            Panel_Post.Visible = true;
            Panel_TrainingInfo.Visible = false;
            Panel_TrainingRecord.Visible = false;
            BindTRPostCertificateData();
        }
    }

    protected DataTable getExportBookList()
    {
        DataSet ds = new DataSet();
        if (ddl_TrainingType.SelectedValue.Trim() == "EmployeeTraining")
        {
            string strHQL = "select A.ID 'SerialNumber',B.IDCard 'IDNumber',B.UserName 'Name',B.Gender 'Gender',A.ProfessionalSkillLevel 'ProfessionalSkillLevel'," +   //ChineseWord
                "A.ProfessionSkillNumber 'ProfessionalSkillAppraisalCertificateNumber',A.ValidityType 'AppraisedOccupation',A.ReleaseTime 'CertificateIssuanceDate',A.AnnValidTime 'AntiTerrorismValidityPeriod',"+   //ChineseWord
                LanguageHandle.GetWord("AAnnCertificateNoAnKongZhengSh").ToString().Trim() +
                "from T_TREmployeeTraining A,T_ProjectMember B Where A.UserCode=B.UserCode ";
            if (txt_NumberNo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_NumberNo.Text))
            {
                strHQL += " and B.IDCard like '%" + txt_NumberNo.Text.Trim() + "%' ";
            }
            if (txt_UserCode.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserCode.Text))
            {
                strHQL += " and A.UserCode like '%" + txt_UserCode.Text.Trim() + "%' ";
            }
            if (txt_UserName.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserName.Text))
            {
                strHQL += " and B.UserName like '%" + txt_UserName.Text.Trim() + "%' ";
            }
            if (TB_DepartCode.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartCode.Text))
            {
                strHQL += " and B.DepartCode like '%" + TB_DepartCode.Text.Trim() + "%' ";
            }
            if (TB_DepartName.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartName.Text))
            {
                strHQL += " and B.DepartName like '%" + TB_DepartName.Text.Trim() + "%' ";
            }
            if (DLC_StartTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_StartTime.Text))
            {
                strHQL += " and '" + DLC_StartTime.Text.Trim() + "'::date-A.EnterTime::date<=0 ";
            }
            if (DLC_EndTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_EndTime.Text))
            {
                strHQL += " and '" + DLC_EndTime.Text.Trim() + "'::date-A.EnterTime::date>=0 ";
            }
            if (txt_TrainingInfo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_TrainingInfo.Text))
            {
                strHQL += " and (A.ProfessionalSkillLevel like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.ProfessionSkillNumber like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                    "or A.ValidityType like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.AnnCertificateNo like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.AnnValidTime like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                    "or A.EnglishRiew like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.TrainingInfo like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.Remark like '%" + txt_TrainingInfo.Text.Trim() + "%') ";
            }
            strHQL += " Order By A.ID ASC ";

            ds = ShareClass.GetDataSetFromSql(strHQL, "T_TREmployeeTraining");
        }
        else if (ddl_TrainingType.SelectedValue.Trim() == "TrainingRecord")
        {
            string strHQL = "select A.ID 'SerialNumber',B.IDCard 'IDNumber',B.UserName 'Name',B.Gender 'Gender',A.TrainingProject 'TrainingProgram'," +   //ChineseWord
                LanguageHandle.GetWord("ATrainingAccordPeiXunYiJuATrai").ToString().Trim() +
                "A.TrainingTime 'TrainingDate' from T_TRTrainingRecordEmp A,T_ProjectMember B Where A.UserCode=B.UserCode ";   //ChineseWord
            if (txt_NumberNo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_NumberNo.Text))
            {
                strHQL += " and B.IDCard like '%" + txt_NumberNo.Text.Trim() + "%' ";
            }
            if (txt_UserCode.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserCode.Text))
            {
                strHQL += " and A.UserCode like '%" + txt_UserCode.Text.Trim() + "%' ";
            }
            if (txt_UserName.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserName.Text))
            {
                strHQL += " and B.UserName like '%" + txt_UserName.Text.Trim() + "%' ";
            }
            if (TB_DepartCode.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartCode.Text))
            {
                strHQL += " and B.DepartCode like '%" + TB_DepartCode.Text.Trim() + "%' ";
            }
            if (TB_DepartName.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartName.Text))
            {
                strHQL += " and B.DepartName like '%" + TB_DepartName.Text.Trim() + "%' ";
            }
            if (DLC_StartTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_StartTime.Text))
            {
                strHQL += " and '" + DLC_StartTime.Text.Trim() + "'::date-A.EnterTime::date<=0 ";
            }
            if (DLC_EndTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_EndTime.Text))
            {
                strHQL += " and '" + DLC_EndTime.Text.Trim() + "'::date-A.EnterTime::date>=0 ";
            }
            if (txt_TrainingInfo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_TrainingInfo.Text))
            {
                strHQL += " and (A.TrainingProject like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.TrainingAccord like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                    "or A.TrainingUnit like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.TrainingAddress like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.TrainingContent like '%" + txt_TrainingInfo.Text.Trim() + "%') ";
            }
            strHQL += " Order By A.ID ASC ";

            ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRTrainingRecordEmp");
        }
        else if (ddl_TrainingType.SelectedValue.Trim() == "SpecialOperations")
        {
            string strHQL = "select A.ID 'SerialNumber',B.IDCard 'IDNumber',B.UserName 'Name',B.Gender 'Gender',B.WorkType 'LaborCategory',A.SpeOpeType 'SpecialOperationsCategory'," +   //ChineseWord
                LanguageHandle.GetWord("ASpeOpeProjectTeChongZuoYeZhun").ToString().Trim() +
                "from T_TRSpecialOperations A,T_ProjectMember B Where A.UserCode=B.UserCode ";
            if (txt_NumberNo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_NumberNo.Text))
            {
                strHQL += " and B.IDCard like '%" + txt_NumberNo.Text.Trim() + "%' ";
            }
            if (txt_UserCode.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserCode.Text))
            {
                strHQL += " and A.UserCode like '%" + txt_UserCode.Text.Trim() + "%' ";
            }
            if (txt_UserName.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserName.Text))
            {
                strHQL += " and B.UserName like '%" + txt_UserName.Text.Trim() + "%' ";
            }
            if (TB_DepartCode.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartCode.Text))
            {
                strHQL += " and B.DepartCode like '%" + TB_DepartCode.Text.Trim() + "%' ";
            }
            if (TB_DepartName.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartName.Text))
            {
                strHQL += " and B.DepartName like '%" + TB_DepartName.Text.Trim() + "%' ";
            }
            if (DLC_StartTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_StartTime.Text))
            {
                strHQL += " and '" + DLC_StartTime.Text.Trim() + "'::date-A.EnterTime::date<=0 ";
            }
            if (DLC_EndTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_EndTime.Text))
            {
                strHQL += " and '" + DLC_EndTime.Text.Trim() + "'::date-A.EnterTime::date>=0 ";
            }
            if (txt_TrainingInfo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_TrainingInfo.Text))
            {
                strHQL += " and (A.SpeOpeNumber like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.SpeOpeProject like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                    "or A.SpeOpeType like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.Remark like '%" + txt_TrainingInfo.Text.Trim() + "%') ";
            }
            strHQL += " Order By A.ID ASC ";

            ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRSpecialOperations");
        }
        else if (ddl_TrainingType.SelectedValue.Trim() == "SpecialEquipment")
        {
            string strHQL = "select A.ID 'SerialNumber',B.IDCard 'IDNumber',B.UserName 'Name',B.Gender 'Gender',B.WorkType 'LaborCategory',A.SpeEquType 'SpecialEquipmentCategory'," +   //ChineseWord
                LanguageHandle.GetWord("ASpeEquProjectTeChongSheBeiZhu").ToString().Trim() +
                "from T_TRSpecialEquipment A,T_ProjectMember B Where A.UserCode=B.UserCode ";
            if (txt_NumberNo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_NumberNo.Text))
            {
                strHQL += " and B.IDCard like '%" + txt_NumberNo.Text.Trim() + "%' ";
            }
            if (txt_UserCode.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserCode.Text))
            {
                strHQL += " and A.UserCode like '%" + txt_UserCode.Text.Trim() + "%' ";
            }
            if (txt_UserName.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserName.Text))
            {
                strHQL += " and B.UserName like '%" + txt_UserName.Text.Trim() + "%' ";
            }
            if (TB_DepartCode.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartCode.Text))
            {
                strHQL += " and B.DepartCode like '%" + TB_DepartCode.Text.Trim() + "%' ";
            }
            if (TB_DepartName.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartName.Text))
            {
                strHQL += " and B.DepartName like '%" + TB_DepartName.Text.Trim() + "%' ";
            }
            if (DLC_StartTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_StartTime.Text))
            {
                strHQL += " and '" + DLC_StartTime.Text.Trim() + "'::date-A.EnterTime::date<=0 ";
            }
            if (DLC_EndTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_EndTime.Text))
            {
                strHQL += " and '" + DLC_EndTime.Text.Trim() + "'::date-A.EnterTime::date>=0 ";
            }
            if (txt_TrainingInfo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_TrainingInfo.Text))
            {
                strHQL += " and (A.SpeEquNumber like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.SpeEquProject like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                    "or A.SpeEquType like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.Remark like '%" + txt_TrainingInfo.Text.Trim() + "%') ";
            }
            strHQL += " Order By A.ID ASC ";

            ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRSpecialEquipment");
        }
        else if (ddl_TrainingType.SelectedValue.Trim() == "WeldingCertification")
        {
            string strHQL = "select A.ID 'SerialNumber',B.IDCard 'IDNumber',B.UserName 'Name',B.Gender 'Gender',A.CertificateNo 'SpecialEquipmentWeldingOperatorCertificateCode'," +   //ChineseWord
                "A.WelderSeal 'WelderStamp',A.HolderProject 'CertifiedProjects',A.ValidTime 'ValidityPeriodOfCertifiedProjects',A.Unit 'Unit',A.Remark 'Remark' from T_TRHolderWelder A," +   //ChineseWord
                "T_ProjectMember B Where A.UserCode=B.UserCode ";
            if (txt_NumberNo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_NumberNo.Text))
            {
                strHQL += " and B.IDCard like '%" + txt_NumberNo.Text.Trim() + "%' ";
            }
            if (txt_UserCode.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserCode.Text))
            {
                strHQL += " and A.UserCode like '%" + txt_UserCode.Text.Trim() + "%' ";
            }
            if (txt_UserName.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserName.Text))
            {
                strHQL += " and B.UserName like '%" + txt_UserName.Text.Trim() + "%' ";
            }
            if (TB_DepartCode.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartCode.Text))
            {
                strHQL += " and B.DepartCode like '%" + TB_DepartCode.Text.Trim() + "%' ";
            }
            if (TB_DepartName.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartName.Text))
            {
                strHQL += " and B.DepartName like '%" + TB_DepartName.Text.Trim() + "%' ";
            }
            if (DLC_StartTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_StartTime.Text))
            {
                strHQL += " and '" + DLC_StartTime.Text.Trim() + "'::date-A.EnterTime::date<=0 ";
            }
            if (DLC_EndTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_EndTime.Text))
            {
                strHQL += " and '" + DLC_EndTime.Text.Trim() + "'::date-A.EnterTime::date>=0 ";
            }
            if (txt_TrainingInfo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_TrainingInfo.Text))
            {
                strHQL += " and (A.CertificateNo like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.WelderSeal like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                    "or A.HolderProject like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.ValidTime like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.Unit like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                    "or A.Remark like '%" + txt_TrainingInfo.Text.Trim() + "%') ";
            }
            strHQL += " Order By A.ID ASC ";

            ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRHolderWelder");
        }
        else if (ddl_TrainingType.SelectedValue.Trim() == "ConstructionManagerCertificate")
        {
            string strHQL = "select A.ID 'SerialNumber',B.IDCard 'IDNumber',B.UserName 'Name',B.Gender 'Gender',B.BirthDay 'DateOfBirth',A.Unit 'Unit',B.WorkType 'LaborCategory',A.Job 'Position'," +   //ChineseWord
                LanguageHandle.GetWord("ACertificateNoGangWeiZhengShuB").ToString().Trim() +
                "from T_TRPostCertificate A,T_ProjectMember B Where A.UserCode=B.UserCode ";
            if (txt_NumberNo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_NumberNo.Text))
            {
                strHQL += " and B.IDCard like '%" + txt_NumberNo.Text.Trim() + "%' ";
            }
            if (txt_UserCode.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserCode.Text))
            {
                strHQL += " and A.UserCode like '%" + txt_UserCode.Text.Trim() + "%' ";
            }
            if (txt_UserName.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserName.Text))
            {
                strHQL += " and B.UserName like '%" + txt_UserName.Text.Trim() + "%' ";
            }
            if (TB_DepartCode.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartCode.Text))
            {
                strHQL += " and B.DepartCode like '%" + TB_DepartCode.Text.Trim() + "%' ";
            }
            if (TB_DepartName.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartName.Text))
            {
                strHQL += " and B.DepartName like '%" + TB_DepartName.Text.Trim() + "%' ";
            }
            if (DLC_StartTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_StartTime.Text))
            {
                strHQL += " and '" + DLC_StartTime.Text.Trim() + "'::date-A.EnterTime::date<=0 ";
            }
            if (DLC_EndTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_EndTime.Text))
            {
                strHQL += " and '" + DLC_EndTime.Text.Trim() + "'::date-A.EnterTime::date>=0 ";
            }
            if (txt_TrainingInfo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_TrainingInfo.Text))
            {
                strHQL += " and (A.CertificateNo like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.Job like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                    "or A.CertificateOffice like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.Unit like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                    "or A.Remark like '%" + txt_TrainingInfo.Text.Trim() + "%') ";
            }
            strHQL += " Order By A.ID ASC ";

            ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRPostCertificate");
        }
        return ds.Tables[0];
    }

    private void CreateExcel(DataTable dt, string fileName)
    {
        DataGrid dg = new DataGrid();
        dg.DataSource = dt;
        dg.DataBind();

        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/vnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dg.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();
    }

    protected void BindTREmployeeTrainingData()
    {
        string strHQL = "select A.*,B.UserName,B.BirthDay,B.WorkType,B.IDCard,B.Gender from T_TREmployeeTraining A,T_ProjectMember B Where A.UserCode=B.UserCode ";
        if (txt_NumberNo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_NumberNo.Text))
        {
            strHQL += " and B.IDCard like '%" + txt_NumberNo.Text.Trim() + "%' ";
        }
        if (txt_UserCode.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserCode.Text))
        {
            strHQL += " and A.UserCode like '%" + txt_UserCode.Text.Trim() + "%' ";
        }
        if (txt_UserName.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserName.Text))
        {
            strHQL += " and B.UserName like '%" + txt_UserName.Text.Trim() + "%' ";
        }
        if (TB_DepartCode.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartCode.Text))
        {
            strHQL += " and B.DepartCode like '%" + TB_DepartCode.Text.Trim() + "%' ";
        }
        if (TB_DepartName.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartName.Text))
        {
            strHQL += " and B.DepartName like '%" + TB_DepartName.Text.Trim() + "%' ";
        }
        if (DLC_StartTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_StartTime.Text))
        {
            strHQL += " and '" + DLC_StartTime.Text.Trim() + "'::date-A.EnterTime::date<=0 ";
        }
        if (DLC_EndTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_EndTime.Text))
        {
            strHQL += " and '" + DLC_EndTime.Text.Trim() + "'::date-A.EnterTime::date>=0 ";
        }
        if (txt_TrainingInfo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_TrainingInfo.Text))
        {
            strHQL += " and (A.ProfessionalSkillLevel like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.ProfessionSkillNumber like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                "or A.ValidityType like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.AnnCertificateNo like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.AnnValidTime like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                "or A.EnglishRiew like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.TrainingInfo like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.Remark like '%" + txt_TrainingInfo.Text.Trim() + "%') ";
        }
        strHQL += " Order By A.ID ASC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TREmployeeTraining");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
        lbl_sql1.Text = strHQL;
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql1.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TREmployeeTraining");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void BindTRSpecialOperationsData()
    {
        string strHQL = "select A.*,B.UserName,B.BirthDay,B.WorkType,B.IDCard,B.Gender from T_TRSpecialOperations A,T_ProjectMember B Where A.UserCode=B.UserCode ";
        if (txt_NumberNo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_NumberNo.Text))
        {
            strHQL += " and B.IDCard like '%" + txt_NumberNo.Text.Trim() + "%' ";
        }
        if (txt_UserCode.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserCode.Text))
        {
            strHQL += " and A.UserCode like '%" + txt_UserCode.Text.Trim() + "%' ";
        }
        if (txt_UserName.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserName.Text))
        {
            strHQL += " and B.UserName like '%" + txt_UserName.Text.Trim() + "%' ";
        }
        if (TB_DepartCode.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartCode.Text))
        {
            strHQL += " and B.DepartCode like '%" + TB_DepartCode.Text.Trim() + "%' ";
        }
        if (TB_DepartName.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartName.Text))
        {
            strHQL += " and B.DepartName like '%" + TB_DepartName.Text.Trim() + "%' ";
        }
        if (DLC_StartTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_StartTime.Text))
        {
            strHQL += " and '" + DLC_StartTime.Text.Trim() + "'::date-A.EnterTime::date<=0 ";
        }
        if (DLC_EndTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_EndTime.Text))
        {
            strHQL += " and '" + DLC_EndTime.Text.Trim() + "'::date-A.EnterTime::date>=0 ";
        }
        if (txt_TrainingInfo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_TrainingInfo.Text))
        {
            strHQL += " and (A.SpeOpeNumber like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.SpeOpeProject like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                "or A.SpeOpeType like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.Remark like '%" + txt_TrainingInfo.Text.Trim() + "%') ";
        }
        strHQL += " Order By A.ID ASC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRSpecialOperations");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql2.Text = strHQL;
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql2.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRSpecialOperations");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BindTRSpecialEquipmentData()
    {
        string strHQL = "select A.*,B.UserName,B.BirthDay,B.WorkType,B.IDCard,B.Gender from T_TRSpecialEquipment A,T_ProjectMember B Where A.UserCode=B.UserCode ";
        if (txt_NumberNo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_NumberNo.Text))
        {
            strHQL += " and B.IDCard like '%" + txt_NumberNo.Text.Trim() + "%' ";
        }
        if (txt_UserCode.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserCode.Text))
        {
            strHQL += " and A.UserCode like '%" + txt_UserCode.Text.Trim() + "%' ";
        }
        if (txt_UserName.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserName.Text))
        {
            strHQL += " and B.UserName like '%" + txt_UserName.Text.Trim() + "%' ";
        }
        if (TB_DepartCode.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartCode.Text))
        {
            strHQL += " and B.DepartCode like '%" + TB_DepartCode.Text.Trim() + "%' ";
        }
        if (TB_DepartName.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartName.Text))
        {
            strHQL += " and B.DepartName like '%" + TB_DepartName.Text.Trim() + "%' ";
        }
        if (DLC_StartTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_StartTime.Text))
        {
            strHQL += " and '" + DLC_StartTime.Text.Trim() + "'::date-A.EnterTime::date<=0 ";
        }
        if (DLC_EndTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_EndTime.Text))
        {
            strHQL += " and '" + DLC_EndTime.Text.Trim() + "'::date-A.EnterTime::date>=0 ";
        }
        if (txt_TrainingInfo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_TrainingInfo.Text))
        {
            strHQL += " and (A.SpeEquNumber like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.SpeEquProject like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                "or A.SpeEquType like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.Remark like '%" + txt_TrainingInfo.Text.Trim() + "%') ";
        }
        strHQL += " Order By A.ID ASC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRSpecialEquipment");

        DataGrid3.CurrentPageIndex = 0;
        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
        lbl_sql3.Text = strHQL;
    }

    protected void DataGrid3_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql3.Text.Trim();
        DataGrid3.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRSpecialEquipment");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }

    protected void BindTRHolderWelderData()
    {
        string strHQL = "select A.*,B.UserName,B.BirthDay,B.WorkType,B.IDCard,B.Gender from T_TRHolderWelder A,T_ProjectMember B Where A.UserCode=B.UserCode ";
        if (txt_NumberNo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_NumberNo.Text))
        {
            strHQL += " and B.IDCard like '%" + txt_NumberNo.Text.Trim() + "%' ";
        }
        if (txt_UserCode.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserCode.Text))
        {
            strHQL += " and A.UserCode like '%" + txt_UserCode.Text.Trim() + "%' ";
        }
        if (txt_UserName.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserName.Text))
        {
            strHQL += " and B.UserName like '%" + txt_UserName.Text.Trim() + "%' ";
        }
        if (TB_DepartCode.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartCode.Text))
        {
            strHQL += " and B.DepartCode like '%" + TB_DepartCode.Text.Trim() + "%' ";
        }
        if (TB_DepartName.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartName.Text))
        {
            strHQL += " and B.DepartName like '%" + TB_DepartName.Text.Trim() + "%' ";
        }
        if (DLC_StartTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_StartTime.Text))
        {
            strHQL += " and '" + DLC_StartTime.Text.Trim() + "'::date-A.EnterTime::date<=0 ";
        }
        if (DLC_EndTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_EndTime.Text))
        {
            strHQL += " and '" + DLC_EndTime.Text.Trim() + "'::date-A.EnterTime::date>=0 ";
        }
        if (txt_TrainingInfo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_TrainingInfo.Text))
        {
            strHQL += " and (A.CertificateNo like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.WelderSeal like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                "or A.HolderProject like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.ValidTime like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.Unit like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                "or A.Remark like '%" + txt_TrainingInfo.Text.Trim() + "%') ";
        }
        strHQL += " Order By A.ID ASC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRHolderWelder");

        DataGrid4.CurrentPageIndex = 0;
        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();
        lbl_sql4.Text = strHQL;
    }

    protected void DataGrid4_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql4.Text.Trim();
        DataGrid4.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRHolderWelder");

        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();
    }

    protected void BindTRPostCertificateData()
    {
        string strHQL = "select A.*,B.UserName,B.BirthDay,B.WorkType,B.IDCard,B.Gender from T_TRPostCertificate A,T_ProjectMember B Where A.UserCode=B.UserCode ";
        if (txt_NumberNo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_NumberNo.Text))
        {
            strHQL += " and B.IDCard like '%" + txt_NumberNo.Text.Trim() + "%' ";
        }
        if (txt_UserCode.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserCode.Text))
        {
            strHQL += " and A.UserCode like '%" + txt_UserCode.Text.Trim() + "%' ";
        }
        if (txt_UserName.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserName.Text))
        {
            strHQL += " and B.UserName like '%" + txt_UserName.Text.Trim() + "%' ";
        }
        if (TB_DepartCode.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartCode.Text))
        {
            strHQL += " and B.DepartCode like '%" + TB_DepartCode.Text.Trim() + "%' ";
        }
        if (TB_DepartName.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartName.Text))
        {
            strHQL += " and B.DepartName like '%" + TB_DepartName.Text.Trim() + "%' ";
        }
        if (DLC_StartTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_StartTime.Text))
        {
            strHQL += " and '" + DLC_StartTime.Text.Trim() + "'::date-A.EnterTime::date<=0 ";
        }
        if (DLC_EndTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_EndTime.Text))
        {
            strHQL += " and '" + DLC_EndTime.Text.Trim() + "'::date-A.EnterTime::date>=0 ";
        }
        if (txt_TrainingInfo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_TrainingInfo.Text))
        {
            strHQL += " and (A.CertificateNo like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.Job like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                "or A.CertificateOffice like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.Unit like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                "or A.Remark like '%" + txt_TrainingInfo.Text.Trim() + "%') ";
        }
        strHQL += " Order By A.ID ASC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRPostCertificate");

        DataGrid5.CurrentPageIndex = 0;
        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();
        lbl_sql5.Text = strHQL;
    }

    protected void DataGrid5_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql5.Text.Trim();
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRPostCertificate");

        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();
    }

    protected void BindTRTrainingRecordEmpData()
    {
        string strHQL = "select A.*,B.UserName,B.BirthDay,B.WorkType,B.IDCard,B.Gender from T_TRTrainingRecordEmp A,T_ProjectMember B Where A.UserCode=B.UserCode ";
        if (txt_NumberNo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_NumberNo.Text))
        {
            strHQL += " and B.IDCard like '%" + txt_NumberNo.Text.Trim() + "%' ";
        }
        if (txt_UserCode.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserCode.Text))
        {
            strHQL += " and A.UserCode like '%" + txt_UserCode.Text.Trim() + "%' ";
        }
        if (txt_UserName.Text.Trim() != "" && !string.IsNullOrEmpty(txt_UserName.Text))
        {
            strHQL += " and B.UserName like '%" + txt_UserName.Text.Trim() + "%' ";
        }
        if (TB_DepartCode.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartCode.Text))
        {
            strHQL += " and B.DepartCode like '%" + TB_DepartCode.Text.Trim() + "%' ";
        }
        if (TB_DepartName.Text.Trim() != "" && !string.IsNullOrEmpty(TB_DepartName.Text))
        {
            strHQL += " and B.DepartName like '%" + TB_DepartName.Text.Trim() + "%' ";
        }
        if (DLC_StartTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_StartTime.Text))
        {
            strHQL += " and '" + DLC_StartTime.Text.Trim() + "'::date-A.EnterTime::date<=0 ";
        }
        if (DLC_EndTime.Text.Trim() != "" && !string.IsNullOrEmpty(DLC_EndTime.Text))
        {
            strHQL += " and '" + DLC_EndTime.Text.Trim() + "'::date-A.EnterTime::date>=0 ";
        }
        if (txt_TrainingInfo.Text.Trim() != "" && !string.IsNullOrEmpty(txt_TrainingInfo.Text))
        {
            strHQL += " and (A.TrainingProject like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.TrainingAccord like '%" + txt_TrainingInfo.Text.Trim() + "%' " +
                "or A.TrainingUnit like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.TrainingAddress like '%" + txt_TrainingInfo.Text.Trim() + "%' or A.TrainingContent like '%" + txt_TrainingInfo.Text.Trim() + "%') ";
        }
        strHQL += " Order By A.ID ASC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRTrainingRecordEmp");

        DataGrid6.CurrentPageIndex = 0;
        DataGrid6.DataSource = ds;
        DataGrid6.DataBind();
        lbl_sql6.Text = strHQL;
    }

    protected void DataGrid6_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql6.Text.Trim();
        DataGrid6.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TRTrainingRecordEmp");

        DataGrid6.DataSource = ds;
        DataGrid6.DataBind();
    }
}
