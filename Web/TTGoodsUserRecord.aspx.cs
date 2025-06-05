using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTGoodsUserRecord : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strHQL;
        IList lst;

        string strGoodsID = Request.QueryString["ID"];

        //this.Title = "物料编号：" + strGoodsID + "用户记录！";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_BeginUseTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndUseTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);

            string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            LB_GoodsID.Text = strGoodsID;

            strHQL = "from ChangeType as changeType Order by changeType.SortNumber ASC";
            ChangeTypeBLL changeTypeBLL = new ChangeTypeBLL();
            lst = changeTypeBLL.GetAllChangeTypes(strHQL);

            DL_Type.DataSource = lst;
            DL_Type.DataBind();

            LoadWareHouseListByAuthority(strUserCode);

            strHQL = "from GoodsUserRecord as goodsUserRecord where goodsUserRecord.GoodsID = " + strGoodsID;
            GoodsUserRecordBLL goodsUserRecordBLL = new GoodsUserRecordBLL();
            lst = goodsUserRecordBLL.GetAllGoodsUserRecords(strHQL);

            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql1.Text = strHQL;

            GoodsBLL goodsBLL = new GoodsBLL();
            Goods goods = new Goods();
            strHQL = "from Goods as goods where goods.ID =" + strGoodsID;
            lst = goodsBLL.GetAllGoodss(strHQL);
            goods = (Goods)lst[0];
            if (goods.Number == 0)
            {
                BT_New.Enabled = false;
            }

            LB_GoodsCode.Text = goods.GoodsCode.Trim();
        }
    }

    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;
        string strHQL;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode= " + "'" + strDepartCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popwindowDepartment') ", true);
    }

    protected void BT_Select_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popwindowDepartment') ", true);

    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strGoodsID, strGoodsCode, strID, strType, strPosition;
        string strBeginUseTime, strEndUseTime;
        string strUserCode, strHQL;
        decimal deNumber1, deNumber2;


        IList lst;

        GoodsBLL goodsBLL = new GoodsBLL();
        Goods goods = new Goods();

        strGoodsID = LB_GoodsID.Text.Trim();

        GoodsUserRecordBLL goodsUserRecordBLL = new GoodsUserRecordBLL();
        GoodsUserRecord goodsUserRecord = new GoodsUserRecord();

        strGoodsCode = LB_GoodsCode.Text.Trim();
        strUserCode = TB_UserCode.Text.Trim().ToUpper();
        strType = DL_Type.SelectedValue;
        deNumber1 = NB_Number.Amount;
        strBeginUseTime = DLC_BeginUseTime.Text;
        strEndUseTime = DLC_EndUseTime.Text;
        strPosition = DL_WareHouse.SelectedValue.Trim();

        strHQL = "from Goods as goods where goods.ID =" + strGoodsID;
        lst = goodsBLL.GetAllGoodss(strHQL);
        goods = (Goods)lst[0];
        deNumber2 = goods.Number;

        if (deNumber1 > deNumber2 | deNumber2 == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCCJLSLWLHSLBNDYCLPKCSLJC") + "')", true);
            return;
        }

        if (strGoodsCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYHBNWKJC") + "')", true);
        }
        else
        {
            goodsUserRecord.GoodsID = int.Parse(strGoodsID);
            goodsUserRecord.UserCode = strUserCode;
            goodsUserRecord.UserName = ShareClass.GetUserName(strUserCode);
            goodsUserRecord.GoodsCode = strGoodsCode;
            goodsUserRecord.Type = strType;
            goodsUserRecord.Number = deNumber1;
            goodsUserRecord.BeginUseTime = DateTime.Parse(strBeginUseTime);
            goodsUserRecord.EndUseTime = DateTime.Parse(strEndUseTime);
            goodsUserRecord.Position = strPosition;

            try
            {
                goodsUserRecordBLL.AddGoodsUserRecord(goodsUserRecord);
                strID = ShareClass.GetMyCreatedMaxGoodsuserRecordID(strGoodsCode);
                LB_ID.Text = strID;

                strHQL = "from Goods as goods where goods.ID =" + strGoodsID;
                lst = goodsBLL.GetAllGoodss(strHQL);
                goods = (Goods)lst[0];

                deNumber2 = goods.Number;
                goods.Number = deNumber2 - deNumber1;
                goodsBLL.UpdateGoods(goods, int.Parse(strGoodsID));

                //strHQL = "from Goods as goods where goods.GoodsCode = " + "'" + strGoodsCode + "'" + " and goods.OwnerCode = " + "'" + strUserCode + "'";
                //lst = goodsBLL.GetAllGoodss(strHQL);
                //if (lst.Count > 0)
                //{
                //    goods = (Goods)lst[0];
                //    intID = goods.ID;
                //    deNumber3 = goods.Number;
                //    goods.Number = deNumber1 + deNumber3;

                //    goodsBLL.UpdateGoods(goods, intID);
                //}
                //else
                //{
                goods.Number = deNumber1;
                goods.OwnerCode = strUserCode;
                goods.OwnerName = ShareClass.GetUserName(strUserCode);
                goods.Position = strPosition;
                goodsBLL.AddGoods(goods);
                //}

                LoadGoodsUserRecord();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTIAOBOCHENGGONG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTIAOBOSHIBAI") + "')", true);
            }
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Gray;
            }

            e.Item.ForeColor = Color.Red;

            string strHQL = "from GoodsUserRecord as goodsUserRecord where goodsUserRecord.ID= " + strID;

            GoodsUserRecordBLL goodsUserRecordBLL = new GoodsUserRecordBLL();

            IList lst = goodsUserRecordBLL.GetAllGoodsUserRecords(strHQL);

            GoodsUserRecord goodsUserRecord = (GoodsUserRecord)lst[0];

            LB_ID.Text = goodsUserRecord.ID.ToString();
            TB_UserCode.Text = goodsUserRecord.UserCode.ToString();
            LB_UserName.Text = goodsUserRecord.UserName.Trim();
            DL_Type.SelectedValue = goodsUserRecord.Type;
            DLC_BeginUseTime.Text = goodsUserRecord.BeginUseTime.ToString("yyyy-MM-dd");
            DLC_EndUseTime.Text = goodsUserRecord.EndUseTime.ToString("yyyy-MM-dd");
            NB_Number.Amount = goodsUserRecord.Number;
            DL_WareHouse.SelectedValue = goodsUserRecord.Position.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql1.Text;

        GoodsUserRecordBLL goodsUserRecordBLL = new GoodsUserRecordBLL();
        IList lst = goodsUserRecordBLL.GetAllGoodsUserRecords(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        TB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }

    protected void LoadGoodsUserRecord()
    {
        string strGoodsID = LB_GoodsID.Text;
        string strHQL = "from GoodsUserRecord as goodsUserRecord where goodsUserRecord.GoodsID = " + strGoodsID;

        GoodsUserRecordBLL goodsUserRecordBLL = new GoodsUserRecordBLL();
        IList lst = goodsUserRecordBLL.GetAllGoodsUserRecords(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql1.Text = strHQL;
    }

    protected void LoadWareHouseListByAuthority(string strUserCode)
    {
        string strHQL;
        string strDepartCode, strDepartString;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strHQL = " Select WHName From T_WareHouse Where ";
        strHQL += " BelongDepartCode in " + strDepartString;
        strHQL += " Order By SortNumber DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WareHouse");

        DL_WareHouse.DataSource = ds;
        DL_WareHouse.DataBind();
    }
}
