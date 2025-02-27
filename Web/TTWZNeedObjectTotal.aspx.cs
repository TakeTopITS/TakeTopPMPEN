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

public partial class TTWZNeedObjectTotal : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();
        }
    }


    private void DataBinder()
    {
        string strNeedObjectHQL = @"select n.*,m.UserName as PurchaseEngineerName from T_WZNeedObject n
                    left join T_ProjectMember m on n.PurchaseEngineer = m.UserCode 
                    order by n.PurchaseEngineer";
        DataTable dtNeedObject = ShareClass.GetDataSetFromSql(strNeedObjectHQL, "NeedObject").Tables[0];

        DG_List.DataSource = dtNeedObject;
        DG_List.DataBind();

        LB_RecordCount.Text = dtNeedObject.Rows.Count.ToString();

        ControlStatusCloseChange();
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

                string strEditID = arrOperate[0];
                string strIsMark = arrOperate[1];
                string strPurchaseEngineer = arrOperate[2];

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strIsMark + "','" + strPurchaseEngineer + "','"+strUserCode+"');", true);
                ControlStatusChange(strIsMark, strPurchaseEngineer, strUserCode);

                HF_NewID.Value = strEditID;
                HF_NewIsMark.Value = strIsMark;
                HF_NewPurchaseEngineer.Value = strPurchaseEngineer;
            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZNeedObjectBLL wZNeedObjectBLL = new WZNeedObjectBLL();
                string strNeedObjectSql = "from WZNeedObject as wZNeedObject where ID = " + cmdArges;
                IList needObjectList = wZNeedObjectBLL.GetAllWZNeedObjects(strNeedObjectSql);
                if (needObjectList != null && needObjectList.Count == 1)
                {
                    WZNeedObject wZNeedObject = (WZNeedObject)needObjectList[0];
                    if (wZNeedObject.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBJBW0BYXSC").ToString().Trim()+"')", true);
                        return;
                    }

                    wZNeedObjectBLL.DeleteWZNeedObject(wZNeedObject);

                    //重新加载列表
                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                }

            }
            else if (cmdName == "edit")
            {
                for (int i = 0; i < DG_List.Items.Count; i++)
                {
                    DG_List.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();

                string strAlertUrl = "TTWZNeedObjectDetail.aspx?id=" + cmdArges;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertNeedObjectPage('" + strAlertUrl + "')", true);
            }
        }
    }


    //加载列表
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }



    protected void BT_NewAdd_Click(object sender, EventArgs e)
    {
        //新增
        //判断当前登录用户＝<采购工程师>，提示“已有记录，不能新增”
        string strCheckNeedSql = string.Format(@"select * from T_WZNeedObject
                    where PurchaseEngineer = '{0}'", strUserCode);
        DataTable dtCheckNeed = ShareClass.GetDataSetFromSql(strCheckNeedSql, "Need").Tables[0];
        if (dtCheckNeed != null && dtCheckNeed.Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYYJLBNXZ").ToString().Trim()+"')", true);
            return;
        }

        if (!string.IsNullOrEmpty(HF_NewID.Value))
        {
            string strNewIsMark = HF_NewIsMark.Value;
            string strNewPurchaseEngineer = HF_NewPurchaseEngineer.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertNeedObjectPage('TTWZNeedObjectDetail.aspx?id=');", true);
        }
        else {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertNeedObjectPage('TTWZNeedObjectDetail.aspx?id=');", true);
        }
    }

    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXFLB").ToString().Trim()+"')", true);
            return;
        }

        string strNewIsMark = HF_NewIsMark.Value;
        string strNewPurchaseEngineer = HF_NewPurchaseEngineer.Value;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertNeedObjectPage('TTWZNeedObjectDetail.aspx?id=" + strEditID + "');", true);
    }



    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //删除
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXFLB").ToString().Trim()+"')", true);
            return;
        }

        string strNewIsMark = HF_NewIsMark.Value;

        WZNeedObjectBLL wZNeedObjectBLL = new WZNeedObjectBLL();
        string strNeedObjectSql = "from WZNeedObject as wZNeedObject where ID = " + strEditID;
        IList needObjectList = wZNeedObjectBLL.GetAllWZNeedObjects(strNeedObjectSql);
        if (needObjectList != null && needObjectList.Count == 1)
        {
            WZNeedObject wZNeedObject = (WZNeedObject)needObjectList[0];
            if (wZNeedObject.IsMark != 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBJBW0BYXSC").ToString().Trim()+"')", true);
                return;
            }

            wZNeedObjectBLL.DeleteWZNeedObject(wZNeedObject);

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
    }




    protected void BT_NewBrowse_Click(object sender, EventArgs e)
    {
        //浏览
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXFLB").ToString().Trim()+"')", true);
            return;
        }

        string strIsMark = HF_NewIsMark.Value;
        string strNewPurchaseEngineer = HF_NewPurchaseEngineer.Value;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertNeedObjectPage('TTWZNeedObjectBrowse.aspx?id=" + strEditID + "');", true);
    }



    
        private void ControlStatusChange(string objIsMark,string objPurchaseEngineer,string objUserCode) {

            BT_NewBrowse.Enabled = true;

            if (objPurchaseEngineer == objUserCode) {
                BT_NewEdit.Enabled = true;
            } else {
                BT_NewEdit.Enabled = false;
            }

            if (objIsMark == "0" && objPurchaseEngineer == objUserCode) {
                BT_NewDelete.Enabled = true;                           //删除
            }
            else {
                BT_NewDelete.Enabled = false;                           //删除
            }
        }



        private void ControlStatusCloseChange() {
            BT_NewEdit.Enabled = false;
            BT_NewDelete.Enabled = false;
            BT_NewBrowse.Enabled = false;
        }





}