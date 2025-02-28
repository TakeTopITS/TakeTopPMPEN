using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.Model;
using ProjectMgt.BLL;

public partial class TTKPITemplateLibrarySet : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "KPI模板库设置", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            LoadKPIType();
            ShareClass.InitialKPITree(TreeView1);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strKPIID, strKPI, strKPIType;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Depth == 1)
        {
            strKPIType = treeNode.Text;
            DL_KPIType.SelectedValue = strKPIType;

            return;
        }

        strKPIID = treeNode.Target.Trim();
        strKPI = treeNode.Text.Trim();

        strHQL = "From KPILibrary as kpiLibrary Where kpiLibrary.ID = " + strKPIID;
        KPILibraryBLL kpiLibraryBLL = new KPILibraryBLL();
        lst = kpiLibraryBLL.GetAllKPILibrarys(strHQL);
        KPILibrary kpiLibrary = new KPILibrary();

        if (lst.Count > 0)
        {
            kpiLibrary = (KPILibrary)lst[0];

            LB_KPIID.Text = kpiLibrary.ID.ToString();
            DL_KPIType.SelectedValue = kpiLibrary.KPIType;

            TB_KPI.Text = kpiLibrary.KPI.Trim();
            TB_Definition.Text = kpiLibrary.Definition.Trim();

            TB_Function.Text = kpiLibrary.KPIFunction.Trim();
            TB_Formula.Text = kpiLibrary.Formula.Trim();
            TB_SqlCode.Text = kpiLibrary.SqlCode.Trim();
            NB_UnitSqlPoint.Amount = kpiLibrary.UnitSqlPoint;
            TB_Source.Text = kpiLibrary.Source.Trim();

            NB_SortNubmer.Amount = kpiLibrary.SortNumber;

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        TB_KPINew.Text = "";
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        AddKPI();
    }

    protected void AddKPI()
    {
        string strKPIID, strKPI, strKPIType, strDefinition, strFunction, strFormula, strSource, strSqlCode;
        int intSortNumber;

        strKPI = TB_KPINew.Text.Trim();
        strKPIType = DL_KPITypeNew.SelectedValue.Trim();
        strDefinition = TB_DefinitionNew.Text.Trim();
        strFunction = TB_FunctionNew.Text.Trim();
        strFormula = TB_FormulaNew.Text.Trim();
        strSqlCode = TB_SqlCodeNew.Text.Trim();
        strSource = TB_SourceNew.Text.Trim();

        if (strSqlCode.ToLower().Contains("create") || strSqlCode.ToLower().Contains("execute") || strSqlCode.ToLower().Contains("delete") || strSqlCode.ToLower().Contains("update") || strSqlCode.ToLower().Contains("drop")
             || strSqlCode.ToLower().Contains("insert") || strSqlCode.ToLower().Contains("alter"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSQLDMBNHDELETEUPDATEDROPINSERTALTERYJJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            return;
        }


        intSortNumber = int.Parse(NB_SortNubmer.Amount.ToString());

        if (strKPI != "" | strKPIType != "" | strDefinition != "")
        {
            KPILibraryBLL kpiLibraryBLL = new KPILibraryBLL();
            KPILibrary kpiLibrary = new KPILibrary();

            try
            {
                kpiLibrary.KPI = strKPI;
                kpiLibrary.KPIType = strKPIType;
                kpiLibrary.Definition = strDefinition;
                kpiLibrary.KPIFunction = strFunction;
                kpiLibrary.Formula = strFormula;
                kpiLibrary.SqlCode = strSqlCode;
                kpiLibrary.UnitSqlPoint = NB_UnitSqlPoint.Amount;
                kpiLibrary.Source = strSource;

                kpiLibrary.SortNumber = intSortNumber;

                kpiLibraryBLL.AddKPILibrary(kpiLibrary);

                strKPIID = ShareClass.GetMyCreatedMaxKPIID();
                LB_KPIID.Text = strKPIID;

                ShareClass.InitialKPITree(TreeView1);

                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;

                DL_KPIType.SelectedValue = DL_KPITypeNew.SelectedValue;

                TB_KPI.Text = TB_KPINew.Text;
                TB_Definition.Text = TB_DefinitionNew.Text;
                TB_Function.Text = TB_FunctionNew.Text;
                TB_Formula.Text = TB_FormulaNew.Text;
                TB_SqlCode.Text = TB_SqlCodeNew.Text;
                NB_UnitSqlPoint.Amount = NB_UnitSqlPointNew.Amount;
                TB_Source.Text = TB_SourceNew.Text;

                NB_SortNubmer.Amount = NB_SortNubmerNew.Amount;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDXBNWKJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strKPIID, strKPI, strKPIType, strDefinition, strFunction, strFormula, strSource, strSqlCode;
        int intSortNumber;

        strKPIID = LB_KPIID.Text.Trim();
        strKPI = TB_KPI.Text.Trim();
        strKPIType = DL_KPIType.SelectedValue.Trim();
        strDefinition = TB_Definition.Text.Trim();
        strFunction = TB_Function.Text.Trim();
        strFormula = TB_Formula.Text.Trim();
        strSqlCode = TB_SqlCode.Text.Trim();
        strSource = TB_Source.Text.Trim();

        if (strSqlCode.ToLower().Contains("create") || strSqlCode.ToLower().Contains("execute") || strSqlCode.ToLower().Contains("delete") || strSqlCode.ToLower().Contains("update") || strSqlCode.ToLower().Contains("drop")
            || strSqlCode.ToLower().Contains("insert") || strSqlCode.ToLower().Contains("alter"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSQLDMBNHDELETEUPDATEDROPINSERTALTERYJJC").ToString().Trim() + "')", true);
            return;
        }

        intSortNumber = int.Parse(NB_SortNubmer.Amount.ToString());

        if (strKPI != "" | strKPIType != "" | strDefinition != "")
        {
            KPILibraryBLL kpiLibraryBLL = new KPILibraryBLL();
            KPILibrary kpiLibrary = new KPILibrary();

            try
            {
                kpiLibrary.KPI = strKPI;
                kpiLibrary.KPIType = strKPIType;
                kpiLibrary.Definition = strDefinition;
                kpiLibrary.KPIFunction = strFunction;
                kpiLibrary.Formula = strFormula;
                kpiLibrary.SqlCode = strSqlCode;
                kpiLibrary.UnitSqlPoint = NB_UnitSqlPoint.Amount;
                kpiLibrary.Source = strSource;

                kpiLibrary.SortNumber = intSortNumber;

                kpiLibraryBLL.UpdateKPILibrary(kpiLibrary, int.Parse(strKPIID));

                ShareClass.InitialKPITree(TreeView1);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDXBNWKJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strKPIID;

        strKPIID = LB_KPIID.Text.Trim();

        try
        {
            strHQL = "Delete From T_KPILibrary Where ID = " + strKPIID;
            ShareClass.RunSqlCommand(strHQL);

            ShareClass.InitialKPITree(TreeView1);

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void btn_ExcelToDataTraining_Click(object sender, EventArgs e)
    {
        string strKPI = "";
        int i = 0;

        string strUserCode = Session["UserCode"].ToString();

        if (ExcelToDBTest() == -1)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZDRSBEXECLBLDSJYCJC").ToString().Trim() ;
            return;
        }
        else
        {
            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim() ;
                return;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim() ;
                return;
            }
            string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim() ;
            }
            else
            {
                FileUpload_Training.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                string strpath = strDocSavePath + newfilename;

                //DataSet ds = ExcelToDataSet(strpath, filename);
                //DataRow[] dr = ds.Tables[0].Select();
                //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
                //int rowsnum = ds.Tables[0].Rows.Count;

                DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
                DataRow[] dr = dt.Select();                        //定义一个DataRow数组
                int rowsnum = dt.Rows.Count;
                if (rowsnum == 0)
                {
                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() ;
                }
                else
                {
                    KPILibraryBLL kpiLibraryBLL = new KPILibraryBLL();
                    KPILibrary kpiLibrary = new KPILibrary();

                    for (i = 0; i < dr.Length; i++)
                    {
                        strKPI = dr[i]["KPI"].ToString().Trim();

                        if (strKPI != "")
                        {
                            try
                            {
                                kpiLibrary.KPIType = dr[i]["KPI类型"].ToString().Trim();   
                                kpiLibrary.KPI = dr[i]["KPI"].ToString().Trim();
                                kpiLibrary.Definition = dr[i][LanguageHandle.GetWord("DingYi").ToString().Trim()].ToString().Trim();
                                kpiLibrary.Formula = dr[i][LanguageHandle.GetWord("GongShi").ToString().Trim()].ToString().Trim();
                                kpiLibrary.SqlCode = dr[i]["SQL代码"].ToString().Trim();   
                                kpiLibrary.UnitSqlPoint = decimal.Parse(dr[i]["UnitScore"].ToString().Trim());   
                                kpiLibrary.Source = dr[i][LanguageHandle.GetWord("LaiYuan").ToString().Trim()].ToString().Trim();
                                kpiLibrary.KPIFunction = dr[i][LanguageHandle.GetWord("GongNengHuoBeiZhu").ToString().Trim()].ToString().Trim();
                                kpiLibrary.SortNumber = int.Parse(dr[i][LanguageHandle.GetWord("ShunXuHao").ToString().Trim()].ToString().Trim());

                                kpiLibraryBLL.AddKPILibrary(kpiLibrary);
                            }
                            catch (Exception err)
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGDRSBJC").ToString().Trim() + " : " + LanguageHandle.GetWord("HangHao").ToString().Trim() + ": " + (i + 2).ToString() + " , " + LanguageHandle.GetWord("MingCheng").ToString().Trim() + ": " + strKPI + " : " + err.Message.ToString() + "<br/>"; ;

                                LogClass.WriteLogFile(this.GetType().BaseType.Name + "：" + LanguageHandle.GetWord("ZZJGDRSBJC").ToString().Trim() + " : " + LanguageHandle.GetWord("HangHao").ToString().Trim() + ": " + (i + 2).ToString() + " , " + LanguageHandle.GetWord("MingCheng").ToString().Trim() + ": " + strKPI + " : " + err.Message.ToString());
                            }
                        }
                    }

                    LoadKPIType();
                    ShareClass.InitialKPITree(TreeView1);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRCG").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected int ExcelToDBTest()
    {
        string strKPI, strKPIType, strDefinition;
        string strUserCode = Session["UserCode"].ToString();

        int j = 0;

        try
        {
            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim() ;
                j = -1;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim() ;
                j = -1;
            }
            string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim() ;
                j = -1;
            }
            else
            {
                FileUpload_Training.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                string strpath = strDocSavePath + newfilename;

                //DataSet ds = ExcelToDataSet(strpath, filename);
                //DataRow[] dr = ds.Tables[0].Select();
                //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
                //int rowsnum = ds.Tables[0].Rows.Count;

                DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
                DataRow[] dr = dt.Select();                        //定义一个DataRow数组
                int rowsnum = dt.Rows.Count;
                if (rowsnum == 0)
                {
                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() ;
                    j = -1;
                }
                else
                {
                    KPILibraryBLL kpiLibraryBLL = new KPILibraryBLL();
                    KPILibrary kpiLibrary = new KPILibrary();

                    for (int i = 0; i < dr.Length; i++)
                    {
                        strKPIType = dr[i]["KPI类型"].ToString().Trim();   
                        strKPI = dr[i]["KPI"].ToString().Trim();
                        strDefinition = dr[i][LanguageHandle.GetWord("DingYi").ToString().Trim()].ToString().Trim();

                        if (strKPI == "" | strKPIType == "" | strDefinition == "")
                        {
                            j = -1;

                            LB_ErrorText.Text += "KPI:" + strKPI + LanguageHandle.GetWord("HuoKPILeiXing").ToString().Trim() + strKPIType + LanguageHandle.GetWord("HuoDingYi").ToString().Trim() + strDefinition + LanguageHandle.GetWord("WeiKong").ToString().Trim();

                            continue;
                        }

                        if (getKPITypeExistCount(strKPIType) == 0)
                        {
                            j = -1;

                            LB_ErrorText.Text += LanguageHandle.GetWord("KPILeiXing").ToString().Trim() + strKPIType + LanguageHandle.GetWord("BuCunZai").ToString().Trim();

                            continue;
                        }
                    }
                }
            }
        }
        catch (Exception err)
        {
            LB_ErrorText.Text += err.Message.ToString() + "<br/>";

            j = -1;
        }

        return j;
    }

    protected int getKPITypeExistCount(string strKPIType)
    {
        string strHQL;

        strHQL = "Select Type From T_KPIType Where Type = '" + strKPIType + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_KPIType");

        return ds.Tables[0].Rows.Count;
    }


    protected void LoadKPIType()
    {
        string strHQL;
        IList lst;

        strHQL = "From KPIType as kpiType Order By kpiType.SortNumber ASC";
        KPITypeBLL kpiTypeBLL = new KPITypeBLL();
        lst = kpiTypeBLL.GetAllKPITypes(strHQL);

        DL_KPIType.DataSource = lst;
        DL_KPIType.DataBind();

        DL_KPITypeNew.DataSource = lst;
        DL_KPITypeNew.DataBind();
    }
}
