using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZCompactEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["compactCode"]))
            {
                string strCompactCode = Request.QueryString["compactCode"].ToString();
                HF_CompactCode.Value = strCompactCode;

                DataBinder(strCompactCode);
            }
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strCompactCode = HF_CompactCode.Value;
            if (!string.IsNullOrEmpty(strCompactCode))
            {
                //�޸�
                string strCompactHQL = "from WZCompact as wZCompact where CompactCode = '" + strCompactCode + "'";
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactHQL);
                if (listCompact != null && listCompact.Count > 0)
                {
                    //�����Ӹ���
                    string InDocument = LanguageHandle.GetWord("ShangChuanDeFuJianLuJing").ToString().Trim();

                    WZCompact wZCompact = (WZCompact)listCompact[0];
                    wZCompact.ProjectCode = TXT_ProjectCode.Text;
                    wZCompact.SupplierCode = TXT_SupplierCode.Text;
                    wZCompact.CompactName = TXT_CompactName.Text;
                    wZCompact.CompactText = TXT_CompactText.Text;
                    wZCompact.SingTime = TXT_SingTime.Text;
                    wZCompact.CompactMoney = Convert.ToDecimal(TXT_CompactMoney.Text);
                    wZCompact.CollectMoney = Convert.ToDecimal(TXT_CollectMoney.Text);
                    wZCompact.RequestMoney = Convert.ToDecimal(TXT_RequestMoney.Text);
                    wZCompact.NotRequestMoney = Convert.ToDecimal(TXT_NotRequestMoney.Text);
                    wZCompact.BeforePayMoney = Convert.ToDecimal(TXT_BeforePayMoney.Text);
                    wZCompact.BeforePayBalance = Convert.ToDecimal(TXT_BeforePayBalance.Text);
                    wZCompact.Compacter = TXT_Compacter.Text;
                    wZCompact.ControlMoney = TXT_ControlMoney.Text;

                    wZCompactBLL.UpdateWZCompact(wZCompact, strCompactCode);
                }
            }
            else
            {
                //����
                //�����Ӹ���
                string InDocument = LanguageHandle.GetWord("ShangChuanDeFuJianLuJing").ToString().Trim();
                WZCompact wZCompact = new WZCompact();
                wZCompact.CompactCode = LanguageHandle.GetWord("ZiDongShengChengGeTongHao01").ToString().Trim(); //TXT_CompactCode.Text.Trim();
                wZCompact.ProjectCode = TXT_ProjectCode.Text;
                wZCompact.SupplierCode = TXT_SupplierCode.Text;
                wZCompact.CompactName = TXT_CompactName.Text;
                wZCompact.CompactText = TXT_CompactText.Text;
                wZCompact.MarkTime = DateTime.Now;//TXT_MarkTime.Text;
                wZCompact.SingTime = TXT_SingTime.Text;
                wZCompact.CompactMoney = Convert.ToDecimal( TXT_CompactMoney.Text);
                wZCompact.CollectMoney = Convert.ToDecimal(TXT_CollectMoney.Text);
                wZCompact.RequestMoney =Convert.ToDecimal( TXT_RequestMoney.Text);
                wZCompact.NotRequestMoney = Convert.ToDecimal(TXT_NotRequestMoney.Text);
                wZCompact.BeforePayMoney = Convert.ToDecimal(TXT_BeforePayMoney.Text);
                wZCompact.BeforePayBalance = Convert.ToDecimal(TXT_BeforePayBalance.Text);
                wZCompact.Compacter = TXT_Compacter.Text;
                wZCompact.ControlMoney = TXT_ControlMoney.Text;
                //wZCompact.Process = "¼��";

                //ʱ����ʱ�ȸ�ֵ����Ȼ�ᱨ�� TODO
                //wZCompact.VerifyTime = DateTime.Now;
                //wZCompact.ApproveTime = DateTime.Now;
                //wZCompact.EffectTime = DateTime.Now;
                //wZCompact.CancelTime = DateTime.Now;

                //���ݹ��̱��룬�ҵ����ɹ�����ʦ������ί�д����ˡ�
                string strProjectHQL = "select * from T_WZProject where ProjectCode = '" + wZCompact.ProjectCode + "'";
                DataTable dtProject = ShareClass.GetDataSetFromSql(strProjectHQL, "strProjectHQL").Tables[0];
                string strPurchaseEngineer = dtProject.Rows.Count == 1 ? dtProject.Rows[0]["PurchaseEngineer"].ToString() : "";
                string strDelegateAgent = dtProject.Rows.Count == 1 ? dtProject.Rows[0]["DelegateAgent"].ToString() : "";
                wZCompact.PurchaseEngineer = strPurchaseEngineer;
                wZCompact.DelegateAgent = strDelegateAgent;
                string strStoreRoom = dtProject.Rows.Count == 1 ? dtProject.Rows[0]["StoreRoom"].ToString() : "";
                //���ݹ�������Ŀ���ҵ��ļ�Ա������Ա
                string strStockHQL = "select * from T_WZStock where StockName = '" + strStoreRoom + "'";
                DataTable dtStock = ShareClass.GetDataSetFromSql(strStockHQL, "strStockHQL").Tables[0];
                string strChecker = dtStock.Rows.Count == 1 ? dtStock.Rows[0]["MaterialCheck"].ToString() : "";
                string strSafekeep = dtStock.Rows.Count == 1 ? dtStock.Rows[0]["Safekeep"].ToString() : "";
                wZCompact.Checker = strChecker;
                wZCompact.Safekeep = strSafekeep;
                //���ݡ��ɹ�����ʦ���ҳ��跽��ţ����˴���
                string strNeedHQL = "select * from T_WZNeedObject where PurchaseEngineer = '" + strPurchaseEngineer + "'";
                DataTable dtNeed = ShareClass.GetDataSetFromSql(strNeedHQL, "strNeedHQL").Tables[0];
                string strNeedCode = dtNeed.Rows.Count ==1 ? dtNeed.Rows[0]["NeedCode"].ToString() : "";
                string strPersonDelegate = dtNeed.Rows.Count ==1 ? dtNeed.Rows[0]["PersonDelegate"].ToString() : "";
                wZCompact.NeedCode = strNeedCode;
                wZCompact.JuridicalPerson = strPersonDelegate;
                wZCompactBLL.AddWZCompact(wZCompact);
                //���跽��ʹ�ñ�Ǹ�Ϊ-1
                string strUpdateNeedHQL = "update T_WZNeedObject set IsMark = -1 where NeedCode = '" + strNeedCode + "'";
                ShareClass.RunSqlCommand(strUpdateNeedHQL);
            }

            Response.Redirect("TTWZCompactList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void DataBinder(string strCompactCode)
    {
        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strWZCompactSql = "from WZCompact as wZCompact where CompactCode = '" + strCompactCode + "'";
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactSql);
        if (listCompact != null && listCompact.Count > 0)
        {
            WZCompact wZCompact = (WZCompact)listCompact[0];
            TXT_CompactCode.Text = wZCompact.CompactCode;
            TXT_ProjectCode.Text = wZCompact.ProjectCode;
            TXT_SupplierCode.Text = wZCompact.SupplierCode;
            TXT_CompactName.Text = wZCompact.CompactName;
            TXT_CompactText.Text = wZCompact.CompactText;
            TXT_MarkTime.Text = wZCompact.MarkTime.ToString();
            TXT_SingTime.Text = wZCompact.SingTime.ToString();
            TXT_CompactMoney.Text = wZCompact.CompactMoney.ToString();
            TXT_CollectMoney.Text = wZCompact.CollectMoney.ToString();
            TXT_RequestMoney.Text = wZCompact.RequestMoney.ToString();
            TXT_NotRequestMoney.Text = wZCompact.NotRequestMoney.ToString();
            TXT_BeforePayMoney.Text = wZCompact.BeforePayMoney.ToString();
            TXT_BeforePayBalance.Text = wZCompact.BeforePayBalance.ToString();
            TXT_Compacter.Text = wZCompact.Compacter;
            TXT_ControlMoney.Text = wZCompact.ControlMoney;
        }
    }
}