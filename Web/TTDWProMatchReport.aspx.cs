using System; using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;

public partial class TTDWProMatchReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
         string strUserCode = Session["UserCode"].ToString();if (!IsPostBack)
        {
            DataProductTypeBinder();

            DataMatchTypeBinder();

            //DataProductMatchBinder();
        }
    }

    private void DataProductTypeBinder()
    {
        DWProductTypeBLL dWProductTypeBLL = new DWProductTypeBLL();
        string strDWProductTypeHQL = "from DWProductType as dWProductType order by dWProductType.ID desc";
        IList listDWProductType = dWProductTypeBLL.GetAllDWProductTypes(strDWProductTypeHQL);

        DDL_Type.DataSource = listDWProductType;
        DDL_Type.DataBind();

        if (listDWProductType.Count > 0)
        {
            DDL_Type.Items.Insert(listDWProductType.Count, new ListItem("ȫ��", "0"));
        }
        else
        {
            DDL_Type.Items.Insert(0, new ListItem("ȫ��", "0"));
        }
    }

    private void DataMatchTypeBinder()
    {
        DWMatchTypeBLL dWMatchTypeBLL = new DWMatchTypeBLL();
        string strDWMatchTypeHQL = "from DWMatchType as dWMatchType order by dWMatchType.ID desc";
        IList listDWMatchType = dWMatchTypeBLL.GetAllDWMatchTypes(strDWMatchTypeHQL);

        DDL_MatchType.DataSource = listDWMatchType;
        DDL_MatchType.DataBind();

        if (listDWMatchType.Count > 0)
        {
            DDL_MatchType.Items.Insert(listDWMatchType.Count, new ListItem("ȫ��", "0"));
        }
        else
        {
            DDL_MatchType.Items.Insert(0, new ListItem("ȫ��", "0"));
        }
    }

    private void DataProductMatchBinder()
    {
        try
        {
            string strMatchName = TXT_MatchName.Text.Trim();
            string strProductType = DDL_Type.SelectedValue;
            string strProductCode = TXT_ProductCode.Text.Trim();
            string strProductName = TXT_ProductName.Text.Trim();
            string strMatchType = DDL_MatchType.SelectedValue;

            StringBuilder strDWMatchHQL = new StringBuilder();
            strDWMatchHQL.Append(" select * from T_DWMatch as dWMatch where 1=1 ");
            if (!string.IsNullOrEmpty(strMatchName))
            {
                strDWMatchHQL.AppendFormat(" and MatchName like '%{0}%'", strMatchName);
            }
            if (!string.IsNullOrEmpty(strMatchType) && strMatchType != "0")
            {
                strDWMatchHQL.AppendFormat(" and MatchType = {0}", strMatchType);
            }
            strDWMatchHQL.Append(" order by dWMatch.MatchName asc;");


            StringBuilder strDWProductHQL = new StringBuilder();
            strDWProductHQL.Append(" select * from T_DWProduct as dWProduct where 1=1 ");
            if (!string.IsNullOrEmpty(strProductType) && strProductType != "0")
            {
                strDWProductHQL.AppendFormat(" and TypeID = {0}", strProductType);
            }
            if (!string.IsNullOrEmpty(strProductCode))
            {
                strDWProductHQL.AppendFormat(" and ProductCode like '%{0}%'", strProductCode);
            }
            if (!string.IsNullOrEmpty(strProductName))
            {
                strDWProductHQL.AppendFormat(" and ProductName like '%{0}%'", strProductName);
            }
            strDWProductHQL.Append(" order by dWProduct.ProductCode desc;");

            string strProMatchHQL = " select * from T_DWProMatch";

            StringBuilder strTotalHQL = new StringBuilder();
            strTotalHQL.Append(strDWMatchHQL.ToString() + strDWProductHQL.ToString() + strProMatchHQL);

            DataSet dsTotal = ShareClass.GetDataSetFromSql(strTotalHQL.ToString(), "Total");

            DataTable dtMatch = dsTotal.Tables[0];
            DataTable dtProduct = dsTotal.Tables[1];
            DataTable dtProductMatch = dsTotal.Tables[2];

            StringBuilder sbHeader = new StringBuilder();
            StringBuilder sbHeader2 = new StringBuilder();
            StringBuilder sbLeft = new StringBuilder();
            StringBuilder sbLeftFooter = new StringBuilder();
            StringBuilder sbContent = new StringBuilder();
            StringBuilder sbFooter1 = new StringBuilder();          //������
            StringBuilder sbFooter2 = new StringBuilder();          //��ɫ�ɱ�
            StringBuilder sbFooter3 = new StringBuilder();          //��ɫ�ϳɱ�
            StringBuilder sbFooter4 = new StringBuilder();          //�䷽��
            //��ͷ����
            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                foreach (DataRow drProduct in dtProduct.Rows)
                {
                    string strProductProductCode = ShareClass.ObjectToString(drProduct["ProductCode"]);
                    string strProductProductName = ShareClass.ObjectToString(drProduct["ProductName"]);

                    //sbHeader.AppendFormat("<td class=\"formItemBgStyle\" width=\"180\">{0}-{1}</td>", dWProduct.ProductCode, dWProduct.ProductName);
                    sbHeader.AppendFormat("<td class=\"formItemBgStyle\" style=\"height:60px; width:180px;\">{0}</td>", strProductProductCode);
                    sbHeader2.AppendFormat("<td class=\"formItemBgStyle\" style=\"height:60px; width:180px;\">{0}</td>", strProductProductName);
                }
                HF_ProductCount.Value = dtProduct.Rows.Count.ToString();
            }
            else
            {
                sbHeader.AppendFormat("<td class=\"formItemBgStyle\" style=\"height:60px; width:180px;\">�޲�Ʒ��¼</td>");
                sbHeader2.AppendFormat("<td class=\"formItemBgStyle\" style=\"height:60px; width:180px;\">�޲�Ʒ��¼</td>");
            }

            if (dtMatch != null && dtMatch.Rows.Count > 0)
            {
                IDictionary<string, decimal> dictionTotal = new Dictionary<string, decimal>();                  //ԭ�ϼ۸���ԭ���������֮��
                IDictionary<string, decimal> dictionProductTotal = new Dictionary<string, decimal>();           //ԭ������֮��
                IDictionary<string, string> dictionProductType = new Dictionary<string, string>();              //��Ʒ����

                //sbLeft.Append("<tr><td class=\"formItemBgStyle\" width=\"150\">ԭ�ϴ���</td></tr>");
                for (int i = 0; i < dtMatch.Rows.Count; i++)
                {
                    DataRow drMatch = dtMatch.Rows[i];

                    string strMatchMatchName = ShareClass.ObjectToString(drMatch["MatchName"]);
                    string strMatchMaterialPrice = ShareClass.ObjectToString(drMatch["MaterialPrice"]);

                    decimal decimalMatchMaterialPrice = 0;
                    decimal.TryParse(strMatchMaterialPrice, out decimalMatchMaterialPrice);

                    string strMatchID = ShareClass.ObjectToString(drMatch["ID"]);

                    sbContent.Append("<tr>");
                    //sbContent.AppendFormat("<td class=\"formItemBgStyle\" width=\"150\" style=\"display:none;\">{0}</td>", dWMatch.MatchName);
                    //sbContent.AppendFormat("<td class=\"formItemBgStyle\" width=\"150\" style=\"display:none;\">{0}</td>", dWMatch.MaterialPrice);
                    sbLeft.Append("<tr>");
                    sbLeft.AppendFormat("<td class=\"formItemBgStyle\" style=\"height:60px; width:100px;\"><p>{0}</p>&nbsp;</td>", strMatchMatchName);
                    sbLeft.AppendFormat("<td class=\"formItemBgStyle\" style=\"height:60px; width:100px;\">{0}</td>", decimalMatchMaterialPrice);
                    if (dtProduct != null && dtProduct.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtProduct.Rows.Count; j++)
                        {
                            DataRow drProduct = dtProduct.Rows[j];

                            string strProductID = ShareClass.ObjectToString(drProduct["ID"]);

                            string strTypeID = ShareClass.ObjectToString(drProduct["TypeID"]);

                            //�ж��Ƿ��й���ֵ
                            DataView dvProMatch = dtProductMatch.DefaultView;
                            dvProMatch.RowFilter = "MatchID = " + strMatchID + " and ProductID = " + strProductID;
                            //dvProMatch.RowFilter = "ProductID = " + dWProduct.ID;
                            DataTable dtValue = dvProMatch.ToTable();
                            if (dtValue != null && dtValue.Rows.Count > 0)
                            {
                                #region ԭ�����Ʒ��ԭ��������ʱ��
                                decimal decimalProductPrice = 0;
                                decimal.TryParse(ShareClass.ObjectToString(dtValue.Rows[0]["ProductPrice"]), out decimalProductPrice);

                                sbContent.Append("<td class=\"formItemBgStyle\" style=\"height:60px; width:180px;\">");
                                string strP = string.Format("<p class=\"playtitle\">{0}</p>", decimalProductPrice);
                                string strEditor = "<a class=\"playeditor\" href=\"javascript:void(0)\">�༭</a>";
                                string strInput = "<input type=\"text\" style=\"display:none;width:70px;\" class=\"playtx\" />";
                                string strSave = string.Format("<a class=\"playsave\" value=\"{0}|{1}\" style=\"display:none;\" href=\"javascript:void(0)\">����</a>", strMatchID, strProductID);
                                sbContent.Append(strP + strEditor + strInput + strSave);
                                sbContent.Append("</td>");

                                if (!dictionTotal.Keys.Contains(j.ToString()))
                                {
                                    dictionTotal.Add(j.ToString(), decimalMatchMaterialPrice * decimalProductPrice);
                                }
                                else
                                {
                                    dictionTotal[j.ToString()] += decimalMatchMaterialPrice * decimalProductPrice;
                                }

                                if (!dictionProductTotal.Keys.Contains(j.ToString()))
                                {
                                    dictionProductTotal.Add(j.ToString(), decimalProductPrice);
                                }
                                else
                                {
                                    dictionProductTotal[j.ToString()] += decimalProductPrice;
                                }

                                if (!dictionProductType.Keys.Contains(j.ToString()))
                                {
                                    dictionProductType.Add(j.ToString(), strTypeID);
                                }
                                #endregion
                            }
                            else
                            {
                                #region  ԭ�����Ʒû��ԭ������
                                sbContent.Append("<td class=\"formItemBgStyle\" style=\"height:60px; width:180px;\">");
                                string strP = "<p class=\"playtitle\">0</p>";
                                string strEditor = "<a class=\"playeditor\" href=\"javascript:void(0)\">�༭</a>";
                                string strInput = "<input type=\"text\" style=\"display:none;width:70px;\" class=\"playtx\" />";
                                string strSave = string.Format("<a class=\"playsave\" value=\"{0}|{1}\" style=\"display:none;\" href=\"javascript:void(0)\">����</a>", strMatchID, strProductID);
                                sbContent.Append(strP + strEditor + strInput + strSave);
                                sbContent.Append("</td>");

                                if (!dictionTotal.Keys.Contains(j.ToString()))
                                {
                                    dictionTotal.Add(j.ToString(), 0);
                                }
                                else
                                {
                                    dictionTotal[j.ToString()] += 0;
                                }

                                if (!dictionProductTotal.Keys.Contains(j.ToString()))
                                {
                                    dictionProductTotal.Add(j.ToString(), 0);
                                }
                                else
                                {
                                    dictionProductTotal[j.ToString()] += 0;
                                }

                                if (!dictionProductType.Keys.Contains(j.ToString()))
                                {
                                    dictionProductType.Add(j.ToString(), strTypeID);
                                }
                                #endregion
                            }
                        }
                    }
                    sbContent.Append("</tr>");
                    sbLeft.Append("</tr>");
                }

                #region ������
                sbFooter1.Append("<tr>");
                //sbFooter1.Append("<td class=\"formItemBgStyle\">&nbsp;</td><td class=\"formItemBgStyle\">&nbsp;</td>");

                if (dtProduct != null && dtProduct.Rows.Count > 0)
                {
                    for (int j = 0; j < dtProduct.Rows.Count; j++)
                    {
                        sbFooter1.AppendFormat("<td class=\"formItemBgStyle\">{0}</td>", dictionProductTotal[j.ToString()].ToString("#0.000"));
                    }
                }
                sbFooter1.Append("</tr>");
                #endregion

                #region ��ɫ�ɱ�
                sbFooter2.Append("<tr>");
                //sbFooter2.Append("<td class=\"formItemBgStyle\">&nbsp;</td><td class=\"formItemBgStyle\">&nbsp;</td>");

                if (dtProduct != null && dtProduct.Rows.Count > 0)
                {
                    for (int j = 0; j < dtProduct.Rows.Count; j++)
                    {
                        if (dictionProductTotal[j.ToString()] != 0)
                        {
                            decimal decimalResult = dictionTotal[j.ToString()] * 1000 / dictionProductTotal[j.ToString()];
                            sbFooter2.AppendFormat("<td class=\"formItemBgStyle\">{0}</td>", decimalResult.ToString("#0.000"));
                        }
                        else
                        {
                            sbFooter2.Append("<td class=\"formItemBgStyle\">0</td>");
                        }
                    }
                }
                sbFooter2.Append("</tr>");
                #endregion

                #region ��ɫ�ϳɱ�
                sbFooter3.Append("<tr>");
                //sbFooter3.Append("<td class=\"formItemBgStyle\">&nbsp;</td><td class=\"formItemBgStyle\">&nbsp;</td>");

                if (dtProduct != null && dtProduct.Rows.Count > 0)
                {
                    for (int j = 0; j < dtProduct.Rows.Count; j++)
                    {
                        if (dictionProductTotal[j.ToString()] != 0)
                        {
                            decimal decimalResult = dictionTotal[j.ToString()] / dictionProductTotal[j.ToString()];
                            string strProductTypeHQL = string.Format("select * from T_DWProductType where ID = {0}", dictionProductType[j.ToString()]);
                            DataTable dtProductType = ShareClass.GetDataSetFromSql(strProductTypeHQL, "ProductType").Tables[0];
                            if (dtProductType != null && dtProductType.Rows.Count > 0)
                            {
                                if (ShareClass.ObjectToString(dtProductType.Rows[0]["ProductType"]).ToLower() == "pvc")
                                {
                                    sbFooter3.AppendFormat("<td class=\"formItemBgStyle\">{0}</td>", 100);
                                }
                                else
                                {
                                    sbFooter3.Append("<td class=\"formItemBgStyle\">0</td>");
                                }
                            }
                        }
                        else
                        {
                            sbFooter3.Append("<td class=\"formItemBgStyle\">0</td>");
                        }
                    }
                }
                sbFooter3.Append("</tr>");
                #endregion

                #region �䷽��
                sbFooter4.Append("<tr>");
                //sbFooter4.Append("<td class=\"formItemBgStyle\">&nbsp;</td><td class=\"formItemBgStyle\">&nbsp;</td>");

                if (dtProduct != null && dtProduct.Rows.Count > 0)
                {
                    for (int j = 0; j < dtProduct.Rows.Count; j++)
                    {
                        if (dictionProductTotal[j.ToString()] != 0)
                        {
                            decimal decimalResult = dictionTotal[j.ToString()] * 1000 / dictionProductTotal[j.ToString()];
                            string strProductTypeHQL = string.Format("select * from T_DWProductType where ID = {0}", dictionProductType[j.ToString()]);
                            DataTable dtProductType = ShareClass.GetDataSetFromSql(strProductTypeHQL, "ProductType").Tables[0];
                            if (dtProductType != null && dtProductType.Rows.Count > 0)
                            {
                                if (ShareClass.ObjectToString(dtProductType.Rows[0]["ProductType"]).ToLower() == "pvc")
                                {
                                    sbFooter4.AppendFormat("<td class=\"formItemBgStyle\">{0}</td>", (decimalResult + 100).ToString("#0.000"));
                                }
                                else
                                {
                                    sbFooter4.AppendFormat("<td class=\"formItemBgStyle\">{0}</td>", decimalResult.ToString("#0.000"));
                                }
                            }
                        }
                        else
                        {
                            sbFooter4.Append("<td class=\"formItemBgStyle\">0</td>");
                        }
                    }
                }
                sbFooter4.Append("</tr>");
                #endregion

                #region ��ߵײ�
                sbLeftFooter.Append("<tr><td class=\"formItemBgStyle\">������</td><td class=\"formItemBgStyle\">&nbsp;</td></tr><tr><td class=\"formItemBgStyle\">��ɫ�ɱ�</td><td class=\"formItemBgStyle\">&nbsp;</td></tr><tr><td class=\"formItemBgStyle\">��ɫ�ϳɱ�</td><td class=\"formItemBgStyle\">&nbsp;</td></tr><tr><td class=\"formItemBgStyle\">�䷽��</td><td class=\"formItemBgStyle\">&nbsp;</td></tr>");
                #endregion
            }
            else
            {
                sbLeft.Append("<tr>");
                sbLeft.Append("<td class=\"formItemBgStyle\" width=\"180\"><p>��ԭ�ϼ�¼</p>&nbsp;</td>");
                sbLeft.Append("<td class=\"formItemBgStyle\" width=\"180\" style=\"display:none;\">0</td>");
                sbLeft.Append("</tr>");
            }

            LT_Left.Text = sbLeft.ToString() + sbLeftFooter.ToString();
            LT_Header.Text = sbHeader.ToString();
            LT_Header2.Text = sbHeader2.ToString();
            LT_Content.Text = sbContent.ToString() + sbFooter1.ToString() + sbFooter2.ToString() + sbFooter3.ToString() + sbFooter4.ToString();
        }
        catch (Exception ex) { }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataProductMatchBinder();
    }


    protected void BT_ReloadSearch_Click(object sender, EventArgs e)
    {
        DataProductMatchBinder();
    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        try
        {
            string strClearHQL = "truncate table T_DWProMatch";

            ShareClass.RunSqlCommand(strClearHQL);

            DataProductMatchBinder();


        }
        catch (Exception ex) { }
    }


    protected void BT_Export_Click(object sender, EventArgs e)
    {
        string strFileName = "ԭ�ϲ�Ʒ�浵";
        Export3Excel(tableExcel, strFileName);
    }


    public void Export3Excel(System.Web.UI.Control objControl, string strFileName)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + strFileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/shlnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        objControl.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();

    }
}