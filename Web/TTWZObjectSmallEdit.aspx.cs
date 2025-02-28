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

public partial class TTWZObjectSmallEdit : System.Web.UI.Page
{
    public string strUserCode
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            LoadMiddleObjectTree();
            BindSmallObject("", "", "");
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
        string strNewCreater = HF_NewCreater.Value;
        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewCreater + "','" + strUserCode + "');", true);
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
                string strCreater = arrOperate[2];

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "','" + strCreater + "','" + strUserCode + "');", true);
                ControlStatusChange(strProgress, strCreater, strUserCode);

                HF_NewXLCode.Value = strEditXLCode;
                HF_NewProgress.Value = strProgress;
                HF_NewCreater.Value = strCreater;
            }
            else if (cmdName == "edit")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strXLHQL = string.Format("from WZMaterialXL as wZMaterialXL where XLCode = '{0}'", cmdArges);
                IList listXL = wZMaterialXLBLL.GetAllWZMaterialXLs(strXLHQL);
                if (listXL != null && listXL.Count > 0)
                {
                    WZMaterialXL wZMaterialXL = (WZMaterialXL)listXL[0];

                    if (wZMaterialXL.CreateProgress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZMaterialXL.Creater != strUserCode)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLRHZBSCJRBYXBJ").ToString().Trim()+"')", true);
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
                    WZMaterialXL wZMaterialXL = (WZMaterialXL)listXL[0];

                    if (wZMaterialXL.CreateProgress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZMaterialXL.Creater != strUserCode)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLRHZBSCJRBYXSC").ToString().Trim()+"')", true);
                        return;
                    }

                    wZMaterialXLBLL.DeleteWZMaterialXL(wZMaterialXL);

                    //重新加载列表
                    string strTreeSelectedNode = TV_BigObject.SelectedValue;
                    string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');

                    string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
                    string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                    BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);
                }
            }
            else if (cmdName == "request")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strCmdHQL = "update T_WZMaterialXL set CreateProgress = 'Application' where XLCode= '" + cmdArges + "'";   //ChineseWord
                ShareClass.RunSqlCommand(strCmdHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCG").ToString().Trim()+"')", true);

                //重新加载列表
                string strTreeSelectedNode = TV_BigObject.SelectedValue;
                string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');

                string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
                string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);
            }
            else if (cmdName == "returnRequest")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strCmdHQL = "update T_WZMaterialXL set CreateProgress = '录入' where XLCode= '" + cmdArges + "'";   //ChineseWord
                ShareClass.RunSqlCommand(strCmdHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTHCG").ToString().Trim()+"')", true);

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
            string strNewCreater = HF_NewCreater.Value;

            if (string.IsNullOrEmpty(HF_XLCode.Value))
            {
                //新增小类代码
                WZMaterialXLBLL wZMaterialXLBLL = new WZMaterialXLBLL();
                WZMaterialXL wZMaterialXL = new WZMaterialXL();
                string strXLCode = TXT_XLCode.Text.Trim();
                //判断小类代码的前两位是不是中类编码
                if (strXLCode.Length != 6)
                {
                    //提示小类代码不能少于2位
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('小类代码应为6位，请修改！');", true);   //ChineseWord
                    return;
                }
                else if (arrTreeSelectedNode[1] != strXLCode.Substring(0, 4))
                {
                    //提示小类代码前两位要与当前的中类代码
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('小类代码的前4位，与中类代码保持一致！');", true);   //ChineseWord
                    return;
                }
                //查询中类代码是否存在
                string strExistXLHQL = string.Format("select * from T_WZMaterialXL where XLCode = '{0}'", strXLCode);
                DataTable dtXL = ShareClass.GetDataSetFromSql(strExistXLHQL, "strExistXLHQL").Tables[0];
                if (dtXL != null && dtXL.Rows.Count > 0)
                {
                    //提示已经存在中类代码
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('小类代码已经存在，不能重复，请修改！');", true);   //ChineseWord
                    return;
                }
                else
                {
                    string strXLName = TXT_XLName.Text.Trim();
                    if (string.IsNullOrEmpty(strXLName))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('小类名称不能为空，请修改！');", true);   //ChineseWord
                        return;
                    }
                    else {
                        //查询小类代码是否存在
                        string strExistXLNameHQL = string.Format("select * from T_WZMaterialXL where XLName = '{0}'", strXLName);
                        DataTable dtXLName = ShareClass.GetDataSetFromSql(strExistXLNameHQL, "strExistXLHQL").Tables[0];
                        if (dtXLName != null && dtXLName.Rows.Count > 0)
                        {
                            //提示已经存在中类名称
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('小类名称已经存在，不能重复，请修改！');", true);   //ChineseWord
                            return;
                        }
                    }

                    wZMaterialXL.DLCode = arrTreeSelectedNode[0];
                    wZMaterialXL.ZLCode = arrTreeSelectedNode[1];
                    wZMaterialXL.XLCode = TXT_XLCode.Text.Trim();
                    wZMaterialXL.XLName = TXT_XLName.Text.Trim();
                    wZMaterialXL.XLDesc = TXT_XLDesc.Text.Trim();
                    wZMaterialXL.IsMark = 0;
                    wZMaterialXL.CreateProgress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                    wZMaterialXL.Creater = strUserCode;
                    wZMaterialXL.CreateTitle = 0;

                    wZMaterialXLBLL.AddWZMaterialXL(wZMaterialXL);

                    //中类使用标记改为-1
                    string strUpdateMaterialZLSQL = "update T_WZMaterialZL set IsMark = -1 where ZLCode ='" + arrTreeSelectedNode[1] + "'";
                    ShareClass.RunSqlCommand(strUpdateMaterialZLSQL);

                    //重新加载小类代码列表
                    BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);

                    HF_XLCode.Value = "";
                    TXT_XLCode.Text = "";
                    TXT_XLName.Text = "";
                    TXT_XLDesc.Text = "";
                    TXT_XLCode.BackColor = Color.White;
                    TXT_XLName.BackColor = Color.White;
                    TXT_XLDesc.BackColor = Color.White;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('新增成功！');", true);   //ChineseWord
                }
            }
            else
            {
                //修改中类代码
                WZMaterialXLBLL wZMaterialXLBLL = new WZMaterialXLBLL();
                string strXLHQL = string.Format("from WZMaterialXL as wZMaterialXL where XLCode = '{0}'", HF_XLCode.Value);
                IList listXL = wZMaterialXLBLL.GetAllWZMaterialXLs(strXLHQL);
                if (listXL != null && listXL.Count > 0)
                {
                    string strXLName = TXT_XLName.Text.Trim();

                    if (string.IsNullOrEmpty(strXLName))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('小类名称不能为空，请修改！');", true);   //ChineseWord
                        return;
                    }
                    else
                    {
                        //查询小类名称是否存在
                        string strExistXLNameHQL = string.Format("select * from T_WZMaterialXL where XLName = '{0}'", strXLName);
                        DataTable dtXLName = ShareClass.GetDataSetFromSql(strExistXLNameHQL, "strExistXLHQL").Tables[0];
                        if (dtXLName != null && dtXLName.Rows.Count > 0)
                        {
                            if (ShareClass.ObjectToString(dtXLName.Rows[0]["XLCode"]) != HF_XLCode.Value)
                            {
                                //提示已经存在中类名称
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('小类名称已经存在，不能重复，请修改！');", true);   //ChineseWord
                                return;
                            }
                        }
                    }

                    WZMaterialXL wZMaterialXL = (WZMaterialXL)listXL[0];
                    wZMaterialXL.XLName = TXT_XLName.Text.Trim();
                    wZMaterialXL.XLDesc = TXT_XLDesc.Text.Trim();
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

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('修改成功！');", true);   //ChineseWord
                }
            }
        }
        else
        {
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择中类代码节点！');", true);   //ChineseWord
            return;
        }
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && arrTreeSelectedNode[3] == "2")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;

            //新增小类代码
            WZMaterialXLBLL wZMaterialXLBLL = new WZMaterialXLBLL();
            WZMaterialXL wZMaterialXL = new WZMaterialXL();
            string strXLCode = TXT_XLCode.Text.Trim();
            //判断小类代码的前两位是不是中类编码
            if (strXLCode.Length < 2)
            {
                //提示小类代码不能少于2位
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('小类代码不能少于2位！');", true);   //ChineseWord
                return;
            }
            else if (arrTreeSelectedNode[1] != strXLCode.Substring(0, 4))
            {
                //提示小类代码前两位要与当前的中类代码
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('小类代码的前4位，与中类代码保持一致！');", true);   //ChineseWord
                return;
            }
            //查询中类代码是否存在
            string strExistXLHQL = string.Format("select * from T_WZMaterialXL where XLCode = '{0}'", strXLCode);
            DataTable dtXL = ShareClass.GetDataSetFromSql(strExistXLHQL, "strExistXLHQL").Tables[0];
            if (dtXL != null && dtXL.Rows.Count > 0)
            {
                //提示已经存在中类代码
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('小类代码已经存在，不能重复，请修改！');", true);   //ChineseWord
                return;
            }
            else
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

                wZMaterialXL.DLCode = arrTreeSelectedNode[0];
                wZMaterialXL.ZLCode = arrTreeSelectedNode[1];
                wZMaterialXL.XLCode = strXLCode;
                wZMaterialXL.XLName = strXLName;
                wZMaterialXL.XLDesc = strXLName;
                wZMaterialXL.IsMark = 0;
                wZMaterialXL.CreateProgress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                wZMaterialXL.Creater = strUserCode;
                wZMaterialXL.CreateTitle = 0;

                wZMaterialXLBLL.AddWZMaterialXL(wZMaterialXL);


                //重新加载小类代码列表
                BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);

                HF_XLCode.Value = "";
                TXT_XLCode.Text = "";
                TXT_XLName.Text = "";
                TXT_XLDesc.Text = "";
                TXT_XLCode.BackColor = Color.White;
                TXT_XLName.BackColor = Color.White;
                TXT_XLDesc.BackColor = Color.White;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('新增成功！');", true);   //ChineseWord
            }
        }
        else
        {
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择中类代码节点！');", true);   //ChineseWord
            return;
        }
    }


    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && arrTreeSelectedNode[3] == "2")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;

            if (string.IsNullOrEmpty(HF_XLCode.Value))
            {
                //提示请先选择要修改的小类代码
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择要修改的小类代码！');", true);   //ChineseWord
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
                    WZMaterialXL wZMaterialXL = (WZMaterialXL)listXL[0];
                    wZMaterialXL.XLName = TXT_XLName.Text.Trim();
                    wZMaterialXL.XLDesc = TXT_XLDesc.Text.Trim();
                    wZMaterialXLBLL.UpdateWZMaterialXL(wZMaterialXL, HF_XLCode.Value);

                    //重新加载小类代码列表
                    BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);

                    HF_XLCode.Value = "";
                    TXT_XLCode.Text = "";
                    TXT_XLName.Text = "";
                    TXT_XLDesc.Text = "";
                    TXT_XLCode.BackColor = Color.White;
                    TXT_XLName.BackColor = Color.White;
                    TXT_XLDesc.BackColor = Color.White;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('修改成功！');", true);   //ChineseWord
                }
            }
        }
        else
        {
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;
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

        string strXLSQL = string.Format(@"select x.*,m.UserName as CreaterName from T_WZMaterialXL x
                                left join T_ProjectMember m on x.Creater = m.UserCode 
                                where x.DLCode = '{0}' 
                                and x.ZLCode = '{1}'", strDLCode, strZLCode);
        
        DataTable dtXL = ShareClass.GetDataSetFromSql(strXLSQL, "XL").Tables[0];

        DG_List.DataSource = dtXL;
        DG_List.DataBind();

        LB_Sql.Text = strXLSQL;


        LB_ShowZLName.Text = strZLCode;// strZLName;
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



    protected void BT_NewAdd_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && arrTreeSelectedNode[3] == "2")
        {
            //新增
            for (int i = 0; i < DG_List.Items.Count; i++)
            {
                DG_List.Items[i].ForeColor = Color.Black;
            }

            HF_XLCode.Value = "";
            TXT_XLCode.Text = "";
            TXT_XLName.Text = "";
            TXT_XLDesc.Text = "";

            TXT_XLCode.BackColor = Color.CornflowerBlue;
            TXT_XLName.BackColor = Color.CornflowerBlue;
            TXT_XLDesc.BackColor = Color.CornflowerBlue;

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
        else
        {
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择中类代码节点！');", true);   //ChineseWord
            return;
        }
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
        string strNewCreater = HF_NewCreater.Value;

        WZMaterialXLBLL wZMaterialXLBLL = new WZMaterialXLBLL();
        string strXLHQL = string.Format("from WZMaterialXL as wZMaterialXL where XLCode = '{0}'", strEditXLCode);
        IList listXL = wZMaterialXLBLL.GetAllWZMaterialXLs(strXLHQL);
        if (listXL != null && listXL.Count > 0)
        {
            WZMaterialXL wZMaterialXL = (WZMaterialXL)listXL[0];

            if (wZMaterialXL.CreateProgress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZMaterialXL.Creater != strUserCode)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLRHZBSCJRBYXBJ").ToString().Trim()+"');", true);
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

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewCreater + "','" + strUserCode + "');", true);
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

        WZMaterialXLBLL wZMaterialXLBLL = new WZMaterialXLBLL();
        string strXLHQL = string.Format("from WZMaterialXL as wZMaterialXL where XLCode = '{0}'", strEditXLCode);
        IList listXL = wZMaterialXLBLL.GetAllWZMaterialXLs(strXLHQL);
        if (listXL != null && listXL.Count > 0)
        {
            WZMaterialXL wZMaterialXL = (WZMaterialXL)listXL[0];

            if (wZMaterialXL.CreateProgress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZMaterialXL.Creater != strUserCode)
            {
                string strNewProgress = HF_NewProgress.Value;
                string strNewCreater = HF_NewCreater.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLRHZBSCJRBYXSC").ToString().Trim()+"');", true);
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
    }



    protected void BT_NewApply_Click(object sender, EventArgs e)
    {
        //申请
        string strEditXLCode = HF_NewXLCode.Value;
        if (string.IsNullOrEmpty(strEditXLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXLLB").ToString().Trim()+"')", true);
            return;
        }

        string strCmdHQL = "update T_WZMaterialXL set CreateProgress = 'Application' where XLCode= '" + strEditXLCode + "'";   //ChineseWord
        ShareClass.RunSqlCommand(strCmdHQL);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCG").ToString().Trim()+"');", true);

        //重新加载列表
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');

        string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
        string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

        BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);
    }


    protected void BT_NewApplyReturn_Click(object sender, EventArgs e)
    {
        //申请退回
        string strEditXLCode = HF_NewXLCode.Value;
        if (string.IsNullOrEmpty(strEditXLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXLLB").ToString().Trim()+"')", true);
            return;
        }

        string strCmdHQL = "update T_WZMaterialXL set CreateProgress = '录入' where XLCode= '" + strEditXLCode + "'";   //ChineseWord
        ShareClass.RunSqlCommand(strCmdHQL);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTHCG").ToString().Trim()+"');", true);

        //重新加载列表
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string[] arrTreeSelectedNode = strTreeSelectedNode.Split('|');

        string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
        string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

        BindSmallObject(arrTreeSelectedNode[0], arrTreeSelectedNode[1], arrTreeSelectedText[1]);
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
            string strNewCreater = HF_NewCreater.Value;
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
            string strNewCreater = HF_NewCreater.Value;
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择中类代码节点！');ControlStatusChange('" + strNewProgress + "','" + strNewCreater + "','" + strUserCode + "');", true);
            return;
        }
    }



    protected void BT_Creater_Click(object sender, EventArgs e)
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

            if (!string.IsNullOrEmpty(HF_SortCreater.Value))
            {
                strXLSQL += " order by x.ZLCode desc,x.Creater desc,x.CreateProgress desc";

                HF_SortCreater.Value = "";
            }
            else
            {
                strXLSQL += " order by x.ZLCode asc,x.Creater asc,x.CreateProgress asc";

                HF_SortCreater.Value = "Creater";
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
            string strNewCreater = HF_NewCreater.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择中类代码节点！');", true);   //ChineseWord
            return;
        }
    }



    private void ControlStatusChange(string objProgress,string  objCreater,string  objUserCode) {

            if (objProgress == LanguageHandle.GetWord("LuRu").ToString().Trim() && objCreater == objUserCode) {
                BT_NewEdit.Enabled = true;
                BT_NewDelete.Enabled = true;
                BT_NewApply.Enabled = true;
                BT_NewApplyReturn.Enabled = false;
            }
            else if (objProgress == LanguageHandle.GetWord("ShenQing").ToString().Trim() && objCreater == objUserCode) {
                BT_NewEdit.Enabled = false;
                BT_NewDelete.Enabled = false;
                BT_NewApply.Enabled = false;
                BT_NewApplyReturn.Enabled = true;
                
            }
            else {
                BT_NewEdit.Enabled = false;
                BT_NewDelete.Enabled = false;
                BT_NewApply.Enabled = false;
                BT_NewApplyReturn.Enabled = false;
            }
        }



        private void ControlStatusCloseChange() {
            BT_NewEdit.Enabled = false;
                BT_NewDelete.Enabled = false;
                BT_NewApply.Enabled = false;
                BT_NewApplyReturn.Enabled = false;
        }




}
