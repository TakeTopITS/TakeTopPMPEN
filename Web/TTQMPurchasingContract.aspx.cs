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

public partial class TTQMPurchasingContract : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","采购合同", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            TB_CreatePer.Text = ShareClass.GetUserName(strUserCode);

            LoadQMPurchasingContractList();

            InitialConstractTreeByAuthority(TreeView4, strUserCode);

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

    protected void LoadQMPurchasingContractList()
    {
        string strHQL;

        strHQL = "Select * From T_QMPurchasingContract Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Name like '%" + TextBox1.Text.Trim() + "%' or Remark like '%" + TextBox1.Text.Trim() + "%' or Code like '%" + TextBox1.Text.Trim() + "%' " +
                "or CompanyName like '%" + TextBox1.Text.Trim() + "%' or CreatePer like '%" + TextBox1.Text.Trim() + "%') ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMPurchasingContract");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_Code.Text = "";
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strCode;

        strCode = LB_Code.Text;

        if (strCode == "")
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
        if (IsQMPurchasingContractName(TB_Name.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        QMPurchasingContractBLL qMPurchasingContractBLL = new QMPurchasingContractBLL();
        QMPurchasingContract qMPurchasingContract = new QMPurchasingContract();

        qMPurchasingContract.CreatePer = TB_CreatePer.Text.Trim();
        qMPurchasingContract.Name = TB_Name.Text.Trim();
        qMPurchasingContract.CompanyName = TB_CompanyName.Text.Trim();
        qMPurchasingContract.Remark = TB_Remark.Text.Trim();
        qMPurchasingContract.Code = GetQMPurchasingContractCode();
        LB_Code.Text = qMPurchasingContract.Code.Trim();
        qMPurchasingContract.CompanyCode = "";
        qMPurchasingContract.CreateDate = DateTime.Now;
        qMPurchasingContract.IsOverAll = "NO";
        qMPurchasingContract.IsTechnicalDisclosure = "NO";
        qMPurchasingContract.QualityInsStatus = "NO";
        qMPurchasingContract.QualityPenNotStatus = "NO";
        qMPurchasingContract.QualityRepStatus = "NO";
        qMPurchasingContract.ReceivingUnit = TB_ReceivingUnit.Text.Trim();
        qMPurchasingContract.Status = "New";
        qMPurchasingContract.TransportUnit = TB_TransportUnit.Text.Trim();
        qMPurchasingContract.EnterCode = strUserCode.Trim();

        qMPurchasingContract.RelatedConstractCode = LB_RelatedConstractCode.Text.Trim();
        qMPurchasingContract.RelatedConstractName = TB_RelatedConstractName.Text.Trim();

        try
        {
            qMPurchasingContractBLL.AddQMPurchasingContract(qMPurchasingContract);

            LoadQMPurchasingContractList();

            //BT_Delete.Visible = true;
            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    /// <summary>
    /// 新增时，获取表T_QMPurchasingContract中最大编号 规则QMPCCX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetQMPurchasingContractCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_QMPurchasingContract Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMPurchasingContract").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(5)) + 1;
            flag = "QMPCC" + pa.ToString();
        }
        else
        {
            flag = "QMPCC1";
        }
        return flag;
    }

    /// <summary>
    /// 新增或更新时，采购合同名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMPurchasingContractName(string strName, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select Code From T_QMPurchasingContract Where Name='" + strName + "' ";
        }
        else
            strHQL = "Select Code From T_QMPurchasingContract Where Name='" + strName + "' and Code<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMPurchasingContract").Tables[0];
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

    protected void Update()
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsQMPurchasingContractName(TB_Name.Text.Trim(), LB_Code.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }

        string strHQL = "From QMPurchasingContract as qMPurchasingContract where qMPurchasingContract.Code = '" + LB_Code.Text.Trim() + "'";
        QMPurchasingContractBLL qMPurchasingContractBLL = new QMPurchasingContractBLL();
        IList lst = qMPurchasingContractBLL.GetAllQMPurchasingContracts(strHQL);
        QMPurchasingContract qMPurchasingContract = (QMPurchasingContract)lst[0];

        qMPurchasingContract.CreatePer = TB_CreatePer.Text.Trim();
        qMPurchasingContract.Name = TB_Name.Text.Trim();
        qMPurchasingContract.CompanyName = TB_CompanyName.Text.Trim();
        qMPurchasingContract.Remark = TB_Remark.Text.Trim();
        qMPurchasingContract.ReceivingUnit = TB_ReceivingUnit.Text.Trim();
        qMPurchasingContract.TransportUnit = TB_TransportUnit.Text.Trim();

        qMPurchasingContract.RelatedConstractCode = LB_RelatedConstractCode.Text.Trim();
        qMPurchasingContract.RelatedConstractName = TB_RelatedConstractName.Text.Trim();


        try
        {
            qMPurchasingContractBLL.UpdateQMPurchasingContract(qMPurchasingContract, qMPurchasingContract.Code);

            LoadQMPurchasingContractList();

            //BT_Delete.Visible = true;
            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
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
        string strCode = LB_Code.Text.Trim();
        if (IsQMPurchasingContract(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCCGHTYBDYWFSCJC") + "')", true);
            return;
        }
        strHQL = "Delete From T_QMPurchasingContract Where Code = '" + strCode + "' ";

        

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadQMPurchasingContractList();

            //BT_Delete.Visible = false;
            //BT_Update.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }

    /// <summary>
    /// 删除时，判断采购合同单是否已被调用，已调用返回true；否则返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMPurchasingContract(string strID)
    {
        bool flag = true;
        bool flag1 = true;
        bool flag2 = true;
        bool flag3 = true;
        bool flag4 = true;
        bool flag5 = true;
        bool flag6 = true;
        bool flag7 = true;
        bool flag8 = true;
        string strHQL;
        strHQL = "Select Code From T_QMEngineerReview Where PurchasingContractCode='" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMEngineerReview").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }
        strHQL = "Select Code From T_QMEngineerWarranty Where PurchasingContractCode='" + strID + "' ";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMEngineerWarranty").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag1 = true;
        }
        else
        {
            flag1 = false;
        }
        strHQL = "Select Code From T_QMMatEquInspection Where PurchasingContractCode='" + strID + "' ";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMMatEquInspection").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag2 = true;
        }
        else
        {
            flag2 = false;
        }
        strHQL = "Select Code From T_QMOverAllEvaluation Where PurchasingContractCode='" + strID + "' ";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMOverAllEvaluation").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag3 = true;
        }
        else
        {
            flag3 = false;
        }
        strHQL = "Select Code From T_QMQualityDefectNotice Where PurchasingContractCode='" + strID + "' ";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityDefectNotice").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag4 = true;
        }
        else
        {
            flag4 = false;
        }
        strHQL = "Select Code From T_QMQualityInspection Where PurchasingContractCode='" + strID + "' ";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityInspection").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag5 = true;
        }
        else
        {
            flag5 = false;
        }
        strHQL = "Select Code From T_QMQualityInspectionSheet Where PurchasingContractCode='" + strID + "' ";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityInspectionSheet").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag6 = true;
        }
        else
        {
            flag6 = false;
        }
        strHQL = "Select Code From T_QMQualityTechnicalDisclosure Where PurchasingContractCode='" + strID + "' ";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityTechnicalDisclosure").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag7 = true;
        }
        else
        {
            flag7 = false;
        }
        strHQL = "Select Code From T_QMSatisfactionSurvey Where PurchasingContractCode='" + strID + "' ";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMSatisfactionSurvey").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag8 = true;
        }
        else
        {
            flag8 = false;
        }
        if (flag || flag1 || flag2 || flag3 || flag4 || flag5 || flag6 || flag7 || flag8)
        {
            return true;
        }
        else
            return false;
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strCode, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strCode = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                strHQL = "From QMPurchasingContract as qMPurchasingContract where qMPurchasingContract.Code = '" + strCode + "'";
                QMPurchasingContractBLL qMPurchasingContractBLL = new QMPurchasingContractBLL();
                lst = qMPurchasingContractBLL.GetAllQMPurchasingContracts(strHQL);
                QMPurchasingContract qMPurchasingContract = (QMPurchasingContract)lst[0];

                LB_Code.Text = qMPurchasingContract.Code.Trim();
                TB_CompanyName.Text = qMPurchasingContract.CompanyName.Trim();
                TB_CreatePer.Text = qMPurchasingContract.CreatePer.Trim();
                TB_Name.Text = qMPurchasingContract.Name.Trim();
                TB_Remark.Text = qMPurchasingContract.Remark.Trim();
                TB_ReceivingUnit.Text = qMPurchasingContract.ReceivingUnit.Trim();
                TB_TransportUnit.Text = qMPurchasingContract.TransportUnit.Trim();
                LB_RelatedConstractCode.Text = qMPurchasingContract.RelatedConstractCode.Trim();
                TB_RelatedConstractName.Text = qMPurchasingContract.RelatedConstractName.Trim();


                //if (qMPurchasingContract.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Delete.Visible = true;
                //    BT_Update.Visible = true;
                //    BT_Update.Enabled = true;
                //    BT_Delete.Enabled = true;
                //}
                //else
                //{
                //    BT_Delete.Visible = false;
                //    BT_Update.Visible = false;
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMPurchasingContract");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadQMPurchasingContractList();
    }
}