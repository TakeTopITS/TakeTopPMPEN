using System; using System.Resources;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTSelectorProjectPlan : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["PlanID"]))
            {
                string strPlanID = Request.QueryString["PlanID"];

                HF_PlanID.Value = strPlanID;

                DataBinder(strPlanID);
            }
        }
    }

    private void DataBinder(string strPlanID)
    {
        #region 正式使用
        //        string strProjectPlanHQL = @"select p.*,COALESCE(r.PlanID,'0') as IsExist from T_Project p
//                    left join T_ProjectPlanRelated_YYUP r on p.ProjectID = r.ProjectID
        //                    where p.ProjectClass = '模板项目'";
        #endregion

        string strProjectPlanHQL = string.Format(@"select p.*,COALESCE(r.PlanID,'0') as IsExist from T_Project p
                    left join T_ProjectPlanRelated_YYUP r on p.ProjectID = r.ProjectID
                    and r.PlanID = {0}
                    where p.ProjectClass = '模板项目'", strPlanID); 
        DataTable dtProjectPlan = ShareClass.GetDataSetFromSql(strProjectPlanHQL, "ProjectPlan").Tables[0];

        DG_List.DataSource = dtProjectPlan;
        DG_List.DataBind();


    }





    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            for (int i = 0; i < DG_List.Items.Count; i++)
            {
                DG_List.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdName = e.CommandName;
            if (cmdName == "select")
            {
                //选择
                try
                {
                    string cmdArges = e.CommandArgument.ToString();

                    //先判断当前实施路线是否有项目了
                    string strProjectSql = string.Format(@"select p.*,r.ProjectID as RelateProjectID from T_ProjectPlanRelated_YYUP r
                                left join T_Project p on r.ProjectID = p.ProjectID
                                where r.PlanID = {0}", HF_PlanID.Value);
                    DataTable dtProject = ShareClass.GetDataSetFromSql(strProjectSql, "project").Tables[0];
                    if (dtProject != null && dtProject.Rows.Count > 0)
                    {
                        DataRow drProject = dtProject.Rows[0];

                        string strExistProjectID = ShareClass.ObjectToString(drProject["ProjectID"] == DBNull.Value ? "" : drProject["ProjectID"]);

                        if (!string.IsNullOrEmpty(strExistProjectID))
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDSSLXYJBDXMSHARECLASSOBJECTTOSTRINGDRPROJECTPROJECTNAMETRIMXXBD").ToString().Trim()+"')", true);
                            return;
                        }
                        else { 
                            //项目已经不存在了，先删除关联关系
                            string strDeleteProjectPlanRelatedSQL = string.Format(@"delete T_ProjectPlanRelated_YYUP where PlanID={0} and ProjectID={1}", HF_PlanID.Value, ShareClass.ObjectToString(drProject["RelateProjectID"]));
                            ShareClass.RunSqlCommand(strDeleteProjectPlanRelatedSQL);

                            string strInsertSQL = string.Format(@"insert into T_ProjectPlanRelated_YYUP(PlanID,ProjectID) values({0},{1})", HF_PlanID.Value, cmdArges);
                            ShareClass.RunSqlCommand(strInsertSQL);

                            DataBinder(HF_PlanID.Value);

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
                        }
                    }
                    else
                    {
                        string strInsertSQL = string.Format(@"insert into T_ProjectPlanRelated_YYUP(PlanID,ProjectID) values({0},{1})", HF_PlanID.Value, cmdArges);
                        ShareClass.RunSqlCommand(strInsertSQL);

                        DataBinder(HF_PlanID.Value);

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
                    }
                }
                catch (Exception ex) {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZCCJC").ToString().Trim()+"')", true);
                }
            }
            else if (cmdName == "cancel")
            {
                //取消
                try
                {
                    string cmdArges = e.CommandArgument.ToString();

                    string strDeleteSql = string.Format(@"delete T_ProjectPlanRelated_YYUP where PlanID = {0} and ProjectID = {1}", HF_PlanID.Value, cmdArges);
                    ShareClass.RunSqlCommand(strDeleteSql);


                    //重新加载列表
                    DataBinder(HF_PlanID.Value);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXCCJC").ToString().Trim()+"')", true);
                }
            }

        }
    }




    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strPlanID = HF_PlanID.Value;
        if (!string.IsNullOrEmpty(strPlanID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXCSSLXJLZXM").ToString().Trim()+"')", true);
            return;
        }

        //string strProjectID = DDL_Project.SelectedValue;
        //if (!string.IsNullOrEmpty(strProjectID))
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZXM").ToString().Trim()+"')", true);
        //    return;
        //}
    }
}
