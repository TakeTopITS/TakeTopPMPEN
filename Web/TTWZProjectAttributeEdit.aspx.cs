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

public partial class TTWZProjectAttributeEdit : System.Web.UI.Page
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


            TXT_AttributeCode.BackColor = Color.CornflowerBlue;
            TXT_AttributeDesc.BackColor = Color.CornflowerBlue;
        }
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strAttributeCode = TXT_AttributeCode.Text.Trim();
            string strAttributeDesc = TXT_AttributeDesc.Text.Trim();
            string strIsMark = TXT_IsMark.Text.Trim();

            if (string.IsNullOrEmpty(strAttributeCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMSXBNWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strAttributeCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMSXBNWFFZF+"')", true);
                return;
            }
            if (strAttributeCode.Length > 10)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMXZBNCG2GZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strAttributeDesc))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSXSMBNWFFZF+"')", true);
                return;
            }
            if (strAttributeDesc.Length > 200)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXZSMBNCG10GZF+"')", true);
                return;
            }

            int intIsMark = 0;
            int.TryParse(strIsMark, out intIsMark);

            WZProjectAttributeBLL wZProjectAttributeBLL = new WZProjectAttributeBLL();
            WZProjectAttribute wZProjectAttribute = new WZProjectAttribute();
            wZProjectAttribute.AttributeCode = strAttributeCode;
            wZProjectAttribute.AttributeDesc = strAttributeDesc;
            wZProjectAttribute.IsMark = intIsMark;

            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //修改
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                //判断项目属性是否重复
                string strCheckProjectAttributeSQL = string.Format(@"select * from T_WZProjectAttribute
                            where AttributeCode = '{0}'", strAttributeCode);
                DataTable dtCheckProjectAttribute = ShareClass.GetDataSetFromSql(strCheckProjectAttributeSQL, "ProjectAttribute").Tables[0];
                if (dtCheckProjectAttribute != null && dtCheckProjectAttribute.Rows.Count > 0)
                {
                    string strCheckID = ShareClass.ObjectToString(dtCheckProjectAttribute.Rows[0]["ID"]);
                    if (strCheckID.Trim() != intID.ToString())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMSXZFXG+"')", true);
                        return;
                    }
                }


                wZProjectAttributeBLL.UpdateWZProjectAttribute(wZProjectAttribute, intID);
            }
            else
            {
                //增加


                //判断项目属性是否重复
                string strCheckProjectAttributeSQL = string.Format(@"select * from T_WZProjectAttribute
                            where AttributeCode = '{0}'", strAttributeCode);
                DataTable dtCheckProjectAttribute = ShareClass.GetDataSetFromSql(strCheckProjectAttributeSQL, "ProjectAttribute").Tables[0];
                if (dtCheckProjectAttribute != null && dtCheckProjectAttribute.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMSXZFXG+"')", true);
                    return;
                }

                //判断项目属性说明是否重复
                string strCheckProjectAttributeDescSQL = string.Format(@"select * from T_WZProjectAttribute
                            where AttributeDesc = '{0}'", strAttributeDesc);
                DataTable dtCheckProjectAttributeDesc = ShareClass.GetDataSetFromSql(strCheckProjectAttributeDescSQL, "ProjectAttributeDesc").Tables[0];
                if (dtCheckProjectAttributeDesc != null && dtCheckProjectAttributeDesc.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMSXSMZFXG+"')", true);
                    return;
                }

                wZProjectAttributeBLL.AddWZProjectAttribute(wZProjectAttribute);
            }

            Response.Redirect("TTWZProjectAttributeList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        WZProjectAttributeBLL wZProjectAttributeBLL = new WZProjectAttributeBLL();
        string strWZProjectAttributeSql = "from WZProjectAttribute as wZProjectAttribute where id = " + id;
        IList listWZProjectAttribute = wZProjectAttributeBLL.GetAllWZProjectAttributes(strWZProjectAttributeSql);
        if (listWZProjectAttribute != null && listWZProjectAttribute.Count > 0)
        {
            WZProjectAttribute wZProjectAttribute = (WZProjectAttribute)listWZProjectAttribute[0];

            TXT_AttributeCode.Text = wZProjectAttribute.AttributeCode;
            TXT_AttributeDesc.Text = wZProjectAttribute.AttributeDesc;
        }
    }
}