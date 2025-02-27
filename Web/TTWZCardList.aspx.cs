using System; using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Drawing;
using System.Data;
using System.Text;

public partial class TTWZCardList : System.Web.UI.Page
{
    string strUserCode;


    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","财务凭证", strUserCode);

        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();
        }
    }

    private void DataBinder()
    {
        string strWZCardHQL = string.Format(@"select c.*,m.UserName as CardMarkerName from T_WZCard c
                        left join T_ProjectMember m on c.CardMarker = m.UserCode 
                        where c.CardMarker = '{0}' 
                        order by c.CardTime desc", strUserCode);
        DataTable dtWZCard = ShareClass.GetDataSetFromSql(strWZCardHQL, "Card").Tables[0];

        DG_List.DataSource = dtWZCard;
        DG_List.DataBind();

        LB_Sql.Text = strWZCardHQL;

        LB_Record.Text = dtWZCard.Rows.Count.ToString();


        //WZCardBLL wZCardBLL = new WZCardBLL();
        
        //string strWZCardHQL = "from WZCard as wZCard where CardMarker = '" + strUserCode + "' order by CardTime desc";
        //IList listWZCard = wZCardBLL.GetAllWZCards(strWZCardHQL);

        //DG_List.DataSource = listWZCard;
        //DG_List.DataBind();

        //LB_Sql.Text = strWZCardHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZCardBLL wZCardBLL = new WZCardBLL();
                string strWZCardHQL = "from WZCard as wZCard where CardCode = '" + cmdArges + "'";
                IList listWZCard = wZCardBLL.GetAllWZCards(strWZCardHQL);
                if (listWZCard != null && listWZCard.Count == 1)
                {
                    WZCard wZCard = (WZCard)listWZCard[0];
                    if (wZCard.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBJBW0BYXSC").ToString().Trim()+"')", true);
                        return;
                    }
                    wZCardBLL.DeleteWZCard(wZCard);

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
                WZCardBLL wZCardBLL = new WZCardBLL();
                string strWZCardHQL = "from WZCard as wZCard where CardCode = '" + cmdArges + "'";
                IList listWZCard = wZCardBLL.GetAllWZCards(strWZCardHQL);
                if (listWZCard != null && listWZCard.Count == 1)
                {
                    WZCard wZCard = (WZCard)listWZCard[0];

                    TXT_CardName.Text = wZCard.CardName;
                    LB_CardCode.Text = wZCard.CardCode;
                }
            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim();
        DataTable dtWZCard = ShareClass.GetDataSetFromSql(strHQL, "Card").Tables[0];

        DG_List.DataSource = dtWZCard;
        DG_List.DataBind();

    }




    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strCardName = TXT_CardName.Text.Trim();
            string strCardCode = LB_CardCode.Text.Trim();
            
            if (string.IsNullOrEmpty(strCardName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPZMCBNWKBC").ToString().Trim()+"')", true);
                return;
            }

            WZCardBLL wZCardBLL = new WZCardBLL();

            if (!string.IsNullOrEmpty(strCardCode))
            {
                //修改
                string strWZCardHQL = "from WZCard as wZCard where CardCode = '" + strCardCode + "'";
                IList listCard = wZCardBLL.GetAllWZCards(strWZCardHQL);
                if (listCard != null && listCard.Count > 0)
                {
                    WZCard wZCard = (WZCard)listCard[0];
                    wZCard.CardName = strCardName;

                    wZCardBLL.UpdateWZCard(wZCard, strCardCode);
                }
            }
            else
            {
                //凭证编号
                string strNewCardCode = CreateNewCardCode();
                

                //增加
                WZCard wZCard = new WZCard();
                wZCard.CardCode = strNewCardCode;
                wZCard.CardName = strCardName;
                wZCard.CardTime = DateTime.Now;
                wZCard.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                wZCard.CardMarker = strUserCode;



                wZCardBLL.AddWZCard(wZCard);
            }

            //重新加载列表
            DataBinder();


            TXT_CardName.Text = "";
            LB_CardCode.Text = "";

            TXT_CardName.BackColor = Color.White;
            LB_CardCode.BackColor = Color.White;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
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

        TXT_CardName.Text = "";
        LB_CardCode.Text = "";

        TXT_CardName.BackColor = Color.White;
        LB_CardCode.BackColor = Color.White;
    }


    protected void BT_Add_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        TXT_CardName.Text = "";
        LB_CardCode.Text = "";

        TXT_CardName.BackColor = Color.CornflowerBlue;
        LB_CardCode.BackColor = Color.White;
    }
    

    //自动生成财务凭证编号
    private string CreateNewCardCode()
    {
        string strNewCardCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                string strCardCodeHQL = "select count(1) as RowNumber from T_WZCard ";
                DataTable dtCardCode = ShareClass.GetDataSetFromSql(strCardCodeHQL, "CardCode").Tables[0];
                int intCardCodeNumber = int.Parse(dtCardCode.Rows[0]["RowNumber"].ToString());
                intCardCodeNumber = intCardCodeNumber + 1;
                do
                {
                    StringBuilder sbCardCode = new StringBuilder();
                    for (int j = 8 - intCardCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbCardCode.Append("0");
                    }
                    strNewCardCode = sbCardCode.ToString() + intCardCodeNumber.ToString();

                    //验证新的移交编号是否存在
                    string strCheckNewCardCodeHQL = "select count(1) as RowNumber from T_WZCard where CardCode = '" + strNewCardCode + "'";
                    DataTable dtCheckNewCardCode = ShareClass.GetDataSetFromSql(strCheckNewCardCodeHQL, "CheckNewCardCode").Tables[0];
                    int intCheckNewCardCode = int.Parse(dtCheckNewCardCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewCardCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intCheckNewCardCode++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }

        return strNewCardCode;
    }
}