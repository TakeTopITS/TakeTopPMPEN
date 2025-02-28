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

public partial class TTWZGetUnitEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["strUnitCode"]))
            {
                string strUnitCode = Request.QueryString["strUnitCode"].ToString();
                HF_ID.Value = strUnitCode;

                BindDataer(strUnitCode);
            }
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strUnitName = TXT_UnitName.Text.Trim();
            //string strLeaderName = TXT_Leader.Text.Trim();
            string strLeader = HF_Leader.Value;
            //string strDelegateAgentName = TXT_DelegateAgent.Text.Trim();
            string strDelegateAgent = HF_DelegateAgent.Value;
            //string strFeeManageName = TXT_FeeManage.Text.Trim();
            string strFeeManage = HF_FeeManage.Value;
            //string strMaterialPersonName = TXT_MaterialPerson.Text.Trim();
            string strMaterialPerson = HF_MaterialPerson.Value;
            if (string.IsNullOrEmpty(strUnitName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZLLDWBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strLeader))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZGLDBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strDelegateAgent))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZWTDLRBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strFeeManage))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZFKZGBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strMaterialPerson))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCLYBNWKBC").ToString().Trim()+"')", true);
                return;
            }

            WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //修改
                string strUnitCode = HF_ID.Value;
                string strUnitHQL = "from WZGetUnit as wZGetUnit where UnitCode = '" + strUnitCode + "'";
                IList unitList = wZGetUnitBLL.GetAllWZGetUnits(strUnitHQL);
                if (unitList != null && unitList.Count > 0)
                {
                    WZGetUnit wZGetUnit = (WZGetUnit)unitList[0];

                    wZGetUnit.UnitName = strUnitName;
                    wZGetUnit.Leader = strLeader;
                    wZGetUnit.DelegateAgent = strDelegateAgent;
                    wZGetUnit.FeeManage = strFeeManage;
                    wZGetUnit.MaterialPerson = strMaterialPerson;

                    wZGetUnitBLL.UpdateWZGetUnit(wZGetUnit, strUnitCode);
                }
            }
            else
            {
                //增加
                WZGetUnit wZGetUnit = new WZGetUnit();

                wZGetUnit.UnitCode = CreateNewGetUnitCode(DDL_UnitType.SelectedValue);//TXT_UnitCode.Text.Trim(); 生成
                wZGetUnit.UnitName = strUnitName;
                wZGetUnit.Leader = strLeader;
                wZGetUnit.DelegateAgent = strDelegateAgent;
                wZGetUnit.FeeManage = strFeeManage;
                wZGetUnit.MaterialPerson = strMaterialPerson;

                wZGetUnitBLL.AddWZGetUnit(wZGetUnit);
            }

            Response.Redirect("TTWZGetUnitList.aspx");
        }
        catch (Exception ex)
        { }
    }

    private void BindDataer(string strUnitCode)
    {
        string strGetUnitHQL = string.Format(@"select g.*,pl.UserName as LeaderName,
                    pd.UserName as DelegateAgentName,
                    pf.UserName as FeeManageName,
                    pm.UserName as MaterialPersonName
                    from T_WZGetUnit g
                    left join T_ProjectMember pl on g.Leader = pl.UserCode
                    left join T_ProjectMember pd on g.DelegateAgent = pd.UserCode
                    left join T_ProjectMember pf on g.FeeManage = pf.UserCode
                    left join T_ProjectMember pm on g.MaterialPerson = pm.UserCode
                    where g.UnitCode = '{0}'", strUnitCode);
        DataTable dtGetUnit = ShareClass.GetDataSetFromSql(strGetUnitHQL, "GetUnit").Tables[0];
        if (dtGetUnit != null && dtGetUnit.Rows.Count > 0)
        {
            DataRow drGetUnit = dtGetUnit.Rows[0];

            string strNewUnitCode = ShareClass.ObjectToString(drGetUnit["UnitCode"]);
            TXT_UnitCode.Text = strNewUnitCode;
            if (strNewUnitCode.Contains("01"))
            {
                DDL_UnitType.SelectedValue = "行政单位";   //ChineseWord
            }
            else
            {
                DDL_UnitType.SelectedValue = "ProjectDepartment";   //ChineseWord
            }
            TXT_UnitName.Text = ShareClass.ObjectToString(drGetUnit["UnitName"]);// wZGetUnit.UnitName;
            TXT_Leader.Text = ShareClass.ObjectToString(drGetUnit["LeaderName"]);//wZGetUnit.Leader;
            HF_Leader.Value = ShareClass.ObjectToString(drGetUnit["Leader"]);
            TXT_DelegateAgent.Text = ShareClass.ObjectToString(drGetUnit["DelegateAgentName"]);//wZGetUnit.DelegateAgent;
            HF_DelegateAgent.Value = ShareClass.ObjectToString(drGetUnit["DelegateAgent"]);
            TXT_FeeManage.Text = ShareClass.ObjectToString(drGetUnit["FeeManageName"]);//wZGetUnit.FeeManage;
            HF_FeeManage.Value = ShareClass.ObjectToString(drGetUnit["FeeManage"]);
            TXT_MaterialPerson.Text = ShareClass.ObjectToString(drGetUnit["MaterialPersonName"]);//wZGetUnit.MaterialPerson;
            HF_MaterialPerson.Value = ShareClass.ObjectToString(drGetUnit["MaterialPerson"]);

            DDL_UnitType.Enabled = false;
        }
        #region 注释没有人员名称的
        //WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
        //string strWZGetUnitSql = "from WZGetUnit as wZGetUnit where UnitCode = '" + strUnitCode + "'";
        //IList unitList = wZGetUnitBLL.GetAllWZGetUnits(strWZGetUnitSql);
        //if (unitList != null && unitList.Count > 0)
        //{
        //    WZGetUnit wZGetUnit = (WZGetUnit)unitList[0];
        //    string strNewUnitCode =wZGetUnit.UnitCode;
        //    TXT_UnitCode.Text = strNewUnitCode;
        //    if (strNewUnitCode.Contains("01"))
        //    {
        //        DDL_UnitType.SelectedValue = "行政单位";
        //    }
        //    else {
        //        DDL_UnitType.SelectedValue = "ProjectDepartment";
        //    }
        //    TXT_UnitName.Text = wZGetUnit.UnitName;
        //    TXT_Leader.Text = wZGetUnit.Leader;
        //    TXT_DelegateAgent.Text = wZGetUnit.DelegateAgent;
        //    TXT_FeeManage.Text = wZGetUnit.FeeManage;
        //    TXT_MaterialPerson.Text = wZGetUnit.MaterialPerson;
        //}
        #endregion
    }


    private string CreateNewGetUnitCode(string strUnitType)
    {
        //生成领料单号  总共长度为4位   0101-0199 0201-9999 
        string strNewUnitCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                int intGetUnitCodeNumber = 0;
                do
                {
                    if (strUnitType == "行政单位")   //ChineseWord
                    {
                        //以行政单位作为领料单位的部分，在创建本表单时直接导入，后期变更由物资管理员负责编辑，编号范围控制在0101-0199之间
                        int intRowNumber = 0;
                        intRowNumber = intGetUnitCodeNumber + 1;
                        if (intRowNumber.ToString().Length == 1)
                        {
                            strNewUnitCode = "010" + intRowNumber.ToString();
                        }
                        else
                        {
                            strNewUnitCode = "01" + intRowNumber.ToString();
                        }
                    }
                    else if (strUnitType == "ProjectDepartment")   //ChineseWord
                    {
                        //以项目部作为领料单位的部分，由经营管理部负责编辑，编号范围控制在0201-9999之间
                        int intRowNumber = 0;
                        intRowNumber = intGetUnitCodeNumber + 1;
                        if ((intRowNumber + 200).ToString().Length == 3)
                        {
                            strNewUnitCode = "0" + (intRowNumber + 200).ToString();
                        }
                        else
                        {
                            strNewUnitCode = (intRowNumber + 200).ToString();
                        }
                    }

                    //验证新的项目编号是滞存在
                    string strCheckNewUnitCodeHQL = "select count(1) as RowNumber from T_WZGetUnit where UnitCode = '" + strNewUnitCode + "'";
                    DataTable dtCheckNewUnitCode = ShareClass.GetDataSetFromSql(strCheckNewUnitCodeHQL, "CheckNewUnitCode").Tables[0];
                    int intCheckNewUnitCode = int.Parse(dtCheckNewUnitCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewUnitCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intGetUnitCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }

        return strNewUnitCode;
    }
}
