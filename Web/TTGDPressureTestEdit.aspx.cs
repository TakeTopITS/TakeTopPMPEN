using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDPressureTestEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataPressureBinder();

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string id = Request.QueryString["id"].ToString();
                HF_ID.Value = id;
                int intID = 0;
                int.TryParse(id, out intID);

                BindData(intID);
            }
        }
    }



    private void DataPressureBinder()
    {
        GDPressureBLL gDPressureBLL = new GDPressureBLL();
        string strGDPressureHQL = "from GDPressure as gDPressure";
        IList listGDPressure = gDPressureBLL.GetAllGDPressures(strGDPressureHQL);

        DDL_TestLoopNo.DataSource = listGDPressure;
        DDL_TestLoopNo.DataTextField = "PressureCode";
        DDL_TestLoopNo.DataValueField = "PressureCode";
        DDL_TestLoopNo.DataBind();

        DDL_TestLoopNo.Items.Insert(0, new ListItem("", ""));
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strTestLoopNo = DDL_TestLoopNo.SelectedValue;
            string strPressTestRec = TXT_PressTestRec.Text.Trim();
            string strPressDate = TXT_PressDate.Text.Trim();
            string strReinstRec = TXT_ReinstRec.Text.Trim();
            string strReinstDate = TXT_ReinstDate.Text.Trim();
            string strFlushingBlock = TXT_FlushingBlock.Text.Trim();
            string strFlushingRec = TXT_FlushingRec.Text.Trim();
            string strFlushingDate = TXT_FlushingDate.Text.Trim();
            string strLeakTestRec = TXT_LeakTestRec.Text.Trim();
            string strLeakDate = TXT_LeakDate.Text.Trim();
            string strRemarks = TXT_Remarks.Text.Trim();

            if (string.IsNullOrEmpty(strTestLoopNo))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZSYBH+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPressTestRec))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZPRESSTESTRECBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strReinstRec))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZREINSTRECBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFlushingBlock))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFLUSHINGBLOCKBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFlushingRec))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFLUSHINGRECBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strLeakTestRec))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZLEAKTESTRECBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRemarks))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZREMARKSBNWFFZF+"')", true);
                return;
            }

            GDPressureTestBLL gDPressureTestBLL = new GDPressureTestBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDPressureTestSql = "from GDPressureTest as gDPressureTest where id = " + intID;
                IList listGDPressureTest = gDPressureTestBLL.GetAllGDPressureTests(strGDPressureTestSql);
                if (listGDPressureTest != null && listGDPressureTest.Count > 0)
                {
                    GDPressureTest gDPressureTest = (GDPressureTest)listGDPressureTest[0];

                    gDPressureTest.TestLoopNo = strTestLoopNo;
                    gDPressureTest.PressTestRec = strPressTestRec;
                    gDPressureTest.PressDate = strPressDate;
                    gDPressureTest.ReinstRec = strReinstRec;
                    gDPressureTest.ReinstDate = strReinstDate;
                    gDPressureTest.FlushingBlock = strFlushingBlock;
                    gDPressureTest.FlushingRec = strFlushingRec;
                    gDPressureTest.FlushingDate = strFlushingDate;
                    gDPressureTest.LeakTestRec = strLeakTestRec;
                    gDPressureTest.LeakDate = strLeakDate;
                    gDPressureTest.Remarks = strRemarks;

                    gDPressureTestBLL.UpdateGDPressureTest(gDPressureTest, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDPressureTest gDPressureTest = new GDPressureTest();
                gDPressureTest.TestLoopNo = strTestLoopNo;
                gDPressureTest.PressTestRec = strPressTestRec;
                gDPressureTest.PressDate = strPressDate;
                gDPressureTest.ReinstRec = strReinstRec;
                gDPressureTest.ReinstDate = strReinstDate;
                gDPressureTest.FlushingBlock = strFlushingBlock;
                gDPressureTest.FlushingRec = strFlushingRec;
                gDPressureTest.FlushingDate = strFlushingDate;
                gDPressureTest.LeakTestRec = strLeakTestRec;
                gDPressureTest.LeakDate = strLeakDate;
                gDPressureTest.Remarks = strRemarks;

                gDPressureTest.IsMark = 0;
                gDPressureTest.UserCode = strUserCode;

                gDPressureTestBLL.AddGDPressureTest(gDPressureTest);
            }

            //Response.Redirect("TTGDPressureTestList.aspx");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDPressureTestBLL gDPressureTestBLL = new GDPressureTestBLL();
        string strGDPressureTestSql = "from GDPressureTest as gDPressureTest where id = " + id;
        IList listGDPressureTest = gDPressureTestBLL.GetAllGDPressureTests(strGDPressureTestSql);
        if (listGDPressureTest != null && listGDPressureTest.Count > 0)
        {
            GDPressureTest gDPressureTest = (GDPressureTest)listGDPressureTest[0];
            DDL_TestLoopNo.SelectedValue = gDPressureTest.TestLoopNo;
            TXT_PressTestRec.Text = gDPressureTest.PressTestRec;
            TXT_PressDate.Text = gDPressureTest.PressDate.ToString();
            TXT_ReinstRec.Text = gDPressureTest.ReinstRec;
            TXT_ReinstDate.Text = gDPressureTest.ReinstDate.ToString();
            TXT_FlushingBlock.Text = gDPressureTest.FlushingBlock;
            TXT_FlushingRec.Text = gDPressureTest.FlushingRec;
            TXT_FlushingDate.Text = gDPressureTest.FlushingDate.ToString();
            TXT_LeakTestRec.Text = gDPressureTest.LeakTestRec;
            TXT_LeakDate.Text = gDPressureTest.LeakDate.ToString();
            TXT_Remarks.Text = gDPressureTest.Remarks;
        }
    }
}