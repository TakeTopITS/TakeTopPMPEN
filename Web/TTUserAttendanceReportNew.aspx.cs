using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.IO;

using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Text;

using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using Npgsql;

using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

//����JSON
using Newtonsoft.Json.Linq;


public partial class TTUserAttendanceReportNew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserName;
        //        string strUserCode = "C7094";
        string strUserCode = Session["UserCode"].ToString();

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            Response.Redirect("TTUserAttendanceReportSAAS.aspx");
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_StartTime.Text = ShareClass.getCurrentMonthStartDay().ToString("yyyy-MM-dd");
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(Resources.lang.ZZJGT, TreeView1, strUserCode);

            var dtLeave = GetLeaveTypeAll();
            if (dtLeave != null)
            {
                leaveTd.Attributes["colspan"] = dtLeave.Rows.Count.ToString();
                rpLeaveCol.DataSource = dtLeave;
                rpLeaveCol.DataBind();
            }


            var dtOvertime = GetOvertimeTypeAll();
            if (dtOvertime != null)
            {
                overtimeTd.Attributes["colspan"] = dtOvertime.Rows.Count.ToString();
                rpOvertimeCol.DataSource = dtOvertime;
                rpOvertimeCol.DataBind();
            }
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
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        try
        {
            var ds = GetData();

            if (ds != null)
            {
                rpReport.DataSource = ds;
                rpReport.DataBind();
            }
        }
        catch(Exception ex)
        {
            LogClass.WriteLogFile(ex.Message.ToString());
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���󣬲��ź��Ӳ���Ա������̫�࣬�ڴ治��������!')", true);
        }
    }

    private DataSet GetData()
    {
        string strDepartCode, strUserName, strStartTime, strEndTime;

        strDepartCode = TB_DepartCode.Text.Trim();
        strUserName = TB_UserName.Text.Trim();

        if (strDepartCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���棬����ѡ�����ٲ�ѯ��')", true);
            return null;
        }

        var startTime = DateTime.Parse(DLC_StartTime.Text);
        var endTime = DateTime.Parse(DLC_EndTime.Text);

        var totalDays = endTime.Subtract(startTime).Days + 1;
        kqjgTd.Attributes["colspan"] = totalDays.ToString();

        var dayList = new List<string>();
        for (DateTime i = startTime; i <= endTime; i = i.AddDays(1))
        {
            dayList.Add(i.Day.ToString());
        }

        rpDays.DataSource = dayList;
        rpDays.DataBind();


        strStartTime = startTime.ToString("yyyy-MM-dd");
        strEndTime = endTime.ToString("yyyy-MM-dd");
        Label4.Text = string.Format("ͳ��ʱ�䣺{0}��{1}", strStartTime, strEndTime);

        if (strStartTime.Substring(0, 8) != strEndTime.Substring(0, 8))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click",
                "alert('���棬��ʼ��������ֻ��ѡ��ͬһ���·ݣ�')", true);
            return null;
        }

        strDepartCode = strDepartCode + "%";
        strUserName = "%" + strUserName + "%";
        //��������û��б�
        DataSet alluser = GetAllUser(strDepartCode, strUserName);
        //�������û���һ���JSON�����
        DataSet ds = GetAllJSONData(strStartTime, strEndTime, alluser);

        return ds;
    }

    private static DataSet GetAllJSONData(string strStartTime, string strEndTime, DataSet alluser)
    {
        StringBuilder sbJson = new StringBuilder("[");
        for (int i = 0; i < alluser.Tables[0].Rows.Count; i++)
        {
            string dcode = alluser.Tables[0].Rows[i]["departcode"].ToString();
            string uname = alluser.Tables[0].Rows[i]["UserName"].ToString();
            string sql = string.Format("SELECT f_userattendancereport('{0}','{1}','{2}','{3}')", strStartTime, strEndTime, dcode.Trim(), uname.Trim());
            //LogClass.WriteLogFile(sql);

            string res = RunSqlCommandJSon(sql);
            sbJson.Append(res.TrimStart('[').TrimEnd(']'));
            if (i < alluser.Tables[0].Rows.Count - 1)
                sbJson.Append(",");
        }
        sbJson.Append("]");

        //LogClass.WriteLogFile(sbJson.ToString());

        DataSet ds = JSonConvertDataset(sbJson.ToString());

        return ds;
    }

    //ͨ�����ź��û�������������û�
    private static DataSet GetAllUser(string departcode, string username)
    {
        string sqlcmd = string.Format("select distinct departcode,UserName from t_projectmember where departcode like '{0}' and username like '{1}' order by departcode", departcode, username);

        DataSet ds = ShareClass.GetDataSetFromSqlNOOperateLog(sqlcmd, "t_projectmember");

        return ds;
    }

    //���з���JSON������JSONתΪDataSet
    public static string RunSqlCommandJSon(string strCmdText)
    {
        NpgsqlConnection myConnection = new NpgsqlConnection(
               ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString);

        ///����Command
        NpgsqlCommand myCommand = new NpgsqlCommand(strCmdText, myConnection);
        myCommand.CommandTimeout = 600;

        ///������
        myConnection.Open();

        var result = myCommand.ExecuteScalar();

        myConnection.Close();

        return result.ToString();

    }

    //JSONתΪDataSet����
    public static DataSet JSonConvertDataset(string jsonstr)
    {
        DataSet ds = new DataSet();
        // �� JSON �ַ�������Ϊ JArray ����
        JArray jsonArray = JArray.Parse(jsonstr);

        //LogClass.WriteLogFile(jsonArray.ToString());

        // ���� DataTable
        DataTable dataTable = new DataTable();

        // �����
        foreach (JProperty property in jsonArray[0])
        {
            dataTable.Columns.Add(property.Name);
        }

        // �����
        foreach (JObject jsonObject in jsonArray)
        {
            DataRow row = dataTable.NewRow();
            foreach (JProperty property in jsonObject.Properties())
            {
                row[property.Name] = property.Value.ToString();
            }
            dataTable.Rows.Add(row);
        }

        ds.Tables.Add(dataTable);

        return ds;
    }

    protected void rpReport_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var rpReportDays = (e.Item.FindControl("rpReportDays") as Repeater);
            var startTime = DateTime.Parse(DLC_StartTime.Text);
            var endTime = DateTime.Parse(DLC_EndTime.Text);
            var totalDays = endTime.Subtract(startTime).Days + 1;
            var dayList = new List<string>();
            for (DateTime i = startTime; i <= endTime; i = i.AddDays(1))
            {
                dayList.Add(i.Day.ToString());
            }
            if (rpReportDays != null)
            {
                rpReportDays.DataSource = dayList;
                rpReportDays.DataBind();
            }

            var rpReportLeave = (e.Item.FindControl("rpReportLeave") as Repeater);
            var dtLeave = GetLeaveTypeAll();
            if (rpReportLeave != null)
            {
                rpReportLeave.DataSource = dtLeave;
                rpReportLeave.DataBind();
            }


            var rpReportOvertime = (e.Item.FindControl("rpReportOvertime") as Repeater);
            var dtOvertime = GetOvertimeTypeAll();
            if (rpReportOvertime != null)
            {
                rpReportOvertime.DataSource = dtOvertime;
                rpReportOvertime.DataBind();
            }
        }
    }

    protected void BT_ExportToExcel_Click(object sender, EventArgs e)
    {

        var ds = GetData();
        if (ds == null)
        {
            return;
        }

        var dt = ds.Tables[0];
        Random a = new Random();
        string fileName = "Ա�����ڱ���" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";

        try
        {
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            //ɾ��������UserCode������1������2
            dt.Columns.Remove("UserCode");
            dt.Columns.Remove("����1");
            dt.Columns.Remove("����2");
            var colNum = dt.Columns.Count;

            var startTime = DateTime.Parse(DLC_StartTime.Text);
            var endTime = DateTime.Parse(DLC_EndTime.Text);
            var strStartTime = startTime.ToString("yyyy-MM-dd");
            var strEndTime = endTime.ToString("yyyy-MM-dd");
            var totalDays = endTime.Subtract(startTime).Days + 1;

            var rowIndex = 0;
            //����
            CellRangeAddress region = new CellRangeAddress(rowIndex, rowIndex, 0, colNum - 1);
            sheet.AddMergedRegion(region);
            var cell = sheet.CreateRow(rowIndex).CreateCell(0);
            cell.SetCellValue("�¶Ȼ���    ͳ�����ڣ�" + strStartTime + "��" + strEndTime);

            var style = workbook.CreateCellStyle() as HSSFCellStyle;
            var font = workbook.CreateFont() as HSSFFont;
            font.IsBold = true;//�Ӵ�
            font.FontName = "����";
            font.FontHeightInPoints = 24;
            style.SetFont(font);
            ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, NPOI.SS.UserModel.BorderStyle.Thin, HSSFColor.Black.Index);
            cell.CellStyle = style;

            rowIndex++;

            //��������ʱ��
            region = new CellRangeAddress(rowIndex, rowIndex, 0, colNum - 1);
            sheet.AddMergedRegion(region);
            cell = sheet.CreateRow(rowIndex).CreateCell(0);
            //            cell.SetCellValue("�¶Ȼ���    ͳ�����ڣ�" + strStartTime + "��" + strEndTime);
            cell.SetCellValue("��������ʱ�䣺" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            style = workbook.CreateCellStyle() as HSSFCellStyle;
            font = workbook.CreateFont() as HSSFFont;
            font.FontName = "����";
            font.FontHeightInPoints = 14;
            style.SetFont(font);
            ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, NPOI.SS.UserModel.BorderStyle.Thin, HSSFColor.Black.Index);
            cell.CellStyle = style;

            rowIndex++;


            style = workbook.CreateCellStyle() as HSSFCellStyle;
            font = workbook.CreateFont() as HSSFFont;
            font.FontName = "����";
            font.FontHeightInPoints = 12;
            style.SetFont(font);
            SetCellBorder(style);
            //��ͷ

            var row1 = sheet.CreateRow(rowIndex);
            var row2 = sheet.CreateRow(rowIndex + 1);
            string[] cols = new string[] { "����", "����", "����", "ְλ" };
            var colIndex = 0;
            for (; colIndex < cols.Length; colIndex++)
            {
                region = new CellRangeAddress(rowIndex, rowIndex + 1, colIndex, colIndex);
                sheet.AddMergedRegion(region);

                cell = row1.CreateCell(colIndex);
                cell.SetCellValue(cols[colIndex]);
                ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, NPOI.SS.UserModel.BorderStyle.Thin, HSSFColor.Black.Index);
                cell.CellStyle = style;
            }


            //���
            var leaveDt = GetLeaveTypeAll();
            if (leaveDt.Rows.Count > 1)
            {
                region = new CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + leaveDt.Rows.Count - 1);
                sheet.AddMergedRegion(region);
            }

            cell = row1.CreateCell(colIndex);
            cell.SetCellValue("���(Сʱ)");
            ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, NPOI.SS.UserModel.BorderStyle.Thin, HSSFColor.Black.Index);
            cell.CellStyle = style;

            for (var i = 0; i < leaveDt.Rows.Count; i++)
            {
                cell = row2.CreateCell(colIndex++);
                cell.SetCellValue(leaveDt.Rows[i]["Type"].ToString());
                cell.CellStyle = style;
            }

            cols = new string[]
            {
                "��������", "Ӧ��������", "�ٵ�����", "�ٵ�ʱ��(����)", "�����ٵ�����",
                "���˴���", "����ʱ��(����)", "��������", "ҹ������"
            };

            for (var i = 0; i < cols.Length; i++)
            {
                region = new CellRangeAddress(rowIndex, rowIndex + 1, colIndex, colIndex);
                sheet.AddMergedRegion(region);

                cell = row1.CreateCell(colIndex++);
                cell.SetCellValue(cols[i]);
                ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, NPOI.SS.UserModel.BorderStyle.Thin, HSSFColor.Black.Index);
                cell.CellStyle = style;
            }

            //�Ӱ�
            var overtimeDt = GetOvertimeTypeAll();
            if (overtimeDt.Rows.Count > 1)
            {
                region = new CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + overtimeDt.Rows.Count - 1);
                sheet.AddMergedRegion(region);
            }

            cell = row1.CreateCell(colIndex);
            cell.SetCellValue("�Ӱ�ʱ��(Сʱ)");
            ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, NPOI.SS.UserModel.BorderStyle.Thin, HSSFColor.Black.Index);
            cell.CellStyle = style;

            for (var i = 0; i < overtimeDt.Rows.Count; i++)
            {
                cell = row2.CreateCell(colIndex++);
                cell.SetCellValue(overtimeDt.Rows[i]["Type"].ToString());
                cell.CellStyle = style;
            }

            //���ڽ��
            region = new CellRangeAddress(rowIndex, rowIndex, colIndex, colIndex + totalDays - 1);
            sheet.AddMergedRegion(region);
            cell = row1.CreateCell(colIndex);
            cell.SetCellValue("���ڽ��");
            ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, NPOI.SS.UserModel.BorderStyle.Thin, HSSFColor.Black.Index);
            cell.CellStyle = style;

            for (var i = 1; i <= totalDays; i++)
            {
                cell = row2.CreateCell(colIndex++);
                cell.SetCellValue(i);
                cell.CellStyle = style;
            }

            rowIndex++;

            //����  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                rowIndex++;
                row1 = sheet.CreateRow(rowIndex);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                    cell.CellStyle = style;
                }
            }

            //תΪ�ֽ�����  
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            var buf = stream.ToArray();

            //��ʼ����htmlҳ�����ش�
            Response.ContentType = "application/octet-stream;";

            //֪ͨ����������ļ������Ǵ�;���������ƽ��б���
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + "\"");
            Response.BinaryWrite(buf);
            Response.Flush();
            Response.End();
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    private void SetCellBorder(ICellStyle style)
    {
        style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
    }

    private DataTable GetLeaveTypeAll()
    {
        string sql = "select * from T_LeaveType order by SortNumber";
        var ds = ShareClass.GetDataSetFromSql(sql, "T_LeaveType");
        if (ds != null && ds.Tables.Count > 0)
        {
            return ds.Tables[0];
        }
        return null;
    }

    private DataTable GetOvertimeTypeAll()
    {
        string sql = "select * from T_OvertimeType order by SortNumber";
        var ds = ShareClass.GetDataSetFromSql(sql, "T_OvertimeType");
        if (ds != null && ds.Tables.Count > 0)
        {
            return ds.Tables[0];
        }
        return null;
    }

    private static DataSet RunSQLProcedure(string pro, List<SqlParameter> values)
    {
        NpgsqlConnection myConnection = new NpgsqlConnection(
              ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString);

        ///����Command
        NpgsqlCommand myCommand = new NpgsqlCommand(pro, myConnection);

        ///������
        myConnection.Open();

        // Create a database reader

        myCommand.CommandType = CommandType.StoredProcedure;

        foreach (SqlParameter sp in values)
        {
            myCommand.Parameters.Add(sp);
        }

        NpgsqlDataAdapter sda = new NpgsqlDataAdapter();
        sda.SelectCommand = myCommand;
        sda.SelectCommand.CommandTimeout = 600;  //ȡ����ʱĬ������  Ĭ����30s   ����һ������
        DataSet ds = new DataSet();
        sda.Fill(ds);

        myConnection.Close();

        return ds;
    }


    //private DataSet GetData()
    //{
    //    string strDepartCode, strUserName, strStartTime, strEndTime;

    //    strDepartCode = TB_DepartCode.Text.Trim();
    //    strUserName = TB_UserName.Text.Trim();

    //    if (strDepartCode == "")
    //    {
    //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���棬����ѡ�����ٲ�ѯ��')", true);
    //        return null;
    //    }

    //    var startTime = DateTime.Parse(DLC_StartTime.Text);
    //    var endTime = DateTime.Parse(DLC_EndTime.Text);

    //    var totalDays = endTime.Subtract(startTime).Days + 1;
    //    kqjgTd.Attributes["colspan"] = totalDays.ToString();

    //    var dayList = new List<string>();
    //    for (DateTime i = startTime; i <= endTime; i = i.AddDays(1))
    //    {
    //        dayList.Add(i.Day.ToString());
    //    }

    //    rpDays.DataSource = dayList;
    //    rpDays.DataBind();


    //    strStartTime = startTime.ToString("yyyy-MM-dd");
    //    strEndTime = endTime.ToString("yyyy-MM-dd");
    //    Label4.Text = string.Format("ͳ��ʱ�䣺{0}��{1}", strStartTime, strEndTime);

    //    if (strStartTime.Substring(0, 8) != strEndTime.Substring(0, 8))
    //    {
    //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click",
    //            "alert('���棬��ʼ��������ֻ��ѡ��ͬһ���·ݣ�')", true);
    //        return null;
    //    }

    //    strDepartCode = strDepartCode + "%";
    //    strUserName = "%" + strUserName + "%";

    //    var ds = RunSQLProcedure("Pro_UserAttendanceReport", new List<SqlParameter>
    //    {
    //        new SqlParameter("@StartDate", strStartTime),
    //        new SqlParameter("@EndDate", strEndTime),
    //        new SqlParameter("@DepartCode", strDepartCode),
    //        new SqlParameter("@UserName", strUserName),
    //    });

    //    return ds;
    //}


}
