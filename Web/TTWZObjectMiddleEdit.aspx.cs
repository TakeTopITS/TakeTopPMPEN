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

public partial class TTWZObjectMiddleEdit : System.Web.UI.Page
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
            BindMiddleObject("", "");
        }

        TXT_ZLCode.ReadOnly = false;
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
            TXT_DLCode.Text = strTreeSelectedNode;

            HF_ZLCode.Value = "";

            TXT_ZLCode.Text = "";
            TXT_ZLName.Text = "";
            TXT_ZLDesc.Text = "";

            //TXT_ZLCode.BackColor = Color.CornflowerBlue;
            //TXT_ZLName.BackColor = Color.CornflowerBlue;
            //TXT_ZLDesc.BackColor = Color.CornflowerBlue;
        }
        else
        {
            BindMiddleObject("", "");
            TXT_DLCode.Text = "";

            HF_ZLCode.Value = "";
        }

        //string strNewProgress = HF_NewProgress.Value;
        //string strNewCreater = HF_NewCreater.Value;
        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewCreater + "','"+strUserCode+"');", true);
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

                string strEditZLCode = arrOperate[0].Trim();
                string strProgress = arrOperate[1].Trim();
                string strCreater = arrOperate[2].Trim();

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "','" + strCreater + "','"+strUserCode+"');", true);
                ControlStatusChange(strProgress, strCreater, strUserCode);

                HF_NewZLCode.Value = strEditZLCode;
                HF_NewProgress.Value = strProgress;
                HF_NewCreater.Value = strCreater;
            }
            else if (cmdName == "edit")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strZLHQL = string.Format("from WZMaterialZL as wZMaterialZL where ZLCode = '{0}'", cmdArges);
                IList listZL = wZMaterialZLBLL.GetAllWZMaterialZLs(strZLHQL);
                if (listZL != null && listZL.Count > 0)
                {
                    WZMaterialZL wZMaterialZL = (WZMaterialZL)listZL[0];

                    if (wZMaterialZL.CreateProgress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZMaterialZL.Creater != strUserCode)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLRHZBSCJRBYXBJ").ToString().Trim()+"')", true);
                        return;
                    }

                    TXT_DLCode.Text = wZMaterialZL.DLCode;
                    TXT_ZLCode.Text = wZMaterialZL.ZLCode;
                    HF_ZLCode.Value = wZMaterialZL.ZLCode;

                    TXT_ZLName.Text = wZMaterialZL.ZLName;
                    TXT_ZLDesc.Text = wZMaterialZL.ZLDesc;

                    //TXT_ZLCode.ReadOnly = true;
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

                    if (wZMaterialZL.CreateProgress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZMaterialZL.Creater != strUserCode)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLRHZBSCJRBYXSC").ToString().Trim()+"')", true);
                        return;
                    }

                    wZMaterialZLBLL.DeleteWZMaterialZL(wZMaterialZL);

                    //重新加载列表
                    string strTreeSelectedNode = TV_BigObject.SelectedValue;
                    string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
                    string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                    BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);
                }
            }
            else if (cmdName == "request")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strCmdHQL = "update T_WZMaterialZL set CreateProgress = 'Application' where ZLCode= '" + cmdArges + "'";   //ChineseWord
                ShareClass.RunSqlCommand(strCmdHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCG").ToString().Trim()+"')", true);

                //重新加载列表
                string strTreeSelectedNode = TV_BigObject.SelectedValue;
                string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
                string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);
            }
            else if (cmdName == "returnRequest")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strCmdHQL = "update T_WZMaterialZL set CreateProgress = '录入' where ZLCode= '" + cmdArges + "'";   //ChineseWord
                ShareClass.RunSqlCommand(strCmdHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTHCG").ToString().Trim()+"')", true);

                //重新加载列表
                string strTreeSelectedNode = TV_BigObject.SelectedValue;
                string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
                string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && strTreeSelectedNode != "all")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;

            if (string.IsNullOrEmpty(HF_ZLCode.Value))
            {
                //新增中类代码
                WZMaterialZLBLL wZMaterialZLBLL = new WZMaterialZLBLL();
                WZMaterialZL wZMaterialZL = new WZMaterialZL();
                string strZLCode = TXT_ZLCode.Text.Trim();

                //判断中类代码的前两位是不是大类编码
                if (strZLCode.Length != 4)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类代码应为4位，请修改！');", true);   //ChineseWord
                    return;
                }
                else if (strTreeSelectedNode != strZLCode.Substring(0, 2))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类代码的前两位，与大类代码保持一致！');", true);   //ChineseWord
                    return;
                }
                if (!ShareClass.CheckStringRight(strZLCode))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类代码不能是非法字符！');", true);   //ChineseWord
                    return;
                }

                if (string.IsNullOrEmpty(TXT_ZLName.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类名称不能为空，请补充！');", true);   //ChineseWord
                    return;
                }

                //查询中类代码是否存在
                string strExistZLHQL = string.Format("select * from T_WZMaterialZL where ZLCode = '{0}'", strZLCode);
                DataTable dtZL = ShareClass.GetDataSetFromSql(strExistZLHQL, "strExistZLHQL").Tables[0];
                if (dtZL != null && dtZL.Rows.Count > 0)
                {
                    //提示已经存在中类代码
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类代码已经存在，不能重复，请修改！');", true);   //ChineseWord
                    return;
                }
                else
                {
                    string strZLName = TXT_ZLName.Text.Trim();
                    string strZLDesc = TXT_ZLDesc.Text.Trim();

                    if (!ShareClass.CheckStringRight(strZLName))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类名称不能是非法字符！');", true);   //ChineseWord
                        return;
                    }
                    if (strZLName.Length > 22)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类名称不能超过22个字符串！');", true);   //ChineseWord
                        return;
                    }

                    //查询中类名称是否存在
                    string strExistZLNameHQL = string.Format("select * from T_WZMaterialZL where ZLName = '{0}'", strZLName);
                    DataTable dtZLName = ShareClass.GetDataSetFromSql(strExistZLNameHQL, "strExistZLHQL").Tables[0];
                    if (dtZLName != null && dtZLName.Rows.Count > 0)
                    {
                        //提示已经存在中类代码
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类名称已经存在，不能重复，请修改！');", true);   //ChineseWord
                        return;
                    }
                    else
                    {

                        if (!ShareClass.CheckStringRight(strZLDesc))
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类描述不能是非法字符！');", true);   //ChineseWord
                            return;
                        }

                        wZMaterialZL.DLCode = strTreeSelectedNode;
                        wZMaterialZL.ZLCode = strZLCode;
                        wZMaterialZL.ZLName = strZLName;
                        wZMaterialZL.ZLDesc = strZLDesc;
                        wZMaterialZL.IsMark = 0;
                        wZMaterialZL.CreateProgress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                        wZMaterialZL.Creater = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();
                        wZMaterialZL.CreateTitle = 0;

                        wZMaterialZLBLL.AddWZMaterialZL(wZMaterialZL);

                        //重新加载中类代码列表
                        BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);

                        TXT_ZLCode.Text = "";
                        TXT_ZLName.Text = "";
                        TXT_ZLDesc.Text = "";
                        TXT_ZLCode.BackColor = Color.White;
                        TXT_ZLName.BackColor = Color.White;
                        TXT_ZLDesc.BackColor = Color.White;

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('新建成功！');", true);   //ChineseWord
                    }
                }
            }
            else
            {
                //修改中类代码
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
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类代码不能为空，请补充！');", true);   //ChineseWord
                        return;
                    }
                    if (strZLCode.Length != 4)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类代码只能为4位！');", true);   //ChineseWord
                        return;
                    }
                    if (!ShareClass.CheckStringRight(strZLName))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类名称不能是非法字符！');", true);   //ChineseWord
                        return;
                    }
                    if (strZLName.Length > 22)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类名称不能超过22个字符串！');", true);   //ChineseWord
                        return;
                    }

                    //查询中类名称是否存在
                    string strExistZLNameHQL = string.Format("select * from T_WZMaterialZL where ZLName = '{0}'", strZLName);
                    DataTable dtZLName = ShareClass.GetDataSetFromSql(strExistZLNameHQL, "strExistZLHQL").Tables[0];
                    if (dtZLName != null && dtZLName.Rows.Count > 0)
                    {
                        if (ShareClass.ObjectToString(dtZLName.Rows[0]["ZLCode"]) != strZLCode)
                        {
                            //提示已经存在中类名称
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类名称已经存在，不能重复，请修改！');", true);   //ChineseWord
                            return;
                        }
                    }

                    if (!ShareClass.CheckStringRight(strZLDesc))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类描述不能是非法字符！');", true);   //ChineseWord
                        return;
                    }

                    WZMaterialZL wZMaterialZL = (WZMaterialZL)listZL[0];
                    wZMaterialZL.ZLCode = strZLCode;
                    wZMaterialZL.ZLName = strZLName;
                    wZMaterialZL.ZLDesc = strZLDesc;
                    wZMaterialZLBLL.UpdateWZMaterialZL(wZMaterialZL, HF_ZLCode.Value);

                    //重新加载中类代码列表
                    BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);

                    HF_ZLCode.Value = "";
                    TXT_ZLCode.Text = "";
                    TXT_ZLName.Text = "";
                    TXT_ZLDesc.Text = "";
                    TXT_ZLCode.BackColor = Color.White;
                    TXT_ZLName.BackColor = Color.White;
                    TXT_ZLDesc.BackColor = Color.White;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('修改成功！');", true);   //ChineseWord

                }
            }
        }
        else
        {
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择大类代码节点！');", true);   //ChineseWord
            return;
        }
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && strTreeSelectedNode != "all")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;

            //新增中类代码
            WZMaterialZLBLL wZMaterialZLBLL = new WZMaterialZLBLL();
            WZMaterialZL wZMaterialZL = new WZMaterialZL();
            string strZLCode = TXT_ZLCode.Text.Trim();
            //判断中类代码的前两位是不是大类编码
            if (strZLCode.Length < 2 || strTreeSelectedNode.Length < 2)
            {
                //提示已经存在中类代码
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('大类代码，或者中类代码不能少于2位！');", true);   //ChineseWord
                return;
            }
            else if (strTreeSelectedNode != strZLCode.Substring(0, 2))
            {
                //提示已经存在中类代码
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类代码的前两位，与大类代码保持一致！');", true);   //ChineseWord
                return;
            }
            if (!ShareClass.CheckStringRight(strZLCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类代码不能是非法字符！');", true);   //ChineseWord
                return;
            }

            //查询中类代码是否存在
            string strExistZLHQL = string.Format("select * from T_WZMaterialZL where ZLCode = '{0}'", strZLCode);
            DataTable dtZL = ShareClass.GetDataSetFromSql(strExistZLHQL, "strExistZLHQL").Tables[0];
            if (dtZL != null && dtZL.Rows.Count > 0)
            {
                //提示已经存在中类代码
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类代码已经存在，不能重复，请修改！');", true);   //ChineseWord
                return;
            }
            else
            {
                string strZLName = TXT_ZLName.Text.Trim();
                string strZLDesc = TXT_ZLDesc.Text.Trim();

                if (!ShareClass.CheckStringRight(strZLName))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类名称不能是非法字符！');", true);   //ChineseWord
                    return;
                }
                if (!ShareClass.CheckStringRight(strZLDesc))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类描述不能是非法字符！');", true);   //ChineseWord
                    return;
                }

                wZMaterialZL.DLCode = strTreeSelectedNode;
                wZMaterialZL.ZLCode = strZLCode;
                wZMaterialZL.ZLName = strZLName;
                wZMaterialZL.ZLDesc = strZLDesc;
                wZMaterialZL.IsMark = 0;
                wZMaterialZL.CreateProgress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                wZMaterialZL.Creater = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();
                wZMaterialZL.CreateTitle = 0;

                wZMaterialZLBLL.AddWZMaterialZL(wZMaterialZL);

                //重新加载中类代码列表
                BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);

                TXT_ZLCode.Text = "";
                TXT_ZLName.Text = "";
                TXT_ZLDesc.Text = "";
                TXT_ZLCode.BackColor = Color.White;
                TXT_ZLName.BackColor = Color.White;
                TXT_ZLDesc.BackColor = Color.White;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('新建成功！');", true);   //ChineseWord
            }

        }
        else
        {
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择大类代码节点！');", true);   //ChineseWord
            return;
        }
    }




    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && strTreeSelectedNode != "all")
        {
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;

            if (string.IsNullOrEmpty(HF_ZLCode.Value))
            {
                //提示请先选择要修改的中类代码
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择要修改的中类代码！');", true);   //ChineseWord
                return;
            }
            else
            {
                //修改中类代码
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
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类代码不能为空，请补充！');", true);   //ChineseWord
                        return;
                    }
                    if (!ShareClass.CheckStringRight(strZLName))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类名称不能是非法字符！');", true);   //ChineseWord
                        return;
                    }
                    if (!ShareClass.CheckStringRight(strZLDesc))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('中类描述不能是非法字符！');", true);   //ChineseWord
                        return;
                    }

                    WZMaterialZL wZMaterialZL = (WZMaterialZL)listZL[0];
                    wZMaterialZL.ZLCode = strZLCode;
                    wZMaterialZL.ZLName = strZLName;
                    wZMaterialZL.ZLDesc = strZLDesc;
                    wZMaterialZLBLL.UpdateWZMaterialZL(wZMaterialZL, HF_ZLCode.Value);

                    //重新加载中类代码列表
                    BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);

                    TXT_ZLCode.Text = "";
                    TXT_ZLName.Text = "";
                    TXT_ZLDesc.Text = "";
                    TXT_ZLCode.BackColor = Color.White;
                    TXT_ZLName.BackColor = Color.White;
                    TXT_ZLDesc.BackColor = Color.White;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('修改成功！');", true);   //ChineseWord
                }
            }
        }
        else
        {
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择大类代码节点！');", true);   //ChineseWord
            return;
        }
    }

    protected void BT_Cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        HF_ZLCode.Value = "";
        TXT_ZLCode.Text = "";
        TXT_ZLName.Text = "";
        TXT_ZLDesc.Text = "";

        TXT_ZLCode.BackColor = Color.White;
        TXT_ZLName.BackColor = Color.White;
        TXT_ZLDesc.BackColor = Color.White;

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }



    /// <summary>
    /// 根据大类代码绑定中类列表
    /// </summary>
    private void BindMiddleObject(string strDLCode, string strDLName)
    {
        DG_List.CurrentPageIndex = 0;

        string strZLSQL = string.Format(@"select z.*,m.UserName as CreaterName from T_WZMaterialZL z
                    left join T_ProjectMember m on z.Creater = m.UserCode 
                    where z.DLCode = '{0}'", strDLCode);

        DataTable dtZL = ShareClass.GetDataSetFromSql(strZLSQL, "ZL").Tables[0];

        DG_List.DataSource = dtZL;
        DG_List.DataBind();

        LB_Sql.Text = strZLSQL;

        LB_ShowDLName.Text = strDLCode;//strDLName;
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


    protected void BT_NewAdd_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && strTreeSelectedNode != "all")
        {
            //新增
            for (int i = 0; i < DG_List.Items.Count; i++)
            {
                DG_List.Items[i].ForeColor = Color.Black;
            }

            HF_ZLCode.Value = "";

            TXT_ZLCode.Text = "";
            TXT_ZLName.Text = "";
            TXT_ZLDesc.Text = "";

            TXT_ZLCode.BackColor = Color.CornflowerBlue;
            TXT_ZLName.BackColor = Color.CornflowerBlue;
            TXT_ZLDesc.BackColor = Color.CornflowerBlue;

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            ControlStatusCloseChange();
        }
        else
        {
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择大类代码节点！');", true);   //ChineseWord
            return;
        }
    }


    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strEditZLCode = HF_NewZLCode.Value;
        if (string.IsNullOrEmpty(strEditZLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDZLLB").ToString().Trim()+"')", true);
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewCreater = HF_NewCreater.Value;

        WZMaterialZLBLL wZMaterialZLBLL = new WZMaterialZLBLL();
        string strZLHQL = string.Format("from WZMaterialZL as wZMaterialZL where ZLCode = '{0}'", strEditZLCode);
        IList listZL = wZMaterialZLBLL.GetAllWZMaterialZLs(strZLHQL);
        if (listZL != null && listZL.Count > 0)
        {
            WZMaterialZL wZMaterialZL = (WZMaterialZL)listZL[0];

            if (wZMaterialZL.CreateProgress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZMaterialZL.Creater != strUserCode)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLRHZBSCJRBYXBJ").ToString().Trim()+"');", true);
                return;
            }

            TXT_DLCode.Text = wZMaterialZL.DLCode;
            TXT_ZLCode.Text = wZMaterialZL.ZLCode;
            HF_ZLCode.Value = wZMaterialZL.ZLCode;

            TXT_ZLName.Text = wZMaterialZL.ZLName;
            TXT_ZLDesc.Text = wZMaterialZL.ZLDesc;

            TXT_ZLCode.BackColor = Color.CornflowerBlue;
            TXT_ZLName.BackColor = Color.CornflowerBlue;
            TXT_ZLDesc.BackColor = Color.CornflowerBlue;
        }

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewCreater + "','" + strUserCode + "');", true);
    }


    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //删除
        string strEditZLCode = HF_NewZLCode.Value;
        if (string.IsNullOrEmpty(strEditZLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDZLLB").ToString().Trim()+"')", true);
            return;
        }

        WZMaterialZLBLL wZMaterialZLBLL = new WZMaterialZLBLL();
        string strZLHQL = string.Format("from WZMaterialZL as wZMaterialZL where ZLCode = '{0}'", strEditZLCode);
        IList listZL = wZMaterialZLBLL.GetAllWZMaterialZLs(strZLHQL);
        if (listZL != null && listZL.Count > 0)
        {
            WZMaterialZL wZMaterialZL = (WZMaterialZL)listZL[0];

            if (wZMaterialZL.CreateProgress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZMaterialZL.Creater != strUserCode)
            {
                string strNewProgress = HF_NewProgress.Value;
                string strNewCreater = HF_NewCreater.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLRHZBSCJRBYXSC").ToString().Trim()+"');", true);
                return;
            }

            wZMaterialZLBLL.DeleteWZMaterialZL(wZMaterialZL);

            //重新加载列表
            string strTreeSelectedNode = TV_BigObject.SelectedValue;
            string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }



    protected void BT_NewApply_Click(object sender, EventArgs e)
    {
        //申请
        string strEditZLCode = HF_NewZLCode.Value;
        if (string.IsNullOrEmpty(strEditZLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDZLLB").ToString().Trim()+"')", true);
            return;
        }

        string strCmdHQL = "update T_WZMaterialZL set CreateProgress = 'Application' where ZLCode= '" + strEditZLCode + "'";   //ChineseWord
        ShareClass.RunSqlCommand(strCmdHQL);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCG").ToString().Trim()+"');", true);

        //重新加载列表
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
        string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

        BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);
    }


    protected void BT_NewApplyReturn_Click(object sender, EventArgs e)
    {
        //申请退回
        string strEditZLCode = HF_NewZLCode.Value;
        if (string.IsNullOrEmpty(strEditZLCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDZLLB").ToString().Trim()+"')", true);
            return;
        }

        string strCmdHQL = "update T_WZMaterialZL set CreateProgress = '录入' where ZLCode= '" + strEditZLCode + "'";   //ChineseWord
        ShareClass.RunSqlCommand(strCmdHQL);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTHCG").ToString().Trim()+"');", true);

        //重新加载列表
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        string strTreeSelectedText = TV_BigObject.SelectedNode.Text;
        string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

        BindMiddleObject(strTreeSelectedNode, arrTreeSelectedText[1]);
    }

    protected void BT_ZLCode_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && strTreeSelectedNode != "all")
        {
            DG_List.CurrentPageIndex = 0;

            string strZLSQL = string.Format(@"select z.*,m.UserName as CreaterName from T_WZMaterialZL z
                    left join T_ProjectMember m on z.Creater = m.UserCode 
                    where z.DLCode = '{0}'", strTreeSelectedNode);

            if (!string.IsNullOrEmpty(HF_SortZLCode.Value))
            {
                strZLSQL += " order by z.ZLCode desc";

                HF_SortZLCode.Value = "";
            }
            else
            {
                strZLSQL += " order by z.ZLCode asc";

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
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择大类代码节点！');", true);   //ChineseWord
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
                    where z.DLCode = '{0}'", strTreeSelectedNode);

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
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择大类代码节点！');", true);   //ChineseWord
            return;
        }
    }



    protected void BT_Creater_Click(object sender, EventArgs e)
    {
        string strTreeSelectedNode = TV_BigObject.SelectedValue;
        if (!string.IsNullOrEmpty(strTreeSelectedNode) && strTreeSelectedNode != "all")
        {
            DG_List.CurrentPageIndex = 0;

            string strZLSQL = string.Format(@"select z.*,m.UserName as CreaterName from T_WZMaterialZL z
                    left join T_ProjectMember m on z.Creater = m.UserCode 
                    where z.DLCode = '{0}'", strTreeSelectedNode);

            if (!string.IsNullOrEmpty(HF_SortCreater.Value))
            {
                strZLSQL += " order by z.DLCode desc,z.Creater desc,z.CreateProgress desc";

                HF_SortCreater.Value = "";
            }
            else
            {
                strZLSQL += " order by z.DLCode asc,z.Creater asc,z.CreateProgress asc";

                HF_SortCreater.Value = "Creater";
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
            //提示已经存在中类代码
            string strNewProgress = HF_NewProgress.Value;
            string strNewCreater = HF_NewCreater.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择大类代码节点！');", true);   //ChineseWord
            return;
        }
    }


    private void ControlStatusChange(string objProgress, string objCreater, string objUserCode)
    {

        if (objProgress == LanguageHandle.GetWord("LuRu").ToString().Trim() && objCreater == objUserCode)
        {
            BT_NewEdit.Enabled = true;
            BT_NewDelete.Enabled = true;
            BT_NewApply.Enabled = true;
            BT_NewApplyReturn.Enabled = false;

        }
        else if (objProgress == LanguageHandle.GetWord("ShenQing").ToString().Trim() && objCreater == objUserCode)
        {
            BT_NewEdit.Enabled = false;
            BT_NewDelete.Enabled = false;
            BT_NewApply.Enabled = false;
            BT_NewApplyReturn.Enabled = true;

        }
        else
        {
            BT_NewEdit.Enabled = false;
            BT_NewDelete.Enabled = false;
            BT_NewApply.Enabled = false;
            BT_NewApplyReturn.Enabled = false;

        }
    }



    private void ControlStatusCloseChange()
    {

        BT_NewEdit.Enabled = false;
        BT_NewDelete.Enabled = false;
        BT_NewApply.Enabled = false;
        BT_NewApplyReturn.Enabled = false;
    }


}
