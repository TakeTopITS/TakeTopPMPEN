using System; using System.Resources;
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
using System.Data.SqlClient;
using System.IO;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using System.Data.OleDb;

public partial class TTCustomerTrainInfoEdit : System.Web.UI.Page
{
    string strUserCodeOld;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode, strUserName;
        string strDepartString;

        strUserCodeOld = Session["UserCode"].ToString().Trim();

        if (Session["UserCode"] != null)
        {
            strUserCode = Session["UserCode"].ToString();
            LB_UserCode.Text = strUserCode;
        }
        else
        {
            Session["UserCode"] = LB_UserCode.Text.Trim();
            strUserCode = LB_UserCode.Text.Trim();
        }

        strUserName = GetUserName(strUserCode);

        string strHQL;
        IList lst;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_EquipmentSpeEquReviewTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EquipmentSpeEquStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_OperationSpeOpeReviewTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_OperationSpeOpeStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_PostBirthDay.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_PostCertificateReviewTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_PostCertificateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_TrainingReleaseTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_TrainingTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strHQL = "from WorkType as workType Order by workType.SortNo ASC";
            BookReaderTypeBLL bookReaderTypeBLL = new BookReaderTypeBLL();
            lst = bookReaderTypeBLL.GetAllBookReaderType(strHQL);

            ddl_EquipmentWorkType.DataSource = lst;
            ddl_EquipmentWorkType.DataBind();
            ddl_EquipmentWorkType.Items.Insert(0, new ListItem("--Select--", ""));

            ddl_OperationWorkType.DataSource = lst;
            ddl_OperationWorkType.DataBind();
            ddl_OperationWorkType.Items.Insert(0, new ListItem("--Select--", ""));

            ddl_PostWorkType.DataSource = lst;
            ddl_PostWorkType.DataBind();
            ddl_PostWorkType.Items.Insert(0, new ListItem("--Select--", ""));

            ddl_TrainingWorkType.DataSource = lst;
            ddl_TrainingWorkType.DataBind();
            ddl_TrainingWorkType.Items.Insert(0, new ListItem("--Select--", ""));


            strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);
            strHQL = "from Department as department Where department.DepartCode in " + strDepartString;

            TakeTopCore.CoreShareClass.InitialUnderDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode.Trim());
        }
    }

    protected string UploadBookDesign()
    {
        //上传规章制度附件
        if (AttachFile.HasFile)
        {
            string strFileName1, strExtendName;

            strFileName1 = this.AttachFile.FileName;//获取上传文件的文件名,包括后缀
            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCodeOld + "\\Images\\";

            string bookimage = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCodeOld + "\\Images\\" + strFileName3;

            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);



            if (fi.Exists)
            {
                return "1";
            }
            else
            {
                try
                {
                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                    return bookimage;
                }
                catch
                {
                    return "2";
                }
            }
        }
        else
        {
            return "0";
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;

        Session["UserCode"] = LB_UserCode.Text.Trim();

        string strDepartCode;
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target;

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid2);

            strHQL = "Select DepartCode,DepartName from T_Department where DepartCode = " + "'" + strDepartCode + "'";
            strHQL += " Union Select DepartCode,DepartName from T_Department where ParentCode = " + "'" + strDepartCode + "'";

            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DepartMent");

            LB_DepartCode.Text = strDepartCode;
        }

        Session["UserCode"] = LB_UserCode.Text.Trim();
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();

        string strHQL;
        IList lst;

        strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];

        LB_EquipmentUserCode.Text = projectMember.UserCode.Trim();
        LB_HolderUserCode.Text = projectMember.UserCode.Trim();
        LB_OperationUserCode.Text = projectMember.UserCode.Trim();
        LB_PostUserCode.Text = projectMember.UserCode.Trim();
        LB_TrainingUserCode.Text = projectMember.UserCode.Trim();

        TB_EquipmentName.Text = projectMember.UserName.Trim();
        TB_HolderName.Text = projectMember.UserName.Trim();
        TB_OperationName.Text = projectMember.UserName.Trim();
        TB_PostName.Text = projectMember.UserName.Trim();
        TB_TrainingName.Text = projectMember.UserName.Trim();

        TB_EquipmentNumberNo.Text = projectMember.IDCard.Trim();
        TB_HolderNumberNo.Text = projectMember.IDCard.Trim();
        TB_OperationNumberNo.Text = projectMember.IDCard.Trim();
        TB_PostNumberNo.Text = projectMember.IDCard.Trim();
        TB_TrainingNumberNo.Text = projectMember.IDCard.Trim();

        ddl_EquipmentSex.SelectedValue = string.IsNullOrEmpty(projectMember.Gender) || projectMember.Gender.Trim() == "" ? "Male" : projectMember.Gender.Trim();
        ddl_HolderSex.SelectedValue = string.IsNullOrEmpty(projectMember.Gender) || projectMember.Gender.Trim() == "" ? "Male" : projectMember.Gender.Trim();
        ddl_OperationSex.SelectedValue = string.IsNullOrEmpty(projectMember.Gender) || projectMember.Gender.Trim() == "" ? "Male" : projectMember.Gender.Trim();
        ddl_PostSex.SelectedValue = string.IsNullOrEmpty(projectMember.Gender) || projectMember.Gender.Trim() == "" ? "Male" : projectMember.Gender.Trim();
        ddl_TrainingSex.SelectedValue = string.IsNullOrEmpty(projectMember.Gender) || projectMember.Gender.Trim() == "" ? "Male" : projectMember.Gender.Trim();

        ddl_EquipmentWorkType.SelectedValue = string.IsNullOrEmpty(projectMember.WorkType) ? "" : projectMember.WorkType.Trim();
        ddl_OperationWorkType.SelectedValue = string.IsNullOrEmpty(projectMember.WorkType) ? "" : projectMember.WorkType.Trim();
        ddl_PostWorkType.SelectedValue = string.IsNullOrEmpty(projectMember.WorkType) ? "" : projectMember.WorkType.Trim();
        ddl_TrainingWorkType.SelectedValue = string.IsNullOrEmpty(projectMember.WorkType) ? "" : projectMember.WorkType.Trim();

        DLC_PostBirthDay.Text = string.IsNullOrEmpty(projectMember.BirthDay.ToString()) ? DateTime.Now.ToString("yyyy-MM-dd") : projectMember.BirthDay.ToString("yyyy-MM-dd");

        TB_HolderUnit.Text = projectMember.DepartName.Trim();
        TB_PostUnit.Text = projectMember.DepartName.Trim();

        LoadTREmployeeTrainingList(strUserCode);
        LoadTRSpecialOperationsList(strUserCode);
        LoadTRSpecialEquipmentList(strUserCode);
        LoadTRHolderWelderList(strUserCode);
        LoadTRPostCertificateList(strUserCode);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        ShareClass.ColorDataGridSelectRow(DataGrid1, e);

        string strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

        strHQL = "from TRHolderWelder as tRHolderWelder where tRHolderWelder.ID = " + strID;
        TRHolderWelderBLL tRHolderWelderBLL = new TRHolderWelderBLL();
        lst = tRHolderWelderBLL.GetAllTRHolderWelders(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRHolderWelder tRHolderWelder = (TRHolderWelder)lst[0];
            LB_HolderID.Text = tRHolderWelder.ID.ToString();
            LB_HolderUserCode.Text = tRHolderWelder.UserCode.Trim();
            TB_HolderProject.Text = string.IsNullOrEmpty(tRHolderWelder.HolderProject) ? "" : tRHolderWelder.HolderProject.Trim();
            TB_HolderValidTime.Text = string.IsNullOrEmpty(tRHolderWelder.ValidTime) ? "" : tRHolderWelder.ValidTime.Trim();
            lbl_AttachPath.Text = string.IsNullOrEmpty(tRHolderWelder.AttachPath) ? "" : tRHolderWelder.AttachPath.Trim();

            BT_ProjectAdd.Visible = true;
            BT_ProjectAdd.Enabled = true;
            BT_ProjectUpdate.Visible = true;
            BT_ProjectUpdate.Enabled = true;
            BT_ProjectDelete.Visible = true;
            BT_ProjectDelete.Enabled = true;
            BT_HolderUpdate.Visible = true;
            BT_HolderUpdate.Enabled = true;
            BT_HolderDelete.Visible = true;
            BT_HolderDelete.Enabled = true;
        }
    }

    protected void BT_TrainingUpdate_Click(object sender, EventArgs e)
    {
        string strID = LB_TrainingID.Text.Trim();
        if (string.IsNullOrEmpty(strID) || strID == "")//新增
        {
            TREmployeeTrainingBLL tREmployeeTrainingBLL = new TREmployeeTrainingBLL();
            TREmployeeTraining tREmployeeTraining = new TREmployeeTraining();
            tREmployeeTraining.AnnCertificateNo = TB_TrainingAnnCertificateNo.Text.Trim();
            tREmployeeTraining.AnnValidTime = TB_TrainingAnnValidTime.Text.Trim();
            tREmployeeTraining.EnglishRiew = TB_TrainingEnglishRiew.Text.Trim();
            tREmployeeTraining.EnterCode = strUserCodeOld.Trim();
            tREmployeeTraining.ProfessionalSkillLevel = TB_TrainingProfessionalSkillLevel.Text.Trim();
            tREmployeeTraining.ProfessionSkillNumber = TB_TrainingProfessionSkillNumber.Text.Trim();
            tREmployeeTraining.ReleaseTime = DateTime.Parse(string.IsNullOrEmpty(DLC_TrainingReleaseTime.Text) || DLC_TrainingReleaseTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_TrainingReleaseTime.Text.Trim());
            tREmployeeTraining.Remark = TB_TrainingRemark.Text.Trim();
            tREmployeeTraining.TrainingInfo = TB_TrainingTrainingInfo.Text.Trim();
            tREmployeeTraining.UserCode = LB_TrainingUserCode.Text.Trim();
            tREmployeeTraining.ValidityType = TB_TrainingValidityType.Text.Trim();
            tREmployeeTraining.EnterTime = DateTime.Now;
            try
            {
                tREmployeeTrainingBLL.AddTREmployeeTraining(tREmployeeTraining);
                LB_TrainingID.Text = GetTREmployeeTrainingMaxID(tREmployeeTraining.UserCode);

                UpdateUserInfo(TB_TrainingName.Text.Trim(), ddl_TrainingSex.SelectedValue.Trim(), TB_TrainingNumberNo.Text.Trim(), ddl_TrainingWorkType.SelectedValue.Trim(), string.Empty, LB_TrainingUserCode.Text.Trim());

                LoadTREmployeeTrainingList(tREmployeeTraining.UserCode);

                BT_TrainingUpdate.Visible = true;
                BT_TrainingUpdate.Enabled = true;
                BT_TrainingDelete.Visible = true;
                BT_TrainingDelete.Enabled = true;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
            }
        }
        else//更新
        {
            string strHQL = "from TREmployeeTraining as tREmployeeTraining where tREmployeeTraining.ID = " + strID;
            TREmployeeTrainingBLL tREmployeeTrainingBLL = new TREmployeeTrainingBLL();
            IList lst = tREmployeeTrainingBLL.GetAllTREmployeeTrainings(strHQL);
            TREmployeeTraining tREmployeeTraining = (TREmployeeTraining)lst[0];

            tREmployeeTraining.AnnCertificateNo = TB_TrainingAnnCertificateNo.Text.Trim();
            tREmployeeTraining.AnnValidTime = TB_TrainingAnnValidTime.Text.Trim();
            tREmployeeTraining.EnglishRiew = TB_TrainingEnglishRiew.Text.Trim();
            tREmployeeTraining.ProfessionalSkillLevel = TB_TrainingProfessionalSkillLevel.Text.Trim();
            tREmployeeTraining.ProfessionSkillNumber = TB_TrainingProfessionSkillNumber.Text.Trim();
            tREmployeeTraining.ReleaseTime = DateTime.Parse(string.IsNullOrEmpty(DLC_TrainingReleaseTime.Text) || DLC_TrainingReleaseTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_TrainingReleaseTime.Text.Trim());
            tREmployeeTraining.Remark = TB_TrainingRemark.Text.Trim();
            tREmployeeTraining.TrainingInfo = TB_TrainingTrainingInfo.Text.Trim();
            tREmployeeTraining.ValidityType = TB_TrainingValidityType.Text.Trim();

            try
            {
                tREmployeeTrainingBLL.UpdateTREmployeeTraining(tREmployeeTraining, tREmployeeTraining.ID);

                UpdateUserInfo(TB_TrainingName.Text.Trim(), ddl_TrainingSex.SelectedValue.Trim(), TB_TrainingNumberNo.Text.Trim(), ddl_TrainingWorkType.SelectedValue.Trim(), string.Empty, LB_TrainingUserCode.Text.Trim());

                LoadTREmployeeTrainingList(tREmployeeTraining.UserCode);

                BT_TrainingUpdate.Visible = true;
                BT_TrainingUpdate.Enabled = true;
                BT_TrainingDelete.Visible = true;
                BT_TrainingDelete.Enabled = true;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_TrainingDelete_Click(object sender, EventArgs e)
    {
        string strHQL = "from TREmployeeTraining as tREmployeeTraining where tREmployeeTraining.ID = '" + LB_TrainingID.Text.Trim() + "' ";
        TREmployeeTrainingBLL tREmployeeTrainingBLL = new TREmployeeTrainingBLL();
        IList lst = tREmployeeTrainingBLL.GetAllTREmployeeTrainings(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TREmployeeTraining tREmployeeTraining = (TREmployeeTraining)lst[0];
            try
            {
                tREmployeeTrainingBLL.DeleteTREmployeeTraining(tREmployeeTraining);

                LoadTREmployeeTrainingList(LB_TrainingUserCode.Text.Trim());

                BT_TrainingUpdate.Visible = true;
                BT_TrainingUpdate.Enabled = true;
                BT_TrainingDelete.Visible = false;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void Btn_OperationSave_Click(object sender, EventArgs e)
    {
        string strID = LB_OperationID.Text.Trim();
        if (string.IsNullOrEmpty(strID) || strID == "")//新增
        {
            TRSpecialOperationsBLL tRSpecialOperationsBLL = new TRSpecialOperationsBLL();
            TRSpecialOperations tRSpecialOperations = new TRSpecialOperations();
            tRSpecialOperations.EnterCode = strUserCodeOld.Trim();
            tRSpecialOperations.Remark = TB_OperationRemark.Text.Trim();
            tRSpecialOperations.SpeOpeNumber = TB_OperationSpeOpeNumber.Text.Trim();
            tRSpecialOperations.SpeOpeProject = TB_OperationSpeOpeProject.Text.Trim();
            tRSpecialOperations.SpeOpeReviewTime = DateTime.Parse(string.IsNullOrEmpty(DLC_OperationSpeOpeReviewTime.Text) || DLC_OperationSpeOpeReviewTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_OperationSpeOpeReviewTime.Text.Trim());
            tRSpecialOperations.SpeOpeStartTime = DateTime.Parse(string.IsNullOrEmpty(DLC_OperationSpeOpeStartTime.Text) || DLC_OperationSpeOpeStartTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_OperationSpeOpeStartTime.Text.Trim());
            tRSpecialOperations.SpeOpeType = TB_OperationSpeOpeType.Text.Trim();
            tRSpecialOperations.UserCode = LB_OperationUserCode.Text.Trim();
            tRSpecialOperations.EnterTime = DateTime.Now;
            try
            {
                tRSpecialOperationsBLL.AddTRSpecialOperations(tRSpecialOperations);
                LB_OperationID.Text = GetTRSpecialOperationsMaxID(tRSpecialOperations.UserCode);

                UpdateUserInfo(TB_OperationName.Text.Trim(), ddl_OperationSex.SelectedValue.Trim(), TB_OperationNumberNo.Text.Trim(), ddl_OperationWorkType.SelectedValue.Trim(), string.Empty, LB_OperationUserCode.Text.Trim());

                LoadTRSpecialOperationsList(tRSpecialOperations.UserCode);

                Btn_OperationSave.Visible = true;
                Btn_OperationSave.Enabled = true;
                Btn_OperationDelete.Visible = true;
                Btn_OperationDelete.Enabled = true;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
            }
        }
        else//更新
        {
            string strHQL = "from TRSpecialOperations as tRSpecialOperations where tRSpecialOperations.ID = " + strID;
            TRSpecialOperationsBLL tRSpecialOperationsBLL = new TRSpecialOperationsBLL();
            IList lst = tRSpecialOperationsBLL.GetAllTRSpecialOperationss(strHQL);
            TRSpecialOperations tRSpecialOperations = (TRSpecialOperations)lst[0];

            tRSpecialOperations.Remark = TB_OperationRemark.Text.Trim();
            tRSpecialOperations.SpeOpeNumber = TB_OperationSpeOpeNumber.Text.Trim();
            tRSpecialOperations.SpeOpeProject = TB_OperationSpeOpeProject.Text.Trim();
            tRSpecialOperations.SpeOpeReviewTime = DateTime.Parse(string.IsNullOrEmpty(DLC_OperationSpeOpeReviewTime.Text) || DLC_OperationSpeOpeReviewTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_OperationSpeOpeReviewTime.Text.Trim());
            tRSpecialOperations.SpeOpeStartTime = DateTime.Parse(string.IsNullOrEmpty(DLC_OperationSpeOpeStartTime.Text) || DLC_OperationSpeOpeStartTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_OperationSpeOpeStartTime.Text.Trim());
            tRSpecialOperations.SpeOpeType = TB_OperationSpeOpeType.Text.Trim();
            try
            {
                tRSpecialOperationsBLL.UpdateTRSpecialOperations(tRSpecialOperations, tRSpecialOperations.ID);

                UpdateUserInfo(TB_OperationName.Text.Trim(), ddl_OperationSex.SelectedValue.Trim(), TB_OperationNumberNo.Text.Trim(), ddl_OperationWorkType.SelectedValue.Trim(), string.Empty, LB_OperationUserCode.Text.Trim());

                LoadTRSpecialOperationsList(tRSpecialOperations.UserCode);

                Btn_OperationSave.Visible = true;
                Btn_OperationSave.Enabled = true;
                Btn_OperationDelete.Visible = true;
                Btn_OperationDelete.Enabled = true;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void Btn_OperationDelete_Click(object sender, EventArgs e)
    {
        string strHQL = "from TRSpecialOperations as tRSpecialOperations where tRSpecialOperations.ID = '" + LB_OperationID.Text.Trim() + "' ";
        TRSpecialOperationsBLL tRSpecialOperationsBLL = new TRSpecialOperationsBLL();
        IList lst = tRSpecialOperationsBLL.GetAllTRSpecialOperationss(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRSpecialOperations tRSpecialOperations = (TRSpecialOperations)lst[0];
            try
            {
                tRSpecialOperationsBLL.DeleteTRSpecialOperations(tRSpecialOperations);

                LoadTRSpecialOperationsList(LB_OperationUserCode.Text.Trim());

                Btn_OperationSave.Visible = true;
                Btn_OperationSave.Enabled = true;
                Btn_OperationDelete.Visible = false;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_EquipmentUpdate_Click(object sender, EventArgs e)
    {
        string strID = LB_EquipmentID.Text.Trim();
        TRSpecialEquipmentBLL tRSpecialEquipmentBLL = new TRSpecialEquipmentBLL();
        if (string.IsNullOrEmpty(strID) || strID == "")//新增
        {
            TRSpecialEquipment tRSpecialEquipment = new TRSpecialEquipment();
            tRSpecialEquipment.EnterCode = strUserCodeOld.Trim();
            tRSpecialEquipment.Remark = TB_EquipmentRemark.Text.Trim();
            tRSpecialEquipment.SpeEquNumber = TB_EquipmentSpeEquNumber.Text.Trim();
            tRSpecialEquipment.SpeEquProject = TB_EquipmentSpeEquProject.Text.Trim();
            tRSpecialEquipment.SpeEquReviewTime = DateTime.Parse(string.IsNullOrEmpty(DLC_EquipmentSpeEquReviewTime.Text) || DLC_EquipmentSpeEquReviewTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_EquipmentSpeEquReviewTime.Text.Trim());
            tRSpecialEquipment.SpeEquStartTime = DateTime.Parse(string.IsNullOrEmpty(DLC_EquipmentSpeEquStartTime.Text) || DLC_EquipmentSpeEquStartTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_EquipmentSpeEquStartTime.Text.Trim());
            tRSpecialEquipment.SpeEquType = TB_EquipmentSpeEquType.Text.Trim();
            tRSpecialEquipment.UserCode = LB_EquipmentUserCode.Text.Trim();
            tRSpecialEquipment.EnterTime = DateTime.Now;
            try
            {
                tRSpecialEquipmentBLL.AddTRSpecialEquipment(tRSpecialEquipment);
                LB_EquipmentID.Text = GetTRSpecialEquipmentMaxID(tRSpecialEquipment.UserCode);

                UpdateUserInfo(TB_EquipmentName.Text.Trim(), ddl_EquipmentSex.SelectedValue.Trim(), TB_EquipmentNumberNo.Text.Trim(), ddl_EquipmentWorkType.SelectedValue.Trim(), string.Empty, LB_EquipmentUserCode.Text.Trim());

                LoadTRSpecialEquipmentList(tRSpecialEquipment.UserCode);

                BT_EquipmentUpdate.Visible = true;
                BT_EquipmentUpdate.Enabled = true;
                BT_EquipmentDelete.Visible = true;
                BT_EquipmentDelete.Enabled = true;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
            }
        }
        else//更新
        {
            string strHQL = "from TRSpecialEquipment as tRSpecialEquipment where tRSpecialEquipment.ID = " + strID;
            IList lst = tRSpecialEquipmentBLL.GetAllTRSpecialEquipments(strHQL);
            TRSpecialEquipment tRSpecialEquipment = (TRSpecialEquipment)lst[0];

            tRSpecialEquipment.Remark = TB_EquipmentRemark.Text.Trim();
            tRSpecialEquipment.SpeEquNumber = TB_EquipmentSpeEquNumber.Text.Trim();
            tRSpecialEquipment.SpeEquProject = TB_EquipmentSpeEquProject.Text.Trim();
            tRSpecialEquipment.SpeEquReviewTime = DateTime.Parse(string.IsNullOrEmpty(DLC_EquipmentSpeEquReviewTime.Text) || DLC_EquipmentSpeEquReviewTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_EquipmentSpeEquReviewTime.Text.Trim());
            tRSpecialEquipment.SpeEquStartTime = DateTime.Parse(string.IsNullOrEmpty(DLC_EquipmentSpeEquStartTime.Text) || DLC_EquipmentSpeEquStartTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_EquipmentSpeEquStartTime.Text.Trim());
            tRSpecialEquipment.SpeEquType = TB_EquipmentSpeEquType.Text.Trim();

            try
            {
                tRSpecialEquipmentBLL.UpdateTRSpecialEquipment(tRSpecialEquipment, tRSpecialEquipment.ID);

                UpdateUserInfo(TB_EquipmentName.Text.Trim(), ddl_EquipmentSex.SelectedValue.Trim(), TB_EquipmentNumberNo.Text.Trim(), ddl_EquipmentWorkType.SelectedValue.Trim(), string.Empty, LB_EquipmentUserCode.Text.Trim());

                LoadTRSpecialEquipmentList(tRSpecialEquipment.UserCode);

                BT_EquipmentUpdate.Visible = true;
                BT_EquipmentUpdate.Enabled = true;
                BT_EquipmentDelete.Visible = true;
                BT_EquipmentDelete.Enabled = true;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_EquipmentDelete_Click(object sender, EventArgs e)
    {
        string strHQL = "from TRSpecialEquipment as tRSpecialEquipment where tRSpecialEquipment.ID = '" + LB_EquipmentID.Text.Trim() + "' ";
        TRSpecialEquipmentBLL tRSpecialEquipmentBLL = new TRSpecialEquipmentBLL();
        IList lst = tRSpecialEquipmentBLL.GetAllTRSpecialEquipments(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRSpecialEquipment tRSpecialEquipment = (TRSpecialEquipment)lst[0];
            try
            {
                tRSpecialEquipmentBLL.DeleteTRSpecialEquipment(tRSpecialEquipment);

                LoadTRSpecialEquipmentList(LB_EquipmentUserCode.Text.Trim());

                BT_EquipmentUpdate.Visible = true;
                BT_EquipmentUpdate.Enabled = true;
                BT_EquipmentDelete.Visible = false;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_PostUpdate_Click(object sender, EventArgs e)
    {
        string strID = LB_PostID.Text.Trim();
        TRPostCertificateBLL tRPostCertificateBLL = new TRPostCertificateBLL();
        if (string.IsNullOrEmpty(strID) || strID == "")//新增
        {
            TRPostCertificate tRPostCertificate = new TRPostCertificate();
            tRPostCertificate.CertificateNo = TB_PostCertificateNo.Text.Trim();
            tRPostCertificate.CertificateOffice = TB_PostCertificateOffice.Text.Trim();
            tRPostCertificate.CertificateReviewTime = DateTime.Parse(string.IsNullOrEmpty(DLC_PostCertificateReviewTime.Text) || DLC_PostCertificateReviewTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_PostCertificateReviewTime.Text.Trim());
            tRPostCertificate.CertificateTime = DateTime.Parse(string.IsNullOrEmpty(DLC_PostCertificateTime.Text) || DLC_PostCertificateTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_PostCertificateTime.Text.Trim());
            tRPostCertificate.EnterCode = strUserCodeOld.Trim();
            tRPostCertificate.Job = TB_PostJob.Text.Trim();
            tRPostCertificate.Remark = TB_PostRemark.Text.Trim();
            tRPostCertificate.Unit = TB_PostUnit.Text.Trim();
            tRPostCertificate.UserCode = LB_PostUserCode.Text.Trim();
            tRPostCertificate.EnterTime = DateTime.Now;
            try
            {
                tRPostCertificateBLL.AddTRPostCertificate(tRPostCertificate);
                LB_PostID.Text = GetTRPostCertificateMaxID(tRPostCertificate.UserCode);

                UpdateUserInfo(TB_PostName.Text.Trim(), ddl_PostSex.SelectedValue.Trim(), TB_PostNumberNo.Text.Trim(), ddl_PostWorkType.SelectedValue.Trim(), DLC_PostBirthDay.Text.Trim(), LB_PostUserCode.Text.Trim());

                LoadTRPostCertificateList(tRPostCertificate.UserCode);

                BT_PostUpdate.Visible = true;
                BT_PostUpdate.Enabled = true;
                BT_PostDelete.Visible = true;
                BT_PostDelete.Enabled = true;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
            }
        }
        else//更新
        {
            string strHQL = "from TRPostCertificate as tRPostCertificate where tRPostCertificate.ID = " + strID;
            IList lst = tRPostCertificateBLL.GetAllTRPostCertificates(strHQL);
            TRPostCertificate tRPostCertificate = (TRPostCertificate)lst[0];

            tRPostCertificate.CertificateNo = TB_PostCertificateNo.Text.Trim();
            tRPostCertificate.CertificateOffice = TB_PostCertificateOffice.Text.Trim();
            tRPostCertificate.CertificateReviewTime = DateTime.Parse(string.IsNullOrEmpty(DLC_PostCertificateReviewTime.Text) || DLC_PostCertificateReviewTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_PostCertificateReviewTime.Text.Trim());
            tRPostCertificate.CertificateTime = DateTime.Parse(string.IsNullOrEmpty(DLC_PostCertificateTime.Text) || DLC_PostCertificateTime.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_PostCertificateTime.Text.Trim());
            tRPostCertificate.Job = TB_PostJob.Text.Trim();
            tRPostCertificate.Remark = TB_PostRemark.Text.Trim();
            tRPostCertificate.Unit = TB_PostUnit.Text.Trim();

            try
            {
                tRPostCertificateBLL.UpdateTRPostCertificate(tRPostCertificate, tRPostCertificate.ID);

                UpdateUserInfo(TB_PostName.Text.Trim(), ddl_PostSex.SelectedValue.Trim(), TB_PostNumberNo.Text.Trim(), ddl_PostWorkType.SelectedValue.Trim(), DLC_PostBirthDay.Text.Trim(), LB_PostUserCode.Text.Trim());

                LoadTRPostCertificateList(tRPostCertificate.UserCode);

                BT_PostUpdate.Visible = true;
                BT_PostUpdate.Enabled = true;
                BT_PostDelete.Visible = true;
                BT_PostDelete.Enabled = true;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_PostDelete_Click(object sender, EventArgs e)
    {
        TRPostCertificateBLL tRPostCertificateBLL = new TRPostCertificateBLL();
        string strHQL = "from TRPostCertificate as tRPostCertificate where tRPostCertificate.ID = '" + LB_PostID.Text.Trim() + "' ";
        IList lst = tRPostCertificateBLL.GetAllTRPostCertificates(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRPostCertificate tRPostCertificate = (TRPostCertificate)lst[0];
            try
            {
                tRPostCertificateBLL.DeleteTRPostCertificate(tRPostCertificate);

                LoadTRPostCertificateList(LB_PostUserCode.Text.Trim());

                BT_PostUpdate.Visible = true;
                BT_PostUpdate.Enabled = true;
                BT_PostDelete.Visible = false;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void LoadTRPostCertificateList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From TRPostCertificate as tRPostCertificate Where tRPostCertificate.UserCode = " + "'" + strUserCode + "'" + " ";
        TRPostCertificateBLL tRPostCertificateBLL = new TRPostCertificateBLL();
        lst = tRPostCertificateBLL.GetAllTRPostCertificates(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRPostCertificate tRPostCertificate = (TRPostCertificate)lst[0];
            LB_PostID.Text = tRPostCertificate.ID.ToString();
            TB_PostCertificateNo.Text = tRPostCertificate.CertificateNo.Trim();
            TB_PostCertificateOffice.Text = tRPostCertificate.CertificateOffice.Trim();
            TB_PostJob.Text = tRPostCertificate.Job.Trim();
            TB_PostRemark.Text = tRPostCertificate.Remark.Trim();
            TB_PostUnit.Text = tRPostCertificate.Unit.Trim();
            DLC_PostCertificateReviewTime.Text = tRPostCertificate.CertificateReviewTime.ToString("yyyy-MM-dd");
            DLC_PostCertificateTime.Text = tRPostCertificate.CertificateTime.ToString("yyyy-MM-dd");

            if (tRPostCertificate.EnterCode.Trim() == strUserCodeOld.Trim())
            {
                BT_PostUpdate.Visible = true;
                BT_PostUpdate.Enabled = true;
                BT_PostDelete.Visible = true;
                BT_PostDelete.Enabled = true;
            }
            else
            {
                BT_PostUpdate.Visible = false;
                BT_PostDelete.Visible = false;
            }
        }
        else
        {
            LB_PostID.Text = "";
            TB_PostCertificateNo.Text = "";
            TB_PostCertificateOffice.Text = "";
            TB_PostJob.Text = "";
            TB_PostRemark.Text = "";
            TB_PostUnit.Text = "";

            BT_PostUpdate.Visible = true;
            BT_PostUpdate.Enabled = true;
            BT_PostDelete.Visible = false;
        }
    }

    protected string GetTRPostCertificateMaxID(string strusercode)
    {
        string flag = "0";
        string strHQL = "from TRPostCertificate as tRPostCertificate where tRPostCertificate.UserCode = '" + strusercode + "' Order By tRPostCertificate.ID Desc ";
        TRPostCertificateBLL tRPostCertificateBLL = new TRPostCertificateBLL();
        IList lst = tRPostCertificateBLL.GetAllTRPostCertificates(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRPostCertificate tRPostCertificate = (TRPostCertificate)lst[0];
            flag = tRPostCertificate.ID.ToString();
        }
        return flag;
    }

    protected void LoadTREmployeeTrainingList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From TRTrainingRecordEmp as tRTrainingRecordEmp Where tRTrainingRecordEmp.UserCode = " + "'" + strUserCode + "'" + " Order By tRTrainingRecordEmp.ID ASC";
        TRTrainingRecordEmpBLL tRTrainingRecordEmpBLL = new TRTrainingRecordEmpBLL();
        lst = tRTrainingRecordEmpBLL.GetAllTRTrainingRecordEmps(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        btn_TrainingAdd.Visible = true;
        btn_TrainingAdd.Enabled = true;

        strHQL = "from TREmployeeTraining as tREmployeeTraining where tREmployeeTraining.UserCode = " + "'" + strUserCode + "'";
        TREmployeeTrainingBLL tREmployeeTrainingBLL = new TREmployeeTrainingBLL();
        lst = tREmployeeTrainingBLL.GetAllTREmployeeTrainings(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TREmployeeTraining tREmployeeTraining = (TREmployeeTraining)lst[0];
            LB_TrainingID.Text = tREmployeeTraining.ID.ToString();
            TB_TrainingAnnCertificateNo.Text = tREmployeeTraining.AnnCertificateNo.Trim();
            TB_TrainingAnnValidTime.Text = tREmployeeTraining.AnnValidTime.Trim();
            TB_TrainingEnglishRiew.Text = tREmployeeTraining.EnglishRiew.Trim();
            TB_TrainingProfessionalSkillLevel.Text = tREmployeeTraining.ProfessionalSkillLevel.Trim();
            TB_TrainingProfessionSkillNumber.Text = tREmployeeTraining.ProfessionSkillNumber.Trim();
            TB_TrainingRemark.Text = tREmployeeTraining.Remark.Trim();
            TB_TrainingTrainingInfo.Text = tREmployeeTraining.TrainingInfo.Trim();
            TB_TrainingValidityType.Text = tREmployeeTraining.ValidityType.Trim();
            DLC_TrainingReleaseTime.Text = tREmployeeTraining.ReleaseTime.ToString("yyyy-MM-dd");

            if (tREmployeeTraining.EnterCode.Trim() == strUserCodeOld.Trim())
            {
                BT_TrainingUpdate.Visible = true;
                BT_TrainingUpdate.Enabled = true;
                BT_TrainingDelete.Visible = true;
                BT_TrainingDelete.Enabled = true;
            }
            else
            {
                BT_TrainingUpdate.Visible = false;
                BT_TrainingDelete.Visible = false;
            }
        }
        else
        {
            LB_TrainingID.Text = "";
            TB_TrainingAnnCertificateNo.Text = "";
            TB_TrainingAnnValidTime.Text = "";
            TB_TrainingEnglishRiew.Text = "";
            TB_TrainingProfessionalSkillLevel.Text = "";
            TB_TrainingProfessionSkillNumber.Text = "";
            TB_TrainingRemark.Text = "";
            TB_TrainingTrainingInfo.Text = "";
            TB_TrainingValidityType.Text = "";

            BT_TrainingUpdate.Visible = true;
            BT_TrainingUpdate.Enabled = true;
            BT_TrainingDelete.Visible = false;
        }
    }

    protected string GetTREmployeeTrainingMaxID(string strusercode)
    {
        string flag = "0";
        string strHQL = "from TREmployeeTraining as tREmployeeTraining where tREmployeeTraining.UserCode = '" + strusercode + "' Order By tREmployeeTraining.ID Desc ";
        TREmployeeTrainingBLL tREmployeeTrainingBLL = new TREmployeeTrainingBLL();
        IList lst = tREmployeeTrainingBLL.GetAllTREmployeeTrainings(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TREmployeeTraining tREmployeeTraining = (TREmployeeTraining)lst[0];
            flag = tREmployeeTraining.ID.ToString();
        }
        return flag;
    }

    protected void LoadTRSpecialOperationsList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from TRSpecialOperations as tRSpecialOperations where tRSpecialOperations.UserCode = " + "'" + strUserCode + "'";
        TRSpecialOperationsBLL tRSpecialOperationsBLL = new TRSpecialOperationsBLL();
        lst = tRSpecialOperationsBLL.GetAllTRSpecialOperationss(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRSpecialOperations tRSpecialOperations = (TRSpecialOperations)lst[0];
            LB_OperationID.Text = tRSpecialOperations.ID.ToString();
            TB_OperationRemark.Text = tRSpecialOperations.Remark.Trim();
            TB_OperationSpeOpeNumber.Text = tRSpecialOperations.SpeOpeNumber.Trim();
            TB_OperationSpeOpeProject.Text = tRSpecialOperations.SpeOpeProject.Trim();
            TB_OperationSpeOpeType.Text = tRSpecialOperations.SpeOpeType.Trim();
            DLC_OperationSpeOpeReviewTime.Text = tRSpecialOperations.SpeOpeReviewTime.ToString("yyyy-MM-dd");
            DLC_OperationSpeOpeStartTime.Text = tRSpecialOperations.SpeOpeStartTime.ToString("yyyy-MM-dd");
            if (tRSpecialOperations.EnterCode.Trim() == strUserCodeOld.Trim())
            {
                Btn_OperationSave.Visible = true;
                Btn_OperationSave.Enabled = true;
                Btn_OperationDelete.Visible = true;
                Btn_OperationDelete.Enabled = true;
            }
            else
            {
                Btn_OperationSave.Visible = false;
                Btn_OperationDelete.Visible = false;
            }
        }
        else
        {
            LB_OperationID.Text = "";
            TB_OperationRemark.Text = "";
            TB_OperationSpeOpeNumber.Text = "";
            TB_OperationSpeOpeProject.Text = "";
            TB_OperationSpeOpeType.Text = "";

            Btn_OperationSave.Visible = true;
            Btn_OperationSave.Enabled = true;
            Btn_OperationDelete.Visible = false;
        }
    }

    protected string GetTRSpecialOperationsMaxID(string strusercode)
    {
        string flag = "0";
        string strHQL = "from TRSpecialOperations as tRSpecialOperations where tRSpecialOperations.UserCode = '" + strusercode + "' Order By tRSpecialOperations.ID Desc ";
        TRSpecialOperationsBLL tRSpecialOperationsBLL = new TRSpecialOperationsBLL();
        IList lst = tRSpecialOperationsBLL.GetAllTRSpecialOperationss(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRSpecialOperations tRSpecialOperations = (TRSpecialOperations)lst[0];
            flag = tRSpecialOperations.ID.ToString();
        }
        return flag;
    }

    protected void LoadTRSpecialEquipmentList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from TRSpecialEquipment as tRSpecialEquipment where tRSpecialEquipment.UserCode = " + "'" + strUserCode + "'";
        TRSpecialEquipmentBLL tRSpecialEquipmentBLL = new TRSpecialEquipmentBLL();
        lst = tRSpecialEquipmentBLL.GetAllTRSpecialEquipments(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRSpecialEquipment tRSpecialEquipment = (TRSpecialEquipment)lst[0];
            LB_EquipmentID.Text = tRSpecialEquipment.ID.ToString();
            TB_EquipmentRemark.Text = tRSpecialEquipment.Remark.Trim();
            TB_EquipmentSpeEquNumber.Text = tRSpecialEquipment.SpeEquNumber.Trim();
            TB_EquipmentSpeEquProject.Text = tRSpecialEquipment.SpeEquProject.Trim();
            TB_EquipmentSpeEquType.Text = tRSpecialEquipment.SpeEquType.Trim();
            DLC_EquipmentSpeEquReviewTime.Text = tRSpecialEquipment.SpeEquReviewTime.ToString("yyyy-MM-dd");
            DLC_EquipmentSpeEquStartTime.Text = tRSpecialEquipment.SpeEquStartTime.ToString("yyyy-MM-dd");

            if (tRSpecialEquipment.EnterCode.Trim() == strUserCodeOld.Trim())
            {
                BT_EquipmentUpdate.Visible = true;
                BT_EquipmentUpdate.Enabled = true;
                BT_EquipmentDelete.Visible = true;
                BT_EquipmentDelete.Enabled = true;
            }
            else
            {
                BT_EquipmentUpdate.Visible = false;
                BT_EquipmentDelete.Visible = false;
            }
        }
        else
        {
            LB_EquipmentID.Text = "";
            TB_EquipmentRemark.Text = "";
            TB_EquipmentSpeEquNumber.Text = "";
            TB_EquipmentSpeEquProject.Text = "";
            TB_EquipmentSpeEquType.Text = "";

            BT_EquipmentUpdate.Visible = true;
            BT_EquipmentUpdate.Enabled = true;
            BT_EquipmentDelete.Visible = false;
        }
    }

    protected string GetTRSpecialEquipmentMaxID(string strusercode)
    {
        string flag = "0";
        string strHQL = "from TRSpecialEquipment as tRSpecialEquipment where tRSpecialEquipment.UserCode = '" + strusercode + "' Order By tRSpecialEquipment.ID Desc ";
        TRSpecialEquipmentBLL tRSpecialEquipmentBLL = new TRSpecialEquipmentBLL();
        IList lst = tRSpecialEquipmentBLL.GetAllTRSpecialEquipments(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRSpecialEquipment tRSpecialEquipment = (TRSpecialEquipment)lst[0];
            flag = tRSpecialEquipment.ID.ToString();
        }
        return flag;
    }

    protected void LoadTRHolderWelderList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From TRHolderWelder as tRHolderWelder Where tRHolderWelder.UserCode = " + "'" + strUserCode + "'" + " Order By tRHolderWelder.ID ASC";
        TRHolderWelderBLL tRHolderWelderBLL = new TRHolderWelderBLL();
        lst = tRHolderWelderBLL.GetAllTRHolderWelders(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        if (lst.Count > 0 && lst != null)
        {
            TRHolderWelder tRHolderWelder = (TRHolderWelder)lst[0];
            LB_HolderID.Text = tRHolderWelder.ID.ToString();
            TB_HolderCertificateNo.Text = string.IsNullOrEmpty(tRHolderWelder.CertificateNo) ? "" : tRHolderWelder.CertificateNo.Trim();
            TB_HolderRemark.Text = string.IsNullOrEmpty(tRHolderWelder.Remark) ? "" : tRHolderWelder.Remark.Trim();
            TB_HolderUnit.Text = string.IsNullOrEmpty(tRHolderWelder.Unit) ? "" : tRHolderWelder.Unit.Trim();
            TB_HolderWelderSeal.Text = string.IsNullOrEmpty(tRHolderWelder.WelderSeal) ? "" : tRHolderWelder.WelderSeal.Trim();
            TB_HolderProject.Text = "";
            TB_HolderValidTime.Text = "";

            if (tRHolderWelder.EnterCode.Trim() == strUserCodeOld.Trim())
            {
                BT_HolderUpdate.Visible = true;
                BT_HolderUpdate.Enabled = true;
                BT_HolderDelete.Visible = true;
                BT_HolderDelete.Enabled = true;
                BT_ProjectAdd.Visible = true;
                BT_ProjectAdd.Enabled = true;
                BT_ProjectDelete.Visible = false;
                BT_ProjectUpdate.Visible = false;
            }
            else
            {
                BT_HolderUpdate.Visible = false;
                BT_HolderDelete.Visible = false;
                BT_ProjectAdd.Visible = false;
                BT_ProjectDelete.Visible = false;
                BT_ProjectUpdate.Visible = false;
            }
        }
        else
        {
            LB_HolderID.Text = "";
            TB_HolderCertificateNo.Text = "";
            TB_HolderRemark.Text = "";
            TB_HolderUnit.Text = "";
            TB_HolderWelderSeal.Text = "";
            TB_HolderProject.Text = "";
            TB_HolderValidTime.Text = "";

            BT_HolderUpdate.Visible = true;
            BT_HolderUpdate.Enabled = true;
            BT_HolderDelete.Visible = false;
            BT_ProjectAdd.Visible = true;
            BT_ProjectAdd.Enabled = true;
            BT_ProjectDelete.Visible = false;
            BT_ProjectUpdate.Visible = false;
        }
    }

    protected string GetTRHolderWelderMaxID(string strusercode)
    {
        string flag = "0";
        string strHQL = "from TRHolderWelder as tRHolderWelder where tRHolderWelder.UserCode = '" + strusercode + "' Order By tRHolderWelder.ID Desc ";
        TRHolderWelderBLL tRHolderWelderBLL = new TRHolderWelderBLL();
        IList lst = tRHolderWelderBLL.GetAllTRHolderWelders(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRHolderWelder tRHolderWelder = (TRHolderWelder)lst[0];
            flag = tRHolderWelder.ID.ToString();
        }
        return flag;
    }

    /// <summary>
    /// 判断持证项目是否是最后一条  若是，返回true；否则返回false
    /// </summary>
    /// <param name="strusercode"></param>
    /// <returns></returns>
    protected bool IsLastTRHolderWelder(string strusercode)
    {
        bool flag = true;
        string strHQL = "from TRHolderWelder as tRHolderWelder where tRHolderWelder.UserCode = '" + strusercode + "' ";
        TRHolderWelderBLL tRHolderWelderBLL = new TRHolderWelderBLL();
        IList lst = tRHolderWelderBLL.GetAllTRHolderWelders(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag = false;
        }
        return flag;
    }

    protected void LoadProjectMember(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName;

        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }

    protected void BT_ProjectAdd_Click(object sender, EventArgs e)
    {
        string strBookImage;
        string strImage = UploadBookDesign().Trim();
        if (strImage.Equals("0"))//图样为空
        {
            strBookImage = "";
        }
        else if (strImage.Equals("1"))//存在同名文件，上传失败，请改名后再上传！
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGCZTMSMJSMJSCSBGMHZSC").ToString().Trim()+"')", true);
            return;
        }
        else if (strImage.Equals("2"))//上传失败，请检查！
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGSCSBJC").ToString().Trim()+"')", true);
            return;
        }
        else
            strBookImage = strImage;

        TRHolderWelderBLL tRHolderWelderBLL = new TRHolderWelderBLL();
        TRHolderWelder tRHolderWelder = new TRHolderWelder();
        tRHolderWelder.EnterCode = strUserCodeOld.Trim();
        tRHolderWelder.HolderProject = TB_HolderProject.Text.Trim();
        tRHolderWelder.UserCode = LB_HolderUserCode.Text.Trim();
        tRHolderWelder.ValidTime = TB_HolderValidTime.Text.Trim();
        tRHolderWelder.EnterTime = DateTime.Now;
        tRHolderWelder.AttachPath = strBookImage;

        try
        {
            string strHQL = "from TRHolderWelder as tRHolderWelder where tRHolderWelder.UserCode = '" + tRHolderWelder.UserCode + "' ";
            IList lst = tRHolderWelderBLL.GetAllTRHolderWelders(strHQL);
            if (lst.Count == 1)
            {
                TRHolderWelder tRHolderWelder1 = (TRHolderWelder)lst[0];
                tRHolderWelder1.HolderProject = tRHolderWelder.HolderProject;
                tRHolderWelder1.ValidTime = tRHolderWelder.ValidTime;
                tRHolderWelder1.AttachPath = tRHolderWelder.AttachPath;
                tRHolderWelderBLL.UpdateTRHolderWelder(tRHolderWelder1, tRHolderWelder1.ID);
            }
            else
            {
                tRHolderWelderBLL.AddTRHolderWelder(tRHolderWelder);
                LB_HolderID.Text = GetTRHolderWelderMaxID(tRHolderWelder.UserCode);
            }

            LoadTRHolderWelderList(tRHolderWelder.UserCode);

            BT_ProjectAdd.Visible = true;
            BT_ProjectAdd.Enabled = true;
            BT_ProjectUpdate.Visible = true;
            BT_ProjectUpdate.Enabled = true;
            BT_ProjectDelete.Visible = true;
            BT_ProjectDelete.Enabled = true;
            BT_HolderUpdate.Visible = true;
            BT_HolderUpdate.Enabled = true;
            BT_HolderDelete.Visible = true;
            BT_HolderDelete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCZXMZJCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCZXMZJSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_ProjectUpdate_Click(object sender, EventArgs e)
    {
        string strBookImage;
        string strImage = UploadBookDesign().Trim();
        if (strImage.Equals("0"))//图样为空
        {
            strBookImage = lbl_AttachPath.Text.Trim();
        }
        else if (strImage.Equals("1"))//存在同名文件，上传失败，请改名后再上传！
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGCZTMSMJSMJSCSBGMHZSC").ToString().Trim()+"')", true);
            return;
        }
        else if (strImage.Equals("2"))//上传失败，请检查！
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGSCSBJC").ToString().Trim()+"')", true);
            return;
        }
        else
            strBookImage = strImage;

        TRHolderWelderBLL tRHolderWelderBLL = new TRHolderWelderBLL();
        string strHQL = "from TRHolderWelder as tRHolderWelder where tRHolderWelder.ID = '" + LB_HolderID.Text.Trim() + "' ";
        IList lst = tRHolderWelderBLL.GetAllTRHolderWelders(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRHolderWelder tRHolderWelder = (TRHolderWelder)lst[0];
            tRHolderWelder.HolderProject = TB_HolderProject.Text.Trim();
            tRHolderWelder.ValidTime = TB_HolderValidTime.Text.Trim();
            tRHolderWelder.AttachPath = strBookImage;
            try
            {
                tRHolderWelderBLL.UpdateTRHolderWelder(tRHolderWelder, tRHolderWelder.ID);
                LoadTRHolderWelderList(tRHolderWelder.UserCode);

                BT_ProjectAdd.Visible = true;
                BT_ProjectAdd.Enabled = true;
                BT_ProjectUpdate.Visible = true;
                BT_ProjectUpdate.Enabled = true;
                BT_ProjectDelete.Visible = true;
                BT_ProjectDelete.Enabled = true;
                BT_HolderUpdate.Visible = true;
                BT_HolderUpdate.Enabled = true;
                BT_HolderDelete.Visible = true;
                BT_HolderDelete.Enabled = true;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCZXMGXCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCZXMGXSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_ProjectDelete_Click(object sender, EventArgs e)
    {
        TRHolderWelderBLL tRHolderWelderBLL = new TRHolderWelderBLL();
        string strHQL = "from TRHolderWelder as tRHolderWelder where tRHolderWelder.ID = '" + LB_HolderID.Text.Trim() + "' ";
        IList lst = tRHolderWelderBLL.GetAllTRHolderWelders(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRHolderWelder tRHolderWelder = (TRHolderWelder)lst[0];
            try
            {
                tRHolderWelderBLL.DeleteTRHolderWelder(tRHolderWelder);
                LoadTRHolderWelderList(LB_HolderUserCode.Text.Trim());
                BT_ProjectAdd.Visible = true;
                BT_ProjectAdd.Enabled = true;
                BT_ProjectUpdate.Visible = false;
                BT_ProjectDelete.Visible = false;
                BT_HolderUpdate.Visible = true;
                BT_HolderUpdate.Enabled = true;
                if (IsLastTRHolderWelder(LB_HolderUserCode.Text.Trim()))
                {
                    BT_HolderDelete.Visible = false;
                }
                else
                {
                    BT_HolderDelete.Visible = true;
                    BT_HolderDelete.Enabled = true;
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZChiZhengXiangMuLanguageHandl").ToString().Trim()+"')", true); 
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZChiZhengXiangMuLanguageHandl").ToString().Trim()+"')", true); 
            }
        }
    }

    protected void BT_HolderUpdate_Click(object sender, EventArgs e)
    {
        string strID = LB_HolderID.Text.Trim();
        TRHolderWelderBLL tRHolderWelderBLL = new TRHolderWelderBLL();
        if (string.IsNullOrEmpty(strID) || strID == "")
        {
            TRHolderWelder tRHolderWelder = new TRHolderWelder();
            tRHolderWelder.CertificateNo = TB_HolderCertificateNo.Text.Trim();
            tRHolderWelder.EnterCode = strUserCodeOld.Trim();
            tRHolderWelder.Remark = TB_HolderRemark.Text.Trim();
            tRHolderWelder.Unit = TB_HolderUnit.Text.Trim();
            tRHolderWelder.UserCode = LB_HolderUserCode.Text.Trim();
            tRHolderWelder.WelderSeal = TB_HolderWelderSeal.Text.Trim();
            tRHolderWelder.EnterTime = DateTime.Now;
            try
            {
                tRHolderWelderBLL.AddTRHolderWelder(tRHolderWelder);
                LB_HolderID.Text = GetTRHolderWelderMaxID(tRHolderWelder.UserCode);

                UpdateUserInfo(TB_HolderName.Text.Trim(), ddl_HolderSex.SelectedValue.Trim(), TB_HolderNumberNo.Text.Trim(), string.Empty, string.Empty, LB_HolderUserCode.Text.Trim());

                LoadTRHolderWelderList(tRHolderWelder.UserCode);

                BT_ProjectAdd.Visible = true;
                BT_ProjectAdd.Enabled = true;
                BT_ProjectUpdate.Visible = false;
                BT_ProjectDelete.Visible = false;
                BT_HolderUpdate.Visible = true;
                BT_HolderUpdate.Enabled = true;
                BT_HolderDelete.Visible = true;
                BT_HolderDelete.Enabled = true;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
            }
        }
        else
        {
            string strHQL = "from TRHolderWelder as tRHolderWelder where tRHolderWelder.UserCode = '" + LB_HolderUserCode.Text.Trim() + "' ";
            IList lst = tRHolderWelderBLL.GetAllTRHolderWelders(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    TRHolderWelder tRHolderWelder = (TRHolderWelder)lst[i];
                    tRHolderWelder.CertificateNo = TB_HolderCertificateNo.Text.Trim();
                    tRHolderWelder.Remark = TB_HolderRemark.Text.Trim();
                    tRHolderWelder.Unit = TB_HolderUnit.Text.Trim();
                    tRHolderWelder.WelderSeal = TB_HolderWelderSeal.Text.Trim();

                    tRHolderWelderBLL.UpdateTRHolderWelder(tRHolderWelder, tRHolderWelder.ID);

                    UpdateUserInfo(TB_HolderName.Text.Trim(), ddl_HolderSex.SelectedValue.Trim(), TB_HolderNumberNo.Text.Trim(), string.Empty, string.Empty, LB_HolderUserCode.Text.Trim());

                    LoadTRHolderWelderList(tRHolderWelder.UserCode);

                    BT_ProjectAdd.Visible = true;
                    BT_ProjectAdd.Enabled = true;
                    BT_ProjectUpdate.Visible = false;
                    BT_ProjectDelete.Visible = false;
                    BT_HolderUpdate.Visible = true;
                    BT_HolderUpdate.Enabled = true;
                    BT_HolderDelete.Visible = true;
                    BT_HolderDelete.Enabled = true;

                    continue;
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_HolderDelete_Click(object sender, EventArgs e)
    {
        TRHolderWelderBLL tRHolderWelderBLL = new TRHolderWelderBLL();
        string strHQL = "from TRHolderWelder as tRHolderWelder where tRHolderWelder.UserCode = '" + LB_HolderUserCode.Text.Trim() + "' ";
        IList lst = tRHolderWelderBLL.GetAllTRHolderWelders(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                TRHolderWelder tRHolderWelder = (TRHolderWelder)lst[i];
                tRHolderWelderBLL.DeleteTRHolderWelder(tRHolderWelder);

                BT_ProjectAdd.Visible = true;
                BT_ProjectAdd.Enabled = true;
                BT_ProjectUpdate.Visible = false;
                BT_ProjectDelete.Visible = false;
                BT_HolderUpdate.Visible = true;
                BT_HolderUpdate.Enabled = true;
                BT_HolderDelete.Visible = false;

                continue;
            }
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
    }

    protected void UpdateUserInfo(string strname, string strsex, string strnumberno, string strworktype, string birthday, string strusercode)
    {
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = '" + strusercode + "' ";
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];
            if (!string.IsNullOrEmpty(strname) && strname != "")
            {
                projectMember.UserName = strname;
            }
            if (!string.IsNullOrEmpty(strsex) && strsex != "")
            {
                projectMember.Gender = strsex;
            }
            if (!string.IsNullOrEmpty(strnumberno) && strnumberno != "")
            {
                projectMember.IDCard = strnumberno;
            }
            if (!string.IsNullOrEmpty(strworktype) && strworktype != "")
            {
                projectMember.WorkType = strworktype;
            }
            if (!string.IsNullOrEmpty(birthday) && birthday != "")
            {
                projectMember.BirthDay = DateTime.Parse(birthday);
                projectMember.Age = DateTime.Now.Year - DateTime.Parse(birthday).Year;
            }
            projectMemberBLL.UpdateProjectMember(projectMember, projectMember.UserCode.Trim().ToUpper());
        }
    }

    protected void btn_ExcelToDataTraining_Click(object sender, EventArgs e)
    {
        if (FileUpload_Training.HasFile == false)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim();
            return;
        }
        string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
        if (IsXls != ".xls" & IsXls != ".xlsx")
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim();
            return;
        }

        TREmployeeTrainingBLL tREmployeeTrainingBLL = new TREmployeeTrainingBLL();
        TREmployeeTraining tREmployeeTraining = new TREmployeeTraining();

        string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
        string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
        string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCodeOld + "\\Doc\\";
        FileInfo fi = new FileInfo(strDocSavePath + newfilename);
        if (fi.Exists)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim();
        }
        else
        {
            FileUpload_Training.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
            string strpath = strDocSavePath + newfilename;

            //DataSet ds = ExcelToDataSet(strpath, filename);
            //DataRow[] dr = ds.Tables[0].Select();
            //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
            //int rowsnum = ds.Tables[0].Rows.Count;

            DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
            DataRow[] dr = dt.Select();                        //定义一个DataRow数组
            int rowsnum = dt.Rows.Count;
            if (rowsnum == 0)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim();
            }
            else
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    tREmployeeTraining.AnnCertificateNo = dr[i][LanguageHandle.GetWord("AnKongZhengShuBianHao").ToString().Trim()].ToString().Trim();
                    tREmployeeTraining.AnnValidTime = dr[i][LanguageHandle.GetWord("AnKongYouXiaoJi").ToString().Trim()].ToString().Trim();
                    tREmployeeTraining.EnglishRiew = dr[i][LanguageHandle.GetWord("SheWaiYingYuKaoHe").ToString().Trim()].ToString().Trim();
                    tREmployeeTraining.EnterCode = strUserCodeOld.Trim();
                    tREmployeeTraining.ProfessionalSkillLevel = dr[i][LanguageHandle.GetWord("ZhiYeJiNengDengJi").ToString().Trim()].ToString().Trim();
                    tREmployeeTraining.ProfessionSkillNumber = dr[i][LanguageHandle.GetWord("ZhiYeJiNengJianDingZhengShuBia").ToString().Trim()].ToString().Trim();
                    tREmployeeTraining.ReleaseTime = DateTime.Parse(string.IsNullOrEmpty(dr[i]["LicenseIssuanceTime"].ToString()) ? DateTime.Now.ToString() : dr[i]["LicenseIssuanceTime"].ToString());   //ChineseWord
                    tREmployeeTraining.Remark = dr[i][LanguageHandle.GetWord("BeiZhu").ToString().Trim()].ToString().Trim();
                    tREmployeeTraining.TrainingInfo = dr[i][LanguageHandle.GetWord("PeiXunXiangGuanXinXi").ToString().Trim()].ToString().Trim();
                    tREmployeeTraining.UserCode = string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("YuanGongDaiMa").ToString().Trim()].ToString()) ? GetUserCode(dr[i][LanguageHandle.GetWord("YuanGongXingMing").ToString().Trim()].ToString().Trim()) : dr[i][LanguageHandle.GetWord("YuanGongDaiMa").ToString().Trim()].ToString().Trim();
                    tREmployeeTraining.ValidityType = dr[i]["AppraisedOccupation"].ToString().Trim();   //ChineseWord
                    tREmployeeTraining.EnterTime = DateTime.Now;

                    try
                    {
                        string strID = "0";
                        if (IsTREmployeeTraining(tREmployeeTraining.UserCode, ref strID))
                        {
                            string strHQL = "from TREmployeeTraining as tREmployeeTraining where tREmployeeTraining.ID = '" + strID + "' ";
                            IList lst = tREmployeeTrainingBLL.GetAllTREmployeeTrainings(strHQL);
                            TREmployeeTraining tREmployeeTraining1 = (TREmployeeTraining)lst[0];
                            tREmployeeTraining1.AnnCertificateNo = tREmployeeTraining.AnnCertificateNo;
                            tREmployeeTraining1.AnnValidTime = tREmployeeTraining.AnnValidTime;
                            tREmployeeTraining1.EnglishRiew = tREmployeeTraining.EnglishRiew;
                            tREmployeeTraining1.EnterCode = tREmployeeTraining.EnterCode;
                            tREmployeeTraining1.ProfessionalSkillLevel = tREmployeeTraining.ProfessionalSkillLevel;
                            tREmployeeTraining1.ProfessionSkillNumber = tREmployeeTraining.ProfessionSkillNumber;
                            tREmployeeTraining1.ReleaseTime = tREmployeeTraining.ReleaseTime;
                            tREmployeeTraining1.Remark = tREmployeeTraining.Remark;
                            tREmployeeTraining1.TrainingInfo = tREmployeeTraining.TrainingInfo;
                            tREmployeeTraining1.ValidityType = tREmployeeTraining.ValidityType;

                            tREmployeeTrainingBLL.UpdateTREmployeeTraining(tREmployeeTraining1, tREmployeeTraining1.ID);
                        }
                        else
                        {
                            tREmployeeTrainingBLL.AddTREmployeeTraining(tREmployeeTraining);
                        }
                    }
                    catch
                    {
                        string Msg = LanguageHandle.GetWord("JingGaoExcelBiaoShuJuDaoRuShiB").ToString().Trim();
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZMSG").ToString().Trim()+"')", true);
                        return;
                    }
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZEXCLEBDRCG").ToString().Trim()+"')", true);
                LoadTREmployeeTrainingList(LB_TrainingUserCode.Text.Trim());
            }
        }
    }

    protected void btn_ExcelToDataOperation_Click(object sender, EventArgs e)
    {
        if (InputFile_Operation.HasFile == false)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim();
            return;
        }
        string IsXls = System.IO.Path.GetExtension(InputFile_Operation.FileName).ToString().ToLower();
        if (IsXls != ".xls" & IsXls != ".xlsx")
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim();
            return;
        }

        TRSpecialOperationsBLL tRSpecialOperationsBLL = new TRSpecialOperationsBLL();
        TRSpecialOperations tRSpecialOperations = new TRSpecialOperations();

        string filename = InputFile_Operation.FileName.ToString();  //获取Execle文件名
        string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
        string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCodeOld + "\\Doc\\";
        FileInfo fi = new FileInfo(strDocSavePath + newfilename);
        if (fi.Exists)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim();
        }
        else
        {
            InputFile_Operation.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
            string strpath = strDocSavePath + newfilename;

            //DataSet ds = ExcelToDataSet(strpath, filename);
            //DataRow[] dr = ds.Tables[0].Select();
            //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
            //int rowsnum = ds.Tables[0].Rows.Count;

            DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
            DataRow[] dr = dt.Select();                        //定义一个DataRow数组
            int rowsnum = dt.Rows.Count;
            if (rowsnum == 0)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim();
            }
            else
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    tRSpecialOperations.EnterCode = strUserCodeOld.Trim();
                    tRSpecialOperations.Remark = dr[i][LanguageHandle.GetWord("BeiZhu").ToString().Trim()].ToString().Trim();
                    tRSpecialOperations.SpeOpeNumber = dr[i][LanguageHandle.GetWord("TeChongZuoYeZhengShuBianHao").ToString().Trim()].ToString().Trim();
                    tRSpecialOperations.SpeOpeProject = dr[i][LanguageHandle.GetWord("TeChongZuoYeZhunCaoXiangMu").ToString().Trim()].ToString().Trim();
                    tRSpecialOperations.SpeOpeReviewTime = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("TeChongZuoYeFuShenShiJian").ToString().Trim()].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("TeChongZuoYeFuShenShiJian").ToString().Trim()].ToString());
                    tRSpecialOperations.SpeOpeStartTime = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("TeChongZuoYeQuZhengShiJian").ToString().Trim()].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("TeChongZuoYeQuZhengShiJian").ToString().Trim()].ToString());
                    tRSpecialOperations.SpeOpeType = dr[i]["SpecialOperationsCategory"].ToString().Trim();   //ChineseWord
                    tRSpecialOperations.UserCode = string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("YuanGongDaiMa").ToString().Trim()].ToString()) ? GetUserCode(dr[i][LanguageHandle.GetWord("YuanGongXingMing").ToString().Trim()].ToString().Trim()) : dr[i][LanguageHandle.GetWord("YuanGongDaiMa").ToString().Trim()].ToString().Trim();
                    tRSpecialOperations.EnterTime = DateTime.Now;
                    try
                    {
                        string strID = "0";
                        if (IsTRSpecialOperations(tRSpecialOperations.UserCode, ref strID))
                        {
                            string strHQL = "from TRSpecialOperations as tRSpecialOperations where tRSpecialOperations.ID = '" + strID + "' ";
                            IList lst = tRSpecialOperationsBLL.GetAllTRSpecialOperationss(strHQL);
                            TRSpecialOperations tRSpecialOperations1 = (TRSpecialOperations)lst[0];
                            tRSpecialOperations1.SpeOpeNumber = tRSpecialOperations.SpeOpeNumber;
                            tRSpecialOperations1.SpeOpeProject = tRSpecialOperations.SpeOpeProject;
                            tRSpecialOperations1.SpeOpeReviewTime = tRSpecialOperations.SpeOpeReviewTime;
                            tRSpecialOperations1.EnterCode = tRSpecialOperations.EnterCode;
                            tRSpecialOperations1.SpeOpeStartTime = tRSpecialOperations.SpeOpeStartTime;
                            tRSpecialOperations1.SpeOpeType = tRSpecialOperations.SpeOpeType;
                            tRSpecialOperations1.Remark = tRSpecialOperations.Remark;

                            tRSpecialOperationsBLL.UpdateTRSpecialOperations(tRSpecialOperations1, tRSpecialOperations1.ID);
                        }
                        else
                        {
                            tRSpecialOperationsBLL.AddTRSpecialOperations(tRSpecialOperations);
                        }
                    }
                    catch
                    {
                        string Msg = LanguageHandle.GetWord("JingGaoExcelBiaoShuJuDaoRuShiB").ToString().Trim();
                        LB_ErrorText.Text += LanguageHandle.GetWord("ZZMSG").ToString().Trim();
                        return;
                    }
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZEXCLEBDRCG").ToString().Trim()+"')", true);
                LoadTRSpecialOperationsList(LB_OperationUserCode.Text.Trim());
            }
        }
    }

    protected void btn_ExcelToDataEquipment_Click(object sender, EventArgs e)
    {
        if (InputFile_Equipment.HasFile == false)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim();
            return;
        }
        string IsXls = System.IO.Path.GetExtension(InputFile_Equipment.FileName).ToString().ToLower();
        if (IsXls != ".xls" & IsXls != ".xlsx")
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim();
            return;
        }

        TRSpecialEquipmentBLL tRSpecialEquipmentBLL = new TRSpecialEquipmentBLL();
        TRSpecialEquipment tRSpecialEquipment = new TRSpecialEquipment();

        string filename = InputFile_Equipment.FileName.ToString();  //获取Execle文件名
        string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
        string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCodeOld + "\\Doc\\";
        FileInfo fi = new FileInfo(strDocSavePath + newfilename);
        if (fi.Exists)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim();
        }
        else
        {
            InputFile_Equipment.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
            string strpath = strDocSavePath + newfilename;

            //DataSet ds = ExcelToDataSet(strpath, filename);
            //DataRow[] dr = ds.Tables[0].Select();
            //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
            //int rowsnum = ds.Tables[0].Rows.Count;

            DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
            DataRow[] dr = dt.Select();                        //定义一个DataRow数组
            int rowsnum = dt.Rows.Count;
            if (rowsnum == 0)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim();
            }
            else
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    tRSpecialEquipment.Remark = dr[i][LanguageHandle.GetWord("BeiZhu").ToString().Trim()].ToString().Trim();
                    tRSpecialEquipment.SpeEquNumber = dr[i][LanguageHandle.GetWord("TeChongSheBeiZhengShuBianHao").ToString().Trim()].ToString().Trim();
                    tRSpecialEquipment.EnterCode = strUserCodeOld.Trim();
                    tRSpecialEquipment.SpeEquProject = dr[i][LanguageHandle.GetWord("TeChongSheBeiZhunCaoXiangMu").ToString().Trim()].ToString().Trim();
                    tRSpecialEquipment.SpeEquReviewTime = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("TeChongSheBeiFuShenShiJian").ToString().Trim()].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("TeChongSheBeiFuShenShiJian").ToString().Trim()].ToString());
                    tRSpecialEquipment.SpeEquStartTime = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("TeChongSheBeiQuZhengShiJian").ToString().Trim()].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("TeChongSheBeiQuZhengShiJian").ToString().Trim()].ToString());
                    tRSpecialEquipment.SpeEquType = dr[i]["SpecialEquipmentOperationsCategory"].ToString().Trim();   //ChineseWord
                    tRSpecialEquipment.UserCode = string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("YuanGongDaiMa").ToString().Trim()].ToString()) ? GetUserCode(dr[i][LanguageHandle.GetWord("YuanGongXingMing").ToString().Trim()].ToString().Trim()) : dr[i][LanguageHandle.GetWord("YuanGongDaiMa").ToString().Trim()].ToString().Trim();
                    tRSpecialEquipment.EnterTime = DateTime.Now;
                    try
                    {
                        string strID = "0";
                        if (IsTRSpecialEquipment(tRSpecialEquipment.UserCode, ref strID))
                        {
                            string strHQL = "from TRSpecialEquipment as tRSpecialEquipment where tRSpecialEquipment.ID = '" + strID + "' ";
                            IList lst = tRSpecialEquipmentBLL.GetAllTRSpecialEquipments(strHQL);
                            TRSpecialEquipment tRSpecialEquipment1 = (TRSpecialEquipment)lst[0];
                            tRSpecialEquipment1.SpeEquNumber = tRSpecialEquipment.SpeEquNumber;
                            tRSpecialEquipment1.SpeEquProject = tRSpecialEquipment.SpeEquProject;
                            tRSpecialEquipment1.SpeEquReviewTime = tRSpecialEquipment.SpeEquReviewTime;
                            tRSpecialEquipment1.EnterCode = tRSpecialEquipment.EnterCode;
                            tRSpecialEquipment1.SpeEquStartTime = tRSpecialEquipment.SpeEquStartTime;
                            tRSpecialEquipment1.SpeEquType = tRSpecialEquipment.SpeEquType;
                            tRSpecialEquipment1.Remark = tRSpecialEquipment.Remark;

                            tRSpecialEquipmentBLL.UpdateTRSpecialEquipment(tRSpecialEquipment1, tRSpecialEquipment1.ID);
                        }
                        else
                        {
                            tRSpecialEquipmentBLL.AddTRSpecialEquipment(tRSpecialEquipment);
                        }
                    }
                    catch
                    {
                        LB_ErrorText.Text += LanguageHandle.GetWord("ZZMSG").ToString().Trim();
                        return;
                    }
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZEXCLEBDRCG").ToString().Trim()+"')", true);
                LoadTRSpecialEquipmentList(LB_EquipmentUserCode.Text.Trim());
            }
        }
    }

    protected void btn_ExcelToDataHolder_Click(object sender, EventArgs e)
    {
        if (InputFile_Holder.HasFile == false)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim();
            return;
        }
        string IsXls = System.IO.Path.GetExtension(InputFile_Holder.FileName).ToString().ToLower();
        if (IsXls != ".xls" & IsXls != ".xlsx")
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim();
            return;
        }

        TRHolderWelderBLL tRHolderWelderBLL = new TRHolderWelderBLL();
        TRHolderWelder tRHolderWelder = new TRHolderWelder();

        string filename = InputFile_Holder.FileName.ToString();  //获取Execle文件名
        string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
        string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCodeOld + "\\Doc\\";
        FileInfo fi = new FileInfo(strDocSavePath + newfilename);
        if (fi.Exists)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim();
        }
        else
        {
            InputFile_Holder.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
            string strpath = strDocSavePath + newfilename;

            //DataSet ds = ExcelToDataSet(strpath, filename);
            //DataRow[] dr = ds.Tables[0].Select();
            //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
            //int rowsnum = ds.Tables[0].Rows.Count;

            DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
            DataRow[] dr = dt.Select();                        //定义一个DataRow数组
            int rowsnum = dt.Rows.Count;
            if (rowsnum == 0)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim();
            }
            else
            {
                string doubleData = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    tRHolderWelder.CertificateNo = dr[i][LanguageHandle.GetWord("TeChongSheBeiHanJieCaoZuoRenYu").ToString().Trim()].ToString().Trim();
                    tRHolderWelder.HolderProject = dr[i][LanguageHandle.GetWord("ChiZhengXiangMu").ToString().Trim()].ToString().Trim();
                    tRHolderWelder.Unit = dr[i][LanguageHandle.GetWord("ChanWei").ToString().Trim()].ToString().Trim();
                    tRHolderWelder.EnterCode = strUserCodeOld.Trim();
                    tRHolderWelder.ValidTime = dr[i][LanguageHandle.GetWord("ChiZhengXiangMuYouXiaoJi").ToString().Trim()].ToString().Trim();
                    tRHolderWelder.WelderSeal = dr[i][LanguageHandle.GetWord("HanGongGangYin").ToString().Trim()].ToString().Trim();
                    tRHolderWelder.Remark = dr[i][LanguageHandle.GetWord("BeiZhu").ToString().Trim()].ToString().Trim();
                    tRHolderWelder.UserCode = string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("YuanGongDaiMa").ToString().Trim()].ToString()) ? GetUserCode(dr[i][LanguageHandle.GetWord("YuanGongXingMing").ToString().Trim()].ToString().Trim()) : dr[i][LanguageHandle.GetWord("YuanGongDaiMa").ToString().Trim()].ToString().Trim();
                    tRHolderWelder.EnterTime = DateTime.Now;
                    tRHolderWelder.AttachPath = dr[i][LanguageHandle.GetWord("SaoMiaoJianLuJing").ToString().Trim()].ToString().Trim();
                    try
                    {
                        if (IsTRHolderWelder(tRHolderWelder.UserCode))//存在时，先删除，后增加
                        {
                            string strHQL = "from TRHolderWelder as tRHolderWelder where tRHolderWelder.UserCode = '" + tRHolderWelder.UserCode + "' ";
                            IList lst = tRHolderWelderBLL.GetAllTRHolderWelders(strHQL);
                            if (lst.Count > 0 && lst != null)
                            {
                                for (int j = 0; j < lst.Count; j++)
                                {
                                    TRHolderWelder tRHolderWelder1 = (TRHolderWelder)lst[j];
                                    if (doubleData == "")
                                    {
                                        tRHolderWelderBLL.DeleteTRHolderWelder(tRHolderWelder1);
                                    }
                                    else
                                    {
                                        string goldvalue = tRHolderWelder.UserCode.Trim() + ",";
                                        if (!doubleData.Contains(goldvalue))
                                        {
                                            tRHolderWelderBLL.DeleteTRHolderWelder(tRHolderWelder1);
                                        }
                                    }
                                    continue;
                                }
                            }
                            doubleData += tRHolderWelder.UserCode + ",";
                        }
                        tRHolderWelderBLL.AddTRHolderWelder(tRHolderWelder);
                    }
                    catch
                    {
                        LB_ErrorText.Text += LanguageHandle.GetWord("ZZMSG").ToString().Trim();
                        return;
                    }
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZEXCLEBDRCG").ToString().Trim()+"')", true);
                LoadTRHolderWelderList(LB_HolderUserCode.Text.Trim());
            }
        }
    }

    protected void btn_ExcelToDataPost_Click(object sender, EventArgs e)
    {
        if (InputFile_Post.HasFile == false)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim();
            return;
        }
        string IsXls = System.IO.Path.GetExtension(InputFile_Post.FileName).ToString().ToLower();
        if (IsXls != ".xls" & IsXls != ".xlsx")
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim();
            return;
        }

        TRPostCertificateBLL tRPostCertificateBLL = new TRPostCertificateBLL();
        TRPostCertificate tRPostCertificate = new TRPostCertificate();

        string filename = InputFile_Post.FileName.ToString();  //获取Execle文件名
        string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
        string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCodeOld + "\\Doc\\";
        FileInfo fi = new FileInfo(strDocSavePath + newfilename);
        if (fi.Exists)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim();
        }
        else
        {
            InputFile_Post.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
            string strpath = strDocSavePath + newfilename;

            //DataSet ds = ExcelToDataSet(strpath, filename);
            //DataRow[] dr = ds.Tables[0].Select();
            //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
            //int rowsnum = ds.Tables[0].Rows.Count;

            DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
            DataRow[] dr = dt.Select();                        //定义一个DataRow数组
            int rowsnum = dt.Rows.Count;
            if (rowsnum == 0)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim();
            }
            else
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    tRPostCertificate.CertificateNo = dr[i][LanguageHandle.GetWord("GangWeiZhengShuBianHao").ToString().Trim()].ToString().Trim();
                    tRPostCertificate.CertificateOffice = dr[i][LanguageHandle.GetWord("FaZhengJiGuan").ToString().Trim()].ToString().Trim();
                    tRPostCertificate.CertificateReviewTime = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("GangWeiZhengShuFuShenShiJian").ToString().Trim()].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("GangWeiZhengShuFuShenShiJian").ToString().Trim()].ToString());
                    tRPostCertificate.EnterCode = strUserCodeOld.Trim();
                    tRPostCertificate.CertificateTime = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("GangWeiZhengShuQuZhengShiJian").ToString().Trim()].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("GangWeiZhengShuQuZhengShiJian").ToString().Trim()].ToString());
                    tRPostCertificate.Job = dr[i][LanguageHandle.GetWord("GangWeiZhiWu").ToString().Trim()].ToString().Trim();
                    tRPostCertificate.Unit = dr[i][LanguageHandle.GetWord("ChanWei").ToString().Trim()].ToString().Trim();
                    tRPostCertificate.Remark = dr[i][LanguageHandle.GetWord("BeiZhu").ToString().Trim()].ToString().Trim();
                    tRPostCertificate.UserCode = string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("YuanGongDaiMa").ToString().Trim()].ToString()) ? GetUserCode(dr[i][LanguageHandle.GetWord("YuanGongXingMing").ToString().Trim()].ToString().Trim()) : dr[i][LanguageHandle.GetWord("YuanGongDaiMa").ToString().Trim()].ToString().Trim();
                    tRPostCertificate.EnterTime = DateTime.Now;
                    try
                    {
                        string strID = "0";
                        if (IsTRPostCertificate(tRPostCertificate.UserCode, ref strID))
                        {
                            string strHQL = "from TRPostCertificate as tRPostCertificate where tRPostCertificate.ID = '" + strID + "' ";
                            IList lst = tRPostCertificateBLL.GetAllTRPostCertificates(strHQL);
                            TRPostCertificate tRPostCertificate1 = (TRPostCertificate)lst[0];
                            tRPostCertificate1.CertificateNo = tRPostCertificate.CertificateNo;
                            tRPostCertificate1.CertificateOffice = tRPostCertificate.CertificateOffice;
                            tRPostCertificate1.CertificateReviewTime = tRPostCertificate.CertificateReviewTime;
                            tRPostCertificate1.EnterCode = tRPostCertificate.EnterCode;
                            tRPostCertificate1.CertificateTime = tRPostCertificate.CertificateTime;
                            tRPostCertificate1.Job = tRPostCertificate.Job;
                            tRPostCertificate1.Unit = tRPostCertificate.Unit;
                            tRPostCertificate1.Remark = tRPostCertificate.Remark;

                            tRPostCertificateBLL.UpdateTRPostCertificate(tRPostCertificate1, tRPostCertificate1.ID);
                        }
                        else
                        {
                            tRPostCertificateBLL.AddTRPostCertificate(tRPostCertificate);
                        }
                    }
                    catch
                    {
                        LB_ErrorText.Text += LanguageHandle.GetWord("ZZMSG").ToString().Trim();
                        return;
                    }
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZEXCLEBDRCG").ToString().Trim()+"')", true);
                LoadTRPostCertificateList(LB_PostUserCode.Text.Trim());
            }
        }
    }

    protected string GetUserCode(string strusername)
    {
        string strusercode;
        string strHQL = " from ProjectMember as projectMember where projectMember.UserName = '" + strusername + "' ";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];
            strusercode = projectMember.UserCode.Trim();
        }
        else
        {
            strusercode = "0";
        }
        return strusercode;
    }

    /// <summary>
    /// 判断培训信息是否存在，存在返回true；否则false
    /// </summary>
    /// <param name="strusercode"></param>
    /// <param name="strID"></param>
    /// <returns></returns>
    protected bool IsTREmployeeTraining(string strusercode, ref string strID)
    {
        bool flag = true;
        string strHQL = "from TREmployeeTraining as tREmployeeTraining where tREmployeeTraining.UserCode = '" + strusercode + "' ";
        TREmployeeTrainingBLL tREmployeeTrainingBLL = new TREmployeeTrainingBLL();
        IList lst = tREmployeeTrainingBLL.GetAllTREmployeeTrainings(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TREmployeeTraining tREmployeeTraining = (TREmployeeTraining)lst[0];
            flag = true;
            strID = tREmployeeTraining.ID.ToString();
        }
        else
        {
            flag = false;
            strID = "0";
        }
        return flag;
    }

    /// <summary>
    /// 判断特种作业信息是否存在，存在返回true；否则false
    /// </summary>
    /// <param name="strusercode"></param>
    /// <param name="strID"></param>
    /// <returns></returns>
    protected bool IsTRSpecialOperations(string strusercode, ref string strID)
    {
        bool flag = true;
        string strHQL = "from TRSpecialOperations as tRSpecialOperations where tRSpecialOperations.UserCode = '" + strusercode + "' ";
        TRSpecialOperationsBLL tRSpecialOperationsBLL = new TRSpecialOperationsBLL();
        IList lst = tRSpecialOperationsBLL.GetAllTRSpecialOperationss(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRSpecialOperations tRSpecialOperations = (TRSpecialOperations)lst[0];
            flag = true;
            strID = tRSpecialOperations.ID.ToString();
        }
        else
        {
            flag = false;
            strID = "0";
        }
        return flag;
    }

    /// <summary>
    /// 判断特种设备信息是否存在，存在返回true；否则false
    /// </summary>
    /// <param name="strusercode"></param>
    /// <param name="strID"></param>
    /// <returns></returns>
    protected bool IsTRSpecialEquipment(string strusercode, ref string strID)
    {
        bool flag = true;
        string strHQL = "from TRSpecialEquipment as tRSpecialEquipment where tRSpecialEquipment.UserCode = '" + strusercode + "' ";
        TRSpecialEquipmentBLL tRSpecialEquipmentBLL = new TRSpecialEquipmentBLL();
        IList lst = tRSpecialEquipmentBLL.GetAllTRSpecialEquipments(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRSpecialEquipment tRSpecialEquipment = (TRSpecialEquipment)lst[0];
            flag = true;
            strID = tRSpecialEquipment.ID.ToString();
        }
        else
        {
            flag = false;
            strID = "0";
        }
        return flag;
    }

    /// <summary>
    /// 判断焊工项目信息是否存在，存在返回true；否则false
    /// </summary>
    /// <param name="strusercode"></param>
    /// <returns></returns>
    protected bool IsTRHolderWelder(string strusercode)
    {
        bool flag = true;
        string strHQL = "from TRHolderWelder as tRHolderWelder where tRHolderWelder.UserCode = '" + strusercode + "' ";
        TRHolderWelderBLL tRHolderWelderBLL = new TRHolderWelderBLL();
        IList lst = tRHolderWelderBLL.GetAllTRHolderWelders(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    /// <summary>
    /// 判断岗位管理信息是否存在，存在返回true；否则false
    /// </summary>
    /// <param name="strusercode"></param>
    /// <param name="strID"></param>
    /// <returns></returns>
    protected bool IsTRPostCertificate(string strusercode, ref string strID)
    {
        bool flag = true;
        string strHQL = "from TRPostCertificate as tRPostCertificate where tRPostCertificate.UserCode = '" + strusercode + "' ";
        TRPostCertificateBLL tRPostCertificateBLL = new TRPostCertificateBLL();
        IList lst = tRPostCertificateBLL.GetAllTRPostCertificates(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRPostCertificate tRPostCertificate = (TRPostCertificate)lst[0];
            flag = true;
            strID = tRPostCertificate.ID.ToString();
        }
        else
        {
            flag = false;
            strID = "0";
        }
        return flag;
    }

    protected void btn_TrainingAdd_Click(object sender, EventArgs e)
    {
        TRTrainingRecordEmpBLL tRTrainingRecordEmpBLL = new TRTrainingRecordEmpBLL();
        TRTrainingRecordEmp tRTrainingRecordEmp = new TRTrainingRecordEmp();
        tRTrainingRecordEmp.EnterCode = strUserCodeOld.Trim();
        tRTrainingRecordEmp.TrainingAccord = TB_TrainingAccord.Text.Trim();
        tRTrainingRecordEmp.TrainingAddress = TB_TrainingAddress.Text.Trim();
        tRTrainingRecordEmp.TrainingContent = TB_TrainingContent.Text.Trim();
        tRTrainingRecordEmp.TrainingProject = TB_TrainingProject.Text.Trim();
        tRTrainingRecordEmp.TrainingTime = DateTime.Parse(string.IsNullOrEmpty(DLC_TrainingTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_TrainingTime.Text.Trim());
        tRTrainingRecordEmp.TrainingUnit = TB_TrainingUnit.Text.Trim();
        tRTrainingRecordEmp.UserCode = LB_TrainingUserCode.Text.Trim();
        tRTrainingRecordEmp.EnterTime = DateTime.Now;
        try
        {
            tRTrainingRecordEmpBLL.AddTRTrainingRecordEmp(tRTrainingRecordEmp);
            txt_ID.Text = GetTRTrainingRecordEmpMaxID(tRTrainingRecordEmp.UserCode);

            LoadTREmployeeTrainingList(tRTrainingRecordEmp.UserCode);

            btn_TrainingAdd.Visible = true;
            btn_TrainingAdd.Enabled = true;
            btn_TrainingUpdate.Visible = true;
            btn_TrainingUpdate.Enabled = true;
            btn_TrainingDelete.Visible = true;
            btn_TrainingDelete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPXMZJCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPXMZJSB").ToString().Trim()+"')", true);
        }
    }

    protected void btn_TrainingUpdate_Click(object sender, EventArgs e)
    {
        TRTrainingRecordEmpBLL tRTrainingRecordEmpBLL = new TRTrainingRecordEmpBLL();
        string strHQL = "from TRTrainingRecordEmp as tRTrainingRecordEmp where tRTrainingRecordEmp.ID = '" + txt_ID.Text.Trim() + "' ";
        IList lst = tRTrainingRecordEmpBLL.GetAllTRTrainingRecordEmps(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRTrainingRecordEmp tRTrainingRecordEmp = (TRTrainingRecordEmp)lst[0];
            tRTrainingRecordEmp.TrainingAccord = TB_TrainingAccord.Text.Trim();
            tRTrainingRecordEmp.TrainingAddress = TB_TrainingAddress.Text.Trim();
            tRTrainingRecordEmp.TrainingContent = TB_TrainingContent.Text.Trim();
            tRTrainingRecordEmp.TrainingProject = TB_TrainingProject.Text.Trim();
            tRTrainingRecordEmp.TrainingTime = DateTime.Parse(string.IsNullOrEmpty(DLC_TrainingTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_TrainingTime.Text.Trim());
            tRTrainingRecordEmp.TrainingUnit = TB_TrainingUnit.Text.Trim();

            try
            {
                tRTrainingRecordEmpBLL.UpdateTRTrainingRecordEmp(tRTrainingRecordEmp, tRTrainingRecordEmp.ID);
                LoadTREmployeeTrainingList(tRTrainingRecordEmp.UserCode);

                btn_TrainingAdd.Visible = true;
                btn_TrainingAdd.Enabled = true;
                btn_TrainingUpdate.Visible = true;
                btn_TrainingUpdate.Enabled = true;
                btn_TrainingDelete.Visible = true;
                btn_TrainingDelete.Enabled = true;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPXMGXCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPXMGXSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void btn_TrainingDelete_Click(object sender, EventArgs e)
    {
        TRTrainingRecordEmpBLL tRTrainingRecordEmpBLL = new TRTrainingRecordEmpBLL();
        string strHQL = "from TRTrainingRecordEmp as tRTrainingRecordEmp where tRTrainingRecordEmp.ID = '" + txt_ID.Text.Trim() + "' ";
        IList lst = tRTrainingRecordEmpBLL.GetAllTRTrainingRecordEmps(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRTrainingRecordEmp tRTrainingRecordEmp = (TRTrainingRecordEmp)lst[0];
            try
            {
                tRTrainingRecordEmpBLL.DeleteTRTrainingRecordEmp(tRTrainingRecordEmp);

                LoadTREmployeeTrainingList(LB_TrainingUserCode.Text.Trim());

                btn_TrainingAdd.Visible = true;
                btn_TrainingAdd.Enabled = true;
                btn_TrainingUpdate.Visible = false;
                btn_TrainingDelete.Visible = false;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZPeiXunXiangMuLanguageHandleG").ToString().Trim()+"')", true); 
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZPeiXunXiangMuLanguageHandleG").ToString().Trim()+"')", true); 
            }
        }
    }

    protected string GetTRTrainingRecordEmpMaxID(string strusercode)
    {
        string flag = "0";
        string strHQL = "from TRTrainingRecordEmp as tRTrainingRecordEmp where tRTrainingRecordEmp.UserCode = '" + strusercode + "' Order By tRTrainingRecordEmp.ID Desc ";
        TRTrainingRecordEmpBLL tRTrainingRecordEmpBLL = new TRTrainingRecordEmpBLL();
        IList lst = tRTrainingRecordEmpBLL.GetAllTRTrainingRecordEmps(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRTrainingRecordEmp tRTrainingRecordEmp = (TRTrainingRecordEmp)lst[0];
            flag = tRTrainingRecordEmp.ID.ToString();
        }
        return flag;
    }

    protected void DataGrid3_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        ShareClass.ColorDataGridSelectRow(DataGrid3, e);

        string strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

        strHQL = "from TRTrainingRecordEmp as tRTrainingRecordEmp where tRTrainingRecordEmp.ID = " + strID;
        TRTrainingRecordEmpBLL tRTrainingRecordEmpBLL = new TRTrainingRecordEmpBLL();
        lst = tRTrainingRecordEmpBLL.GetAllTRTrainingRecordEmps(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            TRTrainingRecordEmp tRTrainingRecordEmp = (TRTrainingRecordEmp)lst[0];
            txt_ID.Text = tRTrainingRecordEmp.ID.ToString();
            TB_TrainingAccord.Text = tRTrainingRecordEmp.TrainingAccord.Trim();
            TB_TrainingAddress.Text = tRTrainingRecordEmp.TrainingAddress.Trim();
            TB_TrainingContent.Text = tRTrainingRecordEmp.TrainingContent.Trim();
            TB_TrainingProject.Text = tRTrainingRecordEmp.TrainingProject.Trim();
            TB_TrainingUnit.Text = tRTrainingRecordEmp.TrainingUnit.Trim();
            DLC_TrainingTime.Text = tRTrainingRecordEmp.TrainingTime.ToString("yyyy-MM-dd");
            if (tRTrainingRecordEmp.EnterCode.Trim() == strUserCodeOld.Trim())
            {
                btn_TrainingAdd.Visible = true;
                btn_TrainingAdd.Enabled = true;
                btn_TrainingUpdate.Visible = true;
                btn_TrainingUpdate.Enabled = true;
                btn_TrainingDelete.Visible = true;
                btn_TrainingDelete.Enabled = true;
            }
            else
            {
                btn_TrainingAdd.Visible = true;
                btn_TrainingAdd.Enabled = true;
                btn_TrainingUpdate.Visible = false;
                btn_TrainingDelete.Visible = false;
            }
        }
    }

    protected void btn_ExcelToDataTraining1_Click(object sender, EventArgs e)
    {
        if (InputFile1.HasFile == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim()+"')", true);
            return;
        }
        string IsXls = System.IO.Path.GetExtension(InputFile1.FileName).ToString().ToLower();
        if (IsXls != ".xls" & IsXls != ".xlsx")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim()+"')", true);
            return;
        }

        TRTrainingRecordEmpBLL tRTrainingRecordEmpBLL = new TRTrainingRecordEmpBLL();
        TRTrainingRecordEmp tRTrainingRecordEmp = new TRTrainingRecordEmp();

        string filename = InputFile1.FileName.ToString();  //获取Execle文件名
        string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
        string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCodeOld + "\\Doc\\";
        FileInfo fi = new FileInfo(strDocSavePath + newfilename);
        if (fi.Exists)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim()+"');</script>");
        }
        else
        {
            InputFile1.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
            string strpath = strDocSavePath + newfilename;

            //DataSet ds = ExcelToDataSet(strpath, filename);
            //DataRow[] dr = ds.Tables[0].Select();
            //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
            //int rowsnum = ds.Tables[0].Rows.Count;

            DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
            DataRow[] dr = dt.Select();                        //定义一个DataRow数组
            int rowsnum = dt.Rows.Count;
            if (rowsnum == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim()+"')", true);
            }
            else
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    tRTrainingRecordEmp.TrainingAccord = dr[i][LanguageHandle.GetWord("PeiXunYiJu").ToString().Trim()].ToString().Trim();
                    tRTrainingRecordEmp.TrainingAddress = dr[i][LanguageHandle.GetWord("PeiXunDeDian").ToString().Trim()].ToString().Trim();
                    tRTrainingRecordEmp.TrainingContent = dr[i][LanguageHandle.GetWord("PeiXunNeiRong").ToString().Trim()].ToString().Trim();
                    tRTrainingRecordEmp.EnterCode = strUserCodeOld.Trim();
                    tRTrainingRecordEmp.TrainingProject = dr[i][LanguageHandle.GetWord("PeiXunXiangMu").ToString().Trim()].ToString().Trim();
                    tRTrainingRecordEmp.TrainingUnit = dr[i][LanguageHandle.GetWord("JuBanChanWei").ToString().Trim()].ToString().Trim();
                    tRTrainingRecordEmp.TrainingTime = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("PeiXunRiJi").ToString().Trim()].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("PeiXunRiJi").ToString().Trim()].ToString());
                    tRTrainingRecordEmp.UserCode = string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("YuanGongDaiMa").ToString().Trim()].ToString()) ? GetUserCode(dr[i][LanguageHandle.GetWord("YuanGongXingMing").ToString().Trim()].ToString().Trim()) : dr[i][LanguageHandle.GetWord("YuanGongDaiMa").ToString().Trim()].ToString().Trim();
                    tRTrainingRecordEmp.EnterTime = DateTime.Now;
                    try
                    {
                        tRTrainingRecordEmpBLL.AddTRTrainingRecordEmp(tRTrainingRecordEmp);
                    }
                    catch
                    {
                        string Msg = LanguageHandle.GetWord("JingGaoExcelBiaoShuJuDaoRuShiB").ToString().Trim();
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZMSG").ToString().Trim()+"')", true);
                        return;
                    }
                }
                LoadTREmployeeTrainingList(LB_TrainingUserCode.Text.Trim());
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZEXCLEBDRCG").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_TrainingCheck_Click(object sender, EventArgs e)
    {
        string strIDCard = TB_TrainingNumberNo.Text.Trim();

        string strHQL;
        IList lst;

        strHQL = "from ProjectMember as projectMember where projectMember.IDCard = '" + strIDCard + "' ";

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];

            LB_EquipmentUserCode.Text = projectMember.UserCode.Trim();
            LB_HolderUserCode.Text = projectMember.UserCode.Trim();
            LB_OperationUserCode.Text = projectMember.UserCode.Trim();
            LB_PostUserCode.Text = projectMember.UserCode.Trim();
            LB_TrainingUserCode.Text = projectMember.UserCode.Trim();

            TB_EquipmentName.Text = projectMember.UserName.Trim();
            TB_HolderName.Text = projectMember.UserName.Trim();
            TB_OperationName.Text = projectMember.UserName.Trim();
            TB_PostName.Text = projectMember.UserName.Trim();
            TB_TrainingName.Text = projectMember.UserName.Trim();

            TB_EquipmentNumberNo.Text = projectMember.IDCard.Trim();
            TB_HolderNumberNo.Text = projectMember.IDCard.Trim();
            TB_OperationNumberNo.Text = projectMember.IDCard.Trim();
            TB_PostNumberNo.Text = projectMember.IDCard.Trim();
            TB_TrainingNumberNo.Text = projectMember.IDCard.Trim();

            ddl_EquipmentSex.SelectedValue = string.IsNullOrEmpty(projectMember.Gender) || projectMember.Gender.Trim() == "" ? "Male" : projectMember.Gender.Trim();
            ddl_HolderSex.SelectedValue = string.IsNullOrEmpty(projectMember.Gender) || projectMember.Gender.Trim() == "" ? "Male" : projectMember.Gender.Trim();
            ddl_OperationSex.SelectedValue = string.IsNullOrEmpty(projectMember.Gender) || projectMember.Gender.Trim() == "" ? "Male" : projectMember.Gender.Trim();
            ddl_PostSex.SelectedValue = string.IsNullOrEmpty(projectMember.Gender) || projectMember.Gender.Trim() == "" ? "Male" : projectMember.Gender.Trim();
            ddl_TrainingSex.SelectedValue = string.IsNullOrEmpty(projectMember.Gender) || projectMember.Gender.Trim() == "" ? "Male" : projectMember.Gender.Trim();

            ddl_EquipmentWorkType.SelectedValue = string.IsNullOrEmpty(projectMember.WorkType) ? "" : projectMember.WorkType.Trim();
            ddl_OperationWorkType.SelectedValue = string.IsNullOrEmpty(projectMember.WorkType) ? "" : projectMember.WorkType.Trim();
            ddl_PostWorkType.SelectedValue = string.IsNullOrEmpty(projectMember.WorkType) ? "" : projectMember.WorkType.Trim();
            ddl_TrainingWorkType.SelectedValue = string.IsNullOrEmpty(projectMember.WorkType) ? "" : projectMember.WorkType.Trim();

            DLC_PostBirthDay.Text = string.IsNullOrEmpty(projectMember.BirthDay.ToString()) ? DateTime.Now.ToString("yyyy-MM-dd") : projectMember.BirthDay.ToString("yyyy-MM-dd");

            TB_HolderUnit.Text = projectMember.DepartName.Trim();
            TB_PostUnit.Text = projectMember.DepartName.Trim();

            LoadTREmployeeTrainingList(projectMember.UserCode.Trim());
            LoadTRSpecialOperationsList(projectMember.UserCode.Trim());
            LoadTRSpecialEquipmentList(projectMember.UserCode.Trim());
            LoadTRHolderWelderList(projectMember.UserCode.Trim());
            LoadTRPostCertificateList(projectMember.UserCode.Trim());
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSFZHBCZJC").ToString().Trim()+"')", true);
            return;
        }
    }
}
