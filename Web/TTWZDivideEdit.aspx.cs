using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZDivideEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["strDivideCode"]))
            {
                string strDivideCode = Request.QueryString["strDivideCode"].ToString();
                HF_DivideCode.Value = strDivideCode;

                BindDivideData(strDivideCode);
            }
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strDivideType = TXT_DivideType.Text.Trim();
            string strDLCode = TXT_DLCode.Text.Trim();
            strDLCode = strDLCode.EndsWith(",") ? strDLCode.TrimEnd(',') : strDLCode;
            if (string.IsNullOrEmpty(strDivideType))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYLBBNWKBC").ToString().Trim()+"')", true);
                return;
            }

            if (string.IsNullOrEmpty(strDLCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDLDMBNWKBC").ToString().Trim()+"')", true);
                return;
            }


            WZDivideBLL wZDivideBLL = new WZDivideBLL();
            WZDivide wZDivide = new WZDivide();
            wZDivide.DivideType = strDivideType;
            wZDivide.DLCode = strDLCode;

            if (!string.IsNullOrEmpty(HF_DivideCode.Value))
            {
                //修改
                string strDivideCode = HF_DivideCode.Value;

                wZDivideBLL.UpdateWZDivide(wZDivide, strDivideCode);
            }
            else
            {
                //验证专业类别是否存在
                string strCheckDivideHQL = string.Format("select count(1) as RowNumber from T_WZDivide where DivideType = '{0}'", strDivideType);
                DataTable dtCheckDivide = ShareClass.GetDataSetFromSql(strCheckDivideHQL, "strCheckDivideHQL").Tables[0];
                int intTypeRowNumber = int.Parse(dtCheckDivide.Rows[0]["RowNumber"].ToString());
                if (intTypeRowNumber > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYLBYCZWZF").ToString().Trim()+"')", true);
                    return;
                }

                //验证涉及大类是否存在
                string[] arrDLCode = strDLCode.Split(',');
                for (int i = 0; i < arrDLCode.Length; i++)
                {
                    string strCheckDLDivideHQL = string.Format("select count(1) as RowNumber from T_WZDivide where DLCode like '%{0}%'", arrDLCode[i]);
                    DataTable dtCheckDLDivide = ShareClass.GetDataSetFromSql(strCheckDLDivideHQL, "strCheckDLDivideHQL").Tables[0];
                    int intDLRowNumber = int.Parse(dtCheckDLDivide.Rows[0]["RowNumber"].ToString());
                    if (intDLRowNumber > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSDLARRDLCODEIYCZWZF").ToString().Trim()+"')", true);
                        return;
                    }
                }


                wZDivide.DivideCode = CreateNewDivideCode();//生成新的专业编号

                //增加
                wZDivideBLL.AddWZDivide(wZDivide);
            }

            Response.Redirect("TTWZDivideList.aspx");
        }
        catch (Exception ex)
        { }
    }

    private void BindDivideData(string strDivideCode)
    {
        WZDivideBLL wZDivideBLL = new WZDivideBLL();
        string strWZDivideSql = "from WZDivide as wZDivide where DivideCode = '" + strDivideCode + "'";
        IList divideList = wZDivideBLL.GetAllWZDivides(strWZDivideSql);
        if (divideList != null && divideList.Count > 0)
        {
            WZDivide wZDivide = (WZDivide)divideList[0];
            TXT_DivideCode.Text = wZDivide.DivideCode;
            TXT_DivideType.Text = wZDivide.DivideType;
            TXT_DLCode.Text = wZDivide.DLCode;
        }
    }


    private string CreateNewDivideCode()
    {
        //生成专业编码
        string strNewDivideCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                int intDivideCodeNumber = 1;
                do
                {
                    StringBuilder sbDivide = new StringBuilder();
                    for (int j = 4 - intDivideCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbDivide.Append("0");
                    }
                    strNewDivideCode = sbDivide.ToString() + intDivideCodeNumber.ToString();

                    //验证专业编码是否存在
                    string strCheckNewDivideCodeHQL = "select count(1) as RowNumber from T_WZDivide where DivideCode = '" + strNewDivideCode + "'";
                    DataTable dtCheckNewDivideCode = ShareClass.GetDataSetFromSql(strCheckNewDivideCodeHQL, "CheckNewDivideCode").Tables[0];
                    int intCheckNewDivideCode = int.Parse(dtCheckNewDivideCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewDivideCode == 0)
                    {
                        isExist = false;
                    }
                    else {
                        intDivideCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }

        return strNewDivideCode;
    }
}