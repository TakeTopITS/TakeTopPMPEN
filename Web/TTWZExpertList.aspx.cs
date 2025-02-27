using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZExpertList : System.Web.UI.Page
{
    public string strUserCode
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();
        }
    }

    private void DataBinder()
    {
        string strWZExpertDatabaseHQL =@"select e.*,p.UserName as CreateCodeName from T_WZExpertDatabase e
                        left join T_ProjectMember p on e.CreateCode = p.UserCode
                        order by e.Name ";
        DataTable dtExpertDatabase = ShareClass.GetDataSetFromSql(strWZExpertDatabaseHQL, "ExpertDatabase").Tables[0];

        DG_List.DataSource = dtExpertDatabase;
        DG_List.DataBind();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;

            if (cmdName == "click")
            {
                for (int i = 0; i < DG_List.Items.Count; i++)
                {
                    DG_List.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                string[] arrOperate = cmdArges.Split('|');

                string strEditExpertCode = arrOperate[0].Trim();
                string strEditCreateCode = arrOperate[1].Trim();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strEditCreateCode + "','"+strUserCode+"');", true);

                HF_NewExpertCode.Value = strEditExpertCode;
                HF_NewCreateCode.Value = strEditCreateCode;
            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZExpertDatabaseBLL wZExpertDatabaseBLL = new WZExpertDatabaseBLL();
                string strWZExpertDatabaseSql = "from WZExpertDatabase as wZExpertDatabase where ExpertCode = '" + cmdArges + "'";
                IList listWZExpertDatabase = wZExpertDatabaseBLL.GetAllWZExpertDatabases(strWZExpertDatabaseSql);
                if (listWZExpertDatabase != null && listWZExpertDatabase.Count == 1)
                {
                    WZExpertDatabase wZExpertDatabase = (WZExpertDatabase)listWZExpertDatabase[0];
                    wZExpertDatabaseBLL.DeleteWZExpertDatabase(wZExpertDatabase);

                    //重新加载列表
                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                }

            }
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            WZExpertDatabaseBLL wZExpertDatabaseBLL = new WZExpertDatabaseBLL();
            string strNameCode = HF_Name.Value;
            string strName = TXT_Name.Text.Trim();
            string strExpertNumber = TXT_ExpertNumber.Text;
            string strJob = TXT_Job.Text.Trim();
            string strJobTitle = TXT_JobTitle.Text.Trim();
            string strExpertType = HF_ExpertType.Value;
            string strExpertTypeChina = TXT_ExpertTypeChina.Text.Trim();
            strExpertType = strExpertType.EndsWith(",") ? strExpertType.TrimEnd(',') : strExpertType;
            string strPhone = TXT_Phone.Text.Trim();

            string strNewCreateCode = HF_NewCreateCode.Value;

            if (string.IsNullOrEmpty(strName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('专家名称不能为空，请补充！');ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strJob))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('职务不能为空，请补充！');ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true); 
                return;
            }
            if (!ShareClass.CheckStringRight(strJob))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('职务不能为非法字符！');ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true); 
                return;
            }
            if (strJob.Length >6)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('职务不能超过6个字符串！');ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strJobTitle))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('职称不能为空，请补充！');ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true); 
                return;
            }
            if (!ShareClass.CheckStringRight(strJobTitle))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('职称不能为非法字符！');ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true); 
                return;
            }
            if (strJobTitle.Length > 6)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('职称不能超过6个字符串！');ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strPhone))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('移动电话不能为空，请补充！');ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true); 
                return;
            }
            if (!ShareClass.CheckStringRight(strPhone))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('移动电话不能为非法字符！');ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true); 
                return;
            }
            if (strPhone.Length > 11)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('移动电话不能超过11位字符！');ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strExpertType))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('专业范围不能为空，请补充！');ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true); 
                return;
            }

            if (!string.IsNullOrEmpty(HF_ExpertCode.Value))
            {
                //修改
                string strExpertCode = HF_ExpertCode.Value;
                string strWZExpertDatabaseHQL = "from WZExpertDatabase as wZExpertDatabase where ExpertCode = '" + strNameCode + "'";
                IList expertList = wZExpertDatabaseBLL.GetAllWZExpertDatabases(strWZExpertDatabaseHQL);
                if (expertList != null && expertList.Count > 0)
                {
                    WZExpertDatabase wZExpertDatabase = (WZExpertDatabase)expertList[0];

                    //判断新的专家代码查询出来的专家编号是否与原来的相等
                    if (wZExpertDatabase.ExpertNumber.Trim() != HF_ExpertNumber.Value.Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('专家已经存在，不允许重复添加！');ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true); 
                        return;
                    }

                    wZExpertDatabase.ExpertCode = strNameCode;
                    wZExpertDatabase.ExpertNumber = strExpertNumber;
                    wZExpertDatabase.Name = strName;
                    wZExpertDatabase.UserCode = strNameCode;
                    wZExpertDatabase.Job = strJob;
                    wZExpertDatabase.JobTitle = strJobTitle;
                    wZExpertDatabase.ExpertType = strExpertType;
                    wZExpertDatabase.ExpertTypeChina = strExpertTypeChina;
                    wZExpertDatabase.Phone = strPhone;

                    wZExpertDatabaseBLL.UpdateWZExpertDatabase(wZExpertDatabase, strExpertCode);
                }
            }
            else
            {
                //判断专家姓名是否已经存在，不允许重复
                string strCheckNameHQL = "select count(1) as RowNumber from T_WZExpertDatabase where ExpertCode = '" + strNameCode + "'";
                DataTable dtCheckName = ShareClass.GetDataSetFromSql(strCheckNameHQL, "strCheckNameHQL").Tables[0];
                int intCheckNameNumber = int.Parse(dtCheckName.Rows[0]["RowNumber"].ToString());
                if (intCheckNameNumber > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('专家已经存在，不允许重复添加！');ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true); 
                    return;
                }
                
                //增加
                WZExpertDatabase wZExpertDatabase = new WZExpertDatabase();
                wZExpertDatabase.ExpertCode = strNameCode;
                wZExpertDatabase.ExpertNumber = strExpertNumber;
                wZExpertDatabase.Name = strName;
                wZExpertDatabase.UserCode = strNameCode;
                wZExpertDatabase.Job = strJob;
                wZExpertDatabase.JobTitle = strJobTitle;
                wZExpertDatabase.ExpertType = strExpertType;
                wZExpertDatabase.ExpertTypeChina = strExpertTypeChina;
                wZExpertDatabase.Phone = strPhone;
                wZExpertDatabase.WorkingPoint = 0;
                wZExpertDatabase.CreateTime = DateTime.Now;
                wZExpertDatabase.CreateCode = strUserCode;

                wZExpertDatabaseBLL.AddWZExpertDatabase(wZExpertDatabase);
            }

            //重新加载
            DataBinder();

            HF_ExpertCode.Value = "";
            HF_ExpertNumber.Value = "";
            HF_NewExpertCode.Value = "";
            TXT_ExpertNumber.Text = "";
            TXT_Name.Text = "";
            TXT_Job.Text = "";
            TXT_ExpertTypeChina.Text = "";
            TXT_Phone.Text = "";
            TXT_JobTitle.Text = "";

            TXT_Name.BackColor = Color.White;
            TXT_Job.BackColor = Color.White;
            TXT_ExpertTypeChina.BackColor = Color.White;
            TXT_Phone.BackColor = Color.White;
            TXT_JobTitle.BackColor = Color.White;
            
            //Response.Redirect("TTWZExpertList.aspx");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatusCloseChange();", true); 
        }
        catch (Exception ex)
        { }
    }

    protected void BT_Cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        HF_ExpertCode.Value = "";
        HF_ExpertNumber.Value = "";
        HF_NewExpertCode.Value = "";
        TXT_ExpertNumber.Text = "";
        TXT_Name.Text = "";
        TXT_Job.Text = "";
        TXT_ExpertTypeChina.Text = "";
        TXT_Phone.Text = "";
        TXT_JobTitle.Text = "";

        TXT_Name.BackColor = Color.White;
        TXT_Job.BackColor = Color.White;
        TXT_ExpertTypeChina.BackColor = Color.White;
        TXT_Phone.BackColor = Color.White;
        TXT_JobTitle.BackColor = Color.White;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    protected void BT_NewAdd_Click(object sender, EventArgs e)
    {
        TXT_ExpertNumber.Text = CreateNewExpertNumber();

        HF_ExpertCode.Value = "";
        HF_ExpertNumber.Value = "";
        HF_NewExpertCode.Value = "";
        TXT_Name.Text = "";
        TXT_Job.Text = "";
        TXT_ExpertTypeChina.Text = "";
        TXT_Phone.Text = "";
        TXT_JobTitle.Text = "";

        TXT_Name.BackColor = Color.CornflowerBlue;
        TXT_Job.BackColor = Color.CornflowerBlue;
        TXT_ExpertTypeChina.BackColor = Color.CornflowerBlue;
        TXT_Phone.BackColor = Color.CornflowerBlue;
        TXT_JobTitle.BackColor = Color.CornflowerBlue;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        string strEditExpertCode = HF_NewExpertCode.Value;
        if (string.IsNullOrEmpty(strEditExpertCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYCZDZJLB").ToString().Trim()+"')", true);
            return;
        }

        string strExpertSql = string.Format(@"select e.*,n.UserName as ExpertName from T_WZExpertDatabase e
                        left join T_ProjectMember n on e.UserCode = n.UserCode 
                        where e.ExpertCode = '{0}'", strEditExpertCode);
        DataTable dtExpert = ShareClass.GetDataSetFromSql(strExpertSql, "Expert").Tables[0];
        if (dtExpert != null && dtExpert.Rows.Count == 1)
        {
            DataRow drExpert = dtExpert.Rows[0];

            TXT_ExpertNumber.Text = ShareClass.ObjectToString(drExpert["ExpertNumber"]);
            HF_Name.Value = ShareClass.ObjectToString(drExpert["UserCode"]);
            TXT_Name.Text = ShareClass.ObjectToString(drExpert["Name"]);
            TXT_Job.Text = ShareClass.ObjectToString(drExpert["Job"]);
            HF_ExpertType.Value = ShareClass.ObjectToString(drExpert["ExpertType"]);
            TXT_ExpertTypeChina.Text = ShareClass.ObjectToString(drExpert["ExpertTypeChina"]);

            TXT_Phone.Text = ShareClass.ObjectToString(drExpert["Phone"]);
            TXT_JobTitle.Text = ShareClass.ObjectToString(drExpert["JobTitle"]);

            TXT_Name.BackColor = Color.CornflowerBlue;
            TXT_Job.BackColor = Color.CornflowerBlue;
            TXT_ExpertTypeChina.BackColor = Color.CornflowerBlue;
            TXT_Phone.BackColor = Color.CornflowerBlue;
            TXT_JobTitle.BackColor = Color.CornflowerBlue;

            HF_ExpertCode.Value = ShareClass.ObjectToString(drExpert["ExpertCode"]);
            HF_ExpertNumber.Value = ShareClass.ObjectToString(drExpert["ExpertNumber"]);

            string strNewCreateCode = HF_NewCreateCode.Value;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewCreateCode + "','" + strUserCode + "');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }


    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        string strEditExpertCode = HF_NewExpertCode.Value;
        if (string.IsNullOrEmpty(strEditExpertCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYCZDZJLB").ToString().Trim()+"')", true);
            return;
        }

        WZExpertDatabaseBLL wZExpertDatabaseBLL = new WZExpertDatabaseBLL();
        string strWZExpertDatabaseSql = "from WZExpertDatabase as wZExpertDatabase where ExpertCode = '" + strEditExpertCode + "'";
        IList listWZExpertDatabase = wZExpertDatabaseBLL.GetAllWZExpertDatabases(strWZExpertDatabaseSql);
        if (listWZExpertDatabase != null && listWZExpertDatabase.Count == 1)
        {
            WZExpertDatabase wZExpertDatabase = (WZExpertDatabase)listWZExpertDatabase[0];
            wZExpertDatabaseBLL.DeleteWZExpertDatabase(wZExpertDatabase);

            //重新加载列表
            DataBinder();

            HF_ExpertCode.Value = "";
            HF_ExpertNumber.Value = "";
            HF_NewExpertCode.Value = "";
            TXT_ExpertNumber.Text = "";
            TXT_Name.Text = "";
            TXT_Job.Text = "";
            TXT_ExpertTypeChina.Text = "";
            TXT_Phone.Text = "";
            TXT_JobTitle.Text = "";

            TXT_Name.BackColor = Color.White;
            TXT_Job.BackColor = Color.White;
            TXT_ExpertTypeChina.BackColor = Color.White;
            TXT_Phone.BackColor = Color.White;
            TXT_JobTitle.BackColor = Color.White;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"');ControlStatusCloseChange();", true);
        }
        else {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }



    /// <summary>
    ///  生成专家编号
    /// </summary>
    private string CreateNewExpertNumber()
    {
        string strNewExpertNumber = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                int intExpertNumber = 1;
                do
                {
                    StringBuilder sbExpertNumber = new StringBuilder();
                    for (int j = 4 - intExpertNumber.ToString().Length; j > 0; j--)
                    {
                        sbExpertNumber.Append("0");
                    }
                    strNewExpertNumber = sbExpertNumber.ToString() + "" + intExpertNumber.ToString();

                    //验证新的专家编号是否存在
                    string strCheckNewExpertNumberHQL = "select count(1) as RowNumber from T_WZExpertDatabase where ExpertNumber = '" + strNewExpertNumber + "'";
                    DataTable dtCheckNewExpertNumber = ShareClass.GetDataSetFromSql(strCheckNewExpertNumberHQL, "ExpertNumber").Tables[0];
                    int intCheckNewExpertNumber = int.Parse(dtCheckNewExpertNumber.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewExpertNumber == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intExpertNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewExpertNumber;
    }
}
