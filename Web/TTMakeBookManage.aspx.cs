using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;

public partial class TTMakeBookManage : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strDepartString;
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","标准与图书查询", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            txt_Borrow.Text = strUserCode;

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            BindDDLOther();
            LoadBookList(txt_BarCode.Text.Trim(), txt_BookName.Text.Trim(), txt_ReferenceNo.Text.Trim(), txt_Author.Text.Trim(), ddl_BookClassificationId.SelectedValue.Trim(), ddl_BookPublishersId.SelectedValue.Trim(),"");
            LoadBookBorrowRecord(txt_Borrow.Text.Trim(), txt_BookInfo.Text.Trim(), ddl_ReaderTypeId.SelectedValue.Trim(), ddl_CertificateId.SelectedValue.Trim());
        }
    }

    protected void BindDDLOther()
    {
        string strHQL = "from BookCertificate as bookCertificate Order by bookCertificate.ID ASC";
        BookCertificateBLL bookCertificateBLL = new BookCertificateBLL();
        IList lst = bookCertificateBLL.GetAllBookCertificate(strHQL);
        ddl_CertificateId.DataSource = lst;
        ddl_CertificateId.DataBind();
        ddl_CertificateId.Items.Insert(0, new ListItem("--Select--", "0"));

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

        strHQL = "from BookClassification as bookClassification Order by bookClassification.ID ASC";
        BookClassificationBLL bookClassificationBLL = new BookClassificationBLL();
        lst = bookClassificationBLL.GetAllBookClassification(strHQL);
        ddl_BookClassificationId.DataSource = lst;
        ddl_BookClassificationId.DataBind();
        ddl_BookClassificationId.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void LoadBookList(string strBarCode, string strBookName, string strReferenceNo, string strAuthor, string strBookClassificationId, string strBookPublishersId, string strBelongDepartCode)
    {
        string strHQL = "Select * From T_BookInformation Where 1=1 ";
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
        if (!string.IsNullOrEmpty(strBelongDepartCode))
        {
            strHQL += " and DepartCode like '%" + strBelongDepartCode.Trim() + "%' ";
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
        if (DropDownList1.SelectedValue.Trim().Equals("0"))
        {
            strHQL += " Order By ID ASC ";
        }
        else if (DropDownList1.SelectedValue.Trim().Equals("1"))//图书
        {
            strHQL += " and BookType='Book' Order By ReferenceNo ASC ";   
        }
        else//标准
        {
            strHQL += " and BookType='Standard' Order By BarCode ASC ";   
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookInformation");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void LoadBookBorrowRecord(string strBorrow, string strBook, string strReaderTypeId, string strCertificateId)
    {
        string strHQL = "Select bookBorrowRecord.*,projectMember.DepartName as BorrowUnit,projectMember.OfficePhone as BorrowPhone,projectMember.MobilePhone as BorrowMobile," +
            "projectMember.WorkType as BorrowType From T_BookBorrowRecord as bookBorrowRecord,T_ProjectMember as projectMember Where bookBorrowRecord.BorrowCode=projectMember.UserCode " +
            "and bookBorrowRecord.BookInfoId in (Select ID From T_BookInformation Where 1=1 ";

        if (!string.IsNullOrEmpty(txt_BarCode.Text.Trim()))
        {
            strHQL += " and BarCode like '%" + txt_BarCode.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(txt_BookName.Text.Trim()))
        {
            strHQL += " and BookName like '%" + txt_BookName.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(txt_ReferenceNo.Text.Trim()))
        {
            strHQL += " and ReferenceNo like '%" + txt_ReferenceNo.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(txt_Author.Text.Trim()))
        {
            strHQL += " and Author like '%" + txt_Author.Text.Trim() + "%' ";
        }
        if (!ddl_BookClassificationId.SelectedValue.Trim().Equals("0"))
        {
            strHQL += " and BookClassificationId = '" + ddl_BookClassificationId.SelectedValue.Trim() + "' ";
        }
        if (!ddl_BookPublishersId.SelectedValue.Trim().Equals("0"))
        {
            strHQL += " and BookPublishersId = '" + ddl_BookPublishersId.SelectedValue.Trim() + "' ";
        }
        strHQL += ")";

        if (!string.IsNullOrEmpty(strBorrow.Trim()))
        {
            strHQL += " and (bookBorrowRecord.BorrowCode like '%" + strBorrow.Trim() + "%' or bookBorrowRecord.BorrowName like '%" + strBorrow.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(strBook.Trim()))
        {
            strHQL += " and (bookBorrowRecord.BookName like '%" + strBook.Trim() + "%' or bookBorrowRecord.BarCode like '%" + strBook.Trim() + "%' or bookBorrowRecord.ReferenceNo like '%" + strBook.Trim() + "%') ";
        }
        if (!strReaderTypeId.Trim().Equals("0"))
        {
            strHQL += " and bookBorrowRecord.ReaderTypeId = '" + strReaderTypeId.Trim() + "' ";
        }
        if (!strCertificateId.Trim().Equals("0"))
        {
            strHQL += " and bookBorrowRecord.CertificateId = '" + strCertificateId.Trim() + "' ";
        }
        if (DropDownList2.SelectedValue.Trim().Equals("0"))
        {
            strHQL += " Order By bookBorrowRecord.BookName ASC ";
        }
        else
        {
            strHQL += " Order By bookBorrowRecord.BarCode ASC ";
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookBorrowRecord");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql2.Text = strHQL;
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

            TB_BelongDepartCode.Text = strDepartCode;
            LB_BelongDepartName.Text = strDepartName;
        }
    }

    protected void BT_QueryBorrow_Click(object sender, EventArgs e)
    {
        LoadBookList(txt_BarCode.Text.Trim(), txt_BookName.Text.Trim(), txt_ReferenceNo.Text.Trim(), txt_Author.Text.Trim(), ddl_BookClassificationId.SelectedValue.Trim(), ddl_BookPublishersId.SelectedValue.Trim(), TB_BelongDepartCode.Text.Trim());
        LoadBookBorrowRecord(txt_Borrow.Text.Trim(), txt_BookInfo.Text.Trim(), ddl_ReaderTypeId.SelectedValue.Trim(), ddl_CertificateId.SelectedValue.Trim());
    }

    protected void BT_QueryBook_Click(object sender, EventArgs e)
    {
        LoadBookList(txt_BarCode.Text.Trim(), txt_BookName.Text.Trim(), txt_ReferenceNo.Text.Trim(), txt_Author.Text.Trim(), ddl_BookClassificationId.SelectedValue.Trim(), ddl_BookPublishersId.SelectedValue.Trim(), TB_BelongDepartCode.Text.Trim());
        LoadBookBorrowRecord(txt_Borrow.Text.Trim(), txt_BookInfo.Text.Trim(), ddl_ReaderTypeId.SelectedValue.Trim(), ddl_CertificateId.SelectedValue.Trim());
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql2.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookBorrowRecord");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookInformation");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void BT_BookExportExcel_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = string.Empty;
                if (DropDownList1.SelectedValue.Trim().Equals("0"))
                {
                    fileName = LanguageHandle.GetWord("BiaoZhunYuTuShuXinXi").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                else if (DropDownList1.SelectedValue.Trim().Equals("1"))
                {
                    fileName = LanguageHandle.GetWord("TuShuXinXi").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                else
                {
                    fileName = LanguageHandle.GetWord("BiaoZhunXinXi").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                CreateExcel(getExportBookList(), fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected DataTable getExportBookList()
    {
        string strHQL = "Select BookType 'Type',BookClassificationName 'StandardOrBookClassification',ClassificationCode 'ClassificationCode',ReferenceNo 'RegistrationNumber',BarCode 'StandardNumber',BookName 'StandardOrBookName'," +   
            "BookPublishersName 'Publisher',Author 'Author',Translator 'Translator',BookNum 'AvailableForLoan',BookUseNum 'BorrowedQuantity',Price 'Price',Introduction 'Remark',PublicationDate '出版/实施日期' From T_BookInformation Where 1=1 ";   
        if (!string.IsNullOrEmpty(txt_BarCode.Text.Trim()))
        {
            strHQL += " and BarCode like '%" + txt_BarCode.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(txt_BookName.Text.Trim()))
        {
            strHQL += " and BookName like '%" + txt_BookName.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(txt_ReferenceNo.Text.Trim()))
        {
            strHQL += " and ReferenceNo like '%" + txt_ReferenceNo.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(TB_BelongDepartCode.Text.Trim()))
        {
            strHQL += " and DepartCode like '%" + TB_BelongDepartCode.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(txt_Author.Text.Trim()))
        {
            strHQL += " and Author like '%" + txt_Author.Text.Trim() + "%' ";
        }
        if (!ddl_BookClassificationId.SelectedValue.Trim().Equals("0"))
        {
            strHQL += " and BookClassificationId = '" + ddl_BookClassificationId.SelectedValue.Trim() + "' ";
        }
        if (DropDownList1.SelectedValue.Trim().Equals("0"))
        {
            strHQL += " Order By ID ASC ";
        }
        else if (DropDownList1.SelectedValue.Trim().Equals("1"))//图书
        {
            strHQL += " and BookType='Book' Order By ReferenceNo ASC ";   
        }
        else//标准
        {
            strHQL += " and BookType='Standard' Order By BarCode ASC ";   
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookInformation");
        return ds.Tables[0];
    }

    private void CreateExcel(DataTable dt, string fileName)
    {
        DataGrid dg = new DataGrid();
        dg.DataSource = dt;
        dg.DataBind();

        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/vnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dg.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();
    }

    protected void BT_BorrowExportExcel_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = LanguageHandle.GetWord("ShuJiJieYueQingKuang").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";

                CreateExcel(getExportBookBorrowList(), fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected DataTable getExportBookBorrowList()
    {
        string strHQL = " Select bookBorrowRecord.BookClassificationName 'StandardOrBookClassification',bookBorrowRecord.ReferenceNo 'RegistrationNumber',bookBorrowRecord.BarCode 'StandardNumber'," +   
            LanguageHandle.GetWord("bookBorrowRecordBookNameBiaoZh").ToString().Trim() +
            LanguageHandle.GetWord("bookBorrowRecordBorrowCodeJieY").ToString().Trim() +
            "projectMember.MobilePhone as 'MobileNumber',bookBorrowRecord.ReaderTypeName as 'BorrowerType' From T_BookBorrowRecord as bookBorrowRecord,T_ProjectMember as projectMember Where bookBorrowRecord.BorrowCode=projectMember.UserCode " +   
            "and bookBorrowRecord.BookInfoId in (Select ID From T_BookInformation Where 1=1 ";

        if (!string.IsNullOrEmpty(txt_BarCode.Text.Trim()))
        {
            strHQL += " and BarCode like '%" + txt_BarCode.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(txt_BookName.Text.Trim()))
        {
            strHQL += " and BookName like '%" + txt_BookName.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(txt_ReferenceNo.Text.Trim()))
        {
            strHQL += " and ReferenceNo like '%" + txt_ReferenceNo.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(txt_Author.Text.Trim()))
        {
            strHQL += " and Author like '%" + txt_Author.Text.Trim() + "%' ";
        }
        if (!ddl_BookClassificationId.SelectedValue.Trim().Equals("0"))
        {
            strHQL += " and BookClassificationId = '" + ddl_BookClassificationId.SelectedValue.Trim() + "' ";
        }
        if (!ddl_BookPublishersId.SelectedValue.Trim().Equals("0"))
        {
            strHQL += " and BookPublishersId = '" + ddl_BookPublishersId.SelectedValue.Trim() + "' ";
        }
        strHQL += ")";

        if (!string.IsNullOrEmpty(txt_Borrow.Text.Trim()))
        {
            strHQL += " and (bookBorrowRecord.BorrowCode like '%" + txt_Borrow.Text.Trim() + "%' or bookBorrowRecord.BorrowName like '%" + txt_Borrow.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(txt_BookInfo.Text.Trim()))
        {
            strHQL += " and (bookBorrowRecord.BookName like '%" + txt_BookInfo.Text.Trim() + "%' or bookBorrowRecord.BarCode like '%" + txt_BookInfo.Text.Trim() + "%' or bookBorrowRecord.ReferenceNo like '%" + txt_BookInfo.Text.Trim() + "%') ";
        }
        if (!ddl_ReaderTypeId.SelectedValue.Trim().Equals("0"))
        {
            strHQL += " and bookBorrowRecord.ReaderTypeId = '" + ddl_ReaderTypeId.SelectedValue.Trim() + "' ";
        }
        if (!ddl_CertificateId.SelectedValue.Trim().Equals("0"))
        {
            strHQL += " and bookBorrowRecord.CertificateId = '" + ddl_CertificateId.SelectedValue.Trim() + "' ";
        }
        if (DropDownList2.SelectedValue.Trim().Equals("0"))
        {
            strHQL += " Order By bookBorrowRecord.BookName ASC ";
        }
        else
        {
            strHQL += " Order By bookBorrowRecord.BarCode ASC ";
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookBorrowRecord");
        return ds.Tables[0];
    }
    
}
