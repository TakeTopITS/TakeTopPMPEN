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

public partial class TTBMSupplierAnaly : System.Web.UI.Page
{
    string strUserCode, strSupplierID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strSupplierID = Request.QueryString["SupplierID"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_StartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndTime.Text = DateTime.Now.AddMonths(6).ToString("yyyy-MM-dd");
            LoadBMSupplierAnalyList(strSupplierID);
        }
    }

    /// <summary>
    /// 取得供应商编码
    /// </summary>
    /// <param name="strID"></param>
    /// <returns></returns>
    protected string GetBMSupplierInfoCode(string strID)
    {
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        string strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + strID + "' ";
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);

        BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];

        return bMSupplierInfo.Code.Trim();
    }

    protected void LoadBMSupplierAnalyList(string strSupplierID)
    {
        string strSupplierCode = GetBMSupplierInfoCode(strSupplierID);
        string strHQL = "Select * From T_BMSupplierAnaly Where SupplierCode='" + strSupplierCode + "' ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and Remark like '%" + TextBox1.Text.Trim() + "%' ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierAnaly");

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
        BMSupplierAnalyBLL bMSupplierAnalyBLL = new BMSupplierAnalyBLL();
        BMSupplierAnaly bMSupplierAnaly = new BMSupplierAnaly();
        bMSupplierAnaly.CreaterCode = strUserCode.Trim();
        bMSupplierAnaly.CreaterName = ShareClass.GetUserName(strUserCode.Trim());
        bMSupplierAnaly.CreateTime = DateTime.Now;
        bMSupplierAnaly.Point = NB_Point.Amount;
        bMSupplierAnaly.Remark = TB_Remark.Text.Trim();
        bMSupplierAnaly.SupplierCode = GetBMSupplierInfoCode(strSupplierID);
        bMSupplierAnaly.BasePoint = NB_BasePoint.Amount;
        bMSupplierAnaly.EndTime = DateTime.Parse(string.IsNullOrEmpty(DLC_EndTime.Text) ? DateTime.Now.ToString() : DLC_EndTime.Text.Trim());
        bMSupplierAnaly.EvaluateProject = TB_EvaluateProject.Text.Trim();
        bMSupplierAnaly.StartTime = DateTime.Parse(string.IsNullOrEmpty(DLC_StartTime.Text) ? DateTime.Now.ToString() : DLC_StartTime.Text.Trim());

        if (bMSupplierAnaly.Point > bMSupplierAnaly.BasePoint)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSKHFBNCGBZFJC").ToString().Trim() + "')", true);
            NB_BasePoint.Focus();
            NB_Point.Focus();
            return;
        }

        try
        {
            bMSupplierAnalyBLL.AddBMSupplierAnaly(bMSupplierAnaly);

            LB_ID.Text = GetMaxBMSupplierAnalyID(bMSupplierAnaly).ToString();

            LoadBMSupplierAnalyList(strSupplierID);
            UpdateSupplierPoint(strSupplierID, 0, bMSupplierAnaly.Point);

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    /// <summary>
    /// 新增时，获取表T_BMSupplierAnaly中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxBMSupplierAnalyID(BMSupplierAnaly bmbf)
    {
        string strHQL = "Select ID From T_BMSupplierAnaly where SupplierCode='" + bmbf.SupplierCode + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierAnaly").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected void Update()
    {
        string strHQL;
        IList lst;
        BMSupplierAnalyBLL bMSupplierAnalyBLL = new BMSupplierAnalyBLL();
        strHQL = "from BMSupplierAnaly as bMSupplierAnaly where bMSupplierAnaly.ID = '" + LB_ID.Text.Trim() + "' ";
        lst = bMSupplierAnalyBLL.GetAllBMSupplierAnalys(strHQL);
        BMSupplierAnaly bMSupplierAnaly = (BMSupplierAnaly)lst[0];
        decimal pointold = bMSupplierAnaly.Point;
        bMSupplierAnaly.CreaterCode = strUserCode.Trim();
        bMSupplierAnaly.CreaterName = ShareClass.GetUserName(strUserCode.Trim());
        bMSupplierAnaly.Point = NB_Point.Amount;
        bMSupplierAnaly.Remark = TB_Remark.Text.Trim();
        bMSupplierAnaly.BasePoint = NB_BasePoint.Amount;
        bMSupplierAnaly.EndTime = DateTime.Parse(string.IsNullOrEmpty(DLC_EndTime.Text) ? DateTime.Now.ToString() : DLC_EndTime.Text.Trim());
        bMSupplierAnaly.EvaluateProject = TB_EvaluateProject.Text.Trim();
        bMSupplierAnaly.StartTime = DateTime.Parse(string.IsNullOrEmpty(DLC_StartTime.Text) ? DateTime.Now.ToString() : DLC_StartTime.Text.Trim());

        if (bMSupplierAnaly.Point > bMSupplierAnaly.BasePoint)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSKHFBNCGBZFJC").ToString().Trim() + "')", true);
            NB_BasePoint.Focus();
            NB_Point.Focus();
            return;
        }

        try
        {
            bMSupplierAnalyBLL.UpdateBMSupplierAnaly(bMSupplierAnaly, bMSupplierAnaly.ID);

            LoadBMSupplierAnalyList(strSupplierID);

            UpdateSupplierPoint(strSupplierID, pointold, bMSupplierAnaly.Point);

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSB").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void Delete()
    {
        string strHQL;
        IList lst;
        BMSupplierAnalyBLL bMSupplierAnalyBLL = new BMSupplierAnalyBLL();
        strHQL = "from BMSupplierAnaly as bMSupplierAnaly where bMSupplierAnaly.ID = '" + LB_ID.Text.Trim() + "' ";
        lst = bMSupplierAnalyBLL.GetAllBMSupplierAnalys(strHQL);
        BMSupplierAnaly bMSupplierAnaly = (BMSupplierAnaly)lst[0];
        decimal pointold = bMSupplierAnaly.Point;

        strHQL = "delete from T_BMSupplierAnaly where ID = '" + LB_ID.Text.Trim() + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadBMSupplierAnalyList(strSupplierID);

            UpdateSupplierPoint(strSupplierID, pointold, 0);

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSB").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMSupplierAnalyList(strSupplierID);
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

                strHQL = "from BMSupplierAnaly as bMSupplierAnaly where bMSupplierAnaly.ID = '" + strID + "' ";

                BMSupplierAnalyBLL bMSupplierAnalyBLL = new BMSupplierAnalyBLL();
                lst = bMSupplierAnalyBLL.GetAllBMSupplierAnalys(strHQL);
                BMSupplierAnaly bMSupplierAnaly = (BMSupplierAnaly)lst[0];

                LB_ID.Text = bMSupplierAnaly.ID.ToString();
                TB_Remark.Text = bMSupplierAnaly.Remark.Trim();
                NB_Point.Amount = bMSupplierAnaly.Point;
                NB_BasePoint.Amount = bMSupplierAnaly.BasePoint;
                TB_EvaluateProject.Text = bMSupplierAnaly.EvaluateProject.Trim();
                DLC_EndTime.Text = string.IsNullOrEmpty(bMSupplierAnaly.EndTime.ToString()) ? DateTime.Now.ToString("yyyy-MM-dd") : bMSupplierAnaly.EndTime.ToString("yyyy-MM-dd");
                DLC_StartTime.Text = string.IsNullOrEmpty(bMSupplierAnaly.StartTime.ToString()) ? DateTime.Now.ToString("yyyy-MM-dd") : bMSupplierAnaly.StartTime.ToString("yyyy-MM-dd");

                //if (bMSupplierAnaly.CreaterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Update.Visible = true;
                //    BT_Delete.Visible = true;
                //    BT_Update.Enabled = true;
                //    BT_Delete.Enabled = true;
                //}
                //else
                //{
                //    BT_Update.Visible = false;
                //    BT_Delete.Visible = false;
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
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = lbl_sql.Text.Trim();

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierAnaly");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    /// <summary>
    /// 新增或更新评价分析时，同步更新承包商评分信息
    /// </summary>
    /// <param name="strID">承包商ID</param>
    /// <param name="strPointOld">原来的评分 新增时为0，更新时为更新前的值</param>
    /// <param name="strPointNew">所录入的评分</param>
    protected void UpdateSupplierPoint(string strID, decimal strPointOld, decimal strPointNew)
    {
        string strHQL = " from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + strID + "' ";
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        if (lst != null && lst.Count > 0)
        {
            BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
            bMSupplierInfo.Point = bMSupplierInfo.Point - strPointOld + strPointNew;
            bMSupplierInfoBLL.UpdateBMSupplierInfo(bMSupplierInfo, bMSupplierInfo.ID);
        }
    }
}