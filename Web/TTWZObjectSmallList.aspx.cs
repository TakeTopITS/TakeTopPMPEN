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

public partial class TTWZObjectSmallList : System.Web.UI.Page
{
    public string strUserCode
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            LoadMiddleObjectTree();

            BindSmallObjectFirst();
        }
    }


    private void LoadMiddleObjectTree()
    {
        TV_BigObject.Nodes.Clear();
        TreeNode Node = new TreeNode();
        Node.Text = "All";
        Node.Value = "all|0|0|0";
        string strDLSQL = "select * from T_WZMaterialDL";
        DataTable dtDL = ShareClass.GetDataSetFromSql(strDLSQL, "DL").Tables[0];
        if (dtDL != null && dtDL.Rows.Count > 0)
        {
            foreach (DataRow drDL in dtDL.Rows)
            {
                TreeNode DLNode = new TreeNode();

                string strDLCode = ShareClass.ObjectToString(drDL["DLCode"]);
                DLNode.Value = strDLCode + "|0|0|1";
                DLNode.Text = strDLCode + " " + ShareClass.ObjectToString(drDL["DLName"]);

                string strZLSQL = string.Format("select * from T_WZMaterialZL where DLCode = '{0}' and CreateTitle = -1", strDLCode);//加多一个条件，创建标志为-1
                DataTable dtZL = ShareClass.GetDataSetFromSql(strZLSQL, "ZL").Tables[0];
                if (dtZL != null && dtZL.Rows.Count > 0)
                {
                    foreach (DataRow drZL in dtZL.Rows)
                    {
                        TreeNode ZLNode = new TreeNode();

                        string strZLCode = ShareClass.ObjectToString(drZL["ZLCode"]);
                        ZLNode.Value = strDLCode + "|" + strZLCode + "|0|2";
                        ZLNode.Text = strZLCode + " " + ShareClass.ObjectToString(drZL["ZLName"]);

                        ZLNode.Collapse();
                        DLNode.ChildNodes.Add(ZLNode);
                    }
                }

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
        string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && arrTreeSelectedNode[3] == "2")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);

            TXT_DLCode.Text = arrTreeSelectedNode[0];
            TXT_ZLCode.Text = arrTreeSelectedNode[1];

            HF_XLCode.Value = "";

            TXT_XLCode.Text = "";
            TXT_XLName.Text = "";
            TXT_XLDesc.Text = "";

            //TXT_XLCode.BackColor = Color.CornflowerBlue;
            //TXT_XLName.BackColor = Color.CornflowerBlue;
            //TXT_XLDesc.BackColor = Color.CornflowerBlue;
        }
        else
        {
            BindSmallObject("", "", "");

            TXT_DLCode.Text = "";
            TXT_ZLCode.Text = "";

            HF_XLCode.Value = "";
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

            WZMaterialXLBLL wZMaterialXLBLL = new WZMaterialXLBLL();
            string cmdName = e.CommandName;
            if (cmdName == "click")
            {
                string cmdArges = e.CommandArgument.ToString();
                string[] arrOperate = cmdArges.Split('|');

                string strEditXLCode = arrOperate[0];
                string strProgress = arrOperate[1];
                string strIsMark = arrOperate[2];

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "','" + strIsMark + "');", true);
                ControlStatusChange(strProgress, strIsMark);

                HF_NewXLCode.Value = strEditXLCode;
                HF_NewProgress.Value = strProgress;
                HF_NewIsMark.Value = strIsMark;
            }
            else if (cmdName == "edit")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strXLHQL = string.Format("from WZMaterialXL as wZMaterialXL where XLCode = '{0}'", cmdArges);
                IList listXL = wZMaterialXLBLL.GetAllWZMaterialXLs(strXLHQL);
                if (listXL != null && listXL.Count > 0)
                {
                    WZMaterialXL wZMaterialXL = (WZMaterialXL)listXL[0];

                    if (wZMaterialXL.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBW0BYXBJ").ToString().Trim()+"')", true);
                        return;
                    }

                    TXT_DLCode.Text = wZMaterialXL.DLCode;
                    TXT_ZLCode.Text = wZMaterialXL.ZLCode;
                    TXT_ZLCode.ReadOnly = true;

                    TXT_XLCode.Text = wZMaterialXL.XLCode;
                    HF_XLCode.Value = wZMaterialXL.XLCode;

                    TXT_XLName.Text = wZMaterialXL.XLName;
                    TXT_XLDesc.Text = wZMaterialXL.XLDesc;

                    TXT_XLCode.BackColor = Color.CornflowerBlue;
                    TXT_XLName.BackColor = Color.CornflowerBlue;
                    TXT_XLDesc.BackColor = Color.CornflowerBlue;
                }
            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strXLHQL = string.Format("from WZMaterialXL as wZMaterialXL where XLCode = '{0}'", cmdArges);
                IList listXL = wZMaterialXLBLL.GetAllWZMaterialXLs(strXLHQL);
                if (listXL != null && listXL.Count > 0)
                {

                }
            }
            else if (cmdName == "approve")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strCmdHQL = "update T_WZMaterialXL set CreateProgress = 'Approved',CreateTitle=-1 where XLCode= '" + cmdArges + "'";
                ShareClass.RunSqlCommand(strCmdHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPZTG").ToString().Trim()+"')", true);

                //重新加载列表
                string strTreeSelectedNode = TV_BigObject.SelectedValue;
                string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');

                string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
                string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);
            }
            else if (cmdName == "notApprove")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strCmdHQL = "update T_WZMaterialXL set CreateProgress = 'Application',CreateTitle=0 where XLCode= '" + cmdArges + "'";   //ChineseWord
                ShareClass.RunSqlCommand(strCmdHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCXPZ").ToString().Trim()+"')", true);

                //重新加载列表
                string strTreeSelectedNode = TV_BigObject.SelectedValue;
                string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');

                string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
                string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);
            }
        }
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && arrTreeSelectedNode[3] == "2")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            string strNewProgress = HF_NewProgress.Value;
            string strNewIsMark = HF_NewIsMark.Value;

            if (string.IsNullOrEmpty(HF_XLCode.Value))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先点击编辑小类代码！');", true);   //ChineseWord
                return;
            }
            else
            {
                //修改小类代码
                WZMaterialXLBLL wZMaterialXLBLL = new WZMaterialXLBLL();
                string strXLHQL = string.Format("from WZMaterialXL as wZMaterialXL where XLCode = '{0}'", HF_XLCode.Value);
                IList listXL = wZMaterialXLBLL.GetAllWZMaterialXLs(strXLHQL);
                if (listXL != null && listXL.Count > 0)
                {
                    string strXLName = TXT_XLName.Text.Trim();
                    string strXLDesc = TXT_XLDesc.Text.Trim();

                    if (!ShareClass.CheckStringRight(strXLName))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('小类名称不能是非法字符！');", true);   //ChineseWord
                        return;
                    }
                    if (!ShareClass.CheckStringRight(strXLDesc))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('小类描述不能是非法字符！');", true);   //ChineseWord
                        return;
                    }

                    WZMaterialXL wZMaterialXL = (WZMaterialXL)listXL[0];
                    wZMaterialXL.XLName = strXLName;
                    wZMaterialXL.XLDesc = strXLDesc;
                    wZMaterialXLBLL.UpdateWZMaterialXL(wZMaterialXL, HF_XLCode.Value);

                    //重新加载中类代码列表
                    BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);

                    HF_XLCode.Value = "";
                    TXT_XLCode.Text = "";
                    TXT_XLName.Text = "";
                    TXT_XLDesc.Text = "";
                    TXT_XLCode.BackColor = Color.White;
                    TXT_XLName.BackColor = Color.White;
                    TXT_XLDesc.BackColor = Color.White;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');", true);   //ChineseWord
                }
            }
        }
        else
        {
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewIsMark = HF_NewIsMark.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择中类代码节点！');", true);   //ChineseWord
            return;
        }
    }



    protected void BT_Cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        HF_XLCode.Value = "";
        TXT_XLCode.Text = "";
        TXT_XLName.Text = "";
        TXT_XLDesc.Text = "";
        TXT_XLCode.BackColor = Color.White;
        TXT_XLName.BackColor = Color.White;
        TXT_XLDesc.BackColor = Color.White;

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }


    /// <summary>
    /// 根据大类代码，中类代码，查询小类代码列表
    /// </summary>
    private void BindSmallObject(string strDLCode, string strZLCode, string strZLName)
    {
        DG_List.CurrentPageIndex = 0;

//        string strXLSQL = string.Format(@"select x.*,m.UserName as CreaterName from T_WZMaterialXL x
//                                left join T_ProjectMember m on x.Creater = m.UserCode 
//                                where x.DLCode = '{0}' 
//                                and x.ZLCode = '{1}' 
//                                and x.CreateProgress in ('Application','Approved')", strDLCode, strZLCode);   //ChineseWord

        string strXLSQL = string.Format(@"select x.*,m.UserName as CreaterName from T_WZMaterialXL x
                                left join T_ProjectMember m on x.Creater = m.UserCode 
                                where x.DLCode = '{0}' 
                                and x.ZLCode = '{1}' 
                                and x.CreateProgress != '录入'", strDLCode, strZLCode);   //ChineseWord

        DataTable dtXL = ShareClass.GetDataSetFromSql(strXLSQL, "XL").Tables[0];

        DG_List.DataSource = dtXL;
        DG_List.DataBind();

        LB_Sql.Text = strXLSQL;

        LB_ShowZLName.Text = strZLCode;// strZLName;
        LB_ShowRecordCount.Text = dtXL.Rows.Count.ToString();

        ControlStatusCloseChange();
    }




    /// <summary>
    /// 根据大类代码，中类代码，查询小类代码列表，首次进来
    /// </summary>
    private void BindSmallObjectFirst()
    {
        DG_List.CurrentPageIndex = 0;

        string strXLSQL = @"select x.*,m.UserName as CreaterName from T_WZMaterialXL x
                                left join T_ProjectMember m on x.Creater = m.UserCode 
                                where x.CreateProgress = 'Application'";   //ChineseWord

        DataTable dtXL = ShareClass.GetDataSetFromSql(strXLSQL, "XL").Tables[0];

        DG_List.DataSource = dtXL;
        DG_List.DataBind();

        LB_Sql.Text = strXLSQL;

        LB_ShowZLName.Text = "";// strZLName;
        LB_ShowRecordCount.Text = dtXL.Rows.Count.ToString();

        ControlStatusCloseChange();
    }





    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim(); ;
        DataTable dtZL = ShareClass.GetDataSetFromSql(strHQL, "XL").Tables[0];

        DG_List.DataSource = dtZL;
        DG_List.DataBind();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }


    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strEditXLCode = HF_NewXLCode.Value;
        if (string.IsNullOrEmpty(strEditXLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXLLB").ToString().Trim()+"')", true);
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewIsMark = HF_NewIsMark.Value;

        WZMaterialXLBLL wZMaterialXLBLL = new WZMaterialXLBLL();
        string strXLHQL = string.Format("from WZMaterialXL as wZMaterialXL where XLCode = '{0}'", strEditXLCode);
        IList listXL = wZMaterialXLBLL.GetAllWZMaterialXLs(strXLHQL);
        if (listXL != null && listXL.Count > 0)
        {
            WZMaterialXL wZMaterialXL = (WZMaterialXL)listXL[0];

            if (wZMaterialXL.IsMark != 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBW0BYXBJ").ToString().Trim()+"');", true);
                return;
            }

            TXT_DLCode.Text = wZMaterialXL.DLCode;
            TXT_ZLCode.Text = wZMaterialXL.ZLCode;
            TXT_ZLCode.ReadOnly = true;

            TXT_XLCode.Text = wZMaterialXL.XLCode;
            HF_XLCode.Value = wZMaterialXL.XLCode;

            TXT_XLName.Text = wZMaterialXL.XLName;
            TXT_XLDesc.Text = wZMaterialXL.XLDesc;

            TXT_XLCode.BackColor = Color.CornflowerBlue;
            TXT_XLName.BackColor = Color.CornflowerBlue;
            TXT_XLDesc.BackColor = Color.CornflowerBlue;
        }

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewIsMark + "');", true);
    }


    protected void BT_NewApprove_Click(object sender, EventArgs e)
    {
        //批准
        string strEditXLCode = HF_NewXLCode.Value;
        if (string.IsNullOrEmpty(strEditXLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXLLB").ToString().Trim()+"')", true);
            return;
        }

        string strCmdHQL = "update T_WZMaterialXL set CreateProgress = 'Approved',CreateTitle=-1 where XLCode= '" + strEditXLCode + "'";
        ShareClass.RunSqlCommand(strCmdHQL);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPZTG").ToString().Trim()+"');", true);

        //重新加载列表
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');

        string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
        string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

        BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);
    }


    protected void BT_NewApproveReturn_Click(object sender, EventArgs e)
    {
        //批准撤消
        string strEditXLCode = HF_NewXLCode.Value;
        if (string.IsNullOrEmpty(strEditXLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXLLB").ToString().Trim()+"')", true);
            return;
        }

        string strCmdHQL = "update T_WZMaterialXL set CreateProgress = 'Application',CreateTitle=0 where XLCode= '" + strEditXLCode + "'";   //ChineseWord
        ShareClass.RunSqlCommand(strCmdHQL);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCXPZ").ToString().Trim()+"');", true);

        //重新加载列表
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');

        string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
        string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

        BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);
    }



    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //删除
        string strEditXLCode = HF_NewXLCode.Value;
        if (string.IsNullOrEmpty(strEditXLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXLLB").ToString().Trim()+"')", true);
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewIsMark = HF_NewIsMark.Value;

        WZMaterialXLBLL wZMaterialXLBLL = new WZMaterialXLBLL();
        string strXLHQL = string.Format("from WZMaterialXL as wZMaterialXL where XLCode = '{0}'", strEditXLCode);
        IList listXL = wZMaterialXLBLL.GetAllWZMaterialXLs(strXLHQL);
        if (listXL != null && listXL.Count > 0)
        {
            WZMaterialXL wZMaterialXL = (WZMaterialXL)listXL[0];

            if (wZMaterialXL.CreateProgress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZMaterialXL.Creater != strUserCode)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('进度不为录入，或者不是创建人，不允许删除！');", true);   //ChineseWord
                return;
            }

            wZMaterialXLBLL.DeleteWZMaterialXL(wZMaterialXL);

            //重新加载列表
            string strTreeSelectedNode = TV_BigObject.SelectedValue;
            string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');

            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewIsMark + "');", true);
    }



    protected void BT_RepartIsMark_Click(object sender, EventArgs e)
    {
        //重做使用标记
        //2. 点击【重做使用标记】按钮，对选定中类范围内〈创建进度〉＝“批准”的记录逐条重做使用标记												
        //本条记录的〈小类代码〉＝ 物资代码〈小类代码〉，写记录：小类代码〈使用标记〉＝“-1”，然后继续做下一条												
        //本条记录的〈小类代码〉≠ 物资代码〈小类代码〉，写记录：小类代码〈使用标记〉＝“0”，然后继续做下一条
        //循环查找，直到选定大类范围内最后一条记录后结束								

        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && arrTreeSelectedNode[3] == "2")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            WZMaterialXLBLL wZMaterialXLBLL = new WZMaterialXLBLL();
            string strWZMaterialXLHQL = string.Format("from WZMaterialXL as wZMaterialXL where CreateProgress = 'Approved' and ZLCode = '{0}'", arrTreeSelectedNode[1]);
            IList listWZMaterialXL = wZMaterialXLBLL.GetAllWZMaterialXLs(strWZMaterialXLHQL);
            if (listWZMaterialXL != null && listWZMaterialXL.Count > 0)
            {
                for (int i = 0; i < listWZMaterialXL.Count; i++)
                {
                    WZMaterialXL wZMaterialXL = (WZMaterialXL)listWZMaterialXL[i];

                    string strObjectHQL = "select * from T_WZObject where XLCode = '" + wZMaterialXL.XLCode + "'";
                    DataTable dtObject = ShareClass.GetDataSetFromSql(strObjectHQL, "Object").Tables[0];
                    if (dtObject != null && dtObject.Rows.Count > 0)
                    {
                        wZMaterialXL.IsMark = -1;
                    }
                    else
                    {
                        wZMaterialXL.IsMark = 0;
                    }

                    wZMaterialXLBLL.UpdateWZMaterialXL(wZMaterialXL, wZMaterialXL.XLCode);
                }

                BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('重做使用标记完成！');", true);   //ChineseWord
            }
            else
            {
                string strNewProgress = HF_NewProgress.Value;
                string strNewIsMark = HF_NewIsMark.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('暂时没有进度在批准和" + arrTreeSelectedNode[1] + "中类代码的小类代码，请稍后再重做使用标记！');", true);   //ChineseWord
                return;
            }
        }
        else
        {
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewIsMark = HF_NewIsMark.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择中类代码节点！');", true);   //ChineseWord
            return;
        }
    }




    protected void BT_XLCode_Click(object sender, EventArgs e)
    {

        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && arrTreeSelectedNode[3] == "2")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            DG_List.CurrentPageIndex = 0;

            string strXLSQL = string.Format(@"select x.*,m.UserName as CreaterName from T_WZMaterialXL x
                                left join T_ProjectMember m on x.Creater = m.UserCode 
                                where x.DLCode = '{0}' 
                                and x.ZLCode = '{1}'", arrTreeSelectedNode[0], arrTreeSelectedNode[1]);

            if (!string.IsNullOrEmpty(HF_SortXLCode.Value))
            {
                strXLSQL += " order by x.XLCode desc";

                HF_SortXLCode.Value = "";
            }
            else
            {
                strXLSQL += " order by x.XLCode asc";

                HF_SortXLCode.Value = "XLCode";
            }

            DataTable dtXL = ShareClass.GetDataSetFromSql(strXLSQL, "XL").Tables[0];

            DG_List.DataSource = dtXL;
            DG_List.DataBind();

            LB_Sql.Text = strXLSQL;

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            ControlStatusCloseChange();
        }
        else
        {
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewIsMark = HF_NewIsMark.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择中类代码节点！');", true);   //ChineseWord
            return;
        }
    }


    protected void BT_IsMark_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && arrTreeSelectedNode[3] == "2")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            DG_List.CurrentPageIndex = 0;

            string strXLSQL = string.Format(@"select x.*,m.UserName as CreaterName from T_WZMaterialXL x
                                left join T_ProjectMember m on x.Creater = m.UserCode 
                                where x.DLCode = '{0}' 
                                and x.ZLCode = '{1}'", arrTreeSelectedNode[0], arrTreeSelectedNode[1]);

            if (!string.IsNullOrEmpty(HF_SortIsMark.Value))
            {
                strXLSQL += " order by x.ZLCode desc,x.IsMark desc,x.CreateTitle desc";

                HF_SortIsMark.Value = "";
            }
            else
            {
                strXLSQL += " order by x.ZLCode asc,x.IsMark asc,x.CreateTitle asc";

                HF_SortIsMark.Value = "IsMark";
            }

            DataTable dtXL = ShareClass.GetDataSetFromSql(strXLSQL, "XL").Tables[0];

            DG_List.DataSource = dtXL;
            DG_List.DataBind();

            LB_Sql.Text = strXLSQL;

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            ControlStatusCloseChange();
        }
        else
        {
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewIsMark = HF_NewIsMark.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择中类代码节点！');", true);   //ChineseWord
            return;
        }
    }



    protected void BT_CreateProgress_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && arrTreeSelectedNode[3] == "2")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            DG_List.CurrentPageIndex = 0;

            string strXLSQL = string.Format(@"select x.*,m.UserName as CreaterName from T_WZMaterialXL x
                                left join T_ProjectMember m on x.Creater = m.UserCode 
                                where x.DLCode = '{0}' 
                                and x.ZLCode = '{1}'", arrTreeSelectedNode[0], arrTreeSelectedNode[1]);

            if (!string.IsNullOrEmpty(HF_SortCreateProgress.Value))
            {
                strXLSQL += " order by x.ZLCode desc,x.CreateProgress desc";

                HF_SortCreateProgress.Value = "";
            }
            else
            {
                strXLSQL += " order by x.ZLCode asc,x.CreateProgress asc";

                HF_SortCreateProgress.Value = "CreateProgress";
            }

            DataTable dtXL = ShareClass.GetDataSetFromSql(strXLSQL, "XL").Tables[0];

            DG_List.DataSource = dtXL;
            DG_List.DataBind();

            LB_Sql.Text = strXLSQL;

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            ControlStatusCloseChange();
        }
        else
        {
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewIsMark = HF_NewIsMark.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择中类代码节点！');", true);   //ChineseWord
            return;
        }
    }






    
        private void ControlStatusChange(string objProgress,string  objIsMark) {

            if (objIsMark == "0") {
                BT_NewEdit.Enabled = true;
                BT_NewDelete.Enabled =true;
            }
            else{
                BT_NewEdit.Enabled = false;
                BT_NewDelete.Enabled = false;
            }


            if (objProgress == LanguageHandle.GetWord("ShenQing").ToString().Trim()) {
                BT_NewApprove.Enabled = true;
            }
            else {
                BT_NewApprove.Enabled = false;

            }

            if (objProgress == "Approved" && objIsMark=="0") {
                BT_NewApproveReturn.Enabled =true;
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
