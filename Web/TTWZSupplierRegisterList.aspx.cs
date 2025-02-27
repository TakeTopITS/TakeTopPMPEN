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

public partial class TTWZSupplierRegisterList : System.Web.UI.Page
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
        string strSupplierHQL = @"select s.*,m.UserName as AuditorName,p.UserName as QualityEngineerName
                                                from T_WZSupplierRegister s
                                                left join T_ProjectMember m on s.Auditor = m.UserCode 
                                                left join T_ProjectMember p on s.QualityEngineer = p.UserCode";

        DataTable dtSupplier = ShareClass.GetDataSetFromSql(strSupplierHQL, "Supplier").Tables[0];

        DG_List.DataSource = dtSupplier;
        DG_List.DataBind();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "submit")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZSupplierRegisterBLL wZSupplierRegisterBLL = new WZSupplierRegisterBLL();
                string strWZSupplierRegisterSql = "from WZSupplierRegister as wZSupplierRegister where id = " + cmdArges;
                IList supplierList = wZSupplierRegisterBLL.GetAllWZSupplierRegisters(strWZSupplierRegisterSql);
                if (supplierList != null && supplierList.Count == 1)
                {
                    WZSupplierRegister wZSupplierRegister = (WZSupplierRegister)supplierList[0];

                    wZSupplierRegister.Progress = LanguageHandle.GetWord("DiJiao").ToString().Trim();

                    wZSupplierRegisterBLL.UpdateWZSupplierRegister(wZSupplierRegister, wZSupplierRegister.ID);

                    //重新加载列表
                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJCG").ToString().Trim()+"')", true);
                }

            }
            else if (cmdName == "submitReturn")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZSupplierRegisterBLL wZSupplierRegisterBLL = new WZSupplierRegisterBLL();
                string strWZSupplierRegisterSql = "from WZSupplierRegister as wZSupplierRegister where id = " + cmdArges;
                IList supplierList = wZSupplierRegisterBLL.GetAllWZSupplierRegisters(strWZSupplierRegisterSql);
                if (supplierList != null && supplierList.Count == 1)
                {
                    WZSupplierRegister wZSupplierRegister = (WZSupplierRegister)supplierList[0];

                    wZSupplierRegister.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();

                    wZSupplierRegisterBLL.UpdateWZSupplierRegister(wZSupplierRegister, wZSupplierRegister.ID);

                    //重新加载列表
                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJTHCG").ToString().Trim()+"')", true);
                }

            }
        }
    }



}