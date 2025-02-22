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

                    //���¼����б�
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ϵ�λ����Ϊ�գ��벹�䣡');ControlStatusChange('" + strNewIsMark + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strLeader))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�����쵼����Ϊ�գ��벹�䣡');ControlStatusChange('" + strNewIsMark + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strFeeManage))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�ѿ����ܲ���Ϊ�գ��벹�䣡');ControlStatusChange('" + strNewIsMark + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strMaterialPerson))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����Ա����Ϊ�գ��벹�䣡');ControlStatusChange('" + strNewIsMark + "')", true);
                return;
            }

            WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
            if (!string.IsNullOrEmpty(HF_UnitCode.Value))
            {
                //�޸�
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
                //����
                WZGetUnit wZGetUnit = new WZGetUnit();

                wZGetUnit.UnitCode = CreateNewGetUnitCode(DDL_UnitType.SelectedValue);//TXT_UnitCode.Text.Trim(); ����
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
            DDL_UnitType.SelectedValue = "������λ";
            TXT_FeeManage.Text = "";
            HF_FeeManage.Value = "";
            TXT_MaterialPerson.Text = "";
            HF_MaterialPerson.Value = "";

            TXT_UnitName.BackColor = Color.White;
            TXT_Leader.BackColor = Color.White;
            DDL_UnitType.BackColor = Color.White;
            TXT_FeeManage.BackColor = Color.White;
            TXT_MaterialPerson.BackColor = Color.White;

            //���¼���
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ɹ���');ControlStatusCloseChange();", true);
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
        DDL_UnitType.SelectedValue = "������λ";
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
        //�������ϵ�λ
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
        DDL_UnitType.SelectedValue = "������λ";
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
        //�༭
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
                DDL_UnitType.SelectedValue = "������λ";
            }
            else
            {
                DDL_UnitType.SelectedValue = "��Ŀ��";
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
        //ɾ��
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
            DDL_UnitType.SelectedValue = "������λ";
            TXT_FeeManage.Text = "";
            HF_FeeManage.Value = "";
            TXT_MaterialPerson.Text = "";
            HF_MaterialPerson.Value = "";

            TXT_UnitName.BackColor = Color.White;
            TXT_Leader.BackColor = Color.White;
            DDL_UnitType.BackColor = Color.White;
            TXT_FeeManage.BackColor = Color.White;
            TXT_MaterialPerson.BackColor = Color.White;

            //���¼����б�
            DataBinder();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"');ControlStatusCloseChange();", true);
        }
        else {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewIsMark + "')", true);
        }
    }
    


    private string CreateNewGetUnitCode(string strUnitType)
    {
        //�������ϵ���  �ܹ�����Ϊ4λ   0101-0199 0201-9999 
        string strNewUnitCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                int intGetUnitCodeNumber = 0;
                do
                {
                    if (strUnitType == "������λ")
                    {
                        //��������λ��Ϊ���ϵ�λ�Ĳ��֣��ڴ�������ʱֱ�ӵ��룬���ڱ�������ʹ���Ա����༭����ŷ�Χ������0101-0199֮��
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
                    else if (strUnitType == "��Ŀ��")
                    {
                        //����Ŀ����Ϊ���ϵ�λ�Ĳ��֣��ɾ�Ӫ��������༭����ŷ�Χ������0201-9999֮��
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

                    //��֤�µ���Ŀ������ʹ���
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