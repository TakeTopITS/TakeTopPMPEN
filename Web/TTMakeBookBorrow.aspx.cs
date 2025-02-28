using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTMakeBookBorrow : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "借阅管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_BorrowDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityAsset(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
            //BindDDL();
            BindDDLOther();
            GetBackDate(DLC_BorrowDate.Text.Trim(), ddl_ReaderTypeId.SelectedValue.Trim());
            LoadBookList(txt_BarCode.Text.Trim(), txt_BookName.Text.Trim(), txt_ReferenceNo.Text.Trim(), txt_Author.Text.Trim(), ddl_BookClassificationId.SelectedValue.Trim(), ddl_BookPublishersId.SelectedValue.Trim());
        }
    }

    protected void GetBackDate(string strborrowdate, string strReaderTypeId)
    {
        if (strReaderTypeId.Trim().Equals("0"))
        {
            DLC_BackDate.Text = strborrowdate;
            lbl_BorrowNum.Text = "0";
            TB_Remark.Text = "";
        }
        else
        {
            string strHQL = "from BookReaderType as bookReaderType Where bookReaderType.ID='" + strReaderTypeId.Trim() + "' ";
            BookReaderTypeBLL bookReaderTypeBLL = new BookReaderTypeBLL();
            IList lst = bookReaderTypeBLL.GetAllBookReaderType(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                BookReaderType bookReaderType = (BookReaderType)lst[0];
                lbl_BorrowNum.Text = bookReaderType.BorrowNum.ToString();
                int strBorrowDays = bookReaderType.BorrowDays;
                DLC_BackDate.Text = DateTime.Parse(strborrowdate).AddDays(strBorrowDays).ToString("yyyy-MM-dd");
                if (bookReaderType.BorrowDays > 0 && bookReaderType.BorrowNum > 0)
                {
                    TB_Remark.Text = LanguageHandle.GetWord("KeJieYueTianShu").ToString().Trim() + strBorrowDays.ToString() + LanguageHandle.GetWord("JieYueShuLiang").ToString().Trim() + lbl_BorrowNum.Text.Trim() + LanguageHandle.GetWord("BenFen").ToString().Trim();
                }
                else
                    TB_Remark.Text = "";
            }
            else
            {
                lbl_BorrowNum.Text = "0";
                DLC_BackDate.Text = strborrowdate;
                TB_Remark.Text = "";
            }
        }
    }

    protected void BindDDLOther()
    {
        string strHQL = "from BookCertificate as bookCertificate Order by bookCertificate.ID ASC";
        BookCertificateBLL bookCertificateBLL = new BookCertificateBLL();
        IList lst = bookCertificateBLL.GetAllBookCertificate(strHQL);
        ddl_CertificateId.DataSource = lst;
        ddl_CertificateId.DataBind();

        strHQL = "from BookPublishers as bookPublishers Order by bookPublishers.ID ASC";
        BookPublishersBLL bookPublishersBLL = new BookPublishersBLL();
        lst = bookPublishersBLL.GetAllBookPublishers(strHQL);
        ddl_BookPublishersId.DataSource = lst;
        ddl_BookPublishersId.DataBind();
        ddl_BookPublishersId.Items.Insert(0, new ListItem("--Select--", "0"));

        strHQL = "from BookReaderType as bookReaderType Order by bookReaderType.ID ASC";
        BookReaderTypeBLL bookReaderTypeBLL = new BookReaderTypeBLL();
        lst = bookReaderTypeBLL.GetAllBookReaderType(strHQL);
        ddl_ReaderTypeId.DataSource = lst;
        ddl_ReaderTypeId.DataBind();
        ddl_ReaderTypeId.Items.Insert(0, new ListItem("--Select--", "0"));

        //strHQL = "from BookClassification as bookClassification Order by bookClassification.ID ASC";
        //BookClassificationBLL bookClassificationBLL = new BookClassificationBLL();
        //lst = bookClassificationBLL.GetAllBookClassification(strHQL);
        strHQL = "Select *,ClassificationType || '_' || ClassificationCode Classification From T_BookClassification Order by ID ASC ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookClassification");
        ddl_BookClassificationId.DataSource = ds;
        ddl_BookClassificationId.DataBind();
        ddl_BookClassificationId.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void LoadBookList(string strBarCode, string strBookName, string strReferenceNo, string strAuthor, string strBookClassificationId, string strBookPublishersId)
    {
        string strDepartString = LB_DepartString.Text.Trim();

        string strHQL = " Select * From T_BookInformation Where BookNum>0 and Status<>'NotBorrowable' and Status<>'Obsolete' ";
        strHQL += " and DepartCode in " + strDepartString;
        if (!string.IsNullOrEmpty(strBarCode.Trim()))
        {
            strHQL += " and BarCode like '%" + strBarCode.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(strBookName.Trim()))
        {
            strHQL += " and BookName like '%" + strBookName.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(strReferenceNo.Trim()))
        {
            strHQL += " and ReferenceNo like '%" + strReferenceNo.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(strAuthor.Trim()))
        {
            strHQL += " and Author like '%" + strAuthor.Trim() + "%' ";
        }
        if (!strBookClassificationId.Trim().Equals("0"))
        {
            strHQL += " and BookClassificationId = '" + strBookClassificationId.Trim() + "' ";
        }
        if (!strBookPublishersId.Trim().Equals("0"))
        {
            strHQL += " and BookPublishersId = '" + strBookPublishersId.Trim() + "' ";
        }
        if (DropDownList1.SelectedValue.Trim().Equals("1"))
        {
            strHQL += " and BookType = 'Book' ";   
        }
        else if (DropDownList1.SelectedValue.Trim().Equals("2"))
        {
            strHQL += " and BookType = 'Standard' ";   
        }
        strHQL += " Order By ID DESC ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookInformation");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL, strID;
            IList lst;

            strID = e.Item.Cells[1].Text.Trim();
            TextBox txtBookUseNum = (TextBox)e.Item.FindControl("txtBookUseNum");

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            if (string.IsNullOrEmpty(TB_ApplicantCode.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJYRBNWKJC").ToString().Trim() + "')", true);
                TB_ApplicantCode.Focus();
                return;
            }
            if (!IsNumeric(txtBookUseNum.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDJYSLYDY0DZSJC").ToString().Trim() + "')", true);
                txtBookUseNum.Focus();
                return;
            }
            if (txtBookUseNum.Text.Trim().Contains(".") || txtBookUseNum.Text.Trim().Contains("-"))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDJYSLYDY0DZSJC").ToString().Trim() + "')", true);
                txtBookUseNum.Focus();
                return;
            }
            if (DateTime.Parse(DLC_BorrowDate.Text.Trim()) > DateTime.Parse(DLC_BackDate.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWJYSJBNDYYJGHSJJC").ToString().Trim() + "')", true);
                return;
            }
            if (GetBookBorrowNum(TB_ApplicantCode.Text.Trim()) + int.Parse(txtBookUseNum.Text.Trim()) > int.Parse(lbl_BorrowNum.Text.Trim()) && int.Parse(lbl_BorrowNum.Text.Trim()) > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWJYZSLYCCYHKJSLXZJC").ToString().Trim() + "')", true);
                return;
            }

            strHQL = "from BookInformation as bookInformation where bookInformation.ID = '" + strID + "' ";
            BookBorrowRecordBLL bookBorrowRecordBLL = new BookBorrowRecordBLL();
            BookInformationBLL bookInformationBLL = new BookInformationBLL();
            lst = bookInformationBLL.GetAllBookInformation(strHQL);
            BookInformation bookInformation = (BookInformation)lst[0];
            BookBorrowRecord bookBorrowRecord = new BookBorrowRecord();
            bookBorrowRecord.Status = LanguageHandle.GetWord("JieChu").ToString().Trim();
            bookBorrowRecord.BookInfoId = bookInformation.ID;
            bookBorrowRecord.BarCode = bookInformation.BarCode;
            bookBorrowRecord.BookName = bookInformation.BookName;
            bookBorrowRecord.ReferenceNo = bookInformation.ReferenceNo;
            bookBorrowRecord.BookClassificationId = bookInformation.BookClassificationId;
            bookBorrowRecord.BookClassificationName = bookInformation.BookClassificationName;
            bookBorrowRecord.BookPublishersId = bookInformation.BookPublishersId;
            bookBorrowRecord.BookPublishersName = bookInformation.BookPublishersName;
            bookBorrowRecord.BookUseNum = int.Parse(txtBookUseNum.Text.Trim());
            bookBorrowRecord.Version = bookInformation.Version;
            bookBorrowRecord.BorrowCode = TB_ApplicantCode.Text.Trim();
            bookBorrowRecord.BorrowName = LB_ApplicantName.Text.Trim();
            bookBorrowRecord.BorrowDate = DateTime.Parse(DLC_BorrowDate.Text.Trim());
            bookBorrowRecord.BackDate = DateTime.Parse(DLC_BackDate.Text.Trim());
            bookBorrowRecord.ReaderTypeId = int.Parse(ddl_ReaderTypeId.SelectedValue.Trim());
            bookBorrowRecord.ReaderTypeName = GetBookReaderTypeName(bookBorrowRecord.ReaderTypeId);
            bookBorrowRecord.CertificateId = int.Parse(ddl_CertificateId.SelectedValue.Trim());
            bookBorrowRecord.CertificateName = GetBookCertificateName(bookBorrowRecord.CertificateId);
            bookBorrowRecord.CertificateNo = txt_CertificateNo.Text.Trim();
            bookBorrowRecord.Remark = TB_Remark.Text.Trim();
            bookBorrowRecord.RealBackDate = DateTime.Parse(DLC_BackDate.Text.Trim());
            bookBorrowRecord.BookRent = 0;

            if (int.Parse(txtBookUseNum.Text.Trim()) > bookInformation.BookNum)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWJYSLYCCSJSLJC").ToString().Trim() + "')", true);
                return;
            }
            bookBorrowRecordBLL.AddBookBorrowRecord(bookBorrowRecord);
            UpdateBookInformation(bookBorrowRecord.BookUseNum.ToString(), "1", LanguageHandle.GetWord("JieChu").ToString().Trim(), bookBorrowRecord.BookInfoId.ToString());//更新书籍信息表
            LoadBookBorrowRecord(TB_ApplicantCode.Text.Trim());
            LoadBookList(txt_BarCode.Text.Trim(), txt_BookName.Text.Trim(), txt_ReferenceNo.Text.Trim(), txt_Author.Text.Trim(), ddl_BookClassificationId.SelectedValue.Trim(), ddl_BookPublishersId.SelectedValue.Trim());
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYHJYSJCG").ToString().Trim() + "')", true);
        }
    }

    //判断输入的字符是否是数字
    private bool IsNumeric(string str)
    {
        System.Text.RegularExpressions.Regex reg1
            = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
        return reg1.IsMatch(str);
    }

    protected void DataGrid3_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        TB_ApplicantCode.Text = strUserCode;
        ProjectMember projectMember = GetProjectMember(strUserCode);
        if (projectMember != null)
        {
            LB_ApplicantName.Text = projectMember.UserName.Trim();
            lbl_ApplicantUnit.Text = projectMember.DepartName.Trim();
            lbl_ApplicantType.Text = projectMember.WorkType.Trim();
            lbl_ApplicantOfficePh.Text = projectMember.OfficePhone.Trim();
            lbl_ApplicantMobilePh.Text = projectMember.MobilePhone.Trim();
            ddl_ReaderTypeId.SelectedValue = GetBookReaderTypeId(projectMember.WorkType.Trim());

            GetBackDate(DLC_BorrowDate.Text.Trim(), ddl_ReaderTypeId.SelectedValue.Trim());
        }
        LoadBookBorrowRecord(strUserCode);
    }

    protected ProjectMember GetProjectMember(string strUserCode)
    {
        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = '" + strUserCode + "' ";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];
            return projectMember;
        }
        else
            return null;
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;
        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void ddl_ReaderTypeId_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetBackDate(DLC_BorrowDate.Text.Trim(), ddl_ReaderTypeId.SelectedValue.Trim());
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL, strID;
            IList lst;

            strID = e.Item.Cells[1].Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from BookBorrowRecord as bookBorrowRecord where bookBorrowRecord.ID = '" + strID + "' ";
            BookBorrowRecordBLL bookBorrowRecordBLL = new BookBorrowRecordBLL();
            lst = bookBorrowRecordBLL.GetAllBookBorrowRecord(strHQL);
            BookBorrowRecord bookBorrowRecord = (BookBorrowRecord)lst[0];
            bookBorrowRecord.Status = LanguageHandle.GetWord("GuiHai").ToString().Trim();
            bookBorrowRecord.RealBackDate = DateTime.Parse(DateTime.Now.ToString());

            bookBorrowRecordBLL.UpdateBookBorrowRecord(bookBorrowRecord, bookBorrowRecord.ID);
            UpdateBookInformation(bookBorrowRecord.BookUseNum.ToString(), "0", LanguageHandle.GetWord("GuiHai").ToString().Trim(), bookBorrowRecord.BookInfoId.ToString());//更新书籍信息表
            LoadBookBorrowRecord(TB_ApplicantCode.Text.Trim());
            LoadBookList(txt_BarCode.Text.Trim(), txt_BookName.Text.Trim(), txt_ReferenceNo.Text.Trim(), txt_Author.Text.Trim(), ddl_BookClassificationId.SelectedValue.Trim(), ddl_BookPublishersId.SelectedValue.Trim());
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYHGHSJCG").ToString().Trim() + "')", true);
        }
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookInformation");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBookList(txt_BarCode.Text.Trim(), txt_BookName.Text.Trim(), txt_ReferenceNo.Text.Trim(), txt_Author.Text.Trim(), ddl_BookClassificationId.SelectedValue.Trim(), ddl_BookPublishersId.SelectedValue.Trim());
    }

    protected void LoadBookBorrowRecord(string strBorrowCode)
    {
        string strHQL = " Select * From T_BookBorrowRecord Where BorrowCode='" + strBorrowCode + "' Order By ID DESC ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookBorrowRecord");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected int GetBookBorrowNum(string strBorrowCode)
    {
        string strHQL = " Select * From T_BookBorrowRecord Where BorrowCode='" + strBorrowCode + "' and Status<>'Return' Order By ID DESC";   
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookBorrowRecord");
        if (ds.Tables[0].Rows.Count > 0 && ds != null)
        {
            int num = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                num += int.Parse(ds.Tables[0].Rows[i]["BookUseNum"].ToString());
            }
            return num;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 更改UseNum(借阅数次)、BookUseNum(借出数量)、BookNum(数量)
    /// </summary>
    /// <param name="strBookUseNum"></param>
    /// <param name="strUseNum"></param>
    /// <param name="strStatus"></param>
    /// <param name="strBookInfoId"></param>
    protected void UpdateBookInformation(string strBookUseNum, string strUseNum, string strStatus, string strBookInfoId)
    {
        BookInformationBLL bookInformationBLL = new BookInformationBLL();
        string strHQL = "from BookInformation as bookInformation where bookInformation.ID = '" + strBookInfoId + "' ";
        IList lst = bookInformationBLL.GetAllBookInformation(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BookInformation bookInformation = (BookInformation)lst[0];
            if (strStatus.Trim().Equals(LanguageHandle.GetWord("GuiHai").ToString().Trim()))
            {
                bookInformation.BookUseNum = bookInformation.BookUseNum - int.Parse(strBookUseNum);
                bookInformation.BookNum = bookInformation.BookNum + int.Parse(strBookUseNum);
            }
            else
            {
                bookInformation.BookUseNum = bookInformation.BookUseNum + int.Parse(strBookUseNum);
                bookInformation.BookNum = bookInformation.BookNum - int.Parse(strBookUseNum);
                bookInformation.UseNum = bookInformation.UseNum + int.Parse(strUseNum);
            }
            bookInformationBLL.UpdateBookInformation(bookInformation, bookInformation.ID);
        }
    }

    protected void DataGrid2_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        Button btn_Back = (Button)e.Item.FindControl("BT_ID");
        HiddenField hfStatus = (HiddenField)e.Item.FindControl("hfStatus");
        if (hfStatus != null)
        {
            if (hfStatus.Value.Trim().Equals(LanguageHandle.GetWord("JieChu").ToString().Trim()))
            {
                if (btn_Back != null)
                {
                    btn_Back.Visible = true;
                }
                else
                {
                    btn_Back.Visible = false;
                }
            }
            else if (hfStatus.Value.Trim().Equals(LanguageHandle.GetWord("GuiHai").ToString().Trim()))
            {
                if (btn_Back != null)
                {
                    btn_Back.Visible = false;
                }
                else
                {
                    btn_Back.Visible = false;
                }
            }
        }
    }

    /// <summary>
    /// 根据借阅者ID获取借阅者类型名称
    /// </summary>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetBookReaderTypeName(int strId)
    {
        string strHQL = " Select TypeName From T_BookReaderType Where ID='" + strId.ToString() + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BookReaderType").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return dt.Rows[0]["TypeName"].ToString();
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 根据用工类型名称获取类型ID
    /// </summary>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetBookReaderTypeId(string strTypeName)
    {
        string strHQL = " Select ID From T_BookReaderType Where TypeName='" + strTypeName + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BookReaderType").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return dt.Rows[0]["ID"].ToString();
        }
        else
        {
            return "0";
        }
    }

    /// <summary>
    /// 根据借阅者证件ID获取借阅者证件名称
    /// </summary>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetBookCertificateName(int strId)
    {
        string strHQL = " Select CertificateName From T_BookCertificate Where ID='" + strId.ToString() + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BookCertificate").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return dt.Rows[0]["CertificateName"].ToString();
        }
        else
        {
            return "";
        }
    }

    protected void TB_ApplicantCode_TextChanged(object sender, EventArgs e)
    {
        string strUserCode = TB_ApplicantCode.Text.Trim();
        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = '" + strUserCode + "' or projectMember.UserName='" + strUserCode + "' ";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];
            TB_ApplicantCode.Text = projectMember.UserCode.Trim();
            LB_ApplicantName.Text = projectMember.UserName.Trim();
            lbl_ApplicantUnit.Text = projectMember.DepartName.Trim();
            lbl_ApplicantType.Text = projectMember.WorkType.Trim();
            lbl_ApplicantOfficePh.Text = projectMember.OfficePhone.Trim();
            lbl_ApplicantMobilePh.Text = projectMember.MobilePhone.Trim();
            ddl_ReaderTypeId.SelectedValue = GetBookReaderTypeId(projectMember.WorkType.Trim());

            GetBackDate(DLC_BorrowDate.Text.Trim(), ddl_ReaderTypeId.SelectedValue.Trim());
            LoadBookBorrowRecord(TB_ApplicantCode.Text.Trim());
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGJYRBCZJC").ToString().Trim() + "')", true);
            TB_ApplicantCode.Focus();
        }
    }

    protected void BT_Select_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Query1_Click(object sender, EventArgs e)
    {
        string strUserCode = TB_ApplicantCode.Text.Trim();
        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = '" + strUserCode + "' or projectMember.UserName='" + strUserCode + "' ";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];
            TB_ApplicantCode.Text = projectMember.UserCode.Trim();
            LB_ApplicantName.Text = projectMember.UserName.Trim();
            lbl_ApplicantUnit.Text = projectMember.DepartName.Trim();
            lbl_ApplicantType.Text = projectMember.WorkType.Trim();
            lbl_ApplicantOfficePh.Text = projectMember.OfficePhone.Trim();
            lbl_ApplicantMobilePh.Text = projectMember.MobilePhone.Trim();
            ddl_ReaderTypeId.SelectedValue = GetBookReaderTypeId(projectMember.WorkType.Trim());

            GetBackDate(DLC_BorrowDate.Text.Trim(), ddl_ReaderTypeId.SelectedValue.Trim());
            LoadBookBorrowRecord(TB_ApplicantCode.Text.Trim());
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGJYRBCZJC").ToString().Trim() + "')", true);
            TB_ApplicantCode.Focus();
        }
    }
}
