using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZExpertEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            //BindGetUnitData();

            if (!string.IsNullOrEmpty(Request.QueryString["strExpertCode"]))
            {
                string strExpertCode = Request.QueryString["strExpertCode"].ToString();
                HF_ID.Value = strExpertCode;

                BindDataer(strExpertCode);
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            WZExpertDatabaseBLL wZExpertDatabaseBLL = new WZExpertDatabaseBLL();
            string strName = TXT_Name.Text.Trim();
            string strExpertUserCode = HF_UserCode.Value;
            string strJob = TXT_Job.Text.Trim();
            string strJobTitle = TXT_JobTitle.Text.Trim();
            string strExpertType = HF_ExpertType.Value;
            string strExpertTypeChina = TXT_ExpertTypeChina.Text.Trim();
            strExpertType = strExpertType.EndsWith(",") ? strExpertType.TrimEnd(',') : strExpertType;
            string strPhone = TXT_Phone.Text.Trim();
            if (string.IsNullOrEmpty(strName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZJMCBNWKBC+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strJob))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZWBNWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strJob))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZWBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strJobTitle))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZCBNWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strJobTitle))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZCBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPhone))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZLXDHBNWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPhone))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZLXDHBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strExpertType))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZYFWBNWKBC+"')", true);
                return;
            }

            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //修改
                string strExpertCode = HF_ID.Value;
                string strWZExpertDatabaseHQL = "from WZExpertDatabase as wZExpertDatabase where ExpertCode = '" + strExpertCode + "'";
                IList expertList = wZExpertDatabaseBLL.GetAllWZExpertDatabases(strWZExpertDatabaseHQL);
                if (expertList != null && expertList.Count > 0)
                {
                    WZExpertDatabase wZExpertDatabase = (WZExpertDatabase)expertList[0];
                    wZExpertDatabase.Name = strName;
                    wZExpertDatabase.UserCode = strExpertUserCode;
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
                string strCheckNameHQL = "select count(1) as RowNumber from T_WZExpertDatabase where ExpertCode = '" + strExpertUserCode + "'";
                DataTable dtCheckName = ShareClass.GetDataSetFromSql(strCheckNameHQL, "strCheckNameHQL").Tables[0];
                int intCheckNameNumber = int.Parse(dtCheckName.Rows[0]["RowNumber"].ToString());
                if (intCheckNameNumber > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZJYJCZBYXZFTJ+"')", true);
                    return;
                }
                //生成专家编号
                //string strNewExpertCode = CreateNewExpertCode();
                

                //增加
                WZExpertDatabase wZExpertDatabase = new WZExpertDatabase();
                wZExpertDatabase.ExpertCode = strExpertUserCode; //strNewExpertCode;
                wZExpertDatabase.Name = strName;
                wZExpertDatabase.UserCode = strExpertUserCode;
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

            Response.Redirect("TTWZExpertList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindDataer(string strExpertCode)
    {
        WZExpertDatabaseBLL wZExpertDatabaseBLL = new WZExpertDatabaseBLL();
        string strWZExpertDatabaseSql = "from WZExpertDatabase as wZExpertDatabase where ExpertCode = '" + strExpertCode + "'";
        IList expertList = wZExpertDatabaseBLL.GetAllWZExpertDatabases(strWZExpertDatabaseSql);
        if (expertList != null && expertList.Count > 0)
        {
            WZExpertDatabase wZExpertDatabase = (WZExpertDatabase)expertList[0];
            TXT_ExpertCode.Text = wZExpertDatabase.ExpertCode;
            TXT_Name.Text = wZExpertDatabase.Name;
            HF_UserCode.Value = wZExpertDatabase.UserCode;
            TXT_Job.Text = wZExpertDatabase.Job;
            TXT_JobTitle.Text = wZExpertDatabase.JobTitle;
            HF_ExpertType.Value = wZExpertDatabase.ExpertType;
            TXT_ExpertTypeChina.Text = wZExpertDatabase.ExpertTypeChina;
            TXT_Phone.Text = wZExpertDatabase.Phone;

            TXT_Name.ReadOnly = true;
            btnName.Disabled = true;

        }
    }


    //private void BindGetUnitData()
    //{
    //    string strDepartmentHQL = "select DepartCode,DepartName from T_Department";
    //    DataTable dtDepartment = ShareClass.GetDataSetFromSql(strDepartmentHQL, "Department").Tables[0];

    //    DDL_WorkUnit.DataSource = dtDepartment;
    //    DDL_WorkUnit.DataTextField = "DepartName";
    //    DDL_WorkUnit.DataValueField = "DepartCode";
    //    DDL_WorkUnit.DataBind();
    //    #region 注释领料单位
    //    //WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
    //    //string strGetUnitHQL = "from WZGetUnit as wZGetUnit";
    //    //IList lstGetUnit = wZGetUnitBLL.GetAllWZGetUnits(strGetUnitHQL);

    //    //DDL_WorkUnit.DataSource = lstGetUnit;
    //    DDL_WorkUnit.DataTextField = "UnitName";
    //    DDL_WorkUnit.DataValueField = "UnitCode";
    //    //DDL_WorkUnit.DataBind();
    //    #endregion
    //}


    //private string CreateNewExpertCode()
    //{
    //    //生成专家编码
    //    string strNewExpertCode = string.Empty;
    //    try
    //    {
    //        lock (this)
    //        {
    //            bool isExist = true;
    //            int intExpertCodeNumber = 1;
    //            do
    //            {
    //                StringBuilder sbExpertCode = new StringBuilder();
    //                for (int j = 4 - intExpertCodeNumber.ToString().Length; j > 0; j--)
    //                {
    //                    sbExpertCode.Append("0");
    //                }
    //                strNewExpertCode = sbExpertCode.ToString() + intExpertCodeNumber.ToString();

    //                //验证专家编码是否存在
    //                string strCheckNewExpertCodeHQL = "select count(1) as RowNumber from T_WZExpert where ExpertCode = '" + strNewExpertCode + "'";
    //                DataTable dtCheckNewExpertCode = ShareClass.GetDataSetFromSql(strCheckNewExpertCodeHQL, "strCheckNewExpertCodeHQL").Tables[0];
    //                int intCheckNewExpertCode = int.Parse(dtCheckNewExpertCode.Rows[0]["RowNumber"].ToString());
    //                if (intCheckNewExpertCode == 0)
    //                {
    //                    isExist = false;
    //                }
    //                else
    //                {
    //                    intExpertCodeNumber++;
    //                }
    //            } while (isExist);
    //        }
    //    }
    //    catch (Exception ex) { }

    //    return strNewExpertCode;
    //}
}