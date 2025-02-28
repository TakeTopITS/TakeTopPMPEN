using System; using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using System.Text;

public partial class TTBookBaseData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;

        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","图书基础数据", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "基础数据设定---" + System.Configuration.ConfigurationManager.AppSettings["SystemName"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            LoadBookReaderType();//读者类型

            LoadBookPublishers();//出版社

            LoadBookClassification();//图书分类
            LoadBookClassification1();//标准分类
            BindDDL();//图书分类树结构

            LoadBookCertificate();//借阅证件
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strTypeID = e.Item.Cells[0].Text.Trim();
        string strTypeName = ((Button)e.Item.FindControl("BT_TypeName")).Text.Trim();
        string strBorrowDays = e.Item.Cells[2].Text.Trim();
        string strBorrowNum = e.Item.Cells[3].Text.Trim();

        LB_TypeID.Text = strTypeID;
        TB_TypeName.Text = strTypeName;
        ddl_TypeName.SelectedValue = strTypeName;
        TB_BorrowDays.Text = strBorrowDays;
        TB_BorrowNum.Text = strBorrowNum;
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strId = e.Item.Cells[0].Text.Trim();
        string strISBNNo = ((Button)e.Item.FindControl("BT_ISBNNo")).Text.Trim();
        string strPublishersName = e.Item.Cells[2].Text.Trim();
        string strPublishersAddress = e.Item.Cells[3].Text.Trim();

        lbl_BookPublishersId.Text = strId;
        TB_ISBNNo.Text = strISBNNo;
        TB_PublishersName.Text = strPublishersName;
        TB_PublishersAddress.Text = strPublishersAddress;
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strId = e.Item.Cells[0].Text.Trim();
        string strClassificationType = ((Button)e.Item.FindControl("BT_ClassificationName")).Text.Trim();
        string strRemark = e.Item.Cells[2].Text.Trim();

        lbl_BookClassificationId.Text = strId;
        TB_ClassificationType.Text = strClassificationType;
        TB_Remark.Text = strRemark;
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strId = e.Item.Cells[0].Text.Trim();
        string strCertificateName = ((Button)e.Item.FindControl("BT_CertificateName")).Text.Trim();
        string strSortNo = e.Item.Cells[2].Text.Trim();

        TB_CertificateName.Text = strCertificateName;
        TB_SortNoC.Text = strSortNo;
        lbl_BookCertificateId.Text = strId;
    }

    protected void BT_NewBookReaderType_Click(object sender, EventArgs e)
    {
     //   string strTypeName = TB_TypeName.Text.Trim();
        string strTypeName = ddl_TypeName.SelectedValue.Trim();
        string strBorrowDays = string.IsNullOrEmpty(TB_BorrowDays.Text.Trim()) ? "0" : TB_BorrowDays.Text.Trim();
        string strBorrowNum = string.IsNullOrEmpty(TB_BorrowNum.Text.Trim()) ? "0" : TB_BorrowNum.Text.Trim();
        if (string.IsNullOrEmpty(strTypeName))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZLXMCJC").ToString().Trim()+"')", true);
            ddl_TypeName.Focus();
            return;
        }
        if (!IsNumeric(strBorrowDays))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSRZDJYTSYDYHDY0DZSJC").ToString().Trim()+"')", true);
            TB_BorrowDays.Focus();
            return;
        }
        if (!IsNumeric(strBorrowNum))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSRZDJYSLYDYHDY0DZSJC").ToString().Trim()+"')", true);
            TB_BorrowNum.Focus();
            return;
        }
        if (strBorrowDays.Contains(".") || strBorrowDays.Contains("-"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSRZDJYTSYDYHDY0DZSJC").ToString().Trim()+"')", true);
            TB_BorrowDays.Focus();
            return;
        }
        if (strBorrowNum.Contains(".") || strBorrowNum.Contains("-"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSRZDJYSLYDYHDY0DZSJC").ToString().Trim()+"')", true);
            TB_BorrowNum.Focus();
            return;
        }
        if (IsBookReaderType(strTypeName))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGLXMCZSJBZYCZJC").ToString().Trim()+"')", true);
            ddl_TypeName.Focus();
            return;
        }

        BookReaderTypeBLL bookReaderTypeBLL = new BookReaderTypeBLL();
        BookReaderType bookReaderType = new BookReaderType();

        try
        {
            bookReaderType.TypeName = strTypeName;
            bookReaderType.BorrowDays = int.Parse(strBorrowDays);
            bookReaderType.BorrowNum = int.Parse(strBorrowNum);

            bookReaderTypeBLL.AddBookReaderType(bookReaderType);

            LoadBookReaderType();
        }
        catch
        {
        }
    }

    protected void BT_DeleteBookReaderType_Click(object sender, EventArgs e)
    {
        string strTypeID = LB_TypeID.Text.Trim();
   //     string strTypeName = TB_TypeName.Text.Trim();
        string strTypeName = ddl_TypeName.SelectedValue.Trim();
        string strBorrowDays = string.IsNullOrEmpty(TB_BorrowDays.Text.Trim()) ? "0" : TB_BorrowDays.Text.Trim();
        string strBorrowNum = string.IsNullOrEmpty(TB_BorrowNum.Text.Trim()) ? "0" : TB_BorrowNum.Text.Trim();

        if (string.IsNullOrEmpty(strTypeName))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZLXMCJC").ToString().Trim()+"')", true);
            ddl_TypeName.Focus();
            return;
        }
        if (!IsNumeric(strBorrowDays))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSRZDJYTSYDYHDY0DZSJC").ToString().Trim()+"')", true);
            TB_BorrowDays.Focus();
            return;
        }
        if (!IsNumeric(strBorrowNum))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSRZDJYSLYDYHDY0DZSJC").ToString().Trim()+"')", true);
            TB_BorrowNum.Focus();
            return;
        }
        if (strBorrowDays.Contains(".") || strBorrowDays.Contains("-"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSRZDJYTSYDYHDY0DZSJC").ToString().Trim()+"')", true);
            TB_BorrowDays.Focus();
            return;
        }
        if (strBorrowNum.Contains(".") || strBorrowNum.Contains("-"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSRZDJYSLYDYHDY0DZSJC").ToString().Trim()+"')", true);
            TB_BorrowNum.Focus();
            return;
        }
        if (IsBookReaderType(strTypeName, strTypeID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGLXMCZSJBZYCZJC").ToString().Trim()+"')", true);
            ddl_TypeName.Focus();
            return;
        }

        BookReaderTypeBLL bookReaderTypeBLL = new BookReaderTypeBLL();
        BookReaderType bookReaderType = new BookReaderType();

        try
        {
            bookReaderType.ID = int.Parse(strTypeID);
            bookReaderType.TypeName = strTypeName;
            bookReaderType.BorrowDays = int.Parse(strBorrowDays);
            bookReaderType.BorrowNum = int.Parse(strBorrowNum);

            bookReaderTypeBLL.UpdateBookReaderType(bookReaderType, bookReaderType.ID);

            LoadBookReaderType();
        }
        catch
        {
        }
    }

    protected void BT_BookPublishersNew_Click(object sender, EventArgs e)
    {
        string strISBNNo = TB_ISBNNo.Text.Trim();
        string strPublishersName = TB_PublishersName.Text.Trim();
        string strPublishersAddress = TB_PublishersAddress.Text.Trim();

        if (IsBookPublishers(strISBNNo, strPublishersName))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGCBSISBNBMHZCBSMZSJBZYCZJC").ToString().Trim()+"')", true);
            TB_ISBNNo.Focus();
            TB_PublishersName.Focus();
            return;
        }

        BookPublishersBLL bookPublishersBLL = new BookPublishersBLL();
        BookPublishers bookPublishers = new BookPublishers();

        try
        {
            bookPublishers.ISBNNo = strISBNNo;
            bookPublishers.PublishersName = strPublishersName;
            bookPublishers.PublishersAddress = strPublishersAddress;

            bookPublishersBLL.AddBookPublishers(bookPublishers);

            LoadBookPublishers();
        }
        catch
        {
        }
    }

    protected void BT_BookPublishersDelete_Click(object sender, EventArgs e)
    {
        string strId = lbl_BookPublishersId.Text.Trim();
        string strISBNNo = TB_ISBNNo.Text.Trim();
        string strPublishersName = TB_PublishersName.Text.Trim();
        string strPublishersAddress = TB_PublishersAddress.Text.Trim();

        if (IsBookPublishersBook(strId.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGCBSYBSYWFSC").ToString().Trim()+"')", true);
            return;
        }

        BookPublishersBLL bookPublishersBLL = new BookPublishersBLL();
        BookPublishers bookPublishers = new BookPublishers();

        try
        {
            bookPublishers.ID = int.Parse(strId);
            bookPublishers.ISBNNo = strISBNNo;
            bookPublishers.PublishersName = strPublishersName;
            bookPublishers.PublishersAddress = strPublishersAddress;

            bookPublishersBLL.DeleteBookPublishers(bookPublishers);

            LoadBookPublishers();
        }
        catch
        {
        }

    }
    protected void BT_BookClassificationNew_Click(object sender, EventArgs e)
    {
        string strClassificationType = TB_ClassificationType.Text.Trim();
        string strRemark = TB_Remark.Text.Trim();

        BookClassificationBLL bookClassificationBLL = new BookClassificationBLL();
        BookClassification bookClassification = new BookClassification();

        try
        {
            bookClassification.ClassificationType = strClassificationType;
            bookClassification.ClassificationCode = "";
            bookClassification.DataType = "BookClassification";   
            bookClassification.Remark = strRemark;

            bookClassificationBLL.AddBookClassification(bookClassification);

            LoadBookClassification();
            BindDDL();
        }
        catch
        {

        }
    }
    protected void BT_BookClassificationDelete_Click(object sender, EventArgs e)
    {
        string strClassificationType = TB_ClassificationType.Text.Trim();
        string strRemark = TB_Remark.Text.Trim();
        string strId = lbl_BookClassificationId.Text.Trim();

        BookClassificationBLL bookClassificationBLL = new BookClassificationBLL();
        BookClassification bookClassification = new BookClassification();

        try
        {
            bookClassification.ID = int.Parse(strId);
            bookClassification.ClassificationCode = "";
            bookClassification.ClassificationType = strClassificationType;
            bookClassification.DataType = "BookClassification";   
            bookClassification.Remark = strRemark;

            bookClassificationBLL.UpdateBookClassification(bookClassification,bookClassification.ID);

            LoadBookClassification();
            BindDDL();
        }
        catch
        {

        }
    }

    protected void BT_BookCertificateNew_Click(object sender, EventArgs e)
    {
        string strCertificateName = TB_CertificateName.Text.Trim();
        string strSortNo = string.IsNullOrEmpty(TB_SortNoC.Text.Trim()) ? "0" : TB_SortNoC.Text.Trim();

        if (!IsNumeric(strSortNo))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSRZDZJPXYDYHDY0DZSJC").ToString().Trim()+"')", true);
            TB_SortNoC.Focus();
            return;
        }
        if (strSortNo.Contains(".") || strSortNo.Contains("-"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSRZDZJPXYDYHDY0DZSJC").ToString().Trim()+"')", true);
            TB_SortNoC.Focus();
            return;
        }
        if (IsBookCertificate(strCertificateName))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGJYZJMCZSJBZYCZJC").ToString().Trim()+"')", true);
            TB_CertificateName.Focus();
            return;
        }

        BookCertificateBLL bookCertificateBLL = new BookCertificateBLL();
        BookCertificate bookCertificate = new BookCertificate();

        try
        {
            bookCertificate.CertificateName = strCertificateName;
            bookCertificate.SortNo = int.Parse(strSortNo);

            bookCertificateBLL.AddBookCertificate(bookCertificate);

            LoadBookCertificate();
        }
        catch
        {
        }

    }

    protected void BT_BookCertificateDelete_Click(object sender, EventArgs e)
    {
        string strCertificateName = TB_CertificateName.Text.Trim();
        string strSortNo = string.IsNullOrEmpty(TB_SortNoC.Text.Trim()) ? "0" : TB_SortNoC.Text.Trim();
        string strId = lbl_BookCertificateId.Text.Trim();

        if (IsBookCertificateBook(strId.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGZJYBSYWFSC").ToString().Trim()+"')", true);
            return;
        }

        BookCertificateBLL bookCertificateBLL = new BookCertificateBLL();
        BookCertificate bookCertificate = new BookCertificate();

        try
        {
            bookCertificate.CertificateName = strCertificateName;
            bookCertificate.SortNo = int.Parse(strSortNo);
            bookCertificate.ID = int.Parse(strId);

            bookCertificateBLL.DeleteBookCertificate(bookCertificate);

            LoadBookCertificate();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 读者类型数据
    /// </summary>
    protected void LoadBookReaderType()
    {
        string strHQL = "from BookReaderType as bookReaderType order by bookReaderType.ID ASC";
        BookReaderTypeBLL bookReaderTypeBLL = new BookReaderTypeBLL();
        IList lst = bookReaderTypeBLL.GetAllBookReaderType(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    /// <summary>
    /// 判断读者类型是否存在  存在返回true；不存在则返回false
    /// </summary>
    protected bool IsBookReaderType(string strtypename)
    {
        bool flag = true;
        string strHQL = "from BookReaderType as bookReaderType Where bookReaderType.TypeName='" + strtypename + "' ";
        BookReaderTypeBLL bookReaderTypeBLL = new BookReaderTypeBLL();
        IList lst = bookReaderTypeBLL.GetAllBookReaderType(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag = true;
        }
        else
            flag = false;
        return flag;
    }
    protected bool IsBookReaderType(string strtypename, string strId)
    {
        bool flag = true;
        string strHQL = "from BookReaderType as bookReaderType Where bookReaderType.TypeName='" + strtypename + "' and bookReaderType.ID<>'" + strId + "' ";
        BookReaderTypeBLL bookReaderTypeBLL = new BookReaderTypeBLL();
        IList lst = bookReaderTypeBLL.GetAllBookReaderType(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag = true;
        }
        else
            flag = false;
        return flag;
    }

    //判断输入的字符是否是数字
    private bool IsNumeric(string str)
    {
        System.Text.RegularExpressions.Regex reg1
            = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
        return reg1.IsMatch(str);
    }

    /// <summary>
    /// 出版社数据
    /// </summary>
    protected void LoadBookPublishers()
    {
        string strHQL = "from BookPublishers as bookPublishers order by bookPublishers.ID ASC";
        BookPublishersBLL bookPublishersBLL = new BookPublishersBLL();
        IList lst = bookPublishersBLL.GetAllBookPublishers(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    /// <summary>
    /// 判断出版社是否存在，存在返回true；否则返回false
    /// </summary>
    /// <param name="strISBNNo"></param>
    /// <param name="strPublishersName"></param>
    /// <returns></returns>
    protected bool IsBookPublishers(string strISBNNo, string strPublishersName)
    {
        bool flag = true;
        string strHQL = "from BookPublishers as bookPublishers Where bookPublishers.ISBNNo='" + strISBNNo + "' or bookPublishers.PublishersName='" + strPublishersName + "' ";
        BookPublishersBLL bookPublishersBLL = new BookPublishersBLL();
        IList lst = bookPublishersBLL.GetAllBookPublishers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag = true;
        }
        else
            flag = false;
        return flag;
    }

    /// <summary>
    /// 图书分类
    /// </summary>
    protected void LoadBookClassification()
    {
        string strHQL = "from BookClassification as bookClassification Where bookClassification.DataType='BookClassification' order by bookClassification.ID ASC";   
        BookClassificationBLL bookClassificationBLL = new BookClassificationBLL();
        IList lst = bookClassificationBLL.GetAllBookClassification(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    /// <summary>
    /// 标准分类
    /// </summary>
    protected void LoadBookClassification1()
    {
        string strHQL = "from BookClassification as bookClassification Where bookClassification.DataType='StandardClassification' order by bookClassification.ID ASC";   
        BookClassificationBLL bookClassificationBLL = new BookClassificationBLL();
        IList lst = bookClassificationBLL.GetAllBookClassification(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    /// <summary>
    /// 图书分类树结构
    /// </summary>
    protected void BindDDL()
    {
        //DataTable dt = GetList(-1);
        //if (dt != null && dt.Rows.Count > 0)
        //{
        //    ddl_ParentId.Items.Clear();
        //    ddl_ParentId.Items.Insert(0, new ListItem("--作为一级分类--", "0"));
        //    SetInterval(ddl_ParentId, 0, " ");
        //}
        //else
        //{
        //    ddl_ParentId.Items.Clear();
        //    ddl_ParentId.Items.Insert(0, new ListItem("--作为一级分类--", "0"));
        //}

        string strHQL = "from WorkType as workType Order by workType.SortNo ASC";
        WorkTypeBLL workTypeBLL = new WorkTypeBLL();
        IList lst = workTypeBLL.GetAllWorkType(strHQL);
        ddl_TypeName.DataSource = lst;
        ddl_TypeName.DataBind();
        ddl_TypeName.Items.Insert(0, new ListItem("--Select--", ""));
    }
    /// <summary>
    /// 判断图书分类名称是否存在，存在返回true；否则返回false
    /// </summary>
    /// <param name="strISBNNo"></param>
    /// <param name="strPublishersName"></param>
    /// <returns></returns>
    //protected bool IsBookClassification(string strClassificationName)
    //{
    //    bool flag = true;
    //    string strHQL = "from BookClassification as bookClassification Where bookClassification.ClassificationName='" + strClassificationName + "' ";
    //    BookClassificationBLL bookClassificationBLL = new BookClassificationBLL();
    //    IList lst = bookClassificationBLL.GetAllBookClassification(strHQL);
    //    if (lst.Count > 0 && lst != null)
    //    {
    //        flag = true;
    //    }
    //    else
    //        flag = false;
    //    return flag;
    //}

    /// <summary>
    /// 根据图书分类ID，获取图书分类名称
    /// </summary>
    /// <param name="strParentId"></param>
    /// <returns></returns>
    //protected string GetClassificationName(string strParentId)
    //{
    //    string result = string.Empty;
    //    string strHQL = "from BookClassification as bookClassification Where bookClassification.ID='" + strParentId + "' ";
    //    BookClassificationBLL bookClassificationBLL = new BookClassificationBLL();
    //    IList lst = bookClassificationBLL.GetAllBookClassification(strHQL);
    //    if (lst.Count > 0 && lst != null)
    //    {
    //        BookClassification bookClassification = (BookClassification)lst[0];
    //        result = bookClassification.ClassificationName;
    //    }
    //    else
    //        result = "";
    //    return result;
    //}

    /// <summary>
    /// 获取图书分类数据
    /// </summary>
    //public DataTable GetList(int strparentid)
    //{
        //string strHQL = "Select ID,ParentId,ClassificationName,SortNo From T_BookClassification ";
        //if (strparentid >= 0)
        //{
        //    strHQL += " Where ParentId='" + strparentid.ToString() + "' ";
        //}
        //strHQL += " Order By SortNo ";
        //DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookClassification");
        //return ds.Tables[0];
    //}

    protected void SetInterval(DropDownList DDL, int parentid, string interval)
    {
        //interval += "├";

        //DataTable list = GetList(parentid);
        //if (list.Rows.Count > 0 && list != null)
        //{
        //    for (int i = 0; i < list.Rows.Count; i++)
        //    {
        //        DDL.Items.Add(new ListItem(string.Format("{0}{1}", interval, list.Rows[i]["ClassificationName"].ToString()), list.Rows[i]["ID"].ToString()));

        //        ///递归
        //        SetInterval(DDL, int.Parse(list.Rows[i]["ID"].ToString()), interval);
        //    }
        //}
    }

    /// <summary>
    /// 借阅证件数据
    /// </summary>
    protected void LoadBookCertificate()
    {
        string strHQL = "from BookCertificate as bookCertificate order by bookCertificate.ID ASC";
        BookCertificateBLL bookCertificateBLL = new BookCertificateBLL();
        IList lst = bookCertificateBLL.GetAllBookCertificate(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }

    /// <summary>
    /// 判断借阅证件名称是否存在，存在返回true；否则返回false
    /// </summary>
    /// <param name="strISBNNo"></param>
    /// <param name="strPublishersName"></param>
    /// <returns></returns>
    protected bool IsBookCertificate(string strCertificateName)
    {
        bool flag = true;
        string strHQL = "from BookCertificate as bookCertificate Where bookCertificate.CertificateName='" + strCertificateName + "' ";
        BookCertificateBLL bookCertificateBLL = new BookCertificateBLL();
        IList lst = bookCertificateBLL.GetAllBookCertificate(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag = true;
        }
        else
            flag = false;
        return flag;
    }

    protected void DataGrid3_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId = e.Item.Cells[0].Text.Trim();
        string strClassificationType = ((Button)e.Item.FindControl("BT_ClassificationName")).Text.Trim();
        string strClassificationCode = e.Item.Cells[2].Text.Trim();
        string strRemark = e.Item.Cells[3].Text.Trim();

        lbl_BookClassificationId1.Text = strId;
        TB_ClassificationType1.Text = strClassificationType;
        TB_Remark1.Text = strRemark;
        TB_ClassificationCode.Text = strClassificationCode;
    }

    protected void BT_BookClassificationNew1_Click(object sender, EventArgs e)
    {
        string strClassificationType = TB_ClassificationType1.Text.Trim();
        string strRemark = TB_Remark1.Text.Trim();
        string strClassificationCode = TB_ClassificationCode.Text.Trim();

        BookClassificationBLL bookClassificationBLL = new BookClassificationBLL();
        BookClassification bookClassification = new BookClassification();

        try
        {
            bookClassification.ClassificationType = strClassificationType;
            bookClassification.ClassificationCode = strClassificationCode;
            bookClassification.DataType = "StandardClassification";   
            bookClassification.Remark = strRemark;

            bookClassificationBLL.AddBookClassification(bookClassification);

            LoadBookClassification1();
            BindDDL();
        }
        catch
        {

        }
    }

    protected void BT_BookClassificationDelete1_Click(object sender, EventArgs e)
    {
        string strClassificationType = TB_ClassificationType1.Text.Trim();
        string strRemark = TB_Remark1.Text.Trim();
        string strId = lbl_BookClassificationId1.Text.Trim();
        string strClassificationCode = TB_ClassificationCode.Text.Trim();

        BookClassificationBLL bookClassificationBLL = new BookClassificationBLL();
        BookClassification bookClassification = new BookClassification();

        try
        {
            bookClassification.ID = int.Parse(strId);
            bookClassification.ClassificationCode = strClassificationCode;
            bookClassification.ClassificationType = strClassificationType;
            bookClassification.DataType = "StandardClassification";   
            bookClassification.Remark = strRemark;

            bookClassificationBLL.UpdateBookClassification(bookClassification, bookClassification.ID);

            LoadBookClassification1();
            BindDDL();
        }
        catch
        {

        }
    }

    /// <summary>
    /// 判断出版社信息是否已使用，已使用返回true；否则返回false
    /// </summary>
    /// <param name="strISBNNo"></param>
    /// <param name="strPublishersName"></param>
    /// <returns></returns>
    protected bool IsBookPublishersBook(string strBookPublishersId)
    {
        bool flag = true;
        bool flag1 = true;
        string strHQL = "from BookInformation as bookInformation Where bookInformation.BookPublishersId='" + strBookPublishersId + "' ";
        BookInformationBLL bookInformationBLL = new BookInformationBLL();
        IList lst = bookInformationBLL.GetAllBookInformation(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag = true;
        }
        else
            flag = false;

        strHQL = "from BookBorrowRecord as bookBorrowRecord Where bookBorrowRecord.BookPublishersId='" + strBookPublishersId + "' ";
        BookBorrowRecordBLL bookBorrowRecordBLL = new BookBorrowRecordBLL();
        lst = bookBorrowRecordBLL.GetAllBookBorrowRecord(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag1 = true;
        }
        else
            flag1 = false;

        if (flag || flag1)
        {
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// 判断证件信息是否已使用，已使用返回true；否则返回false
    /// </summary>
    /// <param name="strISBNNo"></param>
    /// <param name="strPublishersName"></param>
    /// <returns></returns>
    protected bool IsBookCertificateBook(string strBookCertificateId)
    {
        bool flag1 = true;
        string strHQL = "from BookBorrowRecord as bookBorrowRecord Where bookBorrowRecord.CertificateId='" + strBookCertificateId + "' ";
        BookBorrowRecordBLL bookBorrowRecordBLL = new BookBorrowRecordBLL();
        IList lst = bookBorrowRecordBLL.GetAllBookBorrowRecord(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag1 = true;
        }
        else
            flag1 = false;

        return flag1;
    }
}
