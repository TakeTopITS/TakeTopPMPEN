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

public partial class TTWZObjectCodeListAuto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
    }

    //重做使用标记
    protected void BT_Title_Click(object sender, EventArgs e)
    {

        WZObjectBLL wZObjectBLL = new WZObjectBLL();
        string strObjectSQL = "from WZObject as wZObject";
        IList lstWZObject = wZObjectBLL.GetAllWZObjects(strObjectSQL);
        if (lstWZObject != null && lstWZObject.Count > 0)
        {
            for (int i = 0; i < lstWZObject.Count; i++)
            {
                WZObject wZObject = (WZObject)lstWZObject[i];
                string strObjectCode = wZObject.ObjectCode;

                //先看计划明细中是有当前物资代码
                string strCheckPlanDetailHQL = string.Format(@"select * from T_WZPickingPlanDetail 
                                        where ObjectCode = '{0}'", strObjectCode);
                DataTable dtCheckPlanDetail = ShareClass.GetDataSetFromSql(strCheckPlanDetailHQL, "PlanDetail").Tables[0];
                if (dtCheckPlanDetail != null && dtCheckPlanDetail.Rows.Count > 0)
                {
                    wZObject.IsMark = -1;

                    wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

                    continue;
                }

                //移交单
                string strCheckTurnDetailHQL = string.Format(@"select * from T_WZTurnDetail
                                        where ObjectCode = '{0}'", strObjectCode);
                DataTable dtCheckTurnDetail = ShareClass.GetDataSetFromSql(strCheckTurnDetailHQL, "TurnDetail").Tables[0];
                if (dtCheckTurnDetail != null && dtCheckTurnDetail.Rows.Count > 0)
                {
                    wZObject.IsMark = -1;

                    wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

                    continue;
                }

                //采购清单
                string strCheckPurchaseDetailHQL = string.Format(@"select * from T_WZPurchaseDetail
                                        where ObjectCode = '{0}'", strObjectCode);
                DataTable dtCheckPurchaseDetail = ShareClass.GetDataSetFromSql(strCheckPurchaseDetailHQL, "PurchaseDetail").Tables[0];
                if (dtCheckPurchaseDetail != null && dtCheckPurchaseDetail.Rows.Count > 0)
                {
                    wZObject.IsMark = -1;

                    wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

                    continue;
                }

                //合同明细
                string strCheckCompactDetailHQL = string.Format(@"select * from T_WZCompactDetail
                                        where ObjectCode = '{0}'", strObjectCode);
                DataTable dtCheckCompactDetail = ShareClass.GetDataSetFromSql(strCheckCompactDetailHQL, "CompactDetail").Tables[0];
                if (dtCheckCompactDetail != null && dtCheckCompactDetail.Rows.Count > 0)
                {
                    wZObject.IsMark = -1;

                    wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

                    continue;
                }

                //收料单
                string strCheckCollectHQL = string.Format(@"select * from T_WZCollect
                                        where ObjectCode = '{0}'", strObjectCode);
                DataTable dtCheckCollect = ShareClass.GetDataSetFromSql(strCheckCollectHQL, "Collect").Tables[0];
                if (dtCheckCollect != null && dtCheckCollect.Rows.Count > 0)
                {
                    wZObject.IsMark = -1;

                    wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

                    continue;
                }

                //发料单
                string strCheckSendHQL = string.Format(@"select * from T_WZSend
                                        where ObjectCode = '{0}'", strObjectCode);
                DataTable dtCheckSend = ShareClass.GetDataSetFromSql(strCheckSendHQL, "Send").Tables[0];
                if (dtCheckSend != null && dtCheckSend.Rows.Count > 0)
                {
                    wZObject.IsMark = -1;

                    wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

                    continue;
                }

                //以上都没有，就把使用标记改为0

                wZObject.IsMark = 0;

                wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);

            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZZSYBJCG+"')", true);
        }

    }
}