using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZObjectCodeDetail : System.Web.UI.Page
{
    public string strUserCode
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            //���ص�λ�б�
            BindUnitData();

            if (!string.IsNullOrEmpty(Request.QueryString["DLCode"]))
            {
                string strDLCode = Request.QueryString["DLCode"];
                HF_DLCode.Value = strDLCode;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["ZLCode"]))
            {
                string strZLCode = Request.QueryString["ZLCode"];
                HF_ZLCode.Value = strZLCode;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["XLCode"]))
            {
                string strXLCode = Request.QueryString["XLCode"];
                HF_XLCode.Value = strXLCode;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["ObjectCode"]))
            {
                string strExistObjectCode = Request.QueryString["ObjectCode"];

                HF_ObjectCode.Value = strExistObjectCode;

                DataBinder(strExistObjectCode);

            }
            else
            {
                //���ؿؼ�
                SetControlState("other");
            }

            
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        WZObjectBLL wZObjectBLL = new WZObjectBLL();

        string strObjectName = TXT_ObjectName.Text.Trim();
        string strModel = TXT_Model.Text.Trim();
        string strGrade = TXT_Level.Text.Trim();
        string strCriterion = TXT_Standard.Text.Trim();
        int intUnit = 0;
        int.TryParse(DDL_Unit.SelectedValue, out intUnit);
        int intConvertUnit = 0;
        int.TryParse(DDL_ConvertUnit.SelectedValue, out intConvertUnit);
        decimal decimalRatio = 0;
        string strConvertRatio = TXT_ConvertRatio.Text.Trim();
        decimal.TryParse(strConvertRatio, out decimalRatio);

        decimal decimalMarket = 0;
        string strMarket = TXT_Market.Text.Trim();
        decimal.TryParse(strMarket, out decimalMarket);

        string strReferDesc = TXT_ReferDesc.Text.Trim();
        string strReferStandard = TXT_ReferStandard.Text.Trim();

        string strDLCode = HF_DLCode.Value;
        string strZLCode = HF_ZLCode.Value;
        string strXLCode = HF_XLCode.Value;

        //�������ƣ�����ͺţ����𣬱�׼������һ������Ϊ�գ���ʾ���ݿ�ȱ���벹��
        if (string.IsNullOrEmpty(strObjectName))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZWZMCBNWKBC").ToString().Trim()+"')", true);
            return;
        }
        else
        {
            if (strObjectName.Length > 30)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZWZMCBNCG30GZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strObjectName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZWZMCBNBHFFZF").ToString().Trim()+"')", true);
                return;
            }
        }
        if (string.IsNullOrEmpty(strModel))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGGXHBNWKBC").ToString().Trim()+"')", true);
            return;
        }
        if (strModel.Length > 66)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGGXHBNCG66GZF").ToString().Trim()+"')", true);
            return;
        }
        if (string.IsNullOrEmpty(strCriterion))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBZBNWKBC").ToString().Trim()+"')", true);
            return;
        }
        if (strCriterion.Length > 24)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBZBNCG24GZF").ToString().Trim()+"')", true);
            return;
        }
        if (strGrade.Length > 8)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJBBNCG8GZF").ToString().Trim()+"')", true);
            return;
        }
        if (intUnit == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJLDWCWZ").ToString().Trim()+"')", true);
            return;
        }
        if (intConvertUnit == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHSDWCWZ").ToString().Trim()+"')", true);
            return;
        }
        if (string.IsNullOrEmpty(strConvertRatio))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHSXSBNWKBC").ToString().Trim()+"')", true);
            return;
        }
        if (!ShareClass.CheckIsNumber(strConvertRatio))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHSXSBXWXSLXXG").ToString().Trim()+"')", true);
            return;
        }
        if (intUnit == intConvertUnit && decimalRatio != 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHSXSCWXG").ToString().Trim()+"')", true);
            return;
        }
        if (intUnit != intConvertUnit && (decimalRatio == 0 || decimalRatio == 1))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHSGXCWXG").ToString().Trim()+"')", true);
            return;
        }
        if (string.IsNullOrEmpty(strReferDesc))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDZMSBNKBC").ToString().Trim()+"')", true);
            return;
        }
        if (!ShareClass.CheckStringRight(strReferDesc))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDZMSBNBHFFZF").ToString().Trim()+"')", true);
            return;
        }
        if (string.IsNullOrEmpty(strReferStandard))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZDuiZhaoLanguageHandleGetWord").ToString().Trim()+"')", true); 
            return;
        }
        if (!ShareClass.CheckStringRight(strReferStandard))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDZBZBNBHFFZF").ToString().Trim()+"')", true);
            return;
        }

        string strObjectCode = HF_ObjectCode.Value;
        if (!string.IsNullOrEmpty(strObjectCode))
        {
            string strCheckObjectHQL = string.Format(@"select * from T_WZObject
                                where ObjectName = '{0}'
                                and Model = '{1}'
                                and Criterion = '{2}'
                                and Grade = '{3}'
                                and Unit = {4}", strObjectName, strModel, strCriterion, strGrade, intUnit);
            DataTable dtCheckObject = ShareClass.GetDataSetFromSql(strCheckObjectHQL, "strCheckObjectHQL").Tables[0];
            if (dtCheckObject != null && dtCheckObject.Rows.Count > 0)
            {
                if (ShareClass.ObjectToString(dtCheckObject.Rows[0]["ObjectCode"]) != strObjectCode)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJLZFXG").ToString().Trim()+"')", true);
                    return;
                }
            }

            //�޸�
            string strObjectSql = "from WZObject as wZObject where ObjectCode = '" + strObjectCode + "'";
            IList objectList = wZObjectBLL.GetAllWZObjects(strObjectSql);
            if (objectList != null && objectList.Count == 1)
            {
                WZObject wZObject = (WZObject)objectList[0];

                wZObject.DLCode = strDLCode;
                wZObject.ZLCode = strZLCode;
                wZObject.XLCode = strXLCode;
                wZObject.ObjectName = strObjectName;
                wZObject.Criterion = strCriterion;
                wZObject.Grade = strGrade;
                wZObject.Model = strModel;
                wZObject.Unit = intUnit;
                wZObject.ConvertUnit = intConvertUnit;
                wZObject.ConvertRatio = decimalRatio;

                wZObject.Market = decimalMarket;

                wZObject.ReferDesc = strReferDesc;
                wZObject.ReferStandard = strReferStandard;

                wZObjectBLL.UpdateWZObject(wZObject, strObjectCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ɹ���');", true);   //ChineseWord
           }
        }
        else
        {
            string strCheckObjectHQL = string.Format(@"select * from T_WZObject
                                where ObjectName = '{0}'
                                and Model = '{1}'
                                and Criterion = '{2}'
                                and Grade = '{3}'
                                and Unit = {4}", strObjectName, strModel, strCriterion, strGrade, intUnit);
            DataTable dtCheckObject = ShareClass.GetDataSetFromSql(strCheckObjectHQL, "strCheckObjectHQL").Tables[0];
            if (dtCheckObject != null && dtCheckObject.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJLZFXG").ToString().Trim()+"')", true);
                return;
            }

            //����
            WZObject wZObject = new WZObject();
            wZObject.DLCode = strDLCode;
            wZObject.ZLCode = strZLCode;
            wZObject.XLCode = strXLCode;
            wZObject.ObjectCode = CreateObjectCode(strXLCode);
            wZObject.Creater = strUserCode;
            wZObject.ObjectName = strObjectName;
            wZObject.Criterion = strCriterion;
            wZObject.Grade = strGrade;
            wZObject.Model = strModel;
            wZObject.Unit = intUnit;
            wZObject.ConvertUnit = intConvertUnit;
            wZObject.ConvertRatio = decimalRatio;

            wZObject.Market = decimalMarket;

            wZObject.CollectTime = DateTime.Now;

            wZObject.ReferDesc = strReferDesc;
            wZObject.ReferStandard = strReferStandard;

            wZObjectBLL.AddWZObject(wZObject);

            //�޸�С������ʹ�ñ��
            ShareClass.UpdateXLCodeStatus(strXLCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ɹ���');", true);   //ChineseWord
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
    }

    private void DataBinder(string strExistObjectCode)
    {
        WZObjectBLL wZObjectBLL = new WZObjectBLL();

        string strObjectSql = "from WZObject as wZObject where ObjectCode = '" + strExistObjectCode + "'";
        IList objectList = wZObjectBLL.GetAllWZObjects(strObjectSql);
        if (objectList != null && objectList.Count == 1)
        {
            WZObject wZObject = (WZObject)objectList[0];

            LB_ObjectCode.Text = wZObject.ObjectCode;
            TXT_ObjectName.Text = wZObject.ObjectName;
            TXT_Standard.Text = wZObject.Criterion;
            TXT_Level.Text = wZObject.Grade;
            TXT_Model.Text = wZObject.Model;
            DDL_Unit.SelectedValue = wZObject.Unit.ToString();
            DDL_ConvertUnit.SelectedValue = wZObject.ConvertUnit.ToString();
            TXT_ConvertRatio.Text = wZObject.ConvertRatio.ToString();

            TXT_ReferDesc.Text = wZObject.ReferDesc;
            TXT_ReferStandard.Text = wZObject.ReferStandard;

            TXT_Market.Text = wZObject.Market.ToString();

            if (wZObject.IsMark == -1)
            {
                SetControlState("edit");
            }
            else {
                SetControlState("other");
            }
        }
    }


    private void BindUnitData()
    {
        WZSpanBLL wZSpanBLL = new WZSpanBLL();
        string strWZSpan = "from WZSpan as wZSpan";
        IList lstWZSpan = wZSpanBLL.GetAllWZSpans(strWZSpan);

        DDL_Unit.DataSource = lstWZSpan;
        DDL_Unit.DataBind();

        DDL_ConvertUnit.DataSource = lstWZSpan;
        DDL_ConvertUnit.DataBind();

        DDL_ConvertUnit.Items.Insert(0, new ListItem("ѡ��", "0"));   //ChineseWord
    }


    private void SetControlState(string strType)
    {
        if (strType == "edit")
        {
            TXT_ObjectName.ReadOnly = true;            //��������
            TXT_ObjectName.BackColor = Color.White;
            TXT_Level.ReadOnly = true;                 //����
            TXT_Level.BackColor = Color.White;
            TXT_Model.ReadOnly = true;                 //����ͺ�
            TXT_Model.BackColor = Color.White;
            DDL_Unit.Enabled = false;                     //������λ
            DDL_Unit.BackColor = Color.White;
            DDL_ConvertUnit.Enabled = false;             //���㵥λ
            DDL_ConvertUnit.BackColor = Color.White;

            //����Ŀɱ༭
            TXT_Market.ReadOnly = false;                //�г�����
            TXT_Market.BackColor = Color.CornflowerBlue;
            TXT_ConvertRatio.ReadOnly = false;          //����ϵ��
            TXT_ConvertRatio.BackColor = Color.CornflowerBlue;
            TXT_Standard.ReadOnly = false;              //��׼
            TXT_Standard.BackColor = Color.CornflowerBlue;
            TXT_ReferStandard.ReadOnly = false;         //���ձ�׼
            TXT_ReferStandard.BackColor = Color.CornflowerBlue;
            TXT_ReferDesc.ReadOnly = false;             //��������
            TXT_ReferDesc.BackColor = Color.CornflowerBlue;
        }
        else
        {
            //ȫ���ɱ༭
            TXT_ObjectName.ReadOnly = false;            //��������
            TXT_ObjectName.BackColor = Color.CornflowerBlue;
            TXT_Level.ReadOnly = false;                 //����
            TXT_Level.BackColor = Color.CornflowerBlue;
            TXT_Model.ReadOnly = false;                 //����ͺ�
            TXT_Model.BackColor = Color.CornflowerBlue;
            DDL_Unit.Enabled = true;                     //������λ
            DDL_Unit.BackColor = Color.CornflowerBlue;
            DDL_ConvertUnit.Enabled = true;             //���㵥λ
            DDL_ConvertUnit.BackColor = Color.CornflowerBlue;
            TXT_Market.ReadOnly = false;                //�г�����
            TXT_Market.BackColor = Color.CornflowerBlue;
            TXT_ConvertRatio.ReadOnly = false;          //����ϵ��
            TXT_ConvertRatio.BackColor = Color.CornflowerBlue;
            TXT_Standard.ReadOnly = false;              //��׼
            TXT_Standard.BackColor = Color.CornflowerBlue;
            TXT_ReferStandard.ReadOnly = false;         //���ձ�׼
            TXT_ReferStandard.BackColor = Color.CornflowerBlue;
            TXT_ReferDesc.ReadOnly = false;             //��������
            TXT_ReferDesc.BackColor = Color.CornflowerBlue;
        }
    }


    /// <summary>
    ///  �������ʴ��룬����ʱ��Ϊ��ͬһ��С����룬���Կ��������Ĳ���,���������ʱ��С����벻һ��������Ҫ������д����
    /// </summary>
    /// <returns>���ʴ���</returns>
    private string CreateObjectCode(string strXLCode)
    {
        string strNewObjectCode = string.Empty;

        try
        {
            lock (this)
            {
                bool isExist = true;
                string strMinObjectCodeHQL = string.Format("select COUNT(1) as RowNumber from T_WZObject where XLCode = '{0}'", strXLCode);
                DataTable dtMinObjectCode = ShareClass.GetDataSetFromSql(strMinObjectCodeHQL, "strMinObjectCodeHQL").Tables[0];
                int intCount = 0;
                int.TryParse(ShareClass.ObjectToString(dtMinObjectCode.Rows[0]["RowNumber"]), out intCount);
                intCount = intCount + 1;
                do
                {
                    StringBuilder sbObjectCode = new StringBuilder();
                    for (int j = 4 - intCount.ToString().Length; j > 0; j--)
                    {
                        sbObjectCode.Append("0");
                    }
                    strNewObjectCode = strXLCode + sbObjectCode.ToString() + intCount.ToString();

                    //�ж���ȡ�����ʴ����Ƿ��Ѵ���
                    string strIsExistObjectCodeHQL = string.Format("select COUNT(1) as RowNumber from T_WZObject where ObjectCode = '{0}'", strNewObjectCode);
                    DataTable dtIsExistObjectCode = ShareClass.GetDataSetFromSql(strIsExistObjectCodeHQL, "strIsExistObjectCodeHQL").Tables[0];
                    int intIsExistCount = 0;
                    int.TryParse(ShareClass.ObjectToString(dtIsExistObjectCode.Rows[0]["RowNumber"]), out intIsExistCount);
                    if (intIsExistCount == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intCount++;
                    }

                } while (isExist);
            }
        }
        catch (Exception ex)
        { }
        return strNewObjectCode;
    }
}
