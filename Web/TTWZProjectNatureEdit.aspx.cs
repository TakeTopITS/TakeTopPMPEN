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

public partial class TTWZProjectNatureEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string id = Request.QueryString["id"].ToString();
                HF_ID.Value = id;
                int intID = 0;
                int.TryParse(id, out intID);

                BindData(intID);
            }


            TXT_NatureCode.BackColor = Color.CornflowerBlue;
            TXT_NatureDesc.BackColor = Color.CornflowerBlue;
        }
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strNatureCode = TXT_NatureCode.Text.Trim();
            string strNatureDesc = TXT_NatureDesc.Text.Trim();
            string strIsMark = TXT_IsMark.Text.Trim();

            if (string.IsNullOrEmpty(strNatureCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMXZBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strNatureCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMXZBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (strNatureCode.Length > 10)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMXZBNCG2GZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strNatureDesc))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSMHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (strNatureDesc.Length > 200)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSMBNCG10GZF").ToString().Trim()+"')", true);
                return;
            }

            int intIsMark = 0;
            int.TryParse(strIsMark, out intIsMark);

            WZProjectNatureBLL wZProjectNatureBLL = new WZProjectNatureBLL();
            WZProjectNature wZProjectNature = new WZProjectNature();
            wZProjectNature.NatureCode = strNatureCode;
            wZProjectNature.NatureDesc = strNatureDesc;
            wZProjectNature.IsMark = intIsMark;

            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //修改
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                //判断项目属性是否重复
                string strCheckProjectNatureSQL = string.Format(@"select * from T_WZProjectNature
                            where NatureCode = '{0}'", strNatureCode);
                DataTable dtCheckProjectNature = ShareClass.GetDataSetFromSql(strCheckProjectNatureSQL, "ProjectNature").Tables[0];
                if (dtCheckProjectNature != null && dtCheckProjectNature.Rows.Count > 0)
                {
                    string strCheckID = ShareClass.ObjectToString(dtCheckProjectNature.Rows[0]["ID"]);
                    if (strCheckID.Trim() != intID.ToString())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMXZZFXG").ToString().Trim()+"')", true);
                        return;
                    }
                }

                wZProjectNatureBLL.UpdateWZProjectNature(wZProjectNature, intID);
            }
            else
            {
                //增加

                //判断项目性质是否重复
                string strCheckProjectNatureSQL = string.Format(@"select * from T_WZProjectNature
                                where NatureCode ='{0}'", strNatureCode);
                DataTable dtCheckProjectNature = ShareClass.GetDataSetFromSql(strCheckProjectNatureSQL, "ProjectNature").Tables[0];
                if (dtCheckProjectNature != null && dtCheckProjectNature.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMXZZFXG").ToString().Trim()+"')", true);
                    return;
                }

                //判断项目性质说明是否重复
                string strCheckProjectNatureDescSQL = string.Format(@"select * from T_WZProjectNature
                                where NatureDesc ='{0}'", strNatureDesc);
                DataTable dtCheckProjectNatureDesc = ShareClass.GetDataSetFromSql(strCheckProjectNatureDescSQL, "ProjectNatureDesc").Tables[0];
                if (dtCheckProjectNatureDesc != null && dtCheckProjectNatureDesc.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMXZSMZFXG").ToString().Trim()+"')", true);
                    return;
                }

                wZProjectNatureBLL.AddWZProjectNature(wZProjectNature);
            }

            Response.Redirect("TTWZProjectAttributeList.aspx");
            //Response.Redirect("TTWZProjectNatureList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        WZProjectNatureBLL wZProjectNatureBLL = new WZProjectNatureBLL();
        string strWZProjectNatureSql = "from WZProjectNature as wZProjectNature where id = " + id;
        IList listWZProjectNature = wZProjectNatureBLL.GetAllWZProjectNatures(strWZProjectNatureSql);
        if (listWZProjectNature != null && listWZProjectNature.Count > 0)
        {
            WZProjectNature wZProjectNature = (WZProjectNature)listWZProjectNature[0];

            TXT_NatureCode.Text = wZProjectNature.NatureCode;
            TXT_NatureDesc.Text = wZProjectNature.NatureDesc;
            TXT_IsMark.Text = wZProjectNature.IsMark.ToString();
        }
    }
}