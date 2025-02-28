using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Text;
using System.Data.OleDb;
using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.IO;

public partial class TTRCJProjectCostPerformanceBenchmar : System.Web.UI.Page
{
    private string UserCode = "";
    private int ProjectId = 0;
    private StringBuilder sbExcelImportMsg = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        ProjectId = Convert.ToInt32(Request.QueryString["ProjectID"]);
        UserCode = Convert.ToString(Session["UserCode"]);

        if (!IsPostBack)
        {

            if (Request.UrlReferrer != null)
                ViewState["UrlReferrer"] = Request.UrlReferrer.ToString();

            //��û�׼��Ϣ�����б�
            RCJShareClass.InitPerformanceType(DDL_PerformanceType, this.lb_ShowMessage);
            DDL_PerformanceType.SelectedIndex = 0;

            //���سа���
            InitProjectSupplier();
            DDL_ProjectSupplierID.SelectedIndex = 0;

            InitDataList();
        }
    }

    private void InitDataList()
    {
        try
        {
            StringBuilder strSql = new StringBuilder(" SELECT * from V_RCJProjectCostPerformanceList where projectid=");
            strSql.Append(ProjectId.ToString());
            strSql.Append(" and ItemType=");
            strSql.Append(DDL_PerformanceType.SelectedValue);
            strSql.Append(" order by itemno");

            DataSet res = ShareClass.GetDataSetFromSqlNOOperateLog(strSql.ToString(), "V_RCJProjectCostPerformanceList");


            GridView1.DataSource = res;
            GridView1.DataBind();

        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ����ѯ���������쳣��" + exp.Message;
        }
    }

    protected void InitProjectSupplier()
    {
        try
        {
            StringBuilder sb = new StringBuilder(" SELECT Code,Name from T_BMSupplierInfo");

            DataSet res = ShareClass.GetDataSetFromSqlNOOperateLog(sb.ToString(), "T_BMSupplierInfo");

            DDL_ProjectSupplierID.DataSource = res;
            DDL_ProjectSupplierID.DataTextField = "Name";
            DDL_ProjectSupplierID.DataValueField = "Code";
            DDL_ProjectSupplierID.DataBind();
        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "������ʾ�����سа����б���Ϣ����" + exp.Message;
            return;
        }
    }


    private bool IsInputOK()
    {
        if (TB_ItemNo.Text.Trim().Length == 0 || ShareClass.CheckIsAllNumber(TB_ItemNo.Text) == false)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ����������š���������ӦȫΪ���֣�����ȷ��������ԣ�";
            return false;
        }
        if (TB_ItemName.Text.Trim().Length == 0 || TB_ItemName.Text.Trim().Length > 256)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ��������š��������󣬲��ܳ���256�����֣�����ȷ��������ԣ�";
            return false;
        }
        if (TB_SubItem.Text.Trim().Length == 0 || TB_SubItem.Text.Trim().Length > 256)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ�����ֲ�����������󣬲��ܳ���20�����֣�����ȷ��������ԣ�";
            return false;
        }
        if (TB_ItemUnit.Text.Trim().Length == 0 || TB_ItemUnit.Text.Trim().Length > 5)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ������λ���������󣬲��ܳ���5�����֣�����ȷ��������ԣ�";
            return false;
        }
        if (TB_ItemCount.Text.Trim().Length == 0 || ShareClass.CheckIsNumber(TB_ItemCount.Text) == false)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ������������������ӦȫΪ���ִ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (TB_ItemContent.Text.Trim().Length == 0 || TB_ItemContent.Text.Trim().Length > 256)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ�����������ơ��������󣬲��ܳ���256�����֣�����ȷ��������ԣ�";
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemPriceDevice.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ�����豸ԭ�����ۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemPriceMainMaterial.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ��������ԭ�����ۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemWage.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ�����˹�ԭ�����ۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemPriceMaterial.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ��������ԭ�����ۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemPriceMachine.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ������еԭ�����ۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemPriceDeviceBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ����Ԥ���豸�ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemPriceMainMaterialBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ��������Ԥ��ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (false == RCJShareClass.isNumberString(this.tb_ProjectMaterialBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ��������Ԥ��ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (false == RCJShareClass.isNumberString(this.tb_ProjectMachineBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ������еԤ��ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        } 
        if (false == RCJShareClass.isNumberString(TB_ItemPriceWageBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ�����˹�Ԥ��ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (false == RCJShareClass.isNumberString(tb_ItemPricePurchaseFee.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ��������Ѽ�����ѡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (false == RCJShareClass.isNumberString(tb_ItemPricePurchaseFeeBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ�������ʴ�ʩ�ѡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        } 
        if (false == RCJShareClass.isNumberString(TB_ItemComprehensiveFeeBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ������ѡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemTaxesBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ����Ԥ��˰��ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemPriceTotalBudge.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ�����ϼ�Ԥ��ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�";
            return false;
        }
        if (TB_BeginTime.Text.Trim().Length <=0 )
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ������ʼʱ�䡿������������ȷѡ������ԣ�";
            return false;
        }
        if (TB_EndTime.Text.Trim().Length <= 0)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ��������ʱ�䡿������������ȷѡ������ԣ�";
            return false;
        }

        return true;
    }

    //���漨Ч��׼����
    protected void btnSaveBenchmarData_Click(object sender, EventArgs e)
    {
        if (IsInputOK() == false)
            return;

        try
        {
            int iid = 0;
            if (CheckHasRecord(DDL_PerformanceType.SelectedValue, TB_ItemNo.Text , ref iid) == true)
            {
                lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
                lb_ShowMessage.Text = "��Ϣ��ʾ������������Ѿ����ڣ�����ȷ��������ԣ�";
                return;
            }

            if (SaveDataList(0) == true)
            {
                lb_ShowMessage.ForeColor = System.Drawing.Color.Green;
                lb_ShowMessage.Text = "��Ϣ��ʾ���������ݳɹ���";

                InitDataList();
            }

        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ���������������쳣��" + exp.Message;
        }
    }

    private bool SaveDataList(int bAdd)
    {
        try
        {
            //RCJProjectCostPerformanceListBLL cpbBLL = new RCJProjectCostPerformanceListBLL();
            RCJProjectCostPerformanceList item = new RCJProjectCostPerformanceList();

            item.ProjectID = ProjectId;
            item.ItemNo = Convert.ToInt32(TB_ItemNo.Text);
            item.ItemName = TB_ItemName.Text;
            item.ItemType = Convert.ToInt32(DDL_PerformanceType.SelectedValue);
            item.ItemContent = TB_ItemContent.Text;
            item.ItemUnit = TB_ItemUnit.Text;
            item.SubItem = TB_SubItem.Text;
            //item.ItemCount = Convert.ToDouble(TB_ItemCount.Text);
            item.ItemPriceDevice = Convert.ToDouble(TB_ItemPriceDevice.Text);
            item.ItemPriceMainMaterial = Convert.ToDouble(TB_ItemPriceMainMaterial.Text);
            item.ItemPriceWage = Convert.ToDouble(TB_ItemWage.Text);
            item.ItemPriceMaterial = Convert.ToDouble(TB_ItemPriceMaterial.Text);
            item.ItemPriceMachine = Convert.ToDouble(TB_ItemPriceMachine.Text);
            item.ItemPriceDeviceBudget = Convert.ToDouble(TB_ItemPriceDeviceBudget.Text);
            item.ItemPriceMainMaterialBudget = Convert.ToDouble(TB_ItemPriceMainMaterialBudget.Text);
            item.ItemPriceWageBudget = Convert.ToDouble(TB_ItemPriceWageBudget.Text);
            item.ItemComprehensiveFeeBudget = Convert.ToDouble(TB_ItemComprehensiveFeeBudget.Text);
            item.ItemTaxesBudget = Convert.ToDouble(TB_ItemTaxesBudget.Text);
            item.ProjectSupplierID = DDL_ProjectSupplierID.SelectedValue;
            item.ItemPriceMachineBudget = Convert.ToDouble(tb_ProjectMachineBudget.Text);
            item.ItemPriceMaterialBudget = Convert.ToDouble(tb_ProjectMaterialBudget.Text);
            item.ItemPricePurchaseFee = Convert.ToDouble(tb_ItemPricePurchaseFee.Text);
            item.ItemPricePurchaseFeeBudget = Convert.ToDouble(tb_ItemPricePurchaseFeeBudget.Text);
            item.BeginTime = Convert.ToDateTime(TB_BeginTime.Text).ToString("d");
            item.EndTime = Convert.ToDateTime(TB_EndTime.Text).ToString("d");
            item.IfSplit = CB_IfEveryMonth.Checked ? 1 : 0;

            item.AdjustID = RCJShareClass.getMinAdjustID(ProjectId, int.Parse(DDL_PerformanceType.SelectedValue), int.Parse(TB_ItemNo.Text));
            item.ItemPriceChanged = 0;

            //����һ�εĵ�����Ϊ��һ�ε�����
            RCJProjectAdjustPriceList paplData = new RCJProjectAdjustPriceList();
            //RCJProjectAdjustPriceListBLL rpplBLL = new RCJProjectAdjustPriceListBLL();
            paplData.ItemNo = Convert.ToInt32(TB_ItemNo.Text);
            paplData.ProjectID = ProjectId;
            paplData.ItemType = Convert.ToInt32(DDL_PerformanceType.SelectedValue);
            paplData.ItemPriceDeviceAdjust = Convert.ToDouble(TB_ItemPriceDevice.Text);
            paplData.ItemPriceMainMaterialAdjust = Convert.ToDouble(TB_ItemPriceMainMaterial.Text);
            paplData.ItemPriceWageAdjust = Convert.ToDouble(TB_ItemWage.Text);
            paplData.ItemPriceMaterialAdjust = Convert.ToDouble(TB_ItemPriceMaterial.Text);
            paplData.ItemPriceMachineAdjust = Convert.ToDouble(TB_ItemPriceMachine.Text);
            paplData.TaxesPriceAdjust = Convert.ToDouble(TB_ItemTaxesBudget.Text);
            //paplData.ComprehensivePriceAdjust = Convert.ToDouble(TB_ItemPriceTotalBudge.Text);
            paplData.ProjectBCWS = Convert.ToDouble(TB_ItemPriceTotalBudge.Text);
            paplData.ItemNum = Convert.ToDouble(TB_ItemCount.Text);
            paplData.Memo = "ԭ���۸�";


            runAddDataList(bAdd,item, paplData);
        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ���������������쳣��" + exp.Message;

            return false;
        }

        return true;
    }

    //ɾ����¼
    protected void btnDelBenchmarData_Click(object sender, EventArgs e)
    {
        if (GridView1.SelectedIndex == -1)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            string strMsg = "��Ϣ��ʾ����ѡ��һ��Ҫɾ���Ļ�׼�����ٽ���ɾ������!";
            lb_ShowMessage.Text = strMsg;
            return;
        }

        //��ѯ�Ƿ��иü�¼,�м�¼��ɾ��
        try
        {
            //ɾ������

            SaveDataList(2);

            lb_ShowMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowMessage.Text = "��Ϣ��ʾ��ָ����¼ɾ���ɹ���";

            InitDataList();
        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ��ɾ�����������쳣��" + exp.Message;
        }

    }

    private bool CheckHasRecord(string itemtype, string strItemno , ref int iID)
    {
        StringBuilder sb = new StringBuilder("select 1 From T_RCJProjectCostPerformanceList where projectid=");
        sb.Append(ProjectId.ToString());
        sb.Append(" and itemno=");
        sb.Append(strItemno);
        sb.Append(" and ItemType=");
        sb.Append(itemtype);

        DataSet ds = ShareClass.GetDataSetFromSql(sb.ToString(), "T_RCJProjectCostPerformanceList");

        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            return true;

        return false;
    }

    protected void btn_ExcelToDataTraining_Click(object sender, EventArgs e)
    {
        if (FileUpload_Training.HasFile == false)//HasFile�������FileUpload�Ƿ���ָ���ļ�
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            LB_ShowMessageImport.Text = "������ʾ�� ����ѡ��Excel�ļ�!";
            return;//�����ļ�ʱ,����
        }
        string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower() ;//System.IO.Path.GetExtension����ļ�����չ��
        if (IsXls != ".xls" && IsXls != ".xlsx")
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            LB_ShowMessageImport.Text = "������ʾ�� ֻ����ѡ��Excel�ļ�!";
            return;//��ѡ��Ĳ���Excel�ļ�ʱ,����
        }
        string filename = FileUpload_Training.FileName.ToString();              //��ȡExecle�ļ���
        string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//���ļ����ƣ�����׺
        string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + UserCode.Trim() + "\\Doc\\";
        FileInfo fi = new FileInfo(strDocSavePath + newfilename);
        if (fi.Exists)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('Excel�ļ�������');</script>");
        }
        else
        {
            FileUpload_Training.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
            string strPath = strDocSavePath + newfilename;               //SaveAs ���ϴ����ļ����ݱ����ڷ�������

            sbExcelImportMsg.Remove(0, sbExcelImportMsg.Length);
            TB_AnalysMsg.Text = "";

            for (int itype = 0; itype < DDL_PerformanceType.Items.Count; itype++)
            {
                string strTypeId = DDL_PerformanceType.Items[itype].Text;
                int iTypeId = Convert.ToInt32(DDL_PerformanceType.Items[itype].Value);

                DataSet ds = ExcelSqlConnection(strPath, filename, strTypeId);           //�����Զ��巽��
                if (null == ds)
                    continue;
                DataRow[] dr = ds.Tables[0].Select();            //����һ��DataRow����
                int rowsnum = ds.Tables[0].Rows.Count;
                if (rowsnum == 0)
                {
                    sbExcelImportMsg.Append("������ʾ: [");
                    sbExcelImportMsg.Append(strTypeId);
                    sbExcelImportMsg.Append("]�����ݣ�������һ�����͡�\n");
                    continue;
                }
                else
                {
                    int sucnum = 0;
                    for (int i = 0; i < dr.Length; i++)
                    {
                        if (true == AnalysisExcelRecord(dr[i], iTypeId))
                        {
                            sucnum++;
                        }
                        TB_AnalysMsg.Text = sbExcelImportMsg.ToString();
                    }

                    string str = string.Format("��Excel�ļ�����ɱ���Ч��׼���ݹ�{0:D}�����ɹ�{1:D}��,רҵ���ࣺ{2:s}\n", dr.Length, sucnum, strTypeId);
                    sbExcelImportMsg.Append(str);

                    if (sucnum > 0)
                        InitDataList();
                }
            }
        }
    }

    private static System.Data.DataSet ExcelSqlConnection(string filepath, string tableName, string typename)
    {
        string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
        OleDbConnection ExcelConn = new OleDbConnection(strCon);
        try
        {
            using (ExcelConn)
            {
                StringBuilder strCom = new StringBuilder("SELECT * FROM [");
                strCom.Append(typename);
                strCom.Append("$]");

                ExcelConn.Open();
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom.ToString(), ExcelConn);
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "[" + tableName + "$]");
                ExcelConn.Close();
                return ds;
            }
        }
        catch
        {
            ExcelConn.Close();
            return null;
        }
    }

    private bool IsExcelRecordValid(DataRow dr)
    {
        int itemid = 0;
        if (dr[itemid].ToString().Trim().Length == 0 || ShareClass.CheckIsAllNumber(dr[itemid].ToString()) == false)
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��������š���������ӦȫΪ���֣�����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        sbExcelImportMsg.Append("��ʼ���롾������š�Ϊ:");
        sbExcelImportMsg.Append(dr[0].ToString());
        sbExcelImportMsg.Append(" �ļ�Ч��׼����-----------------\n");
        if (dr[itemid].ToString().Trim().Length == 0 || dr[itemid].ToString().Trim().Length > 256)
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ��������š��������󣬲��ܳ���256�����֣�����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (dr[itemid].ToString().Trim().Length == 0 || dr[itemid].ToString().Trim().Length > 256)
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ�����������ơ��������󣬲��ܳ���256�����֣�����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (dr[itemid].ToString().Trim().Length == 0 || dr[itemid].ToString().Trim().Length > 40)
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ�����ֲ�����������󣬲��ܳ���40�����֣�����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (dr[itemid].ToString().Trim().Length == 0 || dr[itemid].ToString().Trim().Length > 5)
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ������λ���������󣬲��ܳ���5�����֣�����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ������������������ӦȫΪ�����Ҵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ�����豸ԭ�����ۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ��������ԭ�����ۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ�����˹�ԭ�����ۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ��������ԭ�����ۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ������еԭ�����ۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ����Ԥ���豸�ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ��������Ԥ��ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ�����˹�Ԥ��ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ��������Ԥ��ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ������еԤ��ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ��������Ѽ�����ѡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ�������ʴ�ʩ�ѡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ������ѡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ����Ԥ��˰��ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (false == RCJShareClass.isNumberString(dr[itemid].ToString()))
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ�����ϼ�Ԥ��ϼۡ���������ӦΪ�����Ҵ��ڵ����㣬����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (dr[itemid].ToString().Trim().Length != 0 && dr[itemid].ToString().Trim().Length > 50)
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ�����а��̡��������󣬲�Ϊ���򳤶�50���ַ���25�����֣�����ȷ��������ԣ�\n");
            return false;
        }
        if(DDL_ProjectSupplierID.Items.FindByValue(dr[itemid].ToString().Trim()) == null)
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ�����а��̡������ڣ�����ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (dr[itemid].ToString().Trim() != "0" && dr[itemid].ToString().Trim() != "1")
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            sbExcelImportMsg.Append("��Ϣ��ʾ�����Զ������¼ƻ����������ֻ��Ϊ0����1������ȷ��������ԣ�\n");
            return false;
        }
        itemid++;
        if (dr[itemid].ToString().Trim() == "0")
        {
            itemid++;
            if (dr[itemid].ToString().Trim().Length != 0 && RCJShareClass.IsDate(dr[itemid].ToString().Trim()) == false)
            {
                LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
                sbExcelImportMsg.Append("��Ϣ��ʾ������ʼʱ�䡿�������󣬸�ʽΪyyyy/mm/dd,����ȷ��������ԣ�\n");
                return false;
            }
            itemid++;
            if (dr[itemid].ToString().Trim().Length != 0 && RCJShareClass.IsDate(dr[itemid].ToString().Trim()) == false)
            {
                LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
                sbExcelImportMsg.Append("��Ϣ��ʾ��������ʱ�䡿��������,��ʽΪyyyy/mm/dd������ȷ��������ԣ�\n");
                return false;
            }
        }
        return true;
    }

    //���������Excel��¼
    private bool AnalysisExcelRecord(DataRow dr, int iTypeId)
    {
        //��¼�Ƿ�Ϸ�,������Ϸ��򲻵���
        if (IsExcelRecordValid(dr) == false)
        {
            sbExcelImportMsg.Append("����һ����¼ʧ�ܣ�����\n");
            return false;
        }

        try
        {
            RCJProjectCostPerformanceList item = new RCJProjectCostPerformanceList();

            item.ProjectID = ProjectId;
            item.ItemNo = Convert.ToInt32(dr[0].ToString());
            item.ItemName = dr[1].ToString();
            item.ItemType = iTypeId;
            item.ItemContent = dr[2].ToString();
            item.SubItem = dr[3].ToString();
            item.ItemUnit = dr[4].ToString();
            //item.ItemCount = Convert.ToDouble(dr[4].ToString());
            item.ItemPriceDeviceBudget = Convert.ToDouble(dr[11].ToString());
            item.ItemPriceMainMaterialBudget = Convert.ToDouble(dr[12].ToString());
            item.ItemPriceWageBudget = Convert.ToDouble(dr[13].ToString()); 
            item.ItemPriceMaterialBudget = Convert.ToDouble(dr[14].ToString());
            item.ItemPriceMachineBudget = Convert.ToDouble(dr[15].ToString());
            item.ItemPricePurchaseFee = Convert.ToDouble(dr[16].ToString());
            item.ItemPricePurchaseFeeBudget = Convert.ToDouble(dr[17].ToString());
            item.ItemComprehensiveFeeBudget = Convert.ToDouble(dr[18].ToString());
            item.ItemTaxesBudget = Convert.ToDouble(dr[19].ToString());
            //item.ItemPriceTotalBudget = Convert.ToDouble(dr[19].ToString());
            item.ProjectSupplierID = dr[21].ToString();
            item.IfSplit = Convert.ToInt32(dr[22].ToString());
            item.BeginTime = dr[23].ToString();
            item.EndTime = dr[24].ToString();

            //����һ�εĵ�����Ϊ��һ�ε�����
            RCJProjectAdjustPriceList paplData = new RCJProjectAdjustPriceList();
            //RCJProjectAdjustPriceListBLL rpplBLL = new RCJProjectAdjustPriceListBLL();
            paplData.ItemNo = Convert.ToInt32(dr[0].ToString());
            paplData.ProjectID = ProjectId;
            paplData.ItemType = iTypeId;
            paplData.ItemPriceDeviceAdjust = Convert.ToDouble(dr[6].ToString());
            paplData.ItemPriceMainMaterialAdjust = Convert.ToDouble(dr[7].ToString());
            paplData.ItemPriceWageAdjust = Convert.ToDouble(dr[8].ToString());
            paplData.ItemPriceMaterialAdjust = Convert.ToDouble(dr[9].ToString());
            paplData.ItemPriceMachineAdjust = Convert.ToDouble(dr[10].ToString());
            //paplData.ComprehensivePriceAdjust = Convert.ToDouble(dr[17].ToString());
            paplData.TaxesPriceAdjust = Convert.ToDouble(dr[19].ToString());
            paplData.ProjectBCWS = Convert.ToDouble(dr[20].ToString());
            paplData.ItemNum = Convert.ToDouble(dr[5].ToString());
            paplData.Memo = "ԭ���۸�";

            RCJProjectCostPerformanceList oldItem = new RCJProjectCostPerformanceList();

            item.ItemPriceChanged = 0;
            //item.AdjustID = RCJShareClass.getMinAdjustID(ProjectId, iTypeId, item.ItemNo);
            runAddDataList(3,item, paplData);

        }
        catch (Exception exp)
        {
            sbExcelImportMsg.Append("��������š�Ϊ:");
            sbExcelImportMsg.Append(dr[0].ToString());
            sbExcelImportMsg.Append(" �ļ�Ч��׼���ݲ��������쳣��");
            sbExcelImportMsg.Append(exp.Message);
            sbExcelImportMsg.Append("\n");
            return false;
        }

        sbExcelImportMsg.Append("��������š�Ϊ:");
        sbExcelImportMsg.Append(dr[0].ToString());
        sbExcelImportMsg.Append(" �ļ�Ч��׼���ݵ���ɹ���\n");

        return true;
    }

    protected bool runAddDataList(int add, RCJProjectCostPerformanceList list, RCJProjectAdjustPriceList adjust)
    {
            StringBuilder sql = new StringBuilder("exec Pro_RCJCostPerformanceListOperation ");
            sql.Append(add.ToString());
            sql.Append(",");
            sql.Append(list.ProjectID.ToString());
            sql.Append(",");
            sql.Append(list.ItemType.ToString());
            sql.Append(","); 
            sql.Append(list.ItemNo.ToString());
            sql.Append(",'");
            sql.Append(list.ItemName);
            sql.Append("','");
            sql.Append(list.ItemContent.ToString());
            sql.Append("','");
            sql.Append(list.ItemUnit);
            sql.Append("',");
            sql.Append(adjust.ItemNum);
            sql.Append(",");
            sql.Append(adjust.ItemPriceDeviceAdjust);
            sql.Append(",");
            sql.Append(adjust.ItemPriceMainMaterialAdjust);
            sql.Append(",");
            sql.Append(adjust.ItemPriceWageAdjust);
            sql.Append(",");
            sql.Append(adjust.ItemPriceMaterialAdjust);
            sql.Append(",");
            sql.Append(adjust.ItemPriceMachineAdjust);
            sql.Append(",");
            sql.Append(list.ItemPriceDeviceBudget);
            sql.Append(",");
            sql.Append(list.ItemPriceMainMaterialBudget);
            sql.Append(",");
            sql.Append(list.ItemPriceWageBudget);
            sql.Append(",");
            sql.Append(list.ItemPriceMaterialBudget);
            sql.Append(",");
            sql.Append(list.ItemPriceMachineBudget);
            sql.Append(",");
            sql.Append(list.ItemPricePurchaseFee);
            sql.Append(",");
            sql.Append(list.ItemPricePurchaseFeeBudget);
            sql.Append(",");
            sql.Append(list.ItemComprehensiveFeeBudget);
            sql.Append(",");
            sql.Append(list.ItemTaxesBudget);
            sql.Append(",");
            sql.Append(adjust.ProjectBCWS);
            sql.Append(",'");
            sql.Append(list.ProjectSupplierID);
            sql.Append("',");
            sql.Append(list.IfSplit);
            sql.Append(",'"); 
            sql.Append(adjust.Memo);
            sql.Append("','");
            sql.Append(list.BeginTime);
            sql.Append("','");
            sql.Append(list.EndTime); 
            sql.Append("','");
            sql.Append(UserCode);
            sql.Append("','");
            sql.Append(list.SubItem); 
            sql.Append("'");

            ShareClass.RunSqlCommand(sql.ToString());

        return true;
    }

    //ѡ��ָ����¼���в���
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            TB_ItemNo.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[1].Text;
            TB_ItemName.Text = HttpUtility.HtmlDecode(GridView1.Rows[GridView1.SelectedIndex].Cells[2].Text);
            DDL_PerformanceType.ClearSelection();
            (DDL_PerformanceType.Items.FindByText(GridView1.Rows[GridView1.SelectedIndex].Cells[3].Text)).Selected = true;
            TB_ItemUnit.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[4].Text;
            TB_ItemCount.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[5].Text.Replace(",", "");
            TB_ItemContent.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[6].Text;
            TB_SubItem.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[7].Text;
            TB_ItemPriceDevice.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[8].Text.Replace(",", "");
            TB_ItemPriceMainMaterial.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[9].Text.Replace(",", "");
            TB_ItemWage.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[10].Text.Replace(",", "");
            TB_ItemPriceMaterial.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[11].Text.Replace(",", "");
            TB_ItemPriceMachine.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[12].Text.Replace(",", "");
            TB_ItemPriceDeviceBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[13].Text.Replace(",", "");
            TB_ItemPriceMainMaterialBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[14].Text.Replace(",", "");
            TB_ItemPriceWageBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[15].Text.Replace(",", "");
            tb_ProjectMaterialBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[16].Text.Replace(",", "");
            tb_ProjectMachineBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[17].Text.Replace(",", "");
            tb_ItemPricePurchaseFee.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[18].Text.Replace(",", "");
            tb_ItemPricePurchaseFeeBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[19].Text.Replace(",", "");
            TB_ItemComprehensiveFeeBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[20].Text.Replace(",", "");
            TB_ItemTaxesBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[21].Text.Replace(",", "");
            TB_ItemPriceTotalBudge.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[22].Text.Replace(",", "");
            DDL_ProjectSupplierID.ClearSelection();
            DDL_ProjectSupplierID.Items.FindByText(GridView1.Rows[GridView1.SelectedIndex].Cells[23].Text).Selected = true;
            CB_IfEveryMonth.Checked = ((Label)GridView1.Rows[GridView1.SelectedIndex].FindControl("Label2")).Text.Trim() == "��";
            TB_BeginTime.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[24].Text;
            TB_EndTime.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[25].Text;

            lb_ShowMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowMessage.Text = "��Ϣ��ʾ��ָ����¼��ѯ�ɹ���";
        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ������ʧ�ܣ�" + exp.Message;
        }
    }

    //�޸�����
    protected void BT_EditBenchmarData_Click(object sender, EventArgs e)
    {
        if (GridView1.SelectedIndex == -1)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            string strMsg = "��Ϣ��ʾ����ѡ��һ��Ҫ�޸ĵĻ�׼�����ٽ����޸Ĳ���!";
            lb_ShowMessage.Text = strMsg;
            return;
        }

        if (IsInputOK() == false)
            return;

        //�޸�ʱ�����޸��Զ�����
        bool IsChecked = ((Label)GridView1.Rows[GridView1.SelectedIndex].FindControl("Label2")).Text.Trim() == "��";
        if (CB_IfEveryMonth.Checked != IsChecked)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            string strMsg = "��Ϣ��ʾ���޸Ĳ���ʱ�����޸ġ��Զ������¼ƻ��!";
            lb_ShowMessage.Text = strMsg;
            CB_IfEveryMonth.Checked = ((Label)GridView1.Rows[GridView1.SelectedIndex].FindControl("Label2")).Text.Trim() == "��";
            return;
        }

        //�޸�ʱ���BCWP��Ϊ�㣬����������С���Ѿ�¼��Ĺ�����
        if (RCJShareClass.CheckTheItemNumber(ProjectId, Convert.ToInt32(DDL_PerformanceType.SelectedValue), Convert.ToInt32(TB_ItemNo.Text), 1, TB_ItemCount.Text) == false)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            string strMsg = "��Ϣ��ʾ������BCWP��Ϊ�㣬�޸Ĳ���ʱ����С���Ѿ�¼��Ĺ�������������ȷ��д���ٽ����޸Ĳ���!";
            lb_ShowMessage.Text = strMsg;
            return;
        }


        //�޸Ĳ���ʱ�����޸��������
        if (TB_ItemNo.Text != GridView1.Rows[GridView1.SelectedIndex].Cells[1].Text)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            string strMsg = "��Ϣ��ʾ�������޸Ĳ���ʱ�����޸�������ţ�����ȷ��д���ٽ����޸Ĳ���!";
            lb_ShowMessage.Text = strMsg;
            return;
        }

        try
        {
            SaveDataList(1);

            lb_ShowMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowMessage.Text = "��Ϣ��ʾ��ָ����¼�޸ĳɹ���";

            InitDataList();
        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ���޸Ĳ��������쳣��" + exp.Message;
        }
    }

    private bool getRelatedListData(ref RCJProjectCostPerformanceList item)
    {
        try
        {

            StringBuilder sql = new StringBuilder("from RCJProjectCostPerformanceList as rRCJProjectCostPerformanceList where ProjectID=");
            sql.Append(ProjectId.ToString());
            sql.Append(" and itemno=");
            sql.Append(TB_ItemNo.Text);
            sql.Append(" and itemtype=");
            sql.Append(DDL_PerformanceType.SelectedValue);

            RCJProjectCostPerformanceListBLL cpbBLL = new RCJProjectCostPerformanceListBLL();

            IList res = cpbBLL.GetAllRCJProjectCostPerformanceList(sql.ToString());

            if (res.Count > 0)
                item = (RCJProjectCostPerformanceList)res[0];
            else
            {
                lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
                lb_ShowMessage.Text = "��Ϣ��ʾ����ѯԭ�м�Ч�б����ݲ���û�ж�Ӧ��¼��"; 
                
                return false;
            }
        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ����ѯԭ�м�Ч�б����ݲ��������쳣��" + exp.Message;

            return false;
        }

        return true;
    }

    private bool getRelatedAdjustData(int adjustid, ref RCJProjectAdjustPriceList item)
    {
        try
        {

            StringBuilder sql = new StringBuilder("from RCJProjectAdjustPriceList as rRCJProjectAdjustPriceList where ProjectID=");
            sql.Append(ProjectId.ToString());
            sql.Append(" and itemno=");
            sql.Append(TB_ItemNo.Text);
            sql.Append(" and itemtype=");
            sql.Append(DDL_PerformanceType.SelectedValue);
            sql.Append(" and AdjustID=");
            sql.Append(adjustid.ToString());

            RCJProjectAdjustPriceListBLL rpplBLL = new RCJProjectAdjustPriceListBLL();

            IList res = rpplBLL.GetAllRCJProjectAdjustPriceList(sql.ToString());

            if (res.Count > 0)
                item = (RCJProjectAdjustPriceList)res[0];
            else
            {
                lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
                lb_ShowMessage.Text = "��Ϣ��ʾ����ѯԭ�м۸�������ݲ���û�ж�Ӧ��¼��";

                return false;
            }
        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = "��Ϣ��ʾ����ѯԭ�м۸�������������쳣��" + exp.Message;

            return false;
        }

        return true;
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.SelectedIndex = -1;
        InitDataList();
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect(ViewState["UrlReferrer"].ToString());
    }

    protected void BT_ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            sbExcelImportMsg.Remove(0, sbExcelImportMsg.Length);
            sbExcelImportMsg.Append("���ڵ�����Ч��׼�б���Ϣ����...\n");
            TB_AnalysMsg.Text = sbExcelImportMsg.ToString();
            //���ݵ���ģ��������ʱ�ļ�
            string filePath = Server.MapPath("~/Template/" + Guid.NewGuid().ToString() + ".xls");
            try
            {
                File.Copy(Server.MapPath("~/Template/��Ч��׼���ݵ���ģ��.xls"), filePath);
            }
            catch (Exception exp)
            {
                sbExcelImportMsg.Append("ģ������쳣��");
                sbExcelImportMsg.Append(exp.Message);
                sbExcelImportMsg.Append("\n");
                TB_AnalysMsg.Text = sbExcelImportMsg.ToString();
                return;
            }

            for (int i = 0; i < DDL_PerformanceType.Items.Count; i++)
            {
                ExportRecordToExcel(filePath, DDL_PerformanceType.Items[i].Value, DDL_PerformanceType.Items[i].Text);
            }

            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Green;
            LB_ShowMessageImport.Text = "������ʾ�� ��Ч��׼���ݵ��뵽Excel�ļ��ɹ�!";

            sbExcelImportMsg.Append("�����ļ��ɹ�\n");
            TB_AnalysMsg.Text = sbExcelImportMsg.ToString();
            //�ļ����浽����

            Response.ContentType = "application/ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment;filename=��Ч��׼����.xls");
            Response.WriteFile(filePath);

        }
        catch (Exception exp)
        {
            sbExcelImportMsg.Append("�����쳣����ȷ��ģ���ļ�����Ч��׼���ݵ���ģ��.xls���Ƿ�����ʹ���У���رպ����ԣ�");
            sbExcelImportMsg.Append(exp.Message);
            sbExcelImportMsg.Append("\n");
            TB_AnalysMsg.Text = sbExcelImportMsg.ToString();
        }
    }


    private void ExportRecordToExcel(string strMidFile, string strItemTypeId, string strItemTypeName)
    {
        try
        {
            //��ѯ��ȡ��¼�б�
            RCJProjectCostPerformanceBenchmarBLL cpbBLL = new RCJProjectCostPerformanceBenchmarBLL();
            StringBuilder strSql = new StringBuilder("From  RCJProjectCostPerformanceList as rRCJProjectCostPerformanceList where projectid=");
            strSql.Append(ProjectId.ToString());
            strSql.Append(" and ItemType=");
            strSql.Append(strItemTypeId);
            strSql.Append(" order by itemno");

            IList list;
            list = cpbBLL.GetAllProjectCostPerformanceBenchmar(strSql.ToString());

            //д���¼���ļ���
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMidFile + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=0'";
            OleDbConnection ExcelConn = new OleDbConnection(strCon);
            using (ExcelConn)
            {
                ExcelConn.Open();

                for (int i = 0; i < list.Count; i++)
                {
                    StringBuilder strCom = new StringBuilder("insert into [");
                    strCom.Append(strItemTypeName);
                    strCom.Append("$] values(");
                    RCJProjectCostPerformanceList cpbData = list[i] as RCJProjectCostPerformanceList;

                    strCom.Append(cpbData.ItemNo);
                    strCom.Append(",'");
                    strCom.Append(cpbData.ItemName);
                    strCom.Append("','");
                    strCom.Append(cpbData.ItemContent);
                    strCom.Append("','");
                    strCom.Append(cpbData.ItemUnit.Trim());
                    strCom.Append("',");
                    //???strCom.Append(cpbData.ItemCount);
                    //strCom.Append(",");
                    strCom.Append(cpbData.ItemPriceDevice);
                    strCom.Append(",");
                    strCom.Append(cpbData.ItemPriceMainMaterial);
                    strCom.Append(","); 
                    strCom.Append(cpbData.ItemPriceWage);
                    strCom.Append(",");
                    strCom.Append(cpbData.ItemPriceMaterial);
                    strCom.Append(",");
                    strCom.Append(cpbData.ItemPriceMachine);
                    strCom.Append(",");
                    strCom.Append(cpbData.ItemPriceDeviceBudget);
                    strCom.Append(",");
                    strCom.Append(cpbData.ItemPriceMainMaterialBudget);
                    strCom.Append(",");
                    strCom.Append(cpbData.ItemPriceWageBudget);
                    strCom.Append(","); 
                    strCom.Append(cpbData.ItemPriceMaterialBudget);
                    strCom.Append(","); 
                    strCom.Append(cpbData.ItemPriceMachineBudget);
                    strCom.Append(",");
                    strCom.Append(cpbData.ItemPricePurchaseFee);
                    strCom.Append(","); 
                    strCom.Append(cpbData.ItemPricePurchaseFeeBudget);
                    strCom.Append(",");
                    strCom.Append(cpbData.ItemComprehensiveFeeBudget);
                    strCom.Append(",");
                    strCom.Append(cpbData.ItemTaxesBudget);
                    strCom.Append(","); 
                    //strCom.Append(cpbData.ItemPriceTotalBudget);
                    //strCom.Append(",");
                    strCom.Append(cpbData.ProjectSupplierID);
                    strCom.Append(")");

                    OleDbCommand cmd = new OleDbCommand(strCom.ToString(), ExcelConn);
                    int linenum = cmd.ExecuteNonQuery();
                }

                sbExcelImportMsg.Append("����[");
                sbExcelImportMsg.Append(strItemTypeName);
                sbExcelImportMsg.Append("]�ɹ�����");
                sbExcelImportMsg.Append(list.Count.ToString());
                sbExcelImportMsg.Append("����\n");
                TB_AnalysMsg.Text = sbExcelImportMsg.ToString();

                ExcelConn.Close();
            }
        }
        catch (Exception exp)
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            StringBuilder sb = new StringBuilder("������ʾ�� ��Ч��׼����[");
            sb.Append(strItemTypeName);
            sb.Append("]���뵽Excel�ļ�ʧ��!");
            sb.Append(exp.Message);
            LB_ShowMessageImport.Text = sb.ToString();
        }
    }

    protected void CB_IfEveryMonth_CheckedChanged(object sender, EventArgs e)
    {
        int intEveryMonth = 0;
        if (CB_IfEveryMonth.Checked == true)
            intEveryMonth = 1;

        try
        {
            //��װ�޸�SQL���
            StringBuilder sql = new StringBuilder("exec Pro_InsertRCJProjectSetup ");
            sql.Append(ProjectId.ToString());
            sql.Append(",");
            sql.Append(intEveryMonth.ToString());

            ShareClass.RunSqlCommand(sql.ToString());

            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Green;
            LB_ShowMessageImport.Text = "��Ϣ��ʾ���޸ġ����¼ƻ���ģʽ�ɹ���";
        }
        catch (Exception exp)
        {
            LB_ShowMessageImport.ForeColor = System.Drawing.Color.Red;
            LB_ShowMessageImport.Text = "������ʾ�� ��Ч��׼���ݵ��뵽Excel�ļ�ʧ��!" + exp.Message;
        }
    }

    private void ClearInputText()
    {
        TB_ItemNo.Text = "";
        TB_ItemName.Text = "";
        TB_ItemUnit.Text = "";
        TB_ItemCount.Text = "";
        TB_ItemContent.Text = "";
        TB_ItemPriceDevice.Text = "";
        TB_ItemPriceMainMaterial.Text = "";
        TB_ItemWage.Text = "";
        TB_ItemPriceMaterial.Text = "";
        TB_ItemPriceMachine.Text = "";
        TB_ItemPriceDeviceBudget.Text = "";
        TB_ItemPriceMainMaterialBudget.Text = "";
        tb_ProjectMachineBudget.Text = "";
        tb_ProjectMaterialBudget.Text = "";
        TB_ItemPriceWageBudget.Text = "";
        TB_ItemComprehensiveFeeBudget.Text = "";
        TB_ItemTaxesBudget.Text = "";
        TB_ItemPriceTotalBudge.Text = "";
        DDL_ProjectSupplierID.SelectedIndex = 0;
        tb_ItemPricePurchaseFee.Text = "";
        tb_ItemPricePurchaseFeeBudget.Text = "";
        lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
        TB_SubItem.Text = "";
        lb_ShowMessage.Text = "��Ϣ��ʾ����";
    }

    protected void DDL_PerformanceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitDataList();

        ClearInputText();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //��꾭��ʱ���б���ɫ�� 
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#A9A9A9'");
            //����Ƴ�ʱ���б���ɫ�� 
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");

            Label lbl1 = (Label)e.Row.FindControl("Label1");
            if ("��" == lbl1.Text)
                e.Row.ForeColor = System.Drawing.Color.Crimson;
            else
                e.Row.ForeColor = System.Drawing.Color.Blue;

            int cellid = 6;
            e.Row.Cells[cellid].ToolTip = e.Row.Cells[cellid].Text;
            if (e.Row.Cells[cellid].Text.Length > 35)
                e.Row.Cells[cellid].Text = e.Row.Cells[cellid].Text.Substring(0, 35);
        }
    }

    protected void BT_Calendar1_Click(object sender, EventArgs e)
    {
        if (TB_BeginTime.Text.Trim().Length == 0)
        {
            Calendar1.VisibleDate = DateTime.Now;
            Calendar1.SelectedDate = DateTime.Now;
        }
        else
        {
            Calendar1.VisibleDate = Convert.ToDateTime(TB_BeginTime.Text);
            Calendar1.SelectedDate = Convert.ToDateTime(TB_BeginTime.Text);
        }


        Calendar2.Visible = false;
        Calendar1.Visible = true;
    }
    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        TB_BeginTime.Text = Calendar1.SelectedDate.ToShortDateString();
        Calendar1.Visible = false;
    }
    protected void BT_Calendar2_Click(object sender, EventArgs e)
    {
        if (TB_EndTime.Text.Trim().Length == 0)
        {
            Calendar2.VisibleDate = DateTime.Now;
            Calendar2.SelectedDate = DateTime.Now;
        }
        else
        {
            Calendar2.VisibleDate = Convert.ToDateTime(TB_EndTime.Text);
            Calendar2.SelectedDate = Convert.ToDateTime(TB_EndTime.Text);
        }

        Calendar2.Visible = true;
        Calendar1.Visible = false;
    }

    protected void Calendar2_SelectionChanged(object sender, EventArgs e)
    {
        TB_EndTime.Text = Calendar2.SelectedDate.ToShortDateString();
        Calendar2.Visible = false;
    }

    protected void bt_QueryByID_Click(object sender, EventArgs e)
    {
        ClearInputText();
        GridView1.SelectedIndex = -1;
        try
        {
            int id = Convert.ToInt32(tb_QueryID.Text);

            StringBuilder strSql = new StringBuilder(" SELECT * from V_RCJProjectCostPerformanceList where projectid=");
            strSql.Append(ProjectId.ToString());
            strSql.Append(" and ItemType=");
            strSql.Append(DDL_PerformanceType.SelectedValue);
            strSql.Append(" and itemno=");
            strSql.Append(id);

            DataSet res = ShareClass.GetDataSetFromSqlNOOperateLog(strSql.ToString(), "V_RCJProjectCostPerformanceList");


            GridView1.DataSource = res;
            GridView1.DataBind();
        }
        catch
        {
            //���������󣬻���Ϊ�գ�����ʾ���м�¼
            InitDataList();
        }
    }
}