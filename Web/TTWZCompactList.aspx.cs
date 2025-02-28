using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZCompactList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();
            DataProjectBinder();
            DataSupplierBinder();

            ControlStatusCloseChange();
        }
    }


    private void DataBinder()
    {
        DG_List.CurrentPageIndex = 0;

        string strCompactHQL = string.Format(@"select c.*,s.SupplierName,
                        p.UserName as PurchaseEngineerName,
                        m.UserName as ControlMoneyName,
                        j.UserName as JuridicalPersonName,
                        d.UserName as DelegateAgentName,
                        t.UserName as CompacterName,
                        k.UserName as SafekeepName,
                        h.UserName as CheckerName
                        from T_WZCompact c
                        left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                        left join T_ProjectMember p on c.PurchaseEngineer = p.UserCode
                        left join T_ProjectMember m on c.ControlMoney = m.UserCode
                        left join T_ProjectMember j on c.JuridicalPerson = j.UserCode
                        left join T_ProjectMember d on c.DelegateAgent = d.UserCode
                        left join T_ProjectMember t on c.Compacter = t.UserCode
                        left join T_ProjectMember k on c.Safekeep = k.UserCode
                        left join T_ProjectMember h on c.Checker = h.UserCode
                        where c.PurchaseEngineer = '{0}' ", strUserCode);
        string strProgress = DDL_WhereProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_WhereProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_WhereCompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }
        strCompactHQL += " order by c.MarkTime desc";

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_Sql.Text = strCompactHQL;
        #region 注释
        //WZCompactBLL wZCompactBLL = new WZCompactBLL();
        //string strCompactHQL = "from WZCompact as wZCompact where PurchaseEngineer = '" + strUserCode + "' order by MarkTime desc";
        //IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactHQL);

        //DG_List.DataSource = listCompact;
        //DG_List.DataBind();
        #endregion
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
            if (cmdName == "click")
            {
                //操作
                string cmdArges = e.CommandArgument.ToString();

                WZCompactBLL wZCompactBLL = new WZCompactBLL();
                string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
                if (listCompact != null && listCompact.Count == 1)
                {
                    WZCompact wZCompact = (WZCompact)listCompact[0];

                    ControlStatusChange(wZCompact.PurchaseEngineer, wZCompact.Progress, wZCompact.IsMark.ToString());

                    HF_NewCompactCode.Value = wZCompact.CompactCode;
                    HF_NewProgress.Value = wZCompact.Progress;
                    HF_NewPurchaseEngineer.Value = wZCompact.PurchaseEngineer;
                    HF_NewIsMark.Value = wZCompact.IsMark.ToString();

                    HL_TransferNormalConatract.NavigateUrl = "TTMakeConstractFromOther.aspx?RelatedType=WZConstract&RelatedCode=" + cmdArges + "&RelatedID=1";
                }
            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZCompactBLL wZCompactBLL = new WZCompactBLL();
                string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
                if (listCompact != null && listCompact.Count == 1)
                {
                    WZCompact wZCompact = (WZCompact)listCompact[0];
                    if (wZCompact.Progress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZCompact.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWLRYJSYBJBW0SBYXSC").ToString().Trim() + "')", true);
                        return;
                    }

                    wZCompactBLL.DeleteWZCompact(wZCompact);

                    //重新加载列表
                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }

            }
            else if (cmdName == "edit")
            {
                string cmdArges = e.CommandArgument.ToString();
                string strCompactSql = string.Format(@"select c.*,p.UserName as ControlMoneyName from T_WZCompact c
                            left join T_ProjectMember p on c.ControlMoney = p.UserCode 
                            where c.CompactCode = '{0}'", cmdArges);
                DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactSql, "Compact").Tables[0];
                if (dtCompact != null && dtCompact.Rows.Count == 1)
                {
                    DataRow drCompact = dtCompact.Rows[0];


                    string strCompactCode = ShareClass.ObjectToString(drCompact["CompactCode"]);
                    TXT_CompactCode.Text = strCompactCode;
                    DDL_Project.SelectedValue = ShareClass.ObjectToString(drCompact["ProjectCode"]);
                    DDL_Supplier.SelectedValue = ShareClass.ObjectToString(drCompact["SupplierCode"]);
                    TXT_CompactName.Text = ShareClass.ObjectToString(drCompact["CompactName"]);
                    HF_ControlMoney.Value = ShareClass.ObjectToString(drCompact["ControlMoney"]);
                    TXT_ControlMoney.Text = ShareClass.ObjectToString(drCompact["ControlMoneyName"]);
                    HF_DelegateAgent.Value = ShareClass.ObjectToString(drCompact["DelegateAgent"]);
                    HF_Compacter.Value = ShareClass.ObjectToString(drCompact["Compacter"]);
                    HF_StoreRoom.Value = ShareClass.ObjectToString(drCompact["StoreRoom"]);
                    HF_Checker.Value = ShareClass.ObjectToString(drCompact["Checker"]);
                    HF_Safekeep.Value = ShareClass.ObjectToString(drCompact["Safekeep"]);

                    LT_CompactText.Text = "<a href='" + ShareClass.ObjectToString(drCompact["CompactTextURL"]) + "' class=\"notTab\" target=\"_blank\">" + ShareClass.ObjectToString(drCompact["CompactText"]) + "</a>";

                    HF_CompactCode.Value = strCompactCode;
                }
                #region 注释
                //string cmdArges = e.CommandArgument.ToString();
                //WZCompactBLL wZCompactBLL = new WZCompactBLL();
                //string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
                //IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
                //if (listCompact != null && listCompact.Count == 1)
                //{
                //    WZCompact wZCompact = (WZCompact)listCompact[0];

                //    TXT_CompactCode.Text = wZCompact.CompactCode;
                //    DDL_Project.SelectedValue = wZCompact.ProjectCode;
                //    DDL_Supplier.SelectedValue = wZCompact.SupplierCode;
                //    TXT_CompactName.Text = wZCompact.CompactName;
                //    HF_ControlMoney.Value = wZCompact.ControlMoney;
                //    TXT_ControlMoney.Text = wZCompact.ControlMoney;
                //    HF_DelegateAgent.Value = wZCompact.DelegateAgent;
                //    HF_Compacter.Value = wZCompact.Compacter;
                //    HF_StoreRoom.Value = wZCompact.StoreRoom;
                //    HF_Checker.Value = wZCompact.Checker;
                //    HF_Safekeep.Value = wZCompact.Safekeep;

                //    LT_CompactText.Text = "<a href='" + wZCompact.CompactTextURL + "'>" + wZCompact.CompactText + "</a>";

                //    HF_CompactCode.Value = wZCompact.CompactCode;
                //}
                #endregion
            }
            else if (cmdName == "sign")
            {
                //草签
                string cmdArges = e.CommandArgument.ToString();
                WZCompactBLL wZCompactBLL = new WZCompactBLL();
                string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
                if (listCompact != null && listCompact.Count == 1)
                {
                    WZCompact wZCompact = (WZCompact)listCompact[0];
                    if (wZCompact.Progress == LanguageHandle.GetWord("LuRu").ToString().Trim())
                    {
                        wZCompact.Progress = LanguageHandle.GetWord("CaoQian").ToString().Trim();
                        wZCompact.SingTime = DateTime.Now.ToString("yyyy-MM-dd");

                        wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                        //重新加载列表
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCG").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTJDBSLRZTBNC").ToString().Trim() + "')", true);
                        return;
                    }
                }
            }
            else if (cmdName == "notSign")
            {
                //草签退回
                string cmdArges = e.CommandArgument.ToString();
                WZCompactBLL wZCompactBLL = new WZCompactBLL();
                string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
                if (listCompact != null && listCompact.Count == 1)
                {
                    WZCompact wZCompact = (WZCompact)listCompact[0];
                    if (wZCompact.Progress == LanguageHandle.GetWord("CaoQian").ToString().Trim())
                    {
                        wZCompact.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                        wZCompact.SingTime = "-";

                        wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                        //重新加载列表
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCTHCG").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTJDBSCZTBNCTH").ToString().Trim() + "')", true);
                        return;
                    }
                }
            }
            else if (cmdName == "detail")
            {
                //明细
                string cmdArges = e.CommandArgument.ToString();
                Response.Redirect("TTWZCompactDetail.aspx?CompactCode=" + cmdArges);
            }
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strProjectCode = DDL_Project.SelectedValue;
            string strSupplierCode = DDL_Supplier.SelectedValue;
            string strCompactName = TXT_CompactName.Text.Trim();
            string strControlMoney = HF_ControlMoney.Value;                         //TXT_ControlMoney.Text.Trim();
            string strDelegateAgent = HF_DelegateAgent.Value;
            string strCompacter = HF_Compacter.Value;
            string strStoreRoom = HF_StoreRoom.Value;
            string strChecker = HF_Checker.Value;
            string strSafekeep = HF_Safekeep.Value;
            string strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择工程项目！');", true);   //ChineseWord
                return;
            }
            if (string.IsNullOrEmpty(strSupplierCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择供应商！');", true);   //ChineseWord
                return;
            }
            if (string.IsNullOrEmpty(strCompactName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('合同名称不能为空，请补充！');", true);   //ChineseWord
                return;
            }
            if (string.IsNullOrEmpty(strControlMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('价格监审不能为空，请补充！');", true);   //ChineseWord
                return;
            }

            string strFileURL = HF_CompactTextURL.Value;
            string strFileName = HF_CompactText.Value;
            //if (!string.IsNullOrEmpty(FUP_CompactText.PostedFile.FileName))
            //{
            //    strFileName = FUP_CompactText.PostedFile.FileName.Substring(0, FUP_CompactText.PostedFile.FileName.LastIndexOf("."));
            //    string strFile = FUP_CompactText.PostedFile.FileName.Substring(FUP_CompactText.PostedFile.FileName.LastIndexOf(".")).ToLower();

            //    if (!string.IsNullOrEmpty(strFileName) && !string.IsNullOrEmpty(strFile))
            //    {
            //        string url = string.Format("../Doc/Templates/Temp/{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), strFile);
            //        strFileURL = url;
            //        string destFilePath = Server.MapPath(url);
            //        FUP_CompactText.SaveAs(destFilePath);
            //    }
            //}

            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strCompactCode = HF_CompactCode.Value;
            if (!string.IsNullOrEmpty(strCompactCode))
            {
                //修改
                string strCompactHQL = "from WZCompact as wZCompact where CompactCode = '" + strCompactCode + "'";
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactHQL);
                if (listCompact != null && listCompact.Count > 0)
                {
                    WZCompact wZCompact = (WZCompact)listCompact[0];
                    wZCompact.ProjectCode = strProjectCode;
                    wZCompact.DelegateAgent = strDelegateAgent;
                    wZCompact.Compacter = strCompacter;
                    wZCompact.StoreRoom = strStoreRoom;
                    wZCompact.Checker = strChecker;
                    wZCompact.Safekeep = strSafekeep;
                    wZCompact.SupplierCode = strSupplierCode;
                    wZCompact.CompactName = strCompactName;
                    if (!string.IsNullOrEmpty(strFileName) && !string.IsNullOrEmpty(strFileURL))
                    {
                        wZCompact.CompactText = strFileName;
                        wZCompact.CompactTextURL = strFileURL;
                    }
                    wZCompact.ControlMoney = strControlMoney;

                    wZCompactBLL.UpdateWZCompact(wZCompact, strCompactCode);
                }
            }
            else
            {
                //增加
                WZCompact wZCompact = new WZCompact();
                //合同编号
                string strNewCompactCode = CreateNewCompactCode();

                wZCompact.CompactCode = strNewCompactCode;
                wZCompact.ProjectCode = strProjectCode;

                wZCompact.DelegateAgent = strDelegateAgent;
                wZCompact.Compacter = strCompacter;
                wZCompact.StoreRoom = strStoreRoom;
                wZCompact.Checker = strChecker;
                wZCompact.Safekeep = strSafekeep;

                wZCompact.SupplierCode = strSupplierCode;
                wZCompact.CompactName = strCompactName;
                if (!string.IsNullOrEmpty(strFileName) && !string.IsNullOrEmpty(strFileURL))
                {
                    wZCompact.CompactText = strFileName;
                    wZCompact.CompactTextURL = strFileURL;
                }
                wZCompact.PurchaseEngineer = strUserCode;
                wZCompact.MarkTime = DateTime.Now;
                wZCompact.SingTime = "";
                wZCompact.ControlMoney = strControlMoney;
                wZCompact.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();

                //时间暂时先赋值，不然会报错 TODO
                wZCompact.VerifyTime = "";
                wZCompact.ApproveTime = "";
                wZCompact.EffectTime = "";
                wZCompact.CancelTime = "";
                wZCompact.ReceiveTime = "";


                //根据“采购工程师”找出需方编号，法人代表
                string strNeedHQL = "from WZNeedObject as wZNeedObject where PurchaseEngineer = '" + strUserCode + "'";
                WZNeedObjectBLL wZNeedObjectBLL = new WZNeedObjectBLL();
                IList lstNeedObject = wZNeedObjectBLL.GetAllWZNeedObjects(strNeedHQL);
                if (lstNeedObject != null && lstNeedObject.Count > 0)
                {
                    WZNeedObject wZNeedObject = (WZNeedObject)lstNeedObject[0];

                    wZCompact.NeedCode = wZNeedObject.NeedCode;
                    wZCompact.JuridicalPerson = wZNeedObject.PersonDelegate;

                    //添加合同
                    wZCompactBLL.AddWZCompact(wZCompact);

                    //将需方的使用标记改为-1
                    wZNeedObject.IsMark = -1;
                    wZNeedObjectBLL.UpdateWZNeedObject(wZNeedObject, wZNeedObject.ID);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('未找到采购工程师对应的需方记录！');", true);   //ChineseWord
                    return;
                }
            }

            //重新加载列表
            DataBinder();

            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatus();ControlStatusCloseChange();", true);   //ChineseWord
        }
        catch (Exception ex)
        { }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        try
        {
            string strProjectCode = DDL_Project.SelectedValue;
            string strSupplierCode = DDL_Supplier.SelectedValue;
            string strCompactName = TXT_CompactName.Text.Trim();
            string strControlMoney = HF_ControlMoney.Value;                         //TXT_ControlMoney.Text.Trim();
            string strDelegateAgent = HF_DelegateAgent.Value;
            string strCompacter = HF_Compacter.Value;
            string strStoreRoom = HF_StoreRoom.Value;
            string strChecker = HF_Checker.Value;
            string strSafekeep = HF_Safekeep.Value;
            string strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择工程项目！');", true);   //ChineseWord
                return;
            }
            if (string.IsNullOrEmpty(strSupplierCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择供应商！');", true);   //ChineseWord
                return;
            }
            if (string.IsNullOrEmpty(strCompactName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('合同名称不能为空，请补充！');", true);   //ChineseWord
                return;
            }
            if (string.IsNullOrEmpty(strControlMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('价格监审不能为空，请补充！');", true);   //ChineseWord
                return;
            }

            string strFileURL = HF_CompactTextURL.Value;
            string strFileName = HF_CompactText.Value;


            WZCompactBLL wZCompactBLL = new WZCompactBLL();

            //增加
            WZCompact wZCompact = new WZCompact();
            //合同编号
            string strNewCompactCode = CreateNewCompactCode();

            wZCompact.CompactCode = strNewCompactCode;
            wZCompact.ProjectCode = strProjectCode;

            wZCompact.DelegateAgent = strDelegateAgent;
            wZCompact.Compacter = strCompacter;
            wZCompact.StoreRoom = strStoreRoom;
            wZCompact.Checker = strChecker;
            wZCompact.Safekeep = strSafekeep;

            wZCompact.SupplierCode = strSupplierCode;
            wZCompact.CompactName = strCompactName;
            if (!string.IsNullOrEmpty(strFileName) && !string.IsNullOrEmpty(strFileURL))
            {
                wZCompact.CompactText = strFileName;
                wZCompact.CompactTextURL = strFileURL;
            }
            wZCompact.PurchaseEngineer = strUserCode;
            wZCompact.MarkTime = DateTime.Now;
            wZCompact.SingTime = "";
            wZCompact.ControlMoney = strControlMoney;
            wZCompact.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();

            //时间暂时先赋值，不然会报错 TODO
            wZCompact.VerifyTime = "";
            wZCompact.ApproveTime = "";
            wZCompact.EffectTime = "";
            wZCompact.CancelTime = "";
            wZCompact.ReceiveTime = "";


            //根据“采购工程师”找出需方编号，法人代表
            string strNeedHQL = "from WZNeedObject as wZNeedObject where PurchaseEngineer = '" + strUserCode + "'";
            WZNeedObjectBLL wZNeedObjectBLL = new WZNeedObjectBLL();
            IList lstNeedObject = wZNeedObjectBLL.GetAllWZNeedObjects(strNeedHQL);
            if (lstNeedObject != null && lstNeedObject.Count > 0)
            {
                WZNeedObject wZNeedObject = (WZNeedObject)lstNeedObject[0];

                wZCompact.NeedCode = wZNeedObject.NeedCode;
                wZCompact.JuridicalPerson = wZNeedObject.PersonDelegate;

                //添加合同
                wZCompactBLL.AddWZCompact(wZCompact);

                //将需方的使用标记改为-1
                wZNeedObject.IsMark = -1;
                wZNeedObjectBLL.UpdateWZNeedObject(wZNeedObject, wZNeedObject.ID);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('未找到采购工程师对应的需方记录！');", true);   //ChineseWord
                return;
            }


            //重新加载列表
            DataBinder();

            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');", true);   //ChineseWord
        }
        catch (Exception ex)
        { }
    }


    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        try
        {
            string strProjectCode = DDL_Project.SelectedValue;
            string strSupplierCode = DDL_Supplier.SelectedValue;
            string strCompactName = TXT_CompactName.Text.Trim();
            string strControlMoney = HF_ControlMoney.Value;                         //TXT_ControlMoney.Text.Trim();
            string strDelegateAgent = HF_DelegateAgent.Value;
            string strCompacter = HF_Compacter.Value;
            string strStoreRoom = HF_StoreRoom.Value;
            string strChecker = HF_Checker.Value;
            string strSafekeep = HF_Safekeep.Value;
            string strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择工程项目！');", true);   //ChineseWord
                return;
            }
            if (string.IsNullOrEmpty(strSupplierCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择供应商！');", true);   //ChineseWord
                return;
            }
            if (string.IsNullOrEmpty(strCompactName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('合同名称不能为空，请补充！');", true);   //ChineseWord
                return;
            }
            if (string.IsNullOrEmpty(strControlMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('价格监审不能为空，请补充！');", true);   //ChineseWord
                return;
            }

            string strFileURL = HF_CompactTextURL.Value;
            string strFileName = HF_CompactText.Value;

            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strCompactCode = HF_CompactCode.Value;
            if (!string.IsNullOrEmpty(strCompactCode))
            {
                //修改
                string strCompactHQL = "from WZCompact as wZCompact where CompactCode = '" + strCompactCode + "'";
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactHQL);
                if (listCompact != null && listCompact.Count > 0)
                {
                    WZCompact wZCompact = (WZCompact)listCompact[0];
                    wZCompact.ProjectCode = strProjectCode;
                    wZCompact.DelegateAgent = strDelegateAgent;
                    wZCompact.Compacter = strCompacter;
                    wZCompact.StoreRoom = strStoreRoom;
                    wZCompact.Checker = strChecker;
                    wZCompact.Safekeep = strSafekeep;
                    wZCompact.SupplierCode = strSupplierCode;
                    wZCompact.CompactName = strCompactName;
                    if (!string.IsNullOrEmpty(strFileName) && !string.IsNullOrEmpty(strFileURL))
                    {
                        wZCompact.CompactText = strFileName;
                        wZCompact.CompactTextURL = strFileURL;
                    }
                    wZCompact.ControlMoney = strControlMoney;

                    wZCompactBLL.UpdateWZCompact(wZCompact, strCompactCode);
                }
            }
            else
            {
                //增加 提示先选择合同列表
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择合同列表！');", true);   //ChineseWord
                return;
            }

            //重新加载列表
            DataBinder();

            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');", true);   //ChineseWord
        }
        catch (Exception ex)
        { }
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        TXT_CompactCode.Text = "";
        DDL_Project.SelectedValue = "";
        DDL_Supplier.SelectedValue = "";
        TXT_CompactName.Text = "";
        TXT_ControlMoney.Text = "";
        HF_ControlMoney.Value = "";
        HF_DelegateAgent.Value = "";
        HF_Compacter.Value = "";
        HF_StoreRoom.Value = "";
        HF_Checker.Value = "";
        HF_Safekeep.Value = "";
        LT_CompactText.Text = "";
        HF_CompactCode.Value = "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    private void DataProjectBinder()
    {
        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectHQL = "from WZProject as wZProject order by MarkTime desc";
        IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);

        DDL_Project.DataSource = listProject;
        DDL_Project.DataBind();

        DDL_Project.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void DDL_Project_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strProjectSelectedValue = DDL_Project.SelectedItem.Text;
        if (!string.IsNullOrEmpty(strProjectSelectedValue))
        {
            try
            {
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                string strProjectHQL = "from WZProject as wZProject where ProjectCode = '" + strProjectSelectedValue + "'";
                IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);
                if (listProject != null && listProject.Count > 0)
                {
                    WZProject wZProject = (WZProject)listProject[0];

                    HF_DelegateAgent.Value = wZProject.DelegateAgent;
                    HF_Compacter.Value = wZProject.Contracter;
                    HF_StoreRoom.Value = wZProject.StoreRoom;
                    HF_Safekeep.Value = wZProject.Safekeep;
                    HF_Checker.Value = wZProject.Checker;

                }
            }
            catch (Exception ex) { }
        }
    }


    private void DataSupplierBinder()
    {
        WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
        string strWZSupplierHQL = "from WZSupplier as wZSupplier";
        IList listWZSupplier = wZSupplierBLL.GetAllWZSuppliers(strWZSupplierHQL);

        DDL_Supplier.DataSource = listWZSupplier;
        DDL_Supplier.DataBind();

        DDL_Supplier.Items.Insert(0, new ListItem("--Select--", ""));
    }


    protected void BT_CompactText_Click(object sender, EventArgs e)
    {
        string strCompactCode = HF_CompactCode.Value;

        if (!string.IsNullOrEmpty(strCompactCode))
        {
            try
            {
                string strCompactText = FUP_CompactText.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strCompactText))
                {
                    string strExtendName = System.IO.Path.GetExtension(strCompactText);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strCompactText);
                    string strExtName = Path.GetExtension(strFileName2);

                    string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

                    string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


                    FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

                    if (fi.Exists)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "');</script>");
                        return;
                    }

                    if (Directory.Exists(strDocSavePath) == false)
                    {
                        //如果不存在就创建file文件夹{
                        Directory.CreateDirectory(strDocSavePath);
                    }

                    FUP_CompactText.SaveAs(strDocSavePath + strFileName3);


                    string strUrl = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                    LT_CompactText.Text = "<a href=\"" + strUrl + "\" class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    WZCompactBLL wZCompactBLL = new WZCompactBLL();
                    string strCompactHQL = "from WZCompact as wZCompact where CompactCode = '" + strCompactCode + "'";
                    IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactHQL);
                    if (listCompact != null && listCompact.Count > 0)
                    {
                        WZCompact wZCompact = (WZCompact)listCompact[0];
                        wZCompact.CompactText = Path.GetFileNameWithoutExtension(strFileName2);
                        wZCompact.CompactTextURL = strUrl;

                        wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);
                    }

                    //重新加载报价文件列表
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('上传报价文件成功！');", true);   //ChineseWord
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择要上传的文件！');", true);   //ChineseWord
                    return;
                }
            }
            catch (Exception ex) { }
        }
        else
        {
            try
            {
                string strCompactText = FUP_CompactText.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strCompactText))
                {
                    string strExtendName = System.IO.Path.GetExtension(strCompactText);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strCompactText);
                    string strExtName = Path.GetExtension(strFileName2);

                    string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

                    string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


                    FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

                    if (fi.Exists)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "');</script>");
                    }

                    if (Directory.Exists(strDocSavePath) == false)
                    {
                        //如果不存在就创建file文件夹{
                        Directory.CreateDirectory(strDocSavePath);
                    }

                    FUP_CompactText.SaveAs(strDocSavePath + strFileName3);

                    LT_CompactText.Text = "<a href=\"" + "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3 + "\"  class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_CompactText.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_CompactTextURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    //重新加载报价文件列表
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('上传采购文件成功！');", true);   //ChineseWord
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择要上传的文件！');", true);   //ChineseWord
                    return;
                }
            }
            catch (Exception ex) { }
        }
    }


    /// <summary>
    ///  生成合同Code
    /// </summary>
    private string CreateNewCompactCode()
    {
        string strNewCompactCode = string.Empty;
        try
        {
            lock (this)
            {
                string strYear = DateTime.Now.Year.ToString();
                string strMonth = DateTime.Now.Month.ToString();
                if (strMonth.Length == 1)
                {
                    strMonth = "0" + strMonth;
                }

                bool isExist = true;
                string strCompactCodeHQL = "select count(1) as RowNumber from T_WZCompact where to_char( MarkTime, 'yyyy-mm-dd') like '%" + strYear + "-" + strMonth + "%'";
                DataTable dtCompactCode = ShareClass.GetDataSetFromSql(strCompactCodeHQL, "CompactCode").Tables[0];
                int intCompactCodeNumber = int.Parse(dtCompactCode.Rows[0]["RowNumber"].ToString());
                intCompactCodeNumber = intCompactCodeNumber + 1;
                do
                {
                    StringBuilder sbCompactCode = new StringBuilder();
                    for (int j = 3 - intCompactCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbCompactCode.Append("0");
                    }
                    strNewCompactCode = strYear + strMonth + "-" + sbCompactCode.ToString() + intCompactCodeNumber.ToString();

                    //验证新的合同编号是否存在
                    string strCheckNewCompactCodeHQL = "select count(1) as RowNumber from T_WZCompact where CompactCode = '" + strNewCompactCode + "'";
                    DataTable dtCheckNewCompactCode = ShareClass.GetDataSetFromSql(strCheckNewCompactCodeHQL, "CheckNewCompactCode").Tables[0];
                    int intCheckNewCompactCode = int.Parse(dtCheckNewCompactCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewCompactCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intCompactCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewCompactCode;
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strCompactHQL = LB_Sql.Text;
        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }

    protected void DDL_WhereProgress_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBinder();
    }

    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataBinder();
    }



    protected void BT_NewAdd_Click(object sender, EventArgs e)
    {
        //新建
        string strEditCompactCode = HF_NewCompactCode.Value;

        if (!string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZCompactListEdit.aspx?CompactCode=');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZCompactListEdit.aspx?CompactCode=');", true);
        }
    }


    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDHTLB").ToString().Trim() + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZCompactListEdit.aspx?CompactCode=" + strEditCompactCode + "');", true);

        //        string strCompactSql = string.Format(@"select c.*,p.UserName as ControlMoneyName from T_WZCompact c
        //                            left join T_ProjectMember p on c.ControlMoney = p.UserCode 
        //                            where c.CompactCode = '{0}'", strEditCompactCode);
        //        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactSql, "Compact").Tables[0];
        //        if (dtCompact != null && dtCompact.Rows.Count == 1)
        //        {
        //            DataRow drCompact = dtCompact.Rows[0];


        //            string strCompactCode = ShareClass.ObjectToString(drCompact["CompactCode"]);
        //            TXT_CompactCode.Text = strCompactCode;
        //            DDL_Project.SelectedValue = ShareClass.ObjectToString(drCompact["ProjectCode"]);
        //            DDL_Supplier.SelectedValue = ShareClass.ObjectToString(drCompact["SupplierCode"]);
        //            TXT_CompactName.Text = ShareClass.ObjectToString(drCompact["CompactName"]);
        //            HF_ControlMoney.Value = ShareClass.ObjectToString(drCompact["ControlMoney"]);
        //            TXT_ControlMoney.Text = ShareClass.ObjectToString(drCompact["ControlMoneyName"]);
        //            HF_DelegateAgent.Value = ShareClass.ObjectToString(drCompact["DelegateAgent"]);
        //            HF_Compacter.Value = ShareClass.ObjectToString(drCompact["Compacter"]);
        //            HF_StoreRoom.Value = ShareClass.ObjectToString(drCompact["StoreRoom"]);
        //            HF_Checker.Value = ShareClass.ObjectToString(drCompact["Checker"]);
        //            HF_Safekeep.Value = ShareClass.ObjectToString(drCompact["Safekeep"]);

        //            LT_CompactText.Text = "<a href='" + ShareClass.ObjectToString(drCompact["CompactTextURL"]) + "' class=\"notTab\" target=\"_blank\">" + ShareClass.ObjectToString(drCompact["CompactText"]) + "</a>";

        //            HF_CompactCode.Value = strCompactCode;
        //        }

    }

    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //删除
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDHTLB").ToString().Trim() + "')", true);
            return;
        }

        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + strEditCompactCode + "'";
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
        if (listCompact != null && listCompact.Count == 1)
        {
            WZCompact wZCompact = (WZCompact)listCompact[0];
            if (wZCompact.Progress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZCompact.IsMark != 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWLRYJSYBJBW0SBYXSC").ToString().Trim() + "');", true);
                return;
            }

            wZCompactBLL.DeleteWZCompact(wZCompact);

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "');ControlStatusCloseChange();", true);
        }
    }


    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        //明细
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDHTLB").ToString().Trim() + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZCompactDetail.aspx?CompactCode=" + strEditCompactCode + "');", true);
        return;
        //Response.Redirect("TTWZCompactDetail.aspx?CompactCode=" + strEditCompactCode);
    }



    protected void BT_NewSign_Click(object sender, EventArgs e)
    {
        //草签
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDHTLB").ToString().Trim() + "')", true);
            return;
        }

        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + strEditCompactCode + "'";
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
        if (listCompact != null && listCompact.Count == 1)
        {
            WZCompact wZCompact = (WZCompact)listCompact[0];
            if (wZCompact.Progress == LanguageHandle.GetWord("LuRu").ToString().Trim())
            {
                wZCompact.Progress = LanguageHandle.GetWord("CaoQian").ToString().Trim();
                wZCompact.SingTime = DateTime.Now.ToString("yyyy-MM-dd");

                wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCG").ToString().Trim() + "');ControlStatusCloseChange();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTJDBSLRZTBNC").ToString().Trim() + "');", true);
                return;
            }
        }
    }



    protected void BT_NewSignReturn_Click(object sender, EventArgs e)
    {
        //草签退回
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDHTLB").ToString().Trim() + "')", true);
            return;
        }

        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + strEditCompactCode + "'";
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
        if (listCompact != null && listCompact.Count == 1)
        {
            WZCompact wZCompact = (WZCompact)listCompact[0];
            if (wZCompact.Progress == LanguageHandle.GetWord("CaoQian").ToString().Trim())
            {
                wZCompact.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                wZCompact.SingTime = "-";

                wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCTHCG").ToString().Trim() + "');ControlStatusCloseChange();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTJDBSCZTBNCTH").ToString().Trim() + "');", true);
                return;
            }
        }
    }


    protected void BT_NewCompactPrint_Click(object sender, EventArgs e)
    {
        //合同打印
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDHTLB").ToString().Trim() + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZCompactListPrint.aspx?CompactCode=" + strEditCompactCode + "');", true);
        return;
    }



    protected void BT_NewDetailPrint_Click(object sender, EventArgs e)
    {
        //明细打印
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDHTLB").ToString().Trim() + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZCompactListPrintDetail.aspx?CompactCode=" + strEditCompactCode + "');", true);
        return;
    }








    protected void BT_SortCompactCode_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strCompactHQL = string.Format(@"select c.*,s.SupplierName,
                        p.UserName as PurchaseEngineerName,
                        m.UserName as ControlMoneyName,
                        j.UserName as JuridicalPersonName,
                        d.UserName as DelegateAgentName,
                        t.UserName as CompacterName,
                        k.UserName as SafekeepName,
                        h.UserName as CheckerName
                        from T_WZCompact c
                        left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                        left join T_ProjectMember p on c.PurchaseEngineer = p.UserCode
                        left join T_ProjectMember m on c.ControlMoney = m.UserCode
                        left join T_ProjectMember j on c.JuridicalPerson = j.UserCode
                        left join T_ProjectMember d on c.DelegateAgent = d.UserCode
                        left join T_ProjectMember t on c.Compacter = t.UserCode
                        left join T_ProjectMember k on c.Safekeep = k.UserCode
                        left join T_ProjectMember h on c.Checker = h.UserCode
                        where c.PurchaseEngineer = '{0}' ", strUserCode);
        string strProgress = DDL_WhereProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_WhereProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_WhereCompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortCompactCode.Value))
        {
            strCompactHQL += " order by c.CompactCode desc";

            HF_SortCompactCode.Value = "";
        }
        else
        {
            strCompactHQL += " order by c.CompactCode asc";

            HF_SortCompactCode.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_Sql.Text = strCompactHQL;

        LB_RecordCount.Text = dtCompact.Rows.Count.ToString();

        ControlStatusCloseChange();
    }



    protected void BT_SortProjectCode_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strCompactHQL = string.Format(@"select c.*,s.SupplierName,
                        p.UserName as PurchaseEngineerName,
                        m.UserName as ControlMoneyName,
                        j.UserName as JuridicalPersonName,
                        d.UserName as DelegateAgentName,
                        t.UserName as CompacterName,
                        k.UserName as SafekeepName,
                        h.UserName as CheckerName
                        from T_WZCompact c
                        left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                        left join T_ProjectMember p on c.PurchaseEngineer = p.UserCode
                        left join T_ProjectMember m on c.ControlMoney = m.UserCode
                        left join T_ProjectMember j on c.JuridicalPerson = j.UserCode
                        left join T_ProjectMember d on c.DelegateAgent = d.UserCode
                        left join T_ProjectMember t on c.Compacter = t.UserCode
                        left join T_ProjectMember k on c.Safekeep = k.UserCode
                        left join T_ProjectMember h on c.Checker = h.UserCode
                        where c.PurchaseEngineer = '{0}' ", strUserCode);
        string strProgress = DDL_WhereProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_WhereProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_WhereCompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortProjectCode.Value))
        {
            strCompactHQL += " order by c.ProjectCode desc";

            HF_SortProjectCode.Value = "";
        }
        else
        {
            strCompactHQL += " order by c.ProjectCode asc";

            HF_SortProjectCode.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_Sql.Text = strCompactHQL;

        LB_RecordCount.Text = dtCompact.Rows.Count.ToString();

        ControlStatusCloseChange();
    }





    protected void BT_SortSupplierCode_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strCompactHQL = string.Format(@"select c.*,s.SupplierName,
                        p.UserName as PurchaseEngineerName,
                        m.UserName as ControlMoneyName,
                        j.UserName as JuridicalPersonName,
                        d.UserName as DelegateAgentName,
                        t.UserName as CompacterName,
                        k.UserName as SafekeepName,
                        h.UserName as CheckerName
                        from T_WZCompact c
                        left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                        left join T_ProjectMember p on c.PurchaseEngineer = p.UserCode
                        left join T_ProjectMember m on c.ControlMoney = m.UserCode
                        left join T_ProjectMember j on c.JuridicalPerson = j.UserCode
                        left join T_ProjectMember d on c.DelegateAgent = d.UserCode
                        left join T_ProjectMember t on c.Compacter = t.UserCode
                        left join T_ProjectMember k on c.Safekeep = k.UserCode
                        left join T_ProjectMember h on c.Checker = h.UserCode
                        where c.PurchaseEngineer = '{0}' ", strUserCode);
        string strProgress = DDL_WhereProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_WhereProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_WhereCompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortSupplierCode.Value))
        {
            strCompactHQL += " order by c.SupplierCode desc";

            HF_SortSupplierCode.Value = "";
        }
        else
        {
            strCompactHQL += " order by c.SupplierCode asc";

            HF_SortSupplierCode.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_Sql.Text = strCompactHQL;

        LB_RecordCount.Text = dtCompact.Rows.Count.ToString();

        ControlStatusCloseChange();
    }



    protected void BT_SortMarkTime_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strCompactHQL = string.Format(@"select c.*,s.SupplierName,
                        p.UserName as PurchaseEngineerName,
                        m.UserName as ControlMoneyName,
                        j.UserName as JuridicalPersonName,
                        d.UserName as DelegateAgentName,
                        t.UserName as CompacterName,
                        k.UserName as SafekeepName,
                        h.UserName as CheckerName
                        from T_WZCompact c
                        left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                        left join T_ProjectMember p on c.PurchaseEngineer = p.UserCode
                        left join T_ProjectMember m on c.ControlMoney = m.UserCode
                        left join T_ProjectMember j on c.JuridicalPerson = j.UserCode
                        left join T_ProjectMember d on c.DelegateAgent = d.UserCode
                        left join T_ProjectMember t on c.Compacter = t.UserCode
                        left join T_ProjectMember k on c.Safekeep = k.UserCode
                        left join T_ProjectMember h on c.Checker = h.UserCode
                        where c.PurchaseEngineer = '{0}' ", strUserCode);
        string strProgress = DDL_WhereProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_WhereProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_WhereCompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortMarkTime.Value))
        {
            strCompactHQL += " order by c.MarkTime desc";

            HF_SortMarkTime.Value = "";
        }
        else
        {
            strCompactHQL += " order by c.MarkTime asc";

            HF_SortMarkTime.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_Sql.Text = strCompactHQL;

        LB_RecordCount.Text = dtCompact.Rows.Count.ToString();

        ControlStatusCloseChange();
    }





    /// <summary>
    ///  重新加载列表
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder();

        ControlStatusCloseChange();
    }






    private void ControlStatusChange(string strPurchaseEngineer, string strProgress, string strIsMark)
    {
        if (strPurchaseEngineer.Trim() == strUserCode.Trim() && strProgress == LanguageHandle.GetWord("LuRu").ToString().Trim())
        {
            BT_NewEdit.Enabled = true;
            BT_NewDetail.Visible = true;
            BT_NewSign.Enabled = true;
            BT_NewSignReturn.Enabled = false;
            BT_NewCompactPrint.Enabled = false;
            BT_NewDetailPrint.Enabled = false;
        }
        else if (strPurchaseEngineer.Trim() == strUserCode.Trim() && strProgress == LanguageHandle.GetWord("CaoQian").ToString().Trim())
        {
            BT_NewEdit.Enabled = false;
            BT_NewDetail.Visible = false;
            BT_NewSign.Enabled = false;
            BT_NewSignReturn.Enabled = true;
            BT_NewCompactPrint.Enabled = false;
            BT_NewDetailPrint.Enabled = false;
        }
        else if (strPurchaseEngineer.Trim() == strUserCode.Trim() && strProgress == LanguageHandle.GetWord("ShengXiao").ToString().Trim())
        {
            BT_NewEdit.Enabled = false;
            BT_NewDetail.Visible = false;
            BT_NewSign.Enabled = false;
            BT_NewSignReturn.Enabled = false;
            BT_NewCompactPrint.Enabled = true;
            BT_NewDetailPrint.Enabled = true;
        }
        else
        {
            BT_NewEdit.Enabled = false;
            BT_NewDetail.Visible = false;
            BT_NewSign.Enabled = false;
            BT_NewSignReturn.Enabled = false;
            BT_NewCompactPrint.Enabled = false;
            BT_NewDetailPrint.Enabled = false;
        }

        if (strPurchaseEngineer.Trim() == strUserCode.Trim() && strProgress == LanguageHandle.GetWord("LuRu").ToString().Trim() && strIsMark == "0")
        {
            BT_NewDelete.Enabled = true;
        }
        else
        {
            BT_NewDelete.Enabled = false;
        }
    }



    private void ControlStatusCloseChange()
    {
        BT_NewEdit.Enabled = false;
        BT_NewDetail.Visible = false;

        BT_NewDelete.Enabled = false;

        BT_NewSign.Enabled = false;
        BT_NewSignReturn.Enabled = false;
        BT_NewCompactPrint.Enabled = false;
        BT_NewDetailPrint.Enabled = false;
    }
}
