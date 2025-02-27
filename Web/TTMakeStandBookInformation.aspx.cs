using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class TTMakeStandBookInformation : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strDepartString;

        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "标准档案", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_PublicationDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lbl_CreateCode.Text = strUserCode;
            lbl_CreateName.Text = ShareClass.GetUserName(strUserCode);

            LoadBookType();

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityAsset(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            LoadBookList(strUserCode, "0");
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            TB_DepartCode.Text = strDepartCode;
            LB_DepartName.Text = ShareClass.GetDepartName(strDepartCode);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        lbl_ID.Text = "";
        TB_BarCode.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID, strBarCode;

        strID = lbl_ID.Text;
        strBarCode = TB_BarCode.Text.Trim();

        if (strID == "")
        {
            AddStandard();
        }
        else
        {
            UpdateStandard();
        }
    }

    protected void AddStandard()
    {
        if (string.IsNullOrEmpty(TB_BarCode.Text.Trim()) || TB_BarCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBZBHBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_BarCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        if (string.IsNullOrEmpty(TB_BookName.Text.Trim()) || TB_BookName.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBZMCBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_BookName.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        if (IsBookInformation(TB_BarCode.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBZBHZSJBZYCZJC").ToString().Trim() + "')", true);
            TB_BarCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
      
        BookInformationBLL bookInformationBLL = new BookInformationBLL();
        BookInformation bookInformation = new BookInformation();

        bookInformation.BarCode = TB_BarCode.Text.Trim();
        bookInformation.Author = TB_Author.Text.Trim();
        bookInformation.BookClassificationId = int.Parse(string.IsNullOrEmpty(DL_BookClassificationId.SelectedValue.Trim()) ? "0" : DL_BookClassificationId.SelectedValue.Trim());
        bookInformation.BookClassificationName = GetBookClassificationName(bookInformation.BookClassificationId);
        bookInformation.BookName = TB_BookName.Text.Trim();
        bookInformation.BookNum = int.Parse(NB_BookNum.Amount.ToString());
        bookInformation.BookPublishersId = int.Parse(string.IsNullOrEmpty(DL_BookPublishersId.SelectedValue.Trim()) ? "0" : DL_BookPublishersId.SelectedValue.Trim());
        bookInformation.BookPublishersName = GetBookPublishersName(bookInformation.BookPublishersId);
        bookInformation.BookUseNum = 0;
        bookInformation.CreateCode = lbl_CreateCode.Text.Trim();
        bookInformation.CreateName = lbl_CreateName.Text.Trim();
        bookInformation.CreateTime = DateTime.Now;
        bookInformation.DepartCode = TB_DepartCode.Text.Trim();
        bookInformation.DepartName = LB_DepartName.Text.Trim();
        bookInformation.Donors = TB_Donors.Text.Trim();
        bookInformation.Status = DL_Status.SelectedValue.Trim();
        bookInformation.Introduction = TB_Introduction.Text.Trim();
        bookInformation.Location = TB_Location.Text.Trim();
        bookInformation.PageNum = int.Parse(NB_PageNum.Amount.ToString());
        bookInformation.Price = NB_Price.Amount;
        bookInformation.PublicationDate = DateTime.Parse(string.IsNullOrEmpty(DLC_PublicationDate.Text.Trim()) ? DateTime.Now.ToString() : DLC_PublicationDate.Text.Trim());
        bookInformation.ReferenceNo = TB_ReferenceNo.Text.Trim();
        bookInformation.Source = DL_Source.SelectedValue.Trim();
        bookInformation.Translator = TB_Translator.Text.Trim();
        bookInformation.UseNum = 0;
        bookInformation.Version = TB_Version.Text.Trim();
        bookInformation.BookImage = HL_StandardDocURL.NavigateUrl;
        bookInformation.BookType = "标准"; 
        bookInformation.ClassificationCode = TextBox1.Text.Trim();

        try
        {
            bookInformationBLL.AddBookInformation(bookInformation);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

            LoadBookList(strUserCode, "0");
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

        }
    }

    protected void UpdateStandard()
    {
        if (string.IsNullOrEmpty(TB_BarCode.Text.Trim()) || TB_BarCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBZBHBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_BarCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        if (string.IsNullOrEmpty(TB_BookName.Text.Trim()) || TB_BookName.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBZMCBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_BookName.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        if (IsBookInformation(TB_BarCode.Text.Trim(), string.IsNullOrEmpty(lbl_ID.Text.Trim()) ? "0" : lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBZBHZSJBZYCZJC").ToString().Trim() + "')", true);
            TB_BarCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
      

        string strHQL = "From BookInformation as bookInformation where bookInformation.ID = '" + lbl_ID.Text.Trim() + "'";
        BookInformationBLL bookInformationBLL = new BookInformationBLL();
        IList lst = bookInformationBLL.GetAllBookInformation(strHQL);

        BookInformation bookInformation = (BookInformation)lst[0];

        bookInformation.BarCode = TB_BarCode.Text.Trim();
        bookInformation.Author = TB_Author.Text.Trim();
        bookInformation.BookClassificationId = int.Parse(string.IsNullOrEmpty(DL_BookClassificationId.SelectedValue.Trim()) ? "0" : DL_BookClassificationId.SelectedValue.Trim());
        bookInformation.BookClassificationName = GetBookClassificationName(bookInformation.BookClassificationId);
        bookInformation.BookName = TB_BookName.Text.Trim();
        bookInformation.BookNum = int.Parse(NB_BookNum.Amount.ToString());
        bookInformation.BookPublishersId = int.Parse(string.IsNullOrEmpty(DL_BookPublishersId.SelectedValue.Trim()) ? "0" : DL_BookPublishersId.SelectedValue.Trim());
        bookInformation.BookPublishersName = GetBookPublishersName(bookInformation.BookPublishersId);
        bookInformation.CreateCode = lbl_CreateCode.Text.Trim();
        bookInformation.CreateName = lbl_CreateName.Text.Trim();
        bookInformation.DepartCode = TB_DepartCode.Text.Trim();
        bookInformation.DepartName = LB_DepartName.Text.Trim();
        bookInformation.Donors = TB_Donors.Text.Trim();
        bookInformation.Status = DL_Status.SelectedValue.Trim();
        bookInformation.Introduction = TB_Introduction.Text.Trim();
        bookInformation.Location = TB_Location.Text.Trim();
        bookInformation.PageNum = int.Parse(NB_PageNum.Amount.ToString());
        bookInformation.Price = NB_Price.Amount;
        bookInformation.PublicationDate = DateTime.Parse(string.IsNullOrEmpty(DLC_PublicationDate.Text.Trim()) ? DateTime.Now.ToString() : DLC_PublicationDate.Text.Trim());
        bookInformation.ReferenceNo = TB_ReferenceNo.Text.Trim();
        bookInformation.Source = DL_Source.SelectedValue.Trim();
        bookInformation.Translator = TB_Translator.Text.Trim();
        bookInformation.Version = TB_Version.Text.Trim();
        bookInformation.BookImage = HL_StandardDocURL.NavigateUrl;
        bookInformation.BookType = "标准"; 
        bookInformation.ClassificationCode = TextBox1.Text.Trim();

        try
        {
            bookInformationBLL.UpdateBookInformation(bookInformation, bookInformation.ID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

            LoadBookList(strUserCode, "1");
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From BookInformation as bookInformation where bookInformation.ID = '" + strId + "'";
                BookInformationBLL bookInformationBLL = new BookInformationBLL();
                lst = bookInformationBLL.GetAllBookInformation(strHQL);

                BookInformation bookInformation = (BookInformation)lst[0];

                TB_BarCode.Text = bookInformation.BarCode;
                TB_Author.Text = bookInformation.Author;
                DL_BookClassificationId.SelectedValue = bookInformation.BookClassificationId.ToString();
                TB_BookName.Text = bookInformation.BookName;
                NB_BookNum.Amount = bookInformation.BookNum;
                DL_BookPublishersId.SelectedValue = bookInformation.BookPublishersId.ToString();
                lbl_CreateCode.Text = bookInformation.CreateCode;
                lbl_CreateName.Text = bookInformation.CreateName;
                TB_DepartCode.Text = bookInformation.DepartCode;
                LB_DepartName.Text = bookInformation.DepartName;
                TB_Donors.Text = bookInformation.Donors;
                DL_Status.SelectedValue = bookInformation.Status;
                TB_Introduction.Text = bookInformation.Introduction;
                TB_Location.Text = bookInformation.Location;
                NB_PageNum.Amount = bookInformation.PageNum;
                NB_Price.Amount = bookInformation.Price;
                DLC_PublicationDate.Text = bookInformation.PublicationDate.ToString("yyyy-MM-dd");
                TB_ReferenceNo.Text = bookInformation.ReferenceNo;
                DL_Source.SelectedValue = bookInformation.Source;
                TB_Translator.Text = bookInformation.Translator;
                TB_Version.Text = bookInformation.Version;
                HL_StandardDocURL.NavigateUrl = bookInformation.BookImage;
                HL_StandardDocURL.Text = Path.GetFileName(Server.MapPath(bookInformation.BookImage));
                lbl_ID.Text = bookInformation.ID.ToString();
                TextBox1.Text = bookInformation.ClassificationCode.Trim();

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                strHQL = "Delete From T_BookInformation Where ID = '" + strId + "' ";

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);

                    LoadBookList(strUserCode, "0");
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
                }
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="strUserCode">用户编码</param>
    /// <param name="strIndex">标签 0 新增，删除；1 更新</param>
    protected void LoadBookList(string strUserCode, string strIndex)
    {
        string strHQL;
        string strDepartString = LB_DepartString.Text.Trim();

        strHQL = "Select * From T_BookInformation Where BookType = '标准' "; 
        strHQL += " and DepartCode in " + strDepartString;

        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (BookName like '%" + TextBox2.Text.Trim() + "%' or ReferenceNo like '%" + TextBox2.Text.Trim() + "%' or BookClassificationName like '%" + TextBox2.Text.Trim() + "%' or Location like '%" + TextBox2.Text.Trim() + "%' or ClassificationCode like '%" + TextBox2.Text.Trim() + "%' or BarCode like '%" + TextBox2.Text.Trim() + "%')";
        }
        strHQL += " Order By ID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookInformation");

        if (strIndex == "0")
        {
            DataGrid2.CurrentPageIndex = 0;
        }

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void LoadBookType()
    {
        string strHQL;
        IList lst;
        //绑定分类BindDDL();
        strHQL = "Select ID,ClassificationType || '_' || ClassificationCode || '_' || Remark as ClassificationName From T_BookClassification Where DataType='标准分类' Order By ID ASC "; 
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookClassification");
        DL_BookClassificationId.DataSource = ds;
        DL_BookClassificationId.DataBind();
        DL_BookClassificationId.Items.Insert(0, new ListItem("--Select--", "0"));

        //绑定出版社
        strHQL = "From BookPublishers as bookPublishers Order By bookPublishers.ID ASC";
        BookPublishersBLL bookPublishersBLL = new BookPublishersBLL();
        lst = bookPublishersBLL.GetAllBookPublishers(strHQL);
        DL_BookPublishersId.DataSource = lst;
        DL_BookPublishersId.DataBind();
    }


    /// <summary>
    /// 根据分类ID获取分类名称
    /// </summary>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetBookClassificationName(int strId)
    {
        string strHQL = "Select ClassificationType From T_BookClassification Where ID='" + strId.ToString() + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BookClassification").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return dt.Rows[0]["ClassificationType"].ToString();
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 根据出版社ID获取出版社名称
    /// </summary>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetBookPublishersName(int strId)
    {
        string strHQL = " Select PublishersName From T_BookPublishers Where ID='" + strId.ToString() + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BookPublishers").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return dt.Rows[0]["PublishersName"].ToString();
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 更新及新增时，检查书籍条码是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBookInformation(string strBarCode, string strId)
    {
        bool flag = true;
        string strHQL;
        if (!string.IsNullOrEmpty(strId.Trim()))
        {
            strHQL = "Select BarCode From T_BookInformation Where BookType = '标准' and BarCode='" + strBarCode + "' and ID<>'" + strId + "' "; 
        }
        else
        {
            strHQL = "Select BarCode From T_BookInformation Where BookType = '标准' and BarCode='" + strBarCode + "'"; 
        }
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BookInformation").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    protected void BtnUP_Click(object sender, EventArgs e)
    {
        //上传规章制度附件
        if (AttachFile.HasFile)
        {
            string strFileName1, strExtendName;

            strFileName1 = this.AttachFile.FileName;//获取上传文件的文件名,包括后缀
            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);

            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName1) + '_' + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";

            string bookimage = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {
                //return "1";
            }
            else
            {
                try
                {
                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);

                    HL_StandardDocURL.NavigateUrl = bookimage;
                    HL_StandardDocURL.Text = strFileName3;
                    //return bookimage;
                }
                catch
                {
                    //return "2";
                }
            }
        }
        else
        {
            //return "0";
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

   
    protected void DL_BookClassificationId_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL = "Select ClassificationCode From T_BookClassification Where ID='" + DL_BookClassificationId.SelectedValue.Trim() + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BookClassification").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            TextBox1.Text = dt.Rows[0]["ClassificationCode"].ToString();
        }
        else
        {
            TextBox1.Text = "";
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookInformation");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBookList(strUserCode, "0");

    }

   
}
