using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWPQMPWPS1 : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","PWPS-1管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            TB_PWPS1DateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadWPQMWeldProQuaName();
            LoadWPQMPWPS1List();
        }
    }

    protected void LoadWPQMWeldProQuaName()
    {
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua Order By wPQMWeldProQua.Code Desc";
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        DL_WeldProCode.DataSource = lst;
        DL_WeldProCode.DataBind();
        DL_WeldProCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadWPQMPWPS1List()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMPWPS1 Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or EntityName like '%" + TextBox1.Text.Trim() + "%' or PWPSAndCategory like '%" + TextBox1.Text.Trim() + "%' " +
            "or PWPSAndStandardNo like '%" + TextBox1.Text.Trim() + "%' or PWPSMetalOther like '%" + TextBox1.Text.Trim() + "%' or PWPSDescr like '%" + TextBox1.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPWPS1");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(DL_WeldProCode.SelectedValue) || DL_WeldProCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSHJGYPDWBXJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        if (IsWPQMPWPS1(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGPWPS1YCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMPWPS1BLL wPQMPWPS1BLL = new WPQMPWPS1BLL();
        WPQMPWPS1 wPQMPWPS1 = new WPQMPWPS1();

        string strAttach = UploadAttach();
        if (strAttach.Equals("0"))
        {
            wPQMPWPS1.WeldedJointDiagram = lbl_Path.Text.Trim();
        }
        else if (strAttach.Equals("1"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim()+"')", true);
            return;
        }
        else if (strAttach.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
            return;
        }
        else
        {
            lbl_Path.Text = strAttach;
            wPQMPWPS1.WeldedJointDiagram = strAttach;
        }

        wPQMPWPS1.C = TB_C.Text.Trim();
        wPQMPWPS1.Cr = TB_Cr.Text.Trim();
        wPQMPWPS1.Mn = TB_Mn.Text.Trim();
        wPQMPWPS1.Mo = TB_Mo.Text.Trim();
        wPQMPWPS1.Nb = TB_Nb.Text.Trim();
        wPQMPWPS1.Ni = TB_Ni.Text.Trim();
        wPQMPWPS1.P = TB_P.Text.Trim();
        wPQMPWPS1.S = TB_S.Text.Trim();
        wPQMPWPS1.Si = TB_Si.Text.Trim();
        wPQMPWPS1.Ti = TB_Ti.Text.Trim();
        wPQMPWPS1.Cu = TB_Cu.Text.Trim();
        wPQMPWPS1.EnterCode = strUserCode.Trim();
        wPQMPWPS1.FilletWeldMateThicknessRange = TB_FilletWeldMateThicknessRange.Text.Trim();
        wPQMPWPS1.ElectInspection = TB_ElectInspection.Text.Trim();
        wPQMPWPS1.FluxInspection = TB_FluxInspection.Text.Trim();
        wPQMPWPS1.WireInspection = TB_WireInspection.Text.Trim();
        wPQMPWPS1.ButtWeldMetaThickRange = TB_ButtWeldMetaThickRange.Text.Trim();
        wPQMPWPS1.WeldedJointOther = TB_WeldedJointOther.Text.Trim();
        wPQMPWPS1.WireStandard = TB_WireStandard.Text.Trim();
        wPQMPWPS1.FluxStandard = TB_FluxStandard.Text.Trim();
        wPQMPWPS1.PWPSDescr = TB_PWPSDescr.Text.Trim();
        wPQMPWPS1.MechanizationDegree = TB_MechanizationDegree.Text.Trim();
        wPQMPWPS1.ElectStandard = TB_ElectStandard.Text.Trim();
        wPQMPWPS1.FilletWeldMetaThickRange = TB_FilletWeldMetaThickRange.Text.Trim();
        wPQMPWPS1.PWPS1DateTime = DateTime.Parse(string.IsNullOrEmpty(TB_PWPS1DateTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : TB_PWPS1DateTime.Text.Trim());
        wPQMPWPS1.PWPSCategory = TB_PWPSCategory.Text.Trim();
        wPQMPWPS1.EntityName = TB_EntityName.Text.Trim();
        wPQMPWPS1.PWPSAndCategory = TB_PWPSAndCategory.Text.Trim();
        wPQMPWPS1.PWPSStandardNo = TB_PWPSStandardNo.Text.Trim();
        wPQMPWPS1.PWPSAndStandardNo = TB_PWPSAndStandardNo.Text.Trim();
        wPQMPWPS1.ButtWeldMateThicknessRange = TB_ButtWeldMateThicknessRange.Text.Trim();
        wPQMPWPS1.PWPSMetalOther = TB_PWPSMetalOther.Text.Trim();
        wPQMPWPS1.ButtWeldOtherInfo = TB_ButtWeldOtherInfo.Text.Trim();
        wPQMPWPS1.FilletWeld = TB_FilletWeld.Text.Trim();
        wPQMPWPS1.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMPWPS1BLL.AddWPQMPWPS1(wPQMPWPS1);
            lbl_ID.Text = GetMaxWPQMPWPS1ID(wPQMPWPS1).ToString();
            UpdateWPQMWeldProQuaData(wPQMPWPS1.WeldProCode.Trim());
            LoadWPQMPWPS1List();

            BT_Update.Visible = true;
            BT_Update.Enabled = true;
            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim()+"')", true);
        }
    }

    protected string UploadAttach()
    {
        //上传附件
        if (AttachFile.HasFile)
        {
            string strFileName1, strExtendName;

            strFileName1 = this.AttachFile.FileName;//获取上传文件的文件名,包括后缀
            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Images\\";

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
                    return "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Images\\" + strFileName3;
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

    protected void UpdateWPQMWeldProQuaData(string strCode)
    {
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua Where wPQMWeldProQua.Code='" + strCode + "'";
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            WPQMWeldProQua wPQMWeldProQua = (WPQMWeldProQua)lst[0];
            wPQMWeldProQua.EntityName = TB_EntityName.Text.Trim();
            wPQMWeldProQua.WeldedJointDiagram = lbl_Path.Text.Trim();
            wPQMWeldProQua.MechanizationDegree = TB_MechanizationDegree.Text.Trim();
            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }

    protected int GetMaxWPQMPWPS1ID(WPQMPWPS1 bmbp)
    {
        string strHQL = "Select ID From T_WPQMPWPS1 where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPWPS1").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMPWPS1(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMPWPS1 Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMPWPS1 Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPWPS1").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(DL_WeldProCode.SelectedValue) || DL_WeldProCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSHJGYPDWBXJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        if (IsWPQMPWPS1(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGPWPS1YCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMPWPS1 as wPQMPWPS1 where wPQMPWPS1.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMPWPS1BLL wPQMPWPS1BLL = new WPQMPWPS1BLL();
        IList lst = wPQMPWPS1BLL.GetAllWPQMPWPS1s(strHQL);

        WPQMPWPS1 wPQMPWPS1 = (WPQMPWPS1)lst[0];

        string strAttach = UploadAttach();
        if (strAttach.Equals("0"))
        {
        }
        else if (strAttach.Equals("1"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim()+"')", true);
            return;
        }
        else if (strAttach.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
            return;
        }
        else
        {
            lbl_Path.Text = strAttach;
            wPQMPWPS1.WeldedJointDiagram = strAttach;
        }

        wPQMPWPS1.C = TB_C.Text.Trim();
        wPQMPWPS1.Cr = TB_Cr.Text.Trim();
        wPQMPWPS1.Mn = TB_Mn.Text.Trim();
        wPQMPWPS1.Mo = TB_Mo.Text.Trim();
        wPQMPWPS1.Nb = TB_Nb.Text.Trim();
        wPQMPWPS1.Ni = TB_Ni.Text.Trim();
        wPQMPWPS1.P = TB_P.Text.Trim();
        wPQMPWPS1.S = TB_S.Text.Trim();
        wPQMPWPS1.Si = TB_Si.Text.Trim();
        wPQMPWPS1.Ti = TB_Ti.Text.Trim();
        wPQMPWPS1.Cu = TB_Cu.Text.Trim();
        wPQMPWPS1.FilletWeldMateThicknessRange = TB_FilletWeldMateThicknessRange.Text.Trim();
        wPQMPWPS1.ElectInspection = TB_ElectInspection.Text.Trim();
        wPQMPWPS1.FluxInspection = TB_FluxInspection.Text.Trim();
        wPQMPWPS1.WireInspection = TB_WireInspection.Text.Trim();
        wPQMPWPS1.ButtWeldMetaThickRange = TB_ButtWeldMetaThickRange.Text.Trim();
        wPQMPWPS1.WeldedJointOther = TB_WeldedJointOther.Text.Trim();
        wPQMPWPS1.WireStandard = TB_WireStandard.Text.Trim();
        wPQMPWPS1.FluxStandard = TB_FluxStandard.Text.Trim();
        wPQMPWPS1.PWPSDescr = TB_PWPSDescr.Text.Trim();
        wPQMPWPS1.MechanizationDegree = TB_MechanizationDegree.Text.Trim();
        wPQMPWPS1.ElectStandard = TB_ElectStandard.Text.Trim();
        wPQMPWPS1.FilletWeldMetaThickRange = TB_FilletWeldMetaThickRange.Text.Trim();
        wPQMPWPS1.PWPS1DateTime = DateTime.Parse(string.IsNullOrEmpty(TB_PWPS1DateTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : TB_PWPS1DateTime.Text.Trim());
        wPQMPWPS1.PWPSCategory = TB_PWPSCategory.Text.Trim();
        wPQMPWPS1.EntityName = TB_EntityName.Text.Trim();
        wPQMPWPS1.PWPSAndCategory = TB_PWPSAndCategory.Text.Trim();
        wPQMPWPS1.PWPSStandardNo = TB_PWPSStandardNo.Text.Trim();
        wPQMPWPS1.PWPSAndStandardNo = TB_PWPSAndStandardNo.Text.Trim();
        wPQMPWPS1.ButtWeldMateThicknessRange = TB_ButtWeldMateThicknessRange.Text.Trim();
        wPQMPWPS1.PWPSMetalOther = TB_PWPSMetalOther.Text.Trim();
        wPQMPWPS1.ButtWeldOtherInfo = TB_ButtWeldOtherInfo.Text.Trim();
        wPQMPWPS1.FilletWeld = TB_FilletWeld.Text.Trim();
        wPQMPWPS1.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMPWPS1BLL.UpdateWPQMPWPS1(wPQMPWPS1, wPQMPWPS1.ID);
            UpdateWPQMWeldProQuaData(wPQMPWPS1.WeldProCode.Trim());
            LoadWPQMPWPS1List();

            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;
            BT_Update.Visible = true;
            BT_Update.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strCode = lbl_ID.Text.Trim();

        strHQL = "Delete From T_WPQMPWPS1 Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadWPQMPWPS1List();

            BT_Update.Visible = false;
            BT_Delete.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadWPQMPWPS1List();
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From WPQMPWPS1 as wPQMPWPS1 where wPQMPWPS1.ID = '" + strId + "'";
            WPQMPWPS1BLL wPQMPWPS1BLL = new WPQMPWPS1BLL();
            lst = wPQMPWPS1BLL.GetAllWPQMPWPS1s(strHQL);

            WPQMPWPS1 wPQMPWPS1 = (WPQMPWPS1)lst[0];

            lbl_ID.Text = wPQMPWPS1.ID.ToString();
            TB_C.Text = wPQMPWPS1.C.Trim();
            TB_Cr.Text = wPQMPWPS1.Cr.Trim();
            TB_Mn.Text = wPQMPWPS1.Mn.Trim();
            TB_Mo.Text = wPQMPWPS1.Mo.Trim();
            TB_Nb.Text = wPQMPWPS1.Nb.Trim();
            TB_Ni.Text = wPQMPWPS1.Ni.Trim();
            TB_P.Text = wPQMPWPS1.P.Trim();
            TB_S.Text = wPQMPWPS1.S.Trim();
            TB_Si.Text = wPQMPWPS1.Si.Trim();
            TB_Ti.Text = wPQMPWPS1.Ti.Trim();
            TB_Cu.Text = wPQMPWPS1.Cu.Trim();
            TB_FilletWeldMateThicknessRange.Text = wPQMPWPS1.FilletWeldMateThicknessRange.Trim();
            TB_ElectInspection.Text = wPQMPWPS1.ElectInspection.Trim();
            TB_FluxInspection.Text = wPQMPWPS1.FluxInspection.Trim();
            TB_WireInspection.Text = wPQMPWPS1.WireInspection.Trim();
            TB_ButtWeldMetaThickRange.Text = wPQMPWPS1.ButtWeldMetaThickRange.Trim();
            TB_WeldedJointOther.Text = wPQMPWPS1.WeldedJointOther.Trim();
            TB_WireStandard.Text = wPQMPWPS1.WireStandard.Trim();
            TB_FluxStandard.Text = wPQMPWPS1.FluxStandard.Trim();
            TB_PWPSDescr.Text = wPQMPWPS1.PWPSDescr.Trim();
            TB_MechanizationDegree.Text = wPQMPWPS1.MechanizationDegree.Trim();
            TB_ElectStandard.Text = wPQMPWPS1.ElectStandard.Trim();
            TB_FilletWeldMetaThickRange.Text = wPQMPWPS1.FilletWeldMetaThickRange.Trim();
            TB_PWPS1DateTime.Text = wPQMPWPS1.PWPS1DateTime.ToString("yyyy-MM-dd");
            TB_PWPSCategory.Text = wPQMPWPS1.PWPSCategory.Trim();
            TB_EntityName.Text = wPQMPWPS1.EntityName.Trim();
            TB_PWPSAndCategory.Text = wPQMPWPS1.PWPSAndCategory.Trim();
            TB_PWPSStandardNo.Text = wPQMPWPS1.PWPSStandardNo.Trim();
            TB_PWPSAndStandardNo.Text = wPQMPWPS1.PWPSAndStandardNo.Trim();
            TB_ButtWeldMateThicknessRange.Text = wPQMPWPS1.ButtWeldMateThicknessRange.Trim();
            TB_PWPSMetalOther.Text = wPQMPWPS1.PWPSMetalOther.Trim();
            TB_ButtWeldOtherInfo.Text = wPQMPWPS1.ButtWeldOtherInfo.Trim();
            TB_FilletWeld.Text = wPQMPWPS1.FilletWeld.Trim();
            DL_WeldProCode.SelectedValue = wPQMPWPS1.WeldProCode.Trim();
            lbl_Path.Text = wPQMPWPS1.WeldedJointDiagram.Trim();

            if (wPQMPWPS1.EnterCode.Trim() == strUserCode.Trim())
            {
                BT_Delete.Visible = true;
                BT_Update.Visible = true;
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
            }
            else
            {
                BT_Update.Visible = false;
                BT_Delete.Visible = false;
            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPWPS1");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void DL_WeldProCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua Where wPQMWeldProQua.Code='" + DL_WeldProCode.SelectedValue.Trim() + "'";
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            WPQMWeldProQua wPQMWeldProQua = (WPQMWeldProQua)lst[0];

            TB_EntityName.Text = string.IsNullOrEmpty(wPQMWeldProQua.EntityName) ? "" : wPQMWeldProQua.EntityName.Trim();
            TB_MechanizationDegree.Text = string.IsNullOrEmpty(wPQMWeldProQua.MechanizationDegree) ? "" : wPQMWeldProQua.MechanizationDegree.Trim();
            lbl_Path.Text = string.IsNullOrEmpty(wPQMWeldProQua.WeldedJointDiagram) ? "" : wPQMWeldProQua.WeldedJointDiagram.Trim();
        }
        else
        {
            TB_MechanizationDegree.Text = "";
            lbl_Path.Text = "";
            TB_EntityName.Text = "";
        }
    }
}