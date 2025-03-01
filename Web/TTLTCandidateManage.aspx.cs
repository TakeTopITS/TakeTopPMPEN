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

public partial class TTLTCandidateManage : System.Web.UI.Page
{
    string strCurrentUserType;

    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //Taketop Software 2006－2012

        string strHQL;

        string strUserCode, strUserName, strDepartString;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);
        strCurrentUserType = ShareClass.GetUserType(strUserCode);

        //LTCandidateInformationBLL ltCandidateInformationBLL = new LTCandidateInformationBLL();
        //bool blVisible = ltCandidateInformationBLL.GetAuthobility("HumanResourcesManagement", strUserCode);
        //if (blVisible == false)
        //{
        //    Response.Redirect("TTDisplayErrors.aspx");
        //    return;
        //}


        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LB_LTCandidateInformationOwner.Text = "";

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            strHQL = "Select * From T_LTCandidateInformation Where BelongDepartCode in " + strDepartString;
            strHQL += " Order By CreateTime ASC";
            DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_LTCandidateInformation");
            DataGrid1.DataSource = ds1;
            DataGrid1.DataBind();

            LB_UserNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + " : " + ds1.Tables[0].Rows.Count.ToString();
            LB_Sql.Text = strHQL;
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDepartCode, strDepartName;
        int intCount;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            strHQL = "Select * From T_LTCandidateInformation Where BelongDepartCode = " + "'" + strDepartCode + "'";
            strHQL += " Order By CreateTime ASC";
            DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_LTCandidateInformation");
            DataGrid1.DataSource = ds1;
            DataGrid1.DataBind();

            intCount = ds1.Tables[0].Rows.Count;

            LB_LTCandidateInformationOwner.Text = strDepartName + " ：";
            LB_UserNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + " : " + intCount.ToString();

            LB_DepartCode.Text = strDepartCode;
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
      

        LB_LTCandidateInformationOwner.Text = "";

        string strUserName = "%" + TB_UserName.Text.Trim() + "%";
        string strDepartString = LB_DepartString.Text.Trim();
        string strStatus = "%" + DL_Status.SelectedValue + "%";
        string strCompany = "%" + TB_Company.Text.Trim() + "%";
        string strCurrentDuty = "%" + TB_CurrentDuty.Text.Trim() + "%";
        string strBriefKeyWord = "%" + TB_BriefKeyWord.Text.Trim() + "%";


        strHQL = "Select * from T_LTCandidateInformation where ";
        strHQL += " UserName Like " + "'" + strUserName + "'";
        strHQL += " and Status Like " + "'" + strStatus + "'";
        strHQL += " and Company Like " + "'" + strCompany + "'";
        strHQL += " and CurrentDuty Like " + "'" + strCurrentDuty + "'";
        strHQL += " and Brief Like " + "'" + strBriefKeyWord + "'";
        strHQL += " and BelongDepartCode in " + strDepartString;
        strHQL += " Order by CreateTime ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_LTCandidateInformation");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_UserNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + " : " + ds.Tables[0].Rows.Count.ToString();
        LB_Sql.Text = strHQL;

        LB_DepartCode.Text = "";
    }


    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_LTCandidateInformation");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void BT_ExportToExcel_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = LanguageHandle.GetWord("HouShuaRenXinXi").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                string strDepartCode = LB_DepartCode.Text.Trim();

                CreateExcel(strDepartCode, fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim() + "')", true);
            }
        }
    }

    private void CreateExcel(string strDepartCode, string fileName)
    {
        string strHQL;
   

        if (strDepartCode == "")//所有成员的情况
        {
            string strUserName = "%" + TB_UserName.Text.Trim() + "%";
            string strDepartString = LB_DepartString.Text.Trim();
            string strStatus = "%" + DL_Status.SelectedValue + "%";
            string strCompany = "%" + TB_Company.Text.Trim() + "%";
            string strCurrentDuty = "%" + TB_CurrentDuty.Text.Trim() + "%";
            string strBriefKeyWord = "%" + TB_BriefKeyWord.Text.Trim() + "%";


            strHQL = string.Format(@"Select UserName {0},Gender {1},Age {2},Company {3},Department {4},
       CurrentDuty {5}, MobilePhone {6},CreateTime {7}, Status {8},BelongDepartCode {9}, BelongDepartName {10}
           From T_LTCandidateInformation Where BelongDepartCode in '{11}",
              LanguageHandle.GetWord("XingMing"),
              LanguageHandle.GetWord("XingBie"),
              LanguageHandle.GetWord("NianLing"),
              LanguageHandle.GetWord("GongSi"),
              LanguageHandle.GetWord("BuMen"),
              LanguageHandle.GetWord("ZhiZe"),
              LanguageHandle.GetWord("YiDongDianHua"),
              LanguageHandle.GetWord("ChuangJianRiQi"),
              LanguageHandle.GetWord("ZhuangTai"),
              LanguageHandle.GetWord("GuiShuBuMenDaiMa"),
              LanguageHandle.GetWord("GuiShuBuMenMingCheng"),
              strDepartString);
            strHQL += " and UserName Like " + "'" + strUserName + "'";
            strHQL += " and Status Like " + "'" + strStatus + "'";
            strHQL += " and Company Like " + "'" + strCompany + "'";
            strHQL += " and CurrentDuty Like " + "'" + strCurrentDuty + "'";
            strHQL += " and Brief Like " + "'" + strBriefKeyWord + "'";
            strHQL += " Order by CreateTime ASC";
        }
        else//按组织架构查询的
        {
            strHQL = string.Format(@"Select UserName {0},Gender {1},Age {2},Company {3},Department {4},
    CurrentDuty {5}, MobilePhone {6},CreateTime {7}, Status {8},BelongDepartCode {9}, BelongDepartName {10}
        From T_LTCandidateInformation Where DepartCode = ‘{11}'",
            LanguageHandle.GetWord("XingMing"),
            LanguageHandle.GetWord("XingBie"),
            LanguageHandle.GetWord("NianLing"),
            LanguageHandle.GetWord("GongSi"),
            LanguageHandle.GetWord("BuMen"),
            LanguageHandle.GetWord("ZhiZe"),
            LanguageHandle.GetWord("YiDongDianHua"),
            LanguageHandle.GetWord("ChuangJianRiQi"),
            LanguageHandle.GetWord("ZhuangTai"),
            LanguageHandle.GetWord("GuiShuBuMenDaiMa"),
            LanguageHandle.GetWord("GuiShuBuMenMingCheng"),
            strDepartCode);
            strHQL += " Order by CreateTime ASC";
        }

        MSExcelHandler.DataTableToExcel(strHQL, fileName);   
    }

}
