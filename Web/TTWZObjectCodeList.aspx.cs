using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZObjectCodeList : System.Web.UI.Page
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
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "物资代码", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            LoadTree();
            DataBinder("", "", "", "");
            BindUnitData();
        }
    }

    private void LoadTree()
    {
        TV_Type.Nodes.Clear();
        TreeNode Node = new TreeNode();
        Node.Text = "所有";
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

                string strZLSQL = string.Format("select * from T_WZMaterialZL where DLCode = '{0}' and CreateTitle = -1", strDLCode);
                DataTable dtZL = ShareClass.GetDataSetFromSql(strZLSQL, "ZL").Tables[0];
                if (dtZL != null && dtZL.Rows.Count > 0)
                {
                    foreach (DataRow drZL in dtZL.Rows)
                    {
                        TreeNode ZLNode = new TreeNode();

                        string strZLCode = ShareClass.ObjectToString(drZL["ZLCode"]);
                        ZLNode.Value = strDLCode + "|" + strZLCode + "|0|2";
                        ZLNode.Text = strZLCode + " " + ShareClass.ObjectToString(drZL["ZLName"]);

                        string strXLSQL = string.Format("select * from T_WZMaterialXL where DLCode = '{0}' and ZLCode = '{1}' and CreateTitle = -1", strDLCode, strZLCode);
                        DataTable dtXL = ShareClass.GetDataSetFromSql(strXLSQL, "XL").Tables[0];
                        if (dtXL != null && dtXL.Rows.Count > 0)
                        {
                            foreach (DataRow drXL in dtXL.Rows)
                            {
                                TreeNode XLNode = new TreeNode();

                                string strXLCode = ShareClass.ObjectToString(drXL["XLCode"]);

                                XLNode.Value = strDLCode + "|" + strZLCode + "|" + strXLCode + "|3";
                                XLNode.Text = strXLCode + " " + ShareClass.ObjectToString(drXL["XLName"]);

                                ZLNode.ChildNodes.Add(XLNode);
                            }
                        }
                        ZLNode.Collapse();
                        DLNode.ChildNodes.Add(ZLNode);
                    }
                }

                DLNode.Collapse();
                Node.ChildNodes.Add(DLNode);
            }
        }
        //Node.ExpandAll();
        TV_Type.Nodes.Add(Node);
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

            WZObjectBLL wZObjectBLL = new WZObjectBLL();
            string cmdName = e.CommandName;
            string cmdArges = e.CommandArgument.ToString();
            if (cmdName == "click")
            {
                string[] arrOperate = cmdArges.Split('|');

                string strEditObjectCode = arrOperate[0];
                string strCreater = arrOperate[1];
                string strIsMark = arrOperate[2];

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strCreater + "','" + strUserCode + "','" + strIsMark + "');", true);
                ControlStatusChange(strCreater, strUserCode, strIsMark);

                HF_NewObjectCode.Value = strEditObjectCode;
                HF_NewCreater.Value = strCreater;
                HF_NewIsMark.Value = strIsMark;
            }
            else if (cmdName == "del")
            {
                string strObjectSql = "from WZObject as wZObject where ObjectCode = '" + cmdArges + "'";
                IList objectList = wZObjectBLL.GetAllWZObjects(strObjectSql);
                if (objectList != null && objectList.Count == 1)
                {
                    WZObject wZObject = (WZObject)objectList[0];

                    if (wZObject.Creater != strUserCode || wZObject.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYBJBW0HZBSCJRBYXSC+"')", true);
                        return;
                    }

                    wZObjectBLL.DeleteWZObject(wZObject);
                    //重新加载列表
                    string strSelectedValue = TV_Type.SelectedNode.Value;
                    string[] arrSelectedValue = strSelectedValue.Split('|');

                    string strTreeSelectedText = TV_Type.SelectedNode.Text;
                    string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                    DataBinder(arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2], arrTreeSelectedText[1]);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);
                }

            }
            else if (cmdName == "edit")
            {
                string strSelectedValue = TV_Type.SelectedNode.Value;
                string[] arrSelectedValue = strSelectedValue.Split('|');
                if (arrSelectedValue[3] == "3")
                {

                    string strDLCode = arrSelectedValue[0];
                    string strZLCode = arrSelectedValue[1];
                    string strXLCode = arrSelectedValue[2];
                    string strObjectCode = cmdArges;

                    string strAlertUrl = "TTWZObjectCodeDetail.aspx?DLCode=" + strDLCode + "&ZLCode=" + strZLCode + "&XLCode=" + strXLCode + "&ObjectCode=" + strObjectCode;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertObjectPage('" + strAlertUrl + "')", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZWZDMZNTJZXLSM+"')", true);
                }
            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text;
        WZObjectBLL wZObjectBLL = new WZObjectBLL();
        IList listObject = wZObjectBLL.GetAllWZObjects(strHQL);

        DG_List.DataSource = listObject;
        DG_List.DataBind();

        TXT_ObjectCode.Text = "";

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (TV_Type.SelectedNode != null)
        {
            WZObjectBLL wZObjectBLL = new WZObjectBLL();
            string strSelectedValue = TV_Type.SelectedNode.Value;
            string[] arrSelectedValue = strSelectedValue.Split('|');

            string strNewCreater = HF_NewCreater.Value;
            string strNewIsMark = HF_NewIsMark.Value;

            if (arrSelectedValue[3] == "3")
            {
                string strObjectName = TXT_ObjectName.Text.Trim();
                string strModel = TXT_Model.Text.Trim();
                string strGrade = TXT_Level.Text.Trim();
                string strCriterion = TXT_Standard.Text.Trim();
                int intUnit = 0;
                int.TryParse(DDL_Unit.SelectedValue, out intUnit);
                int intConvertUnit = 0;
                int.TryParse(DDL_ConvertUnit.SelectedValue, out intConvertUnit);
                decimal decimalRatio = 0;
                string strConvertRatio = TXT_ConvertRatio.Text.Trim();
                decimal.TryParse(strConvertRatio, out decimalRatio);
                //string strReferDesc = TXT_ReferDesc.Text.Trim();
                //string strReferStandard = TXT_ReferStandard.Text.Trim();
                //decimal decimalMarket = 0;
                //string strMarket = TXT_Market.Text.Trim();
                //decimal.TryParse(strMarket, out decimalMarket);
                //物资名称，规格型号，级别，标准，任意一个内容为空，提示数据空缺，请补充
                if (string.IsNullOrEmpty(strObjectName))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('物资名称不能为空，请补充！');", true);
                    return;
                }
                if (string.IsNullOrEmpty(strModel))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('规格型号不能为空，请补充！');", true);
                    return;
                }
                if (string.IsNullOrEmpty(strGrade))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('级别不能为空，请补充！');", true);
                    return;
                }
                if (string.IsNullOrEmpty(strCriterion))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('标准不能为空，请补充！');", true);
                    return;
                }
                if (intConvertUnit == 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('换算单位错误，请修改！');", true);
                    return;
                }
                if (!ShareClass.CheckIsNumber(strConvertRatio))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('换算系数必须为小数类型，请修改！');", true);
                    return;
                }
                //if (!ShareClass.CheckIsNumber(strMarket))
                //{
                //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCJBXWXSLXXG+"')", true);
                //    return;
                //}
                if (intUnit == intConvertUnit && decimalRatio != 1)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('换算系数错误，请修改！');", true);
                    return;
                }
                if (intUnit != intConvertUnit && (decimalRatio == 0 || decimalRatio == 1))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('换算关系错误，请修改！');", true);
                    return;
                }

                string strObjectCode = TXT_ObjectCode.Text;
                if (!string.IsNullOrEmpty(strObjectCode))
                {
                    //修改
                    string strObjectSql = "from WZObject as wZObject where ObjectCode = '" + strObjectCode + "'";
                    IList objectList = wZObjectBLL.GetAllWZObjects(strObjectSql);
                    if (objectList != null && objectList.Count == 1)
                    {
                        WZObject wZObject = (WZObject)objectList[0];

                        wZObject.DLCode = arrSelectedValue[0];
                        wZObject.ZLCode = arrSelectedValue[1];
                        wZObject.XLCode = arrSelectedValue[2];
                        wZObject.ObjectName = strObjectName;
                        wZObject.Criterion = strCriterion;
                        wZObject.Grade = strGrade;
                        wZObject.Model = strModel;
                        wZObject.Unit = intUnit;
                        wZObject.ConvertUnit = intConvertUnit;
                        wZObject.ConvertRatio = decimalRatio;
                        //wZObject.ReferDesc = strReferDesc;
                        //wZObject.ReferStandard = strReferStandard;

                        //wZObject.Market = decimalMarket;

                        wZObjectBLL.UpdateWZObject(wZObject, strObjectCode);
                    }
                }
                else
                {
                    string strCheckObjectHQL = string.Format(@"select * from T_WZObject
                                where ObjectName = '{0}'
                                and Model = '{1}'
                                and Criterion = '{2}'
                                and Grade = '{3}'
                                and Unit = {4}", strObjectName, strModel, strCriterion, strGrade, intUnit);
                    DataTable dtCheckObject = ShareClass.GetDataSetFromSql(strCheckObjectHQL, "strCheckObjectHQL").Tables[0];
                    if (dtCheckObject != null && dtCheckObject.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('记录重复，请修改！');", true);
                        return;
                    }

                    //增加
                    WZObject wZObject = new WZObject();
                    wZObject.DLCode = arrSelectedValue[0];
                    wZObject.ZLCode = arrSelectedValue[1];
                    wZObject.XLCode = arrSelectedValue[2];
                    wZObject.ObjectCode = CreateObjectCode(arrSelectedValue[2]);//BasePageOrder.module.GetQueueObjectCode(arrSelectedValue[2]); //生成物资代码;TXT_ObjectCode.Text;
                    wZObject.Creater = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();
                    wZObject.ObjectName = strObjectName;
                    wZObject.Criterion = strCriterion;
                    wZObject.Grade = strGrade;
                    wZObject.Model = strModel;
                    wZObject.Unit = intUnit;
                    wZObject.ConvertUnit = intConvertUnit;
                    wZObject.ConvertRatio = decimalRatio;
                    //wZObject.ReferDesc = strReferDesc;
                    //wZObject.ReferStandard = strReferStandard;

                    //wZObject.Market = decimalMarket;
                    wZObject.CollectTime = DateTime.Now;

                    wZObjectBLL.AddWZObject(wZObject);

                    //修改小类代码的使用标记
                    ShareClass.UpdateXLCodeStatus(arrSelectedValue[2]);
                }


                //重新加载列表
                string strTreeSelectedText = TV_Type.SelectedNode.Text;
                string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                DataBinder(arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2], arrTreeSelectedText[1]);


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZWZDMZNTJZXLSM+"');", true);
            }
        }
    }




    protected void btnReset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        TXT_ObjectCode.Text = "";
        TXT_ObjectName.Text = "";
        TXT_Standard.Text = "";
        TXT_Level.Text = "";
        TXT_Model.Text = "";
        DDL_Unit.SelectedIndex = 0;
        DDL_ConvertUnit.SelectedIndex = 0;
        TXT_ConvertRatio.Text = "0";
        //TXT_ReferDesc.Text = "";
        //TXT_ReferStandard.Text = "";
        //TXT_Market.Text = "0";


        TXT_ObjectName.BackColor = Color.White;
        TXT_Standard.BackColor = Color.White;
        TXT_Level.BackColor = Color.White;
        TXT_Model.BackColor = Color.White;
        DDL_Unit.BackColor = Color.White;
        DDL_ConvertUnit.BackColor = Color.White;
        TXT_ConvertRatio.BackColor = Color.White;

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }



    protected void BT_CreateCode_Click(object sender, EventArgs e)
    {
        string strNewCreater = HF_NewCreater.Value;
        string strNewIsMark = HF_NewIsMark.Value;

        if (TV_Type.SelectedNode != null)
        {
            WZObjectBLL wZObjectBLL = new WZObjectBLL();
            string strSelectedValue = TV_Type.SelectedNode.Value;
            string[] arrSelectedValue = strSelectedValue.Split('|');
            if (arrSelectedValue[3] == "3")
            {
                string strDLCode = arrSelectedValue[0];
                string strZLCode = arrSelectedValue[1];
                string strXLCode = arrSelectedValue[2];

                string strAlertUrl = "TTWZObjectCodeDetail.aspx?DLCode=" + strDLCode + "&ZLCode=" + strZLCode + "&XLCode=" + strXLCode + "&ObjectCode=";
                if (string.IsNullOrEmpty(HF_NewObjectCode.Value))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertObjectPage('" + strAlertUrl + "');", true);
                }
                else {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertObjectPage('" + strAlertUrl + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZWZDMZNTJZXLSM+"');", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择小类代码！');", true);
            return;
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;
        string strObjectSQL;

        string strSelectedValue = LB_WZObjectType.Text;

        string[] arrSelectedValue = strSelectedValue.Split('|');
        //if (arrSelectedValue[0] != "all")
        if (strSelectedValue != "" & strSelectedValue != "all|0|0|0")
        {
            string strTreeSelectedText = TV_Type.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            strObjectSQL = string.Format(@"select o.*,su.UnitName,
                        sc.UnitName as ConvertUnitName,p.UserName as CreaterName from T_WZObject o
                        left join T_WZSpan su on o.Unit = su.ID
                        left join T_WZSpan sc on o.ConvertUnit = sc.ID 
                        left join T_ProjectMember p on o.Creater = p.UserCode
                        where 1=1 
                        and o.DLCode = '{0}'
                        and o.ZLCode = '{1}'
                        and o.XLCode = '{2}'", arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2]);
          
            strObjectSQL += " and o.ObjectCode Like '%" + TB_WZCode.Text.Trim() + "%'";
            strObjectSQL += " and o.ObjectName Like '%" + TB_WZName.Text.Trim() + "%'";
            strObjectSQL += " and o.Grade Like '%" + TB_WZGrade.Text.Trim() + "'";
            strObjectSQL += " and o.Criterion Like '%" + TB_WZCriterion.Text.Trim() + "%'";
            strObjectSQL += " and o.Model Like '%" + TB_WZModel.Text.Trim() + "%'";
        }
        else
        {
            strObjectSQL = string.Format(@"select o.*,su.UnitName,
                        sc.UnitName as ConvertUnitName,p.UserName as CreaterName from T_WZObject o
                        left join T_WZSpan su on o.Unit = su.ID
                        left join T_WZSpan sc on o.ConvertUnit = sc.ID 
                        left join T_ProjectMember p on o.Creater = p.UserCode
                        where 1=1 ");
            strObjectSQL += " and o.ObjectCode Like '%" + TB_WZCode.Text.Trim() + "%'";
            strObjectSQL += " and o.ObjectName Like '%" + TB_WZName.Text.Trim() + "%'";
            strObjectSQL += " and o.Grade Like '%" + TB_WZGrade.Text.Trim() + "'";
            strObjectSQL += " and o.Criterion Like '%" + TB_WZCriterion.Text.Trim() + "%'";
            strObjectSQL += " and o.Model Like '%" + TB_WZModel.Text.Trim() + "%'";

        }

        DataTable dtObject = ShareClass.GetDataSetFromSql(strObjectSQL, "Object").Tables[0];

        DG_List.DataSource = dtObject;
        DG_List.DataBind();
    }

    protected void TV_Type_SelectedNodeChanged(object sender, EventArgs e)
    {
        if (TV_Type.SelectedNode != null)
        {
            string strSelectedValue = TV_Type.SelectedNode.Value;
            LB_WZObjectType.Text = strSelectedValue;

            string[] arrSelectedValue = strSelectedValue.Split('|');
            if (arrSelectedValue[0] != "all")
            {
                string strTreeSelectedText = TV_Type.SelectedNode.Text;
                string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                DataBinder(arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2], arrTreeSelectedText[1]);

                TXT_ObjectCode.Text = "";

                TXT_ObjectName.Text = "";
                TXT_Standard.Text = "";
                TXT_Level.Text = "";
                TXT_Model.Text = "";
                DDL_Unit.SelectedIndex = 0;
                DDL_ConvertUnit.SelectedIndex = 0;
                TXT_ConvertRatio.Text = "";

                //if (arrSelectedValue[3] == "3")
                //{
                //    TXT_ObjectName.BackColor = Color.CornflowerBlue;
                //    TXT_Standard.BackColor = Color.CornflowerBlue;
                //    TXT_Level.BackColor = Color.CornflowerBlue;
                //    TXT_Model.BackColor = Color.CornflowerBlue;
                //    DDL_Unit.BackColor = Color.CornflowerBlue;
                //    DDL_ConvertUnit.BackColor = Color.CornflowerBlue;
                //    TXT_ConvertRatio.BackColor = Color.CornflowerBlue;
                //}
                //else
                //{
                //    TXT_ObjectName.BackColor = Color.White;
                //    TXT_Standard.BackColor = Color.White;
                //    TXT_Level.BackColor = Color.White;
                //    TXT_Model.BackColor = Color.White;
                //    DDL_Unit.BackColor = Color.White;
                //    DDL_ConvertUnit.BackColor = Color.White;
                //    TXT_ConvertRatio.BackColor = Color.White;
                //}
            }
            else
            {
                TXT_ObjectCode.Text = "";

                TXT_ObjectName.Text = "";
                TXT_Standard.Text = "";
                TXT_Level.Text = "";
                TXT_Model.Text = "";
                DDL_Unit.SelectedIndex = 0;
                DDL_ConvertUnit.SelectedIndex = 0;
                TXT_ConvertRatio.Text = "";

                TXT_ObjectName.BackColor = Color.White;
                TXT_Standard.BackColor = Color.White;
                TXT_Level.BackColor = Color.White;
                TXT_Model.BackColor = Color.White;
                DDL_Unit.BackColor = Color.White;
                DDL_ConvertUnit.BackColor = Color.White;
                TXT_ConvertRatio.BackColor = Color.White;
            }
        }

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }



    private void DataBinder(string strDLCode, string strZLCode, string strXLCode, string strXLName)
    {
        DG_List.CurrentPageIndex = 0;

        string strObjectSQL = string.Format(@"select o.*,su.UnitName,
                        sc.UnitName as ConvertUnitName,p.UserName as CreaterName from T_WZObject o
                        left join T_WZSpan su on o.Unit = su.ID
                        left join T_WZSpan sc on o.ConvertUnit = sc.ID 
                        left join T_ProjectMember p on o.Creater = p.UserCode
                        where 1=1 
                        and o.DLCode = '{0}'
                        and o.ZLCode = '{1}'
                        and o.XLCode = '{2}'", strDLCode, strZLCode, strXLCode);
        DataTable dtObject = ShareClass.GetDataSetFromSql(strObjectSQL, "Object").Tables[0];

        DG_List.DataSource = dtObject;
        DG_List.DataBind();

        LB_Sql.Text = strObjectSQL;

        LB_ShowXLName.Text = strXLCode;// strXLName;
        LB_ShowRecordCount.Text = dtObject.Rows.Count.ToString();

        ControlStatusCloseChange();
        #region 注释
        //DG_List.CurrentPageIndex = 0;

        //WZObjectBLL wZObjectBLL = new WZObjectBLL();
        //string strObjectSQL = "from WZObject as wZObject where 1=1 ";
        //strObjectSQL += " and DLCode = '" + strDLCode + "'";
        //strObjectSQL += " and ZLCode = '" + strZLCode + "'";
        //strObjectSQL += " and XLCode = '" + strXLCode + "'";

        //IList listObject = wZObjectBLL.GetAllWZObjects(strObjectSQL);

        //DG_List.DataSource = listObject;
        //DG_List.DataBind();

        //LB_Sql.Text = strObjectSQL;
        #endregion
    }


    private void BindUnitData()
    {
        WZSpanBLL wZSpanBLL = new WZSpanBLL();
        string strWZSpan = "from WZSpan as wZSpan";
        IList lstWZSpan = wZSpanBLL.GetAllWZSpans(strWZSpan);

        DDL_Unit.DataSource = lstWZSpan;
        DDL_Unit.DataBind();

        DDL_ConvertUnit.DataSource = lstWZSpan;
        DDL_ConvertUnit.DataBind();

        DDL_ConvertUnit.Items.Insert(0, new ListItem("选择", "0"));
    }




    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strEditObjectCode = HF_NewObjectCode.Value;
        if (string.IsNullOrEmpty(strEditObjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJYCZDWZDMLB+"')", true);
            return;
        }

        string strNewCreater = HF_NewCreater.Value;
        string strNewIsMark = HF_NewIsMark.Value;

        string strSelectedValue = TV_Type.SelectedNode.Value;
        string[] arrSelectedValue = strSelectedValue.Split('|');
        if (arrSelectedValue[3] == "3")
        {

            string strDLCode = arrSelectedValue[0];
            string strZLCode = arrSelectedValue[1];
            string strXLCode = arrSelectedValue[2];
            string strObjectCode = strEditObjectCode;

            string strAlertUrl = "TTWZObjectCodeDetail.aspx?DLCode=" + strDLCode + "&ZLCode=" + strZLCode + "&XLCode=" + strXLCode + "&ObjectCode=" + strObjectCode;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertObjectPage('" + strAlertUrl + "');", true);

        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZWZDMZNTJZXLSM+"');", true);
        }
    }



    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //删除
        string strEditObjectCode = HF_NewObjectCode.Value;
        if (string.IsNullOrEmpty(strEditObjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJYCZDWZDMLB+"')", true);
            return;
        }

        string strNewCreater = HF_NewCreater.Value;
        string strNewIsMark = HF_NewIsMark.Value;

        WZObjectBLL wZObjectBLL = new WZObjectBLL();
        string strObjectSql = "from WZObject as wZObject where ObjectCode = '" + strEditObjectCode + "'";
        IList objectList = wZObjectBLL.GetAllWZObjects(strObjectSql);
        if (objectList != null && objectList.Count == 1)
        {
            WZObject wZObject = (WZObject)objectList[0];

            if (wZObject.Creater != strUserCode || wZObject.IsMark != 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYBJBW0HZBSCJRBYXSC+"');", true);
                return;
            }

            wZObjectBLL.DeleteWZObject(wZObject);
            //重新加载列表
            string strSelectedValue = TV_Type.SelectedNode.Value;
            string[] arrSelectedValue = strSelectedValue.Split('|');

            string strTreeSelectedText = TV_Type.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            DataBinder(arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2], arrTreeSelectedText[1]);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"');", true);
        }
        else {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYBJBW0HZBSCJRBYXSC+"');", true);
        }
    }



    protected void BT_NewTitle_Click(object sender, EventArgs e)
    {
        //重做标记
        string strEditObjectCode = HF_NewObjectCode.Value;
        if (string.IsNullOrEmpty(strEditObjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJYCZDWZDMLB+"')", true);
            return;
        }

        WZObjectBLL wZObjectBLL = new WZObjectBLL();
        string strObjectSQL = string.Format(@"from WZObject as wZObject
                        where ObjectCode = '{0}'", strEditObjectCode);
        IList lstWZObject = wZObjectBLL.GetAllWZObjects(strObjectSQL);
        if (lstWZObject != null && lstWZObject.Count > 0)
        {
            WZObject wZObject = (WZObject)lstWZObject[0];
            string strObjectCode = wZObject.ObjectCode;

            int intIsMarkValue = 0;

            //先看计划明细中是有当前物资代码
            string strCheckPlanDetailHQL = string.Format(@"select * from T_WZPickingPlanDetail 
                                        where ObjectCode = '{0}'", strObjectCode);
            DataTable dtCheckPlanDetail = ShareClass.GetDataSetFromSql(strCheckPlanDetailHQL, "PlanDetail").Tables[0];
            if (dtCheckPlanDetail != null && dtCheckPlanDetail.Rows.Count > 0)
            {
                intIsMarkValue = -1;
            }

            if (intIsMarkValue == 0)
            {
                //移交单
                string strCheckTurnDetailHQL = string.Format(@"select * from T_WZTurnDetail
                                        where ObjectCode = '{0}'", strObjectCode);
                DataTable dtCheckTurnDetail = ShareClass.GetDataSetFromSql(strCheckTurnDetailHQL, "TurnDetail").Tables[0];
                if (dtCheckTurnDetail != null && dtCheckTurnDetail.Rows.Count > 0)
                {
                    intIsMarkValue = -1;
                }
            }
            if (intIsMarkValue == 0)
            {
                //采购清单
                string strCheckPurchaseDetailHQL = string.Format(@"select * from T_WZPurchaseDetail
                                        where ObjectCode = '{0}'", strObjectCode);
                DataTable dtCheckPurchaseDetail = ShareClass.GetDataSetFromSql(strCheckPurchaseDetailHQL, "PurchaseDetail").Tables[0];
                if (dtCheckPurchaseDetail != null && dtCheckPurchaseDetail.Rows.Count > 0)
                {
                    intIsMarkValue = -1;
                }
            }
            if (intIsMarkValue == 0)
            {
                //合同明细
                string strCheckCompactDetailHQL = string.Format(@"select * from T_WZCompactDetail
                                        where ObjectCode = '{0}'", strObjectCode);
                DataTable dtCheckCompactDetail = ShareClass.GetDataSetFromSql(strCheckCompactDetailHQL, "CompactDetail").Tables[0];
                if (dtCheckCompactDetail != null && dtCheckCompactDetail.Rows.Count > 0)
                {
                    intIsMarkValue = -1;
                }
            }
            if (intIsMarkValue == 0)
            {
                //收料单
                string strCheckCollectHQL = string.Format(@"select * from T_WZCollect
                                        where ObjectCode = '{0}'", strObjectCode);
                DataTable dtCheckCollect = ShareClass.GetDataSetFromSql(strCheckCollectHQL, "Collect").Tables[0];
                if (dtCheckCollect != null && dtCheckCollect.Rows.Count > 0)
                {
                    intIsMarkValue = -1;
                }
            }
            if (intIsMarkValue == 0)
            {
                //发料单
                string strCheckSendHQL = string.Format(@"select * from T_WZSend
                                        where ObjectCode = '{0}'", strObjectCode);
                DataTable dtCheckSend = ShareClass.GetDataSetFromSql(strCheckSendHQL, "Send").Tables[0];
                if (dtCheckSend != null && dtCheckSend.Rows.Count > 0)
                {
                    intIsMarkValue = -1;
                }
            }
            //以上都没有，就把使用标记改为0
            wZObject.IsMark = intIsMarkValue;

            wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

            //重新加载列表
            string strSelectedValue = TV_Type.SelectedNode.Value;
            string[] arrSelectedValue = strSelectedValue.Split('|');
            string strTreeSelectedText = TV_Type.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');


            DataBinder(arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2], arrTreeSelectedText[1]);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZZSYBJCG+"');", true);

        }
        else {
            string strNewCreater = HF_NewCreater.Value;
            string strNewIsMark = HF_NewIsMark.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYBJBW0HZBSCJRBYXSC+"');", true);
        }
    }



    protected void BT_NewReplace_Click(object sender, EventArgs e)
    {
        //代码替换
        string strEditObjectCode = HF_NewObjectCode.Value;
        if (string.IsNullOrEmpty(strEditObjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJYCZDWZDMLB+"')", true);
            return;
        }

        string strNewCreater = HF_NewCreater.Value;
        string strNewIsMark = HF_NewIsMark.Value;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertObjectPage('TTWZObjectCodeReplace.aspx');", true);
        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickReplaceCode('" + strEditObjectCode + "');ControlStatusChange('" + strNewCreater + "','" + strUserCode + "','" + strNewIsMark + "');", true);
    }



    protected void BT_NewBrowse_Click(object sender, EventArgs e)
    {
        //浏览
        string strEditObjectCode = HF_NewObjectCode.Value;
        if (string.IsNullOrEmpty(strEditObjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJYCZDWZDMLB+"')", true);
            return;
        }

        string strCreater = HF_NewCreater.Value;
        string strIsMark = HF_NewIsMark.Value ;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertObjectPage('TTWZObjectCodeBrowse.aspx?ObjectCode=" + strEditObjectCode + "');", true);
    }


    protected void BT_ReplaceOjbectCode_Click(object sender, EventArgs e)
    {
        //代码替换
        string strReplaceObjectCode = HF_ReplaceObjectCode.Value;
        string strReplaceNewObjectCode = HF_ReplaceNewObjectCode.Value;

        WZObjectBLL wZObjectBLL = new WZObjectBLL();
        string strObjectSql = "from WZObject as wZObject where ObjectCode = '" + strReplaceNewObjectCode + "'";
        IList objectList = wZObjectBLL.GetAllWZObjects(strObjectSql);
        if (objectList != null && objectList.Count > 0)
        {
            //计划明细
            string strUpdatePlanDetailHQL = string.Format(@"update T_WZPickingPlanDetail 
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strReplaceObjectCode, strReplaceNewObjectCode);
            ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

            //移交单
            string strUpdateTurnDetailHQL = string.Format(@"update T_WZTurnDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strReplaceObjectCode, strReplaceNewObjectCode);
            ShareClass.RunSqlCommand(strUpdateTurnDetailHQL);

            //采购清单
            string strUpdatePurchaseDetailHQL = string.Format(@"update T_WZPurchaseDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strReplaceObjectCode, strReplaceNewObjectCode);
            ShareClass.RunSqlCommand(strUpdatePurchaseDetailHQL);

            //合同明细
            string strUpdateCompactDetailHQL = string.Format(@"update T_WZCompactDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strReplaceObjectCode, strReplaceNewObjectCode);
            ShareClass.RunSqlCommand(strUpdateCompactDetailHQL);

            //收料单
            string strUpdateCollectHQL = string.Format(@"update T_WZCollect
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strReplaceObjectCode, strReplaceNewObjectCode);
            ShareClass.RunSqlCommand(strUpdateCollectHQL);

            //发料单
            string strUpdateSendHQL = string.Format(@"update T_WZSend
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strReplaceObjectCode, strReplaceNewObjectCode);
            ShareClass.RunSqlCommand(strUpdateSendHQL);


            //重新加载列表
            string strSelectedValue = TV_Type.SelectedNode.Value;
            string[] arrSelectedValue = strSelectedValue.Split('|');
            string strTreeSelectedText = TV_Type.SelectedNode.Text;
            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

            DataBinder(arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2], arrTreeSelectedText[1]);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTHCG+"');", true);
        }
        else {
            string strNewCreater = HF_NewCreater.Value;
            string strNewIsMark = HF_NewIsMark.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('新的物资代码不存在，请填写正确的物资代码！');", true);
            return;
        }
    }


    //重做使用标记
    protected void BT_Title_Click(object sender, EventArgs e)
    {
        if (TV_Type.SelectedNode != null)
        {
            string strSelectedValue = TV_Type.SelectedNode.Value;
            string[] arrSelectedValue = strSelectedValue.Split('|');
            if (arrSelectedValue[0] != "all")
            {
                TXT_ObjectCode.Text = "";

                WZObjectBLL wZObjectBLL = new WZObjectBLL();
                string strObjectSQL = string.Format(@"from WZObject as wZObject
                        where DLCode = '{0}'
                        and ZLCode = '{1}'
                        and XLCode = '{2}'", arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2]);
                IList lstWZObject = wZObjectBLL.GetAllWZObjects(strObjectSQL);
                if (lstWZObject != null && lstWZObject.Count > 0)
                {
                    for (int i = 0; i < lstWZObject.Count; i++)
                    {
                        WZObject wZObject = (WZObject)lstWZObject[i];
                        string strObjectCode = wZObject.ObjectCode;

                        //先看计划明细中是有当前物资代码
                        string strCheckPlanDetailHQL = string.Format(@"select * from T_WZPickingPlanDetail 
                                        where ObjectCode = '{0}'", strObjectCode);
                        DataTable dtCheckPlanDetail = ShareClass.GetDataSetFromSql(strCheckPlanDetailHQL, "PlanDetail").Tables[0];
                        if (dtCheckPlanDetail != null && dtCheckPlanDetail.Rows.Count > 0)
                        {
                            wZObject.IsMark = -1;

                            wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

                            continue;
                        }

                        //移交单
                        string strCheckTurnDetailHQL = string.Format(@"select * from T_WZTurnDetail
                                        where ObjectCode = '{0}'", strObjectCode);
                        DataTable dtCheckTurnDetail = ShareClass.GetDataSetFromSql(strCheckTurnDetailHQL, "TurnDetail").Tables[0];
                        if (dtCheckTurnDetail != null && dtCheckTurnDetail.Rows.Count > 0)
                        {
                            wZObject.IsMark = -1;

                            wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

                            continue;
                        }

                        //采购清单
                        string strCheckPurchaseDetailHQL = string.Format(@"select * from T_WZPurchaseDetail
                                        where ObjectCode = '{0}'", strObjectCode);
                        DataTable dtCheckPurchaseDetail = ShareClass.GetDataSetFromSql(strCheckPurchaseDetailHQL, "PurchaseDetail").Tables[0];
                        if (dtCheckPurchaseDetail != null && dtCheckPurchaseDetail.Rows.Count > 0)
                        {
                            wZObject.IsMark = -1;

                            wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

                            continue;
                        }

                        //合同明细
                        string strCheckCompactDetailHQL = string.Format(@"select * from T_WZCompactDetail
                                        where ObjectCode = '{0}'", strObjectCode);
                        DataTable dtCheckCompactDetail = ShareClass.GetDataSetFromSql(strCheckCompactDetailHQL, "CompactDetail").Tables[0];
                        if (dtCheckCompactDetail != null && dtCheckCompactDetail.Rows.Count > 0)
                        {
                            wZObject.IsMark = -1;

                            wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

                            continue;
                        }

                        //收料单
                        string strCheckCollectHQL = string.Format(@"select * from T_WZCollect
                                        where ObjectCode = '{0}'", strObjectCode);
                        DataTable dtCheckCollect = ShareClass.GetDataSetFromSql(strCheckCollectHQL, "Collect").Tables[0];
                        if (dtCheckCollect != null && dtCheckCollect.Rows.Count > 0)
                        {
                            wZObject.IsMark = -1;

                            wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

                            continue;
                        }

                        //发料单
                        string strCheckSendHQL = string.Format(@"select * from T_WZSend
                                        where ObjectCode = '{0}'", strObjectCode);
                        DataTable dtCheckSend = ShareClass.GetDataSetFromSql(strCheckSendHQL, "Send").Tables[0];
                        if (dtCheckSend != null && dtCheckSend.Rows.Count > 0)
                        {
                            wZObject.IsMark = -1;

                            wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

                            continue;
                        }

                        //以上都没有，就把使用标记改为0

                        wZObject.IsMark = 0;

                        wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

                    }


                    //重新加载列表
                    string strTreeSelectedText = TV_Type.SelectedNode.Text;
                    string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                    DataBinder(arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2], arrTreeSelectedText[1]);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZZSYBJCG+"')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXZDLZLHXLDM+"')", true);
            return;
        }
    }

    //“错误代码”替换成正确的“物资代码”
    protected void BT_Replace_Click(object sender, EventArgs e)
    {
        if (TV_Type.SelectedNode != null)
        {
            string strSelectedValue = TV_Type.SelectedNode.Value;
            string[] arrSelectedValue = strSelectedValue.Split('|');
            if (arrSelectedValue[0] != "all")
            {
                string strObjectCode = TXT_ObjectCode.Text;
                if (!string.IsNullOrEmpty(strObjectCode))
                {
                    string strReplaceObjectCode = TXT_Replace.Text.Trim();
                    if (!string.IsNullOrEmpty(strReplaceObjectCode))
                    {
                        WZObjectBLL wZObjectBLL = new WZObjectBLL();
                        string strObjectSql = "from WZObject as wZObject where ObjectCode = '" + strReplaceObjectCode + "'";
                        IList objectList = wZObjectBLL.GetAllWZObjects(strObjectSql);
                        if (objectList != null && objectList.Count > 0)
                        {
                            //计划明细
                            string strUpdatePlanDetailHQL = string.Format(@"update T_WZPickingPlanDetail 
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strObjectCode, strReplaceObjectCode);
                            ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

                            //移交单
                            string strUpdateTurnDetailHQL = string.Format(@"update T_WZTurnDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strObjectCode, strReplaceObjectCode);
                            ShareClass.RunSqlCommand(strUpdateTurnDetailHQL);

                            //采购清单
                            string strUpdatePurchaseDetailHQL = string.Format(@"update T_WZPurchaseDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strObjectCode, strReplaceObjectCode);
                            ShareClass.RunSqlCommand(strUpdatePurchaseDetailHQL);

                            //合同明细
                            string strUpdateCompactDetailHQL = string.Format(@"update T_WZCompactDetail
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strObjectCode, strReplaceObjectCode);
                            ShareClass.RunSqlCommand(strUpdateCompactDetailHQL);

                            //收料单
                            string strUpdateCollectHQL = string.Format(@"update T_WZCollect
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strObjectCode, strReplaceObjectCode);
                            ShareClass.RunSqlCommand(strUpdateCollectHQL);

                            //发料单
                            string strUpdateSendHQL = string.Format(@"update T_WZSend
                                        set ObjectCode = '{1}'
                                        where ObjectCode = '{0}'", strObjectCode, strReplaceObjectCode);
                            ShareClass.RunSqlCommand(strUpdateSendHQL);


                            //重新加载列表
                            string strTreeSelectedText = TV_Type.SelectedNode.Text;
                            string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                            DataBinder(arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2], arrTreeSelectedText[1]);

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTHCG+"')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDWZDMBZBXZWZBZCZ+"')", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTXXDWZDM+"')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXZWZDM+"')", true);
                    return;
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXZDLZLHXLDM+"')", true);
            return;
        }
    }


    //加载列表
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        string strNewCreater = HF_NewCreater.Value;
        string strNewIsMark = HF_NewIsMark.Value;

        if (TV_Type.SelectedNode != null)
        {
            string strSelectedValue = TV_Type.SelectedNode.Value;
            string[] arrSelectedValue = strSelectedValue.Split('|');
            if (arrSelectedValue[0] != "all")
            {
                string strTreeSelectedText = TV_Type.SelectedNode.Text;
                string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                DataBinder(arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2], arrTreeSelectedText[1]);

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择小类代码！');", true);
                return;
            }
        }
        else {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            return;
        }
    }

    protected void BT_ObjectCode_Click(object sender, EventArgs e)
    {
        string strNewCreater = HF_NewCreater.Value;
        string strNewIsMark = HF_NewIsMark.Value;

        if (TV_Type.SelectedNode != null)
        {
            WZObjectBLL wZObjectBLL = new WZObjectBLL();
            string strSelectedValue = TV_Type.SelectedNode.Value;
            string[] arrSelectedValue = strSelectedValue.Split('|');
            if (arrSelectedValue[3] == "3")
            {
                string strTreeSelectedText = TV_Type.SelectedNode.Text;
                string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                DG_List.CurrentPageIndex = 0;

                string strObjectSQL = string.Format(@"select o.*,su.UnitName,
                        sc.UnitName as ConvertUnitName,p.UserName as CreaterName from T_WZObject o
                        left join T_WZSpan su on o.Unit = su.ID
                        left join T_WZSpan sc on o.ConvertUnit = sc.ID 
                        left join T_ProjectMember p on o.Creater = p.UserCode
                        where 1=1 
                        and o.DLCode = '{0}'
                        and o.ZLCode = '{1}'
                        and o.XLCode = '{2}'", arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2]);

                if (!string.IsNullOrEmpty(HF_SortObjectCode.Value))
                {
                    strObjectSQL += " order by o.XLCode desc,o.ObjectCode desc";

                    HF_SortObjectCode.Value = "";
                }
                else
                {
                    strObjectSQL += " order by o.XLCode asc,o.ObjectCode asc";

                    HF_SortObjectCode.Value = "ObjectCode";
                }

                DataTable dtObject = ShareClass.GetDataSetFromSql(strObjectSQL, "Object").Tables[0];

                DG_List.DataSource = dtObject;
                DG_List.DataBind();

                LB_Sql.Text = strObjectSQL;

                LB_ShowXLName.Text = arrTreeSelectedText[0];// arrTreeSelectedText[1];
                LB_ShowRecordCount.Text = dtObject.Rows.Count.ToString();

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
                ControlStatusCloseChange();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择小类代码！');", true);
            }
        }
        else {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }


    protected void BT_IsMark_Click(object sender, EventArgs e)
    {
        string strNewCreater = HF_NewCreater.Value;
        string strNewIsMark = HF_NewIsMark.Value;

        if (TV_Type.SelectedNode != null)
        {
            WZObjectBLL wZObjectBLL = new WZObjectBLL();
            string strSelectedValue = TV_Type.SelectedNode.Value;
            string[] arrSelectedValue = strSelectedValue.Split('|');
            if (arrSelectedValue[3] == "3")
            {
                string strTreeSelectedText = TV_Type.SelectedNode.Text;
                string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                DG_List.CurrentPageIndex = 0;

                string strObjectSQL = string.Format(@"select o.*,su.UnitName,
                        sc.UnitName as ConvertUnitName,p.UserName as CreaterName from T_WZObject o
                        left join T_WZSpan su on o.Unit = su.ID
                        left join T_WZSpan sc on o.ConvertUnit = sc.ID 
                        left join T_ProjectMember p on o.Creater = p.UserCode
                        where 1=1 
                        and o.DLCode = '{0}'
                        and o.ZLCode = '{1}'
                        and o.XLCode = '{2}'", arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2]);

                if (!string.IsNullOrEmpty(HF_SortIsMark.Value))
                {
                    strObjectSQL += " order by o.XLCode desc,o.IsMark desc";

                    HF_SortIsMark.Value = "";
                }
                else
                {
                    strObjectSQL += " order by o.XLCode asc,o.IsMark asc";

                    HF_SortIsMark.Value = "ObjectCode";
                }

                DataTable dtObject = ShareClass.GetDataSetFromSql(strObjectSQL, "Object").Tables[0];

                DG_List.DataSource = dtObject;
                DG_List.DataBind();

                LB_Sql.Text = strObjectSQL;

                LB_ShowXLName.Text = arrTreeSelectedText[0]; //arrTreeSelectedText[1];
                LB_ShowRecordCount.Text = dtObject.Rows.Count.ToString();

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
                ControlStatusCloseChange();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择小类代码！');", true);
            }
        }
        else {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }



    protected void BT_ObjectName_Click(object sender, EventArgs e)
    {
        string strNewCreater = HF_NewCreater.Value;
        string strNewIsMark = HF_NewIsMark.Value;

        if (TV_Type.SelectedNode != null)
        {
            WZObjectBLL wZObjectBLL = new WZObjectBLL();
            string strSelectedValue = TV_Type.SelectedNode.Value;
            string[] arrSelectedValue = strSelectedValue.Split('|');
            if (arrSelectedValue[3] == "3")
            {
                string strTreeSelectedText = TV_Type.SelectedNode.Text;
                string[] arrTreeSelectedText = strTreeSelectedText.Split(' ');

                DG_List.CurrentPageIndex = 0;

                string strObjectSQL = string.Format(@"select o.*,su.UnitName,
                        sc.UnitName as ConvertUnitName,p.UserName as CreaterName from T_WZObject o
                        left join T_WZSpan su on o.Unit = su.ID
                        left join T_WZSpan sc on o.ConvertUnit = sc.ID 
                        left join T_ProjectMember p on o.Creater = p.UserCode
                        where 1=1 
                        and o.DLCode = '{0}'
                        and o.ZLCode = '{1}'
                        and o.XLCode = '{2}'", arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2]);

                if (!string.IsNullOrEmpty(HF_SortObjectName.Value))
                {
                    strObjectSQL += " order by o.XLCode desc,o.ObjectName desc,o.Model desc";

                    HF_SortObjectName.Value = "";
                }
                else
                {
                    strObjectSQL += " order by o.XLCode asc,o.ObjectName asc,o.Model asc";

                    HF_SortObjectName.Value = "ObjectName";
                }

                DataTable dtObject = ShareClass.GetDataSetFromSql(strObjectSQL, "Object").Tables[0];

                DG_List.DataSource = dtObject;
                DG_List.DataBind();

                LB_Sql.Text = strObjectSQL;

                LB_ShowXLName.Text = arrTreeSelectedText[0]; //arrTreeSelectedText[1];
                LB_ShowRecordCount.Text = dtObject.Rows.Count.ToString();

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
                ControlStatusCloseChange();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择小类代码！');", true);
            }
        }
        else {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }




    
        private void ControlStatusChange(string objCreater,string objUserCode,string objIsMark) {

            BT_NewEdit.Enabled = true;
            BT_NewTitle.Enabled = true;
            BT_NewReplace.Enabled =true;
            BT_NewBrowse.Enabled = true;
           
            if (objCreater == objUserCode && objIsMark == "0") {
                BT_NewDelete.Enabled = true;                           //删除
            }
            else{
                BT_NewDelete.Enabled = false;                           //删除
            }
        }



        private void ControlStatusCloseChange() {
            BT_NewEdit.Enabled = false;
            BT_NewDelete.Enabled = false;
            BT_NewTitle.Enabled = false;
            BT_NewReplace.Enabled = false;
            BT_NewBrowse.Enabled = false;
            
        }




    /// <summary>
    ///  生成物资代码，导入时因为是同一个小类代码，所以可以批量的产生,而单个添加时，小类代码不一样，所以要单独的写出来
    /// </summary>
    /// <returns>物资代码</returns>
    private string CreateObjectCode(string strXLCode)
    {
        string strNewObjectCode = string.Empty;

        try
        {
            lock (this)
            {
                bool isExist = true;
                string strMinObjectCodeHQL = string.Format("select COUNT(1) as RowNumber from T_WZObject where XLCode = '{0}'", strXLCode);
                DataTable dtMinObjectCode = ShareClass.GetDataSetFromSql(strMinObjectCodeHQL, "strMinObjectCodeHQL").Tables[0];
                int intCount = 0;
                int.TryParse(ShareClass.ObjectToString(dtMinObjectCode.Rows[0]["RowNumber"]), out intCount);
                intCount = intCount + 1;
                do
                {
                    StringBuilder sbObjectCode = new StringBuilder();
                    for (int j = 4 - intCount.ToString().Length; j > 0; j--)
                    {
                        sbObjectCode.Append("0");
                    }
                    strNewObjectCode = strXLCode + sbObjectCode.ToString() + intCount.ToString();

                    //判断所取的物资代号是否已存在
                    string strIsExistObjectCodeHQL = string.Format("select COUNT(1) as RowNumber from T_WZObject where ObjectCode = '{0}'", strNewObjectCode);
                    DataTable dtIsExistObjectCode = ShareClass.GetDataSetFromSql(strIsExistObjectCodeHQL, "strIsExistObjectCodeHQL").Tables[0];
                    int intIsExistCount = 0;
                    int.TryParse(ShareClass.ObjectToString(dtIsExistObjectCode.Rows[0]["RowNumber"]), out intIsExistCount);
                    if (intIsExistCount == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intCount++;
                    }

                } while (isExist);
            }
        }
        catch (Exception ex)
        { }
        return strNewObjectCode;
    }
    
}




