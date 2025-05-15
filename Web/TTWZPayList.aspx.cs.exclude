using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Data;
using System.Text;
using System.Drawing;

public partial class TTWZPayList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); 
        bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataPayBinder();

            DataProjectBinder();
        }
    }

    private void DataPayBinder()
    {
        DG_Pay.CurrentPageIndex = 0;

        string strWZPayHQL = string.Format(@"select p.*,m.UserName as MarkerName from T_WZPay p
                        left join T_ProjectMember m on p.Marker = m.UserCode 
                        where p.Marker ='{0}' 
                        order by p.PayTime desc", strUserCode);
        DataTable dtPay = ShareClass.GetDataSetFromSql(strWZPayHQL, "Pay").Tables[0];

        DG_Pay.DataSource = dtPay;
        DG_Pay.DataBind();

        LB_PaySql.Text = strWZPayHQL;
    }


    private void DataProjectBinder()
    {
        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectHQL = "from WZProject as wZProject where PowerPurchase = '有' order by MarkTime desc";
        IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);

        DDL_Project.DataSource = listProject;
        DDL_Project.DataBind();

        DDL_Project.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void DG_Pay_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "click")
            {
                //操作
                for (int i = 0; i < DG_Pay.Items.Count; i++)
                {
                    DG_Pay.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                string[] arrOperate = cmdArges.Split('|');

                string strEditPayID = arrOperate[0];
                string strProgress = arrOperate[1];

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "');", true);

                HF_NewPayID.Value = strEditPayID;
                HF_NewProgress.Value = strProgress;
            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZPayBLL wZPayBLL = new WZPayBLL();
                string strWZPayHQL = "from WZPay as wZPay where PayID = '" + cmdArges + "'";
                IList listWZPay = wZPayBLL.GetAllWZPays(strWZPayHQL);
                if (listWZPay != null && listWZPay.Count == 1)
                {
                    WZPay wZPay = (WZPay)listWZPay[0];
                    if (wZPay.Progress != "录入" || wZPay.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWLRYJSYBJBW0BYXSC + "')", true);
                        return;
                    }

                    wZPayBLL.DeleteWZPay(wZPay);

                    //重新加载列表
                    DataPayBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCCG + "')", true);
                }

            }
            else if (cmdName == "edit")
            {
                for (int i = 0; i < DG_Pay.Items.Count; i++)
                {
                    DG_Pay.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                WZPayBLL wZPayBLL = new WZPayBLL();
                string strWZPayHQL = "from WZPay as wZPay where PayID = '" + cmdArges + "'";
                IList listWZPay = wZPayBLL.GetAllWZPays(strWZPayHQL);
                if (listWZPay != null && listWZPay.Count == 1)
                {
                    WZPay wZPay = (WZPay)listWZPay[0];

                    if (wZPay.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSYBJBW0BYXBJ + "')", true);
                        return;
                    }

                    TXT_PayID.Text = wZPay.PayID;
                    DDL_Project.SelectedValue = wZPay.ProjectCode;
                    TXT_PayName.Text = wZPay.PayName;
                }
            }
            else if (cmdName == "request")
            {
                //报批
                string cmdArges = e.CommandArgument.ToString();
                WZPayBLL wZPayBLL = new WZPayBLL();
                string strWZPayHQL = "from WZPay as wZPay where PayID = '" + cmdArges + "'";
                IList listWZPay = wZPayBLL.GetAllWZPays(strWZPayHQL);
                if (listWZPay != null && listWZPay.Count == 1)
                {
                    WZPay wZPay = (WZPay)listWZPay[0];

                    if (wZPay.Progress != "录入")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBSLRZTBNBP + "')", true);
                        return;
                    }

                    wZPay.Progress = "报批";

                    wZPayBLL.UpdateWZPay(wZPay, wZPay.PayID);

                    //重新加载预付款列表
                    DataPayBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBPCG + "')", true);
                }
            }
            else if (cmdName == "execute")
            {
                //执行
                string cmdArges = e.CommandArgument.ToString();
                WZPayBLL wZPayBLL = new WZPayBLL();
                string strWZPayHQL = "from WZPay as wZPay where PayID = '" + cmdArges + "'";
                IList listWZPay = wZPayBLL.GetAllWZPays(strWZPayHQL);
                if (listWZPay != null && listWZPay.Count == 1)
                {
                    WZPay wZPay = (WZPay)listWZPay[0];

                    if (wZPay.Progress != "批准")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBSPZZTBNZX + "')", true);
                        return;
                    }

                    //检查1
                    bool IsCheck1 = false;
                    string strCheckHQL1 = string.Format(@"select r.* from T_WZPay p
                            left join T_WZPayDetail d on p.PayID = d.PayID
                            left join T_WZRequest r on d.RequestCode = r.RequestCode
                            where p.PayID = '{0}'", cmdArges);
                    DataTable dtCheck1 = ShareClass.GetDataSetFromSql(strCheckHQL1, "Check1").Tables[0];
                    if (dtCheck1 != null && dtCheck1.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCheck1.Rows)
                        {
                            if (!"报销".Contains(ShareClass.ObjectToString(dr["Progress"])))
                            {
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZYKDJDBSBXXCL + "')", true);
                                return;
                            }
                            else
                            {
                                IsCheck1 = true;
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZBDXYDKDJL + "')", true);
                        return;
                    }

                    if (IsCheck1)
                    {
                        wZPay.Progress = "付款";
                        wZPayBLL.UpdateWZPay(wZPay, wZPay.PayID);

                        WZPayDetailBLL wZPayDetailBLL = new WZPayDetailBLL();
                        string strWZPayDetailHQL = string.Format(@"from WZPayDetail as wZPayDetail 
                        where PayID= '{0}'", wZPay.PayID);
                        IList listWZPayDetail = wZPayDetailBLL.GetAllWZPayDetails(strWZPayDetailHQL);
                        if (listWZPayDetail != null && listWZPayDetail.Count > 0)
                        {
                            for (int i = 0; i < listWZPayDetail.Count; i++)
                            {
                                WZPayDetail wZPayDetail = (WZPayDetail)listWZPayDetail[i];

                                //付款明细<预付进度> = "付款"
                                wZPayDetail.PayProcess = "付款";

                                wZPayDetailBLL.UpdateWZPayDetail(wZPayDetail, wZPayDetail.ID);

                                //请款单
                                WZRequestBLL wZRequestBLL = new WZRequestBLL();
                                string strWZRequestHQL = string.Format(@"from WZRequest as wZRequest
                                        where RequestCode = '{0}'", wZPayDetail.RequestCode);
                                IList listRequest = wZRequestBLL.GetAllWZRequests(strWZRequestHQL);
                                if (listRequest != null && listRequest.Count > 0)
                                {
                                    WZRequest wZRequest = (WZRequest)listRequest[0];

                                    decimal decimalPayMoney = wZRequest.PayMoney + wZPayDetail.PlanMoney;
                                    wZRequest.PayMoney = decimalPayMoney;
                                    decimal decimalArrearage = wZRequest.BorrowMoney - wZRequest.BeforePayMoney - wZRequest.PayMoney;
                                    wZRequest.Arrearage = decimalArrearage;
                                    if (wZRequest.Arrearage == 0)
                                    {
                                        wZRequest.Progress = "完成";
                                        wZRequest.IsFinisth = -1;
                                    }

                                    wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);
                                }
                            }
                        }

                        //重新加载预付款列表
                        DataPayBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZXCG + "')", true);
                    }
                }
            }
        }
    }



    protected void BT_Save_Click(object sender, EventArgs e)
    {
        try
        {
            string strPayID = TXT_PayID.Text;

            string strProjectCode = DDL_Project.SelectedValue;
            string strPayName = TXT_PayName.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('项目编码不能为空，请补充！');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }
            if (string.IsNullOrEmpty(strPayName))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('应付款名称不能为空，请补充！');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPayName))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('应付款名称不能为非法字符！');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }


            if (!string.IsNullOrEmpty(strPayID))
            {
                //修改


                WZPayBLL wZPayBLL = new WZPayBLL();
                string strWZPayHQL = "from WZPay as wZPay where PayID = '" + strPayID + "'";
                IList listWZPay = wZPayBLL.GetAllWZPays(strWZPayHQL);
                if (listWZPay != null && listWZPay.Count == 1)
                {
                    WZPay wZPay = (WZPay)listWZPay[0];

                    wZPay.ProjectCode = strProjectCode;
                    wZPay.PayName = strPayName;

                    wZPayBLL.UpdateWZPay(wZPay, wZPay.PayID);

                    //重新加载
                    DataPayBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatusCloseChange();", true);
                }
            }
            else
            {
                //增加


                WZPay wZPay = new WZPay();
                WZPayBLL wZPayBLL = new WZPayBLL();
                //生成预付款ID
                wZPay.PayID = CreateNewPayID();
                wZPay.ProjectCode = strProjectCode;
                wZPay.PayName = strPayName;
                wZPay.PayTime = DateTime.Now;
                wZPay.Marker = strUserCode;
                wZPay.Progress = "录入";

                wZPayBLL.AddWZPay(wZPay);

                //重新加载
                DataPayBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatusCloseChange();", true);
            }
        }
        catch (Exception ex) { }
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        try
        {
            string strPayID = TXT_PayID.Text;

            string strProjectCode = DDL_Project.SelectedValue;
            string strPayName = TXT_PayName.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('项目编码不能为空，请补充！');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }
            if (string.IsNullOrEmpty(strPayName))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('应付款名称不能为空，请补充！');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPayName))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('应付款名称不能为非法字符！');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }


            //增加


            WZPay wZPay = new WZPay();
            WZPayBLL wZPayBLL = new WZPayBLL();
            //生成预付款ID
            wZPay.PayID = CreateNewPayID();
            wZPay.ProjectCode = strProjectCode;
            wZPay.PayName = strPayName;
            wZPay.PayTime = DateTime.Now;
            wZPay.Marker = strUserCode;
            wZPay.Progress = "录入";

            wZPayBLL.AddWZPay(wZPay);

            //重新加载
            DataPayBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('新建成功！');ControlStatusCloseChange();", true);
        }
        catch (Exception ex) { }
    }


    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        try
        {
            string strPayID = TXT_PayID.Text;

            string strProjectCode = DDL_Project.SelectedValue;
            string strPayName = TXT_PayName.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('项目编码不能为空，请补充！');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }
            if (string.IsNullOrEmpty(strPayName))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('应付款名称不能为空，请补充！');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPayName))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('应付款名称不能为非法字符！');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }


            if (!string.IsNullOrEmpty(strPayID))
            {
                //修改
                WZPayBLL wZPayBLL = new WZPayBLL();
                string strWZPayHQL = "from WZPay as wZPay where PayID = '" + strPayID + "'";
                IList listWZPay = wZPayBLL.GetAllWZPays(strWZPayHQL);
                if (listWZPay != null && listWZPay.Count == 1)
                {
                    WZPay wZPay = (WZPay)listWZPay[0];

                    wZPay.ProjectCode = strProjectCode;
                    wZPay.PayName = strPayName;

                    wZPayBLL.UpdateWZPay(wZPay, wZPay.PayID);

                    //重新加载
                    DataPayBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('修改成功！');ControlStatusCloseChange();", true);
                }
            }
            else
            {
                //增加
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择要修改的付款计划列表！');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }
        }
        catch (Exception ex) { }
    }



    protected void BT_Reset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_Pay.Items.Count; i++)
        {
            DG_Pay.Items[i].ForeColor = Color.Black;
        }

        TXT_PayID.Text = "";
        DDL_Project.SelectedValue = "";
        TXT_PayName.Text = "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    protected void DG_Pay_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_Pay.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_PaySql.Text.Trim();
        DataTable dtPay = ShareClass.GetDataSetFromSql(strHQL, "Pay").Tables[0];

        DG_Pay.DataSource = dtPay;
        DG_Pay.DataBind();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    /// <summary>
    ///  生成预付款ID
    /// </summary>
    private string CreateNewPayID()
    {
        string strNewPayID = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                string strPayIDHQL = string.Format("select count(1) as RowNumber from T_WZPay where to_char( PayTime, 'yyyy-mm-dd') like '{0}%'", DateTime.Now.ToString("yyyy-MM"));
                DataTable dtPayID = ShareClass.GetDataSetFromSql(strPayIDHQL, "PayID").Tables[0];
                int intPayIDNumber = int.Parse(dtPayID.Rows[0]["RowNumber"].ToString());
                intPayIDNumber = intPayIDNumber + 1;
                string strYear = DateTime.Now.Year.ToString();
                string strMonth = DateTime.Now.Month.ToString();
                do
                {
                    StringBuilder sbPayID = new StringBuilder();
                    for (int j = 3 - intPayIDNumber.ToString().Length; j > 0; j--)
                    {
                        sbPayID.Append(" ");
                    }
                    if (strMonth.Length == 1)
                    {
                        strMonth = "0" + strMonth;
                    }
                    strNewPayID = strYear + "" + strMonth + "-" + sbPayID.ToString() + intPayIDNumber.ToString();

                    //验证新的预付款ID是否存在
                    string strCheckNewPayIDHQL = "select count(1) as RowNumber from T_WZPay where PayID = '" + strNewPayID + "'";
                    DataTable dtCheckNewPayID = ShareClass.GetDataSetFromSql(strCheckNewPayIDHQL, "CheckNewPayID").Tables[0];
                    int intCheckNewPayID = int.Parse(dtCheckNewPayID.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewPayID == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intPayIDNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewPayID;
    }


    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strEditPayID = HF_NewPayID.Value;
        if (string.IsNullOrEmpty(strEditPayID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDFKLB + "')", true);
            return;
        }

        WZPayBLL wZPayBLL = new WZPayBLL();
        string strWZPayHQL = "from WZPay as wZPay where PayID = '" + strEditPayID + "'";
        IList listWZPay = wZPayBLL.GetAllWZPays(strWZPayHQL);
        if (listWZPay != null && listWZPay.Count == 1)
        {
            WZPay wZPay = (WZPay)listWZPay[0];

            string strNewProgress = HF_NewProgress.Value;
            if (wZPay.IsMark != 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSYBJBW0BYXBJ + "');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            TXT_PayID.Text = wZPay.PayID;
            DDL_Project.SelectedValue = wZPay.ProjectCode;
            TXT_PayName.Text = wZPay.PayName;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
    }


    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //删除
        string strEditPayID = HF_NewPayID.Value;
        if (string.IsNullOrEmpty(strEditPayID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDFKLB + "')", true);
            return;
        }

        WZPayBLL wZPayBLL = new WZPayBLL();
        string strWZPayHQL = "from WZPay as wZPay where PayID = '" + strEditPayID + "'";
        IList listWZPay = wZPayBLL.GetAllWZPays(strWZPayHQL);
        if (listWZPay != null && listWZPay.Count == 1)
        {
            WZPay wZPay = (WZPay)listWZPay[0];
            if (wZPay.Progress != "录入" || wZPay.IsMark != 0)
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWLRYJSYBJBW0BYXSC + "');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            wZPayBLL.DeleteWZPay(wZPay);

            //重新加载列表
            DataPayBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCCG + "');ControlStatusCloseChange();", true);
        }
    }


    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        //明细
        string strEditPayID = HF_NewPayID.Value;
        if (string.IsNullOrEmpty(strEditPayID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDFKLB + "')", true);
            return;
        }
    }



    protected void BT_NewReport_Click(object sender, EventArgs e)
    {
        //报批
        string strEditPayID = HF_NewPayID.Value;
        if (string.IsNullOrEmpty(strEditPayID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDFKLB + "')", true);
            return;
        }

        WZPayBLL wZPayBLL = new WZPayBLL();
        string strWZPayHQL = "from WZPay as wZPay where PayID = '" + strEditPayID + "'";
        IList listWZPay = wZPayBLL.GetAllWZPays(strWZPayHQL);
        if (listWZPay != null && listWZPay.Count == 1)
        {
            WZPay wZPay = (WZPay)listWZPay[0];

            if (wZPay.Progress != "录入")
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBSLRZTBNBP + "');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            wZPay.Progress = "报批";

            wZPayBLL.UpdateWZPay(wZPay, wZPay.PayID);

            //重新加载预付款列表
            DataPayBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBPCG + "');ControlStatusCloseChange();", true);
        }
    }



    protected void BT_NewExecute_Click(object sender, EventArgs e)
    {
        //执行
        string strEditPayID = HF_NewPayID.Value;
        if (string.IsNullOrEmpty(strEditPayID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDFKLB + "')", true);
            return;
        }

        WZPayBLL wZPayBLL = new WZPayBLL();
        string strWZPayHQL = "from WZPay as wZPay where PayID = '" + strEditPayID + "'";
        IList listWZPay = wZPayBLL.GetAllWZPays(strWZPayHQL);
        if (listWZPay != null && listWZPay.Count == 1)
        {
            WZPay wZPay = (WZPay)listWZPay[0];

            if (wZPay.Progress != "批准")
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBSPZZTBNZX + "');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            //检查1
            bool IsCheck1 = false;
            string strCheckHQL1 = string.Format(@"select r.* from T_WZPay p
                            left join T_WZPayDetail d on p.PayID = d.PayID
                            left join T_WZRequest r on d.RequestCode = r.RequestCode
                            where p.PayID = '{0}'", strEditPayID);
            DataTable dtCheck1 = ShareClass.GetDataSetFromSql(strCheckHQL1, "Check1").Tables[0];
            if (dtCheck1 != null && dtCheck1.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCheck1.Rows)
                {
                    if (!"报销".Contains(ShareClass.ObjectToString(dr["Progress"])))
                    {
                        string strNewProgress = HF_NewProgress.Value;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZYKDJDBSBXXCL + "');ControlStatusChange('" + strNewProgress + "');", true);
                        return;
                    }
                    else
                    {
                        IsCheck1 = true;
                    }
                }
            }
            else
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZBDXYDKDJL + "');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            if (IsCheck1)
            {
                wZPay.Progress = "付款";
                wZPayBLL.UpdateWZPay(wZPay, wZPay.PayID);

                WZPayDetailBLL wZPayDetailBLL = new WZPayDetailBLL();
                string strWZPayDetailHQL = string.Format(@"from WZPayDetail as wZPayDetail 
                        where PayID= '{0}'", wZPay.PayID);
                IList listWZPayDetail = wZPayDetailBLL.GetAllWZPayDetails(strWZPayDetailHQL);
                if (listWZPayDetail != null && listWZPayDetail.Count > 0)
                {
                    for (int i = 0; i < listWZPayDetail.Count; i++)
                    {
                        WZPayDetail wZPayDetail = (WZPayDetail)listWZPayDetail[i];

                        //付款明细<预付进度> = "付款"
                        wZPayDetail.PayProcess = "付款";

                        wZPayDetailBLL.UpdateWZPayDetail(wZPayDetail, wZPayDetail.ID);

                        //请款单
                        WZRequestBLL wZRequestBLL = new WZRequestBLL();
                        string strWZRequestHQL = string.Format(@"from WZRequest as wZRequest
                                        where RequestCode = '{0}'", wZPayDetail.RequestCode);
                        IList listRequest = wZRequestBLL.GetAllWZRequests(strWZRequestHQL);
                        if (listRequest != null && listRequest.Count > 0)
                        {
                            WZRequest wZRequest = (WZRequest)listRequest[0];

                            decimal decimalPayMoney = wZRequest.PayMoney + wZPayDetail.PlanMoney;
                            wZRequest.PayMoney = decimalPayMoney;
                            decimal decimalArrearage = wZRequest.BorrowMoney - wZRequest.BeforePayMoney - wZRequest.PayMoney;
                            wZRequest.Arrearage = decimalArrearage;
                            if (wZRequest.Arrearage == 0)
                            {
                                wZRequest.Progress = "完成";
                                wZRequest.IsFinisth = -1;
                            }

                            wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);
                        }
                    }
                }

                //重新加载预付款列表
                DataPayBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZXCG + "');ControlStatusCloseChange();", true);
            }
        }
    }
}