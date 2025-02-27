using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTBMAnnClaFile : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","招标澄清信息", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_SentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_EnterPer.Text = ShareClass.GetUserName(strUserCode);

            LoadBMBidPlanName();

            LoadBMAnnClaFileList();
        }
    }

    /// <summary>
    /// 获取人员所在部门
    /// </summary>
    /// <param name="strUserCode"></param>
    /// <returns></returns>
    protected string GetUserDepartName(string strUserCode)
    {
        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = '" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];

            strHQL = "From Department as department Where department.DepartCode = '" + projectMember.DepartCode.Trim() + "'";
            DepartmentBLL departmentBLL = new DepartmentBLL();
            lst = departmentBLL.GetAllDepartments(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                Department department = (Department)lst[0];
                return department.DepartName.Trim();
            }
            else
                return "";
        }
        else
            return "";
    }

    protected void LoadBMBidPlanName()
    {
        string strHQL;
        //绑定招标计划名称T_BMBidPlan
        //   strHQL = "select * From T_BMBidPlan where Datediff(day,BidStartDate,'" + DateTime.Now.ToString() + "')>=0 and Datediff(day,BidEndDate,'" + DateTime.Now.ToString() + "')<=0 Order By ID Desc";
        strHQL = "select * From T_BMBidPlan Order By ID Desc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidPlan");
        DL_BidPlanID.DataSource = ds;
        DL_BidPlanID.DataBind();
        DL_BidPlanID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    /// <summary>
    /// 获取招标计划实体
    /// </summary>
    /// <param name="strID"></param>
    /// <returns></returns>
    protected BMBidPlan GetBMBidPlanModel(string strID)
    {
        string strHQL = " from BMBidPlan as bMBidPlan where bMBidPlan.ID = '" + strID.Trim() + "'";
        BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
        IList lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
            return bMBidPlan;
        }
        else
            return null;
    }

    protected void LoadBMAnnClaFileList()
    {
        string strHQL;

        strHQL = "Select * From T_BMAnnClaFile Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Type like '%" + TextBox1.Text.Trim() + "%' or SendContent like '%" + TextBox1.Text.Trim() + "%' or ReplyContent like '%" + TextBox1.Text.Trim() + "%' " +
            "or EnterPer like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and BidPlanName like '%" + TextBox2.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-SentDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-SentDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAnnClaFile");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text;

        if (strID == "")
        {
            Add();
        }
        else
        {
            Update();
        }
    }

    protected void Add()
    {
        if (DL_BidPlanID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBFABJC").ToString().Trim() + "')", true);
            DL_BidPlanID.Focus();
            return;
        }
        //BMBidPlan bMBidPlan = GetBMBidPlanModel(DL_BidPlanID.SelectedValue.Trim());
        //if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) < bMBidPlan.BidStartDate || DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) > bMBidPlan.BidEndDate)
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDRBZZBJHYXNJC").ToString().Trim()+"')", true);
        //    DL_BidPlanID.Focus();
        //    return;
        //}
        BMAnnClaFileBLL bMAnnClaFileBLL = new BMAnnClaFileBLL();
        BMAnnClaFile bMAnnClaFile = new BMAnnClaFile();
        BMSupplierReplyBLL bMSupplierReplyBLL = new BMSupplierReplyBLL();
        BMSupplierReply bMSupplierReply = new BMSupplierReply();

        bMAnnClaFile.EnterPer = TB_EnterPer.Text.Trim();
        bMAnnClaFile.Type = DL_Type.SelectedValue.Trim();
        bMAnnClaFile.SendContent = TB_SendContent.Text.Trim();
        bMAnnClaFile.BidPlanID = int.Parse(DL_BidPlanID.SelectedValue.Trim());
        bMAnnClaFile.BidPlanName = GetBMBidPlanName(bMAnnClaFile.BidPlanID.ToString().Trim());
        bMAnnClaFile.SentDate = DateTime.Parse(DLC_SentDate.Text.Trim());
        bMAnnClaFile.SupplierCode = GetSupplierCode();
        bMAnnClaFile.ReplyDate = DateTime.Parse(DateTime.Now.ToString());
        bMAnnClaFile.EnterCode = strUserCode.Trim();

        if (bMAnnClaFile.SupplierCode.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCBSBJC").ToString().Trim() + "')", true);
            LB_SupplierCode.Focus();
            return;
        }

        try
        {
            bMAnnClaFileBLL.AddBMAnnClaFile(bMAnnClaFile);
            LB_ID.Text = GetMaxBMAnnClaFileID(bMAnnClaFile).ToString();
            //添加回函基础数据
            bMSupplierReply.AnnClaFileID = int.Parse(LB_ID.Text.Trim());
            bMSupplierReply.ReplyDate = bMAnnClaFile.ReplyDate;
            if (bMAnnClaFile.SupplierCode.Trim().Equals("0"))
            {
            }
            else
            {
                string[] sSupplierId = bMAnnClaFile.SupplierCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sSupplierId.Length; i++)
                {
                    bMSupplierReply.SupplierId = int.Parse(sSupplierId[i].ToString().Trim());
                    if (IsBMSupplierReply(LB_ID.Text.Trim(), bMSupplierReply.SupplierId.ToString().Trim()))
                    {
                    }
                    else
                    {
                        bMSupplierReplyBLL.AddBMSupplierReply(bMSupplierReply);
                    }
                }
            }

            LoadBMAnnClaFileList();

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    /// <summary>
    /// 新增时，获取表T_BMAnnClaFile中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxBMAnnClaFileID(BMAnnClaFile bmbp)
    {
        string strHQL = "Select ID From T_BMAnnClaFile where EnterPer='" + bmbp.EnterPer.Trim() + "' and BidPlanID='" + bmbp.BidPlanID.ToString().Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMAnnClaFile").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected string GetBMBidPlanName(string strID)
    {
        string strHQL;
        IList lst;
        //绑定招标计划名称
        strHQL = "From BMBidPlan as bMBidPlan Where bMBidPlan.ID='" + strID + "' ";
        BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
        lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
            return bMBidPlan.Name.Trim();
        }
        else
            return "";
    }

    protected void Update()
    {
        if (DL_BidPlanID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBFABJC").ToString().Trim() + "')", true);
            DL_BidPlanID.Focus();
            return;
        }
        //BMBidPlan bMBidPlan = GetBMBidPlanModel(DL_BidPlanID.SelectedValue.Trim());
        //if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) < bMBidPlan.BidStartDate || DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) > bMBidPlan.BidEndDate)
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDRBZZBJHYXNJC").ToString().Trim()+"')", true);
        //    DL_BidPlanID.Focus();
        //    return;
        //}
        BMSupplierReplyBLL bMSupplierReplyBLL = new BMSupplierReplyBLL();
        BMSupplierReply bMSupplierReply = new BMSupplierReply();
        string strHQL = "From BMAnnClaFile as bMAnnClaFile where bMAnnClaFile.ID = '" + LB_ID.Text.Trim() + "'";
        BMAnnClaFileBLL bMAnnClaFileBLL = new BMAnnClaFileBLL();
        IList lst = bMAnnClaFileBLL.GetAllBMAnnClaFiles(strHQL);
        BMAnnClaFile bMAnnClaFile = (BMAnnClaFile)lst[0];

        bMAnnClaFile.EnterPer = TB_EnterPer.Text.Trim();
        bMAnnClaFile.Type = DL_Type.SelectedValue.Trim();
        bMAnnClaFile.SendContent = TB_SendContent.Text.Trim();
        bMAnnClaFile.BidPlanID = int.Parse(DL_BidPlanID.SelectedValue.Trim());
        bMAnnClaFile.BidPlanName = GetBMBidPlanName(bMAnnClaFile.BidPlanID.ToString().Trim());
        bMAnnClaFile.SentDate = DateTime.Parse(DLC_SentDate.Text.Trim());
        bMAnnClaFile.SupplierCode = GetSupplierCode();
        if (bMAnnClaFile.SupplierCode.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCBSBJC").ToString().Trim() + "')", true);
            LB_SupplierCode.Focus();
            return;
        }
        try
        {
            bMAnnClaFileBLL.UpdateBMAnnClaFile(bMAnnClaFile, bMAnnClaFile.ID);

            //添加回函基础数据
            bMSupplierReply.AnnClaFileID = int.Parse(LB_ID.Text.Trim());
            if (bMAnnClaFile.SupplierCode.Trim().Equals("0"))
            {
            }
            else
            {
                string[] sSupplierId = bMAnnClaFile.SupplierCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sSupplierId.Length; i++)
                {
                    bMSupplierReply.SupplierId = int.Parse(sSupplierId[i].ToString().Trim());
                    if (IsBMSupplierReply(LB_ID.Text.Trim(), bMSupplierReply.SupplierId.ToString().Trim()))
                    {
                    }
                    else
                    {
                        bMSupplierReplyBLL.AddBMSupplierReply(bMSupplierReply);
                    }
                }
            }

            LoadBMAnnClaFileList();

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    protected bool IsBMSupplierReply(string strAnnClaFileID, string strSupplierId)
    {
        string strHQL = "From BMSupplierReply as bMSupplierReply where bMSupplierReply.AnnClaFileID='" + strAnnClaFileID.Trim() + "' and bMSupplierReply.SupplierId='" + strSupplierId.Trim() + "' ";
        BMSupplierReplyBLL bMSupplierReplyBLL = new BMSupplierReplyBLL();
        IList lst = bMSupplierReplyBLL.GetAllBMSupplierReplys(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            return true;
        }
        else
            return false;
    }

    protected void Delete()
    {
        string strHQL;
        string strCode = LB_ID.Text.Trim();

        strHQL = "Delete From T_BMAnnClaFile Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_BMSupplierReply Where AnnClaFileID = '" + strCode + "' ";
            ShareClass.RunSqlCommand(strHQL);

            LoadBMAnnClaFileList();

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;


                strHQL = "From BMAnnClaFile as bMAnnClaFile where bMAnnClaFile.ID = '" + strID + "'";
                BMAnnClaFileBLL bMAnnClaFileBLL = new BMAnnClaFileBLL();
                lst = bMAnnClaFileBLL.GetAllBMAnnClaFiles(strHQL);
                BMAnnClaFile bMAnnClaFile = (BMAnnClaFile)lst[0];

                LB_ID.Text = bMAnnClaFile.ID.ToString().Trim();
                DL_BidPlanID.SelectedValue = bMAnnClaFile.BidPlanID.ToString().Trim();

                BindSupplierCode(DL_BidPlanID.SelectedValue.Trim());//绑定显示的供应商名称

                DLC_SentDate.Text = bMAnnClaFile.SentDate.ToString("yyyy-MM-dd");
                TB_EnterPer.Text = bMAnnClaFile.EnterPer.Trim();
                TB_SendContent.Text = bMAnnClaFile.SendContent.Trim();
                DL_Type.SelectedValue = bMAnnClaFile.Type.Trim();
                GetBackSupplierCode(bMAnnClaFile.SupplierCode.Trim());//绑定显示选中的供应商名称

                //if (bMAnnClaFile.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Update.Visible = true;
                //    BT_Delete.Visible = true;
                //    BT_Update.Enabled = true;
                //    BT_Delete.Enabled = true;
                //}
                //else
                //{
                //    BT_Update.Visible = false;
                //    BT_Delete.Visible = false;
                //}
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            }


            if (e.CommandName == "Delete")
            {
                Delete();

            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAnnClaFile");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMAnnClaFileList();
    }

    protected void DL_BidPlanID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSupplierCode(DL_BidPlanID.SelectedValue.Trim());

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    /// <summary>
    /// 根据招标计划ID绑定显示的供应商名称
    /// </summary>
    /// <param name="strBidPlanId"></param>
    protected void BindSupplierCode(string strBidPlanId)
    {
        string strHQL;
        //绑定招标公告/招标邀
        strHQL = "select * From T_BMAnnInvitation where BidPlanID='" + strBidPlanId + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAnnInvitation");
        if (ds.Tables[0].Rows.Count > 0 && ds != null)
        {
            string strSupplierId = ds.Tables[0].Rows[0]["BidObjects"].ToString();
            if (string.IsNullOrEmpty(strSupplierId.Trim()) || strSupplierId.Trim() == "")
            {
                LB_SupplierCode.DataSource = null;
                LB_SupplierCode.DataBind();
            }
            else
            {
                strHQL = "select * from T_BMSupplierInfo where ID in (" + strSupplierId + ") ";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");
                LB_SupplierCode.DataSource = ds;
                LB_SupplierCode.DataBind();
            }
        }
    }

    /// <summary>
    /// 获取选定供应商的ID序列，格式为1,2,3......
    /// </summary>
    /// <returns></returns>
    protected string GetSupplierCode()
    {
        string flag = string.Empty;
        if (LB_SupplierCode.Items.Count > 0)
        {
            for (int i = 0; i < LB_SupplierCode.Items.Count; i++)
            {
                if (LB_SupplierCode.Items[i].Selected)
                {
                    flag += LB_SupplierCode.Items[i].Value + ",";
                }
            }
        }

        if (!string.IsNullOrEmpty(flag))
        {
            flag = flag.Substring(0, flag.Length - 1);
        }
        else
            return "0";

        return flag;
    }

    /// <summary>
    /// 根据SupplierCode值显示选中的供应商信息
    /// </summary>
    /// <returns></returns>
    protected void GetBackSupplierCode(string strSupplierCode)
    {
        if (string.IsNullOrEmpty(strSupplierCode) || strSupplierCode.Trim() == "")
        {
        }
        else
        {
            if (LB_SupplierCode.Items.Count > 0)
            {
                for (int i = 0; i < LB_SupplierCode.Items.Count; i++)
                {
                    if (strSupplierCode.Contains(LB_SupplierCode.Items[i].Value))
                    {
                        LB_SupplierCode.Items[i].Selected = true;
                    }
                    else
                        LB_SupplierCode.Items[i].Selected = false;
                }
            }
        }
    }

    protected string GetBMSupplierInfoNameList(string strSupplierIdList)
    {
        string NameList = string.Empty;
        string strHQL = "From BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID in (" + strSupplierIdList.Trim() + ") ";
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[i];
                NameList += bMSupplierInfo.Name.Trim() + ",";
            }
            if (string.IsNullOrEmpty(NameList.Trim()))
            {
                return "";
            }
            else
                return NameList.Substring(0, NameList.Length - 1);
        }
        else
            return "";
    }
}