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

public partial class TTWZObjectMiddleList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            LoadMiddleObjectTree();

            BindMiddleObjectFirst();
        }
    }


    private void LoadMiddleObjectTree()
    {
        TV_BigObject.Nodes.Clear();
        TreeNode Node = new TreeNode();
        Node.Text = LanguageHandle.GetWord("SuoYouDaLei").ToString().Trim();
        Node.Value = "all";
        string strDLSQL = "select * from T_WZMaterialDL order by DLCode";
        DataTable dtDL = ShareClass.GetDataSetFromSql(strDLSQL, "DL").Tables[0];
        if (dtDL != null && dtDL.Rows.Count > 0)
        {
            foreach (DataRow drDL in dtDL.Rows)
            {
                TreeNode DLNode = new TreeNode();

                string strDLCode = drDL["DLCode"].ToString();

                DLNode.Value = strDLCode;
                DLNode.Text = strDLCode + " " + drDL["DLName"].ToString();

                DLNode.Collapse();
                Node.ChildNodes.Add(DLNode);
            }
        }
        //Node.ExpandAll();
        TV_BigObject.Nodes.Add(Node);
    }


    protected void TV_BigObject_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && strTreeSelectedNode != "all")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);

        }
        else
        {
            BindMiddleObject("", "");
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewIsMark = HF_NewIsMark.Value;
        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewIsMark + "');", true);
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            for (int i = 0; i < DG_List.Items.Count; i++)
            {
                DG_List.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            WZMaterialZLBLL wZMaterialZLBLL = new WZMaterialZLBLL();
            string cmdName = e.CommandName;
            if (cmdName == "click")
            {
                string cmdArges = e.CommandArgument.ToString();
                string[] arrOperate = cmdArges.Split('|');

                string strEditZLCode = arrOperate[0];
                string strProgress = arrOperate[1];
                string strIsMark = arrOperate[2];

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "','" + strIsMark + "');", true);
                ControlStatusChange(strProgress, strIsMark);

                HF_NewZLCode.Value = strEditZLCode;
                HF_NewProgress.Value = strProgress;
                HF_NewIsMark.Value = strIsMark;
            }
            else if (cmdName == "edit")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strZLHQL = string.Format("from WZMaterialZL as wZMaterialZL where ZLCode = '{0}'", cmdArges);
                IList listZL = wZMaterialZLBLL.GetAllWZMaterialZLs(strZLHQL);
                if (listZL != null && listZL.Count > 0)
                {
                    WZMaterialZL wZMaterialZL = (WZMaterialZL)listZL[0];

                    if (wZMaterialZL.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBJBW0BYXBJ").ToString().Trim()+"')", true);
                        return;
                    }

                    TXT_DLCode.Text = wZMaterialZL.DLCode;
                    string strZLCode = wZMaterialZL.ZLCode;
                    TXT_ZLCode.Text = strZLCode;
                    HF_ZLCode.Value = strZLCode;

                    TXT_ZLName.Text = wZMaterialZL.ZLName;
                    TXT_ZLDesc.Text = wZMaterialZL.ZLDesc;

                    TXT_ZLCode.BackColor = Color.CornflowerBlue;
                    TXT_ZLName.BackColor = Color.CornflowerBlue;
                    TXT_ZLDesc.BackColor = Color.CornflowerBlue;
                }
            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strZLHQL = string.Format("from WZMaterialZL as wZMaterialZL where ZLCode = '{0}'", cmdArges);
                IList listZL = wZMaterialZLBLL.GetAllWZMaterialZLs(strZLHQL);
                if (listZL != null && listZL.Count > 0)
                {
                    WZMaterialZL wZMaterialZL = (WZMaterialZL)listZL[0];
                    wZMaterialZLBLL.DeleteWZMaterialZL(wZMaterialZL);

                    //���¼����б�
                    string strTreeSelectedNode = TV_BigObject.SelectedValue;
                    string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
                    string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                    BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);
                }
            }
            else if (cmdName == "approve")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strCmdHQL = "update T_WZMaterialZL set CreateProgress = 'Approved',CreateTitle=-1 where ZLCode= '" + cmdArges + "'";
                ShareClass.RunSqlCommand(strCmdHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPZTG").ToString().Trim()+"')", true);

                //���¼����б�
                string strTreeSelectedNode = TV_BigObject.SelectedValue;
                string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
                string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);
            }
            else if (cmdName == "notApprove")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strCmdHQL = "update T_WZMaterialZL set CreateProgress = 'Application',CreateTitle=0 where ZLCode= '" + cmdArges + "'";   //ChineseWord
                ShareClass.RunSqlCommand(strCmdHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCXPZ").ToString().Trim()+"')", true);

                //���¼����б�
                string strTreeSelectedNode = TV_BigObject.SelectedValue;
                string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
                string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);
            }
        }
    }


    /// <summary>
    /// ���ݴ������������б�
    /// </summary>
    private void BindMiddleObject(string strDLCode, string strDLName)
    {
        DG_List.CurrentPageIndex = 0;

        string strZLSQL = string.Format(@"select z.*,m.UserName as CreaterName from T_WZMaterialZL z
                    left join T_ProjectMember m on z.Creater = m.UserCode  
                    where z.DLCode = '{0}' 
                    and z.CreateProgress != '¼��'", strDLCode);   //ChineseWord

        DataTable dtZL = ShareClass.GetDataSetFromSql(strZLSQL, "ZL").Tables[0];

        DG_List.DataSource = dtZL;
        DG_List.DataBind();

        LB_Sql.Text = strZLSQL;

        LB_ShowDLName.Text = strDLCode;// strDLName;
        LB_ShowRecordCount.Text = dtZL.Rows.Count.ToString();

        ControlStatusCloseChange();
    }





    /// <summary>
    /// ���ݴ������������б��״ν���
    /// </summary>
    private void BindMiddleObjectFirst()
    {
        DG_List.CurrentPageIndex = 0;

        string strZLSQL = @"select z.*,m.UserName as CreaterName from T_WZMaterialZL z
                    left join T_ProjectMember m on z.Creater = m.UserCode  
                    where z.CreateProgress = 'Application'";   //ChineseWord

        DataTable dtZL = ShareClass.GetDataSetFromSql(strZLSQL, "ZL").Tables[0];

        DG_List.DataSource = dtZL;
        DG_List.DataBind();

        LB_Sql.Text = strZLSQL;

        LB_ShowDLName.Text = "";// strDLName;
        LB_ShowRecordCount.Text = dtZL.Rows.Count.ToString();

        ControlStatusCloseChange();
    }







    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim(); ;
        DataTable dtZL = ShareClass.GetDataSetFromSql(strHQL, "ZL").Tables[0];

        DG_List.DataSource = dtZL;
        DG_List.DataBind();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && strTreeSelectedNode != "all")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            string strNewProgress = HF_NewProgress.Value;
            string strNewIsMark = HF_NewIsMark.Value;

            if (string.IsNullOrEmpty(HF_ZLCode.Value))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ȵ���༭������룡');", true);   //ChineseWord
                return;
            }
            else
            {
                //�޸��������
                WZMaterialZLBLL wZMaterialZLBLL = new WZMaterialZLBLL();
                string strZLHQL = string.Format("from WZMaterialZL as wZMaterialZL where ZLCode = '{0}'", HF_ZLCode.Value);
                IList listZL = wZMaterialZLBLL.GetAllWZMaterialZLs(strZLHQL);
                if (listZL != null && listZL.Count > 0)
                {
                    string strZLCode = TXT_ZLCode.Text.Trim();
                    string strZLName = TXT_ZLName.Text.Trim();
                    string strZLDesc = TXT_ZLDesc.Text.Trim();

                    if (string.IsNullOrEmpty(strZLCode))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('������벻��Ϊ�գ��벹�䣡');", true);   //ChineseWord
                        return;
                    }
                    if (!ShareClass.CheckStringRight(strZLName))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�������Ʋ����ǷǷ��ַ���');", true);   //ChineseWord
                        return;
                    }
                    if (!ShareClass.CheckStringRight(strZLDesc))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�������������ǷǷ��ַ���');", true);   //ChineseWord
                        return;
                    }

                    WZMaterialZL wZMaterialZL = (WZMaterialZL)listZL[0];
                    wZMaterialZL.ZLCode = strZLCode;
                    wZMaterialZL.ZLName = strZLName;
                    wZMaterialZL.ZLDesc = strZLDesc;
                    wZMaterialZLBLL.UpdateWZMaterialZL(wZMaterialZL, HF_ZLCode.Value);

                    //���¼�����������б�
                    BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);

                    TXT_ZLCode.Text = "";
                    TXT_ZLName.Text = "";
                    TXT_ZLDesc.Text = "";
                    TXT_ZLCode.BackColor = Color.White;
                    TXT_ZLName.BackColor = Color.White;
                    TXT_ZLDesc.BackColor = Color.White;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�޸ĳɹ���');", true);   //ChineseWord
                }
            }
        }
        else
        {
            //��ʾ�Ѿ������������
            string strNewProgress = HF_NewProgress.Value;
            string strNewIsMark = HF_NewIsMark.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ��������ڵ㣡');", true);   //ChineseWord
            return;
        }
    }

    protected void BT_Cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        TXT_DLCode.Text = "";
        TXT_ZLCode.Text = "";
        HF_ZLCode.Value = "";
        TXT_ZLName.Text = "";
        TXT_ZLDesc.Text = "";

        TXT_ZLCode.BackColor = Color.White;
        TXT_ZLName.BackColor = Color.White;
        TXT_ZLDesc.BackColor = Color.White;

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }


    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //�༭
        string strEditZLCode = HF_NewZLCode.Value;
        if (string.IsNullOrEmpty(strEditZLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDZLLB").ToString().Trim()+"')", true);
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewIsMark = HF_NewIsMark.Value;

        WZMaterialZLBLL wZMaterialZLBLL = new WZMaterialZLBLL();
        string strZLHQL = string.Format("from WZMaterialZL as wZMaterialZL where ZLCode = '{0}'", strEditZLCode);
        IList listZL = wZMaterialZLBLL.GetAllWZMaterialZLs(strZLHQL);
        if (listZL != null && listZL.Count > 0)
        {
            WZMaterialZL wZMaterialZL = (WZMaterialZL)listZL[0];

            if (wZMaterialZL.IsMark != 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBJBW0BYXBJ").ToString().Trim()+"');", true);
                return;
            }

            TXT_DLCode.Text = wZMaterialZL.DLCode;
            string strZLCode = wZMaterialZL.ZLCode;
            TXT_ZLCode.Text = strZLCode;
            HF_ZLCode.Value = strZLCode;

            TXT_ZLName.Text = wZMaterialZL.ZLName;
            TXT_ZLDesc.Text = wZMaterialZL.ZLDesc;

            TXT_ZLCode.BackColor = Color.CornflowerBlue;
            TXT_ZLName.BackColor = Color.CornflowerBlue;
            TXT_ZLDesc.BackColor = Color.CornflowerBlue;
        }

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewIsMark + "');", true);
    }


    protected void BT_NewApprove_Click(object sender, EventArgs e)
    {
        //��׼
        string strEditZLCode = HF_NewZLCode.Value;
        if (string.IsNullOrEmpty(strEditZLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDZLLB").ToString().Trim()+"')", true);
            return;
        }

        string strCmdHQL = "update T_WZMaterialZL set CreateProgress = 'Approved',CreateTitle=-1 where ZLCode= '" + strEditZLCode + "'";
        ShareClass.RunSqlCommand(strCmdHQL);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPZTG").ToString().Trim()+"');", true);

        //���¼����б�
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
        string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

        BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);
    }


    protected void BT_NewApproveReturn_Click(object sender, EventArgs e)
    {
        //��׼����
        string strEditZLCode = HF_NewZLCode.Value;
        if (string.IsNullOrEmpty(strEditZLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDZLLB").ToString().Trim()+"')", true);
            return;
        }

        string strCmdHQL = "update T_WZMaterialZL set CreateProgress = 'Application',CreateTitle=0 where ZLCode= '" + strEditZLCode + "'";   //ChineseWord
        ShareClass.RunSqlCommand(strCmdHQL);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCXPZ").ToString().Trim()+"');", true);

        //���¼����б�
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
        string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

        BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);
    }



    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //ɾ��
        string strEditZLCode = HF_NewZLCode.Value;
        if (string.IsNullOrEmpty(strEditZLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDZLLB").ToString().Trim()+"')", true);
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewIsMark = HF_NewIsMark.Value;

        WZMaterialZLBLL wZMaterialZLBLL = new WZMaterialZLBLL();
        string strZLHQL = string.Format("from WZMaterialZL as wZMaterialZL where ZLCode = '{0}'", strEditZLCode);
        IList listZL = wZMaterialZLBLL.GetAllWZMaterialZLs(strZLHQL);
        if (listZL != null && listZL.Count > 0)
        {
            WZMaterialZL wZMaterialZL = (WZMaterialZL)listZL[0];

            wZMaterialZLBLL.DeleteWZMaterialZL(wZMaterialZL);

            //���¼����б�
            string strTreeSelectedNode = TV_BigObject.SelectedValue;
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewIsMark + "');", true);
    }



    protected void BT_RepartIsMark_Click(object sender, EventArgs e)
    {
        //����ʹ�ñ��
        //2. ���������ʹ�ñ�ǡ���ť����ѡ�����෶Χ�ڡ��������ȡ�������׼���ļ�¼��������ʹ�ñ��												
        //������¼�ġ�������롵�� С����롴������롵��д��¼��������롴ʹ�ñ�ǡ�����-1����Ȼ���������һ��												
        //������¼�ġ�������롵�� С����롴������롵��д��¼��������롴ʹ�ñ�ǡ�����0����Ȼ���������һ��												
        //ѭ�����ң�ֱ��ѡ�����෶Χ�����һ����¼�����								

        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && strTreeSelectedNode != "all")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            WZMaterialZLBLL wZMaterialZLBLL = new WZMaterialZLBLL();
            string strWZMaterialZLHQL = string.Format("from WZMaterialZL as wZMaterialZL where CreateProgress = 'Approved' and DLCode = '{0}'", arrTreeSelectedText[0]);
            IList listWZMaterialZL = wZMaterialZLBLL.GetAllWZMaterialZLs(strWZMaterialZLHQL);
            if (listWZMaterialZL != null && listWZMaterialZL.Count > 0)
            {
                for (int i = 0; i < listWZMaterialZL.Count; i++)
                {
                    WZMaterialZL wZMaterialZL = (WZMaterialZL)listWZMaterialZL[i];

                    string strMaterialXLHQL = "select * from T_WZMaterialXL where ZLCode = '" + wZMaterialZL.ZLCode + "'";
                    DataTable dtMaterialXL = ShareClass.GetDataSetFromSql(strMaterialXLHQL, "MaterialXL").Tables[0];
                    if (dtMaterialXL != null && dtMaterialXL.Rows.Count > 0)
                    {
                        wZMaterialZL.IsMark = -1;
                    }
                    else
                    {
                        wZMaterialZL.IsMark = 0;
                    }

                    wZMaterialZLBLL.UpdateWZMaterialZL(wZMaterialZL, wZMaterialZL.ZLCode);
                }

                BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ʹ�ñ����ɣ�');ControlStatusCloseChange();", true);
            }
            else
            {
                string strNewProgress = HF_NewProgress.Value;
                string strNewIsMark = HF_NewIsMark.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ʱû�н�������׼��" + arrTreeSelectedText[0] + "��������������룬���Ժ�������ʹ�ñ�ǣ�');", true);   //ChineseWord
                return;
            }
        }
        else
        {
            //��ʾ�Ѿ������������
            string strNewProgress = HF_NewProgress.Value;
            string strNewIsMark = HF_NewIsMark.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ��������ڵ㣡');", true);   //ChineseWord
            return;
        }
    }


    protected void BT_ZLCode_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && strTreeSelectedNode != "all")
        {
            DG_List.CurrentPageIndex = 0;

            string strZLSQL = string.Format(@"select z.*,m.UserName as CreaterName from T_WZMaterialZL z
                    left join T_ProjectMember m on z.Creater = m.UserCode 
                    where z.DLCode = '{0}'
                    and z.CreateProgress != '¼��'", strTreeSelectedNode);   //ChineseWord

            if (!string.IsNullOrEmpty(HF_SortZLCode.Value))
            {
                strZLSQL += " order by z.DLCode desc";

                HF_SortZLCode.Value = "";
            }
            else
            {
                strZLSQL += " order by z.DLCode asc";

                HF_SortZLCode.Value = "ZLCode";
            }

            DataTable dtZL = ShareClass.GetDataSetFromSql(strZLSQL, "ZL").Tables[0];

            DG_List.DataSource = dtZL;
            DG_List.DataBind();

            LB_Sql.Text = strZLSQL;

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            ControlStatusCloseChange();
        }
        else
        {
            //��ʾ�Ѿ������������
            string strNewProgress = HF_NewProgress.Value;
            string strNewIsMark = HF_NewIsMark.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ��������ڵ㣡');", true);   //ChineseWord
            return;
        }
    }


    protected void BT_IsMark_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && strTreeSelectedNode != "all")
        {
            DG_List.CurrentPageIndex = 0;

            string strZLSQL = string.Format(@"select z.*,m.UserName as CreaterName from T_WZMaterialZL z
                    left join T_ProjectMember m on z.Creater = m.UserCode 
                    where z.DLCode = '{0}'
                    and z.CreateProgress != '¼��'", strTreeSelectedNode);   //ChineseWord

            if (!string.IsNullOrEmpty(HF_SortIsMark.Value))
            {
                strZLSQL += " order by z.DLCode desc,z.IsMark desc,z.CreateTitle desc";

                HF_SortIsMark.Value = "";
            }
            else
            {
                strZLSQL += " order by z.DLCode asc,z.IsMark asc,z.CreateTitle asc";

                HF_SortIsMark.Value = "IsMark";
            }

            DataTable dtZL = ShareClass.GetDataSetFromSql(strZLSQL, "ZL").Tables[0];

            DG_List.DataSource = dtZL;
            DG_List.DataBind();

            LB_Sql.Text = strZLSQL;

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            ControlStatusCloseChange();
        }
        else
        {
            //��ʾ�Ѿ������������
            string strNewProgress = HF_NewProgress.Value;
            string strNewIsMark = HF_NewIsMark.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ��������ڵ㣡');", true);   //ChineseWord
            return;
        }
    }

    protected void BT_CreateProgress_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && strTreeSelectedNode != "all")
        {
            DG_List.CurrentPageIndex = 0;

            string strZLSQL = string.Format(@"select z.*,m.UserName as CreaterName from T_WZMaterialZL z
                    left join T_ProjectMember m on z.Creater = m.UserCode 
                    where z.DLCode = '{0}'
                    and z.CreateProgress != '¼��'", strTreeSelectedNode);   //ChineseWord

            if (!string.IsNullOrEmpty(HF_SortCreateProgress.Value))
            {
                strZLSQL += " order by z.DLCode desc,z.CreateProgress desc";

                HF_SortCreateProgress.Value = "";
            }
            else
            {
                strZLSQL += " order by z.DLCode asc,z.CreateProgress asc";

                HF_SortCreateProgress.Value = "CreateProgress";
            }

            DataTable dtZL = ShareClass.GetDataSetFromSql(strZLSQL, "ZL").Tables[0];

            DG_List.DataSource = dtZL;
            DG_List.DataBind();

            LB_Sql.Text = strZLSQL;

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            ControlStatusCloseChange();
        }
        else
        {
            //��ʾ�Ѿ������������
            string strNewProgress = HF_NewProgress.Value;
            string strNewIsMark = HF_NewIsMark.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ��������ڵ㣡');", true);   //ChineseWord
            return;
        }
    }



    
        private void ControlStatusChange(string objProgress,string  objIsMark) {

            
            if (objIsMark == "0") {
                BT_NewEdit.Enabled = true;
                BT_NewDelete.Enabled = true;
        
            } else {
                BT_NewEdit.Enabled = false;
                BT_NewDelete.Enabled = false;
            }

            if (objProgress == LanguageHandle.GetWord("ShenQing").ToString().Trim()) {
                BT_NewApprove.Enabled = true;
            }
            else {
                BT_NewApprove.Enabled = false;
            }

            if (objProgress == "Approved" && objIsMark == "0") {
                BT_NewApproveReturn.Enabled = true;
            }
            else {
                BT_NewApproveReturn.Enabled = false;
            }
        }



        private void ControlStatusCloseChange() {
            BT_NewEdit.Enabled = false;
            BT_NewApprove.Enabled = false;
            BT_NewApproveReturn.Enabled = false;
            BT_NewDelete.Enabled = false;

        }



}
