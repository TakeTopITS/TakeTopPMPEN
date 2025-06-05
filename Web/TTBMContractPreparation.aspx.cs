using System;
using System.Resources;
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

public partial class TTBMContractPreparation : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_SignDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EffectiveDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_PartyAName.Text = ShareClass.GetUserName(strUserCode);
            TB_PartyA.Text = GetUserDepartName(strUserCode);

            LoadBMContractDiscussName();

            LoadBMContractPreparationList();

            InitialConstractTreeByAuthority(TreeView4, strUserCode);
        }
    }
    protected void TreeView4_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strConstractCode, strConstractName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView4.SelectedNode;

        strConstractCode = treeNode.Target.Trim();
        strConstractName = GetConstractName(strConstractCode);

        if (strConstractCode == "")
        {
            strConstractName = "";
        }

        LB_RelatedConstractCode.Text = strConstractCode;
        TB_RelatedConstractName.Text = strConstractName;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }

    public string GetConstractName(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Constract as constract Where constract.ConstractCode = " + "'" + strConstractCode + "'";
        ConstractBLL constractBLL = new ConstractBLL();

        try
        {
            lst = constractBLL.GetAllConstracts(strHQL);
            Constract constract = (Constract)lst[0];
            return constract.ConstractName.Trim();
        }
        catch
        {
            return "";
        }
    }

    //定义合同树（根据权限）
    public void InitialConstractTreeByAuthority(TreeView ConstractTreeView, string strUserCode)
    {
        string strHQL;
        IList lst;

        String strConstractCode, strConstractName;

        //添加根节点
        ConstractTreeView.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>" + LanguageHandle.GetWord("WoDeHeTong") + "</B>";
        node1.Target = "";
        node1.Expanded = true;
        ConstractTreeView.Nodes.Add(node1);

        Constract constract = new Constract();

        strHQL = "from Constract as constract where constract.RecorderCode = " + "'" + strUserCode + "'";
        strHQL += " and constract.ParentCode = ''";
        strHQL += " and constract.Status not in ('Archived','Deleted') ";
        strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";

        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);


        for (int i = 0; i < lst.Count; i++)
        {
            constract = (Constract)lst[i];

            strConstractCode = constract.ConstractCode.Trim();
            strConstractName = constract.ConstractName.Trim();

            node3 = new TreeNode();

            node3.Text = strConstractCode + " " + strConstractName;
            node3.Target = strConstractCode;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            ConstractTreeShowByAuthority(strConstractCode, node3);
            ConstractTreeView.DataBind();
        }
    }

    public static void ConstractTreeShowByAuthority(string strParentCode, TreeNode treeNode)
    {
        string strHQL;
        IList lst1, lst2;

        String strConstractCode, strConstractName;

        ConstractBLL constractBLL = new ConstractBLL();
        Constract constract = new Constract();


        strHQL = "from Constract as constract where ";
        strHQL += " constract.ParentCode = " + "'" + strParentCode + "'";
        strHQL += " and constract.Status not in ('Archived','Deleted') ";
        strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";

        lst1 = constractBLL.GetAllConstracts(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            constract = (Constract)lst1[i];

            strConstractCode = constract.ConstractCode.Trim();
            strConstractName = constract.ConstractName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strConstractCode;
            node.Text = strConstractCode + " " + strConstractName;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;


            strHQL = "from Constract as constract where ";
            strHQL += " constract.ParentCode = " + "'" + strConstractCode + "'";
            strHQL += " and constract.Status not in ('Archived','Deleted') ";
            strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";
            lst2 = constractBLL.GetAllConstracts(strHQL);

            if (lst2.Count > 0)
            {
                ConstractTreeShowByAuthority(strConstractCode, node);
            }
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

    protected void LoadBMContractDiscussName()
    {
        string strHQL;
        IList lst;
        //绑定合同洽谈名称Status = "Qualified"
        //    strHQL = "select * From T_BMContractDiscuss Where Status='Qualified' and ID not in (select ContractDiscussID from T_BMContractPreparation) Order By ID Desc";
        strHQL = "From BMContractDiscuss as bMContractDiscuss Where bMContractDiscuss.Status='Qualified' Order By bMContractDiscuss.ID Desc";
        BMContractDiscussBLL bMContractDiscussBLL = new BMContractDiscussBLL();
        lst = bMContractDiscussBLL.GetAllBMContractDiscusss(strHQL);
        DL_ContractDiscussID.DataSource = lst;
        DL_ContractDiscussID.DataBind();
        DL_ContractDiscussID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    /// <summary>
    /// 新增或更新时，合同洽谈ID是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBMContractPreparationContractDiscussID(string strContractDiscussId, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_BMContractPreparation Where ContractDiscussID='" + strContractDiscussId + "' ";
        }
        else
            strHQL = "Select ID From T_BMContractPreparation Where ContractDiscussID='" + strContractDiscussId + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMContractPreparation").Tables[0];
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

    protected void LoadBMContractPreparationList()
    {
        string strHQL;

        strHQL = "Select * From T_BMContractPreparation Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Name like '%" + TextBox1.Text.Trim() + "%' or PartyB like '%" + TextBox1.Text.Trim() + "%' or PartyA like '%" + TextBox1.Text.Trim() + "%' " +
            "or PartyBName like '%" + TextBox1.Text.Trim() + "%' or PartyAName like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and ContractDiscussName like '%" + TextBox2.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-SignDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-SignDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMContractPreparation");

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
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsBMContractPreparationName(TB_Name.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_ContractDiscussID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGTHTBJC") + "')", true);
            DL_ContractDiscussID.Focus();
            return;
        }
        if (IsBMContractPreparationContractDiscussID(DL_ContractDiscussID.SelectedValue.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGTHTYBZHTJC") + "')", true);
            DL_ContractDiscussID.Focus();
            return;
        }

        BMContractPreparationBLL bMContractPreparationBLL = new BMContractPreparationBLL();
        BMContractPreparation bMContractPreparation = new BMContractPreparation();

        bMContractPreparation.EffectiveDate = DateTime.Parse(DLC_EffectiveDate.Text.Trim());
        bMContractPreparation.PartyBName = TB_PartyBName.Text.Trim();
        bMContractPreparation.PartyA = TB_PartyA.Text.Trim();
        bMContractPreparation.PartyB = lbl_SupplierCode.Text.Trim();
        bMContractPreparation.Name = TB_Name.Text.Trim();
        bMContractPreparation.ContractDiscussID = int.Parse(DL_ContractDiscussID.SelectedValue.Trim());
        bMContractPreparation.ContractDiscussName = GetBMContractDiscussName(bMContractPreparation.ContractDiscussID.ToString().Trim());
        bMContractPreparation.SignDate = DateTime.Parse(DLC_SignDate.Text.Trim());
        bMContractPreparation.PartyAName = TB_PartyAName.Text.Trim();

        bMContractPreparation.RelatedConstractCode = LB_RelatedConstractCode.Text.Trim();
        bMContractPreparation.RelatedConstractName = TB_RelatedConstractName.Text.Trim();

        try
        {
            bMContractPreparationBLL.AddBMContractPreparation(bMContractPreparation);
            LB_ID.Text = GetMaxBMContractPreparationID(bMContractPreparation).ToString();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);

            LoadBMContractPreparationList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    /// <summary>
    /// 新增或更新时，合同编制名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBMContractPreparationName(string strName, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_BMContractPreparation Where Name='" + strName + "' ";
        }
        else
            strHQL = "Select ID From T_BMContractPreparation Where Name='" + strName + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMContractPreparation").Tables[0];
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

    /// <summary>
    /// 新增时，获取表T_BMContractPreparation中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxBMContractPreparationID(BMContractPreparation bmbp)
    {
        string strHQL = "Select ID From T_BMContractPreparation where Name='" + bmbp.Name.Trim() + "' and PartyAName='" + bmbp.PartyAName.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMContractPreparation").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected string GetBMContractDiscussName(string strID)
    {
        string strHQL;
        IList lst;
        //绑定供应商名称
        strHQL = "From BMContractDiscuss as bMContractDiscuss Where bMContractDiscuss.ID='" + strID + "' ";
        BMContractDiscussBLL bMContractDiscussBLL = new BMContractDiscussBLL();
        lst = bMContractDiscussBLL.GetAllBMContractDiscusss(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMContractDiscuss bMContractDiscuss = (BMContractDiscuss)lst[0];
            return bMContractDiscuss.Name.Trim();
        }
        else
            return "";
    }

    protected void Update()
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsBMContractPreparationName(TB_Name.Text.Trim(), LB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_ContractDiscussID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGTHTBJC") + "')", true);
            DL_ContractDiscussID.Focus();
            return;
        }
        if (IsBMContractPreparationContractDiscussID(DL_ContractDiscussID.SelectedValue.Trim(), LB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGTHTYBZHTJC") + "')", true);
            DL_ContractDiscussID.Focus();
            return;
        }

        string strHQL = "From BMContractPreparation as bMContractPreparation where bMContractPreparation.ID = '" + LB_ID.Text.Trim() + "'";
        BMContractPreparationBLL bMContractPreparationBLL = new BMContractPreparationBLL();
        IList lst = bMContractPreparationBLL.GetAllBMContractPreparations(strHQL);
        BMContractPreparation bMContractPreparation = (BMContractPreparation)lst[0];

        bMContractPreparation.EffectiveDate = DateTime.Parse(DLC_EffectiveDate.Text.Trim());
        bMContractPreparation.PartyBName = TB_PartyBName.Text.Trim();
        bMContractPreparation.PartyA = TB_PartyA.Text.Trim();
        bMContractPreparation.PartyB = lbl_SupplierCode.Text.Trim();
        bMContractPreparation.Name = TB_Name.Text.Trim();
        bMContractPreparation.ContractDiscussID = int.Parse(DL_ContractDiscussID.SelectedValue.Trim());
        bMContractPreparation.ContractDiscussName = GetBMContractDiscussName(bMContractPreparation.ContractDiscussID.ToString().Trim());
        bMContractPreparation.SignDate = DateTime.Parse(DLC_SignDate.Text.Trim());
        bMContractPreparation.PartyAName = TB_PartyAName.Text.Trim();

        bMContractPreparation.RelatedConstractCode = LB_RelatedConstractCode.Text.Trim();
        bMContractPreparation.RelatedConstractName = TB_RelatedConstractName.Text.Trim();

        try
        {
            bMContractPreparationBLL.UpdateBMContractPreparation(bMContractPreparation, bMContractPreparation.ID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

            LoadBMContractPreparationList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void Delete()
    {
        string strHQL;
        string strCode = LB_ID.Text.Trim();

        strHQL = "Delete From T_BMContractPreparation Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);

            LoadBMContractPreparationList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
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

                strHQL = "From BMContractPreparation as bMContractPreparation where bMContractPreparation.ID = '" + strID + "'";
                BMContractPreparationBLL bMContractPreparationBLL = new BMContractPreparationBLL();
                lst = bMContractPreparationBLL.GetAllBMContractPreparations(strHQL);

                BMContractPreparation bMContractPreparation = (BMContractPreparation)lst[0];

                LB_ID.Text = bMContractPreparation.ID.ToString().Trim();
                DL_ContractDiscussID.SelectedValue = bMContractPreparation.ContractDiscussID.ToString().Trim();
                DLC_SignDate.Text = bMContractPreparation.SignDate.ToString("yyyy-MM-dd");
                TB_PartyA.Text = bMContractPreparation.PartyA.Trim();
                DLC_EffectiveDate.Text = bMContractPreparation.EffectiveDate.ToString("yyyy-MM-dd");
                TB_PartyAName.Text = bMContractPreparation.PartyAName.Trim();
                TB_Name.Text = bMContractPreparation.Name.Trim();
                TB_PartyBName.Text = bMContractPreparation.PartyBName.Trim();
                TB_PartyB.Text = GetBMSupplierInfoName(bMContractPreparation.PartyB.Trim());
                lbl_SupplierCode.Text = bMContractPreparation.PartyB.Trim();

                LB_RelatedConstractCode.Text = bMContractPreparation.RelatedConstractCode.Trim();
                TB_RelatedConstractName.Text = bMContractPreparation.RelatedConstractName.Trim();

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMContractPreparation");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMContractPreparationList();
    }

    protected void DL_ContractDiscussID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        //绑定供应商名称
        strHQL = "From BMContractDiscuss as bMContractDiscuss Where bMContractDiscuss.ID='" + DL_ContractDiscussID.SelectedValue.Trim() + "' ";
        BMContractDiscussBLL bMContractDiscussBLL = new BMContractDiscussBLL();
        lst = bMContractDiscussBLL.GetAllBMContractDiscusss(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMContractDiscuss bMContractDiscuss = (BMContractDiscuss)lst[0];
            lbl_SupplierCode.Text = bMContractDiscuss.SupplierCode.Trim();
            TB_PartyB.Text = GetBMSupplierInfoName(bMContractDiscuss.SupplierCode.Trim());
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }

    protected string GetBMSupplierInfoName(string strCode)
    {
        string strHQL;
        IList lst;
        //绑定供应商名称
        strHQL = "From BMSupplierInfo as bMSupplierInfo Where bMSupplierInfo.Code='" + strCode + "' ";
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
            return bMSupplierInfo.Name.Trim();
        }
        else
            return "";
    }
}