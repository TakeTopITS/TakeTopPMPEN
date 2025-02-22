using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZGetUnitList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();
        }
    }

    private void DataBinder()
    {
        string strUnitHQL = string.Format(@"select g.*,pl.UserName as LeaderName,
                    pd.UserName as DelegateAgentName,
                    pf.UserName as FeeManageName,
                    pm.UserName as MaterialPersonName
                    from T_WZGetUnit g
                    left join T_ProjectMember pl on g.Leader = pl.UserCode
                    left join T_ProjectMember pd on g.DelegateAgent = pd.UserCode
                    left join T_ProjectMember pf on g.FeeManage = pf.UserCode
                    left join T_ProjectMember pm on g.MaterialPerson = pm.UserCode
                    order by g.UnitCode desc");
        DataTable dtUnit = ShareClass.GetDataSetFromSql(strUnitHQL, "Unit").Tables[0];

        DG_List.DataSource = dtUnit;
        DG_List.DataBind();

        LB_Record.Text = dtUnit.Rows.Count.ToString();
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

                string strNewUnitCode = arrOperate[0];
                string strNewIsMark = arrOperate[1];

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewIsMark + "');", true);

                HF_NewUnitCode.Value = strNewUnitCode;
                HF_NewIsMark.Value = strNewIsMark;
            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
                string strUnitSql = "from WZGetUnit as wZGetUnit where UnitCode = '" + cmdArges + "'";
                IList unitList = wZGetUnitBLL.GetAllWZGetUnits(strUnitSql);
                if (unitList != null && unitList.Count == 1)
                {
                    WZGetUnit wZGetUnit = (WZGetUnit)unitList[0];
                    if (wZGetUnit.IsMark == -1)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYBJW1BYXSC+"')", true);
                        return;
                    }

                    wZGetUnitBLL.DeleteWZGetUnit(wZGetUnit);

                    //重新加载列表
                    DataBinder();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);
                }

            }
        }
    }





    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strUnitName = TXT_UnitName.Text.Trim();
            string strLeader = HF_Leader.Value;
            string strFeeManage = HF_FeeManage.Value;
            string strMaterialPerson = HF_MaterialPerson.Value;

            string strNewIsMark = HF_NewIsMark.Value;

            if (string.IsNullOrEmpty(strUnitName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('领料单位不能为空，请补充！');ControlStatusChange('" + strNewIsMark + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strLeader))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('主管领导不能为空，请补充！');ControlStatusChange('" + strNewIsMark + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strFeeManage))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('费控主管不能为空，请补充！');ControlStatusChange('" + strNewIsMark + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strMaterialPerson))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('材料员不能为空，请补充！');ControlStatusChange('" + strNewIsMark + "')", true);
                return;
            }

            WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
            if (!string.IsNullOrEmpty(HF_UnitCode.Value))
            {
                //修改
                string strUnitCode = HF_UnitCode.Value;
                string strUnitHQL = "from WZGetUnit as wZGetUnit where UnitCode = '" + strUnitCode + "'";
                IList unitList = wZGetUnitBLL.GetAllWZGetUnits(strUnitHQL);
                if (unitList != null && unitList.Count > 0)
                {
                    WZGetUnit wZGetUnit = (WZGetUnit)unitList[0];

                    wZGetUnit.UnitName = strUnitName;
                    wZGetUnit.Leader = strLeader;
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
                wZGetUnit.FeeManage = strFeeManage;
                wZGetUnit.MaterialPerson = strMaterialPerson;

                wZGetUnitBLL.AddWZGetUnit(wZGetUnit);
            }

            TXT_UnitCode.Text = "";
            HF_UnitCode.Value = "";
            HF_NewUnitCode.Value = "";
            HF_NewIsMark.Value = "";
            TXT_UnitName.Text = "";
            TXT_Leader.Text = "";
            HF_Leader.Value = "";
            DDL_UnitType.SelectedValue = "行政单位";
            TXT_FeeManage.Text = "";
            HF_FeeManage.Value = "";
            TXT_MaterialPerson.Text = "";
            HF_MaterialPerson.Value = "";

            TXT_UnitName.BackColor = Color.White;
            TXT_Leader.BackColor = Color.White;
            DDL_UnitType.BackColor = Color.White;
            TXT_FeeManage.BackColor = Color.White;
            TXT_MaterialPerson.BackColor = Color.White;

            //重新加载
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatusCloseChange();", true);
            //Response.Redirect("TTWZGetUnitList.aspx");
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

        TXT_UnitCode.Text = "";
        HF_UnitCode.Value = "";
        HF_NewUnitCode.Value = "";
        HF_NewIsMark.Value = "";
        TXT_UnitName.Text = "";
        TXT_Leader.Text ="";
        HF_Leader.Value ="";
        DDL_UnitType.SelectedValue = "行政单位";
        TXT_FeeManage.Text = "";
        HF_FeeManage.Value = "";
        TXT_MaterialPerson.Text = "";
        HF_MaterialPerson.Value = "";

        TXT_UnitName.BackColor = Color.White;
        TXT_Leader.BackColor = Color.White;
        DDL_UnitType.BackColor = Color.White;
        TXT_FeeManage.BackColor = Color.White;
        TXT_MaterialPerson.BackColor = Color.White;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    protected void BT_Add_Click(object sender, EventArgs e)
    {
        //新增领料单位
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        TXT_UnitCode.Text = "";
        HF_UnitCode.Value = "";
        HF_NewUnitCode.Value = "";
        HF_NewIsMark.Value = "";
        TXT_UnitName.Text = "";
        TXT_Leader.Text = "";
        HF_Leader.Value = "";
        DDL_UnitType.SelectedValue = "行政单位";
        TXT_FeeManage.Text = "";
        HF_FeeManage.Value = "";
        TXT_MaterialPerson.Text = "";
        HF_MaterialPerson.Value = "";

        TXT_UnitName.BackColor = Color.CornflowerBlue;
        TXT_Leader.BackColor = Color.CornflowerBlue;
        DDL_UnitType.BackColor = Color.CornflowerBlue;
        TXT_FeeManage.BackColor = Color.CornflowerBlue;
        TXT_MaterialPerson.BackColor = Color.CornflowerBlue;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strEditUnitCode = HF_NewUnitCode.Value;
        if (string.IsNullOrEmpty(strEditUnitCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJYCZDLLDWLB+"')", true);
            return;
        }

        string strGetUnitHQL = string.Format(@"select g.*,pl.UserName as LeaderName,
                    pf.UserName as FeeManageName,
                    pm.UserName as MaterialPersonName
                    from T_WZGetUnit g
                    left join T_ProjectMember pl on g.Leader = pl.UserCode
                    left join T_ProjectMember pf on g.FeeManage = pf.UserCode
                    left join T_ProjectMember pm on g.MaterialPerson = pm.UserCode
                    where g.UnitCode = '{0}'", strEditUnitCode);
        DataTable dtGetUnit = ShareClass.GetDataSetFromSql(strGetUnitHQL, "GetUnit").Tables[0];

        string strNewIsMark = HF_NewIsMark.Value;

        if (dtGetUnit != null && dtGetUnit.Rows.Count > 0)
        {
            DataRow drGetUnit = dtGetUnit.Rows[0];

            string strIsMark = ShareClass.ObjectToString(drGetUnit["IsMark"]);

            TXT_UnitName.BackColor = Color.CornflowerBlue;
            TXT_Leader.BackColor = Color.CornflowerBlue;
            TXT_FeeManage.BackColor = Color.CornflowerBlue;
            TXT_MaterialPerson.BackColor = Color.CornflowerBlue;

            if (strIsMark == "-1")
            {
                TXT_UnitName.ReadOnly = true;
                TXT_UnitName.BackColor = Color.White;
            }

            string strNewUnitCode = ShareClass.ObjectToString(drGetUnit["UnitCode"]);
            TXT_UnitCode.Text = strNewUnitCode;
            if (strNewUnitCode.Contains("01"))
            {
                DDL_UnitType.SelectedValue = "行政单位";
            }
            else
            {
                DDL_UnitType.SelectedValue = "项目部";
            }
            TXT_UnitName.Text = ShareClass.ObjectToString(drGetUnit["UnitName"]);// wZGetUnit.UnitName;
            TXT_Leader.Text = ShareClass.ObjectToString(drGetUnit["LeaderName"]);//wZGetUnit.Leader;
            HF_Leader.Value = ShareClass.ObjectToString(drGetUnit["Leader"]);
            TXT_FeeManage.Text = ShareClass.ObjectToString(drGetUnit["FeeManageName"]);//wZGetUnit.FeeManage;
            HF_FeeManage.Value = ShareClass.ObjectToString(drGetUnit["FeeManage"]);
            TXT_MaterialPerson.Text = ShareClass.ObjectToString(drGetUnit["MaterialPersonName"]);//wZGetUnit.MaterialPerson;
            HF_MaterialPerson.Value = ShareClass.ObjectToString(drGetUnit["MaterialPerson"]);

            DDL_UnitType.Enabled = false;

            HF_UnitCode.Value = strNewUnitCode;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewIsMark + "')", true);
        }
        else {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewIsMark + "')", true);
        }
    }




    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //删除
        string strEditUnitCode = HF_NewUnitCode.Value;
        if (string.IsNullOrEmpty(strEditUnitCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJYCZDLLDWLB+"')", true);
            return;
        }

        WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
        string strUnitSql = "from WZGetUnit as wZGetUnit where UnitCode = '" + strEditUnitCode + "'";
        IList unitList = wZGetUnitBLL.GetAllWZGetUnits(strUnitSql);

        string strNewIsMark = HF_NewIsMark.Value;

        if (unitList != null && unitList.Count == 1)
        {
            WZGetUnit wZGetUnit = (WZGetUnit)unitList[0];
            if (wZGetUnit.IsMark == -1)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYBJW1BYXSC+"');ControlStatusChange('" + strNewIsMark + "');", true);
                return;
            }

            wZGetUnitBLL.DeleteWZGetUnit(wZGetUnit);

            TXT_UnitCode.Text = "";
            HF_UnitCode.Value = "";
            HF_NewUnitCode.Value = "";
            HF_NewIsMark.Value = "";
            TXT_UnitName.Text = "";
            TXT_Leader.Text = "";
            HF_Leader.Value = "";
            DDL_UnitType.SelectedValue = "行政单位";
            TXT_FeeManage.Text = "";
            HF_FeeManage.Value = "";
            TXT_MaterialPerson.Text = "";
            HF_MaterialPerson.Value = "";

            TXT_UnitName.BackColor = Color.White;
            TXT_Leader.BackColor = Color.White;
            DDL_UnitType.BackColor = Color.White;
            TXT_FeeManage.BackColor = Color.White;
            TXT_MaterialPerson.BackColor = Color.White;

            //重新加载列表
            DataBinder();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"');ControlStatusCloseChange();", true);
        }
        else {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewIsMark + "')", true);
        }
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
                    if (strUnitType == "行政单位")
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
                    else if (strUnitType == "项目部")
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